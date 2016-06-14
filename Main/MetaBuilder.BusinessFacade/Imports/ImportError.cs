namespace MetaBuilder.BusinessFacade.Imports
{
	/// <summary>
	/// Summary description for ImportError.
	/// </summary>
	public class ImportError
	{
		public string Description;
		public string Location;

		public ImportError(string Description, string Location)
		{
			this.Description = Description;
			this.Location = Location;
		}
	}

	public class ImportWarning
	{
		public string Description;
		public string Location;

		public ImportWarning(string Description, string Location)
		{
			this.Description = Description;
			this.Location = Location;
		}
	}

}