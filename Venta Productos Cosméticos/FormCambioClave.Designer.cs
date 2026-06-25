namespace Venta_Productos_Cosméticos
{
    partial class FormCambioClave
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCambioClave));
            pictureBox1 = new PictureBox();
            label1 = new Label();
            btnConfirmar = new Button();
            txtClaveNueva = new TextBox();
            txtClaveActual = new TextBox();
            label2 = new Label();
            label3 = new Label();
            txtConfirmacion = new TextBox();
            label4 = new Label();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Gainsboro;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(57, 125);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(328, 153);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            label1.Location = new Point(510, 45);
            label1.Name = "label1";
            label1.Size = new Size(204, 32);
            label1.TabIndex = 4;
            label1.Text = "Cambio de Clave";
            // 
            // btnConfirmar
            // 
            btnConfirmar.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnConfirmar.Location = new Point(510, 323);
            btnConfirmar.Name = "btnConfirmar";
            btnConfirmar.Size = new Size(184, 45);
            btnConfirmar.TabIndex = 13;
            btnConfirmar.Text = "Confirmar";
            btnConfirmar.UseVisualStyleBackColor = true;
            btnConfirmar.Click += btnConfirmar_Click;
            // 
            // txtClaveNueva
            // 
            txtClaveNueva.Location = new Point(496, 201);
            txtClaveNueva.Name = "txtClaveNueva";
            txtClaveNueva.Size = new Size(218, 23);
            txtClaveNueva.TabIndex = 12;
            // 
            // txtClaveActual
            // 
            txtClaveActual.Location = new Point(496, 147);
            txtClaveActual.Name = "txtClaveActual";
            txtClaveActual.Size = new Size(218, 23);
            txtClaveActual.TabIndex = 11;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(495, 173);
            label2.Name = "label2";
            label2.Size = new Size(126, 25);
            label2.TabIndex = 10;
            label2.Text = "Nueva Clave:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(496, 119);
            label3.Name = "label3";
            label3.Size = new Size(125, 25);
            label3.TabIndex = 9;
            label3.Text = "Clave Actual:";
            // 
            // txtConfirmacion
            // 
            txtConfirmacion.Location = new Point(496, 255);
            txtConfirmacion.Name = "txtConfirmacion";
            txtConfirmacion.Size = new Size(218, 23);
            txtConfirmacion.TabIndex = 15;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(495, 227);
            label4.Name = "label4";
            label4.Size = new Size(138, 25);
            label4.TabIndex = 14;
            label4.Text = "Confirmación:";
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.Location = new Point(133, 323);
            button1.Name = "button1";
            button1.Size = new Size(184, 45);
            button1.TabIndex = 16;
            button1.Text = "Salir";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // FormCambioClave
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.RosyBrown;
            ClientSize = new Size(800, 450);
            Controls.Add(button1);
            Controls.Add(txtConfirmacion);
            Controls.Add(label4);
            Controls.Add(btnConfirmar);
            Controls.Add(txtClaveNueva);
            Controls.Add(txtClaveActual);
            Controls.Add(label2);
            Controls.Add(label3);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Name = "FormCambioClave";
            Text = "FormCambioClave";
            FormClosing += FormCambioClave_FormClosing;
            FormClosed += FormCambioClave_FormClosed;
            Load += FormCambioClave_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label label1;
        private Button btnConfirmar;
        private TextBox txtClaveNueva;
        private TextBox txtClaveActual;
        private Label label2;
        private Label label3;
        private TextBox txtConfirmacion;
        private Label label4;
        private Button button1;
    }
}