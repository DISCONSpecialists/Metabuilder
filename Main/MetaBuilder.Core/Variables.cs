/*
 * Nike SA - CDE Importer
 * Copyright © 2007 - DISCON Specialists
*/

#region Namespaces Used

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using NetSpell.SpellChecker;
using NetSpell.SpellChecker.Dictionary;
using System.Drawing;
using System.Collections.ObjectModel;

#endregion


//In .Net 2.0 Settings(.settings) should be used to provide a read/write way of storing custom config data. 
//The actual settings are scoped to the current user and are stored within "documents and settings", 
//though this is completely transparent to the application. This way the application never needs to 
//physically write to the directory where the application is actually running.

////Deploy your app.config file to the same directory as your application as normal. (THIS IS WHAT WE DO)
////You don't need elevation to read from the configuration file, only if you are writting (in which case use settings). 

#region play with UAC

//MANIFEST FILE (Metabuilder.exe.manifest)
//<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
//    <assembly xmlns="urn:schemas-microsoft-com:asm.v1" manifestVersion="1.0">
//    <trustInfo xmlns="urn:schemas-microsoft-com:asm.v3">
//        <security>
//            <requestedPrivileges>
//                <requestedExecutionLevel level="asInvoker" uiAccess="false"/>
//            </requestedPrivileges>
//        </security>
//    </trustInfo>
//</assembly> 

//These files have been around for a while, and the new section added for UAC control is the requestedExecutionLevel element. The level attribute may be one of the following values:
//Level	Description
//asInvoker	Does not require elevation, runs without requesting elevation using privileges of its parent process.
//highestAvailable	Requests the highest available privilege tokens of its parent process. An administrator account will attempt to elevate to full administrator level, but a standard account will only request elevation to its own highest set of access tokens.
//requireAdministrator	Requires elevation to full administrator privileges.

#endregion

namespace MetaBuilder.Core
{
    /// <summary>
    /// Globals handles global variables such as WeekEndsOn. 
    /// </summary>
    public class Variables
    {
        //these objects can only be represented as artifacts or for 2 cases on colapsiblenodes
        public Collection<System.String> ClassesWithoutStencil = new Collection<string>(new string[] 
                                                                {  //"TimeScheme","LocationScheme",
                                                                    "DataAttribute", "DataField", "GatewayDescription",
                                                                    "Attribute", "DataColumn", "FlowDescription", "ConditionalDescription",
                                                                    "ConnectionSpeed", "ConnectionType", "DependancyDescription", "DependencyDescription",
                                                                    "LocationAssociation", "Logic", "PropOfRealization","ProbOfRealization",
                                                                    "SelectorAttribute", "Condition","ProbabilityOfRealization",
                                                                    //"MeasureType",
                                                                    "MeasureValue",
                                                                    //"DataValue",
                                                                }
                                                              );


        public bool UserDebug
        {
            get
            {
#if DEBUG
                return true;
#endif
                MetaSettings s = new MetaSettings();
                return System.Environment.MachineName == s.GetSetting("USER_DEBUG", "");
            }
        }

        private Dictionary<string, string> classesAndDescriptions = new Dictionary<string, string>();
        public string ClassDescription(string className)
        {
            if (classesAndDescriptions.ContainsKey(className))
                return classesAndDescriptions[className].ToString();
            else
                return "";
        }

        #region Private Fields
        private static Variables globalInstance = null;
        private string connectionString;
        private const string _GraphTraceCategory = "Graphing Events";
        private const string _MetaObjectTraceCategory = "MFactory & MetaObject Events";
        private const string _UIEventsTraceCategory = "UI Events";
        //private Hashtable generalPermissions;
        private Hashtable workspacePermissions;
        private int userID;
        private int currentWorkspaceTypeId;
        private string currentWorkspaceName;
        private string sourceCodePath;
        private string exportsPath;
        private string symbolPath;
        private string stencilPath;
        private string diagramPath;
        private string imagesPath;
        private string companyName;
        private string userFullname;
        private decimal pageHeight;
        private decimal pageWidth;
        private bool showGrid;
        private bool snapToGrid;
        private bool isSet;
        private bool compareWorkspacesForObjects;
        private decimal gridCellSize;
        private string serverConnectionString;
        private Hashtable workspaceHashtable;
        #endregion

        public bool CompareWorkspacesForObjects
        {
            get { return true; }
            set { compareWorkspacesForObjects = true; }
        }

        private bool imageNodeClassLabelVisible;
        public bool ImageNodeClassLabelVisible
        {
            get { return imageNodeClassLabelVisible; }
            set { imageNodeClassLabelVisible = value; }
        }

        private bool intellisenseEnabled;
        public bool IntellisenseEnabled
        {
            get { return intellisenseEnabled; }
            set { intellisenseEnabled = value; }
        }

        private bool spellCheckEnabled;
        public bool SpellCheckEnabled
        {
            get { return spellCheckEnabled; }
            set { spellCheckEnabled = value; }
        }

        private bool customSuggestionEnabled;
        public bool CustomSuggestionEnabled
        {
            get { return customSuggestionEnabled; }
            set { customSuggestionEnabled = value; }
        }

        public static Spelling spelling = new Spelling();
        public static WordDictionary dictionary = new WordDictionary();

        private int numberOfObjectRowsToCache;
        public int NumberOfObjectRowsToCache
        {
            get { return numberOfObjectRowsToCache; }
            set { numberOfObjectRowsToCache = value; }
        }

        private int? serverUserID;
        public int? ServerUserID
        {
            get { return serverUserID; }
            set { serverUserID = value; }
        }

        private bool isServer;
        public bool IsServer
        {
            get { return isServer; }
            set { isServer = value; }
        }

        private bool isDeveloperEdition;
        public bool IsDeveloperEdition
        {
            get { return isDeveloperEdition; }//false; }//
            set { isDeveloperEdition = value; }
        }

        private bool showDeveloperItems;
        public bool ShowDeveloperItems
        {
            get { return showDeveloperItems; }
            set { showDeveloperItems = value; }
        }

        private bool isDesktopEdition;
        public bool IsDesktopEdition
        {
            get { return isDesktopEdition; }
            set { isDesktopEdition = value; }
        }

        private bool isDemo;
        public bool IsDemo
        {
            get { return isDemo; }
            set { isDemo = value; }
        }

        private int demoDaysLeft;
        public int DemoDaysLeft
        {
            get { return demoDaysLeft; }
            set { demoDaysLeft = value; }
        }

        #region Read/Write Accessors

        public bool ShowGrid
        {
            get { return showGrid; }
        }
        public bool SnapToGrid
        {
            get { return snapToGrid; }
        }

