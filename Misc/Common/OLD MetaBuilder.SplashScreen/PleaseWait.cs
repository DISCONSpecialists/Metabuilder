using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Ascend.Windows.Forms;
using Timer=System.Windows.Forms.Timer;

namespace MetaBuilder.SplashScreen
{
	/// <summary>
	/// Summary description for SplashScreen.
	/// </summary>
	public class PleaseWait : Form
	{
        private bool requireClickToClose;

        public bool RequireClickToClose
        {
            get { return requireClickToClose; }
            set { requireClickToClose = value; }
        }

        public static bool RequireClick
        {
            get
            {
                if (ms_frmSplash!=null)
                    return ms_frmSplash.RequireClickToClose;
                return false;
            }
            set
            {
                if (ms_frmSplash != null)
                    ms_frmSplash.RequireClickToClose = value;
            }
        }
	
		// Threading
		static PleaseWait ms_frmSplash = null;
		static Thread ms_oThread = null;

		// Fade in and out.
		private double m_dblOpacityIncrement = .05;
		private double m_dblOpacityDecrement = .08;
		private const int TIMER_INTERVAL = 50;

		// Status and progress bar
		static string ms_sStatus;
		private double m_dblCompletionFraction = 0;
		private Rectangle m_rProgress;

		// Progress smoothing
		private double m_dblLastCompletionFraction = 0.0;
		private double m_dblPBIncrementPerTimerInterval = .015;

		// Self-calibration support
		private bool m_bFirstLaunch = false;
		private DateTime m_dtStart;
		private bool m_bDTSet = false;
		private int m_iIndex = 1;
		private int m_iActualTicks = 0;
		private ArrayList m_alPreviousCompletionFraction;
		private ArrayList m_alActualTimes = new ArrayList();
		private const string REG_KEY_INITIALIZATION = "Initialization";
		private const string REGVALUE_PB_MILISECOND_INCREMENT = "Increment";
        private const string REGVALUE_PB_PERCENTS = "Percents";
        private Timer timer1;
        private GradientAnimation gradientAnimation1;
        private GradientNavigationButton btnContinue;
        private GradientLine gradientLine1;
        private GradientPanel gradientPanel1;
        private GradientCaption gradientCaption1;
        private Label lblStatus;
		private IContainer components;

