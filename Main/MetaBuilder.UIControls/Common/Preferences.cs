using System;
using System.Windows.Forms;
using System.Configuration;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.Core.Storage;
using MetaBuilder.UIControls.GraphingUI;
using b = MetaBuilder.BusinessLogic;
using MSDASC;
using Microsoft.Data.ConnectionUI;
using MetaBuilder.MetaControls;
using MetaBuilder.Meta;
using System.Threading;
using System.Collections.Generic;

namespace MetaBuilder.UIControls.Common
{
    public partial class Preferences : Form
    {

        //treeNode8});
        //    if (Core.Variables.Instance.ShowDeveloperItems)
        //        treeNode10.Nodes.Add(treeNode9);

        #region Constructors (1)

        public Preferences()
        {
            InitializeComponent();
            s = new MetaSettings();
        }

        #endregion Constructors

        #region Methods (16)

        // Private Methods (16) 

        private void cbAutoSave_CheckedChanged(object sender, EventArgs e)
        {
            numAutoSaveInterval.Enabled = cbAutoSave.Checked;
        }

        private void cbCheckDuplicatesWhileDiagramming_CheckedChanged_1(object sender, EventArgs e)
        {
            radioAcrossWorkspaces.Enabled = cbCheckDuplicatesWhileDiagramming.Checked;
            radioCurrentWorkspace.Enabled = cbCheckDuplicatesWhileDiagramming.Checked;
        }

        private void GetAndSetNewPath(string description, string pathName, TextBox txtTarget)
        {
            folderBrowserDialog1.ShowNewFolderButton = true;
            folderBrowserDialog1.SelectedPath = txtTarget.Text;
            folderBrowserDialog1.Description = "Specify the " + description + " path";
            DialogResult res = folderBrowserDialog1.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                txtTarget.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void Preferences_Load(object sender, EventArgs e)
        {
            if (!Core.Variables.Instance.ShowDeveloperItems)
            {
                tvSettings.Nodes[2].Nodes.RemoveByKey("tnStencilImages");

#if !DEBUG
                //if (Core.FilterVariables.filters==null)
                tvSettings.Nodes[2].Nodes.RemoveByKey("tnFilterBrowser");

                checkBoxGeneralSaveOnCreate.Visible = false;
#endif
            }

            //so that we cant set this while a diagram is open to 'mix' two types of saving
            checkBoxGeneralSaveOnCreate.Enabled = DockingForm.DockForm.GetCurrentGraphViewContainer() == null;

            tvSettings.SelectedNode = tvSettings.Nodes["tnGeneral"];
            tabPreferences.SelectedTab = tabPreferences.TabPages["tpgGeneral"];
            // Load settings
            fspec = FilePathManager.Instance.GetSpecification(FileTypeList.SymbolStore);
            tab_View_comboDefaultFromPort.DataSource = Enum.GetValues(typeof(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation));
            tab_View_comboDefaultToPort.DataSource = Enum.GetValues(typeof(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation));
            LoadAndBindData();

            Thread t = new Thread(new ThreadStart(CheckViews));
            t.Start();
        }

        //a way to manually recreate your views if one is missing
        private void CheckViews()
        {
            try
            {
                SetVisibility(false);

                bool missing = false;
                foreach (Class c in DataAccessLayer.DataRepository.Classes(null))
                {
                    if (c.IsActive == false)
                        continue;

                    try
                    {
                        //#if DEBUG
                        //                    c.Name += "!";
                        //#endif

                        DataAccessLayer.DataRepository.Provider.ExecuteNonQuery(System.Data.CommandType.Text, "SELECT * FROM MetaView_" + c.Name + "_Listing");
                    }
                    catch
                    {
                        missing = true;
                        break;
                    }
                }
                if (missing)
                {
                    SetVisibility(true);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString());
            }
        }

        delegate void SetVisibilityCallback(bool visible);
        private void SetVisibility(bool visible)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (groupBoxViews.InvokeRequired)
            {
                SetVisibilityCallback d = new SetVisibilityCallback(SetVisibility);
                this.Invoke(d, new object[] { visible });
            }
            else
            {
                groupBoxViews.Visible = visible;
            }
        }

        private void radioAcrossWorkspaces_CheckedChanged(object sender, EventArgs e)
        {
            radioCurrentWorkspace.Checked = !radioAcrossWorkspaces.Checked;
        }

        private void radioCurrentWorkspace_CheckedChanged(object sender, EventArgs e)
        {
            radioAcrossWorkspaces.Checked = !radioCurrentWorkspace.Checked;
        }

        private void radioRefreshAutomatic_CheckedChanged(object sender, EventArgs e)
        {
            if (radioRefreshAutomatic.Checked)
                UpdateRefreshTypeRadioButtons(Variables.RefreshType.Automatic);
        }

        private void radioRefreshNever_CheckedChanged(object sender, EventArgs e)
        {
            if (radioRefreshNever.Checked)
                UpdateRefreshTypeRadioButtons(Variables.RefreshType.Never);
        }

