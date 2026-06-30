namespace Venta_Productos_Cosméticos
{
    partial class FormPerfil
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
            label1 = new Label();
            treeView1 = new TreeView();
            dgvPerfil = new DataGridView();
            dgvFamilia = new DataGridView();
            dgvPermiso = new DataGridView();
            btnCrearPerfil = new Button();
            btnQuitarPerfil = new Button();
            btnCrearFamilia = new Button();
            btnQuitarFamilia = new Button();
            btnAgregarPermPerfil = new Button();
            btnQuitarPermPerfil = new Button();
            btnAgregarPermFamilia = new Button();
            btnQuitarPermFamilia = new Button();
            button7 = new Button();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            btnAgregarFamPerfil = new Button();
            btnQuitarFamPerfil = new Button();
            label5 = new Label();
            treeViewFamilias = new TreeView();
            ((System.ComponentModel.ISupportInitialize)dgvPerfil).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvFamilia).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvPermiso).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            label1.Location = new Point(31, 20);
            label1.Name = "label1";
            label1.Size = new Size(99, 32);
            label1.TabIndex = 18;
            label1.Text = "Perfiles";
            label1.Click += label1_Click;
            // 
            // treeView1
            // 
            treeView1.Location = new Point(12, 70);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(238, 375);
            treeView1.TabIndex = 19;
            // 
            // dgvPerfil
            // 
            dgvPerfil.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPerfil.Location = new Point(285, 70);
            dgvPerfil.Name = "dgvPerfil";
            dgvPerfil.Size = new Size(280, 150);
            dgvPerfil.TabIndex = 20;
            // 
            // dgvFamilia
            // 
            dgvFamilia.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvFamilia.Location = new Point(285, 295);
            dgvFamilia.Name = "dgvFamilia";
            dgvFamilia.Size = new Size(280, 150);
            dgvFamilia.TabIndex = 21;
            // 
            // dgvPermiso
            // 
            dgvPermiso.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPermiso.Location = new Point(285, 524);
            dgvPermiso.Name = "dgvPermiso";
            dgvPermiso.Size = new Size(280, 147);
            dgvPermiso.TabIndex = 22;
            // 
            // btnCrearPerfil
            // 
            btnCrearPerfil.BackColor = Color.RosyBrown;
            btnCrearPerfil.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCrearPerfil.Location = new Point(584, 70);
            btnCrearPerfil.Name = "btnCrearPerfil";
            btnCrearPerfil.Size = new Size(155, 48);
            btnCrearPerfil.TabIndex = 23;
            btnCrearPerfil.Text = "Crear Perfil";
            btnCrearPerfil.UseVisualStyleBackColor = false;
            btnCrearPerfil.Click += btnCrearPerfil_Click;
            // 
            // btnQuitarPerfil
            // 
            btnQuitarPerfil.BackColor = Color.RosyBrown;
            btnQuitarPerfil.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnQuitarPerfil.Location = new Point(584, 124);
            btnQuitarPerfil.Name = "btnQuitarPerfil";
            btnQuitarPerfil.Size = new Size(155, 96);
            btnQuitarPerfil.TabIndex = 24;
            btnQuitarPerfil.Text = "Quitar Perfil";
            btnQuitarPerfil.UseVisualStyleBackColor = false;
            btnQuitarPerfil.Click += btnQuitarPerfil_Click;
            // 
            // btnCrearFamilia
            // 
            btnCrearFamilia.BackColor = Color.RosyBrown;
            btnCrearFamilia.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCrearFamilia.Location = new Point(584, 295);
            btnCrearFamilia.Name = "btnCrearFamilia";
            btnCrearFamilia.Size = new Size(155, 48);
            btnCrearFamilia.TabIndex = 25;
            btnCrearFamilia.Text = "Crear Familia";
            btnCrearFamilia.UseVisualStyleBackColor = false;
            btnCrearFamilia.Click += btnCrearFamilia_Click;
            // 
            // btnQuitarFamilia
            // 
            btnQuitarFamilia.BackColor = Color.RosyBrown;
            btnQuitarFamilia.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnQuitarFamilia.Location = new Point(584, 349);
            btnQuitarFamilia.Name = "btnQuitarFamilia";
            btnQuitarFamilia.Size = new Size(155, 96);
            btnQuitarFamilia.TabIndex = 26;
            btnQuitarFamilia.Text = "Quitar Familia";
            btnQuitarFamilia.UseVisualStyleBackColor = false;
            btnQuitarFamilia.Click += btnQuitarFamilia_Click;
            // 
            // btnAgregarPermPerfil
            // 
            btnAgregarPermPerfil.BackColor = Color.RosyBrown;
            btnAgregarPermPerfil.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAgregarPermPerfil.Location = new Point(584, 548);
            btnAgregarPermPerfil.Name = "btnAgregarPermPerfil";
            btnAgregarPermPerfil.Size = new Size(155, 102);
            btnAgregarPermPerfil.TabIndex = 27;
            btnAgregarPermPerfil.Text = "Agregar Permiso a Perfil";
            btnAgregarPermPerfil.UseVisualStyleBackColor = false;
            btnAgregarPermPerfil.Click += btnAgregarPermPerfil_Click;
            // 
            // btnQuitarPermPerfil
            // 
            btnQuitarPermPerfil.BackColor = Color.RosyBrown;
            btnQuitarPermPerfil.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnQuitarPermPerfil.Location = new Point(760, 548);
            btnQuitarPermPerfil.Name = "btnQuitarPermPerfil";
            btnQuitarPermPerfil.Size = new Size(155, 102);
            btnQuitarPermPerfil.TabIndex = 28;
            btnQuitarPermPerfil.Text = "Quitar Permiso a Perfil";
            btnQuitarPermPerfil.UseVisualStyleBackColor = false;
            btnQuitarPermPerfil.Click += btnQuitarPermPerfil_Click;
            // 
            // btnAgregarPermFamilia
            // 
            btnAgregarPermFamilia.BackColor = Color.RosyBrown;
            btnAgregarPermFamilia.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAgregarPermFamilia.Location = new Point(760, 309);
            btnAgregarPermFamilia.Name = "btnAgregarPermFamilia";
            btnAgregarPermFamilia.Size = new Size(155, 102);
            btnAgregarPermFamilia.TabIndex = 31;
            btnAgregarPermFamilia.Text = "Agregar Permiso a Familia";
            btnAgregarPermFamilia.UseVisualStyleBackColor = false;
            btnAgregarPermFamilia.Click += btnAgregarPermFamilia_Click;
            // 
            // btnQuitarPermFamilia
            // 
            btnQuitarPermFamilia.BackColor = Color.RosyBrown;
            btnQuitarPermFamilia.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnQuitarPermFamilia.Location = new Point(760, 431);
            btnQuitarPermFamilia.Name = "btnQuitarPermFamilia";
            btnQuitarPermFamilia.Size = new Size(155, 102);
            btnQuitarPermFamilia.TabIndex = 32;
            btnQuitarPermFamilia.Text = "Quitar Permiso a Familia";
            btnQuitarPermFamilia.UseVisualStyleBackColor = false;
            btnQuitarPermFamilia.Click += btnQuitarPermFamilia_Click;
            // 
            // button7
            // 
            button7.BackColor = Color.RosyBrown;
            button7.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button7.Location = new Point(695, 13);
            button7.Name = "button7";
            button7.Size = new Size(113, 39);
            button7.TabIndex = 33;
            button7.Text = "Salir";
            button7.UseVisualStyleBackColor = false;
            button7.Click += button7_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(394, 42);
            label2.Name = "label2";
            label2.Size = new Size(76, 25);
            label2.TabIndex = 34;
            label2.Text = "Perfiles";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(391, 267);
            label3.Name = "label3";
            label3.Size = new Size(81, 25);
            label3.TabIndex = 35;
            label3.Text = "Familias";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(381, 496);
            label4.Name = "label4";
            label4.Size = new Size(91, 25);
            label4.TabIndex = 36;
            label4.Text = "Permisos";
            // 
            // btnAgregarFamPerfil
            // 
            btnAgregarFamPerfil.BackColor = Color.RosyBrown;
            btnAgregarFamPerfil.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAgregarFamPerfil.Location = new Point(760, 70);
            btnAgregarFamPerfil.Name = "btnAgregarFamPerfil";
            btnAgregarFamPerfil.Size = new Size(155, 102);
            btnAgregarFamPerfil.TabIndex = 37;
            btnAgregarFamPerfil.Text = "Agregar Familia a Perfil";
            btnAgregarFamPerfil.UseVisualStyleBackColor = false;
            btnAgregarFamPerfil.Click += btnAgregarFamPerfil_Click_1;
            // 
            // btnQuitarFamPerfil
            // 
            btnQuitarFamPerfil.BackColor = Color.RosyBrown;
            btnQuitarFamPerfil.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnQuitarFamPerfil.Location = new Point(760, 191);
            btnQuitarFamPerfil.Name = "btnQuitarFamPerfil";
            btnQuitarFamPerfil.Size = new Size(155, 102);
            btnQuitarFamPerfil.TabIndex = 38;
            btnQuitarFamPerfil.Text = "Quitar Familia a Perfil";
            btnQuitarFamPerfil.UseVisualStyleBackColor = false;
            btnQuitarFamPerfil.Click += btnQuitarFamPerfil_Click_1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            label5.Location = new Point(25, 461);
            label5.Name = "label5";
            label5.Size = new Size(105, 32);
            label5.TabIndex = 39;
            label5.Text = "Familias";
            // 
            // treeViewFamilias
            // 
            treeViewFamilias.Location = new Point(12, 496);
            treeViewFamilias.Name = "treeViewFamilias";
            treeViewFamilias.Size = new Size(238, 213);
            treeViewFamilias.TabIndex = 40;
            // 
            // FormPerfil
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.MistyRose;
            ClientSize = new Size(927, 749);
            Controls.Add(treeViewFamilias);
            Controls.Add(label5);
            Controls.Add(btnQuitarFamPerfil);
            Controls.Add(btnAgregarFamPerfil);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(button7);
            Controls.Add(btnQuitarPermFamilia);
            Controls.Add(btnAgregarPermFamilia);
            Controls.Add(btnQuitarPermPerfil);
            Controls.Add(btnAgregarPermPerfil);
            Controls.Add(btnQuitarFamilia);
            Controls.Add(btnCrearFamilia);
            Controls.Add(btnQuitarPerfil);
            Controls.Add(btnCrearPerfil);
            Controls.Add(dgvPermiso);
            Controls.Add(dgvFamilia);
            Controls.Add(dgvPerfil);
            Controls.Add(treeView1);
            Controls.Add(label1);
            Name = "FormPerfil";
            Text = "FormPerfil";
            FormClosing += FormPerfil_FormClosing;
            Load += FormPerfil_Load;
            ((System.ComponentModel.ISupportInitialize)dgvPerfil).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvFamilia).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvPermiso).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TreeView treeView1;
        private DataGridView dgvPerfil;
        private DataGridView dgvFamilia;
        private DataGridView dgvPermiso;
        private Button btnCrearPerfil;
        private Button btnQuitarPerfil;
        private Button btnCrearFamilia;
        private Button btnQuitarFamilia;
        private Button btnAgregarPermPerfil;
        private Button btnQuitarPermPerfil;
        private Button btnAgregarPermFamilia;
        private Button btnQuitarPermFamilia;
        private Button button7;
        private Label label2;
        private Label label3;
        private Label label4;
        private Button btnAgregarFamPerfil;
        private Button btnQuitarFamPerfil;
        private Label label5;
        private TreeView treeViewFamilias;
    }
}