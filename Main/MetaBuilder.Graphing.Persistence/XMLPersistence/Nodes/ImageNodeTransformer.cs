using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Behaviours.Observers;
using MetaBuilder.Meta;
using Northwoods.Go;
using Northwoods.Go.Xml;
using MetaBuilder.Graphing.Shapes.Nodes;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Nodes
{
    public class ImageNodeTransformer : EmbeddedObjectsTransformer
    {

        #region Fields (2)

        private readonly List<MetaBase> AlreadyLoadedObjects;
        //private string anchors;

        #endregion Fields

        #region Constructors (1)

        public ImageNodeTransformer()
            : base()
        {
            TransformerType = typeof(ImageNode);
            ElementName = "imageNode";
            IdAttributeUsedForSharedObjects = true;
            BodyConsumesChildElements = true;
            AlreadyLoadedObjects = new List<MetaBase>();
        }

        #endregion Constructors

        #region Methods (7)

        // Public Methods (7) 

        public override object Allocate()
        {
            ImageNode node = new ImageNode();
            node.Initializing = true;
            string boundclass = StringAttr("cls", string.Empty);
            node.BindingInfo = new BindingInfo();
            node.BindingInfo.BindingClass = boundclass;
            return node; //return null if filtered
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            (obj as ImageNode).ImageFilename = StringAttr("imgPath", "");
            (obj as ImageNode).DisplayMember = StringAttr("display", "");
            if (StringAttr("shallowCN", null) != null)
                Reader.MakeShared(StringAttr("shallowCN", null), obj);

            if ((obj as ImageNode).AllocationHandle != null)
            {
                string items = StringAttr("allocations", "");
                if (!string.IsNullOrEmpty(items))
                {
                    foreach (string wholeItem in items.Split('|'))
                    {
                        if (wholeItem == "")
                            continue;
                        string path = "", unique = "", type = "";
                        int x = 0;
                        foreach (string s in wholeItem.Split(';'))
                        {
                            if (x == 0)
                                unique = s;
                            else if (x == 1)
                                path = s;
                            else if (x == 2)
                                type = s;
                            x++;
                        }
                        AllocationHandle.AllocationItem item = new AllocationHandle.AllocationItem(path, unique);
                        (obj as ImageNode).AllocationHandle.Items.Add(item);
                    }
                }
            }

        }
        public override void ConsumeChild(object parent, object child)
        {
            ImageNode node = parent as ImageNode;
            if (child is MetaBase)
            {
                MetaBase mb = child as MetaBase;
                if (mb == null)
                {
                    return;
                }

                MetaBase mbCached = RetrieveAlreadyLoaded(mb.pkid, mb.MachineName);
                if (mbCached != null)
                    mb = mbCached;
                else
                    AlreadyLoadedObjects.Add(mb);

                node.MetaObject = mb;
                node.BindingInfo.BoundObjectID = node.MetaObject.pkid;
            }
            else if (child is QuickPort)
            {
                //node.Remove(child as GoObject);
            }
        }

        public override void ConsumeObjectFinish(object obj)
        {
            ImageNode node = obj as ImageNode;
            node.HookupEvents();

            //if (!replaced)
            base.ConsumeObjectFinish(obj);

            if (node.MetaObject == null)
            {
                node.MetaObject = Loader.CreateInstance(node.BindingInfo.BindingClass);
                node.HookupEvents();
                //return;
            }
            node.Label.FamilyName = StringAttr("font", node.Label.FamilyName);
            node.Label.FontSize = FloatAttr("fontSZ", node.Label.FontSize);
            node.Label.TextColor = ColorAttr("fontColor", node.Label.TextColor);
            node.Label.Bold = BooleanAttr("fontB", node.Label.Bold);
            node.Label.Italic = BooleanAttr("fontI", node.Label.Italic);
            node.Label.Underline = BooleanAttr("fontU", node.Label.Underline);

            node.Initializing = false;

            node.BindToMetaObjectProperties();
        }

        public override void GenerateAttributes(Object obj)
        {
            ImageNode node = obj as ImageNode;

            List<GoObject> unsavedObjects = new List<GoObject>();
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
            base.GenerateAttributes(obj, true);
            WriteAttrVal("cls", node.BindingInfo.BindingClass);
            WriteAttrVal("shallowCN", node.MetaObject.pkid + ":" + node.Location.ToString());

            if (!string.IsNullOrEmpty(node.ImageFilename))
            {
                WriteAttrVal("imgPath", node.ImageFilename);
            }
            if (node.DisplayMember != null)
                WriteAttrVal("display", node.DisplayMember);

            WriteAttrVal("font", node.Label.FamilyName);
            WriteAttrVal("fontSZ", node.Label.FontSize);
            WriteAttrVal("fontColor", node.Label.TextColor);
            WriteAttrVal("fontB", node.Label.Bold);
            WriteAttrVal("fontI", node.Label.Italic);
            WriteAttrVal("fontU", node.Label.Underline);

            if (node.AllocationHandle != null)
            {
                string items = "";
                foreach (AllocationHandle.AllocationItem item in node.AllocationHandle.Items)
                {
                    items += item.WriteString + "|";
                }
                WriteAttrVal("allocations", items);
            }
        }

        public override void GenerateDefinitions(Object obj)
        {
            ImageNode n = (ImageNode)obj;
            Writer.DefineObject(n.MetaObject);
            if (n.AllocationHandle != null)
                Writer.DefineObject(n.AllocationHandle);
            //base.GenerateDefinitions(obj);
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