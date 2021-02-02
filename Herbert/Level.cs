#define EXCEPTION
//For Herbert
#define CONTEST
#define RUNSTATE
//end
//For Designer
//#define DESIGNER
//#define PREDICTER
//#define PATTERN_GENERATER
//end

using System;
using System.Drawing;
using System.Data;
using System.Threading;
using System.Collections;
using System.Globalization;

namespace Designer
{
	/// <summary>
	/// Summary description for Levels.
	/// </summary>
	public class Level:Designer.BaseLevel

	{
        
		# region Constants
        /// <summary>
        /// Stack size which is used to store call of herbert functions.
        /// </summary>
        public const int STACKSIZE = 1024 * 16; /* stacksize can be no more than 1024*16 */
		/// <summary>
		/// constants defined for checking the range in which herbert and hurdle both are present.
		/// </summary>
		private int RNG1 = 12;
		/// <summary>
		/// Radius of small dot generated on with. this is different form dotPoints. 
		/// </summary> 
		public const float DOTRADIOUS = 1.6F;

		/// <summary>
		/// points defined for each button in old scoring logic.
		/// </summary>
		private const int POINTSPERBTTN = 5;

		/// <summary>
		/// offset for drawing dots.
		/// 		/// </summary>
		private const int DOTOFFSET = 6;
		# endregion

		# region static members

		/// <summary>
		/// 
		/// </summary>
		//internal volatile object progClosing = true;

		/// <summary>
		/// keeps track of the time when go said.
		/// </summary>
		//public static int temp_levelTimer;
		public int WhiteButtonCount = 0;
		/// <summary>
		/// this array contains valid level no.
		/// </summary>
		public static int[] arrValidLevelNo;
		//public static string LevelProgram;
		/// <summary>
		/// this array contains valid LevelId.
		/// </summary>
		public static int[] arrLevelId;
#if(DESIGNER)
		/// <summary>
		/// dots(buttons or target) create at designer mode.
		/// 		/// </summary>
		private static Point[] newLevelDots;

		/// <summary>
		/// points where new hurdle needs to be drawn in designer mode.
		/// </summary>
		private static Point[,] newLevelHerdls;
        /*
        /// <summary>
        /// Added by rajesh
        /// To store all unsaved dots with there index
        /// </summary>
        private static Hashtable nonSavedLevelDots;
        /// <summary>
        /// Added by rajesh
        /// To store all unsaved Herdls with there index
        /// </summary>
        private static Hashtable nonSavedLevelHerdls;
        /// <summary>
        /// Added by rajesh
        /// To store Gray info for the level
        /// </summary>
        private static Hashtable nonSavedLevelGray;
        */

		/// <summary>
		/// array to keep track of any tragets grey or not status
		/// </summary>
		private static int[] grey = new int[TOTALSMLDOT];

		/// <summary>
		/// <p>iRoboX: x position of herbert at new level</p>
		/// <p>iRoboY: y position of herbert at new level</p>
		/// <p>iMaxBytes: max bytes allowed for this new level</p>
		/// </summary>
		/// 
		private static int iRoboX = 0, iRoboY = 0, iMaxBytes = 0; 

		/// <summary>
		/// <p>HerbertX: x position of herbert at a level</p>
		/// <p>HerbertY: y position of herbert at a level</p>
		/// </summary>
		/// 
		private int iHerbertX = 0, iHerbertY = 0; 
		public int HerbertX
		{
			set
			{
				iHerbertX = value;
				iRoboX = iHerbertX;
			}
			get
			{
				return iHerbertX;
			}
		}

		public int HerbertY
		{
			set
			{
				iHerbertY = value;
				iRoboY = iHerbertY;
			}
			get
			{
				return iHerbertY;
			}
		}
#endif
		/*Build All Level Data */
		/// <summary>
		/// dataset which contains data for all the levels
		/// </summary>
		//static DataSet dset = null;

		/// <summary>
		/// interger array to store level points returned from database.
		/// </summary> 
		private static int[] arrLvlPoints;

		private static string[] arrLvlProgram;		

		/// <summary>
		/// variable to hold total score of the game.
		/// </summary>
		internal static int totalScore = 0;

		/// <summary>
		/// This point contains the initial position of herbert robo.
		/// </summary>
		public static Point IniHerbtPos = new Point(0,0);

		/// <summary>
		/// number of small dots on each board. this is different from dot Points. dot points are target dot which are empty.
		/// </summary> 
		public static int numSmallDots;

		public bool IsNewLevelDotsPopulated = false;



		/// <summary>
		/// distance bttn each small dot.
		/// </summary>
		//public static int dotDist;
		/// <summary>
		/// variable contains the scoring logic to be used 1 is for new 0 is for old.
		/// </summary>
		internal static int scoringLogic;
		//		/// <summary>
		//		/// Static variable contains the level Number currently active, and is used in property CurrentLevel.
		//		/// </summary>
		//		private static int cLevel;

		# endregion

//        # region Instance members



#if(CONTEST)
        /// <summary>
        /// object variable to hold object of hpathStore which keeps track of path traversed.
        /// </summary>
        private HPathStore objHPath = null;
#endif
#if(PATTERN_GENERATER)
        /// <summary>
        /// object variable to hold object of hpathStore which keeps track of path traversed.
        /// </summary>
        private HPathStore objHPath = null;
#endif      
        /// <summary>
        /// bool varible to keep track of all level Statistics whether they are changed or not.
        /// </summary>
        internal bool isLevelStatisticsChanged = false;

        /// <summary>
        /// bool varible to keep track of all level data whether they are changed or not.
        /// </summary>
        internal bool isLevelDataChanged = false;

        /// <summary>
        /// maximum number of charters defined for given level.
        /// </summary>
        internal int MaxChars = -1;

        /// <summary>
        /// this variable is used to tell whether the message of congratulation for solving the level is shown
        /// or not.
        /// </summary>
        internal bool blnLevelSolvedMsgShown = false;

        /// <summary>
        /// Total bytes used to solve the level for the first time.
        /// </summary>
        internal int iFirstBytes = 0;

        /// <summary>
        /// Program executed to solve the level for the first time.
        /// </summary>
        internal string iFirstProgram = "";

        /// <summary>
        /// Total Go attempts before the first time level was solved.
        /// </summary>
        internal int iFirstGos = 0;

        /// <summary>
        /// Total time spent before the first time level was solved.
        /// </summary>
        internal int iFirstTimeSpent = 0;

        /// <summary>
        /// Total number of characters entered through Keyboard/toolbar buttons but not 
        /// <br></br>through Copy/paste to write a solution which solved the level for the first time.
        /// </summary>
        internal int iFirstCharsTyped = 0;

        /// <summary>
        /// Score acheived the first time level was solved.
        /// </summary>
        internal int iFirstLevelScore = 0;

        /// <summary>
        /// Date when the level was solved for the first time.
        /// </summary>
        internal DateTime iFirstDate = DateTime.MinValue;

        /// <summary>
        /// Date when the best solution was acheived.
        /// </summary>
        internal DateTime iBestDate = DateTime.MinValue;

        /// <summary>
        /// Total number of time a level is visited and atleast for a duration of 1 minute till the level is solved for the first time.
        /// </summary>
        internal int iFirstVisits = 0;

        /// <summary>
        /// Total number of time a level is visited and atleast for a duration of 1 minute till the level is solved with best solution.
        /// </summary>
        internal int iBestVisits = 0;

        /// <summary>
        /// Total number of time a level is visited and atleast for a duration of 1 minute till the last Go was attempted.
        /// </summary>
        internal int iLastVisits = 0;

        /// <summary>
        /// Date when the last Go was attempted.
        /// </summary>
        internal DateTime iLastDate = DateTime.MinValue;

        /// <summary>
        /// Program in the code editor when the last Go was attempted.
        /// </summary>
        internal string iLastProgram = "";

        /// <summary>
        /// Total bytes in use when the last Go was attempted.
        /// </summary>
        internal int iLastBytes = 0;

        /// <summary>
        /// Total Gos attempted so far till the last Go was attempted. The total Gos also includes the last Go attempt.
        /// </summary>
        internal int iLastGos = 0;

        /// <summary>
        /// Time spent so far till the last Go was attempted.
        /// </summary>
        internal int iLastTimeSpent = 0;

        /// <summary>
        /// Total characters entered through keyboard/toolbar till the last Go was attempted.
        /// </summary>
        internal int iLastCharsTyped = 0;

        /// <summary>
        /// Total number of characters entered so far through Keyboard/toolbar buttons but not through Copy/paste.
        /// </summary>
        internal int iTotalCharsTyped = 0;

        /// <summary>
        /// Date when the level was solved for the first time.
        /// </summary>	
        internal DateTime FirstSolutionDT = DateTime.MinValue;

        /// <summary>
        /// Date the first character was entered through keyboard/toolbar buttons but not through copy/paste.
        /// </summary>
        internal DateTime FirstCharTypedDT = DateTime.MinValue;
        /// <summary>
        /// Date when the best solution was acheived.
        /// </summary>
        internal DateTime bBestDate = DateTime.MinValue;

        /// <summary>
        /// error factor to display white or grey buttons.
        /// </summary>
        private int BUTTONOFFSET = 2;
        //internal int leastCharsUsed;

        /// <summary>
        /// Total bytes used for writing the best solution.
        /// </summary>
        internal int iBestBytes;

        /// <summary>
        /// Total number of characters entered through Keyboard/toolbar buttons but not through Copy/paste till the best solution is achieved.
        /// </summary>
        internal int iBestCharsTyped = 0;

        /// <summary>
        /// variable which keeps the range to check whether a white or grey buttons or a wall is hit by herbert.
        /// this is used for checking for a wall or button nearby.
        /// </summary>
        private int DyRNG;

        /// <summary>
        /// Unique key from the database for each level score.
        /// </summary>
        public int LevelScoreId = 0;

        /// <summary>
        /// persistant best level score.
        /// </summary>
        internal int BestLevelScore = 0;

        /// <summary>
        /// this variable keeps track of number of times this level is visited.
        /// this variable is incremented by one if user spends more than 2 seconds
        /// on this levels.
        /// </summary>
        internal int pNumVisits = 0;

        /// <summary>
        /// private variable used to record number of times go button is pressed of herbert for a perticular level. 
        /// </summary>
        internal int iBestGos = 0;

        /// <summary>
        /// total number of small dots on the herbert board.
        /// </summary>
        private const int TOTALSMLDOT = 625;

        /// <summary>
        /// Per button points 
        /// </summary>
        private int PerBttnPtns = 0;

        /// <summary>
        /// Number of char used in this level counted from the program written by user.
        /// </summary>
        internal int charused = 0;

        /// <summary>
        /// variable to store previous points to draw path
        /// </summary>
        public int prePathPosX, prePathPosY;

        /// <summary>
        /// Current Score of this Level. this score is incremented at each dot is achived.
        /// </summary>
        internal int scoreCurrently = 0;
#if(PATTERN_GENERATER)
        #region PATTERN_GENERATER
        ///
        public bool isPatternEnabled = false;

        public bool IsPatternEnabled
        {
            set
            {
                isPatternEnabled = value;
            }
            get
            {
                return isPatternEnabled;
            }
        }

        /// <summary>
        /// To maintain the Pattern image.
        /// </summary>
        public bool isValidPatternImage = true;

        public bool IsValidPatternImage
        {
            set
            {
                isValidPatternImage = value;
            }
            get
            {
                return isValidPatternImage;
            }
        }
        #endregion
#endif
        /// <summary>
        /// bool variable to find whether this level is finished or not.
        /// </summary>
        public bool IsLevelFinished = false;

        /// <summary>
        /// bool variable to find whether this level is finished or not.
        /// </summary>
        public bool IsLevelFinishedPersistant = false;

        public bool blnLevelFinishedMsg = false;

        /// <summary>
        /// check to see if level is updated.
        /// </summary>
        public bool IsLevelUpdate = false;

        /// <summary>
        /// check to see if level display is updated in case of zoom-in or zoom-out.
        /// </summary>
        public bool IsLevelDisplayUpdated = false;

        /// <summary>
        /// Therad used for saving level data on level change.
        /// </summary>
        public Thread tSaveLevel = null;



#if(DESIGNER)
        public string LevelType = "";
        /// <summary>
        /// to check for is level is changed or not.
        /// </summary>
        public bool IsLevelChanged = false;

        /// <summary>
        /// to check if the level solution has been run.
        /// </summary>
        private bool isLevelRun = false;


        public bool IsLevelRun
        {
            set
            {
                isLevelRun = value;
                if (value == false)
                {
                    isLevelSolutionValid = false;
                    ResetDesignerStats();
                }
            }
            get
            {
                return isLevelRun;
            }
        }

        /// <summary>
        /// to check if the level solution actually solves the level.
        /// </summary>
        public bool isLevelSolutionValid = false;
        /// <summary>
        /// This is the maximum length of the stack for a valid solution which solves the level.
        /// </summary>
        private int maxStackLength = 16384;

        public int MaxStackLength
        {
            set
            {

                maxStackLength = value;

            }
            get
            {
                return maxStackLength;
            }
        }
        /// <summary>
        /// To calculate the max no of white button pressesed before gray button get pressed.
        /// </summary>
        private int numWDotPressedBeforeGDot = 0;

        public int NumWDotPressedBeforeGDot
        {
            set
            {
                numWDotPressedBeforeGDot = value;
            }
            get
            {
                return numWDotPressedBeforeGDot;
            }
        }


        /// <summary>
        /// Number of steps(interpreter steps) required to solve the level with a valid solution.
        /// </summary>
        private int numProgramSteps = 0;

        public int NumProgramSteps
        {
            set
            {
                numProgramSteps = value;
            }
            get
            {
                return numProgramSteps;
            }
        }

        /// <summary>
        /// Number of actions(r,s,l) required to solve the level with a valid solution.
        /// </summary>
        private int numHerbertActions = 0;

        public int NumHerbertActions
        {
            set
            {
                numHerbertActions = value;
            }
            get
            {
                return numHerbertActions;
            }
        }

        /// <summary>
        /// Number of gray buttons hit before solving the level with a valid solution.
        /// </summary>
        private int numGrayButtonHits = 0;

        public int NumGrayButtonHits
        {
            set
            {
                numGrayButtonHits = value;
            }
            get
            {
                return numGrayButtonHits;
            }
        }

        /// <summary>
        /// Number of times Herbert hit walls, before solving the level with a valid solution.
        /// </summary>
        private int numWallHits = 0;

        public int NumWallHits
        {
            set
            {
                numWallHits = value;
            }
            get
            {
                return numWallHits;
            }
        }

        // added by Nikhil Kardale on 03/04/2008
        // for use while solving level completely for the first time 
        // i.e. to avoid counting for Herbert moves after level is fully solved
        private int numHerbertActionsPersistent = 0;

        public int NumHerbertActionsPersistent
        {
            set
            {
                numHerbertActionsPersistent = value;
            }
            get
            {
                return numHerbertActionsPersistent;
            }
        }

        private int numProgramStepsPersistent = 0;

        public int NumProgramStepsPersistent
        {
            set
            {
                numProgramStepsPersistent = value;
            }
            get
            {
                return numProgramStepsPersistent;
            }
        }
        
        private int maxStackLengthPersistent = 0;

        public int MaxStackLengthPersistent
        {
            set
            {
                maxStackLengthPersistent = value;
            }
            get
            {
                return maxStackLengthPersistent;
            }
        }

        // end
        
        /*
        
//         //By Rajesh,21/8/06,3885
//         * */

#endif

        /// <summary>
        /// 
        /// </summary>
        public bool IsClearAllChecked = false;

        /// <summary>
        /// NumDots contains the number of dots in this level. 
        /// </summary>
        private int NumDots = -1;

        /// <summary>
        /// Number of grey button present at a given level.
        /// </summary>
        private int NumDotsGray = -1;

        /// <summary>
        /// Number of hurdles in this level.
        /// </summary>
        private int NumHurdles = -1;

        /// <summary>
        /// Points where white dots(targets) will be displayed.
        /// </summary>
        private Point[] arrDotPoints;

        /// <summary>
        /// Points where grey dots(targets) will be displayed.
        /// </summary>
        private Point[] arrDotPointsGray;

        /// <summary>
        /// flage for each target, says target is achived or not.
        /// </summary>
        private bool[] arrTargetAchived;

        /// <summary>
        /// Points between line will be drawn to draw a hurdle.
        /// </summary>
        private Point[,] arrLinePoints;

        /// <summary>
        /// Program written at this level.
        /// </summary>
        internal string iCurrentProgram = "";


        /// <summary>
        /// this keeps the program for the best solution given by user. which is stored in database.
        /// </summary>
        internal string iBestProgram = "";

        /// <summary>
        /// Total Score of this Level. this is the best score for this level for one run of application.
        /// </summary>
        internal int levelScore = 0;

        /// <summary>
        /// buffer level score.
        /// </summary>
        //private int temp_levelScore = 0;

        /// <summary>
        /// numdots pressed pressed at this level not persistant.
        /// </summary>
        public int temp_numDotsPressed = 0;

        /// <summary>
        /// numdots pressed pressed at this level, this is persistant.
        /// </summary>
        public int numDotsPressed = 0;

        /// <summary>
        /// this time variable keeps track of time spent on this level in different time intervals.
        /// </summary>
        //internal int timeSpent = 0;

        /// <summary>
        /// This variable is used to keep track of total time spent on each level
        /// </summary>
        internal int iTotalTimeSpent;

        /// <summary>
        /// Total Gos attempted so far.
        /// </summary>
        internal int iTotalGos;

        /// <summary>
        /// this time variable keeps track of the time spend for getting the best solution for the level.
        /// </summary>
        internal int iBestTimeSpent = 0;


        /// <summary>
        /// Image that contains the image of this Level
        /// </summary>
        internal Bitmap LevelImg = new Bitmap(208 * GlobalData.ZoomInZoomOut, 208 * GlobalData.ZoomInZoomOut);
//#if (RUNSTATE)







        public int iLevelId;
        /// <summary>
        /// herbert's current position 
        /// (co-ordinates of the dot on which herbert is positioned)
        /// </summary>
        private int icurPosX, icurPosY;

        public int curPosX
        {
            set
            {
                if ((value == 0 || value > 204) && !(iLevelId <= 0))
                {
                    //Console.WriteLine("Value-" + value);
                    icurPosX = getRoboXY(iLevelId).X / GlobalData.ZoomInZoomOut;
                    curPosY = getRoboXY(iLevelId).Y / GlobalData.ZoomInZoomOut;
                    prePathPosX = icurPosX;
                    prePathPosY = icurPosY;
                    HerbertMain.InitialOrResetHrbt = new Point(icurPosX, icurPosY);
                    //Console.WriteLine("icurPosX-" + icurPosX);
                }
                else
                    icurPosX = value;
            }
            get { return icurPosX; }
        }

        public int curPosY
        {
            set
            {
                if ((value == 0 || value > 204) && !(iLevelId <= 0))
                {
                    //Console.WriteLine("Value-" + value);
                    curPosX = getRoboXY(iLevelId).X / GlobalData.ZoomInZoomOut;
                    icurPosY = getRoboXY(iLevelId).Y / GlobalData.ZoomInZoomOut;
                    prePathPosX = icurPosX;
                    prePathPosY = icurPosY;
                    HerbertMain.InitialOrResetHrbt = new Point(icurPosX, icurPosY);
                    //Console.WriteLine("icurPosY-" + icurPosY);
                }
                else
                    icurPosY = value;
            }
            get { return icurPosY; }
        }

        /// <summary>
        /// herbert's position prior to the current position
        /// (co-ordinates of the pixel on which herbert was positioned)
        /// </summary>
        public int prePosX, prePosY;

        /// <summary>
        /// Position of next dot on board where Herbert would be positioned
        /// (co-ordinates of the dot on which herbert is going to be positioned)
        /// </summary>
        public int destPosX, destPosY;

        /// <summary>
        /// herbert's position prior to the current position
        /// (co-ordinates of the dot on which herbert was positioned)
        /// </summary>
        public int preDestPosX, preDestPosY;

        ///// <summary>
        ///// variable indicating for the level, 
        ///// whether the end of program is reached
        ///// </summary>
        //public volatile object EndofProg = false;











        //public bool blnLevelFinishedMsg = false;

        /// <summary>
        /// variable which takes care of herbert is reset or not.
        /// </summary>
        public bool blnHReset = false;




        public bool blnIsWallHit;
        /// <summary>
        /// Used for editor text box tracing, it keeps the number of lines in textbox
        /// </summary>
        public int lineCount;





        /// <summary>
        /// specifies the current direction of herbert.
        ///0 is up direction
        ///1 is right direction
        ///2 is down direction
        ///3 is left direction
        /// </summary>
        public int CurDir = 0;

        //Added by Sujith/Vivek on 06/28/2005 for issue id: 1426
        /// <summary>
        /// specifies the previous direction of herbert.
        ///0 is up direction
        ///1 is right direction
        ///2 is down direction
        ///3 is left direction
        /// </summary>

        public int PreDir = 0;

        public bool blnFirstStepClick = true;
		 
//#endif
//        # endregion 

		# region Constructor
		/// <summary>
		/// constructor 
		/// </summary>
		/// <param name="LevelNo">level number</param>
        public Level(int LevelId)
        {
            try
            {
                this.iLevelId = LevelId;
                if (LevelId == 0)
                {
                    NumDots = 0;
                    NumHurdles = 0;
                    arrDotPoints = new Point[0];
                    arrDotPointsGray = new Point[0];
                    arrLinePoints = new Point[0, 0];
#if(DESIGNER)
                    LevelType = "Normal";
#endif
                    //blnNewHerdleBufferChanged = true;//By Rajesh,21/8/06,3885
                    //blnNewDotBufferChanged = true;//By Rajesh,21/8/06,3885
                }
                else
                {
                    DataRow[] rows = GlobalData.dsAllHData.Tables["tblLevelInfoMST"].Select("LevelId='" + LevelId.ToString()+"'");
                    NumDots = int.Parse(rows[0]["Buttons"].ToString());
                    NumHurdles = int.Parse(rows[0]["Walls"].ToString());
#if(DESIGNER)
                    try
                    {
                        LevelType = rows[0]["Type"].ToString();
                    }
                    catch { }
#endif
                    getGameLevelScoreData(LevelId);
                    rows = null;


                    //Added by Vivek Balagangadharan
                    // Description : This part is added to initialise all variables for a level when it is created.
                    blnGoState = true;
                    blnStepState = true;
                    blnHaltState = false;
                    blnResetState = false;
                    blnResumeState = false;
                    blnHReset = true;

                    curPosX = getRoboXY(LevelId).X;
                    curPosY = getRoboXY(LevelId).Y;

                    CurDir = 0;

                    //atLineInfo = new AtLineInfoStruct[HerbertMain.MAXLINES];
                    iSP = 0;
                }
                ResetLevel(LevelId);
            }
            catch
            {
            }
        }
		
		
		# endregion		

		# region methods


#if(CONTEST||PATTERN_GENERATER)
        /// <summary>
		/// saves the path traversed by herbert in the data structure of HPathStore.
		/// </summary>
		/// <param name="currentLevelIndex">level number.</param>
		internal void fSavePath(int currentLevelIndex)
		{
			if(objHPath == null)
			{
				objHPath = new HPathStore();
			}	
			//right
			if(CurDir == 1 && !(preDestPosX < 0 || preDestPosY < 0) && !(preDestPosX > 24*HConstants.DOTSPACE))
			{
				if(!objHPath.isPathAvailable(preDestPosX, preDestPosY, HConstants.HPLEFT))
					//if(!isHurdle(CurDir, GetLevelId(currentLevelIndex), (preDestPosX+8)*GlobalData.ZoomInZoomOut, preDestPosY*GlobalData.ZoomInZoomOut))
						objHPath.fSavePath(preDestPosX, preDestPosY, HConstants.HPLEFT);
			}
			//down
			else if(CurDir == 2 && !(preDestPosX < 0 || preDestPosY < 0) && !(preDestPosY > 24*HConstants.DOTSPACE))
			{
				if(!objHPath.isPathAvailable(preDestPosX, preDestPosY, HConstants.HPDOWN))
					//if(!isHurdle(CurDir, GetLevelId(currentLevelIndex), preDestPosX*GlobalData.ZoomInZoomOut, (preDestPosY+8)*GlobalData.ZoomInZoomOut))
						objHPath.fSavePath(preDestPosX, preDestPosY, HConstants.HPDOWN);
			}
			//up
			else if(CurDir == 0 && !(destPosX < 0 || destPosY < 0))
			{
				if(!objHPath.isPathAvailable(destPosX, destPosY, HConstants.HPDOWN))
					//if(!isHurdle(CurDir, GetLevelId(currentLevelIndex), preDestPosX*GlobalData.ZoomInZoomOut, (preDestPosY+8)*GlobalData.ZoomInZoomOut))
						objHPath.fSavePath(destPosX, destPosY, HConstants.HPDOWN);
			}
			//left
			else if(CurDir == 3 && !(destPosX < 0 || destPosY < 0))
			{
				if(!objHPath.isPathAvailable(destPosX, destPosY, HConstants.HPLEFT))
					//if(!isHurdle(CurDir, GetLevelId(currentLevelIndex), (preDestPosX+8)*GlobalData.ZoomInZoomOut, preDestPosY*GlobalData.ZoomInZoomOut))
						objHPath.fSavePath(destPosX, destPosY, HConstants.HPLEFT);
			}
		}


