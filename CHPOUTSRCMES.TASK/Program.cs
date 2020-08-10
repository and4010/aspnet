using NLog;
using System;
using System.Windows.Forms;

namespace CHPOUTSRCMES.TASK
{
    static class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                MainController.Instance.StartTimer();
                //controller.AddMasterTasker();

                Application.Run(MainController.Instance.MainForm);

            }
            catch(Exception ex)
            {
                logger.Error(ex, "應用程式發生未預期的錯誤!!");
            }
            finally
            {
                MainController.Instance.StopTimer();
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (e.ExceptionObject != null && e.ExceptionObject is Exception) ? (Exception)e.ExceptionObject : null;
            if (ex != null)
            {
                logger.Error(ex, "應用程式發生未預期的錯誤!!");
                Application.Restart();
            }
        }
    }
}
