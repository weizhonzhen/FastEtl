using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;

namespace FastEtlTool
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string className, string frmText);
        [DllImport("user32.dll", EntryPoint = "ShowWindow", CharSet = CharSet.Auto)]
        public static extern int ShowWindow(IntPtr hwnd, int showWay);

        protected override void OnStartup(StartupEventArgs e)
        {
            //初始化表
            FastData.FastMap.InstanceTable("FastEtlModel.DataModel", "FastEtlModel.dll");
            
            if (Process.GetProcessesByName("FastEtlTool").Count() > 1)
            {
                var handle = Process.GetProcessesByName("FastEtlTool")[1].MainWindowHandle;
                if (handle.ToInt32() == 0)
                {
                    var frmHwnd = FindWindow(null, FastApp.Config.Title);
                    ShowWindow(frmHwnd, 1);
                }

                this.Shutdown();
            }
        }
    }
}
