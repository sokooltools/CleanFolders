using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SokoolTools.CleanFolders
{
	internal static class NativeMethods
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern int GetScrollPos(int hWnd, int nBar);

		[DllImport("user32.dll")]
		private static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

		//[DllImport("shlwapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		//private static extern bool PathCompactPathEx	(StringBuilder pszOut, string pszSrc, Int32 cchMax, Int32 dwFlags);

		private const int SB_HORZ = 0x0;
		private const int SB_VERT = 0x1;

		public const int WM_USER = 0x400;

		public const int EM_CUT = 0x300;
		public const int EM_COPY = 0x301;
		public const int EM_PASTE = 0x302;
		public const int EM_CLEAR = 0x303;
		public const int EM_UNDO = 0x304;

		public const int EM_CANUNDO = 0xC6;
		public const int EM_CANPASTE = WM_USER + 50;
		public const int EM_GETTEXTLENGTHEX = WM_USER + 95;

		/// Windows API SendMessage functions
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
		internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, ref GetTextLengthEx wParam, IntPtr lParam);

		// Return the handle of the window that has the focus.
		[DllImport("user32.dll")]
		public static extern IntPtr GetFocus();

		/// Windows API GetParent function
		[DllImport("user32", SetLastError = true)]
		public static extern IntPtr GetParent(IntPtr hwnd);

		// BOOL PathCompactPathEx(LPTSTR pszOut, LPCTSTR pszSrc, UINT cchMax, DWORD dwFlags );
		[DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool PathCompactPathEx(StringBuilder pszOut, string pszPath, int cchMax, int reserved);

		[DllImport("user32.dll")]
		private static extern int MessageBeep(int uType);

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the framework control associated with the specified handle.
		/// </summary>
		/// <param name="hControl">The handle to the control.</param>
		/// <returns></returns>
		//----------------------------------------------------------------------------------------------------
		public static Control GetFrameworkControl(IntPtr hControl)
		{
			Control rv = null;
			if (hControl.ToInt32() != 0)
			{
				// Try the parent, since with a ComboBox, we get a handle to an inner control.
				rv = Control.FromHandle(hControl) ?? GetFrameworkControl(GetParent(hControl));
			}
			return rv;
		}

		// Edit commands for the inner textbox in the ComboBox control.
		public static void Undo(IntPtr hEdit)
		{
			SendMessage(hEdit, EM_UNDO, IntPtr.Zero, IntPtr.Zero);
		}

		public static void Cut(IntPtr hEdit)
		{
			SendMessage(hEdit, EM_CUT, IntPtr.Zero, IntPtr.Zero);
		}

		public static void Copy(IntPtr hEdit)
		{
			SendMessage(hEdit, EM_COPY, IntPtr.Zero, IntPtr.Zero);
		}

		public static void Paste(IntPtr hEdit)
		{
			SendMessage(hEdit, EM_PASTE, IntPtr.Zero, IntPtr.Zero);
		}

		public static bool CanUndo(IntPtr hEdit)
		{
			return SendMessage(hEdit, EM_CANUNDO, IntPtr.Zero, IntPtr.Zero) != IntPtr.Zero;
		}

		public static void Clear(IntPtr hEdit)
		{
			SendMessage(hEdit, EM_CLEAR, IntPtr.Zero, IntPtr.Zero);
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Determine whether there is any format that can be pasted into the rich text box.
		/// </summary>
		/// <param name="hRichText">The handle to the rich text.</param>
		/// <returns><c>true</c> if any format can be pasted; otherwise, <c>false</c>.</returns>
		//----------------------------------------------------------------------------------------------------
		public static bool CanPasteAnyFormat(IntPtr hRichText)
		{
			return SendMessage(hRichText, EM_CANPASTE, IntPtr.Zero, IntPtr.Zero) != IntPtr.Zero;
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the length of a control's Text.  Required since using the RichTextBox.Length property wipes
		/// out the Undo/Redo buffer.
		/// </summary>
		/// <param name="hControl">The h control.</param>
		/// <returns></returns>
		//----------------------------------------------------------------------------------------------------
		public static int GetTextLength(IntPtr hControl)
		{
			var lpGtl = new GetTextLengthEx { uiFlags = 0, uiCodePage = 1200 };
			return SendMessage(hControl, EM_GETTEXTLENGTHEX, ref lpGtl, IntPtr.Zero).ToInt32();
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct GetTextLengthEx
		{
			public int uiFlags;
			public int uiCodePage;
		}

		//.............................................................................................................

		#region MessageBeep

		public static void MessageBeep(MessageBeepType beepType)
		{
			MessageBeep((int)beepType);
		}

		public enum MessageBeepType
		{
			Asterisk = 0x40,
			Exclamation = 0x30,
			Hand = 0x10,
			Question = 0x20
		}

		public static Point GetTreeViewScrollPos(IWin32Window treeView)
		{
			return new Point(
				GetScrollPos((int)treeView.Handle, SB_HORZ),
				GetScrollPos((int)treeView.Handle, SB_VERT));
		}

		public static void SetTreeViewScrollPos(IWin32Window treeView, Point scrollPosition)
		{
			SetScrollPos(treeView.Handle, SB_HORZ, scrollPosition.X, true);
			SetScrollPos(treeView.Handle, SB_VERT, scrollPosition.Y, true);
		}

		public static string TruncatePath(string path, int length)
		{
			var sb = new StringBuilder(260);
			PathCompactPathEx(sb, path, length + 1, 0);
			return sb.ToString();
		}

		#endregion
	}

}

