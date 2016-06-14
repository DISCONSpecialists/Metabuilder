using System;
using System.Drawing;
using System.Drawing.Printing;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Formatting;
using Northwoods.Go;
using Northwoods.Go.Xml;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence
{
    public class MetaGraphXMLReader : GoXmlReader
    {

        #region Constructors (1)

        public MetaGraphXMLReader()
            : base()
        {
            this.UseDOM = true;
        }

        #endregion Constructors

        #region Properties (1)

        // This GoDocument gets all the newly read GoObjects Add'ed to it.
        // If not set to a document before consuming the GraphML,
        // the result will be a new GoDocument containing the graph.
        public NormalDiagram Document
        {
            get { return RootObject as NormalDiagram; }
            set { RootObject = value; }
        }

        #endregion Properties

        #region Methods (4)

        // Public Methods (1) 

        /*
         * {
           
            }*
         */
        public override void ProcessDelayedObjects()
        {
            base.ProcessDelayedObjects();
        }

        // Protected Methods (2) 
        private string removeDecimals(string rawnumber)
        {
            //if (rawnumber.Contains(",") || rawnumber.Contains("."))
            //{
            //    char[] skipAfterTheseChars = new char[] { ',', '.' };
            //    return rawnumber.Substring(0, rawnumber.IndexOfAny(skipAfterTheseChars));
            //}

            return rawnumber.Replace(",", ".");
        }
        protected override object ConsumeRootAttributes(object obj)
        {
            string docDateString = ReadAttrVal("DocDate");
            if (docDateString != null)
            {
                NormalDiagram ndiagram = RootObject as NormalDiagram;
                ndiagram.StartTransaction();
                ndiagram.CreateFrameLayer(null);
                DocumentInfo docInfo = new DocumentInfo();
                docInfo.Author = ReadAttrVal("Authors");
                docInfo.AuthorID = ReadAttrVal("ID");
                docInfo.Date = Core.GlobalParser.ParseGlobalisedDateString(docDateString);
                docInfo.OrganisationUnit = ReadAttrVal("CompanyName");

                //docInfo.Version = ReadAttrVal("Version");
                docInfo.FileName = ReadAttrVal("FileName");
                try
                {
                    docInfo.Name = ReadAttrVal("Description");
                    docInfo.Description = ReadAttrVal("DocType");
                }
                catch (Exception ex)
                {
                    Core.Log.WriteLog("Missing doctype attribute " + Environment.NewLine + ex.ToString());
                }

                try
                {
                    string numbered = ReadAttrVal("WasNumbered");
                    if (!string.IsNullOrEmpty(numbered))
                        ndiagram.WasNumbered = bool.Parse(numbered);

                    string APV = ReadAttrVal("APV");
                    if (!string.IsNullOrEmpty(APV))
                        ndiagram.ArtefactPointersVisible = bool.Parse(APV);

                    string SOI = ReadAttrVal("SOI");
                    if (!string.IsNullOrEmpty(SOI))
                        ndiagram.ShowObjectImages = bool.Parse(SOI);
                }
                catch (Exception ex)
                {
                    //Core.Log.WriteLog("Missing WasNumbered attribute " + Environment.NewLine + ex.ToString());
                }

                docInfo.OriginalFileUniqueID = new Guid(ReadAttrVal("OriginalFileUniqueID"));

                ndiagram.Clear();
                ndiagram.FrameLayer = ndiagram.Layers.CreateNewLayerBefore(null);
                ndiagram.Layers.MoveBefore(ndiagram.LinksLayer, ndiagram.FrameLayer);
                ndiagram.Layers.MoveBefore(ndiagram.LinksLayer, ndiagram.DefaultLayer);
                ndiagram.Layers.MoveAfter(null, ndiagram.LinksLayer);
                ndiagram.InitVersionManager();
                ndiagram.VersionManager.CurrentVersion.OriginalFileUniqueIdentifier = docInfo.OriginalFileUniqueID;
                //ndiagram.VersionManager.CurrentVersion.MajorVersion = docInfo. = docInfo.OriginalFileUniqueID;
                ndiagram.DocumentFrame.Reposition();
                ndiagram.FileType = FileTypeList.Diagram;

                //ndiagram.UndoManager = new GoUndoManager();

                ndiagram.DocumentFrame = new FrameLayerGroup();
                ndiagram.FrameLayer.Add(ndiagram.DocumentFrame);
                ndiagram.FrameLayer.AllowCopy = false;
                ndiagram.FrameLayer.AllowSelect = false;
                ndiagram.FrameLayer.AllowDelete = false;

                float frameBoundsX = float.Parse(ReadAttrVal("FrameBoundsX"), System.Globalization.CultureInfo.InvariantCulture);
                float frameBoundsY = float.Parse(ReadAttrVal("FrameBoundsY"), System.Globalization.CultureInfo.InvariantCulture);
                if (frameBoundsX < 0)
                    frameBoundsX = 60;
                if (frameBoundsY < 0)
                    frameBoundsY = 0;

                string boundsWidth = removeDecimals(ReadAttrVal("FrameBoundsWidth"));
                string boundsHeight = removeDecimals(ReadAttrVal("FrameBoundsHeight"));

                float frameBoundsWidth = float.Parse(boundsWidth, System.Globalization.CultureInfo.InvariantCulture);
                float frameBoundsHeight = float.Parse(boundsHeight, System.Globalization.CultureInfo.InvariantCulture);

                int minorVersion = int.Parse(ReadAttrVal("MinorVersion"));
                int majorVersion = int.Parse(ReadAttrVal("MajorVersion"));
                if (ReadAttrVal("Version") != null)
                {
                    docInfo.Version = ReadAttrVal("Version");
                }
                else
                {
                    if (string.IsNullOrEmpty(docInfo.Version))
                        docInfo.ForceVersion(majorVersion, minorVersion);
                    else
                        docInfo.Version = "";
                }
                string appVersion = ReadAttrVal("AppVersion");

                string wsTypeID = ReadAttrVal("WorkspaceTypeId");
                int WorkspaceTypeId = 0;
                if (wsTypeID != null)
                {
                    bool canParse = int.TryParse(wsTypeID, out WorkspaceTypeId);
                    if (!canParse)
                        WorkspaceTypeId = Core.Variables.Instance.CurrentWorkspaceTypeId;
                }

                if (WorkspaceTypeId == 0)
                {
                    WorkspaceTypeId = 1;
                }

                string wsName = ReadAttrVal("workspacename");
                if (wsName == null)
                    wsName = Core.Variables.Instance.CurrentWorkspaceName;
                if (wsName.Length == 0)
                    wsName = Core.Variables.Instance.CurrentWorkspaceName;

                string maps = ReadAttrVal("ContainsMappingCells");
                bool containsMappings = false;
                if (maps != null)
                    if (maps.Length > 0)
                    {
                        containsMappings = bool.Parse(maps);
                    }
                ndiagram.ContainsILinkContainers = containsMappings;
                ndiagram.VersionManager.CurrentVersion.MinorVersion = minorVersion;
                ndiagram.VersionManager.CurrentVersion.MajorVersion = majorVersion;
                ndiagram.VersionManager.CurrentVersion.AppVersion = appVersion;
                ndiagram.VersionManager.CurrentVersion.FileType = FileTypeList.Diagram;
                ndiagram.VersionManager.CurrentVersion.MachineName = ReadAttrVal("MachineName");
                ndiagram.VersionManager.CurrentVersion.ModifiedDate = Core.GlobalParser.ParseGlobalisedDateString(ReadAttrVal("ModifiedDate"));
                ndiagram.VersionManager.CurrentVersion.Notes = ReadAttrVal("Notes");
                ndiagram.VersionManager.CurrentVersion.WorkspaceTypeId = WorkspaceTypeId;
                ndiagram.VersionManager.CurrentVersion.WorkspaceName = wsName;
                int prevFileID = 0;
                int.TryParse(ReadAttrVal("PreviousDocumentID"), out prevFileID);
                if (prevFileID > 0)
                    ndiagram.VersionManager.CurrentVersion.PreviousDocumentID = prevFileID;

                RectangleF docBounds = new RectangleF(frameBoundsX, frameBoundsY, frameBoundsWidth, frameBoundsHeight);

                string PrinterNameString = ReadAttrVal("PrinterName");
                if (PrinterNameString != null)
                {
                    //foreach (String s in PrinterSettings.InstalledPrinters)
                    {
                        //if (s == PrinterNameString)
                        {
                            DiagramPrintSettings ds = new DiagramPrintSettings();
                            string adjustment = ReadAttrVal("Adjustment");
                            ds.Adjustment = (DiagramPrintSettings.PageAdjustment)Enum.Parse(typeof(DiagramPrintSettings.PageAdjustment), adjustment);
                            ds.Copies = int.Parse(ReadAttrVal("Copies"));
                            ds.FitAcross = int.Parse(ReadAttrVal("FitAcross"));
                            ds.FitDown = int.Parse(ReadAttrVal("FitDown"));
                            ds.PageSet = new System.Drawing.Printing.PageSettings();
                            ds.PageSet.Landscape = bool.Parse(ReadAttrVal("Landscape"));
                            int mLeft = int.Parse(ReadAttrVal("MarginLeft"));
                            int mRight = int.Parse(ReadAttrVal("MarginRight"));
                            int mTop = int.Parse(ReadAttrVal("MarginTop"));
                            int mBottom = int.Parse(ReadAttrVal("MarginBottom"));
                            ds.PageSet.Margins = new System.Drawing.Printing.Margins(mLeft, mRight, mTop, mBottom);

                            string psizeName = ReadAttrVal("PaperSizeName");
                            int psizeWidth = int.Parse(ReadAttrVal("PaperSizeWidth"));
                            int psizeHeight = int.Parse(ReadAttrVal("PaperSizeHeight"));
                            ds.PageSet.PaperSize = new System.Drawing.Printing.PaperSize(psizeName, psizeWidth, psizeHeight);
                            ds.PageSet.Color = bool.Parse(ReadAttrVal("Color"));
                            ds.PrintSettings.PrinterName = PrinterNameString;

                            string pszRaw = ReadAttrVal("PaperSizeRawKind");
                            if (pszRaw != null)
                            {
                                if (pszRaw.Length > 0)
                                {
                                    ds.PageSet.PaperSize.RawKind = int.Parse(pszRaw);
                                    ndiagram.LastPrintSettings = ds;
                                }
                                else
                                    ndiagram.LastPrintSettings = null;
                            }
                            //break;
                        }
                    }
                }

                string testForSheetInfo = ReadAttrVal("SheetPositionX");
                if (testForSheetInfo != null)
                {
                    float sheetX = float.Parse(ReadAttrVal("SheetPositionX"), System.Globalization.CultureInfo.InvariantCulture);
                    float sheetY = float.Parse(ReadAttrVal("SheetPositionY"), System.Globalization.CultureInfo.InvariantCulture);

                    if (sheetX < 0)
                        sheetX = 0;
                    if (sheetY < 0)
                        sheetY = 0;

                    float sheetWidth = float.Parse(removeDecimals(ReadAttrVal("SheetWidth")), System.Globalization.CultureInfo.InvariantCulture);
                    float sheetHeight = float.Parse(removeDecimals(ReadAttrVal("SheetHeight")), System.Globalization.CultureInfo.InvariantCulture);

                    ndiagram.VersionManager.CurrentVersion.SheetBounds = new RectangleF(sheetX, sheetY, sheetWidth, sheetHeight);

                    if (sheetWidth == 0)
                    {
                        Core.Log.WriteLog("Sheet width is 0, computing bounds");
                        ndiagram.VersionManager.CurrentVersion.SheetBounds = ndiagram.ComputeBounds();
                    }
                }
                else
                {
                    Core.Log.WriteLog("Test for sheet info failed, defaulting diagram sheet");
                }

                //if (testForSheetInfo != null)
                //    ndiagram.DocumentFrame.Setup(ndiagram.VersionManager.CurrentVersion.SheetBounds);//docBounds
                //else
                ndiagram.DocumentFrame.Setup(docBounds);//

                ndiagram.DocumentFrame.Update(docInfo);
                //ndiagram.Add(ndiagram.DocumentFrame);

                ndiagram.FinishTransaction("MetaGraphXMLReader::ConsumeRootAttributes");
            }

            return base.ConsumeRootAttributes(obj);
        }

        // return a GoDocument that will be modified;
        // if no value was supplied for a root object,
        // this method creates and returns a GoDocument.
        protected override Object ConsumeRootElement()
        {
            if (this.XmlDocument != null)
            {
                // when traversing the DOM, set the ReaderNode
                this.ReaderNode = this.XmlDocument.DocumentElement;
            }
            if (RootObject == null)
            {
                RootObject = new NormalDiagram();
            }

            return RootObject;
        }

        // Private Methods (1) 

        private void ReadDocumentInfo()
        {

        }

        #endregion Methods

    }
}