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
        int currentline = -1;
        Font PrintingFont;
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
                Text = "ImeTextBox",
                WordWrap = Reference_TextBox.WordWrap,
                Font = Reference_TextBox.Font
            };
            textBox.ImeCompositionHira += TextBox_ImeCompositionHira;
            textBox.TextChanged += TextBox_TextChanged;
            textBox.KeyDown += TextBox_KeyDown;
            textBox.KeyUp += TextBox_KeyUp;
            textBox.KeyPress += TextBox_KeyPress;
            textBox.Click += TextBox_Click;
            this.Controls.Add(textBox);
            Reference_TextBox.Visible = false;
            TextColor = textBox.SelectionColor;
            PrintingFont = new Font("メイリオ", 20);
            LineChangeDetector();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox.Font = fontDialog1.Font;
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                PrintingFont.Dispose();
                PrintingFont = fontDialog1.Font;
                Print_Subtitle();
            }
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            Print_Subtitle();
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            LineChangeDetector();
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Back || e.KeyCode == Keys.PageUp || e.KeyCode == Keys.PageDown || e.KeyCode == Keys.Home || e.KeyCode == Keys.End)
            {                
                var t = new Timer { Interval = 10, Enabled = true };
                t.Tick += (s, ev) =>
                  {
                      t.Enabled = false;
                      LineChangeDetector();
                  };
            }
        }

        private void TextBox_Click(object sender, EventArgs e)
        {
            LineChangeDetector();
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            /*
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
            else if (EQ)
            {
                var charactor = e.KeyChar;
                e.Handled = true;
                textBox.SelectedText = charactor.ToString();
            }
            */
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            /*
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right && EQ)
            {*/
                /*if (textBox.SelectionStart < EQ_head || EQ_last < ime.SelectionStart)
                {
                    Leave_EQmode();
                }*/
            //}
            
        }

        private void TextBox_ImeCompositionHira(object sender, ImeReadableTextBox.ImeCompositionEventArgs e)
        {
            /*
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
            */
        }
        private void Leave_EQmode()
        {
            EQ = false;
            textBox.SelectionLength = 0;
            textBox.SelectionColor = TextColor;
            textBox.ImeMode = ImeMode.Hiragana;
            this.Text = "Form1";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Save_Subtitles();
        }

        private void LineChangeDetector()
        {
            int currentline = 0;
            int charactors = 0;
            if(textBox.Lines.Length == 0) { Print_Subtitle(); return; }
            while (true)
            {
                charactors += textBox.Lines[currentline].Length + 1;//+1は改行コード
                if (charactors > textBox.SelectionStart) { break; }
                else if(currentline >= textBox.Lines.Length -1) { break; }
                currentline++;
            }
            if(currentline != this.currentline) { this.currentline = currentline; }
            Print_Subtitle();
        }

        private void Print_Subtitle()
        {
            string text = "";
            if (pictureBox1.Image != null)pictureBox1.Image.Dispose();
            if (textBox.Lines.Length != 0)
            {
                int currentline = 0;
                int charactors = 0;
                while (true)
                {
                    charactors += textBox.Lines[currentline].Length + 1;//+1は改行コード
                    if (charactors > textBox.SelectionStart) { break; }
                    else if (currentline >= textBox.Lines.Length - 1) { break; }
                    currentline++;
                }
                if (currentline < 0 || currentline >= textBox.Lines.Length) { return; }
                text = textBox.Lines[currentline];
                if (text.EndsWith(":")) text = "";
                if (text.Contains("%")) text = text.Split('%')[0];
            }
            //PictureBox1に表示する
            pictureBox1.Image = Graphicer(text);
        }

        private void Save_Subtitles()
        {
            for (int currentline = 0; currentline < textBox.Lines.Length; currentline++)
            {
                string text = "";
                if (pictureBox1.Image != null) pictureBox1.Image.Dispose();
                if (textBox.Lines[currentline].Length != 0)
                { 
                    text = textBox.Lines[currentline];
                    if (text.EndsWith(":")) continue;
                    if (text.Contains("%")) text = text.Split('%')[0];
                    Graphicer(text).Save(String.Format("Line{0}.bmp", currentline));
                }
            }
        }

        private Bitmap Graphicer(string text)
        {
            PointF pt = new PointF(0,pictureBox1.Height/2);
            var strfmt = new StringFormat { Alignment = StringAlignment.Near,LineAlignment = StringAlignment.Center };
            if (radioButton1.Checked)
            {
                strfmt.Alignment = StringAlignment.Near;
            }
            else if (radioButton2.Checked)
            {
                strfmt.Alignment = StringAlignment.Center;
                pt = new PointF(pictureBox1.Width / 2, pictureBox1.Height/2);
            }
            else if (radioButton3.Checked)
            {
                strfmt.Alignment = StringAlignment.Far;
                pt = new PointF(pictureBox1.Width, pictureBox1.Height/2);
            }
            //描画先とするImageオブジェクトを作成する
            var canvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            //ImageオブジェクトのGraphicsオブジェクトを作成する
            var g = Graphics.FromImage(canvas);
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            //フォントオブジェクトの作成
            var fnt = new Font(PrintingFont.Name,PrintingFont.Size);
            //文字列を位置pt、黒で表示
            g.DrawString(text, fnt, Brushes.Black, pt,strfmt);
            //リソースを解放する
            strfmt.Dispose();
            fnt.Dispose();
            g.Dispose();
            return canvas;
        }
    }
}