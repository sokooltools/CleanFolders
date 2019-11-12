
namespace SokoolTools.CleanFolders
{
	partial class MainForm
	{
		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Required designer variable.
		/// </summary>
		//----------------------------------------------------------------------------------------------------
		private System.ComponentModel.IContainer components = null;

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		//----------------------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		//.............................................................................................................

		#region Windows Form Designer generated code

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		//----------------------------------------------------------------------------------------------------
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuExpandAll = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCollapseAll = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuUncheckAll = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuProperties = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.chkBinFolders = new System.Windows.Forms.CheckBox();
			this.chkObjFolders = new System.Windows.Forms.CheckBox();
			this.chkSuoFiles = new System.Windows.Forms.CheckBox();
			this.chkUsrFiles = new System.Windows.Forms.CheckBox();
			this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
			this.grpAutoSelect = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.grpFolders = new System.Windows.Forms.GroupBox();
			this.txtMscFolders = new System.Windows.Forms.TextBox();
			this.chkMscFolders = new System.Windows.Forms.CheckBox();
			this.btnPicFolder = new System.Windows.Forms.Button();
			this.grpFiles = new System.Windows.Forms.GroupBox();
			this.txtMscFiles = new System.Windows.Forms.TextBox();
			this.chkMscFiles = new System.Windows.Forms.CheckBox();
			this.btnPicFile = new System.Windows.Forms.Button();
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.chkSendToRecycleBin = new System.Windows.Forms.CheckBox();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.btnLog = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.statusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.btnCollapseAll = new SokoolTools.CleanFolders.BorderlessButton();
			this.btnExpandAll = new SokoolTools.CleanFolders.BorderlessButton();
			this.txtFolderPath = new SokoolTools.CleanFolders.EllipsisTextBox();
			this.contextMenuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
			this.grpAutoSelect.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.grpFolders.SuspendLayout();
			this.grpFiles.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// treeView1
			// 
			this.treeView1.AllowDrop = true;
			this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.treeView1.CheckBoxes = true;
			this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
			this.treeView1.ForeColor = System.Drawing.Color.DarkSlateGray;
			this.treeView1.FullRowSelect = true;
			this.treeView1.Location = new System.Drawing.Point(12, 163);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(412, 309);
			this.treeView1.TabIndex = 1;
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.mnuDelete,
			this.toolStripSeparator4,
			this.mnuExpandAll,
			this.mnuCollapseAll,
			this.toolStripSeparator1,
			this.mnuUncheckAll,
			this.toolStripSeparator2,
			this.mnuOptions,
			this.toolStripSeparator5,
			this.mnuProperties,
			this.toolStripSeparator3,
			this.mnuAbout});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(182, 188);
			// 
			// mnuDelete
			// 
			this.mnuDelete.Name = "mnuDelete";
			this.mnuDelete.Size = new System.Drawing.Size(181, 22);
			this.mnuDelete.Text = "Delete Selected...";
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(178, 6);
			// 
			// mnuExpandAll
			// 
			this.mnuExpandAll.Image = global::SokoolTools.CleanFolders.Properties.Resources.expand_all;
			this.mnuExpandAll.Name = "mnuExpandAll";
			this.mnuExpandAll.Size = new System.Drawing.Size(181, 22);
			this.mnuExpandAll.Text = "Expand All";
			// 
			// mnuCollapseAll
			// 
			this.mnuCollapseAll.Image = global::SokoolTools.CleanFolders.Properties.Resources.collapse_all;
			this.mnuCollapseAll.Name = "mnuCollapseAll";
			this.mnuCollapseAll.Size = new System.Drawing.Size(181, 22);
			this.mnuCollapseAll.Text = "Collapse All";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(178, 6);
			// 
			// mnuUncheckAll
			// 
			this.mnuUncheckAll.Name = "mnuUncheckAll";
			this.mnuUncheckAll.Size = new System.Drawing.Size(181, 22);
			this.mnuUncheckAll.Text = "Uncheck All";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(178, 6);
			// 
			// mnuOptions
			// 
			this.mnuOptions.Name = "mnuOptions";
			this.mnuOptions.Size = new System.Drawing.Size(181, 22);
			this.mnuOptions.Text = "Options...";
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(178, 6);
			// 
			// mnuProperties
			// 
			this.mnuProperties.Name = "mnuProperties";
			this.mnuProperties.Size = new System.Drawing.Size(181, 22);
			this.mnuProperties.Text = "&Properties...";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(178, 6);
			// 
			// mnuAbout
			// 
			this.mnuAbout.Name = "mnuAbout";
			this.mnuAbout.Size = new System.Drawing.Size(181, 22);
			this.mnuAbout.Text = "About Clean Folders";
			// 
			// chkBinFolders
			// 
			this.chkBinFolders.AutoSize = true;
			this.chkBinFolders.Checked = true;
			this.chkBinFolders.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkBinFolders.Location = new System.Drawing.Point(57, 24);
			this.chkBinFolders.Name = "chkBinFolders";
			this.chkBinFolders.Size = new System.Drawing.Size(43, 17);
			this.chkBinFolders.TabIndex = 3;
			this.chkBinFolders.Text = "bin";
			this.toolTip1.SetToolTip(this.chkBinFolders, "When checkmarked, automatically selects all folders in the \r\ntree view having the" +
		" name \"bin\".");
			this.chkBinFolders.UseVisualStyleBackColor = true;
			// 
			// chkObjFolders
			// 
			this.chkObjFolders.AutoSize = true;
			this.chkObjFolders.Checked = true;
			this.chkObjFolders.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkObjFolders.Location = new System.Drawing.Point(57, 43);
			this.chkObjFolders.Name = "chkObjFolders";
			this.chkObjFolders.Size = new System.Drawing.Size(43, 17);
			this.chkObjFolders.TabIndex = 3;
			this.chkObjFolders.Text = "obj";
			this.toolTip1.SetToolTip(this.chkObjFolders, "When checkmarked, automatically selects all folders in the \r\ntree view having the" +
		" name \"obj\".");
			this.chkObjFolders.UseVisualStyleBackColor = true;
			// 
			// chkSuoFiles
			// 
			this.chkSuoFiles.AutoSize = true;
			this.chkSuoFiles.Checked = true;
			this.chkSuoFiles.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkSuoFiles.Location = new System.Drawing.Point(56, 24);
			this.chkSuoFiles.Name = "chkSuoFiles";
			this.chkSuoFiles.Size = new System.Drawing.Size(48, 17);
			this.chkSuoFiles.TabIndex = 3;
			this.chkSuoFiles.Text = ".suo";
			this.toolTip1.SetToolTip(this.chkSuoFiles, "When checkmarked, automatically selects all files in the tree \r\nview below having" +
		" the extension \".suo\".");
			this.chkSuoFiles.UseVisualStyleBackColor = true;
			// 
			// chkUsrFiles
			// 
			this.chkUsrFiles.AutoSize = true;
			this.chkUsrFiles.Checked = true;
			this.chkUsrFiles.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkUsrFiles.Location = new System.Drawing.Point(56, 43);
			this.chkUsrFiles.Name = "chkUsrFiles";
			this.chkUsrFiles.Size = new System.Drawing.Size(51, 17);
			this.chkUsrFiles.TabIndex = 3;
			this.chkUsrFiles.Text = ".user";
			this.toolTip1.SetToolTip(this.chkUsrFiles, "When checkmarked, automatically selects all files in the tree\r\nview below having " +
		"the extension \".user\".");
			this.chkUsrFiles.UseVisualStyleBackColor = true;
			// 
			// fileSystemWatcher1
			// 
			this.fileSystemWatcher1.EnableRaisingEvents = true;
			this.fileSystemWatcher1.IncludeSubdirectories = true;
			this.fileSystemWatcher1.SynchronizingObject = this;
			// 
			// grpAutoSelect
			// 
			this.grpAutoSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.grpAutoSelect.Controls.Add(this.tableLayoutPanel1);
			this.grpAutoSelect.Location = new System.Drawing.Point(13, 42);
			this.grpAutoSelect.Name = "grpAutoSelect";
			this.grpAutoSelect.Size = new System.Drawing.Size(411, 98);
			this.grpAutoSelect.TabIndex = 5;
			this.grpAutoSelect.TabStop = false;
			this.grpAutoSelect.Text = "Auto-Select";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.grpFolders, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.grpFiles, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 18);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(405, 77);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// grpFolders
			// 
			this.grpFolders.Controls.Add(this.txtMscFolders);
			this.grpFolders.Controls.Add(this.chkMscFolders);
			this.grpFolders.Controls.Add(this.chkObjFolders);
			this.grpFolders.Controls.Add(this.chkBinFolders);
			this.grpFolders.Controls.Add(this.btnPicFolder);
			this.grpFolders.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpFolders.Location = new System.Drawing.Point(5, 5);
			this.grpFolders.Margin = new System.Windows.Forms.Padding(5);
			this.grpFolders.Name = "grpFolders";
			this.grpFolders.Size = new System.Drawing.Size(192, 67);
			this.grpFolders.TabIndex = 5;
			this.grpFolders.TabStop = false;
			this.grpFolders.Text = "Folder Names";
			this.toolTip1.SetToolTip(this.grpFolders, "Contains folder names that will be automatically selected in the tree view.");
			// 
			// txtMscFolders
			// 
			this.txtMscFolders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.txtMscFolders.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtMscFolders.Location = new System.Drawing.Point(131, 21);
			this.txtMscFolders.Name = "txtMscFolders";
			this.txtMscFolders.Size = new System.Drawing.Size(40, 22);
			this.txtMscFolders.TabIndex = 6;
			this.toolTip1.SetToolTip(this.txtMscFolders, "Enter a \"comma\" or \"semicolon-delimited\" list of folder \r\nnames to be automatical" +
		"ly selected in the tree view.");
			// 
			// chkMscFolders
			// 
			this.chkMscFolders.AutoSize = true;
			this.chkMscFolders.Enabled = false;
			this.chkMscFolders.Location = new System.Drawing.Point(111, 24);
			this.chkMscFolders.Name = "chkMscFolders";
			this.chkMscFolders.Size = new System.Drawing.Size(15, 14);
			this.chkMscFolders.TabIndex = 3;
			this.toolTip1.SetToolTip(this.chkMscFolders, "When checkmarked, automatically selects all files in the tree view\r\nhaving the na" +
		"me(s) defined in the adjacent field.");
			this.chkMscFolders.UseVisualStyleBackColor = true;
			// 
			// btnPicFolder
			// 
			this.btnPicFolder.BackColor = System.Drawing.Color.Transparent;
			this.btnPicFolder.FlatAppearance.BorderSize = 0;
			this.btnPicFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnPicFolder.Image = global::SokoolTools.CleanFolders.Properties.Resources.Folder;
			this.btnPicFolder.Location = new System.Drawing.Point(5, 18);
			this.btnPicFolder.Name = "btnPicFolder";
			this.btnPicFolder.Size = new System.Drawing.Size(48, 48);
			this.btnPicFolder.TabIndex = 4;
			this.btnPicFolder.TabStop = false;
			this.btnPicFolder.UseVisualStyleBackColor = false;
			// 
			// grpFiles
			// 
			this.grpFiles.Controls.Add(this.txtMscFiles);
			this.grpFiles.Controls.Add(this.chkMscFiles);
			this.grpFiles.Controls.Add(this.chkSuoFiles);
			this.grpFiles.Controls.Add(this.chkUsrFiles);
			this.grpFiles.Controls.Add(this.btnPicFile);
			this.grpFiles.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpFiles.Location = new System.Drawing.Point(207, 5);
			this.grpFiles.Margin = new System.Windows.Forms.Padding(5);
			this.grpFiles.Name = "grpFiles";
			this.grpFiles.Size = new System.Drawing.Size(193, 67);
			this.grpFiles.TabIndex = 6;
			this.grpFiles.TabStop = false;
			this.grpFiles.Text = "File Extensions";
			this.toolTip1.SetToolTip(this.grpFiles, "Contains file extensions that will be automatically selected in the tree view.");
			// 
			// txtMscFiles
			// 
			this.txtMscFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.txtMscFiles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtMscFiles.Location = new System.Drawing.Point(137, 20);
			this.txtMscFiles.Name = "txtMscFiles";
			this.txtMscFiles.Size = new System.Drawing.Size(40, 22);
			this.txtMscFiles.TabIndex = 5;
			this.toolTip1.SetToolTip(this.txtMscFiles, resources.GetString("txtMscFiles.ToolTip"));
			// 
			// chkMscFiles
			// 
			this.chkMscFiles.AutoSize = true;
			this.chkMscFiles.Enabled = false;
			this.chkMscFiles.Location = new System.Drawing.Point(117, 24);
			this.chkMscFiles.Name = "chkMscFiles";
			this.chkMscFiles.Size = new System.Drawing.Size(15, 14);
			this.chkMscFiles.TabIndex = 3;
			this.toolTip1.SetToolTip(this.chkMscFiles, "When checkmarked, automatically selects all files in the tree view\r\nhaving the ex" +
		"tension(s) defined in the adjacent field.");
			this.chkMscFiles.UseVisualStyleBackColor = true;
			// 
			// btnPicFile
			// 
			this.btnPicFile.BackColor = System.Drawing.Color.Transparent;
			this.btnPicFile.FlatAppearance.BorderSize = 0;
			this.btnPicFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnPicFile.Image = global::SokoolTools.CleanFolders.Properties.Resources.File;
			this.btnPicFile.Location = new System.Drawing.Point(6, 18);
			this.btnPicFile.Name = "btnPicFile";
			this.btnPicFile.Size = new System.Drawing.Size(48, 48);
			this.btnPicFile.TabIndex = 4;
			this.btnPicFile.TabStop = false;
			this.btnPicFile.UseVisualStyleBackColor = false;
			// 
			// backgroundWorker1
			// 
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			// 
			// chkSendToRecycleBin
			// 
			this.chkSendToRecycleBin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.chkSendToRecycleBin.AutoSize = true;
			this.chkSendToRecycleBin.Checked = true;
			this.chkSendToRecycleBin.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkSendToRecycleBin.Location = new System.Drawing.Point(126, 492);
			this.chkSendToRecycleBin.Name = "chkSendToRecycleBin";
			this.chkSendToRecycleBin.Size = new System.Drawing.Size(126, 17);
			this.chkSendToRecycleBin.TabIndex = 6;
			this.chkSendToRecycleBin.Text = "Send to Recycle Bin";
			this.toolTip1.SetToolTip(this.chkSendToRecycleBin, "When checkmarked, deleted files and folders will be sent to\r\nthe recycle bin as o" +
		"pposed to being permanently deleted.");
			this.chkSendToRecycleBin.UseVisualStyleBackColor = true;
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(341, 481);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(83, 30);
			this.btnClose.TabIndex = 0;
			this.btnClose.Text = "&Close";
			this.toolTip1.SetToolTip(this.btnClose, "Click to close this dialog.");
			this.btnClose.UseVisualStyleBackColor = true;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowse.FlatAppearance.BorderSize = 0;
			this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnBrowse.Image = global::SokoolTools.CleanFolders.Properties.Resources.Open;
			this.btnBrowse.Location = new System.Drawing.Point(403, 13);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(25, 23);
			this.btnBrowse.TabIndex = 4;
			this.toolTip1.SetToolTip(this.btnBrowse, "Click to select a folder to clean.");
			// 
			// btnLog
			// 
			this.btnLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnLog.Enabled = false;
			this.btnLog.Image = global::SokoolTools.CleanFolders.Properties.Resources.Log;
			this.btnLog.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnLog.Location = new System.Drawing.Point(13, 481);
			this.btnLog.Name = "btnLog";
			this.btnLog.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
			this.btnLog.Size = new System.Drawing.Size(99, 30);
			this.btnLog.TabIndex = 0;
			this.btnLog.Text = "&Show Log...";
			this.btnLog.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolTip1.SetToolTip(this.btnLog, "Click to show the log of files and folders which have been deleted.");
			this.btnLog.UseVisualStyleBackColor = true;
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDelete.Enabled = false;
			this.btnDelete.Image = global::SokoolTools.CleanFolders.Properties.Resources.Delete;
			this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnDelete.Location = new System.Drawing.Point(255, 481);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
			this.btnDelete.Size = new System.Drawing.Size(80, 30);
			this.btnDelete.TabIndex = 0;
			this.btnDelete.Text = "&Delete...";
			this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolTip1.SetToolTip(this.btnDelete, "Click to delete the selected files and folders.");
			this.btnDelete.UseVisualStyleBackColor = true;
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.statusLabel1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 520);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(436, 22);
			this.statusStrip1.TabIndex = 7;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// statusLabel1
			// 
			this.statusLabel1.Name = "statusLabel1";
			this.statusLabel1.Size = new System.Drawing.Size(0, 17);
			// 
			// timer1
			// 
			this.timer1.Interval = 1200;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// btnCollapseAll
			// 
			this.btnCollapseAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCollapseAll.Image = global::SokoolTools.CleanFolders.Properties.Resources.collapse_all;
			this.btnCollapseAll.Location = new System.Drawing.Point(34, 143);
			this.btnCollapseAll.Name = "btnCollapseAll";
			this.btnCollapseAll.Size = new System.Drawing.Size(17, 17);
			this.btnCollapseAll.TabIndex = 8;
			this.btnCollapseAll.UseVisualStyleBackColor = false;
			this.btnCollapseAll.Click += new System.EventHandler(this.btnCollapseAll_Click);
			// 
			// btnExpandAll
			// 
			this.btnExpandAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnExpandAll.Image = global::SokoolTools.CleanFolders.Properties.Resources.expand_all;
			this.btnExpandAll.Location = new System.Drawing.Point(12, 143);
			this.btnExpandAll.Name = "btnExpandAll";
			this.btnExpandAll.Size = new System.Drawing.Size(17, 17);
			this.btnExpandAll.TabIndex = 8;
			this.toolTip1.SetToolTip(this.btnExpandAll, "Expand All");
			this.btnExpandAll.UseVisualStyleBackColor = false;
			this.btnExpandAll.Click += new System.EventHandler(this.btnExpandAll_Click);
			// 
			// txtFolderPath
			// 
			this.txtFolderPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFolderPath.Location = new System.Drawing.Point(13, 13);
			this.txtFolderPath.Name = "txtFolderPath";
			this.txtFolderPath.Size = new System.Drawing.Size(389, 22);
			this.txtFolderPath.TabIndex = 2;
			// 
			// MainForm
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(436, 542);
			this.Controls.Add(this.btnCollapseAll);
			this.Controls.Add(this.btnExpandAll);
			this.Controls.Add(this.chkSendToRecycleBin);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.txtFolderPath);
			this.Controls.Add(this.treeView1);
			this.Controls.Add(this.btnLog);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.grpAutoSelect);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.HelpButton = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(452, 343);
			this.Name = "MainForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Clean Folders";
			this.TopMost = true;
			this.contextMenuStrip1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
			this.grpAutoSelect.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.grpFolders.ResumeLayout(false);
			this.grpFolders.PerformLayout();
			this.grpFiles.ResumeLayout(false);
			this.grpFiles.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private EllipsisTextBox txtFolderPath;
		private System.ComponentModel.BackgroundWorker backgroundWorker1;
		private System.IO.FileSystemWatcher fileSystemWatcher1;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnLog;
		private System.Windows.Forms.Button btnPicFile;
		private System.Windows.Forms.Button btnPicFolder;
		private System.Windows.Forms.CheckBox chkBinFolders;
		private System.Windows.Forms.CheckBox chkMscFiles;
		private System.Windows.Forms.CheckBox chkMscFolders;
		private System.Windows.Forms.CheckBox chkObjFolders;
		private System.Windows.Forms.CheckBox chkSendToRecycleBin;
		private System.Windows.Forms.CheckBox chkSuoFiles;
		private System.Windows.Forms.CheckBox chkUsrFiles;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.GroupBox grpAutoSelect;
		private System.Windows.Forms.GroupBox grpFiles;
		private System.Windows.Forms.GroupBox grpFolders;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TextBox txtMscFiles;
		private System.Windows.Forms.TextBox txtMscFolders;
		private System.Windows.Forms.ToolStripMenuItem mnuAbout;
		private System.Windows.Forms.ToolStripMenuItem mnuCollapseAll;
		private System.Windows.Forms.ToolStripMenuItem mnuDelete;
		private System.Windows.Forms.ToolStripMenuItem mnuExpandAll;
		private System.Windows.Forms.ToolStripMenuItem mnuOptions;
		private System.Windows.Forms.ToolStripMenuItem mnuUncheckAll;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripStatusLabel statusLabel1;
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.ToolStripMenuItem mnuProperties;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Timer timer1;
		private BorderlessButton btnExpandAll;
		private BorderlessButton btnCollapseAll;
	}
}

