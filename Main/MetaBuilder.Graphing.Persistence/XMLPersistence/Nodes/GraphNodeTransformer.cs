using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Behaviours.Observers;
using MetaBuilder.Meta;
using Northwoods.Go;
using Northwoods.Go.Xml;
using System.Collections.ObjectModel;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Nodes
{
    public class GraphNodeTransformer : EmbeddedObjectsTransformer
    {

        #region Fields (2)

        private readonly List<MetaBase> AlreadyLoadedObjects;
        //private string anchors;

        #endregion Fields

        #region Constructors (1)

        public GraphNodeTransformer()
            : base()
        {
            TransformerType = typeof(GraphNode);
            ElementName = "gnode";
            IdAttributeUsedForSharedObjects = true;
            BodyConsumesChildElements = true;
            AlreadyLoadedObjects = new List<MetaBase>();
        }

        #endregion Constructors

        #region Methods (7)

        // Public Methods (7) 

        public override object Allocate()
        {
            GraphNode node = new GraphNode();
            string boundclass = StringAttr("cls", string.Empty);
            //replaced = false;
            node = replaceShape(node, boundclass);
            return node; //return null if filtered
        }
        const char defSplitChar = '~';
        const char itemSplitChar = '|';

        bool replaced = false;
        private GraphNode replaceShape(GraphNode obj, string className)
        {
            //replaced = false;
            if (className == "DataView")
            {
                obj.CanBeAutomaticallyReplaced = true;
                return obj;
            }
            foreach (KeyValuePair<string, string> kvp in Core.Variables.Instance.RenamedClasses)
                //{
                if (kvp.Key == className)
                    obj.CanBeAutomaticallyReplaced = true;
            //    {
            //        try
            //        {
            //            GraphNode n = (Core.Variables.Instance.ReturnShape(kvp.Value) as GraphNode).Copy() as GraphNode;
            //            n.MetaObject = obj.MetaObject;
            //            n.HookupEvents();
            //            replaced = true;
            //            return n;
            //        }
            //        catch
            //        {
            //            break;
            //        }
            //    }
            //}

            return obj;
        }

        public override void ConsumeAttributes(object obj)
        {
            if (!replaced)
            {
                base.ConsumeAttributes(obj);

                GraphNode node = obj as GraphNode;
                node.MustLoadFromDatabase = BooleanAttr("mustLoad", false);
                node.AllowedClasses = StringAttr("allowedClasses", "");
                node.Selectable = true;
                string boundclass = StringAttr("cls", string.Empty);

                node.BindingInfo = new BindingInfo();
                node.BindingInfo.BindingClass = boundclass;
                //rebind boundclass
                foreach (KeyValuePair<string, string> kvp in Core.Variables.Instance.RenamedClasses)
                {
                    if (kvp.Key == boundclass)
                    {
                        node.BindingInfo.BindingClass = kvp.Value;
                        break;
                    }
                }

                string boundLabels = StringAttr("lbls", "");
                string[] definitions = boundLabels.Split(defSplitChar);

                for (int i = 0; i < definitions.Length - 1; i++)
                {
                    string[] def = definitions[i].Split(itemSplitChar);
                    string Label = def[0];
                    string BoundToField = def[1];
                    node.BindingInfo.Bindings.Add(Label, BoundToField);
                }

                node.EditMode = false;
            }
            else
            {
                System.Drawing.RectangleF bounds = RectangleFAttr("xy", new System.Drawing.RectangleF());
                //(obj as GraphNode).Width = bounds.Width;
                //(obj as GraphNode).Height = bounds.Height;
                (obj as GraphNode).Position = new System.Drawing.PointF(bounds.X, bounds.Y);
            }
            if (StringAttr("shallowCN", null) != null)
                Reader.MakeShared(StringAttr("shallowCN", null), obj);

            (obj as GraphNode).ImageFilename = StringAttr("imgPath", "");
        }
        public override void ConsumeChild(object parent, object child)
        {
            if (child is GraphNodeGrid)
                return;

            if (child is GoImage)
            {
                return;
            }

            GraphNode node = parent as GraphNode;
            if (!replaced)
            {
                base.ConsumeChild(parent, child);
                if (child != null)
                {
                    if (child is GoObject)
                    {
                        GoObject o = child as GoObject;
                        o.Remove();
                        if (o is GoText)
                        {
                            GoText txt = o as GoText;
                            bool originalEditmode = node.EditMode;
                            if (txt.Maximum != 1999)
                            {
                                node.EditMode = false;

                                node.Add(txt);
                                node.EditMode = originalEditmode;

                                if (txt is BoundLabel)
                                {
                                    txt.AddObserver(node);
                                }
                            }
                            else
                            {
                                txt.ToString();
                            }
                        }
                        else
                            node.Add(o);
                    }
                }
            }
            else if (child is QuickPort)
            {
                base.ConsumeChild(parent, child);
                //we must set the id of this port to the same port of our replaced(current)stencil
                //so that links can work the same way
                QuickPort filePort = (child as QuickPort);
                if (filePort.PartID != -1)
                    this.Reader.MakeShared(filePort.PartID.ToString(), node.GetDefaultPort); //This is a pretty bad hack
                node.Remove(child);
            }
            else if (child is MetaBase)
            {
                MetaBase mb = child as MetaBase;
                if (mb == null)
                {
                    return;
                    //do whatever we need to for this missing class
                }
                ////27 May 2014 - This force a class to a specific shape. That breaks remapping
                //if (node.BindingInfo.BindingClass != mb._ClassName)
                //{
                //    MetaBase mbRecreate = MetaBuilder.Meta.Loader.CreateInstance(node.BindingInfo.BindingClass);
                //    mb.CopyPropertiesToObject(mbRecreate);
                //    mb = mbRecreate;
                //}

                MetaBase mbCached = RetrieveAlreadyLoaded(mb.pkid, mb.MachineName);
                if (mbCached != null)
                    mb = mbCached;
                else
                    AlreadyLoadedObjects.Add(mb);

                node.MetaObject = mb;
                node.EditMode = false;
                node.BindingInfo.BoundObjectID = node.MetaObject.pkid;
            }
        }

        public override void ConsumeObjectFinish(object obj)
        {
            GraphNode node = obj as GraphNode;
            node.Initializing = true;
            node.HookupEvents();

            //if (!replaced)
            base.ConsumeObjectFinish(obj);

            node.IsStencilOnlyText = BooleanAttr("isTextOnly", false);

            GoGroupEnumerator partEnum = node.GetEnumerator();
            float minX = node.Right;
            float minY = node.Bottom;
            //GoObject culpritX = null;
            //GoObject culpritY = null;
            List<GoObject> objsToRemove = new List<GoObject>();
            while (partEnum.MoveNext())
            {
                if (!(partEnum.Current is GraphNodeGrid))
                {
                    if (partEnum.Current.Bounds.Width == 0 && partEnum.Current.Bounds.Height == 0)
                    {
                        objsToRemove.Add(partEnum.Current);
                    }
                }
            }

            for (int i = 0; i < objsToRemove.Count; i++)
            {
                objsToRemove[i].Remove();
            }

            //while (partEnum.MoveNext())
            //{
            //    if (!(partEnum.Current is GraphNodeGrid))
            //    {
            //        if (partEnum.Current.Position.X < minX)
            //        {
            //            minX = partEnum.Current.Position.X;
            //            culpritX = partEnum.Current;
            //        }
            //        if (partEnum.Current.Position.Y < minY)
            //        {
            //            minY = partEnum.Current.Position.Y;
            //            culpritY = partEnum.Current;
            //        }
            //    }
            //}
            if (node.MetaObject == null) //because it is a stencil?!
            {
                node.MetaObject = Loader.CreateInstance(node.BindingInfo.BindingClass);
                node.HookupEvents();
                //do whatever we need to for this missing class
                //return;
            }
            else //if (node.MetaObject != null)
            {
                foreach (KeyValuePair<string, string> labelBinding in node.BindingInfo.Bindings)
                {
                    BoundLabel blabel = node.FindByName(labelBinding.Key) as BoundLabel;
                    if (blabel != null)
                    {
                        object o = node.MetaObject.Get(labelBinding.Value);
                        if (o != null)
                        {
                            if (blabel.Text != o.ToString())
                            {
                                //MetaBase oldMB = node.MetaObject;
                                //node.MetaObject = Loader.CreateInstance(node.MetaObject._ClassName);
                                //oldMB.CopyPropertiesToObject(node.MetaObject);
                                //node.MetaObject.pkid = oldMB.pkid;
                                //node.MetaObject.MachineName = oldMB.MachineName;
                                blabel.Text = o.ToString(); //was s previously. ie: change metaobject to mimic label text. this is incorrect
                                //node.LabelTextChanged(blabel, "", blabel.Text);
                                node.BindingInfo.BoundObjectID = 0;
                            }
                        }
                    }
                }
            }

            if (BooleanAttr("MetaPropList", false) == true)
            {
                node.widthBefore = 180;
                node.InitMetaProps(false);
            }

            node.CalculateGridSize();
            node.Position = new System.Drawing.PointF(minX - node.Width, minY - node.Height);

            FixIfBroken(node);
            //node.Position = new System.Drawing.PointF(minX, minY);
            //try
            //{
            node.BindToMetaObjectProperties();
            //}
            //catch (Exception ex)
            //{
            //Core.Log.WriteLog(ex.ToString());
            //}
            //node.BindMetaObjectImage();
            node.Initializing = false;
        }

        private void FixIfBroken(GraphNode node) //this fixes old 'problem' nodes only
        {
            //if (node.MetaObject.Class == "Function" || node.MetaObject.Class == "Process" || node.MetaObject.Class == "Activity")
            if (!(node is CollapsibleNode))
            {
                if (node.Height - 100 > 5)
                {
                    float newYposition = node.Position.Y + (node.Height - 100);
                    //you have a problem
                    System.Drawing.RectangleF newRect = new System.Drawing.RectangleF(node.Position.X, newYposition, node.Width, 100);
                    node.Bounds = newRect;
                    node.CalculateGridSize();
                    return;
                }
            }
            else
            {

                //foreach (GoObject o in node) //It is saved as a boundlabel
                //{
                //    if (o is GoText && !(o is BoundLabel) && !(o is IndicatorLabel))
                //    {
                //        o.ToString();
                //    }
                //}

                //if (node.Height - 100 - (node as CollapsibleNode).List.Height > 5)
                //{
                //    float newYposition = node.Position.Y + (node.Height - 100);
                //    //you have a problem
                //    System.Drawing.RectangleF newRect = new System.Drawing.RectangleF(node.Position.X, newYposition, node.Width, 100);
                //    node.Bounds = newRect;
                //    node.CalculateGridSize();
                //    return;
                //}
            }
        }

        public override void GenerateAttributes(Object obj)
        {
            GraphNode node = obj as GraphNode;

            Collection<GoObject> unsavedObjects = new Collection<GoObject>();
            foreach (GoObject o in node)
            {
                if (o is IndicatorLabel)
                {
                    unsavedObjects.Add(o);
                }
            }
            for (int i = 0; i < unsavedObjects.Count; i++)
            {
                unsavedObjects[i].Remove();
            }

            if (!(node is CollapsibleNode))
                base.GenerateAttributes(obj, true);
            // otherwise this is generated more than once!
            if (node.GetType().Name.ToString() == typeof(GraphNode).Name)
            {
                if (node.HasBindingInfo)
                {
                    WriteAttrVal("cls", node.BindingInfo.BindingClass);
                    StringBuilder sbLabelBindings = new StringBuilder();
                    foreach (KeyValuePair<string, string> bindingItem in node.BindingInfo.Bindings)
                    {
                        if (bindingItem.Value != "-none-")
                        {
                            sbLabelBindings.Append(bindingItem.Key + "|" + bindingItem.Value + "~");
                        }
                    }
                    WriteAttrVal("lbls", sbLabelBindings.ToString());
                }
            }

            for (int i = 0; i < unsavedObjects.Count; i++)
            {
                node.Add(unsavedObjects[i]);
            }
            WriteAttrVal("shallowCN", node.MetaObject.pkid + ":" + node.Location.ToString());
            WriteAttrVal("isTextOnly", node.IsStencilOnlyText);

            if (!string.IsNullOrEmpty(node.ImageFilename))
            {
                WriteAttrVal("imgPath", node.ImageFilename);
            }

            if (node.MetaPropList != null)
            {
                WriteAttrVal("MetaPropList", node.MetaPropList.Visible);
            }
        }

        public override void GenerateDefinitions(Object obj)
        {
            //if (!(obj is CollapsibleNode))
            //base.GenerateDefinitions(obj);

            GraphNode n = (GraphNode)obj;
            foreach (GoObject child in n)
            {
                if (child is GoImage)
                {
                    continue;
                }
                else if (child is MetaPropertyList)
                {
                    continue;
                }
                else if (child is MetaPropertyNodeRow)
                {
                    continue;
                }
                else
                {
                    Writer.DefineObject(child);
                }
            }
            Writer.DefineObject(n.MetaObject);
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

    }

}