using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Formatting;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go.Xml;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence
{
    public class MetaGraphXMLWriter : GoXmlWriter
    {

        #region Methods (4)

        // Public Methods (1) 

        public override void GenerateObject(object obj)
        {
            if (IsValidElement(obj))
                base.GenerateObject(obj);
        }

        private FileType ftype;
        public FileType File_Type
        {
            get { return ftype; }
            set { ftype = value; }
        }

        // Protected Methods (1) 

        protected override void GenerateRootAttributes()
        {
            base.GenerateRootAttributes();
            XmlWriter w = this.XmlWriter;
            NormalDiagram diagram = GetDiagram();
            if (diagram != null)
            {
                DocumentInfo docInfo = diagram.DocumentFrame.GetDocumentInfo();
                w.WriteAttributeString("ContainsMappingCells", diagram.ContainsILinkContainers.ToString());
                w.WriteAttributeString("Authors", docInfo.Author);
                w.WriteAttributeString("ID", docInfo.AuthorID);
                w.WriteAttributeString("CompanyName", docInfo.OrganisationUnit);
                w.WriteAttributeString("Description", docInfo.Name);
                w.WriteAttributeString("DocDate", docInfo.Date.ToString());
                w.WriteAttributeString("Version", docInfo.Version);
                w.WriteAttributeString("DocType", docInfo.Description.ToString());
                w.WriteAttributeString("FileName", docInfo.FileName);
                w.WriteAttributeString("FrameBoundsX", diagram.DocumentFrame.Bounds.X.ToString());
                w.WriteAttributeString("FrameBoundsY", diagram.DocumentFrame.Bounds.Y.ToString());
                w.WriteAttributeString("FrameBoundsWidth", diagram.DocumentFrame.Bounds.Width.ToString());
                w.WriteAttributeString("FrameBoundsHeight", diagram.DocumentFrame.Bounds.Height.ToString());
                w.WriteAttributeString("OriginalFileUniqueID", diagram.VersionManager.CurrentVersion.OriginalFileUniqueIdentifier.ToString());
                w.WriteAttributeString("WasNumbered", diagram.WasNumbered.ToString());
                w.WriteAttributeString("APV", diagram.ArtefactPointersVisible.ToString());
                w.WriteAttributeString("SOI", diagram.ShowObjectImages.ToString());

                int wsTypeID = diagram.VersionManager.CurrentVersion.WorkspaceTypeId;
                if (wsTypeID == 0)
                    wsTypeID = Core.Variables.Instance.CurrentWorkspaceTypeId;

                string wsName = diagram.VersionManager.CurrentVersion.WorkspaceName;
                if (wsName == string.Empty)
                    wsName = Core.Variables.Instance.CurrentWorkspaceName;

                w.WriteAttributeString("WorkspaceTypeId", wsTypeID.ToString());
                w.WriteAttributeString("workspacename", wsName);

                w.WriteAttributeString("MinorVersion", diagram.VersionManager.CurrentVersion.MinorVersion.ToString());
                w.WriteAttributeString("MajorVersion", diagram.VersionManager.CurrentVersion.MajorVersion.ToString());
                w.WriteAttributeString("AppVersion", Application.ProductVersion);
                w.WriteAttributeString("MachineName", Environment.MachineName);
                w.WriteAttributeString("ModifiedDate", DateTime.Now.ToShortDateString());
                w.WriteAttributeString("Notes", diagram.VersionManager.CurrentVersion.Notes);
                w.WriteAttributeString("PreviousDocumentID", diagram.VersionManager.CurrentVersion.PKID.ToString());

                if (diagram.LastPrintSettings != null)
                {
                    DiagramPrintSettings ds = diagram.LastPrintSettings;
                    w.WriteAttributeString("Adjustment", ds.Adjustment.ToString());
                    w.WriteAttributeString("Copies", ds.Copies.ToString());
                    w.WriteAttributeString("FitAcross", ds.FitAcross.ToString());
                    w.WriteAttributeString("FitDown", ds.FitDown.ToString());
                    w.WriteAttributeString("Landscape", ds.PageSet.Landscape.ToString());
                    w.WriteAttributeString("MarginLeft", ds.PageSet.Margins.Left.ToString());
                    w.WriteAttributeString("MarginRight", ds.PageSet.Margins.Right.ToString());
                    w.WriteAttributeString("MarginTop", ds.PageSet.Margins.Top.ToString());
                    w.WriteAttributeString("MarginBottom", ds.PageSet.Margins.Bottom.ToString());
                    w.WriteAttributeString("PaperSizeName", ds.PageSet.PaperSize.PaperName);
                    w.WriteAttributeString("PaperSizeWidth", ds.PageSet.PaperSize.Width.ToString());
                    w.WriteAttributeString("PaperSizeHeight", ds.PageSet.PaperSize.Height.ToString());
                    w.WriteAttributeString("PrinterName", ds.PrintSettings.PrinterName);
                    w.WriteAttributeString("PaperSizeRawKind", ds.PageSet.PaperSize.RawKind.ToString());
                    w.WriteAttributeString("Color", ds.PageSet.Color.ToString());
                }

                w.WriteAttributeString("SheetPositionX", diagram.VersionManager.CurrentVersion.SheetBounds.X.ToString());
                w.WriteAttributeString("SheetPositionY", diagram.VersionManager.CurrentVersion.SheetBounds.Y.ToString());
                w.WriteAttributeString("SheetWidth", diagram.VersionManager.CurrentVersion.SheetBounds.Width.ToString());
                w.WriteAttributeString("SheetHeight", diagram.VersionManager.CurrentVersion.SheetBounds.Height.ToString());
            }
        }

        // Private Methods (2) 

        private NormalDiagram GetDiagram()
        {
            return Objects as NormalDiagram;
        }

        private bool IsValidElement(object obj)
        {
            bool invalid = (obj is FrameLayerGroup);

            if (obj is FishLink)
            {
                FishLink flink = obj as FishLink;
                if (!(flink.IsValidFishLink()))
                    return false;
            }

            return !invalid;
        }

        #endregion Methods

    }
}