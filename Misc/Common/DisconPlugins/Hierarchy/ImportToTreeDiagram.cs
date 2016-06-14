using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Tools;
using MetaBuilder.Meta;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.PluginSDK;
using MetaBuilder.UIControls.GraphingUI;
using Northwoods.Go;
using Northwoods.Go.Layout;

namespace DisconPlugins.Hierarchy
{
    public class ImportToTreeDiagramPlugin : IPlugin
    {

        #region Properties (1)

        public string Name
        {
            get { return "Import To Tree Diagram"; }
        }

        #endregion Properties

        #region Methods (1)

        // Public Methods (1) 

        public bool PerformAction(IPluginContext context)
        {
            try
            {
                ImportToTreeDiagram importer = new ImportToTreeDiagram();
                importer.Import(context);
                return false;
            }
            catch
            {
                return true;
            }
        }

        #endregion Methods
    }


    public class ImportToTreeDiagram
    {

        #region Fields (2)

        private MetaBuilder.UIControls.Dialogs.QueryForTextImport chooseClass;
        private IPluginContext myContext;

        #endregion Fields

        #region Constructors (1)

        public ImportToTreeDiagram()
        {

        }

        #endregion Constructors

        #region Methods (2)

        // Public Methods (1) 

        public void Import(IPluginContext context)
        {
            //if (context.CurrentStencil == null)
            //{
            //    MessageBox.Show(this,"Please open an applicable stencil first", "Open Stencil", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            myContext = context;

            DoImport();
        }

        // Private Methods (1) 

        private void DoImport()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Text Files|*.txt";
            openDialog.Multiselect = false;
            DialogResult resFile = openDialog.ShowDialog();
            if (resFile == DialogResult.OK)
            {
                chooseClass = new MetaBuilder.UIControls.Dialogs.QueryForTextImport();
                DialogResult result = chooseClass.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string ClassName = chooseClass.MyClass;
                    string defaultField = chooseClass.MyField;
                    PluginTextImportSpecification spec = new PluginTextImportSpecification();
                    spec.ClassName = ClassName;
                    spec.SourceFile = openDialog.FileName;
                    spec.DefaultField = defaultField;

                    TextFile tfile = new TextFile();
                    tfile.Specification = spec;

                    GraphNode myNodeToUse = null;
                    GraphNode newNode = (GraphNode)MetaBuilder.Core.Variables.Instance.ReturnShape(ClassName);
                    if (newNode != null)
                        myNodeToUse = (GraphNode)newNode.Copy();

                    // find the node to use
                    if (myNodeToUse == null && myContext != null && myContext.CurrentStencil != null)
                    {
                        foreach (GoObject o in myContext.CurrentStencil.Document)
                        {
                            if (o is GraphNode)
                            {
                                GraphNode node = o as GraphNode;
                                if (node.BindingInfo.BindingClass == ClassName)
                                {
                                    myNodeToUse = node.Copy() as GraphNode;
                                }
                            }
                        }
                    }

                    if (myNodeToUse == null)
                    {
                        DockingForm.DockForm.m_paletteDocker.stripButtonOpen_Click(this, EventArgs.Empty);
                        //MessageBox.Show(this,"Please open an applicable stencil first", "Open Stencil", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        myContext.CurrentGraphView.StartTransaction();
                        tfile.Import(myContext.CurrentGraphView.Document, myNodeToUse, true);

                        RootOnlyTreeLayout layout = new RootOnlyTreeLayout();
                        layout.Document = myContext.CurrentGraphView.Document;
                        layout.Alignment = GoLayoutTreeAlignment.Start;
                        layout.Arrangement = GoLayoutTreeArrangement.FixedRoots;
                        layout.Style = GoLayoutTreeStyle.LastParents;
                        layout.PerformLayout();

                        myContext.CurrentGraphView.FinishTransaction("Import to Tree Diagram");
                    }
                }
            }
        }

        #endregion Methods

    }
}