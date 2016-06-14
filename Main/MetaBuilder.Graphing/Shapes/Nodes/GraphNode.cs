using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Reflection;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Behaviours.Observers;
using MetaBuilder.Graphing.Shapes.Nodes;
using MetaBuilder.Graphing.Shapes.Nodes.Containers;
using MetaBuilder.Graphing.Tools;
using MetaBuilder.Meta;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Northwoods.Go;
using ObjectAssociation = MetaBuilder.BusinessLogic.ObjectAssociation;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using System.Collections.ObjectModel;

namespace MetaBuilder.Graphing.Shapes
{
    [Serializable]
    public class IndicatorLabel : GoText
    {
        #region Properties (1)

        public override bool Printable
        {
            get { return Variables.Instance.PrintVCIndicators; }
            set { base.Printable = value; }
        }

        #endregion Properties

        public IndicatorLabel()
        {
        }

        public IndicatorLabel(string s, Color c)
        {
            Movable = false;
            Printable = false;
            Editable = false;
            Text = s;
            TextColor = c;
            Selectable = false;
            Deletable = false;
            FontSize = 8f;
        }

    }

    [Serializable]
    public class ChangedIndicatorLabel : IndicatorLabel
    {
        //public override bool Visible
        //{
        //    get
        //    {
        //        //return (View != null && View.DocScale < 0.3f) ? false : base.Visible;
        //        return base.Visible;
        //    }
        //    set
        //    {
        //        base.Visible = value;
        //    }
        //}
        public ChangedIndicatorLabel(string s, Color c)
        {
            Movable = false;
            Printable = false;
            Editable = false;
            Text = s;
            TextColor = c;
            Selectable = false;
            Deletable = false;
            FontSize = 8f;
            AutoResizes = true;
        }
    }

    [Serializable]
    public class GraphNode : GoNode, IGoCollapsible, IShallowCopyable//, IBehaviourShape
    {
        public AllocationHandle AllocationHandle
        {
            get
            {
                foreach (GoObject o in this)
                    if (o is AllocationHandle)
                        return o as AllocationHandle;
                return null;
            }
        }

        //[NonSerialized]
        //private ZoomQuality quality;
        //public ZoomQuality Quality
        //{
        //    get { return quality; }
        //    set
        //    {
        //        quality = value;
        //        //#if DEBUG
        //        //                if (value == ZoomQuality.Low)
        //        //                {
        //        //                    foreach (GoObject o in this)
        //        //                        if (o is IBehaviourShape)
        //        //                            foreach (IBehaviour behaviour in (o as IBehaviourShape).Manager.Behaviours)
        //        //                                if (behaviour is MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour)
        //        //                                    if (o is GoShape)
        //        //                                        (behaviour as MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour).Disable(o as GoShape);
        //        //                }
        //        //                else
        //        //                {
        //        //                    foreach (GoObject o in this)
        //        //                        if (o is IBehaviourShape)
        //        //                            foreach (IBehaviour behaviour in (o as IBehaviourShape).Manager.Behaviours)
        //        //                                if (behaviour is MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour)
        //        //                                    if (o is GoShape)
        //        //                                    {
        //        //                                        MetaBuilder.Graphing.Formatting.ShapeGradientBrush brush = (behaviour as MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour).MyDisabledBrush;
        //        //                                        if (brush != null)
        //        //                                            brush.Apply(o as GoShape);
        //        //                                    }

        //        //                }
        //        //                //All gotext is already handled
        //        //#endif
        //    }
        //}

        private bool mustLoadFromDatabase;
        public bool MustLoadFromDatabase
        {
            get { return mustLoadFromDatabase; }
            set { mustLoadFromDatabase = value; }
        }

        private string allowedClasses;
        public string AllowedClasses
        {
            get { return allowedClasses; }
            set { allowedClasses = value; }
        }

        private bool canBeAutomaticallyReplaced;
        public bool CanBeAutomaticallyReplaced
        {
            get { return canBeAutomaticallyReplaced; }
            set { canBeAutomaticallyReplaced = value; }
        }

        private bool collapsible;
        private bool copyAsShadow;
        //!!!!!
        public bool saved = false;
        //!!!!!
        [NonSerialized]
        private bool parentIsILinkedContainer;
        public bool ParentIsILinkedContainer
        {
            get { return parentIsILinkedContainer; }
            set { parentIsILinkedContainer = value; }
        }

        //[NonSerialized]
        //private bool hasVisualIndicator;
        [NonSerialized]
        private bool requiresAttention;
        [NonSerialized]
        private MetaBase copiedFrom;
        public MetaBase CopiedFrom
        {
            get { return copiedFrom; }
            set { copiedFrom = value; }
        }
        public GraphNode()
        {
            Add(CreateGrid());
            _AutoSizeGrid = true;
            EditMode = true;
            DragsNode = true;
            name = Guid.NewGuid().ToString();
            Collapsible = true;
            //CreatePort();
        }

        #region Grid - Creation, Autosizing

        private bool _AutoSizeGrid;
        public bool AutoSizeGrid
        {
            get { return _AutoSizeGrid; }
            set { _AutoSizeGrid = value; }
        }

        public virtual GoGrid Grid
        {
            get
            {
                if (Count > 0)
                    return this[0] as GoGrid;
                return null;
            }
            set
            {
            }
        }

        protected virtual GoGrid CreateGrid()
        {
            GraphNodeGrid grid = new GraphNodeGrid();
            grid.Selectable = false;
            grid.AutoRescales = true;
            grid.Resizable = true;
            grid.ResizesRealtime = false;
            grid.Bounds = new RectangleF(0, 0, 100, 100);
            grid.SnapDrag = GoViewSnapStyle.Jump;
            grid.SnapResize = GoViewSnapStyle.Jump;
            grid.CellSize = new SizeF(SnapSettings.UnitSize, SnapSettings.UnitSize);
            grid.Style = GoViewGridStyle.Line;
            grid.Pen = StandardPen;
            grid.LineColor = Color.Lavender;
            grid.Brush = null;
            grid.DragsNode = false;
            grid.Movable = false;
            grid.Deletable = false;
            return grid;
        }

        public void SwitchAutoSizeGrid(object sender, EventArgs e)
        {
            AutoSizeGrid = !AutoSizeGrid;
        }

        #endregion

        #region Binding & Editing

