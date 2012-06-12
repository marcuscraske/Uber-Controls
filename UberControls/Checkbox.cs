using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace UberLib.Controls
{
    public partial class Checkbox : UserControl
    {
        #region "Variables"
        private bool value = false;
        private Image imageChecked = null;
        private Image imageUnchecked = null;
        private string text = "Undefined";
        private Font textFont = new Font("Arial", 10.0F, FontStyle.Regular);
        private Color textColour = Color.White;
        private Color backgroundUnchecked = Color.FromArgb(51, 51, 51); // #333333
        private Color backgroundChecked = Color.FromArgb(153, 204, 51); // #99CC33
        #endregion

        #region "Variables - Cache"
        private Point cachePosIconChecked;
        private Point cachePosIconUnchecked;
        private Point cachePosText;
        private SolidBrush cacheBrush;
        private SolidBrush cacheBackgroundChecked;
        private SolidBrush cacheBackgroundUnchecked;
        #endregion

        #region "Methods - Constructors"
        public Checkbox()
        {
            InitializeComponent();
            Cursor = Cursors.Hand;
            rebuildCache_TextColour();
            rebuildCache_Positions();
            rebuildCache_BackgroundColour();
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        #endregion

        #region "Methods - Properties"
        public bool Checked
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                Invalidate();
            }
        }
        public Color BackgroundChecked
        {
            get
            {
                return backgroundChecked;
            }
            set
            {
                backgroundChecked = value;
                rebuildCache_BackgroundColour();
                Invalidate();
            }
        }
        public Color BackgroundUnchecked
        {
            get
            {
                return backgroundUnchecked;
            }
            set
            {
                value = backgroundUnchecked;
                rebuildCache_BackgroundColour();
                Invalidate();
            }
        }
        public Image ImageChecked
        {
            get
            {
                return imageChecked;
            }
            set
            {
                imageChecked = value;
                rebuildCache_Positions();
                Invalidate();
            }
        }
        public Image ImageUnchecked
        {
            get
            {
                return imageUnchecked;
            }
            set
            {
                imageUnchecked = value;
                rebuildCache_Positions();
                Invalidate();
            }
        }
        public string HeaderText
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                rebuildCache_Positions();
                Invalidate();
            }
        }
        public Font HeaderTextFont
        {
            get
            {
                return textFont;
            }
            set
            {
                textFont = value;
                Invalidate();
            }
        }
        public Color HeaderTextColour
        {
            get
            {
                return textColour;
            }
            set
            {
                textColour = value;
                rebuildCache_TextColour();
                Invalidate();
            }
        }
        #endregion

        #region "Methods - Events"
        private void Checkbox_Paint(object sender, PaintEventArgs e)
        {
            // Draw background
            e.Graphics.FillRectangle(value ? cacheBackgroundChecked : cacheBackgroundUnchecked, 0, 0, Width, Height);
            // Draw icon
            if (value)
            {
                if (imageChecked != null) e.Graphics.DrawImage(imageChecked, cachePosIconChecked);
            }
            else
            {
                if (imageUnchecked != null) e.Graphics.DrawImage(imageUnchecked, cachePosIconUnchecked);
            }
            // Draw text
            e.Graphics.DrawString(text, textFont, cacheBrush, cachePosText);
        }
        private void Checkbox_MouseDown(object sender, MouseEventArgs e)
        {
            value = !value; // Invert the checked value
            Invalidate();
        }
        private void Checkbox_Resize(object sender, EventArgs e)
        {
            rebuildCache_Positions();
            rebuildCache_BackgroundColour();
            Invalidate();
        }
        #endregion

        #region "Methods - Cache"
        public void rebuildCache_TextColour()
        {
            cacheBrush = new SolidBrush(textColour);
        }
        public void rebuildCache_Positions()
        {
            // Icons
            if (imageChecked != null) cachePosIconChecked = new Point(5, (Height / 2) - (imageChecked.Height / 2));
            if (imageUnchecked != null) cachePosIconUnchecked = new Point(5, (Height / 2) - (imageUnchecked.Height / 2));
            // Text
            SizeF textSize = CreateGraphics().MeasureString(text, textFont);
            cachePosText = new Point(10 + (imageChecked != null && imageUnchecked != null ? imageChecked.Width > imageUnchecked.Width ? imageChecked.Width : imageUnchecked.Width : 0), (Height / 2) - ((int)textSize.Height / 2));
        }
        public void rebuildCache_BackgroundColour()
        {
            cacheBackgroundChecked = new SolidBrush(backgroundChecked);
            cacheBackgroundUnchecked = new SolidBrush(backgroundUnchecked);
        }
        #endregion
    }
}
