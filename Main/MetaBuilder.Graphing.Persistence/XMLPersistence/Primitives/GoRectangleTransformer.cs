using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Primitives
{
    public class GoRectangleTransformer : BaseGoObjectTransformer
    {

        #region Constructors (1)

        public GoRectangleTransformer()
            : base()
        {
            this.TransformerType = typeof(GoRectangle);
            this.ElementName = "GoRect";
            this.IdAttributeUsedForSharedObjects = true;
        }

        #endregion Constructors

        #region Methods (4)

        // Public Methods (4) 

        public override object Allocate()
        {
            return new GoRectangle();
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            //4 June 2013
            (obj as GoRectangle).Selectable = true;
            (obj as GoRectangle).Deletable = true;
            (obj as GoRectangle).Movable = true;
            (obj as GoRectangle).DragsNode = true;
        }
        public override void ConsumeObjectFinish(object obj)
        {
            base.ConsumeObjectFinish(obj);
        }

        public override void GenerateAttributes(object obj)
        {
            base.GenerateAttributes(obj, true);
        }

        public override bool GenerateElement(object obj)
        {
            if (obj is GraphNodeGrid)
                return false;
            return base.GenerateElement(obj);
        }

        #endregion Methods

    }
}