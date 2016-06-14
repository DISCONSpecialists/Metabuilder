using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MetaBuilder.UIControls.GraphingUI.Tools.ObjectFlowExport
{
    public partial class ObjectFlowExportInterface : Form
    {
        Timer t = new Timer();

        public ObjectFlowExportInterface()
        {
            InitializeComponent();
            t.Tick += new EventHandler(t_Tick);

        }

        private void t_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value < progressBar1.Maximum)
                progressBar1.PerformStep();
            else
                progressBar1.Value = progressBar1.Step * 10;
        }

        string filename = "";
        private void ObjectFlowExportInterface_Load(object sender, EventArgs e)
        {
            if (getFileName().Length > 0)
            {
                progressBar1.Minimum = 10;
                progressBar1.Maximum = 1000;
                progressBar1.Step = 10;
                progressBar1.Style = ProgressBarStyle.Continuous;

                t.Start();

                backgroundWorker1.RunWorkerAsync();
            }
            else
            {
                Close();
            }
        }

        private string getFileName()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Choose a location and filename to save this object flow export";
            sfd.Filter = "Comma Seperated Value File (csv)|*.csv";
            sfd.FileName = "ObjectFlowExport " + DateTime.Now.ToShortDateString();
            sfd.InitialDirectory = Core.Variables.Instance.ExportsPath;
            sfd.DefaultExt = "csv";
            if (sfd.ShowDialog(this) != DialogResult.Cancel)
            {
                return filename = sfd.FileName;
            }
            return filename = "";
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Exporter export = new Exporter(filename);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (backgroundWorker1.CancellationPending)
                return;

            t.Stop();

            System.Diagnostics.Process prc = new System.Diagnostics.Process();
            prc.StartInfo.FileName = Core.strings.GetPath(filename);
            prc.Start();
            Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ObjectFlowExportInterface_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                if (MessageBox.Show(this, "An export is currently processing. Would you like to cancel it and close this form?", "Cancel export", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    backgroundWorker1.CancelAsync();
                }
                else
                    e.Cancel = true;
            }
        }
    }
}