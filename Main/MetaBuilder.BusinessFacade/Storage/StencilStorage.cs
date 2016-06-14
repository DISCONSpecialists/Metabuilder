using System.Data;
using MetaBuilder.DataAccessLayer.OldCode.Diagramming;

namespace MetaBuilder.BusinessFacade.Storage
{
	/// <summary>
	/// Summary description for Stencil.
	/// </summary>
	public class StencilStorage
	{
		public StencilStorage()
		{
			
		}

		public DataView GetList()
		{
			StencilStorageAdapter stencilAdapter = new StencilStorageAdapter();
			return stencilAdapter.GetList();
		}

		public DataView GetList(string StencilType)
		{
			StencilStorageAdapter stencilAdapter = new StencilStorageAdapter();
			return stencilAdapter.GetList(StencilType);
		}
	}
}