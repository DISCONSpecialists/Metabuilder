namespace MetaBuilder.Meta
{
    class AssociationTypeHelper
    {
        private static AssociationTypeHelper globalInstance = null;
        public AssociationTypeHelper()
        {
            
        }
        public static AssociationTypeHelper Instance
		{
            get{
			    if( globalInstance == null )
			    {
                    globalInstance = new AssociationTypeHelper();
			    }
			    return globalInstance;
            }
		}

    }
    
}
