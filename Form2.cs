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

namespace Subtitle_Printer
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            var latex = @"X";
            Bitmap bitmap = null;
            var parser = new TexFormulaParser();
            var formula = parser.Parse(latex);
            var renderer = formula.GetRenderer(TexStyle.Display, 20.0, "Arial");
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
            pictureBox2.Image = bitmap;
        }
    }
}
