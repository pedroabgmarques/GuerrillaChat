using System.Windows.Forms;
using System;
namespace TCPServer
{
    partial class TCPServer
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
            this.components = new System.ComponentModel.Container();
            this.btn_iniciar_programa = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txt_status = new System.Windows.Forms.RichTextBox();
            this.Verificar_ligacoes = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.txt_porto = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_n_clientes = new System.Windows.Forms.Label();
            this.n_clientes = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_iniciar_programa
            // 
            this.btn_iniciar_programa.Location = new System.Drawing.Point(13, 12);
            this.btn_iniciar_programa.Name = "btn_iniciar_programa";
            this.btn_iniciar_programa.Size = new System.Drawing.Size(130, 23);
            this.btn_iniciar_programa.TabIndex = 0;
            this.btn_iniciar_programa.Text = "Iniciar Servidor";
            this.btn_iniciar_programa.UseVisualStyleBackColor = true;
            this.btn_iniciar_programa.Click += new System.EventHandler(this.btn_iniciar_programa_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txt_status);
            this.groupBox1.Location = new System.Drawing.Point(12, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(270, 218);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Status";
            // 
            // txt_status
            // 
            this.txt_status.Location = new System.Drawing.Point(6, 19);
            this.txt_status.Name = "txt_status";
            this.txt_status.ReadOnly = true;
            this.txt_status.Size = new System.Drawing.Size(258, 193);
            this.txt_status.TabIndex = 0;
            this.txt_status.Text = "";
            // 
            // Verificar_ligacoes
            // 
            this.Verificar_ligacoes.Enabled = true;
            this.Verificar_ligacoes.Interval = 5000;
            this.Verificar_ligacoes.Tick += new System.EventHandler(this.Verificar_ligacoes_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(159, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Porto:";
            // 
            // txt_porto
            // 
            this.txt_porto.Location = new System.Drawing.Point(200, 14);
            this.txt_porto.Name = "txt_porto";
            this.txt_porto.Size = new System.Drawing.Size(82, 20);
            this.txt_porto.TabIndex = 3;
            this.txt_porto.Text = "3000";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 271);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Clientes:";
            // 
            // lbl_n_clientes
            // 
            this.lbl_n_clientes.AutoSize = true;
            this.lbl_n_clientes.Location = new System.Drawing.Point(68, 271);
            this.lbl_n_clientes.Name = "lbl_n_clientes";
            this.lbl_n_clientes.Size = new System.Drawing.Size(0, 13);
            this.lbl_n_clientes.TabIndex = 5;
            // 
            // n_clientes
            // 
            this.n_clientes.Enabled = true;
            this.n_clientes.Interval = 500;
            this.n_clientes.Tick += new System.EventHandler(this.n_clientes_Tick);
            // 
            // TCPServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 302);
            this.Controls.Add(this.lbl_n_clientes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_porto);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btn_iniciar_programa);
            this.Name = "TCPServer";
            this.Text = "GuerrilhaIRC - Servidor";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_iniciar_programa;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox txt_status;
        private System.Windows.Forms.Timer Verificar_ligacoes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_porto;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_n_clientes;
        private System.Windows.Forms.Timer n_clientes;



    }
}

