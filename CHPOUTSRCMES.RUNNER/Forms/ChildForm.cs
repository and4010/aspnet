using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CHPOUTSRCMES.TASK.Forms
{
    public partial class ChildForm : BaseForm
    {

        public ChildForm()
        {
            InitializeComponent();
        }

        public void ShowOnParent()
        {
            MdiParent = Controller.MainForm;
            WindowState = FormWindowState.Maximized;
            Show();
        }

    }
}
