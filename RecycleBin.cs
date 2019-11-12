using System;
using System.Runtime.InteropServices;

namespace SokoolTools.CleanFolders
{
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Send files directly to the recycle bin.
	/// </summary>
	//------------------------------------------------------------------------------------------------------------------
	public class RecycleBin
	{
		[DllImport("shell32.dll", CharSet = CharSet.Auto, EntryPoint = "SHFileOperation")]
		private static extern int SHFileOperationx86(ref ShFileOpStructX86 fileOp);

		[DllImport("shell32.dll", CharSet = CharSet.Auto, EntryPoint = "SHFileOperation")]
		private static extern int SHFileOperationx64(ref ShFileOpStructX64 fileOp);

		private static bool IsWOW64Process() => IntPtr.Size == 8;

        //........................................................................................................................

		#region FileOperationFlags enum

		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Possible flags for the SHFileOperation method.
		/// </summary>
		//--------------------------------------------------------------------------------------------------------------
		[Flags]
		public enum FileOperationFlags : ushort
		{
			//--------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Do not show a dialog during the process.
			/// </summary>
			//--------------------------------------------------------------------------------------------------------------
			FOF_SILENT = 0x0004,

			//--------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Do not ask the user to confirm selection.
			/// </summary>
			//--------------------------------------------------------------------------------------------------------------
			FOF_NOCONFIRMATION = 0x0010,

			//--------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Delete the file to the recycle bin.  (Required flag to send a file to the bin)
			/// </summary>
			//--------------------------------------------------------------------------------------------------------------
			FOF_ALLOWUNDO = 0x0040,

			//--------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Do not show the names of the files or folders that are being recycled.
			/// </summary>
			//--------------------------------------------------------------------------------------------------------------
			FOF_SIMPLEPROGRESS = 0x0100,

			//--------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Surpress errors, if any occur during the process.
			/// </summary>
			//--------------------------------------------------------------------------------------------------------------
			FOF_NOERRORUI = 0x0400,

			//--------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Warn if files are too big to fit in the recycle bin and will need to be deleted completely.
			/// </summary>
			//--------------------------------------------------------------------------------------------------------------
			FOF_WANTNUKEWARNING = 0x4000,
		}

		#endregion

		//........................................................................................................................

		#region FileOperationType enum

		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// File Operation Function Type for SHFileOperation
		/// </summary>
		//--------------------------------------------------------------------------------------------------------------
		public enum FileOperationType : uint
		{
			//--------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Move the objects
			/// </summary>
			//--------------------------------------------------------------------------------------------------------------
			FO_MOVE = 0x0001,

			//--------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Copy the objects
			/// </summary>
			//--------------------------------------------------------------------------------------------------------------
			FO_COPY = 0x0002,

			//--------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Delete (or recycle) the objects
			/// </summary>
			//--------------------------------------------------------------------------------------------------------------
			FO_DELETE = 0x0003,

			//--------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Rename the object(s)
			/// </summary>
			//--------------------------------------------------------------------------------------------------------------
			FO_RENAME = 0x0004,
		}

		#endregion

		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Send the specified file or directory to the recycle bin.
		/// </summary>
		/// <param name="path">Location of directory or file to recycle</param>
		/// <param name="flags">FileOperationFlags to add in addition to FOF_ALLOWUNDO</param>
		//--------------------------------------------------------------------------------------------------------------
		public static bool Send(string path, FileOperationFlags flags = FileOperationFlags.FOF_NOCONFIRMATION | FileOperationFlags.FOF_WANTNUKEWARNING)
		{
			try
			{
				if (IsWOW64Process())
				{
					var fs = new ShFileOpStructX64
					    {
							wFunc = FileOperationType.FO_DELETE, 
							pFrom = path + '\0' + '\0', // important to double-terminate the string.
							fFlags = FileOperationFlags.FOF_ALLOWUNDO | flags
						};			
					SHFileOperationx64(ref fs);
				}
				else
				{
					var fs = new ShFileOpStructX86
						{
							wFunc = FileOperationType.FO_DELETE,
							pFrom = path + '\0' + '\0', // important to double-terminate the string.
							fFlags = FileOperationFlags.FOF_ALLOWUNDO | flags
						};
					SHFileOperationx86(ref fs);
				}
				return true;
			}
			catch
			{
				return false;
			}
		}

		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Send file silently to recycle bin.  Surpress dialog, surpress errors, delete if too large.
		/// </summary>
		/// <param name="path">Location of directory or file to recycle</param>
		//--------------------------------------------------------------------------------------------------------------
		public static bool SendSilent(string path)
		{
			return Send(path, FileOperationFlags.FOF_NOCONFIRMATION | FileOperationFlags.FOF_NOERRORUI | FileOperationFlags.FOF_SILENT);
		}

		//........................................................................................................................

		#region Nested type: ShFileOpStructX64

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private struct ShFileOpStructX64
		{
			private readonly IntPtr hwnd;

			[MarshalAs(UnmanagedType.U4)]
			public FileOperationType wFunc;

			public string pFrom;
			private readonly string pTo;
			public FileOperationFlags fFlags;

			[MarshalAs(UnmanagedType.Bool)]
			private readonly bool fAnyOperationsAborted;

			private readonly IntPtr hNameMappings;
			private readonly string lpszProgressTitle;
		}

		#endregion

		//........................................................................................................................

		#region Nested type: ShFileOpStructX86

		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// SHFILEOPSTRUCT for SHFileOperation from COM
		/// </summary>
		//--------------------------------------------------------------------------------------------------------------
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
		private struct ShFileOpStructX86
		{
			private readonly IntPtr hwnd;

			[MarshalAs(UnmanagedType.U4)]
			public FileOperationType wFunc;

			public string pFrom;
			private readonly string pTo;
			public FileOperationFlags fFlags;

			[MarshalAs(UnmanagedType.Bool)]
			private readonly bool fAnyOperationsAborted;

			private readonly IntPtr hNameMappings;
			private readonly string lpszProgressTitle;
		}

		#endregion
	}
}