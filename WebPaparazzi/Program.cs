using System;
using System.Windows.Forms;

namespace WebPaparazzi
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*
            var IEVAlue = 9000; // can be: 9999 , 9000, 8888, 8000, 7000
            var targetApplication = Process.GetCurrentProcess().ProcessName + ".exe";
            var localMachine = Registry.LocalMachine;
            var parentKeyLocation = @"SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl";
            var keyName = "FEATURE_BROWSER_EMULATION";
            //"opening up Key: {0} at {1}".info(keyName, parentKeyLocation);
            var parentKey = localMachine.OpenSubKey(parentKeyLocation);
            var subKey = parentKey.OpenSubKey(keyName, true);
            if (subKey == null)
            {
                parentKey.CreateSubKey(keyName);
                subKey = parentKey.OpenSubKey(keyName, true);
            }
            subKey.SetValue(targetApplication, IEVAlue, RegistryValueKind.DWord);
            //return "all done, now try it on a new process".info();
            */
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}
