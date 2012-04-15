namespace UberLib.Controls
{
    partial class ProgressBar
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ProgressBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.Name = "ProgressBar";
            this.Size = new System.Drawing.Size(239, 24);
            this.SizeChanged += new System.EventHandler(this.ProgressBar_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ProgressBar_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ProgressBar_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ProgressBar_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ProgressBar_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
