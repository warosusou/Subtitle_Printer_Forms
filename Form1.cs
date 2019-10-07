using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using WpfMath;
using WpfMath.Exceptions;

namespace Subtitle_Printer
{
    public partial class Form1 : Form
    {
        const string begintag = "<Tex>";
        const string endtag = "</Tex>";
        ImeReadableTextBox textBox;
        Color TextColor;
        int currentline = -1;
        int lasttextag = -1;
        Font PrintingFont;
        string text_path = "";
        string OriginalFormTitle;
        bool Modified = false;
        bool EQ = false;

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
                Text = "",
                WordWrap = Reference_TextBox.WordWrap,
                Font = Reference_TextBox.Font,
                AcceptsTab = true
            };
            textBox.ImeCompositionHira += TextBox_ImeCompositionHira;
            textBox.TextChanged += TextBox_TextChanged;
            textBox.KeyDown += TextBox_KeyDown;
            textBox.KeyPress += TextBox_KeyPress;
            textBox.Click += TextBox_Click;
            this.Controls.Add(textBox);
            Reference_TextBox.Visible = false;
            TextColor = textBox.SelectionColor;
            PrintingFont = new Font("メイリオ", 20);
            toolStripStatusLabel1.Text = "";
            OriginalFormTitle = this.Text;
            this.Text += " - 無題";
            this.ActiveControl = textBox;
            LineChangeDetector();
            button7.Visible = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Modified) return;
            string name;
            if (text_path == "") name = "無題";
            else name = new DirectoryInfo(text_path).Name;
            var sd = new SaveDialog(name);
            switch (sd.ShowDialog())
            {
                case DialogResult.Yes:
                    if (text_path == "")
                    {
                        string path = Environment.CurrentDirectory;
                        saveFileDialog1.FileName = "Subtitle.txt";
                        saveFileDialog1.Filter = "テキストファイル(*.txt)|*.txt";
                        saveFileDialog1.InitialDirectory = path;
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            SaveText(text_path);
                        }
                        else e.Cancel = true;
                    }
                    else
                    {
                        SaveText(text_path);
                    }
                    break;
                case DialogResult.No:
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
            }

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = new Font(textBox.Font.Name, textBox.Font.Size);
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox.Font = fontDialog1.Font;
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = new Font(PrintingFont.Name, PrintingFont.Size);
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                PrintingFont.Dispose();
                PrintingFont = fontDialog1.Font;
                Print_Subtitle();
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            string path;
            if (text_path == "") { path = Environment.CurrentDirectory; }
            else { path = new DirectoryInfo(text_path).Parent.FullName; }
            saveFileDialog1.FileName = "Subtitle.txt";
            saveFileDialog1.Filter = "テキストファイル(*.txt)|*.txt";
            saveFileDialog1.InitialDirectory = path;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                text_path = saveFileDialog1.FileName;
                SaveText(text_path);
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "テキストファイル(*.txt)|*.txt";
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                text_path = openFileDialog1.FileName;
                LoadText(text_path);
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            var r = new ResolutionForm(pictureBox1.Size);
            if (r.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Size = r.size;
            }
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            /*
            if (EQ)
            {
                LeaveEQmode();
            }
            else
            {
                EnterEQmode();
            }*/
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            Print_Subtitle();
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            LineChangeDetector();
        }

        private void TextBox_Click(object sender, EventArgs e)
        {
            //DeleteEQTag();
            LineChangeDetector();
            //InsertEQTag();
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
            if (!Modified) this.Text += " *";
            Modified = true;
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Back || e.KeyCode == Keys.PageUp || e.KeyCode == Keys.PageDown || e.KeyCode == Keys.Home || e.KeyCode == Keys.End)
            {
                var t = new Timer { Interval = 10, Enabled = true };
                t.Tick += (s, ev) =>
                {
                    t.Enabled = false;
                    //DeleteEQTag();
                    LineChangeDetector();
                    //InsertEQTag();
                };
            }
            else if (e.KeyCode == Keys.S && e.Modifiers == Keys.Control)
            {
                if (text_path == "")
                {
                    string path;
                    if (text_path == "") { path = Environment.CurrentDirectory; }
                    else { path = new DirectoryInfo(text_path).Parent.FullName; }
                    saveFileDialog1.FileName = "Subtitle.txt";
                    saveFileDialog1.Filter = "テキストファイル(*.txt)|*.txt";
                    saveFileDialog1.InitialDirectory = path;
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        text_path = saveFileDialog1.FileName;
                        SaveText(text_path);
                    }
                }
                else
                {
                    SaveText(text_path);
                }
            }
            else if (e.KeyCode == Keys.T && e.Modifiers == Keys.Control)
            {
                InsertEQTag();
            }
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
            }
            */
        }

        private void EnterEQmode()
        {
            EQ = true;
            button7.BackColor = SystemColors.ActiveCaption;
            //this.Text += " - EQ mode";
        }

        private void LeaveEQmode()
        {
            EQ = false;
            //textBox.SelectionLength = 0;
            //textBox.SelectionColor = TextColor;
            //textBox.ImeMode = ImeMode.Hiragana;
            button7.BackColor = SystemColors.Control;
            //this.Text = "Form1";
        }

        private void InsertEQTag()
        {
            //if (!EQ) return;
            int previousSelectionStart = textBox.SelectionStart;
            textBox.Text = textBox.Text.Insert(previousSelectionStart, begintag);
            previousSelectionStart += begintag.Length;
            textBox.SelectionStart = previousSelectionStart;
            textBox.Text = textBox.Text.Insert(previousSelectionStart, endtag);
            lasttextag =previousSelectionStart;
            var t = new Timer { Interval = 1 ,Enabled=true};
            t.Tick += (s, e) => { t.Enabled = false; textBox.SelectionStart = lasttextag; };
        }

        private void DeleteEQTag()
        {
            if (currentline < 0 || currentline >= textBox.Lines.Length) return;
            var text = textBox.Lines[currentline];
            var begin = text.Contains(begintag);
            var end = text.Contains(endtag);
            if (begin && !end)
            {
                text = text.Remove(text.IndexOf(begintag), begintag.Length);
            }
            else if (!begin && end)
            {
                text = text.Remove(text.IndexOf(endtag), endtag.Length);
            }
            else if (begin && end)
            {
                Regex r = new Regex(String.Format("(?<={0}).*(?={1})", begintag, endtag));
                var match = r.Match(text).Value;
                while (match.EndsWith(" ") || match.EndsWith("　")) { match.Remove(match.Length - 1, 1); }
                if (match == "")
                {
                    text = text.Remove(text.IndexOf(begintag), begintag.Length);
                    text = text.Remove(text.IndexOf(endtag), begintag.Length);
                }
            }
            textBox.Lines[currentline] = text;
        }

        private void SaveText(string path)
        {
            var sw = new StreamWriter(path);
            sw.Write(textBox.Text);
            sw.Close();
            Notice(String.Format("{0}として保存しました", new DirectoryInfo(path).Name));
            this.Text = OriginalFormTitle + " - " + new DirectoryInfo(path).Name;
            Modified = false;
        }

        private void LoadText(string path)
        {
            var sw = new StreamReader(path);
            textBox.Text = sw.ReadToEnd();
            sw.Close();
            Notice(String.Format("{0}を読み込みました", new DirectoryInfo(path).Name));
            this.Text = OriginalFormTitle + " - " + new DirectoryInfo(path).Name;
            Modified = false;
        }

        private void Notice(string message)
        {
            toolStripStatusLabel1.Text = message;
            var timer = new Timer { Interval = 5000, Enabled = true };
            timer.Tick += (s, e) =>
            {
                timer.Enabled = false;
                toolStripStatusLabel1.Text = "";
            };
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Save_Subtitles();
        }

        private void LineChangeDetector()
        {
            int currentline = 0;
            int charactors = 0;
            if (textBox.Lines.Length == 0) { Print_Subtitle(); return; }
            while (true)
            {
                charactors += textBox.Lines[currentline].Length + 1;//+1は改行コード
                if (charactors > textBox.SelectionStart) { break; }
                else if (currentline >= textBox.Lines.Length - 1) { break; }
                currentline++;
            }
            if (currentline != this.currentline) { this.currentline = currentline; }
            Print_Subtitle();
        }

        private void Print_Subtitle()
        {
            string text = "";
            Bitmap result = null;
            if (pictureBox1.Image != null) pictureBox1.Image.Dispose();
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
                while (text.EndsWith(" ") || text.EndsWith("　")) { text = text.Remove(text.Length - 1, 1); }
                if (text.EndsWith(":") || text.EndsWith("：")) text = "";
                if (text.Contains("%")) text = text.Split('%')[0];
                else if (text.Contains("％")) text = text.Split('％')[0];
                if (text.Contains(begintag) && text.Contains(endtag))
                {
                    string s1, tex, s2;
                    s1 = text.Substring(0, text.IndexOf(begintag));
                    if (text.IndexOf(begintag) + begintag.Length < 0 || text.IndexOf(endtag) - text.IndexOf(begintag) - begintag.Length < 0) return;
                    tex = text.Substring(text.IndexOf(begintag) + begintag.Length, text.IndexOf(endtag) - text.IndexOf(begintag) - begintag.Length);
                    s2 = text.Substring(text.IndexOf(endtag) + endtag.Length);
                    Bitmap b1, bittex, b2;
                    b1 = Graphicer(s1);
                    bittex = TexPrinter(tex);
                    b2 = Graphicer(s2);
                    if (bittex != null)
                    {
                        int b1_width = 0;
                        int b2_width = 0;
                        if (b1 != null)
                        {
                            b1_width = b1.Width;
                        }
                        if (b2 != null)
                        {
                            b2_width = b2.Width;
                        }
                        result = new Bitmap(b1_width + bittex.Width + b2_width, pictureBox1.Height);
                        using (Graphics g = Graphics.FromImage(result))
                        {
                            var bittex_heightpos = result.Height - bittex.Height;
                            if (bittex_heightpos < 0)
                            {
                                bittex = Shrink(bittex, result.Height);
                                bittex_heightpos = 0;
                            }
                            if (b1 != null) g.DrawImage(b1, 0, 0);
                            g.DrawImage(bittex, b1_width, bittex_heightpos / 2);
                            if (b2 != null) g.DrawImage(b2, b1_width + bittex.Width, 0);
                        }
                    }
                    else
                    {
                        result = Graphicer(s1 + s2);
                    }
                }
                else
                {
                    result = Graphicer(text);
                }
            }
            if (pictureBox1.Image != null) pictureBox1.Image.Dispose();
            //PictureBox1に表示する
            pictureBox1.Image = result;
        }

        private void Save_Subtitles()
        {
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory);
            List<string> files = new List<string>();
            Regex r = new Regex(@"Line\d*\.bmp");
            foreach (var f in di.GetFiles("*.bmp"))
            {
                if (r.IsMatch(f.Name))
                {
                    new FileInfo(f.FullName).Delete();
                }
            }
            for (int currentline = 0; currentline < textBox.Lines.Length; currentline++)
            {
                string text = "";
                Bitmap result = null;
                if (pictureBox1.Image != null) pictureBox1.Image.Dispose();
                if (textBox.Lines[currentline].Length != 0)
                {
                    text = textBox.Lines[currentline];
                    while (text.EndsWith(" ") || text.EndsWith("　")) { text = text.Remove(text.Length - 1, 1); }
                    if (text.EndsWith(":") || text.EndsWith("：")) continue;
                    if (text.Contains("%")) text = text.Split('%')[0];
                    else if (text.Contains("％")) text = text.Split('％')[0];
                    if (text == "") continue;
                    if (text.Contains(begintag) && text.Contains(endtag))
                    {
                        string s1, tex, s2;
                        s1 = text.Substring(0, text.IndexOf(begintag));
                        if (text.IndexOf(begintag) + begintag.Length < 0 || text.IndexOf(endtag) - text.IndexOf(begintag) - begintag.Length < 0) return;
                        tex = text.Substring(text.IndexOf(begintag) + begintag.Length, text.IndexOf(endtag) - text.IndexOf(begintag) - begintag.Length);
                        s2 = text.Substring(text.IndexOf(endtag) + endtag.Length);
                        if (tex != "")
                        {
                            Bitmap b1, bittex, b2;
                            b1 = Graphicer(s1);
                            bittex = TexPrinter(tex);
                            b2 = Graphicer(s2);
                            int b1_width = 0;
                            int b2_width = 0;
                            if (b1 != null)
                            {
                                b1_width = b1.Width;
                            }
                            if (b2 != null)
                            {
                                b2_width = b2.Width;
                            }
                            result = new Bitmap(b1_width + bittex.Width + b2_width, pictureBox1.Height);
                            using (Graphics g = Graphics.FromImage(result))
                            {
                                var bittex_heightpos = result.Height - bittex.Height;
                                if (bittex_heightpos < 0)
                                {
                                    bittex = Shrink(bittex, result.Height);
                                    bittex_heightpos = 0;
                                }
                                if (b1 != null) g.DrawImage(b1, 0, 0);
                                g.DrawImage(bittex, b1_width, bittex_heightpos / 2);
                                if (b2 != null) g.DrawImage(b2, b1_width + bittex.Width, 0);
                            }
                        }
                        else
                        {
                            result = Graphicer(s1 + s2);
                        }
                    }
                    else
                    {
                        result = Graphicer(text);
                    }
                    if (result != null) result.Save(String.Format("Line{0}.bmp", currentline));
                }
            }
        }

        private Bitmap Graphicer(string text)
        {
            string temp = text;
            while (temp.EndsWith(" ") || temp.EndsWith("　")) { temp = temp.Remove(temp.Length - 1, 1); }
            if (temp == "") return null;
            PointF pt = new PointF(0, pictureBox1.Height / 2);
            var strfmt = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
            if (radioButton1.Checked)
            {
                strfmt.Alignment = StringAlignment.Near;
            }
            else if (radioButton2.Checked)
            {
                strfmt.Alignment = StringAlignment.Center;
                pt = new PointF(pictureBox1.Width / 2, pictureBox1.Height / 2);
            }
            else if (radioButton3.Checked)
            {
                strfmt.Alignment = StringAlignment.Far;
                pt = new PointF(pictureBox1.Width, pictureBox1.Height / 2);
            }
            SizeF size = new SizeF(pictureBox1.Width, pictureBox1.Height);
            Bitmap canvas;
            bool gotsize = false;
            while (true)
            {
                //描画先とするImageオブジェクトを作成する
                canvas = new Bitmap((int)size.Width, pictureBox1.Height);
                //ImageオブジェクトのGraphicsオブジェクトを作成する
                using (var g = Graphics.FromImage(canvas))
                {
                    using (var fnt = new Font(PrintingFont.Name, PrintingFont.Size))
                    {
                        //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                        //文字列を位置pt、黒で表示
                        g.DrawString(text, fnt, Brushes.Black, pt, strfmt);
                        if (gotsize) break;
                        //画像サイズを取得
                        size = g.MeasureString(text, fnt);
                        if (size.Width == 0) break;
                        gotsize = true;
                        canvas.Dispose();
                    }
                }
            }
            strfmt.Dispose();
            return canvas;
        }

        private Bitmap TexPrinter(string latex)
        {
            try
            {
                if (latex == "") return null;
                while (latex.EndsWith(" ") || latex.EndsWith("　")) { latex = latex.Remove(latex.Length - 1, 1); }
                Bitmap bitmap = null;

                var parser = new TexFormulaParser();
                var formula = parser.Parse(latex);
                formula.TextStyle = "{StaticResource ClearTypeFormula}";
                var renderer = formula.GetRenderer(TexStyle.Display, PrintingFont.Size, "Arial");
                var bitmapsourse = renderer.RenderToBitmap(0, 0);
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapsourse));
                using (var ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    using (var temp = new Bitmap(ms))
                    {
                        bitmap = new Bitmap(temp);
                    }
                }
                return bitmap;
            }
            catch (Exception e) when(e is TexParseException || e is TexCharacterMappingNotFoundException)
            {
                return Graphicer("!!TexError!!");
            }
        }

        private Bitmap Shrink(Bitmap bm, int height)
        {
            Bitmap result = null;
            var width = bm.Width * (bm.Height / height);
            result = new Bitmap(width, height);
            using (var g = Graphics.FromImage(result))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(bm, 0, 0, width, height);
            }
            return result;
        }
    }
}