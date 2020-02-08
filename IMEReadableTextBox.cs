using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;

namespace Subtitle_Printer
{
    public class ImeReadableRichTextBox : RichTextBox
    {
        public delegate void ImeCompositionEventHandler(object sender, ImeCompositionEventArgs e);

        public event ImeCompositionEventHandler ImeCompositionHira = null;
        public event ImeCompositionEventHandler ImeCompositionKata = null;
        public event ImeCompositionEventHandler ImeCompositedHira = null;
        public event ImeCompositionEventHandler ImeCompositedKata = null;

        // IMEでキーが押されたかのフラグ
        private const int WM_IME_COMPOSITION = 0x010F;
        // 変換確定後文字取得に使用する値(ひらがな)
        private const int GCS_RESULTSTR = 0x0800;
        // 変換確定後文字取得に使用する値(1バイトカタカナ)
        private const int GCS_RESULTREADSTR = 0x0200;
        // IME入力中文字取得に使用する値(ひらがな)
        private const int GCS_COMPSTR = 0x0008;
        // IME入力中文字取得に使用する値(1バイトカタカナ)
        private const int GCS_COMPREADSTR = 0x0001;

        [DllImport("Imm32.dll")]
        private static extern int ImmGetContext(IntPtr hWnd);

        [DllImport("Imm32.dll")]
        private static extern int ImmGetCompositionString(
            int hIMC, int dwIndex, StringBuilder lpBuf, int dwBufLen);

        [DllImport("Imm32.dll")]
        private static extern bool ImmReleaseContext(IntPtr hWnd, int hIMC);


        protected override void WndProc(ref Message m)
        {
            // 変換確定(1バイトカタカナ)
            if (ImeCompositedKata != null && m.Msg == WM_IME_COMPOSITION)
            {
                if (((int)m.LParam & GCS_RESULTREADSTR) > 0)
                {
                    int hIMC = ImmGetContext(this.Handle);
                    int strLen = ImmGetCompositionString(hIMC, GCS_RESULTREADSTR, null, 0);
                    StringBuilder str = new StringBuilder(strLen);

                    ImmGetCompositionString(hIMC, GCS_RESULTREADSTR, str, str.Capacity);
                    ImmReleaseContext(this.Handle, hIMC);

                    if (ImeCompositedKata != null)
                    {
                        // 環境によって文字コードが違うので、それにあわせる
                        byte[] tmp1 = System.Text.Encoding.Default.GetBytes(str.ToString());
                        byte[] tmp2 = new byte[strLen];
                        Array.Copy(tmp1, 0, tmp2, 0, strLen);
                        ImeCompositedKata(this,
                            new ImeCompositionEventArgs(System.Text.Encoding.Default.GetString(tmp2)));
                    }
                }
            }

            // 変換確定(ひらがな)
            if (ImeCompositedHira != null && m.Msg == WM_IME_COMPOSITION)
            {
                if (((int)m.LParam & GCS_RESULTSTR) > 0)
                {
                    int hIMC = ImmGetContext(this.Handle);
                    int strLen = ImmGetCompositionString(hIMC, GCS_RESULTSTR, null, 0);
                    StringBuilder str = new StringBuilder(strLen);

                    ImmGetCompositionString(hIMC, GCS_RESULTSTR, str, str.Capacity);
                    ImmReleaseContext(this.Handle, hIMC);

                    if (ImeCompositedHira != null)
                    {
                        // 環境によって文字コードが違うので、それにあわせる
                        byte[] tmp1 = Encoding.Default.GetBytes(str.ToString());
                        byte[] tmp2 = new byte[strLen];
                        Array.Copy(tmp1, 0, tmp2, 0, strLen);
                        ImeCompositedHira(this,
                            new ImeCompositionEventArgs(Encoding.Default.GetString(tmp2)));
                    }
                }
            }

            // IMEで編集中の文字列 (1バイトカタカナ)
            if (ImeCompositionKata != null && m.Msg == WM_IME_COMPOSITION)
            {
                if (((int)m.LParam & GCS_COMPREADSTR) > 0)
                {
                    int hIMC = ImmGetContext(this.Handle);
                    int strLen = ImmGetCompositionString(hIMC, GCS_COMPREADSTR, null, 0);
                    StringBuilder str = new StringBuilder(strLen);

                    ImmGetCompositionString(hIMC, GCS_COMPREADSTR, str, str.Capacity);
                    ImmReleaseContext(this.Handle, hIMC);

                    if (ImeCompositionKata != null)
                    {
                        // 環境によって文字コードが違うので、それにあわせる
                        byte[] tmp1 = Encoding.Default.GetBytes(str.ToString());
                        byte[] tmp2 = new byte[strLen];
                        Array.Copy(tmp1, 0, tmp2, 0, strLen);
                        ImeCompositionKata(this,
                            new ImeCompositionEventArgs(Encoding.Default.GetString(tmp2)));
                    }
                }
            }

            // IMEで編集中の文字列 (ひらがな)
            if (ImeCompositionHira != null && m.Msg == WM_IME_COMPOSITION)
            {
                if (((int)m.LParam & GCS_COMPSTR) > 0)
                {
                    int hIMC = ImmGetContext(this.Handle);
                    int strLen = ImmGetCompositionString(hIMC, GCS_COMPSTR, null, 0);
                    StringBuilder str = new StringBuilder(strLen);

                    ImmGetCompositionString(hIMC, GCS_COMPSTR, str, str.Capacity);
                    ImmReleaseContext(this.Handle, hIMC);

                    if (ImeCompositionHira != null)
                    {
                        // 環境によって文字コードが違うので、それにあわせる
                        byte[] tmp1 = Encoding.Default.GetBytes(str.ToString());
                        byte[] tmp2 = new byte[strLen];
                        Array.Copy(tmp1, 0, tmp2, 0, strLen);
                        ImeCompositionHira(this,
                            new ImeCompositionEventArgs(Encoding.Default.GetString(tmp2)));
                    }
                }
            }
            base.WndProc(ref m);
        }

        [Serializable]
        public class ImeCompositionEventArgs : EventArgs
        {
            private string str;
            public ImeCompositionEventArgs(string str)
            {
                this.str = str;
            }

            public string InputString
            {
                set
                {
                    this.str = value;
                }
                get
                {
                    return this.str;
                }
            }
        }
    }
}