namespace UberLib.Controls
{
    partial class Checkbox
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
            // Checkbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Name = "Checkbox";
            this.Size = new System.Drawing.Size(197, 34);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Checkbox_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Checkbox_MouseDown);
            this.Resize += new System.EventHandler(this.Checkbox_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
