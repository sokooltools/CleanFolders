using System;
using System.Windows.Forms;

namespace SokoolTools.CleanFolders
{
	public partial class LogFileViewer : Form
	{
		public LogFileViewer()
		{
			InitializeComponent();
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the log data.
		/// </summary>
		/// <value>The log data.</value>
		//----------------------------------------------------------------------------------------------------
		public object LogData
		{
			get => dataGridView1.DataSource;
			set => dataGridView1.DataSource = value;
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the Load event of the LogFileViewer control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
		//----------------------------------------------------------------------------------------------------
		private void LogFileViewer_Load(object sender, EventArgs e)
		{
			dataGridView1.AutoResizeColumns();
			dataGridView1.Columns[2].Width = 540;
			dataGridView1.RowHeadersWidth = 25;
		}

		private void LogFileViewer_FormClosing(object sender, FormClosingEventArgs e)
		{
			//Console.WriteLine(dataGridView1.Columns[1].Width);		
		}
	}
}
