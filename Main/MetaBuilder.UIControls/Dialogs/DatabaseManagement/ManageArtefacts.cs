using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using System.Data.SqlClient;
using MetaBuilder.Meta;

namespace MetaBuilder.UIControls.Dialogs.DatabaseManagement
{
    public partial class ManageArtefacts : Form
    {
        public ManageArtefacts()
        {
            InitializeComponent();
        }

        b.TList<b.AssociationType> associationTypes;

        private void PopulateAssociationTypeDropdown()
        {
            associationTypes = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.AssociationTypeProvider.GetAll();
            associationTypes.Insert(0, new b.AssociationType());
            listAssociationType.DataSource = associationTypes;
            listAssociationType.DisplayMember = "Name";
            listAssociationType.ValueMember = "pkid";

            b.TList<b.Class> classes1 = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassProvider.GetAll();
            b.TList<b.Class> classes2= classes1.Copy();
            b.TList<b.Class> classes3 = classes1.Copy();
            classes1.Insert(0, new b.Class());
            classes2.Insert(0, new b.Class());
            classes3.Insert(0, new b.Class());
            
            listArtefactClass.DataSource = classes1;
            listFromClass.DataSource = classes2;
            listToClass.DataSource = classes3;

            listArtefactClass.DisplayMember = listFromClass.DisplayMember = listToClass.DisplayMember = "Name";
            listArtefactClass.ValueMember = listFromClass.ValueMember = listToClass.ValueMember = "Name";
        }

        private void ManageArtefacts_Load(object sender, EventArgs e)
        {
            PopulateAssociationTypeDropdown();
        }


        public class ArtefactGridDataItem
        {
            public ArtefactGridDataItem(int caid, MetaBase mbf, MetaBase mbt, MetaBase mba, string associationtype)
            {
                this.CAID = caid;
                this.mbFrom = mbf;
                this.mbTo = mbt;
                this.mbArt = mba;
                this.associationtype = associationtype;
            }

            private string associationtype;

            public string AssociationType
            {
                get { return associationtype; }
            }
            private int CAID;
            private MetaBase mbFrom;
            private MetaBase mbTo;
            private MetaBase mbArt;

            public int ArtPKID
            {
                get { return mbArt.pkid; }
            }
            public string ArtMachine
            {
                get { return mbArt.MachineName; }
            }
            public string ArtClass
            {
                get { return mbArt.Class; }
            }
            public string Art
            {
                get { return mbArt.ToString(); }
            }

            public string FromMB
            {
                get { return mbFrom.ToString(); }
            }

            public string ToMB
            {
                get { return mbTo.ToString(); }
            }

            public string FromClass
            {
                get { return mbFrom.Class; }
            }
            public string ToClass
            {
                get { return mbTo.Class; }
            }

            public int FromPKID
            {
                get { return mbFrom.pkid; }
            }
            public int ToPKID
            {
                get { return mbTo.pkid; }
            }

            public string FromMachine
            {
                get { return mbFrom.MachineName; }
            }
            public string ToMachine
            {
                get { return mbTo.MachineName; }
            }
        }

