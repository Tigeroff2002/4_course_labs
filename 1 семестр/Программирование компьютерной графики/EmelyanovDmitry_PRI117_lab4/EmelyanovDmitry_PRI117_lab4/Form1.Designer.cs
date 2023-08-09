namespace EmelyanovDmitry_PRI117_lab4
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
            this.AnT = new Tao.Platform.Windows.SimpleOpenGlControl();
            this.Exit = new System.Windows.Forms.Button();
            this.Visualize = new System.Windows.Forms.Button();
            this.EmelyanovPRI117 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AnT
            // 
            this.AnT.AccumBits = ((byte)(0));
            this.AnT.AutoCheckErrors = false;
            this.AnT.AutoFinish = false;
            this.AnT.AutoMakeCurrent = true;
            this.AnT.AutoSwapBuffers = true;
            this.AnT.BackColor = System.Drawing.Color.Black;
            this.AnT.ColorBits = ((byte)(32));
            this.AnT.DepthBits = ((byte)(16));
            this.AnT.Location = new System.Drawing.Point(70, 43);
            this.AnT.Name = "AnT";
            this.AnT.Size = new System.Drawing.Size(670, 316);
            this.AnT.StencilBits = ((byte)(0));
            this.AnT.TabIndex = 0;
            // 
            // Exit
            // 
            this.Exit.Location = new System.Drawing.Point(82, 393);
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(75, 23);
            this.Exit.TabIndex = 1;
            this.Exit.Text = "Exit";
            this.Exit.UseVisualStyleBackColor = true;
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // Visualize
            // 
            this.Visualize.Location = new System.Drawing.Point(307, 393);
            this.Visualize.Name = "Visualize";
            this.Visualize.Size = new System.Drawing.Size(75, 23);
            this.Visualize.TabIndex = 2;
            this.Visualize.Text = "Visualize";
            this.Visualize.UseVisualStyleBackColor = true;
            this.Visualize.Click += new System.EventHandler(this.Visualize_Click);
            // 
            // EmelyanovPRI117
            // 
            this.EmelyanovPRI117.Location = new System.Drawing.Point(495, 392);
            this.EmelyanovPRI117.Name = "EmelyanovPRI117";
            this.EmelyanovPRI117.Size = new System.Drawing.Size(155, 23);
            this.EmelyanovPRI117.TabIndex = 3;
            this.EmelyanovPRI117.Text = "EmelyanovPRI117";
            this.EmelyanovPRI117.UseVisualStyleBackColor = true;
            this.EmelyanovPRI117.Click += new System.EventHandler(this.EmelyanovPRI117_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.EmelyanovPRI117);
            this.Controls.Add(this.Visualize);
            this.Controls.Add(this.Exit);
            this.Controls.Add(this.AnT);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Tao.Platform.Windows.SimpleOpenGlControl AnT;
        private System.Windows.Forms.Button Exit;
        private System.Windows.Forms.Button Visualize;
        private System.Windows.Forms.Button EmelyanovPRI117;
    }
}

