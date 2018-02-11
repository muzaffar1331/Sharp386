namespace Sharp386
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LabelEAX = new System.Windows.Forms.Label();
            this.LabelEBX = new System.Windows.Forms.Label();
            this.LabelECX = new System.Windows.Forms.Label();
            this.LabelEDX = new System.Windows.Forms.Label();
            this.LabelEIP = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LabelEAX
            // 
            this.LabelEAX.AutoSize = true;
            this.LabelEAX.Location = new System.Drawing.Point(26, 29);
            this.LabelEAX.Name = "LabelEAX";
            this.LabelEAX.Size = new System.Drawing.Size(46, 20);
            this.LabelEAX.TabIndex = 0;
            this.LabelEAX.Text = "EAX:";
            // 
            // LabelEBX
            // 
            this.LabelEBX.AutoSize = true;
            this.LabelEBX.Location = new System.Drawing.Point(26, 72);
            this.LabelEBX.Name = "LabelEBX";
            this.LabelEBX.Size = new System.Drawing.Size(46, 20);
            this.LabelEBX.TabIndex = 1;
            this.LabelEBX.Text = "EBX:";
            // 
            // LabelECX
            // 
            this.LabelECX.AutoSize = true;
            this.LabelECX.Location = new System.Drawing.Point(26, 116);
            this.LabelECX.Name = "LabelECX";
            this.LabelECX.Size = new System.Drawing.Size(46, 20);
            this.LabelECX.TabIndex = 2;
            this.LabelECX.Text = "ECX:";
            // 
            // LabelEDX
            // 
            this.LabelEDX.AutoSize = true;
            this.LabelEDX.Location = new System.Drawing.Point(26, 160);
            this.LabelEDX.Name = "LabelEDX";
            this.LabelEDX.Size = new System.Drawing.Size(47, 20);
            this.LabelEDX.TabIndex = 3;
            this.LabelEDX.Text = "EDX:";
            // 
            // LabelEIP
            // 
            this.LabelEIP.AutoSize = true;
            this.LabelEIP.Location = new System.Drawing.Point(26, 208);
            this.LabelEIP.Name = "LabelEIP";
            this.LabelEIP.Size = new System.Drawing.Size(39, 20);
            this.LabelEIP.TabIndex = 4;
            this.LabelEIP.Text = "EIP:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 386);
            this.Controls.Add(this.LabelEIP);
            this.Controls.Add(this.LabelEDX);
            this.Controls.Add(this.LabelECX);
            this.Controls.Add(this.LabelEBX);
            this.Controls.Add(this.LabelEAX);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelEAX;
        private System.Windows.Forms.Label LabelEBX;
        private System.Windows.Forms.Label LabelECX;
        private System.Windows.Forms.Label LabelEDX;
        private System.Windows.Forms.Label LabelEIP;
    }
}

