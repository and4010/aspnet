using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CHPOUTSRCMES.TASK.Forms
{
    public partial class MainForm : BaseForm
    {

        public MainForm()
        {
            InitializeComponent();
            this.tsl_Version.Text = $"程式版本 {Assembly.GetExecutingAssembly().GetName().Version}";
        }

        private void 單位換算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controller.UomConverterForm.ShowOnParent();
        }

        private void 主檔ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controller.MasterViewForm.ShowOnParent();
        }
    }
}
