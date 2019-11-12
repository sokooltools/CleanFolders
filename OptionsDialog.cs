using System;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;

namespace SokoolTools.CleanFolders
{
	public partial class OptionsDialog : Form
	{
		private const string HIVE = @"Folder\shell\CleanFolders";

		public OptionsDialog()
		{
			InitializeComponent();
			chkShowExplorerContextMenu.Checked = ContextMenuExists;
		}

		private void chkShowExplorerContextMenu_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox cbx = (CheckBox)sender;
			if (cbx.Focused)
				btnOK.Enabled = !(ContextMenuExists && cbx.Checked);
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			if (chkShowExplorerContextMenu.Checked)
				AddContextMenu();
			else
				RemoveContextMenu();
			Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private static bool ContextMenuExists
		{
			get
			{
				using (RegistryKey tstKey = Registry.ClassesRoot.OpenSubKey(HIVE, false))
				{
					return (tstKey != null);
				}
			}
		}

		private static void AddContextMenu()
		{
			using (RegistryKey newkey = Registry.ClassesRoot.CreateSubKey(HIVE))
			{
				if (newkey == null) return;
				newkey.SetValue("", "Clean Folder...");
				newkey.SetValue("Icon", AssemblyLocation);
				newkey.SetValue("MultiSelectModel", "Single");
				newkey.SetValue("OnlyInBrowserWindow", "");
				newkey.SetValue("Position", "Bottom");
				using (RegistryKey subkey = newkey.CreateSubKey("command"))
				{
					if (subkey != null)
						subkey.SetValue("", "\"" + AssemblyLocation + "\" \"%1\"");
				}
			}
		}

		private static void RemoveContextMenu()
		{
			Registry.ClassesRoot.DeleteSubKeyTree(HIVE);
		}

		//------------------------------------------------------------------------------------------
		/// <summary>
		/// AssemblyLocation
		/// </summary>
		/// <returns></returns>
		//------------------------------------------------------------------------------------------
		internal static string AssemblyLocation
		{
			get { return string.Format(@"{0}\{1}.exe", Environment.CurrentDirectory, Assembly.GetExecutingAssembly().GetName().Name); }
		}

	}
}
