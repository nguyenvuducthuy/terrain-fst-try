﻿namespace Maze3D
{
    partial class MazeForm
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
            this.portraitControl = new OpenTK.GLControl();
            this.SuspendLayout();
            // 
            // portraitControl
            // 
            this.portraitControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.portraitControl.BackColor = System.Drawing.Color.Black;
            this.portraitControl.Location = new System.Drawing.Point(13, 13);
            this.portraitControl.Name = "portraitControl";
            this.portraitControl.Size = new System.Drawing.Size(859, 836);
            this.portraitControl.TabIndex = 0;
            this.portraitControl.VSync = true;
            // 
            // MazeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1059, 936);
            this.Controls.Add(this.portraitControl);
            this.DoubleBuffered = true;
            this.Name = "MazeForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.MazeForm_Shown);
            this.ResumeLayout(false);

        }

        #endregion


        private OpenTK.GLControl portraitControl;
    }
}