        private void btnApplyFilter_Click(object sender, EventArgs e)
        {
             
            StringBuilder sbQuery = new StringBuilder();
          sbQuery.Append("SELECT     dbo.Artifact.CAid, dbo.AssociationType.Name AS AssociationType, dbo.ClassAssociation.ParentClass AS FromClass, dbo.ClassAssociation.ChildClass AS ToClass, ");
          sbQuery.Append("A.Class AS ArtClass, A.pkid AS ArtPKID, A.Machine AS ArtMachine, F.pkid AS FromPKID, F.Machine AS FromMachine, T.pkid AS ToPKID, T.Machine AS ToMachine ");
          sbQuery.Append("FROM         dbo.Artifact INNER JOIN ");
          sbQuery.Append("dbo.ClassAssociation ON dbo.Artifact.CAid = dbo.ClassAssociation.CAid INNER JOIN ");
          sbQuery.Append("dbo.MetaObject AS A ON dbo.Artifact.ArtifactObjectID = A.pkid AND dbo.Artifact.ArtefactMachine = A.Machine INNER JOIN ");
          sbQuery.Append("dbo.ObjectAssociation ON dbo.Artifact.CAid = dbo.ObjectAssociation.CAid AND dbo.Artifact.ObjectID = dbo.ObjectAssociation.ObjectID AND  ");
          sbQuery.Append("dbo.Artifact.ChildObjectID = dbo.ObjectAssociation.ChildObjectID AND dbo.Artifact.ObjectMachine = dbo.ObjectAssociation.ObjectMachine AND  ");
          sbQuery.Append("dbo.Artifact.ChildObjectMachine = dbo.ObjectAssociation.ChildObjectMachine INNER JOIN ");
          sbQuery.Append("dbo.MetaObject AS F ON dbo.ObjectAssociation.ObjectID = F.pkid AND dbo.ObjectAssociation.ObjectMachine = F.Machine INNER JOIN ");
          sbQuery.Append("dbo.MetaObject AS T ON dbo.ObjectAssociation.ChildObjectID = T.pkid AND dbo.ObjectAssociation.ChildObjectMachine = T.Machine INNER JOIN ");
          sbQuery.Append("dbo.AssociationType ON dbo.ClassAssociation.AssociationTypeID = dbo.AssociationType.pkid WHERE 1 = 1 ");

            b.Class cFrom = listFromClass.SelectedItem as b.Class;
            b.Class cTo = listToClass.SelectedItem as b.Class;
            b.Class cArt = listArtefactClass.SelectedItem as b.Class;
            b.AssociationType type = listAssociationType.SelectedItem as b.AssociationType;
          
            if (cFrom.Name != null)
            {
                sbQuery.Append(" AND F.Class = '" + cFrom.Name + "'");
            }

            if (cTo.Name != null)
            {
                sbQuery.Append(" AND T.Class = '" + cTo.Name + "'");
            }

            if (cArt.Name != null)
            {
                sbQuery.Append(" AND A.Class='" + cArt.Name + "'");
            }
            if (type != null)
            {
                if (type.pkid > 0)
                {
                    sbQuery.Append(" AND  dbo.AssociationType.pkid = " + type.pkid.ToString());
                }
            }

            SqlCommand cmd = new SqlCommand("", new SqlConnection(Core.Variables.Instance.ConnectionString));
            cmd.CommandText = sbQuery.ToString();
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter dap = new SqlDataAdapter();
            dap.SelectCommand = cmd;
            DataSet dsResults = new DataSet();
            dap.Fill(dsResults, "Artefacts");

            Console.WriteLine(dsResults.Tables[0].Rows.Count.ToString());

            Dictionary<string, List<MetaKey>> itemsToRetrieve = new Dictionary<string, List<MetaKey>>();

            foreach (DataRowView drv in dsResults.Tables[0].DefaultView)
            {
                #region Get Values from Dataset
                string strcFrom = drv["FromClass"].ToString();
                int pkidFrom = int.Parse(drv["FromPKID"].ToString());
                string machineFrom = drv["FromMachine"].ToString();

                string strcTo = drv["ToClass"].ToString();
                int pkidTo = int.Parse(drv["ToPKID"].ToString());
                string machineTo = drv["ToMachine"].ToString();
                

                string strcArt = drv["ArtClass"].ToString();
                int pkidArt = int.Parse(drv["ArtPKID"].ToString());
                string machineArt = drv["ArtMachine"].ToString();
                #endregion

                #region Setup Keys
                List<MetaKey> keys = new List<MetaKey>();
                if (!itemsToRetrieve.ContainsKey(strcFrom))
                {
                    itemsToRetrieve.Add(strcFrom, keys);
                }
                else
                {
                    keys = itemsToRetrieve[strcFrom];
                }

                MetaKey mkey = new MetaKey();
                mkey.PKID = pkidFrom;
                mkey.Machine = machineFrom;
                keys.Add(mkey);

                keys = new List<MetaKey>();
                if (!itemsToRetrieve.ContainsKey(strcTo))
                {
                    itemsToRetrieve.Add(strcTo, keys);
                }
                else
                {
                    keys = itemsToRetrieve[strcTo];
                }

                mkey = new MetaKey();
                mkey.PKID = pkidTo;
                mkey.Machine = machineTo;
                keys.Add(mkey);

                keys = new List<MetaKey>();
                if (!itemsToRetrieve.ContainsKey(strcArt))
                {
                    itemsToRetrieve.Add(strcArt, keys);
                }
                else
                {
                    keys = itemsToRetrieve[strcArt];
                }

                mkey = new MetaKey();
                mkey.PKID = pkidArt;
                mkey.Machine = machineArt;
                keys.Add(mkey);
                #endregion
            }

            List<MetaBase> loaded = Loader.GetFromProvider(itemsToRetrieve, false);
            Dictionary<string, MetaBase> dictLoaded = new Dictionary<string, MetaBase>();
            foreach (MetaBase mbl in loaded)
            {
                string k = mbl.pkid.ToString() + mbl.MachineName;
                if (!(dictLoaded.ContainsKey(k)))
                {
                    dictLoaded.Add(k, mbl);
                }
            }
            List<ArtefactGridDataItem> items = new List<ArtefactGridDataItem>();
            tableModel1.Rows.Clear();
            foreach (DataRowView drv in dsResults.Tables[0].DefaultView)
            {
                #region Get Values from Dataset
                string kFrom = drv["FromPKID"].ToString() + drv["FromMachine"].ToString();
                string kTo = drv["ToPKID"].ToString() + drv["ToMachine"].ToString();
                string kArt = drv["ArtPKID"].ToString() + drv["ArtMachine"].ToString();
                int CAID = int.Parse(drv["CAID"].ToString());
                MetaBase mbFrom = dictLoaded[kFrom];
                MetaBase mbTo = dictLoaded[kTo];
                MetaBase mbArt = dictLoaded[kArt];


                ArtefactGridDataItem item = new ArtefactGridDataItem(CAID,mbFrom,mbTo,mbArt,drv["AssociationType"].ToString());
                XPTable.Models.Row r = new XPTable.Models.Row();
            
                r.Cells.Add(new XPTable.Models.Cell(item.FromMB.ToString()));
                r.Cells.Add(new XPTable.Models.Cell(item.ToMB.ToString()));
                r.Cells.Add(new XPTable.Models.Cell(item.AssociationType));
                r.Cells.Add(new XPTable.Models.Cell(item.Art.ToString()));
                r.Cells.Add(new XPTable.Models.Cell(item.ArtClass));
                r.Cells.Add(new XPTable.Models.Cell(false));
                r.Cells.Add(new XPTable.Models.Cell(item.FromPKID));
                r.Cells.Add(new XPTable.Models.Cell(item.FromMachine));
                r.Cells.Add(new XPTable.Models.Cell(item.FromClass));
                r.Cells.Add(new XPTable.Models.Cell(item.ToPKID));
                r.Cells.Add(new XPTable.Models.Cell(item.ToMachine));
                r.Cells.Add(new XPTable.Models.Cell(item.ToClass));
                r.Tag = item;
                this.tableModel1.Rows.Add(r);
                items.Add(item);
#endregion
            }


            table1.Enabled = (tableModel1.Rows.Count > 0);


        }

        private void btnApplyChanges_Click(object sender, EventArgs e)
        {


        }
    }
}