		/// <summary>
		/// removes the wrong path traversed by herbert in the data structure of HPathStore.
		/// </summary>		
		internal void fRemovePath()
		{
			if(objHPath == null)
			{
				objHPath = new HPathStore();
			}

			if(CurDir == 1 && !(preDestPosX < 0 || preDestPosY < 0))
				objHPath.fRemovePath(preDestPosX, preDestPosY, HConstants.HPLEFT);
			else if(CurDir == 2 && !(preDestPosX < 0 || preDestPosY < 0))
				objHPath.fRemovePath(preDestPosX, preDestPosY, HConstants.HPDOWN);
			else if(CurDir == 0 && !(destPosX < 0 || destPosY < 0))
				objHPath.fRemovePath(destPosX, destPosY, HConstants.HPDOWN);
			else if(CurDir == 3 && !(destPosX < 0 || destPosY < 0))
				objHPath.fRemovePath(destPosX, destPosY, HConstants.HPLEFT);			
		}


		/// <summary>
		/// disposes the path store object.
		/// </summary>
		internal void fDisposePathStore()
		{
			if(objHPath != null)
			{
				objHPath.Dispose();
				objHPath = null;
			}
		}

		
		/// <summary>
		/// displays the stored path on the herbert board.
		/// </summary>
		internal void fDisplayStoredPath()
		{
			if(objHPath != null)
			{				
				short[] arrPath = objHPath.fGetSavedPath();
				int iPrePathPosX = prePathPosX;
				int iPrePathPoxY = prePathPosY;
				for(int i = 0; i < HConstants.GRIDDOTSPERLINE; i++)
				{
					for(int j = 0; j < HConstants.GRIDDOTSPERLINE; j++)
					{
						if(arrPath[i*25+j] != 0)
						{
							if((arrPath[i*25+j] & HConstants.HPDOWN) != 0)
							{								
								prePathPosX = i*HConstants.DOTSPACE+4;
								prePathPosY = (j+1)*HConstants.DOTSPACE+4;
								drawPath((i*HConstants.DOTSPACE+4) * GlobalData.ZoomInZoomOut, (j*HConstants.DOTSPACE+4) * GlobalData.ZoomInZoomOut);
							}
							if((arrPath[i*25+j] & HConstants.HPLEFT) != 0)
							{
								prePathPosX = (i+1)*HConstants.DOTSPACE+4;
								prePathPosY = j*HConstants.DOTSPACE+4;
								drawPath((i*HConstants.DOTSPACE+4) * GlobalData.ZoomInZoomOut, (j*HConstants.DOTSPACE+4)* GlobalData.ZoomInZoomOut);
							}
						}
					}
				}
				prePathPosX = iPrePathPosX;
				prePathPosY = iPrePathPoxY;
			}
		}
#endif

		public static Level GetLevelData(Level objLevel,int iLevelIndex)
		{
			Level retLevelObj= new Level(objLevel.iLevelId);
			retLevelObj.iLevelId=Level.arrLevelId[iLevelIndex];
			retLevelObj.levelScore=Level.arrLvlPoints[iLevelIndex];
			retLevelObj.iCurrentProgram=Level.arrLvlProgram[iLevelIndex];
			retLevelObj.NumDots=objLevel.NumDots;
			retLevelObj.NumDotsGray=objLevel.NumDotsGray;
			retLevelObj.NumHurdles=objLevel.NumHurdles;
#if (DESIGNER)
			retLevelObj.HerbertX=objLevel.HerbertX;
			retLevelObj.HerbertY=objLevel.HerbertY;
#endif
			retLevelObj.arrLinePoints=objLevel.arrLinePoints;

			retLevelObj.arrDotPoints=objLevel.arrDotPoints;
			retLevelObj.arrDotPointsGray=objLevel.arrDotPointsGray;
			return retLevelObj;
		}

		public void SetLevelData(Level objLevel)
		{
			//Level retLevelObj= new Level(objLevel.iLevelId);
			  
			  iCurrentProgram=objLevel.iCurrentProgram;
			  levelScore=objLevel.levelScore;
			  NumDots=objLevel.NumDots;
			  NumDotsGray=objLevel.NumDotsGray;
			  NumHurdles=objLevel.NumHurdles;
			  arrDotPoints=objLevel.arrDotPoints;
			  arrDotPointsGray=objLevel.arrDotPointsGray;
			
		}
		/// <summary>
		/// gets the data for a level socre and assigns them to proper variables.
		/// </summary>
		/// <param name="LevelId">Level Id for which the level data is required.</param>
		internal void getGameLevelScoreData(int LevelId)
		{
            DataRow[] dr = GlobalData.dsAllHData.Tables["tblGameLevelScore"].Select("levelid = '" + LevelId.ToString()+"'");
			if(dr.Length == 0)
			{
				iCurrentProgram = iBestProgram = "";
				LevelScoreId =  0;
				iBestBytes = charused = 0;
				IsLevelFinished = IsLevelFinishedPersistant = false;
				pNumVisits = 0;				
				iBestTimeSpent = 0;
				numDotsPressed = temp_numDotsPressed = 0;
				iBestGos = 0;
				levelScore = BestLevelScore = 0;
				iFirstProgram = "";
				iFirstBytes = 0;
				blnLevelSolvedMsgShown = false;
			}
			else
			{				
				iLastProgram = dr[0]["LastProgram"].ToString();
				iLastBytes = int.Parse(dr[0]["LastBytes"].ToString());
				iLastGos  = int.Parse(dr[0]["LastGos"].ToString());
				iLastTimeSpent = int.Parse(dr[0]["LastTimeSpent"].ToString());
				iLastCharsTyped= int.Parse(dr[0]["LastCharsTyped"].ToString());
				iLastVisits = int.Parse(dr[0]["LastVisits"].ToString());
				iFirstLevelScore = int.Parse(dr[0]["FirstScore"].ToString());
				iFirstVisits = int.Parse(dr[0]["FirstVisits"].ToString());

				iLastDate = DateTime.Parse(dr[0]["LastDate"].ToString());
				iBestVisits = int.Parse(dr[0]["BestVisits"].ToString());
				iBestCharsTyped = int.Parse(dr[0]["BestCharsTyped"].ToString());
				iBestDate = DateTime.Parse(dr[0]["BestDate"].ToString());
				iFirstTimeSpent = int.Parse(dr[0]["FirstTimeSpent"].ToString());
				iFirstGos = int.Parse(dr[0]["FirstGos"].ToString());
				iFirstBytes = int.Parse(dr[0]["FirstBytes"].ToString());
				iFirstCharsTyped = int.Parse(dr[0]["FirstCharsTyped"].ToString());
				iBestProgram =  dr[0]["BestProgram"].ToString();				
				iBestTimeSpent = int.Parse(dr[0]["BestTimeSpent"].ToString());
				iBestGos = int.Parse(dr[0]["BestGos"].ToString());
				iFirstCharsTyped = int.Parse(dr[0]["FirstCharsTyped"].ToString());
			
				FirstCharTypedDT = DateTime.Parse(dr[0]["DateFirstCharTyped"].ToString());
				FirstSolutionDT = DateTime.Parse(dr[0]["FirstDate"].ToString());
				iBestBytes = charused = int.Parse(dr[0]["BestBytes"].ToString());				
				iCurrentProgram =  dr[0]["CurrentProgram"].ToString();				
				LevelScoreId =  int.Parse(dr[0]["levelScoreId"].ToString());
				blnLevelSolvedMsgShown = IsLevelFinished = IsLevelFinishedPersistant = bool.Parse(dr[0]["LevelStatus"].ToString());
				numDotsPressed = int.Parse(dr[0]["BestNumButtonsPressed"].ToString());
				temp_numDotsPressed = 0;
				levelScore = int.Parse(dr[0]["LevelScore"].ToString());
				BestLevelScore = int.Parse(dr[0]["LevelScore"].ToString());
				iFirstProgram = dr[0]["FirstProgram"].ToString();
				pNumVisits = int.Parse(dr[0]["NumLevelVisits"].ToString());
				iTotalGos = int.Parse(dr[0]["TotalGos"].ToString());
				iTotalTimeSpent = int.Parse(dr[0]["TotalTimeSpent"].ToString());
				iTotalCharsTyped = int.Parse(dr[0]["TotalCharsTyped"].ToString());
			}			
			dr = null;
		}
	

		public int getLevelNumber(int LevelId)
		{
			DataRow[] dr = GlobalData.dsAllHData.Tables["tblLevelInfoMST"].Select("LevelId = '"+LevelId.ToString()+"'");
			return int.Parse(dr[0]["LevelNo"].ToString());
		}
			

		/// <summary>
		/// gets all valid level numbers
		/// </summary>
		/// <param name="Index">index in the array</param>
		/// <returns>valid element of the array with given index.</returns>
		public static int GetLevelId(int Index)
		{
			return arrLevelId[Index];
		}
		

		/// <summary>
		/// returns the Level points defined for this level.
		/// </summary>
		/// <param name="LevelNo">Level No for which Points are needed.</param>
		/// <returns>The Points for this level</returns>
		public static int getLevelPoints(int Index)
		{
			return arrLvlPoints[Index];
		}


		public static void saveLevelPoints(int Index, int iLPValue)
		{
			arrLvlPoints[Index] = iLPValue;
		}


		public static string getLevelProgram(int Index)
		{
			string retString="";
			try
			{
				retString = arrLvlProgram[Index];
				//				retString = retString.Replace("\n","\r\n");
				
			}
			catch
			{
				//Console.WriteLine(exp.Message);
			}
			return retString;
		}
		
		public static string getTextProgram(string levelProgram)
		{
			return levelProgram.Replace("\n","\r\n");
		}

		public static void saveLevelProgram(int Index, string levelProgram)
		{
			arrLvlProgram[Index] = levelProgram.Replace("\r\n","\n");;
			//			arrLvlProgram[Index] = levelProgram;
		}

		/// <summary>
		/// get dot points for this level.
		/// </summary>
		/// <param name="LevelNo">Level No for which data is needed.</param>
		/// <returns>data</returns>
		private static Point[] getDotPoints(int LevelNo)
		{
			Point[] temp = new Point[1];
			temp[0].X = -1;
			temp[0].Y = -1;			
			return temp;
		}


		/// <summary>
		/// get the points where tragets will be drawn from database.
		/// </summary>
		/// <param name="LevelNo">level No</param>
		/// <param name="IsGray">which type of dot is needed white or grey.</param>
		/// <returns>Points where targets will be drawn</returns>
		internal static Point[] getDotPointsDB(int LevelId, int IsGray)
		{
			// remove zoominzoomout factor from here and multiply it in the places before display,checking walls,buttons
			Point[] temp = new Point[1];
			temp[0].X = -1;
			temp[0].Y = -1;
			DataRow[] drows = GlobalData.dsAllHData.Tables["tblLevelButtonsMST"].Select("LevelId='"+LevelId.ToString()+"' AND Grey='"+IsGray.ToString()+"'");
			Point[] pts = new Point[drows.Length];
			for(int i=0;i<drows.Length;i++)
			{				
				pts[i] = new Point(int.Parse(drows[i]["ButtonX"].ToString())*HConstants.DOTSPACE+DOTOFFSET,int.Parse(drows[i]["ButtonY"].ToString())*HConstants.DOTSPACE+DOTOFFSET);
			}
			temp = null;
			drows = null;
			return pts;
		}


		/// <summary>
		/// get Line points for this level.
		/// </summary>
		/// <param name="LevelNo">Level No for which Hurdles points are needed.</param>
		/// <returns>retrun Points array where hurdles will be drawn</returns>
		private static Point[,] getLinePoints(int LevelNo)
		{
			Point[,] temp = new Point[1,1];
			temp[0,0].X = -1;
			temp[0,0].Y = -1;
						
			//			switch(LevelNo)
			//			{
			//				
			//				case 1:
			//					return L1_LineHurdles;					
			//				case 2:
			//					return L2_lineHurdles;	
			//				case 3:
			//					return L3_lineHurdles;		
			//				case 4:
			//					return L4_lineHurdles;
			//				case 5:
			//					return L5_lineHurdles;
			//				case 6: 
			//					return L6_lineHurdles;
			//				case 7: 
			//					return L7_lineHurdles;
			//				case 8: 
			//					return L8_lineHurdles;
			//				case 9: 
			//					return temp;
			//				case 10:
			//					return L10_lineHurdles;
			////				case 11:
			////					return L11_lineHurdles;
			//			}
			return temp;
		}

		
		/// <summary>
		/// get line hurdles from database.
		/// </summary>
		/// <param name="LevelNo">Level No for which line points are needed.</param>
		/// <returns>Hurdle Points</returns>
		private static Point[,] getLinePointsDB(int LevelId)
		{
			//			Point[,] temp = new Point[1,1];
			//			temp[0,0].X = -1;
			//			temp[0,0].Y = -1;
			
			DataRow[] drows = GlobalData.dsAllHData.Tables["tblLevelWallsMST"].Select("LevelId='"+LevelId.ToString()+"'");
			Point[,] pts = new Point[drows.Length,2];
			for(int i=0;i<drows.Length;i++)
			{
				//pts[i,0] = new Point(int.Parse(drows[i]["startX"].ToString())*HConstants.DOTSPACE*GlobalData.ZoomInZoomOut+DOTOFFSET*GlobalData.ZoomInZoomOut,int.Parse(drows[i]["startY"].ToString())*HConstants.DOTSPACE*GlobalData.ZoomInZoomOut+DOTOFFSET*GlobalData.ZoomInZoomOut);
				pts[i,0] = new Point(int.Parse(drows[i]["startX"].ToString())*HConstants.DOTSPACE+DOTOFFSET,int.Parse(drows[i]["startY"].ToString())*HConstants.DOTSPACE+DOTOFFSET);
				
				//pts[i,1] = new Point(int.Parse(drows[i]["endX"].ToString())*HConstants.DOTSPACE*GlobalData.ZoomInZoomOut+DOTOFFSET*GlobalData.ZoomInZoomOut,int.Parse(drows[i]["endY"].ToString())*HConstants.DOTSPACE*GlobalData.ZoomInZoomOut+DOTOFFSET*GlobalData.ZoomInZoomOut);
				pts[i,1] = new Point(int.Parse(drows[i]["endX"].ToString())*HConstants.DOTSPACE+DOTOFFSET,int.Parse(drows[i]["endY"].ToString())*HConstants.DOTSPACE+DOTOFFSET);
				
			}
			
			//temp = null;
			drows = null;
			return pts;
		}
		

		/// <summary>
		/// get the (x,y) coordinates for the herbert.
		/// </summary>
		/// <param name="LevelNo">Level No for which herbert's (x,y) points are needed</param>
		/// <returns>retruns the (x,y) point of herbert</returns>
		public Point getRoboXY(int LevelId)
		{
			DataRow[] drows = GlobalData.dsAllHData.Tables["tblLevelInfoMST"].Select("LevelId='"+LevelId.ToString()+"'");
			Point p;
			if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
			{			
				p = new Point(int.Parse(drows[0]["RoboX"].ToString())*GlobalData.ZoomInZoomOut,int.Parse(drows[0]["RoboY"].ToString())*GlobalData.ZoomInZoomOut);
			}
			else
			{
				p = new Point(int.Parse(drows[0]["RoboX"].ToString())*GlobalData.ZoomInZoomOut,int.Parse(drows[0]["RoboY"].ToString())*GlobalData.ZoomInZoomOut);
			}
			drows = null;
			return p;
		}

