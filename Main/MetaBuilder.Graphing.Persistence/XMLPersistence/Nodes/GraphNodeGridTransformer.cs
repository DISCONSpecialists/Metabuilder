using MetaBuilder.Graphing.Shapes;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Nodes
{
    public class GraphNodeGridTransformer : BaseGoObjectTransformer
    {

		#region Constructors (1) 

        public GraphNodeGridTransformer()
            : base()
        {
            this.TransformerType = typeof(GraphNodeGrid);
            this.ElementName = "grid";
        }

		#endregion Constructors 

		#region Methods (2) 


		// Public Methods (2) 

        public override object Allocate()
        {
            GraphNodeGrid grid = new GraphNodeGrid();
            return grid;
        }

        public override bool GenerateElement(object obj)
        {
            return false;
        }

		#endregion Methods 

    }
}