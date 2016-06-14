using Northwoods.Go;
using Northwoods.Go.Layout;

namespace MetaBuilder.Graphing.Layout
{
    public class GoGraphDocLayoutNetwork : GoLayoutNetwork
    {
        #region Constructors (1) 

        public GoGraphDocLayoutNetwork(IGoCollection coll)
        {
            AddNodesAndLinksFromCollection(coll, false);
        }

        #endregion Constructors 

        #region Methods (1) 

        // Public Methods (1) 

        public override void AddNodesAndLinksFromCollection(IGoCollection collection, bool onlytruenodes)
        {
            base.AddNodesAndLinksFromCollection(collection, false);
        }

        #endregion Methods 
    }
}