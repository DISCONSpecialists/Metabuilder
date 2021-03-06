using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Text;
using System.Windows.Forms;
using MetaBuilder.Graphing.Containers;

namespace MetaBuilder.UIControls.GraphingUI
{
    // The BackgroundWorker will be used to perform a long running action
    // on a background thread.  This allows the UI to be free for painting
    // as well as other actions the user may want to perform.  The background
    // thread will use the ReportProgress event to update the ProgressBar
    // on the UI thread.
    public partial class AutoSaveForm : Form
    {
        /// <summary>
        /// The backgroundworker object on which the time consuming operation shall be executed
        /// </summary>
        private BackgroundWorker m_oWorker;

        public AutoSaveForm()
        {
            InitializeComponent();
            m_oWorker = new BackgroundWorker();
            // Create a background worker thread that ReportsProgress &
            // SupportsCancellation
            // Hook up the appropriate events.
            m_oWorker.DoWork += new DoWorkEventHandler(m_oWorker_DoWork);
            m_oWorker.ProgressChanged += new ProgressChangedEventHandler(m_oWorker_ProgressChanged);
            m_oWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(m_oWorker_RunWorkerCompleted);
            m_oWorker.WorkerReportsProgress = true;
            m_oWorker.WorkerSupportsCancellation = false;
        }

        /// <summary>
        /// On completed do the appropriate task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_oWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DockingForm.DockForm.ProgressUpdate(75);
            //Thread.Sleep(10);
            //ndiagram.SuspendsUpdates = false;
            // The background process is complete. We need to inspect
            // our response to see if an error occurred, a cancel was
            // requested or if we completed successfully.  
            //if (e.Cancelled)
            //{
            //    lblStatus.Text = "Task Cancelled.";
            //}

            //// Check to see if an error occurred in the background process.

            //else if (e.Error != null)
            //{
            //    lblStatus.Text = "Error while performing background operation.";
            //    Core.Log.WriteLog(e.Error.ToString());
            //}
            //else
            //{
            //    // Everything completed normally.
            //    lblStatus.Text = "Task Completed...";
            //}

            ////Change the status of the buttons on the UI accordingly
            ////btnStartAsyncOperation.Enabled = true;
            //btnCancel.Enabled = false;
            //this.Close();

            DockingForm.DockForm.ProgressUpdate(100);
            //Thread.Sleep(10);
            DockingForm.DockForm.UpdateTotal(0);
            this.Dispose();
            this.Close();
        }

        /// <summary>
        /// Notification is performed here to the progress bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_oWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            // This function fires on the UI thread so it's safe to edit

            // the UI control directly, no funny business with Control.Invoke :)

            // Update the progressBar with the integer supplied to us from the

            // ReportProgress() function.  

            progressBar1.Value = e.ProgressPercentage;
            lblStatus.Text = "Processing......" + progressBar1.Value.ToString() + "%";
        }

        private NormalDiagram ndiagram;
        private string autosaveFileName;
        public void SetDocument(NormalDiagram ndiagram, string autosaveFileName)
        {
            this.ndiagram = ndiagram;//.CopyAsShallow();
            this.autosaveFileName = autosaveFileName;
        }

        /// <summary>
        /// Time consuming operations go here </br>
        /// i.e. Database operations,Reporting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_oWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // The sender is the BackgroundWorker object we need it to
            // report progress and check for cancellation.
            //NOTE : Never play with the UI thread here...


            // Periodically report progress to the main thread so that it can

            // update the UI.  In most cases you'll just need to send an
            MetaBuilder.Graphing.Persistence.XMLPersistence.AutoSaver autosaver = new MetaBuilder.Graphing.Persistence.XMLPersistence.AutoSaver();
            autosaver.Diagram = ndiagram;
            //ndiagram.SuspendsUpdates = true;
            autosaver.AutosaveFileName = autosaveFileName;
            autosaver.Execute();

            //Thread t = new Thread(ts);
            //t.Start();

            //string filename = ndiagram.Name;
            //bool saved = true;


            /*if (!saved)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("The system cannot find the file specified." + Environment.NewLine);
                sb.Append("1) If you are saving to the network" + Environment.NewLine);
                sb.Append("   a) Check your connection" + Environment.NewLine);
                sb.Append("   b) If you are disconnected, first save the file to your local harddrive." +
                          Environment.NewLine);
                sb.Append("2) Otherwise, check your access rights to the specified file");
              //  MessageBox.Show(this,sb.ToString(), "AutoSave Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/
            // integer that will update a ProgressBar    

            //m_oWorker.ReportProgress(i);


            // Periodically check if a cancellation request is pending.

            // If the user clicks cancel the line

            // m_AsyncWorker.CancelAsync(); if ran above.  This

            // sets the CancellationPending to true.

            // You must check this flag in here and react to it.

            // We react to it by setting e.Cancel to true and leaving


            //if (m_oWorker.CancellationPending)
            //{
            //    // Set the e.Cancel flag so that the WorkerCompleted event
            //    // knows that the process was cancelled.
            //    e.Cancel = true;
            //    m_oWorker.ReportProgress(0);
            //    return;
            //}



            //Report 100% completion on operation completed
            m_oWorker.ReportProgress(100);
        }

        //public void SaveDiagram(ref NormalDiagram ndiagram, string autosaveFileName)
        //{
        //    //Change the status of the buttons on the UI accordingly
        //    //The start button is disabled as soon as the background operation is started
        //    //The Cancel button is enabled so that the user can stop the operation at any point of time
        //    //during the execution
        //    //btnStartAsyncOperation.Enabled = false;
        //    btnCancel.Enabled = true;

        //    // Kickoff the worker thread to begin it's DoWork function.
        //    m_oWorker.RunWorkerAsync();
        //}

        public void StartProcess()
        {
            DockingForm.DockForm.UpdateStatusLabel("Autosaving");
            DockingForm.DockForm.UpdateTotal(100);
            DockingForm.DockForm.ProgressUpdate(25);
            //Thread.Sleep(10);
            //Change the status of the buttons on the UI accordingly
            //The start button is disabled as soon as the background operation is started
            //The Cancel button is enabled so that the user can stop the operation at any point of time
            //during the execution
            //btnStartAsyncOperation.Enabled = false;
            btnCancel.Enabled = true;

            // Kickoff the worker thread to begin it's DoWork function.
            m_oWorker.RunWorkerAsync();
            DockingForm.DockForm.ProgressUpdate(50);
            //Thread.Sleep(10);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (m_oWorker.IsBusy)
            {

                // Notify the worker thread that a cancel has been requested.

                // The cancel will not actually happen until the thread in the

                // DoWork checks the m_oWorker.CancellationPending flag. 

                m_oWorker.CancelAsync();
            }
        }
    }
}