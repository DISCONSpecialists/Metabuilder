using System.Data;
using MetaBuilder.DataAccessLayer.OldCode.Imports.Visio;

namespace MetaBuilder.BusinessFacade.Imports.Visio
{
	/// <summary>
	/// Helper class for Visio Master & Master Properties
	/// </summary>
	public class VisioMasterHelper
	{
		public VisioMasterHelper()
		{
		}

		public DataSet GetVisioMasterInformation()
		{
			VisioDataAdapter vmDA = new VisioDataAdapter();
			return vmDA.GetVisioMasterInformation();
		}

		public int SaveVisioDrawing(string DrawingName, string DrawingVersion, string DrawingType)
		{
			VisioDataAdapter vmDA = new VisioDataAdapter();
			return vmDA.SaveVisioDrawing(DrawingName, DrawingVersion, DrawingType);
		}

		public void DeleteVisioDrawing(int DrawingID)
		{
			VisioDataAdapter vmDA = new VisioDataAdapter();
			vmDA.DeleteVisioDrawing(DrawingID);
		}

		public void AddVisioDrawingAssociation(int DrawingID, int ObjectID1, int ObjectID2, int AssociationTypeID)
		{
			VisioDataAdapter vmada = new VisioDataAdapter();
			vmada.AddVisioDrawingAssociation(DrawingID, ObjectID1, ObjectID2, AssociationTypeID);
		}

		public void AddVisioDrawingObject(int DrawingID, int ObjectID)
		{
            MetaBuilder.BusinessLogic.GraphFileObject obj = new MetaBuilder.BusinessLogic.GraphFileObject();
            obj.GraphFileID = DrawingID;
            obj.MetaObjectID = ObjectID;
            MetaBuilder.DataAccessLayer.DataRepository.GraphFileObjectProvider.Delete(new MetaBuilder.BusinessLogic.GraphFileObjectKey(DrawingID, ObjectID));
            MetaBuilder.DataAccessLayer.DataRepository.GraphFileObjectProvider.Save(obj);
            
            /*VisioDataAdapter vmada = new VisioDataAdapter();
            /*VisioDataAdapter vmada = new VisioDataAdapter();
            vmada.AddVisioDrawingObject(DrawingID, ObjectID);*/
		}

		public DataView CheckForExistingDrawing(string DrawingName)
		{
			VisioDataAdapter vmada = new VisioDataAdapter();
			return vmada.CheckForExistingDrawing(DrawingName);
		}

		public void AddVisioDrawingFlow(int DrawingID, int FromObj, int ToObj, int ArtifactID)
		{
			VisioDataAdapter vmada = new VisioDataAdapter();
			vmada.AddVisioDrawingFlow(DrawingID, FromObj, ToObj, ArtifactID);
		}


	}
}