﻿namespace Particles
{
    partial class ParticleForm
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
            // ParticleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 861);
            this.Controls.Add(this.portraitControl);
            this.KeyPreview = true;
            this.Name = "ParticleForm";
            this.Text = "ParticleForm";
            this.Load += new System.EventHandler(this.ParticleForm_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ParticleForm_KeyPress);
            this.ResumeLayout(false);

        }

        #endregion

        private OpenTK.GLControl portraitControl;
    }
}

