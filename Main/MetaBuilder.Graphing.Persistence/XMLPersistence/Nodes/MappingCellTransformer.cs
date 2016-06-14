using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Behaviours.Observers;
using MetaBuilder.Graphing.Shapes.Nodes;
using MetaBuilder.Meta;
using Northwoods.Go;
using Northwoods.Go.Xml;
using MetaBuilder.Graphing.Shapes.Nodes.Containers;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Nodes
{
    public class MappingCellTransformer : EmbeddedObjectsTransformer
    {

        #region Fields (3)

        private readonly List<MetaBase> AlreadyLoadedObjects;
        //private string anchors;
        //private string lblText;

        #endregion Fields

        #region Constructors (1)

        public MappingCellTransformer()
            : base()
        {
            TransformerType = typeof(MappingCell);
            ElementName = "swimlane";
            IdAttributeUsedForSharedObjects = true;
            BodyConsumesChildElements = true;
            AlreadyLoadedObjects = new List<MetaBase>();
        }

        #endregion Constructors

        #region Methods (6)

        // Public Methods (6) 

        public override object Allocate()
        {
            MappingCell node = new MappingCell();
            return node;
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);

            MappingCell node = obj as MappingCell;

            node.Selectable = true;

            //node.Position = PointFAttr("MapCellPosition", new PointF());
            //node.BackGround.Bounds = rectBounds;
            //lblText = StringAttr("lblText", "");

            node.Label.TextColor = ColorAttr("lblColour", Color.Black);
            node.Label.FontSize = FloatAttr("mcfontSz", 10);
            node.Label.Bold = BooleanAttr("mcbold", false);
            node.Label.Italic = BooleanAttr("mcitalic", false);
            node.Label.FamilyName = StringAttr("mcfont", "Tahoma");
            node.Label.Multiline = BooleanAttr("mcmulti", false);
            node.Label.StrikeThrough = BooleanAttr("mcstrike", false);
            Type tStringTrimming = typeof(StringTrimming);
            node.Label.StringTrimming = (StringTrimming)Enum.Parse(tStringTrimming, StringAttr("mctrim", StringTrimming.None.ToString()));
            node.Label.Underline = BooleanAttr("mcul", false);
            //node.Label.WrappingWidth = FloatAttr("mlblWrap", 50);

            //RectangleF rectBounds = RectangleFAttr("RectBounds", new System.Drawing.RectangleF());
            //RectangleF bounds = RectangleFAttr("Bounds", new System.Drawing.RectangleF());
            ////node.BackGround.Bounds = bounds;
            //node.Bounds = rectBounds;
            node.MaxHeight = BooleanAttr("mH", false);
            node.MaxWidth = BooleanAttr("mW", false);
            node.Masked = BooleanAttr("mask", false);
        }

        public override void ConsumeObjectFinish(object obj)
        {
            //base.ConsumeObjectFinish(obj);

            MappingCell mcell = obj as MappingCell;
            mcell.Initializing = true;
            mcell.HookupEvents();
            mcell.BindToMetaObjectProperties();

            RectangleF rectBounds = RectangleFAttr("RectBounds", new System.Drawing.RectangleF());
            RectangleF bounds = RectangleFAttr("Bounds", new System.Drawing.RectangleF());
            //bounds.Height -= 1;
            bounds.Height = RoundOff((int)bounds.Height);
            bounds.Width = RoundOff((int)bounds.Width);
            mcell.Bounds = bounds;
            mcell.BackGround.Bounds = bounds;
            mcell.Initializing = false;
            //mcell.RectangleLocation = StringAttr("rectLoc", "None");
            mcell.RepositionRectangle(StringAttr("rectLoc", "None"));
            mcell.HeaderRectangle.Brush = new SolidBrush(ColorAttr("headercolor", Color.WhiteSmoke));
        }

        private int RoundOff(int i)
        {
            return ((int)Math.Round(i / 10.0)) * 10;
        }

        public override void ConsumeChild(object parent, object child)
        {
            MappingCell mcell = parent as MappingCell;
            if (child is MetaBase)
            {
                MetaBase mb = child as MetaBase;
                mcell.MetaObject = mb;
                //mcell.BindToMetaObjectProperties();
            }
            //else if (child is GoRectangle)
            //{
            //    RectangleF rectBounds = RectangleFAttr("RectBounds", new System.Drawing.RectangleF());
            //    mcell.BackGround.Bounds = rectBounds;
            //else
            //{
            //    mcell.Contains(child);
            //}

            //else
            //{
            //    //mcell.Label.Text = lblText;
            //    //mcell.Label.Size = SizeFAttr("mlblSize", new SizeF(50, 50));
            //}
            //return;
        }

        public override void GenerateAttributes(Object obj)
        {
            MappingCell node = obj as MappingCell;

            WriteAttrVal("RectBounds", node.BackGround.Bounds);
            WriteAttrVal("Bounds", node.Bounds);
            //WriteAttrVal("MapCellPosition", node.Position);
            WriteAttrVal("lblText", node.Label.Text);
            WriteAttrVal("lblColour", node.Label.TextColor);
            WriteAttrVal("mcfontSz", node.Label.FontSize);
            WriteAttrVal("mcbold", node.Label.Bold);
            WriteAttrVal("mcitalic", node.Label.Italic);
            WriteAttrVal("mcfont", node.Label.Font.FontFamily.Name);
            WriteAttrVal("mcmulti", node.Label.Multiline);
            WriteAttrVal("mcstrike", node.Label.StrikeThrough);
            WriteAttrVal("mctrim", node.Label.StringTrimming.ToString());
            WriteAttrVal("mcul", node.Label.Underline);
            WriteAttrVal("mlblSize", node.Label.Size);
            WriteAttrVal("mlblWrap", node.Label.WrappingWidth);
            WriteAttrVal("rectLoc", node.RectangleLocation);
            WriteAttrVal("headercolor", (node.HeaderRectangle.Brush as SolidBrush).Color);

            WriteAttrVal("mH", node.MaxHeight);
            WriteAttrVal("mW", node.MaxWidth);

            WriteAttrVal("mask", node.Masked);

            base.GenerateAttributes(obj, true);

            //Writer.GenerateObject(node.MetaObject);
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