        private void radioRefreshPrompt_CheckedChanged(object sender, EventArgs e)
        {
            if (radioRefreshPrompt.Checked)
                UpdateRefreshTypeRadioButtons(Variables.RefreshType.Prompt);
        }

        private void tab_View_cbSnapToGrid_CheckedChanged(object sender, EventArgs e)
        {
            numArrowMoveSmall.Enabled = !tab_View_cbSnapToGrid.Checked;
        }

        #endregion Methods

        #region Private Fields
        private FileDialogSpecification fspec;
        private MetaSettings s;
        #endregion

        #region EventHandling
        private void btnOK_Click(object sender, EventArgs e)
        {
            ApplyImageChange();
            ApplyChanges();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ApplyImageChange()
        {
            if (ChangedStencilImages.Count > 0)
                Core.Variables.Instance.ImageCache = null;

            foreach (KeyValuePair<object, int> v in ChangedStencilImages)
            {
                string q = "UPDATE [X] SET Uri_ID = " + v.Value.ToString();
                if (v.Key is Class)
                {
                    q = q.Replace("[X]", "Class");
                    q += " WHERE Name = '" + (v.Key as Class).Name + "'";
                }
                else if (v.Key is DomainDefinitionPossibleValue)
                {
                    q = q.Replace("[X]", "DomainDefinitionPossibleValue");
                    q += " WHERE Description = '" + (v.Key as DomainDefinitionPossibleValue).Description + "' AND DomainDefinitionID = " + (v.Key as DomainDefinitionPossibleValue).DomainDefinitionID.ToString();
                }
                else
                {
                    q = "";
                    Log.WriteLog("Preferences::ApplyImageChange::" + v.Key.GetType().ToString() + "::" + v.Key.ToString() + " is an unknown uri Handler");
                }
                if (q != "")
                    DataAccessLayer.DataRepository.Provider.ExecuteNonQuery(System.Data.CommandType.Text, q);
            }
        }

        private void ApplyChanges()
        {
            s.PutSetting(MetaSettings.PATH_DIAGRAMS, tab_General_Paths_DiagramsText.Text);
            s.PutSetting(MetaSettings.PATH_EXPORTS, tab_General_Paths_ExportsText.Text);
            s.PutSetting(MetaSettings.PATH_SYMBOLSTORES, tab_General_Paths_SymbolsText.Text);
            s.PutSetting(MetaSettings.PATH_SOURCE, tab_General_Paths_SourceText.Text);

            //s.PutSetting(MetaSettings.CHECK_EXISTENCE_ON_OTHER_DIAGRAMS_ON_DELETE, cbCheckExistenceElsewhere.Checked);

            s.PutSetting(MetaSettings.GEN_DBCONNLOCAL, tab_Database_txtLocalInstanceConnectionString.Text);
            s.PutSetting(MetaSettings.GEN_DBCONNSYNC, tab_Database_txtSyncServerConnectionString.Text);
            s.PutSetting(MetaSettings.GEN_COMPANY, tab_General_txtCompany.Text);
            s.PutSetting(MetaSettings.GEN_FULLNAME, tab_General_txtFullname.Text);
            s.PutSetting(MetaSettings.GEN_INITIALS, tab_General_txtInitials.Text);
            //s.PutSetting(MetaSettings.GEN_AUTOSAVEINTERVAL, tab_Save_numericAutoSaveInterval.Value);
            //s.PutSetting(MetaSettings.GEN_AUTOSAVEENABLED, tab_Save_cbAutoSaveDocuments.Checked);
            s.PutSetting(MetaSettings.GEN_PROMPTFORDOCPROPS, tab_Save_cbPromptForDocumentPropertiesOnFirstSave.Checked);
            s.PutSetting(MetaSettings.VER_AUTOCHECKOUT, tab_Save_cbAutoCheckoutAllObjects.Checked);
            s.PutSetting(MetaSettings.VER_PURGEMINORVERSIONS, tab_Save_cbPromptToPurgeMinorVersions.Checked);
            s.PutSetting(MetaSettings.VER_PURGEINTERVAL, tab_Save_numericAutoSaveInterval.Value);
            s.PutSetting(MetaSettings.VIEW_GRIDCELLSIZE, tab_View_numericGridCellSize.Value);

            s.PutSetting(MetaSettings.VIEW_CANVASWIDTH, tab_View_numericDiagramWidth.Value);
            s.PutSetting(MetaSettings.VIEW_CANVASHEIGHT, tab_View_numericDiagramHeight.Value);

            s.PutSetting(MetaSettings.VIEW_SHOWGRID, tab_View_cbShowGrid.Checked);
            s.PutSetting(MetaSettings.VIEW_SNAPTOGRID, tab_View_cbSnapToGrid.Checked);
            s.PutSetting(MetaSettings.VIEW_SMOOTHINGMODE, tab_View_comboGridSmoothing.Text.ToString());
            s.PutSetting(MetaSettings.VIEW_CHECK_DUPLICATES, cbCheckDuplicatesWhileDiagramming.Checked);
            s.PutSetting(MetaSettings.VIEW_CHECK_DUPLICATES_ACROSSWORKSPACES, radioAcrossWorkspaces.Checked);

            s.PutSetting(MetaSettings.VIEW_ARROWMOVELARGE, float.Parse(numArrowMoveLarge.Value.ToString(), System.Globalization.CultureInfo.InvariantCulture));
            s.PutSetting(MetaSettings.VIEW_ARROWMOVESMALL, float.Parse(numArrowMoveSmall.Value.ToString(), System.Globalization.CultureInfo.InvariantCulture));
            s.PutSetting(MetaSettings.VIEW_VALIDATE_ON_OPEN, true);
            s.PutSetting(MetaSettings.VIEW_CHECK_DUPLICATES_ACROSSWORKSPACES, radioAcrossWorkspaces.Checked);
            s.PutSetting(MetaSettings.PRINT_ARTEFACTLINES, cbPrintArtefactPointers.Checked);
            s.PutSetting(MetaSettings.PRINT_COMMENTS, cbPrintComments.Checked);
            s.PutSetting(MetaSettings.PRINT_VCINDICATORS, cbPrintSourceControlIndicators.Checked);
            s.PutSetting(MetaSettings.AUTOSAVEENABLED, cbAutoSave.Checked);
            s.PutSetting(MetaSettings.AUTOSAVEINTERVAL, int.Parse(numAutoSaveInterval.Value.ToString()));
            s.PutSetting(MetaSettings.VIEW_SNAPRESIZE, cbSnapResize.Checked);
            s.PutSetting(MetaSettings.SPELLCHECK, cbSpellChecker.Checked);
            s.PutSetting(MetaSettings.CUSTOMSUGGESTION, cbCustomSuggestion.Checked);
            s.PutSetting(MetaSettings.INTELLISENSE, cbIntellisense.Checked);
            s.PutSetting(MetaSettings.IMAGENODE_CLASS, cbDisplayImageNodeClassName.Checked);
            s.PutSetting(MetaSettings.VIEW_COMPARE_MO_WORKSPACES, true);

            s.PutSetting(MetaSettings.USE_QUICKPANEL, cbUseQuickPanel.Checked);
            s.PutSetting(MetaSettings.USE_SHALLOWCOPYCOLOR, cbUseShallowCopyColor.Checked);
            s.PutSetting(MetaSettings.VERBOSE_LOGGING, checkBoxGeneralVerboseLogging.Checked);
            s.PutSetting(MetaSettings.SAVE_ON_CREATE, checkBoxGeneralSaveOnCreate.Checked);
            s.PutSetting(MetaSettings.DEFAULT_FROM_PORT, tab_View_comboDefaultFromPort.SelectedItem.ToString());
            s.PutSetting(MetaSettings.DEFAULT_TO_PORT, tab_View_comboDefaultToPort.SelectedItem.ToString());

            if (radioRefreshAutomatic.Checked)
                s.PutSetting(MetaSettings.VIEW_COMPAREANDREFRESH_TYPE, Variables.RefreshType.Automatic.ToString());

            if (radioRefreshPrompt.Checked)
                s.PutSetting(MetaSettings.VIEW_COMPAREANDREFRESH_TYPE, Variables.RefreshType.Prompt.ToString());

            if (radioRefreshNever.Checked)
                s.PutSetting(MetaSettings.VIEW_COMPAREANDREFRESH_TYPE, Variables.RefreshType.Never.ToString());

            s.PutSetting(MetaSettings.COMPARE_LINKS, cbCompareLinks.Checked);

            Variables.Instance.ServerConnectionString = tab_Database_txtSyncServerConnectionString.Text;

            #region TODO
            Variables.Instance.ConnectionString = tab_Database_txtLocalInstanceConnectionString.Text;
            // Open App.Config of executable
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            // Add an connection string
            config.ConnectionStrings.ConnectionStrings.Remove("netTiersConnectionString");
            ConnectionStringSettings conStringSettings = new ConnectionStringSettings("netTiersConnectionString", tab_Database_txtLocalInstanceConnectionString.Text);
            config.ConnectionStrings.ConnectionStrings.Add(conStringSettings);
            // Save modified configuration file.
            config.Save(ConfigurationSaveMode.Modified);
            // reload of a changed section.
            ConfigurationManager.RefreshSection("connectionStrings");
            ConfigurationManager.RefreshSection("netTiersConnectionString");
            #endregion

            s.PutSetting(MetaSettings.MERGEDUPLICATEBEHAVIOUR, comboBoxMergeStrategy.SelectedItem.ToString());
            Variables.Instance.MergeDuplicateBehaviour = comboBoxMergeStrategy.SelectedItem.ToString();

            DockingForm.DockForm.ApplyNewSettings();
            CoreInjector coreInjector = new CoreInjector();
            coreInjector.InjectCoreVariables(false);
            coreInjector.InjectConnections();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            // Ignore any changes
            Close();
        }

        bool stencilImageListBinding = false;
        private void tvSettings_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string tag = e.Node.Tag as string;
            if ((tag != null) && (tag.Length > 0))
            {
                if (tabPreferences.TabPages[tag] != null)
                    tabPreferences.SelectTab(tag);

                if (tag.Contains("Image"))
                {
                    checkedListBoxStencilImage.Items.Clear();
                    foreach (UURI u in DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.UURIProvider.GetAll())
                    {
                        uuriDisplay d = new uuriDisplay();
                        d.PKID = u.pkid;
                        d.FileName = strings.GetFileNameOnly(u.FileName);
                        checkedListBoxStencilImage.Items.Add(d, CheckState.Unchecked);
                    }
                }
                else if (tag.Contains("Filter"))
                {
                    ResetFilterTab();
                }
            }
        }

