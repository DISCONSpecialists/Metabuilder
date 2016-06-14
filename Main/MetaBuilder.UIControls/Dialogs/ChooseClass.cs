using System;
using System.Windows.Forms;
using MetaBuilder.BusinessFacade.Exports;
using MetaBuilder.Core;

namespace MetaBuilder.UIControls.Dialogs
{
    public partial class ChooseClass : Form
    {

        #region Fields (2)

        #endregion Fields

        #region Constructors (1)

        public ChooseClass(string title, string label)
        {
            InitializeComponent();
            FormText = title;
            LabelText = label;
            SetupDropdown();
        }
        public ChooseClass(string title, string label, string allowedClasses)
        {
            InitializeComponent();
            FormText = title;
            LabelText = label;
            SetupDropdown(allowedClasses);
        }

        #endregion Constructors

        #region Properties (5)

        public string FormText
        {
            get { return this.Text; }
            set { this.Text = value; }
        }

        public string LabelText
        {
            get { return this.label1.Text; }
            set { label1.Text = value; }
        }

        public string SelectedClass
        {
            get { return classDropdown1.SelectedClass; }
        }

        #endregion Properties

        #region Methods (3)
        // Public Methods (2) 
        public void SetupDropdown(string allowedClasses)
        {
            if (allowedClasses != "")
                classDropdown1.Init(allowedClasses);
            else
                classDropdown1.Init();
            classDropdown1.SelectedIndex = 0;
        }
        public void SetupDropdown()
        {
            classDropdown1.Init();
            classDropdown1.SelectedIndex = 0;
        }

        private void classDropdown1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnOK_Click(sender, EventArgs.Empty);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                btnOK_Click(this, EventArgs.Empty);
                return;
            }
            base.OnKeyDown(e);
        }
        // Private Methods (1) 

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!classDropdown1.Initializing)
            {
                if (classDropdown1.SelectedClass.Length > 0)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        #endregion Methods

    }
}