        public decimal GridCellSize
        {
            get { return gridCellSize; }
        }

        public decimal PageHeight
        {
            get { return pageHeight; }
            set { pageHeight = value; }
        }
        public decimal PageWidth
        {
            get { return pageWidth; }
            set { pageWidth = value; }
        }

        //public Hashtable GeneralPermissions
        //{
        //    get { return generalPermissions; }
        //}
        public string UserFullName
        {
            get { return userFullname; }
        }
        public string CompanyName
        {
            get { return companyName; }
        }
        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        public string CurrentWorkspaceName
        {
            get { return currentWorkspaceName; }
            set
            {
                Log.WriteLog("Current workspace changed to " + value);
                currentWorkspaceName = value;
            }
        }
        public int CurrentWorkspaceTypeId
        {
            get { return currentWorkspaceTypeId; }
            set { currentWorkspaceTypeId = value; }
        }

        //ws.Name + "#" + ws.WorkspaceTypeId,NULL
        public Hashtable WorkspaceHashtable
        {
            get { return workspaceHashtable; }
            set
            {
                workspaceHashtable = value;
            }
        }

        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        public string ExportsPath
        {
            get { return exportsPath.EndsWith("\\") ? exportsPath : exportsPath + "\\"; }
        }

        public string SymbolPath
        {
            get { return symbolPath.EndsWith("\\") ? symbolPath : symbolPath + "\\"; }
        }

        public string StencilPath
        {
            get { return stencilPath.EndsWith("\\") ? stencilPath : stencilPath + "\\"; }
        }

        public string DiagramPath
        {
            get { return diagramPath.EndsWith("\\") ? diagramPath : diagramPath + "\\"; }
        }

        public string ImagesPath
        {
            get { return imagesPath; }
        }

        public static string GraphTraceCategory
        {
            get { return _GraphTraceCategory; }
        }

        public static string MetaObjectTraceCategory
        {
            get { return _MetaObjectTraceCategory; }
        }

        public static string UIEventsTraceCategory
        {
            get { return _UIEventsTraceCategory; }
        }
        public bool IsSet
        {
            get { return isSet; }
        }
        /// <summary>
        /// Provides the caller with the namespace in which generated classes are located
        /// </summary>
        public static string MetaNameSpace
        {
            get
            {
                if (Core.FilterVariables.filterName != "settings.xml")
                {
                    string filterName = Core.FilterVariables.filterName.Replace(".", "").Replace("xml", "").Replace("settings", "").Replace("-", "");

                    return "MetaBuilder.Meta.Generated" + filterName;
                }
                return "MetaBuilder.Meta.Generated";
            }
        }

        /// <summary>
        /// The path where the source files are generated
        /// </summary>
        public string SourceCodePath
        {
            get
            {
                return sourceCodePath;
            }
            set
            {
                sourceCodePath = value;
            }
        }

        private string userDomainIdentifier;
        public string UserDomainIdentifier
        {
            get { return userDomainIdentifier; }
            set { userDomainIdentifier = value; }
        }

        /// <summary>
        /// The path the Meta.dll etc
        /// </summary>
        public string MetaAssemblyPath
        {
            get
            {
                // string dllname = Assembly.GetExecutingAssembly().Location;

                string dllPath = Assembly.GetExecutingAssembly().CodeBase.Substring(8);
                string executionPath = Path.GetDirectoryName(dllPath);
                return executionPath + "\\";
                //return dllPath.Substring(0, dllPath.LastIndexOf("\\")) + "\\";
                //return dllname.Substring(0, dllname.LastIndexOf("\\")) + "\\"; 
            }
        }

        /// <summary>
        /// The full path & filename of the generated assembly
        /// </summary>
        public string MetaAssembly
        {
            get
            {
                //if (Core.FilterVariables.filterName != "settings.xml")
                //{
                //    string filterName = Core.FilterVariables.filterName.Replace(".", "").Replace("xml", "").Replace("settings", "").Replace("-", "");

                //    return MetaAssemblyPath + MetaNameSpace + filterName + ".dll";
                //}

                return MetaAssemblyPath + MetaNameSpace + ".dll";
            }
        }

        /// <summary>
        /// The full path & filename of the generated sorters assembly
        /// </summary>
        public string MetaSortAssembly
        {
            get
            {
                //if (Core.FilterVariables.filterName != "settings.xml")
                //{
                //    string filterName = Core.FilterVariables.filterName.Replace(".", "").Replace("xml", "").Replace("settings", "").Replace("-", "");
                //    return MetaAssemblyPath + MetaNameSpace + ".Sorters" + filterName + ".dll";
                //}

                return MetaAssemblyPath + MetaNameSpace + ".Sorters.dll";
            }
        }

        /// <summary>
        /// The full path & filename of the generated enum assembly
        /// </summary>
        public string MetaEnumAssembly
        {
            get
            {
                //if (Core.FilterVariables.filterName != "settings.xml")
                //{
                //    string filterName = Core.FilterVariables.filterName.Replace(".", "").Replace("xml", "").Replace("settings", "").Replace("-", "");
                //    return MetaAssemblyPath + MetaNameSpace + ".Enums" + filterName + ".dll";
                //}

                return MetaAssemblyPath + MetaNameSpace + ".Enums.dll";
            }
        }
        public string ServerConnectionString
        {
            get { return serverConnectionString; }
            set { serverConnectionString = value; }
        }
        public enum RefreshType { Automatic, Prompt, Never }

        private RefreshType refreshOnOpenDiagram;
        public RefreshType RefreshOnOpenDiagram
        {
            get { return refreshOnOpenDiagram; }
            set { refreshOnOpenDiagram = value; }
        }

        #endregion

        #region Constructor
        public Variables(int UserID, int WorkspaceTypeId, string WorkspaceName, Hashtable workspaces, RefreshType rtype)
        {
            workspaceHashtable = workspaces;
            this.userID = UserID;
            this.currentWorkspaceTypeId = WorkspaceTypeId;
            this.currentWorkspaceName = WorkspaceName;
            this.RefreshOnOpenDiagram = rtype;

            SetDefaults();
            isSet = true;
        }

        //public Variables(int UserID, RefreshType rtype)
        //{
        //    this.userID = UserID;
        //    this.RefreshOnOpenDiagram = rtype;

        //    SetDefaults();
        //    isSet = true;
        //}

        #endregion

        public Variables()
        {
            SetDefaults();
            workspaceHashtable = new Hashtable();
        }

