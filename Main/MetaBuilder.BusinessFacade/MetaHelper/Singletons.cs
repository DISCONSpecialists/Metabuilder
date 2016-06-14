namespace MetaBuilder.BusinessFacade.MetaHelper
{
    public class Singletons
    {
        private static AssociationHelper associationHelper = null;
        private static ClassHelper classHelper = null;
        private static ObjectHelper objectHelper = null;

        public static AssociationHelper GetAssociationHelper()
        {
            if (associationHelper == null)
            {
                associationHelper = new AssociationHelper();
            }
            return associationHelper;
        }

        public static ClassHelper GetClassHelper()
        {
            if (classHelper == null)
            {
                classHelper = new ClassHelper();
            }
            return classHelper;
        }

        public static ObjectHelper GetObjectHelper()
        {
            if (objectHelper == null)
            {
                objectHelper = new ObjectHelper(false);
            }
            return objectHelper;
        }
       
    }
}