        sealed class uuriDisplay
        {

            private int pkid;

            public int PKID
            {
                get { return pkid; }
                set { pkid = value; }
            }

            private string fileName;

            public string FileName
            {
                get { return fileName; }
                set { fileName = value; }
            }

            public override string ToString()
            {
                return FileName;
            }

        }

        #endregion

        #region Binding
        public void LoadAndBindData()
        {
            tab_General_Paths_DiagramsText.Text = s.GetSetting(MetaSettings.PATH_DIAGRAMS, Application.StartupPath + "\\MetaData\\Diagrams");
            tab_General_Paths_ExportsText.Text = s.GetSetting(MetaSettings.PATH_EXPORTS, Application.StartupPath + "\\MetaData\\Export");
            tab_General_Paths_SymbolsText.Text = s.GetSetting(MetaSettings.PATH_SYMBOLSTORES, Application.StartupPath + "\\MetaData\\SymbolStores");
            tab_General_Paths_SourceText.Text = s.GetSetting(MetaSettings.PATH_SOURCE, Application.StartupPath + "\\MetaData\\SourceFiles");

            //cbCheckExistenceElsewhere.Checked = s.GetSetting(MetaSettings.CHECK_EXISTENCE_ON_OTHER_DIAGRAMS_ON_DELETE, false);

            tab_General_txtFullname.Text = s.GetSetting(MetaSettings.GEN_FULLNAME, "My Fullname");
            tab_General_txtInitials.Text = s.GetSetting(MetaSettings.GEN_INITIALS, "");
            tab_General_txtCompany.Text = s.GetSetting(MetaSettings.GEN_COMPANY, "DISCON Specialists");
            tab_Save_cbPromptForDocumentPropertiesOnFirstSave.Checked = s.GetSetting(MetaSettings.GEN_PROMPTFORDOCPROPS, false);
            tab_Save_cbAutoSaveDocuments.Checked = s.GetSetting(MetaSettings.GEN_AUTOSAVEENABLED, true);
            tab_Save_cbPromptToPurgeMinorVersions.Checked = s.GetSetting(MetaSettings.VER_PURGEMINORVERSIONS, false);
            tab_Save_cbAutoCheckoutAllObjects.Checked = s.GetSetting(MetaSettings.VER_AUTOCHECKOUT, true);
            tab_Save_numericAutoSaveInterval.Value = s.GetSetting(MetaSettings.GEN_AUTOSAVEINTERVAL, 10);
            tab_View_comboGridSmoothing.Text = s.GetSetting(MetaSettings.VIEW_SMOOTHINGMODE, "Default");
            tab_View_cbShowGrid.Checked = s.GetSetting(MetaSettings.VIEW_SHOWGRID, false);
            tab_View_cbSnapToGrid.Checked = s.GetSetting(MetaSettings.VIEW_SNAPTOGRID, true);
            tab_View_numericGridCellSize.Value = s.GetSetting(MetaSettings.VIEW_GRIDCELLSIZE, 20);

            tab_View_numericDiagramWidth.Value = s.GetSetting(MetaSettings.VIEW_CANVASWIDTH, 542);
            tab_View_numericDiagramHeight.Value = s.GetSetting(MetaSettings.VIEW_CANVASHEIGHT, 345);

            tab_Database_txtLocalInstanceConnectionString.Text = s.GetSetting(MetaSettings.GEN_DBCONNLOCAL, "server=.\\SqlExpress;Initial Catalog=MetaBuilder;Integrated Security=true;");
            tab_Database_txtSyncServerConnectionString.Text = s.GetSetting(MetaSettings.GEN_DBCONNSYNC, "server=.\\SqlExpress;Initial Catalog=MetaBuilder;Integrated Security=true;");
            tab_Database_txtSyncServerConnectionString.Enabled = Variables.Instance.IsServer;

            cbCheckDuplicatesWhileDiagramming.Checked = s.GetSetting(MetaSettings.VIEW_CHECK_DUPLICATES, true);
            radioAcrossWorkspaces.Checked = s.GetSetting(MetaSettings.VIEW_CHECK_DUPLICATES_ACROSSWORKSPACES, true);
            radioCurrentWorkspace.Checked = !radioAcrossWorkspaces.Checked;
            numArrowMoveLarge.Value = decimal.Parse(s.GetSetting(MetaSettings.VIEW_ARROWMOVELARGE, 50f).ToString(), System.Globalization.CultureInfo.InvariantCulture);
            numArrowMoveSmall.Value = decimal.Parse(s.GetSetting(MetaSettings.VIEW_ARROWMOVESMALL, 5f).ToString(), System.Globalization.CultureInfo.InvariantCulture);
            radioAcrossWorkspaces.Checked = s.GetSetting(MetaSettings.VIEW_CHECK_DUPLICATES_ACROSSWORKSPACES, true);
            cbPrintSourceControlIndicators.Checked = s.GetSetting(MetaSettings.PRINT_VCINDICATORS, true);
            cbPrintComments.Checked = s.GetSetting(MetaSettings.PRINT_COMMENTS, false);
            cbPrintArtefactPointers.Checked = s.GetSetting(MetaSettings.PRINT_ARTEFACTLINES, false);
            numArrowMoveSmall.Enabled = !tab_View_cbSnapToGrid.Checked;
            numAutoSaveInterval.Value = s.GetSetting(MetaSettings.AUTOSAVEINTERVAL, 5);
            cbAutoSave.Checked = s.GetSetting(MetaSettings.AUTOSAVEENABLED, true);
            cbSpellChecker.Checked = s.GetSetting(MetaSettings.SPELLCHECK, true);
            cbCustomSuggestion.Checked = s.GetSetting(MetaSettings.CUSTOMSUGGESTION, true);
            cbIntellisense.Checked = s.GetSetting(MetaSettings.INTELLISENSE, true);
            cbDisplayImageNodeClassName.Checked = s.GetSetting(MetaSettings.IMAGENODE_CLASS, true);
            numAutoSaveInterval.Enabled = cbAutoSave.Checked;
            cbSnapResize.Checked = s.GetSetting(MetaSettings.VIEW_SNAPRESIZE, false);
            //cbWarnWhenOpeningDiffWSDiagram.Checked = s.GetSetting(MetaSettings.WARNDIAGRAMDIFFWORKSPACE, true);

            cbUseQuickPanel.Checked = s.GetSetting(MetaSettings.USE_QUICKPANEL, true);
            cbUseShallowCopyColor.Checked = s.GetSetting(MetaSettings.USE_SHALLOWCOPYCOLOR, false);
            checkBoxGeneralVerboseLogging.Checked = s.GetSetting(MetaSettings.VERBOSE_LOGGING, false);
            checkBoxGeneralSaveOnCreate.Checked = s.GetSetting(MetaSettings.SAVE_ON_CREATE, false);

            tab_View_comboDefaultFromPort.SelectedItem = Enum.Parse(typeof(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation), s.GetSetting(MetaSettings.DEFAULT_FROM_PORT, "Bottom"));
            tab_View_comboDefaultToPort.SelectedItem = Enum.Parse(typeof(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation), s.GetSetting(MetaSettings.DEFAULT_TO_PORT, "Left"));

            //string refreshType = s.GetSetting(MetaSettings.VIEW_COMPAREANDREFRESH_TYPE, "Prompt");

            cbCompareLinks.Checked = s.GetSetting(MetaSettings.COMPARE_LINKS, false);
            Variables.RefreshType rtype = (Variables.RefreshType)Enum.Parse(typeof(Variables.RefreshType), s.GetSetting(MetaSettings.VIEW_COMPAREANDREFRESH_TYPE, Variables.RefreshType.Prompt.ToString()), true);
            UpdateRefreshTypeRadioButtons(rtype);

            comboBoxMergeStrategy.SelectedItem = s.GetSetting(MetaSettings.MERGEDUPLICATEBEHAVIOUR, "None");

            BindImageUI(); //can probably be threaded
        }
        private void BindImageUI()
        {
            try
            {
                checkedListBoxStencilImage.Enabled = buttonStencilImagesBrowse.Enabled = false;
                TreeNode classNode = new TreeNode("Class");
                classNode.ToolTipText = "By default, the image bound to a class will display.";
                foreach (Class c in DataAccessLayer.DataRepository.Classes(Core.Variables.Instance.ClientProvider))//.Provider.ClassProvider.GetAll()
                {
                    if (c.IsActive != true)
                        continue;

                    TreeNode cNode = new TreeNode(c.Name);
                    cNode.Tag = c;
                    classNode.Nodes.Add(cNode);
                }
                TreeNode domainNode = new TreeNode("Domain");
                domainNode.ToolTipText = "When selected, a domain value's image will override that of the class' image - if there is one present.";
                TList<DomainDefinition> domains = DataAccessLayer.DataRepository.Domains(Core.Variables.Instance.ClientProvider);
                domains.Sort("Name");
                foreach (DomainDefinition dd in domains)
                {
                    if (dd.IsActive != true)
                        continue;
                    TreeNode dNode = new TreeNode(dd.Name);
                    dNode.Tag = dd;
                    TList<DomainDefinitionPossibleValue> values = DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.DomainDefinitionPossibleValueProvider.GetByDomainDefinitionID(dd.pkid);
                    values.Sort("Series");
                    foreach (DomainDefinitionPossibleValue ddv in values)
                    {
                        if (ddv.IsActive != true)
                            continue;
                        TreeNode dValueNode = new TreeNode(ddv.Description);
                        dValueNode.Tag = ddv;
                        dNode.Nodes.Add(dValueNode);
                    }
                    domainNode.Nodes.Add(dNode);
                }
                treeViewImages.ShowNodeToolTips = true;
                treeViewImages.Nodes.Add(classNode);
                treeViewImages.Nodes.Add(domainNode);
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString());
            }
        }
        private void UpdateRefreshTypeRadioButtons(Variables.RefreshType refreshType)
        {
            switch (refreshType)
            {
                case Variables.RefreshType.Prompt:
                    radioRefreshPrompt.Checked = true;
                    radioRefreshNever.Checked = false;
                    radioRefreshAutomatic.Checked = false;
                    break;
                case Variables.RefreshType.Automatic:
                    radioRefreshPrompt.Checked = false;
                    radioRefreshNever.Checked = false;
                    radioRefreshAutomatic.Checked = true;
                    break;
                case Variables.RefreshType.Never:
                    radioRefreshAutomatic.Checked = false;
                    radioRefreshNever.Checked = true;
                    radioRefreshPrompt.Checked = false;
                    break;

            }
        }