        private bool checkDuplicates;
        public bool CheckDuplicates
        {
            get { return checkDuplicates; }
            set { checkDuplicates = value; }
        }

        private bool checkDuplicatesInformMeImmediately;
        public bool CheckDuplicatesInformMeImmediately
        {
            get { return checkDuplicatesInformMeImmediately; }
        }

        private bool validateVersionControl;
        public bool ValidateVersionControl
        {
            get { return true; }
            set { validateVersionControl = true; }
        }

        private bool checkDuplicatesAcrossWorkspaces;
        public bool CheckDuplicatesAcrossWorkspaces
        {
            get { return checkDuplicatesAcrossWorkspaces; }
        }

        private bool printComments;
        public bool PrintComments
        {
            get { return printComments; }
            set { printComments = value; }
        }

        private bool autoSaveEnabled;
        public bool AutoSaveEnabled
        {
            get { return autoSaveEnabled; }
            set { autoSaveEnabled = value; }
        }

        private int autoSaveInterval;
        public int AutoSaveInterval
        {
            get { return autoSaveInterval; }
            set { autoSaveInterval = value; }
        }

        private bool printArtefactLines;
        public bool PrintArtefactLines
        {
            get { return printArtefactLines; }
            set { printArtefactLines = value; }
        }

        private bool saveToDatabaseEnabled;
        public bool SaveToDatabaseEnabled
        {
            get { return saveToDatabaseEnabled; }
            set { saveToDatabaseEnabled = value; }
        }

        private bool printVCIndicators;
        public bool PrintVCIndicators
        {
            get { return printVCIndicators; }
            set { printVCIndicators = value; }
        }

        private bool warnWhenDiagramIsFromADifferentWorkspace;
        public bool WarnWhenDiagramIsFromADifferentWorkspace
        {
            get { return warnWhenDiagramIsFromADifferentWorkspace; }
            set { warnWhenDiagramIsFromADifferentWorkspace = value; }
        }

        //private bool checkExistenceOnOtherDiagramsOnDelete;

        //public bool CheckExistenceOnOtherDiagramsOnDelete
        //{
        //    get { return checkExistenceOnOtherDiagramsOnDelete; }
        //    set { checkExistenceOnOtherDiagramsOnDelete = value; }
        //}

        private bool verboseLogging;
        public bool VerboseLogging
        {
            get { return verboseLogging; }
            set { verboseLogging = value; }
        }

        private string defaultWorkspace;
        public string DefaultWorkspace
        {
            get { return defaultWorkspace; }
            set { defaultWorkspace = value; }
        }

        private bool useQuickPanel;
        public bool UseQuickPanel
        {
            get { return useQuickPanel; }
            set { useQuickPanel = value; }
        }

        private bool useShallowCopyColor;
        public bool UseShallowCopyColor
        {
            get { return useShallowCopyColor; }
            set { useShallowCopyColor = value; }
        }

        private int defaultWorkspaceID;
        public int DefaultWorkspaceID
        {
            get { return defaultWorkspaceID; }
            set { defaultWorkspaceID = value; }
        }

        private Dictionary<string, object> shapeCache;
        public Dictionary<string, object> ShapeCache
        {
            get
            {
                if (shapeCache == null)
                    shapeCache = new Dictionary<string, object>();
                return shapeCache;
            }
            set { shapeCache = value; }
        }
        //Dataview
        //Dataview_Logical
        //Dataview_Physical
        public object ReturnShape(string className)
        {
            if (shapeCache != null)
                foreach (KeyValuePair<string, object> e in shapeCache)
                {
                    if (e.Key == className)
                        return e.Value;
                }

            Log.WriteLog(className + ": Returned null " + className + "  for ReturnShape from shape cache in variables");
            return null;
            //return ShapeCache == null ? null : ShapeCache.Where(o => o.Key == className).Select(o => o.Value).FirstOrDefault();
        }

        private Dictionary<string, string> renamedClasses;
        public Dictionary<string, string> RenamedClasses
        {
            get
            {
                if (renamedClasses == null)
                {
                    renamedClasses = Xmler.GetRemappedClasses();
                }

                return renamedClasses;
            }
        }

        private Collection<string> classFieldRemappedField;
        public Collection<string> ClassFieldRemappedField
        {
            get
            {
                if (classFieldRemappedField == null)
                {
                    classFieldRemappedField = Xmler.GetRemappedFields();
                }
                return classFieldRemappedField;
            }
        }

        private Collection<string> classToFieldRemappedFields;
        public Collection<string> ClassToFieldRemappedFields
        {
            get
            {
                if (classToFieldRemappedFields == null)
                {
                    classToFieldRemappedFields = Xmler.GetRemappedClassToFields();
                }
                return classToFieldRemappedFields;
            }
        }

