using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Nodes;
using MetaBuilder.Graphing.Shapes.General;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Links
{
    public class VisualLinkTransformer : BaseGoObjectTransformer
    {
        public VisualLinkTransformer()
            : base()
        {
            this.TransformerType = typeof(VisualLink);
            this.ElementName = "visualink";
            this.IdAttributeUsedForSharedObjects = true;
        }
        public override object Allocate()
        {
            return new VisualLink();
        }
        public override void GenerateAttributes(object obj)
        {
            base.GenerateAttributes(obj);

            VisualLink link = obj as VisualLink;
            link.Initializing = true;
            WriteAttrVal("penDashStyle", (obj as VisualLink).Pen.DashStyle.ToString());
            WriteAttrVal("from", this.Writer.MakeShared(link.FromPort));
            WriteAttrVal("to", this.Writer.MakeShared(link.ToPort));

            link.Initializing = false;
        }
        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);

            VisualLink link = obj as VisualLink;
            link.Initializing = true;
            DashStyle d = (DashStyle)Enum.Parse(typeof(DashStyle), StringAttr("penDashStyle", "DashDot"), true);
            link.SetStyle(d);
            link.Initializing = false;
        }
        public override void ConsumeObjectFinish(object obj)
        {
            base.ConsumeObjectFinish(obj);

            VisualLink link = obj as VisualLink;
            link.Initializing = true;

            String fromid = StringAttr("from", null);
            if (fromid != null)
            {
                link.FromPort = this.Reader.FindShared(fromid) as GoPort;
            }
            String toid = StringAttr("to", null);
            if (toid != null)
            {
                link.ToPort = this.Reader.FindShared(toid) as GoPort;
            }

            Reader.AddDelayedRef(obj, "FromPort", fromid);
            Reader.AddDelayedRef(obj, "ToPort", toid);

            link.Initializing = false;
        }
    }
}