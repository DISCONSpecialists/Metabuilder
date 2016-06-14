using System;
using MetaBuilder.Graphing.Persistence.XMLPersistence.Nodes;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Groups
{

    public class GoGroupTransformer : EmbeddedObjectsTransformer
    {

        #region Constructors (1)

        public GoGroupTransformer()
            : base()
        {
            this.TransformerType = typeof(GoGroup);
            this.ElementName = "group";
            this.BodyConsumesChildElements = true;
            this.IdAttributeUsedForSharedObjects = true;
        }

        #endregion Constructors

        #region Methods (4)


        // Public Methods (4) 

        public override object Allocate()
        {
            GoGroup obj = new GoGroup();
            return obj;
        }

        public override void ConsumeChild(object parent, object child)
        {
            base.ConsumeChild(parent, child);
            GoGroup grpParent = parent as GoGroup;
            if (child is GoObject)
            {
                if (child is GoText && !(child is BoundLabel))
                    (child as GoText).Deletable = (child as GoText).Movable = false;

                GoObject oChild = child as GoObject;
                if (!(grpParent.Contains(oChild)))
                {
                    oChild.Remove();
                    grpParent.Add(oChild);
                }
            }
        }
        //override g
        public override void ConsumeObjectFinish(object obj)
        {
            base.ConsumeObjectFinish(obj);

            GoGroup oGroup = obj as GoGroup;
            if (oGroup.ParentNode is GraphNode)
            {
                GoGroupEnumerator groupEnum = oGroup.GetEnumerator();
                while (groupEnum.MoveNext())
                {
                    if (groupEnum.Current is BoundLabel)
                    {
                        groupEnum.Current.AddObserver(oGroup.ParentNode);
                    }
                }
            }
        }

        public override void GenerateAttributes(object obj)
        {
            if (obj.GetType().Name.ToString().EndsWith("Group"))
                base.GenerateAttributes(obj, true);
            else
                base.GenerateAttributes(obj);
        }

        #endregion Methods

    }
}