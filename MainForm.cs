using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

namespace SokoolTools.CleanFolders
{
	public partial class MainForm : Form
	{
		private TreeNode _dirTreeNode;
		private TreeNode _topNode;

		private bool _isFilesChanged;
		private bool _isFormActivated;
		private bool _isUpdatingTreeView;

		private FormWindowState _lastState = FormWindowState.Normal;
		private static readonly Color UncheckedColor = Color.DarkSlateGray;
		private static readonly Color CheckedColor = Color.Brown;

		private static int _iFoldersDeleted;
		private static int _iFilesDeleted;

		private static int _totalFoldersCheckmarked;
		private static int _totalFilesCheckmarked;

		private const string DELETED = "Deleted";
		private const string COULD_NOT_DELETE = "Could not delete";

		private enum FileType
		{
			Folder = 0,
			File = 1
		}

		public MainForm(IList<string> args)
		{
			InitializeComponent();

			AttachEventHandlers();

			LogFile = new List<LogData>();

			if (args.Count > 0)
				txtFolderPath.Text = args[0];
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			ReloadTree();
			timer1.Stop();
		}

		private void AttachEventHandlers()
		{
			DragEnter += Folder_DragEnter;
			DragDrop += Folder_DragDrop;
			txtFolderPath.DragEnter += Folder_DragEnter;
			txtFolderPath.DragDrop += Folder_DragDrop;
			treeView1.DragEnter += Folder_DragEnter;
			treeView1.DragDrop += Folder_DragDrop;
			treeView1.AfterCheck += TreeView1_AfterCheck;
			treeView1.MouseClick += TreeView1_MouseClick;
			treeView1.MouseDoubleClick += TreeView1_MouseDoubleClick;

			btnDelete.Click += BtnDelete_Click;
			btnClose.Click += btnCancel_Click;
			btnBrowse.Click += btnBrowse_Click;
			btnLog.Click += btnLog_Click;

			contextMenuStrip1.Opening += ContextMenu_Opening;
			mnuDelete.Click += contextMenu_Click;
			mnuExpandAll.Click += contextMenu_Click;
			mnuCollapseAll.Click += contextMenu_Click;
			mnuUncheckAll.Click += contextMenu_Click;
			mnuOptions.Click += contextMenu_Click;
			mnuAbout.Click += contextMenu_Click;
			mnuProperties.Click += contextMenu_Click;

			txtFolderPath.TextChanged += TxtFilePath_TextChanged;
			txtMscFiles.TextChanged += TxtMscFiles_TextChanged;
			txtMscFolders.TextChanged += TxtMscFolders_TextChanged;

			chkBinFolders.CheckStateChanged += ChkFilesAndOrFolders_CheckedStateChanged;
			chkObjFolders.CheckStateChanged += ChkFilesAndOrFolders_CheckedStateChanged;
			chkMscFolders.CheckStateChanged += ChkFilesAndOrFolders_CheckedStateChanged;
			chkSuoFiles.CheckStateChanged += ChkFilesAndOrFolders_CheckedStateChanged;
			chkUsrFiles.CheckStateChanged += ChkFilesAndOrFolders_CheckedStateChanged;
			chkMscFiles.CheckStateChanged += ChkFilesAndOrFolders_CheckedStateChanged;

			backgroundWorker1.DoWork += BackgroundWorker1_DoWork;
			backgroundWorker1.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;

			fileSystemWatcher1.IncludeSubdirectories = true;
			fileSystemWatcher1.Created += FileSystemWatcher1_Changed;
			fileSystemWatcher1.Deleted += FileSystemWatcher1_Changed;
			fileSystemWatcher1.Renamed += FileSystemWatcher1_Renamed;
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the log file.
		/// </summary>
		/// <value>The log file.</value>
		//----------------------------------------------------------------------------------------------------
		private List<LogData> LogFile { get; set; }

		private void FileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
		{
			_isFilesChanged = true;
			AutoRefreshTree();
		}

		private void FileSystemWatcher1_Renamed(object sender, RenamedEventArgs e)
		{
			_isFilesChanged = true;
			AutoRefreshTree();
		}

		protected override void OnResizeEnd(EventArgs e)
		{
			ResizeForm();
		}

		protected override void OnDeactivate(EventArgs e)
		{
			_isFormActivated = false;
		}

		protected override void OnActivated(EventArgs e)
		{
			_isFormActivated = true;

			if (WindowState != FormWindowState.Minimized) // TODO:
				AutoRefreshTree();
			TopMost = true;
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the event raised when a folder is dragged onto this application.
		/// </summary>
		//----------------------------------------------------------------------------------------------------
		private static void Folder_DragEnter(object sender, DragEventArgs e)
		{
			if (!e.Data.GetDataPresent(DataFormats.FileDrop, false))
				return;

			var filePath = (string[])e.Data.GetData(DataFormats.FileDrop, false);
			bool isFolder = (File.GetAttributes(filePath[0]) & FileAttributes.Directory) == FileAttributes.Directory;
			if (isFolder)
				e.Effect = DragDropEffects.Copy;
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the event raised when a folder is dragged then dropped onto this application.
		/// </summary>
		//----------------------------------------------------------------------------------------------------
		private void Folder_DragDrop(object sender, DragEventArgs e)
		{
			Activate();
			var folders = (string[])e.Data.GetData(DataFormats.FileDrop);
			txtFolderPath.Text = folders[0];
			treeView1.Focus();
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the event raised when the "File Path" control has its text changed.
		/// </summary>
		//----------------------------------------------------------------------------------------------------
		private void TxtFilePath_TextChanged(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;
			_topNode = null;
			if (!string.IsNullOrEmpty(txtFolderPath.Text))
				backgroundWorker1.RunWorkerAsync(txtFolderPath.Text);
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the DoWork event of the backgroundWorker1 control.
		/// </summary>
		//----------------------------------------------------------------------------------------------------
		private void BackgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			_totalFoldersCheckmarked = 0;
			_totalFilesCheckmarked = 0;
			_totalFileSize = 0;

			_dirTreeNode = GetDirectoryTree(e.Argument.ToString());
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the RunWorkerCompleted event of the backgroundWorker1 control.
		/// </summary>
		//----------------------------------------------------------------------------------------------------
		private void BackgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
		{
			treeView1.Nodes.Clear();
			if (_dirTreeNode != null)
			{
				treeView1.Nodes.Add(_dirTreeNode);
				treeView1.Nodes[0].Expand();
			}

			if (treeView1.Nodes.Count > 0 && _topNode != null)
			{
				TreeNode node = FindNodeByFullName(treeView1.Nodes, _topNode.Tag.ToString());
				treeView1.TopNode = node;
			}

			bool isValidPath = Directory.Exists(txtFolderPath.Text);

			btnDelete.Enabled = isValidPath;

			UpdateStatusBar();

			if (!isValidPath)
				txtFolderPath.Text = string.Empty;
			else
				fileSystemWatcher1.Path = txtFolderPath.Text;

			_isFilesChanged = false;

			Cursor = Cursors.Default;
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the event raised when any of the "Auto-Select" checkboxes have their check changed.
		/// </summary>
		//----------------------------------------------------------------------------------------------------
		private void ChkFilesAndOrFolders_CheckedStateChanged(object sender, EventArgs e)
		{
            if (!(sender is CheckBox cbx) || !cbx.Focused) return;
            ReloadTree();
			cbx.ThreeState = false;
		}

		private void TxtMscFiles_TextChanged(object sender, EventArgs e)
		{
			bool isNullOrEmpty = string.IsNullOrEmpty(txtMscFiles.Text);
			if (txtMscFiles.Focused)
			{
				timer1.Stop();
				chkMscFiles.Checked = !isNullOrEmpty;
				timer1.Start();
			}
			chkMscFiles.Enabled = !isNullOrEmpty;
		}

		private void TxtMscFolders_TextChanged(object sender, EventArgs e)
		{
			bool isNullOrEmpty = string.IsNullOrEmpty(txtMscFolders.Text);
			if (txtMscFolders.Focused)
			{
				timer1.Stop();
				chkMscFolders.Checked = !isNullOrEmpty;
				timer1.Start();
			}
			chkMscFolders.Enabled = !isNullOrEmpty;
		}

		private void BtnDelete_Click(object sender, EventArgs e)
		{
			int totalFilesToDelete = _totalFilesCheckmarked + _totalFoldersCheckmarked;

			string permanently = chkSendToRecycleBin.Checked ? string.Empty : " permanently";
			string text = totalFilesToDelete == 1
							? $"Are you sure you want to{permanently} delete the selected file or folder?"
							: $"Are you sure you want to{permanently} delete the {totalFilesToDelete} selected files and/or folders?";
			MessageBoxIcon icon = chkSendToRecycleBin.Checked ? MessageBoxIcon.Exclamation : MessageBoxIcon.Stop;
			if (MessageDialog.Show(this, text, Text, MessageBoxButtons.OKCancel, icon) != DialogResult.OK)
				return;

			Cursor = Cursors.WaitCursor;
			fileSystemWatcher1.Deleted -= FileSystemWatcher1_Changed;
			try
			{
				_iFoldersDeleted = 0;
				_iFilesDeleted = 0;

				try
				{
					if (chkSendToRecycleBin.Checked)
						DeleteNodesTopToBottom(treeView1.Nodes);
					else
						DeleteNodesBottomToTop(treeView1.Nodes);
				}
				catch (Exception ex)
				{
					if (ex is OperationCanceledException)
						Console.WriteLine(@"Cancelled");
				}

				ReloadTree();

				int totalDeleted = _iFoldersDeleted + _iFilesDeleted;
				int totalNotDeleted = totalFilesToDelete - totalDeleted;

				var sb = new StringBuilder();

				if (chkSendToRecycleBin.Checked)
				{
					sb.AppendFormat(totalDeleted == 1
											? "{0} file or folder was sent to the recycle bin.\n"
											: "{0} files and/or folders were sent to the recycle bin.\n", totalDeleted);
				}
				else
				{
					sb.AppendFormat(totalDeleted == 1
											? "{0} file or folder was permanently deleted.\n"
											: "{0} files and/or folders were permanently deleted.\n", totalDeleted);
				}

				if (totalNotDeleted > 0)
				{
					sb.AppendFormat(totalNotDeleted == 1
						? "{0} file and/or folder could not be deleted.\n"
						: "{0} files and/or folders could not be deleted.\n", totalNotDeleted);
				}

				MessageDialog.Show(this, sb.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			finally
			{
				btnLog.Enabled = true;
				fileSystemWatcher1.Deleted += FileSystemWatcher1_Changed;
				Cursor = Cursors.Default;
			}
		}

		private void TreeView1_AfterCheck(object sender, TreeViewEventArgs e)
		{
			if (!_isUpdatingTreeView)
			{
				SetMainCheckboxCheckState(e.Node);

				_isUpdatingTreeView = true;

				if (e.Action == TreeViewAction.ByMouse)
					treeView1.SelectedNode = e.Node;

				UpdateTotal(e.Node);

				UncheckParents(e.Node);
				CheckmarkChildren(e.Node, e.Node.Checked);

				_isUpdatingTreeView = false;
			}

			UpdateStatusBar();
		}

		private void TreeView1_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right) return;
			TreeNode node = treeView1.GetNodeAt(e.X, e.Y);
			treeView1.SelectedNode = node;
		}

		private void TreeView1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			TreeNode node = treeView1.GetNodeAt(e.X, e.Y);
			treeView1.SelectedNode = node;
			node.Checked = !node.Checked;
		}

		private void ContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			bool isEnabled = treeView1.Nodes.Count > 0;
			mnuDelete.Enabled = isEnabled;
			mnuExpandAll.Enabled = isEnabled;
			mnuCollapseAll.Enabled = isEnabled;
			mnuUncheckAll.Enabled = isEnabled;
			mnuProperties.Enabled = isEnabled;
		}

		private void contextMenu_Click(object sender, EventArgs e)
		{
			if (sender == mnuDelete)
			{
				string msg =
					$"Are you sure you want to delete the selected {(treeView1.SelectedNode.ImageIndex == (int) FileType.Folder ? "folder" : "file")}?";
				if (MessageDialog.Show(this, msg, Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
					return;

				fileSystemWatcher1.Deleted -= FileSystemWatcher1_Changed;
				try
				{
					DeleteNode(treeView1.SelectedNode);
					ReloadTree();
				}
				catch (Exception ex)
				{
					if (ex is OperationCanceledException)
						Console.WriteLine(@"Cancelled");
				}
				finally
				{
					btnLog.Enabled = true;
					fileSystemWatcher1.Deleted -= FileSystemWatcher1_Changed;
				}
			}
			else if (sender == mnuExpandAll)
			{
				treeView1.ExpandAll();
			}
			else if (sender == mnuCollapseAll)
			{
				treeView1.CollapseAll();
			}
			else if (sender == mnuUncheckAll)
			{
				UncheckCheckboxes(grpAutoSelect);
				ReloadTree();
			}
			else if (sender == mnuOptions)
			{
				using (var dlg = new OptionsDialog())
				{
					dlg.ShowDialog(this);
				}
			}
			else if (sender == mnuProperties)
			{
				TopMost = false;
				TreeNode node = treeView1.SelectedNode;
				FileProperties.Show(node.Tag.ToString());
			}
			else if (sender == mnuAbout)
			{
				using (var dlg = new AboutBox())
				{
					dlg.ShowDialog(this);
				}
			}
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			using (var dlg = new FolderBrowserDialog())
			{
				dlg.SelectedPath = txtFolderPath.Text;
				dlg.ShowNewFolderButton = false;
				dlg.Description = @"Select Folder To Clean:";
				if (dlg.ShowDialog(this) == DialogResult.OK)
					txtFolderPath.Text = dlg.SelectedPath;
			}
		}

		private void btnLog_Click(object sender, EventArgs e)
		{
			using (var dlg = new LogFileViewer())
			{
				dlg.LogData = LogFile;
				dlg.ClientSize = ClientSize;
				dlg.ShowDialog(this);
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void btnExpandAll_Click(object sender, EventArgs e)
		{
			treeView1.ExpandAll();
		}

		private void btnCollapseAll_Click(object sender, EventArgs e)
		{
			treeView1.CollapseAll();
		}

		//.............................................................................................................

		#region Helper Methods

		private void ResizeForm()
		{
			var delta = treeView1.ClientSize.Height % treeView1.ItemHeight;
			if (delta > 0)
				Height += treeView1.ItemHeight - delta;
		}

		private static ulong _totalFileSize;

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the total number of files checkmarked and their total size.
		/// </summary>
		/// <param name="node">The node.</param>
		//----------------------------------------------------------------------------------------------------
		private static void UpdateTotal(TreeNode node)
		{
			if (node.ImageIndex == (int)FileType.File)
			{
				ulong fileSize = (ulong)new FileInfo(node.Tag.ToString()).Length;
				if (node.Checked)
					_totalFileSize += fileSize;
				else
					_totalFileSize -= fileSize;
			}
			if (node.Checked)
			{
				node.ForeColor = CheckedColor;
				_totalFilesCheckmarked++;
			}
			else
			{
				node.ForeColor = UncheckedColor;
				_totalFilesCheckmarked--;
			}
		}

		private void UpdateStatusBar()
		{
			statusLabel1.Text =
				$@"Total files/folders marked for deletion: {_totalFilesCheckmarked + _totalFoldersCheckmarked}"
				+ $@" [Size: {FormatFileSize(_totalFileSize, 0)}]";
			btnDelete.Enabled = _totalFilesCheckmarked + _totalFoldersCheckmarked > 0;
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Places a checkmark beside the child nodes where it is appropriate.
		/// </summary>
		/// <param name="treeNode">The tree node.</param>
		/// <param name="isChecked">if set to <c>true</c> then it is checked.</param>
		//----------------------------------------------------------------------------------------------------
		private static void CheckmarkChildren(TreeNode treeNode, bool isChecked)
		{
			foreach (TreeNode node in treeNode.Nodes)
			{
				if (node.Checked != isChecked)
				{
					node.Checked = isChecked;
					UpdateTotal(node);
				}
				if (node.Nodes.Count > 0)
					CheckmarkChildren(node, isChecked);
			}
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Reloads the tree control by re-reading the folder path.
		/// </summary>
		//----------------------------------------------------------------------------------------------------
		private void ReloadTree()
		{
			if (treeView1.Nodes.Count == 0)
				return;

			// Hold onto the node at the top of the treeview.
			_topNode = treeView1.TopNode;

			Cursor = Cursors.WaitCursor;
			backgroundWorker1.RunWorkerAsync(txtFolderPath.Text);
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Creates the directory tree. (This gets called from a separate thread).
		/// </summary>
		/// <param name="filePath">The file path.</param>
		//----------------------------------------------------------------------------------------------------
		private TreeNode GetDirectoryTree(string filePath)
		{
			TreeNode treeNode = null;
			var rootDirectoryInfo = new DirectoryInfo(filePath);
			if (rootDirectoryInfo.Exists)
			{
				treeNode = CreateDirectoryNode(rootDirectoryInfo, false);
			}
			return treeNode;
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Creates the directory node.
		/// </summary>
		/// <param name="directoryInfo">The directory info.</param>
		/// <param name="isParentChecked">if set to <c>true</c> if parent is checked.</param>
		/// <returns></returns>
		//----------------------------------------------------------------------------------------------------
		private TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo, bool isParentChecked)
		{
			if (!directoryInfo.Exists) return null;

			var parentDirNode = new TreeNode(directoryInfo.Name) { Tag = directoryInfo.FullName, ImageIndex = (int)FileType.Folder }; //, ImageIndex = 0 };

			bool status = isParentChecked || IsFolderNameChecked(directoryInfo.Name);

			if (status)
			{
				if (!parentDirNode.Checked)
				{
					parentDirNode.Checked = true;
					parentDirNode.ForeColor = CheckedColor;
					_totalFoldersCheckmarked++;
				}
				parentDirNode.Expand();
			}

			// Process all the 'sub-folders' in the current folder.
			foreach (DirectoryInfo subDirectoryInfo in directoryInfo.GetDirectories())
			{
				TreeNode childDirNode = CreateDirectoryNode(subDirectoryInfo, parentDirNode.Checked);
				childDirNode.ImageIndex = (int)FileType.Folder;
				parentDirNode.Nodes.Add(childDirNode);

				if (childDirNode.IsExpanded)
					parentDirNode.Expand();

				if (status)
					childDirNode.Checked = true;
			}

			// Process all the 'files' in the current folder.
			foreach (FileInfo file in directoryInfo.GetFiles())
			{
				var fileNode = new TreeNode(file.Name) { Tag = file.FullName, ImageIndex = (int)FileType.File };

				parentDirNode.Nodes.Add(fileNode);
				if (!(parentDirNode.Checked || IsFileExtensionChecked(file.Name))) continue;

				if (!fileNode.Checked)
				{
					fileNode.Checked = true;
					UpdateTotal(fileNode);
				}
				parentDirNode.Expand();
			}

			return parentDirNode;
		}

		private bool IsFolderNameChecked(string folderName)
		{
			string[] collection = txtMscFolders.Text.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
			return chkBinFolders.CheckState != CheckState.Unchecked && folderName.Equals("bin", StringComparison.InvariantCultureIgnoreCase)
				 || chkObjFolders.CheckState != CheckState.Unchecked && folderName.Equals("obj", StringComparison.InvariantCultureIgnoreCase)
				 || chkMscFolders.CheckState != CheckState.Unchecked && folderName.EqualsAny(collection);
		}

		private bool IsFileExtensionChecked(string fileName)
		{
			string[] collection = txtMscFiles.Text.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
			return chkSuoFiles.CheckState != CheckState.Unchecked && fileName.EndsWith(".suo", StringComparison.InvariantCultureIgnoreCase)
				 || chkUsrFiles.CheckState != CheckState.Unchecked && fileName.EndsWith(".user", StringComparison.InvariantCultureIgnoreCase)
				 || chkMscFiles.CheckState != CheckState.Unchecked && fileName.EndsWithAny(collection);
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the main checkbox checkstate.
		/// </summary>
		/// <param name="node">The tree node.</param>
		//----------------------------------------------------------------------------------------------------
		private void SetMainCheckboxCheckState(TreeNode node)
		{
			if (node.Text.Equals("bin", StringComparison.InvariantCultureIgnoreCase))
			{
				chkBinFolders.CheckStateChanged -= ChkFilesAndOrFolders_CheckedStateChanged;
				if (IsAnyFolderCheckmarked(treeView1.Nodes, "bin", node.Checked))
				{
					chkBinFolders.CheckState = node.Checked ? CheckState.Checked : CheckState.Unchecked;
					chkBinFolders.ThreeState = false;
				}
				else
				{
					chkBinFolders.ThreeState = true;
					chkBinFolders.CheckState = CheckState.Indeterminate;
				}
				chkBinFolders.CheckStateChanged += ChkFilesAndOrFolders_CheckedStateChanged;
			}
			else if (node.Text.Equals("obj", StringComparison.InvariantCultureIgnoreCase))
			{
				chkObjFolders.CheckStateChanged -= ChkFilesAndOrFolders_CheckedStateChanged;
				if (IsAnyFolderCheckmarked(treeView1.Nodes, "obj", node.Checked))
				{
					chkObjFolders.CheckState = node.Checked ? CheckState.Checked : CheckState.Unchecked;
					chkObjFolders.ThreeState = false;
				}
				else
				{
					chkObjFolders.ThreeState = true;
					chkObjFolders.CheckState = CheckState.Indeterminate;
				}
				chkObjFolders.CheckStateChanged += ChkFilesAndOrFolders_CheckedStateChanged;
			}
			else if (node.Text.Equals(".svn", StringComparison.InvariantCultureIgnoreCase))
			{
				chkMscFolders.CheckStateChanged -= ChkFilesAndOrFolders_CheckedStateChanged;
				if (IsAnyFolderCheckmarked(treeView1.Nodes, "svn", node.Checked))
				{
					chkMscFolders.CheckState = node.Checked ? CheckState.Checked : CheckState.Unchecked;
					chkMscFolders.ThreeState = false;
				}
				else
				{
					chkMscFolders.ThreeState = true;
					chkMscFolders.CheckState = CheckState.Indeterminate;
				}
				chkMscFolders.CheckStateChanged += ChkFilesAndOrFolders_CheckedStateChanged;
			}
			else if (node.Text.EndsWith(".suo", StringComparison.InvariantCultureIgnoreCase))
			{
				chkSuoFiles.CheckStateChanged -= ChkFilesAndOrFolders_CheckedStateChanged;
				if (IsAnyFileCheckmarked(treeView1.Nodes, ".suo", node.Checked))
				{
					chkSuoFiles.CheckState = node.Checked ? CheckState.Checked : CheckState.Unchecked;
					chkSuoFiles.ThreeState = false;
				}
				else
				{
					chkSuoFiles.ThreeState = true;
					chkSuoFiles.CheckState = CheckState.Indeterminate;
				}
				chkSuoFiles.CheckStateChanged += ChkFilesAndOrFolders_CheckedStateChanged;
			}
			else if (node.Text.EndsWith(".user", StringComparison.InvariantCultureIgnoreCase))
			{
				chkUsrFiles.CheckStateChanged -= ChkFilesAndOrFolders_CheckedStateChanged;
				if (IsAnyFileCheckmarked(treeView1.Nodes, ".user", node.Checked))
				{
					chkUsrFiles.CheckState = node.Checked ? CheckState.Checked : CheckState.Unchecked;
					chkUsrFiles.ThreeState = false;
				}
				else
				{
					chkUsrFiles.ThreeState = true;
					chkUsrFiles.CheckState = CheckState.Indeterminate;
				}
				chkUsrFiles.CheckStateChanged += ChkFilesAndOrFolders_CheckedStateChanged;
			}
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Returns an indication as to whether any folder of the specified name contains a checkmark when 
		/// 'isChecked' is true or does not contain a checkmark when 'isChecked' is false.
		/// </summary>
		/// <param name="nodes">The nodes.</param>
		/// <param name="folderName">Name of the folder.</param>
		/// <param name="isChecked">
		/// if set to <c>true</c> then a folder of the specified name containing a checkmark returns true.
		/// </param>
		/// <returns></returns>
		//----------------------------------------------------------------------------------------------------
		private static bool IsAnyFolderCheckmarked(IEnumerable nodes, string folderName, bool isChecked)
		{
			// Return false to indicate indeterminate.
			foreach (TreeNode node in nodes.Cast<TreeNode>().Where(tn => tn != null))
			{
				if (node.Text.Equals(folderName, StringComparison.InvariantCultureIgnoreCase)) // && node.ImageIndex == (int)FileType.Folder
				{
					if (isChecked && !node.Checked || !isChecked && node.Checked)
						return false;
				}
				if (IsAnyFolderCheckmarked(node.Nodes, folderName, isChecked) == false)
					return false;
			}
			return true;
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Finds a node in the tree by its full name.
		/// </summary>
		/// <param name="nodes">The nodes.</param>
		/// <param name="fullName">The full name.</param>
		/// <returns></returns>
		//----------------------------------------------------------------------------------------------------
		private static TreeNode FindNodeByFullName(IEnumerable nodes, string fullName)
		{
			if (nodes != null)
				foreach (TreeNode currentNode in nodes)
				{
					if (currentNode.Tag.ToString().Equals(fullName, StringComparison.InvariantCultureIgnoreCase))
						return currentNode;
					TreeNode nodeTmp = FindNodeByFullName(currentNode.Nodes, fullName);
					if (nodeTmp != null)
						return nodeTmp;
				}
			return null;
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Returns an indication as to whether any file ending with the specified extension contains a 
		/// checkmark when 'isChecked' is true or does not contain a checkmark when 'isChecked' is false.
		/// </summary>
		/// <param name="nodes">The nodes.</param>
		/// <param name="extension">extension of the file.</param>
		/// <param name="isChecked">
		/// if set to <c>true</c> then a file with the specified extension containing a checkmark returns 
		/// true.
		/// </param>
		/// <returns></returns>
		//----------------------------------------------------------------------------------------------------
		private static bool IsAnyFileCheckmarked(IEnumerable nodes, string extension, bool isChecked)
		{
			// Return false to indicate indeterminate.
			foreach (TreeNode node in nodes.Cast<TreeNode>().Where(tn => tn != null))
			{
				if (node.Text.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase)) // && node.ImageIndex == (int)FileType.File
				{
					if (isChecked && !node.Checked || !isChecked && node.Checked)
						return false;
				}
				if (IsAnyFileCheckmarked(node.Nodes, extension, isChecked) == false)
					return false;
			}
			return true;
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Delete nodes by traversing nodes recursively beginning at the top.
		/// </summary>
		/// <param name="nodes">The nodes.</param>
		//----------------------------------------------------------------------------------------------------
		private void DeleteNodesTopToBottom(IEnumerable nodes)
		{
			foreach (TreeNode tn in nodes.Cast<TreeNode>().Where(tn => tn != null))
			{
				if (tn.Checked)
					DeleteNode(tn);
				else if (tn.ImageIndex == (int)FileType.Folder) // Not checked but it's a folder.

					DeleteNodesTopToBottom(tn.Nodes);
			}
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Delete nodes by traversing nodes recursively beginning at the bottom.
		/// </summary>
		/// <param name="nodes">The nodes.</param>
		//----------------------------------------------------------------------------------------------------
		private void DeleteNodesBottomToTop(IEnumerable nodes)
		{
			foreach (TreeNode tn in nodes.Cast<TreeNode>().Where(tn => tn != null))
			{
				DeleteNodesBottomToTop(tn.Nodes);

				if (tn.Checked)
					DeleteNode(tn);
			}
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Deletes the specified node.
		/// </summary>
		/// <param name="tn">The tn.</param>
		//----------------------------------------------------------------------------------------------------
		private void DeleteNode(TreeNode tn)
		{
			string fullPath = tn.Tag.ToString();
			int imageIndex = tn.ImageIndex;
			string timeStamp = DateTime.Now.ToString(CultureInfo.InvariantCulture);
			switch (imageIndex)
			{
				case (int)FileType.Folder:
					{
						try
						{
							if (chkSendToRecycleBin.Checked)
							{
								int filesToBeDeleted = GetChildNodeCount(tn.Nodes);
								FileSystem.DeleteDirectory(fullPath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin, UICancelOption.ThrowException);
								_iFilesDeleted += filesToBeDeleted;
							}
							else
								Directory.Delete(fullPath);
							LogFile.Add(new LogData { TimeStamp = timeStamp, Type = "Folder", FullPath = fullPath, Status = DELETED });
							_iFoldersDeleted++;
						}
						catch (Exception ex)
						{
							LogFile.Add(new LogData { TimeStamp = timeStamp, Type = "Folder", FullPath = fullPath, Status = COULD_NOT_DELETE });
							if (ex is OperationCanceledException)
								throw;
						}
					}
					break;
				case (int)FileType.File:
					{
						try
						{
							if (chkSendToRecycleBin.Checked)
								FileSystem.DeleteFile(fullPath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin, UICancelOption.ThrowException);
							else
								File.Delete(fullPath);
							LogFile.Add(new LogData { TimeStamp = timeStamp, Type = "File", FullPath = fullPath, Status = DELETED });
							_iFilesDeleted++;
						}
						catch (Exception ex)
						{
							LogFile.Add(new LogData { TimeStamp = timeStamp, Type = "File", FullPath = fullPath, Status = COULD_NOT_DELETE });
							if (ex is OperationCanceledException)
								throw;
						}
					}
					break;
			}
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a count of all the child nodes of the specified nodes.
		/// </summary>
		/// <param name="nodes">The nodes.</param>
		/// <returns></returns>
		//----------------------------------------------------------------------------------------------------
		private static int GetChildNodeCount(IEnumerable nodes)
		{
			int count = 0;
			foreach (TreeNode tn in nodes.Cast<TreeNode>().Where(tn => tn != null))
			{
				count++;
				if (tn.Nodes.Count > 0)
					count += GetChildNodeCount(tn.Nodes);
			}
			return count;
		}

		private static void UncheckCheckboxes(Control parentCtrl)
		{
			foreach (Control ctrl in parentCtrl.Controls)
			{
				var cbx = ctrl as CheckBox;
				if (cbx != null)
					cbx.Checked = false;
				else if (ctrl.HasChildren)
					UncheckCheckboxes(ctrl);
			}
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Unchecks the parents.
		/// </summary>
		/// <param name="node">The node.</param>
		//----------------------------------------------------------------------------------------------------
		private static void UncheckParents(TreeNode node)
		{
			while (true)
			{
				if (node?.Parent == null || node.Checked)
					return;

				if (node.Parent.Checked)
				{
					node.Parent.Checked = false;
					UpdateTotal(node.Parent);
				}
				node = node.Parent;
			}
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Auto refreshes the tree.
		/// </summary>
		//----------------------------------------------------------------------------------------------------
		private void AutoRefreshTree()
		{
			if (!_isFilesChanged || !_isFormActivated) return;

			ReloadTree();
			MessageDialog.Show(this, @"One or more files were changed resulting in the tree view being refreshed automatically.", Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		protected override void OnClientSizeChanged(EventArgs e)
		{
			if (WindowState != _lastState)
			{
				_lastState = WindowState;
				OnWindowStateChanged(e);
			}
			base.OnClientSizeChanged(e);
		}

		protected void OnWindowStateChanged(EventArgs e)
		{
			if (_lastState == FormWindowState.Normal)
				AutoRefreshTree();
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the file size as Bytes, KB, MB, GB, etc. along with its corresponding suffix
		/// depending on the actual file size in bytes.
		/// </summary>
		/// <param name="sizeInBytes">Size of the file in bytes</param>
		/// <param name="decimalPlaces">Number of decimal places to display</param>
		/// <returns>String value indicating the size in bytes, kilobytes, megabytes, etc.</returns>
		//----------------------------------------------------------------------------------------------------
		public static string FormatFileSize(ulong sizeInBytes, uint decimalPlaces)
		{
			double d;
			string suffix;
			if (sizeInBytes < 1ul << 10)
				return sizeInBytes + " Bytes";                                                            // byte
			if (sizeInBytes < 1ul << 20) { d = sizeInBytes / (double)(1ul << 10); suffix = "KB"; }    // kilobyte
			else if (sizeInBytes < 1ul << 30) { d = sizeInBytes / (double)(1ul << 20); suffix = "MB"; } // megabyte
			else if (sizeInBytes < 1ul << 40) { d = sizeInBytes / (double)(1ul << 30); suffix = "GB"; } // gigabyte
			else if (sizeInBytes < 1ul << 50) { d = sizeInBytes / (double)(1ul << 40); suffix = "TB"; } // terabyte
			else if (sizeInBytes < 1ul << 60) { d = sizeInBytes / (double)(1ul << 50); suffix = "PB"; } // petabyte
			else { d = sizeInBytes / (double)(1ul << 60); suffix = "EB"; }                                // exabyte
			return string.Format($"{{0:F{decimalPlaces}}} {suffix}", d);
		}

		#endregion
	}

	public class LogData
	{
		public string TimeStamp { get; set; }
		public string @Type { get; set; }
		public string FullPath { get; set; }
		public string Status { get; set; }
	}

}