using System;
//using System.Drawing;
//using System.Windows.Forms;


namespace Designer
{
	/// <summary>
	/// Summary description for cHerbertBoard.
	/// </summary>
	public class cHerbertBoard:	System.Windows.Forms.Panel
	{
		public cHerbertBoard():base()
		{
			//
			// todo: add constructor logic here
			//
			this.SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(System.Windows.Forms.ControlStyles.DoubleBuffer, true);
			this.SetStyle(System.Windows.Forms.ControlStyles.UserPaint, true);
			//this.SetStyle(System.Windows.Forms.ControlStyles.Opaque,true);			
		}		
	}
}
