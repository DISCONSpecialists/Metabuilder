using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.MetaControls;

namespace MetaBuilder.UIControls.Dialogs.DatabaseManagement
{
    public partial class AddAssociationsToRecycler : Form
    {

        #region Constructors (1)
        bool Server = false;
        private string Provider { get { return Server ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider; } }
        public AddAssociationsToRecycler(bool server)
        {
            Server = server;
            InitializeComponent();
            FormClosing += new FormClosingEventHandler(AddAssociationsToRecycler_FormClosing);
            this.associationLocatorControl1.Dock = DockStyle.Fill;
            //this.panel1.Dock = DockStyle.Fill;
        }

        private void AddAssociationsToRecycler_FormClosing(object sender, FormClosingEventArgs e)
        {
            associationLocatorControl1.CancelThread();
        }

        #endregion Constructors

        #region Methods (3)


        // Private Methods (3) 

        private void AddAssociationsToRecycler_Load(object sender, EventArgs e)
        {
            associationLocatorControl1.ExcludeStatuses.Add(VCStatusList.Obsolete);
            associationLocatorControl1.ExcludeStatuses.Add(VCStatusList.MarkedForDelete);
            if (!Server)
            {
                associationLocatorControl1.ExcludeStatuses.Add(VCStatusList.CheckedOutRead);
                associationLocatorControl1.ExcludeStatuses.Add(VCStatusList.CheckedIn);
            }
            associationLocatorControl1.AllowMultipleSelection = true;
            associationLocatorControl1.IncludeStatusCombo = true;
            associationLocatorControl1.DoInitialising();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<ObjectAssociationKey, ObjectAssociationInclObj> kvp in associationLocatorControl1.SelectedAssociations)
            {
                ObjectAssociation oa = DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.Get(kvp.Key);
                if (oa != null)
                {
                    oa.VCStatusID = (int)VCStatusList.MarkedForDelete;
                    DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.Save(oa);
                    associationLocatorControl1.RemoveRow(kvp.Key);

                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        #endregion Methods

    }
}