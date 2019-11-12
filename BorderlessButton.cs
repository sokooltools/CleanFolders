using System.Drawing;
using System.Windows.Forms;

namespace SokoolTools.CleanFolders
{
	public class BorderlessButton : Button
	{
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			// Draw a border using a pen 5 pixels wide having the same color as the background.
			e.Graphics.DrawRectangle(new Pen(BackColor, 5), ClientRectangle);
		}
	}
}