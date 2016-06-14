using System;
using System.Diagnostics;
using System.Windows.Forms;
using MetaBuilder.Graphing.Containers;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace MetaBuilder.UIControls.Dialogs
{
    public partial class DebugInfo : Form
    {

		#region Fields (2) 

        private GraphView currentGraphView;
        private Exception innerException;

		#endregion Fields 

		#region Constructors (1) 

        public DebugInfo()
        {
            InitializeComponent();
        }

		#endregion Constructors 

		#region Properties (2) 

        public GraphView CurrentGraphView
        {
            get { return currentGraphView; }
            set { currentGraphView = value; }
        }

        public Exception InnerException
        {
            get { return innerException; }
            set { innerException = value; }
        }

		#endregion Properties 

		#region Methods (1) 


		// Private Methods (1) 

        private void btnOK_Click(object sender, EventArgs e)
        {
            LogEntry logEntry = new LogEntry();
            logEntry.Categories.Clear();
            logEntry.Categories.Add("General");
            logEntry.Priority = 5;
            logEntry.Severity = TraceEventType.Error;
            if (CurrentGraphView != null)
            {
                logEntry.ExtendedProperties.Add("Primary Selection", CurrentGraphView.Selection.Primary);
                logEntry.ExtendedProperties.Add("Filename", CurrentGraphView.Doc.Name);
                logEntry.ExtendedProperties.Add("File Type", CurrentGraphView.Doc.FileType);
                logEntry.ExtendedProperties.Add("Selection Count", CurrentGraphView.Selection.Count);
                logEntry.ExtendedProperties.Add("Total Count", CurrentGraphView.Doc.Count);
            }
            logEntry.Message = txtDebugInfo.Text + Environment.NewLine + Environment.NewLine + innerException.Message +
                               Environment.NewLine + innerException.StackTrace;
            Logger.Write(logEntry);
        }


		#endregion Methods 

    }
}