		/// <summary>
		/// Constructor
		/// </summary>
        public PleaseWait()
		{
			InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
			this.Opacity = .00;
			timer1.Interval = TIMER_INTERVAL;
			timer1.Start();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
            try
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
            catch { }
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.gradientAnimation1 = new Ascend.Windows.Forms.GradientAnimation();
            this.btnContinue = new Ascend.Windows.Forms.GradientNavigationButton();
            this.gradientLine1 = new Ascend.Windows.Forms.GradientLine();
            this.gradientPanel1 = new Ascend.Windows.Forms.GradientPanel();
            this.gradientCaption1 = new Ascend.Windows.Forms.GradientCaption();
            this.lblStatus = new System.Windows.Forms.Label();
            this.gradientPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // gradientAnimation1
            // 
            this.gradientAnimation1.BackColor = System.Drawing.Color.SteelBlue;
            this.gradientAnimation1.Border = new Ascend.Border(1);
            this.gradientAnimation1.GradientHighColor = System.Drawing.Color.SteelBlue;
            this.gradientAnimation1.GradientLowColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.gradientAnimation1.Interval = 50;
            this.gradientAnimation1.Location = new System.Drawing.Point(86, 10);
            this.gradientAnimation1.Name = "gradientAnimation1";
            this.gradientAnimation1.Size = new System.Drawing.Size(190, 10);
            this.gradientAnimation1.Speed = 30;
            this.gradientAnimation1.TabIndex = 4;
            this.gradientAnimation1.TabStop = false;
            // 
            // btnContinue
            // 
            this.btnContinue.CornerRadius = new Ascend.CornerRadius(3);
            this.btnContinue.GradientLowColor = System.Drawing.Color.Orange;
            this.btnContinue.Location = new System.Drawing.Point(219, 85);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(63, 20);
            this.btnContinue.TabIndex = 5;
            this.btnContinue.Text = "Continue";
            this.btnContinue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnContinue.Visible = false;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // gradientLine1
            // 
            this.gradientLine1.GradientLowColor = System.Drawing.Color.Gray;
            this.gradientLine1.Location = new System.Drawing.Point(7, 22);
            this.gradientLine1.Name = "gradientLine1";
            this.gradientLine1.Size = new System.Drawing.Size(267, 2);
            this.gradientLine1.TabIndex = 6;
            // 
            // gradientPanel1
            // 
            this.gradientPanel1.AntiAlias = true;
            this.gradientPanel1.Border = new Ascend.Border(1);
            this.gradientPanel1.Controls.Add(this.btnContinue);
            this.gradientPanel1.Controls.Add(this.gradientCaption1);
            this.gradientPanel1.Controls.Add(this.lblStatus);
            this.gradientPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gradientPanel1.Location = new System.Drawing.Point(0, 0);
            this.gradientPanel1.Name = "gradientPanel1";
            this.gradientPanel1.Size = new System.Drawing.Size(285, 108);
            this.gradientPanel1.TabIndex = 7;
            // 
            // gradientCaption1
            // 
            this.gradientCaption1.Alpha = 0;
            this.gradientCaption1.ForeColor = System.Drawing.Color.Black;
            this.gradientCaption1.Location = new System.Drawing.Point(3, 3);
            this.gradientCaption1.Name = "gradientCaption1";
            this.gradientCaption1.Size = new System.Drawing.Size(77, 21);
            this.gradientCaption1.TabIndex = 0;
            this.gradientCaption1.Text = "Please Wait";
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(4, 27);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(272, 78);
            this.lblStatus.TabIndex = 2;
            // 
            // PleaseWait
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(285, 108);
            this.Controls.Add(this.gradientLine1);
            this.Controls.Add(this.gradientAnimation1);
            this.Controls.Add(this.gradientPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PleaseWait";
            this.Opacity = 0;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MetaBuilder Startup";
            this.TopMost = true;
            this.DoubleClick += new System.EventHandler(this.SplashScreen_DoubleClick);
            this.Click += new System.EventHandler(this.PleaseWait_Click);
            this.Load += new System.EventHandler(this.PleaseWait_Load);
            this.gradientPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		// ************* Static Methods *************** //

		// A static method to create the thread and 
		// launch the SplashScreen.
		static public void ShowPleaseWaitForm()
		{
			// Make sure it's only launched once.
			if( ms_frmSplash != null )
				return;
            ms_oThread = new Thread(new ThreadStart(ShowForm));
			ms_oThread.IsBackground = true;
			ms_oThread.ApartmentState = ApartmentState.STA;
			ms_oThread.Start();
		}

		// A property returning the splash screen instance
        static public PleaseWait PleaseWaitForm 
		{
			get
			{
				return ms_frmSplash;
			} 
		}

		// A private entry point for the thread.
		static private void ShowForm()
		{
            ms_frmSplash = new PleaseWait();
			Application.Run(ms_frmSplash);
		}

		// A static method to close the SplashScreen
		static public void CloseForm()
        {

            if (ms_frmSplash != null && ms_frmSplash.IsDisposed == false)
            {
                // Make it start going away.
                ms_frmSplash.m_dblOpacityIncrement = -ms_frmSplash.m_dblOpacityDecrement;
            }
            ms_oThread = null;	// we don't need these any more.
            ms_frmSplash = null;
            
        }

		// A static method to set the status and update the reference.
		static public void SetStatus(string newStatus)
		{
			SetStatus(newStatus, true);
		}

		// A static method to set the status and optionally update the reference.
		// This is useful if you are in a section of code that has a variable
		// set of status string updates.  In that case, don't set the reference.
		static public void SetStatus(string newStatus, bool setReference)
		{
			ms_sStatus = newStatus;
			if( ms_frmSplash == null )
				return;
			if( setReference )
				ms_frmSplash.SetReferenceInternal();
		}

		// Static method called from the initializing application to 
		// give the splash screen reference points.  Not needed if
		// you are using a lot of status strings.
		static public void SetReferencePoint()
		{
			if( ms_frmSplash == null )
				return;
			ms_frmSplash.SetReferenceInternal();

		}

		// ************ Private methods ************

		// Internal method for setting reference points.
		private void SetReferenceInternal()
		{
			if( m_bDTSet == false )
			{
				m_bDTSet = true;
				m_dtStart = DateTime.Now;
				ReadIncrements();
			}
			double dblMilliseconds = ElapsedMilliSeconds();
			m_alActualTimes.Add(dblMilliseconds);
			m_dblLastCompletionFraction = m_dblCompletionFraction;
			if( m_alPreviousCompletionFraction != null && m_iIndex < m_alPreviousCompletionFraction.Count )
				m_dblCompletionFraction = (double)m_alPreviousCompletionFraction[m_iIndex++];
			else
				m_dblCompletionFraction = ( m_iIndex > 0 )? 1: 0;
		}

		// Utility function to return elapsed Milliseconds since the 
		// SplashScreen was launched.
		private double ElapsedMilliSeconds()
		{
			TimeSpan ts = DateTime.Now - m_dtStart;
			return ts.TotalMilliseconds;
		}

		// Function to read the checkpoint intervals from the previous invocation of the
		// splashscreen from the registry.
		private void ReadIncrements()
		{
			string sPBIncrementPerTimerInterval = RegistryAccess.GetStringRegistryValue( REGVALUE_PB_MILISECOND_INCREMENT, "0.0015");
			double dblResult;

			if( Double.TryParse(sPBIncrementPerTimerInterval, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out dblResult) == true )
				m_dblPBIncrementPerTimerInterval = dblResult;
			else
				m_dblPBIncrementPerTimerInterval = .0015;

			string sPBPreviousPctComplete = RegistryAccess.GetStringRegistryValue( REGVALUE_PB_PERCENTS, "" );

			if( sPBPreviousPctComplete != "" )
			{
				string [] aTimes = sPBPreviousPctComplete.Split(null);
				m_alPreviousCompletionFraction = new ArrayList();

				for(int i = 0; i < aTimes.Length; i++ )
				{
					double dblVal;
					if( Double.TryParse(aTimes[i], NumberStyles.Float, NumberFormatInfo.InvariantInfo, out dblVal) )
						m_alPreviousCompletionFraction.Add(dblVal);
					else
						m_alPreviousCompletionFraction.Add(1.0);
				}
			}
			else
			{
				m_bFirstLaunch = true;
			}
		}

		// Method to store the intervals (in percent complete) from the current invocation of
		// the splash screen to the registry.
		private void StoreIncrements()
		{
			string sPercent = "";
			double dblElapsedMilliseconds = ElapsedMilliSeconds();
			for( int i = 0; i < m_alActualTimes.Count; i++ )
				sPercent += ((double)m_alActualTimes[i]/dblElapsedMilliseconds).ToString("0.####", NumberFormatInfo.InvariantInfo) + " ";

			RegistryAccess.SetStringRegistryValue( REGVALUE_PB_PERCENTS, sPercent );

			m_dblPBIncrementPerTimerInterval = 1.0/(double)m_iActualTicks;
			RegistryAccess.SetStringRegistryValue( REGVALUE_PB_MILISECOND_INCREMENT, m_dblPBIncrementPerTimerInterval.ToString("#.000000", NumberFormatInfo.InvariantInfo));
		}

		//********* Event Handlers ************

		// Tick Event handler for the Timer control.  Handle fade in and fade out.  Also
		// handle the smoothed progress bar.
		private void timer1_Tick(object sender, EventArgs e)
		{
            
			lblStatus.Text = ms_sStatus;

            if (RequireClickToClose)
            {
                this.gradientAnimation1.Enabled = false;
                btnContinue.Visible = true;
                this.gradientAnimation1.Stop();
            }
            else
            {/*
                if (progressBar1.Value < 100)
                    this.progressBar1.Value++;
                else
                    this.progressBar1.Value = 0;*/
            }
            
			if( m_dblOpacityIncrement > 0 )
			{
				m_iActualTicks++;
				if( this.Opacity < 1 )
					this.Opacity += m_dblOpacityIncrement;
			}
			else
			{
				if( this.Opacity > 0 )
					this.Opacity += m_dblOpacityIncrement;
				else
				{
					StoreIncrements();
					this.Close();
					//Debug.WriteLine("Called this.Close()");
				}
			}
            /*
			if( m_bFirstLaunch == false && m_dblLastCompletionFraction < m_dblCompletionFraction )
			{
				m_dblLastCompletionFraction += m_dblPBIncrementPerTimerInterval;
				int width = (int)Math.Floor(pnlStatus.ClientRectangle.Width * m_dblLastCompletionFraction);
				int height = pnlStatus.ClientRectangle.Height;
				int x = pnlStatus.ClientRectangle.X;
				int y = pnlStatus.ClientRectangle.Y;
				if( width > 0 && height > 0 )
				{
					m_rProgress = new Rectangle( x, y, width, height);
					pnlStatus.Invalidate(m_rProgress);
					int iSecondsLeft = 1 + (int)(TIMER_INTERVAL * ((1.0 - m_dblLastCompletionFraction)/m_dblPBIncrementPerTimerInterval)) / 1000;

				}
			}*/
		}

		// Paint the portion of the panel invalidated during the tick event.
		private void pnlStatus_Paint(object sender, PaintEventArgs e)
		{
			if( m_bFirstLaunch == false && e.ClipRectangle.Width > 0 && m_iActualTicks > 1 )
			{
                try
                {
                    if (m_rProgress.X > 0 && m_rProgress.Y > 0 && m_rProgress.Width > 0)
                    {
                        LinearGradientBrush brBackground = new LinearGradientBrush(m_rProgress, Color.White, Color.FromArgb(255, 128, 0), LinearGradientMode.Horizontal);
                        e.Graphics.FillRectangle(brBackground, m_rProgress);
                    }
                }
                catch { }
			}
		}

		// Close the form if they double click on it.
		private void SplashScreen_DoubleClick(object sender, EventArgs e)
		{
			CloseForm();
		}

        private void lblStatus_Click(object sender, EventArgs e)
        {
            CloseForm();
        }

        private void PleaseWait_Click(object sender, EventArgs e)
        {
            CloseForm();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CloseForm();
        }

        private void PleaseWait_Load(object sender, EventArgs e)
        {
            this.gradientAnimation1.Start();
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            CloseForm();
        }

        
	}

}
