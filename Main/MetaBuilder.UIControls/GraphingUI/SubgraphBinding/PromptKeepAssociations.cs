using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MetaBuilder.Graphing.Shapes.Nodes.Containers;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using MetaBuilder.Meta;

namespace MetaBuilder.UIControls.GraphingUI.SubgraphBinding
{
    public partial class PromptKeepAssociations : Form
    {
        public PromptKeepAssociations()
        {
            InitializeComponent();
        }

        public void BindObjectsToPrompt()
        {
            foreach (KeyValuePair<ILinkedContainer,List<MetaBase>> kvp in ObjectsToPrompt)
            {
                if (kvp.Value.Count > 0)
                {
                    if (kvp.Key.MetaObject != null)
                    {
                        
                        foreach (MetaBase mb in kvp.Value)
                        {
                            // Get EMREL
                            foreach (EmbeddedRelationship embrel in kvp.Key.ObjectRelationships)
                            {
                                if (embrel.MyMetaObject == mb)
                                {
                                    XPTable.Models.Row r = new XPTable.Models.Row();
                                    string containerString = kvp.Key.MetaObject.ToString();
                                    string childString = mb.ToString();
                                    if (containerString == string.Empty)
                                    {
                                        containerString = "Null (" + kvp.Key.MetaObject._ClassName + ")";
                                    }

                                    if (childString == string.Empty)
                                    {
                                        childString = "Null (" + mb._ClassName + ")";
                                    }
                                    r.Cells.Add(new XPTable.Models.Cell(containerString));
                                    r.Cells.Add(new XPTable.Models.Cell(childString));
                                    r.Cells.Add(new XPTable.Models.Cell(embrel.MyAssociation.Caption));
                                    r.Cells.Add(new XPTable.Models.Cell(1));
                                    
                                    r.Tag = embrel;
                                    table2.TableModel.Rows.Add(r);
                                }
                            }
                        }
                    }
                }
            }
        }

        private Dictionary<ILinkedContainer,List<MetaBase>> objectsToPrompt;
        public Dictionary<ILinkedContainer,List<MetaBase>> ObjectsToPrompt
        {
            get { return objectsToPrompt; }
            set { objectsToPrompt = value; }
        }


	
        private void btnOK_Click(object sender, EventArgs e)
        {
            foreach (XPTable.Models.Row r in table2.TableModel.Rows)
            {
                if (!(r.Cells[3].Checked))
                {
                    Console.WriteLine("Remove");

                    foreach (KeyValuePair<ILinkedContainer, List<MetaBase>> kvp in this.ObjectsToPrompt)
                    {
                        if (kvp.Key.MetaObject.ToString() == r.Cells[0].Text)
                        {
                            EmbeddedRelationship embrel = r.Tag as EmbeddedRelationship;
                            kvp.Key.ObjectRelationships.Remove(r.Tag as EmbeddedRelationship);
                            try
                            {
                                b.ObjectAssociationKey oakey = new MetaBuilder.BusinessLogic.ObjectAssociationKey();
                                oakey.CAid = embrel.MyAssociation.CAid;
                                oakey.ChildObjectID = embrel.MyMetaObject.pkid;
                                oakey.ChildObjectMachine = embrel.MyMetaObject.MachineName;
                                oakey.ObjectID = kvp.Key.MetaObject.pkid;
                                oakey.ObjectMachine = kvp.Key.MetaObject.MachineName;

                                b.ObjectAssociation oaActual = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Get(oakey);

                                if (VCStatusTool.UserHasControl(oaActual))
                                {
                                    d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Delete(oaActual);
                                }
                            }
                            catch { }
                        }
                    }

                }

            }
            this.Close();
        }
    }
}