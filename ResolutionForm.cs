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
    public partial class ResolutionForm : Form
    {
        public Size size { get; private set; }
        public ResolutionForm()
        {
            InitializeComponent();
            this.size = new Size();
        }

        public ResolutionForm(Size size)
        {
            InitializeComponent();
            this.size = size;
            textBox1.Text = size.Height.ToString();
            textBox2.Text = size.Width.ToString();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if(Int32.TryParse(textBox1.Text,out int h) && Int32.TryParse(textBox2.Text,out int w))
            {
                this.size = new Size(w, h);
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("有効な数字を入力してください", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
