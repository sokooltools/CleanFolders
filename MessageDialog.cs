using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SokoolTools.CleanFolders.Properties;

namespace SokoolTools.CleanFolders
{
    //----------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Displays a message box with the specified text, caption, icon, and an unlimited number of buttons.
    /// </summary>
    //----------------------------------------------------------------------------------------------------------------------------
    public sealed class MessageDialog : IDisposable
    {
        //..................................................................................................................................

        #region Fields

        private const int SPACE_BETWEEN_BUTTONS = 7;
        private const int SPACE_BETWEEN_TEXT_AND_BUTTONS = 15;
        private const int PADDING = 12;
        private const int MIN_TEXT_HEIGHT = 32;
        private const int BUTTON_HEIGHT = 23;
        private const int TEXT_LEFT = PADDING + 50;
        private string _clickedButton;

        private List<Button> _buttonControls;
        private Form _form;
        private Icon _icon;
        private Label _label;

        #endregion

        //..................................................................................................................................

        #region Private Constructor

        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Constructor
        /// </summary>
        //------------------------------------------------------------------------------------------------------------------------
        private MessageDialog()
        {
            InitializeComponent();
        }

        #endregion

        //..................................................................................................................................

        #region Public Properties & Methods

        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the font to use for display.
        /// </summary>
        //------------------------------------------------------------------------------------------------------------------------
        public static Font Font { get; set; } = SystemInformation.MenuFont;

        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets whether the control box will be shown.
        /// </summary>
        //------------------------------------------------------------------------------------------------------------------------
        public static bool ShowControlBox { get; set; } = true;

        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets whether buttons will be fit to their contents or set to <see cref="ButtonWidth"/>. Default is true.
        /// </summary>
        //------------------------------------------------------------------------------------------------------------------------
        public static bool AutoSizeButtons { get; set; } = true;

        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the width of all of the buttons. Only applies if <see cref="AutoSizeButtons"/> is set to true.
        /// <remarks>
        /// Default value is 73.
        /// </remarks>
        /// </summary>
        //------------------------------------------------------------------------------------------------------------------------
        public static int ButtonWidth { get; set; } = 73;

        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Shows the modal dialog.
        /// </summary>
        /// <param name="text">The text to display in the message dialog.</param>
        /// <param name="caption">The text to display in the title bar of the message dialog.</param>
        /// <param name="icon">
        /// One of the message box icon values that specifies which icon to display in the message
        /// dialog.
        /// </param>
        /// <param name="buttons">Parameter array of strings representing the buttons to display in the message dialog.</param>
        /// <returns>
        /// The string representing the button which was clicked or an empty string if the message
        /// dialog is closed by other means.
        /// </returns>
        //------------------------------------------------------------------------------------------------------------------------
        public static int Show(string text, string caption, MessageBoxIcon icon, params string[] buttons)
        {
            //int i;
            //MessageDialog md = null;
            //try
            //{
            //    md = new MessageDialog();
            //    i = md.ShowForm(text, caption, icon, buttons);
            //}
            //finally
            //{
            //    if (md != null) md.Dispose();
            //}
            //return i;

            using (var dlg = new MessageDialog())
            {
                return dlg.ShowForm(text, caption, icon, buttons);
            }
        }

        #endregion

        //..................................................................................................................................

        #region Private Event Handlers

        private void ButtonClick(object sender, EventArgs e)
        {
            _clickedButton = ((Button)sender).Text;
            _form.Close();
        }

        private void PaintForm(object sender, PaintEventArgs e)
        {
            using (Graphics g = e.Graphics)
            {
                g.DrawIcon(_icon, PADDING, PADDING);
            }
        }

