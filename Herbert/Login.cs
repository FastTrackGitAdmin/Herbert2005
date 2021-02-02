//#define DLL
//For Herbert
#define CONTEST
#define OFFLINE_HEBERT
//end
//For Designer
//#define DESIGNER
//End


using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Globalization;
using System.Configuration;
using System.Xml;
using System.Net;


namespace Designer
{

#if(CONTEST || DESIGNER)
    /// <summary>
    /// Summary description for Login.
    /// </summary>
    public class Login : System.Windows.Forms.Form
    {
        private System.Windows.Forms.TextBox txtLoginId;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lbEmail;
        private System.Windows.Forms.Label lbPassword;
        private System.Windows.Forms.Label lbPasswordError;
        private System.Windows.Forms.Label lbLoginError;
        private System.Windows.Forms.Button btLogin;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox txtProxyId;
        private System.Windows.Forms.TextBox txtProxyPassword;
        private System.Windows.Forms.GroupBox gbNetwork;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDomain;
        private System.Windows.Forms.Button bttnCancelProxy;
        private System.Windows.Forms.Button bttnOK;
        private System.Windows.Forms.GroupBox gbLogin;

        #region User Variables
        private bool IsSelectedContest;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// stores the password of the user
        /// </summary>
        object oPassword = new object();

        /// <summary>
        /// stores the email of the user 
        /// </summary>
        object oEmail = new object();

        /// <summary>
        /// stores the contestid for herbert 
        /// </summary>
        object oContestId = new object();

        /// <summary>
        /// stores the version for herbert 
        /// </summary>
        object oVersion = new object();

        /// <summary>
        /// this is set for checking the instance override 
        /// </summary>
        object oMode = new object();
        /// <summary>
        /// Stores the OS version of the user's machine
        /// </summary>
        object oOSVersion = new object();

        /// <summary>
        /// Stores the .NET CLR Version on the user's machine
        /// </summary>
        object oCLRVersion = new object();

        /// <summary>
        /// Stores the IP Address of the user's machine
        /// </summary>
        object oIPAddress = new object();

        /// <summary>
        /// Stores the Culture Information on the user's machine
        /// </summary>
        object oCultureInfo = new object();

        /// <summary>
        /// Indicates whether application launched is Herbert or Designer
        /// </summary>
        bool isDesignerApp = new bool();

        /// <summary>
        /// path where RememberMe information will be stored
        /// </summary>
        string pathToRememberMeFile = Application.StartupPath + "\\RememberMe";

        object Version = new object();
        private System.Windows.Forms.GroupBox gbContest;
        private System.Windows.Forms.Label lbSelectContest;
        private System.Windows.Forms.ComboBox cmbContestList;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btRunGuest;
#if(DESIGNER)
		private System.Windows.Forms.Button btnLaunchDesigner;
#endif
#if(CONTEST)
        private System.Windows.Forms.Button btLaunchHerbert;
#endif
        private Label lblPort;
        public Label lblAddress;
        private TextBox txtPort;
        private TextBox txtAddress;
        private Label label5;
        private Label label8;
        private Label label6;
        private Label label7;
        private Button btPractice;
        private CheckBox chkRememberMe;
        DataSet dsContestsList = new DataSet();

        #endregion

