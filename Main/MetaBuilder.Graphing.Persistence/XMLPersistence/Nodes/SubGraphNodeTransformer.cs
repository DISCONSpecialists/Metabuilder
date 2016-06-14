using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Behaviours.Observers;
using MetaBuilder.Graphing.Shapes.Nodes;
using MetaBuilder.Meta;
using Northwoods.Go;
using Northwoods.Go.Xml;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using MetaBuilder.Graphing.Shapes.Nodes.Containers;
using System.Drawing;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Nodes
{
    public class SubGraphNodeTransformer : BaseGoObjectTransformer
    {

        #region Fields (4)

        private readonly List<MetaBase> AlreadyLoadedObjects;
        //private string anchors;
        private List<QuickAndDirtyDefaultClassAssociationDefinition> defaultClassAssocDefinitions;
        //private List<QuickAndDirtyObjectRelationshipDefinition> definitions;

        #endregion Fields

        #region Constructors (1)

        public SubGraphNodeTransformer()
            : base()
        {
            TransformerType = typeof(SubgraphNode);
            ElementName = "subgnode";
            IdAttributeUsedForSharedObjects = true;
            BodyConsumesChildElements = true;
            AlreadyLoadedObjects = new List<MetaBase>();
        }

        #endregion Constructors

        #region Methods (7)

        // Public Methods (7) 

        public override object Allocate()
        {
            SubgraphNode node = new SubgraphNode();
            return node;
        }
        const char definitionSplitChar = ':';
        const char itemSplitChar = '|';
        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);

            SubgraphNode node = obj as SubgraphNode;
            node.Initializing = true;

            node.IsStencil = BooleanAttr("stencil", false);
            //node.Bounds = RectangleFAttr("xy", new RectangleF());
            node.BoundsBeforeLoad = RectangleFAttr("xy", new RectangleF());

            node.Selectable = true;
            string boundclass = StringAttr("cls", string.Empty);
            defaultClassAssocDefinitions = new List<QuickAndDirtyDefaultClassAssociationDefinition>();
            string sDefaultClassAssociations = StringAttr("DefaultClassAssociations", "");
            if (sDefaultClassAssociations.Length > 0)
            {
                string[] items = sDefaultClassAssociations.Split(definitionSplitChar);
                foreach (string sItem in items)
                {
                    if (sItem.Length > 0)
                    {
                        string[] itemDef = sItem.Split(itemSplitChar);
                        QuickAndDirtyDefaultClassAssociationDefinition defaultClassAssociation = new QuickAndDirtyDefaultClassAssociationDefinition();
                        defaultClassAssociation.CAid = int.Parse(itemDef[0]);
                        defaultClassAssociation.ChildClass = itemDef[1];
                        defaultClassAssociation.AssociationTypeID = int.Parse(itemDef[2]);
                        defaultClassAssociation.Caption = itemDef[3];
                        defaultClassAssocDefinitions.Add(defaultClassAssociation);
                    }
                }
            }

            //string[] definitions = boundLabels.Split(defSplitChar);
            node.BindingInfo = new BindingInfo();
            node.BindingInfo.BindingClass = boundclass;

            if (boundclass.Length > 0)
            {
                node.Label.Editable = false;
                int pkid = int.Parse(StringAttr("moPKID", "0"));
                string Mach = StringAttr("moMachine", "");
                if (pkid > 0 && d.DataRepository.Connections.ContainsKey(Core.Variables.Instance.ClientProvider))
                {
                    node.MetaObject = Loader.GetByID(boundclass, pkid, Mach);
                    //23 January 2013
                    //if (node.MetaObject == null)
                    //{
                    //    node.MetaObject = Loader.CreateInstance(boundclass);
                    //    //Happens when you transfer the diagram from A=B but B wont have objects so they wont load and the object is null
                    //    //So we have to manually create the object from the saved data
                    //    node.MetaObject.pkid = pkid;
                    //    node.MetaObject.MachineName = Mach;
                    //    //the workspace only exists on A but we can try to set it.
                    //    //default to sandbox
                    //    try
                    //    {
                    //        int workspaceID = int.Parse(StringAttr("moWorkspaceType", "1"));
                    //        string workspaceName = StringAttr("moWorkspace", "Sandbox");
                    //        node.MetaObject.WorkspaceName = workspaceName;
                    //        node.MetaObject.WorkspaceTypeId = workspaceID;
                    //    }
                    //    catch
                    //    {
                    //        node.MetaObject.WorkspaceName = "SandBox";
                    //        node.MetaObject.WorkspaceTypeId = 1;
                    //    }
                    //}
                }
                if (node.MetaObject != null)
                {
                    if (node.MetaObject._ClassName == "CSF")
                    {
                        if (node.Text != "")
                        {
                            if (node.MetaObject.ToString() == "")
                            {
                                node.MetaObject.Set("Number", node.Text);
                            }
                        }
                    }
                }
            }

            node.CollapsedTopLeftMargin = SizeFAttr("CollapsedTopLeftMargin", node.CollapsedTopLeftMargin);
            node.CollapsedBottomRightMargin = SizeFAttr("CollapsedBottomRightMargin", node.CollapsedTopLeftMargin);

            node.Label.Deletable = false;
            node.Label.TextColor = ColorAttr("lblColour", Color.Black);
            node.Label.FontSize = FloatAttr("sgfontSz", 10);
            node.Label.Bold = BooleanAttr("sgbold", false);
            node.Label.Italic = BooleanAttr("sgitalic", false);
            node.Label.FamilyName = StringAttr("sgfont", "Tahoma");
            node.Label.Multiline = BooleanAttr("sgmulti", false);
            node.Label.StrikeThrough = BooleanAttr("sgstrike", false);
            Type tStringTrimming = typeof(StringTrimming);
            node.Label.StringTrimming = (StringTrimming)Enum.Parse(tStringTrimming, StringAttr("sgtrim", StringTrimming.None.ToString()));
            node.Label.Underline = BooleanAttr("sgul", false);
            node.Label.Size = SizeFAttr("sglblSize", new SizeF(50, 50));
            node.Label.WrappingWidth = FloatAttr("sglblWrap", 150);

            node.BackgroundColor = ColorAttr("BackgroundColor", Color.Lavender);
        }

        public override void ConsumeChild(object parent, object child)
        {
            SubgraphNode node = parent as SubgraphNode;
            node.Initializing = true;
            if (child is GoCollapsibleHandle)
                return;
            //7 January 2013 Inherit goText Like Hyperlink
            //8 January 2013 Labels are boundlabels
            if (child is GoText && !(child is Hyperlink) && !(child is BoundLabel) && !(child is ResizableBalloonComment) && !(child is ResizableComment))
                return;
            //23 January 2013
            if (child is MetaBase && !(child is GraphNode))
            //return;
            {
                MetaBase mb = child as MetaBase;
                if (node.BindingInfo.BindingClass != mb._ClassName)
                {
                    MetaBase mbRecreate = MetaBuilder.Meta.Loader.CreateInstance(node.BindingInfo.BindingClass);
                    mb.CopyPropertiesToObject(mbRecreate);
                    mb = mbRecreate;
                }

                MetaBase mbCached = RetrieveAlreadyLoaded(mb.pkid, mb.MachineName);
                if (mbCached != null)
                    mb = mbCached;
                else
                    AlreadyLoadedObjects.Add(mb);

                node.MetaObject = mb;
                node.BindingInfo.BoundObjectID = node.MetaObject.pkid;
            }

            if (!(child is GoText))
            {
                base.ConsumeChild(parent, child);
            }

            if (child != null)
            {
                if (child is GoObject)
                {
                    GoObject o = child as GoObject;

                    if (o is GoText)
                    {
                        o.Remove();
                        GoText txt = o as GoText;
                        node.Add(txt);
                        if (txt is BoundLabel)
                        {
                            txt.AddObserver(node);
                        }
                    }
                    else
                    {
                        //node.Add(o);
                    }
                }
            }
            node.Initializing = false;
        }

        public override void ConsumeObjectFinish(object obj)
        {
            base.ConsumeObjectFinish(obj);

            SubgraphNode node = obj as SubgraphNode;
            node.Initializing = true;
            //node.LoadDefaultClassBindings();
            if (!node.IsStencil)
            {
                if (node.MetaObject == null)
                {
                    node.CreateMetaObject(this, EventArgs.Empty);
                    node.MetaObject.Set("Name", node.Label.Text);
                }
                node.HookupEvents();
            }

            List<QuickAndDirtyObjectRelationshipDefinition> definitions = new List<QuickAndDirtyObjectRelationshipDefinition>();
            node.ObjectRelationships = new List<EmbeddedRelationship>();
            string sObjectRelationships = StringAttr("ObjectRelationships", "");
            if (sObjectRelationships.Length > 0)
            {
                string[] items = sObjectRelationships.Split(definitionSplitChar);
                foreach (string sItem in items)
                {
                    if (sItem.Length > 0)
                    {
                        string[] itemDef = sItem.Split(itemSplitChar);
                        QuickAndDirtyObjectRelationshipDefinition objDef = new QuickAndDirtyObjectRelationshipDefinition();

                        objDef.CAid = int.Parse(itemDef[2]);
                        objDef.PKID = int.Parse(itemDef[0]);
                        objDef.Machine = itemDef[1];

                        definitions.Add(objDef);
                        objDef = null;
                    }
                }
            }

            node.ObjectRelationships = new List<EmbeddedRelationship>();
            if (!node.IsStencil)
            {
                if (d.DataRepository.Connections.ContainsKey(Core.Variables.Instance.ClientProvider))
                {
                    b.TList<b.ClassAssociation> allowedAssociations = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByParentClass(node.MetaObject._ClassName);

                    node.DefaultClassBindings = new Dictionary<string, ClassAssociation>();
                    foreach (GoObject child in node)
                    {
                        if (child is IMetaNode)
                        {
                            IMetaNode imnode = child as IMetaNode;
                            //23 January 2013
                            //Incase a child is in the node but not the definitions
                            bool imNodeNotInDefinitions = true;
                            foreach (QuickAndDirtyObjectRelationshipDefinition def in definitions)
                            {
                                if ((imnode.MetaObject.pkid == def.PKID) && (imnode.MetaObject.MachineName == def.Machine))
                                {
                                    //there is an object like this child in the definitions of the saved node's ObjectRelationships attribute
                                    imNodeNotInDefinitions = false;
                                    allowedAssociations.Filter = "CAid = " + def.CAid.ToString() + " AND IsActive = 'True'";
                                    if (allowedAssociations.Count > 0)
                                    {
                                        EmbeddedRelationship emrel = new EmbeddedRelationship();
                                        emrel.MyMetaObject = imnode.MetaObject;
                                        emrel.MyAssociation = allowedAssociations[0];
                                        bool found = false;
                                        foreach (EmbeddedRelationship emrelExisting in node.ObjectRelationships)
                                        {
                                            if (emrelExisting.MyMetaObject.pkid == imnode.MetaObject.pkid && emrelExisting.MyAssociation.CAid == allowedAssociations[0].CAid)
                                            {
                                                found = true;
                                                imNodeNotInDefinitions = false;
                                            }
                                        }
                                        if (!found)
                                            node.ObjectRelationships.Add(emrel);

                                        if (!(node.DefaultClassBindings.ContainsKey(emrel.MyMetaObject._ClassName)))
                                        {
                                            ClassAssociation ca = new ClassAssociation();
                                            ca.CAid = def.CAid;
                                            ca.ParentClass = node.MetaObject._ClassName;
                                            ca.ChildClass = emrel.MyMetaObject._ClassName;
                                            ca.Caption = allowedAssociations[0].Caption;
                                            ca.AssociationTypeID = allowedAssociations[0].AssociationTypeID;
                                            node.DefaultClassBindings.Add(emrel.MyMetaObject._ClassName, ca);
                                        }
                                    }
                                }
                            }
                            //what if it is not in the definitions but just inside the node?
                            if (imNodeNotInDefinitions)
                            {
                                //here we have a node that is a child of this node but is not included in the definitions
                                //hmm should we ignore context and data consistency or follow this code?
                                EmbeddedRelationship emrel = new EmbeddedRelationship();
                                emrel.MyMetaObject = imnode.MetaObject;
                                try
                                {
                                    foreach (ClassAssociation cAss in d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByParentClass(node.MetaObject.Class))
                                    {
                                        //4 March 2013 removed isdefault
                                        if (cAss.ChildClass == imnode.MetaObject.Class) // && cAss.IsDefault
                                        {
                                            emrel.MyAssociation = cAss;
                                            if (!(node.DefaultClassBindings.ContainsKey(imnode.MetaObject._ClassName)))
                                            {
                                                node.DefaultClassBindings.Add(imnode.MetaObject._ClassName, cAss);
                                            }
                                            break;
                                        }
                                    }
                                }
                                catch
                                {
                                }
                                if (emrel.MyAssociation != null) //No default association can be found
                                {
                                    bool found = false;
                                    foreach (EmbeddedRelationship emrelExisting in node.ObjectRelationships)
                                    {
                                        if (emrelExisting.MyMetaObject.pkid == imnode.MetaObject.pkid && emrelExisting.MyAssociation.CAid == allowedAssociations[0].CAid)
                                        {
                                            found = true;
                                        }
                                    }
                                    if (!found)
                                        node.ObjectRelationships.Add(emrel);
                                }
                            }
                        }
                    }

                }
                foreach (QuickAndDirtyDefaultClassAssociationDefinition qddca in this.defaultClassAssocDefinitions)
                {
                    if (node.DefaultClassBindings.ContainsKey(qddca.ChildClass))
                    {
                        node.DefaultClassBindings[qddca.ChildClass].CAid = qddca.CAid;
                        node.DefaultClassBindings[qddca.ChildClass].Caption = qddca.Caption;
                        node.DefaultClassBindings[qddca.ChildClass].AssociationTypeID = qddca.AssociationTypeID;
                        node.DefaultClassBindings[qddca.ChildClass].ChildClass = qddca.ChildClass;
                    }
                }
            }
            //node.Expand(); //It is saved in expanded mode so no reason to expand it

            //node.Label.Text = StringAttr("lblText", string.Empty);
            node.Resizable = true;
            node.LayoutLabel();
            node.LayoutPort();

            node.Initializing = false;

            node.Handle.Location = new PointF(node.BoundsBeforeLoad.Location.X + node.CollapsedTopLeftMargin.Width, node.BoundsBeforeLoad.Location.Y + node.CollapsedTopLeftMargin.Height);

            if (node.IsStencil)
                node.Collapse();

            node.BindToMetaObjectProperties();
        }

        public override void GenerateAttributes(Object obj)
        {
            SubgraphNode node = obj as SubgraphNode;

            bool collapsed = !node.IsExpanded;
            //23 January 2013 Force expansion on save
            if (collapsed)
                node.Expand();
            WriteAttrVal("stencil", node.IsStencil);
            WriteAttrVal("Collapsed", collapsed);
            //WriteAttrVal("Expanded", node.Expanded);
            WriteAttrVal("BackgroundColor", node.BackgroundColor);
            // otherwise this is generated more than once!
            if (node.GetType().Name.ToString() == typeof(SubgraphNode).Name)
            {
                base.GenerateAttributes(obj, true);
                if (node.HasBindingInfo)
                {
                    WriteAttrVal("cls", node.BindingInfo.BindingClass);
                    WriteAttrVal("lblText", node.Label.Text);

                    WriteAttrVal("lblColour", node.Label.TextColor);
                    WriteAttrVal("sgfontSz", node.Label.FontSize);
                    WriteAttrVal("sgbold", node.Label.Bold);
                    WriteAttrVal("sgitalic", node.Label.Italic);
                    WriteAttrVal("sgfont", node.Label.Font.FontFamily.Name);
                    WriteAttrVal("sgmulti", node.Label.Multiline);
                    WriteAttrVal("sgstrike", node.Label.StrikeThrough);
                    WriteAttrVal("sgtrim", node.Label.StringTrimming.ToString());
                    WriteAttrVal("sgul", node.Label.Underline);
                    WriteAttrVal("sglblSize", node.Label.Size);
                    //WriteAttrVal("sglblWrap", node.Label.WrappingWidth);

                    if (node.MetaObject != null)
                    {
                        WriteAttrVal("moPKID", node.MetaObject.pkid);
                        WriteAttrVal("moMachine", node.MetaObject.MachineName);
                        WriteAttrVal("moWorkspace", node.MetaObject.WorkspaceName);
                        WriteAttrVal("moWorkspaceType", node.MetaObject.WorkspaceTypeId);
                    }
                }
                StringBuilder sbObjectRelationships = new StringBuilder();
                foreach (EmbeddedRelationship rel in node.ObjectRelationships)
                {
                    sbObjectRelationships.Append(rel.MyMetaObject.pkid.ToString() + "|" + rel.MyMetaObject.MachineName + "|" + rel.MyAssociation.CAid.ToString() + ":");
                }
                WriteAttrVal("ObjectRelationships", sbObjectRelationships.ToString());

                StringBuilder sbDefaultAssociations = new StringBuilder();
                foreach (KeyValuePair<string, ClassAssociation> defAsssoc in node.DefaultClassBindings)
                {
                    sbDefaultAssociations.Append(defAsssoc.Value.CAid.ToString() + "|" + defAsssoc.Value.ChildClass + "|" + defAsssoc.Value.AssociationTypeID.ToString() + "|" +
                                                 defAsssoc.Value.Caption + ":");
                }
                WriteAttrVal("DefaultClassAssociations", sbDefaultAssociations.ToString());
                WriteAttrVal("CollapsedBottomRightMargin", node.CollapsedBottomRightMargin);
                WriteAttrVal("CollapsedTopLeftMargin", node.CollapsedTopLeftMargin);
            }
            if (collapsed)
                node.Collapse();
        }

        public override void GenerateDefinitions(Object obj)
        {
            //if (!(obj is CollapsibleNode))
            //base.GenerateDefinitions(obj);

            SubgraphNode n = (SubgraphNode)obj;
            foreach (GoObject child in n)
            {
                if (!SkipGeneration(child))
                {
                    if (child is IMetaNode)
                    {
                        if ((child as IMetaNode).MetaObject != n.MetaObject)
                        {
                            // Skip Generation
                        }
                        else
                            Writer.DefineObject(child);
                    }
                    else
                        Writer.DefineObject(child);
                }
            }
            // Writer.DefineObject(n.MetaObject);
        }

        public MetaBase RetrieveAlreadyLoaded(int pkid, string machinename)
        {
            foreach (MetaBase mbCached in AlreadyLoadedObjects)
            {
                if ((mbCached.pkid == pkid) && (mbCached.MachineName == machinename))
                {
                    return mbCached;
                }
            }
            return null;
        }

        #endregion Methods

        #region Nested Classes (2)

        private class QuickAndDirtyDefaultClassAssociationDefinition
        {

            #region Fields (4)

            private int associationTypeID;
            private int caid;
            private string caption;
            private string childClass;

            #endregion Fields

            #region Properties (4)

            public int AssociationTypeID
            {
                get { return associationTypeID; }
                set { associationTypeID = value; }
            }

            public int CAid
            {
                get { return caid; }
                set { caid = value; }
            }

            public string Caption
            {
                get { return caption; }
                set { caption = value; }
            }

            public string ChildClass
            {
                get { return childClass; }
                set { childClass = value; }
            }

            #endregion Properties

        }
        private class QuickAndDirtyObjectRelationshipDefinition
        {

            #region Fields (3)

            private int caid;
            private string machine;
            private int pkid;

            #endregion Fields

            #region Properties (3)

            public int CAid
            {
                get { return caid; }
                set { caid = value; }
            }

            public string Machine
            {
                get { return machine; }
                set { machine = value; }
            }

            public int PKID
            {
                get { return pkid; }
                set { pkid = value; }
            }

            #endregion Properties

        }

        #endregion Nested Classes

    }
}