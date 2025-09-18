using Microsoft.Win32;
using QuadGamingSecurity.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace QuadGamingSecurity
{
    public partial class Form1 : Form
    {
        private string[] PortsNames;
        private SerialPort ConnectedSerialPort;

        private List<IntPtr> OpenedWindowsList = new List<IntPtr>();
        private IntPtr SelectedWindow;

        private bool PropperExiting;

        private RegistryKey RegistryKey;

        private Size DefaultWindowSize = new Size(600, 600);
        private int ConsoleHeight = 158;

        private bool ConsoleVisible
        {
            get => ConsoleTextBox.Visible;
            set
            {
                ConsoleCheckbox.CheckedChanged -= new EventHandler(ConsoleCheckbox_CheckedChanged);
                ConsoleCheckbox.Checked = value;
                ConsoleCheckbox.CheckedChanged += new EventHandler(ConsoleCheckbox_CheckedChanged);
                ConsoleTextBox.Visible = value;
                Size = new Size(Size.Width, DefaultWindowSize.Height - (!value ? ConsoleHeight : 0));
                Settings.Default.ConsoleVisible = value;
                Settings.Default.Save();
            }
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc callPtr, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetShellWindow();

        public Form1()
        {
            InitializeComponent();

            Console.SetOut(new TextBoxWriter(ConsoleTextBox));

            RefreshWindowsList();
            RefreshPortsList();

            Size = DefaultWindowSize;

            RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (RegistryKey.GetValue("QuadGamingSecurity") != null)
            {
                AutostartCheckbox.Checked = true;
            }

            ConsoleVisible = Settings.Default.ConsoleVisible;

            NotifyIcon.ContextMenu = new ContextMenu();
            NotifyIcon.ContextMenu.MenuItems.Add(new MenuItem("Foreground now", (object sender, EventArgs args) => { ForegroundSelectedWindow(); }));
            NotifyIcon.ContextMenu.MenuItems.Add(new MenuItem("Open", (object sender, EventArgs args) => { ShowFromTray(); }));
            NotifyIcon.ContextMenu.MenuItems.Add(new MenuItem("Exit", (object sender, EventArgs args) => { ExitPropperly(); }));
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!string.IsNullOrEmpty(Settings.Default.AutoConnectPort))
            {
                AutoConnectCheckbox.Checked = true;
                OpenPort(Settings.Default.AutoConnectPort);
            }
        }

        private void DevicesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectPort(PortsListBox.SelectedIndex);
        }

        public void SelectPort(int index)
        {
            if (index != 0)
            {
                if (index != -1)
                {
                    Console.WriteLine("Port selected: " + PortsNames[index - 1]);
                    ClosePort();
                    OpenPort(PortsNames[index - 1]);
                }
            }
            else
            {
                Console.WriteLine("Port deselected");
                ClosePort();
            }
        }

        public void RefreshPortsList()
        {
            Console.WriteLine("Refreshing ports list...");

            ClosePort();

            PortsNames = SerialPort.GetPortNames();

            PortsListBox.Items.Clear();
            PortsListBox.Items.Add("(none)");
            foreach (string portName in PortsNames)
            {
                PortsListBox.Items.Add(portName);
            }
        }

        public void OpenPort(string portName)
        {
            Console.WriteLine("Opening " + portName);
            if (AutoConnectCheckbox.Checked)
            {
                Settings.Default.AutoConnectPort = portName;
                Settings.Default.Save();
            }

            try
            {
                ConnectedSerialPort = new SerialPort(portName, 9600);
                ConnectedSerialPort.Open();
                Thread portReadThread = new Thread(() =>
                {
                    Invoke((MethodInvoker) delegate { Console.WriteLine(portName + " opened"); });
                    try
                    {
                        while (ConnectedSerialPort.IsOpen)
                        {
                            byte[] buffer = new byte[1024];
                            string message = ConnectedSerialPort.ReadLine();
                            Invoke((MethodInvoker) delegate
                            {
                                Console.WriteLine("Received: " + message);
                                ForegroundSelectedWindow();
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        Invoke((MethodInvoker) delegate { Console.WriteLine($"Receive from {portName} failed. {e.GetType()}: {e.Message.Replace("\n", "")}"); });
                    }
                });
                portReadThread.IsBackground = true;
                portReadThread.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Opening {portName} failed. {e.GetType()}: {e.Message.Replace("\n", "")}");
            }
        }

        public void ClosePort()
        {
            if (ConnectedSerialPort != null)
            {
                if (ConnectedSerialPort.IsOpen)
                {
                    Console.WriteLine("Closing " + ConnectedSerialPort.PortName);
                    ConnectedSerialPort.Close();
                    Console.WriteLine(ConnectedSerialPort.PortName + " closed");
                }
            }
        }

        public void RefreshWindowsList()
        {
            Console.WriteLine("Refreshing windows list...");

            SelectedWindow = IntPtr.Zero;

            WindowsListBox.Items.Clear();
            WindowsListBox.Items.Add("(none)");

            OpenedWindowsList.Clear();

            IntPtr shellWindow = GetShellWindow();
            EnumWindows(delegate (IntPtr hWnd, IntPtr lParam)
            {
                if (!IsWindowVisible(hWnd)) return true;
                if (hWnd == shellWindow) return true;
                int textLength = GetWindowTextLength(hWnd);
                if (textLength == 0) return true;
                StringBuilder stringBuilder = new StringBuilder(textLength);
                GetWindowText(hWnd, stringBuilder, textLength + 1);
                string windowText = stringBuilder.ToString();
                OpenedWindowsList.Add(hWnd);
                WindowsListBox.Items.Add(windowText);

                return true;
            }, IntPtr.Zero);
        }

        private void WindowsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectWindow(WindowsListBox.SelectedIndex);
        }

        public void SelectWindow(int index)
        {
            if (index != 0)
            {
                if (index != -1)
                {
                    Console.WriteLine("Window selected: " + WindowsListBox.SelectedItem);
                    SelectedWindow = OpenedWindowsList[WindowsListBox.SelectedIndex - 1];
                }
            }
            else
            {
                Console.WriteLine("Window deselected");
                SelectedWindow = IntPtr.Zero;
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                HideToTray();
            }
        }

        public void HideToTray()
        {
            Visible = false;
            ShowInTaskbar = false;
        }

        public void ShowFromTray()
        {
            Visible = true;
            ShowInTaskbar = true;
            Show();
            WindowState = FormWindowState.Normal;
            RefreshWindowsList();
            RefreshPortsList();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!PropperExiting)
            {
                e.Cancel = true;
                HideToTray();
            }
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            ForegroundSelectedWindow();
        }

        public void ExitPropperly()
        {
            PropperExiting = true;
            Application.Exit();
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            ShowFromTray();
        }

        public void ForegroundSelectedWindow()
        {
            if (SelectedWindow != IntPtr.Zero)
            {
                ForegroundWindowBypassed(SelectedWindow);
                ShowFromTray();
                ForegroundWindowBypassed(SelectedWindow);
            }
        }

        public void ForegroundWindowBypassed(IntPtr hWnd)
        {
            IntPtr foregroundWindow = GetForegroundWindow();
            uint currentThreadId = GetCurrentThreadId();
            uint foregroundThreadId = GetWindowThreadProcessId(foregroundWindow, out _);
            AttachThreadInput(currentThreadId, foregroundThreadId, true);
            SetForegroundWindow(hWnd);
            AttachThreadInput(currentThreadId, foregroundThreadId, false);
        }

        private void RefreshPortsListButton_Click(object sender, EventArgs e)
        {
            RefreshPortsList();
        }

        private void RefreshProcessesListButton_Click(object sender, EventArgs e)
        {
            RefreshWindowsList();
        }

        private void ForegroundSelectedProcessButton_Click(object sender, EventArgs e)
        {
            ForegroundSelectedWindow();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            ExitPropperly();
        }

        private void AutostartCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (AutostartCheckbox.Checked)
            {
                RegistryKey.SetValue("QuadGamingSecurity", Application.ExecutablePath + " /autostart");
            }
            else
            {
                RegistryKey.DeleteValue("QuadGamingSecurity", false);
            }
        }

        private void ConsoleCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            ConsoleVisible = ConsoleCheckbox.Checked;
        }

        private void AutoConnectCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (AutoConnectCheckbox.Checked)
            {
                if (PortsListBox.SelectedIndex != -1)
                {
                    Settings.Default.AutoConnectPort = PortsNames[PortsListBox.SelectedIndex - 1];
                    Settings.Default.Save();
                }
            }
            else
            {
                Settings.Default.AutoConnectPort = "";
                Settings.Default.Save();
            }
        }
    }
}
