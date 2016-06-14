#region Copyright © 2007 - DISCON Specialists
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: Structuretree.cs
//
#endregion

using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Threading;
using System.Windows.Forms;
using MetaBuilder.Meta;

namespace MetaBuilder.UIControls.Tree
{
    /// <summary>
    /// MetaBaseTree with a few small differences:
    /// The tree has checkboxes, and carries ObjectIDs of checked nodes in an Int[] array.
    /// </summary>
    public delegate void DelegateSetHashtable(Hashtable hash);

    public class StructureTree : MetaBaseTree
    {

		#region Fields (7) 

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private Container components = null;
        private ObjectIDentifier[] items;
        public DelegateSetHashtable m_DelegateSetHashtable;
        private Thread m_WorkerThread;
        public Hashtable MetaBaseObjects;
        private string rootclass;
        private Hashtable StructureNodes;

		#endregion Fields 

		#region Constructors (1) 

        public StructureTree()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            MetaDepth = int.Parse("0" + ConfigurationManager.AppSettings["MetaDepth"]);
            m_DelegateAddNode = new DelegateAddNode(AddNode);
            m_DelegateSetHashtable = new DelegateSetHashtable(SetHashtable);
            MetaBaseObjects = new Hashtable();
            hashCaptionNodes = new Hashtable();
        }

		#endregion Constructors 

		#region Properties (2) 

        public ObjectIDentifier[] Items
        {
            get { return items; }
            set { items = value; }
        }

        public string RootClass
        {
            get { return rootclass; }
            set { rootclass = value; }
        }

		#endregion Properties 

		#region Delegates and Events (1) 


		// Events (1) 

        [Description("Raised when the control has finished loading.")]
        [Category("Behaviour")]
        [Browsable(true)]
        public event EventHandler FinishedLoading;


		#endregion Delegates and Events 

		#region Methods (8) 


		// Public Methods (6) 

        public override void AddNode(TreeNode n)
        {
            Nodes.Add(n);
        }

        public override void BuildShallowObjectNode(ObjectNode node, int ImageIndex)
        {
            if (StructureNodes == null)
                StructureNodes = new Hashtable();
            base.BuildShallowObjectNode(node, ImageIndex);
            MetaBase mb = (MetaBase) node.Tag;
            if (!MetaBaseObjects.ContainsKey(mb.pkid))
            {
                MetaBaseObjects.Add(mb.pkid, mb);
                StructureNodes.Add(mb.pkid, new ArrayList());
                ((ArrayList) StructureNodes[mb.pkid]).Add(node);
            }
        }

        public override void BuildTree()
        {
            nodeCount = 0;
            cachedNodesRetrieved = 0;
            Nodes.Clear();
            dsAssociationData = null;
            CheckBoxes = true;
            //this.LoadTree();
            //LoadTree();
            m_WorkerThread = new Thread(new ThreadStart(LoadTree));
            m_WorkerThread.Name = "BuildTree Thread";
            m_WorkerThread.Start();
            StructureNodes = new Hashtable();
        }

        public override void LoadTree()
        {
            base.InitColors();
            MetaBaseObjects = new Hashtable();
            for (int i = 0; i < items.Length; i++)
            {
                IMetaBase currentobject = Loader.GetByID(RootClass,items[i].PKID, items[i].Machine);
                    // MOET NIE INTS stuur nie.
                currentobject.LoadEmbeddedObjects(1, 3);
                /*System.Diagnostics.Debug.WriteLine("");
                System.Diagnostics.Debug.WriteLine("");
                System.Diagnostics.Debug.WriteLine("currentobject" + currentobject.ToString());
                System.Diagnostics.Debug.WriteLine("");
                System.Diagnostics.Debug.WriteLine("");*/
                ObjectNode tnRoot = new ObjectNode(currentobject.ToString());
                tnRoot.Tag = currentobject;
                BuildShallowObjectNode(tnRoot, 0);
                try
                {
                    Thread.Sleep(500);
                    Invoke(m_DelegateAddNode, new Object[] {tnRoot});
                }
                catch
                {
                    try
                    {
                        Thread.Sleep(500);
                        Invoke(m_DelegateAddNode, new Object[] {tnRoot});
                    }
                    catch
                    {
                        try
                        {
                            Thread.Sleep(500);
                            Invoke(m_DelegateAddNode, new Object[] {tnRoot});
                        }
                        catch
                        {
                        }
                    }
                }
            }
            Invoke(m_DelegateSetHashtable, new Object[] {MetaBaseObjects});
            OnFinishedLoading(new EventArgs());
        }

        public virtual void OnFinishedLoading(EventArgs e)
        {
            if (FinishedLoading != null)
            {
                FinishedLoading(this, e);
            }
        }

        /// <summary>
        /// Checkes or unchecks all ObjectNodes where an object exists according to the given node's state
        /// </summary>
        /// <param name="ObjectID"></param>
        /// <param name="tnRootInstance"></param>
        public void RolloutNodeCheckedStatus(int ObjectID, TreeNode tnRootInstance)
        {
            if (StructureNodes[ObjectID] != null)
            {
                ArrayList nodeInstances = (ArrayList) StructureNodes[ObjectID];
                foreach (TreeNode tn in nodeInstances)
                {
                    if (tn != tnRootInstance)
                    {
                        object o = tn.Tag;
                        tn.Tag = null;
                        tn.Checked = tnRootInstance.Checked;
                        tn.Tag = o;
                    }
                }
            }
        }



		// Protected Methods (1) 

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }



		// Private Methods (1) 

        private void SetHashtable(Hashtable hash)
        {
            MetaBaseObjects = hash;
        }


		#endregion Methods 


        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion
    }
}