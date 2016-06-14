using System;
using System.Reflection;
using System.Windows.Forms;
using MetaBuilder.Meta;

namespace MetaBuilder.UIControls.Dialogs
{
    public partial class QueryForTextImport : Form
    {

        #region Fields (1)

        private bool isExport;

        #endregion Fields

        #region Constructors (1)

        public QueryForTextImport()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties (3)

        public bool IsExport
        {
            get { return isExport; }
            set { isExport = value; }
        }

        public string MyClass
        {
            get { return classDropdown1.SelectedClass; }
        }

        public string MyField
        {
            get { return comboDefaultField.SelectedItem.ToString(); }
        }

        #endregion Properties

        #region Methods (5)

        // Private Methods (5) 

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (MyClass != null)
            {
                if (MyClass != string.Empty)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            else
            {
                errorProvider1.SetError(classDropdown1, "Select a value");
            }
        }

        private void classDropdown1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!classDropdown1.Initializing)
            {
                UpdateFields();
            }
        }

        private void QueryForTextImport_Load(object sender, EventArgs e)
        {
            classDropdown1.Init();
            UpdateFields();
            if (isExport)
            {
                label1.Text = label1.Text.Replace("Import", "Export");
                label2.Text = label2.Text.Replace("Import", "Export");
                Text = Text.Replace("Import", "Export");
            }
        }

        private void UpdateFields()
        {
            string className = classDropdown1.SelectedClass;
            MetaBase mb = Loader.CreateInstance(className);

            //PropertyInfo[] props = mb.GetType().GetProperties();
            comboDefaultField.Items.Clear();
            foreach (PropertyInfo prop in mb.GetMetaPropertyList(false))
            {
                comboDefaultField.Items.Add(prop.Name);
            }
            comboDefaultField.SelectedIndex = 0;
        }

        #endregion Methods

    }
}