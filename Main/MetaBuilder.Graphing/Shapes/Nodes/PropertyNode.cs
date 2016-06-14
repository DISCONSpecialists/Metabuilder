using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Northwoods.Go;
using System.Drawing;
using MetaBuilder.Meta;
using MetaBuilder.Core;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;

namespace MetaBuilder.Graphing.Shapes.Nodes
{
    public class PropertyNode : GoNode, IMetaNode
    {
        public PropertyNode()
        {
            Initialize();
            BindingInfo = new BindingInfo();
            BindingInfo.BindingClass = "Function";
            CreateMetaObject(this, EventArgs.Empty);
        }

        public void Initialize()
        {
            Add(new QuickPort());

            this.Initializing = true;
            Add(new PropertyNodeBody());
            this.Initializing = false;
            LayoutChildren(null);

        }

        public QuickPort Port
        {
            get { return this[0] as QuickPort; }
        }

        public PropertyNodeBody Body
        {
            get { return this[1] as PropertyNodeBody; }
        }

        // let the body (a list group) decide how to resize the node
        public override GoObject SelectionObject
        {
            get { return this.Body; }
        }

        private MetaBase metaObject;
        public MetaBase MetaObject
        {
            get { return metaObject; }
            set
            {
                if (value != null)
                {
                    if (metaObject == null || metaObject.Class != value.Class)
                    {
                        BindingInfo = new BindingInfo();
                        BindingInfo.BindingClass = value.Class;
                    }
                }
                metaObject = value;
            }
        }

        #region IMetaNode Members

        //[NonSerialized]
        private MetaBase copiedFrom;
        public MetaBase CopiedFrom
        {
            get { return copiedFrom; }
            set { copiedFrom = value; }
        }

        //[NonSerialized]
        BindingInfo bindingInfo;
        public BindingInfo BindingInfo
        {
            get { return bindingInfo; }
            set { bindingInfo = value; }
        }

        public EventHandler _contentsChanged;
        public EventHandler ContentsChanged
        {
            get { return _contentsChanged; }
            set { _contentsChanged = value; }
        }

        public bool HasBindingInfo
        {
            get
            {
                bool retval = false;
                if (BindingInfo != null)
                {
                    retval = BindingInfo.BindingClass.Length > 0;
                }
                return retval;
            }
        }

        [NonSerialized]
        bool requiresAttention;
        public bool RequiresAttention
        {
            get { return requiresAttention; }
            set { requiresAttention = value; }
        }

        public void CreateMetaObject(object sender, EventArgs e)
        {
            if (HasBindingInfo)
            {
                if (BindingInfo.BindingClass != null)
                {
                    MetaObject = Loader.CreateInstance(BindingInfo.BindingClass);
                }
            }
        }

        public void BindToMetaObjectProperties()
        {
            //do nothing
        }
        public void BindMetaObjectImage()
        {
        }
        public void LoadMetaObject(int ID, string Machine)
        {
            if (HasBindingInfo)
            {
                if (BindingInfo.BindingClass != null)
                {
                    metaObject = Loader.GetByID(bindingInfo.BindingClass, ID, Machine);
                    HookupEvents();
                    BindToMetaObjectProperties();
                    //FireMetaObjectChanged(null, EventArgs.Empty);
                }
            }
        }

        public void FireMetaObjectChanged(object sender, EventArgs e)
        {
            BindToMetaObjectProperties();

            if (Core.Variables.Instance.SaveOnCreate)
                SaveToDatabase(sender, e);
        }

        public void HookupEvents()
        {
            if (metaObject != null)
            {
                metaObject.Changed += FireMetaObjectChanged;
            }
        }

        public void OnContentsChanged(object sender, EventArgs e)
        {
            if (ContentsChanged != null)
            {
                ContentsChanged(this, e);
            }
        }

