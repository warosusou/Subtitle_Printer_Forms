using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Subtitle_Printer
{
    public partial class SaveDialog : Form
    {
        public SaveDialog(string name)
        {
            InitializeComponent();
            label1.Text = name + "の変更を保存しますか";
            this.ActiveControl = button1;
            this.Text = "";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
