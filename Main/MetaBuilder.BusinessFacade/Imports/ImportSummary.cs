using System.Collections;

namespace MetaBuilder.BusinessFacade.Imports
{
	/// <summary>
	/// Summary description for ImportSummary.
	/// </summary>
	public class ImportSummary
	{
		public ImportSummary()
		{
			details = new ArrayList();
		}

		private ArrayList details;

		public ArrayList Details
		{
			get { return details; }
			set { details = value; }
		}

		public bool ErrorsOccurred
		{
			get { return errorsOccurred; }
		}

		private bool errorsOccurred
		{
			get
			{
				bool FoundErrors = false;
				foreach (ImportDetail detail in this.details)
				{
					if (detail.HasErrors)
					{
						FoundErrors = true;
					}
				}
				return FoundErrors;
			}
		}
	}
}