        #endregion

        //ADDED Connection String Modifier
        private static string PromptForConnectionString(string currentConnectionString)
        {
            SQLConnectionStringBuilder build = new SQLConnectionStringBuilder();
            build.SQLConnectionString.ConnectionString = currentConnectionString;
            build.Bind();
            if (build.ShowDialog() == DialogResult.OK)
            {
                return build.SQLConnectionString.ConnectionString;
            }
            return currentConnectionString;

            //try
            //{
            //    DataSource sqlDataSource = new DataSource("MicrosoftSqlServer", "Microsoft SQL Server");
            //    sqlDataSource.Providers.Add(DataProvider.SqlDataProvider);
            //    DataConnectionDialog dcd = new DataConnectionDialog();
            //    dcd.DataSources.Add(sqlDataSource);
            //    dcd.SelectedDataProvider = DataProvider.SqlDataProvider;
            //    dcd.SelectedDataSource = sqlDataSource;
            //    //dcd.ConnectionString = currentConnectionString;

            //    if (dcd.ShowDialog(this) == DialogResult.OK)
            //    {
            //        return dcd.ConnectionString;
            //    }
            //    return currentConnectionString;
            //}
            //catch (Exception ex)
            //{
            //    Log.WriteLog(ex.ToString());
            //    return currentConnectionString;
            //}
            //return currentConnectionString;

            //try
            //{
            //    DataLinks dataLinks = new DataLinksClass();
            //    ADODB.Connection dialogConnection;
            //    string generatedConnectionString = string.Empty;
            //    if (currentConnectionString == String.Empty)
            //    {
            //        dialogConnection = (ADODB.Connection)dataLinks.PromptNew();
            //        generatedConnectionString = dialogConnection.ConnectionString;
            //    }
            //    else
            //    {
            //        dialogConnection = new ADODB.Connection();
            //        dialogConnection.Provider = "SQLOLEDB.1";
            //        ADODB.Property persistProperty = dialogConnection.Properties["Persist Security Info"];
            //        persistProperty.Value = true;

            //        dialogConnection.ConnectionString = currentConnectionString;
            //        dataLinks = new DataLinks();

            //        object objConn = dialogConnection;
            //        if (dataLinks.PromptEdit(ref objConn))
            //        {
            //            generatedConnectionString = dialogConnection.ConnectionString;
            //        }
            //    }
            //    generatedConnectionString = generatedConnectionString.Replace("Provider=SQLOLEDB.1;", string.Empty);
            //    generatedConnectionString = generatedConnectionString.Replace("Provider=SQLOLEDB;", string.Empty);
            //    bool pppp = !generatedConnectionString.Contains("Integrated Security=SSPI")
            //                && !generatedConnectionString.Contains("Trusted_Connection=True")
            //                && !generatedConnectionString.Contains("Password=")
            //                && !generatedConnectionString.Contains("Pwd=");
            //    if (pppp)
            //    {
            //        if (dialogConnection.Properties["Password"] != null)
            //            generatedConnectionString += ";Password=" + dialogConnection.Properties["Password"].Value;
            //    }

            //    if (!generatedConnectionString.Contains("Initial Catalog") && !generatedConnectionString.Contains("Data Source"))
            //        return currentConnectionString;
            //}
            //catch (Exception ex)
            //{
            //    Log.WriteLog(ex.ToString());
            //    return currentConnectionString;
            //}
            return currentConnectionString;
        }

