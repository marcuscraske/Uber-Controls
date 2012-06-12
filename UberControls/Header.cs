/*
 * Creative Commons Attribution-ShareAlike 3.0 unported
 * ***************************************************************
 * Author:  limpygnome
 * E-mail:  limpygnome@gmail.com
 * Site:    ubermeat.co.uk
 * ***************************************************************
 * Credit to:
 * -- none
 * 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace UberLib.Controls
{
    public partial class Header : UserControl
    {
        #region "Variables"
        private Image icon = null;
        private String text = "Default";
        private SolidBrush brush = new SolidBrush(Color.Black);
        #endregion

        #region "Variables - Cache"
        private Point cachedPositionText = new Point(0, 0);
        private Point cachedPositionImage = new Point(0, 0);
        #endregion

        #region "Methods - Constructors"
        public Header()
        {
            InitializeComponent();
            rebuildCachedPos_Text();
            rebuildCachedPos_Image();
        }
        #endregion

        #region "Methods - Properties"
        public String HeaderText
        {
            get { return text; }
            set { text = value; rebuildCachedPos_Text(); Invalidate(); }
        }
        public Image HeaderIcon
        {
            get { return icon; }
            set { icon = value; rebuildCachedPos_Image(); Invalidate(); }
        }
        public Color HeaderColour
        {
            get { return brush.Color; }
            set { brush = new SolidBrush(value); Invalidate(); }
        }
        #endregion

        #region "Methods - Events"
        private void Header_Paint(object sender, PaintEventArgs e)
        {
            if(icon != null) e.Graphics.DrawImage(icon, cachedPositionImage);
            e.Graphics.DrawString(text, Font, brush, cachedPositionText);
        }
        private void Header_Resize(object sender, EventArgs e)
        {
            rebuildCachedPos_Text();
            rebuildCachedPos_Image();
        }
        #endregion

        #region "Methods - Cache"
        void rebuildCachedPos_Text()
        {
            SizeF size = this.CreateGraphics().MeasureString(text, Font);
            cachedPositionText = new Point(5 + (icon != null ? icon.Width : 0), (int)((Height / 2) - (size.Height / 2)));
        }
        void rebuildCachedPos_Image()
        {
            cachedPositionImage = new Point(5, (Height / 2) - 8);
        }
        #endregion
    }
}