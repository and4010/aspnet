using System;
using System.Windows.Forms;

namespace CHPOUTSRCMES.TASK
{
    static class Program
    {
        

        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (var controller = MainController.Instance)
            {
                controller.StartTimer();
                //controller.AddMasterTasker();

                Application.Run(controller.MainForm);
                controller.StopTimer();
            }
        }
    }
}
