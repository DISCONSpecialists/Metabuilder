using System.Data;
using MetaBuilder.DataAccessLayer.OldCode.Imports.Visio;

namespace MetaBuilder.BusinessFacade.Imports.Visio
{
	/// <summary>
	/// Summary description for ADDHelper.
	/// </summary>
	public class ADDHelper
	{
		public ADDHelper()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public DataView GetADDDrawings()
		{
			VisioDataAdapter vda = new VisioDataAdapter();
			return vda.GetADDDrawings();
		}

		public DataSet RetrieveWorkspaceEntitiesAndAttributes()
		{
			ADDAdapter adda = new ADDAdapter();
			return adda.RetrieveWorkspaceEntitiesAndAttributes();
		}
	}
}