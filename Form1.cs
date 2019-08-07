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
    public partial class Form1 : Form
    {
        ImeReadableTextBox textBox;
        Color TextColor;
        bool EQ;
        public Form1()
        {
            InitializeComponent();
            textBox = new ImeReadableTextBox
            {
                Size = Reference_TextBox.Size,
                Location = Reference_TextBox.Location,
                Multiline = Reference_TextBox.Multiline,
                Anchor = Reference_TextBox.Anchor,
                Margin = Reference_TextBox.Margin,
                Text = "ImeTextBox"
            };
            textBox.ImeCompositionHira += TextBox_ImeCompositionHira;
            textBox.KeyDown += TextBox_KeyDown;
            textBox.KeyPress += TextBox_KeyPress;
            textBox.TextChanged += TextBox_TextChanged;
            this.Controls.Add(textBox);
            Reference_TextBox.Visible = false;
            TextColor = textBox.SelectionColor;
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBox.SelectionStart + "</Tex>".Length > textBox.Text.Length) { return; }
            if (e.KeyChar == '\\' && textBox.Text[textBox.SelectionStart - 1] == '\\' && textBox.Text.Substring(textBox.SelectionStart, "</Tex>".Length) == "</Tex>" && EQ)
            {
                var pos = textBox.SelectionStart;
                textBox.Text = textBox.Text.Remove(pos - 1, 1);
                textBox.SelectionStart = pos;
                e.Handled = true;
                Leave_EQmode();
                textBox.SelectionStart += "</Tex>".Length;
            }
            else if (e.KeyChar == '￥' && !EQ)
            {
                e.Handled = true;
            }
            else if(EQ)
            {
                var charactor = e.KeyChar;
                e.Handled = true;
                textBox.SelectedText = charactor.ToString();
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right && EQ)
            {
                /*if (textBox.SelectionStart < EQ_head || EQ_last < ime.SelectionStart)
                {
                    Leave_EQmode();
                }*/
            }
        }

        private void TextBox_ImeCompositionHira(object sender, ImeReadableTextBox.ImeCompositionEventArgs e)
        {
            if (e.InputString == "￥" && !EQ)
            {
                var pos = textBox.SelectionStart;
                EQ = true;
                textBox.ImeMode = ImeMode.Disable;
                textBox.Text = textBox.Text.Insert(pos, "<Tex></Tex>");
                textBox.Text = textBox.Text.Remove(pos + "<Tex></Tex>".Length, 1);
                textBox.SelectionStart = pos + "<Tex>".Length;
                textBox.SelectionColor = Color.Red;
                this.Text += " - EQ mode";
            }
        }
        private void Leave_EQmode()
        {
            EQ = false;
            textBox.SelectionLength = 0;
            textBox.SelectionColor = TextColor;
            textBox.ImeMode = ImeMode.Hiragana;
            this.Text = "Form1";
        }
    }
}