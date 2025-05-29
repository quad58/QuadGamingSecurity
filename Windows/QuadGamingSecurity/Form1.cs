using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

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

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        public Form1()
        {
            InitializeComponent();

            RefreshWindowsList();
            RefreshPortsList();

            RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (RegistryKey.GetValue("QuadGamingSecurity") != null)
            {
                AutostartCheckbox.Checked = true;
            }

            NotifyIcon.ContextMenu = new ContextMenu();
            NotifyIcon.ContextMenu.MenuItems.Add(new MenuItem("Foreground now", (object sender, EventArgs args) => { ForegroundSelectedWindow(); }));
            NotifyIcon.ContextMenu.MenuItems.Add(new MenuItem("Open", (object sender, EventArgs args) => { ShowFromTray(); }));
            NotifyIcon.ContextMenu.MenuItems.Add(new MenuItem("Exit", (object sender, EventArgs args) => { ExitPropperly(); }));
        }

        private void DevicesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PortsListBox.SelectedIndex != 0)
            {
                LogLine("Port selected: " + PortsNames[PortsListBox.SelectedIndex - 1]);
                ClosePort();
                OpenPort(PortsNames[PortsListBox.SelectedIndex - 1]);
            }
            else
            {
                LogLine("Port deselected");
                ClosePort();
            }
        }

        private void RefreshPortsList()
        {
            Console.WriteLine("Refreshing ports list...");

            ClosePort();

            PortsNames = SerialPort.GetPortNames();

            PortsListBox.Items.Clear();
            PortsListBox.Items.Add("(none)");
            foreach (string portName in PortsNames)
            {
                LogLine(portName);
                PortsListBox.Items.Add(portName);
            }
        }

        private void OpenPort(string portName)
        {
            LogLine("Opening " + portName);
            try
            {
                ConnectedSerialPort = new SerialPort(portName, 9600);
                ConnectedSerialPort.Open();
                Thread portReadThread = new Thread(() =>
                {
                    Invoke((MethodInvoker) delegate { LogLine(portName + " opened"); });
                    try
                    {
                        while (ConnectedSerialPort.IsOpen)
                        {
                            byte[] buffer = new byte[1024];
                            string message = ConnectedSerialPort.ReadLine();
                            Invoke((MethodInvoker) delegate
                            {
                                LogLine("Received: " + message);
                                ForegroundSelectedWindow();
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        Invoke((MethodInvoker) delegate { LogLine($"Receive from {portName} failed. {e.GetType()}: {e.Message.Replace("\n", "")}"); });
                    }
                });
                portReadThread.IsBackground = true;
                portReadThread.Start();
            }
            catch (Exception e)
            {
                LogLine($"Opening {portName} failed. {e.GetType()}: {e.Message.Replace("\n", "")}");
            }
        }

        private void ClosePort()
        {
            if (ConnectedSerialPort != null)
            {
                if (ConnectedSerialPort.IsOpen)
                {
                    LogLine("Closing " + ConnectedSerialPort.PortName);
                    ConnectedSerialPort.Close();
                    LogLine(ConnectedSerialPort.PortName + " closed");
                }
            }
        }

        private void WindowsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (WindowsListBox.SelectedIndex != 0)
            {
                LogLine("Window selected: " + WindowsListBox.SelectedItem);
                SelectedWindow = OpenedWindowsList[WindowsListBox.SelectedIndex - 1];
            }
            else
            {
                LogLine("Window deselected");
                SelectedWindow = IntPtr.Zero;
            }
        }

        private void RefreshWindowsList()
        {
            LogLine("Refreshing windows list...");

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
                Console.WriteLine(windowText);
                OpenedWindowsList.Add(hWnd);
                WindowsListBox.Items.Add(windowText);

                return true;
            }, IntPtr.Zero);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                HideToTray();
            }
        }

        private void HideToTray()
        {
            Visible = false;
            ShowInTaskbar = false;
        }

        private void ShowFromTray()
        {
            Visible = true;
            ShowInTaskbar = true;
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

        private void ExitPropperly()
        {
            PropperExiting = true;
            Application.Exit();
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            ShowFromTray();
        }

        private void ForegroundSelectedWindow()
        {
            if (SelectedWindow != IntPtr.Zero)
            {
                ForegroundWindowBypassed(SelectedWindow);
                ShowFromTray();
                ForegroundWindowBypassed(SelectedWindow);
            }
        }

        private void ForegroundWindowBypassed(IntPtr hWnd)
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

        private void Form1_Shown(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void LogLine(string line)
        {
            Console.WriteLine(line);
            ConsoleTextBox.Text = line + "\r\n" + ConsoleTextBox.Text;
        }

        private void AutostartCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (AutostartCheckbox.Checked)
            {
                RegistryKey.SetValue("QuadGamingSecurity", Application.ExecutablePath);
            }
            else
            {
                RegistryKey.DeleteValue("QuadGamingSecurity", false);
            }
        }
    }
}
