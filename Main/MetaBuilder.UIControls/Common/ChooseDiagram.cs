using System;
using System.Windows.Forms;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer.OldCode.Diagramming;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UIControls.Common
{
    public partial class ChooseDiagram : Form
    {

		#region Fields (1) 

        private TList<GraphFile> availableFiles;

		#endregion Fields 

		#region Constructors (1) 

        public ChooseDiagram()
        {
            InitializeComponent();
        }

		#endregion Constructors 

		#region Properties (1) 

        public int SelectedFileID
        {
            get
            {
                if (comboDiagrams.SelectedIndex > -1)
                {
                    string sFileName = comboDiagrams.SelectedItem as string;
                    foreach (GraphFile file in availableFiles)
                    {
                        if (file.Name == sFileName)
                            return file.pkid;
                    }
                }
                return -1;
            }
        }

		#endregion Properties 

		#region Methods (2) 


		// Private Methods (2) 

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
        private void ChooseDiagram_Load(object sender, EventArgs e)
        {
            // Populate the list of graphfiles
            TempFileGraphAdapter tfga = new TempFileGraphAdapter();
            availableFiles =
                tfga.GetFilesByWorkspaceTypeIdWorkspaceName(Variables.Instance.CurrentWorkspaceTypeId,
                                                            Variables.Instance.CurrentWorkspaceName,
                                                            (int) FileTypeList.Diagram,false);
            char c = '\\';
            foreach (GraphFile file in availableFiles)
            {
                string[] split = file.Name.Split(c);
                if (split.Length > 1)
                {
                    string filename = "..\\" + split[split.Length - 2] + "\\" + split[split.Length - 1];
                    comboDiagrams.Items.Add(filename);
                }
                else
                {
                    comboDiagrams.Items.Add(file.Name);
                }
            }
        }


		#endregion Methods 

    }
}