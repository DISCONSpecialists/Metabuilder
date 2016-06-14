namespace MetaBuilder.BusinessFacade.Imports
{
	public class MaximumDeterminedArgs
	{
		public int Maximum;

		public MaximumDeterminedArgs()
		{
		}
	}

	public class FileProgressChangedArgs
	{
		public int Progress;

		public FileProgressChangedArgs()
		{
		}
	}

	public class FileStatusChangedArgs
	{
		public string Document;
		public ImportDetail Detail;

		public FileStatusChangedArgs()
		{
		}
	}
}