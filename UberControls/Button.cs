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
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace UberLib.Controls
{
    [DefaultEvent("MouseClick")] // Default event for VS designer when le developer double-clicks the control
    public partial class Button : UserControl
    {
        #region "Constructors"
        public Button()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            // Rebuild cache
            cacheRebuild_Background_Normal();
            cacheRebuild_Background_OnClick();
            cacheRebuild_Background_OnHover();
            cacheRebuild_TextPosition();
            cacheRebuild_Text_Normal();
            cacheRebuild_Text_OnClick();
            cacheRebuild_Text_OnHover();
            // Set cursor to hand-pointer
            Cursor = Cursors.Hand;
        }
        #endregion

        #region "Enums"
        /// <summary>
        /// The current mode/state of the button.
        /// </summary>
        enum Mode
        {
            Normal,
            Hover,
            Clicked,
        }
        /// <summary>
        /// The position of the text horizontally.
        /// </summary>
        public enum TextTypeX
        {
            Left,
            Centre,
            Right
        }
        /// <summary>
        /// The position of the text vertically.
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
        /// The current state of the button.
        /// </summary>
        private Mode _Mode = Mode.Normal;
        /// <summary>
        /// The text written on the button.
        /// </summary>
        private string _Text = "Untitled";
        /// <summary>
        /// The font of the text written on the button.
        /// </summary>
        private Font _Style_Text_Font = new Font("Verdana", 12.0f, FontStyle.Bold);
        /// <summary>
        /// The orientation of the text horizontally.
        /// </summary>
        private TextTypeX _Style_Text_Position_X = TextTypeX.Centre;
        /// <summary>
        /// The orientation of the text vertically.
        /// </summary>
        private TextTypeY _Style_Text_Position_Y = TextTypeY.Centre;
        /// <summary>
        /// The padding of the text from the edges of the control.
        /// </summary>
        private float _Style_Text_Padding = 5.0f;
        /// <summary>
        /// Top colour of the background in normal mode.
        /// </summary>
        private Color _Style_Normal_Background_1 = Color.DarkGray;
        /// <summary>
        /// Bottom colour of the background in normal mode.
        /// </summary>
        private Color _Style_Normal_Background_2 = Color.SlateGray;
        /// <summary>
        /// The orientation of the background in normal mode.
        /// </summary>
        private LinearGradientMode _Style_Normal_Background = LinearGradientMode.Vertical;
        /// <summary>
        /// The background image drawn in normal mode.
        /// </summary>
        private Image _Style_Normal_Background_Image = null;
        /// <summary>
        /// The colour of the text in normal mode.
        /// </summary>
        private Color _Style_Normal_Text_Colour = Color.White;
        /// <summary>
        /// The top colour of the background in on-hover mode.
        /// </summary>
        private Color _Style_OnHover_Background_1 = Color.LightGray;
        /// <summary>
        /// The bottom colour of the background in on-hover mode.
        /// </summary>
        private Color _Style_OnHover_Background_2 = Color.White;
        /// <summary>
        /// The orientation of the background in on-hover mode.
        /// </summary>
        private LinearGradientMode _Style_OnHover_Background = LinearGradientMode.Vertical;
        /// <summary>
        /// The background image drawn in on-hover mode.
        /// </summary>
        private Image _Style_OnHover_Background_Image = null;
        /// <summary>
        /// The colour of the text in on-hover text.
        /// </summary>
        private Color _Style_OnHover_Text_Colour = Color.White;
        /// <summary>
        /// The top colour of the background in on-click mode.
        /// </summary>
        private Color _Style_OnClick_Background_1 = Color.LawnGreen;
        /// <summary>
        /// The bottom colour of the background in on-click mode.
        /// </summary>
        private Color _Style_OnClick_Background_2 = Color.LimeGreen;
        /// <summary>
        /// The orientation of the background in on-click mode.
        /// </summary>
        private LinearGradientMode _Style_OnClick_Background = LinearGradientMode.Vertical;
        /// <summary>
        /// The background image drawn in the on-click mode.
        /// </summary>
        private Image _Style_OnClick_Background_Image = null;
        /// <summary>
        /// The colour of the text in on-click mode.
        /// </summary>
        private Color _Style_OnClick_Text_Colour = Color.White;
        /// <summary>
        /// The size of the border.
        /// </summary>
        private float _Style_Border_Width = 2.0f;
        /// <summary>
        /// The colour of the border.
        /// </summary>
        private Color _Style_Border_Colour = Color.Black;

        private Brush _cacheStyleBackgroundNormal;
        private Brush _cacheStyleBackgroundOnHover;
        private Brush _cacheStyleBackgroundOnClick;
        private float _cacheTextX;
        private float _cacheTextY;
        private SolidBrush _cacheTextNormal;
        private SolidBrush _cacheTextOnHover;
        private SolidBrush _cacheTextOnClick;
        #endregion

        #region "Methods - Properties"
        /// <summary>
        /// The text written on the button.
        /// </summary>
        public string ButtonText
        {
            get
            {
                return _Text;
            }
            set
            {
                _Text = value;
                cacheRebuild_TextPosition();
                Invalidate();
            }
        }
        /// <summary>
        /// The font of the text written on the button.
        /// </summary>
        public Font Style_Text_Font
        {
            get
            {
                return _Style_Text_Font;
            }
            set
            {
                _Style_Text_Font = value;
                cacheRebuild_TextPosition();
                Invalidate();
            }
        }
        /// <summary>
        /// The orientation of the text horizontally.
        /// </summary>
        public TextTypeX Style_Text_Position_Horizontal
        {
            get
            {
                return _Style_Text_Position_X;
            }
            set
            {
                _Style_Text_Position_X = value;
                cacheRebuild_TextPosition();
                Invalidate();
            }
        }
        /// <summary>
        /// The orientation of the text vertically.
        /// </summary>
        public TextTypeY Style_Text_Position_Vertical
        {
            get
            {
                return _Style_Text_Position_Y;
            }
            set
            {
                _Style_Text_Position_Y = value;
                cacheRebuild_TextPosition();
                Invalidate();
            }
        }
        /// <summary>
        /// Top colour of the background in normal mode.
        /// </summary>
        public Color Style_Normal_Background_1
        {
            get
            {
                return _Style_Normal_Background_1;
            }
            set
            {
                _Style_Normal_Background_1 = value;
                cacheRebuild_Background_Normal();
                if(_Mode == Mode.Normal) Invalidate();
            }
        }
        /// <summary>
        /// Bottom colour of the background in normal mode.
        /// </summary>
        public Color Style_Normal_Background_2
        {
            get
            {
                return _Style_Normal_Background_2;
            }
            set
            {
                _Style_Normal_Background_2 = value;
                cacheRebuild_Background_Normal();
                if(_Mode == Mode.Normal) Invalidate();
            }
        }
        /// <summary>
        /// The background image drawn in normal mode.
        /// </summary>
        public Image Style_Normal_Background_Image
        {
            get
            {
                return _Style_Normal_Background_Image;
            }
            set
            {
                _Style_Normal_Background_Image = value;
                if(_Mode == Mode.Normal) Invalidate();
            }
        }
        /// <summary>
        /// The orientation of the background in normal mode.
        /// </summary>
        public LinearGradientMode Style_Normal_Background
        {
            get
            {
                return _Style_Normal_Background;
            }
            set
            {
                _Style_Normal_Background = value;
                if (_Mode == Mode.Normal) Invalidate();
            }
        }
        /// <summary>
        /// The colour of the text in normal mode.
        /// </summary>
        public Color Style_Normal_Text_Colour
        {
            get
            {
                return _Style_Normal_Text_Colour;
            }
            set
            {
                _Style_Normal_Text_Colour = value;
                cacheRebuild_Text_Normal();
                if(_Mode == Mode.Normal) Invalidate();
            }
        }
        /// <summary>
        /// The top colour of the background in on-hover mode.
        /// </summary>
        public Color Style_OnHover_Background_1
        {
            get
            {
                return _Style_OnHover_Background_1;
            }
            set
            {
                _Style_OnHover_Background_1 = value;
                cacheRebuild_Background_OnHover();
                if(_Mode == Mode.Hover) Invalidate();
            }
        }
        /// <summary>
        /// The bottom colour of the background in on-hover mode.
        /// </summary>
        public Color Style_OnHover_Background_2
        {
            get
            {
                return _Style_OnHover_Background_2;
            }
            set
            {
                _Style_OnHover_Background_2 = value;
                cacheRebuild_Background_OnHover();
                if(_Mode == Mode.Hover) Invalidate();
            }
        }
        /// <summary>
        /// The background image drawn in on-hover mode.
        /// </summary>
        public Image Style_OnHover_Background_Image
        {
            get
            {
                return _Style_OnHover_Background_Image;
            }
            set
            {
                _Style_OnHover_Background_Image = value;
                if(_Mode == Mode.Hover) Invalidate();
            }
        }
        /// <summary>
        /// The colour of the text in on-hover text.
        /// </summary>
        public Color Style_OnHover_Text_Colour
        {
            get
            {
                return _Style_OnHover_Text_Colour;
            }
            set
            {
                _Style_OnHover_Text_Colour = value;
                cacheRebuild_Text_OnHover();
                if(_Mode == Mode.Hover) Invalidate();
            }
        }
        /// <summary>
        /// The orientation of the background in on-hover mode.
        /// </summary>
        public LinearGradientMode Style_OnHover_Background
        {
            get
            {
                return _Style_OnHover_Background;
            }
            set
            {
                _Style_OnHover_Background = value;
                if (_Mode == Mode.Hover) Invalidate();
            }
        }
        /// <summary>
        /// The top colour of the background in on-click mode.
        /// </summary>
        /// 
        public Color Style_OnClick_Background_1
        {
            get
            {
                return _Style_OnClick_Background_1;
            }
            set
            {
                _Style_OnClick_Background_1 = value;
                cacheRebuild_Background_OnClick();
                if(_Mode == Mode.Clicked) Invalidate();
            }
        }
        /// <summary>
        /// The bottom colour of the background in on-click mode.
        /// </summary>
        public Color Style_OnClick_Background_2
        {
            get
            {
                return _Style_OnClick_Background_2;
            }
            set
            {
                _Style_OnClick_Background_2 = value;
                cacheRebuild_Background_OnClick();
                if(_Mode == Mode.Clicked) Invalidate();
            }
        }
        /// <summary>
        /// The orientation of the background in on-click mode.
        /// </summary>
        public Image Style_OnClick_Background_Image
        {
            get
            {
                return _Style_OnClick_Background_Image;
            }
            set
            {
                _Style_OnClick_Background_Image = value;
                if(_Mode == Mode.Clicked) Invalidate();
            }
        }
        /// <summary>
        /// The colour of the text in on-click mode.
        /// </summary>
        public Color Style_OnClick_Text_Colour
        {
            get
            {
                return _Style_OnClick_Text_Colour;
            }
            set
            {
                _Style_OnClick_Text_Colour = value;
                cacheRebuild_Text_OnClick();
                if(_Mode == Mode.Clicked) Invalidate();
            }
        }
        /// <summary>
        /// The orientation of the background in on-click mode.
        /// </summary>
        public LinearGradientMode Style_OnClick_Background
        {
            get
            {
                return _Style_OnClick_Background;
            }
            set
            {
                _Style_OnClick_Background = value;
                if (_Mode == Mode.Clicked) Invalidate();
            }
        }
        /// <summary>
        /// The size of the border.
        /// </summary>
        public float Style_Border_Size
        {
            get
            {
                return _Style_Border_Width;
            }
            set
            {
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
        #endregion

        #region "Methods - Events"
        private void Button_Paint(object sender, PaintEventArgs e)
        {
            // Draw background & text
            switch(_Mode)
            {
                case Mode.Normal:
                    e.Graphics.FillRectangle(_cacheStyleBackgroundNormal, 0, 0, Width, Height);
                    e.Graphics.DrawString(_Text, _Style_Text_Font, _cacheTextNormal, _cacheTextX, _cacheTextY);
                    break;
                case Mode.Hover:
                    e.Graphics.FillRectangle(_cacheStyleBackgroundOnHover, 0, 0, Width, Height);
                    e.Graphics.DrawString(_Text, _Style_Text_Font, _cacheTextOnHover, _cacheTextX, _cacheTextY);
                    break;
                case Mode.Clicked:
                    e.Graphics.FillRectangle(_cacheStyleBackgroundOnClick, 0, 0, Width, Height);
                    e.Graphics.DrawString(_Text, _Style_Text_Font, _cacheTextOnClick, _cacheTextX, _cacheTextY);
                    break;
            }
            // Draw border
            int t = (int)Math.Floor((double)_Style_Border_Width / 2);
            if (_Style_Border_Width > 0) e.Graphics.DrawRectangle(new Pen(_Style_Border_Colour, _Style_Border_Width), t, t, Width - _Style_Border_Width, Height - _Style_Border_Width);
        }
        bool _hovering = false;
        /// <summary>
        /// Invoked when the users mouse enters the control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_MouseEnter(object sender, EventArgs e)
        {
            _hovering = true;
            if(_Mode != Mode.Clicked) _Mode = Mode.Hover;
            Invalidate();
        }
        /// <summary>
        /// Invoked when the users mouse leaves the control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_MouseLeave(object sender, EventArgs e)
        {
            _hovering = false;
            if (_Mode != Mode.Clicked)
            {
                _Mode = Mode.Normal;
                Invalidate();
            }
        }
        /// <summary>
        /// Invoked when the users mouse clicks down on the control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_MouseDown(object sender, MouseEventArgs e)
        {
            _Mode = Mode.Clicked;
            Invalidate();
        }
        /// <summary>
        /// Invoked when the users mouse clicks up on the control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            if (_hovering) _Mode = Mode.Hover;
            else _Mode = Mode.Normal;
            Invalidate();
        }
        /// <summary>
        /// Invoked when the control resizes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Resize(object sender, EventArgs e)
        {
            cacheRebuild_TextPosition();
            cacheRebuild_Background_Normal();
            cacheRebuild_Background_OnClick();
            cacheRebuild_Background_OnHover();
        }
        #endregion

        #region "Methods - Cache Rebuilding"
        private void cacheRebuild_Background_Normal()
        {
            _cacheStyleBackgroundNormal = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), _Style_Normal_Background_1, _Style_Normal_Background_2, _Style_Normal_Background);
        }
        private void cacheRebuild_Background_OnHover()
        {
            _cacheStyleBackgroundOnHover = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), _Style_OnHover_Background_1, _Style_OnHover_Background_2, _Style_OnHover_Background);
        }
        private void cacheRebuild_Background_OnClick()
        {
            _cacheStyleBackgroundOnClick = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), _Style_OnClick_Background_1, _Style_OnClick_Background_2, _Style_OnClick_Background);
        }
        private void cacheRebuild_Text_Normal()
        {
            _cacheTextNormal = new SolidBrush(_Style_Normal_Text_Colour);
        }
        private void cacheRebuild_Text_OnHover()
        {
            _cacheTextOnHover = new SolidBrush(_Style_OnHover_Text_Colour);
        }
        private void cacheRebuild_Text_OnClick()
        {
            _cacheTextOnClick = new SolidBrush(_Style_OnClick_Text_Colour);
        }
        private void cacheRebuild_TextPosition()
        {
            // Get the size of the text
            SizeF textSize = CreateGraphics().MeasureString(_Text, _Style_Text_Font);
            // Build the position for X
            switch (_Style_Text_Position_X)
            {
                case TextTypeX.Left:
                    _cacheTextX = _Style_Text_Padding;
                    break;
                case TextTypeX.Centre:
                    _cacheTextX = ((float)Width / 2.0F) - (textSize.Width / 2.0F);
                    break;
                case TextTypeX.Right:
                    _cacheTextX = (float)Width - _Style_Text_Padding;
                    break;
            }
            // Build the position for Y
            switch (_Style_Text_Position_Y)
            {
                case TextTypeY.Top:
                    _cacheTextY = _Style_Text_Padding;
                    break;
                case TextTypeY.Centre:
                    _cacheTextY = ((float)Height / 2.0F) - (textSize.Height / 2.0F);
                    break;
                case TextTypeY.Bottom:
                    _cacheTextY = Height - _Style_Text_Padding;
                    break;
            }
        }
        #endregion
    }
}
