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

        private void MainForm_Load(object sender, EventArgs e)
        {
            Controller.ScheduleForm.ShowOnParent();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show("是否確定要關閉程式?", "關閉程式", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

        }
    }
}
