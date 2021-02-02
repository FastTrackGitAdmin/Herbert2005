#define CONTEST

using System;

namespace Designer
{
#if(CONTEST)
	/// <summary>
	/// Summary description for HPathStore.
	/// </summary>
	public class HPathStore:IDisposable
	{
		/// <summary>
		/// arr to store path traversed by herbert.
		/// </summary>
		private short[] arrHPath;
		
		/// <summary>
		/// constructor.
		/// </summary>
		public HPathStore()
		{
			//
			// TODO: Add constructor logic here
			//
			arrHPath = new short[HConstants.GRIDDOTS];
		}

		/// <summary>
		/// saves the path traversed by herbert in the data structure.
		/// </summary>
		/// <param name="PerDestPosX">Current X co ordinate of the herbert</param>
		/// <param name="PerDestPosY">Current Y co ordinate of the herbert</param>
		/// <param name="CurDir">Current dir of herbert in which it is moving.</param>
		public void fSavePath(int PerDestPosX, int PerDestPosY, short HPLEFTorDOWN)
		{		
			int X = PerDestPosX/HConstants.DOTSPACE;
			int Y = PerDestPosY/HConstants.DOTSPACE;			
			arrHPath[X*25+Y] |= (short)HPLEFTorDOWN;
		}
	
		/// <summary>
		/// remove the wrong path traversed by herbert in the data structure.
		/// </summary>
		/// <param name="PerDestPosX">Current X co ordinate of the herbert</param>
		/// <param name="PerDestPosY">Current Y co ordinate of the herbert</param>
		/// <param name="CurDir">Current dir of herbert in which it is moving.</param>
		public void fRemovePath(int PerDestPosX, int PerDestPosY, short HPLEFTorDOWN)
		{			
			int X = PerDestPosX/HConstants.DOTSPACE;
			int Y = PerDestPosY/HConstants.DOTSPACE;			
			
			/*==============================================================================
			 *  Added By : Vivek Balagangadharan
			 *  Description : The check was added to fix the issue of index out of range exception
			 *				  in PT. Issue Id:3602	
			 *  Added On : 28-Mar-2006
			 * ==============================================================================*/
			if((X*25+Y) < HConstants.GRIDDOTS)
			{
				arrHPath[X*25+Y] &= (short)~HPLEFTorDOWN;
			}
		}
	
		/// <summary>
		/// dispose implementation.
		/// </summary>
		public void Dispose()
		{
			arrHPath = null;			
		}

		public short[] fGetSavedPath()
		{
			return arrHPath;
		}

		public bool isPathAvailable(int iPosX, int iPosY, short HPLEFTorDOWN)
		{
			int X = iPosX/HConstants.DOTSPACE;
			int Y = iPosY/HConstants.DOTSPACE;
			if((arrHPath[X*25+Y] & HPLEFTorDOWN) > 0)
				return true;
			else
				return false;
		}
	}
#endif
}
