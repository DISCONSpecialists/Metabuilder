using System;
using MetaBuilder.Graphing.Containers;
using Northwoods.Go;
using Northwoods.Go.Xml;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Groups
{
    public class SubGraphTransformer : BaseTransformer
    {

		#region Constructors (1) 

        public SubGraphTransformer()
        {
            this.TransformerType = typeof(TreeSubGraph);
            this.ElementName = "SubGraph";
            this.IdAttributeUsedForSharedObjects = true;
            this.BodyConsumesChildElements = true;
        }

		#endregion Constructors 

		#region Methods (7) 

		// Public Methods (6) 

        public override object Allocate()
        {
            TreeSubGraph sg = new TreeSubGraph();
            return sg;
        }

        public override void ConsumeAttributes(Object obj)
        {
            base.ConsumeAttributes(obj);
            TreeSubGraph x = (TreeSubGraph)obj;
            x.Text = StringAttr("Text", "");
            x.Position = PointFAttr("xy", x.Position);
            x.Selectable = true;
            x.Movable = true;
            
            this.Reader.MakeShared(StringAttr("Port", null), x.Port);
            x.Deletable = false;
        }

        public override void ConsumeChild(Object parent, Object child)
        {
           // base.ConsumeChild(parent, child);
            TreeSubGraph x = (TreeSubGraph)parent;
            if (!Skip(x,child as GoObject))
            {
                GoObject c = child as GoObject;
                x.Add(c);
            }
        }

        public override void GenerateAttributes(Object obj)
        {
            base.GenerateAttributes(obj);
            TreeSubGraph x = (TreeSubGraph)obj;
            WriteAttrVal("Text", x.Text);
        }

        public override void GenerateBody(Object obj)
        {
       //     base.GenerateBody(obj);
            TreeSubGraph x = (TreeSubGraph)obj;
            // do nodes first, then links and other stuff
            foreach (GoObject child in x)
            {
                if (Skip(x, child)) continue;
                if (child is IGoNode)
                {
                    this.Writer.GenerateObject(child);
                }
            }
            foreach (GoObject child in x)
            {
                if (Skip(x, child)) continue;
                if (!(child is IGoNode))
                {
                    this.Writer.GenerateObject(child);
                }
            }
        }

        public override void GenerateDefinitions(Object obj)
        {
        //    base.GenerateDefinitions(obj);
            TreeSubGraph x = (TreeSubGraph)obj;
            foreach (GoObject child in x)
            {
                if (!Skip(x, child))
                {
                    this.Writer.DefineObject(child);
                }
            }
        }

		// Private Methods (1) 

        private bool Skip(GoSubGraph x, GoObject c)
        {
            return (c == x.Handle || c == x.Label || c == x.Port || c == x.CollapsedObject);
        }

		#endregion Methods 

    }
}
