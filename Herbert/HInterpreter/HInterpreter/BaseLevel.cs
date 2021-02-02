#define EXCEPTION
#define CONTEST
#define RUNSTATE
//#define DESIGNER



using System;
using System.Drawing;
using System.Data;
using System.Threading;
using System.Collections;
using System.Globalization;
//using System.Collections;
//using System.ComponentModel;
//using System.Windows.Forms;
//using System.Threading;
//using System.Data.SqlClient;
//using System.Configuration;

namespace Designer
{      
       
    /// <summary>
    /// This Class contain the attributes require for the interpreter.
    /// </summary>
    public class BaseLevel
    {
       
        /// <summary>
        /// <p>blnRorLStep: ture if next execution char is r or l</p>
        /// </summary>
        public bool blnRorLStep = false;

        /// <summary>
        /// this variable holds the r or l in case of step.
        /// </summary>
        public char StepCharRorL = ' ';

        public static int herbertState = 0;

        public int[] actualVirtualLineNum = new int[MAXLINES];
        


        //Added by Vivek Balagangadharan
        // Description : These variables are required to store information about
        //				 currently executing line.	
        // Added On : 20-Jul-2005

        /// <summary>
        /// This structure contains information about
        /// the currenlty executing line.
        /// </summary>

        /// <summary>
        /// This structure contains information about each line of the solution
        /// like function name, number of parameters
        /// and starting point of this function definition.
        /// An array is created of this structure and 0th element is
        /// used for storing the execution line in the code written in
        /// the code editor.
        /// </summary>

        public struct AtLineInfoStruct
        {
            /// <summary>
            /// Start point of the line.
            /// </summary>
            public int iStart;
            /// <summary>
            /// Funtion Name.
            /// </summary>
            public char cFunc;
            /// <summary>
            /// Number of parameters in the function
            /// </summary>
            public short sNumParms;
            /// <summary>
            /// flag for status
            /// </summary>
            public short sFlags;
            /// <summary>
            /// Character array representing the parameters
            /// </summary>
            public char[,] paramNames;
            /// <summary>
            /// End index used for Line tracing.
            /// </summary>
            public int iEndIndexLT;
            /// <summary>
            /// Start index used for Line tracing.
            /// </summary>
            public int iStartIndexLT;
            public int[] frameCharNo;
        }
        /// <summary>
        /// This contains information of which is the currently executing
        /// char.
        /// </summary>
        public struct tCurLineStruct
        {
            public int iLineNum;
            public int iFirstCmd;
            public int iLength;
        }
        /// <summary>
        /// Summary description for Levels.
        /// </summary>
        public struct tHStateStruct
        {
            /// <summary>
            /// Program counter
            /// </summary>
            public int PC;

            /// <summary>
            /// Currently executing line
            /// </summary>
            public int curLine;

            /// <summary>
            /// Current operator to be executed 
            /// </summary>
            public char CurrentOp;

            public char RunState;
            public short Flags;
        }
        


        /// <summary>
        /// variable indicating for the level, 
        /// whether the end of program is reached
        /// </summary>
        public volatile object EndofProg = false;


        //Added by Vivek Balagangadharan
        // Description : 
        // Added On : 21-Jul-2005
        public bool blnResumeState = false;

        //State for step menu
        public bool blnStepState = true;

        //State for go menu
        public bool blnGoState = true;

        //State for Halt Menu
        public bool blnHaltState = false;

        //State for Reset menu
        public bool blnResetState = false;

        /// <summary>
        /// this tells the state of go button whether it is displaying the go image or pause image.
        /// </summary>
        // state 0 : Go
        // state 1 : Pause
        public int goPauseState = 0;

        /// <summary>
        /// variable used to indicate that step by step Menu is in use.
        /// </summary>
        public bool stepByStep = false;

        /// <summary>
        /// Variable to indicate that herbert has completed one step.
        /// this is used for step by step execution.
        /// </summary>
        public bool OneStepDone = false;

        /// <summary>
        /// Variable to see if go button is clicked this is
        /// used only in case of Step by Step process.
        /// </summary>
        public bool goClicked = false;

        //public bool blnLevelFinishedMsg = false;

        /// <summary>
        /// variable which takes care of herbert is reset or not.
        /// </summary>
        public bool blnHReset = false;


        /// <summary>
        /// instance of tHStateStructure
        /// </summary>
        public tHStateStruct tHState;

 

        // variables defined to be used in interperter.

        /// <summary>
        /// Stores the length of the program
        /// </summary>
        public int iProgLen;

        /// <summary>
        /// Used for stack tracing.
        /// </summary>
        public int iTrashedSP;

        /// <summary>
        /// Used as current stack pointer.
        /// </summary>
        public int iSP;

        public bool sStackTrashed = false;

        public bool sStackInitialised = true;

        /// <summary>
        /// number of lines in the code editor.
        /// </summary>
        public int iNumLines;

        /// <summary>
        /// Used for editor text box tracing, it keeps the number of lines in textbox
        /// </summary>
        public int lineCount;

        /// <summary>
        /// instance of tCurLineStruct
        /// </summary>
        public tCurLineStruct tCurLine;
        const int MAXLINES = 15;
        public const int STACKSIZE = 1024 * 16;
        public AtLineInfoStruct[] atLineInfo = new AtLineInfoStruct[MAXLINES];

        /// <summary>
        /// this variable contains currently executing line.
        /// </summary>
        public char[] acCurrentLine = new char[1010];

        public bool syntaxCheck = true, SyntaxError = false;
        public bool checkSyntaxGo = true;
        public int[] aiStack = new int[STACKSIZE];
        /// <summary>
        /// Therad used for saving level data on level change.
        /// </summary>
        public int stackInitialisationCounter = 0;

        /// <summary>
        /// this char array contains program written in code editor
        /// as char formate.
        /// </summary>
        public char[] pcProg;

        public static string getTextProgram(string levelProgram)
        {
            return levelProgram.Replace("\n", "\r\n");
        }  

       
    
    }
}
