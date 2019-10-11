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
    public partial class EqFontSizeForm : Form
    {
        private Size picturebox1Size;
        private double eqsize;
        readonly string eq = @"S_n = \sum_{k=1}^{n}f\left(t_k\right)\left(x_{k}-x_{k-1}\right)";
        public double EQSize {get { return eqsize; } }
        public bool AutoShrink { get; private set; }

        public EqFontSizeForm(Size pic1,double size,bool autoshrink)
        {
            InitializeComponent();
            picturebox1Size = pic1;
            eqsize = size;
            AutoShrink = autoshrink;
            checkBox1.Checked = autoshrink;
        }

        private void EqFontSizeForm_Load(object sender, EventArgs e)
        {
            pictureBox1.Size = picturebox1Size;
            pictureBox1.Image = Form1.ImageDrawer.TexPrinter(eq);
            textBox1.Text = eqsize.ToString();
            if(pictureBox1.Location.X + pictureBox1.Width + pictureBox1.Location.X > this.Width)
            {
                this.Width = pictureBox1.Location.X + pictureBox1.Width + pictureBox1.Location.X * 2;
            }
            if(pictureBox1.Location.Y + pictureBox1.Height + pictureBox1.Location.X > this.Height)
            {
                this.Height = pictureBox1.Location.Y + pictureBox1.Height + pictureBox1.Location.Y;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if(Double.TryParse(textBox1.Text, out var size))
            {
                size++;
                eqsize = size;
                if (pictureBox1.Image != null) pictureBox1.Image.Dispose();
                pictureBox1.Image = Form1.ImageDrawer.TexPrinter(eq, eqsize);
                textBox1.Text = eqsize.ToString();
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (Double.TryParse(textBox1.Text, out var size))
            {
                if (size == 1) return;
                size--;
                eqsize = size;
                if (pictureBox1.Image != null) pictureBox1.Image.Dispose();
                pictureBox1.Image = Form1.ImageDrawer.TexPrinter(eq, eqsize);
                textBox1.Text = eqsize.ToString();
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if(Double.TryParse(textBox1.Text,out var size))
            {
                eqsize = size;
                if (pictureBox1.Image != null) pictureBox1.Image.Dispose();
                pictureBox1.Image = Form1.ImageDrawer.TexPrinter(eq, eqsize);
            }
            else if(textBox1.Text == "")
            {
                return;
            }
            else
            {
                textBox1.Text = eqsize.ToString();
                textBox1.SelectionStart = textBox1.Text.Length;
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            AutoShrink = checkBox1.Checked;
            DialogResult = DialogResult.OK;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