        private void SetDefaults()
        {
            MetaSettings s = new MetaSettings();
            this.connectionString = s.GetSetting(MetaSettings.GEN_DBCONNLOCAL, "server=.\\SqlExpress;Initial Catalog=MetaBuilder;Integrated Security=true");
            this.serverConnectionString = s.GetSetting(MetaSettings.GEN_DBCONNSYNC, "server=.\\SqlExpress;Initial Catalog=MetaBuilder;Integrated Security=true");
            this.sourceCodePath = Application.StartupPath + "\\MetaData\\SourceFiles\\";
            this.exportsPath = s.GetSetting(MetaSettings.PATH_EXPORTS, Application.StartupPath + "\\MetaData\\Export\\");
            this.symbolPath = s.GetSetting(MetaSettings.PATH_SYMBOLS, Application.StartupPath + "\\MetaData\\Symbols\\");
            this.stencilPath = s.GetSetting(MetaSettings.PATH_SYMBOLSTORES, Application.StartupPath + "\\MetaData\\SymbolStores\\");//Application.StartupPath + "\\MetaData\\SymbolStores\\";
            this.diagramPath = s.GetSetting(MetaSettings.PATH_DIAGRAMS, Application.StartupPath + "\\MetaData\\Diagrams\\");
            this.imagesPath = Application.StartupPath + "\\MetaData\\Images\\";
            this.userFullname = s.GetSetting(MetaSettings.GEN_FULLNAME, System.Environment.UserName);
            this.companyName = s.GetSetting(MetaSettings.GEN_COMPANY, "DISCON Specialists");
            this.gridCellSize = s.GetSetting(MetaSettings.VIEW_GRIDCELLSIZE, decimal.Parse("20", System.Globalization.CultureInfo.InvariantCulture));
            this.showGrid = s.GetSetting(MetaSettings.VIEW_SHOWGRID, false);
            this.snapToGrid = s.GetSetting(MetaSettings.VIEW_SNAPTOGRID, true);
            this.printArtefactLines = s.GetSetting(MetaSettings.PRINT_ARTEFACTLINES, false);
            this.printComments = s.GetSetting(MetaSettings.PRINT_COMMENTS, false);
            this.printVCIndicators = s.GetSetting(MetaSettings.PRINT_VCINDICATORS, false);
            this.ValidateVersionControl = s.GetSetting(MetaSettings.VIEW_VALIDATE_ON_OPEN, false);
            this.AutoSaveEnabled = s.GetSetting(MetaSettings.AUTOSAVEENABLED, false);
            this.AutoSaveInterval = s.GetSetting(MetaSettings.AUTOSAVEINTERVAL, 5);
            this.SaveToDatabaseEnabled = s.GetSetting(MetaSettings.SAVETODATABASEENABLED, true);
            this.NumberOfObjectRowsToCache = s.GetSetting(MetaSettings.CACHE_NUMBEROFOBJECTROWS, 250);
            this.WarnWhenDiagramIsFromADifferentWorkspace = s.GetSetting(MetaSettings.WARNDIAGRAMDIFFWORKSPACE, true);
            //this.CheckExistenceOnOtherDiagramsOnDelete = s.GetSetting(MetaSettings.CHECK_EXISTENCE_ON_OTHER_DIAGRAMS_ON_DELETE, false);
            this.CompareWorkspacesForObjects = s.GetSetting(MetaSettings.VIEW_COMPARE_MO_WORKSPACES, false);
            this.SpellCheckEnabled = s.GetSetting(MetaSettings.SPELLCHECK, false);
            this.CustomSuggestionEnabled = s.GetSetting(MetaSettings.CUSTOMSUGGESTION, false);
            this.IntellisenseEnabled = s.GetSetting(MetaSettings.INTELLISENSE, false);
            this.ImageNodeClassLabelVisible = s.GetSetting(MetaSettings.IMAGENODE_CLASS, true);

            this.VerboseLogging = s.GetSetting(MetaSettings.VERBOSE_LOGGING, false);
            this.CompareLinks = s.GetSetting(MetaSettings.COMPARE_LINKS, false);
            this.SaveOnCreate = s.GetSetting(MetaSettings.SAVE_ON_CREATE, false);
            this.DefaultWorkspace = s.GetSetting(MetaSettings.DEFAULT_WORKSPACE, "");
            this.DefaultWorkspaceID = s.GetSetting(MetaSettings.DEFAULT_WORKSPACE_ID, 0);
            this.UseQuickPanel = s.GetSetting(MetaSettings.USE_QUICKPANEL, true);
            this.UseShallowCopyColor = s.GetSetting(MetaSettings.USE_SHALLOWCOPYCOLOR, true);
            this.DefaultFromPort = s.GetSetting(MetaSettings.DEFAULT_FROM_PORT, "Bottom");
            this.DefaultToPort = s.GetSetting(MetaSettings.DEFAULT_TO_PORT, "Left");

            this.MergeDuplicateBehaviour = s.GetSetting(MetaSettings.MERGEDUPLICATEBEHAVIOUR, "None");

            checkDuplicates = s.GetSetting(MetaSettings.VIEW_CHECK_DUPLICATES, false);
            if (checkDuplicates)
            {
                checkDuplicatesAcrossWorkspaces = s.GetSetting(MetaSettings.VIEW_CHECK_DUPLICATES_ACROSSWORKSPACES, true);
                checkDuplicatesInformMeImmediately = s.GetSetting(MetaSettings.VIEW_CHECK_DUPLICATES_ALERT, false);
            }

            pageWidth = s.GetSetting(MetaSettings.VIEW_CANVASWIDTH, 1626);
            pageHeight = s.GetSetting(MetaSettings.VIEW_CANVASHEIGHT, 1035);

            buildClassDescriptions();
        }

        private bool compareLinks;
        public bool CompareLinks
        {
            get { return compareLinks; }
            set { compareLinks = value; }
        }

