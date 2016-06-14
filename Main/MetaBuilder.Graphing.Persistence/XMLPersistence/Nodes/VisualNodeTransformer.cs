using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing.Shapes.Nodes;
using MetaBuilder.Graphing.Shapes.General;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Nodes
{
    public class VisualNodeTransformer : BaseGoObjectTransformer
    {
        public VisualNodeTransformer()
            : base()
        {
            this.TransformerType = typeof(VisualNode);
            this.ElementName = "visualnode";
            this.IdAttributeUsedForSharedObjects = true;
            this.BodyConsumesChildElements = true;
        }
        public override object Allocate()
        {
            return new VisualNode();
        }
        public override void GenerateAttributes(object obj)
        {
            base.GenerateAttributes(obj);
            WriteAttrVal("id", this.Writer.MakeShared(obj as VisualNode));
        }
        public override void ConsumeChild(object parent, object child)
        {
            base.ConsumeChild(parent, child);
        }
        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
        }
    }
}