namespace Subtitle_Printer
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.eqSizeButton = new System.Windows.Forms.Button();
            this.screenResolutionButton = new System.Windows.Forms.Button();
            this.textLoadButton = new System.Windows.Forms.Button();
            this.textSaveButton = new System.Windows.Forms.Button();
            this.printFontSelectButton = new System.Windows.Forms.Button();
            this.editorFontSelect = new System.Windows.Forms.Button();
            this.alignRightRadioButton = new System.Windows.Forms.RadioButton();
            this.alignCenterRadioButton = new System.Windows.Forms.RadioButton();
            this.alignLeftRadioButton = new System.Windows.Forms.RadioButton();
            this.printButton = new System.Windows.Forms.Button();
            this.Reference_TextBox = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel2 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.eqSizeButton);
            this.panel1.Controls.Add(this.screenResolutionButton);
            this.panel1.Controls.Add(this.textLoadButton);
            this.panel1.Controls.Add(this.textSaveButton);
            this.panel1.Controls.Add(this.printFontSelectButton);
            this.panel1.Controls.Add(this.editorFontSelect);
            this.panel1.Controls.Add(this.alignRightRadioButton);
            this.panel1.Controls.Add(this.alignCenterRadioButton);
            this.panel1.Controls.Add(this.alignLeftRadioButton);
            this.panel1.Controls.Add(this.printButton);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(892, 31);
            this.panel1.TabIndex = 1;
            // 
            // eqSizeButton
            // 
            this.eqSizeButton.Location = new System.Drawing.Point(291, 3);
            this.eqSizeButton.Name = "eqSizeButton";
            this.eqSizeButton.Size = new System.Drawing.Size(75, 23);
            this.eqSizeButton.TabIndex = 9;
            this.eqSizeButton.Text = "数式サイズ";
            this.eqSizeButton.UseVisualStyleBackColor = true;
            this.eqSizeButton.Click += new System.EventHandler(this.eqSizeButton_Click);
            // 
            // screenResolutionButton
            // 
            this.screenResolutionButton.Location = new System.Drawing.Point(602, 3);
            this.screenResolutionButton.Name = "screenResolutionButton";
            this.screenResolutionButton.Size = new System.Drawing.Size(75, 23);
            this.screenResolutionButton.TabIndex = 8;
            this.screenResolutionButton.Text = "画像解像度";
            this.screenResolutionButton.UseVisualStyleBackColor = true;
            this.screenResolutionButton.Click += new System.EventHandler(this.screenResolutionButton_Click);
            // 
            // textLoadButton
            // 
            this.textLoadButton.Location = new System.Drawing.Point(797, 3);
            this.textLoadButton.Name = "textLoadButton";
            this.textLoadButton.Size = new System.Drawing.Size(90, 23);
            this.textLoadButton.TabIndex = 7;
            this.textLoadButton.Text = "テキスト読込";
            this.textLoadButton.UseVisualStyleBackColor = true;
            this.textLoadButton.Click += new System.EventHandler(this.textLoadButton_Click);
            // 
            // textSaveButton
            // 
            this.textSaveButton.Location = new System.Drawing.Point(701, 3);
            this.textSaveButton.Name = "textSaveButton";
            this.textSaveButton.Size = new System.Drawing.Size(90, 23);
            this.textSaveButton.TabIndex = 6;
            this.textSaveButton.Text = "テキスト保存";
            this.textSaveButton.UseVisualStyleBackColor = true;
            this.textSaveButton.Click += new System.EventHandler(this.textSaveButton_Click);
            // 
            // printFontSelectButton
            // 
            this.printFontSelectButton.Location = new System.Drawing.Point(195, 3);
            this.printFontSelectButton.Name = "printFontSelectButton";
            this.printFontSelectButton.Size = new System.Drawing.Size(90, 23);
            this.printFontSelectButton.TabIndex = 5;
            this.printFontSelectButton.Text = "印刷用フォント";
            this.printFontSelectButton.UseVisualStyleBackColor = true;
            this.printFontSelectButton.Click += new System.EventHandler(this.printFontSelectButton_Click);
            // 
            // editorFontSelect
            // 
            this.editorFontSelect.Location = new System.Drawing.Point(99, 3);
            this.editorFontSelect.Name = "editorFontSelect";
            this.editorFontSelect.Size = new System.Drawing.Size(90, 23);
            this.editorFontSelect.TabIndex = 4;
            this.editorFontSelect.Text = "エディタフォント";
            this.editorFontSelect.UseVisualStyleBackColor = true;
            this.editorFontSelect.Click += new System.EventHandler(this.editorFontSelectButton_Click);
            // 
            // alignRightRadioButton
            // 
            this.alignRightRadioButton.AutoSize = true;
            this.alignRightRadioButton.Location = new System.Drawing.Point(540, 6);
            this.alignRightRadioButton.Name = "alignRightRadioButton";
            this.alignRightRadioButton.Size = new System.Drawing.Size(56, 16);
            this.alignRightRadioButton.TabIndex = 3;
            this.alignRightRadioButton.TabStop = true;
            this.alignRightRadioButton.Text = "右揃え";
            this.alignRightRadioButton.UseVisualStyleBackColor = true;
            this.alignRightRadioButton.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // alignCenterRadioButton
            // 
            this.alignCenterRadioButton.AutoSize = true;
            this.alignCenterRadioButton.Location = new System.Drawing.Point(466, 6);
            this.alignCenterRadioButton.Name = "alignCenterRadioButton";
            this.alignCenterRadioButton.Size = new System.Drawing.Size(68, 16);
            this.alignCenterRadioButton.TabIndex = 2;
            this.alignCenterRadioButton.TabStop = true;
            this.alignCenterRadioButton.Text = "中央揃え";
            this.alignCenterRadioButton.UseVisualStyleBackColor = true;
            this.alignCenterRadioButton.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // alignLeftRadioButton
            // 
            this.alignLeftRadioButton.AutoSize = true;
            this.alignLeftRadioButton.Checked = true;
            this.alignLeftRadioButton.Location = new System.Drawing.Point(404, 6);
            this.alignLeftRadioButton.Name = "alignLeftRadioButton";
            this.alignLeftRadioButton.Size = new System.Drawing.Size(56, 16);
            this.alignLeftRadioButton.TabIndex = 1;
            this.alignLeftRadioButton.TabStop = true;
            this.alignLeftRadioButton.Text = "左揃え";
            this.alignLeftRadioButton.UseVisualStyleBackColor = true;
            this.alignLeftRadioButton.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // printButton
            // 
            this.printButton.Location = new System.Drawing.Point(3, 3);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(90, 23);
            this.printButton.TabIndex = 0;
            this.printButton.Text = "印刷";
            this.printButton.UseVisualStyleBackColor = true;
            this.printButton.Click += new System.EventHandler(this.printButton_Click);
            // 
            // Reference_TextBox
            // 
            this.Reference_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Reference_TextBox.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Reference_TextBox.Location = new System.Drawing.Point(12, 50);
            this.Reference_TextBox.Multiline = true;
            this.Reference_TextBox.Name = "Reference_TextBox";
            this.Reference_TextBox.Size = new System.Drawing.Size(892, 333);
            this.Reference_TextBox.TabIndex = 2;
            this.Reference_TextBox.Text = "Reference TextBox";
            this.Reference_TextBox.WordWrap = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(2, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(472, 50);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.SizeChanged += new System.EventHandler(this.PictureBox1_SizeChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.AutoScroll = true;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Location = new System.Drawing.Point(12, 389);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(892, 102);
            this.panel2.TabIndex = 4;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 494);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(916, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(916, 516);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.Reference_TextBox);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button printButton;
        private System.Windows.Forms.TextBox Reference_TextBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RadioButton alignRightRadioButton;
        private System.Windows.Forms.RadioButton alignCenterRadioButton;
        private System.Windows.Forms.RadioButton alignLeftRadioButton;
        private System.Windows.Forms.Button editorFontSelect;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.Button printFontSelectButton;
        private System.Windows.Forms.Button textSaveButton;
        private System.Windows.Forms.Button textLoadButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button screenResolutionButton;
        private System.Windows.Forms.Button eqSizeButton;
    }
}