        private void tab_Database_btnLocalInstanceConnectionString_Click(object sender, EventArgs e)
        {
            tab_Database_txtLocalInstanceConnectionString.Text = PromptForConnectionString(tab_Database_txtLocalInstanceConnectionString.Text);
        }
        private void tab_Database_lblSyncServerConnectionString_Click(object sender, EventArgs e)
        {
            tab_Database_txtSyncServerConnectionString.Text = PromptForConnectionString(tab_Database_txtSyncServerConnectionString.Text);
        }

        private void textBoxPathClicked(object sender, EventArgs e)
        {
            TextBox t = sender as TextBox;
            GetAndSetNewPath(t.Tag.ToString(), t.Text, t);
        }

        #region Stencil Images

        private void treeViewImages_AfterSelect(object sender, TreeViewEventArgs e)
        {
            LoadImage(null);
            int currentUriID = -1;
            checkedListBoxStencilImage.Enabled = buttonStencilImagesBrowse.Enabled = true;
            if (e.Node.Tag is Class)
            {
                int.TryParse(DataAccessLayer.DataRepository.Provider.ExecuteScalar(System.Data.CommandType.Text, "Select Uri_ID FROM Class where Name = '" + (e.Node.Tag as Class).Name + "'").ToString(), out currentUriID);
            }
            else if (e.Node.Tag is DomainDefinitionPossibleValue)
            {
                int.TryParse(DataAccessLayer.DataRepository.Provider.ExecuteScalar(System.Data.CommandType.Text, "Select Uri_ID FROM DomainDefinitionPossibleValue where DomainDefinitionID = " + (e.Node.Tag as DomainDefinitionPossibleValue).DomainDefinitionID + " AND Description = '" + (e.Node.Tag as DomainDefinitionPossibleValue).Description + "'").ToString(), out currentUriID);
            }
            else
            {
                checkedListBoxStencilImage.Enabled = buttonStencilImagesBrowse.Enabled = false;
            }

            if (e.Node.Tag != null)
            {
                if (ChangedStencilImages.ContainsKey(e.Node.Tag))
                {
                    currentUriID = ChangedStencilImages[e.Node.Tag];
                }
            }
            selectingValue = true;
            for (int i = 0; i < checkedListBoxStencilImage.Items.Count; i++)
            {
                //checkedListBoxStencilImage.SetSelected(i, true);
                uuriDisplay display = checkedListBoxStencilImage.Items[i] as uuriDisplay;
                if (display.PKID == currentUriID)
                {
                    checkedListBoxStencilImage.SetItemChecked(i, true);
                    checkedListBoxStencilImage.SetSelected(i, true);
                    LoadImage(DataAccessLayer.DataRepository.GetUUri(display.PKID));// .Connections[Core.Variables.Instance.ClientProvider].Provider.UURIProvider.GetBypkid(display.PKID));
                }
                else
                {
                    checkedListBoxStencilImage.SetItemChecked(i, false);
                    checkedListBoxStencilImage.SetSelected(i, false);
                }
            }
            selectingValue = false;
        }

