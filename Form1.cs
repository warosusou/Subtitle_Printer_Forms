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
        private const string beginTag = "$";
        private const string endTag = "$";
        private ImeReadableRichTextBox textBox;
        private Color textColor;
        private int currentLine = -1;
        private int lastTextTag = -1;
        private Font printingFont;
        private string textPath = "";
        private string originalFormTitle;
        private bool modified = false;
        private bool EQ = false;

        Font PrintingFont
        {
            get { return printingFont; }
            set { printingFont = value; ImageDrawer.PrintingFont = printingFont; }
        }

        public Form1()
        {
            InitializeComponent();
            textBox = new ImeReadableRichTextBox
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
            textColor = textBox.SelectionColor;
            PrintingFont = new Font("メイリオ", 20);
            //ImageGenerator.Alignmentを設定させるためにradioButton_Checkedを発火
            panel1.Controls.OfType<RadioButton>().Single(x => x.Checked == true).Checked = true;
            toolStripStatusLabel1.Text = "";
            originalFormTitle = this.Text;
            this.Text += " - 無題";
            this.ActiveControl = textBox;
            ImageDrawer.pictureBox1 = pictureBox1.Size;
            LineChangeDetector();
            eqSizeButton.Visible = true;
            ImageDrawer.EQSize = PrintingFont.Size;
            ImageDrawer.AutoShrink = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!modified) return;
            string name;
            if (textPath == "") name = "無題";
            else name = new DirectoryInfo(textPath).Name;
            var sd = new SaveDialog(name);
            switch (sd.ShowDialog())
            {
                case DialogResult.Yes:
                    if (textPath == "")
                    {
                        string path = Environment.CurrentDirectory;
                        saveFileDialog1.FileName = "Subtitle.txt";
                        saveFileDialog1.Filter = "テキストファイル(*.txt)|*.txt";
                        saveFileDialog1.InitialDirectory = path;
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            SaveText(textPath);
                        }
                        else e.Cancel = true;
                    }
                    else
                    {
                        SaveText(textPath);
                    }
                    break;
                case DialogResult.No:
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
            }

        }

        private void EditorFontSelectButton_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = new Font(textBox.Font.Name, textBox.Font.Size);
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox.Font = fontDialog1.Font;
            }
        }

        private void PrintFontSelectButton_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = new Font(PrintingFont.Name, PrintingFont.Size);
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                PrintingFont.Dispose();
                PrintingFont = fontDialog1.Font;
                PrintSubtitle();
            }
        }

        private void TextSaveButton_Click(object sender, EventArgs e)
        {
            string path;
            if (textPath == "") { path = Environment.CurrentDirectory; }
            else { path = new DirectoryInfo(textPath).Parent.FullName; }
            saveFileDialog1.FileName = "Subtitle.txt";
            saveFileDialog1.Filter = "テキストファイル(*.txt)|*.txt";
            saveFileDialog1.InitialDirectory = path;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textPath = saveFileDialog1.FileName;
                SaveText(textPath);
            }
        }

        private void TextLoadButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "テキストファイル(*.txt)|*.txt";
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textPath = openFileDialog1.FileName;
                LoadText(textPath);
            }
        }

        private void ScreenResolutionButton_Click(object sender, EventArgs e)
        {
            var r = new ResolutionForm(pictureBox1.Size);
            if (r.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Size = r.size;
            }
        }

        private void EqSizeButton_Click(object sender, EventArgs e)
        {
            var f = new EqFontSizeForm(ImageDrawer.pictureBox1,ImageDrawer.EQSize,ImageDrawer.AutoShrink);
            f.ShowDialog();
            if(f.DialogResult == DialogResult.OK)
            {
                ImageDrawer.EQSize = f.EQSize;
                ImageDrawer.AutoShrink = f.AutoShrink;
                PrintSubtitle();
            }
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (alignLeftRadioButton.Checked)
            {
                ImageDrawer.Alignment = Alignment.Left;
            }
            else if (alignCenterRadioButton.Checked)
            {
                ImageDrawer.Alignment = Alignment.Center;
            }
            else if (alignRightRadioButton.Checked)
            {
                ImageDrawer.Alignment = Alignment.Right;
            }
            PrintSubtitle();
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
            if (!modified) this.Text += " *";
            modified = true;
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
                if (textPath == "")
                {
                    string path;
                    if (textPath == "") { path = Environment.CurrentDirectory; }
                    else { path = new DirectoryInfo(textPath).Parent.FullName; }
                    saveFileDialog1.FileName = "Subtitle.txt";
                    saveFileDialog1.Filter = "テキストファイル(*.txt)|*.txt";
                    saveFileDialog1.InitialDirectory = path;
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        textPath = saveFileDialog1.FileName;
                        SaveText(textPath);
                    }
                }
                else
                {
                    SaveText(textPath);
                }
            }
            else if (e.KeyCode == Keys.T && e.Modifiers == Keys.Control)
            {
                InsertEQTag();
            }
        }

        private void TextBox_ImeCompositionHira(object sender, ImeReadableRichTextBox.ImeCompositionEventArgs e)
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

        private void PictureBox1_SizeChanged(object sender, EventArgs e)
        {
            ImageDrawer.pictureBox1 = pictureBox1.Size;
        }

        private void EnterEQmode()
        {
            EQ = true;
            eqSizeButton.BackColor = SystemColors.ActiveCaption;
            //this.Text += " - EQ mode";
        }

        private void LeaveEQmode()
        {
            EQ = false;
            //textBox.SelectionLength = 0;
            //textBox.SelectionColor = TextColor;
            //textBox.ImeMode = ImeMode.Hiragana;
            eqSizeButton.BackColor = SystemColors.Control;
            //this.Text = "Form1";
        }

        private void InsertEQTag()
        {
            //if (!EQ) return;
            int previousSelectionStart = textBox.SelectionStart;
            textBox.Text = textBox.Text.Insert(previousSelectionStart, beginTag);
            previousSelectionStart += beginTag.Length;
            textBox.SelectionStart = previousSelectionStart;
            textBox.Text = textBox.Text.Insert(previousSelectionStart, endTag);
            lastTextTag = previousSelectionStart;
            var t = new Timer { Interval = 1, Enabled = true };
            t.Tick += (s, e) => { t.Enabled = false; textBox.SelectionStart = lastTextTag; };
        }

        private void DeleteEQTag()
        {
            if (currentLine < 0 || currentLine >= textBox.Lines.Length) return;
            var text = textBox.Lines[currentLine];
            var begin = text.Contains(beginTag);
            var end = text.Contains(endTag);
            if (begin && !end)
            {
                text = text.Remove(text.IndexOf(beginTag), beginTag.Length);
            }
            else if (!begin && end)
            {
                text = text.Remove(text.IndexOf(endTag), endTag.Length);
            }
            else if (begin && end)
            {
                Regex r = new Regex(String.Format("(?<={0}).*(?={1})", beginTag, endTag));
                var match = r.Match(text).Value;
                while (match.EndsWith(" ") || match.EndsWith("　")) { match.Remove(match.Length - 1, 1); }
                if (match == "")
                {
                    text = text.Remove(text.IndexOf(beginTag), beginTag.Length);
                    text = text.Remove(text.IndexOf(endTag), beginTag.Length);
                }
            }
            textBox.Lines[currentLine] = text;
        }

        private void SaveText(string path)
        {
            var sw = new StreamWriter(path);
            sw.Write(textBox.Text);
            sw.Close();
            Notice(String.Format("{0}として保存しました", new DirectoryInfo(path).Name));
            this.Text = originalFormTitle + " - " + new DirectoryInfo(path).Name;
            modified = false;
        }

        private void LoadText(string path)
        {
            var sw = new StreamReader(path);
            textBox.Text = sw.ReadToEnd();
            sw.Close();
            Notice(String.Format("{0}を読み込みました", new DirectoryInfo(path).Name));
            this.Text = originalFormTitle + " - " + new DirectoryInfo(path).Name;
            modified = false;
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

        private void PrintButton_Click(object sender, EventArgs e)
        {
            SaveSubtitles();
        }

        private void LineChangeDetector()
        {
            int currentline = 0;
            int charactors = 0;
            if (textBox.Lines.Length == 0) { PrintSubtitle(); return; }
            while (true)
            {
                charactors += textBox.Lines[currentline].Length + 1;//+1は改行コード
                if (charactors > textBox.SelectionStart) { break; }
                else if (currentline >= textBox.Lines.Length - 1) { break; }
                currentline++;
            }
            if (currentline != this.currentLine) { this.currentLine = currentline; }
            PrintSubtitle();
        }

        private void PrintSubtitle()
        {
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
                result = ImageDrawer.LineBitmap(textBox.Lines[currentline]);
            }
            if (pictureBox1.Image != null) pictureBox1.Image.Dispose();
            //PictureBox1に表示する
            pictureBox1.Image = result;
        }

        private void SaveSubtitles()
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
                if (textBox.Lines[currentline].Length != 0)
                {
                    result = ImageDrawer.LineBitmap(textBox.Lines[currentline]);
                    text = textBox.Lines[currentline];
                    while (text.EndsWith(" ") || text.EndsWith("　")) { text = text.Remove(text.Length - 1, 1); }
                    if (text.EndsWith(":") || text.EndsWith("：")) continue;
                    if (text.Contains("%")) text = text.Split('%')[0];
                    else if (text.Contains("％")) text = text.Split('％')[0];
                    if (text == "") continue;
                    if (result != null) result.Save(String.Format("Line{0}.bmp", currentline));
                }
            }
            Notice("印刷完了");
        }

        internal static class ImageDrawer
        {
            internal static Font PrintingFont;
            internal static Size pictureBox1;
            internal static Alignment Alignment;
            internal static double EQSize;
            internal static bool AutoShrink;

            internal static Bitmap TexPrinter(string latex)
            {
                return TexPrinter(latex, EQSize);
            }

            internal static Bitmap TexPrinter(string latex,double scale)
            {
                try
                {
                    if (latex == "") return null;
                    while (latex.EndsWith(" ") || latex.EndsWith("　")) { latex = latex.Remove(latex.Length - 1, 1); }
                    Bitmap bitmap = null;

                    var parser = new TexFormulaParser();
                    var formula = parser.Parse(latex);
                    formula.TextStyle = "{StaticResource ClearTypeFormula}";
                    var renderer = formula.GetRenderer(TexStyle.Display, scale, "Arial");
                    if(renderer.RenderSize.Width == 0 || renderer.RenderSize.Height == 0) { return bitmap; }
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
                catch (Exception e) when (e is TexParseException || e is TexCharacterMappingNotFoundException)
                {
                    return Graphicer("!!TexError!!");
                }
            }

            internal static Bitmap Graphicer(string text)
            {
                string temp = text;
                while (temp.EndsWith(" ") || temp.EndsWith("　")) { temp = temp.Remove(temp.Length - 1, 1); }
                if (temp == "") return null;
                PointF pt = new PointF(0, pictureBox1.Height / 2);
                var strfmt = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
                switch (Alignment)
                {
                    case Alignment.Left:
                        strfmt.Alignment = StringAlignment.Near;
                        break;
                    case Alignment.Center:
                        strfmt.Alignment = StringAlignment.Center;
                        pt = new PointF(pictureBox1.Width / 2, pictureBox1.Height / 2);
                        break;
                    case Alignment.Right:
                        strfmt.Alignment = StringAlignment.Far;
                        pt = new PointF(pictureBox1.Width, pictureBox1.Height / 2);
                        break;
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

            internal static Bitmap Shrink(Bitmap bm)
            {
                Bitmap result = null;
                var width = bm.Width * (int)(bm.Height / (pictureBox1.Height * 0.95));
                result = new Bitmap(width, (int)(pictureBox1.Height * 0.95));
                using (var g = Graphics.FromImage(result))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(bm, 0, 0, width, (int)(pictureBox1.Height * 0.95));
                }
                return result;
            }

            internal static Bitmap LineBitmap(string text)
            {
                Bitmap result = null;
                while (text.EndsWith(" ") || text.EndsWith("　")) { text = text.Remove(text.Length - 1, 1); }
                if (text.EndsWith(":") || text.EndsWith("：")) text = "";
                if (text.Contains("%")) text = text.Split('%')[0];
                else if (text.Contains("％")) text = text.Split('％')[0];
                if (text == "") { return result; }
                if (text.Contains(beginTag) && text.Length > text.IndexOf(beginTag) + beginTag.Length && text.Substring(text.IndexOf(beginTag) + beginTag.Length).Contains(endTag))
                {
                    var sections = new List<Section>();
                    int result_width = 0;
                    int result_height = ImageDrawer.pictureBox1.Height;
                    while (text.Length != 0)
                    {
                        string section = "";
                        int begintagpos = text.IndexOf(beginTag);
                        if (begintagpos > 0)
                        {
                            section = text.Substring(0, begintagpos);
                            text = text.Substring(begintagpos);
                        }
                        else if(begintagpos == 0 && text.IndexOf(endTag,begintagpos + 1) > begintagpos)
                        {
                            section = text.Substring(0, text.IndexOf(endTag,begintagpos + 1) + endTag.Length);
                            if (text.IndexOf(endTag, begintagpos + 1) + endTag.Length < text.Length)
                            {
                                text = text.Substring(text.IndexOf(endTag,begintagpos + 1) + endTag.Length);
                            }
                            else
                            {
                                text = "";
                            }
                        }
                        else
                        {
                            section = text;
                            text = "";
                        }
                        if (section == "") { continue; }
                        sections.Add(new Section(section));
                    }
                    sections.RemoveAll(x => x.Image == null || x.Image.Width == 0);
                    foreach(var s in sections)
                    {
                        if (s.Image.Height > result_height && ImageDrawer.AutoShrink) { s.ShrinkImage(); }
                        result_width += s.Image.Width;
                    }
                    if (result_width == 0) { return result; }
                    result = new Bitmap(result_width, result_height);
                    using (Graphics g = Graphics.FromImage(result))
                    {
                        int xpos = 0;
                        foreach (var s in sections)
                        {
                            var ypos = (result.Height - s.Image.Height) / 2;
                            g.DrawImage(s.Image, xpos, ypos);
                            xpos += s.Image.Width;
                        }
                    }
                }
                else
                {
                    result = ImageDrawer.Graphicer(text);
                }
                return result;
            }
        }
        class Section
        {
            private string text;
            public Bitmap Image { get; private set; }
            public string Text
            {
                get
                {
                    return text;
                }
                set
                {
                    text = value;
                    if (text.Length >= beginTag.Length + endTag.Length &&text.IndexOf(beginTag) == 0 && text.IndexOf(endTag,text.IndexOf(beginTag) + beginTag.Length) == text.Length -1)
                    {
                        text = text.Remove(0, beginTag.Length).Remove(text.Length - 1 - endTag.Length, endTag.Length);
                        Image = ImageDrawer.TexPrinter(text);
                    }
                    else
                    {
                        Image = ImageDrawer.Graphicer(text);
                    }
                }
            }
            public Section(string text)
            {
                this.text = "";
                this.Image = null;
                Text = text;
            }

            public void ShrinkImage()
            {
                Image = ImageDrawer.Shrink(Image);
            }
        }
        internal enum Alignment
        {
            Left, Center, Right
        }
    }
}