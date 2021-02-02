#define DESIGNER
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Designer
{
    public class HInterpreter
    {

        /// <summary>
        /// Stack size which is used to store call of herbert functions.
        /// </summary>
        public const int STACKSIZE = 1024 * 16; /* stacksize can be no more than 1024*16 */

        /// <summary>
        /// CR is char which represents the retrun key.
        /// </summary>
        private const char CR = '\n';
        /// <summary>
        /// Short constants defined, TRUE = 1 and FALSE = 0.
        /// </summary>
        private const short TRUE = 1, FALSE = 0;
        /// <summary>
        /// these contants are defined for different states of herbert.
        /// HFBLINK is used for blinking herbert.		
        /// </summary>
        private const short HFBLINK = 0x01;

        /// <summary>
        /// these contants are defined for different states of herbert.
        /// HFSSTEP is used for steps.	
        /// </summary>
        private const short HFSSTEP = 0x02;

        /// <summary>
        /// these contants are defined for different states of herbert.
        /// and HFSTEPD is for saying that herbert is in normal position.
        /// </summary>
        private const short HFSTEPD = 0x04;

        /// <summary>
        /// these contants are defined for different states of herbert.
        /// HFNWLIN is used for saying that new line execution is now started.
        /// </summary>
        private const short HFNWLIN = 0x08;

        /// <summary>
        /// defined some constants like static SIZE, TRUE and FALSE
        /// </summary>
        /// Vijay, check why only 20 lines.
        //bc1311		private const int MAXLINES = 20, STACKSIZE = 1024*256;
        private const int MAXNUM = 255, MINNUM = -256;
        /// <summary>
        /// Max number of lines allowed in the code editor. Which is 15.
        /// </summary>
        public const int MAXLINES = 15;
        /// <summary>
        /// Max number of characters allowed in one line of Herbert code. 
        /// <p>This is not Herbert Char count but simple char count.</p>
        /// </summary>
        private const int MAXCHARSONLINE = 127;

        /// <summary>
        /// variables that contains the boxes starting point as TraceX and top as TraceY and width of the 
        /// trace box.
        /// </summary>
        private int TraceX, TraceWidth;
        /// <summary>
        /// this variable is used for killing the thread which is 
        /// running in an infinite loop. This thread runs herbert.
        /// Used by reference..
        /// </summary>
        public volatile object progClosing=false;
        public volatile object m_blnThreadStarted=true;
        /// <summary>
        /// To store value of displayEdTraceOnce
        /// </summary>
        bool displayEdTraceOnce = true;
        /// <summary>
        /// variable to set trace on or off.
        /// </summary>
        private bool traceOn = false;
        private bool isInternalErrorLogged = true;
        /// <summary>
        /// variable to check whether trace is displayed or not.
        /// </summary>
        private short sTraceDisplayed=1;
        
        /// <summary>
        /// this is an array defined to be used in interpreter.
        /// </summary>
        private short[] asOptions = { TRUE, TRUE, FALSE };
        
        /// <summary>
        /// variables for storing the data of richtext box
        /// </summary>
        int txtTotalLength;
        int txtTotalLines;
        string[] txtLines;

        /// <summary>
        /// this is the int variable which defines the sleep time in miliseconds 
        /// for a given thread. 
        /// </summary>
        internal static int herbtSpeed = 90;

        /// <summary>
        /// check if herbert is redrawn
        /// this is to avoid the error of 
        /// object locking this variable is
        /// set to true in paint method of herbertPicbox
        /// and it is set to false if location of pic box
        /// is changed in our thread.
        /// </summary>		
        private bool hrbtRedrawn = true;
        private bool blnChangeLevel = false;
        bool blnLineTraceStarted = false;
        /// <summary>
        /// bool varaible indicates system error.
        /// Should be used as by reference
        /// </summary>
        private volatile bool blnSystemError = false;


        #region Delegates and events to Handle the Communication with Herbert
        object bSemaphore = true;
        object bMonitorLChange = false;

        public delegate void deleEditorTrace(int iLine);
        public deleEditorTrace raiseEditorTrace;

        public delegate void deleGetTextForLTFrame();
        public deleGetTextForLTFrame raiseGetTextForLTFrame;

        public delegate void deleInternalError(int iErrorCode);
        public deleInternalError raiseInternalError;

        public delegate void deleKillThreadNoWait();
        public deleKillThreadNoWait raiseKillThreadNoWait;

        public delegate void deleKillThread();
        public deleKillThread raiseKillThread;

        //This to handle the Enod of program for interpreter.
        public delegate void deleInterpretrEndOfProgram();
        public deleInterpretrEndOfProgram raiseInterpretrEndOfProgram;

        public delegate void deleDisplayTrace(int iTraceX, int iTraceWidth);
        public deleDisplayTrace raiseDisplayTrace;

        public delegate void deleDispMessage(string strMessage);
        public deleDispMessage raiseDispMessage;

        public delegate void deleSelectError(int ipStartChar, int ipSelectionLength);
        public deleSelectError raiseSelectError;

        public delegate bool deleMoveHerbie();
        public deleMoveHerbie raiseMoveHerbie;

        public delegate void deleInterpretHerbertPositionUpdate();
        public deleInterpretHerbertPositionUpdate raiseInterpretHerbertPositionUpdate;

        public delegate void deleCommand(string strOp);
        public deleCommand raiseCommand;

        public delegate void deleTxtCoEditorVisible(bool isVisible);
        public deleTxtCoEditorVisible raiseTxtCodeEditorVisible;

        public delegate void deleSetdisplayEdTraceOnce(bool isdisplayEdTraceOnce);
        public deleTxtCoEditorVisible raiseSetdisplayEdTraceOnce;

        public delegate void deleEnableMenu();
        public deleEnableMenu raiseEnableMenu;

        public delegate void deleCheckSyntaxMenuUpdate();
        public deleCheckSyntaxMenuUpdate raiseCheckSyntaxMenuUpdate;

        public delegate void deleCheckSyntaxUpadteLevelData(ref BaseLevel objBaseLevel);
        public deleCheckSyntaxUpadteLevelData raiseCheckSyntaxUpadteLevelData;

        public delegate void deleHrbtRedrawn(bool IsHrbtRedrawn);
        public deleHrbtRedrawn raiseHrbtRedrawn;

        public delegate void deleSetGoPauseState();
        public deleSetGoPauseState raiseSetGoPauseState;

        public delegate void deleEvalCurLineMoveHerbert();
        public deleEvalCurLineMoveHerbert raiseEvalCurLineMoveHerbert;
        public delegate void testdelegate();
        public testdelegate mytestdelegate;

        public delegate void deleUpdateMaxStackLength(int MaxStackLength);
        public deleUpdateMaxStackLength raiseUpdateMaxStackLength;

        public delegate void deleUpdateNumProgramSteps();
        public deleUpdateNumProgramSteps raiseUpdateNumProgramSteps;
        #endregion

        /// <summary>
        /// This contain the Level information
        /// </summary>
        private BaseLevel curLevel;

        public HInterpreter(ref BaseLevel iLevel, object oProgClosing, object oM_blnThreadStarted)
        {
            curLevel = (BaseLevel)iLevel;
            progClosing = oProgClosing;
            m_blnThreadStarted = oM_blnThreadStarted;
        }
        public HInterpreter(string strLevelSolution)
        {
            curLevel = new BaseLevel();
            curLevel.tHState.Flags = 0;
            curLevel.tHState.CurrentOp = ' ';
            curLevel.tHState.curLine = 0;
            curLevel.tHState.PC = 0;
            curLevel.tCurLine.iFirstCmd = 0;
            curLevel.tCurLine.iLength = 0;
            curLevel.tCurLine.iLineNum = 0;
            //atLineInfo = new AtLineInfoStruct[MAXLINES];
            curLevel.aiStack = new int[STACKSIZE];
            //09/03/05
            /*reset all status variables to appropriate values.*/
            curLevel.blnHReset = false;
            curLevel.goClicked = true;
            curLevel.iSP = 0;
            curLevel.stackInitialisationCounter = 0;

            if (curLevel.blnRorLStep)
            {
                curLevel.blnRorLStep = false;
                //Command = StepCharRorL.ToString().ToUpper();
            }
            curLevel.pcProg = strLevelSolution.Replace("\r", "").ToCharArray();
            if(!strLevelSolution.Contains("\r\n"))
            {
                strLevelSolution = strLevelSolution.Replace("\n", "\r\n");
            }
            getTextData(strLevelSolution.Replace("\n", ""));            
            
            curLevel.blnGoState = false;
            curLevel.blnHaltState = true;
            curLevel.tHState.Flags |= HFSSTEP;
            curLevel.tHState.Flags &= ~HFSTEPD;
            /*set menu enable r disable acording to need.*/
            curLevel.stepByStep = false;
            curLevel.blnResumeState = false;
            //curLevel.pcProg = strLevelSolution.Replace("\r", "\n").ToCharArray();            
            //iProgLen = txtCodeEditor.Text.Length;
            curLevel.iProgLen = curLevel.pcProg.Length;
            //getTextData(strLevelSolution);
            curLevel.tHState.Flags =10;         
        }
        public void SetProgClosing(ref object oProgClosing)
        {
            progClosing = oProgClosing;
        }
        public void SetTraceOn(bool IsTraceOn)
        {
            traceOn = IsTraceOn;
        }
        public void SetDisplayEdTraceOnce(bool IsDisplayEdTraceOnce)
        {
            displayEdTraceOnce = IsDisplayEdTraceOnce;
        }
        public void SetTraceDisplayed(short sTraceDisplayed)
        {
            this.sTraceDisplayed = sTraceDisplayed;
        }
        public void SetHerbertSpeed(int iherbtSpeed)
        {
            herbtSpeed = iherbtSpeed;
        }
        public void SetTextData(int iTxtTotalLength, int iTxtTotalLines, string[] strATxtLines)
        {
            txtTotalLength = iTxtTotalLength;
            txtTotalLines = iTxtTotalLines;
            txtLines = strATxtLines;
        }
        /// <summary>
        /// generates the lines out of textbox data.
        /// </summary>
        private void getTextData(string strLevelSolution)
        {
            if (strLevelSolution != null)
            {
                txtTotalLength = strLevelSolution.Replace("\r", "").Length;
                txtTotalLines = 0;
                for (int i = 0; i < strLevelSolution.Length; i++)
                {
                    if (strLevelSolution[i] == '\r')
                    {
                        txtTotalLines++;
                    }
                }                
                txtTotalLines++;
                txtLines = new string[txtTotalLines];
                txtLines = strLevelSolution.Split("\r".ToCharArray());
                for (int i = 0; i < txtTotalLines; i++)
                {
                    txtLines[i] = txtLines[i].Replace("\r", "");
                }
            }
        }
        public void SetBlnLineTraceStarted(bool IsblnLineTraceStarted)
        {
            blnLineTraceStarted = IsblnLineTraceStarted;
        }
        
        public void invokeDelegate()
        {
            mytestdelegate();
        }


        /// <summary>
        /// checks for stack frame if it is valid.
        /// </summary>
        /// <param name="iSP">current stack point</param>
        /// <returns>true or false</returns>
        private bool ValidFrame(int iSP)
        {
            int iNextFrame;
            /* argument stack contains 0*/
            if (curLevel.aiStack[iSP] == 0)
                return (false);
            else
            {
                iNextFrame = (int)(iSP + 1 + (((uint)curLevel.aiStack[iSP] & 0xf000) >> 12)) % STACKSIZE;	/*+box info*/ /*??????????*/

                bool a = curLevel.iTrashedSP >= iNextFrame && curLevel.iTrashedSP <= iSP;

                if (iNextFrame > iSP)
                    return (curLevel.iTrashedSP >= iNextFrame || curLevel.iTrashedSP <= iSP);
                else
                    return (curLevel.iTrashedSP >= iNextFrame && curLevel.iTrashedSP <= iSP);
            }
        }
        /// <summary>
        /// returns the frame.
        /// </summary>
        /// <returns>new stack pointer.</returns>
        private int PopFrame()/*result must be assigned to iSP - trashes old frame*/
        {
            //			int iNextFrame;
            //
            //			iNextFrame=(int)(iSP+1+(((uint)(aiStack[iSP]&0xf000))>>12))%STACKSIZE; /*?????????????*/
            //			if (iSP == iTrashedSP)
            //				aiStack[iSP]=0;
            //			return(iNextFrame);
            int iNextFrame;
#if(DESIGNER)
            if(raiseUpdateMaxStackLength!=null)
            raiseUpdateMaxStackLength(curLevel.iSP);
#endif
            iNextFrame = (int)(curLevel.iSP + 1 + (((uint)(curLevel.aiStack[curLevel.iSP] & 0xf000)) >> 12)) % STACKSIZE; /*?????????????*/
            if (curLevel.iSP == curLevel.iTrashedSP)
                curLevel.aiStack[curLevel.iSP] = 0;
            //bc1280 start - added to check for popping a frame into a trashed portion of stack
            if (((iNextFrame > curLevel.iSP && (curLevel.iSP < curLevel.iTrashedSP && iNextFrame >= curLevel.iTrashedSP))
                || (iNextFrame < curLevel.iSP && (curLevel.iSP < curLevel.iTrashedSP || iNextFrame >= curLevel.iTrashedSP))))
            {
                //DispMessage("Program too complex (stack overflow)");
                //InternalError(-1);
            }
            //bc1280 end

            return (iNextFrame);
        }


        /// <summary>
        /// returns the current argument frame.
        /// </summary>
        /// <param name="iValFrame">valid frame number.</param>
        /// <returns>valid frame number</returns>
        private int CurrentArgFrame(int iValFrame)
        {
            int iArgFrame;

            //bc1311			iArgFrame=aiStack[(iValFrame+1)%STACKSIZE];
            //bc1311			/* not able to understand */
            //bc1311			return((((iArgFrame&0xc000)^0x4000) == 0)?(iArgFrame&0x07ff):iValFrame); /*?????????*/
            for (iArgFrame = iValFrame; ((curLevel.aiStack[(iArgFrame + 1) % STACKSIZE] & 0xc000) ^ 0x4000) == 0; iArgFrame = curLevel.aiStack[((iArgFrame & 0x3fff) + 1) % STACKSIZE])
            {
                //bc1280 start
                if (curLevel.sStackTrashed && curLevel.iTrashedSP > curLevel.iSP && curLevel.iTrashedSP < (iArgFrame & 0x3fff))
                {
                    //DispMessage("Program too complex (stack overflow)");
                    //InternalError(-1);
                }
            }

            iArgFrame &= 0x3fff;

            if (curLevel.sStackTrashed && curLevel.iTrashedSP > curLevel.iSP && curLevel.iTrashedSP < (iArgFrame))
            {
                //DispMessage("Program too complex (stack overflow)");
                //InternalError(-1);
            }
            //bc1280 end
            //			if (curLevel.sStackTrashed && iTrashedSP > iSP && iTrashedSP < iArgFrame) 
            //			{
            //				DispMessage("Program too complex (stack overflow)");
            //				InternalError(-1);
            //			}
            return (iArgFrame);

        }


        /// <summary>
        /// skips the current execution pointer to end of argument.
        /// </summary>
        /// <param name="piPC">current execution point</param>
        private void PSkipToArgEnd(ref int piPC)
        {
            int iParenLevel;

            iParenLevel = 0;
            while (iParenLevel != 0 || (curLevel.pcProg[piPC] != ',' && curLevel.pcProg[piPC] != ')'))
            {
                if (piPC == curLevel.iProgLen - 1)
                    break;
                switch (curLevel.pcProg[(piPC)++])
                {
                    case '(':
                        iParenLevel++;
                        break;
                    case ')':
                        iParenLevel--;
                        break;
                    default:
                        break;
                }
            }
        }


        /// <summary>
        /// skips the current execution pointer to end of argument.
        /// </summary>
        /// <param name="piPC">current execution point</param>
        private void CSkipToArgEnd(ref int piPC)
        {
            int iParenLevel;
            char c;

            iParenLevel = 0;
            for (c = curLevel.acCurrentLine[curLevel.tCurLine.iFirstCmd + (piPC)++];
                iParenLevel != 0 || (c != ',' && c != ')');
                c = curLevel.acCurrentLine[curLevel.tCurLine.iFirstCmd + (piPC)++])
            {
                if (curLevel.tCurLine.iFirstCmd + (piPC) == curLevel.iProgLen - 1)
                    break;
                switch (c)
                {
                    case '(':
                        iParenLevel++;
                        break;
                    case ')':
                        iParenLevel--;
                        break;
                    default:
                        break;
                }
            }
            (piPC)--;
        }
        /// <summary>
        /// this returns the number of chars before the starting char of the an actual line in txt code editor.
        /// </summary>
        /// <param name="iActualLineNum">actual line number got from function getActualLineNum(int iVirtualLineNum)</param>
        /// <returns>num chars before any lines start point excluding '\n'</returns>
        private int getActualCharNum(int iActualLineNum)
        {
            //for proper error messages.
            int numChars = 0;
            //			lock(txtCodeEditor)
            //			{
            for (int k = 0; k < iActualLineNum; k++)
            {
                numChars += txtLines[k].Length;
            }
            //			}
            return numChars;
        }
        /// <summary>
        /// gets the actual line number of the line in the structure tHState which is used in interperter.
        /// both line numbers are different.
        /// </summary>
        /// <param name="iVirtualLineNum">line of tHstate struct.</param>
        /// <returns>actual line number.</returns>
        private int getActualLineNum(int iVirtualLineNum)
        {
            //for proper error messages.
            int iY = 0;
            //get line number from top iY will contain line number.
            for (int k = 0; k < curLevel.atLineInfo[iVirtualLineNum].iStart; k++)
            {
                if (curLevel.pcProg[k] == CR)
                    iY += 1;
            }
            return iY;
        }
        //Added by Sujith on 07/20/2005 for solving issue 1718
        //Start
        private bool CheckProgram()
        {
            bool blnFuncName = false;
            for (int i = 0; i < curLevel.pcProg.Length; i++)
            {
                if (curLevel.pcProg[i] != ':' && curLevel.pcProg[i] != '|' && curLevel.pcProg[i] != ',' && curLevel.pcProg[i] != '-' && curLevel.pcProg[i] != '+' && curLevel.pcProg[i] != '(' && curLevel.pcProg[i] != ')' && !Char.IsDigit(curLevel.pcProg[i]) && !Char.IsUpper(curLevel.pcProg[i]) && curLevel.pcProg[i] != 's' && curLevel.pcProg[i] != 'r' && curLevel.pcProg[i] != 'l' && curLevel.pcProg[i] != CR)
                {
                    for (int j = 0; j < curLevel.iNumLines; j++)
                    {

                        if (curLevel.atLineInfo[j].cFunc == curLevel.pcProg[i])
                        {
                            blnFuncName = true;
                        }
                    }
                    if (!blnFuncName)
                    {
                        if(raiseDispMessage!=null)
                        raiseDispMessage("No function definition found for '" + curLevel.pcProg[i] + "'");
                        if(raiseSelectError!=null)
                        raiseSelectError(i, 1);
                        return false;
                    }
                }

                blnFuncName = false;
            }
            return true;
        }


        // End
        private bool CheckNumArgs(ref int index, int iVirtualLineNum)
        {
            int i = index;
            int persistantIndex = index;
            int numComma = 0;
            bool openBracks = false, closeBracks = false;
            int iOpenParamLevel = 0, iCloseParamLevel = 0;
            while (i < curLevel.iProgLen && curLevel.pcProg[i] != CR)
            {
                bool refdone = false;
                switch (curLevel.pcProg[i])
                {
                    case '(': openBracks = true;
                        if (iOpenParamLevel == 1)
                        {
                            if(raiseDispMessage!=null)
                                raiseDispMessage("Too many '(' for function '" + curLevel.pcProg[persistantIndex].ToString() + "'");
                            if(raiseSelectError!=null)
                                raiseSelectError(i, 1);
                            return false;
                        }
                        iOpenParamLevel++;
                        break;
                    case ')': closeBracks = true;
                        if (iCloseParamLevel == 1)
                        {
                            if(raiseDispMessage!=null)
                                raiseDispMessage("Too many ')' for function '" + curLevel.pcProg[persistantIndex].ToString() + "'");
                            if(raiseSelectError!=null)
                                raiseSelectError(i, 1);
                            return false;
                        }
                        iCloseParamLevel++;
                        if (numComma < curLevel.atLineInfo[iVirtualLineNum].sNumParms && i - 1 >= 0)
                        {
                            if (curLevel.atLineInfo[iVirtualLineNum].paramNames[numComma, 1] == 'I')
                            {
                                if (!Char.IsNumber(curLevel.pcProg[i - 1]) && !Char.IsUpper(curLevel.pcProg[i - 1]) && curLevel.pcProg[i - 1] != ')')
                                {
                                    if (raiseDispMessage != null)
                                        raiseDispMessage("Numeric argument expected ");
                                    if (raiseSelectError != null)
                                        raiseSelectError(i - 1, 1);
                                    return false;
                                }

                            }
                            else if (curLevel.atLineInfo[iVirtualLineNum].paramNames[numComma, 1] == 'S')
                            {
                                if (Char.IsNumber(curLevel.pcProg[i - 1]) && !Char.IsUpper(curLevel.pcProg[i - 1]))
                                {
                                    if (raiseDispMessage != null)
                                        raiseDispMessage("Procedural argument expected");
                                    if (raiseSelectError != null)
                                        raiseSelectError(i - 1, 1);
                                    return false;
                                }

                            }
                        }
                        break;
                    case ',':
                        if (numComma < curLevel.atLineInfo[iVirtualLineNum].sNumParms && i - 1 >= 0)
                        {
                            if (curLevel.atLineInfo[iVirtualLineNum].paramNames[numComma, 1] == 'I')
                            {
                                if (!Char.IsNumber(curLevel.pcProg[i - 1]) && !Char.IsUpper(curLevel.pcProg[i - 1]))
                                {
                                    if (raiseDispMessage != null)
                                        raiseDispMessage("Integer parameter expected");
                                    if (raiseSelectError != null)
                                        raiseSelectError(i - 1, 1);
                                    return false;
                                }

                            }
                            else if (curLevel.atLineInfo[iVirtualLineNum].paramNames[numComma, 1] == 'S')
                            {
                                if (Char.IsNumber(curLevel.pcProg[i - 1]) && !Char.IsUpper(curLevel.pcProg[i - 1]))
                                {
                                    if (raiseDispMessage != null)
                                        raiseDispMessage("Character parameter expected");
                                    if (raiseSelectError != null)
                                        raiseSelectError(i - 1, 1);
                                    return false;
                                }

                            }
                        }
                        numComma++;
                        break;
                    default:
                        try
                        {
                            if (i > index && Char.IsLower(curLevel.pcProg[i]))
                            {
                                int txtCENumLine = 0;
                                //							lock(txtCodeEditor)
                                //							{
                                txtCENumLine = txtTotalLines;

                                //							}
                                bool functfound = false;
                                for (int z = 1; z < txtCENumLine; z++)
                                {
                                    if (curLevel.pcProg[i] == curLevel.atLineInfo[z].cFunc)
                                    {
                                        //begin changed by piyush
                                        //for Error:
                                        /*
                                        No function found error for following code
                                        a(A,B,C):ABa(Ass,BC,Cs)
                                        b:rslsr
                                        a(b,b,)
                                        */
                                        functfound = true;
                                        if (curLevel.atLineInfo[z].sNumParms > 0)
                                        {
                                            //end changed by piyush

                                            if (curLevel.atLineInfo[iVirtualLineNum].paramNames[numComma, 1] == 'I')
                                            {
                                                if (!Char.IsNumber(curLevel.pcProg[i]) && !Char.IsUpper(curLevel.pcProg[i]))
                                                {
                                                    if(raiseDispMessage!=null)
                                                        raiseDispMessage("Integer parameter expected");
                                                    if(raiseSelectError!=null)
                                                        raiseSelectError(i, 1);
                                                    return false;
                                                }
                                            }
                                            functfound = true;
                                            bool blnRetrun = CheckNumArgs(ref i, z);
                                            if (!blnRetrun)
                                                return false;
                                            refdone = true;
                                            break;
                                        }
                                    }

                                }
                                if (curLevel.pcProg[i] != 's' && curLevel.pcProg[i] != 'l' && curLevel.pcProg[i] != 'r' && curLevel.pcProg[i] != '|' && !functfound)
                                {
                                    if (raiseDispMessage != null)
                                        raiseDispMessage("No function definition found for '" + curLevel.pcProg[i].ToString() + "'");
                                    if (raiseSelectError != null)
                                        raiseSelectError(i, 1);
                                    return false;
                                }
                            }
                            else if (Char.IsWhiteSpace(curLevel.pcProg[i]))
                            {
                                if (raiseDispMessage != null)
                                    raiseDispMessage("Whitespace is not allowed");
                                if(raiseSelectError != null)
                                    raiseSelectError(i, 1);
                                return false;
                            }
                            else if (!(Char.IsLetterOrDigit(curLevel.pcProg[i]) || curLevel.pcProg[i] == '+' || curLevel.pcProg[i] == '-'))
                            {
                                if (curLevel.pcProg[i] == '&')
                                {
                                    if(raiseDispMessage!=null)
                                        raiseDispMessage("'&&' is an invalid character");
                                    if(raiseSelectError!=null)
                                        raiseSelectError(i, 1);
                                    return false;
                                }
                                else
                                {
                                    if (raiseDispMessage != null)
                                        raiseDispMessage("'" + curLevel.pcProg[i].ToString() + "' is an invalid character");
                                    if(raiseSelectError!=null)
                                        raiseSelectError(i, 1);
                                    
                                    return false;
                                }
                            }
                        }
                        catch
                        {

                            ////Console.WriteLine(exp.Message);
                        }
                        break;
                }
                if (i < curLevel.iProgLen)
                    if (curLevel.pcProg[i] == ')' && !refdone)
                    {
                        index = ++i;
                        break;
                    }
                if (!refdone)
                    i++;
                else
                    refdone = false;
            }
            if (!openBracks && curLevel.atLineInfo[iVirtualLineNum].sNumParms != 0)
            {
                if(raiseDispMessage!=null)
                    raiseDispMessage("No parameters passed to function '" + curLevel.pcProg[persistantIndex].ToString() + "'");
                if(raiseSelectError!=null)
                    raiseSelectError(persistantIndex, 1);
                return false;
            }
            else if (!closeBracks && curLevel.atLineInfo[iVirtualLineNum].sNumParms != 0)
            {
                if (raiseDispMessage != null)
                    raiseDispMessage("')' missing for function '" + curLevel.pcProg[persistantIndex].ToString() + "'");
                if (raiseSelectError != null)
                    raiseSelectError(persistantIndex, 1);
                return false;
            }
            else if (numComma + 1 != curLevel.atLineInfo[iVirtualLineNum].sNumParms && curLevel.atLineInfo[iVirtualLineNum].sNumParms != 0)
            {
                if (raiseDispMessage != null)
                    raiseDispMessage("Incorrect number of arguments for function '" + curLevel.pcProg[persistantIndex].ToString() + "' ");
                if (raiseSelectError != null)
                    raiseSelectError(persistantIndex, 1);
                return false;
            }
            return true;
        }
        private bool checkInvalidChar()
        {
            //txtTotalLength;
            for (int i = 0; i < txtTotalLength; i++)
            {
                int ASCIIval = (int)curLevel.pcProg[i];
                switch (curLevel.pcProg[i])
                {
                    case (char)10:
                        break;
                    case ',':
                        break;
                    case ':':
                        break;
                    case ')':
                        //						//begin added by piyush
                        //						//for bugid 1329
                        //						if(pcProg[i - 1] == '+' || pcProg[i - 1] == '-')
                        //						{
                        //							DispMessage("'"+pcProg[i - 1] + "' is an invalid character");
                        //							selectError(i-1,1);
                        //							return false;
                        //						}
                        //						//end added by piyush
                        break;
                    case '(':
                        if (i - 1 >= 0)
                        {
                            if (curLevel.pcProg[i - 1] == 's' || curLevel.pcProg[i - 1] == 'r' || curLevel.pcProg[i - 1] == 'l')
                            {
                                if (raiseDispMessage != null)
                                    raiseDispMessage("'" + curLevel.pcProg[i - 1] + "' keywords cannot be used as function name");
                                if (raiseSelectError != null)
                                    raiseSelectError(i - 1, 1);
                                return false;
                            }
                        }
                        //begin added by piyush
                        //						//for bugid 1329
                        //						
                        //						if(i+1 < iProgLen && pcProg[i+1] != CR)
                        //						{
                        //							if(pcProg[i + 1] == '+' || pcProg[i + 1] == '-')
                        //							{
                        //								DispMessage("'"+pcProg[i + 1] + "' is an invalid character");
                        //								selectError(i+1,1);
                        //								return false;
                        //							}
                        //						}
                        //						//end added by piyush
                        break;
                    case '-':
                        //begin added by piyush on 31 Mar 2005
                        if (i + 1 < curLevel.iProgLen && i - 1 >= 0)
                        {
                            if (curLevel.pcProg[i - 1] == '(' && curLevel.pcProg[i + 1] == ')')
                            {
                                if (raiseDispMessage != null)
                                    raiseDispMessage("'" + curLevel.pcProg[i] + "' is an invalid character");
                                if (raiseSelectError != null)
                                    raiseSelectError(i, 1);
                                return false;
                            }
                            else if (curLevel.pcProg[i + 1] == ')')
                            {
                                if (raiseDispMessage != null)
                                    raiseDispMessage("'" + curLevel.pcProg[i] + "' is an invalid character");
                                if (raiseSelectError != null)
                                    raiseSelectError(i, 1);
                                return false;
                            }//added condition for more than 1 consecutive -
                            else if (curLevel.pcProg[i - 1] != '(' && !Char.IsLetterOrDigit(curLevel.pcProg[i - 1]))
                            {
                                if (raiseDispMessage != null)
                                    raiseDispMessage("'" + curLevel.pcProg[i] + "' is an invalid character");
                                if (raiseSelectError != null)
                                    raiseSelectError(i, 1);
                                return false;
                            }
                        }
                        //end added by piyush on 31 Mar 2005
                        //						if(i+1 < iProgLen)
                        //							if(Char.IsNumber(pcProg[i-1]) && Char.IsNumber(pcProg[i+1]))
                        //							{
                        ////bc								DispMessage("Subtraction of numbers is not allowed");
                        ////bc								selectError(i,1);
                        ////bc								return false;
                        //							}
                        break;
                    case '+':
                        //begin added by piyush on 31 Mar 2005
                        if (i + 1 < curLevel.iProgLen && i - 1 >= 0)
                        {
                            if (curLevel.pcProg[i - 1] == '(' && curLevel.pcProg[i + 1] == ')')
                            {
                                if (raiseDispMessage != null)
                                    raiseDispMessage("'" + curLevel.pcProg[i] + "' is an invalid character");
                                if (raiseSelectError != null)
                                    raiseSelectError(i, 1);
                                return false;
                            }
                            else if (curLevel.pcProg[i + 1] == ')')
                            {
                                if (raiseDispMessage != null)
                                    raiseDispMessage("'" + curLevel.pcProg[i] + "' is an invalid character");
                                if (raiseSelectError != null)
                                    raiseSelectError(i, 1);
                                return false;
                            }	//added condition for more than 1 consecutive +
                            else if (curLevel.pcProg[i - 1] != '(' && !Char.IsLetterOrDigit(curLevel.pcProg[i - 1]))
                            {
                                if (raiseDispMessage != null)
                                    raiseDispMessage("'" + curLevel.pcProg[i] + "' is an invalid character");
                                if (raiseSelectError != null)
                                    raiseSelectError(i, 1);
                                return false;
                            }
                        }
                        //end added by piyush on 31 Mar 2005
                        //						if(i+1 < iProgLen)
                        //							if(Char.IsNumber(pcProg[i-1]) && Char.IsNumber(pcProg[i+1]))
                        //							{
                        ////bc								DispMessage("Addition of numbers is not allowed");
                        ////bc								selectError(i,1);
                        ////bc								return false;
                        //							}
                        break;
                    case '|':
                        break;
                    default:
                        int z = i;
                        if (Char.IsDigit(curLevel.pcProg[i]))
                        {
                            StringBuilder sbNumber = new StringBuilder();
                            while (Char.IsDigit(curLevel.pcProg[i]))
                            {
                                sbNumber.Append(curLevel.pcProg[i]);
                                i++;
                                if (i == curLevel.iProgLen)
                                    break;
                            }

                            i--;

                            /*Added by Vivek to set range of numeric values between -255 and 256*/

                            try
                            {
                                short tmpNum = 0;
                                if (z - 1 > 0)
                                {
                                    if (curLevel.pcProg[z - 1] == '-')
                                    {
                                        tmpNum = System.Int16.Parse(sbNumber.Insert(0, "-").ToString());
                                    }
                                    else
                                        tmpNum = System.Int16.Parse(sbNumber.ToString());
                                }
                                if (tmpNum < -256)
                                {
                                    if (raiseDispMessage != null)
                                        raiseDispMessage("Number should not be less than -256");
                                    if (raiseSelectError != null)
                                        raiseSelectError(i - sbNumber.Length + 1, sbNumber.Length);
                                    return false;
                                }
                                else if (tmpNum > 255)
                                {
                                    if (raiseDispMessage != null)
                                        raiseDispMessage("Number should not be greater than 255");
                                    if (raiseSelectError != null)
                                        raiseSelectError(i - sbNumber.Length + 1, sbNumber.Length);
                                    return false;
                                }

                            }
                            catch (OverflowException)
                            {
                                if (z - 1 > 0)
                                {
                                    if (curLevel.pcProg[z - 1] == '-')
                                    {
                                        if (raiseDispMessage != null)
                                            raiseDispMessage("Number should not be less than -256");
                                        if (raiseSelectError != null)
                                            raiseSelectError(i - sbNumber.Length + 1, sbNumber.Length);
                                        return false;
                                    }
                                    else
                                    {
                                        if (raiseDispMessage != null)
                                            raiseDispMessage("Number should not be greater than 255");
                                        if (raiseSelectError != null)
                                            raiseSelectError(i - sbNumber.Length + 1, sbNumber.Length);
                                        return false;
                                    }
                                }
                            }
                        }
                        else if (!Char.IsLetterOrDigit(curLevel.pcProg[i]))
                        {
                            if (Char.IsWhiteSpace(curLevel.pcProg[i]))
                            {
                                if (raiseDispMessage != null)
                                    raiseDispMessage("Whitespace is not allowed");
                                if (raiseSelectError != null)
                                    raiseSelectError(i, 1);
                                return false;
                            }
                            else if (curLevel.pcProg[i] == '&')
                            {
                                if (raiseDispMessage != null)
                                    raiseDispMessage("'&&' is an invalid character");
                                if (raiseSelectError != null)
                                    raiseSelectError(i, 1);
                                return false;
                            }
                            else
                            {
                                if (raiseDispMessage != null)
                                    raiseDispMessage("'" + curLevel.pcProg[i].ToString() + "' is an invalid character");
                                if (raiseSelectError != null)
                                    raiseSelectError(i, 1);
                                return false;
                            }
                        }
                        else if ((ASCIIval < 65) || (ASCIIval > 122))
                        {
                            if (raiseDispMessage != null)
                                raiseDispMessage("'" + curLevel.pcProg[i].ToString() + "' is an invalid character");
                            if (raiseSelectError != null)
                                raiseSelectError(i, 1);
                            return false;
                        }
                        break;
                }
            }
            return true;
        }
        /// <summary>
        /// increases the current execution point by 1.
        /// </summary>
        /// <param name="piPC">current execution point.</param>
        /// <returns>always true</returns>
        private bool CheckVStatement(ref int piPC)
        {
            (piPC)++;
            return (true);
        }
        /// <summary>
        /// checks the defination of function whether this is allowed or not.
        /// </summary>
        /// <param name="piNumParms">number of parameters of fucntion</param>
        /// <param name="iPC">current execution point.</param>
        /// <returns>returns true if defination is as per the rules else false.</returns>
        private bool CheckDef(ref int piNumParms, int iPC)
        {
            //int i;
            bool sCorrect;
            char c;

            sCorrect = true;
            piNumParms = 0;
            c = curLevel.pcProg[iPC++];
            /* function name is upper case letter */
            if (c == ':')
            {
                if(raiseDispMessage!=null)
                    raiseDispMessage("Missing function name");
                sCorrect = false;
            }
            else
                if (!Char.IsLetter(c))
                {
                    //begin added by piyush on 31 Mar 2005
                    if (Char.IsWhiteSpace(c))
                    {
                        if (raiseDispMessage != null)
                            raiseDispMessage("Whitespace is not allowed");
                        if(raiseSelectError!=null)
                            raiseSelectError(iPC - 1, 1);
                        sCorrect = false;
                    }
                    //end added by piyush on 31 Mar 2005
                    else
                    {
                        if (raiseDispMessage != null)
                            raiseDispMessage("Only lowercase letters allowed for function names");
                        sCorrect = false;
                    }
                }
                else
                    if (!Char.IsLower(c))
                    {
                        if (raiseDispMessage != null)
                            raiseDispMessage("Use lowercase function names");
                        sCorrect = false;
                    }
            if (sCorrect)
            {
                switch (curLevel.pcProg[iPC++])
                {
                    case '(':
                        do
                        { /* navigate till you don't find char ',' */
                            c = curLevel.pcProg[iPC++];
                            if (!Char.IsUpper(c))
                            {
                                if (raiseDispMessage != null)
                                    raiseDispMessage("Illegal parameter name");
                                sCorrect = false;
                            }
                            (piNumParms)++;
                        } while (curLevel.pcProg[iPC++] == ',');
                        iPC--;
                        /* check to see if last char is ')' else give proper error msg. */
                        if (sCorrect && curLevel.pcProg[iPC++] != ')')
                        {
                            if (raiseDispMessage != null)
                                raiseDispMessage("')' expected");
                            sCorrect = false;
                        }
                        /* check to see if last char is next to last char is ':' else give proper error msg. */
                        if (sCorrect && curLevel.pcProg[iPC++] != ':')
                        {
                            if (raiseDispMessage != null)
                                raiseDispMessage("':' expected");
                            sCorrect = false;
                        }
                        break;
                    /* if function defined without any parameter */
                    case ':':
                        while (sCorrect && iPC < curLevel.iProgLen && curLevel.pcProg[iPC] != CR)
                        {
                            sCorrect = CheckVStatement(ref iPC); /* always returns true and increments the parameter pointer by 1*/
                        }
                        break;
                    default:
                        if (raiseDispMessage != null)
                            raiseDispMessage("':' or '(' expected");
                        sCorrect = false;
                        break;
                }
            }
            return (sCorrect);
        }
        private bool findDuplicateFunNames()
        {
            bool blnReturn = true;
            int txtCENumLines = 0;
            txtCENumLines = txtTotalLines;
            for (int i = 1; i < txtCENumLines; i++)
            {
                for (int j = i + 1; j < txtCENumLines; j++)
                {
                    if (curLevel.atLineInfo[i].cFunc == curLevel.atLineInfo[j].cFunc && curLevel.atLineInfo[i].cFunc != '\0')
                    {
                        //selectError(getActualCharNum(atLineInfo[j].iStart), 1);
                        if (raiseDispMessage != null)
                            raiseDispMessage("Duplicate function name '" + curLevel.atLineInfo[i].cFunc.ToString() + "'");
                        blnReturn = false;
                        break;
                    }
                }
                if (!blnReturn)
                    break;
            }
            for (int i = 0; i < txtCENumLines; i++)
            {
                if (txtLines[i].Length == 0)
                {
                    int x = i + 1;
                    if (raiseDispMessage != null)
                        raiseDispMessage("Line no " + x.ToString() + ": Blank lines are not allowed");
                    blnReturn = false;
                    break;
                }
            }
            return blnReturn;
        }
        private bool findParametersAndTypes()
        {
            int txtceNumLine = 0;

            txtceNumLine = txtTotalLines;

            for (int i = 1; i < curLevel.iNumLines; i++)
            {
                if (curLevel.atLineInfo[i].sNumParms > 15)
                {
                    if(raiseDispMessage != null)
                        raiseDispMessage("Function '" + curLevel.pcProg[curLevel.atLineInfo[i].iStart].ToString() + "': More than 15 parameters are not allowed");
                    if(raiseSelectError!=null)
                        raiseSelectError(curLevel.atLineInfo[i].iStart, 1);
                    return false;
                }
                curLevel.atLineInfo[i].paramNames = new char[curLevel.atLineInfo[i].sNumParms, 2];
                int index = 0;
                if (curLevel.atLineInfo[i].sNumParms >= 0)
                {
                    int j = 0;
                    for (j = curLevel.atLineInfo[i].iStart; curLevel.pcProg[j] != ':' && j < curLevel.iProgLen; j++)
                    {
                        switch (curLevel.pcProg[j])
                        {
                            case ',':
                                break;
                            case '(':
                                break;
                            case ')':
                                break;
                            default:
                                if (Char.IsUpper(curLevel.pcProg[j]) && Char.IsLetter(curLevel.pcProg[j]) && curLevel.atLineInfo[i].sNumParms > 0)
                                {
                                    curLevel.atLineInfo[i].paramNames[index, 0] = curLevel.pcProg[j];
                                    curLevel.atLineInfo[i].paramNames[index, 1] = 'A';
                                    for (int y = index - 1; y >= 0; y--)
                                    {
                                        if (curLevel.atLineInfo[i].paramNames[y, 0] == curLevel.atLineInfo[i].paramNames[index, 0])
                                        {
                                            if (raiseDispMessage != null)
                                                raiseDispMessage("Duplicate parameter name for function'" + curLevel.atLineInfo[i].cFunc.ToString() + "'");
                                            if (raiseSelectError != null)
                                                raiseSelectError(j, 1);
                                            return false;
                                        }
                                    }
                                    index++;
                                }
                                break;
                        }
                    }
                    //begin added by piyush for limiting max chars on line to 127 on 31 Mar 2005
                    int charCountPerLine = 0;
                    //end added by piyush for limiting max chars on line to 127 on 31 Mar 2005
                    for (int k = j; k < curLevel.iProgLen && curLevel.pcProg[k] != CR; k++)
                    {
                        charCountPerLine++;
                        if (Char.IsUpper(curLevel.pcProg[k]))
                        {
                            bool IsParamDefined = false;

                            for (int paramCount = 0; paramCount < curLevel.atLineInfo[i].sNumParms; paramCount++)
                            {
                                if (curLevel.atLineInfo[i].paramNames[paramCount, 0] == curLevel.pcProg[k])
                                {
                                    IsParamDefined = true;
                                    break;
                                }
                            }

                            if (IsParamDefined == false)
                            {
                                if (raiseDispMessage != null)
                                    raiseDispMessage("Parameter '" + curLevel.pcProg[k].ToString() + "' is not found in function definition");
                                if(raiseSelectError!=null)
                                    raiseSelectError(k, 1);
                                return false;
                            }
                        }
                        for (int l = 0; l < curLevel.atLineInfo[i].sNumParms; l++)
                        {
                            if (curLevel.pcProg[k] == curLevel.atLineInfo[i].paramNames[l, 0])
                            {
                                if (k - 1 >= 0)
                                    switch (curLevel.pcProg[k - 1])
                                    {
                                        case '|':
                                            break;
                                        case ':':
                                            break;
                                        case ',':
                                            break;
                                        case '(':
                                            break;
                                        case ')':
                                            break;
                                        case '+':
                                            if (curLevel.atLineInfo[i].paramNames[l, 1] != 'S' || curLevel.atLineInfo[i].paramNames[l, 1] == 'A')
                                                curLevel.atLineInfo[i].paramNames[l, 1] = 'I';
                                            else
                                            {
                                                if(raiseDispMessage!=null)
                                                    raiseDispMessage("Parameter '" + curLevel.atLineInfo[i].paramNames[l, 0].ToString() + "': procedural argument used as numeric");
                                                if(raiseSelectError!=null)
                                                    raiseSelectError(k, 1);
                                                return false;
                                            }
                                            break;
                                        case '-':
                                            if (curLevel.atLineInfo[i].paramNames[l, 1] != 'S' || curLevel.atLineInfo[i].paramNames[l, 1] == 'A')
                                                curLevel.atLineInfo[i].paramNames[l, 1] = 'I';
                                            else
                                            {
                                                if (raiseDispMessage != null)
                                                    raiseDispMessage("Parameter '" + curLevel.atLineInfo[i].paramNames[l, 0].ToString() + "': procedural argument used as numeric");
                                                if (raiseSelectError != null)
                                                    raiseSelectError(k, 1);
                                                return false;
                                            }
                                            break;
                                        default:
                                            if (Char.IsLetter(curLevel.pcProg[k - 1]))
                                            {
                                                if (curLevel.atLineInfo[i].paramNames[l, 1] != 'I' || curLevel.atLineInfo[i].paramNames[l, 1] == 'A')
                                                    curLevel.atLineInfo[i].paramNames[l, 1] = 'S';
                                                else
                                                {
                                                    if (raiseDispMessage != null)
                                                        raiseDispMessage("Parameter '" + curLevel.atLineInfo[i].paramNames[l, 0].ToString() + "' : procedural argument used as numeric");
                                                    if (raiseSelectError != null)
                                                        raiseSelectError(k, 1);
                                                    return false;
                                                }
                                                break;
                                            }
                                            else if (curLevel.pcProg[k - 1] == ' ')
                                            {
                                                if (raiseDispMessage != null)
                                                    raiseDispMessage("Whitespace is not allowed");
                                                if (raiseSelectError != null)
                                                    raiseSelectError(k - 1, 1);
                                                return false;
                                            }
                                            else if ((curLevel.pcProg[k - 1] != CR))
                                            {
                                                //begin changed by piyush on 31 Mar 2005
                                                if (raiseDispMessage != null)
                                                    raiseDispMessage("Invalid character");
                                                //end changed by piyush on 31 Mar 2005
                                                if (raiseSelectError != null)
                                                    raiseSelectError(k - 1, 1);
                                                return false;
                                            }
                                            break;
                                    }
                                if (k + 1 < curLevel.iProgLen)
                                    switch (curLevel.pcProg[k + 1])
                                    {
                                        case '|':
                                            break;
                                        case ':':
                                            break;
                                        case ',':
                                            break;
                                        case '(':
                                            break;
                                        case ')':
                                            break;
                                        case '+':
                                            if (curLevel.atLineInfo[i].paramNames[l, 1] != 'S' || curLevel.atLineInfo[i].paramNames[l, 1] == 'A')
                                                curLevel.atLineInfo[i].paramNames[l, 1] = 'I';
                                            else
                                            {
                                                if (raiseDispMessage != null)
                                                    raiseDispMessage("Parameter '" + curLevel.atLineInfo[i].paramNames[l, 0].ToString() + "': procedural argument used as numeric");
                                                if (raiseSelectError != null)
                                                    raiseSelectError(k, 1);
                                                return false;
                                            }
                                            break;
                                        case '-':
                                            if (curLevel.atLineInfo[i].paramNames[l, 1] != 'S' || curLevel.atLineInfo[i].paramNames[l, 1] == 'A')
                                                curLevel.atLineInfo[i].paramNames[l, 1] = 'I';
                                            else
                                            {
                                                if (raiseDispMessage != null)
                                                    raiseDispMessage("Parameter '" + curLevel.atLineInfo[i].paramNames[l, 0].ToString() + "': procedural argument used as numeric");
                                                if (raiseSelectError != null)
                                                    raiseSelectError(k, 1);
                                                return false;
                                            }
                                            break;
                                        default:
                                            if (Char.IsLetter(curLevel.pcProg[k + 1]))
                                            {
                                                if (curLevel.atLineInfo[i].paramNames[l, 1] != 'I' || curLevel.atLineInfo[i].paramNames[l, 1] == 'A')
                                                    curLevel.atLineInfo[i].paramNames[l, 1] = 'S';
                                                else
                                                {
                                                    if (raiseDispMessage != null)
                                                        raiseDispMessage("Parameter '" + curLevel.atLineInfo[i].paramNames[l, 0].ToString() + "': procedural argument used as numeric");
                                                    if (raiseSelectError != null)
                                                        raiseSelectError(k, 1);
                                                    return false;
                                                }
                                                break;
                                            }
                                            else if (Char.IsWhiteSpace(curLevel.pcProg[k - 1]))
                                            {
                                                if (raiseDispMessage != null)
                                                    raiseDispMessage("Whitespace is not allowed");
                                                if (raiseSelectError != null)
                                                    raiseSelectError(k + 1, 1);
                                                return false;
                                            }
                                            else if ((curLevel.pcProg[k + 1] != CR))
                                            {
                                                //begin changed by piyush on 31 Mar 2005
                                                if (raiseDispMessage != null)
                                                    raiseDispMessage("Invalid character");
                                                //end changed by piyush on 31 Mar 2005
                                                if (raiseSelectError != null)
                                                    raiseSelectError(k + 1, 1);
                                                return false;
                                            }
                                            break;
                                    }
                            }
                        }
                    }
                    //added by vivek
                    for (int k = j; k < curLevel.iProgLen && curLevel.pcProg[k] != CR; k++)
                    {
                        if (curLevel.pcProg[k] == '(')
                        {
                            if ((k - 1 >= 0) && Char.IsUpper(curLevel.pcProg[k - 1]))
                            {
                                if (raiseDispMessage != null)
                                    raiseDispMessage("Function name expected.");
                                if (raiseSelectError != null)
                                    raiseSelectError(k - 1, 1);
                                return false;
                            }
                        }
                    }
                    //begin added by piyush for limiting max chars on line to 127 on 31 Mar 2005
                    charCountPerLine = charCountPerLine - 1;
                    if (charCountPerLine > MAXCHARSONLINE)
                    {
                        int actualLineNumber = getActualLineNum(i) + 1;
                        if (raiseDispMessage != null)
                            raiseDispMessage("Number of characters on line " + actualLineNumber.ToString() + " exceeded maximum limit");
                        return false;
                    }
                    //end added by piyush for limiting max chars on line to 127 on 31 Mar 2005
                }
            }
            return true;
        }
        private bool CheckFunctionCalls()
        {
            bool blnReturn = true;
            blnReturn = CheckExecutionLine();
            if (!blnReturn)
                return blnReturn;
            //			int totalLines = txtCodeEditor.Lines.Length;
            //			int refindex = 0;
            //			for(int i = 1; i < totalLines; i++)
            //			{
            //				char c = atLineInfo[i].cFunc;
            //				int charindex = 0;
            //				while(charindex < pcProg.Length)
            //				{
            //					if(pcProg[charindex] == c && charindex != atLineInfo[i].iStart)
            //					{
            //						if(charindex+1 < pcProg.Length)
            //						{
            //							if(pcProg[charindex + 1] != ':')
            //							{
            //								if(atLineInfo[i].sNumParms == 0 && pcProg[charindex+1] == '(')
            //								{
            //									DispMessage("No parameter required for function '" +pcProg[charindex].ToString() + "'");
            //									blnReturn = false;
            //									break;
            //								}
            //								else if(atLineInfo[i].sNumParms != 0)
            //								{
            //									refindex = charindex;
            //									blnReturn = CheckNumArgs(ref refindex, atLineInfo[i].sNumParms);
            //									if(!blnReturn)
            //										break;
            //								}
            //							}
            //						}
            //						else if(atLineInfo[i].sNumParms != 0)
            //						{
            //							DispMessage("Number of parameter for function '" + pcProg[charindex].ToString()+"' " + "not matching");
            //							blnReturn = false;
            //							break;
            //						}
            //
            //					}
            //					charindex++;
            //				}
            //			}
            blnReturn = parseForCheckNumArgs(0);
            return blnReturn;
        }
        /// <summary>
        /// dummy function.
        /// </summary>
        /// <param name="iPC"></param>
        /// <returns></returns>
        private bool CheckExec(int iPC)
        {
            return (true);
        }
        private bool CheckExecutionLine()
        {
            if (txtLines[getActualLineNum(0)].Length > MAXCHARSONLINE)
            {
                int actualLineNumber = getActualLineNum(0) + 1;
                if (raiseDispMessage != null)
                    raiseDispMessage("Number of characters on line " + actualLineNumber.ToString() + " exceeded maximum limit");
                return false;
            }
            else
                if (txtTotalLines == 1)
                {
                    for (int i = 0; i < curLevel.pcProg.Length; i++)
                    {
                        if (curLevel.pcProg[i] != '|')
                        {
                            //bc added + and - check
                            if (!Char.IsLetter(curLevel.pcProg[i]) && curLevel.pcProg[i] != '+' && curLevel.pcProg[i] != '-')
                            {
                                if (raiseDispMessage != null)
                                    raiseDispMessage("Invalid character '" + curLevel.pcProg[i].ToString() + "' in execution line");
                                if(raiseSelectError!=null)
                                    raiseSelectError(i, 1);
                                return false;
                            }
                            else
                                if (Char.IsUpper(curLevel.pcProg[i]))
                                {
                                    if (raiseDispMessage != null)
                                        raiseDispMessage("Uppercase letters only allowed for parameter names");
                                    if (raiseSelectError != null)
                                        raiseSelectError(i, 1);
                                    return false;
                                }
                                else if (!(curLevel.pcProg[i] == 's' || curLevel.pcProg[i] == 'r' || curLevel.pcProg[i] == 'l' || curLevel.pcProg[i] == '|'))
                                {
                                    if (raiseDispMessage != null)
                                        raiseDispMessage("No function definition found for '" + curLevel.pcProg[i].ToString() + "'");
                                    if (raiseSelectError != null)
                                        raiseSelectError(i, 1);
                                    return false;
                                }
                        }
                    }
                }
                else
                {
                    for (int i = curLevel.atLineInfo[0].iStart; i < curLevel.atLineInfo[0].iStart + txtLines[getActualLineNum(0)].Length; i++)
                    {
                        if (curLevel.pcProg[i] != '|')
                        {
                            //bc added + and -
                            if (!Char.IsLetter(curLevel.pcProg[i]) && curLevel.pcProg[i] != '(' && curLevel.pcProg[i] != ')' && curLevel.pcProg[i] != '+' && curLevel.pcProg[i] != '-' && curLevel.pcProg[i] != ',' && !Char.IsNumber(curLevel.pcProg[i]))
                            {
                                if (raiseDispMessage != null)
                                    raiseDispMessage("Invalid character '" + curLevel.pcProg[i].ToString() + "' in execution line");
                                if (raiseSelectError != null)
                                    raiseSelectError(i, 1);
                                return false;
                            }
                            else
                                if (Char.IsUpper(curLevel.pcProg[i]))
                                {
                                    if (raiseDispMessage != null)
                                        raiseDispMessage("'" + curLevel.pcProg[i].ToString() + "' is undefined");
                                    if (raiseSelectError != null)
                                        raiseSelectError(i, 1);
                                    return false;
                                }
                        }
                    }
                }
            //			else
            //			{
            //				CheckNumArgs(ref atLineInfo[0].iStart, 0);
            //			}
            return true;
        }
        private bool parseForCheckNumArgs(int startIndex)
        {
            bool blnReturn = true;
            int totalLines = txtTotalLines;
            int refindex = 0;
            for (int i = 1; i < totalLines; i++)
            {
                char c = curLevel.atLineInfo[i].cFunc;
                if (c == 's' || c == 'l' || c == 'r' || c == '|')
                {
                    if (raiseDispMessage != null)
                        raiseDispMessage("'" + c.ToString() + "' cannot be used as function name");
                    if (raiseSelectError != null)
                        raiseSelectError(curLevel.atLineInfo[i].iStart, 1);
                    blnReturn = false;
                    break;
                }
                int charindex = startIndex;
                while (charindex < curLevel.pcProg.Length)
                {
                    if (curLevel.pcProg[charindex] == c && charindex != curLevel.atLineInfo[i].iStart)
                    {
                        if (charindex + 1 < curLevel.pcProg.Length)
                        {
                            if (curLevel.pcProg[charindex + 1] != ':')
                            {
                                if (curLevel.atLineInfo[i].sNumParms == 0 && curLevel.pcProg[charindex + 1] == '(')
                                {
                                    if (raiseDispMessage != null)
                                        raiseDispMessage("No parameters required for function '" + curLevel.pcProg[charindex].ToString() + "'");
                                    if (raiseSelectError != null)
                                        raiseSelectError(charindex, 1);
                                    blnReturn = false;
                                    break;
                                }
                                else if (curLevel.atLineInfo[i].sNumParms != 0)
                                {
                                    refindex = charindex;
                                    blnReturn = CheckNumArgs(ref refindex, i);
                                    if (!blnReturn)
                                        break;
                                }
                            }
                        }
                        else if (curLevel.atLineInfo[i].sNumParms != 0)
                        {
                            if (raiseDispMessage != null)
                                raiseDispMessage("Incorrect number of arguments for function '" + curLevel.pcProg[charindex].ToString() + "' ");
                            if (raiseSelectError != null)
                                raiseSelectError(charindex, 1);
                            blnReturn = false;
                            break;
                        }

                    }
                    charindex++;
                    if (!blnReturn)
                        break;
                }
            }
            return blnReturn;
        }
      

        /// <summary>
        /// checks for the syantax of the code written in the code editor.
        /// </summary>
        /// <returns>returns true if syntax is correct else false</returns>
        public bool CheckSyntax()/* called only from h-events once. */
        {
            /* don't forget
                no lines > 127
                max max #lines = 15
            */
            //int i;
            int iVerPC, iWorkPC, iNumParms = 0;
            bool sCorrect, sTemp;
            sCorrect = true;
            curLevel.iNumLines = 1;							/*save room for exec*/ /*executable line will be stored at 0 index */
            curLevel.atLineInfo[0].iStart = -1;
            iWorkPC = curLevel.tHState.PC = 0;                     //tHState( a structure defined in hvars.h)


            /* skip all those lines which are blank, means only enter is pressed. */
            char c = 'a';
            while (iWorkPC < curLevel.iProgLen && (c = curLevel.pcProg[iWorkPC]) == CR)
                iWorkPC++;
            iVerPC = iWorkPC;

            //Checking till the end of the program or till an error is reached

            while (sCorrect && iWorkPC < curLevel.iProgLen)
            { /* While loop A */
                sTemp = false;

                //Checking one line at a time, /*this looks whether this lines defines a function or not. */
                int iNumColns = 0;
                while (iWorkPC < curLevel.iProgLen && curLevel.pcProg[iWorkPC] != CR)
                { /* while loop B */
                    if (curLevel.pcProg[iWorkPC] == ':')
                    {
                        iNumColns++;
                        if (iNumColns > 1)
                        {
                            if(raiseDispMessage!=null)
                                raiseDispMessage("Only one ':' allowed for one function.");
                            if(raiseSelectError!=null)
                                raiseSelectError(iWorkPC, 1);
                            sCorrect = false;
                            break;
                        }
                        sTemp = true;
                    }
                    iWorkPC++;
                } /* end of while loop B*/
                if (iNumColns > 1)
                    break;
                /* if sTemp is true this means that this line contains a function defination. */
                if (sTemp)
                { //If the char is a colon. Probably evaluating a function
                    if (sCorrect = CheckDef(ref iNumParms, iVerPC))
                    { /* iVerPC is the current char number where a function def starts */

                        //If the number of lines exceeds particular number then show error - Too many lines. MAXLINES = 16
                        if (curLevel.iNumLines < MAXLINES)
                        {
                            curLevel.atLineInfo[curLevel.iNumLines].iStart = iVerPC; /*Starting point for the function in terms of char count in entire code. */
                            curLevel.atLineInfo[curLevel.iNumLines].cFunc = curLevel.pcProg[iVerPC]; //The function name -> This is stored at the line info array
                            curLevel.atLineInfo[curLevel.iNumLines].sNumParms = (short)iNumParms; // number of parameters for the function
                            curLevel.atLineInfo[curLevel.iNumLines].sFlags = 0;			/*not yet used*/
                            curLevel.atLineInfo[curLevel.iNumLines].iStartIndexLT = 0;
                            curLevel.atLineInfo[curLevel.iNumLines].iEndIndexLT = 0;
                            curLevel.actualVirtualLineNum[getActualLineNum(curLevel.iNumLines)] = curLevel.iNumLines;
                            curLevel.iNumLines++; /* increase this for next function defination. */
                        }
                        else
                        {
                            if(raiseDispMessage!=null)
                                raiseDispMessage("Too many lines");
                            //vijay
                            if(raiseKillThread!=null)
                                raiseKillThread();
                            //progClosing = true;
                            if(raiseCheckSyntaxMenuUpdate!=null)
                            raiseCheckSyntaxMenuUpdate();
                            curLevel.blnResetState = true;
                           
                            sCorrect = false;
                        }
                    }
                }
                else
                {
                    //Equivalent  to sCorrect = TRUE
                    /* checks whether too many executable lines are there. One and only one executable line is allowed*/
                    if (sCorrect = CheckExec(iVerPC))
                    {
                        if (curLevel.atLineInfo[0].iStart != -1)
                        { /* if iStart is not = -1 means we already have one exec line. */
                            if(raiseDispMessage!=null)
                                raiseDispMessage("Too many execution lines");
                            //vijay
                            if(raiseKillThread!=null)
                                raiseKillThread();
                            //progClosing = true;
                            if(raiseCheckSyntaxMenuUpdate!=null)
                            raiseCheckSyntaxMenuUpdate();
                            curLevel.blnResetState = true;
                            
                            sCorrect = false;
                        }
                        else
                        { /*store executable line here. and iStart will become none negative*/
                            curLevel.atLineInfo[0].iStart = iVerPC;
                            curLevel.atLineInfo[0].cFunc = ' ';
                            curLevel.atLineInfo[0].sNumParms = curLevel.atLineInfo[0].sFlags = 0;
                            curLevel.actualVirtualLineNum[getActualLineNum(0)] = 0;
                        }
                    }
                }


                /*reach to the end of line.*/
                while (iWorkPC < curLevel.iProgLen && curLevel.pcProg[iWorkPC] == CR)
                    iWorkPC++;
                iVerPC = iWorkPC; /* starting point of new line. */
            } /* end of while loop A*/

            /* if no executable line is there, one and only one executable line is allowed. */
            if (sCorrect && curLevel.atLineInfo[0].iStart == -1)
            {
                if(raiseCheckSyntaxMenuUpdate!=null)
                raiseCheckSyntaxMenuUpdate();
                if (raiseDispMessage != null)
                    raiseDispMessage("No execution line");
                //vijay
                if (raiseKillThread != null)
                    raiseKillThread();
                //progClosing = true;
                
                curLevel.blnResetState = true;
                
                //HaltHrbt_Click(null,null);
                sCorrect = false;
            }
            if (curLevel.iNumLines > 15 && sCorrect)
            {
                if(raiseCheckSyntaxMenuUpdate!=null)
                raiseCheckSyntaxMenuUpdate();
                if (raiseDispMessage != null)
                    raiseDispMessage("More than 15 lines of code is not allowed");
                //vijay
                if (raiseKillThread != null)
                    raiseKillThread();
                //progClosing = true;
               
                curLevel.blnResetState = true;
                
                //HaltHrbt_Click(null,null);
                sCorrect = false;
            }
            if (sCorrect)
                sCorrect = checkInvalidChar();
            if (sCorrect)
                sCorrect = findDuplicateFunNames();
            if (sCorrect)
                sCorrect = findParametersAndTypes();
            if (sCorrect)
                sCorrect = CheckFunctionCalls();

            //Added by Sujith on 07/20/2005 for solving issue 1718
            //Start
            if (sCorrect)
                sCorrect = CheckProgram();
            //End

            //iPc is defined as 0

            //Therefore if sCorrect = TRUE THEN -1
            //ELSE iLine = 0


            curLevel.tHState.curLine = curLevel.tHState.PC = (sCorrect) ? 0 : -1;

            if (!sCorrect)
            {
                if (raiseCheckSyntaxUpadteLevelData != null)
                    raiseCheckSyntaxUpadteLevelData(ref curLevel);
                curLevel.blnResetState = true;
                
            }
            curLevel.tHState.Flags |= HFNWLIN;
            //Not persistaing while going through the dll.
            //hunlock((**ahtTERecs[PLAYWIN]).htext);
            if (raiseCheckSyntaxUpadteLevelData!=null)
            raiseCheckSyntaxUpadteLevelData(ref curLevel);
            return (sCorrect);
        }
        /// <summary>
        /// this is the main execution function which takes care of 
        /// interpreting program.
        /// </summary>
        public void Interpreter()
        {
            //			if(!progClosing)
            //			{
            int iFirstChar, iLastChar, i, iLine, iVal, iArgFrame;
            char cOp;
            short sNewLine = 0;
            short sAllPos, sStop;
            if (displayEdTraceOnce)
            {
                displayEdTraceOnce = false;
               // raiseSetdisplayEdTraceOnce(false);
                //iStartIndexLT = iEndIndexLT = iFrameLT = 0;
                if (traceOn)
                {
                    if (raiseEditorTrace != null)
                        raiseEditorTrace(0);
                }
            }

            // evaluates current line and sets the value to memebers of tCurLine structure
            //////////* now program error may be here. *//////////
            ///
            if ((curLevel.tHState.Flags & HFNWLIN) > 0)
            {     //tHState defined structure HFWLIN is defined in hvars.h

                //if(!progClosing)
                EvalCurLine(curLevel.tHState.curLine); /* this contains 0 if code is correct else -1 this is set in CheckSyntax(), this case
				it will be 0. */
                if (traceOn)
                {
                    if (raiseGetTextForLTFrame != null)
                        raiseGetTextForLTFrame();
                }

                if (curLevel.tHState.PC >= curLevel.tCurLine.iLength)
                {
                    //Event Handler Work.
                    //if (isInternalErrorLogged)
                    //{
                    //    sbErrorLog.Append("\n Internal Error 20 raised in Interpreter()");
                    //}
                    if (raiseInternalError != null)
                        raiseInternalError(20);
                }
            }


            // set the value of sStop
            sStop = FALSE;           // local variable (short data type)

            // Probably to change the frame size, frame number and changing rectangle and process characters of particular line
            // Transfer no. of byte used etc
            for (cOp = curLevel.acCurrentLine[curLevel.tCurLine.iFirstCmd + curLevel.tHState.PC];  // acCurrentLine array of type char of size MAXLINE
                (cOp == '\0' || cOp == CR || cOp == ',' || cOp == ')') && !(sStop > 0); // check the condition depend on the syntax of code
                cOp = curLevel.acCurrentLine[curLevel.tCurLine.iFirstCmd + curLevel.tHState.PC])
            { // get the next character of line

                if (ValidFrame(curLevel.iSP))
                {
                    if ((curLevel.tHState.curLine = (curLevel.aiStack[curLevel.iSP] & 0x0780) >> 7) >= curLevel.iNumLines)  // iNumLines is integer variable define
                    {
                        //in vars.h..... aiStack[]-> data type int, defined in hvars.h of size 1024
                        //Event Handler Work
                        //if (isInternalErrorLogged)
                        //{
                        //    sbErrorLog.Append("\n Internal Error 22 raised in Interpreter()");
                        //}
                        if (raiseInternalError != null)
                            raiseInternalError(22);
                    }
                    curLevel.tHState.PC = curLevel.aiStack[curLevel.iSP] & 0x007f; // tHState structure

                    curLevel.iSP = PopFrame(); //PopFrame-> Method define hint.h.. I guess that its for iterating through frames
                    // frmaes--> either its layers of different level or frames or grid points

                    curLevel.tHState.Flags |= HFNWLIN;

                    EvalCurLine(curLevel.tHState.curLine);
                    if (traceOn)
                    {
                        if (raiseGetTextForLTFrame != null)
                            raiseGetTextForLTFrame();
                    }
                    if ((curLevel.tCurLine.iFirstCmd + curLevel.tHState.PC) >= curLevel.tCurLine.iLength)
                    {
                        //Event handler Work
                        //if (isInternalErrorLogged)
                        //{
                        //    sbErrorLog.Append("\n Internal Error 24 raised in Interpreter()");
                        //}
                        if (raiseInternalError != null)
                            raiseInternalError(24);
                    }
                }
                else
                {
                    sStop = TRUE;
                    if (!curLevel.sStackTrashed)
                    {
                        //vijay
                        if (raiseKillThreadNoWait != null)
                            raiseKillThreadNoWait();
                        //progClosing = true;
                        lock (curLevel.EndofProg)
                        {
                            curLevel.EndofProg = true;
                        }
                        //displayBox = false; commented by karthikeyan 16/03

                        //curLevel[currentLevelId].chkLevelFinished(currentLevelId);
# if(CONTEST)
						if(currentLevelIndex > 0)
							lblScoreCurrent.Text = curLevel.levelScore.ToString()+"/" + Level.getLevelPoints(currentLevelIndex).ToString();	
						else
							lblScoreCurrent.Text = curLevel.levelScore.ToString()+"/" + currentLevelIndex.ToString();
#endif
#if(DESIGNER)
                        //if(currentLevelIndex > 0)
                        //    lblScoreCurrent.Text = curLevel.levelScore.ToString()+"/" + Level.getLevelPoints(currentLevelIndex).ToString();	
                        //else
                        //    lblScoreCurrent.Text = curLevel.levelScore.ToString()+"/" + lvlpoints.ToString();
#endif
# if(CONTEST)
						lblTotalScore.Text = Level.totalScore.ToString()+"/"+iSumOfAllLevelPoints.ToString();
#endif
                        if (raiseInterpretrEndOfProgram != null)
                            raiseInterpretrEndOfProgram();
                        //TraceWidth = 0;
                        //lblLineTracing.Text = "";
                        //TraceX = 0;
                        //TraceWidth = 0;
                        ////By:Rajesh
                        ////Date:8/8/06
                        ////To resolve issue: 3514
                        ////if(traceOn)
                        //displayTrace();
                        ////Debug(herbtSpeed.ToString());
                        //DispMessage("End of program reached");

                        //// Fix issue of Herbert stopping at 45 degrees.


                        //EnableMenu();
                        //mnuGoHrbt.Enabled = false;
                        curLevel.blnGoState = false;
                        //mnuStepHrbt.Enabled = false;
                        curLevel.blnStepState = false;
                        //mnuResumeHrtb.Enabled = false;
                        curLevel.blnResumeState = false;
                        //mnuHaltHrbt.Enabled = false;
                        curLevel.blnHaltState = false;
                        //mnuResetHerbt.Enabled = true;
                        curLevel.blnResetState = true;
                        //btGoPause.Enabled = false;
                        //btStop.Enabled = true;
                        //btStep.Enabled = false;

                        //// commented by karthikeyan 25072005
                        ////						if(curLevel.IsLevelFinished)
                        ////							lblLevelSolved.Text = "(Solved!)";
                        ////						else
                        ////							lblLevelSolved.Text = "(Unsolved)";
                        //lock (txtCodeEditor)
                        //{
                        //    txtCodeEditor.Visible = true;
                        //}
                        //getImages();
                        //RotateImages(curLevel.CurDir);
                        //lock(arrHrbtImage[stateArray[0]])
                        {
#if (INVOKER)

							if(this.herbertPicBox.InvokeRequired)
							{
								try
								{
									if(herbtSpeed == 0)
//										lock(imgForPicBox)
//										{
											imgForPicBox = arrHrbtImage[stateArray[0]];
//										}
									else
										imgForPicBox = arrHrbtImage[stateArray[0]];
								}
								catch
								{}
								try
								{
									t.Abort();
								}
								catch
								{}
								this.herbertPicBox.Invoke(mi);
								/////Console.WriteLine("Invoking....");
							}
							else
							{
#endif
                            //this.herbertPicBox.Image = arrHrbtImage[stateArray[0]];
#if (INVOKER)
							}

							//this.herbertPicBox.Image = arrHrbtImage[stateArray[0]];

#endif
                        }
                        //herbertPicBox.Invalidate();
                        //txtCodeEditor.Focus();
                    }
                    else
                    {
                        //vijay
                        if (raiseKillThreadNoWait != null)
                            raiseKillThreadNoWait();
                        else//Added By Rajesh 10/04/07
                        {
                            lock (progClosing)
                            {
                                progClosing = true;
                            }
                        }
                        lock (curLevel.EndofProg)
                        {
                            curLevel.EndofProg = true;
                        }
                        //TraceWidth = 0;						
                        //curLevel[currentLevelId].chkLevelFinished(currentLevelId);
# if(CONTEST)
						if(currentLevelIndex > 0)
							lblScoreCurrent.Text = curLevel.levelScore.ToString()+"/" + Level.getLevelPoints(currentLevelIndex).ToString();	
						else
							lblScoreCurrent.Text = curLevel.levelScore.ToString()+"/" + currentLevelIndex.ToString();
#endif
#if(DESIGNER)
                        //if(currentLevelIndex > 0)
                        //    lblScoreCurrent.Text = curLevel.levelScore.ToString()+"/" + Level.getLevelPoints(currentLevelIndex).ToString();	
                        //else
                        //    lblScoreCurrent.Text = curLevel.levelScore.ToString()+"/" + lvlpoints.ToString();
#endif
# if(CONTEST)
						lblTotalScore.Text = Level.totalScore.ToString()+"/"+iSumOfAllLevelPoints.ToString();
#endif
                        if (raiseInterpretrEndOfProgram != null)
                            raiseInterpretrEndOfProgram();                      
                        //lblLineTracing.Text = "";
                        //TraceX = 0;
                        //TraceWidth = 0;
                        ////By:Rajesh
                        ////Date:8/8/06
                        ////To resolve issue: 3514
                        ////if(traceOn)
                        //displayTrace();
                        ////Debug(herbtSpeed.ToString());
                        //DispMessage("End of program reached");

                        //EnableMenu();
                        //mnuGoHrbt.Enabled = false;
                        curLevel.blnGoState = false;
                        //mnuStepHrbt.Enabled = false;
                        curLevel.blnStepState = false;
                        //mnuResumeHrtb.Enabled = false;
                        curLevel.blnResumeState = false;
                        //mnuHaltHrbt.Enabled = false;
                        curLevel.blnHaltState = false;
                        //mnuResetHerbt.Enabled = true;
                        curLevel.blnResetState = true;
                        //btGoPause.Enabled = false;
                        //btStep.Enabled = false;
                        //btStop.Enabled = true;
                        //if (curLevel.IsLevelFinishedPersistant)
                        //    lblLevelSolved.Text = "(Solved!)";
                        //else
                        //    lblLevelSolved.Text = "(Unsolved)";
                        //lock (txtCodeEditor)
                        //{
                        //    txtCodeEditor.Visible = true;
                        //}


                        //getImages();
                        //RotateImages(curLevel.CurDir);
                        //						lock(arrHrbtImage[stateArray[0]])
                        {
#if (INVOKER)

							if(this.herbertPicBox.InvokeRequired && herbtSpeed != 0)
							{
								try
								{
									if(herbtSpeed == 0)
									{
										Thread.Sleep(200);
										imgForPicBox = arrHrbtImage[stateArray[0]];
										Thread.Sleep(200);
									}
									else
										imgForPicBox = arrHrbtImage[stateArray[0]];
								}
								catch
								{
								}
								Thread.Sleep(200);
								try
								{
									t.Abort();
								}
								catch
								{}
								this.herbertPicBox.Invoke(mi);
								/////Console.WriteLine("Invoking....");
							}
							else
							{
#endif
                            //this.herbertPicBox.Image = arrHrbtImage[stateArray[0]];
#if (INVOKER)
							}

							//this.herbertPicBox.Image = arrHrbtImage[stateArray[0]];

#endif
                        }
                        //herbertPicBox.Invalidate();

                        //txtCodeEditor.Focus();
                    }
                }
            } /* end of for loop */

            // inbuilt function of MAC, set the current graphics port (basic or color) takes
            //parameter as pointer to structure

            //setport(&atWindows[PLAYWIN]);  // defined above

            //
            if (sTraceDisplayed > 0)
            { //sTraceDisplayed-> short data type defined in hvars.h
                //ToggleCurBox(); //functon defined in hint.h
                if ((sStop > 0) || ((sNewLine = (short)(curLevel.tHState.Flags & HFNWLIN))) > 0)
                {
                    curLevel.tHState.Flags &= ~HFNWLIN;
                    //ToggleLineBox();  // funtion defined in hint.h
                    //eraserect(&tCurRect); // graphics function of Mac.. erases rectangle
                    // tCurRect is rect type define in hvars.h
                }
            }
            //Added by Vivek Balagangadharan
            // Description : Added the condition for checking levelindex. Levelindex 0 was giving exception.
            // Added On : 01-Aug-2005
           // if (currentLevelIndex > 0) Comment by Rajesh 24/04/07
            {
                if (sNewLine > 0 || curLevel.atLineInfo[curLevel.tHState.curLine].cFunc == cOp)
                {
                    //reset Line tracing vairables.
                    if (traceOn)
                        if(raiseGetTextForLTFrame!=null)
                        raiseGetTextForLTFrame();
                    //??//curLevel.atLineInfo[curLevel.tHState.curLine].iStartIndexLT = curLevel.atLineInfo[curLevel.tHState.curLine].iEndIndexLT = iStartIndexLT = iEndIndexLT = 0;
                    //??//TraceX = 0;
                    //EditorTrace(tCurLine.iLineNum);
                }
            }
            //Probably change the line
            if (sTraceDisplayed != asOptions[2])
            {
                if (sTraceDisplayed > 0)
                {
                    if (!(sStop > 0 || sNewLine > 0))
                    {
                        int txtCENumLine = 0;
                        txtCENumLine = txtTotalLines;
                        for (int j = 1; j < txtCENumLine; j++)
                        {
                            if ((curLevel.atLineInfo[j].cFunc == cOp && curLevel.atLineInfo[j].sNumParms == 0) || cOp == 's' || cOp == 'r' || cOp == 'l' || cOp == '|')
                            {
                                TraceX = curLevel.tHState.PC + curLevel.tCurLine.iFirstCmd;
                                //TraceWidth = -3+charWidth(acCurrentLine,tHState.iPC  + tCurLine.iFirstCmd, (Math.Abs(iLastChar - iFirstChar)),ExecLbl.Font);
                                //TraceWidth = charWidth(acCurrentLine, 0, tHState.iPC  + tCurLine.iFirstCmd + (Math.Abs(iLastChar - iFirstChar)),ExecLbl.Font) - TraceX;
                                TraceWidth = 1;//Math.Abs(iLastChar - iFirstChar);
                                //By:Rajesh
                                //Date:8/8/06
                                //To resolve issue: 3514
                                //if(traceOn)
                                if(raiseDisplayTrace!=null)
                                raiseDisplayTrace(TraceX, TraceWidth);
                                curLevel.OneStepDone = false;
                            }
                        }
                        //ToggleLineBox();
                        //eraserect(&tCurRect);
                    }
                }
                else
                {
                    if (!((sStop > 0) || (sNewLine > 0)))
                    {
                        if (traceOn)
                        {
                            if (raiseEditorTrace != null)
                                raiseEditorTrace(curLevel.tCurLine.iLineNum);
                        }// function define in hint.h which take integer as parameter
                        //DrawCurrentLine();  // Did not get it in files
                    }
                }
            }
            for (int m = 0; m < curLevel.tCurLine.iLength - 1; m++)
            {
                if (m - 1 >= 0 && m + 1 < curLevel.tCurLine.iLength - 1)
                    if (curLevel.acCurrentLine[m - 1] == '(' && curLevel.acCurrentLine[m + 1] == ')' && curLevel.acCurrentLine[m] == '*')
                    {
                        raiseDispMessage("Unable to evaluate parameter");
                        raiseInternalError(-1);
                    }
            }
            if (cOp == '*')
            {
                if(raiseDispMessage!=null)
                raiseDispMessage("");
            }
            // check for the syntax which have been converted in either 's', 'r', 'l' or '|'
            //and process it accordingly

            iFirstChar = iLastChar = curLevel.tHState.PC;

            if (!(sStop > 0))
            {
#if(DESIGNER)
				//Rajesh
				//To calculate no. of program steps.
				//Date 3/5/06

				if(raiseUpdateNumProgramSteps!=null)
                raiseUpdateNumProgramSteps();
#endif
                //if(!progClosing)
                switch (cOp)
                {
                    case '|':
                        //vijay
                        if (raiseKillThreadNoWait != null)
                            raiseKillThreadNoWait();
                        //progClosing = true;
                        curLevel.tHState.CurrentOp = cOp;
                        //displayBox = true;		commented by karthikeyan 16/03 
                        if (raiseEnableMenu != null)
                            raiseEnableMenu();

                        if (raiseSetGoPauseState != null)
                            raiseSetGoPauseState();
                        //Event Handler
                        //mnuHaltHrbt.Enabled = false;
                        //this.hTooltip.SetToolTip(this.btGoPause, "Resume");
                        //btGoPause.Image = pbRun.Image;

                        curLevel.blnHaltState = false;
                        curLevel.goPauseState = 0;
                       


                        //hrbthalted = true; commented by karthikeyan 16/03
                        curLevel.tHState.PC++;
                        TraceX = curLevel.tHState.PC + curLevel.tCurLine.iFirstCmd;
                        TraceWidth = 1;
                        //By:Rajesh
                        //Date:8/8/06
                        //To resolve issue: 3514
                        //if(traceOn)
                        if (raiseDisplayTrace != null)
                            raiseDisplayTrace(TraceX, TraceWidth);
                        break;
                    case 's':
                    case 'r':
                    case 'l':

                        curLevel.tHState.CurrentOp = cOp;

                        //displayBox = true;				commented by karthikeyan 16/03

                        TraceX = curLevel.tHState.PC + curLevel.tCurLine.iFirstCmd;

                        TraceWidth = 1;
                        //By:Rajesh
                        //Date:8/8/06
                        //To resolve issue: 3514
                        //if(traceOn)
                        if (raiseDisplayTrace != null)
                            raiseDisplayTrace(TraceX, TraceWidth);
                        if ((cOp == 'r' || cOp == 'l'))
                        {
                            if(raiseHrbtRedrawn!=null)
                                raiseHrbtRedrawn(true);
                        }
                        //							StepCharRorL = cOp;
                        if (curLevel.stepByStep && (cOp == 'r' || cOp == 'l') && !curLevel.goClicked)
                        {
                            curLevel.StepCharRorL = cOp;
                            curLevel.blnRorLStep = true;
                        }
                        else
                        {
                            //blnRorLStep = false;
                            //Command = cOp.ToString().ToUpper();
                            if (raiseCommand != null)
                                raiseCommand(cOp.ToString().ToUpper());
                        }
                        curLevel.tHState.PC++;
                        curLevel.OneStepDone = false;

                        break;
                    // to throw the errors depend on the other or default characters(other than r, s, l)
                    default:
                        curLevel.tHState.CurrentOp = 'i';                        
                        if(raiseEditorTrace!=null)
                        raiseEditorTrace(curLevel.tHState.curLine);
                    TraceX = curLevel.tHState.PC + curLevel.tCurLine.iFirstCmd;

                    TraceWidth = 1;
                    //By:Rajesh
                    //Date:8/8/06
                    //To resolve issue: 3514
                    //if(traceOn)
                    if (raiseDisplayTrace != null)
                        raiseDisplayTrace(TraceX, TraceWidth);
                        if (Char.IsLower(cOp))
                        {
                            //	displayBox = true; commented by karthikeyan 16/03

                            for (iLine = 1; iLine < curLevel.iNumLines && curLevel.atLineInfo[iLine].cFunc != cOp; iLine++) ;

                            if (iLine >= curLevel.iNumLines)
                            {
                                blnSystemError = true;
                                if (raiseTxtCodeEditorVisible != null)
                                    raiseTxtCodeEditorVisible(true);//txtCodeEditor.Visible = true;                                
                                int iY = getActualLineNum(curLevel.tHState.curLine);
                                int charno = 0;
                                charno = curLevel.tHState.PC + txtLines[iY].IndexOf(":") + 2;
                                charno += getActualCharNum(iY);
                                if (raiseDispMessage != null)
                                    raiseDispMessage("No function definition found for '" + cOp.ToString() + "'");
                                if (raiseSelectError != null)
                                    raiseSelectError(charno + iY - 1, 1);
                                if (raiseInternalError != null)
                                    raiseInternalError(-1);
                                //InternalError(26);
                            }
                            curLevel.iSP = PushFrame(1 + curLevel.atLineInfo[iLine].sNumParms);
#if(DESIGNER)                 
                            if(raiseUpdateMaxStackLength!=null)
                            raiseUpdateMaxStackLength(curLevel.iSP);
#endif
                            iLastChar++;
                            sAllPos = TRUE;
                            if (curLevel.atLineInfo[iLine].sNumParms > 0)
                            {
                                iLastChar++;
                                for (i = 0; i < curLevel.atLineInfo[iLine].sNumParms; i++)
                                {
                                    curLevel.aiStack[(curLevel.iSP + 1 + i) % STACKSIZE] = EvalArg(ref sAllPos, ref iLastChar);
                                    iLastChar++;
                                }

                                //displayBox = true;				commented by karthikeyan 16/03
                                //TraceX = charWidth(acCurrentLine,0,tHState.iPC  + tCurLine.iFirstCmd,ExecLbl.Font);
                                TraceX = curLevel.tHState.PC + curLevel.tCurLine.iFirstCmd;
                                //TraceWidth = -3+charWidth(acCurrentLine,tHState.iPC  + tCurLine.iFirstCmd, (Math.Abs(iLastChar - iFirstChar)),ExecLbl.Font);
                                //TraceWidth = charWidth(acCurrentLine, 0, tHState.iPC  + tCurLine.iFirstCmd + (Math.Abs(iLastChar - iFirstChar)),ExecLbl.Font) - TraceX;
                                TraceWidth = Math.Abs(iLastChar - iFirstChar);
                                //								TraceX = 0;
                                //								for(int i2 = 0; i2 <= (tHState.iPC  + tCurLine.iFirstCmd); i2++)
                                //									TraceX += charWidth(acCurrentLine[i2],ExecLbl.Font);
                                //
                                //By:Rajesh
                                //Date:8/8/06
                                //To resolve issue: 3514
                                //if(traceOn)
                                if (raiseDisplayTrace != null)
                                    raiseDisplayTrace(TraceX, TraceWidth);
                                //checking if program is closed or end of program is reached
                                //then thread should not sleep.
                                //if(!progClosing && !EndofProg)
                                if (!(bool)curLevel.EndofProg && herbtSpeed != 0)
                                    Thread.Sleep(herbtSpeed);
                                curLevel.OneStepDone = false;
                            }



                            curLevel.aiStack[curLevel.iSP] = (curLevel.atLineInfo[iLine].sNumParms << 12)
                                | (curLevel.tHState.curLine << 7) | (iLastChar & 0x007f);
                            if ((sAllPos > 0))
                            {
                                curLevel.tHState.curLine = iLine;
                                curLevel.tHState.PC = 0;
                                curLevel.tHState.Flags |= HFNWLIN;
                            }
                            else
                            {
                                curLevel.iSP = PopFrame();
                                curLevel.tHState.PC = iLastChar;
                            }
                            iLastChar--;


                        }

                            // evaluates the character case. isupper returns true if character is Upper Case
                        else if (Char.IsUpper(cOp))
                        {

                            if (((iVal = GetVal(curLevel.tCurLine.iLineNum, cOp, curLevel.iSP)) & 0xf000) > 0)
                            {
                                blnSystemError = true;
                                if (raiseTxtCodeEditorVisible != null)
                                    raiseTxtCodeEditorVisible(true);
                                //txtCodeEditor.Visible = true;
                                int iY = getActualLineNum(curLevel.tHState.curLine);
                                int charno = 0;
                                charno = curLevel.tHState.PC + txtLines[iY].IndexOf(":") + 2;
                                charno += getActualCharNum(iY);
                                if (raiseDispMessage != null)
                                    raiseDispMessage("Recursive parameter definition");
                                if (raiseSelectError != null)
                                    raiseSelectError(charno + iY - 1, 1);
                                if (raiseInternalError != null)
                                    raiseInternalError(-1);
                                //InternalError(28);
                            }
                            iArgFrame = CurrentArgFrame(curLevel.iSP);
                            iArgFrame = (int)(iArgFrame + 1 + (((uint)curLevel.aiStack[iArgFrame] & 0xf000) >> 12)) % STACKSIZE;
                            //bc1280 start added 2005-03-30
                            if (curLevel.sStackTrashed && curLevel.iTrashedSP > curLevel.iSP && curLevel.iTrashedSP < iArgFrame)
                            {
                                if (raiseDispMessage != null)
                                    raiseDispMessage("Program too complex (stack overflow)");
                                if (raiseInternalError != null)
                                    raiseInternalError(-1);
                            }
                            //bc1280 end
                            //								//bc 2005-03-30
                            //								if (curLevel.sStackTrashed && iTrashedSP > iSP && iTrashedSP < iArgFrame) 
                            //								{
                            //									DispMessage("Program too complex (stack overflow)");
                            //									InternalError(-1);
                            //								}
                            curLevel.iSP = PushFrame(2);
                            curLevel.aiStack[(curLevel.iSP + 1) % STACKSIZE] = iArgFrame | 0x4000;
                            curLevel.aiStack[curLevel.iSP] = 0x1000 | (curLevel.tHState.curLine << 7) | ((curLevel.tHState.PC + 1) & 0x007f);
                            curLevel.tHState.curLine = (iVal >> 7) & 0x000f;
                            curLevel.tHState.PC = iVal & 0x007f;
                            curLevel.tHState.Flags |= HFNWLIN;
                        }
                        else
                        {
                            //for proper error messages.
                            int iActualLineNum = getActualLineNum(curLevel.tHState.curLine);
                            if (raiseDispMessage != null)
                                raiseDispMessage("Invalid character");
                            if (raiseSelectError != null)
                                raiseSelectError(getActualCharNum(iActualLineNum) + iActualLineNum, txtLines[iActualLineNum].Length);
                            if (raiseInternalError != null)
                                raiseInternalError(-1);
                            //InternalError(29);
                        }
                        break;
                }

                //				ExecLbl.Text = new string(acCurrentLine);
                //				displayBox = true;				
                //				TraceX = iFirstChar * 7 + tCurLine.iFirstCmd*7;
                //				TraceWidth = (Math.Abs(iLastChar - iFirstChar) +1)*7;
                //				//ExecLbl.Text = new string(acCurrentLine);



                // draw line depends on the option choosen
                if ((sTraceDisplayed = asOptions[1]) > 0)
                {
                    if (sNewLine > 0)
                    {
                        if (traceOn)
                        {
                            if (raiseEditorTrace != null)
                                raiseEditorTrace(curLevel.tCurLine.iLineNum);
                        }
                        //reset Line tracing vairables.
                        //iStartIndexLT = iEndIndexLT = 0;
                        //DrawCurrentLine();
                    }
                    //DrawCurBox(iFirstChar,iLastChar);
                }
                if (sNewLine > 0)
                {
                    //EditorTrace(tCurLine.iLineNum);
                    //reset Line tracing vairables.
                    //iStartIndexLT = iEndIndexLT = iFrameLT = 0;
                    //TraceX = 0;
                    //DrawCurrentLine();
                }
            }

                // If character is not interpreted correctly, It halts Herbie and set rect position
            else
            {
                //HaltHerbie(FALSE);
                curLevel.tHState.RunState = 'e';
                //EnableRunMenu(0x0010);			/*10000*/
                //setrect(&tCurBox,0,0,0,0);
                ///setrect(&tLineBox,0,0,0,0);
                //acCurrentLine[0]=0;
                //sTraceDisplayed=FALSE;
            }

            /*if(movechars == true)
                    Command = cOp.ToString().ToUpper();*/
            //Inbuilt function of MAC. Sets the size, pattern, and pattern mode of the graphics pen in the current graphics port
            //to their initial values.
            //pennormal();
            //hunlock((**ahtTERecs[PLAYWIN]).htext);
            //			}
        }
        /// <summary>
        /// evaluates the arguments and replaces it with it's values.
        /// </summary>
        /// <param name="psAllPos"></param>
        /// <param name="piPC"></param>
        /// <returns>returns the value of argument</returns>
        private int EvalArg(ref short psAllPos, ref int piPC)
        {
            int iTemp;
            char c;

            c = curLevel.acCurrentLine[curLevel.tCurLine.iFirstCmd + piPC];
            if (c == ',' || c == ')')
                return ((curLevel.tCurLine.iLineNum << 7) + (piPC & 0x007f));
            else if (c == '-' || c == '0' || c == '*')
            {
                CSkipToArgEnd(ref piPC);
                psAllPos = FALSE;
                return (0);
            }
            else if (Char.IsDigit(c))
            {
                iTemp = c - '0';
                for (c = curLevel.acCurrentLine[curLevel.tCurLine.iFirstCmd + (++(piPC))];
                    Char.IsDigit(c);
                    c = curLevel.acCurrentLine[curLevel.tCurLine.iFirstCmd + (++(piPC))])
                    iTemp = 10 * iTemp + (c - '0');
                return (-iTemp);
            }
            else
            {
                iTemp = (curLevel.tCurLine.iLineNum << 7) + (piPC & 0x007f);
                CSkipToArgEnd(ref piPC);
                return (iTemp);
            }
        }
        /// <summary>
        /// inserts the frame in stack.
        /// </summary>
        /// <param name="iSize">size of the frame.</param>
        /// <returns>new stack pointer.</returns>
        private int PushFrame(int iSize)
        {
            int iNewSP;
            /* i need to check for trashing of currentargframe */

            if ((iNewSP = curLevel.iSP - iSize) < 0)
            {
                iNewSP += STACKSIZE;
                if (curLevel.iTrashedSP > iNewSP || curLevel.iTrashedSP <= curLevel.iSP)
                {
                    curLevel.iTrashedSP = iNewSP;
                    curLevel.sStackTrashed = true;
                    curLevel.stackInitialisationCounter++;
                    /*if(curLevel.stackInitialisationCounter > 1)
                    {
                        DispMessage("Program too complex (stack overflow)");
                        InternalError(-1);
                        curLevel.stackInitialisationCounter = 0;
                    }*/
                    //curLevel.sStackInitialised = !(curLevel.sStackInitialised) ; 
                }
            }
            else
            {
                if (curLevel.iTrashedSP > iNewSP || curLevel.iTrashedSP <= curLevel.iSP)
                {
                    curLevel.iTrashedSP = iNewSP;
                }
            }
            return (iNewSP);
        }
        /// <summary>
        /// This is an infinite loop that interprets and executes herbert.
        /// This runs as a thread.
        /// </summary>

        public void Interpret()
        {
#if(SPOOL) 
			//Console.WriteLine(" ");
			//Console.WriteLine("@@@@@@@@@");
			//Console.WriteLine("Interpret() started " + Thread.CurrentThread.Name);
#endif
            lock (m_blnThreadStarted)
            {
                m_blnThreadStarted = true;
            }
            lock (progClosing)
            {
                if ((bool)progClosing)
                    Thread.Sleep(500);
                else
                    Thread.Sleep(200);
            }
            //			MessageBox.Show(this, "m_blnThreadStarted:" + m_blnThreadStarted.ToString(), "vinay");
            //			MessageBox.Show(this, "progClosing:" + curLevel.progClosing.ToString(), "vinay");
            //			MessageBox.Show(this, "bMonitorLChange:" + bMonitorLChange.ToString(), "vinay");
            //			MessageBox.Show(this, "bSemaphore:" + bSemaphore.ToString(), "vinay");
            lock (progClosing)
            {
                if ((bool)progClosing)
                {
                    //MessageBox.Show(this, "Returning", "vinay");
                    return;
                }
            }
            //check for end of program and progClosing.
            try
            {
                
                //check for end of program and progClosing.
#if(SPOOL) 
				//Console.WriteLine("Interpret() loop started " + Thread.CurrentThread.Name);
#endif
                while (true)
                {
                    int iWait = (int)(herbtSpeed * 2) + 200;

                    #region Changes by Ani
                    // Check for Level change notification
                    if (Monitor.TryEnter(bMonitorLChange, iWait))
                    {
                        #region Monitoring Level change variable
                        if ((bool)bMonitorLChange)
                        {
                            //Level change initiated so return
                            //m_blnThreadStarted = false;
                            try
                            {
                                Monitor.Exit(bMonitorLChange); // it is hear means level change was initiated
                            }
                            catch { }
                            break;
                        }
                        // Acquire lock on semaphore while still holding a lock on the bMonitorLChange
                        // as this would avoid it interference from the Level change thread.
                        if (Monitor.TryEnter(bSemaphore, iWait))
                        {
                            #region Semaphore & other
                            // Interpretation takes place
                            if (!((bool)(bSemaphore)))
                            {
                                Monitor.Exit(bSemaphore);
                                try
                                {
                                    Monitor.Exit(bMonitorLChange); // by Pavan to solve issue 3606, as the object bMonitorLChange is never released if it breaks here
                                }
                                catch { }
                                break;
                            }
                            if (blnChangeLevel)
                            {
                                blnChangeLevel = false;
                                //m_blnThreadStarted = false;
                                try
                                {
                                    Monitor.Exit(bSemaphore);
                                }
                                catch
                                {
                                }
                                //								try{Thread.CurrentThread.Abort();}
                                //								catch{}
                                try
                                {
                                    Monitor.Exit(bMonitorLChange); // by Pavan to solve issue 3606, as the object bMonitorLChange is never released if it breaks here
                                }
                                catch { }
                                break;
                            }
                            bSemaphore = true;
                            try
                            {
                                Monitor.Exit(bMonitorLChange);
                            }
                            catch
                            {
                            }
                            #endregion

                            #region Inside Semaphore Lock
                            try
                            {
                                #region Program Closing as well as End of Program
                                lock (progClosing)
                                {
                                    if ((bool)progClosing)
                                    {
                                        //m_blnThreadStarted = false;
                                        try
                                        {
                                            Monitor.Exit(bSemaphore);
                                        }
                                        catch
                                        {
                                        }
                                        //										try
                                        //										{
                                        //											Thread.CurrentThread.Abort();
                                        //										}
                                        //										catch{}
                                        try
                                        {
                                            Monitor.Exit(bMonitorLChange); // by Pavan to solve issue 3606, as the object bMonitorLChange is never released if it breaks here
                                        }
                                        catch { }
                                        break;
                                    }
                                }
                                lock (curLevel.EndofProg)
                                {
                                    if ((bool)curLevel.EndofProg)
                                    {
                                        //m_blnThreadStarted = false;
                                        try
                                        {
                                            Monitor.Exit(bSemaphore);
                                        }
                                        catch
                                        {
                                        }
                                        //										try
                                        //										{
                                        //											Thread.CurrentThread.Abort();
                                        //										}
                                        //										catch{}
                                        try
                                        {
                                            Monitor.Exit(bMonitorLChange); // by Pavan to solve issue 3606, as the object bMonitorLChange is never released if it breaks here
                                        }
                                        catch { }
                                        //Added By Rajesh 11/04/07
                                        if (raiseDispMessage != null)
                                        {
                                            raiseDispMessage("End of program reached");
                                        }
                                        break;
                                    }
                                }
                                #endregion
                                #region Of MoveHerbie
                                if (raiseMoveHerbie == null || raiseMoveHerbie())
                                {
                                    #region r1
                                    if (blnChangeLevel)
                                    {
                                        blnChangeLevel = false;
                                        //m_blnThreadStarted = false;
                                        try
                                        {
                                            Monitor.Exit(bSemaphore);
                                        }
                                        catch
                                        {
                                        }
                                        //										try{Thread.CurrentThread.Abort();}
                                        //										catch{}
                                        // The following Message would still pop up even though
                                        // it appears after current thread abort
                                        //MessageBox.Show(this,"Change Level After Move Herbie");
                                        try
                                        {
                                            Monitor.Exit(bMonitorLChange); // by Pavan to solve issue 3606, as the object bMonitorLChange is never released if it breaks here
                                        }
                                        catch { }
                                        break;
                                    }
                                    #endregion
                                    #region r2
                                    try
                                    {
                                        // -- Introduced by aniruddha to prevent other threads from manipulating Herbert's Level Data
                                        //Event Handler...
                                        if(raiseInterpretHerbertPositionUpdate!=null)
                                        raiseInterpretHerbertPositionUpdate();                                        
                                    }
                                    catch { }
                                    #endregion
                                    Interpreter();
                                    if (blnSystemError)
                                    {
                                        //m_blnThreadStarted = false;
                                        try
                                        {
                                            Monitor.Exit(bSemaphore);
                                        }
                                        catch
                                        {
                                        }
                                        try
                                        {
                                            Monitor.Exit(bMonitorLChange); // by Pavan to solve issue 3606, as the object bMonitorLChange is never released if it breaks here
                                        }
                                        catch { }
                                        break;
                                    }
                                }
                                else
                                {
                                    if (blnLineTraceStarted || !traceOn)
                                        Thread.Sleep(herbtSpeed);
                                }
                                #endregion
                                // Let go of the Semaphore for small time so that the Level CHange functionality
                                // can acquire the lock in case of changing the level
                                try
                                {
                                    Monitor.Exit(bSemaphore);
                                }
                                catch
                                {
                                }
                            }
                            catch
                            {
                                // Needs to let go even on error
                                try
                                {
                                    Monitor.Exit(bSemaphore);
                                }
                                catch
                                {
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            // This means LevelScroll has lock on the semaphore.. 
                            //m_blnThreadStarted = false;
                            try
                            {
                                Monitor.Exit(bMonitorLChange);
                            }
                            catch
                            {
                            }
                            break;
                        }
                        #endregion
                    }
                    else
                    {
                        // Lock couldn't be acquired 
                        // Signifies levelchange is in progress
                        //m_blnThreadStarted = false;
                        break;
                    }
                    #endregion

                }// end while	
                //Monitor.PulseAll(bSemaphore);
#if(SPOOL) 
				//Console.WriteLine("Interpret() loop ended " + Thread.CurrentThread.Name);
#endif
                //RestoreResources();
                if (blnSystemError)
                {
                    //HaltHrbt_Click(null,null);
                    //Event Handler..
                    //DisableMenu();
                    //btStop.Enabled = true;
                    //mnuResetHerbt.Enabled = true;
                    //curLevel.blnResetState = true;
                }
            }
            finally
            {
                lock (m_blnThreadStarted)
                {
                    m_blnThreadStarted = false;
                }
                //RestoreResources();
#if(SPOOL) 
				//Console.WriteLine("Interpret() finally " + Thread.CurrentThread.Name);
#endif
            }
#if(SPOOL) 
			//Console.WriteLine("Interpret() ended " + Thread.CurrentThread.Name);
			//Console.WriteLine("@@@@@@@@@");
			//Console.WriteLine(" ");
#endif
        }
        /// <summary>
        /// calculates the value of parameter in function defination.
        /// </summary>
        /// <param name="iLine">line number of function defination</param>
        /// <param name="piS">start point of the function.</param>
        /// <param name="piD">end point of the fucntion.</param>
        /// <param name="cSep1">char to be ignored</param>
        /// <param name="cSep2">char to be ignored</param>
        private void TransferNumber(int iLine, ref int piS, ref int piD, char cSep1, char cSep2)
        {
            //			if(!progClosing)
            //			{
            char cBinOp, c;
            int iResult, iTemp;
            short sValid;

            cBinOp = '+';
            iResult = 0;
            /* true if code doesnot contains any errors */
            sValid = TRUE;
            /* if code is valid AND next char is not one of cSep1 AND cSep2 and current pointer is not more than program length */
            for (c = curLevel.pcProg[piS]; (sValid > 0) && c != cSep1 && c != cSep2 && piS < curLevel.iProgLen; c = curLevel.pcProg[piS])
            {
                if (c == '-' || c == '+')
                {
                    cBinOp = c; /* Binary Operator type */
                    (piS)++;
                }
                /*if argument is a number */
                else if (Char.IsDigit(c))
                {
                    /* c-'0' is integer value of char in c */
                    for (iTemp = c - '0', (piS)++; Char.IsDigit(curLevel.pcProg[piS]); (piS)++)
                        /* if number is > 9 get all digits */
                        iTemp = 10 * iTemp + (curLevel.pcProg[piS] - '0');
                    /* if + than make number positive otherwise negative */
                    iResult += ((cBinOp == '+') ? iTemp : -iTemp);
                    //bc /* added brian 2005-03-30 */
                    if (iResult > MAXNUM || iResult < MINNUM)
                    {
                        //begin added by piyush on 31 Mar 2005
                        if (raiseDispMessage != null)
                            raiseDispMessage("Number " + iResult + " is out of range of valid values.");
                        sValid = FALSE;
                        if (raiseInternalError != null)
                            raiseInternalError(-1);
                        //end added by piyush on 31 Mar 2005
                    }
                    /*char[] chariResult = iResult.ToString().ToCharArray();
                        for(int i = 0; i < chariResult.Length; i++)
                            acCurrentLine[piD++] = chariResult[i];*/
                }
                /* if argument is a lower case letter than set error */
                else if (Char.IsLower(c))
                    sValid = FALSE;
                /* if Argument is a upper char.*/
                else if (Char.IsUpper(c))
                {
                    if ((iTemp = GetVal(iLine, c, curLevel.iSP)) > 0)
                        sValid = FALSE;
                    else
                    {
                        //iResult = 0;
                        iResult += ((cBinOp == '+') ? -iTemp : iTemp);
                        (piS)++;
                        //bc /* added brian 2005-03-30 */
                        if (iResult > MAXNUM || iResult < MINNUM)
                        {
                            //begin added by piyush on 31 Mar 2005
                            if (raiseDispMessage != null)
                                raiseDispMessage("Number " + iResult + " is out of range of valid values.");
                            sValid = FALSE;
                            if (raiseInternalError != null)
                                raiseInternalError(-1);
                            //end added by piyush on 31 Mar 2005
                        }
                    }
                }
                else
                    sValid = FALSE;
                /*if(piS == iProgLen)
                    {
                        piS--;
                        break;
                    }*/
            }
            if ((sValid > 0))
            {
                char[] chariResult = iResult.ToString().ToCharArray();
                for (int i = 0; i < chariResult.Length; i++)
                    curLevel.acCurrentLine[piD++] = chariResult[i];
                //piD+=sprintf(&acCurrentLine[piD],"%d",iResult); //sprintf always returns 0! */
                //sprintf(&acCurrentLine[piD],"%d",iResult);
                //while (acCurrentLine[++(piD)].CompareTo('\0') == 0) /* navigate to end of line*/
                ;
            }
            else
            {
                curLevel.acCurrentLine[(piD)++] = '*'; /* add error char at the end of execution line.*/
                PSkipToArgEnd(ref piS); /* skips to char ')'*/
            }
            //}
        }
        /// <summary>
        /// replaces the fucntion call by function defination in currently
        /// executing line.
        /// </summary>
        /// <param name="iLine">ine number of function defination</param>
        /// <param name="piS">start point of the function</param>
        /// <param name="piD">end point of the fucntion</param>
        /// <param name="cSep1">char to be ignored</param>
        /// <param name="cSep2">char to be ignored</param>
        private void TransferArg(int iLine, ref int piS, ref int piD, char cSep1, char cSep2)
        {
            //			if(!progClosing)
            //			{
            char c = ' ';
            if (piS < curLevel.iProgLen)
                c = curLevel.pcProg[piS];
            /* if char is one of the argument that we should ignore than return*/
            if (c == cSep1 || c == cSep2 || piS >= curLevel.iProgLen)
                return;
            /* if char is - or + than transfer that number ignoring the cSep1, cSep2 chars */
            if (c == '-' || c == '+' || Char.IsDigit(c))
                TransferNumber(iLine, ref piS, ref piD, cSep1, cSep2);
            /* function call in another function eg. a(A):sf(A) */
            else if (Char.IsLower(c) || c == '|')
                TransferFunc(iLine, ref piS, ref piD, cSep1, cSep2);
            /* if uppercase letter than replace it with value*/
            else if (Char.IsUpper(c))
                if (GetVal(iLine, c, curLevel.iSP) < 0)
                    TransferNumber(iLine, ref piS, ref piD, cSep1, cSep2);
                else
                    TransferFunc(iLine, ref piS, ref piD, cSep1, cSep2);
            else
            {
                curLevel.acCurrentLine[(piD)++] = '*';
                PSkipToArgEnd(ref piS); /* if code is not valid move and a * is inserted to the execution line then skip to end of arg list */
            }
            //}
        }
        /// <summary>
        /// return the value of parameter.
        /// </summary>
        /// <param name="iLine">currently executing line</param>
        /// <param name="cParm">parameter whose value to be calculated.</param>
        /// <param name="iValFrame"></param>
        /// <returns></returns>
        private int GetVal(int iLine, char cParm, int iValFrame)
        {
            int i, iLineStart;

            iLineStart = curLevel.atLineInfo[iLine].iStart;
            /* increment is by 2 because we are skipping ',' char*/
            for (i = 2; Char.IsUpper(curLevel.pcProg[iLineStart + i]) && curLevel.pcProg[iLineStart + i] != cParm; i += 2)
                if (i == curLevel.iProgLen - 1)
                    break;
            /* if lower case parameter */
            if (!Char.IsUpper(curLevel.pcProg[iLineStart + i]))
            {
                //Event Handler ...
                //if (isInternalErrorLogged)
                //{
                //    sbErrorLog.Append("\n Internal Error 25 raised in GetVal(" + iLine.ToString() + "," + cParm.ToString() + "," + iValFrame.ToString() + ")");
                //}
                if (raiseInternalError != null)
                    raiseInternalError(25);
            }
            /*return the arguments from stack*/
            return (curLevel.aiStack[(CurrentArgFrame(iValFrame) + (i >> 1)) % STACKSIZE]); /* i >> 1 ==== devide i by 2*/
        }

        /// <summary>
        /// replaces the fucntion call by function defination in currently
        /// executing line.
        /// </summary>
        /// <param name="iLine">ine number of function defination</param>
        /// <param name="piS">start point of the function</param>
        /// <param name="piD">end point of the fucntion</param>
        /// <param name="cSep1">char to be ignored</param>
        /// <param name="cSep2">char to be ignored</param>
        private void TransferFunc(int iLine, ref int piS, ref int piD, char cSep1, char cSep2)
        {
            //			if(!progClosing)
            //			{
            short sValid;
            int iOrigD;
            char c;

            /* true if code doesnot contains any errors */
            sValid = TRUE;
            iOrigD = piD;
            /* if code is valid AND next char is not one of cSep1 AND cSep2 and current pointer is not more than program length */
            for (c = curLevel.pcProg[piS]; (sValid > 0) && c != cSep1 && c != cSep2 && piS < curLevel.iProgLen; c = curLevel.pcProg[piS])
            {

                if (Char.IsLower(c) || c == '|')
                {
                    curLevel.acCurrentLine[(piD)++] = curLevel.pcProg[(piS)++];
                }
                else if (Char.IsUpper(c))
                {/* Upper case means it's a parameter */
                    if (GetVal(iLine, c, curLevel.iSP) > 0)
                    { /* returns the parameter value */
                        curLevel.acCurrentLine[(piD)++] = c;
                        (piS)++;
                    }
                    else
                        sValid = FALSE; /* argument value is not valid */
                }
                else if (c == '(')
                { /* if opening braces */
                    curLevel.acCurrentLine[(piD)++] = '(';
                    (piS)++;
                    /* add value of all arguments to the execution line. */
                    do
                    {
                        if (piS < curLevel.iProgLen)
                        {
                            TransferArg(iLine, ref piS, ref piD, ',', ')');
                            curLevel.acCurrentLine[(piD)++] = curLevel.pcProg[(piS)++];
                        }
                    } while (curLevel.pcProg[(piS) - 1] != ')' && piS != curLevel.iProgLen); /* till closing braces */
                }
                else /* if not opening '(' set code is invalid */
                    sValid = FALSE;
                if (piS == curLevel.iProgLen)
                {
                    piS--;
                    break;
                }
            }
            if (!(sValid > 0))
            {
                curLevel.acCurrentLine[(piD) = iOrigD] = '*';
                (piD)++;
                PSkipToArgEnd(ref piS); /* if code is not valid move and a * is inserted to the execution line then skip to end of arg list */
            }
            //}
        }

        private void EvalCurLine(int iLine)/*assumes pcprog set up*/
        {
            //			if(!progClosing)
            //			{
            int iS, iD;

            iD = 0; /* temp program line number*/
            iS = curLevel.atLineInfo[iLine].iStart; /* this is the start point for this function in hole code. */
            curLevel.tCurLine.iFirstCmd = 0;
            TransferFunc(iLine, ref iS, ref iD, ':', CR); // transfer function by it's defination. CR is constant and it's value is '\n'*/
            if (curLevel.pcProg[iS] == ':' && iS < curLevel.iProgLen)
            { /* see if the next program char is ':' and current counter is not more than program lenght*/
                curLevel.acCurrentLine[iD++] = ':'; /* add current char to execution line. */
                curLevel.tCurLine.iFirstCmd = iD;   /* set the next execution point */
                iS++;					/* increase the program counter */
                TransferFunc(iLine, ref iS, ref iD, CR, CR); /* call this function with skip able char both as '\n' */
            }
            curLevel.acCurrentLine[iD] = '\0';
            curLevel.tCurLine.iLineNum = iLine;
            curLevel.tCurLine.iLength = iD + 1;
            //EditorTrace(tCurLine.iLineNum);
            if (curLevel.tHState.curLine == 0)
            {
                curLevel.OneStepDone = false;
                //displayBox = true;		commented by karthikeyan 16/03
                //TraceX = charWidth(acCurrentLine,0,tHState.iPC  + tCurLine.iFirstCmd,ExecLbl.Font);
                TraceX = curLevel.tHState.PC + curLevel.tCurLine.iFirstCmd;
                //				TraceX = 0;
                //				for(int i = 0; i <= (tHState.iPC  + tCurLine.iFirstCmd); i++)
                //					TraceX += charWidth(acCurrentLine[i],ExecLbl.Font);
                //TraceWidth = charWidth(acCurrentLine,0,tHState.iPC  + tCurLine.iFirstCmd + 1,ExecLbl.Font) - TraceX;
                //TraceWidth = 1;
                //TraceWidth = -3+charWidth(acCurrentLine[tHState.iPC  + tCurLine.iFirstCmd],ExecLbl.Font);// (Math.Abs(iLastChar - iFirstChar) +1)*7;
                //iStartIndexLT = iEndIndexLT = iFrameLT = 0;
                //TraceX = 0;
                int txtCENumLine = 0;
                //				lock(txtCodeEditor)
                //				{
                txtCENumLine = txtTotalLines;
                //				}
                for (int j = 1; j < txtCENumLine; j++)
                {
                    if ((curLevel.atLineInfo[j].cFunc == curLevel.pcProg[TraceX] && curLevel.atLineInfo[j].sNumParms == 0))
                    {
                        //TraceX =  tHState.iPC  + tCurLine.iFirstCmd;
                        //TraceWidth = -3+charWidth(acCurrentLine,tHState.iPC  + tCurLine.iFirstCmd, (Math.Abs(iLastChar - iFirstChar)),ExecLbl.Font);							
                        //TraceWidth = charWidth(acCurrentLine, 0, tHState.iPC  + tCurLine.iFirstCmd + (Math.Abs(iLastChar - iFirstChar)),ExecLbl.Font) - TraceX;							
                        TraceWidth = 1;//Math.Abs(iLastChar - iFirstChar);
                        if (traceOn)
                        {
                            if (raiseGetTextForLTFrame != null)
                                raiseGetTextForLTFrame();
                            if (raiseDisplayTrace != null)
                                raiseDisplayTrace(TraceX, TraceWidth);
                        }
                        break;
                        //OneStepDone = false;
                    }
                }
                //displayTrace();
                if (raiseEvalCurLineMoveHerbert != null)
                raiseEvalCurLineMoveHerbert();
            }
            //}
        }        
    }
}