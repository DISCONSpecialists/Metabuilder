using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing.Shapes.General;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Links
{
    public class VisualPortTransformer : BaseGoObjectTransformer
    {
        public VisualPortTransformer()
            : base()
        {
            this.TransformerType = typeof(VisualPort);
            this.ElementName = "visualport";
            this.IdAttributeUsedForSharedObjects = true;
        }
        public override object Allocate()
        {
            return new VisualPort();
        }
        public override void GenerateAttributes(object obj)
        {
            base.GenerateAttributes(obj);
            WriteAttrVal("id", this.Writer.MakeShared(obj as VisualPort));
        }
        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);

            if (IsAttrPresent("id"))
                this.Reader.MakeShared(StringAttr("id", null), obj);
        }
        public override void ConsumeObjectFinish(object obj)
        {
            base.ConsumeObjectFinish(obj);
        }
    }
}