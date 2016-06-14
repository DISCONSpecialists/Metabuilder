using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Graphing.Persistence.XMLPersistence.Other;
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

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Nodes
{
    public class RationaleTransformer : BaseGoObjectTransformer
    {

        #region Fields (4)
        private GoObject anchor;
        private readonly List<MetaBase> AlreadyLoadedObjects;
        //private string anchors;
        #endregion Fields

        #region Constructors (1)

        public RationaleTransformer()
            : base()
        {
            TransformerType = typeof(Rationale);
            ElementName = "rationale";
            IdAttributeUsedForSharedObjects = false;
            this.BodyConsumesChildElements = true;
            AlreadyLoadedObjects = new List<MetaBase>();
        }

        #endregion Constructors

        #region Methods (7)

        // Public Methods (7) 

        public override object Allocate()
        {
            Rationale node = new Rationale();
            node.Reanchorable = true;
            return node;
        }
        public override void GenerateAttributes(object obj)
        {
            base.GenerateAttributes(obj, false);
            Rationale x = obj as Rationale;
            WriteAttrVal("txt", x.Text);

            if (x.Anchor != null)
            {
                if (x.Anchor is CollapsingRecordNodeItem)
                {
                    CollapsingRecordNodeItem item = x.Anchor as CollapsingRecordNodeItem;
                    if (item.MetaObject != null)
                        WriteAttrVal("anchor", this.Writer.MakeShared(item.MetaObject));
                }
                else
                    WriteAttrVal("anchor", this.Writer.MakeShared(x.Anchor));
            }

            SizeF marTopLeft = x.TopLeftMargin;
            SizeF marBottomRight = x.BottomRightMargin;
            WriteAttrVal("marginBottomRight", marBottomRight);
            WriteAttrVal("marginTopLeft", marTopLeft);
            WriteAttrVal("xy", x.Bounds);

            WriteAttrVal("ratClr", ((x.Background as GoShape).Brush as SolidBrush).Color);

            x.Printable = Core.Variables.Instance.PrintComments;
            Writer.GenerateObject(x.Label);
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);

            Rationale node = obj as Rationale;
            node.Selectable = true;
            node.BindingInfo = new BindingInfo();
            node.BindingInfo.BindingClass = "Rationale";

            //x.Label.AutoResizes = false;
            node.Text = StringAttr("txt", "");
            string sMargins = StringAttr("marginTopLeft", "");
            if (sMargins.Length > 0)
            {
                node.TopLeftMargin = SizeFAttr("marginTopLeft", new SizeF());
                node.BottomRightMargin = SizeFAttr("marginBottomRight", new SizeF());
            }
            RectangleF bounds = RectangleFAttr("xy", new RectangleF());
            node.Width = bounds.Width;
            node.Height = bounds.Height;
            node.Position = new PointF(bounds.X, bounds.Y);
            node.Reanchorable = true;
            //node.HookupEvents();
        }

        public override void ConsumeChild(object parent, object child)
        {
            Rationale node = parent as Rationale;
            if (child is MetaBase)
            {
                MetaBase mb = child as MetaBase;
                //24 January 2013 A rationale can only ever be a rationale
                //Cornes diagrams load as datacolumns? Therefore:
                //This makes new metaobject for this
                //Rationales cannot be shallow copied (bar F8) so this makes sense
                if (mb.Class.ToLower() != "rationale") //create a new object with the current text
                {
                    string currentText = node.Text;
                    node.CreateMetaObject(this, new EventArgs());
                    node.MetaObject.Set("Value", currentText); //WTF IS THIS?
                    node.MetaObject.WorkspaceName = mb.WorkspaceName;
                    node.MetaObject.WorkspaceTypeId = mb.WorkspaceTypeId;
                    //node.Text = currentText;
                    node.BindToMetaObjectProperties();
                    node.Label.Editable = false;
                    return;
                }

                //the node has saved as a rationale correctly so it can be loaded as per usual
                node.MetaObject = mb;
                node.HookupEvents();
                //node.BindToMetaObjectProperties();
                node.Label.Editable = false;
            }
            else if (child is GoText)
            {
                node.Label.FamilyName = (child as GoText).FamilyName;
                node.Label.FontSize = (child as GoText).FontSize;
                node.Label.TextColor = (child as GoText).TextColor;
                node.Label.Bold = (child as GoText).Bold;
                node.Label.Underline = (child as GoText).Underline;
                node.Label.Italic = (child as GoText).Italic;
                return; //Dont actually consume this child
            }
            base.ConsumeChild(parent, child);
        }

        //(Background as GoShape).Brush = new SolidBrush(Color.FromArgb(253, 226, 173));
        public override void ConsumeObjectFinish(object obj)
        {
            Rationale node = obj as Rationale;
            node.HookupEvents();

            base.ConsumeObjectFinish(obj);

            String anchorid = StringAttr("anchor", null);
            if (anchorid != null)
            {
                anchor = this.Reader.FindShared(anchorid) as GoObject;
                if (anchor == null)
                {
                    Reader.AddDelayedRef(obj, "anchor", anchorid);
                }
                else
                {
                    node.Anchor = anchor;
                }
            }

            node.Bounds = RectangleFAttr("xy", new RectangleF());
            PointF labelPos = new PointF(node.Bounds.X + node.TopLeftMargin.Width, node.Bounds.Y + node.TopLeftMargin.Height);

            node.Label.Bounds = new RectangleF(labelPos, new SizeF(node.Bounds.Width - node.TopLeftMargin.Width - node.BottomRightMargin.Width, node.Bounds.Height - node.TopLeftMargin.Height - node.BottomRightMargin.Height));

            node.Label.Editable = false;
            node.Background.Editable = true;
            node.Background.Resizable = true;
            node.Background.Reshapable = true;
            node.Editable = true;
            node.Resizable = true;
            node.Reshapable = true;
            node.Shadowed = true;
            //node.CalculateGridSize();
            //node.Position = new System.Drawing.PointF(minX, minY);
            try
            {
                Color c = ColorAttr("ratClr", Color.Transparent);
                if (c != Color.Transparent)
                    (node.Background as GoShape).Brush = new SolidBrush(c);
            }
            catch
            {
            }
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

        public override void UpdateReference(object obj, string prop, object referred)
        {
            if (prop == "anchor")
            {
                GoObject anchoredTo = referred as GoObject;
                if (anchoredTo != null)
                {
                    ResizableBalloonComment x = obj as ResizableBalloonComment;
                    x.Anchor = anchoredTo;
                }

                if (referred is MetaBase)
                {
                    MetaBase mbReferred = referred as MetaBase;
                    ResizableBalloonComment x = obj as ResizableBalloonComment;
                    x.Anchor = mbReferred.tag as GoObject;
                }
            }
        }
    }
}