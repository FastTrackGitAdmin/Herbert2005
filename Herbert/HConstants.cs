using System;

namespace Designer
{
	/// <summary>
	/// Summary description for HConstants.
	/// </summary>
	struct HConstants
	{
		/// <summary>
		/// space(number of pixel) between 2 small dots in the ZOOMOUT mode.
		/// </summary>
		public const int DOTSPACE = 8;
		
		/// <summary>
		/// number of small dots in the herbert board
		/// </summary>
		public const int GRIDDOTSPERLINE = 25;

		/// <summary>
		/// total number of small dots in herbert board.
		/// </summary>
		public const int GRIDDOTS = GRIDDOTSPERLINE*GRIDDOTSPERLINE;

		/// <summary>
		/// large mode of herbert.
		/// </summary>
		public const int ZOOMIN = 2;

		/// <summary>
		/// small mode of herbert.
		/// </summary>
		public const int ZOOMOUT = 1;
		
		/// <summary>
		/// these contants are defined for different states path of herbert.
		/// HPLEFT is used for showing that a path is drawn from a givin point to left of the point.
		/// HPLEFT is same to curDir = 0.		
		/// </summary>
		public const short HPLEFT = 0x01;

		/// <summary>
		/// these contants are defined for different states path of herbert.
		/// HPDOWN is used for showing that a path is drawn from a givin point to down of the point.
		/// HPDOWN is same to curDir = 3.
		/// </summary>
		public const short HPDOWN = 0x02;
	}
}
