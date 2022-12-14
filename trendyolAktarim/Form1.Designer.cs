
namespace trendyolAktarim
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
            this.btnSqlKontrol = new System.Windows.Forms.Button();
            this.btnSiparis = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSqlKontrol
            // 
            this.btnSqlKontrol.Location = new System.Drawing.Point(13, 13);
            this.btnSqlKontrol.Name = "btnSqlKontrol";
            this.btnSqlKontrol.Size = new System.Drawing.Size(124, 43);
            this.btnSqlKontrol.TabIndex = 0;
            this.btnSqlKontrol.Text = "SQL Kontrol";
            this.btnSqlKontrol.UseVisualStyleBackColor = true;
            this.btnSqlKontrol.Click += new System.EventHandler(this.btnSqlKontrol_Click);
            // 
            // btnSiparis
            // 
            this.btnSiparis.Location = new System.Drawing.Point(143, 13);
            this.btnSiparis.Name = "btnSiparis";
            this.btnSiparis.Size = new System.Drawing.Size(133, 43);
            this.btnSiparis.TabIndex = 2;
            this.btnSiparis.Text = "Siparişleri Çek";
            this.btnSiparis.UseVisualStyleBackColor = true;
            this.btnSiparis.Click += new System.EventHandler(this.btnSiparis_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 72);
            this.Controls.Add(this.btnSiparis);
            this.Controls.Add(this.btnSqlKontrol);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing_1);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSqlKontrol;
        private System.Windows.Forms.Button btnSiparis;
    }
}

