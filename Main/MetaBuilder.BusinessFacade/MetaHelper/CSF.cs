using MetaBuilder.DataAccessLayer.OldCode.Meta;

namespace MetaBuilder.BusinessFacade.MetaHelper
{
    public class CSF
    {

        public static void CalculateCriticalities()
        {
            AssociationAdapter adap = new AssociationAdapter(false);
            adap.CalculateCriticalities();
        }
    }
}
