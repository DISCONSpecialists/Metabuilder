using System.Windows.Forms;

namespace MetaBuilder.UIControls.Dialogs
{
    public partial class AssociationManager : Form
    {

		#region Constructors (1) 
        bool Server = false;
        public AssociationManager(bool server)
        {
            InitializeComponent();
            this.associationLocatorControl1.AllowMultipleSelection = true;
            associationLocatorControl1.IncludeStatusCombo = true;
            this.associationLocatorControl1.DoInitialising();
        }

		#endregion Constructors 

    }
}