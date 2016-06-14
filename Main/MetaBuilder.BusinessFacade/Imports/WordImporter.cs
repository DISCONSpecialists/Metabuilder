using System;
using System.Collections;
using System.Reflection;
using InteropWord2K;
using Word = InteropWord2K;

namespace MetaBuilder.BusinessFacade.Imports.FSD
{
	/// <summary>
	/// Parses Word Documents, creates FunctionStructures as per indentation/bulleted lists.
	/// All Non-Decompositions need to be changed manually afterwards.
	/// </summary>
	public class WordImporter :BaseImporter
	{
		public Application application;

		public WordImporter()
		{
		}

		public override void Import(string[] Files, bool IndicateErrors)
		{
			object readOnly = false;
			object isVisible = true;
			object missing = Missing.Value;
			application = new ApplicationClass();
			application.Visible = true;
			for (int FileCounter = 0; FileCounter < Files.Length; FileCounter++)
			{
				object fileName = Files[FileCounter];
				Document activeDocument = application.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref isVisible);
				activeDocument.Activate();
				string line = string.Empty;
				int Indent;
				int Tabs = 0;
				int DefaultTabStop = Convert.ToInt32(activeDocument.DefaultTabStop);
				//LineFunction[] lines = new LineFunction[]{};
				SortedList lines = new SortedList();

				int LineNo = 0;
				LineFunction lfunction;
				long p_count = activeDocument.Paragraphs.Count;

				for (int parcounter = 1; parcounter <= p_count; parcounter++)
				{
					Paragraph currentParagraph = activeDocument.Paragraphs.Item(parcounter);

					line = currentParagraph.Range.Text;
					Indent = Convert.ToInt32(Convert.ToInt32(currentParagraph.LeftIndent)/DefaultTabStop);

					string trimmedAndUnTabbed = line.Trim().Replace("\t", "");
					string trimmedAndUnreturned = trimmedAndUnTabbed.Replace("\n", "").Replace("\r", "");
					if (trimmedAndUnreturned.Length > 0)
					{
						lfunction = new LineFunction();
						lfunction.Line = trimmedAndUnreturned;
						Tabs = GetNumberOfTabs(line);
						lfunction.Indent = Tabs + Indent;
						lfunction.Function = new Function(lfunction.Line);
                        lfunction.LineNo = LineNo;
						lines.Add(LineNo, lfunction);
						LineNo++;
					}
				}


				for (int l = 0; l < lines.Count; l++)
				{
					lfunction = (LineFunction) lines[l];
					lfunction.Function = new Function(lfunction.Line);
					Function possibleParent = GetParentFunction(lines, lfunction);
					if (possibleParent != null)
						lfunction.Function.ParentFunction = possibleParent;
				}

				for (int l = 0; l < lines.Count; l++)
				{
					lfunction = (LineFunction) lines[l];
					lfunction.Function.Save();
				}

				for (int l = 0; l < lines.Count; l++)
				{
					lfunction = (LineFunction) lines[l];
					if (lfunction.Indent == 0)
					{
						lfunction.Function.DebugFunction(lfunction.Indent);
					}
				}


				FileStatusChangedArgs fileStatusArgs = new FileStatusChangedArgs();
				fileStatusArgs = new FileStatusChangedArgs();
				fileStatusArgs.Detail = new ImportDetail();
				fileStatusArgs.Detail.DrawingID = 5;
				fileStatusArgs.Document = Files[0];
				object save = true;

				(activeDocument as _Document).Close(ref save, ref missing, ref missing);
				this.OnFileStatusChanged(fileStatusArgs);
			}
			(application as _Application).Quit(ref missing, ref missing, ref missing);

		}

		private Function GetParentFunction(SortedList lines, LineFunction lfunction)
		{
			LineFunction testLFunction;

			for (int x = lfunction.LineNo; x >= 0; x--)
			{
				testLFunction = (LineFunction) lines[x];
				if (testLFunction.Indent == lfunction.Indent - 1)
				{
					return testLFunction.Function;
				}
			}
			return null;
		}

		private int GetNumberOfTabs(string TabbedString)
		{
            char tabChar = '\t';
            string[] split = TabbedString.Split(tabChar);
            return split.Length;
            /*
			int numberOfTabs = 0;
			int tabIndex = TabbedString.IndexOf("\t");
			if (tabIndex > -1)
			{
				numberOfTabs++;
			}

			while (TabbedString.StartsWith("\t"))
			{
				TabbedString = TabbedString.Substring(1, TabbedString.Length - 1);
				numberOfTabs++;
			}
			return numberOfTabs;*/
		}
	}

	public class LineFunction
	{
		public string Line;
		public int LineNo;
		public int Indent;
		public Function Function;
	}
}