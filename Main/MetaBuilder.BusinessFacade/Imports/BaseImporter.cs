using System;
using System.Diagnostics;
using System.Threading;

namespace MetaBuilder.BusinessFacade.Imports
{
	public delegate void MaximumDeterminedEventHandler(object sender, MaximumDeterminedArgs e);

	public delegate void FileStatusChangedEventHandler(object sender, FileStatusChangedArgs e);


	/// <summary>
	/// Summary description for BaseImporter.
	/// </summary>
	public class BaseImporter : IImporter
	{
		// TODO: Implement ImportSummary
		// private ImportSummary importSummary;

		// TODO: Will currentDetail be used?
		private ImportDetail currentDetail;

		public BaseImporter()
		{
			currentDetail = new ImportDetail();
		}

		public virtual void Import(string[] Files, bool MarkErrors)
		{
			for (int FileCounter = 0; FileCounter < Files.Length; FileCounter++)
			{
				string FileName = Files[FileCounter];
				FileStatusChangedArgs fileStatusArgs = new FileStatusChangedArgs();
				fileStatusArgs = new FileStatusChangedArgs();
				fileStatusArgs.Detail = new ImportDetail();
				fileStatusArgs.Detail.DrawingID = 5;
				fileStatusArgs.Document = FileName;
				if (FileCounter != Files.Length - 1)
				{
					if (Convert.ToDouble(Convert.ToDouble(FileCounter)/2.0d) == Convert.ToDouble(FileCounter/2))
					{
						ImportError err = new ImportError("An inexplicable error occurred. Redo from start.", "Cell A30");
						fileStatusArgs.Detail.AddError(err);
					}
					else
					{
						ImportWarning warning = new ImportWarning("Import Warning", "Cell A30");
						fileStatusArgs.Detail.AddWarning(warning);
					}
				}


				this.OnFileStatusChanged(fileStatusArgs);
			}
		}

		public void Import(string[] Files)
		{
			Import(Files, false);
		}

		private void AddProgress()
		{
			this.OnFileProgressChanged(EventArgs.Empty);
		}

		private void SetMaximum(int Maximum)
		{
			MaximumDeterminedArgs args = new MaximumDeterminedArgs();
			args.Maximum = Maximum;
			OnMaximumDeterminded(args);
		}

		private void SetFileStatus(FileStatusChangedArgs e)
		{
			FileStatusChangedArgs args = new FileStatusChangedArgs();
			args.Document = this.currentDetail.DrawingID.ToString();
			this.OnFileStatusChanged(e);
		}

		#region EventHandlers

		public event MaximumDeterminedEventHandler MaximumDeterminded;

		protected virtual void OnMaximumDeterminded(MaximumDeterminedArgs e)
		{
			if (MaximumDeterminded != null)
			{
				MaximumDeterminded(this, e);
			}
		}

		public event EventHandler FileProgressChanged;

		protected virtual void OnFileProgressChanged(EventArgs e)
		{
			if (FileProgressChanged != null)
			{
				FileProgressChanged(this, e);
			}
		}

		public event FileStatusChangedEventHandler FileStatusChanged;

		protected virtual void OnFileStatusChanged(FileStatusChangedArgs e)
		{
			if (FileStatusChanged != null)
			{
				FileStatusChanged(this, e);
			}
		}

		#endregion 
	}
}