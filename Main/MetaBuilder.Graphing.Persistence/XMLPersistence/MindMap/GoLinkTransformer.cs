using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;
using MetaBuilder.Graphing.Shapes.Nodes;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.MindMap
{
    public class GoLinkTransformer : BaseGoObjectTransformer
    {
        public GoLinkTransformer()
            : base()
        {
            this.BodyConsumesChildElements = true;
            this.TransformerType = typeof(GoLink);
            this.ElementName = "mmgolink";
            this.IdAttributeUsedForSharedObjects = true;
        }

        public override object Allocate()
        {
            GoLink link = new GoLink();
            return link;
            //return base.Allocate();
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            //GoLink link = obj as GoLink;
            //link.PartID = Int32Attr("id", 0);

            //String fromid = StringAttr("from", null);
            //if (fromid != null)
            //{
            //    MindMapNode from = this.Reader.FindShared(fromid) as MindMapNode;
            //    if (from != null)
            //    {
            //        link.FromPort = from.Port;
            //    }
            //}
            //String toid = StringAttr("to", null);
            //if (toid != null)
            //{
            //    MindMapNode to = this.Reader.FindShared(toid) as MindMapNode;
            //    if (to != null)
            //    {
            //        link.ToPort = to.Port;
            //    }
            //}
        }
        public override void ConsumeObjectFinish(object obj)
        {
            base.ConsumeObjectFinish(obj);
            //GoLink link = obj as GoLink;
            //link.Selectable = true;
            //link.Movable = true;
            //link.Editable = true;
            //link.Deletable = true;
            //link.Shadowed = true;

            //GoText t = new GoText();
            //t.Printable = false;
            //t.Deletable = true;
            //t.Text = "Readonly";
            //link.MidLabel = t;
        }

        public override void GenerateAttributes(object obj)
        {
            base.GenerateAttributes(obj);

            GoLink link = (GoLink)obj;
            MindMapNode from = link.FromNode as MindMapNode;
            if (from != null)
            {
                //String fromid = this.Writer.FindShared(from);
                WriteAttrVal("from", from.GUID);
            }
            MindMapNode to = link.ToNode as MindMapNode;
            if (to != null)
            {
                //String toid = this.Writer.FindShared(to);
                WriteAttrVal("to", to.GUID);
            }

            //WriteAttrRef("id", link.PartID);
        }
    }
}