namespace HexagonStealthGame {
    partial class StealthGame {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose ();
            }
            base.Dispose (disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent () {
            this.btn_draw = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.btn_load = new System.Windows.Forms.Button();
            this.btn_clear = new System.Windows.Forms.Button();
            this.chk_edit = new System.Windows.Forms.CheckBox();
            this.txt_file = new System.Windows.Forms.TextBox();
            this.btn_tick = new System.Windows.Forms.Button();
            this.chk_vision = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btn_draw
            // 
            this.btn_draw.Location = new System.Drawing.Point(710, 741);
            this.btn_draw.Name = "btn_draw";
            this.btn_draw.Size = new System.Drawing.Size(72, 23);
            this.btn_draw.TabIndex = 0;
            this.btn_draw.Text = "Draw";
            this.btn_draw.UseVisualStyleBackColor = true;
            this.btn_draw.Click += new System.EventHandler(this.Btn_draw_Click);
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(710, 554);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(72, 23);
            this.btn_save.TabIndex = 1;
            this.btn_save.Text = "Save";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.Btn_save_Click);
            // 
            // btn_load
            // 
            this.btn_load.Location = new System.Drawing.Point(710, 525);
            this.btn_load.Name = "btn_load";
            this.btn_load.Size = new System.Drawing.Size(72, 23);
            this.btn_load.TabIndex = 2;
            this.btn_load.Text = "Load";
            this.btn_load.UseVisualStyleBackColor = true;
            this.btn_load.Click += new System.EventHandler(this.Btn_load_Click);
            // 
            // btn_clear
            // 
            this.btn_clear.Location = new System.Drawing.Point(710, 770);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(72, 23);
            this.btn_clear.TabIndex = 3;
            this.btn_clear.Text = "Clear";
            this.btn_clear.UseVisualStyleBackColor = true;
            this.btn_clear.Click += new System.EventHandler(this.Btn_clear_Click);
            // 
            // chk_edit
            // 
            this.chk_edit.AutoSize = true;
            this.chk_edit.Location = new System.Drawing.Point(710, 502);
            this.chk_edit.Name = "chk_edit";
            this.chk_edit.Size = new System.Drawing.Size(68, 17);
            this.chk_edit.TabIndex = 4;
            this.chk_edit.Text = "Edit Map";
            this.chk_edit.UseVisualStyleBackColor = true;
            // 
            // txt_file
            // 
            this.txt_file.Location = new System.Drawing.Point(710, 583);
            this.txt_file.Name = "txt_file";
            this.txt_file.Size = new System.Drawing.Size(72, 20);
            this.txt_file.TabIndex = 5;
            this.txt_file.Text = "default";
            // 
            // btn_tick
            // 
            this.btn_tick.Location = new System.Drawing.Point(710, 12);
            this.btn_tick.Name = "btn_tick";
            this.btn_tick.Size = new System.Drawing.Size(72, 23);
            this.btn_tick.TabIndex = 6;
            this.btn_tick.Text = "Tick";
            this.btn_tick.UseVisualStyleBackColor = true;
            this.btn_tick.Click += new System.EventHandler(this.Btn_tick_Click);
            // 
            // chk_vision
            // 
            this.chk_vision.Checked = true;
            this.chk_vision.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_vision.Location = new System.Drawing.Point(710, 718);
            this.chk_vision.Name = "chk_vision";
            this.chk_vision.Size = new System.Drawing.Size(68, 17);
            this.chk_vision.TabIndex = 7;
            this.chk_vision.Text = "Vision";
            this.chk_vision.UseVisualStyleBackColor = true;
            // 
            // StealthGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(794, 813);
            this.Controls.Add(this.chk_vision);
            this.Controls.Add(this.btn_tick);
            this.Controls.Add(this.txt_file);
            this.Controls.Add(this.chk_edit);
            this.Controls.Add(this.btn_clear);
            this.Controls.Add(this.btn_load);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.btn_draw);
            this.KeyPreview = true;
            this.Name = "StealthGame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StealthGame";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.StealthGame_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.StealthGame_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.StealthGame_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.StealthGame_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_draw;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Button btn_load;
        private System.Windows.Forms.Button btn_clear;
        private System.Windows.Forms.CheckBox chk_edit;
        private System.Windows.Forms.TextBox txt_file;
        private System.Windows.Forms.Button btn_tick;
        private System.Windows.Forms.CheckBox chk_vision;
    }
}

