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
    public partial class Scrollbar : UserControl
    {
        #region "Enums"
        public enum ScrollbarMode
        {
            Horizontal,
            Vertical
        }
        #endregion

        #region "Variables"
        private ScrollbarMode mode = ScrollbarMode.Horizontal;
        private Color trackerColour = Color.Black;
        private Image trackerImage = null;
        private float trackerSize = 50.0F;
        private float valueMin = 0.0F;
        private float valueMax = 100.0F;
        #endregion

        #region "Variables - Cache"
        private RectangleF cacheRenderTracker;
        private SolidBrush cacheTrackerBrush;
        private bool cacheMouseDown = false;
        private float cacheValue = 0.5f;
        #endregion

        #region "Methods - Constructors"
        public Scrollbar()
        {
            InitializeComponent();
            rebuildCache_Rendering();
            rebuildCache_TrackerColour();
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        #endregion

        #region "Methods - Properties"
        public Color TrackerColour
        {
            get
            {
                return trackerColour;
            }
            set
            {
                trackerColour = value;
                rebuildCache_TrackerColour();
                Invalidate();
            }
        }
        public Image TrackerImage
        {
            get
            {
                return trackerImage;
            }
            set
            {
                rebuildCache_Rendering();
                Invalidate();
            }
        }
        public float TrackerSize
        {
            get
            {
                return trackerSize;
            }
            set
            {
                rebuildCache_Rendering();
                Invalidate();
            }
        }
        public ScrollbarMode Mode
        {
            get
            {
                return mode;
            }
            set
            {
                rebuildCache_Rendering();
                Invalidate();
            }
        }
        public float Value
        {
            get
            {
                return valueMin + ((valueMax - valueMin) * cacheValue);
            }
            set
            {
                cacheValue = (valueMax - value) / (valueMax - valueMin); // Generate the actual value between 0 to 1 (percentage)
                rebuildCache_Rendering();
                Invalidate();
            }
        }
        public float ValueMax
        {
            get
            {
                return valueMax;
            }
            set
            {
                if(value > valueMin) valueMax = value;
            }
        }
        public float ValueMin
        {
            get
            {
                return valueMin;
            }
            set
            {
                if(value < valueMax) valueMin = value;
            }
        }
        #endregion

        #region "Methods - Events"
        private void Scrollbar_Paint(object sender, PaintEventArgs e)
        {
            if (trackerImage != null)
                e.Graphics.DrawImage(trackerImage, cacheRenderTracker);
            else
                e.Graphics.FillRectangle(cacheTrackerBrush, cacheRenderTracker);
        }
        private void Scrollbar_Resize(object sender, EventArgs e)
        {
            rebuildCache_Rendering();
        }
        private void Scrollbar_MouseDown(object sender, MouseEventArgs e)
        {
            cacheMouseDown = true;
            eventMouseDown(e);
        }
        private void Scrollbar_MouseUp(object sender, MouseEventArgs e)
        {
            cacheMouseDown = false;
            eventMouseDown(e);
        }
        private void Scrollbar_MouseMove(object sender, MouseEventArgs e)
        {
            if (cacheMouseDown)
                eventMouseDown(e);
        }
        void eventMouseDown(MouseEventArgs e)
        {
            cacheValue = (float)(e.X - (trackerSize / 2)) / ((float)Width - (trackerSize));
            if (cacheValue < 0) cacheValue = 0;
            else if (cacheValue > 1) cacheValue = 1;
            Invalidate();
            rebuildCache_Rendering();
        }
        #endregion

        #region "Methods - Cache"
        private void rebuildCache_TrackerColour()
        {
            cacheTrackerBrush = new SolidBrush(trackerColour);
        }
        private void rebuildCache_Rendering()
        {
            RectangleF tracker = new RectangleF();
            switch (mode)
            {
                case ScrollbarMode.Horizontal:
                    tracker.X = (Width * cacheValue) - (trackerSize * cacheValue);
                    tracker.Y = 0;
                    tracker.Width = trackerSize;
                    tracker.Height = Height;
                    break;
                case ScrollbarMode.Vertical:
                    tracker.X = 0;
                    tracker.Y = (Height * cacheValue) - (trackerSize * cacheValue);
                    tracker.Width = Width;
                    tracker.Height = trackerSize;
                    break;
            }
            cacheRenderTracker = tracker;
        }
        #endregion
    }
}