        private void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27)
                _form.Close();
        }

        private void Form_Closed(object sender, FormClosedEventArgs e)
        {
            Dispose();
        }

        #endregion

        //..................................................................................................................................

        #region    Helper Methods

        private void InitializeComponent()
        {
            try
            {
                _label = new Label
                {
                    Top = PADDING,
                    Left = TEXT_LEFT,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Height = MIN_TEXT_HEIGHT,
                    Width = (300 - PADDING - TEXT_LEFT)
                };

                _form = new Form
                {
                    MinimizeBox = false,
                    MaximizeBox = false,
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Font = Font,
                    ControlBox = ShowControlBox,
                    ShowInTaskbar = false,
                    KeyPreview = true,
                    //TopMost = true
                };
                _form.Paint += PaintForm;
                _form.KeyDown += KeyDown;
                _form.FormClosed += Form_Closed;
                _form.Controls.Add(_label);
            }
            catch
            {
                Dispose();
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Shows the form.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="buttons">The buttons.</param>
        /// <returns></returns>
        //------------------------------------------------------------------------------------------------------------------------
        private int ShowForm(string text, string caption, MessageBoxIcon icon, params string[] buttons)
        {
            try
            {
                SetIconAndBeep(icon);

                _form.Text = caption;
                _label.Text = text;

                int aggregateButtonWidth = 0;
                SizeF stringSize;
                _buttonControls = new List<Button>();
                var maxDialogWidth = (int)(Screen.PrimaryScreen.WorkingArea.Width * .6);

                Graphics g = _form.CreateGraphics();
                try
                {
                    stringSize = g.MeasureString(text, _form.Font, maxDialogWidth - PADDING - TEXT_LEFT);
                    int maxWidth = 0;
                    foreach (string buttonText in buttons)
                    {
                        var button = new Button
                        {
                            Text = buttonText,
                            FlatStyle = FlatStyle.System,
                            Height = BUTTON_HEIGHT,
                            Width = AutoSizeButtons ? (int)g.MeasureString(buttonText, _form.Font).Width + 20 : ButtonWidth
                        };
                        button.Click += ButtonClick;
                        maxWidth = Math.Max(maxWidth, button.Width);
                        aggregateButtonWidth += button.Width;
                        _buttonControls.Add(button);

                    }
                    if (AutoSizeButtons)
                    {
                        aggregateButtonWidth = 0;
                        foreach (var button in _buttonControls)
                        {
                            button.Width = maxWidth;
                            aggregateButtonWidth += button.Width;
                        }
                    }
                }
                catch
                {
                    Dispose();
                    throw;
                }
                finally
                {
                    g.Dispose();
                }

                SetLabelSize(stringSize);

                int dialogWithStringWidth = (int)stringSize.Width + PADDING + TEXT_LEFT;
                int width = Math.Min(maxDialogWidth, dialogWithStringWidth);

                aggregateButtonWidth += ((_buttonControls.Count - 1) * SPACE_BETWEEN_BUTTONS);

                SetFormClientSize(aggregateButtonWidth, width);

                int buttonLeft = (_form.ClientSize.Width / 2) - (aggregateButtonWidth / 2);
                int buttonTop = PADDING + _label.ClientSize.Height + SPACE_BETWEEN_TEXT_AND_BUTTONS;

                foreach (var button in _buttonControls)
                {
                    button.Left = buttonLeft;
                    button.Top = buttonTop;
                    _form.Controls.Add(button);
                    buttonLeft += button.Width + SPACE_BETWEEN_BUTTONS;
                }

                _form.ShowDialog();

                for (int i = 0; i < buttons.Length; i++)
                {
                    if (_clickedButton == buttons[i])
                        return i;
                }
                return buttons.Length; // ...May want to return -1 instead?
            }
            catch
            {
                Dispose();
                throw;
            }
        }

        private void SetLabelSize(SizeF stringSize)
        {
            int labelWidth = (int)stringSize.Width + 5;
            var labelHeight = (int)Math.Max(stringSize.Height + 5, _label.Height);
            _label.ClientSize = new Size(labelWidth, labelHeight);
        }

        private void SetFormClientSize(int aggregateButtonWidth, int width)
        {
            int formWidth = Math.Max(aggregateButtonWidth + (PADDING * 2), width);
            int formHeight = PADDING + _label.Height + SPACE_BETWEEN_TEXT_AND_BUTTONS + BUTTON_HEIGHT + PADDING;
            _form.ClientSize = new Size(formWidth, formHeight);
        }

        private void SetIconAndBeep(MessageBoxIcon icon)
        {
            switch (icon)
            {
                case MessageBoxIcon.Asterisk:
                    // case MessageBoxIcon.Information:
                    _icon = SystemIcons.Asterisk;
                    NativeMethods.MessageBeep(NativeMethods.MessageBeepType.Asterisk);
                    break;
                case MessageBoxIcon.Exclamation:
                    // case MessageBoxIcon.Warning:
                    _icon = SystemIcons.Exclamation;
                    NativeMethods.MessageBeep(NativeMethods.MessageBeepType.Exclamation);
                    break;
                case MessageBoxIcon.Hand:
                    // case MessageBoxIcon.Error:
                    // case MessageBoxIcon.Stop:
                    _icon = SystemIcons.Error;
                    NativeMethods.MessageBeep(NativeMethods.MessageBeepType.Hand);
                    break;
                case MessageBoxIcon.Question:
                    _icon = SystemIcons.Question;
                    NativeMethods.MessageBeep(NativeMethods.MessageBeepType.Question);
                    break;
                default:
                    throw new ApplicationException("Unknown icon");
            }
        }

        #endregion

        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Displays a message box informing the user that changes were made and asks if the user would like to save changes 
        /// before continuing.<para>
        /// Options are <b>Save</b> (<c>DialogResult.Yes</c>), <b>Discard</b> (<c>DialogResult.No</c>), or <b>Cancel</b> 
        /// (<c>DialogResult.Cancel</c>).</para>
        /// </summary>
        /// <param name="parent">The parent form.</param>
        /// <param name="text">Text to display in the dialog itself.</param>
        /// <param name="caption">Caption to display in the title bar.</param>
        /// <returns>
        /// <c>DialogResult.Yes</c> for <b>Save</b>, <c>DialogResult.No</c> for <b>Discard</b>, <c>DialogResult.Cancel</c> for 
        /// <b>Cancel</b>.
        /// </returns>
        //------------------------------------------------------------------------------------------------------------------------
        public static DialogResult ShowSaveChanges(Form parent, string text, string caption)
        {
            bool isTopMost = parent.TopMost;
            parent.TopMost = false;
            try
            {
                int buttonOrdinal = Show(text,
                                                 caption,
                                                 MessageBoxIcon.Question,
                                                 Resources.MessageBoxButton_Save,
                                                 Resources.MessageBoxButton_DontSave,
                                                 Resources.MessageBoxButton_Cancel);
                switch (buttonOrdinal)
                {
                    case 0: // 'Save' button clicked.
                        return DialogResult.Yes;
                    case 1: // 'Don't Save' button clicked.
                        return DialogResult.No;
                    case 2: // 'Cancel' button clicked.
                    case 3: // 'Close' button clicked.
                        return DialogResult.Cancel;
                }
                return DialogResult.Cancel;
            }
            finally
            {
                parent.TopMost = isTopMost;
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Displays a message box in front of the specified parent and with the specified text, caption, buttons, and icon.
        /// </summary>
        /// <param name="parent">The parent form.</param>
        /// <param name="text">The text to display in the dialog.</param>
        /// <param name="caption">The caption to display in the titlebar.</param>
        /// <param name="buttons">The buttons.</param>
        /// <param name="icon">The icon.</param>
        /// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult"/> values.</returns>
        //------------------------------------------------------------------------------------------------------------------------
        public static DialogResult Show(Form parent, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            bool isTopMost = parent.TopMost;
            parent.TopMost = false;
            try
            {
                switch (buttons)
                {
                    case MessageBoxButtons.OK:
                        {
                            int buttonOrdinal = Show(text, caption, icon, "OK");
                            switch (buttonOrdinal)
                            {
                                case 0:
                                    return DialogResult.OK;
                                case 1:
                                    return DialogResult.Cancel;
                            }
                        }
                        break;
                    case MessageBoxButtons.OKCancel:
                        {
                            int buttonOrdinal = Show(text, caption, icon, "OK", "Cancel");
                            switch (buttonOrdinal)
                            {
                                case 0:
                                    return DialogResult.OK;
                                case 1:
                                case 2:
                                    return DialogResult.Cancel;
                            }
                        }
                        break;
                    case MessageBoxButtons.YesNo:
                        {
                            int buttonOrdinal = Show(text, caption, icon, "Yes", "No");
                            switch (buttonOrdinal)
                            {
                                case 0:
                                    return DialogResult.Yes;
                                case 1:
                                case 2:
                                    return DialogResult.No;
                            }
                        }
                        break;
                    case MessageBoxButtons.YesNoCancel:
                        {
                            int buttonOrdinal = Show(text, caption, icon, "Yes", "No", "Cancel");
                            switch (buttonOrdinal)
                            {
                                case 0:
                                    return DialogResult.Yes;
                                case 1:
                                    return DialogResult.No;
                                case 2:
                                case 3:
                                    return DialogResult.Cancel;
                            }
                        }
                        break;
                        // TODO: add two more cases: RetryCancel, and AbortRetryIgnore
                }
                return DialogResult.Cancel;
            }
            finally
            {
                parent.TopMost = isTopMost;
            }
        }

        //..................................................................................................................................

        #region IDisposable Members

        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>

        //------------------------------------------------------------------------------------------------------------------------
        public void Dispose()
        {
            if (_buttonControls != null)
            {
                foreach (var button in _buttonControls)
                    button.Dispose();
                _buttonControls = null;
            }
            if (_label != null)
            {
                _label.Dispose();
                _label = null;
            }
            if (_icon != null)
            {
                _icon.Dispose();
                _icon = null;
            }
            if (_form != null)
            {
                _form.Dispose();
                _form = null;
            }
            if (Font != null)
            {
                Font.Dispose();
                Font = null;
            }
        }

        #endregion
    }
}