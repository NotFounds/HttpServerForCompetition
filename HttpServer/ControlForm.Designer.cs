namespace HttpServer
{
    partial class ControlForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleatDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.allselectAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.colorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.button4 = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "AccessLog";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.BackColor = System.Drawing.Color.White;
            this.richTextBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.richTextBox1.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.richTextBox1.Location = new System.Drawing.Point(12, 60);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(471, 490);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.TabStop = false;
            this.richTextBox1.Text = "";
            this.richTextBox1.GotFocus += new System.EventHandler(this.richTextBox1_GotFocus);
            this.richTextBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.richTextBox1_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyCToolStripMenuItem,
            this.deleatDToolStripMenuItem,
            this.toolStripSeparator1,
            this.allselectAToolStripMenuItem,
            this.toolStripSeparator2,
            this.colorToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(143, 104);
            // 
            // copyCToolStripMenuItem
            // 
            this.copyCToolStripMenuItem.Name = "copyCToolStripMenuItem";
            this.copyCToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.copyCToolStripMenuItem.Text = "Copy(&C)";
            this.copyCToolStripMenuItem.Click += new System.EventHandler(this.copyCToolStripMenuItem_Click);
            // 
            // deleatDToolStripMenuItem
            // 
            this.deleatDToolStripMenuItem.Name = "deleatDToolStripMenuItem";
            this.deleatDToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.deleatDToolStripMenuItem.Text = "Deleat(&D)";
            this.deleatDToolStripMenuItem.Click += new System.EventHandler(this.deleatDToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(139, 6);
            // 
            // allselectAToolStripMenuItem
            // 
            this.allselectAToolStripMenuItem.Name = "allselectAToolStripMenuItem";
            this.allselectAToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.allselectAToolStripMenuItem.Text = "AllSelect(&A)";
            this.allselectAToolStripMenuItem.Click += new System.EventHandler(this.allselectAToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(139, 6);
            // 
            // colorToolStripMenuItem
            // 
            this.colorToolStripMenuItem.Name = "colorToolStripMenuItem";
            this.colorToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.colorToolStripMenuItem.Text = "Color";
            this.colorToolStripMenuItem.Click += new System.EventHandler(this.colorToolStripMenuItem_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "port : ";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(51, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(86, 19);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "8080";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 564);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "CommandLine";
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Location = new System.Drawing.Point(12, 579);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(471, 19);
            this.textBox2.TabIndex = 1;
            this.textBox2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox2_KeyDown);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(143, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 22);
            this.button1.TabIndex = 3;
            this.button1.Text = "ServerStart";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(229, 10);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(81, 22);
            this.button2.TabIndex = 4;
            this.button2.Text = "ContestStart";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(421, 10);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(62, 22);
            this.button3.TabIndex = 5;
            this.button3.Text = "ClearLog";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // colorDialog1
            // 
            this.colorDialog1.AnyColor = true;
            this.colorDialog1.SolidColorOnly = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(229, 34);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(81, 23);
            this.button4.TabIndex = 6;
            this.button4.Text = "ContestStop";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // ControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(498, 611);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 800);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "ControlForm";
            this.Text = "HttpServer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ControlForm_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleatDToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem allselectAToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem colorToolStripMenuItem;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button button4;
    }
}

