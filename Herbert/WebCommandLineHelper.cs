using System;
using System.Web;
//using System.Security;
using System.Diagnostics;
using System.Collections;
using System.Deployment.Application;
using System.Text;
using System.Windows.Forms;

namespace Designer
{
	internal class WebCommandLineHelper 
	{
		
		static internal bool Empty(string s) { return s == null || s.Length == 0; }

		static char separationChar = '?';
		protected static char SeparationCharacter 
		{
			get { return separationChar; }
			set { separationChar = value; }
		}

		static internal bool LaunchedFromUrl 
		{
			get 
			{
				try 
				{
					// Check if we have a site
                    //string url = (string)AppDomain.CurrentDomain.GetData("APPBASE");
                    //System.Security.Policy.Site.CreateFromUrl(url);
                    //added By Rajesh for Herbert Inetrpreter , required for clickonce..
                    ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
                    String str = ad.ActivationUri.ToString();
                    //MessageBox.Show("in Try:" , "Herbert - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return true;
				}
				catch( Exception exp) 
				{
                    //MessageBox.Show("in Catch:"+exp.Message, "Herbert - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
			}
		}

		static internal string GetLaunchUrlWithArgs() 
		{
			if( !LaunchedFromUrl ) throw new ApplicationException("Launch URL not available (not launched from URL)");

			// Only works for .NET 1.1+
			AppDomain domain = AppDomain.CurrentDomain;
			object obj = domain.GetData("APP_LAUNCH_URL");
			string appLaunchUrl = (obj != null ? obj.ToString() : "");
            //added By Rajesh required for clickonce.
            ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
            appLaunchUrl = ad.ActivationUri.ToString();            
			// Fall back for .NET 1.0
			if( Empty(appLaunchUrl) ) 
			{
				const string ext = ".config";
				string configFile = domain.SetupInformation.ConfigurationFile;
				Debug.Assert(configFile.ToLower().EndsWith(ext));
				appLaunchUrl = configFile.Substring(0, configFile.Length - ext.Length);
			}

			return appLaunchUrl;
		}

		static internal string GetLaunchUrlNoArgs() 
		{
			string url = GetLaunchUrlWithArgs();
			int delimiter = url.IndexOf(separationChar);
			if( delimiter < 0 ) return url;
			return url.Substring(0, delimiter);
		}

		static internal string GetLaunchUrlOnlyArgs() 
		{
			string url = GetLaunchUrlWithArgs();
			int delimiter = url.IndexOf(separationChar);
			if( delimiter < 0 ) return null;
			return url.Substring(delimiter + 1);
		}


		// Ripped this off from HttpUtility
		protected class UrlDecoder 
		{
			private int bufferSize;
			private int numChars;
			private char[] charBuffer;
			private int numBytes;
			private byte[] byteBuffer;
			private System.Text.Encoding encoding;

			internal UrlDecoder(int bufferSize, Encoding encoding) 
			{
				this.bufferSize = bufferSize;
				this.encoding = encoding;
				this.charBuffer = new char[bufferSize];
			}

			private void FlushBytes () 
			{
				if( this.numBytes > 0 ) 
				{
					this.numChars += this.encoding.GetChars(this.byteBuffer, 0, this.numBytes, this.charBuffer, this.numChars);
					this.numBytes = 0;
				}
			}

			internal void AddChar(char ch) 
			{
				int i;
				this.FlushBytes();
				this.numChars = (i = this.numChars)  +  1;
				this.charBuffer[i] = ch;
			}

			internal void AddByte(byte  b) 
			{
				if( this.byteBuffer == null ) this.byteBuffer = new Byte[this.bufferSize];
				this.byteBuffer[this.numBytes++] = b;
			}

			internal string GetString () 
			{
				this.FlushBytes ();
				if( this.numChars > 0) return new String (this.charBuffer, 0, this.numChars);
				return String.Empty;
			}
		}

		// Ripped this off from HttpUtility
		static protected int HexToInt(char h) 
		{
			if( (h  >=  '0') && (h  <=  '9') ) return h  -  '0';
			if( (h  >=  'a') && (h  <=  'f') ) return (h  -  'a')  +  10;
			if( (h  >=  'A') && (h  <=  'F') ) return (h  -  'A')  +  10;
			return -1;
		}

		// Ripped this off from HttpUtility
		static internal string UrlDecode(string s) 
		{
			int length = s.Length;
			UrlDecoder decoder = new UrlDecoder(length, System.Text.Encoding.UTF8);
			for( int currIndex = 0; currIndex != length; ++currIndex ) 
			{
				char currChar = s[currIndex];
				if( currChar == '+' ) currChar = ' ';
				else if( currChar == '%' && currIndex < length - 2 ) 
				{
					if( s[currIndex + 1] == 'u' && currIndex < length - 5 ) 
					{
						int digit1 = HexToInt(s[currIndex + 2]);
						int digit2 = HexToInt(s[currIndex + 3]);
						int digit3 = HexToInt(s[currIndex + 4]);
						int digit4 = HexToInt(s[currIndex + 5]);
						if( digit1 >= 0 && digit2 >= 0 && digit3 >= 0 && digit4 >= 0 ) 
						{
							currChar = (char)((ushort) digit1 << 12 | digit2 << 8 | digit3 << 4 | digit4);
							currIndex += 5;
							decoder.AddChar(currChar);
							continue;
						}
					}
					else 
					{
						int digit1 = HexToInt(s[currIndex + 1]);
						int digit2 = HexToInt(s[currIndex + 2]);
						if( digit1 >= 0 && digit2 >= 0 ) 
						{
							currIndex += 2;
							decoder.AddByte((byte)(digit1 << 4 | digit2));
							continue;
						}
					}
				}

				if( (currChar & 0xFF80) == 0 )
					decoder.AddByte((byte)currChar);
				else
					decoder.AddChar(currChar);
			}

			return decoder.GetString();
		}

		static internal string[] GetCommandLineArgs(string[] argsFromMain) 
		{

            
			if( !LaunchedFromUrl ) return argsFromMain;

			string s = GetLaunchUrlOnlyArgs();
			if( Empty(s) ) return new string[0];

			ArrayList argList = new ArrayList();

			// Ripped this off from HttpValueCollection to parse query
			// string manually, as we may not have permissions to use
			// HttpRequest to parse the query string for us
			int length = s.Length;
			int currIndex = 0;
			while( currIndex < length ) 
			{
				int argIndex = currIndex;
				int equalsIndex = -1;
				while( currIndex < length ) 
				{
					if( s[currIndex] == '=' ) 
					{
						if( equalsIndex < 0 ) equalsIndex = currIndex;
					}
					else if( s[currIndex] == '&' ) break;
					currIndex++;
				}

				string key = null;
				string value = null;
				if( equalsIndex >= 0 ) 
				{
					key = s.Substring(argIndex, equalsIndex - argIndex);
					value = s.Substring(equalsIndex + 1, currIndex - equalsIndex - 1);
				}
				else 
				{
					value = s.Substring(argIndex, currIndex - argIndex);
				}

				if( !Empty(value) ) 
				{
					if( !Empty(key) ) argList.Add(UrlDecode(key) + "=" + UrlDecode(value));
					else argList.Add(UrlDecode(value));
				}

				currIndex++;
			}

			return (string[])argList.ToArray(typeof(string));
		}

	}

	public class ConfigFileHandler: IHttpHandler 
	{
		// Redirect calls to "foo.exe?foo.config"
		// to "foo.exe.config"
		public void ProcessRequest(HttpContext context) 
		{
			//      using( FileStream file = new FileStream(@"c:\temp\configHandlerLog.txt", FileMode.Append, FileAccess.Write) ) {
			//        using( StreamWriter writer = new StreamWriter(file) ) {
			//          writer.WriteLine("ProcessRequest: Request.PhysicalPath= " + context.Request.PhysicalPath);
			//          writer.WriteLine("ProcessRequest: Request.Url= " + context.Request.Url);
			//        }
			//      }

			// Just the .exe part in the file system
			string path = context.Request.PhysicalPath;

			// The entire request URL, include args and .config
			string url = context.Request.RawUrl;

			// If someone's asking for a .config, strip the arguments
			string ext = ".config";
			if( url.ToLower().EndsWith(ext) ) 
			{
				context.Response.WriteFile(path + ext);
			}
				// If someone's asking for the .exe, send it
			else 
			{
				context.Response.ContentType = "application/octet-stream";
				context.Response.WriteFile(path);
			}
		}

		public bool IsReusable 
		{
			get { return true; }
		}
	}
}
