using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Graphing.Formatting;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Behaviours.Internal;
using MetaBuilder.Graphing.Shapes.Nodes.Containers;
using MetaBuilder.Graphing.Shapes.Primitives;
using MetaBuilder.Meta;
using Northwoods.Go.Xml;
using Northwoods.Go;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.General;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Nodes
{
    public class ValueChainSubgraphTransformer : BaseGoObjectTransformer
    {

        #region Fields (4)

        private readonly List<MetaBase> AlreadyLoadedObjects;
        //private string anchors;
        private List<QuickAndDirtyDefaultClassAssociationDefinition> defaultClassAssocDefinitions;
        private List<QuickAndDirtyObjectRelationshipDefinition> definitions;

        #endregion Fields

        #region Constructors (1)

        public ValueChainSubgraphTransformer()
            : base()
        {
            TransformerType = typeof(ValueChain);
            ElementName = "vcsubgnode";
            IdAttributeUsedForSharedObjects = true;
            BodyConsumesChildElements = true;
            AlreadyLoadedObjects = new List<MetaBase>();
        }

        #endregion Constructors

        #region Methods (9)

        // Public Methods (8) 

        public override object Allocate()
        {
            ValueChain node = new ValueChain(false);

            //   node.Initializing = true;
            /*    for (int i = 0; i < node.Count; i++)
                    if (node[i] is NonPrintingQuickPort)
                        node[i].Remove();*/
            return node;
        }
        const char definitionSplitChar = ':';
        const char itemSplitChar = '|';

        public override void ConsumeAttributes(object obj)
        {
            ValueChain node = obj as ValueChain;
            base.ConsumeAttributes(obj);

            node.Selectable = true;
            node.Label.Text = StringAttr("lblText", string.Empty);
            //BoundLabel lbl = node.Label as BoundLabel;
            GoText lbl = node.Label as GoText;
            definitions = new List<QuickAndDirtyObjectRelationshipDefinition>();
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
                    }
                }
            }
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

            int pkid = int.Parse(StringAttr("moPKID", "0"));
            string Mach = StringAttr("moMachine", "");
            if (pkid > 0 && d.DataRepository.Connections.ContainsKey(Core.Variables.Instance.ClientProvider))
            {
                node.MetaObject = Loader.GetByID("Function", pkid, Mach);
            }

            node.Initializing = true;
            RectangleF rectBounds = RectangleFAttr("GridBounds", new RectangleF(new PointF(), new SizeF()));
            if (rectBounds.Width > 0)
            {
                node.Grid.Bounds = rectBounds;
            }
            else
            {
                RectangleFAttr("xy", new RectangleF(new PointF(), new SizeF()));
            }

            Color cborder;
            Color couter;
            Color cinner;
            bool hasGradient = BooleanAttr("grad", false);
            if (hasGradient)
            {
                cborder = ColorAttr("grad_bordercolor", Color.Black);
                couter = ColorAttr("grad_outercolor", Color.Black);
                cinner = ColorAttr("grad_innercolor", Color.Black);
                CreateFill(node.Background, ref cborder, ref couter, ref cinner);
            }
            else
            {
                GoShape shape = node.Background as GoShape;
                if ((shape != null) && (!(shape is QuickStroke)))
                {
                    cborder = Color.Black;
                    cinner = ColorAttr("solidColor", Color.White);
                    couter = cinner;
                    CreateFill(node.Background, ref cborder, ref couter, ref cinner);
                }
            }

            lbl.FontSize = FloatAttr("fontSz", 10);
            lbl.Alignment = Int32Attr("align", 0);
            lbl.Bold = BooleanAttr("bold", false);
            lbl.Italic = BooleanAttr("ita", false);
            lbl.StrikeThrough = BooleanAttr("strike", false);
            lbl.TextColor = ColorAttr("colour", Color.Black);
            lbl.Underline = BooleanAttr("ul", false);
            lbl.Wrapping = BooleanAttr("wrap", false);
            lbl.FamilyName = StringAttr("font", "Tahoma");
            if (lbl.Wrapping)
            {
                float wrappingWidth = FloatAttr("wrapw", 4f);
                lbl.WrappingWidth = wrappingWidth;
            }
            node.RemoveChildName("rightPort");
            node.RemoveChildName("leftPort");
            node.RemoveChildName("topPort");
            node.RemoveChildName("bottomPort");
            node.Initializing = false;
        }

        public override void ConsumeChild(object parent, object child)
        {
            if (child is QuickPort && (!(child is NonPrintingQuickPort)))
                return;
            if (child is GoCollapsibleHandle)
                return;
            //7 January 2013 Inherit goText Like Hyperlink
            //8 January 2013 Labels are boundlabels
            if (child is GoText && !(child is Hyperlink) && !(child is BoundLabel) && !(child is ResizableBalloonComment) && !(child is ResizableComment))
                return;
            if (child is GoGroup && !(child is ResizableBalloonComment) && !(child is ResizableComment))
            {
                GoGroup grp = child as GoGroup;
                if (grp.Count == 2)
                    if (grp.First is GoRectangle && grp.Last is GoText)
                        return;
            }

            //if (child is ValueChain)
            //{
            //    ValueChain vcChild = child as ValueChain;
            //    //vcChild.RepositionPorts();
            //    //vcChild.ReanchorPorts();
            //}
            if (child is MetaBase)
            {
                ValueChain vcs = parent as ValueChain;
                vcs.MetaObject = child as MetaBase;
                return;
            }
            ValueChain node = parent as ValueChain;
            base.ConsumeChild(parent, child);
            if (child != null)
            {
                if (child is NonPrintingQuickPort)
                {
                    NonPrintingQuickPort npqr = child as NonPrintingQuickPort;
                    if (npqr.Name.Length > 0)
                        if (node.FindChild(npqr.Name) == null)
                            node.AddChildName(npqr.Name, npqr);
                    return;
                }
                else if (child is GoObject)
                {
                    GoObject o = child as GoObject;
                    if (o.IsInDocument)
                        o.Remove();
                    if (o is GoText)
                    {
                        GoText txt = o as GoText;
                        node.Add(txt);
                        if (txt is BoundLabel)
                            txt.AddObserver(node);
                    }
                    else
                        node.Add(o);
                }
            }
            node.Initializing = false;
        }

        public override void ConsumeObjectFinish(object obj)
        {
            ValueChain node = obj as ValueChain;
            // bring the text to the front
            node.ObjectRelationships = new List<EmbeddedRelationship>();
            node.DefaultClassBindings = new Dictionary<string, ClassAssociation>();
            if (d.DataRepository.Connections.ContainsKey(Core.Variables.Instance.ClientProvider))
            {

                b.TList<b.ClassAssociation> allowedAssociations = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByParentClass(node.MetaObject._ClassName);

                foreach (GoObject child in node)
                {
                    if (child is IMetaNode)
                    {
                        IMetaNode imnode = child as IMetaNode;
                        foreach (QuickAndDirtyObjectRelationshipDefinition def in definitions)
                        {
                            if ((imnode.MetaObject.pkid == def.PKID) && (imnode.MetaObject.MachineName == def.Machine))
                            {
                                allowedAssociations.Filter = "CAid = " + def.CAid.ToString();
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
                                //foreach (KeyValuePair<string,ClassAssociation> classAssoc )
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
            // now lets look at those anchors again!
            //  RestoreAnchors(node);

            // Add those ports!
            //    node.RepositionPorts();
            //    node.Initializing = false;
            node.ResizeBackgroundToGrid();
            node.LayoutLabel();
            node.RepositionPorts();
            base.ConsumeObjectFinish(obj);
            node.Initializing = true;
            //node.MetaObject.Set("Name", node.Label.Text);
            node.Initializing = false;
        }

        public override void GenerateAttributes(Object obj)
        {
            ValueChain node = obj as ValueChain;
            base.GenerateAttributes(obj, true);
            // otherwise this is generated more than once!
            if (node.GetType().Name.ToString() == typeof(ValueChain).Name)
            {
                if (node.HasBindingInfo)
                {
                    WriteAttrVal("cls", node.BindingInfo.BindingClass);
                    WriteAttrVal("lblText", node.Label.Text);

                    if (node.MetaObject != null)
                    {
                        WriteAttrVal("moPKID", node.MetaObject.pkid);
                        WriteAttrVal("moMachine", node.MetaObject.MachineName);
                    }
                }
                StringBuilder sbObjectRelationships = new StringBuilder();
                foreach (EmbeddedRelationship rel in node.ObjectRelationships)
                {
                    sbObjectRelationships.Append(rel.MyMetaObject.pkid.ToString() + "|" + rel.MyMetaObject.MachineName + "|" +
                                                 rel.MyAssociation.CAid.ToString() + ":");
                }

                StringBuilder sbDefaultAssociations = new StringBuilder();
                foreach (KeyValuePair<string, ClassAssociation> defAsssoc in node.DefaultClassBindings)
                {
                    sbDefaultAssociations.Append(defAsssoc.Value.CAid.ToString() + "|" + defAsssoc.Value.ChildClass +
                                                 "|" + defAsssoc.Value.AssociationTypeID.ToString() + "|" +
                                                 defAsssoc.Value.Caption + ":");
                }
                WriteAttrVal("DefaultClassAssociations", sbDefaultAssociations.ToString());
                WriteAttrVal("ObjectRelationships", sbObjectRelationships.ToString());
                WriteAttrVal("GridBounds", node.Grid.Bounds);
            }

            GradientBehaviour gbeh = node.Background.Manager.GetExistingBehaviour(typeof(GradientBehaviour)) as GradientBehaviour;
            if (gbeh != null)
            {
                if (gbeh.MyBrush.OuterColor != gbeh.MyBrush.InnerColor)
                {

                    WriteAttrVal("grad", true);
                    WriteAttrVal("grad_bordercolor", gbeh.MyBrush.BorderColor);
                    WriteAttrVal("grad_outercolor", gbeh.MyBrush.OuterColor);
                    WriteAttrVal("grad_innercolor", gbeh.MyBrush.InnerColor);
                    WriteAttrVal("grad_type", gbeh.MyBrush.GradientType.ToString());
                }
                else
                {
                    WriteAttrVal("solidColor", gbeh.MyBrush.InnerColor);
                }
            }
            else
            {
                GoShape shape = node.Background as GoShape;
                if (shape != null)
                {
                    SolidBrush sbrush = shape.Brush as SolidBrush;
                    if (sbrush != null)
                        WriteAttrVal("solidColor", sbrush.Color);
                }
            }

            GoText lbl = node.Label;
            WriteAttrVal("font", lbl.FamilyName);
            WriteAttrVal("align", lbl.Alignment);
            if (lbl.Bold)
                WriteAttrVal("bold", lbl.Bold);

            if (lbl.Italic)
                WriteAttrVal("ita", lbl.Italic);

            WriteAttrVal("fontSz", lbl.FontSize);
            if (lbl.StrikeThrough)
                WriteAttrVal("strike", lbl.StrikeThrough);

            if (lbl.Underline)
                WriteAttrVal("ul", lbl.Underline);
            if (lbl.Wrapping)
            {
                WriteAttrVal("wrap", lbl.Wrapping);
                WriteAttrVal("wrapw", lbl.WrappingWidth);
            }
            WriteAttrVal("colour", lbl.TextColor);
        }

        public override void GenerateDefinitions(Object obj)
        {
            //base.GenerateDefinitions(obj);
            //return;

            ValueChain n = (ValueChain)obj;
            foreach (GoObject child in n)
            {
                if (!SkipGeneration(child))
                    //    {
                    //        if (child is IMetaNode)
                    //        {
                    //            if ((child as IMetaNode).MetaObject != n.MetaObject)
                    //            {
                    //                 //Skip Generation
                    //                if (child is ValueChain)
                    //                {
                    //                    //base.GenerateDefinitions(child);
                    //                }
                    //            }
                    //            else
                    //                Writer.DefineObject(child);
                    //        }
                    //        else
                    //9 January 2013 (Labels load but are not part of the valuechain, written as root objects?)
                    if (child != n.Label && (child is BoundLabel || child is Hyperlink))
                    {
                        //Writer.DefineObject(child);
                        Writer.GenerateObject(child);
                    }
                //}
            }
            //Writer.DefineObject(n.MetaObject);
            //base.GenerateDefinitions(obj);
        }

        public MetaBase RetrieveAlreadyLoaded(int pkid, string machinename)
        {
            foreach (MetaBase mbCached in AlreadyLoadedObjects)
                if ((mbCached.pkid == pkid) && (mbCached.MachineName == machinename))
                    return mbCached;
            return null;
        }

        public bool SkipGeneration(GoObject obj)
        {
            return obj is GoButton || obj is IGoHandle || obj is GoPolygon || obj is GoRectangle || obj is IMetaNode;
            //return obj is GoButton || obj is IGoHandle || obj is GoPolygon || obj is BoundLabel || obj is GoRectangle;
        }

        // Private Methods (1) 

        private void CreateFill(object obj, ref Color cborder, ref Color couter, ref Color cinner)
        {
            GoShape shape = obj as GoShape;
            IBehaviourShape ibshape = shape as IBehaviourShape;
            GradientBehaviour gbeh = new GradientBehaviour();
            gbeh.MyBrush = new ShapeGradientBrush();
            gbeh.MyBrush.BorderColor = cborder;
            gbeh.MyBrush.OuterColor = couter;
            gbeh.MyBrush.InnerColor = cinner;
            string defaultGradient = StringAttr("grad_type", GradientType.ForwardDiagonal.ToString());
            Type tGradType = typeof(GradientType);
            gbeh.MyBrush.GradientType = (GradientType)Enum.Parse(tGradType, defaultGradient);
            ibshape.Manager.AddBehaviour(gbeh);
            gbeh.Update(shape);
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