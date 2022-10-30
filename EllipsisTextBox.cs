using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

//===========================================================================================================
// Sample usage:
//
//		EllipsisTextBox txtBox = new EllipsisTextBox();
//		txtBox.EllipsisType = EllipsisTextBox.EllipsisLocation.Path;
//		txtBox.Text = @"C:\directory\subdirectory\filename.ext";
// 
// Depending on the width of the textbox, it will display something like "C:\di…\filename.ext".
//
// However,
//		string boo = txtBox.Text; yields
//		boo == @"C:\directory\subdirectory\filename.ext", the original string, NOT the truncated string.
//
//===========================================================================================================

namespace SokoolTools.CleanFolders
{
	//--------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Creates a TextBox with built-in ellipsis control.
	/// (1) Specify EllipsisTextBox.EllipsisType (e.g., = EllipsisLocation.Path)
	/// (2) Put text into the textbox (e.g., EllipsisTextBox.Text = @"c:\directory\file.txt")
	/// The displayed text is the original text modified by the EllipsisType.
	/// (e.g., "…\file.txt" if that's what fills the small textbox)
	/// The string returned by EllipsisTextBox.Text is the original text, which may differ from what is 
	/// showing in the textbox.
	/// </summary>
	//--------------------------------------------------------------------------------------------------------
	public class EllipsisTextBox : TextBox
	{
		private string _fullText;
		private string _ellipsisText;
		private EllipsisLocation _ellipsisLocation = EllipsisLocation.None;
		private TextFormatFlags _ellipsisTextFormatFlag = 0;
		private readonly ToolTip _toolTip1;

		public enum EllipsisLocation { Path, Word, End, None };
		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="EllipsisTextBox"/> class.
		/// </summary>
		//----------------------------------------------------------------------------------------------------
		public EllipsisTextBox()  // : base()
		{
			AllowDrop = true;
			_fullText = String.Empty;
			base.Text = String.Empty;
			base.TextChanged += EllipsisTextBox_TextChanged;
			Enter += EllipsisTextBox_Enter;
			Leave += EllipsisTextBox_Leave;
			Resize += EllipsisTextBox_Resize;
			EllipsisType = EllipsisLocation.Path;
			_toolTip1 = new ToolTip { ShowAlways = true };
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Indicates where the ellipsis appears inside the text.
		/// (e.g. TextFormatFlags.PathEllipsis, TextFormatFlags.EndEllipsis, TextFormatFlags.WordEllipsis...)
		/// </summary>
		/// <value>The type of the ellipsis. </value>
		//----------------------------------------------------------------------------------------------------
		[Category("Misc"), Description("Indicates where the ellipsis appears inside the text.")]
		[DefaultValue(EllipsisLocation.Path)]
		public EllipsisLocation EllipsisType
        {
            get => _ellipsisLocation;
            set
            {
                _ellipsisLocation = value;
                switch (value)
                {
                    case EllipsisLocation.None:
                        _ellipsisTextFormatFlag = 0;
                        break;
                    case EllipsisLocation.End:
                        _ellipsisTextFormatFlag = TextFormatFlags.EndEllipsis;
                        break;
                    case EllipsisLocation.Path:
                        _ellipsisTextFormatFlag = TextFormatFlags.PathEllipsis;
                        break;
                    case EllipsisLocation.Word:
                        _ellipsisTextFormatFlag = TextFormatFlags.WordEllipsis;
                        break;
                    default:
                        _ellipsisTextFormatFlag = 0;
                        _ellipsisLocation = EllipsisLocation.None;
                        break;
                }
            }
        }

        //----------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the original, unmodified assigned text or sets a copy of the unmodified assigned text, but
        /// displays it modified by the <see cref="EllipsisType"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The text displayed in the control.
        /// </returns>
        //----------------------------------------------------------------------------------------------------
        [Category("Misc"), Description("Gets the original, unmodified assigned text or sets a copy of the unmodified"
			+ " assigned text, but displays it modified by the EllipsisType.")]
		public new string Text
		{
			get => Focused ? base.Text : _fullText;
            set
			{
				// Ensure we get a trimmed copy of the original string,
				// NOT a reference to the original string.
				// _NOTE: seems to be a "bug" in .Trim() – returns a reference
				// to the original string if .Trim() does not need to modify the original string, otherwise, it returns a reference to a new string.
				// We could use…
				// string truncText = (new StringBuilder(value)).ToString();
				// but drop the below to see the .Trim() anomaly!
				_fullText = (value + " ").Trim(); // We want a new copy, not a reference to value!

				// Change copy of fullText to ellipsis truncated form
				//truncText = (new StringBuilder(value)).ToString();
				_ellipsisText = (_fullText + " ").Trim(); // Want a new copy to modify, else fullText would be modified, too!
				TextRenderer.MeasureText(_ellipsisText, Font, new Size(Width, Height), TextFormatFlags.ModifyString | _ellipsisTextFormatFlag);
				base.Text = _ellipsisText;

				_toolTip1.SetToolTip(this, _fullText);
			}
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether the control can accept data that the user drags onto it.
		/// </summary>
		/// <value></value>
		/// <returns>Always returns true.</returns>
		//----------------------------------------------------------------------------------------------------
		//[Browsable(false)]
		//[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Category("Misc"), Description("Gets or sets a value indicating whether the control can accept data that the user drags onto it [default is True].")]
		[DefaultValue(true)]
		public sealed override bool AllowDrop { get; set; }

		[Category("Misc"), Description("Gets or sets a value indicating whether the control will handle files dropped onto it [default is false].")]
		[DefaultValue(false)]
		public bool HandleDrop { get; set; }

		private bool IsTextChangedIgnored { get; set; }

		public new event EventHandler TextChanged;

		private void EllipsisTextBox_TextChanged(object sender, EventArgs e)
		{
			if (!IsTextChangedIgnored)
				TextChanged?.Invoke(this, e);
		}

		private void EllipsisTextBox_Enter(object sender, EventArgs e)
		{
			IsTextChangedIgnored = true;
			base.Text = _fullText;
			IsTextChangedIgnored = false;
		}

		private void EllipsisTextBox_Leave(object sender, EventArgs e)
		{
			IsTextChangedIgnored = true;
			Text = base.Text;
			IsTextChangedIgnored = false;
		}

		private void EllipsisTextBox_Resize(object sender, EventArgs e)
		{
			if (Focused) return;
			IsTextChangedIgnored = true;
			Text = _fullText;
			IsTextChangedIgnored = false;
		}

		protected override void OnDragEnter(DragEventArgs e)
		{
			if (HandleDrop)
			{
				if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
					e.Effect = DragDropEffects.Copy;
			}
			else
				base.OnDragEnter(e);
		}

		protected override void OnDragDrop(DragEventArgs e)
		{
			if (HandleDrop)
			{
				var filePath = (string[]) e.Data.GetData(DataFormats.FileDrop, false);
				if (filePath.Length > 0) Text = filePath[0];
			}
			else
				base.OnDragDrop(e);
		}

	}
}
