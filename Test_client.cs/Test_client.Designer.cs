using System.Windows.Forms;
using System;
namespace Test_client.cs
{
    partial class Test_client
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
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
            this.btn_ligar = new System.Windows.Forms.Button();
            this.pedir_lista = new System.Windows.Forms.Timer();
            this.SuspendLayout();
            // 
            // btn_ligar
            // 
            this.btn_ligar.Location = new System.Drawing.Point(98, 12);
            this.btn_ligar.Name = "btn_ligar";
            this.btn_ligar.Size = new System.Drawing.Size(75, 23);
            this.btn_ligar.TabIndex = 0;
            this.btn_ligar.Text = "Ligar";
            this.btn_ligar.UseVisualStyleBackColor = true;
            this.btn_ligar.Click += new System.EventHandler(this.btn_ligar_Click);
            // 
            // pedir_lista
            // 
            this.pedir_lista.Enabled = true;
            this.pedir_lista.Interval = 120000;
            this.pedir_lista.Tick += new System.EventHandler(this.pedir_lista_Tick);
            // 
            // Test_client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 50);
            this.Controls.Add(this.btn_ligar);
            this.Name = "Test_client";
            this.Text = "GuerrillaIRC - Test Client";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_ligar;
        private Timer pedir_lista;
    }
}

