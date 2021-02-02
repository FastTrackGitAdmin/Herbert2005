using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Designer
{
	/// <summary>
	/// Summary description for MyPicBox.
	/// </summary>
	public class MyPicBox : System.Windows.Forms.Control
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Image image;


		public Image Image
		{
			get
			{
				return image;
			}
			set
			{
				image = value;
			}
		}


		public MyPicBox()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitComponent call
			this.SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(System.Windows.Forms.ControlStyles.DoubleBuffer, true);
			this.SetStyle(System.Windows.Forms.ControlStyles.UserPaint, true);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
		}
		#endregion

		protected override void OnPaint(PaintEventArgs pe)
		{
			//Console.WriteLine(", mypic, 12");
			// TODO: Add custom paint code here
			pe.Graphics.DrawImageUnscaled(image, 0, 0, image.Width, image.Height); 

			// Calling the base class OnPaint
			base.OnPaint(pe);
		}
	}
}
