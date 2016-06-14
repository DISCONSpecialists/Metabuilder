#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: ClassEditor.cs
//

#endregion

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Janus.Windows.EditControls;
using Janus.Windows.GridEX;
using MetaBuilder.Core;
using MetaBuilder.Meta.Builder;
using MetaBuilder.Sync;

namespace MetaBuilder.WinUI.Dictionary
{
    /// <summary>
    /// Summary description for ClassEditor.
    /// </summary>

    public class ClassEditor : Form
    {
        private GridEX gridMeta;
        private SqlDataAdapter sqlClassAdapter;
        private SqlDataAdapter sqlFieldAdapter;
        private UIButton btnSave;
        private SqlDataAdapter sqlAssTypeAdapter;
        private SqlDataAdapter sqlAssAdapter;
        private SqlCommand sqlSelectCommand3;
        private SqlCommand sqlInsertCommand3;
        private SqlCommand sqlUpdateCommand3;
        private SqlCommand sqlDeleteCommand3;
        private SqlCommand sqlSelectCommand5;
        private SqlCommand sqlInsertCommand5;
        private SqlCommand sqlUpdateCommand5;
        private SqlCommand sqlDeleteCommand5;
        private SqlDataAdapter sqlAllowedArtifact;
        private Panel pnlBottom;
        private Panel pnlClassEditor;
        private TabControl tabCtl;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private SqlConnection sqlConnClasses;
        private SqlDataAdapter sqlDomainDefinitionAdapter;
        private SqlCommand sqlSelectCommand6;
        private SqlCommand sqlInsertCommand6;
        private SqlCommand sqlUpdateCommand6;
        private SqlCommand sqlDeleteCommand6;
        private SqlConnection sqlConnDomainDefs;
        private SqlDataAdapter sqlDomainDefinitionValues;
        private dsMetaData dsMetaData1;
        private GridEX gridDomains;
        private SqlCommand sqlSelectCommand4;
        private SqlCommand sqlInsertCommand4;
        private SqlCommand sqlUpdateCommand4;
        private SqlCommand sqlDeleteCommand4;
        private SqlCommand sqlSelectCommand1;
        private SqlCommand sqlInsertCommand1;
        private SqlCommand sqlUpdateCommand1;
        private SqlCommand sqlDeleteCommand1;
        private SqlDataAdapter sqlClassType;
        private SqlCommand sqlSelectCommand8;
        private SqlCommand sqlInsertCommand8;
        private SqlCommand sqlUpdateCommand8;
        private SqlCommand sqlDeleteCommand8;
        private StatusBar statusBar1;
        private ProgressBar progressBar1;
        private SqlCommand sqlSelectCommand7;
        private SqlCommand sqlInsertCommand7;
        private SqlCommand sqlUpdateCommand7;
        private SqlCommand sqlDeleteCommand7;
        private SqlCommand sqlSelectCommand2;
        private SqlCommand sqlInsertCommand2;
        private SqlCommand sqlUpdateCommand2;
        private SqlCommand sqlDeleteCommand2;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components = null;
        private TabPage tabPage3;
        private Label label1;