        public Login()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtLoginId = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lbEmail = new System.Windows.Forms.Label();
            this.lbPassword = new System.Windows.Forms.Label();
            this.btLogin = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.lbLoginError = new System.Windows.Forms.Label();
            this.lbPasswordError = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtProxyId = new System.Windows.Forms.TextBox();
            this.txtProxyPassword = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.gbNetwork = new System.Windows.Forms.GroupBox();
            this.bttnCancelProxy = new System.Windows.Forms.Button();
            this.bttnOK = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDomain = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.gbLogin = new System.Windows.Forms.GroupBox();
            this.chkRememberMe = new System.Windows.Forms.CheckBox();
            this.btPractice = new System.Windows.Forms.Button();
            this.gbContest = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btRunGuest = new System.Windows.Forms.Button();
            this.lbSelectContest = new System.Windows.Forms.Label();
            this.cmbContestList = new System.Windows.Forms.ComboBox();
#if(DESIGNER)
            this.btnLaunchDesigner = new System.Windows.Forms.Button();
#endif
#if(CONTEST)
            this.btLaunchHerbert = new System.Windows.Forms.Button();
#endif
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.gbNetwork.SuspendLayout();
            this.gbLogin.SuspendLayout();
            this.gbContest.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtLoginId
            // 
            this.txtLoginId.Location = new System.Drawing.Point(152, 44);
            this.txtLoginId.MaxLength = 500;
            this.txtLoginId.Name = "txtLoginId";
            this.txtLoginId.Size = new System.Drawing.Size(216, 20);
            this.txtLoginId.TabIndex = 0;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(152, 102);
            this.txtPassword.MaxLength = 100;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(216, 20);
            this.txtPassword.TabIndex = 1;
            // 
            // lbEmail
            // 
            this.lbEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbEmail.Location = new System.Drawing.Point(40, 44);
            this.lbEmail.Name = "lbEmail";
            this.lbEmail.Size = new System.Drawing.Size(100, 24);
            this.lbEmail.TabIndex = 2;
            this.lbEmail.Text = "E-MAIL:";
            this.lbEmail.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbPassword
            // 
            this.lbPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPassword.Location = new System.Drawing.Point(40, 102);
            this.lbPassword.Name = "lbPassword";
            this.lbPassword.Size = new System.Drawing.Size(100, 24);
            this.lbPassword.TabIndex = 3;
            this.lbPassword.Text = "PASSWORD:";
            this.lbPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btLogin
            // 
            this.btLogin.BackColor = System.Drawing.SystemColors.Control;
#if(CONTEST)
            this.btLogin.Location = new System.Drawing.Point(104, 188);
#endif
#if(DESIGNER)
            this.btLogin.Location = new System.Drawing.Point(120, 188);
#endif
            this.btLogin.Name = "btLogin";
            this.btLogin.Size = new System.Drawing.Size(75, 24);
            this.btLogin.TabIndex = 4;
            this.btLogin.Text = "Login";
            this.btLogin.UseVisualStyleBackColor = false;
            this.btLogin.Click += new System.EventHandler(this.btLogin_Click);
            // 
            // btPractice
            // 
            this.btPractice.Location = new System.Drawing.Point(272, 168);
            this.btPractice.Name = "btPractice";
            this.btPractice.Size = new System.Drawing.Size(75, 24);
            this.btPractice.TabIndex = 8;
            this.btPractice.Text = "Practice";
            this.btPractice.UseVisualStyleBackColor = false;
            this.btPractice.Click += new System.EventHandler(this.btPractice_Click);
            // 
            // btCancel
            // 
            this.btCancel.BackColor = System.Drawing.SystemColors.Control;
#if(CONTEST)
            this.btCancel.Location = new System.Drawing.Point(189, 188);
#endif
#if(DESIGNER)
            this.btCancel.Location = new System.Drawing.Point(240, 188);
#endif
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 24);
            this.btCancel.TabIndex = 5;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = false;
            this.btCancel.Click += new System.EventHandler(this.button2_Click);
            // 
            // lbLoginError
            // 
            this.lbLoginError.AutoSize = true;
            this.lbLoginError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLoginError.ForeColor = System.Drawing.Color.Red;
            this.lbLoginError.Location = new System.Drawing.Point(152, 74);
            this.lbLoginError.Name = "lbLoginError";
            this.lbLoginError.Size = new System.Drawing.Size(140, 13);
            this.lbLoginError.TabIndex = 6;
            this.lbLoginError.Text = "Please enter your Email";
            this.lbLoginError.Visible = false;
            // 
            // lbPasswordError
            // 
            this.lbPasswordError.AutoSize = true;
            this.lbPasswordError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPasswordError.ForeColor = System.Drawing.Color.Red;
            this.lbPasswordError.Location = new System.Drawing.Point(152, 132);
            this.lbPasswordError.Name = "lbPasswordError";
            this.lbPasswordError.Size = new System.Drawing.Size(164, 13);
            this.lbPasswordError.TabIndex = 7;
            this.lbPasswordError.Text = "Please enter your Password";
            this.lbPasswordError.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // txtProxyId
            // 
            this.txtProxyId.Location = new System.Drawing.Point(152, 25);
            this.txtProxyId.Name = "txtProxyId";
            this.txtProxyId.Size = new System.Drawing.Size(214, 20);
            this.txtProxyId.TabIndex = 9;
            // 
            // txtProxyPassword
            // 
            this.txtProxyPassword.Location = new System.Drawing.Point(152, 64);
            this.txtProxyPassword.Name = "txtProxyPassword";
            this.txtProxyPassword.PasswordChar = '*';
            this.txtProxyPassword.Size = new System.Drawing.Size(214, 20);
            this.txtProxyPassword.TabIndex = 10;
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPort.Location = new System.Drawing.Point(267, 163);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(38, 13);
            this.lblPort.TabIndex = 21;
            this.lblPort.Text = "Port:";
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddress.Location = new System.Drawing.Point(40, 163);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(97, 13);
            this.lblAddress.TabIndex = 20;
            this.lblAddress.Text = "Proxy Server:";
            this.lblAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(311, 160);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(55, 20);
            this.txtPort.TabIndex = 15;
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(152, 160);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(95, 20);
            this.txtAddress.TabIndex = 14;
            // 
            // gbNetwork
            // 
            this.gbNetwork.Controls.Add(this.bttnCancelProxy);
            this.gbNetwork.Controls.Add(this.bttnOK);
            this.gbNetwork.Controls.Add(this.label3);
            this.gbNetwork.Controls.Add(this.txtDomain);
            this.gbNetwork.Controls.Add(this.label2);
            this.gbNetwork.Controls.Add(this.label1);
            this.gbNetwork.Controls.Add(this.txtProxyPassword);
            this.gbNetwork.Controls.Add(this.txtProxyId);
            this.gbNetwork.Controls.Add(this.lblAddress);
            this.gbNetwork.Controls.Add(this.lblPort);
            this.gbNetwork.Controls.Add(this.txtPort);
            this.gbNetwork.Controls.Add(this.txtAddress);
            this.gbNetwork.Controls.Add(this.label5);
            this.gbNetwork.Controls.Add(this.label6);
            this.gbNetwork.Controls.Add(this.label7);
            this.gbNetwork.Controls.Add(this.label8);
            this.gbNetwork.Location = new System.Drawing.Point(8, 0);
            this.gbNetwork.Name = "gbNetwork";
            this.gbNetwork.Size = new System.Drawing.Size(416, 240);
            this.gbNetwork.TabIndex = 11;
            this.gbNetwork.TabStop = false;
            this.gbNetwork.Visible = false;
            // 
            // bttnCancelProxy
            // 
            this.bttnCancelProxy.Location = new System.Drawing.Point(256, 200);
            this.bttnCancelProxy.Name = "bttnCancelProxy";
            this.bttnCancelProxy.Size = new System.Drawing.Size(75, 23);
            this.bttnCancelProxy.TabIndex = 17;
            this.bttnCancelProxy.Text = "Cancel";
            this.bttnCancelProxy.Click += new System.EventHandler(this.bttnCancelProxy_Click);
            // 
            // bttnOK
            // 
            this.bttnOK.Location = new System.Drawing.Point(96, 200);
            this.bttnOK.Name = "bttnOK";
            this.bttnOK.Size = new System.Drawing.Size(75, 23);
            this.bttnOK.TabIndex = 16;
            this.bttnOK.Text = "Ok";
            this.bttnOK.Click += new System.EventHandler(this.bttnOK_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(37, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 14;
            this.label3.Text = "Domain:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDomain
            // 
            this.txtDomain.Location = new System.Drawing.Point(152, 113);
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(214, 20);
            this.txtDomain.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(40, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 12;
            this.label2.Text = "Password: ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(32, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 23);
            this.label1.TabIndex = 11;
            this.label1.Text = "Proxy UserId: ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(372, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(15, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "*";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(372, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "*";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(253, 163);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(15, 13);
            this.label7.TabIndex = 26;
            this.label7.Text = "*";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(372, 113);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(15, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "*";
            // 
            // chkRememberMe
            // 
            this.chkRememberMe.AutoSize = true;
            this.chkRememberMe.Location = new System.Drawing.Point(152, 155);
            this.chkRememberMe.Name = "chkRememberMe";
            this.chkRememberMe.Size = new System.Drawing.Size(95, 17);
            this.chkRememberMe.TabIndex = 9;
            this.chkRememberMe.Text = "Remember Me";
            this.chkRememberMe.UseVisualStyleBackColor = true;
            // 
            // btPractice
            // 
            this.btPractice.Location = new System.Drawing.Point(272, 188);
            this.btPractice.Name = "btPractice";
            this.btPractice.Size = new System.Drawing.Size(75, 24);
            this.btPractice.TabIndex = 8;
            this.btPractice.Text = "Practice";
            this.btPractice.UseVisualStyleBackColor = false;
            this.btPractice.Click += new System.EventHandler(this.btPractice_Click);
            // 
            // gbContest
            // 
            this.gbContest.Controls.Add(this.label4);
            this.gbContest.Controls.Add(this.btRunGuest);
            this.gbContest.Controls.Add(this.lbSelectContest);
            this.gbContest.Controls.Add(this.cmbContestList);

            // 
            // gbLogin
            // 
            this.gbLogin.Controls.Add(this.chkRememberMe);
            this.gbLogin.Controls.Add(this.lbPasswordError);
            this.gbLogin.Controls.Add(this.lbLoginError);
            this.gbLogin.Controls.Add(this.btCancel);
            this.gbLogin.Controls.Add(this.btLogin);
#if(CONTEST)
            this.gbLogin.Controls.Add(this.btPractice);
#endif
            this.gbLogin.Controls.Add(this.lbPassword);
            this.gbLogin.Controls.Add(this.lbEmail);
            this.gbLogin.Controls.Add(this.txtPassword);
            this.gbLogin.Controls.Add(this.txtLoginId);
            this.gbLogin.Location = new System.Drawing.Point(8, 0);
            this.gbLogin.Name = "gbLogin";
            this.gbLogin.Size = new System.Drawing.Size(416, 240);
            this.gbLogin.TabIndex = 12;
            this.gbLogin.TabStop = false;
            // 
            // gbContest
            // 
            this.gbContest.Controls.Add(this.label4);
            this.gbContest.Controls.Add(this.btRunGuest);
            this.gbContest.Controls.Add(this.lbSelectContest);
            this.gbContest.Controls.Add(this.cmbContestList);
#if(DESIGNER)
            this.gbContest.Controls.Add(this.btnLaunchDesigner);
#endif
#if(CONTEST)
            this.gbContest.Controls.Add(this.btLaunchHerbert);
#endif
            this.gbContest.Location = new System.Drawing.Point(8, 0);
            this.gbContest.Name = "gbContest";
            this.gbContest.Size = new System.Drawing.Size(416, 240);
            this.gbContest.TabIndex = 18;
            this.gbContest.TabStop = false;
            this.gbContest.Text = "Select Contest";
            this.gbContest.Visible = false;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(264, 168);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 16);
            this.label4.TabIndex = 4;
            this.label4.Text = "(work not saved)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btRunGuest
            // 
            this.btRunGuest.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRunGuest.Location = new System.Drawing.Point(264, 136);
            this.btRunGuest.Name = "btRunGuest";
            this.btRunGuest.Size = new System.Drawing.Size(100, 25);
            this.btRunGuest.TabIndex = 3;
            this.btRunGuest.Text = "Run as guest";
            this.btRunGuest.Click += new System.EventHandler(this.btRunGuest_Click);
            // 
            // lbSelectContest
            // 
            this.lbSelectContest.AllowDrop = true;
            this.lbSelectContest.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSelectContest.Location = new System.Drawing.Point(40, 64);
            this.lbSelectContest.Name = "lbSelectContest";
            this.lbSelectContest.Size = new System.Drawing.Size(100, 23);
            this.lbSelectContest.TabIndex = 1;
            this.lbSelectContest.Text = "Select Contest";
            // 
            // cmbContestList
            // 
            this.cmbContestList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbContestList.Location = new System.Drawing.Point(144, 64);
            this.cmbContestList.MaxLength = 500;
            this.cmbContestList.Name = "cmbContestList";
            this.cmbContestList.Size = new System.Drawing.Size(240, 21);
            this.cmbContestList.TabIndex = 0;
            this.cmbContestList.SelectedIndexChanged += new System.EventHandler(this.cmbContestList_SelectedIndexChanged);
#if(CONTEST)
            // 
            // btLaunchHerbert
            // 
            this.btLaunchHerbert.Location = new System.Drawing.Point(144, 136);
            this.btLaunchHerbert.Name = "btLaunchHerbert";
            this.btLaunchHerbert.Size = new System.Drawing.Size(100, 25);
            this.btLaunchHerbert.TabIndex = 6;
            this.btLaunchHerbert.Text = "Launch Herbert";
            this.btLaunchHerbert.Click += new System.EventHandler(this.btLaunchHerbert_Click);
#endif
            // 
            // btnLaunchDesigner
            // 
#if(DESIGNER)
            this.btnLaunchDesigner.Location = new System.Drawing.Point(144, 136);
            this.btnLaunchDesigner.Name = "btnLaunchDesigner";
            this.btnLaunchDesigner.Size = new System.Drawing.Size(100, 25);
            this.btnLaunchDesigner.TabIndex = 5;
            this.btnLaunchDesigner.Text = "Launch Designer";
            this.btnLaunchDesigner.Click += new System.EventHandler(this.btnLaunchDesigner_Click);
#endif
            
            // 
            // Login
            // 
            this.AcceptButton = this.btLogin;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(432, 253);
            this.Controls.Add(this.gbContest);
            this.Controls.Add(this.gbLogin);
            this.Controls.Add(this.gbNetwork);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
#if(CONTEST)
            this.Text = "Herbert - Login";
#endif
#if(DESIGNER)
            this.Text = "Designer - Login";
#endif
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.gbNetwork.ResumeLayout(false);
            this.gbNetwork.PerformLayout();
            this.gbLogin.ResumeLayout(false);
            this.gbLogin.PerformLayout();
            this.gbContest.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
        

        private void Login_Load(object sender, System.EventArgs e)
        {
            string sHostname = "";

#if(DEBUG)
            //txtLoginId.Text = "rajesh.chandratre@fasttrackteam.com";
            //txtPassword.Text = "dinaNath";
#endif
            //#if(DESIGNER)
            //			GlobalData.HerbertMode=HMode.Designer;
            //			return;
            //#endif

            #region remember me
            // -- added by NikhilK on 26/10/2007 for Remember Me functionality --
            try
            {
                XmlTextReader xmReader = new XmlTextReader(pathToRememberMeFile);
                xmReader.Read();
                while (xmReader.Read())
                {
                    if (xmReader.LocalName == "LoginId")
                    {
                        txtLoginId.Text = xmReader.ReadElementContentAsString();
                        if (txtLoginId.Text == "")
                        {
                            GlobalData.isLoadFromRememberMeFile = false;
                            txtLoginId.Focus();
                            chkRememberMe.Checked = false;
                        }
                        else
                        {
                            GlobalData.isLoadFromRememberMeFile = true;
                            txtPassword.Focus();
                            chkRememberMe.Checked = true;
                        }
                        break;
                    }
                }
                xmReader.Close();
            }
            catch (Exception ee)
            {
                txtLoginId.Text = "";
            }
            // -- End of Remember Me functionality --
            #endregion

            if (GlobalData.GUID.Trim() == "")
            {
#if(CONTEST)
                GlobalData.HerbertMode = HMode.Contest;
                return;
#endif
#if(DESIGNER)
				GlobalData.HerbertMode = HMode.Designer;
				return;
#endif
            }
            if (GlobalData.GUID.Trim() == "0")
            {
                GlobalData.HerbertMode = HMode.Tutorial;
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
            else
            {
                try
                {
                    // checking the validitity for GUID string 
                    if (GlobalData.HerbertMode != HMode.Tutorial)
                        System.Data.SqlTypes.SqlGuid.Parse(GlobalData.GUID);
#if(DESIGNER)
                    if (GlobalData.iLevelId.Trim() != "-1")
                    {
                        //Added By Rajesh 10/04/07
                        //to Launch the Designer for the admin inetrface to see the Level
                        GlobalData.HerbertMode = HMode.Designer;
                        this.DialogResult = DialogResult.Yes;
                        this.Close();
                        return;
                    }
                    else if (GlobalData.iPatternId.Trim() != "-1")
                    {
                        //Added By Rajesh 10/04/07
                        //to Launch the Designer for the admin inetrface to see the Level
                        GlobalData.HerbertMode = HMode.Designer;
                        this.DialogResult = DialogResult.Yes;
                        this.Close();
                        return;
                    }
#endif

                    //checking the validitity for GUID from database
                    GlobalData.initlizeWS();
                    //MessageBox.Show(this, "GUID::" + GlobalData.GUID, "Login error", MessageBoxButtons.OK, MessageBoxIcon.Warning);						
                    Object oGUID = HerbertMain.EnableMyMenu(GlobalData.GUID);

                    // added by Nikhil Kardale on 13/11/2007
                    GlobalData.isHerbertFromSite = true;
                    #region retrieve user's machine's info
                    // --- retrieving information and encrypting it ---

                    // OS Version
                    oOSVersion = HerbertMain.EnableMyMenu(System.Environment.OSVersion.ToString());

                    // Culture Info
                    // added by Nikhil Kardale to fix issue 9910 - incorrect record of culture info in DB
                    oCultureInfo = HerbertMain.EnableMyMenu(GlobalData.startingCulture.ToString());

                    // CLR Version
                    oCLRVersion = HerbertMain.EnableMyMenu(System.Environment.Version.ToString());

                    // IP Address
                    sHostname = Dns.GetHostName();
                    oIPAddress = HerbertMain.EnableMyMenu(Dns.GetHostEntry(sHostname).AddressList[0].ToString());

                    // whether application launched is Herbert or Designer
#if(CONTEST)
                    isDesignerApp = false;
#endif
#if(DESIGNER)
                    isDesignerApp = true;
#endif
                    // --- end of retrieving data and encryption ---
                    #endregion

#if(DLL)

					if(GlobalData.DS.IsValidGUID(oGUID) == 1) 
					{
						// don't show login form directly Launch Herbert 
						GlobalData.HerbertMode = HMode.Contest;
						this.DialogResult = DialogResult.Yes;
						this.Close();
					}
#else
#if(DESIGNER)

                    if (GlobalData.HS.IsValidDGUID(oGUID, oOSVersion, oCultureInfo, oCLRVersion, oIPAddress, GlobalData.isHerbertFromSite, GlobalData.isBehindProxy, isDesignerApp) == 1) 
#endif
#if(CONTEST)
                    //int isvalid = GlobalData.HS.IsValidGUID(oGUID);

                    if (GlobalData.HS.IsValidGUID(oGUID, oOSVersion, oCultureInfo, oCLRVersion, oIPAddress, GlobalData.isHerbertFromSite, GlobalData.isBehindProxy, isDesignerApp) == 1)
#endif
                    {
                        // don't show login form directly Launch Herbert 
#if(DESIGNER)
						GlobalData.HerbertMode = HMode.Designer;
#endif
#if(CONTEST)
                        GlobalData.HerbertMode = HMode.Contest;
#endif
                        this.DialogResult = DialogResult.Yes;
                        this.Close();
                    }
#endif
                    else // invalid GUID
                    {
                        GlobalData.HerbertMode = HMode.Contest;
                        gbNetwork.Visible = false;
                        gbLogin.Visible = true;
                        // show the login  form. 
                        return;
                    }
                }
                catch (Exception exp) // this means that the GUID is invalid
                {
                    if (exp.Message.IndexOf("HTTP status 407") >= 0 || exp.Message.IndexOf("Unauthorized") >= 0)
                    {
                        try
                        {
                            System.Net.WebProxy wProxy;
                            wProxy = System.Net.WebProxy.GetDefaultProxy();
                            gbNetwork.Text = "Proxy: " + wProxy.Address.Host;
                            txtAddress.Text = wProxy.Address.Host;
                            txtPort.Text = wProxy.Address.Port.ToString();

                        }
                        catch
                        {
                            txtAddress.Text = "";
                            txtPort.Text = "";
                            MessageBox.Show(this, "H0093: Herbert exe does not have enough permissions. \nTo grant this permission please see the help section.", "Login error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        gbNetwork.Visible = true;
                        gbLogin.Visible = false;
                        //MessageBox.Show(this, "It appears that you are behind a proxy.\nPlease enter your proxy credentials.", "Login error", MessageBoxButtons.OK,MessageBoxIcon.Warning);						
                        if (txtProxyId.Text == "" && txtProxyPassword.Text == "" && txtDomain.Text == "")
                        {
                            MessageBox.Show(this, "H0089: It appears that you are behind a proxy.\nPlease enter your proxy credentials.", "Login error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            GlobalData.isBehindProxy = true;
                        }
                        else
                            MessageBox.Show(this, "H0092: Please check your proxy credentials.", "Login error", MessageBoxButtons.OK, MessageBoxIcon.Warning);


                        //wProxy = new System.Net.WebProxy(GlobalData.URL);


                        GlobalData.authReg = true;
                    }
#if(PROBE)
                    //ProbeId: 004
                    MessageBox.Show(this, "ProbeID: 004" + "\n Msg: " + exp.Message + "\n Trace: " + exp.StackTrace + "\n Inner Msg: " + exp.InnerException, "Herbert - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
                    GlobalData.HerbertMode = HMode.Contest;
                    // show the login  form. 
                    return;
                }
            }



#if(DLL)
			this.ControlBox = false;
#endif
        }


        private void btLogin_Click(object sender, System.EventArgs e)
        {
            string sHostname = "";
            
            GlobalData.loginName = txtLoginId.Text;
            GlobalData.password = txtPassword.Text;

            #region remember me
            // -- added by NikhilK on 26/10/2007 for Remember Me functionality --
            if (!GlobalData.isSwitchContestPerformed)
            {
                if (!GlobalData.isContestFinished)
                {
                    if (chkRememberMe.Checked)
                    {
                        WriteToRememberMeFile();
                    }
                    else if (!chkRememberMe.Checked && GlobalData.isLoadFromRememberMeFile == true)
                    {
                        try
                        {
                            File.Delete(pathToRememberMeFile);
                            GlobalData.isLoadFromRememberMeFile = false;
                        }
                        catch (Exception eee)
                        {
                            // do nothing
                        }
                    }
                    else
                    {
                        // do nothing
                    }
                }
            }
            // -- End of Remember Me functionality --
            #endregion

            if (txtLoginId.Text.Trim() == "")
            {
                lbLoginError.Text = "Please enter your Email";
                lbLoginError.Visible = true;
                txtLoginId.Focus();
                return;
            }
            else
            {
                if (Regex.IsMatch(txtLoginId.Text, @"^([a-zA-Z0-9_\-\.\+]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$") == false)
                {
                    lbLoginError.Text = "Please enter a valid Email";
                    lbLoginError.Visible = true;
                    txtLoginId.Focus();
                    return;
                }

                lbLoginError.Visible = false;
            }

            if (txtPassword.Text.Trim() == "")
            {
                lbPasswordError.Visible = true;
                txtPassword.Focus();
                return;
            }
            else
            {
                lbPasswordError.Visible = false;
            }

            #region retrieving user's machine's info
            // --- retrieving information and encrypting it ---
            
            //object oPassword = new object();
            oPassword = HerbertMain.EnableMyMenu(Encrypt(txtPassword.Text));
            String str = oPassword.ToString();
            oEmail = HerbertMain.EnableMyMenu(txtLoginId.Text);
            //int 
            oContestId = HerbertMain.EnableMyMenu(GlobalData.iContestId);
            oMode = HerbertMain.EnableMyMenu("0");
            Version = HerbertMain.EnableMyMenu(Application.ProductVersion);

            // --- added by NikhilK on 1/11/2007 ---
            
            // OS Version
            oOSVersion = HerbertMain.EnableMyMenu(System.Environment.OSVersion.ToString());

            // Culture Info
            // added by Nikhil Kardale to fix issue 9910 - incorrect record of culture info in DB
            oCultureInfo = HerbertMain.EnableMyMenu(GlobalData.startingCulture.ToString());

            // CLR Version
            oCLRVersion = HerbertMain.EnableMyMenu(System.Environment.Version.ToString());

            // IP Address
            sHostname = Dns.GetHostName();
            oIPAddress = HerbertMain.EnableMyMenu(Dns.GetHostEntry(sHostname).AddressList[0].ToString());

            // whether application launched is Herbert or Designer
#if(CONTEST)
            isDesignerApp = false;
#endif
#if(DESIGNER)
            isDesignerApp = true;
#endif
            // --- end of retrieving data and encryption ---
            #endregion

#if(DLL)		
			GlobalData.initlizeWS();
#else
            GlobalData.initlizeWS();
#endif
#if(OFFLINE_HEBERT)
            if (!GlobalData.isHerbertFromSite && !GlobalData.IsSessionSaved)
            {
                Designer.XMLEncryptor xm = new XMLEncryptor(GlobalData.loginName, GlobalData.password);
                string strPath = IsUserSessionPresent(xm);
                if (strPath != string.Empty && File.Exists(strPath))
                {
                    GlobalData.strSessionFilePath = strPath;
                    GlobalData.IsLoadFromFile = true;
                    GlobalData.HerbertMode = HMode.Tutorial;
                    this.DialogResult = DialogResult.Yes;
                    this.Close();
                    GlobalData.HerbertMode = HMode.Contest;
                    return;
                }
            }
#endif

            int RetryCount = 0;
            //added By Rajesh 24/01/07. to retry 3 times.
            bool blnLoginSucces = true;
            while (blnLoginSucces)
            {
                try
                {
#if(DLL)
				dsContestsList =  GlobalData.DS.GetUserContestList( oEmail, oPassword );
#else
#if(DESIGNER)				
                    dsContestsList = GlobalData.HS.DGetUserContestList(oEmail, oPassword, Version, oOSVersion, oCultureInfo, oCLRVersion, oIPAddress, GlobalData.isHerbertFromSite, GlobalData.isBehindProxy, isDesignerApp);
#else
                    dsContestsList = GlobalData.HS.GetUserContestList(oEmail, oPassword, Version, oOSVersion, oCultureInfo, oCLRVersion, oIPAddress, GlobalData.isHerbertFromSite, GlobalData.isBehindProxy, isDesignerApp);
                    
#endif

#endif
                    break;
                }
                catch (Exception exp)
                {                    
                    if (exp.Message.ToLower().IndexOf("invalid user") >= 0)
                    {
                        MessageBox.Show(this, "H0090: Authentication failed.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        gbNetwork.Visible = false;
                        gbLogin.Visible = true;
                        txtPassword.Text = "";
                        txtPassword.Focus();
                        this.AcceptButton = this.btLogin;
                        blnLoginSucces = false;//24/01/07
                        return;
                    }
                    else
                        if (exp.Message.IndexOf("HTTP status 407") >= 0 || exp.Message.IndexOf("Unauthorized") >= 0 || exp.Message.IndexOf("HTTP status 403") >= 0)
                        {
                            System.Net.WebProxy wProxy;
                            /*Start: Added By rajesh 15/11 to show credential window*/
                            try
                            {
                                wProxy = System.Net.WebProxy.GetDefaultProxy();
                                if (wProxy != null)
                                    gbNetwork.Text = "Proxy: " + wProxy.Address.Host;
                                txtAddress.Text = wProxy.Address.Host;
                                txtPort.Text = wProxy.Address.Port.ToString();
                            }
                            catch (Exception innerExp)
                            {
                                if (exp.Message.IndexOf("Request for the permission") >= 0)
                                {
                                    MessageBox.Show(this, "H0093: Herbert exe does not have enough permissions. \nTo grant this permission please see the help section.", "Login error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    blnLoginSucces = false;//24/01/07
                                    return;
                                }
                                else
                                {

                                }
                            }
                            gbNetwork.Visible = true;
                            this.AcceptButton = this.bttnOK;
                            gbLogin.Visible = false;
                            //MessageBox.Show(this, "It appears that you are behind a proxy.\nPlease enter your proxy credentials.", "Login error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            //blnLoginSucces = false;
                            //blnProxyAuthReq = true;

                            //wProxy=new System.Net.WebProxy(GlobalData.URL);//added By Rajesh


                            GlobalData.authReg = true;
#if(PROBE)
                            //string strCredential = "";
                            //    if (wProxy.Credentials != null)
                            //    {
                            //        strCredential = wProxy.Credentials.ToString();
                            //    }
                            //        //ProbeId: 006


                            //    if(wProxy!=null)                       
                            //    MessageBox.Show(this, "H0089: It appears that you are behind a proxy.\nPlease enter your proxy credentials." + "Proxy: " + wProxy.Address.ToString() + "Proxy URI:" + wProxy.GetProxy(new System.Uri(GlobalData.URL)).ToString() + "Proxy Credentials:" + strCredential + "\nProbeID: 006" + "\nTimeStamp:" + DateTime.Now.ToUniversalTime() + "\n Msg: " + exp.Message + "\n Trace: " + exp.StackTrace + "\n Inner Msg: " + exp.InnerException , "Login error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
#endif
                            if (txtProxyId.Text == "" && txtProxyPassword.Text == "")
                            {
                                MessageBox.Show(this, "H0089: It appears that you are behind a proxy.\nPlease enter your proxy credentials.", "Login error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                GlobalData.isBehindProxy = true;
                            }
                            else
                                MessageBox.Show(this, "H0092: Please check your proxy credentials.", "Login error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            blnLoginSucces = false;//24/01/07
                            return;

                        }
                        //21-Dec-1006 Rajesh, For not supported version.
                        else if (exp.Message.IndexOf("HS1001") >= 0)
                        {
                            string message = exp.Message.Substring(exp.Message.IndexOf("HS1001"), exp.Message.IndexOf("HS1002") - exp.Message.IndexOf("HS1001"));
                            MessageBox.Show(this, message, "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            //blnLoginSucces = false;
                            blnLoginSucces = false;//24/01/07
                            return;
                        }
                        //End
                        else if (exp.Message.IndexOf("timed out") > 0)
                        {
                            RetryCount++;
                            if (RetryCount == 3)
                            {
                                MessageBox.Show(this, "H0085: Connection to the server timed out. Please try again later.", "Herbert - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;//24/01/07
                            }
                            //return;//24/01/07
                        }
                        else if (exp.Message.IndexOf("underlying connection was closed") > 0)
                        {
                            RetryCount++;

                            //MessageBox.Show(this,RetryCount.ToString());
                            if (RetryCount == 3)
                            {
                                MessageBox.Show(this, "H0086: Connection failed. Please check your network connection.", "Herbert - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;//24/01/07
                            }
                            //return;24/01/07
                        }
                        else if (exp.Message.IndexOf("Currently you are using older version of Herbert") > 0)
                        {
                            MessageBox.Show(this, "H0087: Currently you are using older version of Herbert.\nYou may not be able to access all features of Herbert. Please download latest version of Herbert.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            blnLoginSucces = false;//24/01/07
                            return;
                        }
                        else if (exp.Message.IndexOf("System.Net.WebPermission") > 0)
                        {
                            MessageBox.Show(this, "H0093: Herbert exe does not have enough permissions. \nTo grant this permission please see the help section.", "Login error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            blnLoginSucces = false;//24/01/07
                            return;
                        }
                        else
                        {
#if(PROBE)
                            //Probe:001
                            MessageBox.Show(this, "H0088: Connection to the server failed. Please try again later. " + "\nProbeID: 001" + "\n Msg: " + exp.Message + "\n Trace: " + exp.StackTrace + "\n Inner Msg: " + exp.InnerException, "Herbert - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
                            if (txtProxyId.Text == "" && txtProxyPassword.Text == "")
                            {
                                MessageBox.Show(this, "H0088: Connection to the server failed. Please try again later.", "Herbert - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {//added by rajesh 24/01/07 
                                MessageBox.Show(this, "H0094: Connection to the server failed. Please check your proxy server address.", "Login error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            blnLoginSucces = false;//24/01/07
                            return;
                        }
                }
            }
            
            //Reading from encrypted xml file - added by Nikhil Kardale on 30/7/2007
            //string strSessionPath = Application.StartupPath + "\\Session1.xml";

            //if (File.Exists(strSessionPath))
            //{
            //    try
            //    {
            //        GlobalData.dsAllHData = new DataSet();
            //        //GlobalData.dsAllHData.ReadXml(strSessionPath, XmlReadMode.ReadSchema);

            //        //Decryption code - added by Nikhil Kardale on 30/7/2007
            //        Designer.XMLEncryptor xmDecFile = new XMLEncryptor(GlobalData.loginName, GlobalData.password);
            //        GlobalData.dsAllHData = xmDecFile.ReadEncryptedXML("Session1.xml");
            //        GlobalData.GUID = GlobalData.dsAllHData.Tables[7].Rows[0][0].ToString();
            //        GlobalData.IsLoadFromFile = true;
            //    }
            //    catch (Exception exp)
            //    {
            //        throw (new Exception("Invalid Session"));
            //    }
            //}

            
#if(DESIGNER)
			if(dsContestsList.Tables[0].Rows.Count==0)
			{
				//MessageBox.Show(this,"Sorry! You are not registered to any contest with designer.\nTo register please visit http://www.wildnoodle.com.","Login",MessageBoxButtons.OK, MessageBoxIcon.Information );
				MessageBox.Show(this,"Sorry! You are not registered to any contest with designer.\nTo register please visit http://www.wildnoodle.com.", "Herbert - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
#endif

                gbLogin.Visible = false;
                gbNetwork.Visible = false;
                gbContest.Visible = true;
#if(CONTEST)
                this.AcceptButton = this.btLaunchHerbert;
#endif
#if(DESIGNER)
                this.AcceptButton = this.btnLaunchDesigner;
#endif
#if(!DESIGNER)
                int iTutorial = dsContestsList.Tables[0].Rows.Count;
                DataRow dr = dsContestsList.Tables[0].NewRow();
                dr["Title"] = "Tutorial";
                dr["contestid"] = "0";

                dsContestsList.Tables[0].Rows.Add(dr);
#endif

                cmbContestList.DataSource = dsContestsList.Tables[0].DefaultView;
                cmbContestList.DisplayMember = "Title";
                //cmbContestList.ValueMember = "ContestId";


                cmbContestList.Text = "Tutorial";
                // added by NikhilK on 15-11-2007 to fix issue id. 8690 (Run as guest enabled after 'Switch Contest')
                this.btRunGuest.Enabled = false;
                ///Added By Rajesh
                ///to stop guest user
#if(DESIGNER)
			this.btRunGuest.Visible=false;
			this.btRunGuest.Enabled =false;
			label4.Visible=false;
#endif
                ///end

            }

            #region remember me
            // added by Nikhil Kardale - method to write to the Remember Me file
            private void WriteToRememberMeFile()
            {
                if (txtLoginId.Text == "")
                {
                    return;
                }
                else
                {
                    try
                    {
                        if (File.Exists(pathToRememberMeFile))
                            File.Delete(pathToRememberMeFile);

                        XmlTextWriter xmWriter = new XmlTextWriter(pathToRememberMeFile, null);
                        xmWriter.WriteStartDocument();
                        xmWriter.WriteStartElement("RememberMe");
                        xmWriter.WriteStartElement("LoginId");
                        xmWriter.WriteString(txtLoginId.Text);
                        xmWriter.WriteEndElement();
                        xmWriter.WriteEndDocument();
                        xmWriter.Close();
                        File.SetAttributes(pathToRememberMeFile, FileAttributes.Hidden);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show(this, "Herbert could not save your credentials.", "Herbert - Error", MessageBoxButtons.OK);
                    }
                }
            }
            #endregion


        private void button2_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }

        private void bttnOK_Click(object sender, System.EventArgs e)
        {
            if (GlobalData.GUID != "" && GlobalData.GUID != "0")
            {
                GlobalData.ProxyDomain = txtDomain.Text.Trim();
                GlobalData.ProxyUserName = txtProxyId.Text.Trim();
                //Added By Rajesh 12/01/07
                GlobalData.ProxyUserPassword = txtProxyPassword.Text.Trim();
                GlobalData.ProxyPort = txtPort.Text;
                GlobalData.ProxyServer = txtAddress.Text;
                GlobalData.HerbertMode = HMode.Contest;
                this.AcceptButton = this.btLogin;
                // 'if' condition added by NikhilK to fix issue 9552
                if (GlobalData.IsLoadFromFile != true)
                {
                    Login_Load(null, null);
                    //end
                    //this.DialogResult = DialogResult.Yes;
                }
                else 
                {
                    this.Close();
                }
                //end
                
            }
            else
            {
                if (txtLoginId.Text != "" && txtPassword.Text != "")
                {//for offline herbert proxy detected in btLogin_click.
                    GlobalData.ProxyDomain = txtDomain.Text.Trim();
                    GlobalData.ProxyUserName = txtProxyId.Text.Trim();
                    GlobalData.ProxyUserPassword = txtProxyPassword.Text.Trim();
                    GlobalData.ProxyPort = txtPort.Text;
                    GlobalData.ProxyServer = txtAddress.Text;
                    btLogin_Click(null, null);
                }
                else
                {//For the offline herbert. proxy detected at login_Load
                    this.AcceptButton = this.btLogin;
                    gbNetwork.Visible = false;
                    gbLogin.Visible = true;

                }
            }
        }


        private void bttnCancelProxy_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }


        /// <summary>
        /// To encrypt plain text
        /// </summary>
        /// <param name="Val"></param>
        /// <returns></returns>
        public static string Encrypt(string Val)
        {
            string passPhrase = "wildnoodle"; // can be any string
            string saltValue = "fasttrack";  // can be any string
            string hashAlgorithm = "MD5"; // either "MD5"  or "SHA1"
            int passwordIterations = 5; // can be any number
            string initVector = "98@6D$9C%9*C#324"; // must be 16 bytes (Characters)
            int keySize = 192; // can be 192 or 128
            /*All the above values must be same in both encryption and decryption functions  */

            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);

            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(Val.ToString());

            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);

            byte[] keyBytes = password.GetBytes(keySize / 8);

            RijndaelManaged symmetricKey = new RijndaelManaged();

            symmetricKey.Mode = CipherMode.CBC;

            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

            cryptoStream.FlushFinalBlock();

            byte[] cipherTextBytes = memoryStream.ToArray();

            memoryStream.Close();

            cryptoStream.Close();

            string cipherText = Convert.ToBase64String(cipherTextBytes);

            return cipherText;
        }


        private void btLaunchHerbert_Click(object sender, System.EventArgs e)
        {
            Version = Application.ProductVersion;

            int contestid = 0;

            string contest = cmbContestList.Text;
            if (contest.ToLower() == "tutorial")
            {
                GlobalData.HerbertMode = HMode.Tutorial;
                this.DialogResult = DialogResult.Yes;
                return;
            }


            string cid;
            for (int i = 0; i < dsContestsList.Tables[0].Rows.Count; i++)
            {
                cid = dsContestsList.Tables[0].Rows[i]["Title"].ToString();
                if (cid.ToLower() == contest.ToLower())
                {
                    string ContestVersion = dsContestsList.Tables[0].Rows[i]["ContestVersion"].ToString();
                    if (ContestVersion != null || ContestVersion != "")
                    {
                        string ExeVersion = Version.ToString();
                        //if (CheckOldVersion(ContestVersion, ExeVersion))
                        //{
                        //    MessageBox.Show(this, "Currently you are using older version of Herbert for the selected contest.\nYou may not be able to access all features of Herbert. Please download latest version of Herbert", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //    return;
                        //}
                    }
                    contestid = int.Parse(dsContestsList.Tables[0].Rows[i]["contestid"].ToString());
                    break;
                }
            }

            GlobalData.iContestId = contestid;

            #region

            //oPassword = HerbertMain.EnableMyMenu(Encrypt(txtPassword.Text));
            String str = oPassword.ToString();
            //oEmail = HerbertMain.EnableMyMenu(txtLoginId.Text);
            //int 
            oContestId = HerbertMain.EnableMyMenu(GlobalData.iContestId);
            oMode = HerbertMain.EnableMyMenu("0");
            object oVersion = HerbertMain.EnableMyMenu(Version);

            DataSet dsAuthenticate = new DataSet();

            bool blnLoginSucces = true;
            bool blnProxyAuthReq = false;
            int RetryCount = 0;
            while (blnLoginSucces)
            {
                try
                {
#if(DLL)
					dsAuthenticate =  GlobalData.DS.Authenticate(oEmail,oPassword,oContestId,oMode);						
#else
                    //dsAuthenticate =  GlobalData.HS.Authenticate(oEmail,oPassword,oContestId,oMode);//,Version);
                    dsAuthenticate = GlobalData.HS.AuthenticateNew(oEmail, oPassword, oContestId, oMode, oVersion);
                    //start:Rajesh: To warn user to update exe.:30/10/06
                    if (dsAuthenticate.Tables.Count == 2)
                    {
                        if (dsAuthenticate.Tables[1].Rows[0][0].ToString().Equals("1"))
                        {
                            MessageBox.Show(this, "Currently you are using older version of Herbert for the selected contest.\nYou may not be able to access all features of Herbert. Please download latest version of Herbert.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    //end
#endif
                    break;
                }
                catch (Exception exp)
                {
                    if (exp.Message.IndexOf("HTTP status 407") >= 0 || exp.Message.IndexOf("Unauthorized") >= 0)
                    {
                        gbNetwork.Visible = true;
                        gbLogin.Visible = false;
                        MessageBox.Show(this, "It appears that you are behind a proxy.\nPlease enter your proxy credentials.", "Login error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        GlobalData.isBehindProxy = true;
                        blnLoginSucces = false;
                        blnProxyAuthReq = true;
                        System.Net.WebProxy wProxy;
                        wProxy = System.Net.WebProxy.GetDefaultProxy();
                        gbNetwork.Text = "Proxy: " + wProxy.Address.Host;
                        GlobalData.authReg = true;
                    }
                    else
                        if (exp.Message.IndexOf("HS1001") >= 0)
                        {
                            string message = exp.Message.Substring(exp.Message.IndexOf("HS1001"), exp.Message.IndexOf("HS1002") - exp.Message.IndexOf("HS1001"));
                            MessageBox.Show(this, message, "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            blnLoginSucces = false;
                        }
                        else if (exp.Message.IndexOf("Instance Loading") >= 0)
                        {

                            DialogResult dr = MessageBox.Show(this, "May be an instance of Herbert is loading. Do you want to override that instance?", "Login", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (dr == DialogResult.Yes)
                            {
                                oMode = HerbertMain.EnableMyMenu("1");
                                blnLoginSucces = true;
                            }
                            else
                            {
                                blnLoginSucces = false;
                                break;
                            }
                        }
                        else if (exp.Message.IndexOf("Another instance running") >= 0)
                        {
                            DialogResult dr = MessageBox.Show(this, "It appears you are already running Herbert. You can only run one instance of Herbert at a time. \nNote: You might receive this message after a computer crash. If that is the case you can ignore this warning. Work from your prior instance may be lost. \nDo you want to continue?", "Login", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (dr == DialogResult.Yes)
                            {
                                oMode = HerbertMain.EnableMyMenu("1");
                                blnLoginSucces = true;
                            }
                            else
                            {
                                blnLoginSucces = false;
                                break;
                            }
                        }
                        else if (exp.Message.IndexOf("timed-out") > 0)
                        {
                            RetryCount++;
                            blnLoginSucces = true;
                            //MessageBox.Show(this,RetryCount.ToString());
                            if (RetryCount == 3)
                            {
                                //MessageBox.Show(this,"H0082: Unable to load data. Connection to the server timed out. Please try again later.","Herbert - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                MessageBox.Show(this, "H0082: Connection to the server timed out. Please try again later.", "Herbert - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                blnLoginSucces = false;
                            }
                        }
                        else if (exp.Message.IndexOf("underlying connection was closed") > 0)
                        {
                            RetryCount++;
                            blnLoginSucces = true;
                            //MessageBox.Show(this,RetryCount.ToString());
                            if (RetryCount == 3)
                            {
                                //MessageBox.Show(this,"H0083: Unable to load data. Connection failed. Please check your network connection.","Herbert - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                MessageBox.Show(this, "H0083: Connection failed. Please check your network connection.", "Herbert - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                blnLoginSucces = false;
                            }
                        }
                        else
                        {
#if(PROBE)
                            //ProbeId: 005
                            MessageBox.Show(this, "H0082: Connection to the server timed out. Please try again later."+"\nProbeID: 005" + "\n Msg: " + exp.Message + "\n Trace: " + exp.StackTrace + "\n Inner Msg: " + exp.InnerException, "Herbert - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            blnLoginSucces = false;
#endif
                            MessageBox.Show(this, "H0084: Connection to the server failed. Please try again later.", "Herbert - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            blnLoginSucces = false;
                            break;
                        }
                }
            }
            try
            {
                if (!blnProxyAuthReq)
                {
                    if (dsAuthenticate.Tables[0].Rows.Count > 0 && blnLoginSucces)
                    {
                        GlobalData.GUID = dsAuthenticate.Tables[0].Rows[0]["GUID"].ToString();
                        GlobalData.HerbertMode = HMode.Contest;
                        this.DialogResult = DialogResult.Yes;
                    }
                    else
                    {
                        this.DialogResult = DialogResult.No;
                    }
                }
            }
            catch
            {
                //this.DialogResult = DialogResult.No;
            }

            //lbl.Text += HerbertMain.s;				

            #endregion
        }


        private void cmbContestList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string contest = cmbContestList.Text;
            if (contest.ToLower() == "tutorial")
            {
#if(!DESIGNER)
                btLaunchHerbert.Enabled = true;
                btRunGuest.Enabled = false;
#endif
                return;
            }
            string cid;
            for (int i = 0; i < dsContestsList.Tables[0].Rows.Count; i++)
            {
                cid = dsContestsList.Tables[0].Rows[i]["Title"].ToString();
                if (cid.ToLower() == contest.ToLower())
                {
#if(CONTEST)
                    if (dsContestsList.Tables[0].Rows[i]["IsReg"].ToString() == "1")
                    {
#if(!DESIGNER)
                        btLaunchHerbert.Enabled = true;
                        btRunGuest.Enabled = false;
#endif
                    }
                    else
                    {
                        if (dsContestsList.Tables[0].Rows[i]["IsUserRegistered"].ToString() == "0")
                        {
#if(!DESIGNER)
                            btLaunchHerbert.Enabled = false;
                            btRunGuest.Enabled = true;
#endif
                        }
                        else
                        {
#if(!DESIGNER)
                            btLaunchHerbert.Enabled = true;
                            btRunGuest.Enabled = true;
#endif
                        }
                    }
#endif
                }


            }
        }


        private void btRunGuest_Click(object sender, System.EventArgs e)
        {
            Version = Application.ProductVersion;

            int contestid = 0;

            string contest = cmbContestList.Text;
            if (contest.ToLower() == "tutorial")
            {
                GlobalData.HerbertMode = HMode.Tutorial;
                this.DialogResult = DialogResult.Yes;
                return;
            }

            string cid;
            for (int i = 0; i < dsContestsList.Tables[0].Rows.Count; i++)
            {
                cid = dsContestsList.Tables[0].Rows[i]["Title"].ToString();
                if (cid.ToLower() == contest.ToLower())
                {
                    string ContestVersion = dsContestsList.Tables[0].Rows[i]["ContestVersion"].ToString();
                    if (ContestVersion != null && ContestVersion != "")
                    {
                        string ExeVersion = Version.ToString();
                        if (CheckOldVersion(ContestVersion, ExeVersion))
                        {
                            MessageBox.Show(this, "Currently you are using older version of Herbert for the selected contest.\nYou may not be able to access all features of Herbert. Please download latest version of Herbert", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    contestid = int.Parse(dsContestsList.Tables[0].Rows[i]["contestid"].ToString());
                    break;
                }
            }

            GlobalData.iContestId = contestid;

            /*if( contestid == 0 )
            {
                GlobalData.HerbertMode = HMode.Tutorial;
                this.DialogResult = DialogResult.Yes;
                return;
            }*/

            #region

            //oPassword = HerbertMain.EnableMyMenu(Encrypt(txtPassword.Text));
            String str = oPassword.ToString();
            //oEmail = HerbertMain.EnableMyMenu(txtLoginId.Text);
            //int 
            oContestId = HerbertMain.EnableMyMenu(GlobalData.iContestId);
            oMode = HerbertMain.EnableMyMenu("0");
            oVersion = HerbertMain.EnableMyMenu(Version);

            DataSet dsAuthenticate = new DataSet();

            bool blnLoginSucces = true;
            bool blnProxyAuthReq = false;
            int RetryCount = 0;
            while (blnLoginSucces)
            {
                try
                {
#if(DLL)
					dsAuthenticate =  GlobalData.DS.GetDataForGuest(oEmail,oPassword,oContestId,oMode);						
#else
                    dsAuthenticate = GlobalData.HS.GetDataForGuest(oEmail, oPassword, oContestId, oMode, oVersion);
#endif
                    break;
                }
                catch (Exception exp)
                {
                    if (exp.Message.IndexOf("HTTP status 407") >= 0 || exp.Message.IndexOf("Unauthorized") >= 0)
                    {
                        gbNetwork.Visible = true;
                        gbLogin.Visible = false;
                        GlobalData.isBehindProxy = true;
                        MessageBox.Show(this, "It appears that you are behind a proxy.\nPlease enter your proxy credentials.", "Login error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        blnLoginSucces = false;
                        blnProxyAuthReq = true;
                        System.Net.WebProxy wProxy;                        
                        try
                        {
                            wProxy = System.Net.WebProxy.GetDefaultProxy();
                            gbNetwork.Text = "Proxy: " + wProxy.Address.Host;
                            txtAddress.Text = wProxy.Address.Host;
                            txtPort.Text = wProxy.Address.Port.ToString();
                        }
                        catch { }
                        GlobalData.authReg = true;
                    }
                    else if (exp.Message.IndexOf("timed-out") > 0)
                    {
                        RetryCount++;
                        blnLoginSucces = true;
                        //MessageBox.Show(this,RetryCount.ToString());
                        if (RetryCount == 3)
                        {
                            //MessageBox.Show(this,"H0082: Unable to load data. Connection to the server timed out. Please try again later.","Herbert - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            MessageBox.Show(this, "H0082: Connection to the server timed out. Please try again later.", "Herbert - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            blnLoginSucces = false;
                        }
                    }
                    else if (exp.Message.IndexOf("underlying connection was closed") > 0)
                    {
                        RetryCount++;
                        blnLoginSucces = true;
                        //MessageBox.Show(this,RetryCount.ToString());
                        if (RetryCount == 3)
                        {
                            //MessageBox.Show(this,"H0083: Unable to load data. Connection failed. Please check your network connection.","Herbert - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            MessageBox.Show(this, "H0083: Connection failed. Please check your network connection.", "Herbert - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            blnLoginSucces = false;
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "H0084: Connection to the server failed. Please try again later.", "Herbert - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        blnLoginSucces = false;
                        break;
                    }
                }
            }
            try
            {
                if (!blnProxyAuthReq)
                {
                    if (dsAuthenticate.Tables[0].Rows.Count > 0 && blnLoginSucces)
                    {
                        GlobalData.GUID = dsAuthenticate.Tables[0].Rows[0]["GUID"].ToString();
                        GlobalData.HerbertMode = HMode.Contest;
                        this.DialogResult = DialogResult.Yes;
                    }
                    else
                    {
                        this.DialogResult = DialogResult.No;
                    }
                }
            }
            catch (Exception exp)
            {
                this.DialogResult = DialogResult.No;
            }

            //lbl.Text += HerbertMain.s;				

            #endregion

        }


        /// <summary>
        /// Created by Pavan for checking the version of supporting contests
        /// </summary>
        /// <param name="ContestVersion"></param>
        /// <param name="ExeVersion"></param>
        /// <returns></returns>
        private bool CheckOldVersion(string ContestVersion, string ExeVersion)
        {
            string[] cversion = ContestVersion.Split(new char[] { '.' }, 10);
            string[] eversion = ExeVersion.Split(new char[] { '.' }, 10);

            if (!(int.Parse(eversion[0].ToString()) >= int.Parse(cversion[0].ToString())))
            {
                return true;
            }
            else if (int.Parse(eversion[0].ToString()) > int.Parse(cversion[0].ToString()))
            {
                return false;
            }
            else
            {
                if (!(int.Parse(eversion[1].ToString()) >= int.Parse(cversion[1].ToString())))
                {
                    return true;
                }
                else if (int.Parse(eversion[1].ToString()) > int.Parse(cversion[1].ToString()))
                {
                    return false;
                }
                else
                {
                    if (!(int.Parse(eversion[2].ToString()) >= int.Parse(cversion[2].ToString())))
                    {
                        return true;
                    }
                    else if (int.Parse(eversion[2].ToString()) >= int.Parse(cversion[2].ToString()))
                    {
                        return false;
                    }
                    else
                    {
                        if (!(int.Parse(eversion[3].ToString()) >= int.Parse(cversion[3].ToString())))
                        {
                            return true;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }

            return false;

        }


        private void btnLaunchDesigner_Click(object sender, System.EventArgs e)
        {
            Version = Application.ProductVersion;

            int contestid = 0;

            string contest = cmbContestList.Text;
            if (contest.ToLower() == "tutorial")
            {
                GlobalData.HerbertMode = HMode.Tutorial;
                this.DialogResult = DialogResult.Yes;
                return;
            }


            string cid;
            for (int i = 0; i < dsContestsList.Tables[0].Rows.Count; i++)
            {
                cid = dsContestsList.Tables[0].Rows[i]["Title"].ToString();
                if (cid.ToLower() == contest.ToLower())
                {
                    string ContestVersion = dsContestsList.Tables[0].Rows[i]["ContestVersion"].ToString();
                    //					if( ContestVersion != null || ContestVersion != "" )
                    //					{
                    //#if (!DESIGNER)
                    //						string ExeVersion = Version.ToString();
                    //						if( CheckOldVersion(ContestVersion,ExeVersion) )
                    //						{
                    //							MessageBox.Show(this,"Currently you are using older version of Herbert for the selected contest.\nYou may not be able to access all features of Herbert. Please download latest version of Herbert","Login",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //							return;
                    //						}
                    //#endif
                    //					}
                    contestid = int.Parse(dsContestsList.Tables[0].Rows[i]["contestid"].ToString());
                    break;
                }
            }

            GlobalData.iContestId = contestid;

            #region

            oPassword = HerbertMain.EnableMyMenu(Encrypt(txtPassword.Text));
            String str = oPassword.ToString();
            oEmail = HerbertMain.EnableMyMenu(txtLoginId.Text);
            //int 
            oContestId = HerbertMain.EnableMyMenu(GlobalData.iContestId);
            oMode = HerbertMain.EnableMyMenu("0");
            object oVersion = HerbertMain.EnableMyMenu(Version);

            DataSet dsAuthenticate = new DataSet();

            bool blnLoginSucces = true;
            bool blnProxyAuthReq = false;
            int RetryCount = 0;
            while (blnLoginSucces)
            {
                try
                {
#if(DLL)
					dsAuthenticate =  GlobalData.DS.Authenticate(oEmail,oPassword,oContestId,oMode);						
#else
                 
                    dsAuthenticate = GlobalData.HS.DAuthenticateNew(oEmail, oPassword, oContestId, oMode, oVersion);
#endif
                    break;
                }
                catch (Exception exp)
                {
                    if (exp.Message.IndexOf("HTTP status 407") >= 0 || exp.Message.IndexOf("Unauthorized") >= 0)
                    {
                        gbNetwork.Visible = true;
                        gbLogin.Visible = false;
                        MessageBox.Show(this, "It appears that you are behind a proxy.\nPlease enter your proxy credentials.", "Login error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        GlobalData.isBehindProxy = true;
                        blnLoginSucces = false;
                        blnProxyAuthReq = true;
                        System.Net.WebProxy wProxy;
                        wProxy = System.Net.WebProxy.GetDefaultProxy();
                        gbNetwork.Text = "Proxy: " + wProxy.Address.Host;
                        GlobalData.authReg = true;
                    }
                    else
                        if (exp.Message.IndexOf("HS1001") >= 0)
                        {
                            string message = exp.Message.Substring(exp.Message.IndexOf("HS1001"), exp.Message.IndexOf("HS1002") - exp.Message.IndexOf("HS1001"));
                            MessageBox.Show(this, message, "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            blnLoginSucces = false;
                        }
                        else if (exp.Message.IndexOf("Instance Loading") >= 0)
                        {

                            DialogResult dr = MessageBox.Show(this, "May be an instance of Herbert is loading. Do you want to override that instance?", "Login", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (dr == DialogResult.Yes)
                            {
                                oMode = HerbertMain.EnableMyMenu("1");
                                blnLoginSucces = true;
                            }
                            else
                            {
                                blnLoginSucces = false;
                                break;
                            }
                        }
                        else if (exp.Message.IndexOf("Another instance running") >= 0)
                        {
                            DialogResult dr = MessageBox.Show(this, "It appears you are already running Herbert. You can only run one instance of Herbert at a time. \nNote: You might receive this message after a computer crash. If that is the case you can ignore this warning. Work from your prior instance may be lost. \nDo you want to continue?", "Login", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (dr == DialogResult.Yes)
                            {
                                oMode = HerbertMain.EnableMyMenu("1");
                                blnLoginSucces = true;
                            }
                            else
                            {
                                blnLoginSucces = false;
                                break;
                            }
                        }
                        else if (exp.Message.IndexOf("timed-out") > 0)
                        {
                            RetryCount++;
                            blnLoginSucces = true;
                            //MessageBox.Show(this,RetryCount.ToString());
                            if (RetryCount == 3)
                            {
                                //MessageBox.Show(this,"H0082: Unable to load data. Connection to the server timed out. Please try again later.","Herbert - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                MessageBox.Show(this, "H0082: Connection to the server timed out. Please try again later.", "Herbert - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                blnLoginSucces = false;
                            }
                        }
                        else if (exp.Message.IndexOf("underlying connection was closed") > 0)
                        {
                            RetryCount++;
                            blnLoginSucces = true;
                            //MessageBox.Show(this,RetryCount.ToString());
                            if (RetryCount == 3)
                            {
                                //MessageBox.Show(this,"H0083: Unable to load data. Connection failed. Please check your network connection.","Herbert - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                MessageBox.Show(this, "H0083: Connection failed. Please check your network connection.", "Herbert - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                blnLoginSucces = false;
                            }
                        }
                        else
                        {
                            MessageBox.Show(this, "H0084: Connection to the server failed. Please try again later.", "Herbert - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            blnLoginSucces = false;
                            break;
                        }
                }
            }

            try
            {
                if (dsAuthenticate.Tables[0].Rows.Count > 0 && blnLoginSucces)
                {
                    GlobalData.GUID = dsAuthenticate.Tables[0].Rows[0]["DGUID"].ToString();
                    GlobalData.HerbertMode = HMode.Designer;
                    this.DialogResult = DialogResult.Yes;
                }
                else
                {
                    this.DialogResult = DialogResult.No;
                }
            }
            catch
            {
                this.DialogResult = DialogResult.No;
            }

            //lbl.Text += HerbertMain.s;				

            #endregion
            //this.DialogResult = DialogResult.Yes;
        }
        /// <summary>
        /// To show the contest selection page.
        /// </summary>
        public void ShowConetestSeltionWindow()
        {            
            txtLoginId.Text = GlobalData.loginName;
            txtPassword.Text = GlobalData.password;           
            btLogin_Click(null, null);
        }
        // ------------ added by NikhilK to fix issue 9552 ---------------
        public void CheckForProxyForOfflineVersion()
        {
            System.Net.WebProxy wProxy;

            wProxy = System.Net.WebProxy.GetDefaultProxy();
            if (wProxy != null)
                gbNetwork.Text = "Proxy: " + wProxy.Address.Host;
            txtAddress.Text = wProxy.Address.Host;
            txtPort.Text = wProxy.Address.Port.ToString();
        }


        /// <summary>
        /// To show network login window while uploading solutions after logging in from offline version
        /// </summary>
        /// // ------------ added by NikhilK to fix issue 9552 ---------------
        public void ShowNetworkLoginWindow()
        {
            gbLogin.Enabled = false;
            gbNetwork.Enabled = true;
            gbContest.Enabled = false;
            //bttnOK_Click(null, null);
        }
        /// <summary>
        /// added by Nikhil Kardale on 25/7/2007
        /// </summary>
        #region Practice button - Tutorial Mode
        private void btPractice_Click(object sender, EventArgs e)
        {
            // -- added by NikhilK on 26/10/2007 for Remember Me functionality --

            if (chkRememberMe.Checked)
            {
                WriteToRememberMeFile();
            }
            else if (!chkRememberMe.Checked && GlobalData.isLoadFromRememberMeFile == true)
            {
                try
                {
                    File.Delete(pathToRememberMeFile);
                    GlobalData.isLoadFromRememberMeFile = false;
                }
                catch (Exception eee)
                {
                    // do nothing
                }
            }
            else
            {
                // do nothing
            }

            // -- End of Remember Me functionality --
            GlobalData.HerbertMode = HMode.Tutorial;
            this.DialogResult = DialogResult.Yes;
            GlobalData.iShowSwtichContest = 0;
            return;
        }
        #endregion
#if(OFFLINE_HEBERT)
        private string IsUserSessionPresent(Designer.XMLEncryptor xmObject)
        {
            string retPath=String.Empty;
            string strDirectryPath = Designer.HerbertMain.GetDirecteryPathForLogin(); 
            string strMasterPath = strDirectryPath + "\\Herbert.Offline";

            if (File.Exists(strMasterPath))
            {
                try
                {
                    DataSet ds = new DataSet();
                    Designer.XMLEncryptor xmTm = new XMLEncryptor();
                    ds = xmTm.ReadEncryptedXML(strMasterPath);
                    DataRow[] drow= ds.Tables[0].Select("Col1='" + HerbertMain.EnableMyMenu(xmObject.GetKey()).ToString()+"'");
                    if(drow.Length!=0)
                    retPath = drow[0][1].ToString();
                }
                catch
                {
                }
            }
            return retPath;
        }
#endif
    }
#endif
}

