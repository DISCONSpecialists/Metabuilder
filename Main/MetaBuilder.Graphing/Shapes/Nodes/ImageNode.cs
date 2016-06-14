using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;
using MetaBuilder.Meta;
using MetaBuilder.Graphing.Shapes.Behaviours;
using System.Drawing;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.BusinessLogic;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Data;

namespace MetaBuilder.Graphing.Shapes.Nodes
{
    [Serializable]
    public class ImageNode : GoIconicNode, IGoCollapsible, IShallowCopyable, IMetaNode
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

        public void ReplaceAllocationHandle(AllocationHandle handle)
        {
            if (AllocationHandle != null)
                AllocationHandle.Remove();
            handle.DragsNode = true;
            this.Add(handle);
        }

        [NonSerialized]
        public GoImage image = null;// = new GoImage();

        [NonSerialized]
        private string imageFilename;
        public string ImageFilename
        {
            get { return imageFilename; }
            set
            {
                //if (imageFilename == value && image != null)
                //    return;

                PointF loc = this.Position;
                if (image != null)
                {
                    if (image.Parent != null)

                        if (image.Parent != this)
                            image = new GoImage();
                }
                else
                {
                    image = new GoImage();
                }

                //if (imageFilename == value)
                //    return;
                imageFilename = value;
                try
                {
                    if (imageFilename == "")
                    {
                        if (System.IO.File.Exists(System.Windows.Forms.Application.StartupPath + "\\Metadata\\Images\\missing.png"))
                            image.Image = System.Drawing.Image.FromFile(System.Windows.Forms.Application.StartupPath + "\\Metadata\\Images\\missing.png", true);
                        else
                        {
                            //Icon = null;
                            this.Position = loc;
                            return;
                        }
                    }
                    else
                    {
                        if (System.IO.File.Exists(imageFilename))
                        {
                            image.Image = System.Drawing.Image.FromFile(imageFilename, true);
                        }
                        else
                        {
                            imageFilename = System.Windows.Forms.Application.StartupPath + "\\Metadata\\Images\\" + strings.GetFileNameOnly(imageFilename);
                            if (System.IO.File.Exists(imageFilename))
                            {
                                image.Image = System.Drawing.Image.FromFile(imageFilename, true);
                            }
                            else
                            {
                                return;
                            }
                        }
                    }

                    //image.LoadImage();
                    image.Size = new SizeF(50, 50);
                    Icon.Size = new SizeF(50, 50);
                    Icon = image;
                    this.Position = loc;
                }
                catch
                {
                }
            }
        }

        [NonSerialized]
        private string displayMember;
        public string DisplayMember
        {
            get { return displayMember; }
            set
            {
                displayMember = value;
                BindToMetaObjectProperties();
            }
        }

        public void calculateClassLabelPosition()
        {
            classLabel.Location = new PointF(this.Icon.Left, this.Icon.Top - 10);
            if (MetaObject == null)
                return;

            float halfLabelWidth = classLabel.Width / 2;
            float halfIconWidth = this.Icon.Width / 2;
            float baseX = classLabel.Position.X;
            if (halfLabelWidth < halfIconWidth)
            {
                classLabel.Position = new PointF(baseX + (halfIconWidth - halfLabelWidth), this.Icon.Top - 10);
            }
            else if (halfLabelWidth > halfIconWidth)
            {
                classLabel.Position = new PointF(baseX - (halfLabelWidth - halfIconWidth), this.Icon.Top - 10);
            }
            //else
            //{
            //    classLabel.Location = new PointF(this.Icon.Left, this.Icon.Top - 4);
            //}
            classLabel.Printable = classLabel.Visible = Core.Variables.Instance.ImageNodeClassLabelVisible;

            //#if DEBUG
            GeneratePorts();
            //#endif
        }

        public BoundLabel classLabel
        {
            get
            {
                foreach (GoObject o in this)
                    if (o is BoundLabel)
                        if ((o as BoundLabel).Name == "cls_id")
                            return o as BoundLabel;

                return null;

                //this.Add(AddClassLabel());
                //return classLabel;
            }
        }

        public ImageNode()
        {
            Initialize(null, 1, "Image node");

            Editable = true;

            Label = new ExpandableTextBoxLabel();
            Label.Alignment = GoObject.MiddleCenter;
            Label.Editable = true;
            Label.StringTrimming = StringTrimming.Word;
            Label.WrappingWidth = 160;
            Label.Clipping = true;
            Label.Width = 160;
            Label.AutoResizes = false;
            Label.Selectable = false;

            Label.AddObserver(this);

            Icon.Selectable = false;

            this.Add(AddClassLabel());

            //Port.PortObject = this;

            this.Port.FromSpot = 0;
            this.Port.ToSpot = 0;

            //Copyable = false;

            ReplaceAllocationHandle(new AllocationHandle());
            collapsible = true;
        }