        private string connString = "";// "Server=.\\SQLExpress;Initial Catalog=MetaBuilder;Integrated Security=true;";
        public ClassEditor()
        {
            //changed from server connection
            connString = MetaBuilder.Core.Variables.Instance.ConnectionString;
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            try
            {
                DevLicensedComponent devLic = new DevLicensedComponent();
            }
            catch
            {
                throw new Exception("Developer License Invalid");
                Close();
            }

            //connString = 
            this.sqlConnClasses.ConnectionString = connString;
            this.sqlConnDomainDefs.ConnectionString = connString;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.Run(new ClassEditor());
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Janus.Windows.GridEX.GridEXLayout gridEXLayout3 = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClassEditor));
            Janus.Windows.GridEX.GridEXLayout gridEXLayout4 = new Janus.Windows.GridEX.GridEXLayout();
            this.dsMetaData1 = new MetaBuilder.WinUI.Dictionary.dsMetaData();
            this.gridMeta = new Janus.Windows.GridEX.GridEX();
            this.sqlConnClasses = new System.Data.SqlClient.SqlConnection();
            this.sqlClassAdapter = new System.Data.SqlClient.SqlDataAdapter();
            this.sqlDeleteCommand1 = new System.Data.SqlClient.SqlCommand();
            this.sqlConnDomainDefs = new System.Data.SqlClient.SqlConnection();
            this.sqlInsertCommand1 = new System.Data.SqlClient.SqlCommand();
            this.sqlSelectCommand1 = new System.Data.SqlClient.SqlCommand();
            this.sqlUpdateCommand1 = new System.Data.SqlClient.SqlCommand();
            this.sqlFieldAdapter = new System.Data.SqlClient.SqlDataAdapter();
            this.sqlDeleteCommand2 = new System.Data.SqlClient.SqlCommand();
            this.sqlInsertCommand2 = new System.Data.SqlClient.SqlCommand();
            this.sqlSelectCommand2 = new System.Data.SqlClient.SqlCommand();
            this.sqlUpdateCommand2 = new System.Data.SqlClient.SqlCommand();
            this.btnSave = new Janus.Windows.EditControls.UIButton();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.pnlClassEditor = new System.Windows.Forms.Panel();
            this.sqlAssTypeAdapter = new System.Data.SqlClient.SqlDataAdapter();
            this.sqlDeleteCommand3 = new System.Data.SqlClient.SqlCommand();
            this.sqlInsertCommand3 = new System.Data.SqlClient.SqlCommand();
            this.sqlSelectCommand3 = new System.Data.SqlClient.SqlCommand();
            this.sqlUpdateCommand3 = new System.Data.SqlClient.SqlCommand();
            this.sqlAssAdapter = new System.Data.SqlClient.SqlDataAdapter();
            this.sqlDeleteCommand4 = new System.Data.SqlClient.SqlCommand();
            this.sqlInsertCommand4 = new System.Data.SqlClient.SqlCommand();
            this.sqlSelectCommand4 = new System.Data.SqlClient.SqlCommand();
            this.sqlUpdateCommand4 = new System.Data.SqlClient.SqlCommand();
            this.sqlSelectCommand5 = new System.Data.SqlClient.SqlCommand();
            this.sqlInsertCommand5 = new System.Data.SqlClient.SqlCommand();
            this.sqlUpdateCommand5 = new System.Data.SqlClient.SqlCommand();
            this.sqlDeleteCommand5 = new System.Data.SqlClient.SqlCommand();
            this.sqlAllowedArtifact = new System.Data.SqlClient.SqlDataAdapter();
            this.tabCtl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gridDomains = new Janus.Windows.GridEX.GridEX();
            this.sqlDomainDefinitionAdapter = new System.Data.SqlClient.SqlDataAdapter();
            this.sqlDeleteCommand6 = new System.Data.SqlClient.SqlCommand();
            this.sqlInsertCommand6 = new System.Data.SqlClient.SqlCommand();
            this.sqlSelectCommand6 = new System.Data.SqlClient.SqlCommand();
            this.sqlUpdateCommand6 = new System.Data.SqlClient.SqlCommand();
            this.sqlDomainDefinitionValues = new System.Data.SqlClient.SqlDataAdapter();
            this.sqlDeleteCommand7 = new System.Data.SqlClient.SqlCommand();
            this.sqlInsertCommand7 = new System.Data.SqlClient.SqlCommand();
            this.sqlSelectCommand7 = new System.Data.SqlClient.SqlCommand();
            this.sqlUpdateCommand7 = new System.Data.SqlClient.SqlCommand();
            this.sqlClassType = new System.Data.SqlClient.SqlDataAdapter();
            this.sqlDeleteCommand8 = new System.Data.SqlClient.SqlCommand();
            this.sqlInsertCommand8 = new System.Data.SqlClient.SqlCommand();
            this.sqlSelectCommand8 = new System.Data.SqlClient.SqlCommand();
            this.sqlUpdateCommand8 = new System.Data.SqlClient.SqlCommand();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dsMetaData1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridMeta)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.pnlClassEditor.SuspendLayout();
            this.tabCtl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDomains)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dsMetaData1
            // 
            this.dsMetaData1.DataSetName = "dsMetaData";
            this.dsMetaData1.Locale = new System.Globalization.CultureInfo("en-US");
            this.dsMetaData1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // gridMeta
            // 
            this.gridMeta.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridMeta.DataMember = "Class";
            this.gridMeta.DataSource = this.dsMetaData1;
            this.gridMeta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMeta.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.gridMeta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.gridMeta.GroupByBoxVisible = false;
            this.gridMeta.Hierarchical = true;
            gridEXLayout3.DataMember = "Class";
            gridEXLayout3.DataSource = this.dsMetaData1;
            gridEXLayout3.IsCurrentLayout = true;
            gridEXLayout3.Key = "Default Class Layout";
            gridEXLayout3.LayoutString = resources.GetString("gridEXLayout3.LayoutString");
            this.gridMeta.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            gridEXLayout3});
            this.gridMeta.Location = new System.Drawing.Point(0, 0);
            this.gridMeta.Name = "gridMeta";
            this.gridMeta.Size = new System.Drawing.Size(876, 519);
            this.gridMeta.TabIndex = 0;
            this.gridMeta.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003;
            this.gridMeta.RecordUpdated += new System.EventHandler(this.gridMeta_RecordUpdated);
            this.gridMeta.FormattingRow += new Janus.Windows.GridEX.RowLoadEventHandler(this.gridMeta_FormattingRow);
            this.gridMeta.RecordAdded += new System.EventHandler(this.gridMeta_RecordAdded);
            this.gridMeta.GetNewRow += new Janus.Windows.GridEX.GetNewRowEventHandler(this.gridMeta_GetNewRow);
            // 
            // sqlConnClasses
            // 
            this.sqlConnClasses.ConnectionString = "Server=.\\SQLExpress;Initial Catalog=MetaBuilder;Integrated Security=true;";
            this.sqlConnClasses.FireInfoMessageEventOnUserErrors = false;
            // 
            // sqlClassAdapter
            // 
            this.sqlClassAdapter.DeleteCommand = this.sqlDeleteCommand1;
            this.sqlClassAdapter.InsertCommand = this.sqlInsertCommand1;
            this.sqlClassAdapter.SelectCommand = this.sqlSelectCommand1;
            this.sqlClassAdapter.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "Class", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("Name", "Name"),
                        new System.Data.Common.DataColumnMapping("DescriptionCode", "DescriptionCode"),
                        new System.Data.Common.DataColumnMapping("Category", "Category"),
                        new System.Data.Common.DataColumnMapping("ClassType", "ClassType"),
                        new System.Data.Common.DataColumnMapping("IsActive", "IsActive")})});
            this.sqlClassAdapter.UpdateCommand = this.sqlUpdateCommand1;
            // 
            // sqlDeleteCommand1
            // 
            this.sqlDeleteCommand1.CommandText = resources.GetString("sqlDeleteCommand1.CommandText");
            this.sqlDeleteCommand1.Connection = this.sqlConnDomainDefs;
            this.sqlDeleteCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Original_Name", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Name", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_DescriptionCode", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "DescriptionCode", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_DescriptionCode", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "DescriptionCode", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_Category", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "Category", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_Category", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Category", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_ClassType", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "ClassType", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_ClassType", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "ClassType", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_IsActive", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "IsActive", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_IsActive", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "IsActive", System.Data.DataRowVersion.Original, null)});
            // 
            // sqlConnDomainDefs
            // 
            this.sqlConnDomainDefs.ConnectionString = "Server=.\\SQLExpress;Initial Catalog=MetaBuilder;Integrated Security=true;";
            this.sqlConnDomainDefs.FireInfoMessageEventOnUserErrors = false;
            // 
            // sqlInsertCommand1
            // 
            this.sqlInsertCommand1.CommandText = resources.GetString("sqlInsertCommand1.CommandText");
            this.sqlInsertCommand1.Connection = this.sqlConnDomainDefs;
            this.sqlInsertCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Name", System.Data.SqlDbType.VarChar, 0, "Name"),
            new System.Data.SqlClient.SqlParameter("@DescriptionCode", System.Data.SqlDbType.VarChar, 0, "DescriptionCode"),
            new System.Data.SqlClient.SqlParameter("@Category", System.Data.SqlDbType.VarChar, 0, "Category"),
            new System.Data.SqlClient.SqlParameter("@ClassType", System.Data.SqlDbType.VarChar, 0, "ClassType"),
            new System.Data.SqlClient.SqlParameter("@IsActive", System.Data.SqlDbType.Bit, 0, "IsActive")});
            // 
            // sqlSelectCommand1
            // 
            this.sqlSelectCommand1.CommandText = "SELECT Name, DescriptionCode, Category, ClassType,IsActive FROM Class";
            this.sqlSelectCommand1.Connection = this.sqlConnDomainDefs;
            // 
            // sqlUpdateCommand1
            // 
            this.sqlUpdateCommand1.CommandText = resources.GetString("sqlUpdateCommand1.CommandText");
            this.sqlUpdateCommand1.Connection = this.sqlConnDomainDefs;
            this.sqlUpdateCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Name", System.Data.SqlDbType.VarChar, 0, "Name"),
            new System.Data.SqlClient.SqlParameter("@DescriptionCode", System.Data.SqlDbType.VarChar, 0, "DescriptionCode"),
            new System.Data.SqlClient.SqlParameter("@Category", System.Data.SqlDbType.VarChar, 0, "Category"),
            new System.Data.SqlClient.SqlParameter("@ClassType", System.Data.SqlDbType.VarChar, 0, "ClassType"),
            new System.Data.SqlClient.SqlParameter("@IsActive", System.Data.SqlDbType.Bit, 0, "IsActive"),
            new System.Data.SqlClient.SqlParameter("@Original_Name", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Name", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_DescriptionCode", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "DescriptionCode", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_DescriptionCode", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "DescriptionCode", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_Category", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "Category", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_Category", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Category", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_ClassType", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "ClassType", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_ClassType", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "ClassType", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_IsActive", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "IsActive", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_IsActive", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "IsActive", System.Data.DataRowVersion.Original, null)});
            // 
            // sqlFieldAdapter
            // 
            this.sqlFieldAdapter.DeleteCommand = this.sqlDeleteCommand2;
            this.sqlFieldAdapter.InsertCommand = this.sqlInsertCommand2;
            this.sqlFieldAdapter.SelectCommand = this.sqlSelectCommand2;
            this.sqlFieldAdapter.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "Field", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("pkid", "pkid"),
                        new System.Data.Common.DataColumnMapping("Class", "Class"),
                        new System.Data.Common.DataColumnMapping("Name", "Name"),
                        new System.Data.Common.DataColumnMapping("DataType", "DataType"),
                        new System.Data.Common.DataColumnMapping("Category", "Category"),
                        new System.Data.Common.DataColumnMapping("Description", "Description"),
                        new System.Data.Common.DataColumnMapping("IsUnique", "IsUnique"),
                        new System.Data.Common.DataColumnMapping("SortOrder", "SortOrder"),
                        new System.Data.Common.DataColumnMapping("IsActive", "IsActive")})});
            this.sqlFieldAdapter.UpdateCommand = this.sqlUpdateCommand2;
            // 
            // sqlDeleteCommand2
            // 
            this.sqlDeleteCommand2.CommandText = resources.GetString("sqlDeleteCommand2.CommandText");
            this.sqlDeleteCommand2.Connection = this.sqlConnDomainDefs;
            this.sqlDeleteCommand2.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Original_pkid", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "pkid", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_Class", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Class", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_Name", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Name", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_DataType", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "DataType", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_Category", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Category", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_Description", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "Description", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_Description", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Description", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_IsUnique", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "IsUnique", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_IsUnique", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "IsUnique", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_SortOrder", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "SortOrder", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_SortOrder", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "SortOrder", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_IsActive", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "IsActive", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_IsActive", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "IsActive", System.Data.DataRowVersion.Original, null)});
            // 
            // sqlInsertCommand2
            // 
            this.sqlInsertCommand2.CommandText = resources.GetString("sqlInsertCommand2.CommandText");
            this.sqlInsertCommand2.Connection = this.sqlConnDomainDefs;
            this.sqlInsertCommand2.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Class", System.Data.SqlDbType.VarChar, 0, "Class"),
            new System.Data.SqlClient.SqlParameter("@Name", System.Data.SqlDbType.VarChar, 0, "Name"),
            new System.Data.SqlClient.SqlParameter("@DataType", System.Data.SqlDbType.VarChar, 0, "DataType"),
            new System.Data.SqlClient.SqlParameter("@Category", System.Data.SqlDbType.VarChar, 0, "Category"),
            new System.Data.SqlClient.SqlParameter("@Description", System.Data.SqlDbType.VarChar, 0, "Description"),
            new System.Data.SqlClient.SqlParameter("@IsUnique", System.Data.SqlDbType.Bit, 0, "IsUnique"),
            new System.Data.SqlClient.SqlParameter("@SortOrder", System.Data.SqlDbType.Int, 0, "SortOrder"),
            new System.Data.SqlClient.SqlParameter("@IsActive", System.Data.SqlDbType.Bit, 0, "IsActive")});
            // 
            // sqlSelectCommand2
            // 
            this.sqlSelectCommand2.CommandText = "SELECT pkid, Class, Name, DataType, Category, Description, IsUnique, SortOrder,Is" +
                "Active FROM Field";
            this.sqlSelectCommand2.Connection = this.sqlConnDomainDefs;
            // 
            // sqlUpdateCommand2
            // 
            this.sqlUpdateCommand2.CommandText = resources.GetString("sqlUpdateCommand2.CommandText");
            this.sqlUpdateCommand2.Connection = this.sqlConnDomainDefs;
            this.sqlUpdateCommand2.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Class", System.Data.SqlDbType.VarChar, 0, "Class"),
            new System.Data.SqlClient.SqlParameter("@Name", System.Data.SqlDbType.VarChar, 0, "Name"),
            new System.Data.SqlClient.SqlParameter("@DataType", System.Data.SqlDbType.VarChar, 0, "DataType"),
            new System.Data.SqlClient.SqlParameter("@Category", System.Data.SqlDbType.VarChar, 0, "Category"),
            new System.Data.SqlClient.SqlParameter("@Description", System.Data.SqlDbType.VarChar, 0, "Description"),
            new System.Data.SqlClient.SqlParameter("@IsUnique", System.Data.SqlDbType.Bit, 0, "IsUnique"),
            new System.Data.SqlClient.SqlParameter("@SortOrder", System.Data.SqlDbType.Int, 0, "SortOrder"),
            new System.Data.SqlClient.SqlParameter("@IsActive", System.Data.SqlDbType.Bit, 0, "IsActive"),
            new System.Data.SqlClient.SqlParameter("@Original_pkid", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "pkid", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_Class", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Class", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_Name", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Name", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_DataType", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "DataType", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_Category", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Category", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_Description", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "Description", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_Description", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Description", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_IsUnique", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "IsUnique", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_IsUnique", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "IsUnique", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_SortOrder", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "SortOrder", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_SortOrder", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "SortOrder", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_IsActive", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "IsActive", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_IsActive", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "IsActive", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@pkid", System.Data.SqlDbType.Int, 4, "pkid")});
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(512, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(224, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save && Compile MetaModel";
            this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.LightSlateGray;
            this.pnlBottom.Controls.Add(this.progressBar1);
            this.pnlBottom.Controls.Add(this.statusBar1);
            this.pnlBottom.Controls.Add(this.btnSave);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 545);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(884, 57);
            this.pnlBottom.TabIndex = 2;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(4, 8);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(502, 16);
            this.progressBar1.TabIndex = 3;
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 35);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(884, 22);
            this.statusBar1.SizingGrip = false;
            this.statusBar1.TabIndex = 2;
            // 
            // pnlClassEditor
            // 
            this.pnlClassEditor.Controls.Add(this.gridMeta);
            this.pnlClassEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlClassEditor.Location = new System.Drawing.Point(0, 0);
            this.pnlClassEditor.Name = "pnlClassEditor";
            this.pnlClassEditor.Size = new System.Drawing.Size(876, 519);
            this.pnlClassEditor.TabIndex = 3;
            // 
            // sqlAssTypeAdapter
            // 
            this.sqlAssTypeAdapter.DeleteCommand = this.sqlDeleteCommand3;
            this.sqlAssTypeAdapter.InsertCommand = this.sqlInsertCommand3;
            this.sqlAssTypeAdapter.SelectCommand = this.sqlSelectCommand3;
            this.sqlAssTypeAdapter.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "AssociationType", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("pkid", "pkid"),
                        new System.Data.Common.DataColumnMapping("Name", "Name")})});
            this.sqlAssTypeAdapter.UpdateCommand = this.sqlUpdateCommand3;
            // 
            // sqlDeleteCommand3
            // 
            this.sqlDeleteCommand3.CommandText = "DELETE FROM AssociationType WHERE (pkid = @Original_pkid) AND (Name = @Original_N" +
                "ame OR @Original_Name IS NULL AND Name IS NULL)";
            this.sqlDeleteCommand3.Connection = this.sqlConnClasses;
            this.sqlDeleteCommand3.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Original_pkid", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "pkid", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_Name", System.Data.SqlDbType.VarChar, 25, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Name", System.Data.DataRowVersion.Original, null)});
            // 
            // sqlInsertCommand3
            // 
            this.sqlInsertCommand3.CommandText = "INSERT INTO AssociationType(Name) VALUES (@Name); SELECT pkid, Name FROM Associat" +
                "ionType WHERE (pkid = @@IDENTITY)";
            this.sqlInsertCommand3.Connection = this.sqlConnClasses;
            this.sqlInsertCommand3.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Name", System.Data.SqlDbType.VarChar, 25, "Name")});
            // 
            // sqlSelectCommand3
            // 
            this.sqlSelectCommand3.CommandText = "SELECT pkid, Name FROM AssociationType";
            this.sqlSelectCommand3.Connection = this.sqlConnClasses;
            // 
            // sqlUpdateCommand3
            // 
            this.sqlUpdateCommand3.CommandText = resources.GetString("sqlUpdateCommand3.CommandText");
            this.sqlUpdateCommand3.Connection = this.sqlConnClasses;
            this.sqlUpdateCommand3.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Name", System.Data.SqlDbType.VarChar, 25, "Name"),
            new System.Data.SqlClient.SqlParameter("@Original_pkid", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "pkid", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_Name", System.Data.SqlDbType.VarChar, 25, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Name", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@pkid", System.Data.SqlDbType.Int, 4, "pkid")});
            // 
            // sqlAssAdapter
            // 
            this.sqlAssAdapter.DeleteCommand = this.sqlDeleteCommand4;
            this.sqlAssAdapter.InsertCommand = this.sqlInsertCommand4;
            this.sqlAssAdapter.SelectCommand = this.sqlSelectCommand4;
            this.sqlAssAdapter.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "ClassAssociation", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("CAid", "CAid"),
                        new System.Data.Common.DataColumnMapping("ParentClass", "ParentClass"),
                        new System.Data.Common.DataColumnMapping("ChildClass", "ChildClass"),
                        new System.Data.Common.DataColumnMapping("AssociationTypeID", "AssociationTypeID"),
                        new System.Data.Common.DataColumnMapping("Caption", "Caption"),
                        new System.Data.Common.DataColumnMapping("AssociationObjectClass", "AssociationObjectClass"),
                        new System.Data.Common.DataColumnMapping("IsDefault", "IsDefault"),
                        new System.Data.Common.DataColumnMapping("IsActive", "IsActive")})});
            this.sqlAssAdapter.UpdateCommand = this.sqlUpdateCommand4;
            // 
            // sqlDeleteCommand4
            // 
            this.sqlDeleteCommand4.CommandText = resources.GetString("sqlDeleteCommand4.CommandText");
            this.sqlDeleteCommand4.Connection = this.sqlConnDomainDefs;
            this.sqlDeleteCommand4.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Original_CAid", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CAid", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_ParentClass", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "ParentClass", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_ChildClass", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "ChildClass", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_AssociationTypeID", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "AssociationTypeID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_Caption", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "Caption", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_Caption", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Caption", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_AssociationObjectClass", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "AssociationObjectClass", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_AssociationObjectClass", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "AssociationObjectClass", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_IsDefault", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "IsDefault", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_IsActive", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "IsActive", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_IsActive", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "IsActive", System.Data.DataRowVersion.Original, null)});
            // 
            // sqlInsertCommand4
            // 
            this.sqlInsertCommand4.CommandText = resources.GetString("sqlInsertCommand4.CommandText");
            this.sqlInsertCommand4.Connection = this.sqlConnDomainDefs;
            this.sqlInsertCommand4.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@ParentClass", System.Data.SqlDbType.VarChar, 0, "ParentClass"),
            new System.Data.SqlClient.SqlParameter("@ChildClass", System.Data.SqlDbType.VarChar, 0, "ChildClass"),
            new System.Data.SqlClient.SqlParameter("@AssociationTypeID", System.Data.SqlDbType.Int, 0, "AssociationTypeID"),
            new System.Data.SqlClient.SqlParameter("@Caption", System.Data.SqlDbType.VarChar, 0, "Caption"),
            new System.Data.SqlClient.SqlParameter("@AssociationObjectClass", System.Data.SqlDbType.VarChar, 0, "AssociationObjectClass"),
            new System.Data.SqlClient.SqlParameter("@IsDefault", System.Data.SqlDbType.Bit, 0, "IsDefault"),
            new System.Data.SqlClient.SqlParameter("@IsActive", System.Data.SqlDbType.Bit, 0, "IsActive")});
            // 
            // sqlSelectCommand4
            // 
            this.sqlSelectCommand4.CommandText = "SELECT CAid, ParentClass, ChildClass, AssociationTypeID, Caption, AssociationObje" +
                "ctClass,IsDefault ,IsActive FROM ClassAssociation";
            this.sqlSelectCommand4.Connection = this.sqlConnDomainDefs;
            // 
            // sqlUpdateCommand4
            // 
            this.sqlUpdateCommand4.CommandText = resources.GetString("sqlUpdateCommand4.CommandText");
            this.sqlUpdateCommand4.Connection = this.sqlConnDomainDefs;
            this.sqlUpdateCommand4.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@ParentClass", System.Data.SqlDbType.VarChar, 0, "ParentClass"),
            new System.Data.SqlClient.SqlParameter("@ChildClass", System.Data.SqlDbType.VarChar, 0, "ChildClass"),
            new System.Data.SqlClient.SqlParameter("@AssociationTypeID", System.Data.SqlDbType.Int, 0, "AssociationTypeID"),
            new System.Data.SqlClient.SqlParameter("@Caption", System.Data.SqlDbType.VarChar, 0, "Caption"),
            new System.Data.SqlClient.SqlParameter("@AssociationObjectClass", System.Data.SqlDbType.VarChar, 0, "AssociationObjectClass"),
            new System.Data.SqlClient.SqlParameter("@IsDefault", System.Data.SqlDbType.Bit, 0, "IsDefault"),
            new System.Data.SqlClient.SqlParameter("@IsActive", System.Data.SqlDbType.Bit, 0, "IsActive"),
            new System.Data.SqlClient.SqlParameter("@Original_CAid", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CAid", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_ParentClass", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "ParentClass", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_ChildClass", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "ChildClass", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_AssociationTypeID", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "AssociationTypeID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_Caption", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "Caption", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_Caption", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Caption", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_AssociationObjectClass", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "AssociationObjectClass", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_AssociationObjectClass", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "AssociationObjectClass", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_IsDefault", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "IsDefault", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_IsActive", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "IsActive", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_IsActive", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "IsActive", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@CAid", System.Data.SqlDbType.Int, 4, "CAid")});
            // 
            // sqlSelectCommand5
            // 
            this.sqlSelectCommand5.CommandText = "SELECT CAid, Class,IsActive FROM AllowedArtifact";
            this.sqlSelectCommand5.Connection = this.sqlConnClasses;
            // 
            // sqlInsertCommand5
            // 
            this.sqlInsertCommand5.CommandText = "INSERT INTO [AllowedArtifact] ([CAid], [Class], [IsActive]) VALUES (@CAid, @Class" +
                ", @IsActive);\r\nSELECT CAid, Class, IsActive FROM AllowedArtifact WHERE (CAid = @" +
                "CAid) AND (Class = @Class)";
            this.sqlInsertCommand5.Connection = this.sqlConnClasses;
            this.sqlInsertCommand5.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@CAid", System.Data.SqlDbType.Int, 0, "CAid"),
            new System.Data.SqlClient.SqlParameter("@Class", System.Data.SqlDbType.VarChar, 0, "Class"),
            new System.Data.SqlClient.SqlParameter("@IsActive", System.Data.SqlDbType.Bit, 0, "IsActive")});
            // 
            // sqlUpdateCommand5
            // 
            this.sqlUpdateCommand5.CommandText = resources.GetString("sqlUpdateCommand5.CommandText");
            this.sqlUpdateCommand5.Connection = this.sqlConnClasses;
            this.sqlUpdateCommand5.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@CAid", System.Data.SqlDbType.Int, 0, "CAid"),
            new System.Data.SqlClient.SqlParameter("@Class", System.Data.SqlDbType.VarChar, 0, "Class"),
            new System.Data.SqlClient.SqlParameter("@IsActive", System.Data.SqlDbType.Bit, 0, "IsActive"),
            new System.Data.SqlClient.SqlParameter("@Original_CAid", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CAid", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_Class", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Class", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_IsActive", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "IsActive", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_IsActive", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "IsActive", System.Data.DataRowVersion.Original, null)});
            // 
            // sqlDeleteCommand5
            // 
            this.sqlDeleteCommand5.CommandText = "DELETE FROM [AllowedArtifact] WHERE (([CAid] = @Original_CAid) AND ([Class] = @Or" +
                "iginal_Class) AND ((@IsNull_IsActive = \'True\' AND [IsActive] IS NULL) OR ([IsAct" +
                "ive] = @Original_IsActive)))";
            this.sqlDeleteCommand5.Connection = this.sqlConnClasses;
            this.sqlDeleteCommand5.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Original_CAid", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CAid", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_Class", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Class", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_IsActive", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "IsActive", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_IsActive", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "IsActive", System.Data.DataRowVersion.Original, null)});
            // 
            // sqlAllowedArtifact
            // 
            this.sqlAllowedArtifact.DeleteCommand = this.sqlDeleteCommand5;
            this.sqlAllowedArtifact.InsertCommand = this.sqlInsertCommand5;
            this.sqlAllowedArtifact.SelectCommand = this.sqlSelectCommand5;
            this.sqlAllowedArtifact.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "AllowedArtifact", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("CAid", "CAid"),
                        new System.Data.Common.DataColumnMapping("Class", "Class"),
                        new System.Data.Common.DataColumnMapping("IsActive", "IsActive")})});
            this.sqlAllowedArtifact.UpdateCommand = this.sqlUpdateCommand5;
            // 
            // tabCtl
            // 
            this.tabCtl.Controls.Add(this.tabPage1);
            this.tabCtl.Controls.Add(this.tabPage2);
            this.tabCtl.Controls.Add(this.tabPage3);
            this.tabCtl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtl.Location = new System.Drawing.Point(0, 0);
            this.tabCtl.Name = "tabCtl";
            this.tabCtl.SelectedIndex = 0;
            this.tabCtl.Size = new System.Drawing.Size(884, 545);
            this.tabCtl.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pnlClassEditor);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(876, 519);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Class, Field, Association && Artifact Editor";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.gridDomains);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(876, 519);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Domain Definitions";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // gridDomains
            // 
            this.gridDomains.DataMember = "DomainDefinition";
            this.gridDomains.DataSource = this.dsMetaData1;
            gridEXLayout4.LayoutString = resources.GetString("gridEXLayout4.LayoutString");
            this.gridDomains.DesignTimeLayout = gridEXLayout4;
            this.gridDomains.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDomains.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.gridDomains.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridDomains.GroupByBoxVisible = false;
            this.gridDomains.Hierarchical = true;
            this.gridDomains.Location = new System.Drawing.Point(0, 0);
            this.gridDomains.Name = "gridDomains";
            this.gridDomains.Size = new System.Drawing.Size(876, 519);
            this.gridDomains.TabIndex = 0;
            // 
            // sqlDomainDefinitionAdapter
            // 
            this.sqlDomainDefinitionAdapter.DeleteCommand = this.sqlDeleteCommand6;
            this.sqlDomainDefinitionAdapter.InsertCommand = this.sqlInsertCommand6;
            this.sqlDomainDefinitionAdapter.SelectCommand = this.sqlSelectCommand6;
            this.sqlDomainDefinitionAdapter.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "DomainDefinition", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("pkid", "pkid"),
                        new System.Data.Common.DataColumnMapping("Name", "Name")})});
            this.sqlDomainDefinitionAdapter.UpdateCommand = this.sqlUpdateCommand6;
            // 
            // sqlDeleteCommand6
            // 
            this.sqlDeleteCommand6.CommandText = "DELETE FROM DomainDefinition WHERE (pkid = @Original_pkid) AND (Name = @Original_" +
                "Name)";
            this.sqlDeleteCommand6.Connection = this.sqlConnDomainDefs;
            this.sqlDeleteCommand6.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Original_pkid", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "pkid", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_Name", System.Data.SqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Name", System.Data.DataRowVersion.Original, null)});
            // 
            // sqlInsertCommand6
            // 
            this.sqlInsertCommand6.CommandText = "INSERT INTO DomainDefinition(Name) VALUES (@Name); SELECT pkid, Name FROM DomainD" +
                "efinition WHERE (pkid = @@IDENTITY)";
            this.sqlInsertCommand6.Connection = this.sqlConnDomainDefs;
            this.sqlInsertCommand6.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Name", System.Data.SqlDbType.VarChar, 50, "Name")});
            // 
            // sqlSelectCommand6
            // 
            this.sqlSelectCommand6.CommandText = "SELECT pkid, Name FROM DomainDefinition";
            this.sqlSelectCommand6.Connection = this.sqlConnDomainDefs;
            // 
            // sqlUpdateCommand6
            // 
            this.sqlUpdateCommand6.CommandText = "UPDATE DomainDefinition SET Name = @Name WHERE (pkid = @Original_pkid) AND (Name " +
                "= @Original_Name); SELECT pkid, Name FROM DomainDefinition WHERE (pkid = @pkid)";
            this.sqlUpdateCommand6.Connection = this.sqlConnDomainDefs;
            this.sqlUpdateCommand6.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Name", System.Data.SqlDbType.VarChar, 50, "Name"),
            new System.Data.SqlClient.SqlParameter("@Original_pkid", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "pkid", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_Name", System.Data.SqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Name", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@pkid", System.Data.SqlDbType.Int, 4, "pkid")});
            // 
            // sqlDomainDefinitionValues
            // 
            this.sqlDomainDefinitionValues.DeleteCommand = this.sqlDeleteCommand7;
            this.sqlDomainDefinitionValues.InsertCommand = this.sqlInsertCommand7;
            this.sqlDomainDefinitionValues.SelectCommand = this.sqlSelectCommand7;
            this.sqlDomainDefinitionValues.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "DomainDefinitionPossibleValue", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("DomainDefinitionID", "DomainDefinitionID"),
                        new System.Data.Common.DataColumnMapping("PossibleValue", "PossibleValue"),
                        new System.Data.Common.DataColumnMapping("Series", "Series"),
                        new System.Data.Common.DataColumnMapping("Description", "Description"),
                        new System.Data.Common.DataColumnMapping("IsActive", "IsActive")})});
            this.sqlDomainDefinitionValues.UpdateCommand = this.sqlUpdateCommand7;
            // 
            // sqlDeleteCommand7
            // 
            this.sqlDeleteCommand7.CommandText = resources.GetString("sqlDeleteCommand7.CommandText");
            this.sqlDeleteCommand7.Connection = this.sqlConnDomainDefs;
            this.sqlDeleteCommand7.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Original_DomainDefinitionID", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "DomainDefinitionID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_PossibleValue", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "PossibleValue", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_Series", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Series", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_Description", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "Description", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_Description", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Description", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_IsActive", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "IsActive", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_IsActive", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "IsActive", System.Data.DataRowVersion.Original, null)});
            // 
            // sqlInsertCommand7
            // 
            this.sqlInsertCommand7.CommandText = resources.GetString("sqlInsertCommand7.CommandText");
            this.sqlInsertCommand7.Connection = this.sqlConnDomainDefs;
            this.sqlInsertCommand7.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@DomainDefinitionID", System.Data.SqlDbType.Int, 0, "DomainDefinitionID"),
            new System.Data.SqlClient.SqlParameter("@PossibleValue", System.Data.SqlDbType.VarChar, 0, "PossibleValue"),
            new System.Data.SqlClient.SqlParameter("@Series", System.Data.SqlDbType.Int, 0, "Series"),
            new System.Data.SqlClient.SqlParameter("@Description", System.Data.SqlDbType.VarChar, 0, "Description"),
            new System.Data.SqlClient.SqlParameter("@IsActive", System.Data.SqlDbType.Bit, 0, "IsActive")});
            // 
            // sqlSelectCommand7
            // 
            this.sqlSelectCommand7.CommandText = "SELECT DomainDefinitionID, PossibleValue, Series, Description,IsActive FROM Domai" +
                "nDefinitionPossibleValue";
            this.sqlSelectCommand7.Connection = this.sqlConnDomainDefs;
            // 
            // sqlUpdateCommand7
            // 
            this.sqlUpdateCommand7.CommandText = resources.GetString("sqlUpdateCommand7.CommandText");
            this.sqlUpdateCommand7.Connection = this.sqlConnDomainDefs;
            this.sqlUpdateCommand7.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@DomainDefinitionID", System.Data.SqlDbType.Int, 0, "DomainDefinitionID"),
            new System.Data.SqlClient.SqlParameter("@PossibleValue", System.Data.SqlDbType.VarChar, 0, "PossibleValue"),
            new System.Data.SqlClient.SqlParameter("@Series", System.Data.SqlDbType.Int, 0, "Series"),
            new System.Data.SqlClient.SqlParameter("@Description", System.Data.SqlDbType.VarChar, 0, "Description"),
            new System.Data.SqlClient.SqlParameter("@IsActive", System.Data.SqlDbType.Bit, 0, "IsActive"),
            new System.Data.SqlClient.SqlParameter("@Original_DomainDefinitionID", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "DomainDefinitionID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_PossibleValue", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "PossibleValue", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_Series", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Series", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_Description", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "Description", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_Description", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Description", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_IsActive", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "IsActive", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_IsActive", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "IsActive", System.Data.DataRowVersion.Original, null)});
            // 
            // sqlClassType
            // 
            this.sqlClassType.DeleteCommand = this.sqlDeleteCommand8;
            this.sqlClassType.InsertCommand = this.sqlInsertCommand8;
            this.sqlClassType.SelectCommand = this.sqlSelectCommand8;
            this.sqlClassType.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "ClassType", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("ClassType", "ClassType")})});
            this.sqlClassType.UpdateCommand = this.sqlUpdateCommand8;
            // 
            // sqlDeleteCommand8
            // 
            this.sqlDeleteCommand8.CommandText = "DELETE FROM ClassType WHERE (ClassType = @Original_Type)";
            this.sqlDeleteCommand8.Connection = this.sqlConnDomainDefs;
            this.sqlDeleteCommand8.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Original_ClassType", System.Data.SqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "ClassType", System.Data.DataRowVersion.Original, null)});
            // 
            // sqlInsertCommand8
            // 
            this.sqlInsertCommand8.CommandText = "INSERT INTO ClassType(ClassType) VALUES (@ClassType); SELECT ClassType FROM Class" +
                "Type WHERE (ClassType = @ClassType)";
            this.sqlInsertCommand8.Connection = this.sqlConnDomainDefs;
            this.sqlInsertCommand8.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@ClassType", System.Data.SqlDbType.VarChar, 50, "ClassType")});
            // 
            // sqlSelectCommand8
            // 
            this.sqlSelectCommand8.CommandText = "SELECT ClassType FROM ClassType";
            this.sqlSelectCommand8.Connection = this.sqlConnDomainDefs;
            // 
            // sqlUpdateCommand8
            // 
            this.sqlUpdateCommand8.CommandText = "UPDATE ClassType SET ClassType = @ClassType WHERE (ClassType = @Original_ClassTyp" +
                "e); SELECT ClassType FROM ClassType WHERE (ClassType = @ClassType)";
            this.sqlUpdateCommand8.Connection = this.sqlConnDomainDefs;
            this.sqlUpdateCommand8.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@ClassType", System.Data.SqlDbType.VarChar, 50, "ClassType"),
            new System.Data.SqlClient.SqlParameter("@Original_ClassType", System.Data.SqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "ClassType", System.Data.DataRowVersion.Original, null)});
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(876, 519);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Information";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(870, 513);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ClassEditor
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(884, 602);
            this.Controls.Add(this.tabCtl);
            this.Controls.Add(this.pnlBottom);
            this.Name = "ClassEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MetaModel Configuration Editor";
            this.Load += new System.EventHandler(this.ClassEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dsMetaData1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridMeta)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.pnlClassEditor.ResumeLayout(false);
            this.tabCtl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDomains)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private void ClassEditor_Load(object sender, EventArgs e)
        {
            String strHostName = Dns.GetHostName();
            // Console.WriteLine("Host Name: " + strHostName);

            // Find host by name
            IPHostEntry iphostentry = Dns.GetHostEntry(strHostName);

            // Enumerate IP addresses
            //int nIP = 0;
            foreach (IPAddress ipaddress in iphostentry.AddressList)
            {
                /* Console.WriteLine("IP #" + ++nIP + ": " +
                                  ipaddress.ToString());*/
                /*if (ipaddress.ToString().Substring(0, 3) == "172")
                    foundValidIP = true;*/
            }

            // if (foundValidIP)
            {
                /*UserLogin loginform = new UserLogin();
                loginform.Closed += new EventHandler(this.loginform_Closed);
                loginform.ShowDialog(this);*/
            }
            LoadData();


        }

        private void loginform_Closed(object sender, EventArgs e)
        {
            /*  if (true)//.UserID > 0) && (globals.HasPermission(4, true)))
              {*/
            LoadData();
            /* }
             else
             {
                 MessageBox.Show(this,"You do not have the required permission to edit the MetaModel", "Permission not granted", MessageBoxButtons.OK);
                 this.Close();
             }*/
        }

        private void LoadData()
        {
            this.sqlDomainDefinitionAdapter.Fill(dsMetaData1.DomainDefinition);
            this.sqlDomainDefinitionValues.Fill(dsMetaData1.DomainDefinitionPossibleValue);
            this.sqlClassType.Fill(dsMetaData1.ClassType);
            this.sqlClassAdapter.Fill(dsMetaData1.Class);

            this.sqlFieldAdapter.Fill(dsMetaData1.Field);
            this.sqlAssTypeAdapter.Fill(dsMetaData1.AssociationType);
            this.sqlAssAdapter.Fill(dsMetaData1.ClassAssociation);
            this.sqlAllowedArtifact.Fill(dsMetaData1.AllowedArtifact);

            DataTable dtDataTypes = new DataTable("DataTypes");
            dtDataTypes.Columns.Add("Name", typeof(String));
            DataRow dtR = dtDataTypes.NewRow();
            dtR["Name"] = "System.String";
            dtDataTypes.Rows.Add(dtR);

            dtR = dtDataTypes.NewRow();
            dtR["Name"] = "System.Int32";
            dtDataTypes.Rows.Add(dtR);

            dtR = dtDataTypes.NewRow();
            dtR["Name"] = "System.Int32?";
            dtDataTypes.Rows.Add(dtR);

            dtR = dtDataTypes.NewRow();
            dtR["Name"] = "System.DateTime";
            dtDataTypes.Rows.Add(dtR);

            dtR = dtDataTypes.NewRow();
            dtR["Name"] = "LongText";
            dtDataTypes.Rows.Add(dtR);

            dtR = dtDataTypes.NewRow();
            dtR["Name"] = "System.Boolean";
            dtDataTypes.Rows.Add(dtR);

            dtR = dtDataTypes.NewRow();
            dtR["Name"] = "System.Double";
            dtDataTypes.Rows.Add(dtR);

            foreach (DataRowView drvClass in dsMetaData1.Tables["Class"].DefaultView)
            {
                dtR = dtDataTypes.NewRow();
                dtR["Name"] = drvClass["Name"].ToString();
                dtDataTypes.Rows.Add(dtR);
            }
            foreach (DataRowView drvClass in dsMetaData1.Tables["Class"].DefaultView)
            {
                dtR = dtDataTypes.NewRow();
                dtR["Name"] = "List<" + drvClass["Name"].ToString() + ">";
                dtDataTypes.Rows.Add(dtR);
            }

            dtR = dtDataTypes.NewRow();
            dtR["Name"] = "Attachment";
            dtDataTypes.Rows.Add(dtR);

            dtR = dtDataTypes.NewRow();
            dtR["Name"] = "List<Attachment>";
            dtDataTypes.Rows.Add(dtR);

            foreach (DataRowView drvDomainDefinition in dsMetaData1.DomainDefinition.DefaultView)
            {
                dtR = dtDataTypes.NewRow();
                dtR["Name"] = drvDomainDefinition["Name"].ToString();
                dtDataTypes.Rows.Add(dtR);
            }

            foreach (DataRowView drvDomainDefinition in dsMetaData1.DomainDefinition.DefaultView)
            {
                dtR = dtDataTypes.NewRow();
                dtR["Name"] = "List<" + drvDomainDefinition["Name"].ToString() + ">";
                dtDataTypes.Rows.Add(dtR);
            }
            dtDataTypes.AcceptChanges();

            this.gridMeta.DropDowns[0].DataSource = dtDataTypes;
            this.gridMeta.DropDowns[1].DataSource = dsMetaData1.AssociationType;
            this.gridMeta.DropDowns[2].DataSource = dsMetaData1.Class;
            this.gridMeta.DropDowns[3].DataSource = dsMetaData1.ClassType;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.statusBar1.Text = "Updating database...";
                /*Thread workerThread = new Thread(new ThreadStart(this.UpdateData));
                workerThread.Start();
                this.progressBar1.Value = 0;*/

                UpdateData();
                GenerateAndCompile();

                // now merge!
                /*MergeModule.CreateModule cmodule = new MergeModule.CreateModule();
                cmodule.Create(connString);
                MergeModule.Merger merger = new MergeModule.Merger();*/

            }
            catch (Exception x)
            {
                MessageBox.Show(this, "The changes were unsuccessful.\n" + x.ToString());
                Core.Log.WriteLog(x.ToString());
                try
                {
                    EndLogging();
                }
                catch
                {
                    //dont care
                }
            }

        }

        private void GenerateAndCompile()
        {
            this.statusBar1.Text = "Compiling Assemblies...";
            this.progressBar1.Maximum = dsMetaData1.Tables["Class"].DefaultView.Count;
            this.progressBar1.Minimum = 0;
            this.progressBar1.Value = 0;
            StartLogging();

            // Create compiler & viewbuilder
            Compiler comp = new Compiler(dsMetaData1.Tables["Class"].DefaultView.Count, dsMetaData1.DomainDefinition.DefaultView.Count);
            ViewBuilder viewBuilder = new ViewBuilder();

            this.statusBar1.Text = "Compiling Enums...";
            // Iterate through domain definitions, generating code for each
            foreach (DataRowView drvEnum in dsMetaData1.DomainDefinition.DefaultView)
            {
                DataView dvValues = dsMetaData1.DomainDefinitionPossibleValue.DefaultView;
                dvValues.RowFilter = "DomainDefinitionID=" + drvEnum["pkid"].ToString();
                dvValues.Sort = "Series ASC";
                comp.CreateEnum(Variables.MetaNameSpace, drvEnum["Name"].ToString());
                foreach (DataRowView drvVal in dvValues)
                {
                    if (bool.Parse(drvVal["IsActive"].ToString()) == false)
                        continue;
                    int order = int.Parse(drvVal["Series"].ToString());
                    string EnumValue = drvVal["PossibleValue"].ToString();
                    comp.AddEnumValue(drvVal["Description"].ToString(), EnumValue, order);
                }
                comp.GenerateEnumCode();
            }
            comp.CompileEnumCode();
            LogErrors(comp);

            this.statusBar1.Text = "Creating Classes And Sorters...";
            // Iterate through classes, generating code for each
            foreach (DataRowView drvClass in dsMetaData1.Tables["Class"].DefaultView)
            {
                // Create the class
                DataView dvFields = dsMetaData1.Tables["Field"].DefaultView;
                dvFields.RowFilter = "Class='" + drvClass["Name"].ToString() + "'";
                comp.CreateClass(Variables.MetaNameSpace, drvClass["Name"].ToString());

                foreach (DataRowView drvField in dvFields)
                {
                    string fieldname = drvField["Name"].ToString().ToLower();
                    string propname = drvField["Name"].ToString();
                    bool isDomain = false;
                    foreach (DataRowView drvEnums in this.dsMetaData1.DomainDefinition.DefaultView)
                    {
                        if (drvEnums["Name"].ToString() == drvField["DataType"].ToString())
                        {
                            isDomain = true;
                        }
                    }
                    if (!isDomain && drvField["DataType"].ToString().IndexOf("Attachment") == -1)
                    {
                        bool IsActive = bool.Parse(drvField["IsActive"].ToString());
                        comp.AddField(fieldname, propname, drvField["DataType"].ToString(), drvField["Description"].ToString(), drvField["Category"].ToString(), IsActive);
                    }
                    else if (drvField["DataType"].ToString() == "Attachment")
                    {
                        comp.AddAttachmentField(fieldname, propname, drvField["DataType"].ToString(), drvField["Description"].ToString(), drvField["Category"].ToString());
                    }
                    else if (drvField["DataType"].ToString() == "List<Attachment>")
                    {
                        comp.AddAttachmentListField(fieldname, propname, drvField["DataType"].ToString(), drvField["Description"].ToString(), drvField["Category"].ToString());
                    }
                    else
                    {
                        bool IsActive = bool.Parse(drvField["IsActive"].ToString());
                        comp.AddDomainField(fieldname, propname, drvField["DataType"].ToString(), drvField["Description"].ToString(), drvField["Category"].ToString(), drvField["DataType"].ToString(), IsActive);
                    }

                }
                if (drvClass["DescriptionCode"].ToString().Length > 0)
                {
                    ////5 February 2013
                    //if (drvClass["Name"].ToString() == "Rationale")
                    //{
                    //    comp.AddToStringField(drvClass["DescriptionCode"].ToString() + ".ToString()");
                    //}
                    //else
                    comp.AddToStringField(drvClass["DescriptionCode"].ToString());
                }
                comp.GenerateCode();

                // Create the field sorter (typeeditor derived from basesorter)
                dvFields.Sort = "SortOrder";
                string[] sortedFields = new string[dvFields.Count];
                int fieldCounter = 0;
                foreach (DataRowView drvField in dvFields)
                {
                    sortedFields[fieldCounter] = drvField["Name"].ToString();
                    fieldCounter++;
                }

                comp.CreateSortClass(Variables.MetaNameSpace, drvClass["Name"].ToString() + "Sort", sortedFields);
                comp.GenerateSortCode();

                viewBuilder.BuildView(drvClass["Name"].ToString());
                this.progressBar1.Value++;
            }

            this.statusBar1.Text = "Compiling Sorters...";
            // Since the MetaBase related classes are dependent on the sorter classes, compile the sorters first.
            comp.CompileSortCode();
            LogErrors(comp);

            this.statusBar1.Text = "Compiling Classes...";
            // Now compile the MetaBase related classes
            comp.CompileCode();
            LogErrors(comp);

            EndLogging();
            if (comp.MainAssemblyMoved && comp.SortAssemblyMoved)
                MessageBox.Show(this, "Please not that 2 dlls have been placed in your c:\\ drive. You must move/copy these into MetaBuilder's root folder before they will be used. This has happened because those dlls are currently in use.", "Dynamic Link Library Location", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (comp.MainAssemblyMoved)
                MessageBox.Show(this, "Please not that the Main Assembly has been placed in your c:\\ drive. You must move/copy this file into MetaBuilder's root folder before it will be used. This has happened because those dlls are currently in use.", "Dynamic Link Library Location", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (comp.SortAssemblyMoved)
                MessageBox.Show(this, "Please not that the Sort Assembly has been placed in your c:\\ drive. You must move/copy this file into MetaBuilder's root folder before it will be used. This has happened because those dlls are currently in use.", "Dynamic Link Library Location", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(this, "No errors have occurred in the compilation of this metamodel", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private StreamWriter swriter;
        bool ErrorsOccurred;
        private void StartLogging()
        {
            swriter = new StreamWriter(Variables.Instance.MetaAssemblyPath + "\\CompilerLog.log", false);
            ErrorsOccurred = false;
        }
        private void EndLogging()
        {
            swriter.Close();
            if (ErrorsOccurred)
            {
                string fileName = Variables.Instance.MetaAssemblyPath + "CompilerLog.log";
                this.statusBar1.Text = "Errors occurred during compilation. Check " + fileName + " for details";
                this.progressBar1.Value = 0;
                Process newProc = Process.Start(fileName);
            }
            else
            {
                this.statusBar1.Text = "Completed without errors";
                this.progressBar1.Value = 0;
            }
        }

        private void LogErrors(Compiler comp)
        {
            if (comp.results.Errors.Count > 0)
            {
                ErrorsOccurred = true;
                foreach (CompilerError err in comp.results.Errors)
                {
                    swriter.WriteLine("FileName: " + err.FileName + Environment.NewLine + "Error Number: " + err.ErrorNumber + Environment.NewLine + "Error Text: " + err.ErrorText);
                }
            }
        }

        private void UpdateData()
        {
            this.progressBar1.Maximum = 5;
            this.progressBar1.Minimum = 0;
            this.progressBar1.Value = 0;

            //TODO: Add hectic validation here
            #region Update Domain Definitions
            DataTable changedDomainDefinitionRecords = dsMetaData1.DomainDefinition.GetChanges();
            if (changedDomainDefinitionRecords != null && changedDomainDefinitionRecords.Rows.Count > 0)
            {
                foreach (DataRow dr in changedDomainDefinitionRecords.Rows)
                {
                    if (dr["Name"].ToString().IndexOf(" ") > 0)
                    {
                        MessageBox.Show(this, "Domain Definitions: " + Environment.NewLine + "Ensure that there are no spaces in the Domain Definition Names");
                        return;
                    }
                }

                this.sqlDomainDefinitionAdapter.Update(dsMetaData1.DomainDefinition);
                this.progressBar1.Value++;
            }
            #endregion

            #region Update Domain Definition Possible Values
            DataTable changedDomainDefinitionPossibleValueRecords = dsMetaData1.DomainDefinitionPossibleValue.GetChanges();
            if (changedDomainDefinitionPossibleValueRecords != null && changedDomainDefinitionPossibleValueRecords.Rows.Count > 0)
            {
                // Ensure that none of the records have spaces in the value field
                foreach (DataRow dr in changedDomainDefinitionPossibleValueRecords.Rows)
                {
                    if (dr["PossibleValue"].ToString().IndexOf(" ") > 0 || dr["PossibleValue"].ToString().IndexOf("\\") > 0 || dr["PossibleValue"].ToString().IndexOf("/") > 0 || dr["PossibleValue"].ToString().IndexOf("-") > 0)
                    {
                        MessageBox.Show(this, "Domain Definition Values: " + Environment.NewLine + "Ensure that there are no spaces, slashes or dashes in the Domain Definition Values");
                        return;
                    }
                }
                this.sqlDomainDefinitionValues.Update(dsMetaData1.DomainDefinitionPossibleValue);
                this.progressBar1.Value++;
            }
            #endregion

            #region Update classes
            DataTable changedClassRecords = dsMetaData1.Class.GetChanges();
            if (changedClassRecords != null && changedClassRecords.Rows.Count > 0)
            {
                // Ensure that none of the records have spaces in the class names
                foreach (DataRow dr in changedClassRecords.Rows)
                {
                    if (dr["Name"].ToString().IndexOf(" ") > 0)
                    {
                        MessageBox.Show(this, "Classes: " + Environment.NewLine + "Ensure that there are no spaces in the Class names");
                        return;
                    }
                }
                this.sqlClassAdapter.Update(dsMetaData1, "Class");
                this.progressBar1.Value++;
            }
            #endregion

            #region Update Fields
            // Update fields
            DataTable changedFieldRecords = dsMetaData1.Field.GetChanges();
            if (changedFieldRecords != null && changedFieldRecords.Rows.Count > 0)
            {
                // Ensure that none of the records have spaces in the field names
                foreach (DataRow dr in changedFieldRecords.Rows)
                {
                    if (dr["Name"].ToString().IndexOf(" ") > 0)
                    {
                        MessageBox.Show(this, "Fields: " + Environment.NewLine + "Ensure that there are no spaces in the Field names");
                        return;
                    }
                }
                this.sqlFieldAdapter.Update(dsMetaData1.Field);
                this.progressBar1.Value++;
            }
            #endregion

            #region Update Associations
            DataTable changedAssRecords = dsMetaData1.ClassAssociation.GetChanges();
            if (changedAssRecords != null && changedAssRecords.Rows.Count > 0)
            {
                sqlAssAdapter.AcceptChangesDuringUpdate = true;
                this.sqlAssAdapter.Update(dsMetaData1.ClassAssociation);

                this.progressBar1.Value++;
            }

            #endregion

            #region Update artifacts
            DataTable changedArtifactRecords = dsMetaData1.AllowedArtifact.GetChanges();
            if (changedArtifactRecords != null && changedArtifactRecords.Rows.Count > 0)
            {
                this.sqlAllowedArtifact.Update(dsMetaData1.AllowedArtifact);
                this.progressBar1.Value++;
            }
            #endregion

            this.statusBar1.Text = "DataTables Updated...";
            this.progressBar1.Value++;
        }

        private void gridMeta_RecordUpdated(object sender, EventArgs e)
        {
        }

        private void gridMeta_FormattingRow(object sender, RowLoadEventArgs e)
        {
        }

        private void gridMeta_RecordAdded(object sender, EventArgs e)
        {

        }

        private void gridMeta_GetNewRow(object sender, GetNewRowEventArgs e)
        {
            //e.NewRow.ToString();
            //try
            //{
            //    (e.NewRow as Janus.Windows.GridEX.GridEXRow).RowStyle.BackColor = System.Drawing.Color.AliceBlue;
            //}
            //catch
            //{
            //}
        }

    }
}