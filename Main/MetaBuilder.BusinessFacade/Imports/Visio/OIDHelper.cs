using System.Data;
using MetaBuilder.DataAccessLayer.OldCode.Imports.Visio;

namespace MetaBuilder.BusinessFacade.Imports.Visio
{
	/// <summary>
	/// Summary description for OIDHelper.
	/// </summary>
	public class OIDHelper
	{
		public OIDHelper()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public DataView GetOIDDrawings()
		{
			VisioDataAdapter vda = new VisioDataAdapter();
			return vda.GetOIDDrawings();
		}

		public void BuildReportingCache()
		{
			VisioDataAdapter vda = new VisioDataAdapter();
			vda.BuildOIDReportingCache();
		}
	}
}