        private void buildClassDescriptions()
        {
            //" + System.Environment.NewLine + "
            classesAndDescriptions.Add("Activity", "- Is a type of Function." + System.Environment.NewLine + "- An activity is a major unit of work to be completed for a process." + System.Environment.NewLine + "- Incorporates a set of tasks that occur over time and has a defined goal." + System.Environment.NewLine + "- Describes transformations from an initial state to a final state." + System.Environment.NewLine + "- Has inputs and outputs." + System.Environment.NewLine + "- Consumes resources and accounts for resource implications." + System.Environment.NewLine + "- Can be expressed in the form of a Value Chain." + System.Environment.NewLine + "- Refer to: Function, Capability, Process, Task");
            classesAndDescriptions.Add("ApplicationInterface", " - The representation of the flow of data (and information) between Architecture Objects as part of the operation of a system." + System.Environment.NewLine + "- The Architecture objects could be of the class application component, data component, organisation unit, service, function and other." + System.Environment.NewLine + "- The data flow can be represented on a conceptual, logical or physical level of detail." + System.Environment.NewLine + "- Could represent the transition from data through information to intelligence and knowledge.");
            classesAndDescriptions.Add("Attribute", "- Representation of data of a natural construct or system in its smallest atomic form, on a logical level of detail." + System.Environment.NewLine + "- Defines a property of an object, event, data class or data view." + System.Environment.NewLine + "- Further describes a data entity or data class." + System.Environment.NewLine + "- Examples are Person Name or Delivery Date." + System.Environment.NewLine + "- Synonyms: Property" + System.Environment.NewLine + "- Refer to: Data Entity, Data Class, Logical Level of Detail");
            classesAndDescriptions.Add("CategoryFactor", "- By studying the external and target environment according to the categories of influence, the factors within these environments that will impact the business could be derived." + System.Environment.NewLine + "- An example of an external driver is a change in regulation or compliance rules which, for example, require changes to the way an organization operates; i.e., Sarbanes-Oxley in the US." + System.Environment.NewLine + "- Sub types include: Political, Economical, Socio Economical, Technology Trends, Legislation, Environmental, Cultural, Industrial and other." + System.Environment.NewLine + "- Synonyms: PESTLE");
            classesAndDescriptions.Add("Competency", "The skills, knowledge, experience and other abilities required for a job or possessed by a person.");
            classesAndDescriptions.Add("JobCompetency", "The skills, knowledge, experience and other abilities required for a job or possessed by a person.");
            classesAndDescriptions.Add("ComputingComponent", "- Technically, a computer is a programmable machine. This means it can execute a programmed list of instructions and respond to new instructions that it is given. Today, however, the term is most often used to refer to the desktop and laptop computers that most people use. When referring to a desktop model, the term 'computer' technically only refers to the computer itself -- not the monitor, keyboard, and mouse. Still, it is acceptable to refer to everything together as the computer. If you want to be really technical, the box that holds the computer is called the 'system unit'" + System.Environment.NewLine + "- Some of the major parts of a personal computer (or PC) include the motherboard, CPU, memory (or RAM), hard drive, and video card. While personal computers are by far the most common type of computers today, there are several other types of computers. For example, a 'minicomputer' is a powerful computer that can support many users at once. A 'mainframe' is a large, high-powered computer that can perform billions of calculations from multiple sources at one time. Finally, a 'supercomputer' is a machine that can process billions of instructions a second and is used to calculate extremely complex calculations.");
            classesAndDescriptions.Add("Condition", "- Gateways are used to control how Sequence Flows interact as they converge and diverge within a Process." + System.Environment.NewLine + "- If the flow does not need to be controlled, then a Gateway is not needed." + System.Environment.NewLine + "- The term “Gateway” implies that there is a gating mechanism that either allows or disallows passage through the Gateway  as tokens arrive at a Gateway, they can be: merged together on input and/or split apart on output as the Gateway mechanisms are invoked. " + System.Environment.NewLine + "- Gateways, are capable of consuming or generating additional tokens, effectively controlling the execution semantics of a given Process." + System.Environment.NewLine + "- The main difference is that Gateways do not represent ‘work’ being done and they are considered to have zero effect on the operational measures of the Process being executed (cost, time, etc.). " + System.Environment.NewLine + "- Gateways can define all the types of Business Process Sequence Flow behaviour:  decisions/branching (exclusive, inclusive, complex) merging, forking, joining.");
            classesAndDescriptions.Add("Gateway", "- Gateways are used to control how Sequence Flows interact as they converge and diverge within a Process." + System.Environment.NewLine + "- If the flow does not need to be controlled, then a Gateway is not needed." + System.Environment.NewLine + "- The term “Gateway” implies that there is a gating mechanism that either allows or disallows passage through the Gateway  as tokens arrive at a Gateway, they can be: merged together on input and/or split apart on output as the Gateway mechanisms are invoked. " + System.Environment.NewLine + "- Gateways, are capable of consuming or generating additional tokens, effectively controlling the execution semantics of a given Process." + System.Environment.NewLine + "- The main difference is that Gateways do not represent ‘work’ being done and they are considered to have zero effect on the operational measures of the Process being executed (cost, time, etc.). " + System.Environment.NewLine + "- Gateways can define all the types of Business Process Sequence Flow behaviour:  decisions/branching (exclusive, inclusive, complex) merging, forking, joining.");
            classesAndDescriptions.Add("Conditional", "");//A specific technology infrastructure product or technology infrastructure product instance. For example, a particular product" + System.Environment.NewLine + "version of a Commercial Off-The-Shelf (COTS) solution, or a specific brand and version of server.
            classesAndDescriptions.Add("ConditionalDescription", "Further describes the flow between gateway and function, process, activity or task in terms of message, sequence and data value.");
            classesAndDescriptions.Add("ConnectionSize", "- A strategic theme is the logical grouping of strategic imperatives deemed to be of strategic importance to an organisation.");
            classesAndDescriptions.Add("ConnectionSpeed", "- Defines the strategic direction (intent) for a system." + System.Environment.NewLine + "- Specific time frame." + System.Environment.NewLine + "- Measurable outcome and clear yardstick." + System.Environment.NewLine + "- Should be a set of independent statements but is composite - can be decomposed into sub-goals." + System.Environment.NewLine + "- Should be aligned with the functions of the system." + System.Environment.NewLine + "- SMART (Simple, Measurable, Achievable, Realistic, Time-based)." + System.Environment.NewLine + "- Derived from influencing factors and implications on the system." + System.Environment.NewLine + "- Synonyms: Critical Success Factor (CSFs), Strategic Objective, Strategic Goal, Mission, Vision");
            classesAndDescriptions.Add("ConnectionType", "- The mechanisms necessary to direct or guide the execution of a system, to ensure performance, to enable the  exploiting of opportunities, and to ensure that risks are managed appropriately and resources are used responsibly." + System.Environment.NewLine + "- Governance mechanism types could be Policy, Plan, Contract, Standard Operating Procedure, Control, Standard, Act, regulation, Frameworks, Reference Model, etc." + System.Environment.NewLine + "- Internal Governance mechanism are derived from the strategy and architecture of a system.");
            classesAndDescriptions.Add("CSF", "- Defines the strategic direction (intent) for a system." + System.Environment.NewLine + "- Specific time frame." + System.Environment.NewLine + "- Measurable outcome and clear yardstick." + System.Environment.NewLine + "- Should be a set of independent statements but is composite - can be decomposed into sub-goals." + System.Environment.NewLine + "- Should be aligned with the functions of the system." + System.Environment.NewLine + "- SMART (Simple, Measurable, Achievable, Realistic, Time-based)." + System.Environment.NewLine + "- Derived from influencing factors and implications on the system." + System.Environment.NewLine + "- Synonyms: Critical Success Factor (CSFs), Strategic Objective, Strategic Goal, Mission, Vision");
            classesAndDescriptions.Add("StrategicStatement", "- Defines the strategic direction (intent) for a system." + System.Environment.NewLine + "- Specific time frame." + System.Environment.NewLine + "- Measurable outcome and clear yardstick." + System.Environment.NewLine + "- Should be a set of independent statements but is composite - can be decomposed into sub-goals." + System.Environment.NewLine + "- Should be aligned with the functions of the system." + System.Environment.NewLine + "- SMART (Simple, Measurable, Achievable, Realistic, Time-based)." + System.Environment.NewLine + "- Derived from influencing factors and implications on the system." + System.Environment.NewLine + "- Synonyms: Critical Success Factor (CSFs), Strategic Objective, Strategic Goal, Mission, Vision");
            classesAndDescriptions.Add("DataColumn", "- Physical data construct that allows for the specification of a group of data values." + System.Environment.NewLine + "- Fields are logical structures maintained by the database management system (DBMS) or are  part of physical information artefacts. " + System.Environment.NewLine + "- Refer to: Data Column, Data Table");
            classesAndDescriptions.Add("DataSchema", "- Clustering by means of abstraction or generalisation of data to represent data on a conceptual level of detail. This can be done for logical or physical data." + System.Environment.NewLine + "- The data clustering can be done in a formal manner using the cohesion or affinity of data as a basis or through informal analysis." + System.Environment.NewLine + "- Data subject areas can be reverse engineered or masterminded from a zero base." + System.Environment.NewLine + "- Synonyms: Data Concept, Data Theme, Data Cluster, Data Package" + System.Environment.NewLine + "- Refer to: Abstraction, Generalisation, Data Schema");
            classesAndDescriptions.Add("DataSubjectArea", "- Clustering by means of abstraction or generalisation of data to represent data on a conceptual level of detail. This can be done for logical or physical data." + System.Environment.NewLine + "- The data clustering can be done in a formal manner using the cohesion or affinity of data as a basis or through informal analysis." + System.Environment.NewLine + "- Data subject areas can be reverse engineered or masterminded from a zero base." + System.Environment.NewLine + "- Synonyms: Data Concept, Data Theme, Data Cluster, Data Package" + System.Environment.NewLine + "- Refer to: Abstraction, Generalisation, Data Schema");
            classesAndDescriptions.Add("DataTable", "- Physical data construct that allows for the specification of a group of data columns (or fields)." + System.Environment.NewLine + "- Tables are logical structures maintained by the database management system (DBMS). " + System.Environment.NewLine + "- Tables are made up of columns and rows." + System.Environment.NewLine + "- Refer to: Data Field");
            classesAndDescriptions.Add("DataView", "");
            classesAndDescriptions.Add("DependencyDescription", "Further describes the functional dependency (association) between data entities in terms of dependency type, dependency value, etc. In relational database theory, a functional dependency is a constraint between two sets of attributes in a relation from a database.");
            classesAndDescriptions.Add("Employee", "- A human being regarded as an individual that is relevant to the system." + System.Environment.NewLine + "- Refer to: Employee.");
            classesAndDescriptions.Add("Person", "- A human being regarded as an individual that is relevant to the system." + System.Environment.NewLine + "- Refer to: Employee.");
            classesAndDescriptions.Add("Entity", "An encapsulation of data that is recognized by a business domain expert as a thing. Logical data entities can be tied to applications, repositories, and services and may be structured according to implementation considerations.");
            classesAndDescriptions.Add("DataEntity", "An encapsulation of data that is recognized by a business domain expert as a thing. Logical data entities can be tied to applications, repositories, and services and may be structured according to implementation considerations.");
            classesAndDescriptions.Add("Environment", "The surroundings or conditions in which a system operates.");
            classesAndDescriptions.Add("EnvironmentCategory", "");
            classesAndDescriptions.Add("Exception", "");
            classesAndDescriptions.Add("FlowDescription", "Further describes the flow between objects in terms of the object that flows, sequence and time.");
            classesAndDescriptions.Add("Function", "- Describes what needs to be done. " + System.Environment.NewLine + "- Is the action or intended purpose of a person or object in a specific role." + System.Environment.NewLine + "- Expresses a goal / result that has to be achieved. Is described by a verb and a qualified object. " + System.Environment.NewLine + "- Can be composed into a sub-function that is a decomposition or classification of a superior function. " + System.Environment.NewLine + "- Acts as an index mechanism for operational definitions in the form of for example an operating model, business process or use case." + System.Environment.NewLine + "- Synonyms: Goal, Objective, Value Chain Step" + System.Environment.NewLine + "- Refer to:  Process, Activity, Task");
            classesAndDescriptions.Add("GovernanceMechanism", "- The mechanisms necessary to direct or guide the execution of a system, to ensure performance, to enable the  exploiting of opportunities, and to ensure that risks are managed appropriately and resources are used responsibly." + System.Environment.NewLine + "- Governance mechanism types could be Policy, Plan, Contract, Standard Operating Procedure, Control, Standard, Act, regulation, Frameworks, Reference Model, etc." + System.Environment.NewLine + "- Internal Governance mechanisms are derived from the strategy and architecture of a system.");
            classesAndDescriptions.Add("Implication", "- An implication is the effect that An influencing factor has on a system given a specific scenario. Implications are specific to An organisation." + System.Environment.NewLine + "- Sub types include: Strength, Weakness, Opportunity, Threat" + System.Environment.NewLine + "- Refer to: influencing factor");
            classesAndDescriptions.Add("Iteration", "");
            classesAndDescriptions.Add("ITInfrastructureEnvironment", "- Packaging of hardware and software at a location to form a software or data hosting environment." + System.Environment.NewLine + "- Managed and referred to as a unit." + System.Environment.NewLine + "- Can be of the type Dev, QA, Test, Prod, DR");
            classesAndDescriptions.Add("Job", "- A Job is a natural grouping of roles (or processes, activities, tasks) for association with a Job Position. A number of Job Positions (Posts) can exist for a Job." + System.Environment.NewLine + "- A Job is identified by a Job Title and further described by means of a  Job Description or Job Profile." + System.Environment.NewLine + "- Refer to: Job Position, Role");
            classesAndDescriptions.Add("JobPosition", "- The instance of a Job within a chain of command of an organization structure." + System.Environment.NewLine + "- Smallest unit reflected on the organisation structure to which employees can be associated." + System.Environment.NewLine + "- Synonyms: Post" + System.Environment.NewLine + "- Refer to: Job");
            classesAndDescriptions.Add("Position", "- The instance of a Job within a chain of command of an organization structure." + System.Environment.NewLine + "- Smallest unit reflected on the organisation structure to which employees can be associated." + System.Environment.NewLine + "- Synonyms: Post" + System.Environment.NewLine + "- Refer to: Job");
            classesAndDescriptions.Add("Location", "- Reference to a virtual or physical location as per location schema." + System.Environment.NewLine + "- Example: South African Geographical Structure (Gauteng, Pretoria, Irene, Justice Mohamed Street, ...)" + System.Environment.NewLine + "- Within computer storage, the code used to designate the location of a specific piece of data." + System.Environment.NewLine + "- Refer to: Location Unit, Location Schema");
            classesAndDescriptions.Add("LocationAssociation", "Further describes the association between location units in terms of distance, frequency of travel and method of travel.");
            classesAndDescriptions.Add("LocationScheme", "- A Locality Schema is the identification of a location structure." + System.Environment.NewLine + "- Example: SA Geographical Structure" + System.Environment.NewLine + "- Refer to: Location Unit");
            classesAndDescriptions.Add("LocationUnit", "- Reference to the units that form part of the Structure of a Location Schema." + System.Environment.NewLine + "- example: Province, Region, Area, etc." + System.Environment.NewLine + "- Refer to: location Schema");
            classesAndDescriptions.Add("Logic", "Further describes the functional dependency (association) between data entities in terms of dependency type, dependency value, etc. In relational database theory, a functional dependency is a constraint between two sets of attributes in a relation from a database.");
            classesAndDescriptions.Add("LogicalInformationArtefact", "- A Logical Information Artefact is an aggregation of Data Attributes and / or Data Classes (or Data Entities) that are used as an enabler, input or output for a system. " + System.Environment.NewLine + "- A Logical Information Artefact can have more than one physical representation, depending on the context that it is used in. For this purpose the Information Artefact has a relationship with an Information Carrier. " + System.Environment.NewLine + "- A Physical Information Artefact could be in the form of a report, a message, form and other format that are electronic or paper based. Can include structure data such as XML messages or unstructured data such as word processing files, html files (web pages), project plans, presentation files, spreadsheets, graphics, audio files, video files, emails and more." + System.Environment.NewLine + "- Basic occurrences of information as a product." + System.Environment.NewLine + "- Synonyms: Data Product, Data View, Dataset, Information Carrier" + System.Environment.NewLine + "Refer to: Data Attribute, Data Class, Data Entity");
            classesAndDescriptions.Add("LogicalITInfrastructureComponent", "An encapsulation of information technology infrastructure that is independent of a particular product. A class of technology product. For example, Network Router.");
            classesAndDescriptions.Add("Measure", "- Metric or unit of quantity for measuring or stating the quality (effectiveness and efficiency) of a system. " + System.Environment.NewLine + "- An indicator or factor that can be tracked, usually on an ongoing basis, to determine success or alignment with objectives and goals." + System.Environment.NewLine + "- Provides insight for architecture change required. " + System.Environment.NewLine + "- Can be classified as Areas, Indicators and Values. " + System.Environment.NewLine + "- Requires logic to define how to calculate the measurement value. " + System.Environment.NewLine + "- Requires a mechanism for producing measurement values.");
            //" + System.Environment.NewLine + "
            classesAndDescriptions.Add("MeasurementItem", "");
            classesAndDescriptions.Add("MutuallyExclusiveIndicator", "Depicts the type of generalization association between data entities. A generalization is exclusive if every instance of the parent entity is at most an instance of one of the children (XOR), otherwise it is inclusive (AND).");
            classesAndDescriptions.Add("SubsetIndicator", "Depicts the type of generalization association between data entities. A generalization is exclusive if every instance of the parent entity is at most an instance of one of the children (XOR), otherwise it is inclusive (AND).");
            classesAndDescriptions.Add("Network", "A Network refers to a physical computer network or data network is a telecommunications network that allows computers to exchange data. In computer networks, networked computing devices pass data to each other along data connections. E.g a specific LAN or WAN or a named VLAN.");
            classesAndDescriptions.Add("NetworkComponent", "When you have two or more computers connected to each other, you have a network. The purpose of a network is to enable the sharing of files and information between multiple systems. The Internet could be described as a global network of networks. Computer networks can be connected through cables, such as Ethernet cables or phone lines, or wirelessly, using wireless networking cards that send and receive data through the air.");
            classesAndDescriptions.Add("Object", "- Packaging of related elements that naturally belongs together (tightly coupled) and which is referred to and managed as a unit. " + System.Environment.NewLine + "- A general term that is used to mean one part of something more complex. " + System.Environment.NewLine + "- An Object can be a  business object, application object or a tangible object such as a device, person, facility, etc.");
            classesAndDescriptions.Add("OrganizationalUnit", "- A social unit of people, systematically structured and managed to meet a need or to pursue collective goals on a continuing basis." + System.Environment.NewLine + "- An Organisation Unit represent the hierarchy of units in the organisation's structure." + System.Environment.NewLine + "- A self-contained unit of resources with goals, objectives, and measures. " + System.Environment.NewLine + "- Organization units may include external parties and business partner organizations." + System.Environment.NewLine + "- Sub types include: divisions, business areas, operating units, departments, sections and sub-sections." + System.Environment.NewLine + "- Refer to: Organisation");
            classesAndDescriptions.Add("Peripheral", "A computer peripheral is any external device that communicates with the computer. For example, a printer or handheld device.");
            classesAndDescriptions.Add("PeripheralComponent", "A computer peripheral is any external device that communicates with the computer. For example, a printer or handheld device.");
            classesAndDescriptions.Add("PhysicalDataComponent", "- A boundary zone that encapsulates related data entities to for m a physical location to be held. For example, a purchase order business object, comprising purchase order header and item business object nodes. " + System.Environment.NewLine + "- A data component is a data (and information) repository of a set of integrated data objects. " + System.Environment.NewLine + "- The objects and their relationships are modelled by means of a data structure." + System.Environment.NewLine + "- Subtypes include: Database, Operational Data Store (ODS), Data Warehouse, Data Mart, Data File, Information Artefact");
            classesAndDescriptions.Add("PhysicalInformationArtefact", "- A particular implementation of a Logical Information Artefact." + System.Environment.NewLine + "- Reference is made to the physical mechanism, format and other physical properties of the Information Artefact." + System.Environment.NewLine + "- Could be In the form of a file, report, message, form and other format that are electronic or paper based. Can include structure data such as XML messages or unstructured data such as word processing files, html files (web pages), project plans, presentation files, spreadsheets, graphics, audio files, video files, emails and more." + System.Environment.NewLine + "- Examples: Printed Copy of a Document, Electronic XML Message" + System.Environment.NewLine + "- Synonyms: Information Carrier, Information Artefact, Data Product");
            classesAndDescriptions.Add("PhysicalSoftwareComponent", "- An encapsulation of software functionality that is aligned to implementation structuring." + System.Environment.NewLine + "- Is a separately identifiable piece of software that is responsible for a clearly defined set of functions. " + System.Environment.NewLine + "- In programming design, an information system is divided into application components that in turn are made up of modules." + System.Environment.NewLine + "- In object-oriented programming and distributed object technology, a component is a reusable program building block that can be combined with other components in the same or other computers in a distributed network to form an application." + System.Environment.NewLine + "- A component form units when compiling, linking, or operating the system (for example, executables)." + System.Environment.NewLine + "- Components can be contained in other components or call up other components." + System.Environment.NewLine + "- Relates to UML Component Diagrams. Where a component is relevant to an application, the component can be linked to an Application and a UML Component diagram can be assigned to the Component." + System.Environment.NewLine + "- Refer to: Application, Information System, Application Package");
            classesAndDescriptions.Add("ProbOfRealization", "The extent to which the scenario is likely to realise. Measured as a percentage.");
            classesAndDescriptions.Add("Process", "- Is a type of a function." + System.Environment.NewLine + "- Identifies the requirement for an integrated definition of operation that includes at least tasks, data, time, locality and roles." + System.Environment.NewLine + "- A process enables one or more functions and is decomposed into tasks." + System.Environment.NewLine + "- Describes transformations from one state to another." + System.Environment.NewLine + "- Consumes resources such as applications, data, time, people and accounts for resource implications and produces outputs." + System.Environment.NewLine + "- Can be expressed in the form of a value chain." + System.Environment.NewLine + "- Processes have clear business reasons for existing, accountable owners, clear roles and responsibilities around the execution of the process, and the means to measure performance." + System.Environment.NewLine + "- Synonyms: Business Process, Procedure");
            classesAndDescriptions.Add("Rationale", "");
            classesAndDescriptions.Add("Responsibility", "");
            classesAndDescriptions.Add("Role", "- The usual or expected function of an actor, or the part somebody or something plays in a particular action or event. An Actor may have a number of roles." + System.Environment.NewLine + "- The part an individual plays in an organization and the contribution they make through the application of their skills, knowledge, experience, and abilities.");
            classesAndDescriptions.Add("Scenario", "- Collective name for a state that has implied factors and which a system is exposed to." + System.Environment.NewLine + "- Is time-based (historical, current or future state)." + System.Environment.NewLine + "- Has an implied probability of realisation." + System.Environment.NewLine + "- Typically used to analyse what the future will entail in order to define strategy and to plan for the future.");
            classesAndDescriptions.Add("SelectorAttribute", "The data attribute that is used to select the instances of the parent data entity.");
            classesAndDescriptions.Add("Skill", "");
            classesAndDescriptions.Add("Software", "");
            classesAndDescriptions.Add("StorageComponent", "An encapsulation of software functionality that is independent of a particular implementation. For example, the classification of all purchase request processing applications implemented in an enterprise.");
            classesAndDescriptions.Add("StrategicTheme", "- A strategic theme is the logical grouping of strategic imperatives deemed to be of strategic importance to an organisation.");
            classesAndDescriptions.Add("SystemComponent", "- Technically, a computer is a programmable machine. This means it can execute a programmed list of instructions and respond to new instructions that it is given. Today, however, the term is most often used to refer to the desktop and laptop computers that most people use. When referring to a desktop model, the term 'computer' technically only refers to the computer itself -- not the monitor, keyboard, and mouse. Still, it is acceptable to refer to everything together as the computer. If you want to be really technical, the box that holds the computer is called the 'system unit'" + System.Environment.NewLine + "- Some of the major parts of a personal computer (or PC) include the motherboard, CPU, memory (or RAM), hard drive, and video card. While personal computers are by far the most common type of computers today, there are several other types of computers. For example, a 'minicomputer' is a powerful computer that can support many users at once. A 'mainframe' is a large, high-powered computer that can perform billions of calculations from multiple sources at one time. Finally, a 'supercomputer' is a machine that can process billions of instructions a second and is used to calculate extremely complex calculations.");
            classesAndDescriptions.Add("TimeDifference", "");
            classesAndDescriptions.Add("TimeIndicator", "");
            classesAndDescriptions.Add("TimeReference", "- Reference to time as per time schema." + System.Environment.NewLine + "- Example: Financial Calendar (Any Year, Any Quarter, Month 1, Day 1)" + System.Environment.NewLine + "- Refer to: Time, Time Schema, Time Unit");
            classesAndDescriptions.Add("TimeScheme", "- A Time Schema is the identification of a time structure." + System.Environment.NewLine + "- Example: Batch Processing Calendar, Financial Calendar, Julian Calendar" + System.Environment.NewLine + "- Synonyms: Time Calendar" + System.Environment.NewLine + "- Refer to: Time Unit, Time");
            classesAndDescriptions.Add("TimeUnit", "- Time schema is decomposed into time units." + System.Environment.NewLine + "- Example: Year, month, week and day." + System.Environment.NewLine + "- Refer to: Time Schema, Time");
        }

