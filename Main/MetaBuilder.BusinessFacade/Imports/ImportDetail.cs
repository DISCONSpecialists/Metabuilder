using System.Collections;

namespace MetaBuilder.BusinessFacade.Imports
{
	/// <summary>
	/// Summary description for ImportDetail.
	/// </summary>
	public class ImportDetail
	{
		private ArrayList importErrors;
		private ArrayList importWarnings;
		private int drawingID;

		public ImportDetail()
		{
		}

		public int DrawingID
		{
			get { return drawingID; }
			set { drawingID = value; }
		}

		public bool HasWarnings
		{
			get { return hasWarnings; }
		}

		private bool hasWarnings
		{
			get { return (ImportWarnings != null); }
		}

		public bool HasErrors
		{
			get { return hasErrors; }
		}

		private bool hasErrors
		{
			get { return (ImportErrors != null); }
		}

		public ArrayList ImportErrors
		{
			get { return importErrors; }
		}

		public ArrayList ImportWarnings
		{
			get { return importWarnings; }
		}

		public void AddError(ImportError error)
		{
			if (this.importErrors == null)
			{
				this.importErrors = new ArrayList();
			}
			this.importErrors.Add(error);
		}

		public void AddWarning(ImportWarning warning)
		{
			if (this.importWarnings == null)
			{
				this.importWarnings = new ArrayList();
			}
			this.importWarnings.Add(warning);
		}

	}
}