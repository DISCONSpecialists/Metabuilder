using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.MetaControls;

namespace MetaBuilder.UIControls.Dialogs.DatabaseManagement
{
    public partial class ManageMarkedForDeleteAssociations : Form
    {

        #region Constructors (1)

        bool Server = false;
        private string Provider { get { return Server ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider; } }
        public ManageMarkedForDeleteAssociations(bool server)
        {
            Server = server;
            InitializeComponent();
            FormClosing += new FormClosingEventHandler(ManageMarkedForDeleteAssociations_FormClosing);
        }

        private void ManageMarkedForDeleteAssociations_FormClosing(object sender, FormClosingEventArgs e)
        {
            associationLocatorControl1.CancelThread();
        }

        #endregion Constructors

        #region Methods (3)


        // Private Methods (3) 

        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<ObjectAssociationKey, ObjectAssociationInclObj> kvp in associationLocatorControl1.SelectedAssociations)
            {
                associationLocatorControl1.RemoveRow(kvp.Key);

                ObjectAssociation oa = DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.Get(kvp.Key);
                TList<GraphFileAssociation> gfassocs = DataRepository.Connections[Provider].Provider.GraphFileAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(kvp.Key.CAid, kvp.Key.ObjectID, kvp.Key.ChildObjectID, kvp.Key.ObjectMachine, kvp.Key.ChildObjectMachine);
                DataRepository.Connections[Provider].Provider.GraphFileAssociationProvider.Delete(gfassocs);
                if (oa != null)
                {
                    try
                    {
                        DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.Delete(oa);
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<ObjectAssociationKey, ObjectAssociationInclObj> kvp in associationLocatorControl1.SelectedAssociations)
            {
                try
                {
                    ObjectAssociation oa = DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.Get(kvp.Key);
                    if (oa != null)
                    {
                        if (oa.VCMachineID == null)
                            oa.VCStatusID = (int)VCStatusList.None;
                        else
                            oa.VCStatusID = (int)VCStatusList.CheckedOut;

                        DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.Save(oa);
                        associationLocatorControl1.RemoveRow(kvp.Key);
                    }
                }
                catch
                {
                }
            }
        }

        private void ManageMarkedForDeleteObjects_Load(object sender, EventArgs e)
        {
            if (Server)
                associationLocatorControl1.ExcludeVCItems = false;
            else
                associationLocatorControl1.ExcludeVCItems = true;
            associationLocatorControl1.LimitToStatus = 8;
            associationLocatorControl1.DoInitialising();

        }

        #endregion Methods

    }
}