        private void GeneratePorts()
        {
            if (Initializing)
                return;
            foreach (GoObject o in this)
                if (o is QuickPort)
                    return;

            this.Add(makePort(0));
            this.Add(makePort(90));
            this.Add(makePort(180));
            this.Add(makePort(270));
        }

        public GoPort GetPort(string pos)
        {
            foreach (GoObject o in this)
                if (o is QuickPort)
                    if ((o as QuickPort).PortPosition.ToString() == pos)
                        return o as GoPort;

            return Port;
        }

        public override GoPort Port
        {
            get
            {
                return base.Port;
            }
            set
            {
                base.Port = value;
            }
        }

        private QuickPort makePort(float Direction)
        {
            QuickPort p = new QuickPort();
            p.OutgoingLinksDirection = p.IncomingLinksDirection = Direction;
            if (Direction == 0)
            {
                p.Location = new PointF(this.Center.X + 25, this.Center.Y - 5);
                p.PortPosition = MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Right;
            }
            else if (Direction == 180)
            {
                p.Location = new PointF(this.Center.X - 25 - p.Width, this.Center.Y - 10);
                p.PortPosition = MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Left;
            }
            else if (Direction == 90)
            {
                p.Location = new PointF(this.Left + (this.Width / 2) - 5, this.Label.Bottom);
                p.PortPosition = MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Bottom;
            }
            else if (Direction == 270)
            {
                p.Location = new PointF(this.Left + (this.Width / 2) - 5, this.classLabel.Top - 10);
                p.PortPosition = MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Top;
            }
            p.Brush = Brushes.Gray;//new SolidBrush(Color.Gray);
            return p;
        }

        public BoundLabel AddClassLabel()
        {
            BoundLabel lbl = new BoundLabel();

            lbl.Selectable = false;
            //lbl.Width = 160;
            lbl.AutoResizes = true;
            //lbl.Alignment = MiddleCenter;
            lbl.DragsNode = true;
            lbl.Name = "cls_id";
            lbl.Text = "NULL";
            lbl.Deletable = false;
            lbl.Movable = false;
            lbl.FontSize = 7;
            lbl.TextColor = Color.FromArgb(-15132304);
            lbl.Location = new PointF(this.Icon.Left, this.Icon.Top - 4);
            lbl.Editable = false;
            lbl.Printable = true;

            return lbl;
        }

        protected override GoPort CreatePort()
        {
            GoPort p = new GoPort();
            p.Style = GoPortStyle.None;
            p.Size = new SizeF(5, 5);
            p.FromSpot = NoSpot;
            p.ToSpot = NoSpot;
            //p.PortObject = this;
            return p;
        }

        //[NonSerialized]
        private MetaBase metaObject;
        public MetaBase MetaObject
        {
            get { return metaObject; }
            set
            {
                if (value != null)
                {
                    if (MetaObject == null || MetaObject.Class != value.Class)
                    {
                        BindingInfo = new BindingInfo();
                        BindingInfo.BindingClass = value.Class;
                    }
                }
                metaObject = value;
            }
        }

        #region IMetaNode Members

        [NonSerialized]
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