		/// <summary>
		/// deletes the extra path created in the large mode of herbert due to small image size.
		/// </summary>
		/// <param name="curPosX">Current X coordinate of the Herbert.</param>
		/// <param name="curPosY">Current Y coordinate of the Herbert.</param>
		/// <param name="prePosX">Previous X coordinate of the Herbert.</param>
		/// <param name="prePosY">Previous Y coordinate of the Herbert.</param>
		public void ErasePath(int curPosX, int curPosY,int prePosX,int prePosY)
		{
#if (EXCEPTION)	
			try
			{
#endif
				int zizo = GlobalData.ZoomInZoomOut;
				int RNGX = 0, RNGY = 0;
				if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
				{
					RNGX = 4*zizo+1;
					RNGY = 4*zizo+2;
				}
				else
				{
					RNGX = 3;
					RNGY = 3;
				}				
				lock(LevelImg)
				{
					Graphics bufferGraphics = Graphics.FromImage(LevelImg);					
#if (EXCEPTION)
					try
					{
#endif
						Pen WhitePen = new Pen(Color.White,GlobalData.ZoomInZoomOut);
#if (EXCEPTION)
						try
						{
#endif
                           
                            //if (GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
                            //    bufferGraphics.DrawLine(WhitePen, curPosX + 4 + GlobalData.ZoomInZoomOut, curPosY + 4 + GlobalData.ZoomInZoomOut, prePosX + 4 + GlobalData.ZoomInZoomOut, prePosY + 4 + GlobalData.ZoomInZoomOut);
                            //else
                            //    bufferGraphics.DrawLine(WhitePen, curPosX + 3, curPosY + 3, prePosX + 3, prePosY + 3);
                            //Added By Rajesh to resolve the glitch issue. //TODO21/05/07
                            if (GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
                            {
                                if(!(this.curPosX==this.destPosX && this.curPosY==this.destPosY))
                                    bufferGraphics.DrawLine(WhitePen, (this.curPosX * GlobalData.ZoomInZoomOut) + 4 + GlobalData.ZoomInZoomOut, (this.curPosY * GlobalData.ZoomInZoomOut) + 4 + GlobalData.ZoomInZoomOut, (this.preDestPosX * GlobalData.ZoomInZoomOut) + 4 + GlobalData.ZoomInZoomOut, (this.preDestPosY * GlobalData.ZoomInZoomOut) + 4 + GlobalData.ZoomInZoomOut);
                            }
                            else
                                if (!(this.curPosX == this.destPosX && this.curPosY == this.destPosY))
                                    bufferGraphics.DrawLine(WhitePen, (this.curPosX * GlobalData.ZoomInZoomOut) + 3, (this.curPosY * GlobalData.ZoomInZoomOut) + 3, (this.preDestPosX * GlobalData.ZoomInZoomOut) + 3, (this.preDestPosY * GlobalData.ZoomInZoomOut) + 3);
                                
                            

							WhitePen.Dispose();
#if (EXCEPTION)
						}
						catch
						{
							//Console.WriteLine(exp2.Message + ", drawPath, 13");
						}			
						//button is getting cut because of path extansion resolution.
						if(Math.Abs(curPosX - prePosX) + 4* GlobalData.ZoomInZoomOut == HConstants.DOTSPACE*GlobalData.ZoomInZoomOut 
							|| Math.Abs(curPosX - prePosX) + 4* GlobalData.ZoomInZoomOut == HConstants.DOTSPACE*GlobalData.ZoomInZoomOut)
						{
							switch(CurDir)
							{
								case 0:
									curPosY--;
									prePosY+=3;
									break;
								case 1: 
									curPosX++;
									prePosX-=3;
									break;
								case 2: 
									curPosY++;
									prePosY-=3;
									break;
								case 3: 
									curPosX--;
									prePosX+=3;
									break;
							}
						}
						for(int i = 0; i < NumDots; i++)
						{
							if((Math.Abs(curPosX - arrDotPoints[i].X*zizo) < RNGX) && (Math.Abs(curPosY - arrDotPoints[i].Y*zizo) < RNGY))
							{
								if(arrTargetAchived[i] == true)
								{
									dotFill(arrDotPoints[i]);
								}
							}
						}
						Pen BP1 = new Pen(Color.Black,1.5F); 
						for(int i = 0; i < NumDotsGray; i++)
						{
							if((Math.Abs(curPosX - arrDotPointsGray[i].X*zizo) < RNGX) && (Math.Abs(curPosY - arrDotPointsGray[i].Y*zizo) < RNGY))
							{
								using(SolidBrush forgrey = new SolidBrush(System.Drawing.Color.Gray))
								{
									bufferGraphics.FillEllipse(forgrey,arrDotPointsGray[i].X*zizo-zizo,arrDotPointsGray[i].Y*zizo-zizo+1,4*zizo-BUTTONOFFSET,4*zizo-BUTTONOFFSET);
									bufferGraphics.DrawEllipse(BP1,arrDotPointsGray[i].X*zizo-zizo,arrDotPointsGray[i].Y*zizo-zizo+1,4*zizo-BUTTONOFFSET,4*zizo-BUTTONOFFSET);
								}
							}		
						}
						
						try
						{
#endif
							BP1.Dispose();
							bufferGraphics.Dispose();
							bufferGraphics = null;
#if (EXCEPTION)
						}
						catch
						{
							//Console.WriteLine(exp4.Message + ", drawPath, 15");
						}
					}
					catch
					{
						//Console.WriteLine(exp1.Message + ", drawPath, 12");
					}	
#endif
				}
#if (EXCEPTION)
			}
			catch
			{			
				//Console.WriteLine(e.Message + ", drawPath, 11");				
			}
#endif	
		}


		/// <summary>
		/// function to draw path
		/// </summary>
		/// <param name="curPosX">current position of herbert in x direction</param>
		/// <param name="curPosY">current position of herbert in y direction</param>
		public void drawPath(int curPosX, int curPosY)
		{
#if (EXCEPTION)	
			try
			{
#endif
				lock(LevelImg)
				{
					Graphics bufferGraphics = Graphics.FromImage(LevelImg);
#if (EXCEPTION)
					try
					{
#endif
						Pen BlackPen = new Pen(Color.Black,GlobalData.ZoomInZoomOut);
#if (EXCEPTION)
						try
						{
#endif
							/*if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
								bufferGraphics.DrawLine(BlackPen, curPosX+4+GlobalData.ZoomInZoomOut, curPosY+4+GlobalData.ZoomInZoomOut, prePathPosX+4+GlobalData.ZoomInZoomOut,prePathPosY+4+GlobalData.ZoomInZoomOut);								
							else
								bufferGraphics.DrawLine(BlackPen, curPosX+3, curPosY+3, prePathPosX+3,prePathPosY+3);*/
							if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
								bufferGraphics.DrawLine(BlackPen, curPosX+4+GlobalData.ZoomInZoomOut, curPosY+4+GlobalData.ZoomInZoomOut, (prePathPosX * GlobalData.ZoomInZoomOut )+4+GlobalData.ZoomInZoomOut,(prePathPosY * GlobalData.ZoomInZoomOut )+4+GlobalData.ZoomInZoomOut);								
							else
								bufferGraphics.DrawLine(BlackPen, curPosX+3, curPosY+3, (prePathPosX * GlobalData.ZoomInZoomOut )+3,(prePathPosY * GlobalData.ZoomInZoomOut)+3);
						
							BlackPen.Dispose();
#if (EXCEPTION)
						}
						catch
						{
							//Console.WriteLine(exp2.Message + ", drawPath, 13");
						}
						
						try
						{
#endif
							prePathPosX = curPosX / GlobalData.ZoomInZoomOut;
							prePathPosY = curPosY / GlobalData.ZoomInZoomOut;
#if (EXCEPTION)
						}
						catch
						{
							//Console.WriteLine(exp3.Message + ", drawPath, 14");
						}
						try
						{
#endif
							bufferGraphics.Dispose();
							bufferGraphics = null;
#if (EXCEPTION)
						}
						catch
						{
							//Console.WriteLine(exp4.Message + ", drawPath, 15");
						}
					}
					catch
					{
						//Console.WriteLine(exp1.Message + ", drawPath, 12");
					}	
#endif
				}
#if (EXCEPTION)
			}
			catch
			{			
				//Console.WriteLine(e.Message + ", drawPath, 11");				
			}
#endif
		}


		public int getSavedLevelPoints(int LevelId)
		{
			if(LevelId != 0)
			{
				DataRow[] dr = GlobalData.dsAllHData.Tables["tblLevelInfoMST"].Select("LevelId='"+LevelId.ToString()+"'");
				return int.Parse(dr[0]["Points"].ToString());
			}
			else
				return 0;
		}

		
		/// <summary>
		/// fucnction which prepares the data for levels.
		/// </summary>
		/// <returns>returns the total number of level returned from database.</returns>
        public static Level[] BuildAllLevelData()
        {
            Level[] pLevels = null;

            if (GlobalData.HerbertMode != HMode.Tutorial)
            {
#if(DESIGNER)
                //2006-31-07//To get the sorted //Rajesh 
                DataView dv = GlobalData.dsAllHData.Tables["tblLevelInfoMST"].DefaultView;
                dv.Sort = "levelno asc";
                //DataRow[] dr = GlobalData.dsAllHData.Tables["tblLevelInfoMST"].Select("Orderby levelno asc");
                DataRow[] dr = dv.ToTable().Select();
#endif
#if (CONTEST)
                DataRow[] dr = GlobalData.dsAllHData.Tables["tblLevelInfoMST"].Select();
#endif

                if (GlobalData.iPatternId == "-1")
                {
                    arrLvlPoints = new int[dr.Length + 2];
                    arrLvlPoints[0] = 0;
                }
                if (GlobalData.iPatternId == "-1")
                {
                    arrLvlProgram = new string[dr.Length + 2];
                    arrLvlProgram[0] = "";
                }

                arrValidLevelNo = new int[dr.Length + 2];
                if (GlobalData.iPatternId == "-1")
                {
                    arrLevelId = new int[dr.Length + 2];
                    arrLevelId[0] = 0;
                }

                arrValidLevelNo[0] = 0;

                if (dr.Length > 0)
                {
                    try
                    {
                        for (int i = 0; i < dr.Length; i++)
                        {
                            //arrLvlPoints[i+1] = int.Parse(dr[i]["Points"].ToString());
                            if (GlobalData.iPatternId == "-1")
                            {
                                arrLvlPoints[i + 1] = int.Parse(dr[i]["Points"].ToString());
                            }
# if(DESIGNER)
                            if (GlobalData.iPatternId == "-1")
                            {
                                arrLvlProgram[i + 1] = dr[i]["LevelProgram"].ToString();
                            }

                            /*==============================================================================
                             *  Modified By : Vivek Balagangadharan
                             *  Description : iLevelId is a comma separated list of levels. -1 is its default value.
                             *  Modified On : 10-Apr-2006
                             *  Special Comments : iLevelId was an int type initially
                             * ==============================================================================*/
                            if (GlobalData.iLevelId != "-1")
                            {
                                if (GlobalData.iPatternId == "-1")
                                    arrLvlProgram[i + 1] = dr[i]["LevelProgram"].ToString();
                            }
#endif
                            //
                            //arrValidLevelNo[i+1] = int.Parse(dr[i]["LevelNo"].ToString());
                            arrValidLevelNo[i + 1] = int.Parse(dr[i]["LevelNo"].ToString());
                            if (GlobalData.iPatternId == "-1")
                            {
                                arrLevelId[i + 1] = int.Parse(dr[i]["LevelId"].ToString());
                            }
                        }
                        //</final changes 02/12/05>
                        //return dr.Length;

                    }
                    catch
                    {

                        //Console.WriteLine("BuildAllLevelData: get Level Data, "+exp.Message + ", BuildAllLevelData, 31");

                        //return 0;
                    }
                }
                //Level[] arrLevels = null
                if (GlobalData.iPatternId == "-1")
                {
                    pLevels = new Level[1 + dr.Length];
                }
                else
                {
                    int cnt = 0;
                    if (arrLevelId == null)
                    {
                        char[] seperater ={ ',' };
                        cnt = GlobalData.iPatternId.Split(seperater).Length;
                    }
                    else
                    {
                        cnt = arrLevelId.Length - 1;
                    }
                    pLevels = new Level[cnt + 1];
                }
                //dr = null;
                if (GlobalData.iPatternId != "-1" && dr.Length <= 0)
                {
                    char[] seperater ={ ',' };
                    int cnt = GlobalData.iPatternId.Split(seperater).Length;
                    pLevels = new Level[1 + cnt];
                    arrLvlPoints = new int[cnt + 1];

                    arrLvlProgram = new string[cnt + 1];
                    arrLvlProgram[0] = "";

                    arrValidLevelNo = new int[cnt + 1];
                    arrLevelId = new int[cnt + 1];
                    arrLvlPoints[0] = 0;

                    arrValidLevelNo[0] = 0;
                    arrLevelId[0] = 0;
                    //pLevels = new Level[cnt];
                    try
                    {
                        //DataColumn[] dcPattern = GlobalData.dsAllHData.Tables[7].Rows[i][
                        for (int i = 0; i < cnt; i++)
                        {
                            arrLvlProgram[i + 1] = GlobalData.dsAllHData.Tables[7].Rows[i]["pattern"].ToString();
                        }
                    }
                    catch
                    {
                    }
                    //arrLvlProgram[cnt] = "";
                }
                int piLevels = pLevels.Length;

                for (int i = 0; i < piLevels; i++)
                {
                    pLevels[i] = new Level(arrLevelId[i]);
#if(PATTERN_GENERATER)
                    if (arrLevelId[i] == 0 && GlobalData.iPatternId != "-1")
                    {
                        string strtracesign = "";
                        if (i != 0)
                        {
                            try
                            {
                                strtracesign = GlobalData.dsAllHData.Tables[7].Rows[i - 1]["tracesignature"].ToString();
                                strtracesign = GetTrace(strtracesign);
                            }
                            catch { }
                        }
                        pLevels[i].GenerateTraceImage(strtracesign, GlobalData.ZoomInZoomOut);
                        pLevels[i].drawLevel(arrLevelId[i]);
                    }
                    else
                    {
                        pLevels[i].drawLevel(arrLevelId[i]);
                        pLevels[i].isValidPatternImage = false;
                    }
#endif
                }

                //return 0;
            }
            else
            {
                pLevels = new Level[1];
                pLevels[0] = new Level(0);
            }
            return pLevels;
        }
		

		/// <summary>
		/// Resets the game board.
		/// </summary>
		/// <param name="LevelNo">Level Number whose board needs to be reset.</param>
		public void ResetLevel(int LevelId)
		{
			if(NumDots != 0)
			{
				if(LevelId > 0)
				{
					arrDotPoints = getDotPointsDB(LevelId,0);
					arrDotPointsGray = getDotPointsDB(LevelId,1);
					NumDotsGray = arrDotPointsGray.Length;
					NumDots = arrDotPoints.Length;
#if (DESIGNER)
					HerbertX = getRoboXY(LevelId).X / GlobalData.ZoomInZoomOut;
					HerbertY = getRoboXY(LevelId).Y / GlobalData.ZoomInZoomOut;
#endif					
				}
				else
					arrDotPoints = getDotPoints(LevelId);
				
				arrTargetAchived = new bool[NumDots];
			}
			if(NumHurdles != 0)
			{
				//if(LevelNo > 10)
				if(LevelId > 0)
				{
					arrLinePoints = getLinePointsDB(LevelId);
					//NumHurdles = arrLinePoints.Length;
				}
				else
				{
					arrLinePoints = getLinePoints(LevelId);
				}
			}
			arrTargetAchived = new bool[NumDots];
			for(int i = 0; i < NumDots; i++)
			{
				//set all traget achived flag to false.
				arrTargetAchived[i] = false;
			}
            
            
			LevelImg = new Bitmap(208*GlobalData.ZoomInZoomOut, 208*GlobalData.ZoomInZoomOut);
			drawLevel(LevelId);
			scoreCurrently = 0;
			RNG1 = 12*GlobalData.ZoomInZoomOut;
			//if(NumDots != temp_numDotsPressed)
			temp_numDotsPressed = 0;
		}
		

#if(CONTEST)
		/// <summary>
		/// Resets the game board to its previous state, i.e. after level change.
		/// </summary>
		/// <param name="LevelNo">Level Number whose board needs to be reset.</param>
		public void ResetLevelState(int LevelId)
		{
			
			if(NumDots != 0)
			{
				//if(LevelNo > 10)
				if(LevelId > 0)
				{
					arrDotPoints = getDotPointsDB(LevelId,0);
					arrDotPointsGray = getDotPointsDB(LevelId,1);
					NumDotsGray = arrDotPointsGray.Length;
					NumDots = arrDotPoints.Length;					
				}
				else
					arrDotPoints = getDotPoints(LevelId);
				
				//arrTargetAchived = new bool[NumDots];
			}
			if(NumHurdles != 0)
			{
				//if(LevelNo > 10)
				if(LevelId > 0)
					arrLinePoints = getLinePointsDB(LevelId);
				else
					arrLinePoints = getLinePoints(LevelId);
			}
			/*for(int i = 0; i < NumDots; i++)
			{
				//set all traget achived flag to false.
				arrTargetAchived[i] = false;
			}*/
			LevelImg = new Bitmap(208*GlobalData.ZoomInZoomOut, 208*GlobalData.ZoomInZoomOut);
			drawLevel(LevelId);

			for(int i=0; i<NumDots; i++)
			{
				if(arrTargetAchived[i] == true)
				{					
					dotFill(arrDotPoints[i]);	
				}
			}
			//scoreCurrently = 0;
			RNG1 = 12*GlobalData.ZoomInZoomOut;
			/*if(NumDots != temp_numDotsPressed)
				temp_numDotsPressed = 0;*/
		}

#endif
		public void resetLevelDesigner(int LevelId)
		{
			arrTargetAchived = new bool[NumDots];
			for(int i = 0; i < NumDots; i++)
			{
				//set all traget achived flag to false.
				arrTargetAchived[i] = false;
			}
            ///aded By rajesh
            //getNonSavedDesignData();
            //updateBuffer();
            //saveUpdatedLevelData(iLevelId);
#if(PATTERN_GENERATER)
            if (GlobalData.iPatternId == "-1" || !(IsValidPatternImage))
            {
#endif
                LevelImg = new Bitmap(208 * GlobalData.ZoomInZoomOut, 208 * GlobalData.ZoomInZoomOut);
                drawLevel(LevelId);
#if(PATTERN_GENERATER)
            }
#endif
                scoreCurrently = 0;
			RNG1 = 12*GlobalData.ZoomInZoomOut;
			//if(NumDots != temp_numDotsPressed)
			temp_numDotsPressed = 0;
			levelScore = 0;
			BestLevelScore = 0;
		}


		/// <summary>
		/// fill a dot when herbert reaches to this target.
		/// </summary>
		/// <param name="fill">this is (x,y) location of the (LEFT,TOP) of this target.</param>
		public void dotFill(Point fill)
		{			
			
			lock(LevelImg)
			{
				int zizo = GlobalData.ZoomInZoomOut;
				Graphics bufferGraphics = Graphics.FromImage(LevelImg);
				
				if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
				{
					bufferGraphics.FillEllipse(System.Drawing.Brushes.Black,fill.X*zizo-zizo,fill.Y*zizo-zizo+1,4*zizo-BUTTONOFFSET,4*zizo-BUTTONOFFSET);
					
				}
				else
					bufferGraphics.FillEllipse(System.Drawing.Brushes.Black,fill.X*zizo-zizo,fill.Y*zizo-zizo,4*zizo,4*zizo);
				bufferGraphics.Dispose();		
				bufferGraphics = null;
			}
		}


		/// <summary>
		/// draw level board 
		/// </summary>
		/// <param name="LevelNo">Level Number whose board needs to be drawn</param>
		public void drawLevel(int LevelId)
		{		
			lock(LevelImg)
			{
				int zizo = GlobalData.ZoomInZoomOut;
				Graphics bufferGraphics = Graphics.FromImage(LevelImg);
				bufferGraphics.FillRectangle(System.Drawing.Brushes.White, 0, 0, 208*zizo, 208*zizo);
				
				//if level is not 0 then draw smalldots as well as buttons and hurdles if present.
				for(int i = 0; i < HConstants.GRIDDOTSPERLINE; i++)
					for(int j = 0; j < HConstants.GRIDDOTSPERLINE; j++)
					{						
						if(GlobalData.ZoomInZoomOut == HConstants.ZOOMOUT)
							bufferGraphics.FillEllipse(System.Drawing.Brushes.Black,(i)*HConstants.DOTSPACE*zizo+6.0F,(j)*HConstants.DOTSPACE*zizo+6.0F,DOTRADIOUS,DOTRADIOUS);
						else
							bufferGraphics.FillEllipse(System.Drawing.Brushes.Black,(i)*HConstants.DOTSPACE*zizo+6.0F*2+0.5F,(j)*HConstants.DOTSPACE*zizo+6.0F*2+0.5F,2.5F,2.5F);

					}
				//2005-12-06 changes for save path in memory.
#if(PATTERN_GENERATER)
                //if(GlobalData.iPatternId!="-1"&& IsPatternEnabled )
                if (IsPatternEnabled)
                    fDisplayStoredPath();
#endif
#if(CONTEST)
                fDisplayStoredPath();
#endif
                Pen BP = new Pen(Color.Black,1);
				Pen BP1 = new Pen(Color.Black,1.5F);
				
				for(int i = 0; i < NumDots; i++)
				{	
					if(arrDotPoints[i].X != -1 && arrDotPoints[i].Y != -1)
					{
						if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
						{
							bufferGraphics.FillEllipse(System.Drawing.Brushes.White,arrDotPoints[i].X*zizo-zizo,arrDotPoints[i].Y*zizo-zizo+1,4*zizo-BUTTONOFFSET,4*zizo-BUTTONOFFSET);
							bufferGraphics.DrawEllipse(BP1,arrDotPoints[i].X*zizo-zizo,arrDotPoints[i].Y*zizo-zizo+1,4*zizo-BUTTONOFFSET,4*zizo-BUTTONOFFSET);
						}
						else
						{
							bufferGraphics.FillEllipse(System.Drawing.Brushes.White,arrDotPoints[i].X-1,arrDotPoints[i].Y-1,4*GlobalData.ZoomInZoomOut,4*GlobalData.ZoomInZoomOut);
							bufferGraphics.DrawEllipse(BP,arrDotPoints[i].X-1,arrDotPoints[i].Y-1, 4*GlobalData.ZoomInZoomOut,4*GlobalData.ZoomInZoomOut);
						}
					}
				}
				
				using(SolidBrush forgrey = new SolidBrush(System.Drawing.Color.Gray))
				{
					for(int i = 0; i < NumDotsGray; i++)
					{	
						if(arrDotPointsGray[i].X != -1 && arrDotPointsGray[i].Y != -1)
						{
							if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
							{								
								bufferGraphics.FillEllipse(forgrey,arrDotPointsGray[i].X*zizo-zizo,arrDotPointsGray[i].Y*zizo-zizo+1,4*zizo-BUTTONOFFSET,4*zizo-BUTTONOFFSET);
								bufferGraphics.DrawEllipse(BP1,arrDotPointsGray[i].X*zizo-zizo,arrDotPointsGray[i].Y*zizo-zizo+1,4*zizo-BUTTONOFFSET,4*zizo-BUTTONOFFSET);
							}
							else
							{
								bufferGraphics.FillEllipse(System.Drawing.Brushes.Gray,arrDotPointsGray[i].X-1,arrDotPointsGray[i].Y-1,4*GlobalData.ZoomInZoomOut,4*GlobalData.ZoomInZoomOut);
								bufferGraphics.DrawEllipse(BP,arrDotPointsGray[i].X-1,arrDotPointsGray[i].Y-1, 4*GlobalData.ZoomInZoomOut,4*GlobalData.ZoomInZoomOut);
							}
						}
					}
				}
				
				//BP.Dispose();
				BP.Width = 2+GlobalData.ZoomInZoomOut;
				//Pen LBP = new Pen(Color.Black,2+GlobalData.ZoomInZoomOut);
				for(int i = 0; i < NumHurdles; i++)
				{
					if(arrLinePoints[i,0].X != -1 && arrLinePoints[i,0].Y != -1 && arrLinePoints[i,1].Y != -1 && arrLinePoints[i,1].Y != -1)
					{
						if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
						{
							if(arrLinePoints[i,0].X == arrLinePoints[i,1].X)
								if(arrLinePoints[i,0].Y > arrLinePoints[i,1].Y)
									bufferGraphics.DrawLine(BP,arrLinePoints[i,0].X*zizo+2,arrLinePoints[i,0].Y*zizo+1+zizo,arrLinePoints[i,1].X*zizo+2,arrLinePoints[i,1].Y*zizo-2+2);
								else
									bufferGraphics.DrawLine(BP,arrLinePoints[i,0].X*zizo+2,arrLinePoints[i,0].Y*zizo-2+2,arrLinePoints[i,1].X*zizo+2,arrLinePoints[i,1].Y*zizo+zizo+1);
							else
								if(arrLinePoints[i,0].X > arrLinePoints[i,1].X)
								bufferGraphics.DrawLine(BP,arrLinePoints[i,0].X*zizo+zizo+1,arrLinePoints[i,0].Y*zizo+2,arrLinePoints[i,1].X*zizo-2+2,arrLinePoints[i,1].Y*zizo+2);
							else
								bufferGraphics.DrawLine(BP,arrLinePoints[i,0].X*zizo-2+2,arrLinePoints[i,0].Y*zizo+2,arrLinePoints[i,1].X*zizo+zizo+1,arrLinePoints[i,1].Y*zizo+2);
						}
						else
						{
							if(arrLinePoints[i,0].X == arrLinePoints[i,1].X)
								if(arrLinePoints[i,0].Y > arrLinePoints[i,1].Y)
									bufferGraphics.DrawLine(BP,arrLinePoints[i,0].X+1,arrLinePoints[i,0].Y+1+1,arrLinePoints[i,1].X+1,arrLinePoints[i,1].Y+1);
								else
									bufferGraphics.DrawLine(BP,arrLinePoints[i,0].X+1,arrLinePoints[i,0].Y+1,arrLinePoints[i,1].X+1,arrLinePoints[i,1].Y+1+1);
							else
								if(arrLinePoints[i,0].X > arrLinePoints[i,1].X)
								bufferGraphics.DrawLine(BP,arrLinePoints[i,0].X+1+1,arrLinePoints[i,0].Y+1,arrLinePoints[i,1].X+1,arrLinePoints[i,1].Y+1);
							else
								bufferGraphics.DrawLine(BP,arrLinePoints[i,0].X+1,arrLinePoints[i,0].Y+1,arrLinePoints[i,1].X+1+1,arrLinePoints[i,1].Y+1);
						}
					}
				}
				
				//			}	
				//LBP.Dispose();
				BP.Dispose();
				BP1.Dispose();
				bufferGraphics.Dispose();
				BP = null;
				BP1 = null;
				bufferGraphics = null;
			}
		}


		/// <summary>
		/// check to see if robo is reached to a target.
		/// and preforms different operations as per the 
		/// target type (grey or white)
		/// </summary>
		/// <param name="LevelNo">Level Number for which this check is going on.</param>
		/// <param name="curPosX">Herbert X position</param>
		/// <param name="curPosY">Herbert Y position</param>
		/// <param name="herbertBoard">Herbert boards refrence</param>
		/// <returns></returns>
		//		public bool CheckforDot(int LevelId,int curPosX, int curPosY, ref Panel herbertBoard)
		//		{		
		public bool CheckforDot(int LevelId,int curPosX, int curPosY, ref cHerbertBoard herbertboard, int destPosX, int destPosY, int TotalTimeInSecRemaining,int GameInterval, DateTime InstanceStartTime)
		{		
			int zizo = GlobalData.ZoomInZoomOut;
			int RNGX = 0, RNGY = 0;
			if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
			{
				RNGX = 3*zizo+1;
				RNGY = 3*zizo+2;
			}
			else
			{
				RNGX = 3;
				RNGY = 3;
			}

			for(int i = 0; i < NumDots; i++)
			{
				//range is taken 3 because herberts center point and buttons center points are not
				//same. Points are  not same to place herbert at proper position.
				if(curPosX == destPosX && curPosY == destPosY && HerbertMain.herbertState >= 7)
					if((Math.Abs(curPosX - arrDotPoints[i].X*zizo) < RNGX) && (Math.Abs(curPosY - arrDotPoints[i].Y*zizo) < RNGY))
					{
						if(arrTargetAchived[i] == false)
						{					
							dotFill(arrDotPoints[i]);	
							//Changes for 15/04/2005 numDotsPressed
							if(NumDots>temp_numDotsPressed)
								temp_numDotsPressed++;	
							//changes on 03/01/05
# if(OldScoringLogic)
						if(scoringLogic == 1)
						{
#endif
							scoreCurrently += calPerBttnPoints(LevelId);
							if(scoreCurrently >= levelScore)
							{	
								if(!IsLevelFinishedPersistant)
									iBestTimeSpent = iTotalTimeSpent;
								if(levelScore <= scoreCurrently)
									levelScore = scoreCurrently; 
								//								if(scoreCurrently >= BestLevelScore)
								//								{
								if(scoreCurrently > BestLevelScore)
								{
									totalScore -= BestLevelScore;
									totalScore += scoreCurrently;
									BestLevelScore = levelScore;
									//bestTimeToSolve = timeSpent;
									
									iBestProgram = iCurrentProgram;
									iBestTimeSpent = iTotalTimeSpent;
									iBestGos  = iTotalGos;
									iBestBytes = charused;
									iBestCharsTyped = iTotalCharsTyped;
									iBestDate = InstanceStartTime.AddSeconds(TotalTimeInSecRemaining - GameInterval);
									iBestVisits = pNumVisits;
									
									isLevelDataChanged = true;

									//									if(charused <= leastCharsUsed || leastCharsUsed == 0)
									//leastCharsUsed = charused;
									
									//Changes for 15/04/2005 numDotsPressed
									numDotsPressed = temp_numDotsPressed;
								}
							}
# if(OldScoringLogic)
						}

						else
						{
							scoreCurrently += POINTSPERBTTN*getLevelNumber(LevelId);
							if(scoreCurrently >= levelScore)
							{				
								if(!IsLevelFinishedPersistant)
									iBestTimeSpent = timeSpent;
								if(levelScore <= scoreCurrently)
									levelScore = scoreCurrently; 
								if(scoreCurrently > BestLevelScore)
								{
									totalScore -= BestLevelScore;
									totalScore += scoreCurrently;
									BestLevelScore = levelScore;
									iBestTimeSpent = timeSpent;
									iBestProgram = iCurrentProgram;									
									iBestVisits = pNumVisits;
//									if(charused <= leastCharsUsed || leastCharsUsed == 0)
									iBestBytes = charused;
									//Changes for 15/04/2005 numDotsPressed
									numDotsPressed = temp_numDotsPressed;
								}
							}
						}
#endif
							arrTargetAchived[i] = true;
							//Changes for 15/04/2005 numDotsPressed
							//						if(NumDots>temp_numDotsPressed)
							//							temp_numDotsPressed++;
							//						
							//						if(numDotsPressed <= temp_numDotsPressed)
							//							numDotsPressed = temp_numDotsPressed;
							//vinay02/09/05HerbertBoard.Refresh();;
							return true;
						}
						else
							dotFill(arrDotPoints[i]);
					}					
			}
			//check for grey buttons. if any button is achived.
			//vinay02/09/05
			bool done = false;
			for(int i = 0; i < NumDots; i++)
			{
				done = arrTargetAchived[i];
				if(done)
					break;				
			}
			//vinay02/09/05
			if(done)
			{
				for(int i = 0; i < NumDotsGray; i++)
				{
					if(curPosX == destPosX && curPosY == destPosY && HerbertMain.herbertState >= 7)
						if((Math.Abs(curPosX - arrDotPointsGray[i].X*zizo) < RNGX) && (Math.Abs(curPosY - arrDotPointsGray[i].Y*zizo) < RNGY))
						{					
							resetDueToGray();
                            //Added By Rajesh 23/07/07
#if(DESIGNER)
                            // added by Nikhil Kardale on 02/04/2008 to stop counting white button hits after a level has been completely solved 
                            if (!IsLevelFinishedPersistant)
                                NumWDotPressedBeforeGDot += temp_numDotsPressed;
#endif
                            //end
#if(DESIGNER)							
                            // added by Nikhil Kardale on 02/04/2008 to stop counting gray button hits after a level has been completely solved
                            if (!IsLevelFinishedPersistant)
                                NumGrayButtonHits++;
#endif

							herbertboard.Refresh();
							temp_numDotsPressed = 0;

							if(IsLevelFinished)
							{
								IsLevelFinished = false;
							}
							//totalScore -= levelScore;
							scoreCurrently = 0;
							//levelScore = 0;
							//BestLevelScore = 0;
							return true;														
						}					
				}
			}
			return false;
		}

		
		/// <summary>
		/// this fucntion resets all target achived and resets the currently score.
		/// </summary>
		private void resetDueToGray()
		{
			lock(LevelImg)
			{
				int zizo = GlobalData.ZoomInZoomOut;
				Graphics bufferGraphics = Graphics.FromImage(LevelImg);
				
				Pen BP = new Pen(Color.Black,1);
				Pen BP1 = new Pen(Color.Black,1.5F);
				for(int i = 0; i < NumDots; i++)
				{						
					if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
					{
						bufferGraphics.FillEllipse(System.Drawing.Brushes.White,arrDotPoints[i].X*zizo-zizo,arrDotPoints[i].Y*zizo-zizo+1,4*zizo-BUTTONOFFSET,4*zizo-BUTTONOFFSET);
						bufferGraphics.DrawEllipse(BP1,arrDotPoints[i].X*zizo-zizo,arrDotPoints[i].Y*zizo-zizo+1,4*zizo-BUTTONOFFSET,4*zizo-BUTTONOFFSET);
					}
					else
					{
						bufferGraphics.FillEllipse(System.Drawing.Brushes.White,arrDotPoints[i].X-1,arrDotPoints[i].Y*zizo-1,4*zizo,4*zizo);
						bufferGraphics.DrawEllipse(BP,arrDotPoints[i].X-1,arrDotPoints[i].Y-1, 4*zizo,4*zizo);
					}
					arrTargetAchived[i] = false;
				}
				BP.Dispose();
				BP1.Dispose();
				bufferGraphics.Dispose();
				BP = null;
				BP1 = null;
				bufferGraphics = null;
			}
		}

		
		/// <summary>
		/// check to see if robo is hitting a line or hurdle.
		/// </summary>
		/// <param name="curDir">Current direction of herbert<see cref="HerbertMain:CurDir"/></param>
		/// <param name="LevelNo">Level No for which this check is going on.</param>
		/// <param name="curPosX">Current position in X direction of herbert</param>
		/// <param name="curPosY">Current position in Y direction of herbert</param>
		/// <returns>returns true if herbert is hitting a wall else false.</returns>
		public bool isHurdle(int curDir, int LevelId, int curPosX, int curPosY)
		{	
			int zizo = GlobalData.ZoomInZoomOut;
			if(GlobalData.ZoomInZoomOut == HConstants.ZOOMOUT)
			{				
				switch(curDir)
				{
						//Up dir
					case 0:		
						//check for all hurdles at this level.
					
						for(int i = 0; i < NumHurdles; i++)
						{
							//check to see if any lines y point is within the range of herbert's y point, range is used here
							//because curPos$ is the center point of herbert not the top point. so even if
							//herbert touches the wall it's center point is still some distance away from 
							//hurdle.
							if((Math.Abs(curPosY - arrLinePoints[i,0].Y)<RNG1-5*GlobalData.ZoomInZoomOut) && curPosY>arrLinePoints[i,0].Y)
							{
								//dynamic range is the line length	
								DyRNG = (Math.Abs(arrLinePoints[i,0].X - arrLinePoints[i,1].X));
								//for all lines whose y point is within the range of herbert's y point,
								//check to see if any of these lines x points are with in the range of
								//herbert's x point.
								//this if condition takes care of horizontal lines
								if(Math.Abs(curPosX - arrLinePoints[i,0].X)<DyRNG && Math.Abs(curPosX - arrLinePoints[i,1].X)<DyRNG && arrLinePoints[i,0].X != arrLinePoints[i,1].X)
								{								
									return true;
								}
									//these too if conditions takes care  of the case when herbert is not 
									//hiting the center of the line but hitting the end of line of a horizontal line.
								else if(Math.Abs(curPosX - arrLinePoints[i,0].X)<RNG1-6*GlobalData.ZoomInZoomOut)
									return true;
								else if(Math.Abs(curPosX - arrLinePoints[i,1].X)<RNG1-6*GlobalData.ZoomInZoomOut)
									return true;

							}
							//this if condition takes care of vertical lines, means herbert hits the end or staring points of line.
							if(curPosX+2*GlobalData.ZoomInZoomOut == arrLinePoints[i,0].X && arrLinePoints[i,0].X == arrLinePoints[i,1].X && curPosY>arrLinePoints[i,0].Y)
							{							
								if((Math.Abs(curPosY - arrLinePoints[i,0].Y)<RNG1-5*GlobalData.ZoomInZoomOut) || (Math.Abs(curPosY - arrLinePoints[i,1].Y)<RNG1-5*GlobalData.ZoomInZoomOut))
									return true;							
							}
						}
					
					
						break;
						//Right dir
					case 1:	
						//check for all hurdles at this level.		
						for(int i = 0; i < NumHurdles; i++)
						{
							//check to see if any lines x point is within the range of herbert's x point, range is used here
							//because curPos$ is the center point of herbert not the top point. so even if
							//herbert touches the wall it's center point is still some distance away from 
							//hurdle.
							if((Math.Abs(curPosX - arrLinePoints[i,0].X)<RNG1-GlobalData.ZoomInZoomOut) && curPosX < arrLinePoints[i,0].X)
							{
								//dynamic range is the line length	
								DyRNG = (Math.Abs(arrLinePoints[i,0].Y - arrLinePoints[i,1].Y));
								//for all lines whose y point is within the range of herbert's x point,
								//check to see if any of these lines x points are with in the range of
								//herbert's x point.
								//this if condition takes care of vertical lines
								if(Math.Abs(curPosY - arrLinePoints[i,0].Y)<DyRNG && Math.Abs(curPosY - arrLinePoints[i,1].Y)<DyRNG && arrLinePoints[i,0].Y != arrLinePoints[i,1].Y)
								{								
									return true;
								}
									//these too if conditions takes care  of the case when herbert is not 
									//hiting the center of the line but hitting the end of line of a vertical line.
								else if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
								{
									if(Math.Abs(curPosY - arrLinePoints[i,0].Y)<RNG1-7*GlobalData.ZoomInZoomOut)
										return true;
									else if(Math.Abs(curPosY - arrLinePoints[i,1].Y)<RNG1-7*GlobalData.ZoomInZoomOut)
										return true;
								}
								else
								{
									if(Math.Abs(curPosY - arrLinePoints[i,0].Y)<RNG1-6)
										return true;
									else if(Math.Abs(curPosY - arrLinePoints[i,1].Y)<RNG1-6)
										return true;
								}
							}	
							//this if condition takes care of horizontal lines, means herbert hits the end or staring points of line.
							if(curPosY+2*GlobalData.ZoomInZoomOut == arrLinePoints[i,0].Y && arrLinePoints[i,0].Y == arrLinePoints[i,1].Y && curPosX < arrLinePoints[i,0].X)
							{
								if(((Math.Abs(curPosX - arrLinePoints[i,0].X)<RNG1-2*GlobalData.ZoomInZoomOut) || (Math.Abs(curPosX - arrLinePoints[i,1].X)<RNG1-2*GlobalData.ZoomInZoomOut)) && curPosX < arrLinePoints[i,0].X)
									return true;
							}
						}
						break;
						//Down Dir
					case 2:
						//check for all hurdles at this level.
						for(int i = 0; i < NumHurdles; i++)
						{
							//check to see if any lines y point is within the range of herbert's y point, range is used here
							//because curPos$ is the center point of herbert not the top point. so even if
							//herbert touches the wall it's center point is still some distance away from 
							//hurdle.
							if((Math.Abs(curPosY - arrLinePoints[i,0].Y)<RNG1) && curPosY<arrLinePoints[i,0].Y)
							{
								//dynamic range is the line length	
								DyRNG = (Math.Abs(arrLinePoints[i,0].X - arrLinePoints[i,1].X));
								//for all lines whose y point is within the range of herbert's y point,
								//check to see if any of these lines x points are with in the range of
								//herbert's x point.
								//this if condition takes care of horizontal lines
								if(Math.Abs(curPosX - arrLinePoints[i,0].X)<DyRNG && Math.Abs(curPosX - arrLinePoints[i,1].X)<DyRNG && arrLinePoints[i,0].X != arrLinePoints[i,1].X)
								{								
									return true;
								}
									//these too if conditions takes care  of the case when herbert is not 
									//hiting the center of the line but hitting the end of line of a horizontal line.
								else if(Math.Abs(curPosX - arrLinePoints[i,0].X)<RNG1-6*GlobalData.ZoomInZoomOut)
									return true;
								else if(Math.Abs(curPosX - arrLinePoints[i,1].X)<RNG1-6*GlobalData.ZoomInZoomOut)
									return true;
							}	
							//this if condition takes care of vertical lines, means herbert hits the end or staring points of line.
							if(curPosX+2*GlobalData.ZoomInZoomOut == arrLinePoints[i,0].X && arrLinePoints[i,0].X == arrLinePoints[i,1].X && arrLinePoints[i,0].Y>curPosY)
							{
								if((Math.Abs(curPosY - arrLinePoints[i,0].Y)<RNG1) || (Math.Abs(curPosY - arrLinePoints[i,1].Y)<RNG1))
									return true;
							}
						}
						break;
						//Left Dir
					case 3:
						for(int i = 0; i < NumHurdles; i++)
						{
							//check to see if any lines x point is within the range of herbert's x point, range is used here
							//because curPos$ is the center point of herbert not the top point. so even if
							//herbert touches the wall it's center point is still some distance away from 
							//hurdle.
							if((Math.Abs(curPosX - arrLinePoints[i,0].X)<RNG1-4*GlobalData.ZoomInZoomOut) && curPosX > arrLinePoints[i,0].X)
							{
								//dynamic range is the line length	
								DyRNG = (Math.Abs(arrLinePoints[i,0].Y - arrLinePoints[i,1].Y));							
								//for all lines whose y point is within the range of herbert's y point,
								//check to see if any of these lines x points are with in the range of
								//herbert's x point.
								//this if condition takes care of horizontal lines
								if(Math.Abs(curPosY - arrLinePoints[i,0].Y)<DyRNG && Math.Abs(curPosY - arrLinePoints[i,1].Y)<DyRNG && arrLinePoints[i,0].Y != arrLinePoints[i,1].Y)
								{								
									return true;
								}
									//these too if conditions takes care  of the case when herbert is not 
									//hiting the center of the line but hitting the end of line of a vertical line.
								else if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
								{
									if(Math.Abs(curPosY - arrLinePoints[i,0].Y)<RNG1-7*GlobalData.ZoomInZoomOut)
										return true;
									else if(Math.Abs(curPosY - arrLinePoints[i,1].Y)<RNG1-7*GlobalData.ZoomInZoomOut)
										return true;
								}
								else
								{
									if(Math.Abs(curPosY - arrLinePoints[i,0].Y)<RNG1-6)
										return true;
									else if(Math.Abs(curPosY - arrLinePoints[i,1].Y)<RNG1-6)
										return true;
								}
							}
							//this if condition takes care of horizontal lines, means herbert hits the end or staring points of line.
							if(curPosY+2*GlobalData.ZoomInZoomOut == arrLinePoints[i,0].Y && arrLinePoints[i,0].Y == arrLinePoints[i,1].Y && arrLinePoints[i,0].X<curPosX)
							{
								if(((Math.Abs(curPosX - arrLinePoints[i,0].X)<RNG1-5*GlobalData.ZoomInZoomOut) || (Math.Abs(curPosX - arrLinePoints[i,1].X)<RNG1-5*GlobalData.ZoomInZoomOut)) )
									return true;
							}
						}	
					
						break;
				}
			}
			else
			{
				switch(curDir)
				{
						//Up dir
					case 0:		
						//check for all hurdles at this level.
					
						for(int i = 0; i < NumHurdles; i++)
						{
							//check to see if any lines y point is within the range of herbert's y point, range is used here
							//because curPos$ is the center point of herbert not the top point. so even if
							//herbert touches the wall it's center point is still some distance away from 
							//hurdle.
							if((Math.Abs(curPosY - arrLinePoints[i,0].Y*zizo)<RNG1-5-10) && curPosY>arrLinePoints[i,0].Y*zizo)
							{
								//dynamic range is the line length	
								DyRNG = (Math.Abs(arrLinePoints[i,0].X*zizo - arrLinePoints[i,1].X*zizo));
								//for all lines whose y point is within the range of herbert's y point,
								//check to see if any of these lines x points are with in the range of
								//herbert's x point.
								//this if condition takes care of horizontal lines
								if(Math.Abs(curPosX - arrLinePoints[i,0].X*zizo)<DyRNG && Math.Abs(curPosX - arrLinePoints[i,1].X*zizo)<DyRNG && arrLinePoints[i,0].X*zizo != arrLinePoints[i,1].X*zizo)
								{								
									return true;
								}
									//these too if conditions takes care  of the case when herbert is not 
									//hiting the center of the line but hitting the end of line of a horizontal line.
								else if(Math.Abs(curPosX - arrLinePoints[i,0].X*zizo)<RNG1-6*zizo - 4)
									return true;
								else if(Math.Abs(curPosX - arrLinePoints[i,1].X*zizo)<RNG1-6*zizo - 4)
									return true;

							}
							//this if condition takes care of vertical lines, means herbert hits the end or staring points of line.
							if(curPosX+2*zizo == arrLinePoints[i,0].X*zizo && arrLinePoints[i,0].X*zizo == arrLinePoints[i,1].X*zizo && curPosY>arrLinePoints[i,0].Y*zizo)
							{							
								if((Math.Abs(curPosY - arrLinePoints[i,0].Y*zizo)<RNG1-5*zizo-5) || (Math.Abs(curPosY - arrLinePoints[i,1].Y*zizo)<RNG1-5*zizo-5))
									return true;							
							}
						}
					
					
						break;
						//Right dir
					case 1:	
						//check for all hurdles at this level.		
						for(int i = 0; i < NumHurdles; i++)
						{
							//check to see if any lines x point is within the range of herbert's x point, range is used here
							//because curPos$ is the center point of herbert not the top point. so even if
							//herbert touches the wall it's center point is still some distance away from 
							//hurdle.
							if((Math.Abs(curPosX - arrLinePoints[i,0].X*zizo)<RNG1-zizo-1) && curPosX < arrLinePoints[i,0].X*zizo)
							{
								//dynamic range is the line length	
								DyRNG = (Math.Abs(arrLinePoints[i,0].Y*zizo - arrLinePoints[i,1].Y*zizo));
								//for all lines whose y point is within the range of herbert's x point,
								//check to see if any of these lines x points are with in the range of
								//herbert's x point.
								//this if condition takes care of vertical lines
								if(Math.Abs(curPosY - arrLinePoints[i,0].Y*zizo)<DyRNG && Math.Abs(curPosY - arrLinePoints[i,1].Y*zizo)<DyRNG && arrLinePoints[i,0].Y*zizo != arrLinePoints[i,1].Y*zizo)
								{								
									return true;
								}
									//these too if conditions takes care  of the case when herbert is not 
									//hiting the center of the line but hitting the end of line of a vertical line.
								else if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
								{
									if(Math.Abs(curPosY - arrLinePoints[i,0].Y*zizo)<RNG1-7*zizo-1)
										return true;
									else if(Math.Abs(curPosY - arrLinePoints[i,1].Y*zizo)<RNG1-7*zizo-1)
										return true;
								}
								else
								{
									if(Math.Abs(curPosY - arrLinePoints[i,0].Y*zizo)<RNG1-6)
										return true;
									else if(Math.Abs(curPosY - arrLinePoints[i,1].Y*zizo)<RNG1-6)
										return true;
								}
							}	
							//this if condition takes care of horizontal lines, means herbert hits the end or staring points of line.
							if(curPosY+2*zizo == arrLinePoints[i,0].Y*zizo && arrLinePoints[i,0].Y*zizo == arrLinePoints[i,1].Y*zizo && curPosX < arrLinePoints[i,0].X*zizo)
							{
								if(((Math.Abs(curPosX - arrLinePoints[i,0].X*zizo)<RNG1-2*zizo-1) || (Math.Abs(curPosX - arrLinePoints[i,1].X*zizo)<RNG1-2*zizo-1)) && curPosX < arrLinePoints[i,0].X*zizo)
									return true;
							}
						}
						break;
						//Down Dir
					case 2:
						//check for all hurdles at this level.
						for(int i = 0; i < NumHurdles; i++)
						{
							//check to see if any lines y point is within the range of herbert's y point, range is used here
							//because curPos$ is the center point of herbert not the top point. so even if
							//herbert touches the wall it's center point is still some distance away from 
							//hurdle.
							if((Math.Abs(curPosY - arrLinePoints[i,0].Y*zizo)<RNG1-1) && curPosY<arrLinePoints[i,0].Y*zizo)
							{
								//dynamic range is the line length	
								DyRNG = (Math.Abs(arrLinePoints[i,0].X*zizo - arrLinePoints[i,1].X*zizo));
								//for all lines whose y point is within the range of herbert's y point,
								//check to see if any of these lines x points are with in the range of
								//herbert's x point.
								//this if condition takes care of horizontal lines
								if(Math.Abs(curPosX - arrLinePoints[i,0].X*zizo)<DyRNG && Math.Abs(curPosX - arrLinePoints[i,1].X*zizo)<DyRNG && arrLinePoints[i,0].X*zizo != arrLinePoints[i,1].X*zizo)
								{								
									return true;
								}
									//these too if conditions takes care  of the case when herbert is not 
									//hiting the center of the line but hitting the end of line of a horizontal line.
								else if(Math.Abs(curPosX - arrLinePoints[i,0].X*zizo)<RNG1-6*zizo-1)
									return true;
								else if(Math.Abs(curPosX - arrLinePoints[i,1].X*zizo)<RNG1-6*zizo-1)
									return true;
							}	
							//this if condition takes care of vertical lines, means herbert hits the end or staring points of line.
							if(curPosX+2*zizo == arrLinePoints[i,0].X*zizo && arrLinePoints[i,0].X*zizo == arrLinePoints[i,1].X*zizo && arrLinePoints[i,0].Y*zizo>curPosY)
							{
								if((Math.Abs(curPosY - arrLinePoints[i,0].Y*zizo)<RNG1-1) || (Math.Abs(curPosY - arrLinePoints[i,1].Y*zizo)<RNG1-1))
									return true;
							}
						}
						break;
						//Left Dir
					case 3:
						for(int i = 0; i < NumHurdles; i++)
						{
							//check to see if any lines x point is within the range of herbert's x point, range is used here
							//because curPos$ is the center point of herbert not the top point. so even if
							//herbert touches the wall it's center point is still some distance away from 
							//hurdle.
							if((Math.Abs(curPosX - arrLinePoints[i,0].X*zizo)<RNG1-4-8) && curPosX > arrLinePoints[i,0].X*zizo)
							{
								//dynamic range is the line length	
								DyRNG = (Math.Abs(arrLinePoints[i,0].Y*zizo - arrLinePoints[i,1].Y*zizo));							
								//for all lines whose y point is within the range of herbert's y point,
								//check to see if any of these lines x points are with in the range of
								//herbert's x point.
								//this if condition takes care of horizontal lines
								if(Math.Abs(curPosY - arrLinePoints[i,0].Y*zizo)<DyRNG && Math.Abs(curPosY - arrLinePoints[i,1].Y*zizo)<DyRNG && arrLinePoints[i,0].Y*zizo != arrLinePoints[i,1].Y*zizo)
								{								
									return true;
								}
									//these too if conditions takes care  of the case when herbert is not 
									//hiting the center of the line but hitting the end of line of a vertical line.
								else if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
								{
									if(Math.Abs(curPosY - arrLinePoints[i,0].Y*zizo)<RNG1-7*zizo-3)
										return true;
									else if(Math.Abs(curPosY - arrLinePoints[i,1].Y*zizo)<RNG1-7*zizo-3)
										return true;
								}
								else
								{
									if(Math.Abs(curPosY - arrLinePoints[i,0].Y*zizo)<RNG1-6)
										return true;
									else if(Math.Abs(curPosY - arrLinePoints[i,1].Y*zizo)<RNG1-6)
										return true;
								}
							}
							//this if condition takes care of horizontal lines, means herbert hits the end or staring points of line.
							if(curPosY+2*zizo == arrLinePoints[i,0].Y*zizo && arrLinePoints[i,0].Y*zizo == arrLinePoints[i,1].Y*zizo && arrLinePoints[i,0].X*zizo<curPosX)
							{
								if(((Math.Abs(curPosX - arrLinePoints[i,0].X*zizo)<RNG1-5*zizo-3) || (Math.Abs(curPosX - arrLinePoints[i,1].X*zizo)<RNG1-5*zizo-3)) )
									return true;
							}
						}	
					
						break;
				}
			}
			return false;			
		}
		



		/// <summary>
		/// this function will save level program while navigating between levels.
		/// </summary>
		/// <param name="program">program for this level.</param>
		//		public void SaveLevelData(string program)
		//		{
		//			LevelProgram = program;
		//		}

		/// <summary>
		/// returns the best level program written for a perticular level.
		/// </summary>
		/// <returns>returns the best level program written for a perticular level.</returns>
		//		public string getBestLevelProgram()
		//		{
		//			if(bestLevelProgram != null)
		//				return bestLevelProgram;
		//
		//			return "";
		//		}


		/// <summary>
		/// this function retruns the level program if available.
		/// </summary>
		/// <returns>level program</returns>
		//		public string GetLevelProgram()
		//		{
		//			if(LevelProgram != null)
		//				return LevelProgram;
		//			
		//			return "";
		//		}

		
		/// <summary>
		/// returns the Max chars that can be used to write program for a level.
		/// </summary>
		/// <param name="LevelNo">level number for which max char is needed.</param>
		/// <returns>max chars or the level</returns>
		public int getMaxChar(int LevelId)
		{
			if(LevelId==0)
			{				
				return 0;
			}
			else if(MaxChars == -1)
			{
				DataRow[] rows = GlobalData.dsAllHData.Tables["tblLevelInfoMST"].Select("LevelId='"+LevelId.ToString()+"'");
				MaxChars = int.Parse(rows[0]["MaxChars"].ToString());
				return int.Parse(rows[0]["MaxChars"].ToString());
			}
			else
				return MaxChars;
		}
		

		public static int fgetLevelPoints(int LevelId)
		{	
			if(LevelId == 0)
				return 0;
			else
			{
				DataRow[] rows = GlobalData.dsAllHData.Tables["tblLevelInfoMST"].Select("LevelId='"+LevelId.ToString()+"'");
				return int.Parse(rows[0]["Points"].ToString());
			}
		}
	

		/// <summary>
		/// function to calculate per button points.
		/// </summary>
		/// <param name="LevelNo">Level No for which per button points needs to be calculated</param>
		/// <returns>the pre button points</returns>
		private int calPerBttnPoints(int LevelId)
		{
			int maxChar = 0;
			int LvlPoints = 0;

			if(GlobalData.HerbertMode == HMode.Designer)
				LvlPoints = GlobalData.DeginerLevelPoints;
			else
				LvlPoints = fgetLevelPoints(LevelId);

			if(LevelId == 0 && IsLevelUpdate == true)
				maxChar = CountChars();
			else
				maxChar = getMaxChar(LevelId);

			if(charused > 2*maxChar)
				PerBttnPtns = 0;
			else if(charused <= maxChar)
				PerBttnPtns = (int)LvlPoints/(2*NumDots);
			else
				//check, when this would execute?, Vijay
				PerBttnPtns = (int)LvlPoints*(2*maxChar - charused)/(2*NumDots*maxChar);

			return PerBttnPtns;
		}




		
#if(CONTEST)
		
		/***********************************************************************************************
		 * Added by: Vivek Balagangadharan
		 * Added On: 02-Mar-2006
		 * Function: CalcScore
		 * Parameters: Number of buttons Pressed, Bytes Used,Level Id
		 * Description: Function calculates the levelscore
		 * Purpose: This function is added as a part of a final check before save.
		 *			This patch is introduced for the maintenance on 02-Mar-2005 on wildnoodle.com/ic2006
		 ************************************************************************************************/
		public int CalcScore(int numbuttonsPressed, int bestBytes,int LevelId)
		{
			int maxChar = 0;
			int LvlPoints = 0;
			
			LvlPoints = fgetLevelPoints(LevelId);

			//getting the max chars to solve for the level
			if(LevelId != 0)
				maxChar = getMaxChar(LevelId);
			//if the level is solved
			if ((numbuttonsPressed == NumDots) && (bestBytes <= maxChar))
				return ((LvlPoints * maxChar) / bestBytes);
			//if the bestbytes is greater than twice of max chars to solve
			else if(bestBytes > 2*maxChar)
				PerBttnPtns = 0;
			//
			else if(bestBytes <= maxChar)
				PerBttnPtns = (int)LvlPoints/(2*NumDots);
			else				
				PerBttnPtns = (int)LvlPoints*(2*maxChar - bestBytes)/(2*NumDots*maxChar);

			return (PerBttnPtns * numbuttonsPressed);
		}
#endif
		/// <summary>
		/// function for checking if level is finished or if all the white buttons are achived.
		/// this fucntion also calculates the total score and level score.
		/// </summary>
		/// <param name="LevelNo">Level No of level for which this check will take place.</param>
		/// <returns>returns true if level is finished else false</returns>
		public bool chkLevelFinished(int LevelId, int TotalTimeInSecRemaining,int GameInterval,DateTime InstanceStartTime)
		{
			bool done = false;
			//check if all the targets are achieved
			for(int i = 0; i < NumDots; i++)
			{
				done = arrTargetAchived[i];
				if(!done)
					break;				
			}
			if(done)
			{	
				int t_levelScore = 0;
				
				int maxChar = 0;
				//if new scoring logic.
# if(OldScoringLogic)	
				if(scoringLogic == 1)
				{
#endif  
				int LvlPoints = 0;
				PerBttnPtns = calPerBttnPoints(LevelId);
				if(GlobalData.HerbertMode == HMode.Designer)
				{
					//LvlPoints = GlobalData.DeginerLevelPoints;
#if(DESIGNER)
				if(IsLevelChanged)
					{
						LvlPoints = GlobalData.DeginerLevelPoints;
					}
					else
						LvlPoints = fgetLevelPoints(LevelId);
#else
					LvlPoints = GlobalData.DeginerLevelPoints;
#endif
				}
				else
					LvlPoints = fgetLevelPoints(LevelId);

				//check, this could be CurrentLevelIndex
				if(LevelId == 0 && IsLevelUpdate == true)
					maxChar = CountChars();
				else
					maxChar = getMaxChar(LevelId);

				//check, temp_numDotsPressed >= NumDots not required
				if(charused <= maxChar && temp_numDotsPressed >= NumDots)
				{
					try
					{
						t_levelScore = LvlPoints*maxChar/charused;
					}
					catch
					{
					}
					scoreCurrently = t_levelScore;
					IsLevelFinished = true;
				}
				else 
				{
					t_levelScore = temp_numDotsPressed*PerBttnPtns;
					scoreCurrently = t_levelScore;
				}
# if(OldScoringLogic)
				}

				else
				{
					if(getLevelNumber(LevelId) == 0 && IsLevelUpdate == true)
						maxChar = CountChars();
					else
						maxChar = getMaxChar(LevelId);
					if(maxChar < charused)
					{
						t_levelScore = NumDots*POINTSPERBTTN*getLevelNumber(LevelId) + getLevelNumber(LevelId)*POINTSPERBTTN*POINTSPERBTTN;
						scoreCurrently = t_levelScore;						
					}
					else if(maxChar == charused)
					{
						t_levelScore = NumDots*POINTSPERBTTN*getLevelNumber(LevelId) + 2*getLevelNumber(LevelId)*POINTSPERBTTN*POINTSPERBTTN;
						scoreCurrently = t_levelScore;
						IsLevelFinished = true;
					}
					else if(maxChar > charused)
					{
						t_levelScore = NumDots*POINTSPERBTTN*getLevelNumber(LevelId) + 2*getLevelNumber(LevelId)*POINTSPERBTTN*POINTSPERBTTN + (maxChar - charused)*POINTSPERBTTN*getLevelNumber(LevelId);
						scoreCurrently = t_levelScore;
						IsLevelFinished = true;
					}
				}
# endif
#if(DESIGNER)
				/*==============================================================================
						*  Added By : Vivek Balagangadharan
						*  Description : setiing the variable which indicates that the level is solved
						*  Added On : 14-Apr-2006
						* ==============================================================================*/
				isLevelSolutionValid = true;
				/*End*/
#endif
					
				if(t_levelScore > BestLevelScore)
				{
					totalScore -= BestLevelScore;
					totalScore += t_levelScore;
					BestLevelScore = t_levelScore;
					levelScore = t_levelScore;	

					iBestProgram = iCurrentProgram;
					iBestGos = iTotalGos;
					iBestCharsTyped = iTotalCharsTyped;
					iBestDate = InstanceStartTime.AddSeconds(TotalTimeInSecRemaining - GameInterval);
					iBestBytes = charused;
					iBestTimeSpent = iTotalTimeSpent;
					iBestVisits = pNumVisits;

					isLevelDataChanged = true;

					//					if(IsLevelFinishedPersistant)
					//						numSolveAtmpt++;
					if(maxChar >= charused)
					{

						IsLevelFinishedPersistant = true;
					}
					//					if(charused <= leastCharsUsed || leastCharsUsed == 0)
					//leastCharsUsed = charused;
					
					numDotsPressed = temp_numDotsPressed;
					if(iFirstProgram == "" && IsLevelFinishedPersistant)
					{
						iFirstProgram = iCurrentProgram;
						iFirstBytes = charused;
						iFirstCharsTyped = iTotalCharsTyped;
						iFirstDate = InstanceStartTime.AddSeconds(TotalTimeInSecRemaining - GameInterval);
						iFirstGos = iTotalGos;
						iFirstTimeSpent = iTotalTimeSpent;
						iFirstLevelScore = t_levelScore;
						iFirstVisits = pNumVisits;
					}
					//blnPersistData = true;
				}			
			}	

			return done;
		}
		

		/// <summary>
		/// count the number of chars in the program.
		/// </summary>
		/// <returns>number of chars in program</returns>
		private int CountChars()
		{
			
			if(iCurrentProgram.Length == 0)
				return 0;
			char pc;
			int iCount,i,iMaxLineLen,iLineLen;
			long lTemp;
			
			iMaxLineLen=iLineLen=iCount=0;
			
			lTemp=iCurrentProgram.Length;
			char[] txtCode = iCurrentProgram.ToCharArray();
			
			if(lTemp >= 0)
			{
				i=0;
				while(i < lTemp)
				{
					pc=txtCode[i];
					if(Char.IsDigit(pc))
					{
						System.Text.StringBuilder sbNumber = new System.Text.StringBuilder();
						while(Char.IsDigit(txtCode[i]))
						{
							sbNumber.Append(txtCode[i]);
							i++;
							if(i == lTemp)
								break;
						}
						iCount++;
						i--;						
					}
					if (pc <= 'Z' && pc >= 'A' )
					{
						iCount++;
					}
					if (pc <= 'z' && pc >= 'a')
					{
						iCount++;	
					}
					
					i++;
					
				}
			}
			/*Old char counting logic */
			/*if(lTemp >= 0)
				for(i = 0;i < lTemp;i++)
				{
					pc=txtCode[i];
					if (pc <= 'Z' && pc >= 'A' )
						iCount++;
					if (pc <= 'z' && pc >= 'a')
						iCount++;			
				}	*/			
			return(iCount);
		}


#if(DESIGNER)
        /*==============================================================================
		 *  Function Name : ResetDesignerStats
		 *  Parameters : none		
		 *  Return values : none
		 *  Description : Resets all the stats for a solution like #wallhits, #HerbertActions.
		 *  Dependencies : 
		 *  Created On : 26-Apr-2006
		 *  Created By : Vivek Balagangadharan
		 *  Special Comments : These stats are required for difficulty prediction of a level design.
		 * ==============================================================================*/
		/// <summary>
		/// Resets all the stats for a solution like #wallhits, #HerbertActions.
		/// </summary>
		public void ResetDesignerStats()
		{
			maxStackLength = 0;
			numProgramSteps = 0;
			numHerbertActions = 0;
			numGrayButtonHits = 0;
			numWallHits = 0;
            NumWDotPressedBeforeGDot = 0;
		}
#endif
		
		# endregion

		# region Designer Methods

#if(DESIGNER)
		/// <summary>
		/// function for designing buttons in designer mode.
		/// </summary>
		/// <param name="X">X point of button where button or target needs to be drawn</param>
		/// <param name="Y">Y point of button where button or target needs to be drawn</param>
		/// <param name="iGrey">0 if white button or target is drawn and 1 if grey button is to be drawn</param>
		public void DesignDot(int X, int Y, int iGrey)
		{
            //blnNewDotBufferChanged = true;//By Rajesh,21/8/06,3885
			//check if there is already hurdle is present then do not draw a dot.
			if(newLevelHerdls[X*25+Y,0].X == -1 && newLevelHerdls[X*25+Y,0].Y == -1 && newLevelDots[X*25+Y].X == -1 && newLevelDots[X*25+Y].Y== -1)
			{
				if((X-1) >= 0 && (Y-1) >= 0)
				{
					if(newLevelHerdls[(X-1)*25+Y,1].X == -1 && newLevelHerdls[(X-1)*25+Y,1].Y == -1)
					{
						if(newLevelHerdls[X*25+Y-1,2].X == -1 && newLevelHerdls[X*25+Y-1,2].Y == -1)
						{
							lock(LevelImg)
							{
								Graphics bufferGraphics = Graphics.FromImage(LevelImg);
								Pen BP = new Pen(Color.Black,1);
								Pen BP1 = new Pen(Color.Black,1.5F);
								Brush GB = new SolidBrush(System.Drawing.Color.Gray);
								//bufferGraphics.FillEllipse(System.Drawing.Brushes.White,X*HConstants.DOTSPACE+DOTOFFSET-1,Y*HConstants.DOTSPACE+DOTOFFSET-1,4,4);
								
								//Pen BP = new Pen(System.Drawing.Brushes.Black,1);
								if(iGrey == 1)
								{
									//bufferGraphics.FillEllipse(System.Drawing.Brushes.Gray,X*HConstants.DOTSPACE+DOTOFFSET-1,Y*HConstants.DOTSPACE+DOTOFFSET-1,4,4);					
									if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
									{
										bufferGraphics.FillEllipse(GB,X*HConstants.DOTSPACE*GlobalData.ZoomInZoomOut-GlobalData.ZoomInZoomOut+DOTOFFSET*GlobalData.ZoomInZoomOut,Y*HConstants.DOTSPACE*GlobalData.ZoomInZoomOut-GlobalData.ZoomInZoomOut+1+DOTOFFSET*GlobalData.ZoomInZoomOut,4*GlobalData.ZoomInZoomOut-BUTTONOFFSET,4*GlobalData.ZoomInZoomOut-BUTTONOFFSET);
										bufferGraphics.DrawEllipse(BP1,X*HConstants.DOTSPACE*GlobalData.ZoomInZoomOut-GlobalData.ZoomInZoomOut+DOTOFFSET*GlobalData.ZoomInZoomOut,Y*HConstants.DOTSPACE*GlobalData.ZoomInZoomOut-GlobalData.ZoomInZoomOut+1+DOTOFFSET*GlobalData.ZoomInZoomOut,4*GlobalData.ZoomInZoomOut-BUTTONOFFSET,4*GlobalData.ZoomInZoomOut-BUTTONOFFSET);
									}
									else
									{
										bufferGraphics.FillEllipse(GB,X*HConstants.DOTSPACE-1+DOTOFFSET,Y*HConstants.DOTSPACE-1+DOTOFFSET,4*GlobalData.ZoomInZoomOut,4*GlobalData.ZoomInZoomOut);
										bufferGraphics.DrawEllipse(BP,X*HConstants.DOTSPACE-1+DOTOFFSET,Y*HConstants.DOTSPACE-1+DOTOFFSET, 4*GlobalData.ZoomInZoomOut,4*GlobalData.ZoomInZoomOut);
									}
								}
								else
								{
									if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
									{
										bufferGraphics.FillEllipse(System.Drawing.Brushes.White,X*HConstants.DOTSPACE*GlobalData.ZoomInZoomOut-GlobalData.ZoomInZoomOut+DOTOFFSET*GlobalData.ZoomInZoomOut,Y*HConstants.DOTSPACE*GlobalData.ZoomInZoomOut-GlobalData.ZoomInZoomOut+1+DOTOFFSET*GlobalData.ZoomInZoomOut,4*GlobalData.ZoomInZoomOut-BUTTONOFFSET,4*GlobalData.ZoomInZoomOut-BUTTONOFFSET);
										bufferGraphics.DrawEllipse(BP1,X*HConstants.DOTSPACE*GlobalData.ZoomInZoomOut-GlobalData.ZoomInZoomOut+DOTOFFSET*GlobalData.ZoomInZoomOut,Y*HConstants.DOTSPACE*GlobalData.ZoomInZoomOut-GlobalData.ZoomInZoomOut+1+DOTOFFSET*GlobalData.ZoomInZoomOut,4*GlobalData.ZoomInZoomOut-BUTTONOFFSET,4*GlobalData.ZoomInZoomOut-BUTTONOFFSET);
									}
									else
									{
										bufferGraphics.FillEllipse(System.Drawing.Brushes.White,X*HConstants.DOTSPACE-1+DOTOFFSET,Y*HConstants.DOTSPACE-1+DOTOFFSET,4*GlobalData.ZoomInZoomOut,4*GlobalData.ZoomInZoomOut);
										bufferGraphics.DrawEllipse(BP,X*HConstants.DOTSPACE-1+DOTOFFSET,Y*HConstants.DOTSPACE-1+DOTOFFSET, 4*GlobalData.ZoomInZoomOut,4*GlobalData.ZoomInZoomOut);
									}
								}
								//bufferGraphics.DrawEllipse(BP,X*HConstants.DOTSPACE+DOTOFFSET-1,Y*HConstants.DOTSPACE+DOTOFFSET-1,4,4);
								BP.Dispose();
								BP1.Dispose();
								GB.Dispose();
								bufferGraphics.Dispose();
								newLevelDots[X*25+Y].X = X;
								newLevelDots[X*25+Y].Y = Y;
								//Console.WriteLine(X*25+Y);
								grey[X*25+Y] = iGrey;
								IsLevelChanged = true;
								IsLevelRun = false;
								
							}
						}
					}
				}
				else
				{
					lock(LevelImg)
					{
						Graphics bufferGraphics = Graphics.FromImage(LevelImg);
						Pen BP = new Pen(Color.Black,1);
						Pen BP1 = new Pen(Color.Black,1.5F);
						Brush GB = new SolidBrush(System.Drawing.Color.Gray);
						//bufferGraphics.FillEllipse(System.Drawing.Brushes.White,X*HConstants.DOTSPACE+DOTOFFSET-1,Y*HConstants.DOTSPACE+DOTOFFSET-1,4,4);
								
						//Pen BP = new Pen(System.Drawing.Brushes.Black,1);
						if(iGrey == 1)
						{
							//bufferGraphics.FillEllipse(System.Drawing.Brushes.Gray,X*HConstants.DOTSPACE+DOTOFFSET-1,Y*HConstants.DOTSPACE+DOTOFFSET-1,4,4);					
							if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
							{
								bufferGraphics.FillEllipse(GB,X*HConstants.DOTSPACE*GlobalData.ZoomInZoomOut-GlobalData.ZoomInZoomOut+DOTOFFSET*GlobalData.ZoomInZoomOut,Y*HConstants.DOTSPACE*GlobalData.ZoomInZoomOut-GlobalData.ZoomInZoomOut+1+DOTOFFSET*GlobalData.ZoomInZoomOut,4*GlobalData.ZoomInZoomOut-BUTTONOFFSET,4*GlobalData.ZoomInZoomOut-BUTTONOFFSET);
								bufferGraphics.DrawEllipse(BP1,X*HConstants.DOTSPACE*GlobalData.ZoomInZoomOut-GlobalData.ZoomInZoomOut+DOTOFFSET*GlobalData.ZoomInZoomOut,Y*HConstants.DOTSPACE*GlobalData.ZoomInZoomOut-GlobalData.ZoomInZoomOut+1+DOTOFFSET*GlobalData.ZoomInZoomOut,4*GlobalData.ZoomInZoomOut-BUTTONOFFSET,4*GlobalData.ZoomInZoomOut-BUTTONOFFSET);
							}
							else
							{
								bufferGraphics.FillEllipse(GB,X*HConstants.DOTSPACE-1+DOTOFFSET,Y*HConstants.DOTSPACE-1+DOTOFFSET,4*GlobalData.ZoomInZoomOut,4*GlobalData.ZoomInZoomOut);
								bufferGraphics.DrawEllipse(BP,X*HConstants.DOTSPACE-1+DOTOFFSET,Y*HConstants.DOTSPACE-1+DOTOFFSET, 4*GlobalData.ZoomInZoomOut,4*GlobalData.ZoomInZoomOut);
							}
						}
						else
						{
							if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
							{
								bufferGraphics.FillEllipse(System.Drawing.Brushes.White,X*HConstants.DOTSPACE*GlobalData.ZoomInZoomOut-GlobalData.ZoomInZoomOut+DOTOFFSET*GlobalData.ZoomInZoomOut,Y*HConstants.DOTSPACE*GlobalData.ZoomInZoomOut-GlobalData.ZoomInZoomOut+1+DOTOFFSET*GlobalData.ZoomInZoomOut,4*GlobalData.ZoomInZoomOut-BUTTONOFFSET,4*GlobalData.ZoomInZoomOut-BUTTONOFFSET);
								bufferGraphics.DrawEllipse(BP1,X*HConstants.DOTSPACE*GlobalData.ZoomInZoomOut-GlobalData.ZoomInZoomOut+DOTOFFSET*GlobalData.ZoomInZoomOut,Y*HConstants.DOTSPACE*GlobalData.ZoomInZoomOut-GlobalData.ZoomInZoomOut+1+DOTOFFSET*GlobalData.ZoomInZoomOut,4*GlobalData.ZoomInZoomOut-BUTTONOFFSET,4*GlobalData.ZoomInZoomOut-BUTTONOFFSET);
							}
							else
							{
								bufferGraphics.FillEllipse(System.Drawing.Brushes.White,X*HConstants.DOTSPACE-1+DOTOFFSET,Y*HConstants.DOTSPACE-1+DOTOFFSET,4*GlobalData.ZoomInZoomOut,4*GlobalData.ZoomInZoomOut);
								bufferGraphics.DrawEllipse(BP,X*HConstants.DOTSPACE-1+DOTOFFSET,Y*HConstants.DOTSPACE-1+DOTOFFSET, 4*GlobalData.ZoomInZoomOut,4*GlobalData.ZoomInZoomOut);
							}
						}
						//bufferGraphics.DrawEllipse(BP,X*HConstants.DOTSPACE+DOTOFFSET-1,Y*HConstants.DOTSPACE+DOTOFFSET-1,4,4);
						BP.Dispose();
						BP1.Dispose();
						GB.Dispose();
						bufferGraphics.Dispose();
						newLevelDots[X*25+Y].X = X;
						newLevelDots[X*25+Y].Y = Y;
						grey[X*25+Y] = iGrey;
						IsLevelChanged = true;
						IsLevelRun = false;
						
					}				
				}
			}
		}


		/// <summary>
		/// drawing hurdle in desinger mode.
		/// </summary>
		/// <param name="X1">x of first point</param>
		/// <param name="Y1">y of first point</param>
		/// <param name="X2">x of second point</param>
		/// <param name="Y2">y of second point</param>
		public void DesignHurdle(int X1, int Y1, int X2, int Y2)
		{
            //blnNewHerdleBufferChanged = true;//By Rajesh,21/8/06,3885
			//this check is for only properly drawing the lines only.
			if(Y1 == Y2)
			{	//check to see if there is already a dot present.
				if(newLevelDots[X1*25+Y1].X == -1 && newLevelDots[X2*25+Y1].X == -1 && CheckRoboXY(X1,Y1) && CheckRoboXY(X2,Y2) )
				{
					lock(LevelImg)
					{
						Graphics bufferGraphics = Graphics.FromImage(LevelImg);
						Pen LBP = new Pen(Color.Black,3);
						LBP.Width = 2+GlobalData.ZoomInZoomOut;
						if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
						{
							if(X1 == X2)
								if(Y1 > Y2)
									bufferGraphics.DrawLine(LBP,GlobalData.ZoomInZoomOut*(X1*HConstants.DOTSPACE+DOTOFFSET)+2,GlobalData.ZoomInZoomOut*(Y1*HConstants.DOTSPACE+DOTOFFSET)+1+GlobalData.ZoomInZoomOut,GlobalData.ZoomInZoomOut*(X2*HConstants.DOTSPACE+DOTOFFSET)+1,GlobalData.ZoomInZoomOut*(Y2*HConstants.DOTSPACE+DOTOFFSET)-2+1);
								else
									bufferGraphics.DrawLine(LBP,GlobalData.ZoomInZoomOut*(X1*HConstants.DOTSPACE+DOTOFFSET)+2,GlobalData.ZoomInZoomOut*(Y1*HConstants.DOTSPACE+DOTOFFSET)-2+1,GlobalData.ZoomInZoomOut*(X2*HConstants.DOTSPACE+DOTOFFSET)+1,GlobalData.ZoomInZoomOut*(Y2*HConstants.DOTSPACE+DOTOFFSET)+GlobalData.ZoomInZoomOut+1);
							else
								if(X1 > X2)
								bufferGraphics.DrawLine(LBP,GlobalData.ZoomInZoomOut*(X1*HConstants.DOTSPACE+DOTOFFSET)+GlobalData.ZoomInZoomOut+1,GlobalData.ZoomInZoomOut*(Y1*HConstants.DOTSPACE+DOTOFFSET)+2,GlobalData.ZoomInZoomOut*(X2*HConstants.DOTSPACE+DOTOFFSET)-2+1,GlobalData.ZoomInZoomOut*(Y2*HConstants.DOTSPACE+DOTOFFSET)+2);
							else
								bufferGraphics.DrawLine(LBP,GlobalData.ZoomInZoomOut*(X1*HConstants.DOTSPACE+DOTOFFSET)-2+2,GlobalData.ZoomInZoomOut*(Y1*HConstants.DOTSPACE+DOTOFFSET)+2,GlobalData.ZoomInZoomOut*(X2*HConstants.DOTSPACE+DOTOFFSET)+GlobalData.ZoomInZoomOut+1,GlobalData.ZoomInZoomOut*(Y2*HConstants.DOTSPACE+DOTOFFSET)+2);
						}
						else
						{
							if(X1 == X2)
								if(Y1 > Y2)
									bufferGraphics.DrawLine(LBP,X1*HConstants.DOTSPACE+DOTOFFSET,Y1*HConstants.DOTSPACE+DOTOFFSET+1,X2*HConstants.DOTSPACE+DOTOFFSET+1,Y2*HConstants.DOTSPACE+DOTOFFSET+1);
								else
									bufferGraphics.DrawLine(LBP,X1*HConstants.DOTSPACE+DOTOFFSET+1,Y1*HConstants.DOTSPACE+DOTOFFSET,X2*HConstants.DOTSPACE+DOTOFFSET+1,Y2*HConstants.DOTSPACE+DOTOFFSET+1+1);
							else
								if(X1 > X2)
								bufferGraphics.DrawLine(LBP,X1*HConstants.DOTSPACE+DOTOFFSET+1+1,Y1*HConstants.DOTSPACE+DOTOFFSET+1,X2*HConstants.DOTSPACE+DOTOFFSET+1,Y2*HConstants.DOTSPACE+DOTOFFSET+1);
							else
								bufferGraphics.DrawLine(LBP,X1*HConstants.DOTSPACE+DOTOFFSET,Y1*HConstants.DOTSPACE+DOTOFFSET+1,X2*HConstants.DOTSPACE+DOTOFFSET+1+1,Y2*HConstants.DOTSPACE+DOTOFFSET+1);
						}
						LBP.Dispose();
						bufferGraphics.Dispose();
						LBP = null;
						bufferGraphics = null;
						newLevelHerdls[X1*25+Y1,0].X = X1;
						newLevelHerdls[X1*25+Y1,0].Y = Y1;
						newLevelHerdls[X1*25+Y1,1].X = X2;
						newLevelHerdls[X1*25+Y1,1].Y = Y2;
						IsLevelChanged = true;
						IsLevelRun = false;
						
					}
				}
			}
			else
			{
				if(newLevelDots[X1*25+Y1].Y == -1 && newLevelDots[X1*25+Y2].Y == -1  && CheckRoboXY(X1,Y1) && CheckRoboXY(X2,Y2))
				{
					lock(LevelImg)
					{
						Graphics bufferGraphics = Graphics.FromImage(LevelImg);
						Pen LBP = new Pen(Color.Black,3);
						LBP.Width = 2+GlobalData.ZoomInZoomOut;
						if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
						{
							if(X1 == X2)
								if(Y1 > Y2)
									bufferGraphics.DrawLine(LBP,GlobalData.ZoomInZoomOut*(X1*HConstants.DOTSPACE+DOTOFFSET)+2,GlobalData.ZoomInZoomOut*(Y1*HConstants.DOTSPACE+DOTOFFSET)+GlobalData.ZoomInZoomOut-1,GlobalData.ZoomInZoomOut*(X2*HConstants.DOTSPACE+DOTOFFSET)+1,GlobalData.ZoomInZoomOut*(Y2*HConstants.DOTSPACE+DOTOFFSET)-0);
								else
									bufferGraphics.DrawLine(LBP,GlobalData.ZoomInZoomOut*(X1*HConstants.DOTSPACE+DOTOFFSET)+2,GlobalData.ZoomInZoomOut*(Y1*HConstants.DOTSPACE+DOTOFFSET),GlobalData.ZoomInZoomOut*(X2*HConstants.DOTSPACE+DOTOFFSET)+2,GlobalData.ZoomInZoomOut*(Y2*HConstants.DOTSPACE+DOTOFFSET)+GlobalData.ZoomInZoomOut+2);
							else
								if(X1 > X2)
								bufferGraphics.DrawLine(LBP,GlobalData.ZoomInZoomOut*(X1*HConstants.DOTSPACE+DOTOFFSET)+GlobalData.ZoomInZoomOut+1,GlobalData.ZoomInZoomOut*(Y1*HConstants.DOTSPACE+DOTOFFSET)+2,GlobalData.ZoomInZoomOut*(X2*HConstants.DOTSPACE+DOTOFFSET)-2+1,GlobalData.ZoomInZoomOut*(Y2*HConstants.DOTSPACE+DOTOFFSET)+2);
							else
								bufferGraphics.DrawLine(LBP,GlobalData.ZoomInZoomOut*(X1*HConstants.DOTSPACE+DOTOFFSET)-2+2,GlobalData.ZoomInZoomOut*(Y1*HConstants.DOTSPACE+DOTOFFSET)+2,GlobalData.ZoomInZoomOut*(X2*HConstants.DOTSPACE+DOTOFFSET)+GlobalData.ZoomInZoomOut+1,GlobalData.ZoomInZoomOut*(Y2*HConstants.DOTSPACE+DOTOFFSET)+2);
						}
						else
						{
							if(X1 == X2)
								if(Y1 > Y2)
									bufferGraphics.DrawLine(LBP,X1*8+6,Y1*8+6+1,X2*8+6+1,Y2*8+6+1);
								else
									bufferGraphics.DrawLine(LBP,X1*8+6+1,Y1*8+6+1,X2*8+6+1,Y2*8+6+1+1);
							else
								if(X1 > X2)
								bufferGraphics.DrawLine(LBP,X1*8+6+1+1,Y1*8+6+1,X2*8+6+1,Y2*8+6+1);
							else
								bufferGraphics.DrawLine(LBP,X1*8+6+1,Y1*8+6+1,X2*8+6+1+1,Y2*8+6+1);
						}
						LBP.Dispose();
						bufferGraphics.Dispose();
						LBP = null;
						bufferGraphics = null;
						newLevelHerdls[X1*25+Y1,0].X = X1;
						newLevelHerdls[X1*25+Y1,0].Y = Y1;
						newLevelHerdls[X1*25+Y1,2].X = X2;
						newLevelHerdls[X1*25+Y1,2].Y = Y2;
						IsLevelChanged = true;
						IsLevelRun = false;
						
					}
				}
			}			
		}

	
		/// <summary>
		///  this function is used to check if herbert is being placed over a wall or a button.
		/// </summary>/ 
		/// <param name="X">X coordinate</param>
		/// <param name="Y">Y coordinate</param>
		/// <returns>if will or button is there it returns false else true</returns>
		public static bool CheckLineButton(int X, int Y)
		{
			if(newLevelHerdls[X*25+Y,0].X == -1 && newLevelHerdls[X*25+Y,0].Y == -1 && newLevelDots[X*25+Y].X == -1 && newLevelDots[X*25+Y].Y == -1)
			{
				if((X-1) >= 0 && (Y-1) >= 0)
				{
					if(newLevelHerdls[(X-1)*25+Y,1].X == -1 && newLevelHerdls[(X-1)*25+Y,1].Y == -1)
					{
						if(newLevelHerdls[X*25+Y-1,2].X == -1 && newLevelHerdls[X*25+Y-1,2].Y == -1)
						{
							return true;
						}
					}
				}
				else if(X == 0 && Y != 0)
				{
					if(Y-1 >= 0)
						if(newLevelHerdls[X*25+Y-1,2].X == -1 && newLevelHerdls[X*25+Y-1,2].Y == -1)
						{
							return true;
						}
				}
				else if(Y == 0 && X != 0)
				{
					if(X-1 >= 0)
						if(newLevelHerdls[(X-1)*25+Y,1].X == -1 && newLevelHerdls[(X-1)*25+Y,1].Y == -1)
						{							
							return true;							
						}
				}
				else 
					return true;
			}
			return false;
		}


		/// <summary>
		///  this function is used to check if any Wall or line is being placed over herbert.
		/// </summary>/ 
		/// <param name="X">X coordinate</param>
		/// <param name="Y">Y coordinate</param>
		/// <returns>if herbert is there it returns false else true</returns>
		/// 
		public static bool CheckRoboXY(int X, int Y)
		{
			if(GlobalData.ZoomInZoomOut*(X*HConstants.DOTSPACE+4) == iRoboX*GlobalData.ZoomInZoomOut && GlobalData.ZoomInZoomOut*(Y*HConstants.DOTSPACE+4) == iRoboY*GlobalData.ZoomInZoomOut)
				return false;
			else 
				return true;
		}


		/// <summary>
		/// sets the robos x and y position.
		/// </summary>
		/// <param name="X">x position of herbert</param>
		/// <param name="Y">y position of herbert</param>
		public static void SetRoboPos(int X, int Y)
		{
			iRoboX = X;
			iRoboY = Y;
		}

		// Commented by Vivek, as this function is no longer used.
		//		/// <summary>
		//		/// gets the robos x and y position.
		//		/// </summary>
		//		public static Point getRoboPos()
		//		{
		//			Point p;
		//			p = new Point(iRoboX*GlobalData.ZoomInZoomOut,iRoboY*GlobalData.ZoomInZoomOut);
		//			//p = new Point(iRoboX, iRoboY);
		//			return p;
		//		}


		/// <summary>
		/// set max bytes for this new level.
		/// </summary>
		/// <param name="MaxBytes">pass max bytes</param>
		public static void setMaxBytes(int MaxBytes)
		{
			iMaxBytes = MaxBytes;
		}


		/// <summary>
		/// function which erases the hurdle or traget created on level 0 by designer.
		/// </summary>
		/// <param name="X">x postion of this hurdle or traget</param>
		/// <param name="Y">y postion of this hurdle or traget</param>
		public void Erase(int X, int Y,int lineIndex)
		{
			lock(LevelImg)
			{
				Graphics bufferGraphics = Graphics.FromImage(LevelImg);
				if(newLevelDots[X*25+Y].X == X && newLevelDots[X*25+Y].Y == Y)
				{
					IsNewLevelDotsPopulated = true;
					if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
					{
						bufferGraphics.FillEllipse(System.Drawing.Brushes.White,X*HConstants.DOTSPACE*GlobalData.ZoomInZoomOut-GlobalData.ZoomInZoomOut+DOTOFFSET*GlobalData.ZoomInZoomOut-1,Y*HConstants.DOTSPACE*GlobalData.ZoomInZoomOut-GlobalData.ZoomInZoomOut+DOTOFFSET*GlobalData.ZoomInZoomOut,4*GlobalData.ZoomInZoomOut,4*GlobalData.ZoomInZoomOut);
						bufferGraphics.FillEllipse(System.Drawing.Brushes.Black,(X)*HConstants.DOTSPACE*GlobalData.ZoomInZoomOut+6.0F*2+0.5F,(Y)*HConstants.DOTSPACE*GlobalData.ZoomInZoomOut+6.0F*2+0.5F,2.5F,2.5F);
					}
					else
					{
						bufferGraphics.FillEllipse(System.Drawing.Brushes.White,X*HConstants.DOTSPACE+DOTOFFSET-2,Y*HConstants.DOTSPACE+DOTOFFSET-2,6,6);				
						bufferGraphics.FillEllipse(System.Drawing.Brushes.Black,(X)*HConstants.DOTSPACE+6.0F,(Y)*HConstants.DOTSPACE+6.0F,DOTRADIOUS,DOTRADIOUS);						
					}
					newLevelDots[X*25+Y].X = -1;
					newLevelDots[X*25+Y].Y = -1;
					grey[X*25+Y] = -1;
					IsLevelChanged = true;
					IsLevelRun = false;
					
				}
				else
					if(newLevelHerdls[X*25+Y,0].X == X && newLevelHerdls[X*25+Y,0].Y == Y && newLevelHerdls[X*25+Y,lineIndex].X != -1 && newLevelHerdls[X*25+Y,lineIndex].Y != -1)
				{	
					IsNewLevelDotsPopulated = true;
					IsLevelChanged = true;
					IsLevelRun = false;
					
					int X1 = newLevelHerdls[X*25+Y,0].X;
					int Y1 = newLevelHerdls[X*25+Y,0].Y;
					int X2 = newLevelHerdls[X*25+Y,lineIndex].X;
					int Y2 = newLevelHerdls[X*25+Y,lineIndex].Y;				 
					Pen LBP = new Pen(Color.White,3);
					LBP.Width = 2 + GlobalData.ZoomInZoomOut;
					if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
					{
						if(X1 == X2)
							if(Y1 > Y2)
								bufferGraphics.DrawLine(LBP,GlobalData.ZoomInZoomOut*(X1*HConstants.DOTSPACE+DOTOFFSET)+2,GlobalData.ZoomInZoomOut*(Y1*HConstants.DOTSPACE+DOTOFFSET)+GlobalData.ZoomInZoomOut-1,GlobalData.ZoomInZoomOut*(X2*HConstants.DOTSPACE+DOTOFFSET)+1,GlobalData.ZoomInZoomOut*(Y2*HConstants.DOTSPACE+DOTOFFSET)+1);
							else
								bufferGraphics.DrawLine(LBP,GlobalData.ZoomInZoomOut*(X1*HConstants.DOTSPACE+DOTOFFSET)+2,GlobalData.ZoomInZoomOut*(Y1*HConstants.DOTSPACE+DOTOFFSET),GlobalData.ZoomInZoomOut*(X2*HConstants.DOTSPACE+DOTOFFSET)+2,GlobalData.ZoomInZoomOut*(Y2*HConstants.DOTSPACE+DOTOFFSET)+GlobalData.ZoomInZoomOut+2);
						else
							if(X1 > X2)
							bufferGraphics.DrawLine(LBP,GlobalData.ZoomInZoomOut*(X1*HConstants.DOTSPACE+DOTOFFSET)+GlobalData.ZoomInZoomOut,GlobalData.ZoomInZoomOut*(Y1*HConstants.DOTSPACE+DOTOFFSET)+2,GlobalData.ZoomInZoomOut*(X2*HConstants.DOTSPACE+DOTOFFSET)-2,GlobalData.ZoomInZoomOut*(Y2*HConstants.DOTSPACE+DOTOFFSET)+2);
						else
							bufferGraphics.DrawLine(LBP,GlobalData.ZoomInZoomOut*(X1*HConstants.DOTSPACE+DOTOFFSET),GlobalData.ZoomInZoomOut*(Y1*HConstants.DOTSPACE+DOTOFFSET)+2,GlobalData.ZoomInZoomOut*(X2*HConstants.DOTSPACE+DOTOFFSET)+GlobalData.ZoomInZoomOut+1,GlobalData.ZoomInZoomOut*(Y2*HConstants.DOTSPACE+DOTOFFSET)+2);

						bufferGraphics.FillEllipse(System.Drawing.Brushes.Black,(X1)*HConstants.DOTSPACE*GlobalData.ZoomInZoomOut+6.0F*2+0.5F,(Y1)*HConstants.DOTSPACE*GlobalData.ZoomInZoomOut+6.0F*2+0.5F,2.5F,2.5F);
						bufferGraphics.FillEllipse(System.Drawing.Brushes.Black,(X2)*HConstants.DOTSPACE*GlobalData.ZoomInZoomOut+6.0F*2+0.5F,(Y2)*HConstants.DOTSPACE*GlobalData.ZoomInZoomOut+6.0F*2+0.5F,2.5F,2.5F);
					}
					else
					{
						if(X1 == X2)
							if(Y1 > Y2)
								bufferGraphics.DrawLine(LBP,X1*HConstants.DOTSPACE+DOTOFFSET,Y1*HConstants.DOTSPACE+DOTOFFSET,X2*HConstants.DOTSPACE+DOTOFFSET+1,Y2*HConstants.DOTSPACE+DOTOFFSET+2);
							else
								bufferGraphics.DrawLine(LBP,X1*HConstants.DOTSPACE+DOTOFFSET+1,Y1*HConstants.DOTSPACE+DOTOFFSET,X2*HConstants.DOTSPACE+DOTOFFSET+1,Y2*HConstants.DOTSPACE+DOTOFFSET+3);
						else
							if(X1 > X2)
							bufferGraphics.DrawLine(LBP,X1*HConstants.DOTSPACE+DOTOFFSET+1,Y1*HConstants.DOTSPACE+DOTOFFSET+1,X2*HConstants.DOTSPACE+DOTOFFSET+2,Y2*HConstants.DOTSPACE+DOTOFFSET+1);
						else
							bufferGraphics.DrawLine(LBP,X1*HConstants.DOTSPACE+DOTOFFSET-1,Y1*HConstants.DOTSPACE+DOTOFFSET+1,X2*HConstants.DOTSPACE+DOTOFFSET+4,Y2*HConstants.DOTSPACE+DOTOFFSET+1);

						bufferGraphics.FillEllipse(System.Drawing.Brushes.Black,(X1)*HConstants.DOTSPACE+6.0F,(Y1)*HConstants.DOTSPACE+6.0F,DOTRADIOUS,DOTRADIOUS);
						bufferGraphics.FillEllipse(System.Drawing.Brushes.Black,(X2)*HConstants.DOTSPACE+6.0F,(Y2)*HConstants.DOTSPACE+6.0F,DOTRADIOUS,DOTRADIOUS);
					}
					LBP.Color = Color.Black;
					//draw all surrounding walls.
					//same point down wall.
					int Indx = 1;
					if(lineIndex == 1)
					{
						Indx = 2;
					}
					X1 = newLevelHerdls[X*25+Y,0].X;
					Y1 = newLevelHerdls[X*25+Y,0].Y;
					X2 = newLevelHerdls[X*25+Y,Indx].X;
					Y2 = newLevelHerdls[X*25+Y,Indx].Y;	
					if(X2 != -1 && Y2 != -1 )
					{	
						if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
						{
							fDrawErasedWall(X1,Y1,X2,Y2, ref bufferGraphics, ref LBP);
						}
						else
						{
							fDrawErasedWallOut(X1,Y1,X2,Y2, ref bufferGraphics, ref LBP);
						}
					}
					//preposition point horizontal wall.
					if((X-1)>=0)
					{
						X1 = newLevelHerdls[(X-1)*25+Y,0].X;
						Y1 = newLevelHerdls[(X-1)*25+Y,0].Y;
						X2 = newLevelHerdls[(X-1)*25+Y,1].X;
						Y2 = newLevelHerdls[(X-1)*25+Y,1].Y;

                        if (X2 != -1 && Y2 != -1)
                        {
                            if (GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
                            {
                                fDrawErasedWall(X1, Y1, X2, Y2, ref bufferGraphics, ref LBP);
                            }
                            else
                            {
                                fDrawErasedWallOut(X1, Y1, X2, Y2, ref bufferGraphics, ref LBP);
                            }
                        }
///Added By Rajesh
///Date 18/8/06
///Issue: 3788
                        else
                        {
                            X1 = newLevelHerdls[(X - 1) * 25 + Y + 1, 0].X;
                            Y1 = newLevelHerdls[(X - 1) * 25 + Y + 1, 0].Y;
                            X2 = newLevelHerdls[(X - 1) * 25 + Y + 1, 1].X;
                            Y2 = newLevelHerdls[(X - 1) * 25 + Y + 1, 1].Y;

                            if (X2 != -1 && Y2 != -1)
                            {
                                if (GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
                                {
                                    fDrawErasedWall(X1, Y1, X2, Y2, ref bufferGraphics, ref LBP);
                                }
                                else
                                {
                                    fDrawErasedWallOut(X1, Y1, X2, Y2, ref bufferGraphics, ref LBP);
                                }
                            }
                        }
///end
					}
					//upper point down wall.
					if((Y-1)>=0)
					{
						X1 = newLevelHerdls[(X)*25+Y-1,0].X;
						Y1 = newLevelHerdls[(X)*25+Y-1,0].Y;
						X2 = newLevelHerdls[(X)*25+Y-1,2].X;
						Y2 = newLevelHerdls[(X)*25+Y-1,2].Y;
						if(X2 != -1 && Y2 != -1)
						{
							if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
							{
								fDrawErasedWall(X1,Y1,X2,Y2, ref bufferGraphics, ref LBP);
							}
							else
							{
								fDrawErasedWallOut(X1,Y1,X2,Y2, ref bufferGraphics, ref LBP);
							}
						}
					}
					//if wall is horizontal from the target point.
					if(lineIndex == 1)
					{							
						//next point horizontal wall.
						X1 = newLevelHerdls[(X+1)*25+Y,0].X;
						Y1 = newLevelHerdls[(X+1)*25+Y,0].Y;
						X2 = newLevelHerdls[(X+1)*25+Y,1].X;
						Y2 = newLevelHerdls[(X+1)*25+Y,1].Y;
						if(X2 != -1 && Y2 != -1)
						{
							if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
							{
								fDrawErasedWall(X1,Y1,X2,Y2, ref bufferGraphics, ref LBP);
							}
							else
							{
								fDrawErasedWallOut(X1,Y1,X2,Y2, ref bufferGraphics, ref LBP);
							}
						}
						//next point down wall.
						X1 = newLevelHerdls[(X+1)*25+Y,0].X;
						Y1 = newLevelHerdls[(X+1)*25+Y,0].Y;
						X2 = newLevelHerdls[(X+1)*25+Y,2].X;
						Y2 = newLevelHerdls[(X+1)*25+Y,2].Y;
						if(X2 != -1 && Y2 != -1)
						{
							if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
							{
								fDrawErasedWall(X1,Y1,X2,Y2, ref bufferGraphics, ref LBP);
							}
							else
							{
								fDrawErasedWallOut(X1,Y1,X2,Y2, ref bufferGraphics, ref LBP);
							}
						}
						//upper point of next point down wall.
						if((Y-1)>=0)
						{
							X1 = newLevelHerdls[(X+1)*25+Y-1,0].X;
							Y1 = newLevelHerdls[(X+1)*25+Y-1,0].Y;
							X2 = newLevelHerdls[(X+1)*25+Y-1,2].X;
							Y2 = newLevelHerdls[(X+1)*25+Y-1,2].Y;
							if(X2 != -1 && Y2 != -1)
							{
								if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
								{
									fDrawErasedWall(X1,Y1,X2,Y2, ref bufferGraphics, ref LBP);
								}
								else
								{
									fDrawErasedWallOut(X1,Y1,X2,Y2, ref bufferGraphics, ref LBP);
								}
							}
						}
					}
						//if wall is down from the target point.
					else
					{
						//next point horizontal wall.
						X1 = newLevelHerdls[(X)*25+Y+1,0].X;
						Y1 = newLevelHerdls[(X)*25+Y+1,0].Y;
						X2 = newLevelHerdls[(X)*25+Y+1,1].X;
						Y2 = newLevelHerdls[(X)*25+Y+1,1].Y;
						if(X2 != -1 && Y2 != -1)
						{
							if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
							{
								fDrawErasedWall(X1,Y1,X2,Y2, ref bufferGraphics, ref LBP);
							}
							else
							{
								fDrawErasedWallOut(X1,Y1,X2,Y2, ref bufferGraphics, ref LBP);
							}
						}
						//next point down wall.
						X1 = newLevelHerdls[(X)*25+Y+1,0].X;
						Y1 = newLevelHerdls[(X)*25+Y+1,0].Y;
						X2 = newLevelHerdls[(X)*25+Y+1,2].X;
						Y2 = newLevelHerdls[(X)*25+Y+1,2].Y;
						if(X2 != -1 && Y2 != -1 )
						{
							if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
							{
								fDrawErasedWall(X1,Y1,X2,Y2, ref bufferGraphics, ref LBP);
							}
							else
							{
								fDrawErasedWallOut(X1,Y1,X2,Y2, ref bufferGraphics, ref LBP);
							}
						}
						//upper point of next point down wall.
						if((X-1)>=0)
						{
							X1 = newLevelHerdls[(X-1)*25+Y+1,0].X;
							Y1 = newLevelHerdls[(X-1)*25+Y+1,0].Y;
							X2 = newLevelHerdls[(X-1)*25+Y+1,2].X;
							Y2 = newLevelHerdls[(X-1)*25+Y+1,2].Y;
							
							if(X2 != -1 && Y2 != -1 )
							{
								if(GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
								{
									fDrawErasedWall(X1,Y1,X2,Y2, ref bufferGraphics, ref LBP);
								}
								else
								{
									fDrawErasedWallOut(X1,Y1,X2,Y2, ref bufferGraphics, ref LBP);
								}
							}
						}
					}
					LBP.Dispose();
					LBP = null;	
					
					if(newLevelHerdls[X*25+Y,3-lineIndex].X == -1)
					{
						newLevelHerdls[X*25+Y,0].X = -1;
						newLevelHerdls[X*25+Y,0].Y = -1;
					}
					newLevelHerdls[X*25+Y,lineIndex].X = -1;
					newLevelHerdls[X*25+Y,lineIndex].Y = -1;

				}
				
				bufferGraphics.Dispose();
				bufferGraphics = null;				
			}
		}


		private void fDrawErasedWall(int X1, int Y1, int X2, int Y2,ref Graphics bufferGraphics, ref Pen BP)
		{
			if(X1 == X2)
				if(Y1 > Y2)
					bufferGraphics.DrawLine(BP,GlobalData.ZoomInZoomOut*(X1*HConstants.DOTSPACE+DOTOFFSET)+2,GlobalData.ZoomInZoomOut*(Y1*HConstants.DOTSPACE+DOTOFFSET)+1+GlobalData.ZoomInZoomOut,GlobalData.ZoomInZoomOut*(X2*HConstants.DOTSPACE+DOTOFFSET)+2,GlobalData.ZoomInZoomOut*(Y2*HConstants.DOTSPACE+DOTOFFSET)-2+2);
				else
					bufferGraphics.DrawLine(BP,GlobalData.ZoomInZoomOut*(X1*HConstants.DOTSPACE+DOTOFFSET)+2,GlobalData.ZoomInZoomOut*(Y1*HConstants.DOTSPACE+DOTOFFSET),GlobalData.ZoomInZoomOut*(X2*HConstants.DOTSPACE+DOTOFFSET)+2,GlobalData.ZoomInZoomOut*(Y2*HConstants.DOTSPACE+DOTOFFSET)+GlobalData.ZoomInZoomOut+1);
			else
				if(X1 > X2)
				bufferGraphics.DrawLine(BP,GlobalData.ZoomInZoomOut*(X1*HConstants.DOTSPACE+DOTOFFSET)+GlobalData.ZoomInZoomOut+1,GlobalData.ZoomInZoomOut*(Y1*HConstants.DOTSPACE+DOTOFFSET)+2,GlobalData.ZoomInZoomOut*(X2*HConstants.DOTSPACE+DOTOFFSET)-2+2,GlobalData.ZoomInZoomOut*(Y2*HConstants.DOTSPACE+DOTOFFSET)+2);
			else
				bufferGraphics.DrawLine(BP,GlobalData.ZoomInZoomOut*(X1*HConstants.DOTSPACE+DOTOFFSET)-2+2,GlobalData.ZoomInZoomOut*(Y1*HConstants.DOTSPACE+DOTOFFSET)+2,GlobalData.ZoomInZoomOut*(X2*HConstants.DOTSPACE+DOTOFFSET)+GlobalData.ZoomInZoomOut+1,GlobalData.ZoomInZoomOut*(Y2*HConstants.DOTSPACE+DOTOFFSET)+2);
		}


		private void fDrawErasedWallOut(int X1 , int Y1, int X2, int Y2, ref Graphics bufferGraphics, ref Pen BP)
		{
			if(X1 == X2)
				if(Y1 > Y2)
					bufferGraphics.DrawLine(BP,X1*HConstants.DOTSPACE+DOTOFFSET,Y1*HConstants.DOTSPACE+DOTOFFSET+1,X2*HConstants.DOTSPACE+DOTOFFSET+1,Y2*HConstants.DOTSPACE+DOTOFFSET+1);
				else
					bufferGraphics.DrawLine(BP,X1*HConstants.DOTSPACE+DOTOFFSET+1,Y1*HConstants.DOTSPACE+DOTOFFSET,X2*HConstants.DOTSPACE+DOTOFFSET+1,Y2*HConstants.DOTSPACE+DOTOFFSET+1+1);
			else
				if(X1 > X2)
				bufferGraphics.DrawLine(BP,X1*HConstants.DOTSPACE+DOTOFFSET+1+1,Y1*HConstants.DOTSPACE+DOTOFFSET+1,X2*HConstants.DOTSPACE+DOTOFFSET+1,Y2*HConstants.DOTSPACE+DOTOFFSET+1);
			else
				bufferGraphics.DrawLine(BP,X1*HConstants.DOTSPACE+DOTOFFSET+1,Y1*HConstants.DOTSPACE+DOTOFFSET+1,X2*HConstants.DOTSPACE+DOTOFFSET+1+2,Y2*HConstants.DOTSPACE+DOTOFFSET+1);
		}


		/// <summary>
		/// generate level designer data
		/// </summary>
 
		public static void LevelDesignData()
		{
			newLevelDots = new Point[TOTALSMLDOT];
			newLevelHerdls = new Point[TOTALSMLDOT,3];
			//grey = new int[625];//By Rajesh,21/8/06,3885
			for(int i = 0; i < TOTALSMLDOT; i++)
			{
				newLevelDots[i].X = -1;
				newLevelDots[i].Y = -1;				
				grey[i] = -1;
			}			
			for(int i = 0; i < TOTALSMLDOT; i++)
			{				
				newLevelHerdls[i,0].X = -1;
				newLevelHerdls[i,0].Y = -1;
				newLevelHerdls[i,1].X = -1;
				newLevelHerdls[i,1].Y = -1;			
				newLevelHerdls[i,2].X = -1;
				newLevelHerdls[i,2].Y = -1;		
			}			
		}
        /*
        /// <summary>
        /// generate level designer data//By Rajesh,21/8/06,3885
        /// </summary>

        public void setNonSavedDesignData()
        {
            //to solve erase islevelchanged is added
            if (blnNewDotBufferChanged || blnNewHerdleBufferChanged || IsLevelChanged)
            {
                if (nonSavedLevelDots == null)
                    nonSavedLevelDots = new Hashtable();
                if (newLevelDots != null)
                {
                    Point[] prevLevelDots = newLevelDots;
                    if (!nonSavedLevelDots.Contains((object)iLevelId))
                    {
                        nonSavedLevelDots.Add((object)iLevelId, prevLevelDots);
                    }
                    else
                    {
                        nonSavedLevelDots[(object)iLevelId] = prevLevelDots;
                    }
                }
                if (nonSavedLevelHerdls == null)
                    nonSavedLevelHerdls = new Hashtable();
                if (newLevelHerdls != null)
                {
                    Point[,] prevLevelHerdls = newLevelHerdls;
                    if (!nonSavedLevelHerdls.Contains((object)iLevelId))
                    {
                        nonSavedLevelHerdls.Add((object)iLevelId, prevLevelHerdls);
                    }
                    else
                    {
                        nonSavedLevelHerdls[(object)iLevelId] = prevLevelHerdls;
                    }

                }
                if (nonSavedLevelGray == null)
                    nonSavedLevelGray = new Hashtable();
                if (grey != null)
                {
                    int[] prevGray = grey;
                    if (!nonSavedLevelGray.Contains((object)iLevelId))
                    {
                        nonSavedLevelGray.Add((object)iLevelId, prevGray);
                    }
                    else
                    {
                        nonSavedLevelGray[(object)iLevelId] = prevGray;
                    }
                }
            }       
                
            
            
           
        }
         
        /// <summary>
        /// To remove the non saved data after saving a level//By Rajesh,21/8/06,3885
        /// </summary>
        public void clearNonSavedData()
        {
            if (nonSavedLevelDots != null)
            {
                if (nonSavedLevelDots.Contains((object)iLevelId))
                {
                    nonSavedLevelDots.Remove((object)iLevelId);
                }
            }
            if (nonSavedLevelHerdls != null)
            {
                if (nonSavedLevelHerdls.Contains((object)iLevelId))
                {
                    nonSavedLevelHerdls.Remove((object)iLevelId);
                }
            }

        }
        /// <summary>
        /// To get the non saved data into newLevelDots n nonSavedLevelHerdls
        /// </summary>
        public void updateBuffer()
        {
            if (nonSavedLevelDots != null || nonSavedLevelHerdls != null)
            {
                if (nonSavedLevelDots != null)
                {
                    if (nonSavedLevelDots.Contains((object)iLevelId))
                    {
                        Point[] tempLevelDots = (Point[])nonSavedLevelDots[(object)iLevelId];
                        newLevelDots = tempLevelDots;
                    }
                }

                if (nonSavedLevelHerdls != null)
                {
                    if (nonSavedLevelHerdls.Contains((object)iLevelId))
                    {
                        Point[,] tempLevelHerdles = (Point[,])nonSavedLevelHerdls[(object)iLevelId];
                        newLevelHerdls = tempLevelHerdles;
                    }
                }
                if (nonSavedLevelGray != null)
                {
                    if (nonSavedLevelGray.Contains((object)iLevelId))
                    {
                        int[] tempLevelGray = (int[])nonSavedLevelGray[(object)iLevelId];
                        grey = tempLevelGray;
                    }
                }
            }
            else
            {
                LevelDesignData();
            }
 
        }
        
        /// <summary>
        /// To set IsLevelUpdate and IsLevelChanged
        /// </summary>
        public void setIsLevelChanged()
        {
            if (iLevelId != 0)
            {

                if (nonSavedLevelDots.Contains((object)(iLevelId)) || nonSavedLevelHerdls.Contains((object)(iLevelId)))
                {
                    //IsLevelUpdate = true;
                    IsLevelChanged = true;
                }
            }
 
        }*/
        

        /// <summary>
        /// gets the level data in the global data structure 
        /// to update a perticular level.
        /// </summary>
        /// <param name="LevelNo">Level No of the currently active level</param>
        

        public void getLevelDataToUpdate(int LevelId)
		{
			LevelDesignData();
            //updateBuffer();//By Rajesh,21/8/06,3885
			int zizo = GlobalData.ZoomInZoomOut;
#if(PATTERN_GENERATER)
            if (LevelId != 0 && !IsClearAllChecked && (!IsLevelUpdate || GlobalData.iPatternId != "-1"))
#else
            if (LevelId != 0 && !IsLevelUpdate && !IsClearAllChecked)
#endif
			{
				//get data of hurdles and buttons.
				DataRow[] drowsButtons = GlobalData.dsAllHData.Tables["tblLevelButtonsMST"].Select("LevelId="+LevelId.ToString());
				Point[] ptsButtons = new Point[drowsButtons.Length];
				int[] iGrey = new int[drowsButtons.Length];
				for(int i=0;i<drowsButtons.Length;i++)
				{
					ptsButtons[i] = new Point(int.Parse(drowsButtons[i]["ButtonX"].ToString()),int.Parse(drowsButtons[i]["ButtonY"].ToString()));
					iGrey[i] = int.Parse(drowsButtons[i]["Grey"].ToString());
				}

				for(int i = 0; i < drowsButtons.Length; i++)
				{
					newLevelDots[ptsButtons[i].X*25+ptsButtons[i].Y].X = ptsButtons[i].X;
					newLevelDots[ptsButtons[i].X*25+ptsButtons[i].Y].Y = ptsButtons[i].Y;
					grey[ptsButtons[i].X*25+ptsButtons[i].Y] = iGrey[i];
				}
				IsNewLevelDotsPopulated = true;

				DataRow[] drowsLines = GlobalData.dsAllHData.Tables["tblLevelWallsMST"].Select("LevelId="+LevelId.ToString());
			
				Point[,] ptsLines = new Point[drowsLines.Length,2];
				for(int i=0;i<drowsLines.Length;i++)
				{
					ptsLines[i,0] = new Point(int.Parse(drowsLines[i]["startX"].ToString()), int.Parse(drowsLines[i]["startY"].ToString()));
					ptsLines[i,1] = new Point(int.Parse(drowsLines[i]["endX"].ToString()), int.Parse(drowsLines[i]["endY"].ToString()));
				}
				
				for(int i=0;i<drowsLines.Length;i++)
				{
					if(ptsLines[i,0].X == ptsLines[i,1].X)
					{
						newLevelHerdls[ptsLines[i,0].X*25+ptsLines[i,0].Y,0].X = ptsLines[i,0].X;
						newLevelHerdls[ptsLines[i,0].X*25+ptsLines[i,0].Y,0].Y = ptsLines[i,0].Y;
						newLevelHerdls[ptsLines[i,0].X*25+ptsLines[i,0].Y,2].X = ptsLines[i,1].X;
						newLevelHerdls[ptsLines[i,0].X*25+ptsLines[i,0].Y,2].Y = ptsLines[i,1].Y;
					}
					else
					{
						newLevelHerdls[ptsLines[i,0].X*25+ptsLines[i,0].Y,0].X = ptsLines[i,0].X;
						newLevelHerdls[ptsLines[i,0].X*25+ptsLines[i,0].Y,0].Y = ptsLines[i,0].Y;
						newLevelHerdls[ptsLines[i,0].X*25+ptsLines[i,0].Y,1].X = ptsLines[i,1].X;
						newLevelHerdls[ptsLines[i,0].X*25+ptsLines[i,0].Y,1].Y = ptsLines[i,1].Y;
					}
				}
			}
			else
			{
				IsNewLevelDotsPopulated = true;
				for(int i = 0; i < arrDotPoints.Length; i++)
				{
					if(arrDotPoints[i].X != -1)
					{
						newLevelDots[((arrDotPoints[i].X-DOTOFFSET)/(HConstants.DOTSPACE))*25+(arrDotPoints[i].Y-DOTOFFSET)/(HConstants.DOTSPACE)].X = (arrDotPoints[i].X-DOTOFFSET)/(HConstants.DOTSPACE);
						newLevelDots[((arrDotPoints[i].X-DOTOFFSET)/(HConstants.DOTSPACE))*25+(arrDotPoints[i].Y-DOTOFFSET)/(HConstants.DOTSPACE)].Y = (arrDotPoints[i].Y-DOTOFFSET)/(HConstants.DOTSPACE);
						grey[((arrDotPoints[i].X-DOTOFFSET)/(HConstants.DOTSPACE))*25+(arrDotPoints[i].Y-DOTOFFSET)/(HConstants.DOTSPACE)] = 0;
					}
				}
				
				for(int i = 0; i < arrDotPointsGray.Length; i++)
				{
					if(arrDotPointsGray[i].X != -1)
					{
						newLevelDots[((arrDotPointsGray[i].X-DOTOFFSET)/(HConstants.DOTSPACE))*25+(arrDotPointsGray[i].Y-DOTOFFSET)/(HConstants.DOTSPACE)].X = (arrDotPointsGray[i].X-DOTOFFSET)/(HConstants.DOTSPACE);
						newLevelDots[((arrDotPointsGray[i].X-DOTOFFSET)/(HConstants.DOTSPACE))*25+(arrDotPointsGray[i].Y-DOTOFFSET)/(HConstants.DOTSPACE)].Y = (arrDotPointsGray[i].Y-DOTOFFSET)/(HConstants.DOTSPACE);
						grey[((arrDotPointsGray[i].X-DOTOFFSET)/(HConstants.DOTSPACE))*25+(arrDotPointsGray[i].Y-DOTOFFSET)/(HConstants.DOTSPACE)] = 1;
					}
				}
				if(NumHurdles>0)
				{
					for(int i=0;i<arrLinePoints.Length/2;i++)
					{
						if(arrLinePoints[i,0].X == arrLinePoints[i,1].X)
						{
							newLevelHerdls[((arrLinePoints[i,0].X-DOTOFFSET)/(HConstants.DOTSPACE))*25+(arrLinePoints[i,0].Y-DOTOFFSET)/(HConstants.DOTSPACE),0].X = (arrLinePoints[i,0].X-DOTOFFSET)/(HConstants.DOTSPACE);
							newLevelHerdls[((arrLinePoints[i,0].X-DOTOFFSET)/(HConstants.DOTSPACE))*25+(arrLinePoints[i,0].Y-DOTOFFSET)/(HConstants.DOTSPACE),0].Y = (arrLinePoints[i,0].Y-DOTOFFSET)/(HConstants.DOTSPACE);
							newLevelHerdls[((arrLinePoints[i,0].X-DOTOFFSET)/(HConstants.DOTSPACE))*25+(arrLinePoints[i,0].Y-DOTOFFSET)/(HConstants.DOTSPACE),2].X = (arrLinePoints[i,1].X-DOTOFFSET)/(HConstants.DOTSPACE);
							newLevelHerdls[((arrLinePoints[i,0].X-DOTOFFSET)/(HConstants.DOTSPACE))*25+(arrLinePoints[i,0].Y-DOTOFFSET)/(HConstants.DOTSPACE),2].Y = (arrLinePoints[i,1].Y-DOTOFFSET)/(HConstants.DOTSPACE);
						}
						else
						{
							newLevelHerdls[((arrLinePoints[i,0].X-DOTOFFSET)/(HConstants.DOTSPACE))*25+(arrLinePoints[i,0].Y-DOTOFFSET)/(HConstants.DOTSPACE),0].X = (arrLinePoints[i,0].X-DOTOFFSET)/(HConstants.DOTSPACE);
							newLevelHerdls[((arrLinePoints[i,0].X-DOTOFFSET)/(HConstants.DOTSPACE))*25+(arrLinePoints[i,0].Y-DOTOFFSET)/(HConstants.DOTSPACE),0].Y = (arrLinePoints[i,0].Y-DOTOFFSET)/(HConstants.DOTSPACE);
							newLevelHerdls[((arrLinePoints[i,0].X-DOTOFFSET)/(HConstants.DOTSPACE))*25+(arrLinePoints[i,0].Y-DOTOFFSET)/(HConstants.DOTSPACE),1].X = (arrLinePoints[i,1].X-DOTOFFSET)/(HConstants.DOTSPACE);
							newLevelHerdls[((arrLinePoints[i,0].X-DOTOFFSET)/(HConstants.DOTSPACE))*25+(arrLinePoints[i,0].Y-DOTOFFSET)/(HConstants.DOTSPACE),1].Y = (arrLinePoints[i,1].Y-DOTOFFSET)/(HConstants.DOTSPACE);
						}
					}
				}
			}            
			IsLevelUpdate = true;
			IsLevelDisplayUpdated = true;
		}


		/// <summary>
		/// gets the update level data in designer mode.
		/// </summary>
		/// <param name="levelNo">level no which needs to be updated.</param>
		public void saveUpdatedLevelData(int LevelId)
		{
			//code for counting the number of grey buttons in the updated level
			int greyButton = 0;
			int whiteButton = 0;
			for(int i = 0; i < HConstants.GRIDDOTS; i++)
			{
				if(grey[i] == 0)
					whiteButton++;
				else if(grey[i] == 1)
					greyButton++;
			}
			arrTargetAchived = new bool[whiteButton];
			getUpdatedGreyNWhiteButtons(whiteButton, greyButton);
			getUpdatedLines();
			arrTargetAchived = new bool[NumDots];
			for(int i = 0; i < NumDots; i++)
			{
				//set all traget achived flag to false.
				arrTargetAchived[i] = false;
			}
			//check and remove these code bellow.
			LevelImg = new Bitmap(208*GlobalData.ZoomInZoomOut, 208*GlobalData.ZoomInZoomOut);
			drawLevel(LevelId);
			scoreCurrently = 0;
			RNG1 = 12*GlobalData.ZoomInZoomOut;
			//if(NumDots != temp_numDotsPressed)
			temp_numDotsPressed = 0;
		}


		/// <summary>
		/// get the buttons if any level is updated or a new level is created
		/// at level 0.
		/// </summary>
		/// <param name="numWhiteButtons">Total number of white buttons now</param>
		/// <param name="numGreyButtons">Total number of grey buttons now</param>
		internal void getUpdatedGreyNWhiteButtons(int numWhiteButtons, int numGreyButtons)
		{

			NumDots = numWhiteButtons;
			arrDotPoints = new Point[NumDots];
			NumDotsGray = numGreyButtons;
			arrDotPointsGray = new Point[NumDotsGray];
			int j = 0, k = 0;
			for(int i = 0; i < HConstants.GRIDDOTS; i++)
			{				
				if(newLevelDots[i].X != -1)
				{
					if(grey[i] == 0)
					{
						arrDotPoints[j].X = (newLevelDots[i].X*HConstants.DOTSPACE+DOTOFFSET);
						arrDotPoints[j].Y = (newLevelDots[i].Y*HConstants.DOTSPACE+DOTOFFSET);
						j++;
					}
					else if(grey[i] == 1)
					{
						arrDotPointsGray[k].X = (newLevelDots[i].X*HConstants.DOTSPACE+DOTOFFSET);
						arrDotPointsGray[k].Y = (newLevelDots[i].Y*HConstants.DOTSPACE+DOTOFFSET);
						k++;
					}
				}
			}
		}
        internal void getUpdatedGreyNWhiteButtons()
        {
            int numGreyButtons = 0;
            int numWhiteButtons = 0;
            for (int i = 0; i < HConstants.GRIDDOTS; i++)
            {
                if (grey[i] == 0)
                    numWhiteButtons++;
                else if (grey[i] == 1)
                    numGreyButtons++;
            }
            NumDots = numWhiteButtons;
            arrDotPoints = new Point[NumDots];
            NumDotsGray = numGreyButtons;
            arrDotPointsGray = new Point[NumDotsGray];
            int j = 0, k = 0;
            for (int i = 0; i < HConstants.GRIDDOTS; i++)
            {
                if (newLevelDots[i].X != -1)
                {
                    if (grey[i] == 0)
                    {
                        arrDotPoints[j].X = (newLevelDots[i].X * HConstants.DOTSPACE + DOTOFFSET);
                        arrDotPoints[j].Y = (newLevelDots[i].Y * HConstants.DOTSPACE + DOTOFFSET);
                        j++;
                    }
                    else if (grey[i] == 1)
                    {
                        arrDotPointsGray[k].X = (newLevelDots[i].X * HConstants.DOTSPACE + DOTOFFSET);
                        arrDotPointsGray[k].Y = (newLevelDots[i].Y * HConstants.DOTSPACE + DOTOFFSET);
                        k++;
                    }
                }
            }
        }

		/// <summary>
		/// get the line points if a new level is created or an existing level is updated.
		/// </summary>
		private void getUpdatedLines()
		{
			int numLines = 0;
			for(int i = 0; i < HConstants.GRIDDOTS; i++)
			{
				if(newLevelHerdls[i,0].X != -1)
				{
					if(newLevelHerdls[i,1].X != -1)
						numLines++;
					if(newLevelHerdls[i,2].X != -1)
						numLines++;
				}
			}
			arrLinePoints = new Point[numLines,2];
			NumHurdles = numLines;
			int j = 0;
			for(int i = 0; i < HConstants.GRIDDOTS; i++)
			{
				if(newLevelHerdls[i,0].X != -1)
				{
					if(newLevelHerdls[i,1].X != -1)
					{
						arrLinePoints[j,0].X = (newLevelHerdls[i,0].X*HConstants.DOTSPACE+DOTOFFSET);
						arrLinePoints[j,0].Y = (newLevelHerdls[i,0].Y*HConstants.DOTSPACE+DOTOFFSET);
						arrLinePoints[j,1].X = (newLevelHerdls[i,1].X*HConstants.DOTSPACE+DOTOFFSET);
						arrLinePoints[j,1].Y = (newLevelHerdls[i,1].Y*HConstants.DOTSPACE+DOTOFFSET);
						j++;
					}
					if(newLevelHerdls[i,2].X != -1)
					{
						arrLinePoints[j,0].X = (newLevelHerdls[i,0].X*HConstants.DOTSPACE+DOTOFFSET);
						arrLinePoints[j,0].Y = (newLevelHerdls[i,0].Y*HConstants.DOTSPACE+DOTOFFSET);
						arrLinePoints[j,1].X = (newLevelHerdls[i,2].X*HConstants.DOTSPACE+DOTOFFSET);
						arrLinePoints[j,1].Y = (newLevelHerdls[i,2].Y*HConstants.DOTSPACE+DOTOFFSET);
						j++;						
					}
				}
			}
		}


		/// <summary>
		/// save level data for new level.
		/// </summary>
        public void SaveNewLevel(int LevelId, int LevelNo, Object Values, Object oper, string lvlProgram, string strLeveltype)
		{		
			GlobalData.initlizeWS();
			//Insert all dots points.
			int iNumDots = 0;
			int index1 = arrDotPoints.Length + arrDotPointsGray.Length;
			//int newindex = newLevelDots.Length;
			for(int i = 0; i < TOTALSMLDOT; i++)
			{
				if(newLevelDots[i].X != -1 && newLevelDots[i].Y != -1)
				{
					iNumDots++;
				}
			}
			
			object[] arrButtonX;
			object[] arrButtonY;
			object[] arrGrey;
			object[] arrStartX ;
			object[] arrStartY ;
			object[] arrEndX ;
			object[] arrEndY ;

			if(!IsLevelChanged)
			{
				//send only arrdotpoints
				string IsGrey = "0";
				DataRow[] drowsWhite = GlobalData.dsAllHData.Tables["tblLevelButtonsMST"].Select("LevelId="+LevelId.ToString()+" AND Grey="+IsGrey);
				IsGrey = "1";
				DataRow[] drowsGrey = GlobalData.dsAllHData.Tables["tblLevelButtonsMST"].Select("LevelId="+LevelId.ToString()+" AND Grey="+IsGrey);
				arrButtonX = new object[drowsWhite.Length+drowsGrey.Length];
				arrButtonY = new object[drowsWhite.Length+drowsGrey.Length];
				arrGrey = new object[drowsWhite.Length+drowsGrey.Length];
				
				for(int i=0;i<drowsWhite.Length;i++)
				{
					
					arrButtonX[i] = int.Parse(drowsWhite[i]["ButtonX"].ToString());
					arrButtonY[i] = int.Parse(drowsWhite[i]["ButtonY"].ToString());
					arrGrey[i] = 0;
				}
				for(int i = drowsWhite.Length,j=0;i< drowsWhite.Length+drowsGrey.Length;i++,j++)
				{
					arrButtonX[i] = int.Parse(drowsGrey[j]["ButtonX"].ToString());
					arrButtonY[i] = int.Parse(drowsGrey[j]["ButtonY"].ToString());
					arrGrey[i] = 1;
				}


			}
			else
			{
				//append extra
				arrButtonX = new object[iNumDots];
				arrButtonY = new object[iNumDots];
				arrGrey = new object[iNumDots];
				int index2 = 0;
				for(int i = 0; i < TOTALSMLDOT; i++)
				{
					if(newLevelDots[i].X != -1 && newLevelDots[i].Y != -1)
					{
						arrButtonX[index2] = newLevelDots[i].X;
						arrButtonY[index2] = newLevelDots[i].Y;
						arrGrey[index2] = grey[i];
						index2++;
					}
				}	
				
			}
			//Insert all Hurdle points.
			int iNumHurdles = 0;
			for(int i = 0; i < TOTALSMLDOT; i++)
			{
				if(newLevelHerdls[i,0].X != -1 && newLevelHerdls[i,0].Y != -1)
				{
					for(int j = 1; j < 3;j++)
					{
						if(newLevelHerdls[i,j].X != -1 && newLevelHerdls[i,j].Y != -1)
						{
							iNumHurdles++;
						}
					}
				}
			}
			if(!IsLevelChanged)
			{
				DataRow[] drows = GlobalData.dsAllHData.Tables["tblLevelWallsMST"].Select("LevelId="+LevelId.ToString());
				arrStartX = new object[drows.Length];
				arrStartY = new object[drows.Length];
				arrEndX = new object[drows.Length];
				arrEndY = new object[drows.Length];
				
				for(int i =0 ;i<drows.Length ;i++)
				{
					arrStartX[i] = int.Parse(drows[i]["StartX"].ToString());
					arrStartY[i] = int.Parse(drows[i]["StartY"].ToString());
					arrEndX[i] = int.Parse(drows[i]["EndX"].ToString());
					arrEndY[i] = int.Parse(drows[i]["EndY"].ToString());
				}
			}
			else
			{
				arrStartX = new object[iNumHurdles];
				arrStartY = new object[iNumHurdles];
				arrEndX = new object[iNumHurdles];
				arrEndY = new object[iNumHurdles];
				int index = 0;
				for(int i = 0; i < TOTALSMLDOT; i++)
				{
					if(newLevelHerdls[i,0].X != -1 && newLevelHerdls[i,0].Y != -1)
					{
						for(int j = 1; j < 3;j++)
						{
							if(newLevelHerdls[i,j].X != -1 && newLevelHerdls[i,j].Y != -1)
							{
								arrStartX[index] = newLevelHerdls[i,0].X;
								arrStartY[index] = newLevelHerdls[i,0].Y;
								arrEndX[index] = newLevelHerdls[i,j].X;
								arrEndY[index] = newLevelHerdls[i,j].Y;
								index++;
							}
						}
					}
				}
			}
			
			
			object oLevelId;
			object oLevelNo;
			
			oLevelId = LevelId;
			oLevelNo = LevelNo;
			
			object oNumButtons = iNumDots;
			object oNumHurdles = iNumHurdles;
			object oMaxChars = iMaxBytes;
			object oRoboX = iRoboX;
			object oRoboY = iRoboY;
			object oValidity = 1;
			object oLevelPoints = GlobalData.DeginerLevelPoints;
			object oDifficultyBucket = 0;
			object oContestantId = GlobalData.ContestantId;
			DataSet dsLevelData  = new DataSet();
			//object oLevelProgram;
			int iRetryCount = 0;
			while(true)
			{
				GlobalData.initlizeWS();
				try
				{
					/*==============================================================================
					*  Modified By : Vivek Balagangadharan
					*  Description : iLevelId is a comma separated list of levels. -1 is its default value.
					*  Modified On : 10-Apr-2006
					*  Special Comments : iLevelId was an int type initially
					* ==============================================================================*/
							
                    //if(GlobalData.iLevelId != "-1")
                    //{
                    //    dsLevelData = GlobalData.HS.DLevelInformation(oLevelId, oNumButtons, oNumHurdles, oMaxChars, oRoboX, oRoboY,
                    //        oValidity, oLevelPoints,oDifficultyBucket, arrButtonX, arrButtonY,
                    //        arrGrey, arrStartX, arrStartY, arrEndX, arrEndY, Values,oper,lvlProgram);
                    //}
                    //else
                    {

#if(PREDICTER)
                        dsLevelData = GlobalData.HS.LevelInformation(oLevelId, oLevelNo, oNumButtons, oNumHurdles, oMaxChars, oRoboX, oRoboY,
                        oValidity, oLevelPoints, oDifficultyBucket, oLevelPoints, arrButtonX, arrButtonY,
                        arrGrey, arrStartX, arrStartY, arrEndX, arrEndY, oContestantId, Values, oper, lvlProgram, MaxStackLengthPersistent,
                        NumProgramStepsPersistent, NumHerbertActionsPersistent, NumGrayButtonHits, NumWallHits, strLeveltype, NumWDotPressedBeforeGDot);
#else
                        dsLevelData = GlobalData.HS.LevelInformation(oLevelId, oLevelNo, oNumButtons, oNumHurdles, oMaxChars, oRoboX, oRoboY,
                            oValidity, oLevelPoints, oDifficultyBucket, oLevelPoints, arrButtonX, arrButtonY,
                            arrGrey, arrStartX, arrStartY, arrEndX, arrEndY, oContestantId, Values, oper, lvlProgram);
#endif
					}
                    if (LevelId == 0)
                    {
                        if (dsLevelData.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsLevelData.Tables[0].Rows.Count; i++)
                                GlobalData.dsAllHData.Tables[1].ImportRow(dsLevelData.Tables[0].Rows[i]);
                        }
                        //						else
                        //							GlobalData.dsAllHData.Tables[1].ImportRow(dsLevelData.Tables[0].Rows[0]);

                        if (dsLevelData.Tables[1].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsLevelData.Tables[1].Rows.Count; i++)
                                GlobalData.dsAllHData.Tables[2].ImportRow(dsLevelData.Tables[1].Rows[i]);
                        }
                        //						else
                        //							GlobalData.dsAllHData.Tables[2].ImportRow(dsLevelData.Tables[1].Rows[0]);

                        if (dsLevelData.Tables[2].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsLevelData.Tables[2].Rows.Count; i++)
                                GlobalData.dsAllHData.Tables[3].ImportRow(dsLevelData.Tables[2].Rows[i]);
                        }
                        //						else
                        //							GlobalData.dsAllHData.Tables[3].ImportRow(dsLevelData.Tables[2].Rows[0]);

                        GlobalData.dsAllHData.AcceptChanges();
                        if (GlobalData.iPatternId != "-1")
                        {
                            if (LevelNo >= 0)
                            {
                                if (LevelNo < arrLevelId.Length)
                                {
                                    arrLevelId[LevelNo] = int.Parse(dsLevelData.Tables[0].Rows[0]["LevelId"].ToString());
                                    arrLvlProgram[LevelNo] = dsLevelData.Tables[0].Rows[0]["LevelProgram"].ToString();
                                    arrLvlPoints[LevelNo] = int.Parse(dsLevelData.Tables[0].Rows[0]["Points"].ToString());
                                }
                                else
                                {
                                    arrLvlProgram[0] = "";
                                    arrLvlPoints[0] = 0;
                                    int[] arrNewLevelid = new int[arrLevelId.Length + 1];
                                    string[] arrNewLvlProgram = new string[arrLvlProgram.Length + 1];
                                    int[] arrNewLvlPoints = new int[arrLvlPoints.Length + 1];
                                    for (int cnt = 0; cnt < LevelNo; cnt++)
                                    {
                                        arrNewLevelid[cnt] = arrLevelId[cnt];
                                        arrNewLvlProgram[cnt] = arrLvlProgram[cnt];
                                        arrNewLvlPoints[cnt] = arrLvlPoints[cnt];
                                    }
                                    arrNewLevelid[LevelNo] = int.Parse(dsLevelData.Tables[0].Rows[0]["LevelId"].ToString());
                                    arrNewLvlProgram[LevelNo] = dsLevelData.Tables[0].Rows[0]["LevelProgram"].ToString();
                                    arrNewLvlPoints[LevelNo] = int.Parse(dsLevelData.Tables[0].Rows[0]["Points"].ToString());

                                    arrLevelId = arrNewLevelid;
                                    arrLvlProgram = arrNewLvlProgram;
                                    arrLvlPoints = arrNewLvlPoints;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (GlobalData.iPatternId != "-1")
                        {
                            if (LevelNo >= 0)
                            {
                                arrLevelId[LevelNo] = int.Parse(dsLevelData.Tables[0].Rows[0]["LevelId"].ToString());
                                arrLvlProgram[LevelNo] = dsLevelData.Tables[0].Rows[0]["LevelProgram"].ToString();
                                arrLvlPoints[LevelNo] = int.Parse(dsLevelData.Tables[0].Rows[0]["Points"].ToString());
                            }
                        }
                        # region removing existing rows
                        DataRow[] dr = new DataRow[GlobalData.dsAllHData.Tables[1].Select("levelid = " + LevelId.ToString()).Length];
                        dr = GlobalData.dsAllHData.Tables[1].Select("levelid = " + LevelId.ToString());
                        if (dr.Length > 0)
                        {
                            for (int i = 0; i < dr.Length; i++)
                            {
                                GlobalData.dsAllHData.Tables[1].Rows.Remove(dr[i]);
                            }
                        }

                        dr = new DataRow[GlobalData.dsAllHData.Tables[2].Select("levelid = " + LevelId.ToString()).Length];
                        dr = GlobalData.dsAllHData.Tables[2].Select("levelid = " + LevelId.ToString());
                        if (dr.Length > 0)
                        {
                            for (int i = 0; i < dr.Length; i++)
                            {
                                GlobalData.dsAllHData.Tables[2].Rows.Remove(dr[i]);
                            }
                        }

                        dr = new DataRow[GlobalData.dsAllHData.Tables[3].Select("levelid = " + LevelId.ToString()).Length];
                        dr = GlobalData.dsAllHData.Tables[3].Select("levelid = " + LevelId.ToString());
                        if (dr.Length > 0)
                        {
                            for (int i = 0; i < dr.Length; i++)
                            {
                                GlobalData.dsAllHData.Tables[3].Rows.Remove(dr[i]);
                            }
                        }
                        #endregion

                        # region Populating dataset with updated data

                        if (dsLevelData.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsLevelData.Tables[0].Rows.Count; i++)
                                GlobalData.dsAllHData.Tables[1].ImportRow(dsLevelData.Tables[0].Rows[i]);
                        }

                        GlobalData.dsAllHData.Tables[1].DefaultView.Sort = "levelno asc";


                        if (dsLevelData.Tables[1].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsLevelData.Tables[1].Rows.Count; i++)
                                GlobalData.dsAllHData.Tables[2].ImportRow(dsLevelData.Tables[1].Rows[i]);
                        }


                        if (dsLevelData.Tables[2].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsLevelData.Tables[2].Rows.Count; i++)
                                GlobalData.dsAllHData.Tables[3].ImportRow(dsLevelData.Tables[2].Rows[i]);
                        }

                        GlobalData.dsAllHData.AcceptChanges();

                        # endregion
                    }
					break;
				}
				catch(Exception exp)
				{
					iRetryCount++;
					if(iRetryCount == 3)
					{
						throw new Exception(exp.Message);
					}
				}
			}

			arrButtonX = null;
			arrButtonY = null;
			arrGrey = null;
			arrStartX = null;
			arrStartY = null;
			arrEndX = null;
			arrEndY = null;	
			oLevelId = null;
			oLevelNo = null;
			oNumButtons = null;
			oNumHurdles = null;
			oMaxChars = null;
			oRoboX = null;
			oRoboY = null;
			oValidity = null;
			oLevelPoints = null;
			oDifficultyBucket = null;
			oContestantId = null;	
			GlobalData.HS = null;
		}


		public int getLevelScore(int LevelId)
		{
			DataRow[] dr = GlobalData.dsAllHData.Tables["tblGameLevelScore"].Select("LevelId = "+LevelId.ToString());
			if(dr.Length == 0)
				return 1;
			else
				return int.Parse(dr[0]["LevelScore"].ToString());
		}


		/// <summary>
		/// checks to see if atleast one dot is designed on the designer level.
		/// </summary>
		/// <returns>returns true if a dot is designed.</returns>
		public bool CheckAtLeastOneDot(int index)
		{
			if(index>0)
			{
				if(IsNewLevelDotsPopulated)
				{
					for(int i = 0; i < HConstants.GRIDDOTS; i++)
					{
						if(newLevelDots[i].X != -1 && grey[i]!=1)
							return true;
					}
				}
				else
				{
					for(int i = 0; i < NumDots; i++)
					{
						if(arrDotPoints[i].X != -1)
							return true;
					}	
					for(int i = 0; i < HConstants.GRIDDOTS; i++)
					{
						if(newLevelDots[i].X != -1 && grey[i]!=1)
							return true;
					}
				}

				

			}
			else
			{
				for(int i = 0; i < HConstants.GRIDDOTS; i++)
				{
					if(newLevelDots[i].X != -1 && grey[i]!=1)
						return true;
				}
				
			}
			return false;
		}


		/// <summary>
		/// check to see if all white buttons pressed.
		/// </summary>
		/// <returns>true if all buttons pressed else false</returns>
		public bool checkAllButtonsPressed()
		{
			if(NumDots == numDotsPressed)
				return true;

			return false;
		}
		

		public void ClearLevel(int LevelId)
        {
            if (arrDotPoints != null)
            {
                int ml = arrDotPoints.Length;
                if (arrDotPoints.Length != 0 || arrDotPointsGray.Length != 0 || arrLinePoints.Length != 0)
                {
                    for (int i = 0; i < NumDots; i++)
                    {
                        arrDotPoints[i].X = -1;
                        arrDotPoints[i].Y = -1;
                    }
                    for (int i = 0; i < NumDotsGray; i++)
                    {
                        arrDotPointsGray[i].X = -1;
                        arrDotPointsGray[i].Y = -1;
                    }
                    for (int i = 0; i < NumHurdles; i++)
                    {
                        arrLinePoints[i, 0].X = -1;
                        arrLinePoints[i, 0].Y = -1;
                        arrLinePoints[i, 1].X = -1;
                        arrLinePoints[i, 1].Y = -1;
                    }
                    arrTargetAchived = new bool[NumDots];
                    for (int i = 0; i < NumDots; i++)
                    {
                        //set all traget achived flag to false.
                        arrTargetAchived[i] = false;
                    }
                    LevelImg = new Bitmap(208 * GlobalData.ZoomInZoomOut, 208 * GlobalData.ZoomInZoomOut);

                    NumDots = 0;
                    NumDotsGray = 0;
                    NumHurdles = 0;
                    scoreCurrently = 0;
                    RNG1 = 12 * GlobalData.ZoomInZoomOut;
                    //if(NumDots != temp_numDotsPressed)
                    temp_numDotsPressed = 0;

                }
#if(PATTERN_GENERATER)

                if (objHPath != null)
                {
                    objHPath.Dispose();
                }
                    objHPath = null;                
#endif
            }
                drawLevel(LevelId);
            
			
		}
#endif

		#endregion
		
		#region Comments
		
		

		/*#########################################################################################*/
		//defining static data for different levels
		/*******************************************************************************************/
		
		/* data common for all levels, index of these arrays represents the level no.*/
		//No of dots per level.
		//private static int[] L_NumDots =    {0,1,3,1,1,1,2,4,4,4,44,4};
		//No of hurdles per Level.
		//private static int[] L_NumHurdles = {0,0,6,2,3,3,6,12,4,0,96,22};

		//Max char allowed in per level.
		//private static int[] L_MaxCharPerLevel = {0,4,20,10,7,4,20,20,16,11,21,11};




		//		/*******************************************************************************************/
		//		// seperate data for each level.
		//
		//		//Level 0.
		//		/*******************************************************************************************/
		//		//Level 1.
		//		private static Point[] L1_dotPoints = new Point[1];
		//		private static Point[,] L1_LineHurdles = new Point[1,2];
		//		/*******************************************************************************************/
		//		//Level 2.
		//		private static Point[] L2_dotPoints = new Point[3];
		//		private static Point[,] L2_lineHurdles = new Point[6,2];
		//		/*******************************************************************************************/
		//		//Level 3.
		//		private static Point[] L3_dotPoints = new Point[1];
		//		private static Point[,] L3_lineHurdles = new Point[2,2];
		//		/*******************************************************************************************/
		//		
		//		//Level 4.
		//		private static Point[] L4_dotPoints = new Point[1];
		//		private static Point[,] L4_lineHurdles = new Point[3,2];
		//
		//		/*******************************************************************************************/
		//		
		//		//Level 4.
		//		private static Point[] L5_dotPoints = new Point[1];
		//		private static Point[,] L5_lineHurdles = new Point[3,2];
		//		/*******************************************************************************************/
		//
		//		//Level 5.
		//		private static Point[] L6_dotPoints = new Point[2];
		//		private static Point[,] L6_lineHurdles = new Point[6,2];
		//		/*******************************************************************************************/
		//
		//		//Level 6.
		//		private static Point[] L7_dotPoints = new Point[4];
		//		private static Point[,] L7_lineHurdles = new Point[12,2];
		//		/*******************************************************************************************/
		//
		//		//Level 7.
		//		private static Point[] L8_dotPoints = new Point[4];
		//		private static Point[,] L8_lineHurdles = new Point[4,2];
		//		/*******************************************************************************************/
		//
		//		//Level 9.
		//		private static Point[] L9_dotPoints = new Point[4];		
		//		/*******************************************************************************************/
		//
		//		//Level 9.
		//		private static Point[] L10_dotPoints = new Point[44];	
		//		private static Point[,] L10_lineHurdles = new Point[96,2];
		//
		////		private static Point[] L11_dotPoints; 	
		////		private static Point[,] L11_lineHurdles;
		///
		/*******************************************************************************************/
		/*#########################################################################################*/

		//defining static functions for getting static data of different levels

		#endregion
#if(CONTEST)
        #region Session management
        /// <summary>
        /// To update the gameLevelscore data into the global Dataset.
        /// </summary>
        internal void setGameLevelScoreData()
        {
            try
            {
                DataRow[] dr = GlobalData.dsAllHData.Tables["tblGameLevelScore"].Select("LevelScoreId = '" + LevelScoreId.ToString()+"'");

                dr[0]["LastProgram"] = iLastProgram;
                dr[0]["LastBytes"] = iBestBytes;
                dr[0]["LastGos"] = iLastGos.ToString();
                dr[0]["LastTimeSpent"] = iLastTimeSpent.ToString();
                dr[0]["LastCharsTyped"] = iLastCharsTyped.ToString();
                dr[0]["LastVisits"] = iLastVisits.ToString();
                dr[0]["FirstScore"] = iFirstLevelScore.ToString();
                dr[0]["FirstVisits"] = iFirstVisits.ToString();

                dr[0]["LastDate"] = iLastDate.ToString();
                dr[0]["BestVisits"] = iBestVisits.ToString();
                dr[0]["BestCharsTyped"] = iBestCharsTyped.ToString();
                dr[0]["BestDate"] = iBestDate.ToString();
                dr[0]["FirstTimeSpent"] = iFirstTimeSpent.ToString();
                dr[0]["FirstGos"] = iFirstGos.ToString();
                dr[0]["FirstBytes"] = iFirstBytes.ToString();
                dr[0]["FirstCharsTyped"] = iFirstCharsTyped.ToString();
                dr[0]["BestProgram"] = iBestProgram;
                dr[0]["BestTimeSpent"] = iBestTimeSpent.ToString();
                dr[0]["BestGos"] = iBestGos.ToString();
                dr[0]["FirstCharsTyped"] = iFirstCharsTyped.ToString();

                dr[0]["DateFirstCharTyped"] = FirstCharTypedDT.ToString();
                dr[0]["FirstDate"] = FirstSolutionDT.ToString();
                dr[0]["BestBytes"] = iBestBytes.ToString();
                dr[0]["CurrentProgram"] = iCurrentProgram;
                dr[0]["levelScoreId"] = LevelScoreId.ToString();
                dr[0]["LevelStatus"] = IsLevelFinishedPersistant.ToString();
                dr[0]["BestNumButtonsPressed"] = numDotsPressed.ToString();

                dr[0]["LevelScore"] = levelScore.ToString();
                dr[0]["LevelScore"] = BestLevelScore.ToString();
                dr[0]["FirstProgram"] = iFirstProgram.ToString();
                dr[0]["NumLevelVisits"] = pNumVisits.ToString();
                dr[0]["TotalGos"] = iTotalGos.ToString();
                dr[0]["TotalTimeSpent"] = iTotalTimeSpent.ToString();
                dr[0]["TotalCharsTyped"] = iTotalCharsTyped.ToString();
                GlobalData.dsAllHData.AcceptChanges();
            }
            catch (Exception exp)
            {

            }
        }
        #endregion
#endif
#if(PATTERN_GENERATER)
        #region GENRATE_PATTERN
        /// <summary>
        /// Adde By Rajesh to Check if there Pattern  avialable.
        /// </summary>
        /// <returns></returns>
        internal bool IsPatternPresent()
        {
            if (objHPath == null)
            {
                return false;
            }
            else
                return true;
        }
        /// <summary>
        /// draw level board after erase with Pattern enabled.
        /// </summary>
        /// <param name="LevelNo">Level Number whose board needs to be drawn</param>
        public void drawLevelAfterErase()
        {
            getUpdatedLines();
            lock (LevelImg)
            {
                int zizo = GlobalData.ZoomInZoomOut;
                Graphics bufferGraphics = Graphics.FromImage(LevelImg);
                bufferGraphics.FillRectangle(System.Drawing.Brushes.White, 0, 0, 208 * zizo, 208 * zizo);

                //if level is not 0 then draw smalldots as well as buttons and hurdles if present.
                for (int i = 0; i < HConstants.GRIDDOTSPERLINE; i++)
                    for (int j = 0; j < HConstants.GRIDDOTSPERLINE; j++)
                    {
                        if (GlobalData.ZoomInZoomOut == HConstants.ZOOMOUT)
                            bufferGraphics.FillEllipse(System.Drawing.Brushes.Black, (i) * HConstants.DOTSPACE * zizo + 6.0F, (j) * HConstants.DOTSPACE * zizo + 6.0F, DOTRADIOUS, DOTRADIOUS);
                        else
                            bufferGraphics.FillEllipse(System.Drawing.Brushes.Black, (i) * HConstants.DOTSPACE * zizo + 6.0F * 2 + 0.5F, (j) * HConstants.DOTSPACE * zizo + 6.0F * 2 + 0.5F, 2.5F, 2.5F);

                    }
                //2005-12-06 changes for save path in memory.
#if(CONTEST || PATTERN_GENERATER)
                if (IsPatternEnabled)
                    fDisplayStoredPath();
#endif
                Pen BP = new Pen(Color.Black, 1);
                Pen BP1 = new Pen(Color.Black, 1.5F);

                for (int i = 0; i < newLevelDots.Length; i++)
                {
                    if (newLevelDots[i].X != -1 && newLevelDots[i].Y != -1)
                    {
                        if (GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
                        {
                            if (grey[i] == 0)
                            {
                                bufferGraphics.FillEllipse(System.Drawing.Brushes.White, (newLevelDots[i].X * HConstants.DOTSPACE + DOTOFFSET) * zizo - zizo, (newLevelDots[i].Y * HConstants.DOTSPACE + DOTOFFSET) * zizo - zizo + 1, 4 * zizo - BUTTONOFFSET, 4 * zizo - BUTTONOFFSET);
                                bufferGraphics.DrawEllipse(BP1, (newLevelDots[i].X * HConstants.DOTSPACE + DOTOFFSET) * zizo - zizo, (newLevelDots[i].Y * HConstants.DOTSPACE + DOTOFFSET) * zizo - zizo + 1, 4 * zizo - BUTTONOFFSET, 4 * zizo - BUTTONOFFSET);
                            }
                            else
                            {
                                bufferGraphics.FillEllipse(System.Drawing.Brushes.Gray,(newLevelDots[i].X * HConstants.DOTSPACE + DOTOFFSET) * zizo - zizo, (newLevelDots[i].Y * HConstants.DOTSPACE + DOTOFFSET) * zizo - zizo + 1, 4 * zizo - BUTTONOFFSET, 4 * zizo - BUTTONOFFSET);
                                bufferGraphics.DrawEllipse(BP1, (newLevelDots[i].X * HConstants.DOTSPACE + DOTOFFSET) * zizo - zizo, (newLevelDots[i].Y * HConstants.DOTSPACE + DOTOFFSET) * zizo - zizo + 1, 4 * zizo - BUTTONOFFSET, 4 * zizo - BUTTONOFFSET);                                

                            }
                        }
                        else
                        {
                            if (grey[i] == 0)
                            {
                                bufferGraphics.FillEllipse(System.Drawing.Brushes.White, (newLevelDots[i].X * HConstants.DOTSPACE + DOTOFFSET) - 1, (newLevelDots[i].Y * HConstants.DOTSPACE + DOTOFFSET) - 1, 4 * GlobalData.ZoomInZoomOut, 4 * GlobalData.ZoomInZoomOut);
                                bufferGraphics.DrawEllipse(BP, (newLevelDots[i].X * HConstants.DOTSPACE + DOTOFFSET) - 1, (newLevelDots[i].Y * HConstants.DOTSPACE + DOTOFFSET) - 1, 4 * GlobalData.ZoomInZoomOut, 4 * GlobalData.ZoomInZoomOut);
                            }
                            else
                            {
                                bufferGraphics.FillEllipse(System.Drawing.Brushes.Gray, (newLevelDots[i].X * HConstants.DOTSPACE + DOTOFFSET) - 1, (newLevelDots[i].Y * HConstants.DOTSPACE + DOTOFFSET) - 1, 4 * GlobalData.ZoomInZoomOut, 4 * GlobalData.ZoomInZoomOut);
                                bufferGraphics.DrawEllipse(BP, (newLevelDots[i].X * HConstants.DOTSPACE + DOTOFFSET) - 1, (newLevelDots[i].Y * HConstants.DOTSPACE + DOTOFFSET) - 1, 4 * GlobalData.ZoomInZoomOut, 4 * GlobalData.ZoomInZoomOut);
                            }
                        }
                    }
                }

                //BP.Dispose();
                BP.Width = 2 + GlobalData.ZoomInZoomOut;
                //Pen LBP = new Pen(Color.Black,2+GlobalData.ZoomInZoomOut);
                //Code to restore the Line.
                for (int i = 0; i < NumHurdles; i++)
                {
                    if (arrLinePoints[i, 0].X != -1 && arrLinePoints[i, 0].Y != -1 && arrLinePoints[i, 1].Y != -1 && arrLinePoints[i, 1].Y != -1)
                    {
                        if (GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
                        {
                            if (arrLinePoints[i, 0].X == arrLinePoints[i, 1].X)
                                if (arrLinePoints[i, 0].Y > arrLinePoints[i, 1].Y)
                                    bufferGraphics.DrawLine(BP, arrLinePoints[i, 0].X * zizo + 2, arrLinePoints[i, 0].Y * zizo + 1 + zizo, arrLinePoints[i, 1].X * zizo + 2, arrLinePoints[i, 1].Y * zizo - 2 + 2);
                                else
                                    bufferGraphics.DrawLine(BP, arrLinePoints[i, 0].X * zizo + 2, arrLinePoints[i, 0].Y * zizo - 2 + 2, arrLinePoints[i, 1].X * zizo + 2, arrLinePoints[i, 1].Y * zizo + zizo + 1);
                            else
                                if (arrLinePoints[i, 0].X > arrLinePoints[i, 1].X)
                                    bufferGraphics.DrawLine(BP, arrLinePoints[i, 0].X * zizo + zizo + 1, arrLinePoints[i, 0].Y * zizo + 2, arrLinePoints[i, 1].X * zizo - 2 + 2, arrLinePoints[i, 1].Y * zizo + 2);
                                else
                                    bufferGraphics.DrawLine(BP, arrLinePoints[i, 0].X * zizo - 2 + 2, arrLinePoints[i, 0].Y * zizo + 2, arrLinePoints[i, 1].X * zizo + zizo + 1, arrLinePoints[i, 1].Y * zizo + 2);
                        }
                        else
                        {
                            if (arrLinePoints[i, 0].X == arrLinePoints[i, 1].X)
                                if (arrLinePoints[i, 0].Y > arrLinePoints[i, 1].Y)
                                    bufferGraphics.DrawLine(BP, arrLinePoints[i, 0].X + 1, arrLinePoints[i, 0].Y + 1 + 1, arrLinePoints[i, 1].X + 1, arrLinePoints[i, 1].Y + 1);
                                else
                                    bufferGraphics.DrawLine(BP, arrLinePoints[i, 0].X + 1, arrLinePoints[i, 0].Y + 1, arrLinePoints[i, 1].X + 1, arrLinePoints[i, 1].Y + 1 + 1);
                            else
                                if (arrLinePoints[i, 0].X > arrLinePoints[i, 1].X)
                                    bufferGraphics.DrawLine(BP, arrLinePoints[i, 0].X + 1 + 1, arrLinePoints[i, 0].Y + 1, arrLinePoints[i, 1].X + 1, arrLinePoints[i, 1].Y + 1);
                                else
                                    bufferGraphics.DrawLine(BP, arrLinePoints[i, 0].X + 1, arrLinePoints[i, 0].Y + 1, arrLinePoints[i, 1].X + 1 + 1, arrLinePoints[i, 1].Y + 1);
                        }
                    }
                }



                //			}	
                //LBP.Dispose();
                BP.Dispose();
                BP1.Dispose();
                bufferGraphics.Dispose();
                BP = null;
                BP1 = null;
                bufferGraphics = null;
            }
        }
        public void GenerateTraceImage(string strLevelTrace, int ZoomInZoomout)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HerbertMain));
            System.Drawing.Image herbertImage;
            int zizo = ZoomInZoomout;
            if (GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
            {
                herbertImage = ((System.Drawing.Image)(resources.GetObject("StandingEyeClosedZOOMIN.Image")));
            }
            else
                herbertImage = ((System.Drawing.Image)(resources.GetObject("StandingEyeClosed.Image")));
            //Create a memory bitmap for storing the image drawn
            //Bitmap memoryBitmap = new Bitmap(132, 132);

            //Create a graphics object from bitmap object
            if (LevelImg == null)
            {
                LevelImg = new Bitmap(208 * GlobalData.ZoomInZoomOut, 208 * GlobalData.ZoomInZoomOut);
            }
            Graphics gfx = Graphics.FromImage(LevelImg);

            // Create a new pen that we shall use for drawing the line
            Pen myPen = new Pen(Color.Black);
            Pen BP = new Pen(Color.Black, 1);
            if (LevelImg == null)
            {
                gfx.FillRectangle(System.Drawing.Brushes.White, 0, 0, 208 * zizo, 208 * zizo);


                //Create a memory stream to store the image in it.
                //MemoryStream mstream=new MemoryStream();
                // Loop and create the dotted panel on the form
                for (int i = 0; i < HConstants.GRIDDOTSPERLINE; i++)
                    for (int j = 0; j < HConstants.GRIDDOTSPERLINE; j++)
                    {
                        if (GlobalData.ZoomInZoomOut == HConstants.ZOOMOUT)
                            gfx.FillEllipse(System.Drawing.Brushes.Black, (i) * HConstants.DOTSPACE * zizo + 6.0F, (j) * HConstants.DOTSPACE * zizo + 6.0F, DOTRADIOUS, DOTRADIOUS);
                        else
                            gfx.FillEllipse(System.Drawing.Brushes.Black, (i) * HConstants.DOTSPACE * zizo + 6.0F * 2 + 0.5F, (j) * HConstants.DOTSPACE * zizo + 6.0F * 2 + 0.5F, 2.5F, 2.5F);

                    }
            }

            int direction = 0;
            //Declare the starting and ending positions of X and Y

            int startX = 100 * GlobalData.ZoomInZoomOut;
            int startY = 100 * GlobalData.ZoomInZoomOut;


            if (GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
            {
                startX += 6;
                startY += 6;
            }
            else
            {
                startX += 3;
                startY += 3;
            }
            int endX = startX;
            int endY = startY;
            preDestPosX = startX;
            preDestPosY = startY;
            destPosX = startX;
            destPosY = startY;
            BP.Width = GlobalData.ZoomInZoomOut;

            int iDotspace = HConstants.DOTSPACE * zizo;




            //Declare the pattern text as the string


            //Convert the text into char array

            int cnt = 0;
            for (int i = 0; i < strLevelTrace.Length; i++)
            {
                cnt = i;
                switch (strLevelTrace[i])
                {
                    case 's':
                        switch (direction)
                        {
                            case 0:
                                if ((endY - iDotspace) > 0)
                                {
                                    endY = endY - iDotspace;
                                    destPosY = endY;
                                    gfx.DrawLine(BP, startX, startY, endX, endY);
                                    startX = endX;
                                    startY = endY;
                                }
                                break;

                            case 1:
                                if ((endX + iDotspace) < (208 * GlobalData.ZoomInZoomOut - iDotspace))
                                {
                                    endX = endX + iDotspace;
                                    destPosX = endX;
                                    gfx.DrawLine(BP, startX, startY, endX, endY);
                                    startX = endX;
                                    startY = endY;
                                }
                                break;

                            case 2:
                                if ((endY + iDotspace) < (208 * GlobalData.ZoomInZoomOut - iDotspace))
                                {
                                    endY = endY + iDotspace;
                                    destPosY = endY;
                                    gfx.DrawLine(BP, startX, startY, endX, endY);
                                    startX = endX;
                                    startY = endY;
                                }
                                break;

                            case 3:
                                if ((endX - iDotspace) >= 0)
                                {
                                    endX = endX - iDotspace;
                                    destPosX = endX;
                                    gfx.DrawLine(BP, startX, startY, endX, endY);
                                    startX = endX;
                                    startY = endY;
                                }
                                break;
                        }
                        fSavePath(0);
                        break;

                    case 'r':
                        direction++;
                        if (direction == 4)
                            direction = 0;
                        break;

                    case 'l':
                        direction--;
                        if (direction == -1)
                            direction = 3;
                        break;
                }
                CurDir = direction;               
                
                preDestPosX = startX;
                preDestPosY = startY;
            }
            PointF HerbertLocation = new PointF();
            if (GlobalData.ZoomInZoomOut == HConstants.ZOOMIN)
            {
                HerbertLocation = new System.Drawing.Point(((startX * GlobalData.ZoomInZoomOut) - 3 * GlobalData.ZoomInZoomOut - 1), ((startY * GlobalData.ZoomInZoomOut) - 3 * GlobalData.ZoomInZoomOut - 1 + 2));
            }
            else
                HerbertLocation = new System.Drawing.Point(((startX * GlobalData.ZoomInZoomOut) - 6), ((startY * GlobalData.ZoomInZoomOut) - 6));

            gfx.Dispose();

        }
        
        /// <summary>
        /// To generate the trace from the trace signature.
        /// </summary>
        /// <param name="strTracesignature"></param>
        /// <returns></returns>
        public static string GetTrace(string strTracesignature)
        {
            string path = "";
            for (int i = 0; i < strTracesignature.Length; i++)
            {
                path = DecodeTrace(Convert.ToChar(strTracesignature.Substring(i, 1))) + path;
            }
            return path;
        }

        /// <summary>
        /// Generate bit decode herbert path  in herbert command 
        /// form in bit format and packing them into single character.
        /// 
        /// 00 = s , 01 = r
        /// 10 = l , 11 = last appending bit
        /// /// Ex: 16 = 00010000 = ssrs  
        /// </summary>
        /// <param name="path">trace</param>
        /// <returns>decoded herbert command</returns>
        private static string DecodeTrace(char trace)
        {
            // four times to fill last place blank places with 11
            int iTrace = Convert.ToInt32(trace);
            string strHerbertpath = string.Empty;
            for (int i = 0; i < 4; i++)
            {
                int commnad = ((iTrace >> (2 * i)) & 0x03);
                switch (commnad)
                {
                    case 0: strHerbertpath = strHerbertpath + "s";
                        break;
                    case 1: strHerbertpath = strHerbertpath + "r";
                        break;
                    case 2: strHerbertpath = strHerbertpath + "l";
                        break;
                    default: strHerbertpath = strHerbertpath + "";
                        break;
                }
            }
            return strHerbertpath;
        }

        #endregion
#endif
	}
}
