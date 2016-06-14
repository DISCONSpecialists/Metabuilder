using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MetaBuilder.Graphing.Containers;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DocHandlers.ImportExport
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
        }

        public NormalDiagram MyDiagram;
        #region Imports
        private void lnkImportToDiagramText_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void lnkImportToDatabaseText_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void lnkImportToDatabaseExcel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
        #endregion

        #region Exports

        private void lnkExportFromDiagramTextIndented_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void lnkExportFromDiagramTextNumbered_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void lnkExportFromDiagramTextIndentedNumbered_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void lnkExportFromDatabaseExcel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void lnkExportFromDatabaseTextIndented_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void lnkExportFromDatabaseTextNumbered_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void lnkExportFromDatabaseTextIndentedNumbered_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
        #endregion

        #region Threading
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }
        #endregion

        #region Prompting

        #endregion

        #region Specifications
        
        public class Specification
        {
            public Specification()
            {
                
            }
            public SpecificationDirection Direction;
            public SpecificationFileType FileType;
            public SpecificationOptions Options;

        }
        public enum SpecificationDirection { Import, Export };
        public enum SpecificationFileType { Text, Excel };
        public enum SpecificationOptions { Indented, Numbered, IncludeData };


        #endregion


    }
}