using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CHPOUTSRCMES.TASK
{
    public partial class BaseForm : Form
    {
        internal MainController Controller { set; get; }


        public BaseForm()
        {
            InitializeComponent();
        }
    }
}
