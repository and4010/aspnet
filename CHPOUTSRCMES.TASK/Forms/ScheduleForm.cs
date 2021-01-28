using CHPOUTSRCMES.TASK.Enums;
using CHPOUTSRCMES.TASK.Models.UnitOfWork;
using Dapper;
using NLog;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CHPOUTSRCMES.TASK.Forms
{
    public partial class ScheduleForm : ChildForm
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        private static object objectLock = new object();

        public ScheduleForm()
        {
            InitializeComponent();
        }


        public void MessageUpdate(string taskname, string interval, string status, string lastExecutionTime, MessageType messageType)
        {
            ListViewItem item = new ListViewItem(new string[] { taskname, interval, status, lastExecutionTime })
            {
                ForeColor = getMeesageColor(messageType)
            };
            ListAdd(ltv_Schedule, item);
        }


        private Color getMeesageColor(MessageType messageType)
        {
            Color color = Color.Black;
            switch (messageType)
            {
                case MessageType.Normal:
                    color = Color.Black;
                    break;
                case MessageType.Important:
                    color = Color.Green;
                    break;
                case MessageType.Error:
                    color = Color.DarkRed;
                    break;
                default:
                    color = Color.Black;
                    break;
            }
            return color;
        }

        /// <summary>
        /// 新增項目至指定ListView
        /// </summary>
        /// <param name="theList"></param>
        /// <param name="theItem"></param>
        private void ListAdd(ListView theList, ListViewItem theItem)
        {
            EventHandler handler = delegate
            {
                lock (objectLock)
                {
                    if (theList.Items.Count > 0)
                    {
                        var item = theList.FindItemWithText(theItem.SubItems[0].Text, true, 0);
                        if (item != null)
                        {
                            theList.Items.RemoveAt(item.Index);
                        }
                    }

                    theList.Items.Insert(0, theItem);
                }
            };

            if (theList.InvokeRequired)
            {
                //while (!theList.IsHandleCreated)
                //{
                //解決窗體關閉時出現“訪問已釋放句柄“的異常
                if (theList.Disposing || theList.IsDisposed)
                    return;
                //}
                theList.Invoke(handler);
            }
            else
            {
                handler.Invoke(theList, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 新增項目至指定ListView
        /// </summary>
        /// <param name="theList"></param>
        /// <param name="theItem"></param>
        private void ListRemoveAll(ListView theList)
        {
            EventHandler handler = delegate
            {
                lock (objectLock)
                {
                    if (theList.Items.Count > 0)
                    {
                        theList.Items.Clear();
                    }
                }
            };

            if (theList.InvokeRequired)
            {
                //while (!theList.IsHandleCreated)
                //{
                //解決窗體關閉時出現“訪問已釋放句柄“的異常
                if (theList.Disposing || theList.IsDisposed)
                    return;
                //}
                theList.Invoke(handler);
            }
            else
            {
                handler.Invoke(theList, EventArgs.Empty);
            }
        }


        private void ScheduleForm_Load(object sender, EventArgs e)
        {
            Controller.TaskerMessageUpdate(Controller.masterTasker, "", "");
            Controller.TaskerMessageUpdate(Controller.ctrTasker, "", "");
            Controller.TaskerMessageUpdate(Controller.dlvTasker, "", "");
            Controller.TaskerMessageUpdate(Controller.ospTasker, "", "");
            Controller.TaskerMessageUpdate(Controller.trfTasker, "", "");

            Controller.StartTimer();
            Controller.AddMasterTasker(Controller.configuration);
            
        }
    }
}
