using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace QuadGamingSecurity
{
    public static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            Mutex mutex = new Mutex(true, Application.ExecutablePath.Replace("\\", "#"), out bool createdNew);
            if (createdNew)
            {
                GC.KeepAlive(mutex);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Form1 form = new Form1();
                if (!args.Contains("/autostart"))
                {
                    form.Show();
                }
                Application.Run();
            }
        }
    }
}