        private void LoadImage(UURI uri)
        {
            if (stencilImageListBinding)
                return;
            if (uri == null)
            {
                pictureBoxStencilImagesImage.ImageLocation = null;
            }
            else
            {
                pictureBoxStencilImagesImage.ImageLocation = uri.FileName;
                pictureBoxStencilImagesImage.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void buttonStencilImagesBrowse_Click(object sender, EventArgs e)
        {
            if (treeViewImages.SelectedNode.Tag == null)
                return;
            //if image selected
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select an image";
            dlg.Multiselect = false;
            dlg.Filter = "Image/Icon files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.ico) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.ico";
            if (dlg.ShowDialog(this) != DialogResult.OK)
                return;
            if (dlg.FileName.Length == 0)
                return;

            //save new
            UURI u = null;
            foreach (UURI dbu in DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.UURIProvider.GetAll())
            {
                if (dbu.FileName == dlg.FileName)
                {
                    u = dbu;
                    break;
                }
            }
            if (u == null)
            {
                u = new UURI();
                u.FileName = dlg.FileName;
                DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.UURIProvider.Save(u);
            }

            LoadImage(u);

            uuriDisplay d = new uuriDisplay();
            d.PKID = u.pkid;
            d.FileName = strings.GetFileNameOnly(u.FileName);
            checkedListBoxStencilImage.Items.Add(d, CheckState.Checked);
            checkedListBoxStencilImage.SetSelected(checkedListBoxStencilImage.Items.Count - 1, true);
        }

        private void checkedListBoxStencilImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBoxStencilImage.SelectedItem == null)
                return;
            uuriDisplay d = checkedListBoxStencilImage.SelectedItem as uuriDisplay;
            LoadImage(DataAccessLayer.DataRepository.GetUUri(d.PKID));// .Connections[Core.Variables.Instance.ClientProvider].Provider.UURIProvider.GetBypkid(d.PKID));
        }
        private void checkedListBoxStencilImage_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (treeViewImages.SelectedNode.Tag == null)
            {
                //switch back
                //cant click here anyway
                e.NewValue = e.CurrentValue;
                return;
            }
            else if (selectingValue) //default selected uri
            {
                return;
            }
            for (int i = 0; i < checkedListBoxStencilImage.Items.Count; i++)
            {
                selectingValue = true;
                if (checkedListBoxStencilImage.GetItemChecked(i) == true)
                    checkedListBoxStencilImage.SetItemChecked(i, false);
                selectingValue = false;
            }
            uuriDisplay d = null;
            if (e.NewValue == CheckState.Checked)
            {
                d = checkedListBoxStencilImage.Items[e.Index] as uuriDisplay;
                LoadImage(DataAccessLayer.DataRepository.GetUUri(d.PKID));// .Connections[Core.Variables.Instance.ClientProvider].Provider.UURIProvider.GetBypkid(d.PKID));
            }
            else
            {
                LoadImage(null);
            }
            UpdateImage(treeViewImages.SelectedNode.Tag, d);
        }

        Dictionary<object, int> ChangedStencilImages = new Dictionary<object, int>();
        private void UpdateImage(object item, uuriDisplay uri)
        {
            int id = uri == null ? -1 : uri.PKID;
            if (ChangedStencilImages.ContainsKey(item))
            {
                ChangedStencilImages[item] = id;
            }
            else
            {
                ChangedStencilImages.Add(item, id);
            }
        }

        bool selectingValue = false;

        private void pictureBoxStencilImagesImage_Click(object sender, EventArgs e)
        {
            if (pictureBoxStencilImagesImage.ImageLocation != null && pictureBoxStencilImagesImage.Image != null)
            {
                if (System.IO.File.Exists(pictureBoxStencilImagesImage.ImageLocation))
                {
                    System.Diagnostics.Process.Start("explorer.exe", "/select, " + pictureBoxStencilImagesImage.ImageLocation);
                    return;
                }
                else
                {
                    MessageBox.Show(this, "The image's location cannot be opened because it cannot be found.", "Image error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show(this, "There is no image bound to the selected object", "Image error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        #region Filters

        private void ResetFilterTab()
        {
            listBoxFilters.Items.Clear();
            checkBoxCopyFilter.Checked = false;
            buttonDelete.Enabled = false;
            if (Core.FilterVariables.filters != null)
            {
                foreach (string s in Core.FilterVariables.filters)
                {
                    if (s.ToString() != "settings.xml")
                        listBoxFilters.Items.Add(new FilterItem(s));
                }
            }
        }
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            FilterItem item = null;
            if (listBoxFilters.SelectedItem != null)
                item = listBoxFilters.SelectedItem as FilterItem;

            if (item != null)
            {
                if (item.Filename == Core.FilterVariables.filterName)
                {
                    MessageBox.Show(this, "You cannot delete the active filter.", "Unable to Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                System.IO.File.Delete(Application.StartupPath + "\\" + item.Filename);
                Core.FilterVariables.filters.Remove(item.Filename);

                ResetFilterTab();
            }
        }
        private void listBoxFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonDelete.Enabled = listBoxFilters.SelectedItem != null;
        }
        private void checkBoxCopyFilter_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxFilters.Enabled = checkBoxCopyFilter.Checked;
            //bind to filters
            comboBoxFilters.Items.Clear();
            foreach (string s in Core.FilterVariables.filters)
            {
                //if (s.ToString() != "DISCON")
                comboBoxFilters.Items.Add(new FilterItem(s));
            }
            if (checkBoxCopyFilter.Checked)
                comboBoxFilters.SelectedIndex = 0;
        }
        private void buttonNew_Click(object sender, EventArgs e)
        {
            //string name = textBoxNewFilter.Text;
            if (textBoxNewFilter.Text.Length == 0)
                return;

            FilterItem item = new FilterItem(Core.FilterVariables.filterName);
            if (checkBoxCopyFilter.Checked)
            {
                item = comboBoxFilters.SelectedItem as FilterItem;
            }

            System.IO.File.Copy(Application.StartupPath + "\\" + item.Filename, Application.StartupPath + "\\settings-" + textBoxNewFilter.Text + ".xml", false);
            if (Core.FilterVariables.filters != null)
                Core.FilterVariables.filters.Add("settings-" + textBoxNewFilter.Text + ".xml");

            textBoxNewFilter.Text = "";

            ResetFilterTab();
        }
        private void comboBoxFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            //dont need this
        }

        #endregion

        private void buttonRebuildViews_Click(object sender, EventArgs e)
        {
            MetaBuilder.Meta.Builder.ViewBuilder b = new MetaBuilder.Meta.Builder.ViewBuilder();
            foreach (Class c in DataAccessLayer.DataRepository.Classes(null)) //Provider.GetAll()
                if (c.IsActive == true)
                    b.BuildView(c.Name);
        }

        private void buttonBindImages_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "This will rebind all images in the database to be stored in the default images folder. It will not copy or move any images. Do you want to proceed?", "Database Image Rebind", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            Log.WriteLog("Default URI (Preferences)");
            try
            {
                foreach (BusinessLogic.UURI uri in DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.UURIProvider.GetAll())
                {
                    uri.FileName = Application.StartupPath + "\\Metadata\\Images\\" + uri.FileName.Substring(uri.FileName.LastIndexOf("\\"));
                    uri.EntityState = MetaBuilder.BusinessLogic.EntityState.Changed;
                    DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.UURIProvider.Save(uri);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString());
            }
        }

    }
}