        string Provider = Core.Variables.Instance.ClientProvider;
        public MetaBuilder.BusinessLogic.TList<MetaBuilder.BusinessLogic.ObjectAssociation> SaveToDatabase(object sender, EventArgs e)
        {
            if (Provider == null)
            {
                Provider = Core.Variables.Instance.ClientProvider;
            }
            RequiresAttention = false;
            if (metaObject != null)
            {
                try
                {
                    if (e == EventArgs.Empty)
                        metaObject.SaveToRepository(Guid.NewGuid(), Provider);
                    //Log.WriteLog("saved");
                }
                catch
                {
                    // Console.WriteLine("GraphNode cannot save properly " + x.ToString());
                    try
                    {
                        metaObject.pkid = 0;
                        try
                        {
                            if (e == EventArgs.Empty)
                                metaObject.SaveToRepository(Guid.NewGuid(), Provider);
                            Log.WriteLog("saved after pkid=0");
                        }
                        catch (Exception wException)
                        {
                            metaObject.WorkspaceName = Variables.Instance.CurrentWorkspaceName;
                            metaObject.WorkspaceTypeId = Variables.Instance.CurrentWorkspaceTypeId;
                            metaObject.SaveToRepository(Guid.NewGuid(), Provider);

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

                SaveReference(sender, e, Provider);

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

                    referenceAssociation.CAid = MetaBuilder.BusinessFacade.MetaHelper.Singletons.GetAssociationHelper().GetAssociationTypeForParentChildClass(ReferenceNode.MetaObject.Class, this.metaObject.Class, ReferenceAssociation);

                    referenceAssociation.ObjectID = this.metaObject.pkid;
                    referenceAssociation.ObjectMachine = this.metaObject.MachineName;

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

        [NonSerialized]
        bool parentIsILinkedContainer;
        public bool ParentIsILinkedContainer
        {
            get { return parentIsILinkedContainer; }
            set { parentIsILinkedContainer = value; }
        }

        #endregion

        public override bool OnDoubleClick(GoInputEventArgs evt, GoView view)
        {
            Body.AddRow(false);
            return true;
            //return base.OnDoubleClick(evt, view);
        }

        public override void LayoutChildren(GoObject childchanged)
        {
            base.LayoutChildren(childchanged);

            Port.Bounds = this.Bounds;
            Port.Bounds.Inflate(10, 10);
            //Port.Style = GoPortStyle.None;
            Port.Brush = Brushes.LightBlue;
        }

    }

    public class PropertyNodeBody : GoListGroup
    {
        public PropertyNodeBody()
        {
            this.Initializing = true;
            this.Selectable = false;
            this.Resizable = false;
            this.ResizesRealtime = true;
            this.Orientation = Orientation.Vertical;
            this.Alignment = Middle;
            this.PickableBackground = true;

            AddRow(true);

            LayoutChildren(null);
            this.Initializing = false;
        }

        public PropertyNodeBodyHeader Header
        {
            get
            {
                foreach (GoObject o in this)
                {
                    if (o is PropertyNodeBodyHeader)
                        return o as PropertyNodeBodyHeader;
                }
                return null;
            }
        }

        public PropertyNodeBodyHeader AddRow(bool IsHeader)
        {
            if (IsHeader)
                if (Header != null)
                    return Header;

            PropertyNodeBodyHeader row = new PropertyNodeBodyHeader();
            row.IsHeader = IsHeader;
            row.Initialize();
            this.Add(row);
            LayoutChildren(null);
            return row;
        }

    }

    public class PropertyNodeBodyHeader : GoListGroup
    {
        public GoObject LeftLabel;
        public GoObject RightLabel;

        private bool isHeader;
        public bool IsHeader
        {
            get { return isHeader; }
            set { isHeader = value; }
        }

        public void Initialize()
        {
            this.Initializing = true;
            this.Spacing = 0;
            this.AutoRescales = false;
            this.Width = 300;
            this.Height = 24;
            this.Orientation = Orientation.Horizontal;
            this.TopLeftMargin = new SizeF(0, 0);
            this.BottomRightMargin = new SizeF(0, 0);
            this.LinePen = Pens.Black;
            this.BorderPen = Pens.Black;
            this.DragsNode = true;

            if (IsHeader)
            {
                this.Brush = Brushes.LightBlue;
                this.Deletable = false;
                this.Selectable = false;

                LeftLabel = MakeLabel("Property");
                this.Add(LeftLabel);
                RightLabel = MakeLabel("Value");
                this.Add(RightLabel);
                //top left bottom right
            }
            else
            {
                this.Brush = Brushes.White;
                this.Selectable = true;
                //add objects
                LeftLabel = MakeText("My Property", "DataAttribute");
                this.Add(LeftLabel);
                RightLabel = MakeText("My Value", "DataValue");
                this.Add(RightLabel);
                //left right
            }

            this.Initializing = false;
            LayoutChildren(null);
        }

        private ExpandableTextBoxLabel MakeLabel(String s)
        {
            ExpandableTextBoxLabel t = new ExpandableTextBoxLabel();
            t.AutoResizes = false;
            t.AutoRescales = false;
            t.Text = s;
            t.Alignment = MiddleCenter;
            t.Selectable = false;
            t.DragsNode = true;
            t.StringTrimming = StringTrimming.EllipsisCharacter;
            t.Clipping = true;
            t.Editable = false;
            t.Width = 150;
            t.Height = 20;

            return t;
        }
        private PropertyNodeObject MakeText(String s, String Class)
        {
            return new PropertyNodeObject(s, Class);
        }

        public override bool OnDoubleClick(GoInputEventArgs evt, GoView view)
        {
            if (IsHeader)
            {
                (this.Parent as PropertyNodeBody).AddRow(false);
            }
            LayoutChildren(null);
            return true;
        }
    }

    public class PropertyNodeObject : GoNode, IMetaNode
    {
        public PropertyNodeObject(String s, String Class)
        {
            this.Resizable = false;
            this.AutoRescales = false;
            this.Selectable = true;
            this.DragsNode = true;
            this.Width = 150;
            this.Height = 20;

            BindingInfo = new BindingInfo();
            BindingInfo.BindingClass = Class;

            this.Add(MakeText(s));

            CreateMetaObject(this, EventArgs.Empty);
            HookupEvents();
            this.DragsNode = true;
            LayoutChildren(null);
        }

        private BoundLabel MakeText(String s)
        {
            BoundLabel t = new BoundLabel();
            t.AutoResizes = false;
            t.AutoRescales = false;
            t.Text = s;
            t.Alignment = MiddleLeft;
            t.Selectable = false;
            t.StringTrimming = StringTrimming.EllipsisCharacter;
            t.Clipping = true;
            t.Width = 150;
            t.Height = 20;

            t.Wrapping = true;
            t.WrappingWidth = 150;

            t.AddObserver(this);
            return t;
        }

        public BoundLabel Label
        {
            get
            {
                foreach (GoObject o in this)
                    if (o is BoundLabel)
                        return o as BoundLabel;

                //should never hit this
                this.Add(MakeText("was null"));
                return Label;
            }
        }

        private MetaBase metaObject;
        public MetaBase MetaObject
        {
            get { return metaObject; }
            set
            {
                if (value != null)
                {
                    if (metaObject == null || metaObject.Class != value.Class)
                    {
                        BindingInfo = new BindingInfo();
                        BindingInfo.BindingClass = value.Class;
                    }
                }
                metaObject = value;
            }
        }

        #region IMetaNode Members

        //[NonSerialized]
        private MetaBase copiedFrom;
        public MetaBase CopiedFrom
        {
            get { return copiedFrom; }
            set { copiedFrom = value; }
        }

        //[NonSerialized]
        BindingInfo bindingInfo;
        public BindingInfo BindingInfo
        {
            get { return bindingInfo; }
            set { bindingInfo = value; }
        }

        public EventHandler _contentsChanged;
        public EventHandler ContentsChanged
        {
            get { return _contentsChanged; }
            set { _contentsChanged = value; }
        }

        public bool HasBindingInfo
        {
            get
            {
                bool retval = false;
                if (BindingInfo != null)
                {
                    retval = BindingInfo.BindingClass.Length > 0;
                }
                return retval;
            }
        }

        [NonSerialized]
        bool requiresAttention;
        public bool RequiresAttention
        {
            get { return requiresAttention; }
            set { requiresAttention = value; }
        }

        public void CreateMetaObject(object sender, EventArgs e)
        {
            if (HasBindingInfo)
            {
                if (BindingInfo.BindingClass != null)
                {
                    MetaObject = Loader.CreateInstance(BindingInfo.BindingClass);
                }
            }
        }

        public void BindToMetaObjectProperties()
        {
            //do nothing
            Label.Text = metaObject.ToString();
        }
        public void BindMetaObjectImage()
        {
        }
        public void LoadMetaObject(int ID, string Machine)
        {
            if (HasBindingInfo)
            {
                if (BindingInfo.BindingClass != null)
                {
                    metaObject = Loader.GetByID(bindingInfo.BindingClass, ID, Machine);
                    HookupEvents();
                    BindToMetaObjectProperties();
                    //FireMetaObjectChanged(null, EventArgs.Empty);
                }
            }
        }

        public void FireMetaObjectChanged(object sender, EventArgs e)
        {
            BindToMetaObjectProperties();

            if (Core.Variables.Instance.SaveOnCreate)
                SaveToDatabase(sender, e);
        }

        public void HookupEvents()
        {
            if (metaObject != null)
            {
                metaObject.Changed += FireMetaObjectChanged;
            }
        }

        public void OnContentsChanged(object sender, EventArgs e)
        {
            if (ContentsChanged != null)
            {
                ContentsChanged(this, e);
            }
        }

        string Provider = Core.Variables.Instance.ClientProvider;
        public MetaBuilder.BusinessLogic.TList<MetaBuilder.BusinessLogic.ObjectAssociation> SaveToDatabase(object sender, EventArgs e)
        {
            if (Provider == null)
            {
                Provider = Core.Variables.Instance.ClientProvider;
            }
            RequiresAttention = false;
            if (metaObject != null)
            {
                try
                {
                    if (e == EventArgs.Empty)
                        metaObject.SaveToRepository(Guid.NewGuid(), Provider);
                    //Log.WriteLog("saved");
                }
                catch
                {
                    // Console.WriteLine("GraphNode cannot save properly " + x.ToString());
                    try
                    {
                        metaObject.pkid = 0;
                        try
                        {
                            if (e == EventArgs.Empty)
                                metaObject.SaveToRepository(Guid.NewGuid(), Provider);
                            Log.WriteLog("saved after pkid=0");
                        }
                        catch (Exception wException)
                        {
                            metaObject.WorkspaceName = Variables.Instance.CurrentWorkspaceName;
                            metaObject.WorkspaceTypeId = Variables.Instance.CurrentWorkspaceTypeId;
                            metaObject.SaveToRepository(Guid.NewGuid(), Provider);

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

                SaveReference(sender, e, Provider);

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

                    referenceAssociation.CAid = MetaBuilder.BusinessFacade.MetaHelper.Singletons.GetAssociationHelper().GetAssociationTypeForParentChildClass(ReferenceNode.MetaObject.Class, this.metaObject.Class, ReferenceAssociation);

                    referenceAssociation.ObjectID = this.metaObject.pkid;
                    referenceAssociation.ObjectMachine = this.metaObject.MachineName;

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

        [NonSerialized]
        bool parentIsILinkedContainer;
        public bool ParentIsILinkedContainer
        {
            get { return parentIsILinkedContainer; }
            set { parentIsILinkedContainer = value; }
        }

        #endregion

        protected override void OnObservedChanged(GoObject observed, int subhint, int oldI, object oldVal, RectangleF oldRect, int newI, object newVal, RectangleF newRect)
        {
            switch (subhint)
            {
                case 1501:
                    if (observed is BoundLabel)
                    {
                        LabelTextChanged((string)oldVal, (string)newVal);
                    }
                    break;
            }
            base.OnObservedChanged(observed, subhint, oldI, oldVal, oldRect, newI, newVal, newRect);
        }
        public void LabelTextChanged(string OldText, string NewText)
        {
            if (MetaObject != null)
            {
                try
                {
                    MetaObject.Set("Name", NewText);
                    BindToMetaObjectProperties();
                }
                catch
                {
                    //find this class's description code
                }
            }
        }

    }

    public class PropertyNodePort : QuickPort
    {
        public PropertyNodePort()
        {
        }
    }

}
