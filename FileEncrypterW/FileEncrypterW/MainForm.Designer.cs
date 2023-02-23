namespace FileEncrypterW
{
    partial class MainForm
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
            this.title_lbl = new System.Windows.Forms.Label();
            this.subtitle1_lbl = new System.Windows.Forms.Label();
            this.author_lbl = new System.Windows.Forms.Label();
            this.path_lbl = new System.Windows.Forms.Label();
            this.path_txtbox = new System.Windows.Forms.TextBox();
            this.password_txtbox = new System.Windows.Forms.TextBox();
            this.password_lbl = new System.Windows.Forms.Label();
            this.log_txtbox = new System.Windows.Forms.TextBox();
            this.encrypt_btn = new System.Windows.Forms.Button();
            this.decrypt_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // title_lbl
            // 
            this.title_lbl.AutoSize = true;
            this.title_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title_lbl.Location = new System.Drawing.Point(12, 9);
            this.title_lbl.Name = "title_lbl";
            this.title_lbl.Size = new System.Drawing.Size(213, 37);
            this.title_lbl.TabIndex = 0;
            this.title_lbl.Text = "File Encrypter";
            // 
            // subtitle1_lbl
            // 
            this.subtitle1_lbl.AutoSize = true;
            this.subtitle1_lbl.Location = new System.Drawing.Point(19, 50);
            this.subtitle1_lbl.Name = "subtitle1_lbl";
            this.subtitle1_lbl.Size = new System.Drawing.Size(88, 13);
            this.subtitle1_lbl.TabIndex = 1;
            this.subtitle1_lbl.Text = "WinForms edition";
            // 
            // author_lbl
            // 
            this.author_lbl.AutoSize = true;
            this.author_lbl.Location = new System.Drawing.Point(174, 50);
            this.author_lbl.Name = "author_lbl";
            this.author_lbl.Size = new System.Drawing.Size(51, 13);
            this.author_lbl.TabIndex = 2;
            this.author_lbl.Text = "by Unyxe";
            // 
            // path_lbl
            // 
            this.path_lbl.AutoSize = true;
            this.path_lbl.Location = new System.Drawing.Point(12, 96);
            this.path_lbl.Name = "path_lbl";
            this.path_lbl.Size = new System.Drawing.Size(66, 13);
            this.path_lbl.TabIndex = 3;
            this.path_lbl.Text = "Folder path: ";
            // 
            // path_txtbox
            // 
            this.path_txtbox.Location = new System.Drawing.Point(74, 93);
            this.path_txtbox.Name = "path_txtbox";
            this.path_txtbox.Size = new System.Drawing.Size(151, 20);
            this.path_txtbox.TabIndex = 4;
            // 
            // password_txtbox
            // 
            this.password_txtbox.Location = new System.Drawing.Point(74, 122);
            this.password_txtbox.MaxLength = 16;
            this.password_txtbox.Name = "password_txtbox";
            this.password_txtbox.PasswordChar = '*';
            this.password_txtbox.Size = new System.Drawing.Size(151, 20);
            this.password_txtbox.TabIndex = 6;
            // 
            // password_lbl
            // 
            this.password_lbl.AutoSize = true;
            this.password_lbl.Location = new System.Drawing.Point(12, 125);
            this.password_lbl.Name = "password_lbl";
            this.password_lbl.Size = new System.Drawing.Size(59, 13);
            this.password_lbl.TabIndex = 5;
            this.password_lbl.Text = "Password: ";
            // 
            // log_txtbox
            // 
            this.log_txtbox.Location = new System.Drawing.Point(231, 12);
            this.log_txtbox.Multiline = true;
            this.log_txtbox.Name = "log_txtbox";
            this.log_txtbox.Size = new System.Drawing.Size(273, 162);
            this.log_txtbox.TabIndex = 7;
            // 
            // encrypt_btn
            // 
            this.encrypt_btn.Location = new System.Drawing.Point(13, 150);
            this.encrypt_btn.Name = "encrypt_btn";
            this.encrypt_btn.Size = new System.Drawing.Size(105, 23);
            this.encrypt_btn.TabIndex = 8;
            this.encrypt_btn.Text = "Encrypt";
            this.encrypt_btn.UseVisualStyleBackColor = true;
            this.encrypt_btn.Click += new System.EventHandler(this.encrypt_btn_Click);
            // 
            // decrypt_btn
            // 
            this.decrypt_btn.Location = new System.Drawing.Point(124, 150);
            this.decrypt_btn.Name = "decrypt_btn";
            this.decrypt_btn.Size = new System.Drawing.Size(101, 23);
            this.decrypt_btn.TabIndex = 9;
            this.decrypt_btn.Text = "Decrypt";
            this.decrypt_btn.UseVisualStyleBackColor = true;
            this.decrypt_btn.Click += new System.EventHandler(this.decrypt_btn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 186);
            this.Controls.Add(this.decrypt_btn);
            this.Controls.Add(this.encrypt_btn);
            this.Controls.Add(this.log_txtbox);
            this.Controls.Add(this.password_txtbox);
            this.Controls.Add(this.password_lbl);
            this.Controls.Add(this.path_txtbox);
            this.Controls.Add(this.path_lbl);
            this.Controls.Add(this.author_lbl);
            this.Controls.Add(this.subtitle1_lbl);
            this.Controls.Add(this.title_lbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.Text = "File Encrypter by Unyxe";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label title_lbl;
        private System.Windows.Forms.Label subtitle1_lbl;
        private System.Windows.Forms.Label author_lbl;
        private System.Windows.Forms.Label path_lbl;
        private System.Windows.Forms.TextBox path_txtbox;
        private System.Windows.Forms.TextBox password_txtbox;
        private System.Windows.Forms.Label password_lbl;
        private System.Windows.Forms.TextBox log_txtbox;
        private System.Windows.Forms.Button encrypt_btn;
        private System.Windows.Forms.Button decrypt_btn;
    }
}

