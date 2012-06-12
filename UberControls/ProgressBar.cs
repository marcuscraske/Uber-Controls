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
    public partial class ProgressBar : UserControl
    {
        #region "Enums"
        /// <summary>
        /// The orientation of the filled bar.
        /// </summary>
        public enum OrientationType
        {
            LeftToRight,
            RightToLeft,
            TopToBottom,
            BottomToTop
        }
        /// <summary>
        /// The location of the text horizontally.
        /// </summary>
        public enum TextTypeX
        {
            Left,
            Centre,
            Right
        }
        /// <summary>
        /// The location of the text vertically.
        /// </summary>
        public enum TextTypeY
        {
            Top,
            Centre,
            Bottom
        }
        #endregion

        #region "Variables"
        /// <summary>
        /// The orientation of the filled-bar.
        /// </summary>
        private OrientationType _Orientation = OrientationType.LeftToRight;
        /// <summary>
        /// The width of the border.
        /// </summary>
        private int _Style_Border_Width = 2;
        /// <summary>
        /// The colour of the border.
        /// </summary>
        private Color _Style_Border_Colour = Color.Black;
        /// <summary>
        /// The colour of the filled bar.
        /// </summary>
        private Color _Style_Colour_Filled = Color.White;
        /// <summary>
        /// The colour of the empty area.
        /// </summary>
        private Color _Style_Colour_Empty = Color.DarkGray;
        /// <summary>
        /// The image of the filled area.
        /// </summary>
        private Image _Style_Image_Filled = null;
        /// <summary>
        /// The image of the empty area.
        /// </summary>
        private Image _Style_Image_Empty = null;
        /// <summary>
        /// The value of the progress-bar.
        /// </summary>
        private float _Value = 0.5f; // Between 0.0 to 1.0 (0% - 100%)
        /// <summary>
        /// If true, the user will not be able to drag the filled bar and change the value.
        /// </summary>
        private bool _Readonly = false;
        /// <summary>
        /// The position of the text horizontally.
        /// </summary>
        private TextTypeX _Text_Position_X = TextTypeX.Centre;
        /// <summary>
        /// The position of the text vertically.
        /// </summary>
        private TextTypeY _Text_Position_Y = TextTypeY.Centre;
        /// <summary>
        /// The background of the text.
        /// </summary>
        private Color _Text_Background = Color.FromArgb(90, 0, 0, 0);
        /// <summary>
        /// The colour of the text.
        /// </summary>
        private Color _Text_Colour = Color.White;
        /// <summary>
        /// The font of the text.
        /// </summary>
        private Font _Text_Font = new Font("Verdana", 10.0f, FontStyle.Bold);
        /// <summary>
        /// The padding of the text background.
        /// </summary>
        private int _Text_Padding = 4;
        /// <summary>
        /// The number of decimal points of the value displayed as text.
        /// </summary>
        private int _Text_Decimal_Points = 2;

        #region "Cached values - used to decrease processing"
        private RectangleF cacheRenderFilled;
        private RectangleF cacheRenderEmpty;
        private RectangleF cacheRenderSrcFilled;
        private RectangleF cacheRenderSrcEmpty;
        private SolidBrush cacheRenderFilledColour;
        private SolidBrush cacheRenderEmptyColour;
        private string cacheText;
        private PointF cacheTextPosition;
        private SolidBrush cacheTextColour;
        private RectangleF cacheTextBackground;
        private SolidBrush cacheTextBackgroundColour;
        #endregion

        #endregion

        #region "Constructors"
        public ProgressBar()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            // Build cache values - sequential dependence too!
            cacheRebuild_EmptyColour();
            cacheRebuild_FilledColour();
            cacheRebuild_TextBackgroundColour();
            cacheRebuild_TextColour();
            cacheRebuild_Text();
            cacheRebuild_TextnTextBackgroundPositionSize();
            cacheRebuild_FilledEmptySize();
        }
        #endregion

        #region "Methods - Properties"
        /// <summary>
        /// The orientation of the filled-bar.
        /// </summary>
        public OrientationType Orientation
        {
            get
            {
                return _Orientation;
            }
            set
            {
                _Orientation = value;
                rebuildCursor();
                cacheRebuild_FilledEmptySize();
            }
        }
        /// <summary>
        /// The width of the border.
        /// </summary>
        public int Style_Border_Width
        {
            get
            {
                return _Style_Border_Width;
            }
            set
            {
                if (value < 0) throw new Exception("Value must be equal or greater to zero!");
                _Style_Border_Width = value;
                Invalidate();
            }
        }
        /// <summary>
        /// The colour of the border.
        /// </summary>
        public Color Style_Border_Colour
        {
            get
            {
                return _Style_Border_Colour;
            }
            set
            {
                _Style_Border_Colour = value;
                Invalidate();
            }
        }
        /// <summary>
        /// The colour of the filled bar.
        /// </summary>
        public Color Style_Colour_Filled
        {
            get
            {
                return _Style_Colour_Filled;
            }
            set
            {
                _Style_Colour_Filled = value;
                cacheRebuild_FilledColour();
                Invalidate();
            }
        }
        /// <summary>
        /// The colour of the empty area.
        /// </summary>
        public Color Style_Colour_Empty
        {
            get
            {
                return _Style_Colour_Empty;
            }
            set
            {
                _Style_Colour_Empty = value;
                cacheRebuild_EmptyColour();
                Invalidate();
            }
        }
        /// <summary>
        /// The image of the filled area.
        /// </summary>
        public Image Style_Image_Filled
        {
            get
            {
                return _Style_Image_Filled;
            }
            set
            {
                _Style_Image_Filled = value;
                cacheRebuild_FilledEmptySize();
                Invalidate();
            }
        }
        /// <summary>
        /// The image of the empty area.
        /// </summary>
        public Image Style_Image_Empty
        {
            get
            {
                return _Style_Image_Empty;
            }
            set
            {
                _Style_Image_Empty = value;
                cacheRebuild_FilledEmptySize();
                Invalidate();
            }
        }
        /// <summary>
        /// The value of the progress-bar.
        /// </summary>
        public float Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (value >= 0.0f && value <= 1.0f)
                {
                    _Value = value;
                    cacheRebuild_Text();
                    cacheRebuild_TextnTextBackgroundPositionSize();
                    cacheRebuild_FilledEmptySize();
                    Invalidate();
                }
            }
        }
        /// <summary>
        /// If true, the user will not be able to drag the filled bar and change the value.
        /// </summary>
        public bool ReadOnly
        {
            get
            {
                return _Readonly;
            }
            set
            {
                _Readonly = value;
                rebuildCursor();
            }
        }
        /// <summary>
        /// The position of the text horizontally.
        /// </summary>
        public TextTypeX Text_Position_X
        {
            get
            {
                return _Text_Position_X;
            }
            set
            {
                _Text_Position_X = value;
                cacheRebuild_TextnTextBackgroundPositionSize();
                Invalidate();
            }
        }
        /// <summary>
        /// The position of the text vertically.
        /// </summary>
        public TextTypeY Text_Position_Y
        {
            get
            {
                return _Text_Position_Y;
            }
            set
            {
                _Text_Position_Y = value;
                cacheRebuild_TextnTextBackgroundPositionSize();
                Invalidate();
            }
        }
        /// <summary>
        /// The background of the text.
        /// </summary>
        public Color Text_Background
        {
            get
            {
                return _Text_Background;
            }
            set
            {
                _Text_Background = value;
                cacheRebuild_TextBackgroundColour();
                Invalidate();
            }
        }
        /// <summary>
        /// The colour of the text.
        /// </summary>
        public Color Text_Colour
        {
            get
            {
                return _Text_Colour;
            }
            set
            {
                _Text_Colour = value;
                cacheRebuild_TextColour();
                Invalidate();
            }
        }
        /// <summary>
        /// The font of the text.
        /// </summary>
        public Font Text_Font
        {
            get
            {
                return _Text_Font;
            }
            set
            {
                _Text_Font = value;
                Invalidate();
            }
        }
        /// <summary>
        /// The padding of the text background.
        /// </summary>
        public int Text_Padding
        {
            get
            {
                return _Text_Padding;
            }
            set
            {
                _Text_Padding = value;
                cacheRebuild_TextnTextBackgroundPositionSize();
                Invalidate();
            }
        }
        /// <summary>
        /// The number of decimal points of the value displayed as text.
        /// </summary>
        public int Text_Decimal_Points
        {
            get
            {
                return _Text_Decimal_Points;
            }
            set
            {
                if (_Text_Decimal_Points < 0) throw new Exception("Decimal points cannot be less than zero!");
                _Text_Decimal_Points = value;
                cacheRebuild_Text();
                cacheRebuild_TextnTextBackgroundPositionSize();
            }
        }
        #endregion

        #region "Methods - Events"
        private void ProgressBar_Paint(object sender, PaintEventArgs e)
        {
            // Draw bars
            e.Graphics.FillRectangle(new SolidBrush(_Style_Colour_Filled), cacheRenderFilled);
            e.Graphics.FillRectangle(new SolidBrush(_Style_Colour_Empty), cacheRenderEmpty);
            if (_Style_Image_Filled != null) e.Graphics.DrawImage(_Style_Image_Filled, cacheRenderFilled, cacheRenderSrcFilled, GraphicsUnit.Pixel);
            if (_Style_Image_Empty != null) e.Graphics.DrawImage(_Style_Image_Empty, cacheRenderEmpty, cacheRenderSrcFilled, GraphicsUnit.Pixel);
            // Draw text background
            e.Graphics.FillRectangle(cacheTextBackgroundColour, cacheTextBackground);
            // Draw text
            e.Graphics.DrawString(cacheText, _Text_Font, cacheTextColour, cacheTextPosition);
            // Draw border
            int t = (int)Math.Floor((double)_Style_Border_Width / 2);
            if (_Style_Border_Width != 0) e.Graphics.DrawRectangle(new Pen(_Style_Border_Colour, _Style_Border_Width), t, t, Width - _Style_Border_Width, Height - _Style_Border_Width);
        }
        /// <summary>
        /// Invoked when the control is resized.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgressBar_SizeChanged(object sender, EventArgs e)
        {
            cacheRebuild_FilledEmptySize();
            cacheRebuild_TextnTextBackgroundPositionSize();
            Invalidate();
        }
        /// <summary>
        /// Used to store the state of the mouse.
        /// </summary>
        private bool _MouseDown = false;
        /// <summary>
        /// Invoked when the user presses down with the mouse on the control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgressBar_MouseDown(object sender, MouseEventArgs e)
        {
            _MouseDown = true;
        }
        /// <summary>
        /// Invoked when the user presses up with the mouse on the control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgressBar_MouseUp(object sender, MouseEventArgs e)
        {
            _MouseDown = false;
        }
        /// <summary>
        /// Invoked when the users mouse moves within the control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgressBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_Readonly && _MouseDown)
            {
                // Calculate change in value
                switch (_Orientation)
                {
                    case OrientationType.LeftToRight:
                        _Value = clamp((float)e.X / (float)Width);
                        break;
                    case OrientationType.RightToLeft:
                        _Value = clamp(1.0f - (float)e.X / (float)Width);
                        break;
                    case OrientationType.TopToBottom:
                        _Value = clamp((float)e.Y / (float)Height);
                        break;
                    case OrientationType.BottomToTop:
                        _Value = clamp(1 - (float)e.Y / (float)Height);
                        break;
                }
                cacheRebuild_FilledEmptySize();
                Invalidate();
            }
        }
        #endregion

        #region "Methods - Cache"
        private void cacheRebuild_FilledEmptySize()
        {
            cacheRenderFilled = new RectangleF(); cacheRenderEmpty = new RectangleF();
            switch (_Orientation)
            {
                case OrientationType.LeftToRight:
                    cacheRenderFilled.Width = (int)((float)Width * _Value);
                    cacheRenderFilled.X = 0;
                    cacheRenderFilled.Height = Height;
                    cacheRenderFilled.Y = 0;
                    cacheRenderEmpty.X = Width - (Width - cacheRenderFilled.Width);
                    cacheRenderEmpty.Y = 0;
                    cacheRenderEmpty.Width = Width - cacheRenderFilled.Width;
                    cacheRenderEmpty.Height = Height;
                    if (_Style_Image_Filled != null)
                    {
                        cacheRenderSrcFilled.X = 0;
                        cacheRenderSrcFilled.Y = 0;
                        cacheRenderSrcFilled.Width = _Style_Image_Filled.Width * _Value;
                        cacheRenderSrcFilled.Height = _Style_Image_Filled.Height;
                    }
                    if (_Style_Image_Empty != null)
                    {
                        cacheRenderSrcEmpty.X = _Style_Image_Empty.Width * _Value;
                        cacheRenderSrcEmpty.Y = 0;
                        cacheRenderSrcEmpty.Width = _Style_Image_Empty.Width - cacheRenderSrcEmpty.X;
                        cacheRenderSrcEmpty.Height = _Style_Image_Empty.Height;
                    }
                    break;
                case OrientationType.RightToLeft:
                    cacheRenderFilled.Width = (int)((float)Width * _Value);
                    cacheRenderFilled.X = Width - cacheRenderFilled.Width;
                    cacheRenderFilled.Height = Height;
                    cacheRenderFilled.Y = 0;
                    cacheRenderEmpty.X = 0;
                    cacheRenderEmpty.Y = 0;
                    cacheRenderEmpty.Width = Width - cacheRenderFilled.Width;
                    cacheRenderEmpty.Height = Height;
                    if (_Style_Image_Filled != null)
                    {
                        cacheRenderSrcFilled.X = _Style_Image_Empty.Width * _Value;
                        cacheRenderSrcFilled.Y = 0;
                        cacheRenderSrcFilled.Width = _Style_Image_Filled.Width - cacheRenderSrcEmpty.X;
                        cacheRenderSrcFilled.Height = _Style_Image_Filled.Height;
                    }
                    if (_Style_Image_Empty != null)
                    {
                        cacheRenderSrcEmpty.X = 0;
                        cacheRenderSrcEmpty.Y = 0;
                        cacheRenderSrcEmpty.Width = _Style_Image_Empty.Width * _Value;
                        cacheRenderSrcEmpty.Height = _Style_Image_Empty.Height;
                    }
                    break;
                case OrientationType.TopToBottom:
                    cacheRenderFilled.Height = (int)((float)Height * _Value);
                    cacheRenderFilled.Y = 0;
                    cacheRenderFilled.Width = Width;
                    cacheRenderFilled.X = 0;
                    cacheRenderEmpty.X = 0;
                    cacheRenderEmpty.Y = Height - (Height - cacheRenderFilled.Height);
                    cacheRenderEmpty.Width = Width;
                    cacheRenderEmpty.Height = Height - cacheRenderFilled.Height;
                    if (_Style_Image_Filled != null)
                    {
                        cacheRenderSrcFilled.X = 0;
                        cacheRenderSrcFilled.Y = 0;
                        cacheRenderSrcFilled.Width = _Style_Image_Filled.Width;
                        cacheRenderSrcFilled.Height = _Style_Image_Filled.Height * _Value;
                    }
                    if (_Style_Image_Empty != null)
                    {
                        cacheRenderSrcEmpty.X = 0;
                        cacheRenderSrcEmpty.Y = _Style_Image_Empty.Height * _Value;
                        cacheRenderSrcEmpty.Width = _Style_Image_Empty.Width;
                        cacheRenderSrcEmpty.Height = _Style_Image_Empty.Height - cacheRenderSrcEmpty.Y;
                    }
                    break;
                case OrientationType.BottomToTop:
                    cacheRenderFilled.Height = (int)((float)Height * _Value);
                    cacheRenderFilled.Y = Height - cacheRenderFilled.Height;
                    cacheRenderFilled.Width = Width;
                    cacheRenderFilled.X = 0;
                    cacheRenderEmpty.X = 0;
                    cacheRenderEmpty.Y = 0;
                    cacheRenderEmpty.Width = Width;
                    cacheRenderEmpty.Height = Height - cacheRenderFilled.Height;
                    if (_Style_Image_Filled != null)
                    {
                        cacheRenderSrcFilled.X = 0;
                        cacheRenderSrcFilled.Y = _Style_Image_Filled.Height * (1 - _Value);
                        cacheRenderSrcFilled.Width = _Style_Image_Filled.Width;
                        cacheRenderSrcFilled.Height = _Style_Image_Filled.Height * _Value;
                    }
                    if (_Style_Image_Empty != null)
                    {
                        cacheRenderSrcEmpty.X = 0;
                        cacheRenderSrcEmpty.Y = _Style_Image_Empty.Height * _Value;
                        cacheRenderSrcEmpty.Width = _Style_Image_Empty.Width;
                        cacheRenderSrcEmpty.Height = _Style_Image_Empty.Height - cacheRenderSrcEmpty.Y;
                    }
                    break;
            }
        }
        private void cacheRebuild_FilledColour()
        {
            cacheRenderFilledColour = new SolidBrush(_Style_Colour_Filled);
        }
        private void cacheRebuild_EmptyColour()
        {
            cacheRenderEmptyColour = new SolidBrush(_Style_Colour_Empty);
        }
        private void cacheRebuild_Text()
        {
            cacheText = (_Value * 100).ToString("0.00") + "%";
        }
        private void cacheRebuild_TextnTextBackgroundPositionSize()
        {
            // Measure the size of the text
            SizeF MT = this.CreateGraphics().MeasureString(cacheText, _Text_Font);
            // Generate the position and sizes of the text and text background based on the set orientations
            switch (_Text_Position_X)
            {
                case TextTypeX.Left:
                    cacheTextPosition.X = _Text_Padding;
                    cacheTextBackground.X = 0;
                    break;
                case TextTypeX.Centre:
                    cacheTextPosition.X = (Width / 2) - ((_Text_Padding + (int)MT.Width + _Text_Padding) / 2) + _Text_Padding;
                    cacheTextBackground.X = (Width / 2) - ((_Text_Padding + (int)MT.Width + _Text_Padding) / 2);
                    break;
                case TextTypeX.Right:
                    cacheTextPosition.X = Width - (_Text_Padding + MT.Width);
                    cacheTextBackground.X = Width - (_Text_Padding + (int)MT.Width + _Text_Padding);
                    break;
            }
            switch (_Text_Position_Y)
            {
                case TextTypeY.Top:
                    cacheTextPosition.Y = _Text_Padding;
                    cacheTextBackground.Y = 0;
                    break;
                case TextTypeY.Centre:
                    cacheTextPosition.Y = (Height / 2) - ((_Text_Padding + (int)MT.Height + _Text_Padding) / 2) + _Text_Padding;
                    cacheTextBackground.Y = (Height / 2) - ((_Text_Padding + (int)MT.Height + _Text_Padding) / 2);
                    break;
                case TextTypeY.Bottom:
                    cacheTextPosition.Y = Height - (_Text_Padding + (int)MT.Height);
                    cacheTextBackground.Y = Height - (_Text_Padding + (int)MT.Height + _Text_Padding);
                    break;
            }
            cacheTextBackground.Width = _Text_Padding + (int)MT.Width + _Text_Padding;
            cacheTextBackground.Height = _Text_Padding + (int)MT.Height + _Text_Padding;
        }
        private void cacheRebuild_TextColour()
        {
            cacheTextColour = new SolidBrush(_Text_Colour);
        }
        private void cacheRebuild_TextBackgroundColour()
        {
            cacheTextBackgroundColour = new SolidBrush(_Text_Background);
        }
        #endregion

        #region "Methods"
        /// <summary>
        /// Rebuilds the cursor for the control based on its state.
        /// </summary>
        private void rebuildCursor()
        {
            if (_Readonly) this.Cursor = System.Windows.Forms.Cursors.Default;
            else
            {
                switch (_Orientation)
                {
                    case OrientationType.LeftToRight:
                    case OrientationType.RightToLeft:
                        this.Cursor = System.Windows.Forms.Cursors.VSplit; break;
                    case OrientationType.TopToBottom:
                    case OrientationType.BottomToTop:
                        this.Cursor = System.Windows.Forms.Cursors.HSplit; break;
                }
            }
        }
        /// <summary>
        /// Returns the inputted value fixed inclusive of range 0.0 to 1.0.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private float clamp(float value)
        {
            return value < 0.0f ? 0 : value > 1.0f ? 1.0f : value;
        }
        #endregion
    }
}