        private bool _editMode;
        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                Grid.Style = (value) ? GoViewGridStyle.Line : GoViewGridStyle.None;
                Grid.Pen = (value) ? StandardPen : new Pen(Color.Transparent);
                Grid.Selectable = value;
            }
        }

        #endregion

        #region Find Items

        public IIdentifiable FindByName(string name)
        {
            foreach (GoObject o in this)
            {
                if (o is IIdentifiable)
                {
                    IIdentifiable inamedObject = o as IIdentifiable;
                    if (inamedObject.Name == name)
                    {
                        return inamedObject;
                    }
                }
                if (o is GoGroup)
                {
                    GoGroup grp = o as GoGroup;
                    foreach (GoObject obj in grp)
                    {
                        IIdentifiable inamedObject2 = obj as IIdentifiable;
                        if (inamedObject2 != null)
                            if (inamedObject2.Name == name)
                            {
                                return inamedObject2;
                            }
                        if (obj is GoGroup)
                        {
                            GoGroup grpChild = obj as GoGroup;
                            foreach (GoObject objChild in grpChild)
                            {
                                IIdentifiable inamedObject3 = objChild as IIdentifiable;
                                if (inamedObject3 != null)
                                    if (inamedObject3.Name == name)
                                    {
                                        return inamedObject3;
                                    }
                            }
                        }
                    }
                }
            }
            return null;
        }

        // unlike normal GoGroups, PickObjects picks all children at a particular PointF, not just the "top" one
        public override IGoCollection PickObjects(PointF p, bool selectableOnly, IGoCollection coll, int max)
        {
            if (coll == null) coll = new GoCollection();
            if (coll.Count >= max) return coll;
            if (!CanView()) return coll;
            foreach (GoObject child in Backwards)
            {
                GraphNode disp = child as GraphNode;
                if (disp != null)
                {
                    disp.PickObjects(p, selectableOnly, coll, max);
                }
                else
                {
                    GoObject picked = child.Pick(p, selectableOnly);
                    if (picked != null)
                    {
                        coll.Add(picked);
                        if (coll.Count >= max) return coll;
                    }
                }
            }
            return coll;
        }

        public ArrayList FindItemsOfType(Type t)
        {
            ArrayList retval = new ArrayList();
            foreach (object o in this)
            {
                if (o.GetType() == t)
                {
                    retval.Add(o);
                }
            }
            return retval;
        }

        #endregion

        #region Add, Drop Items

        public override void Add(GoObject obj)
        {
            if (!(obj is GraphNode))
            {
                base.Add(obj);
            }
            else
            {
                GraphNode node = obj as GraphNode;
                CalculateGridSize();
                BindingInfo = node.bindingInfo;

                HookupEvents();
            }
            if (obj is IndicatorLabel || obj is ChangedIndicatorLabel)
                return;
            //obj.Selectable = true;
            if (EditMode)
            {
                if (obj is IIdentifiable)
                {
                    IIdentifiable namedObject = obj as IIdentifiable;
                    GoObject existingChild = FindChild(namedObject.Name);
                    if (existingChild != null)
                    {
                        // Replace it
                        existingChild.Remove();
                    }
                    if (obj.Parent == this)
                        AddChildName((obj as IIdentifiable).Name, obj); // Add it
                }
                if (obj is BoundLabel)
                {
                    obj.AddObserver(this);
                }
            }
        }

        // When dragging a non-GraphNode (eg, a shape) onto a GraphNode, highlight it
        public override bool OnEnterLeave(GoObject from, GoObject to, GoView view)
        {
            if (from is GraphNode && from != this)
            {
                ((GraphNode)from).SetHighlight(false);
            }
            if (view.Tool is PortMovingTool)
            {
                SetHighlight(true);
            }
            else
            {
                SetHighlight(view.Tool is GoToolDragging && to == this && !view.Selection.IsEmpty
                             && IsAddable(view.Selection.Primary) && view.Selection.Primary.Parent != this);
            }
            return true;
        }

        // called after a GoToolDragging drop
        public override bool OnSelectionDropped(GoView view)
        {
            if (EditMode)
                AddPartsOrLinkNodes(view.Selection);
            return true;
        }

        // don't add other nodes as children
        public virtual bool IsAddable(GoObject obj)
        {
            return !(obj is GraphNode);
        }

        // return a collection of objects that actually are added to this container
        public virtual IGoCollection AddPartsOrLinkNodes(IGoCollection coll)
        {
            // skip all dropped ShapeContainers or Shelves--just include Items and GoText labels et al
            GoCollection items = new GoCollection();
            foreach (GoObject obj in coll)
            {
                if (IsAddable(obj))
                    items.Add(obj);
            }
            if (items.IsEmpty)
                return items;
            IGoCollection added = AddCollection(items, false);
            return added;
        }

        #endregion

        #region Port Manipulation

        public GoPort GetDefaultPort
        {
            get
            {
                for (int i = 0; i < Count; i++)
                {
                    if (this[i] is GoPort)
                    {
                        if (this[i].Visible)
                        {
                            if (this[i] is QuickPort)
                            {
                                if ((this[i] as QuickPort).PortPosition == (General.QuickPortHelper.QuickPortLocation)Enum.Parse(typeof(General.QuickPortHelper.QuickPortLocation), Core.Variables.Instance.DefaultFromPort))
                                    return this[i] as GoPort;
                            }
                            else
                            {
                                return this[i] as GoPort;
                            }
                        }
                    }
                    else if (this[i] is GraphNode) //no idea how this code will ever run
                    {
                        for (int x = 0; x < (this[i] as GraphNode).Count; x++)
                        {
                            if ((this[i] as GraphNode)[x] is GoPort)
                                return (this[i] as GraphNode)[x] as GoPort;
                        }
                    }
                }

                foreach (GoObject o in this)
                    if (o is QuickPort)
                        return o as GoPort;

                return null;
            }
        }

        /// <summary>
        /// Helper function designed for Repeater Item Instances (at some stage we might want to allow ports within repeaters)
        /// </summary>
        public void RemovePorts()
        {
            foreach (GoObject o in this)
            {
                if (o is GoPort)
                    o.Remove();
            }
        }

        public void FormatPorts()
        {
            foreach (GoObject o in this)
            {
                if (o is GoPort)
                {
                    o.Deletable = false;
                    o.Resizable = false;
                    o.Reshapable = false;
                }
            }
            /* try
            {
                if (this.Count > 1)
                {
                    int portCount = 0;
                    GoNodePortEnumerator i = Ports.GetEnumerator();
                    while (i.MoveNext())
                    {

                        portCount++;
                    }
                    this[0].Bounds = new RectangleF(new PointF(Position.X - 5, Position.Y - 5), new SizeF(Width + 10, Height + 10));

                }
            }
            catch (Exception x)
            {
                System.Diagnostics.Debug.WriteLine(x.ToString());
            }*/
        }

        /* Was going to use this approach, using invisible links approach now
         * public void LinkGraphNode(GoObject obj)
        {
            // only graphnodes can be auto linked
            if (obj is GraphNode)
            {
                GraphNode gnode = obj as GraphNode;

                if (gnode.HasBindingInfo && HasBindingInfo)
                {
                    // now, check for "valid" associations
                    int CAid =BusinessFacade.MetaHelper.Singletons.GetClassHelper().GetAssociationBetweenClasses(BindingInfo.BindingClass, gnode.BindingInfo.BindingClass);
                    if (CAid > 0)
                    {
                        // this approach adds an association
                    }
                }
                
            }
        }*/

        #endregion

        #region Properties & Accessors

        private BindingInfo bindingInfo;
        private MetaBase metaObject;

        public int PortCount
        {
            get
            {
                int i = 0;
                GoNodePortEnumerator enumerator = Ports.GetEnumerator();
                while (enumerator.MoveNext())
                    i++;
                return i;
            }
        }

        public virtual Collection<RepeaterSection> RepeaterSections
        {
            get
            {
                Collection<RepeaterSection> rsections = new Collection<RepeaterSection>();
                GoGroupEnumerator enumerator = GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current is RepeaterSection)
                        rsections.Add(enumerator.Current as RepeaterSection);

                    if (enumerator.Current is CollapsingRecordNodeItemList)
                    {
                        GoGroupEnumerator colNodeEnum = (enumerator.Current as CollapsingRecordNodeItemList).GetEnumerator();
                        while (colNodeEnum.MoveNext())
                        {
                            if (colNodeEnum.Current is RepeaterSection)
                                rsections.Add(colNodeEnum.Current as RepeaterSection);
                        }
                    }
                }
                return rsections;
            }
        }

        public Collection<BoundLabel> BoundLabels
        {
            get
            {
                Collection<BoundLabel> retval = new Collection<BoundLabel>();
                foreach (GoObject obj in this)
                {
                    if (obj is BoundLabel)
                        retval.Add(obj as BoundLabel);
                }
                return retval;
            }
        }

        public MetaBase MetaObject
        {
            get
            {
                return metaObject;
            }
            set
            {
                //if (value == null)
                //    hooked = false;
                metaObject = value;
            }
        }

        public bool HasBindingInfo
        {
            get
            {
                bool retval = false;
                if (BindingInfo != null)
                {
                    retval = (BindingInfo.BindingClass.Length > 0);
                }
                return retval;
            }
        }

        public BindingInfo BindingInfo
        {
            get { return bindingInfo; }
            set { bindingInfo = value; }
        }

        #endregion

        #region DataBinding

        public virtual void BindToMetaObjectProperties()
        {
            try
            {
                if (MetaObject == null)
                    return;
                //if (Document != null)
                //{
                //    SkipsUndoManager = true;
                //    //Document.StartTransaction();
                //}
                if (!Initializing && BindingInfo != null && BindingInfo.Bindings != null)
                {
                    foreach (KeyValuePair<string, string> kvpair in BindingInfo.Bindings)
                    {
                        //string val;
                        if (kvpair.Value != "-none-")
                        {
                            BoundLabel label = FindByName(kvpair.Key) as BoundLabel;
                            if (label == null)
                                continue;

                            object propvalue = MetaObject.Get(kvpair.Value);
                            if (propvalue != null)
                            {
                                //val = propvalue.ToString();
                                if (label.EditorStyle == GoTextEditorStyle.ComboBox)
                                {
                                    //label.FontSize = 7;
                                    //label.Font = new Font(label.Font.FontFamily, 7f);
                                    //label.TextColor = Color.FromArgb(-15132304);

                                    if (label.Name.Contains("Def_"))
                                    {
                                        //label.FontSize = 7;
                                        //label.TextColor = Color.FromArgb(-15132304);
                                        if (!label.Choices.Contains(propvalue.ToString()))
                                        {
                                            label.Choices.Add(propvalue.ToString());
                                        }
                                    }
                                }
                                label.Text = propvalue.ToString();
                            }
                            //else
                            //{
                            //    PropertyInfo pinfo = MetaObject.GetType().GetProperty(kvpair.Value);
                            //    if (pinfo != null)
                            //    {
                            //        if (pinfo.PropertyType.ToString() == "System.Nullable`1[System.Int32]")
                            //        {
                            //            label.Text = "";
                            //        }
                            //    }
                            //}
                        }
                    }
                }
                //if (Document != null)
                //{
                //    //Document.FinishTransaction("Altering graphnode style");
                //    SkipsUndoManager = false;
                //}

                if (this.Count > 6 && ((!IsStencilOnlyText && BindingInfo != null && BindingInfo.BindingClass != "DataValue")))
                {
                    BindMetaObjectImage();
                }
                else
                {
                    IsStencilOnlyText = true;
                    //try
                    //{
                    if (MetaObject != null && MetaObject.Class == "GovernanceMechanism")
                    {
                        if (this[4] != null)
                            BindBoundsToStringWidth(this[4] as BoundLabel);
                    }
                    else
                    {
                        if (this[2] != null)
                            BindBoundsToStringWidth(this[2] as BoundLabel);
                    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    //Text binding problem
                    //}
                }

                if (!Initializing)
                {
                    if (MetaPropList != null && MetaPropList.Visible == true)
                    {
                        InitMetaProps(true); //remove
                        InitMetaProps(true); //add
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("GraphNode::BindToMetaObjectProperties" + Environment.NewLine + ex.ToString());
            }
        }

        private void BindBoundsToStringWidth(BoundLabel lbl)
        {
            try
            {
                Resizable = false;
                Reshapable = false;

                if (lbl == null)
                    return;

                lbl.AutoResizes = true;

                Bitmap objBitmap = default(Bitmap);
                Graphics g = default(Graphics);

                objBitmap = new Bitmap(500, 200);
                g = Graphics.FromImage(objBitmap);

                //SizeF stringSize = objGraphics.MeasureString(str, new Font("Arial", 12));
                SizeF s = g.MeasureString(lbl.Text, lbl.Font);
                //set width to whatever text width is!
                lbl.Width = this.Width = s.Width;
                lbl.Height = this.Height = s.Height;
                Bounds.Inflate(2, 2);

                objBitmap.Dispose();
                g.Dispose();
            }
            catch
            {

            }
        }

        private bool isStencilOnlyText;
        public bool IsStencilOnlyText
        {
            get { return isStencilOnlyText; }
            set { isStencilOnlyText = value; }
        }

        public void BindMetaObjectImage()
        {
            //return;
            //if (View == null) //skip this function when opening. This really should be initializing
            //    return;
            //dont care about employees
            if (MetaObject == null || strings.replaceApostrophe(MetaObject.Class.ToString()) == "DataEntity" || strings.replaceApostrophe(MetaObject.Class.ToString()) == "SubsetIndicator") // || MetaObject.Class == "Employee" || MetaObject.Class == "Person")
                return;

            SkipsUndoManager = true;
            string filename = "";

            #region NotVisibleSetting
            if (Document is MetaBuilder.Graphing.Containers.NormalDiagram)
            {
                if (!(Document as MetaBuilder.Graphing.Containers.NormalDiagram).ShowObjectImages)
                {
                    //reset label location and width
                    foreach (GoObject obj in this)
                    {
                        if (obj is BoundLabel)
                        {
                            if ((obj as BoundLabel).ddf == 0 && (obj as BoundLabel).Name.Contains("Def_") && obj.Left > (this.Grid.Left + 15))
                            {
                                if (this.MetaObject.Class == "GovernanceMechanism" || this.MetaObject.Class == "ResourceType")
                                    (obj as BoundLabel).Position = new PointF(this.Grid.Left + 10, (obj as BoundLabel).Position.Y);
                            }
                            else if ((obj as BoundLabel).Text == this.MetaObject.ToString() || (obj as BoundLabel).Text.Replace(" ", "") == (this.MetaObject.Class + "Name"))
                            {
                                if ((obj as BoundLabel).Name.Contains("Def_") && (obj as BoundLabel).ddf == 0)
                                    continue;

                                if (MetaObject.Class == "Object" || MetaObject.Class.Contains("Location")) //those half triangle shapes
                                {
                                    (obj as BoundLabel).Width = this.Grid.Width - 40;
                                }
                                else
                                {
                                    (obj as BoundLabel).Width = this.Grid.Width - 20;
                                }

                                //(obj as BoundLabel).Width = this.Width - imgWidth - 20;
                                (obj as BoundLabel).Position = new PointF(this.Grid.Left + 10 + 1, (obj as BoundLabel).Position.Y);
                                //(obj as BoundLabel).Multiline = true; OVERRIDER ENTER TO END EDIT
                                //break;
                            }
                            else
                            {
                                (obj as BoundLabel).ToString();
                            }
                        }
                        //make images invisible and bot printable
                        else if (obj is GoImage)
                        {
                            obj.Visible = false;
                            obj.Printable = false;
                        }
                    }

                    //gtfo
                    SkipsUndoManager = false;
                    return;
                }
            }
            #endregion

            if (DataRepository.Connections.ContainsKey(Core.Variables.Instance.ClientProvider))
            {
                #region Class URI

                if (Variables.Instance.ImageCache.ContainsKey(strings.replaceApostrophe(MetaObject.Class.ToString())))
                {
                    filename = Variables.Instance.ImageCache[strings.replaceApostrophe(MetaObject.Class.ToString())];
                }
                else
                {
                    try
                    {
                        object classdb = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ExecuteScalar(CommandType.Text, "SELECT TOP 1 Uri_ID FROM Class WHERE Name = '" + strings.replaceApostrophe(MetaObject.Class.ToString()) + "' AND Uri_ID > 0 ");

                        if (!(classdb is DBNull) && classdb != null)
                        {
                            int? classURIID = int.Parse(classdb.ToString());
                            if (classURIID != null)
                            {
                                filename = DataRepository.GetUUri(classURIID).FileName.ToString();// .Connections[Core.Variables.Instance.ClientProvider].Provider.UURIProvider.GetBypkid((int)classURIID).FileName.ToString();
                                Variables.Instance.ImageCache.Add(strings.replaceApostrophe(MetaObject.Class.ToString()), filename);
                            }
                        }
                        else
                        {
                            Variables.Instance.ImageCache.Add(strings.replaceApostrophe(MetaObject.Class.ToString()), "");
                        }
                        classdb = null;
                    }
                    catch
                    {
                        //missing column ez prevent crash :)
                    }
                }
                #endregion

                //TList<Field> fields = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.FieldProvider.GetByClass(this.MetaObject.Class);
                //fields.Sort("SortOrder");

                //foreach (Field f in fields)
                //using (TList<Field> fields = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.FieldProvider.GetByClass(this.MetaObject.Class))
                //{
                foreach (Field f in DataRepository.ClientFieldsByClass(this.metaObject.Class))
                {
                    if (f.DataType.Contains(".") || f.IsActive == false) // || !string.IsNullOrEmpty(filename)
                        continue;
                    //string propertyName = pInfo.Name;
                    object propertyValue = MetaObject.Get(f.Name);
                    if (propertyValue != null && propertyValue.ToString().Length > 0) //Type Uri
                    {
                        if (Variables.Instance.ImageCache.ContainsKey(strings.replaceApostrophe(MetaObject.Class.ToString()) + ":" + strings.replaceApostrophe(propertyValue.ToString())) && !string.IsNullOrEmpty(Variables.Instance.ImageCache[strings.replaceApostrophe(MetaObject.Class.ToString()) + ":" + strings.replaceApostrophe(propertyValue.ToString())]))
                        {
                            filename = Variables.Instance.ImageCache[strings.replaceApostrophe(MetaObject.Class.ToString()) + ":" + strings.replaceApostrophe(propertyValue.ToString())];
                        }
                        else
                        {
                            try
                            {
                                object db = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ExecuteScalar(CommandType.Text, "SELECT TOP 1 URI_ID FROM DomainDefinitionPossibleValue WHERE PossibleValue = '" + strings.replaceApostrophe(propertyValue.ToString()) + "' AND URI_ID > 0 AND DomainDefinitionID IN (SELECT pkid FROM DomainDefinition WHERE Name = '" + f.DataType + "')");

                                if (db is DBNull || db == null)
                                {
                                    if (!Variables.Instance.ImageCache.ContainsKey(strings.replaceApostrophe(MetaObject.Class.ToString()) + ":" + strings.replaceApostrophe(propertyValue.ToString())))
                                        Variables.Instance.ImageCache.Add(strings.replaceApostrophe(MetaObject.Class.ToString()) + ":" + strings.replaceApostrophe(propertyValue.ToString()), "");
                                    continue;
                                }
                                int? URIID = int.Parse(db.ToString());
                                db = null;
                                if (URIID != null)
                                {
                                    filename = DataRepository.GetUUri(URIID).FileName.ToString();// DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.UURIProvider.GetBypkid((int)URIID).FileName.ToString();
                                    if (!Variables.Instance.ImageCache.ContainsKey(strings.replaceApostrophe(MetaObject.Class.ToString()) + ":" + strings.replaceApostrophe(propertyValue.ToString())))
                                        Variables.Instance.ImageCache.Add(strings.replaceApostrophe(MetaObject.Class.ToString()) + ":" + strings.replaceApostrophe(propertyValue.ToString()), filename);
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.WriteLog(ex.ToString());
                            }
                        }
                    }
                }
                //}
                //fields = null;
                //fields.Dispose();
            }
            else //VIEWER
            {
                if (ImageFilename != null && ImageFilename != "")
                {
                    filename = System.Windows.Forms.Application.StartupPath + "\\MetaData\\Images\\" + strings.GetFileNameOnly(ImageFilename);
                    //return;
                }
            }

            float imgWidth = 40;
            bool hasImage = false;
            GoImage img = null;
            float metapropHeightsubtraction = 0;
            if (MetaPropList != null && MetaPropList.Visible)
                metapropHeightsubtraction = MetaPropList.Height;
            foreach (GoObject obj in this)
            {
                if (obj is GoImage)
                {
                    img = (obj as GoImage);
                    changeImageOnStencilBecausePropertyChanged(img, filename);
                    img.Size = new SizeF(imgWidth, imgWidth);
                    if (this is CollapsibleNode)// (this.metaObject.Class == "DataTable")
                    {
                        if (this.MetaObject.Class == "DataTable")
                            img.Location = new PointF(this.Grid.Left + 10, this.Grid.Top + 10);
                        else
                            img.Location = new PointF(this.Grid.Left + 10, this.Grid.Top + 30);
                    }
                    else
                    {
                        if (this.MetaObject.Class == "DataSubjectArea")
                            img.Location = new PointF(this.Grid.Left + 20, this.Grid.Top + ((this.Grid.Height - metapropHeightsubtraction) / 2) - 10);
                        else
                            img.Location = new PointF(this.Grid.Left + 10, this.Grid.Top + ((this.Grid.Height - metapropHeightsubtraction) / 2) - 10);
                    }
                    img.DragsNode = true;
                    img.Selectable = false;
                    img.Printable = true;
                    hasImage = true;
                    break; //break so we dont change more than 1 image
                }
            }
            if (!hasImage && filename.Length > 0) //This adds an image to the node if it does not have 1 already
            {
                img = new GoImage();
                img.DragsNode = true;
                img.Selectable = false;
                img.Printable = true;
                changeImageOnStencilBecausePropertyChanged(img, filename);
                img.Size = new SizeF(imgWidth, imgWidth);
                if (this is CollapsibleNode)// (this.metaObject.Class == "DataTable")
                {
                    if (this.MetaObject.Class == "DataTable")
                        img.Location = new PointF(this.Grid.Left + 10, this.Grid.Top + 10);
                    else
                        img.Location = new PointF(this.Grid.Left + 10, this.Grid.Top + 30);
                }
                else
                {
                    if (this.MetaObject.Class == "DataSubjectArea")
                        img.Location = new PointF(this.Grid.Left + 20, this.Grid.Top + ((this.Grid.Height - metapropHeightsubtraction) / 2) - 10);
                    else
                        img.Location = new PointF(this.Grid.Left + 10, this.Grid.Top + ((this.Grid.Height - metapropHeightsubtraction) / 2) - 10);
                }
                hasImage = true;
                this.Add(img);
            }

            if (hasImage)
            {
                img.Visible = true;
                img.Printable = true;
            }

            if (hasImage && this.MetaObject.Class != "DataView" && this.MetaObject.Class != "DataSubjectArea" && this.MetaObject.Class != "DataSchema" && this.MetaObject.Class != "Employee" && this.MetaObject.Class != "Person" && this.MetaObject.Class != "PhysicalInformationArtefact" && this.MetaObject.Class != "LogicalInformationArtefact" && this.MetaObject.Class != "DataDomain")
            {
                //move label to make space for the image
                foreach (GoObject obj in this)
                {
                    if (obj is BoundLabel)
                    {
                        if ((obj as BoundLabel).ddf == 0 && (obj as BoundLabel).Name.Contains("Def_") && obj.Left > (this.Grid.Left + 15))
                        {
                            if (this.MetaObject.Class == "GovernanceMechanism" || this.MetaObject.Class == "ResourceType")
                                (obj as BoundLabel).Position = new PointF(this.Grid.Left + 10, (obj as BoundLabel).Position.Y);
                        }
                        else if ((obj as BoundLabel).Text == this.MetaObject.ToString() || (obj as BoundLabel).Text.Replace(" ", "") == (this.MetaObject.Class + "Name"))
                        {
                            if ((obj as BoundLabel).Name.Contains("Def_") && (obj as BoundLabel).ddf == 0)
                                continue;
                            if (filename.Length == 0)
                            {
                                if (img != null)
                                    this.Remove(img);
                                hasImage = false;
                                break;
                            }

                            if (MetaObject.Class == "Object" || MetaObject.Class.Contains("Location")) //those half triangle shapes
                            {
                                (obj as BoundLabel).Width = this.Grid.Width - imgWidth - 40;
                            }
                            else
                            {
                                (obj as BoundLabel).Width = this.Grid.Width - imgWidth - 20;
                            }

                            //(obj as BoundLabel).Width = this.Width - imgWidth - 20;
                            (obj as BoundLabel).Position = new PointF(this.Grid.Left + imgWidth + 10 + 1, (obj as BoundLabel).Position.Y);
                            //(obj as BoundLabel).Multiline = true; OVERRIDER ENTER TO END EDIT
                            //break;
                        }
                    }
                }
            }
            else if (this.MetaObject.Class == "Employee" && this.MetaObject.Class == "Person")
            {

            }
            if (filename.Length == 0 && this.MetaObject.Class != "DataView" && this.MetaObject.Class != "DataSubjectArea" && this.MetaObject.Class != "DataSchema" && this.MetaObject.Class != "Employee" && this.MetaObject.Class != "Person" && this.MetaObject.Class != "PhysicalInformationArtefact" && this.MetaObject.Class != "LogicalInformationArtefact" && this.MetaObject.Class != "DataDomain")
            {
                bool isGradientTriangle = false;
                foreach (GoObject obj in this)
                {
                    if (obj is Shapes.Primitives.GradientTriangle && this.MetaObject.Class == "GovernanceMechanism") //Control
                    {
                        isGradientTriangle = true;
                        //Log.WriteLog("GraphNode::BindMetaObjectImage::(GovernanceMechanism)-" + this.ToString() + " as [Control] detected");
                    }

                    if (obj is BoundLabel)
                    {
                        if (isGradientTriangle) //Control
                        {
                            (obj as BoundLabel).AutoResizes = true;
                            (obj as BoundLabel).Position = new PointF(this.Grid.Left + 23, (obj as BoundLabel).Position.Y);
                            BindBoundsToStringWidth(obj as BoundLabel);
                        }
                        else
                        {
                            if ((obj as BoundLabel).ddf == 0 && (obj as BoundLabel).Name.Contains("Def_") && obj.Left > (this.Grid.Left + 15))
                            {
                                if (this.MetaObject.Class == "GovernanceMechanism" || this.MetaObject.Class == "ResourceType")
                                    (obj as BoundLabel).Position = new PointF(this.Grid.Left + 10, (obj as BoundLabel).Position.Y);
                            }
                            else if ((obj as BoundLabel).Text == this.MetaObject.ToString() || (obj as BoundLabel).Text.Replace(" ", "") == (this.MetaObject.Class + "Name") || (obj as BoundLabel).Text.Replace(" ", "") == (this.MetaObject.Class))
                            {
                                if ((obj as BoundLabel).Name.Contains("Def_") && (obj as BoundLabel).ddf == 0)
                                    continue;
                                if (MetaObject.Class == "Object" || MetaObject.Class.Contains("Location"))
                                {
                                    (obj as BoundLabel).Width = this.Grid.Width - 40;
                                }
                                else
                                {
                                    (obj as BoundLabel).Width = this.Grid.Width - 20;
                                }
                                (obj as BoundLabel).Position = new PointF(this.Grid.Left + 10, (obj as BoundLabel).Position.Y);
                                //(obj as BoundLabel).Multiline = true; OVERRIDE ENTER TO END EDIT
                                //break;
                            }
                        }
                    }
                }
            }
            else if (this.MetaObject.Class == "Employee" && this.MetaObject.Class == "Person")
            {

            }
            if (img != null)
            {
                img.Shadowed = false;
            }
            SkipsUndoManager = false;
        }

        private string imageFilename;
        public string ImageFilename
        {
            get { return imageFilename; }
            set { imageFilename = value; }
        }

        [NonSerialized]
        public bool loadedImage = false; //Used when opening or changing back from black and white (setting to false causes image to reload from disk)

        public void changeImageOnStencilBecausePropertyChanged(GoImage image, string newImagePath)
        {
            //SizeF oldSize = image.Size;
            //Hack for x86/64
            if (!System.IO.File.Exists(newImagePath))
            {
                if (System.IO.File.Exists(newImagePath.Replace(" (x86)", "")))
                {
                    newImagePath = newImagePath.Replace(" (x86)", "");
                }
                else if (System.IO.File.Exists(newImagePath.Replace("Program Files", "Program Files (x86)")))
                {
                    newImagePath = newImagePath.Replace("Program Files", "Program Files (x86)");
                }
            }

            if (newImagePath == ImageFilename && loadedImage) //dont change the image if we dont have to
                return;

            loadedImage = true;
            try
            {
                if (!System.IO.File.Exists(newImagePath))
                {
                    image.Image = null;
                    ImageFilename = "";
#if DEBUG
                    image.Image = System.Drawing.Image.FromFile(System.Windows.Forms.Application.StartupPath + "\\Metadata\\Images\\missing.png", true);
                    //image.LoadImage();
#endif
                }
                else
                {
                    ImageFilename = newImagePath;
                    image.Image = Image.FromFile(newImagePath, true);
                    //image.LoadImage();
                }
            }
            catch
            {
            }
        }

        public void MockBind()
        {
            foreach (KeyValuePair<string, string> kvpair in BindingInfo.Bindings)
            {
                BoundLabel label = FindByName(kvpair.Key) as BoundLabel;
                if (label != null)
                    label.Text = kvpair.Value;
            }
        }

        #endregion

        #region Repeater Manipulation

        public List<CollapsingRecordNodeItem> AddRepeaterObjects(Dictionary<MetaObjectKey, MetaBase> childshapes)
        {
            List<CollapsingRecordNodeItem> retval = new List<CollapsingRecordNodeItem>();
            DataTable dt = Singletons.GetAssociationHelper().GetAssociatedObjects(MetaObject.pkid, MetaObject.MachineName);
            DataView dv = dt.DefaultView;
            foreach (RepeaterBindingInfo repeaterBindingInfo in bindingInfo.RepeaterBindings)
            {
                //ohelper.GetObjects(repeaterBindingInfo.Association.ParentClass,this.metaObject.pkid,repeaterBindingInfo.
                dv.RowFilter = "CAid=" + repeaterBindingInfo.Association.ID;
                foreach (DataRowView drv in dv)
                {
                    RepeaterSection section = FindByName(repeaterBindingInfo.RepeaterName) as RepeaterSection;
                    if (section != null)
                    {
                        CollapsingRecordNodeItem node = section.AddItemFromCode() as CollapsingRecordNodeItem;
                        int ChildObjectID = int.Parse(drv["ChildObjectID"].ToString());
                        node.LoadMetaObject(ChildObjectID, drv["ChildObjectMachine"].ToString());
                        node.HookupEvents();
                        node.BindToMetaObjectProperties();

                        childshapes.Add(new MetaObjectKey(node.MetaObject.pkid, node.MetaObject.MachineName),
                                        node.MetaObject);
                        retval.Add(node);
                    }
                }
            }
            return retval;
        }

        #endregion

        #region EventHandling

        [NonSerialized]
        public EventHandler _contentsChanged;

        //[NonSerialized]
        //private bool hooked = false;

        //[NonSerialized]
        //private List<string> hoookedList = new List<string>();
        public void HookupEvents()
        {
            //if (!hooked)
            ////    return;
            if (MetaObject != null)
            {
                //if (MetaObject.IsChangedNull())
                try
                {
                    MetaObject.Changed -= FireMetaObjectChanged;
                }
                catch
                {
                }

                MetaObject.Changed += FireMetaObjectChanged;
                //hooked = true;
                //#if DEBUG
                //                hoookedList.Add(DateTime.Now.ToString() + Environment.NewLine + Environment.StackTrace);
                //#endif
            }

            if (this is CollapsibleNode)
            {
                foreach (RepeaterSection drs in RepeaterSections)
                {
                    RepeaterBindingInfo info = bindingInfo.GetRepeaterBindingInfo(drs.Name);
                    if (info == null)
                    {
                        continue;
                    }
                    foreach (GoObject obj in drs)
                    {
                        if (obj is CollapsingRecordNodeItem)
                        {
                            if (!(obj as CollapsingRecordNodeItem).IsHeader)
                            {
                                (obj as CollapsingRecordNodeItem).HookupEvents();
                            }
                        }
                    }
                }
            }
        }
        public void UnHookEvents()
        {
            //if (!hooked)
            ////    return;
            if (MetaObject != null)
            {
                //if (MetaObject.IsChangedNull())
                try
                {
                    MetaObject.Changed -= FireMetaObjectChanged;
                }
                catch
                {
                }

                //MetaObject.Changed += FireMetaObjectChanged;
                //hooked = true;
                //#if DEBUG
                //                hoookedList.Add(DateTime.Now.ToString() + Environment.NewLine + Environment.StackTrace);
                //#endif
            }

            if (this is CollapsibleNode)
            {
                foreach (RepeaterSection drs in RepeaterSections)
                {
                    RepeaterBindingInfo info = bindingInfo.GetRepeaterBindingInfo(drs.Name);
                    if (info == null)
                    {
                        continue;
                    }
                    foreach (GoObject obj in drs)
                    {
                        if (obj is CollapsingRecordNodeItem)
                        {
                            if (!(obj as CollapsingRecordNodeItem).IsHeader)
                            {
                                (obj as CollapsingRecordNodeItem).UnHookEvents();
                            }
                        }
                    }
                }
            }
        }

        public bool skipautomaticsave = false;
        public void FireMetaObjectChanged(object sender, EventArgs e)
        {
            OnContentsChanged(sender, e);
            BindToMetaObjectProperties();

            #region This is how we hack node updates to change links :( sadpanda
            if (MetaObject != null && MetaObject.Class == "SubsetIndicator")
            {
                foreach (QLink l in DestinationLinks)
                {
                    if (l.AssociationType == LinkAssociationType.SubSetOf)
                    {
                        try
                        {
                            skipautomaticsave = true;
                            if ((l.FromNode as IMetaNode).MetaObject.Get("IsCompleteSet") != null)
                            {
                                if ((l.FromNode as IMetaNode).MetaObject.Get("IsCompleteSet").ToString().ToLower() == "no")
                                {
                                    l.FromArrowStyle = GoStrokeArrowheadStyle.Cross;
                                    l.FromArrow = true;
                                }
                                else
                                {
                                    l.FromArrowStyle = GoStrokeArrowheadStyle.Polygon;
                                    l.FromArrow = false;
                                }
                            }
                            else
                            {
                                l.FromArrowStyle = GoStrokeArrowheadStyle.Polygon;
                                l.FromArrow = false;
                            }
                        }
                        catch
                        {
                            l.FromArrowStyle = GoStrokeArrowheadStyle.Polygon;
                            l.FromArrow = false;
                        }
                    }
                }
            }
            #endregion

            if (Core.Variables.Instance.SaveOnCreate && !skipautomaticsave)
            {
                SaveToDatabase(sender, e);
            }
            skipautomaticsave = false;
            HookupEvents();//? event is being set to null
        }

        public EventHandler ContentsChanged
        {
            get { return _contentsChanged; }
            set { _contentsChanged = value; }
        }
        public virtual void OnContentsChanged(object sender, EventArgs e)
        {
            if (ContentsChanged != null)
            {
                ContentsChanged(this, e);
            }
        }

        protected override void OnObservedChanged(GoObject observed, int subhint, int oldI, object oldVal, RectangleF oldRect, int newI, object newVal, RectangleF newRect)
        {
            switch (subhint)
            {
                case 1501: // Probably Boundlabel's Text that changed
                    if (observed is BoundLabel)
                    {
                        GoCollectionEnumerator obsEnum = observed.Observers.GetEnumerator();
                        //while (obsEnum.MoveNext())
                        //{
                        //    // Console.WriteLine(obsEnum.Current.ToString());
                        //}
                        if (observed.ParentNode != this && observed.Parent != this)
                        {
                            if (!(observed.ParentNode is ValueChain))
                            {
                                observed.RemoveObserver(this);
                                Log.WriteLog("LabelTextChanged returned because parent is not this node");
                                return;
                            }
                        }
                        BoundLabel culprit = observed as BoundLabel;
                        LabelTextChanged(culprit, (string)oldVal, (string)newVal);
                    }
                    break;
            }
            //Manager.OnObservedChanged(observed, subhint, oldI, oldVal, oldRect, newI, newVal, newRect, this);
            base.OnObservedChanged(observed, subhint, oldI, oldVal, oldRect, newI, newVal, newRect);
        }

        public void LabelTextChanged(BoundLabel label, string OldText, string NewText)
        {
            if (MetaObject != null)
            {
                if (bindingInfo != null)
                {
                    foreach (KeyValuePair<string, string> kvpair in bindingInfo.Bindings)
                    {
                        if (kvpair.Key == label.Name)
                        {
                            skipautomaticsave = true;
                            MetaObject.Set(kvpair.Value, NewText);
                        }
                    }
                    if (BindingInfo.Bindings.Count > 0)
                        FireMetaObjectChanged(this, EventArgs.Empty);
                }
            }
        }

        #endregion

        #region MetaObject

        public void CreateMetaObject(object sender, EventArgs args)
        {
            if (HasBindingInfo)
            {
                if (BindingInfo.BindingClass != null)
                {
                    MetaObject = Loader.CreateInstance(BindingInfo.BindingClass);
                }
            }
        }

        public void LoadMetaObject(int ID, string Machine)
        {
            if (HasBindingInfo)
            {
                if (BindingInfo.BindingClass != null)
                {
                    MetaObject = Loader.GetByID(bindingInfo.BindingClass, ID, Machine);
                    HookupEvents();
                    BindToMetaObjectProperties();
                    //FireMetaObjectChanged(null, EventArgs.Empty);
                }
            }
        }

        public TList<ObjectAssociation> SaveToDatabase(object sender, EventArgs e)
        {
            return SaveToDatabase(sender, e, Core.Variables.Instance.ClientProvider);
        }

        public TList<ObjectAssociation> SaveToDatabase(object sender, EventArgs e, string Provider)
        {
            //if (Variables.Instance.VerboseLogging)
            //    Log.WriteLog("save to database with VCUSER:" + metaObject.VCUser, "GraphNode Save Initiated", System.Diagnostics.TraceEventType.Information);
            if (Provider.Length == 0)
            {
                Provider = Core.Variables.Instance.ClientProvider;
            }
            RequiresAttention = false;
            if (MetaObject != null)
            {
                try
                {
                    if (e == EventArgs.Empty)
                        MetaObject.SaveToRepository(Guid.NewGuid(), Provider);
                    //Log.WriteLog("saved");
                }
                catch
                {
                    // Console.WriteLine("GraphNode cannot save properly " + x.ToString());
                    try
                    {
                        MetaObject.pkid = 0;
                        try
                        {
                            if (e == EventArgs.Empty)
                                MetaObject.SaveToRepository(Guid.NewGuid(), Provider);
                            Log.WriteLog("saved after pkid=0");
                        }
                        catch (Exception wException)
                        {
                            MetaObject.WorkspaceName = Variables.Instance.CurrentWorkspaceName;
                            MetaObject.WorkspaceTypeId = Variables.Instance.CurrentWorkspaceTypeId;
                            MetaObject.SaveToRepository(Guid.NewGuid(), Provider);

                            //ADDED
                            LogEntry logEntry = new LogEntry();
                            logEntry.Message = "Difference in metamodels between diagram and database. Third attempt. After setting workspace to current" + Environment.NewLine + wException.ToString();
                            if (Variables.Instance.VerboseLogging)
                                Logger.Write(logEntry);
                        }
                    }
                    catch (Exception sException)
                    {
                        LogEntry logEntry = new LogEntry();
                        logEntry.Message = "Difference in metamodels between diagram and database. Second Attempt. PKID = 0" + Environment.NewLine + sException.ToString();
                        if (Variables.Instance.VerboseLogging)
                            Logger.Write(logEntry);
                        return null;
                    }
                }

                //seperated to prevent save exception
                TList<ObjectAssociation> assocsToSave = new TList<ObjectAssociation>();
                TList<ObjectAssociation> assocsToUpdate = new TList<ObjectAssociation>();
                TList<ObjectAssociation> assocsSaved = new TList<ObjectAssociation>();

                #region Complex Node Assocation

                //if (!Core.Variables.Instance.SaveOnCreate) //only when you edit the child?
                foreach (RepeaterSection drs in RepeaterSections)
                {
                    RepeaterBindingInfo info = bindingInfo.GetRepeaterBindingInfo(drs.Name);
                    if (info == null)
                    {
                        Log.WriteLog("Graphnode::SaveToDatabase::MissingRepeaterInfo::" + this.MetaObject.ToString() + "::" + drs.Name);
                        continue;
                    }
                    foreach (GoObject obj in drs)
                    {
                        if (obj is CollapsingRecordNodeItem)
                        {
                            CollapsingRecordNodeItem item = obj as CollapsingRecordNodeItem;
                            if (!item.IsHeader || item.MetaObject != null)
                            {
                                if (item.CopiedFrom != null && item.CopiedFrom.pkid != 0)
                                {
                                    item.MetaObject = item.CopiedFrom;
                                    item.SaveToDatabase(sender, e, Provider);
                                }
                                else
                                {
                                    item.SaveToDatabase(sender, e, Provider);
                                }
                                if (item.MetaObject != null)
                                {
                                    //Singletons.GetAssociationHelper().AddQuickAssociation(this.metaObject.pkid, dnode.metaObject.pkid, info.Association.ID);
                                    ObjectAssociation oass = new ObjectAssociation();
                                    if (info.Association.ID.ToString() == "666" || info.Association.ID.ToString() == "667" || info.Association.ID.ToString() == "668" || info.Association.ID.ToString() == "669" || info.Association.ID.ToString() == "670" || info.Association.ID.ToString() == "671" || info.Association.ChildClass == "DataField" || info.Association.ChildClass == "DataColumn" || info.Association.ChildClass == "DataAttribute" || info.Association.ChildClass == "Attribute")// && !MetaObject.Class.ToLower().Contains("artefact")
                                    {
                                        if (info.Association.ChildClass == "DataColumn")
                                            info.Association.ChildClass = "DataField";
                                        else if (info.Association.ChildClass == "Attribute")
                                            info.Association.ChildClass = "DataAttribute";

                                        TList<ClassAssociation> associations = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.Find("ParentClass='" + MetaObject.Class + "' AND ChildClass='" + info.Association.ChildClass);
                                        foreach (ClassAssociation classAssociation in associations)
                                        {
                                            if (classAssociation.IsActive == true && classAssociation.AssociationTypeID == 4)
                                            {
                                                if (info.RepeaterName.Contains("Key"))
                                                {
                                                    if (classAssociation.Caption.Contains("Key"))
                                                    {
                                                        oass.CAid = classAssociation.CAid;
                                                        break;
                                                    }
                                                }
                                                else if (info.RepeaterName.Contains("Descrip"))
                                                {
                                                    if (classAssociation.Caption.Contains("Descrip"))
                                                    {
                                                        oass.CAid = classAssociation.CAid;
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    oass.CAid = classAssociation.CAid;
                                                }
                                            }
                                        }
                                        associations.Dispose();
                                    }
                                    else if (info.Association.ID.ToString() == "666" || info.Association.ID.ToString() == "667" || info.Association.ID.ToString() == "668" || info.Association.ID.ToString() == "669" || info.Association.ID.ToString() == "670" || info.Association.ID.ToString() == "671")
                                    {
                                        Association a = Singletons.GetAssociationHelper().GetAssociation(MetaObject.Class, item.MetaObject.Class, 4);
                                        if (a != null)
                                        {
                                            oass.CAid = a.ID;
                                        }
                                        else
                                        {
                                            Log.WriteLog("GraphNode::SaveToDatabase-" + MetaObject.Class + " to " + item.MetaObject.Class + " Mapping cannot be found");
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        oass.CAid = info.Association.ID;
                                    }
                                    oass.ObjectID = MetaObject.pkid;
                                    oass.ChildObjectID = item.MetaObject.pkid;
                                    oass.ObjectMachine = MetaObject.MachineName;
                                    oass.ChildObjectMachine = item.MetaObject.MachineName;

                                    ObjectAssociation existingComplexAss = DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(oass.CAid, oass.ObjectID, oass.ChildObjectID, oass.ObjectMachine, oass.ChildObjectMachine);
                                    if (existingComplexAss == null)
                                    {
                                        oass.VCStatusID = 7;
                                        assocsToSave.Add(oass);
                                    }
                                    else
                                    {
                                        if (existingComplexAss.State == VCStatusList.MarkedForDelete && MetaObject.WorkspaceTypeId < 3)
                                            existingComplexAss.VCStatusID = (int)VCStatusList.None;
                                        //else
                                        //    oass.VCStatusID = existingComplexAss.VCStatusID;

                                        assocsToUpdate.Add(existingComplexAss);
                                    }
                                }
                            }
                        }
                    }
                }

                //save
                foreach (ObjectAssociation ass in assocsToSave)
                {
                    try
                    {
                        DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.Save(ass);
                        assocsSaved.Add(ass);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteLog("Could not save complex node association" + Environment.NewLine + ass.ToString() + Environment.NewLine + ex.ToString());
                    }
                }
                //update
                foreach (ObjectAssociation ass in assocsToUpdate)
                {
                    try
                    {
                        DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.Save(ass);
                        assocsSaved.Add(ass);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteLog("Could not update complex node association" + Environment.NewLine + ass.ToString() + Environment.NewLine + ex.ToString());
                    }
                }
                #endregion

                SaveReference(sender, e, Provider);

                //List<GoObject> remove = new List<GoObject>();
                //foreach (GoObject o in this)
                //    if (o is IndicatorLabel)
                //        if ((o as IndicatorLabel).TextColor == Color.Red)
                //        {
                //            remove.Add(o);
                //        }
                //foreach (GoObject o in remove)
                //    o.Remove();

                return assocsSaved; //returns all saved/updated associations only

            }
            return null;
        }

        private void SaveReference(object sender, EventArgs e, string Provider)
        {
            if (ReferenceNodes != null)
            {
                foreach (GraphNode ReferenceNode in ReferenceNodes)
                {
                    ReferenceNode.SaveToDatabase(sender, e, Provider);

                    ObjectAssociation referenceAssociation = new ObjectAssociation();

                    referenceAssociation.CAid = Singletons.GetAssociationHelper().GetAssociationTypeForParentChildClass(ReferenceNode.MetaObject.Class, this.MetaObject.Class, ReferenceAssociation);

                    referenceAssociation.ObjectID = this.MetaObject.pkid;
                    referenceAssociation.ObjectMachine = this.MetaObject.MachineName;

                    referenceAssociation.ChildObjectID = ReferenceNode.MetaObject.pkid;
                    referenceAssociation.ChildObjectMachine = ReferenceNode.MetaObject.MachineName;

                    referenceAssociation.Machine = Environment.MachineName;

                    try
                    {
                        if (referenceAssociation.CAid > 0)
                            DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.Save(referenceAssociation);
                    }
                    catch
                    {
                        //exists
                    }
                }
                ReferenceNodes = null;
            }
        }

        [NonSerialized]
        public List<GraphNode> ReferenceNodes;
        [NonSerialized]
        public LinkAssociationType ReferenceAssociation;

        //public void SaveToDatabase(object sender, EventArgs e, string Provider)
        //{
        //    if (Provider == null)
        //    {
        //        Provider = Core.Variables.Instance.ClientProvider;
        //    }
        //    RequiresAttention = false;
        //    if (metaObject != null)
        //    {
        //        try
        //        {
        //            metaObject.SaveToRepository(Guid.NewGuid(), Provider);
        //        }
        //        catch 
        //        {
        //            // Console.WriteLine("GraphNode cannot save properly " + x.ToString());
        //            try
        //            {
        //                metaObject._pkid = 0;
        //                try
        //                {
        //                    metaObject.SaveToRepository(Guid.NewGuid(), Provider);
        //                }
        //                catch
        //                {
        //                    metaObject.WorkspaceName = Variables.Instance.CurrentWorkspaceName;
        //                    metaObject.WorkspaceTypeId = Variables.Instance.CurrentWorkspaceTypeId;
        //                    metaObject.SaveToRepository(Guid.NewGuid(), Provider);
        //                }
        //            }
        //            catch
        //            {
        //                LogEntry logEntry = new LogEntry();
        //                logEntry.Message = "Seems to be a difference in metamodels between diagram and database";
        //                Logger.Write(logEntry);
        //                return;
        //            }
        //        }
        //        TList<ObjectAssociation> assocsToSave = new TList<ObjectAssociation>();


        //        foreach (RepeaterSection drs in RepeaterSections)
        //        {
        //            RepeaterBindingInfo info = bindingInfo.GetRepeaterBindingInfo(drs.Name);
        //            foreach (GoObject obj in drs)
        //            {
        //                if (obj is CollapsingRecordNodeItem)
        //                {
        //                    CollapsingRecordNodeItem item = obj as CollapsingRecordNodeItem;
        //                    if (!item.IsHeader || item.MetaObject != null)
        //                    {
        //                        item.SaveToDatabase(sender, e, Provider);
        //                        if (item.MetaObject != null)
        //                        {
        //                            //Singletons.GetAssociationHelper().AddQuickAssociation(this.metaObject.pkid, dnode.metaObject.pkid, info.Association.ID);
        //                            ObjectAssociation oass = new ObjectAssociation();
        //                            oass.CAid = info.Association.ID;
        //                            oass.ObjectID = metaObject.pkid;
        //                            oass.ChildObjectID = item.MetaObject.pkid;
        //                            oass.ObjectMachine = metaObject.MachineName;
        //                            oass.ChildObjectMachine = item.MetaObject.MachineName;
        //                            oass.VCStatusID = 1;
        //                            if (DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(oass.CAid,oass.ObjectID,oass.ChildObjectID,oass.ObjectMachine,oass.ChildObjectMachine) ==null)
        //                                assocsToSave.Add(oass);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        try
        //        {
        //            DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.Save(assocsToSave);
        //        }
        //        catch
        //        {
        //        }
        //    }
        //}

        #endregion

        #region GoNode overrides

        // make sure the GraphNode's Label is positioned above the top-left corner of the Grid;
        // this makes it potentially useful for selection and dragging even if all of the
        // grid is covered by Items
        public override void LayoutChildren(GoObject childchanged)
        {
            if (EditMode)
            {
                if ((Count > 1) && (AutoSizeGrid))
                {
                    if (Grid != null)
                    {
                        GraphNodeGrid grid = Grid as GraphNodeGrid;
                        if (grid.AutoSize && grid.Visible)
                        {
                            if (childchanged is GoText)
                            {
                                GoText txt = childchanged as GoText;
                                if (txt.Maximum == 1999)
                                {
                                    // return;
                                }
                            }
                            CalculateGridSize();
                            //Grid.Bounds = new RectangleF(Grid.Position, new SizeF(0, 0));
                            //Grid.Bounds = ComputeBounds();
                        }
                    }
                }
                CalculateGridSize();
                //Grid.Bounds = new RectangleF(Grid.Position, new SizeF(0, 0));
                //Grid.Bounds = ComputeBounds();
            }
            // 31 OCT (bit below)
            /* if (!Initializing)
            {
                GoObject currentWinner = null;
                
                foreach (GoObject oContender in this)
                {

                    if (currentWinner == null)
                    {
                        currentWinner = oContender;
                    }
                    else
                    {
                        float oCalced = oContender.Size.Width * oContender.Height;
                        if (oCalced > currentWinner.Size.Width * currentWinner.Size.Height && currentWinner != oContender)
                        {
                            currentWinner = oContender;
                        }
                    }
                    this.Port.PortObject = currentWinner;
                    this.Port.Bounds = currentWinner.Bounds;
                    this.Port.Bounds.Inflate(5, 5);
                    this.ComputeBounds();
                }
            }*/

            base.LayoutChildren(childchanged);
        }

        public bool overrideshadowset = false;
        public override bool Shadowed
        {
            get
            {
                return base.Shadowed;
            }
            set
            {
                if (Shadowed == true && value == false)
                {
                    //Cancel setting a shadow to false when it was true
                    if (MetaObject.pkid > 0)
                        if (!overrideshadowset)
                            return;
                }
                foreach (GoObject obj in this)
                {
                    if (obj is GoImage)
                    {
                        (obj as GoImage).Shadowed = false;
                    }
                }
                overrideshadowset = false;
                base.Shadowed = value;
            }
        }

        public float widthBefore = 0;
        float rightBefore = 0;
        float bottomBefore = 0;

        public void CalculateGridSize()
        {
            if (Grid != null)
            {
                float fMinY = Grid.Top;
                float fMinX = Grid.Left;
                float fMaxY = 0;
                float fMaxX = 0;
                foreach (GoObject o in this)
                {
                    if (o != Grid && o.Visible)
                    {
                        if (o.Top < fMinY)
                            fMinY = o.Top;

                        if (o.Left < fMinX)
                            fMinX = o.Left;

                        if (o.Right > fMaxX)
                            fMaxX = o.Right;

                        if (o.Bottom > fMaxY)
                            fMaxY = o.Bottom;
                    }
                }

                //Grid.Bounds = new RectangleF(new PointF(Grid.Position.X, fMinY), new SizeF(0, 0));
                Grid.Bounds = new RectangleF(new PointF(fMinX, fMinY), new SizeF(fMaxX - fMinX, fMaxY - fMinY));
                Grid.Bounds = ComputeBounds();
            }
        }

        #endregion

        #region Highlighting

        public static readonly Pen StandardPen = new Pen(Color.Black, 2);
        public static readonly Pen HighlightPen = new Pen(Color.Fuchsia, 2);
        // visually distinguish this Display from others,
        // to indicate that it can (or just did) add an Item
        public void SetHighlight(bool show)
        {
            if (EditMode)
            {
                GoGrid grid = Grid;
                if (grid != null)
                {
                    if (show)
                    {
                        grid.Pen = HighlightPen;
                    }
                    else
                    {
                        grid.Pen = StandardPen;
                    }
                }
            }
        }

        #endregion

        #region Locking, Unlocking shapes for dragging

        public void LockShapes()
        {
            SetChildrenDraggingState(true);
        }

        public void UnlockShapes()
        {
            SetChildrenDraggingState(false);
        }

        private void SetChildrenDraggingState(bool ChildrenDragNode)
        {
            if (Count > 1)
            {
                for (int i = 1; i < Count; i++)
                {
                    this[i].DragsNode = ChildrenDragNode;
                    this[i].Deletable = !ChildrenDragNode;
                }
            }
            //Grid.Selectable = false;
        }

        #endregion

        //public bool HasVisualIndicator
        //{
        //    get { return hasVisualIndicator; }
        //    set { hasVisualIndicator = value; }
        //}

        public override bool Copyable
        {
            get
            {
                //if (RequiresAttention)
                //    return false;
                return true; // BYPASS SHADOWED ISSUE! base.Copyable;
            }
            set { base.Copyable = value; }
        }

        public bool MetaObjectLocked
        {
            get
            {
                if (MetaObject != null)
                {
                    if (VCStatusTool.UserHasControl(MetaObject))
                    {
                        return false;
                    }
                    return true;
                }
                return false;
            }
        }

        public override bool DragsNode
        {
            get
            {
                if (Parent is ShapeGroup)
                {
                    return true;
                }
                return false; // return base.DragsNode;
            }
            set { base.DragsNode = value; }
        }

        #region IGoCollapsible Members

        public void Collapse()
        {
        }

        public void Expand()
        {
        }

        public bool Collapsible
        {
            get { return collapsible; }
            set { collapsible = value; }
        }

        public bool IsExpanded
        {
            get { return true; }
        }

        #endregion

        #region IMetaNode Members

        public bool RequiresAttention
        {
            get { return requiresAttention; }
            set
            {
                if (requiresAttention == value)
                    return;
                requiresAttention = value;
                try
                {
                    Color c = Color.Red;
                    CalculateGridSize();
                    //Grid.Visible = true;
                    Grid.Brush = new SolidBrush(Color.FromArgb(125, c.R, c.G, c.B));
                    Grid.Printable = false;

                    Grid.Visible = value;
                }
                catch
                {
                    //Invalid operationexception
                    //only when you close and choose to save while there are duplicates on the diagram
                }
            }
        }

        #endregion

        #region IShallowCopyable Members

        public bool CopyAsShadow
        {
            get { return copyAsShadow; }
            set { copyAsShadow = value; }
        }

        public GoObject CopyAsShallow()
        {
            MetaBase mo = MetaObject;
            GraphNode gnode = Copy() as GraphNode;
            gnode.MetaObject = mo;
            gnode.HookupEvents();
            gnode.BindToMetaObjectProperties();
            return gnode;
        }

        #endregion

        public override GoObject CopyObject(GoCopyDictionary env)
        {
            env.Delayeds.Add(this);
            GoObject retval = base.CopyObject(env);

            Collection<GoObject> removeThese = new Collection<GoObject>();
            foreach (DictionaryEntry o in env)
            {
                if (o.Value.GetType().ToString().Contains("NumberingText") || o.Value.GetType().ToString().Contains("IndicatorLabel"))
                {
                    removeThese.Add(o.Value as GoObject);
                }
            }
            foreach (GoObject o in removeThese)
            {
                o.Remove();
                env.Remove(o);
                env.Delayeds.Remove(o);
            }
            removeThese = null;

            GraphNode node = retval as GraphNode;
            node.RequiresAttention = false;
            node.BindingInfo = this.BindingInfo.Copy();
            node.CopiedFrom = this.MetaObject;
            //node.Shadowed = false;

            HookupEvents();
            node.HookupEvents();
            //node.ImageFilename = ImageFilename;
            //node.BindToMetaObjectProperties();

            if (CopyAsShadow)
            {
                if (AllocationHandle != null && node.AllocationHandle != null)
                {
                    node.AllocationHandle.Items = new Collection<AllocationHandle.AllocationItem>();

                    foreach (AllocationHandle.AllocationItem i in AllocationHandle.Items)
                        node.AllocationHandle.Items.Add(i);
                }
                node.MetaObject = MetaObject;
                node.HookupEvents();
                node.CopyAsShadow = true;

                return node;
            }
            return retval;
        }

        public void CopyAllocationToNode(GraphNode node)
        {
            if (AllocationHandle != null && node.AllocationHandle != null)
            {
                node.AllocationHandle.Items = new Collection<AllocationHandle.AllocationItem>();

                foreach (AllocationHandle.AllocationItem i in AllocationHandle.Items)
                    node.AllocationHandle.Items.Add(i);

                node.AllocationHandle.SetStyle();
            }
        }

        public override void CopyObjectDelayed(GoCopyDictionary env, GoObject newobj)
        {
            GraphNode node = newobj as GraphNode;

            Collection<GoObject> removeThese = new Collection<GoObject>();
            foreach (DictionaryEntry o in env)
            {
                if (o.Value.GetType().ToString().Contains("NumberingText") || o.Value.GetType().ToString().Contains("IndicatorLabel"))
                {
                    removeThese.Add(o.Value as GoObject);
                }
            }
            foreach (GoObject o in removeThese)
            {
                o.Remove();
                env.Remove(o);
                env.Delayeds.Remove(o);
            }
            removeThese = null;

            node.RequiresAttention = false;
            if (node.MetaObject != null)
            {
                node.MetaObject.Changed -= FireMetaObjectChanged;
                node.MetaObject = null;
            }

            base.CopyObjectDelayed(env, node);
            if (bindingInfo != null)
                node.BindingInfo = BindingInfo.Copy();

            if (MetaObject == null)
            {
                CreateMetaObject(this, null);
            }
            //node.Shadowed = false;
            if (CopyAsShadow)
            {
                node.MetaObject = MetaObject;
                node.CopyAsShadow = true;
                CopyAsShadow = false;
            }
            else
            {
                node.CreateMetaObject(null, EventArgs.Empty);
                if (node.MetaObject != null && MetaObject != null)
                    MetaObject.CopyPropertiesToObject(node.MetaObject);
            }

            node.HookupEvents();
            node.Name = Name.Substring(0, Name.Length);

            //what if you shallow copy an object which was on a diagram that has been saved after you clear the database?
            //if (metaObject != null)
            //    if (metaObject.pkid > 0)
            //        node.metaObject.IsInDatabase = true;

            //node.HookupEvents();
            //node.ImageFilename = ImageFilename;
            //node.BindToMetaObjectProperties();
        }

        public override void Remove(GoObject obj)
        {
            base.Remove(obj);

            if (obj is IndicatorLabel || obj is ChangedIndicatorLabel)
                return;

            if (Grid != null)
            {
                GraphNodeGrid grid = Grid as GraphNodeGrid;
                if (grid.AutoSize)
                {
                    CalculateGridSize();
                    //Grid.Bounds = ComputeBounds();
                }
            }
            CalculateGridSize();
        }

        #region IIdentifiable Implementation

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        #endregion

        //public override int Count
        //{
        //    get
        //    {
        //        int x = 0;
        //        bool foundAnIndicator = false;
        //        foreach (GoObject o in this)
        //        {
        //            if (!(o is IndicatorLabel))
        //                x++;
        //            else
        //                foundAnIndicator = true;
        //        }

        //        if (foundAnIndicator == true)
        //            return x;

        //        return base.Count;
        //    }
        //}

        private float metapropwidth = 300;
        public void InitMetaProps(bool reposition)
        {
            if (widthBefore == 0)
            {
                widthBefore = this.Width;
            }

            rightBefore = Grid.Right;
            bottomBefore = Grid.Bottom;

            if (MetaPropList == null)
            {
                MetaPropertyList mp = new MetaPropertyList();
                mp.Init(MetaObject, metapropwidth);
                if (mp.Rows.Count == 0)
                    return;
                //the additional 10 here is the height sum of all the objects that were saved 'under' the list. Mostly a line of ports which have a height of 10
                PointF newLocation = new PointF(this.Left, !reposition ? (this.Bottom - (mp.Height + 10)) : this.Bottom);
                mp.Location = newLocation;

                Add(mp);
            }

            MetaPropList.Visible = !MetaPropList.Visible;

            if (reposition)
            {
                List<QuickPort> bottomPorts = new List<QuickPort>();

                #region Position
                if (MetaPropList.Visible)
                {
                    foreach (GoObject o in this)
                    {
                        if (o is QuickPort)
                        {
                            if (o.Bottom == bottomBefore)
                            {
                                o.Location = new PointF(o.Location.X + ((metapropwidth - widthBefore) / 2), o.Location.Y + MetaPropList.Height + o.Height);

                                bottomPorts.Add(o as QuickPort);
                            }
                            else if (o.Right + 15 > rightBefore || (o as QuickPort).PortPosition.ToString().StartsWith("Right"))
                            {
                                o.Location = new PointF(o.Location.X + (metapropwidth - widthBefore) + o.Width, o.Location.Y);
                            }
                        }
                        else if (o is Shapes.Behaviours.IBehaviourShape)
                        {
                            o.Width = metapropwidth;
                        }
                        else if (o is BoundLabel)
                        {
                            if ((o as BoundLabel).Name == "cls_id")
                            {
                                o.Location = new PointF(o.Location.X + (metapropwidth - widthBefore), o.Location.Y);
                            }
                            else if (o.Height > 40) //for the main label
                            {
                                o.Width = o.Width + (metapropwidth - widthBefore);
                            }
                        }
                        else if (o is AllocationHandle)
                        {
                            o.Location = new PointF(o.Location.X + (metapropwidth - widthBefore), o.Location.Y);
                        }
                    }

                    LayoutChildren(null);

                    #region reposition bottom ports
                    //            bottomPorts.Sort(delegate(QuickPort p1, QuickPort p2)
                    //{
                    //    return p1.Location.X.CompareTo(p2.Location.x);
                    //});

                    if (!reposition && bottomPorts.Count > 0) //ie opening
                        MetaPropList.Bottom = bottomPorts[0].Top - 5; ;

                    float spacing = metapropwidth / bottomPorts.Count;
                    float customX = spacing / 2;
                    foreach (QuickPort port in bottomPorts)
                    {
                        //port.Location = new PointF(Grid.Left + customX, port.Location.Y);

                        customX += spacing;
                    }
                    #endregion

                    //was null and therefore added
                    //Add(MetaPropList);
                }
                else
                {
                    foreach (GoObject o in this)
                    {
                        if (o is QuickPort)
                        {
                            if (o.Bottom == bottomBefore)
                            {
                                o.Location = new PointF(o.Location.X - ((metapropwidth - widthBefore) / 2), o.Location.Y - MetaPropList.Height - o.Height);

                                bottomPorts.Add(o as QuickPort);
                            }
                            else if (o.Right + 15 > rightBefore || (o as QuickPort).PortPosition.ToString().StartsWith("Right"))
                            {
                                o.Location = new PointF(o.Location.X - (metapropwidth - widthBefore) - o.Width, o.Location.Y);
                            }
                        }
                        else if (o is Shapes.Behaviours.IBehaviourShape)
                        {
                            o.Width = widthBefore - 10;
                        }
                        else if (o is BoundLabel)
                        {
                            if ((o as BoundLabel).Name == "cls_id")
                            {
                                o.Location = new PointF(o.Location.X - (metapropwidth - widthBefore), o.Location.Y);
                            }
                            else if (o.Height > 40) //for the main label
                            {
                                o.Width = widthBefore - 50;
                            }
                        }
                        else if (o is AllocationHandle)
                        {
                            o.Location = new PointF(o.Location.X - (metapropwidth - widthBefore), o.Location.Y);
                        }
                    }

                    #region reposition bottom ports
                    //            bottomPorts.Sort(delegate(QuickPort p1, QuickPort p2)
                    //{
                    //    return p1.Location.X.CompareTo(p2.Location.x);
                    //});
                    float spacing = widthBefore / bottomPorts.Count;
                    float customX = spacing / 2;
                    foreach (QuickPort port in bottomPorts)
                    {
                        //port.Location = new PointF(Grid.Left + customX, port.Location.Y);

                        customX += spacing;
                    }
                    #endregion

                    MetaPropList.Clear();
                    Remove(MetaPropList);
                }
                #endregion
            }
            else
            {
                //do something else when opening?
            }

            CalculateGridSize();
            HookupEvents();
        }

        public MetaPropertyList MetaPropList
        {
            get
            {
                foreach (GoObject o in this)
                    if (o is MetaPropertyList)
                        return o as MetaPropertyList;
                return null;
            }
        }

        public GoPort GetPort(string pos)
        {
            foreach (GoObject o in this)
                if (o is QuickPort)
                    if ((o as QuickPort).PortPosition.ToString() == pos)
                        return o as GoPort;

            return GetDefaultPort;
        }

    }

    [Serializable]
    public class MetaPropertyList : GoListGroup
    {
        public override PointF Location
        {
            get
            {
                return base.Location;
            }
            set
            {
                base.Location = value;
            }
        }

        public MetaPropertyList()
        {
            this.Initializing = true;
            this.Selectable = false;
            this.Resizable = false;
            this.ResizesRealtime = true;
            this.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.Spacing = 0;
            this.Alignment = Middle;
            this.PickableBackground = true;
            this.Brush = Brushes.White;
            this.Copyable = false;

            this.Visible = false;

            LayoutChildren(null);
            this.Initializing = false;
        }

        public void Init(MetaBase mbase, float width)
        {
            this.Initializing = true;
            foreach (PropertyInfo prop in mbase.GetMetaPropertyList(false))
                if (prop.GetValue(mbase, null) != null && prop.GetValue(mbase, null).ToString() != "" && prop.GetValue(mbase, null).ToString() != mbase.ToString())
                    this.Add(new MetaPropertyNodeRow(prop.Name, prop.GetValue(mbase, null).ToString(), width));
            this.Initializing = false;
            LayoutChildren(null);
        }

        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
                Printable = value;
            }
        }

        public Collection<MetaPropertyNodeRow> Rows
        {
            get
            {
                Collection<MetaPropertyNodeRow> rows = new Collection<MetaPropertyNodeRow>();
                foreach (GoObject o in this)
                    if (o is MetaPropertyNodeRow)
                        rows.Add(o as MetaPropertyNodeRow);
                return rows;
            }
        }

    }

    [Serializable]
    public class MetaPropertyNodeRow : GoListGroup
    {
        float w = 0;

        public MetaPropertyNodeRow(string Name, string Value, float width)
        {
            w = width;
            this.Initializing = true;
            this.Spacing = 5;
            this.AutoRescales = false;
            this.Width = w;
            this.Height = 14;
            this.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopLeftMargin = new SizeF(0, 0);
            this.BottomRightMargin = new SizeF(0, 0);
            this.LinePen = Pens.Gray;
            this.BorderPen = Pens.Black;
            this.DragsNode = true;

            this.Deletable = false;
            this.Selectable = false;

            this.Add(MakeText(Name));
            this.Add(MakeText(Value));
            (this[1] as GoText).Bold = true;

            this.Visible = true;

            this.Initializing = false;
            LayoutChildren(null);
        }

        private GoText MakeText(String s)
        {
            GoText t = new GoText();
            t.AutoResizes = false;
            t.AutoRescales = false;
            t.Text = s;
            t.Alignment = MiddleLeft;
            t.Selectable = false;
            t.DragsNode = true;
            t.Clipping = true;
            t.StringTrimming = StringTrimming.EllipsisCharacter;
            t.Wrapping = true;
            t.WrappingWidth = (w / 2);
            t.Editable = false;
            t.Width = w / 2;

            //Graphics grfx = Graphics.FromImage(new Bitmap(1, 1));
            //SizeF size = grfx.MeasureString(t.Text, t.Font);

            //if (size.Height > 12)
            //    t.Height = 12 * size.Height / 12;
            //else
            t.Height = 12;
            t.FontSize = 8;

            return t;
        }
        //private BoundLabel MakeLabel(PropertyInfo prop)
        //{
        //    BoundLabel t = new BoundLabel();
        //    t.AutoResizes = false;
        //    t.AutoRescales = false;

        //    if (prop.GetValue(CopiedFrom, null) != null)
        //        t.Text = prop.GetValue(CopiedFrom, null).ToString();

        //    t.Alignment = MiddleLeft;
        //    t.Selectable = false;
        //    t.DragsNode = true;
        //    t.StringTrimming = StringTrimming.EllipsisCharacter;
        //    t.Editable = true;
        //    t.Width = w / 2;
        //    t.Height = 12;
        //    t.FontSize = 8;

        //    return t;
        //}

    }

}