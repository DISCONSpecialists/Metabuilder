using System;
using System.Diagnostics;
using System.Windows.Forms;
using MetaBuilder.BusinessFacade.Exports;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UIControls.Common
{
    public partial class TempMappingExporter : Form
    {

        #region Fields (1)

        private b.Workspace workspace;

        #endregion Fields

        #region Constructors (1)

        public TempMappingExporter()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties (1)

        public b.Workspace Workspace
        {
            get { return workspace; }
            set { workspace = value; }
        }

        #endregion Properties

        #region Methods (3)

        // Private Methods (3) 

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExcelMappings excelMappings = new ExcelMappings();
            excelMappings.Workspace = this.Workspace;

            if (comboRelationshipType.SelectedIndex >= 0)
            {
                AssociationType assocType = comboRelationshipType.SelectedItem as AssociationType;
                string filename = excelMappings.DoExport(classDropdown1.SelectedClass, classDropdown2.SelectedClass, assocType.pkid);
                if (filename.Length > 0)
                    Process.Start(filename);
                Close();
            }
            else
            {
                if (MessageBox.Show(this, "Would you like to export all association types for the selected classes?", "Association Type Not Selected", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string filename = excelMappings.DoExport(classDropdown1.SelectedClass, classDropdown2.SelectedClass);
                    if (filename.Length > 0)
                        Process.Start(filename);
                    Close();
                }
            }
        }

        private void TempMappingExporter_Load(object sender, EventArgs e)
        {
            classDropdown1.Init();
            classDropdown2.Init();
            TList<AssociationType> assocTypes = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.AssociationTypeProvider.GetAll();
            assocTypes.Sort("Name");
            foreach (AssociationType assocType in assocTypes)
            {
                comboRelationshipType.Items.Add(assocType);
            }
            comboRelationshipType.DisplayMember = "Name";
            comboRelationshipType.ValueMember = "pkid";
        }

        #endregion Methods

    }
}