        private string defaultFromPort;
        public string DefaultFromPort { get { return defaultFromPort; } set { defaultFromPort = value; } }

        private string defaultToPort;
        public string DefaultToPort { get { return defaultToPort; } set { defaultToPort = value; } }

        // This is the method we will call when we want to
        // get a hold of the global variables in our application.
        public static Variables Instance
        {
            get
            {
                if (globalInstance == null)
                {
                    globalInstance = new Variables();
                }
                return globalInstance;
            }
            set
            {
                globalInstance = value;
            }
        }

        private bool validatedRegistration;
        public bool ValidatedRegistration
        {
            get { return validatedRegistration; }
            set { validatedRegistration = value; }
        }

        private const string clientProvider = "Client";
        public string ClientProvider
        {
            get { return clientProvider; }
        }
        private const string serverProvider = "Server";
        public string ServerProvider
        {
            get { return serverProvider; }
        }

        private bool isViewer = false;
        public bool IsViewer
        {
            get { return isViewer; }
            set { isViewer = value; }
        }

        private Dictionary<string, Image> imageExtensions;
        public Dictionary<string, Image> ImageExtensions
        {
            get
            {
                if (imageExtensions == null)
                    imageExtensions = new Dictionary<string, Image>();
                return imageExtensions;
            }
            set { imageExtensions = value; }
        }

