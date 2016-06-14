using MetaBuilder.DataAccessLayer.OldCode.Tools;

namespace MetaBuilder.BusinessFacade.Tools
{
	/// <summary>
	/// Summary description for Caching.
	/// </summary>
	public class Caching
	{
		public Caching()
		{
			
		}

		public void BuildADDReportCache()
		{
			ReportAdapter rad = new ReportAdapter();
			rad.BuildADDReportCache();
		}
	}
}