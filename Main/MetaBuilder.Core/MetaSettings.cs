using System;
using System.Windows.Forms;
using System.Xml;

namespace MetaBuilder.Core
{
    public class MetaSettings
    {
        XmlDocument xmlDocument = new XmlDocument();

        string documentPath = Application.StartupPath + "\\" + Core.FilterVariables.filterName;

        public MetaSettings()
        {
            try
            {
                if (System.IO.File.Exists(documentPath))
                    xmlDocument.Load(documentPath);
                else
                    xmlDocument.LoadXml("<settings></settings>");
            }
            catch
            {
                xmlDocument.LoadXml("<settings></settings>");
            }
        }

        // Int
        public int GetSetting(string xPath, int defaultValue)
        { return Convert.ToInt16(GetSetting(xPath, Convert.ToString(defaultValue))); }

        public void PutSetting(string xPath, int value)
        { PutSetting(xPath, Convert.ToString(value)); }

        // Decimal
        public decimal GetSetting(string xPath, decimal defaultValue)
        { return Convert.ToDecimal(GetSetting(xPath, Convert.ToString(defaultValue))); }

        public void PutSetting(string xPath, decimal value)
        { PutSetting(xPath, Convert.ToString(value)); }

        // bool
        public bool GetSetting(string xPath, bool defaultValue)
        { return bool.Parse(GetSetting(xPath, Convert.ToString(defaultValue))); }

        public void PutSetting(string xPath, bool value)
        { PutSetting(xPath, Convert.ToString(value)); }

        // Float
        public float GetSetting(string xPath, float defaultValue)
        { return float.Parse(GetSetting(xPath, Convert.ToString(defaultValue)), System.Globalization.CultureInfo.InvariantCulture); }

        public void PutSetting(string xPath, float value)
        { PutSetting(xPath, Convert.ToString(value)); }

        public string GetSetting(string xPath, string defaultValue)
        {
            XmlNode xmlNode = xmlDocument.SelectSingleNode("settings/" + xPath);
            if (xmlNode != null) { return xmlNode.InnerText; }
            else { return defaultValue; }
        }

        public void PutSetting(string xPath, string value)
        {
            XmlNode xmlNode = xmlDocument.SelectSingleNode("settings/" + xPath);
            if (xmlNode == null) { xmlNode = createMissingNode("settings/" + xPath); }
            xmlNode.InnerText = value;
            xmlDocument.Save(documentPath);
        }

        private XmlNode createMissingNode(string xPath)
        {
            string[] xPathSections = xPath.Split('/');
            string currentXPath = "";
            XmlNode testNode = null;
            XmlNode currentNode = xmlDocument.SelectSingleNode("settings");
            foreach (string xPathSection in xPathSections)
            {
                currentXPath += xPathSection;
                testNode = xmlDocument.SelectSingleNode(currentXPath);
                if (testNode == null) { currentNode.InnerXml += "<" + xPathSection + "></" + xPathSection + ">"; }
                currentNode = xmlDocument.SelectSingleNode(currentXPath);
                currentXPath += "/";
            }
            return currentNode;
        }

        public const string PATH_DIAGRAMS = "Paths_Diagrams";
        public const string PATH_EXPORTS = "Paths_Export";
        public const string PATH_SYMBOLS = "Paths_Symbols";
        public const string PATH_SYMBOLSTORES = "Paths_SymbolStores";
        public const string PATH_SOURCE = "Paths_SourceFiles";

        public const string GEN_DBCONNLOCAL = "Database_LocalInstanceConnectionString";
        public const string GEN_DBCONNSYNC = "Database_SyncServerConnectionString";
        public const string GEN_COMPANY = "General_Company";
        public const string GEN_FULLNAME = "General_UserFullName";
        public const string GEN_INITIALS = "General_UserInitials";

        public const string GEN_AUTOSAVEINTERVAL = "Save_AutosaveInterval";
        public const string GEN_AUTOSAVEENABLED = "Save_EnableAutosave";
        public const string GEN_PROMPTFORDOCPROPS = "Save_PromptForDocumentPropertiesOnFirstSave";
        public const string VER_AUTOCHECKOUT = "Versioning_AutoCheckoutAllShapes";
        public const string VER_PURGEMINORVERSIONS = "Versioning_PurgeMinorVersions";
        public const string VER_PURGEINTERVAL = "Versioning_PurgeMinorVersionsInterval";
        public const string VIEW_CHECK_DUPLICATES = "View_CheckDuplicates";
        public const string VIEW_CHECK_DUPLICATES_ALERT = "View_CheckDuplicates_Alert_Mechanism";
        public const string VIEW_CHECK_DUPLICATES_ACROSSWORKSPACES = "View_CheckDuplicatesAcrossWorkspaces";
        public const string VIEW_VALIDATE_ON_OPEN = "View_ValidateOnOpen";
        public const string VIEW_GRIDCELLSIZE = "View_GridCellSize";
        public const string VIEW_SHOWGRID = "View_ShowGrid";
        public const string VIEW_SNAPTOGRID = "View_SnapToGrid";
        public const string VIEW_SNAPRESIZE = "View_SnapResize";
        public const string VIEW_SMOOTHINGMODE = "View_SmoothingMode";
        public const string VIEW_ARROWMOVESMALL = "View_ArrowMoveSmall";
        public const string VIEW_ARROWMOVELARGE = "View_ArrowMoveLarge";

        public const string VIEW_CANVASHEIGHT = "View_CanvasHeight";
        public const string VIEW_CANVASWIDTH = "View_CanvasWidth";

        public const string AUTOSAVEINTERVAL = "View_AutoSaveInterval";
        public const string AUTOSAVEENABLED = "View_AutoSaveEnabled";
        public const string SPELLCHECK = "View_SpellCheck";
        public const string CUSTOMSUGGESTION = "View_CustomSuggestion";
        public const string INTELLISENSE = "View_Intellisense";

        public const string IMAGENODE_CLASS = "ImageNode_Class";

        public const string SAVETODATABASEENABLED = "View_SaveToDatabaseEnabled";

        public const string PRINT_ARTEFACTLINES = "Print_ArtefactLines";
        public const string PRINT_COMMENTS = "Print_Comments";
        public const string PRINT_VCINDICATORS = "Print_VCIndicators";
        public const string CACHE_NUMBEROFOBJECTROWS = "Cache_NumberOfObjectRowsToCache";
        public const string VIEW_COMPAREANDREFRESH_TYPE = "View_CompareAndRefresh";
        public const string COMPARE_LINKS = "Compare_Links";
        public const string WARNDIAGRAMDIFFWORKSPACE = "View_WarnDiagramDiffWorkspace";
        public const string VIEW_COMPARE_MO_WORKSPACES = "View_CompareMOWorkspaces";
        //public const string CHECK_EXISTENCE_ON_OTHER_DIAGRAMS_ON_DELETE = "View_CheckExistenceOnOtherDiagramsOnDelete";

        public const string VERBOSE_LOGGING = "Verbose_Logging";
        public const string SAVE_ON_CREATE = "Save_On_Create";
        public const string DEFAULT_WORKSPACE = "Default_Workspace";
        public const string DEFAULT_WORKSPACE_ID = "Default_Workspace_ID";
        public const string USE_QUICKPANEL = "Use_QuickPanel";
        public const string DEFAULT_TO_PORT = "Default_To_Port";
        public const string DEFAULT_FROM_PORT = "Default_From_Port";
        public const string USE_SHALLOWCOPYCOLOR = "Use_Shallow_Copy_Color";

        public const string MERGEDUPLICATEBEHAVIOUR = "Merge_Duplicate_Behaviour";
    }
}