        public Image GetIcon(string path)
        {
            if (ImageExtensions.ContainsKey(strings.GetFileExtension(path)))
                return ImageExtensions[strings.GetFileExtension(path)];
            Image i = (Image)Storage.FileIconManager.GetFileIcon(path, Storage.FileIconManager.IconSize.Small, false).ToBitmap();
            ImageExtensions.Add(strings.GetFileExtension(path), i);
            return i;
        }

        //Default disables the merge feature
        //Default,Concatenate,FirstValue,Manual
        private string mergeDuplicateBehaviour = "None";
        public string MergeDuplicateBehaviour
        {
            get { return mergeDuplicateBehaviour; }
            set { mergeDuplicateBehaviour = value; }
        }
        //class/class:value, file
        private Dictionary<string, string> imageCache;
        public Dictionary<string, string> ImageCache
        {
            get
            {
                if (imageCache == null)
                    imageCache = new Dictionary<string, string>();
                return imageCache;
            }
            set { imageCache = value; }
        }

        private bool checkForUpdates;
        public bool CheckForUpdates
        {
            get { return checkForUpdates; }
            set { checkForUpdates = value; }
        }

        private bool saveOnCreate;
        public bool SaveOnCreate
        {
            get { return saveOnCreate; }
            set { saveOnCreate = value; }
        }
    }
}