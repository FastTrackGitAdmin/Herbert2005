using System;
using System.Data;
using System.Windows.Forms;

namespace Designer
{
	enum HMode
	{
		Contest,
		Designer,
		Tutorial,
		Session
	}

	/// <summary>
	/// Summary description for GlobalData.
	/// </summary>
    [System.Net.WebPermission(System.Security.Permissions.SecurityAction.Assert, Unrestricted = true)]
	public class GlobalData
	{
		# region UserVariables

		internal static int ZoomInZoomOut = HConstants.ZOOMOUT;
		internal static HMode HerbertMode = HMode.Tutorial;
		
		/// <summary>
		/// vairable takes care of whether user can skip level or not.
		/// </summary>
		public static int SkipLevel = 1;

		/// <summary>
		/// data set which contains all data related to herbert like level data user data, contest data.
		/// </summary>
		public static DataSet dsAllHData = null;

		public static string GUID = "";

		public static int ContestantId;

		public static string URL;
        
		public static HService.HDataService HS;
		
		public static int iContestId = 0;

		public static int iIniSeedRandomDelay = 0;

		public static bool IsLocalVersion = false;

		public static int DeginerLevelPoints = 0;
		public static int sessionTimeOut = 0;
		
		/// <summary>
		/// this is the title appearing on the application. 
		/// </summary>
		public static string herbertTitle = "Herbert - Tutorial";
		
		/// <summary>
		/// this is the int variable which defines the sleep time in miliseconds 
		/// for a given thread. 
		/// </summary>
		public static int smoothScrollInterval = 0;
		
		/// <summary>
		/// number of levels in each bucket. On solving the requisite no. of levels, these many levels would appear
		/// </summary>
		public static int numLevelsPerBucket;

		/// <summary>
		/// percentage of total levels required to be solved for more levels(= bucket size) to be displayed.
		/// </summary>
		public static int reqdLevelSolvePercentage;

		//public static int iLevelId = -1;
		public static string iLevelId = "-1";
        
        public static string iPatternId = "-1";

        public static bool IsreloadApplication = false;
        /// <summary>
        /// enable or disable timer for Herbert
        /// </summary>
        public static int iTimerEnable = -1;
        /// <summary>
        /// enable or disable Switch Contest for Herbert
        /// </summary>
        public static int iShowSwtichContest = 1;

        // added by NikhilK on 30/10/2007 for Remember Me faeture with Switch Contest feature
        /// <summary>
        /// Indicates whether contest has been switched using 'Switch Contest' menu
        /// </summary>
        public static bool isSwitchContestPerformed = false;

		/// <summary>
		/// keeps track of whether this version is going to use web service or dll for database access.
		/// </summary>
		public static string strLocalVersion;
		
		/// <summary>
		/// This is the variable which is set to notify the polling thread 
		/// to silently upload the data at the contest close
		/// TODO: Silent Upload
		/// </summary>
		public static bool bSilentUpload=false;
		/// <summary>
		/// Date Time to check at upload
		/// </summary>
		public static DateTime dtCheck;
                
		/// <summary>
		/// To store the loginname of the user
		/// </summary>
        public static string loginName = string.Empty;

        /// <summary>
        /// To store the loginname of the user
        /// </summary>
        public static string password = string.Empty;

        public static bool IsShowContestList = false;

        public static bool IsLoadFromFile = false;
        /// <summary>
        /// To indicate that session file get generated.
        /// </summary>
        internal static bool IsSessionSaved = false;

        internal static string strSessionFilePath = "";

        // added by NikhilK on 25/10/2007
        public static int currentUnsolvedBucket = 0;

        // added by NikhilK on 26/10/2007
        /// <summary>
        /// Indicates whether email has been filled using 'RememberMe' file
        /// </summary>
        public static bool isLoadFromRememberMeFile = false;

        // added by NikhilK on 29/10/2007 for toggling timer
        /// <summary>
        /// Indicates whether the user has toggled timer visibility
        /// </summary>
        public static bool isTimerToggleVisible = false;

        // added by NikhilK on 1/11/2007
        /// <summary>
        /// Indicates whether a particular contest is finished
        /// </summary>
        public static bool isContestFinished = false;

        // added by NikhilK on 1/11/2007
        /// <summary>
        /// Indicates whether user is behind a proxy
        /// </summary>
        public static bool isBehindProxy = false;

        /// <summary>
        /// To check whether Herbert has been launched from site or directly using the URL
        /// </summary>
        public static bool isHerbertFromSite = false;

        /// <summary>
        /// To check whether the contest being played is of the type Employment Contest
        /// </summary>
        public static bool isEmploymentContest = false;

        /// <summary>
        /// To read culture settings of user's machine while starting application
        /// </summary>
        public static object startingCulture = new object();

        //added By Rajesh To show Contetst closing once.
        internal static bool IsContestClosingMsgShown = false;

        public static bool IsDesignerClosed = false;

		# endregion

		# region Constructor
		/// <summary>
		/// constructor.
		/// </summary>
		public GlobalData()
		{
			//
			// TODO: Add constructor logic here
			//			
		}
		# endregion

		public static string ProxyUserName = "";
		public static string ProxyUserPassword = "";
		public static string ProxyDomain = "";
        //Added By Rajesh
        public static string ProxyServer = "";
        public static string ProxyPort = "";
		public static bool authReg = false;

		/// <summary>
		/// initlizes the web service with give session timeout.
		/// </summary>
		/// <param name="SessionTimeOut">time out for webservice</param>
		public static void initlizeWS()
		{
			try
			{	
				if(HS == null || authReg)
				{
					HS = new Designer.HService.HDataService();
					HS.Url = URL;
					HS.Timeout = sessionTimeOut;
					HS.PreAuthenticate=true;
					HS.Proxy = GetProxy();
				}
			}
			catch(Exception exp)
			{
				throw new Exception(exp.Message);
			}
		}

		public static System.Net.WebProxy GetProxy()
		{
            System.Net.WebProxy wProxy = null;
			if(ProxyUserName != "")
			{
                System.Net.ICredentials credentials;
                if (ProxyDomain == "")
                {
                    credentials = new System.Net.NetworkCredential(ProxyUserName, ProxyUserPassword);
                }
                else
                    credentials = new System.Net.NetworkCredential(ProxyUserName, ProxyUserPassword, ProxyDomain);
                //Added By Rajeshc 15/11
                
                try
                {
                    wProxy = System.Net.WebProxy.GetDefaultProxy();
                    int port = wProxy.Address.Port;
                }
                catch
                {
                    int port;
                    try
                    {
                        port = Int32.Parse(ProxyPort.ToString());
                    }
                    catch { port = 0; }
                    try
                    {
                        wProxy = new System.Net.WebProxy(ProxyServer, port);
                    }
                    catch
                    {
                    }
                }
                try
                {
                    wProxy.Credentials = credentials;
                }
                catch { }
                return wProxy;
			}
            try
            {
                wProxy = System.Net.WebProxy.GetDefaultProxy();
                int port = wProxy.Address.Port;
            }
            catch
            {
                wProxy= null;
            }
            return wProxy;
		}
	}
}
