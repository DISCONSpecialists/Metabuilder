using System;

namespace MetaBuilder.BusinessFacade.Imports
{
	/// <summary>
	/// Summary description for IImporter.
	/// </summary>
	public interface IImporter
	{
		void Import(string[] SourceFiles, bool IndicateErrors);
		event FileStatusChangedEventHandler FileStatusChanged;
		event EventHandler FileProgressChanged;
		event MaximumDeterminedEventHandler MaximumDeterminded;
	}
}