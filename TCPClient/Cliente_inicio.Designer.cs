using System;
using System.Windows.Forms;
namespace TCPClient
{
    partial class Cliente_inicio
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
            this.components = new System.ComponentModel.Container();
            this.btn_ligar = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txt_status = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_comando = new System.Windows.Forms.TextBox();
            this.btn_comando = new System.Windows.Forms.Button();
            this.txt_clientes = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_servidor = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_porto = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_nick = new System.Windows.Forms.TextBox();
            this.pedir_lista = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_ligar
            // 
            this.btn_ligar.Location = new System.Drawing.Point(571, 12);
            this.btn_ligar.Name = "btn_ligar";
            this.btn_ligar.Size = new System.Drawing.Size(111, 23);
            this.btn_ligar.TabIndex = 0;
            this.btn_ligar.Text = "Ligar ao Servidor";
            this.btn_ligar.UseVisualStyleBackColor = true;
            this.btn_ligar.Click += new System.EventHandler(this.btn_ligar_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txt_status);
            this.groupBox1.Location = new System.Drawing.Point(12, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(540, 367);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Status";
            // 
            // txt_status
            // 
            this.txt_status.Location = new System.Drawing.Point(6, 19);
            this.txt_status.Name = "txt_status";
            this.txt_status.ReadOnly = true;
            this.txt_status.Size = new System.Drawing.Size(522, 335);
            this.txt_status.TabIndex = 2;
            this.txt_status.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 428);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Comando:";
            // 
            // txt_comando
            // 
            this.txt_comando.Location = new System.Drawing.Point(76, 425);
            this.txt_comando.Name = "txt_comando";
            this.txt_comando.Size = new System.Drawing.Size(395, 20);
            this.txt_comando.TabIndex = 3;
            // 
            // btn_comando
            // 
            this.btn_comando.Location = new System.Drawing.Point(477, 423);
            this.btn_comando.Name = "btn_comando";
            this.btn_comando.Size = new System.Drawing.Size(75, 23);
            this.btn_comando.TabIndex = 4;
            this.btn_comando.Text = "Enviar";
            this.btn_comando.UseVisualStyleBackColor = true;
            this.btn_comando.Click += new System.EventHandler(this.btn_comando_Click);
            // 
            // txt_clientes
            // 
            this.txt_clientes.FormattingEnabled = true;
            this.txt_clientes.Location = new System.Drawing.Point(571, 49);
            this.txt_clientes.Name = "txt_clientes";
            this.txt_clientes.Size = new System.Drawing.Size(176, 355);
            this.txt_clientes.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Servidor:";
            // 
            // txt_servidor
            // 
            this.txt_servidor.Location = new System.Drawing.Point(73, 14);
            this.txt_servidor.Name = "txt_servidor";
            this.txt_servidor.Size = new System.Drawing.Size(135, 20);
            this.txt_servidor.TabIndex = 7;
            this.txt_servidor.Text = "localhost";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(218, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Porto:";
            // 
            // txt_porto
            // 
            this.txt_porto.Location = new System.Drawing.Point(259, 14);
            this.txt_porto.Name = "txt_porto";
            this.txt_porto.Size = new System.Drawing.Size(67, 20);
            this.txt_porto.TabIndex = 9;
            this.txt_porto.Text = "3000";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(342, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Nickname:";
            // 
            // txt_nick
            // 
            this.txt_nick.Location = new System.Drawing.Point(406, 15);
            this.txt_nick.Name = "txt_nick";
            this.txt_nick.Size = new System.Drawing.Size(146, 20);
            this.txt_nick.TabIndex = 0;
            // 
            // pedir_lista
            // 
            this.pedir_lista.Enabled = true;
            this.pedir_lista.Interval = 120000;
            this.pedir_lista.Tick += new System.EventHandler(this.pedir_lista_Tick);
            // 
            // Cliente_inicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 463);
            this.Controls.Add(this.txt_nick);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_porto);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_servidor);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_clientes);
            this.Controls.Add(this.btn_comando);
            this.Controls.Add(this.txt_comando);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btn_ligar);
            this.MaximizeBox = false;
            this.Name = "Cliente_inicio";
            this.Text = "GuerrillaIRC";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_ligar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox txt_status;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_comando;
        private System.Windows.Forms.Button btn_comando;
        private System.Windows.Forms.ListBox txt_clientes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_servidor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_porto;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_nick;
        private Timer pedir_lista;

    }
    
}