        [NonSerialized]
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
                    retval = (BindingInfo.BindingClass.Length > 0);
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
            PointF loc = this.Position;
            if (MetaObject == null)
            {
                classLabel.Text = "NULL METAOBJECT";
            }
            else
            {
                classLabel.Text = MetaObject.Class;
                if (DisplayMember == null || DisplayMember.Length == 0)
                {
                    Label.Text = MetaObject.ToString();
                }
                else
                {
                    try
                    {
                        Label.Text = MetaObject.Get(DisplayMember).ToString();
                    }
                    catch
                    {
                        DisplayMember = null;
                    }
                }
                BindMetaObjectImage();
            }
            this.Position = loc;
            calculateClassLabelPosition();
        }
        public void BindMetaObjectImage()
        {
            try
            {
                //if (View == null) //skip this function when opening. This really should be initializing
                //    return;
                //dont care about employees
                if (MetaObject == null)// || MetaObject.Class == "Employee" || MetaObject.Class == "Person")
                    return;

                //SkipsUndoManager = true;
                string filename = "";
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

                    //using (TList<Field> fields = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.FieldProvider.GetByClass(this.MetaObject.Class))
                    {
                        foreach (Field f in DataRepository.ClientFieldsByClass(this.metaObject.Class))
                        {
                            if (f.DataType.Contains(".") || f.IsActive == false)// || !string.IsNullOrEmpty(filename))
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
                                            filename = DataRepository.GetUUri(URIID).FileName.ToString();// .Connections[Core.Variables.Instance.ClientProvider].Provider.UURIProvider.GetBypkid((int)URIID).FileName.ToString();
                                            if (!Variables.Instance.ImageCache.ContainsKey(strings.replaceApostrophe(MetaObject.Class.ToString()) + ":" + strings.replaceApostrophe(propertyValue.ToString())))
                                                Variables.Instance.ImageCache.Add(strings.replaceApostrophe(MetaObject.Class.ToString()) + ":" + strings.replaceApostrophe(propertyValue.ToString()), filename);
                                            break;
                                        }
                                    }
                                    catch
                                    {
                                        Log.WriteLog("BindMetaObjectImage - " + propertyValue + " is not a valid value to search for an image. " + this.MetaObject.ToString());
                                    }
                                }
                            }
                            //if (!(string.IsNullOrEmpty(propertyValue))) //Type Uri
                            //{
                            //    try
                            //    {
                            //        object db = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ExecuteScalar(CommandType.Text, "SELECT TOP 1 URI_ID FROM DomainDefinitionPossibleValue WHERE PossibleValue = '" + strings.replaceApostrophe(propertyValue.ToString()) + "' AND URI_ID > 0 AND DomainDefinitionID IN (SELECT pkid FROM DomainDefinition WHERE Name = '" + f.DataType + "')");

                            //        if (db is DBNull || db == null)
                            //            continue;
                            //        int? URIID = int.Parse(db.ToString());
                            //        if (URIID != null)
                            //        {
                            //            filename = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.UURIProvider.GetBypkid((int)URIID).FileName.ToString();
                            //        }
                            //    }
                            //    catch
                            //    {
                            //        Log.WriteLog("BindMetaObjectImage - " + propertyValue + " is not a valid value to search for an image. " + this.MetaObject.ToString());
                            //    }
                            //}
                        }
                    }
                }
                else //VIEWER
                {
                    if (ImageFilename != null && ImageFilename != "")
                    {
                        filename = System.Windows.Forms.Application.StartupPath + "\\MetaData\\Images\\" + strings.GetFileNameOnly(ImageFilename);
                        //return;
                    }
                }

                if (!System.IO.File.Exists(filename))
                {
                    if (System.IO.File.Exists(filename.Replace(" (x86)", "")))
                    {
                        filename = filename.Replace(" (x86)", "");
                    }
                    else if (System.IO.File.Exists(filename.Replace("Program Files", "Program Files (x86)")))
                    {
                        filename = filename.Replace("Program Files", "Program Files (x86)");
                    }
                }
                if (System.IO.File.Exists(filename))
                {
                    ImageFilename = filename;
                }
                else
                {
                    ImageFilename = "";
                }
                if (AllocationHandle != null)
                    AllocationHandle.Location = new PointF(Icon.Right - 10, Icon.Bottom - 10);
                //SkipsUndoManager = false;

            }
            catch (Exception ex)
            {
                Log.WriteLog("ImageNode::BindToMetaObjectProperties" + Environment.NewLine + ex.ToString());
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

        public void FireMetaObjectChanged(object sender, EventArgs e)
        {
            BindToMetaObjectProperties();

            if (Core.Variables.Instance.SaveOnCreate)
            {
                SaveToDatabase(sender, e);
            }
        }

        public void HookupEvents()
        {
            if (MetaObject != null)
            {
                try
                {
                    MetaObject.Changed -= FireMetaObjectChanged;
                }
                catch
                {
                }
                MetaObject.Changed += FireMetaObjectChanged;
            }
        }
        public void UnHookEvents()
        {
            if (MetaObject != null)
            {
                try
                {
                    MetaObject.Changed -= FireMetaObjectChanged;
                }
                catch
                {
                }
            }
        }

        public void OnContentsChanged(object sender, EventArgs e)
        {
            if (ContentsChanged != null)
            {
                ContentsChanged(this, e);
            }
        }

        [NonSerialized]
        private string Provider = Core.Variables.Instance.ClientProvider;
        public MetaBuilder.BusinessLogic.TList<MetaBuilder.BusinessLogic.ObjectAssociation> SaveToDatabase(object sender, EventArgs e)
        {
            if (Provider == null)
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
                //TList<ObjectAssociation> assocsToSave = new TList<ObjectAssociation>();
                //TList<ObjectAssociation> assocsToUpdate = new TList<ObjectAssociation>();
                TList<ObjectAssociation> assocsSaved = new TList<ObjectAssociation>();

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

                    referenceAssociation.CAid = MetaBuilder.BusinessFacade.MetaHelper.Singletons.GetAssociationHelper().GetAssociationTypeForParentChildClass(ReferenceNode.MetaObject.Class, this.MetaObject.Class, ReferenceAssociation);

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
                case 1501: // Probably Boundlabel's Text that changed
                    if (observed is GoText)
                    {
                        LabelTextChanged((string)oldVal, (string)newVal);
                    }
                    break;
            }
            //Manager.OnObservedChanged(observed, subhint, oldI, oldVal, oldRect, newI, newVal, newRect, this);
            base.OnObservedChanged(observed, subhint, oldI, oldVal, oldRect, newI, newVal, newRect);
        }
        public void LabelTextChanged(string OldText, string NewText)
        {
            if (MetaObject != null)
            {
                try
                {
                    if (MetaObject.Class != "Person") // or any other multiple value tostrings()
                    {
                        if (DisplayMember == null || DisplayMember.Length == 0)
                            MetaObject.Set("Name", NewText);
                        else
                            MetaObject.Set(DisplayMember, NewText);
                        Label.Editable = true;
                    }
                    else
                    {
                        Label.Editable = false;
                    }
                    //BindToMetaObjectProperties();
                }
                catch
                {
                    //find this class's description code
                }
            }
        }

        #region IShallowCopyable Members

        [NonSerialized]
        bool copyAsShadow;
        public bool CopyAsShadow
        {
            get { return copyAsShadow; }
            set { copyAsShadow = value; }
        }

        public GoObject CopyAsShallow()
        {
            ImageNode node = this.Copy() as ImageNode;
            node.MetaObject = this.MetaObject;
            node.HookupEvents();
            node.BindingInfo = BindingInfo.Copy();
            return node;
        }

        #endregion

        #region IIdentifiable Members

        [NonSerialized]
        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        #endregion

        public override GoObject CopyObject(GoCopyDictionary env)
        {
            env.Delayeds.Add(this);
            GoObject retval = base.CopyObject(env);
            ImageNode node = retval as ImageNode;
            node.RequiresAttention = false;
            if (this.BindingInfo != null)
            {
                node.BindingInfo = this.BindingInfo.Copy();
            }
            else
            {
                node.BindingInfo = new BindingInfo();
                if (this.MetaObject != null)
                    node.BindingInfo.BindingClass = this.MetaObject.Class;
            }
            if (this.MetaObject != null)
            {
                node.MetaObject = Loader.CreateInstance(this.MetaObject.Class);
                this.MetaObject.CopyPropertiesToObject(node.MetaObject);
                node.CopiedFrom = this.MetaObject;
            }
            else
            {
                this.ToString();
            }
            //node.Shadowed = false;
            HookupEvents();
            node.HookupEvents();
            node.BindToMetaObjectProperties();

            if (CopyAsShadow)
            {
                node.MetaObject = MetaObject;
                node.CopyAsShadow = true;

                return node;
            }
            return node;
        }
        public override void CopyObjectDelayed(GoCopyDictionary env, GoObject newobj)
        {
            //return;
            ImageNode node = newobj as ImageNode;
            node.RequiresAttention = false;
            if (node.MetaObject != null)
            {
                //node.MetaObject.Changed -= FireMetaObjectChanged;
                node.MetaObject = null;
            }

            //base.CopyObjectDelayed(env, node);
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
                if (node.MetaObject != null)
                    MetaObject.CopyPropertiesToObject(node.MetaObject);
            }

            node.HookupEvents();
            //if (metaObject != null)
            //    if (metaObject.pkid > 0)
            //        node.metaObject.IsInDatabase = true;

            HookupEvents();
        }

        public override bool Shadowed
        {
            get
            {
                return false;
            }
            set
            {
                base.Shadowed = false;
                //foreach (GoObject obj in this)
                //{
                //    obj.Shadowed = false;
                //}
            }
        }

        #region IGoCollapsible Members

        public void Collapse()
        {
        }

        public void Expand()
        {
        }

        [NonSerialized]
        private bool collapsible;
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

        public override GoObject SelectionObject
        {
            get
            {
                //return this;
                return base.SelectionObject;
            }
        }
    }
}
