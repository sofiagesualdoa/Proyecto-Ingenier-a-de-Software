namespace Venta_Productos_Cosméticos
{
    partial class FormErrorIntegridad
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
            btnRecalcular = new Button();
            btnCerrarSesion = new Button();
            label1 = new Label();
            button1 = new Button();
            dgvErrores = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvErrores).BeginInit();
            SuspendLayout();
            // 
            // btnRecalcular
            // 
            btnRecalcular.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRecalcular.ForeColor = Color.Black;
            btnRecalcular.Location = new Point(528, 100);
            btnRecalcular.Name = "btnRecalcular";
            btnRecalcular.Size = new Size(199, 80);
            btnRecalcular.TabIndex = 0;
            btnRecalcular.Tag = "Administrar Error";
            btnRecalcular.Text = "Recalcular Dígitos Verificadores";
            btnRecalcular.UseVisualStyleBackColor = true;
            btnRecalcular.Click += btnRecalcular_Click;
            // 
            // btnCerrarSesion
            // 
            btnCerrarSesion.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCerrarSesion.ForeColor = Color.Black;
            btnCerrarSesion.Location = new Point(528, 335);
            btnCerrarSesion.Name = "btnCerrarSesion";
            btnCerrarSesion.Size = new Size(199, 80);
            btnCerrarSesion.TabIndex = 1;
            btnCerrarSesion.Text = "Cerrar Sesión";
            btnCerrarSesion.UseVisualStyleBackColor = true;
            btnCerrarSesion.Click += btnCerrarSesion_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            label1.Location = new Point(152, 28);
            label1.Name = "label1";
            label1.Size = new Size(463, 32);
            label1.TabIndex = 2;
            label1.Text = "Error de Integridad en la Base de Datos";
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.Black;
            button1.Location = new Point(528, 216);
            button1.Name = "button1";
            button1.Size = new Size(199, 80);
            button1.TabIndex = 3;
            button1.Tag = "Administrar Error";
            button1.Text = "Restaurar con BackUp";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // dgvErrores
            // 
            dgvErrores.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvErrores.Location = new Point(39, 100);
            dgvErrores.Name = "dgvErrores";
            dgvErrores.Size = new Size(471, 315);
            dgvErrores.TabIndex = 4;
            // 
            // FormErrorIntegridad
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.RosyBrown;
            ClientSize = new Size(800, 450);
            Controls.Add(dgvErrores);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(btnCerrarSesion);
            Controls.Add(btnRecalcular);
            ForeColor = Color.Black;
            Name = "FormErrorIntegridad";
            Text = "FormErrorIntegridad";
            Load += FormErrorIntegridad_Load;
            ((System.ComponentModel.ISupportInitialize)dgvErrores).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnRecalcular;
        private Button btnCerrarSesion;
        private Label label1;
        private Button button1;
        private DataGridView dgvErrores;
    }
}