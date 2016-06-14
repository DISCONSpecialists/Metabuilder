
#region Using directives

using System;
using System.Collections;
using System.Collections.Specialized;


using System.Web.Configuration;
using System.Data;
using System.Data.Common;
using System.Configuration.Provider;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.DataAccessLayer.Bases;

#endregion

namespace MetaBuilder.DataAccessLayer.SqlClient
{
    /// <summary>
    /// This class is the Sql implementation of the NetTiersProvider.
    /// </summary>
    public sealed class SqlNetTiersProvider : MetaBuilder.DataAccessLayer.Bases.NetTiersProvider
    {
        private static object syncRoot = new Object();
        private string _applicationName;
        private string _connectionString;
        private bool _useStoredProcedure;
        string _providerInvariantName;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlNetTiersProvider"/> class.
        ///</summary>
        public SqlNetTiersProvider()
        {
        }

        /// <summary>
        /// Initializes the provider.
        /// </summary>
        /// <param name="name">The friendly name of the provider.</param>
        /// <param name="config">A collection of the name/value pairs representing the provider-specific attributes specified in the configuration for this provider.</param>
        /// <exception cref="T:System.ArgumentNullException">The name of the provider is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">An attempt is made to call <see cref="M:System.Configuration.Provider.ProviderBase.Initialize(System.String,System.Collections.Specialized.NameValueCollection)"></see> on a provider after the provider has already been initialized.</exception>
        /// <exception cref="T:System.ArgumentException">The name of the provider has a length of zero.</exception>
        public override void Initialize(string name, NameValueCollection config)
        {
            // Verify that config isn't null
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            // Assign the provider a default name if it doesn't have one
            if (String.IsNullOrEmpty(name))
            {
                name = "SqlNetTiersProvider";
            }

            // Add a default "description" attribute to config if the
            // attribute doesn't exist or is empty
            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "NetTiers Sql provider");
            }

            // Call the base class's Initialize method
            base.Initialize(name, config);

            // Initialize _applicationName
            _applicationName = config["applicationName"];

            if (string.IsNullOrEmpty(_applicationName))
            {
                _applicationName = "/";
            }
            config.Remove("applicationName");


            #region "Initialize UseStoredProcedure"
            string storedProcedure = config["useStoredProcedure"];
            if (string.IsNullOrEmpty(storedProcedure))
            {
                throw new ProviderException("Empty or missing useStoredProcedure");
            }
            this._useStoredProcedure = Convert.ToBoolean(config["useStoredProcedure"]);
            config.Remove("useStoredProcedure");
            #endregion

            #region ConnectionString

            // Initialize _connectionString
            _connectionString = config["connectionString"];
            config.Remove("connectionString");

            string connect = config["connectionStringName"];
            config.Remove("connectionStringName");

            if (String.IsNullOrEmpty(_connectionString))
            {
                if (String.IsNullOrEmpty(connect))
                {
                    throw new ProviderException("Empty or missing connectionStringName");
                }

                if (DataRepository.ConnectionStrings[connect] == null)
                {
                    throw new ProviderException("Missing connection string");
                }

                _connectionString = DataRepository.ConnectionStrings[connect].ConnectionString;
            }

            if (String.IsNullOrEmpty(_connectionString))
            {
                throw new ProviderException("Empty connection string");
            }

            #endregion

            #region "_providerInvariantName"

            // initialize _providerInvariantName
            this._providerInvariantName = config["providerInvariantName"];

            if (String.IsNullOrEmpty(_providerInvariantName))
            {
                throw new ProviderException("Empty or missing providerInvariantName");
            }
            config.Remove("providerInvariantName");

            #endregion

        }

        /// <summary>
        /// Creates a new <c cref="TransactionManager"/> instance from the current datasource.
        /// </summary>
        /// <returns></returns>
        public override TransactionManager CreateTransaction()
        {
            return new TransactionManager(this._connectionString);
        }

        /// <summary>
        /// Gets a value indicating whether to use stored procedure or not.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this repository use stored procedures; otherwise, <c>false</c>.
        /// </value>
        public bool UseStoredProcedure
        {
            get { return this._useStoredProcedure; }
            set { this._useStoredProcedure = value; }
        }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString
        {
            get { return this._connectionString; }
            set { this._connectionString = value; }
        }

        /// <summary>
        /// Gets or sets the invariant provider name listed in the DbProviderFactories machine.config section.
        /// </summary>
        /// <value>The name of the provider invariant.</value>
        public string ProviderInvariantName
        {
            get { return this._providerInvariantName; }
            set { this._providerInvariantName = value; }
        }

        ///<summary>
        /// Indicates if the current <c cref="NetTiersProvider"/> implementation supports Transacton.
        ///</summary>
        public override bool IsTransactionSupported
        {
            get
            {
                return true;
            }
        }


        #region "VCStatusProvider"

        private SqlVCStatusProvider innerSqlVCStatusProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="VCStatus"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override VCStatusProviderBase VCStatusProvider
        {
            get
            {
                if (innerSqlVCStatusProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlVCStatusProvider == null)
                        {
                            this.innerSqlVCStatusProvider = new SqlVCStatusProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlVCStatusProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlVCStatusProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlVCStatusProvider SqlVCStatusProvider
        {
            get { return VCStatusProvider as SqlVCStatusProvider; }
        }

        #endregion


        #region "DomainDefinitionPossibleValueProvider"

        private SqlDomainDefinitionPossibleValueProvider innerSqlDomainDefinitionPossibleValueProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="DomainDefinitionPossibleValue"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override DomainDefinitionPossibleValueProviderBase DomainDefinitionPossibleValueProvider
        {
            get
            {
                if (innerSqlDomainDefinitionPossibleValueProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlDomainDefinitionPossibleValueProvider == null)
                        {
                            this.innerSqlDomainDefinitionPossibleValueProvider = new SqlDomainDefinitionPossibleValueProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlDomainDefinitionPossibleValueProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlDomainDefinitionPossibleValueProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlDomainDefinitionPossibleValueProvider SqlDomainDefinitionPossibleValueProvider
        {
            get { return DomainDefinitionPossibleValueProvider as SqlDomainDefinitionPossibleValueProvider; }
        }

        #endregion


        #region "WorkspaceProvider"

        private SqlWorkspaceProvider innerSqlWorkspaceProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="Workspace"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override WorkspaceProviderBase WorkspaceProvider
        {
            get
            {
                if (innerSqlWorkspaceProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlWorkspaceProvider == null)
                        {
                            this.innerSqlWorkspaceProvider = new SqlWorkspaceProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlWorkspaceProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlWorkspaceProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlWorkspaceProvider SqlWorkspaceProvider
        {
            get { return WorkspaceProvider as SqlWorkspaceProvider; }
        }

        #endregion


        #region "ClassTypeProvider"

        private SqlClassTypeProvider innerSqlClassTypeProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="ClassType"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override ClassTypeProviderBase ClassTypeProvider
        {
            get
            {
                if (innerSqlClassTypeProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlClassTypeProvider == null)
                        {
                            this.innerSqlClassTypeProvider = new SqlClassTypeProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlClassTypeProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlClassTypeProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlClassTypeProvider SqlClassTypeProvider
        {
            get { return ClassTypeProvider as SqlClassTypeProvider; }
        }

        #endregion


        #region "WorkspaceTypeProvider"

        private SqlWorkspaceTypeProvider innerSqlWorkspaceTypeProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="WorkspaceType"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override WorkspaceTypeProviderBase WorkspaceTypeProvider
        {
            get
            {
                if (innerSqlWorkspaceTypeProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlWorkspaceTypeProvider == null)
                        {
                            this.innerSqlWorkspaceTypeProvider = new SqlWorkspaceTypeProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlWorkspaceTypeProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlWorkspaceTypeProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlWorkspaceTypeProvider SqlWorkspaceTypeProvider
        {
            get { return WorkspaceTypeProvider as SqlWorkspaceTypeProvider; }
        }

        #endregion


        #region "UserProvider"

        private SqlUserProvider innerSqlUserProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="User"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override UserProviderBase UserProvider
        {
            get
            {
                if (innerSqlUserProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlUserProvider == null)
                        {
                            this.innerSqlUserProvider = new SqlUserProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlUserProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlUserProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlUserProvider SqlUserProvider
        {
            get { return UserProvider as SqlUserProvider; }
        }

        #endregion


        #region "GraphFileProvider"

        private SqlGraphFileProvider innerSqlGraphFileProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="GraphFile"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override GraphFileProviderBase GraphFileProvider
        {
            get
            {
                if (innerSqlGraphFileProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlGraphFileProvider == null)
                        {
                            this.innerSqlGraphFileProvider = new SqlGraphFileProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlGraphFileProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlGraphFileProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlGraphFileProvider SqlGraphFileProvider
        {
            get { return GraphFileProvider as SqlGraphFileProvider; }
        }

        #endregion


        #region "ClassProvider"

        private SqlClassProvider innerSqlClassProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="Class"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override ClassProviderBase ClassProvider
        {
            get
            {
                if (innerSqlClassProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlClassProvider == null)
                        {
                            this.innerSqlClassProvider = new SqlClassProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlClassProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlClassProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlClassProvider SqlClassProvider
        {
            get { return ClassProvider as SqlClassProvider; }
        }

        #endregion


        #region "MetaObjectProvider"

        private SqlMetaObjectProvider innerSqlMetaObjectProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="MetaObject"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override MetaObjectProviderBase MetaObjectProvider
        {
            get
            {
                if (innerSqlMetaObjectProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMetaObjectProvider == null)
                        {
                            this.innerSqlMetaObjectProvider = new SqlMetaObjectProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMetaObjectProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMetaObjectProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMetaObjectProvider SqlMetaObjectProvider
        {
            get { return MetaObjectProvider as SqlMetaObjectProvider; }
        }

        #endregion


        #region "UserPermissionProvider"

        private SqlUserPermissionProvider innerSqlUserPermissionProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="UserPermission"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override UserPermissionProviderBase UserPermissionProvider
        {
            get
            {
                if (innerSqlUserPermissionProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlUserPermissionProvider == null)
                        {
                            this.innerSqlUserPermissionProvider = new SqlUserPermissionProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlUserPermissionProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlUserPermissionProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlUserPermissionProvider SqlUserPermissionProvider
        {
            get { return UserPermissionProvider as SqlUserPermissionProvider; }
        }

        #endregion


        #region "ConfigProvider"

        private SqlConfigProvider innerSqlConfigProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="Config"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override ConfigProviderBase ConfigProvider
        {
            get
            {
                if (innerSqlConfigProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlConfigProvider == null)
                        {
                            this.innerSqlConfigProvider = new SqlConfigProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlConfigProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlConfigProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlConfigProvider SqlConfigProvider
        {
            get { return ConfigProvider as SqlConfigProvider; }
        }

        #endregion


        #region "AssociationTypeProvider"

        private SqlAssociationTypeProvider innerSqlAssociationTypeProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="AssociationType"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override AssociationTypeProviderBase AssociationTypeProvider
        {
            get
            {
                if (innerSqlAssociationTypeProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlAssociationTypeProvider == null)
                        {
                            this.innerSqlAssociationTypeProvider = new SqlAssociationTypeProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlAssociationTypeProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlAssociationTypeProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlAssociationTypeProvider SqlAssociationTypeProvider
        {
            get { return AssociationTypeProvider as SqlAssociationTypeProvider; }
        }

        #endregion


        #region "ClassAssociationProvider"

        private SqlClassAssociationProvider innerSqlClassAssociationProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="ClassAssociation"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override ClassAssociationProviderBase ClassAssociationProvider
        {
            get
            {
                if (innerSqlClassAssociationProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlClassAssociationProvider == null)
                        {
                            this.innerSqlClassAssociationProvider = new SqlClassAssociationProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlClassAssociationProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlClassAssociationProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlClassAssociationProvider SqlClassAssociationProvider
        {
            get { return ClassAssociationProvider as SqlClassAssociationProvider; }
        }

        #endregion


        #region "ObjectAssociationProvider"

        private SqlObjectAssociationProvider innerSqlObjectAssociationProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="ObjectAssociation"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override ObjectAssociationProviderBase ObjectAssociationProvider
        {
            get
            {
                if (innerSqlObjectAssociationProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlObjectAssociationProvider == null)
                        {
                            this.innerSqlObjectAssociationProvider = new SqlObjectAssociationProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlObjectAssociationProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlObjectAssociationProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlObjectAssociationProvider SqlObjectAssociationProvider
        {
            get { return ObjectAssociationProvider as SqlObjectAssociationProvider; }
        }

        #endregion


        #region "FileTypeProvider"

        private SqlFileTypeProvider innerSqlFileTypeProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="FileType"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override FileTypeProviderBase FileTypeProvider
        {
            get
            {
                if (innerSqlFileTypeProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlFileTypeProvider == null)
                        {
                            this.innerSqlFileTypeProvider = new SqlFileTypeProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlFileTypeProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlFileTypeProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlFileTypeProvider SqlFileTypeProvider
        {
            get { return FileTypeProvider as SqlFileTypeProvider; }
        }

        #endregion


        #region "ObjectFieldValueProvider"

        private SqlObjectFieldValueProvider innerSqlObjectFieldValueProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="ObjectFieldValue"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override ObjectFieldValueProviderBase ObjectFieldValueProvider
        {
            get
            {
                if (innerSqlObjectFieldValueProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlObjectFieldValueProvider == null)
                        {
                            this.innerSqlObjectFieldValueProvider = new SqlObjectFieldValueProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlObjectFieldValueProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlObjectFieldValueProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlObjectFieldValueProvider SqlObjectFieldValueProvider
        {
            get { return ObjectFieldValueProvider as SqlObjectFieldValueProvider; }
        }

        #endregion


        #region "GraphFileAssociationProvider"

        private SqlGraphFileAssociationProvider innerSqlGraphFileAssociationProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="GraphFileAssociation"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override GraphFileAssociationProviderBase GraphFileAssociationProvider
        {
            get
            {
                if (innerSqlGraphFileAssociationProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlGraphFileAssociationProvider == null)
                        {
                            this.innerSqlGraphFileAssociationProvider = new SqlGraphFileAssociationProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlGraphFileAssociationProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlGraphFileAssociationProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlGraphFileAssociationProvider SqlGraphFileAssociationProvider
        {
            get { return GraphFileAssociationProvider as SqlGraphFileAssociationProvider; }
        }

        #endregion


        #region "AllowedArtifactProvider"

        private SqlAllowedArtifactProvider innerSqlAllowedArtifactProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="AllowedArtifact"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override AllowedArtifactProviderBase AllowedArtifactProvider
        {
            get
            {
                if (innerSqlAllowedArtifactProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlAllowedArtifactProvider == null)
                        {
                            this.innerSqlAllowedArtifactProvider = new SqlAllowedArtifactProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlAllowedArtifactProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlAllowedArtifactProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlAllowedArtifactProvider SqlAllowedArtifactProvider
        {
            get { return AllowedArtifactProvider as SqlAllowedArtifactProvider; }
        }

        #endregion


        #region "DomainDefinitionProvider"

        private SqlDomainDefinitionProvider innerSqlDomainDefinitionProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="DomainDefinition"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override DomainDefinitionProviderBase DomainDefinitionProvider
        {
            get
            {
                if (innerSqlDomainDefinitionProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlDomainDefinitionProvider == null)
                        {
                            this.innerSqlDomainDefinitionProvider = new SqlDomainDefinitionProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlDomainDefinitionProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlDomainDefinitionProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlDomainDefinitionProvider SqlDomainDefinitionProvider
        {
            get { return DomainDefinitionProvider as SqlDomainDefinitionProvider; }
        }

        #endregion


        #region "ArtifactProvider"

        private SqlArtifactProvider innerSqlArtifactProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="Artifact"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override ArtifactProviderBase ArtifactProvider
        {
            get
            {
                if (innerSqlArtifactProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlArtifactProvider == null)
                        {
                            this.innerSqlArtifactProvider = new SqlArtifactProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlArtifactProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlArtifactProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlArtifactProvider SqlArtifactProvider
        {
            get { return ArtifactProvider as SqlArtifactProvider; }
        }

        #endregion


        #region "PermissionProvider"

        private SqlPermissionProvider innerSqlPermissionProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="Permission"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override PermissionProviderBase PermissionProvider
        {
            get
            {
                if (innerSqlPermissionProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlPermissionProvider == null)
                        {
                            this.innerSqlPermissionProvider = new SqlPermissionProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlPermissionProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlPermissionProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlPermissionProvider SqlPermissionProvider
        {
            get { return PermissionProvider as SqlPermissionProvider; }
        }

        #endregion


        #region "GraphFileObjectProvider"

        private SqlGraphFileObjectProvider innerSqlGraphFileObjectProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="GraphFileObject"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override GraphFileObjectProviderBase GraphFileObjectProvider
        {
            get
            {
                if (innerSqlGraphFileObjectProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlGraphFileObjectProvider == null)
                        {
                            this.innerSqlGraphFileObjectProvider = new SqlGraphFileObjectProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlGraphFileObjectProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlGraphFileObjectProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlGraphFileObjectProvider SqlGraphFileObjectProvider
        {
            get { return GraphFileObjectProvider as SqlGraphFileObjectProvider; }
        }

        #endregion


        #region "FieldProvider"

        private SqlFieldProvider innerSqlFieldProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="Field"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override FieldProviderBase FieldProvider
        {
            get
            {
                if (innerSqlFieldProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlFieldProvider == null)
                        {
                            this.innerSqlFieldProvider = new SqlFieldProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlFieldProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlFieldProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlFieldProvider SqlFieldProvider
        {
            get { return FieldProvider as SqlFieldProvider; }
        }

        #endregion


        #region "UURI PRovider"

        private SqlUURIProvider innerSqlUURIProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="Field"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override UURIProviderBase UURIProvider
        {
            get
            {
                if (innerSqlUURIProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlUURIProvider == null)
                        {
                            this.innerSqlUURIProvider = new SqlUURIProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlUURIProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlFieldProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlUURIProvider SqlUURIProvider
        {
            get { return UURIProvider as SqlUURIProvider; }
        }

        #endregion


        #region "METAView_Activity_ListingProvider"

        private SqlMETAView_Activity_ListingProvider innerSqlMETAView_Activity_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Activity_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Activity_ListingProviderBase METAView_Activity_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Activity_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Activity_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Activity_ListingProvider = new SqlMETAView_Activity_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Activity_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Activity_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Activity_ListingProvider SqlMETAView_Activity_ListingProvider
        {
            get { return METAView_Activity_ListingProvider as SqlMETAView_Activity_ListingProvider; }
        }

        #endregion


        #region "METAView_Activity_RetrievalProvider"

        private SqlMETAView_Activity_RetrievalProvider innerSqlMETAView_Activity_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Activity_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Activity_RetrievalProviderBase METAView_Activity_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Activity_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Activity_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Activity_RetrievalProvider = new SqlMETAView_Activity_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Activity_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Activity_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Activity_RetrievalProvider SqlMETAView_Activity_RetrievalProvider
        {
            get { return METAView_Activity_RetrievalProvider as SqlMETAView_Activity_RetrievalProvider; }
        }

        #endregion


        #region "METAView_ApplicationInterface_ListingProvider"

        private SqlMETAView_ApplicationInterface_ListingProvider innerSqlMETAView_ApplicationInterface_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_ApplicationInterface_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_ApplicationInterface_ListingProviderBase METAView_ApplicationInterface_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_ApplicationInterface_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_ApplicationInterface_ListingProvider == null)
                        {
                            this.innerSqlMETAView_ApplicationInterface_ListingProvider = new SqlMETAView_ApplicationInterface_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_ApplicationInterface_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_ApplicationInterface_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_ApplicationInterface_ListingProvider SqlMETAView_ApplicationInterface_ListingProvider
        {
            get { return METAView_ApplicationInterface_ListingProvider as SqlMETAView_ApplicationInterface_ListingProvider; }
        }

        #endregion


        #region "METAView_ApplicationInterface_RetrievalProvider"

        private SqlMETAView_ApplicationInterface_RetrievalProvider innerSqlMETAView_ApplicationInterface_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_ApplicationInterface_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_ApplicationInterface_RetrievalProviderBase METAView_ApplicationInterface_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_ApplicationInterface_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_ApplicationInterface_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_ApplicationInterface_RetrievalProvider = new SqlMETAView_ApplicationInterface_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_ApplicationInterface_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_ApplicationInterface_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_ApplicationInterface_RetrievalProvider SqlMETAView_ApplicationInterface_RetrievalProvider
        {
            get { return METAView_ApplicationInterface_RetrievalProvider as SqlMETAView_ApplicationInterface_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Assumption_ListingProvider"

        private SqlMETAView_Assumption_ListingProvider innerSqlMETAView_Assumption_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Assumption_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Assumption_ListingProviderBase METAView_Assumption_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Assumption_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Assumption_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Assumption_ListingProvider = new SqlMETAView_Assumption_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Assumption_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Assumption_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Assumption_ListingProvider SqlMETAView_Assumption_ListingProvider
        {
            get { return METAView_Assumption_ListingProvider as SqlMETAView_Assumption_ListingProvider; }
        }

        #endregion


        #region "METAView_Assumption_RetrievalProvider"

        private SqlMETAView_Assumption_RetrievalProvider innerSqlMETAView_Assumption_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Assumption_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Assumption_RetrievalProviderBase METAView_Assumption_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Assumption_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Assumption_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Assumption_RetrievalProvider = new SqlMETAView_Assumption_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Assumption_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Assumption_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Assumption_RetrievalProvider SqlMETAView_Assumption_RetrievalProvider
        {
            get { return METAView_Assumption_RetrievalProvider as SqlMETAView_Assumption_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Attribute_ListingProvider"

        private SqlMETAView_Attribute_ListingProvider innerSqlMETAView_Attribute_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Attribute_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Attribute_ListingProviderBase METAView_Attribute_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Attribute_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Attribute_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Attribute_ListingProvider = new SqlMETAView_Attribute_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Attribute_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Attribute_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Attribute_ListingProvider SqlMETAView_Attribute_ListingProvider
        {
            get { return METAView_Attribute_ListingProvider as SqlMETAView_Attribute_ListingProvider; }
        }

        #endregion


        #region "METAView_Attribute_RetrievalProvider"

        private SqlMETAView_Attribute_RetrievalProvider innerSqlMETAView_Attribute_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Attribute_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Attribute_RetrievalProviderBase METAView_Attribute_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Attribute_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Attribute_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Attribute_RetrievalProvider = new SqlMETAView_Attribute_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Attribute_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Attribute_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Attribute_RetrievalProvider SqlMETAView_Attribute_RetrievalProvider
        {
            get { return METAView_Attribute_RetrievalProvider as SqlMETAView_Attribute_RetrievalProvider; }
        }

        #endregion


        #region "METAView_BusinessCompetency_ListingProvider"

        private SqlMETAView_BusinessCompetency_ListingProvider innerSqlMETAView_BusinessCompetency_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_BusinessCompetency_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_BusinessCompetency_ListingProviderBase METAView_BusinessCompetency_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_BusinessCompetency_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_BusinessCompetency_ListingProvider == null)
                        {
                            this.innerSqlMETAView_BusinessCompetency_ListingProvider = new SqlMETAView_BusinessCompetency_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_BusinessCompetency_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_BusinessCompetency_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_BusinessCompetency_ListingProvider SqlMETAView_BusinessCompetency_ListingProvider
        {
            get { return METAView_BusinessCompetency_ListingProvider as SqlMETAView_BusinessCompetency_ListingProvider; }
        }

        #endregion


        #region "METAView_BusinessCompetency_RetrievalProvider"

        private SqlMETAView_BusinessCompetency_RetrievalProvider innerSqlMETAView_BusinessCompetency_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_BusinessCompetency_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_BusinessCompetency_RetrievalProviderBase METAView_BusinessCompetency_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_BusinessCompetency_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_BusinessCompetency_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_BusinessCompetency_RetrievalProvider = new SqlMETAView_BusinessCompetency_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_BusinessCompetency_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_BusinessCompetency_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_BusinessCompetency_RetrievalProvider SqlMETAView_BusinessCompetency_RetrievalProvider
        {
            get { return METAView_BusinessCompetency_RetrievalProvider as SqlMETAView_BusinessCompetency_RetrievalProvider; }
        }

        #endregion


        #region "METAView_BusinessInterface_ListingProvider"

        private SqlMETAView_BusinessInterface_ListingProvider innerSqlMETAView_BusinessInterface_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_BusinessInterface_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_BusinessInterface_ListingProviderBase METAView_BusinessInterface_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_BusinessInterface_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_BusinessInterface_ListingProvider == null)
                        {
                            this.innerSqlMETAView_BusinessInterface_ListingProvider = new SqlMETAView_BusinessInterface_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_BusinessInterface_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_BusinessInterface_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_BusinessInterface_ListingProvider SqlMETAView_BusinessInterface_ListingProvider
        {
            get { return METAView_BusinessInterface_ListingProvider as SqlMETAView_BusinessInterface_ListingProvider; }
        }

        #endregion


        #region "METAView_BusinessInterface_RetrievalProvider"

        private SqlMETAView_BusinessInterface_RetrievalProvider innerSqlMETAView_BusinessInterface_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_BusinessInterface_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_BusinessInterface_RetrievalProviderBase METAView_BusinessInterface_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_BusinessInterface_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_BusinessInterface_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_BusinessInterface_RetrievalProvider = new SqlMETAView_BusinessInterface_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_BusinessInterface_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_BusinessInterface_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_BusinessInterface_RetrievalProvider SqlMETAView_BusinessInterface_RetrievalProvider
        {
            get { return METAView_BusinessInterface_RetrievalProvider as SqlMETAView_BusinessInterface_RetrievalProvider; }
        }

        #endregion


        #region "METAView_CAD_ListingProvider"

        private SqlMETAView_CAD_ListingProvider innerSqlMETAView_CAD_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_CAD_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_CAD_ListingProviderBase METAView_CAD_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_CAD_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_CAD_ListingProvider == null)
                        {
                            this.innerSqlMETAView_CAD_ListingProvider = new SqlMETAView_CAD_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_CAD_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_CAD_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_CAD_ListingProvider SqlMETAView_CAD_ListingProvider
        {
            get { return METAView_CAD_ListingProvider as SqlMETAView_CAD_ListingProvider; }
        }

        #endregion


        #region "METAView_CAD_RetrievalProvider"

        private SqlMETAView_CAD_RetrievalProvider innerSqlMETAView_CAD_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_CAD_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_CAD_RetrievalProviderBase METAView_CAD_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_CAD_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_CAD_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_CAD_RetrievalProvider = new SqlMETAView_CAD_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_CAD_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_CAD_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_CAD_RetrievalProvider SqlMETAView_CAD_RetrievalProvider
        {
            get { return METAView_CAD_RetrievalProvider as SqlMETAView_CAD_RetrievalProvider; }
        }

        #endregion


        #region "METAView_CADReal_ListingProvider"

        private SqlMETAView_CADReal_ListingProvider innerSqlMETAView_CADReal_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_CADReal_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_CADReal_ListingProviderBase METAView_CADReal_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_CADReal_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_CADReal_ListingProvider == null)
                        {
                            this.innerSqlMETAView_CADReal_ListingProvider = new SqlMETAView_CADReal_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_CADReal_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_CADReal_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_CADReal_ListingProvider SqlMETAView_CADReal_ListingProvider
        {
            get { return METAView_CADReal_ListingProvider as SqlMETAView_CADReal_ListingProvider; }
        }

        #endregion


        #region "METAView_CADReal_RetrievalProvider"

        private SqlMETAView_CADReal_RetrievalProvider innerSqlMETAView_CADReal_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_CADReal_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_CADReal_RetrievalProviderBase METAView_CADReal_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_CADReal_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_CADReal_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_CADReal_RetrievalProvider = new SqlMETAView_CADReal_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_CADReal_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_CADReal_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_CADReal_RetrievalProvider SqlMETAView_CADReal_RetrievalProvider
        {
            get { return METAView_CADReal_RetrievalProvider as SqlMETAView_CADReal_RetrievalProvider; }
        }

        #endregion


        #region "METAView_CategoryFactor_ListingProvider"

        private SqlMETAView_CategoryFactor_ListingProvider innerSqlMETAView_CategoryFactor_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_CategoryFactor_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_CategoryFactor_ListingProviderBase METAView_CategoryFactor_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_CategoryFactor_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_CategoryFactor_ListingProvider == null)
                        {
                            this.innerSqlMETAView_CategoryFactor_ListingProvider = new SqlMETAView_CategoryFactor_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_CategoryFactor_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_CategoryFactor_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_CategoryFactor_ListingProvider SqlMETAView_CategoryFactor_ListingProvider
        {
            get { return METAView_CategoryFactor_ListingProvider as SqlMETAView_CategoryFactor_ListingProvider; }
        }

        #endregion


        #region "METAView_CategoryFactor_RetrievalProvider"

        private SqlMETAView_CategoryFactor_RetrievalProvider innerSqlMETAView_CategoryFactor_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_CategoryFactor_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_CategoryFactor_RetrievalProviderBase METAView_CategoryFactor_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_CategoryFactor_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_CategoryFactor_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_CategoryFactor_RetrievalProvider = new SqlMETAView_CategoryFactor_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_CategoryFactor_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_CategoryFactor_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_CategoryFactor_RetrievalProvider SqlMETAView_CategoryFactor_RetrievalProvider
        {
            get { return METAView_CategoryFactor_RetrievalProvider as SqlMETAView_CategoryFactor_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Channel_ListingProvider"

        private SqlMETAView_Channel_ListingProvider innerSqlMETAView_Channel_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Channel_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Channel_ListingProviderBase METAView_Channel_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Channel_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Channel_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Channel_ListingProvider = new SqlMETAView_Channel_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Channel_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Channel_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Channel_ListingProvider SqlMETAView_Channel_ListingProvider
        {
            get { return METAView_Channel_ListingProvider as SqlMETAView_Channel_ListingProvider; }
        }

        #endregion


        #region "METAView_Channel_RetrievalProvider"

        private SqlMETAView_Channel_RetrievalProvider innerSqlMETAView_Channel_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Channel_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Channel_RetrievalProviderBase METAView_Channel_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Channel_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Channel_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Channel_RetrievalProvider = new SqlMETAView_Channel_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Channel_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Channel_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Channel_RetrievalProvider SqlMETAView_Channel_RetrievalProvider
        {
            get { return METAView_Channel_RetrievalProvider as SqlMETAView_Channel_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Competency_ListingProvider"

        private SqlMETAView_Competency_ListingProvider innerSqlMETAView_Competency_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Competency_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Competency_ListingProviderBase METAView_Competency_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Competency_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Competency_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Competency_ListingProvider = new SqlMETAView_Competency_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Competency_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Competency_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Competency_ListingProvider SqlMETAView_Competency_ListingProvider
        {
            get { return METAView_Competency_ListingProvider as SqlMETAView_Competency_ListingProvider; }
        }

        #endregion


        #region "METAView_Competency_RetrievalProvider"

        private SqlMETAView_Competency_RetrievalProvider innerSqlMETAView_Competency_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Competency_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Competency_RetrievalProviderBase METAView_Competency_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Competency_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Competency_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Competency_RetrievalProvider = new SqlMETAView_Competency_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Competency_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Competency_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Competency_RetrievalProvider SqlMETAView_Competency_RetrievalProvider
        {
            get { return METAView_Competency_RetrievalProvider as SqlMETAView_Competency_RetrievalProvider; }
        }

        #endregion


        #region "METAView_ComputingComponent_ListingProvider"

        private SqlMETAView_ComputingComponent_ListingProvider innerSqlMETAView_ComputingComponent_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_ComputingComponent_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_ComputingComponent_ListingProviderBase METAView_ComputingComponent_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_ComputingComponent_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_ComputingComponent_ListingProvider == null)
                        {
                            this.innerSqlMETAView_ComputingComponent_ListingProvider = new SqlMETAView_ComputingComponent_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_ComputingComponent_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_ComputingComponent_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_ComputingComponent_ListingProvider SqlMETAView_ComputingComponent_ListingProvider
        {
            get { return METAView_ComputingComponent_ListingProvider as SqlMETAView_ComputingComponent_ListingProvider; }
        }

        #endregion


        #region "METAView_ComputingComponent_RetrievalProvider"

        private SqlMETAView_ComputingComponent_RetrievalProvider innerSqlMETAView_ComputingComponent_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_ComputingComponent_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_ComputingComponent_RetrievalProviderBase METAView_ComputingComponent_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_ComputingComponent_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_ComputingComponent_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_ComputingComponent_RetrievalProvider = new SqlMETAView_ComputingComponent_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_ComputingComponent_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_ComputingComponent_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_ComputingComponent_RetrievalProvider SqlMETAView_ComputingComponent_RetrievalProvider
        {
            get { return METAView_ComputingComponent_RetrievalProvider as SqlMETAView_ComputingComponent_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Condition_ListingProvider"

        private SqlMETAView_Condition_ListingProvider innerSqlMETAView_Condition_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Condition_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Condition_ListingProviderBase METAView_Condition_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Condition_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Condition_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Condition_ListingProvider = new SqlMETAView_Condition_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Condition_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Condition_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Condition_ListingProvider SqlMETAView_Condition_ListingProvider
        {
            get { return METAView_Condition_ListingProvider as SqlMETAView_Condition_ListingProvider; }
        }

        #endregion


        #region "METAView_Condition_RetrievalProvider"

        private SqlMETAView_Condition_RetrievalProvider innerSqlMETAView_Condition_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Condition_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Condition_RetrievalProviderBase METAView_Condition_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Condition_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Condition_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Condition_RetrievalProvider = new SqlMETAView_Condition_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Condition_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Condition_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Condition_RetrievalProvider SqlMETAView_Condition_RetrievalProvider
        {
            get { return METAView_Condition_RetrievalProvider as SqlMETAView_Condition_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Conditional_ListingProvider"

        private SqlMETAView_Conditional_ListingProvider innerSqlMETAView_Conditional_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Conditional_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Conditional_ListingProviderBase METAView_Conditional_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Conditional_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Conditional_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Conditional_ListingProvider = new SqlMETAView_Conditional_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Conditional_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Conditional_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Conditional_ListingProvider SqlMETAView_Conditional_ListingProvider
        {
            get { return METAView_Conditional_ListingProvider as SqlMETAView_Conditional_ListingProvider; }
        }

        #endregion


        #region "METAView_Conditional_RetrievalProvider"

        private SqlMETAView_Conditional_RetrievalProvider innerSqlMETAView_Conditional_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Conditional_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Conditional_RetrievalProviderBase METAView_Conditional_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Conditional_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Conditional_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Conditional_RetrievalProvider = new SqlMETAView_Conditional_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Conditional_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Conditional_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Conditional_RetrievalProvider SqlMETAView_Conditional_RetrievalProvider
        {
            get { return METAView_Conditional_RetrievalProvider as SqlMETAView_Conditional_RetrievalProvider; }
        }

        #endregion


        #region "METAView_ConditionalDescription_ListingProvider"

        private SqlMETAView_ConditionalDescription_ListingProvider innerSqlMETAView_ConditionalDescription_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_ConditionalDescription_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_ConditionalDescription_ListingProviderBase METAView_ConditionalDescription_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_ConditionalDescription_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_ConditionalDescription_ListingProvider == null)
                        {
                            this.innerSqlMETAView_ConditionalDescription_ListingProvider = new SqlMETAView_ConditionalDescription_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_ConditionalDescription_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_ConditionalDescription_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_ConditionalDescription_ListingProvider SqlMETAView_ConditionalDescription_ListingProvider
        {
            get { return METAView_ConditionalDescription_ListingProvider as SqlMETAView_ConditionalDescription_ListingProvider; }
        }

        #endregion


        #region "METAView_ConditionalDescription_RetrievalProvider"

        private SqlMETAView_ConditionalDescription_RetrievalProvider innerSqlMETAView_ConditionalDescription_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_ConditionalDescription_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_ConditionalDescription_RetrievalProviderBase METAView_ConditionalDescription_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_ConditionalDescription_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_ConditionalDescription_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_ConditionalDescription_RetrievalProvider = new SqlMETAView_ConditionalDescription_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_ConditionalDescription_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_ConditionalDescription_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_ConditionalDescription_RetrievalProvider SqlMETAView_ConditionalDescription_RetrievalProvider
        {
            get { return METAView_ConditionalDescription_RetrievalProvider as SqlMETAView_ConditionalDescription_RetrievalProvider; }
        }

        #endregion


        #region "METAView_ConnectionSize_ListingProvider"

        private SqlMETAView_ConnectionSize_ListingProvider innerSqlMETAView_ConnectionSize_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_ConnectionSize_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_ConnectionSize_ListingProviderBase METAView_ConnectionSize_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_ConnectionSize_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_ConnectionSize_ListingProvider == null)
                        {
                            this.innerSqlMETAView_ConnectionSize_ListingProvider = new SqlMETAView_ConnectionSize_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_ConnectionSize_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_ConnectionSize_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_ConnectionSize_ListingProvider SqlMETAView_ConnectionSize_ListingProvider
        {
            get { return METAView_ConnectionSize_ListingProvider as SqlMETAView_ConnectionSize_ListingProvider; }
        }

        #endregion


        #region "METAView_ConnectionSize_RetrievalProvider"

        private SqlMETAView_ConnectionSize_RetrievalProvider innerSqlMETAView_ConnectionSize_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_ConnectionSize_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_ConnectionSize_RetrievalProviderBase METAView_ConnectionSize_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_ConnectionSize_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_ConnectionSize_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_ConnectionSize_RetrievalProvider = new SqlMETAView_ConnectionSize_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_ConnectionSize_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_ConnectionSize_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_ConnectionSize_RetrievalProvider SqlMETAView_ConnectionSize_RetrievalProvider
        {
            get { return METAView_ConnectionSize_RetrievalProvider as SqlMETAView_ConnectionSize_RetrievalProvider; }
        }

        #endregion


        #region "METAView_ConnectionSpeed_ListingProvider"

        private SqlMETAView_ConnectionSpeed_ListingProvider innerSqlMETAView_ConnectionSpeed_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_ConnectionSpeed_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_ConnectionSpeed_ListingProviderBase METAView_ConnectionSpeed_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_ConnectionSpeed_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_ConnectionSpeed_ListingProvider == null)
                        {
                            this.innerSqlMETAView_ConnectionSpeed_ListingProvider = new SqlMETAView_ConnectionSpeed_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_ConnectionSpeed_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_ConnectionSpeed_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_ConnectionSpeed_ListingProvider SqlMETAView_ConnectionSpeed_ListingProvider
        {
            get { return METAView_ConnectionSpeed_ListingProvider as SqlMETAView_ConnectionSpeed_ListingProvider; }
        }

        #endregion


        #region "METAView_ConnectionSpeed_RetrievalProvider"

        private SqlMETAView_ConnectionSpeed_RetrievalProvider innerSqlMETAView_ConnectionSpeed_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_ConnectionSpeed_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_ConnectionSpeed_RetrievalProviderBase METAView_ConnectionSpeed_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_ConnectionSpeed_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_ConnectionSpeed_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_ConnectionSpeed_RetrievalProvider = new SqlMETAView_ConnectionSpeed_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_ConnectionSpeed_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_ConnectionSpeed_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_ConnectionSpeed_RetrievalProvider SqlMETAView_ConnectionSpeed_RetrievalProvider
        {
            get { return METAView_ConnectionSpeed_RetrievalProvider as SqlMETAView_ConnectionSpeed_RetrievalProvider; }
        }

        #endregion


        #region "METAView_ConnectionType_ListingProvider"

        private SqlMETAView_ConnectionType_ListingProvider innerSqlMETAView_ConnectionType_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_ConnectionType_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_ConnectionType_ListingProviderBase METAView_ConnectionType_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_ConnectionType_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_ConnectionType_ListingProvider == null)
                        {
                            this.innerSqlMETAView_ConnectionType_ListingProvider = new SqlMETAView_ConnectionType_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_ConnectionType_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_ConnectionType_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_ConnectionType_ListingProvider SqlMETAView_ConnectionType_ListingProvider
        {
            get { return METAView_ConnectionType_ListingProvider as SqlMETAView_ConnectionType_ListingProvider; }
        }

        #endregion


        #region "METAView_ConnectionType_RetrievalProvider"

        private SqlMETAView_ConnectionType_RetrievalProvider innerSqlMETAView_ConnectionType_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_ConnectionType_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_ConnectionType_RetrievalProviderBase METAView_ConnectionType_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_ConnectionType_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_ConnectionType_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_ConnectionType_RetrievalProvider = new SqlMETAView_ConnectionType_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_ConnectionType_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_ConnectionType_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_ConnectionType_RetrievalProvider SqlMETAView_ConnectionType_RetrievalProvider
        {
            get { return METAView_ConnectionType_RetrievalProvider as SqlMETAView_ConnectionType_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Constraint_ListingProvider"

        private SqlMETAView_Constraint_ListingProvider innerSqlMETAView_Constraint_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Constraint_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Constraint_ListingProviderBase METAView_Constraint_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Constraint_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Constraint_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Constraint_ListingProvider = new SqlMETAView_Constraint_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Constraint_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Constraint_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Constraint_ListingProvider SqlMETAView_Constraint_ListingProvider
        {
            get { return METAView_Constraint_ListingProvider as SqlMETAView_Constraint_ListingProvider; }
        }

        #endregion


        #region "METAView_Constraint_RetrievalProvider"

        private SqlMETAView_Constraint_RetrievalProvider innerSqlMETAView_Constraint_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Constraint_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Constraint_RetrievalProviderBase METAView_Constraint_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Constraint_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Constraint_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Constraint_RetrievalProvider = new SqlMETAView_Constraint_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Constraint_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Constraint_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Constraint_RetrievalProvider SqlMETAView_Constraint_RetrievalProvider
        {
            get { return METAView_Constraint_RetrievalProvider as SqlMETAView_Constraint_RetrievalProvider; }
        }

        #endregion


        #region "METAView_CSF_ListingProvider"

        private SqlMETAView_CSF_ListingProvider innerSqlMETAView_CSF_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_CSF_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_CSF_ListingProviderBase METAView_CSF_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_CSF_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_CSF_ListingProvider == null)
                        {
                            this.innerSqlMETAView_CSF_ListingProvider = new SqlMETAView_CSF_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_CSF_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_CSF_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_CSF_ListingProvider SqlMETAView_CSF_ListingProvider
        {
            get { return METAView_CSF_ListingProvider as SqlMETAView_CSF_ListingProvider; }
        }

        #endregion


        #region "METAView_CSF_RetrievalProvider"

        private SqlMETAView_CSF_RetrievalProvider innerSqlMETAView_CSF_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_CSF_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_CSF_RetrievalProviderBase METAView_CSF_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_CSF_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_CSF_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_CSF_RetrievalProvider = new SqlMETAView_CSF_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_CSF_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_CSF_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_CSF_RetrievalProvider SqlMETAView_CSF_RetrievalProvider
        {
            get { return METAView_CSF_RetrievalProvider as SqlMETAView_CSF_RetrievalProvider; }
        }

        #endregion


        #region "METAView_DataAttribute_ListingProvider"

        private SqlMETAView_DataAttribute_ListingProvider innerSqlMETAView_DataAttribute_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_DataAttribute_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_DataAttribute_ListingProviderBase METAView_DataAttribute_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_DataAttribute_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_DataAttribute_ListingProvider == null)
                        {
                            this.innerSqlMETAView_DataAttribute_ListingProvider = new SqlMETAView_DataAttribute_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_DataAttribute_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_DataAttribute_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_DataAttribute_ListingProvider SqlMETAView_DataAttribute_ListingProvider
        {
            get { return METAView_DataAttribute_ListingProvider as SqlMETAView_DataAttribute_ListingProvider; }
        }

        #endregion


        #region "METAView_DataAttribute_RetrievalProvider"

        private SqlMETAView_DataAttribute_RetrievalProvider innerSqlMETAView_DataAttribute_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_DataAttribute_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_DataAttribute_RetrievalProviderBase METAView_DataAttribute_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_DataAttribute_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_DataAttribute_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_DataAttribute_RetrievalProvider = new SqlMETAView_DataAttribute_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_DataAttribute_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_DataAttribute_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_DataAttribute_RetrievalProvider SqlMETAView_DataAttribute_RetrievalProvider
        {
            get { return METAView_DataAttribute_RetrievalProvider as SqlMETAView_DataAttribute_RetrievalProvider; }
        }

        #endregion


        #region "METAView_DataColumn_ListingProvider"

        private SqlMETAView_DataColumn_ListingProvider innerSqlMETAView_DataColumn_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_DataColumn_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_DataColumn_ListingProviderBase METAView_DataColumn_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_DataColumn_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_DataColumn_ListingProvider == null)
                        {
                            this.innerSqlMETAView_DataColumn_ListingProvider = new SqlMETAView_DataColumn_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_DataColumn_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_DataColumn_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_DataColumn_ListingProvider SqlMETAView_DataColumn_ListingProvider
        {
            get { return METAView_DataColumn_ListingProvider as SqlMETAView_DataColumn_ListingProvider; }
        }

        #endregion


        #region "METAView_DataColumn_RetrievalProvider"

        private SqlMETAView_DataColumn_RetrievalProvider innerSqlMETAView_DataColumn_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_DataColumn_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_DataColumn_RetrievalProviderBase METAView_DataColumn_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_DataColumn_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_DataColumn_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_DataColumn_RetrievalProvider = new SqlMETAView_DataColumn_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_DataColumn_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_DataColumn_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_DataColumn_RetrievalProvider SqlMETAView_DataColumn_RetrievalProvider
        {
            get { return METAView_DataColumn_RetrievalProvider as SqlMETAView_DataColumn_RetrievalProvider; }
        }

        #endregion


        #region "METAView_DataDomain_ListingProvider"

        private SqlMETAView_DataDomain_ListingProvider innerSqlMETAView_DataDomain_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_DataDomain_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_DataDomain_ListingProviderBase METAView_DataDomain_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_DataDomain_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_DataDomain_ListingProvider == null)
                        {
                            this.innerSqlMETAView_DataDomain_ListingProvider = new SqlMETAView_DataDomain_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_DataDomain_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_DataDomain_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_DataDomain_ListingProvider SqlMETAView_DataDomain_ListingProvider
        {
            get { return METAView_DataDomain_ListingProvider as SqlMETAView_DataDomain_ListingProvider; }
        }

        #endregion


        #region "METAView_DataDomain_RetrievalProvider"

        private SqlMETAView_DataDomain_RetrievalProvider innerSqlMETAView_DataDomain_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_DataDomain_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_DataDomain_RetrievalProviderBase METAView_DataDomain_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_DataDomain_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_DataDomain_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_DataDomain_RetrievalProvider = new SqlMETAView_DataDomain_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_DataDomain_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_DataDomain_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_DataDomain_RetrievalProvider SqlMETAView_DataDomain_RetrievalProvider
        {
            get { return METAView_DataDomain_RetrievalProvider as SqlMETAView_DataDomain_RetrievalProvider; }
        }

        #endregion


        #region "METAView_DataEntity_ListingProvider"

        private SqlMETAView_DataEntity_ListingProvider innerSqlMETAView_DataEntity_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_DataEntity_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_DataEntity_ListingProviderBase METAView_DataEntity_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_DataEntity_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_DataEntity_ListingProvider == null)
                        {
                            this.innerSqlMETAView_DataEntity_ListingProvider = new SqlMETAView_DataEntity_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_DataEntity_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_DataEntity_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_DataEntity_ListingProvider SqlMETAView_DataEntity_ListingProvider
        {
            get { return METAView_DataEntity_ListingProvider as SqlMETAView_DataEntity_ListingProvider; }
        }

        #endregion


        #region "METAView_DataEntity_RetrievalProvider"

        private SqlMETAView_DataEntity_RetrievalProvider innerSqlMETAView_DataEntity_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_DataEntity_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_DataEntity_RetrievalProviderBase METAView_DataEntity_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_DataEntity_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_DataEntity_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_DataEntity_RetrievalProvider = new SqlMETAView_DataEntity_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_DataEntity_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_DataEntity_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_DataEntity_RetrievalProvider SqlMETAView_DataEntity_RetrievalProvider
        {
            get { return METAView_DataEntity_RetrievalProvider as SqlMETAView_DataEntity_RetrievalProvider; }
        }

        #endregion


        #region "METAView_DataField_ListingProvider"

        private SqlMETAView_DataField_ListingProvider innerSqlMETAView_DataField_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_DataField_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_DataField_ListingProviderBase METAView_DataField_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_DataField_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_DataField_ListingProvider == null)
                        {
                            this.innerSqlMETAView_DataField_ListingProvider = new SqlMETAView_DataField_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_DataField_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_DataField_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_DataField_ListingProvider SqlMETAView_DataField_ListingProvider
        {
            get { return METAView_DataField_ListingProvider as SqlMETAView_DataField_ListingProvider; }
        }

        #endregion


        #region "METAView_DataField_RetrievalProvider"

        private SqlMETAView_DataField_RetrievalProvider innerSqlMETAView_DataField_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_DataField_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_DataField_RetrievalProviderBase METAView_DataField_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_DataField_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_DataField_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_DataField_RetrievalProvider = new SqlMETAView_DataField_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_DataField_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_DataField_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_DataField_RetrievalProvider SqlMETAView_DataField_RetrievalProvider
        {
            get { return METAView_DataField_RetrievalProvider as SqlMETAView_DataField_RetrievalProvider; }
        }

        #endregion


        #region "METAView_DataFlowIndicator_ListingProvider"

        private SqlMETAView_DataFlowIndicator_ListingProvider innerSqlMETAView_DataFlowIndicator_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_DataFlowIndicator_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_DataFlowIndicator_ListingProviderBase METAView_DataFlowIndicator_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_DataFlowIndicator_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_DataFlowIndicator_ListingProvider == null)
                        {
                            this.innerSqlMETAView_DataFlowIndicator_ListingProvider = new SqlMETAView_DataFlowIndicator_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_DataFlowIndicator_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_DataFlowIndicator_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_DataFlowIndicator_ListingProvider SqlMETAView_DataFlowIndicator_ListingProvider
        {
            get { return METAView_DataFlowIndicator_ListingProvider as SqlMETAView_DataFlowIndicator_ListingProvider; }
        }

        #endregion


        #region "METAView_DataFlowIndicator_RetrievalProvider"

        private SqlMETAView_DataFlowIndicator_RetrievalProvider innerSqlMETAView_DataFlowIndicator_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_DataFlowIndicator_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_DataFlowIndicator_RetrievalProviderBase METAView_DataFlowIndicator_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_DataFlowIndicator_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_DataFlowIndicator_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_DataFlowIndicator_RetrievalProvider = new SqlMETAView_DataFlowIndicator_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_DataFlowIndicator_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_DataFlowIndicator_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_DataFlowIndicator_RetrievalProvider SqlMETAView_DataFlowIndicator_RetrievalProvider
        {
            get { return METAView_DataFlowIndicator_RetrievalProvider as SqlMETAView_DataFlowIndicator_RetrievalProvider; }
        }

        #endregion


        #region "METAView_DataSchema_ListingProvider"

        private SqlMETAView_DataSchema_ListingProvider innerSqlMETAView_DataSchema_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_DataSchema_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_DataSchema_ListingProviderBase METAView_DataSchema_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_DataSchema_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_DataSchema_ListingProvider == null)
                        {
                            this.innerSqlMETAView_DataSchema_ListingProvider = new SqlMETAView_DataSchema_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_DataSchema_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_DataSchema_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_DataSchema_ListingProvider SqlMETAView_DataSchema_ListingProvider
        {
            get { return METAView_DataSchema_ListingProvider as SqlMETAView_DataSchema_ListingProvider; }
        }

        #endregion


        #region "METAView_DataSchema_RetrievalProvider"

        private SqlMETAView_DataSchema_RetrievalProvider innerSqlMETAView_DataSchema_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_DataSchema_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_DataSchema_RetrievalProviderBase METAView_DataSchema_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_DataSchema_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_DataSchema_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_DataSchema_RetrievalProvider = new SqlMETAView_DataSchema_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_DataSchema_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_DataSchema_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_DataSchema_RetrievalProvider SqlMETAView_DataSchema_RetrievalProvider
        {
            get { return METAView_DataSchema_RetrievalProvider as SqlMETAView_DataSchema_RetrievalProvider; }
        }

        #endregion


        #region "METAView_DataSubjectArea_ListingProvider"

        private SqlMETAView_DataSubjectArea_ListingProvider innerSqlMETAView_DataSubjectArea_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_DataSubjectArea_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_DataSubjectArea_ListingProviderBase METAView_DataSubjectArea_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_DataSubjectArea_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_DataSubjectArea_ListingProvider == null)
                        {
                            this.innerSqlMETAView_DataSubjectArea_ListingProvider = new SqlMETAView_DataSubjectArea_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_DataSubjectArea_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_DataSubjectArea_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_DataSubjectArea_ListingProvider SqlMETAView_DataSubjectArea_ListingProvider
        {
            get { return METAView_DataSubjectArea_ListingProvider as SqlMETAView_DataSubjectArea_ListingProvider; }
        }

        #endregion


        #region "METAView_DataSubjectArea_RetrievalProvider"

        private SqlMETAView_DataSubjectArea_RetrievalProvider innerSqlMETAView_DataSubjectArea_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_DataSubjectArea_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_DataSubjectArea_RetrievalProviderBase METAView_DataSubjectArea_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_DataSubjectArea_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_DataSubjectArea_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_DataSubjectArea_RetrievalProvider = new SqlMETAView_DataSubjectArea_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_DataSubjectArea_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_DataSubjectArea_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_DataSubjectArea_RetrievalProvider SqlMETAView_DataSubjectArea_RetrievalProvider
        {
            get { return METAView_DataSubjectArea_RetrievalProvider as SqlMETAView_DataSubjectArea_RetrievalProvider; }
        }

        #endregion


        #region "METAView_DataTable_ListingProvider"

        private SqlMETAView_DataTable_ListingProvider innerSqlMETAView_DataTable_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_DataTable_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_DataTable_ListingProviderBase METAView_DataTable_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_DataTable_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_DataTable_ListingProvider == null)
                        {
                            this.innerSqlMETAView_DataTable_ListingProvider = new SqlMETAView_DataTable_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_DataTable_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_DataTable_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_DataTable_ListingProvider SqlMETAView_DataTable_ListingProvider
        {
            get { return METAView_DataTable_ListingProvider as SqlMETAView_DataTable_ListingProvider; }
        }

        #endregion


        #region "METAView_DataTable_RetrievalProvider"

        private SqlMETAView_DataTable_RetrievalProvider innerSqlMETAView_DataTable_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_DataTable_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_DataTable_RetrievalProviderBase METAView_DataTable_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_DataTable_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_DataTable_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_DataTable_RetrievalProvider = new SqlMETAView_DataTable_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_DataTable_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_DataTable_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_DataTable_RetrievalProvider SqlMETAView_DataTable_RetrievalProvider
        {
            get { return METAView_DataTable_RetrievalProvider as SqlMETAView_DataTable_RetrievalProvider; }
        }

        #endregion


        #region "METAView_DataValue_ListingProvider"

        private SqlMETAView_DataValue_ListingProvider innerSqlMETAView_DataValue_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_DataValue_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_DataValue_ListingProviderBase METAView_DataValue_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_DataValue_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_DataValue_ListingProvider == null)
                        {
                            this.innerSqlMETAView_DataValue_ListingProvider = new SqlMETAView_DataValue_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_DataValue_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_DataValue_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_DataValue_ListingProvider SqlMETAView_DataValue_ListingProvider
        {
            get { return METAView_DataValue_ListingProvider as SqlMETAView_DataValue_ListingProvider; }
        }

        #endregion


        #region "METAView_DataValue_RetrievalProvider"

        private SqlMETAView_DataValue_RetrievalProvider innerSqlMETAView_DataValue_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_DataValue_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_DataValue_RetrievalProviderBase METAView_DataValue_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_DataValue_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_DataValue_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_DataValue_RetrievalProvider = new SqlMETAView_DataValue_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_DataValue_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_DataValue_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_DataValue_RetrievalProvider SqlMETAView_DataValue_RetrievalProvider
        {
            get { return METAView_DataValue_RetrievalProvider as SqlMETAView_DataValue_RetrievalProvider; }
        }

        #endregion


        #region "METAView_DataView_ListingProvider"

        private SqlMETAView_DataView_ListingProvider innerSqlMETAView_DataView_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_DataView_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_DataView_ListingProviderBase METAView_DataView_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_DataView_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_DataView_ListingProvider == null)
                        {
                            this.innerSqlMETAView_DataView_ListingProvider = new SqlMETAView_DataView_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_DataView_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_DataView_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_DataView_ListingProvider SqlMETAView_DataView_ListingProvider
        {
            get { return METAView_DataView_ListingProvider as SqlMETAView_DataView_ListingProvider; }
        }

        #endregion


        #region "METAView_DataView_RetrievalProvider"

        private SqlMETAView_DataView_RetrievalProvider innerSqlMETAView_DataView_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_DataView_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_DataView_RetrievalProviderBase METAView_DataView_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_DataView_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_DataView_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_DataView_RetrievalProvider = new SqlMETAView_DataView_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_DataView_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_DataView_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_DataView_RetrievalProvider SqlMETAView_DataView_RetrievalProvider
        {
            get { return METAView_DataView_RetrievalProvider as SqlMETAView_DataView_RetrievalProvider; }
        }

        #endregion


        #region "METAView_DatedResponsibility_ListingProvider"

        private SqlMETAView_DatedResponsibility_ListingProvider innerSqlMETAView_DatedResponsibility_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_DatedResponsibility_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_DatedResponsibility_ListingProviderBase METAView_DatedResponsibility_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_DatedResponsibility_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_DatedResponsibility_ListingProvider == null)
                        {
                            this.innerSqlMETAView_DatedResponsibility_ListingProvider = new SqlMETAView_DatedResponsibility_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_DatedResponsibility_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_DatedResponsibility_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_DatedResponsibility_ListingProvider SqlMETAView_DatedResponsibility_ListingProvider
        {
            get { return METAView_DatedResponsibility_ListingProvider as SqlMETAView_DatedResponsibility_ListingProvider; }
        }

        #endregion


        #region "METAView_DatedResponsibility_RetrievalProvider"

        private SqlMETAView_DatedResponsibility_RetrievalProvider innerSqlMETAView_DatedResponsibility_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_DatedResponsibility_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_DatedResponsibility_RetrievalProviderBase METAView_DatedResponsibility_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_DatedResponsibility_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_DatedResponsibility_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_DatedResponsibility_RetrievalProvider = new SqlMETAView_DatedResponsibility_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_DatedResponsibility_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_DatedResponsibility_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_DatedResponsibility_RetrievalProvider SqlMETAView_DatedResponsibility_RetrievalProvider
        {
            get { return METAView_DatedResponsibility_RetrievalProvider as SqlMETAView_DatedResponsibility_RetrievalProvider; }
        }

        #endregion


        #region "METAView_DependencyDescription_ListingProvider"

        private SqlMETAView_DependencyDescription_ListingProvider innerSqlMETAView_DependencyDescription_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_DependencyDescription_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_DependencyDescription_ListingProviderBase METAView_DependencyDescription_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_DependencyDescription_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_DependencyDescription_ListingProvider == null)
                        {
                            this.innerSqlMETAView_DependencyDescription_ListingProvider = new SqlMETAView_DependencyDescription_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_DependencyDescription_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_DependencyDescription_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_DependencyDescription_ListingProvider SqlMETAView_DependencyDescription_ListingProvider
        {
            get { return METAView_DependencyDescription_ListingProvider as SqlMETAView_DependencyDescription_ListingProvider; }
        }

        #endregion


        #region "METAView_DependencyDescription_RetrievalProvider"

        private SqlMETAView_DependencyDescription_RetrievalProvider innerSqlMETAView_DependencyDescription_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_DependencyDescription_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_DependencyDescription_RetrievalProviderBase METAView_DependencyDescription_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_DependencyDescription_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_DependencyDescription_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_DependencyDescription_RetrievalProvider = new SqlMETAView_DependencyDescription_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_DependencyDescription_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_DependencyDescription_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_DependencyDescription_RetrievalProvider SqlMETAView_DependencyDescription_RetrievalProvider
        {
            get { return METAView_DependencyDescription_RetrievalProvider as SqlMETAView_DependencyDescription_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Driver_ListingProvider"

        private SqlMETAView_Driver_ListingProvider innerSqlMETAView_Driver_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Driver_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Driver_ListingProviderBase METAView_Driver_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Driver_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Driver_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Driver_ListingProvider = new SqlMETAView_Driver_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Driver_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Driver_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Driver_ListingProvider SqlMETAView_Driver_ListingProvider
        {
            get { return METAView_Driver_ListingProvider as SqlMETAView_Driver_ListingProvider; }
        }

        #endregion


        #region "METAView_Driver_RetrievalProvider"

        private SqlMETAView_Driver_RetrievalProvider innerSqlMETAView_Driver_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Driver_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Driver_RetrievalProviderBase METAView_Driver_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Driver_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Driver_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Driver_RetrievalProvider = new SqlMETAView_Driver_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Driver_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Driver_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Driver_RetrievalProvider SqlMETAView_Driver_RetrievalProvider
        {
            get { return METAView_Driver_RetrievalProvider as SqlMETAView_Driver_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Employee_ListingProvider"

        private SqlMETAView_Employee_ListingProvider innerSqlMETAView_Employee_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Employee_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Employee_ListingProviderBase METAView_Employee_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Employee_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Employee_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Employee_ListingProvider = new SqlMETAView_Employee_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Employee_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Employee_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Employee_ListingProvider SqlMETAView_Employee_ListingProvider
        {
            get { return METAView_Employee_ListingProvider as SqlMETAView_Employee_ListingProvider; }
        }

        #endregion


        #region "METAView_Employee_RetrievalProvider"

        private SqlMETAView_Employee_RetrievalProvider innerSqlMETAView_Employee_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Employee_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Employee_RetrievalProviderBase METAView_Employee_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Employee_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Employee_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Employee_RetrievalProvider = new SqlMETAView_Employee_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Employee_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Employee_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Employee_RetrievalProvider SqlMETAView_Employee_RetrievalProvider
        {
            get { return METAView_Employee_RetrievalProvider as SqlMETAView_Employee_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Entity_ListingProvider"

        private SqlMETAView_Entity_ListingProvider innerSqlMETAView_Entity_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Entity_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Entity_ListingProviderBase METAView_Entity_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Entity_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Entity_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Entity_ListingProvider = new SqlMETAView_Entity_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Entity_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Entity_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Entity_ListingProvider SqlMETAView_Entity_ListingProvider
        {
            get { return METAView_Entity_ListingProvider as SqlMETAView_Entity_ListingProvider; }
        }

        #endregion


        #region "METAView_Entity_RetrievalProvider"

        private SqlMETAView_Entity_RetrievalProvider innerSqlMETAView_Entity_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Entity_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Entity_RetrievalProviderBase METAView_Entity_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Entity_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Entity_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Entity_RetrievalProvider = new SqlMETAView_Entity_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Entity_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Entity_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Entity_RetrievalProvider SqlMETAView_Entity_RetrievalProvider
        {
            get { return METAView_Entity_RetrievalProvider as SqlMETAView_Entity_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Environment_ListingProvider"

        private SqlMETAView_Environment_ListingProvider innerSqlMETAView_Environment_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Environment_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Environment_ListingProviderBase METAView_Environment_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Environment_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Environment_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Environment_ListingProvider = new SqlMETAView_Environment_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Environment_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Environment_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Environment_ListingProvider SqlMETAView_Environment_ListingProvider
        {
            get { return METAView_Environment_ListingProvider as SqlMETAView_Environment_ListingProvider; }
        }

        #endregion


        #region "METAView_Environment_RetrievalProvider"

        private SqlMETAView_Environment_RetrievalProvider innerSqlMETAView_Environment_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Environment_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Environment_RetrievalProviderBase METAView_Environment_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Environment_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Environment_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Environment_RetrievalProvider = new SqlMETAView_Environment_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Environment_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Environment_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Environment_RetrievalProvider SqlMETAView_Environment_RetrievalProvider
        {
            get { return METAView_Environment_RetrievalProvider as SqlMETAView_Environment_RetrievalProvider; }
        }

        #endregion


        #region "METAView_EnvironmentCategory_ListingProvider"

        private SqlMETAView_EnvironmentCategory_ListingProvider innerSqlMETAView_EnvironmentCategory_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_EnvironmentCategory_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_EnvironmentCategory_ListingProviderBase METAView_EnvironmentCategory_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_EnvironmentCategory_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_EnvironmentCategory_ListingProvider == null)
                        {
                            this.innerSqlMETAView_EnvironmentCategory_ListingProvider = new SqlMETAView_EnvironmentCategory_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_EnvironmentCategory_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_EnvironmentCategory_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_EnvironmentCategory_ListingProvider SqlMETAView_EnvironmentCategory_ListingProvider
        {
            get { return METAView_EnvironmentCategory_ListingProvider as SqlMETAView_EnvironmentCategory_ListingProvider; }
        }

        #endregion


        #region "METAView_EnvironmentCategory_RetrievalProvider"

        private SqlMETAView_EnvironmentCategory_RetrievalProvider innerSqlMETAView_EnvironmentCategory_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_EnvironmentCategory_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_EnvironmentCategory_RetrievalProviderBase METAView_EnvironmentCategory_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_EnvironmentCategory_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_EnvironmentCategory_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_EnvironmentCategory_RetrievalProvider = new SqlMETAView_EnvironmentCategory_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_EnvironmentCategory_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_EnvironmentCategory_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_EnvironmentCategory_RetrievalProvider SqlMETAView_EnvironmentCategory_RetrievalProvider
        {
            get { return METAView_EnvironmentCategory_RetrievalProvider as SqlMETAView_EnvironmentCategory_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Event_ListingProvider"

        private SqlMETAView_Event_ListingProvider innerSqlMETAView_Event_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Event_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Event_ListingProviderBase METAView_Event_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Event_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Event_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Event_ListingProvider = new SqlMETAView_Event_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Event_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Event_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Event_ListingProvider SqlMETAView_Event_ListingProvider
        {
            get { return METAView_Event_ListingProvider as SqlMETAView_Event_ListingProvider; }
        }

        #endregion


        #region "METAView_Event_RetrievalProvider"

        private SqlMETAView_Event_RetrievalProvider innerSqlMETAView_Event_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Event_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Event_RetrievalProviderBase METAView_Event_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Event_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Event_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Event_RetrievalProvider = new SqlMETAView_Event_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Event_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Event_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Event_RetrievalProvider SqlMETAView_Event_RetrievalProvider
        {
            get { return METAView_Event_RetrievalProvider as SqlMETAView_Event_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Exception_ListingProvider"

        private SqlMETAView_Exception_ListingProvider innerSqlMETAView_Exception_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Exception_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Exception_ListingProviderBase METAView_Exception_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Exception_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Exception_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Exception_ListingProvider = new SqlMETAView_Exception_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Exception_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Exception_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Exception_ListingProvider SqlMETAView_Exception_ListingProvider
        {
            get { return METAView_Exception_ListingProvider as SqlMETAView_Exception_ListingProvider; }
        }

        #endregion


        #region "METAView_Exception_RetrievalProvider"

        private SqlMETAView_Exception_RetrievalProvider innerSqlMETAView_Exception_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Exception_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Exception_RetrievalProviderBase METAView_Exception_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Exception_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Exception_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Exception_RetrievalProvider = new SqlMETAView_Exception_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Exception_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Exception_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Exception_RetrievalProvider SqlMETAView_Exception_RetrievalProvider
        {
            get { return METAView_Exception_RetrievalProvider as SqlMETAView_Exception_RetrievalProvider; }
        }

        #endregion


        #region "METAView_ExtractionRule_ListingProvider"

        private SqlMETAView_ExtractionRule_ListingProvider innerSqlMETAView_ExtractionRule_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_ExtractionRule_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_ExtractionRule_ListingProviderBase METAView_ExtractionRule_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_ExtractionRule_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_ExtractionRule_ListingProvider == null)
                        {
                            this.innerSqlMETAView_ExtractionRule_ListingProvider = new SqlMETAView_ExtractionRule_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_ExtractionRule_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_ExtractionRule_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_ExtractionRule_ListingProvider SqlMETAView_ExtractionRule_ListingProvider
        {
            get { return METAView_ExtractionRule_ListingProvider as SqlMETAView_ExtractionRule_ListingProvider; }
        }

        #endregion


        #region "METAView_ExtractionRule_RetrievalProvider"

        private SqlMETAView_ExtractionRule_RetrievalProvider innerSqlMETAView_ExtractionRule_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_ExtractionRule_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_ExtractionRule_RetrievalProviderBase METAView_ExtractionRule_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_ExtractionRule_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_ExtractionRule_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_ExtractionRule_RetrievalProvider = new SqlMETAView_ExtractionRule_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_ExtractionRule_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_ExtractionRule_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_ExtractionRule_RetrievalProvider SqlMETAView_ExtractionRule_RetrievalProvider
        {
            get { return METAView_ExtractionRule_RetrievalProvider as SqlMETAView_ExtractionRule_RetrievalProvider; }
        }

        #endregion


        #region "METAView_FlowDescription_ListingProvider"

        private SqlMETAView_FlowDescription_ListingProvider innerSqlMETAView_FlowDescription_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_FlowDescription_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_FlowDescription_ListingProviderBase METAView_FlowDescription_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_FlowDescription_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_FlowDescription_ListingProvider == null)
                        {
                            this.innerSqlMETAView_FlowDescription_ListingProvider = new SqlMETAView_FlowDescription_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_FlowDescription_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_FlowDescription_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_FlowDescription_ListingProvider SqlMETAView_FlowDescription_ListingProvider
        {
            get { return METAView_FlowDescription_ListingProvider as SqlMETAView_FlowDescription_ListingProvider; }
        }

        #endregion


        #region "METAView_FlowDescription_RetrievalProvider"

        private SqlMETAView_FlowDescription_RetrievalProvider innerSqlMETAView_FlowDescription_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_FlowDescription_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_FlowDescription_RetrievalProviderBase METAView_FlowDescription_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_FlowDescription_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_FlowDescription_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_FlowDescription_RetrievalProvider = new SqlMETAView_FlowDescription_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_FlowDescription_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_FlowDescription_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_FlowDescription_RetrievalProvider SqlMETAView_FlowDescription_RetrievalProvider
        {
            get { return METAView_FlowDescription_RetrievalProvider as SqlMETAView_FlowDescription_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Function_ListingProvider"

        private SqlMETAView_Function_ListingProvider innerSqlMETAView_Function_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Function_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Function_ListingProviderBase METAView_Function_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Function_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Function_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Function_ListingProvider = new SqlMETAView_Function_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Function_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Function_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Function_ListingProvider SqlMETAView_Function_ListingProvider
        {
            get { return METAView_Function_ListingProvider as SqlMETAView_Function_ListingProvider; }
        }

        #endregion


        #region "METAView_Function_RetrievalProvider"

        private SqlMETAView_Function_RetrievalProvider innerSqlMETAView_Function_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Function_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Function_RetrievalProviderBase METAView_Function_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Function_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Function_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Function_RetrievalProvider = new SqlMETAView_Function_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Function_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Function_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Function_RetrievalProvider SqlMETAView_Function_RetrievalProvider
        {
            get { return METAView_Function_RetrievalProvider as SqlMETAView_Function_RetrievalProvider; }
        }

        #endregion


        #region "METAView_FunctionalDependency_ListingProvider"

        private SqlMETAView_FunctionalDependency_ListingProvider innerSqlMETAView_FunctionalDependency_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_FunctionalDependency_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_FunctionalDependency_ListingProviderBase METAView_FunctionalDependency_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_FunctionalDependency_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_FunctionalDependency_ListingProvider == null)
                        {
                            this.innerSqlMETAView_FunctionalDependency_ListingProvider = new SqlMETAView_FunctionalDependency_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_FunctionalDependency_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_FunctionalDependency_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_FunctionalDependency_ListingProvider SqlMETAView_FunctionalDependency_ListingProvider
        {
            get { return METAView_FunctionalDependency_ListingProvider as SqlMETAView_FunctionalDependency_ListingProvider; }
        }

        #endregion


        #region "METAView_FunctionalDependency_RetrievalProvider"

        private SqlMETAView_FunctionalDependency_RetrievalProvider innerSqlMETAView_FunctionalDependency_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_FunctionalDependency_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_FunctionalDependency_RetrievalProviderBase METAView_FunctionalDependency_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_FunctionalDependency_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_FunctionalDependency_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_FunctionalDependency_RetrievalProvider = new SqlMETAView_FunctionalDependency_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_FunctionalDependency_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_FunctionalDependency_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_FunctionalDependency_RetrievalProvider SqlMETAView_FunctionalDependency_RetrievalProvider
        {
            get { return METAView_FunctionalDependency_RetrievalProvider as SqlMETAView_FunctionalDependency_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Gateway_ListingProvider"

        private SqlMETAView_Gateway_ListingProvider innerSqlMETAView_Gateway_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Gateway_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Gateway_ListingProviderBase METAView_Gateway_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Gateway_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Gateway_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Gateway_ListingProvider = new SqlMETAView_Gateway_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Gateway_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Gateway_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Gateway_ListingProvider SqlMETAView_Gateway_ListingProvider
        {
            get { return METAView_Gateway_ListingProvider as SqlMETAView_Gateway_ListingProvider; }
        }

        #endregion


        #region "METAView_Gateway_RetrievalProvider"

        private SqlMETAView_Gateway_RetrievalProvider innerSqlMETAView_Gateway_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Gateway_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Gateway_RetrievalProviderBase METAView_Gateway_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Gateway_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Gateway_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Gateway_RetrievalProvider = new SqlMETAView_Gateway_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Gateway_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Gateway_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Gateway_RetrievalProvider SqlMETAView_Gateway_RetrievalProvider
        {
            get { return METAView_Gateway_RetrievalProvider as SqlMETAView_Gateway_RetrievalProvider; }
        }

        #endregion


        #region "METAView_GatewayDescription_ListingProvider"

        private SqlMETAView_GatewayDescription_ListingProvider innerSqlMETAView_GatewayDescription_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_GatewayDescription_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_GatewayDescription_ListingProviderBase METAView_GatewayDescription_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_GatewayDescription_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_GatewayDescription_ListingProvider == null)
                        {
                            this.innerSqlMETAView_GatewayDescription_ListingProvider = new SqlMETAView_GatewayDescription_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_GatewayDescription_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_GatewayDescription_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_GatewayDescription_ListingProvider SqlMETAView_GatewayDescription_ListingProvider
        {
            get { return METAView_GatewayDescription_ListingProvider as SqlMETAView_GatewayDescription_ListingProvider; }
        }

        #endregion


        #region "METAView_GatewayDescription_RetrievalProvider"

        private SqlMETAView_GatewayDescription_RetrievalProvider innerSqlMETAView_GatewayDescription_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_GatewayDescription_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_GatewayDescription_RetrievalProviderBase METAView_GatewayDescription_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_GatewayDescription_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_GatewayDescription_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_GatewayDescription_RetrievalProvider = new SqlMETAView_GatewayDescription_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_GatewayDescription_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_GatewayDescription_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_GatewayDescription_RetrievalProvider SqlMETAView_GatewayDescription_RetrievalProvider
        {
            get { return METAView_GatewayDescription_RetrievalProvider as SqlMETAView_GatewayDescription_RetrievalProvider; }
        }

        #endregion


        #region "METAView_GovernanceMechanism_ListingProvider"

        private SqlMETAView_GovernanceMechanism_ListingProvider innerSqlMETAView_GovernanceMechanism_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_GovernanceMechanism_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_GovernanceMechanism_ListingProviderBase METAView_GovernanceMechanism_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_GovernanceMechanism_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_GovernanceMechanism_ListingProvider == null)
                        {
                            this.innerSqlMETAView_GovernanceMechanism_ListingProvider = new SqlMETAView_GovernanceMechanism_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_GovernanceMechanism_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_GovernanceMechanism_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_GovernanceMechanism_ListingProvider SqlMETAView_GovernanceMechanism_ListingProvider
        {
            get { return METAView_GovernanceMechanism_ListingProvider as SqlMETAView_GovernanceMechanism_ListingProvider; }
        }

        #endregion


        #region "METAView_GovernanceMechanism_RetrievalProvider"

        private SqlMETAView_GovernanceMechanism_RetrievalProvider innerSqlMETAView_GovernanceMechanism_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_GovernanceMechanism_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_GovernanceMechanism_RetrievalProviderBase METAView_GovernanceMechanism_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_GovernanceMechanism_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_GovernanceMechanism_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_GovernanceMechanism_RetrievalProvider = new SqlMETAView_GovernanceMechanism_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_GovernanceMechanism_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_GovernanceMechanism_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_GovernanceMechanism_RetrievalProvider SqlMETAView_GovernanceMechanism_RetrievalProvider
        {
            get { return METAView_GovernanceMechanism_RetrievalProvider as SqlMETAView_GovernanceMechanism_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Implication_ListingProvider"

        private SqlMETAView_Implication_ListingProvider innerSqlMETAView_Implication_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Implication_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Implication_ListingProviderBase METAView_Implication_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Implication_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Implication_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Implication_ListingProvider = new SqlMETAView_Implication_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Implication_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Implication_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Implication_ListingProvider SqlMETAView_Implication_ListingProvider
        {
            get { return METAView_Implication_ListingProvider as SqlMETAView_Implication_ListingProvider; }
        }

        #endregion


        #region "METAView_Implication_RetrievalProvider"

        private SqlMETAView_Implication_RetrievalProvider innerSqlMETAView_Implication_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Implication_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Implication_RetrievalProviderBase METAView_Implication_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Implication_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Implication_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Implication_RetrievalProvider = new SqlMETAView_Implication_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Implication_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Implication_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Implication_RetrievalProvider SqlMETAView_Implication_RetrievalProvider
        {
            get { return METAView_Implication_RetrievalProvider as SqlMETAView_Implication_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Iteration_ListingProvider"

        private SqlMETAView_Iteration_ListingProvider innerSqlMETAView_Iteration_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Iteration_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Iteration_ListingProviderBase METAView_Iteration_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Iteration_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Iteration_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Iteration_ListingProvider = new SqlMETAView_Iteration_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Iteration_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Iteration_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Iteration_ListingProvider SqlMETAView_Iteration_ListingProvider
        {
            get { return METAView_Iteration_ListingProvider as SqlMETAView_Iteration_ListingProvider; }
        }

        #endregion


        #region "METAView_Iteration_RetrievalProvider"

        private SqlMETAView_Iteration_RetrievalProvider innerSqlMETAView_Iteration_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Iteration_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Iteration_RetrievalProviderBase METAView_Iteration_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Iteration_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Iteration_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Iteration_RetrievalProvider = new SqlMETAView_Iteration_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Iteration_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Iteration_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Iteration_RetrievalProvider SqlMETAView_Iteration_RetrievalProvider
        {
            get { return METAView_Iteration_RetrievalProvider as SqlMETAView_Iteration_RetrievalProvider; }
        }

        #endregion


        #region "METAView_ITInfrastructureEnvironment_ListingProvider"

        private SqlMETAView_ITInfrastructureEnvironment_ListingProvider innerSqlMETAView_ITInfrastructureEnvironment_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_ITInfrastructureEnvironment_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_ITInfrastructureEnvironment_ListingProviderBase METAView_ITInfrastructureEnvironment_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_ITInfrastructureEnvironment_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_ITInfrastructureEnvironment_ListingProvider == null)
                        {
                            this.innerSqlMETAView_ITInfrastructureEnvironment_ListingProvider = new SqlMETAView_ITInfrastructureEnvironment_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_ITInfrastructureEnvironment_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_ITInfrastructureEnvironment_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_ITInfrastructureEnvironment_ListingProvider SqlMETAView_ITInfrastructureEnvironment_ListingProvider
        {
            get { return METAView_ITInfrastructureEnvironment_ListingProvider as SqlMETAView_ITInfrastructureEnvironment_ListingProvider; }
        }

        #endregion


        #region "METAView_ITInfrastructureEnvironment_RetrievalProvider"

        private SqlMETAView_ITInfrastructureEnvironment_RetrievalProvider innerSqlMETAView_ITInfrastructureEnvironment_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_ITInfrastructureEnvironment_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_ITInfrastructureEnvironment_RetrievalProviderBase METAView_ITInfrastructureEnvironment_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_ITInfrastructureEnvironment_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_ITInfrastructureEnvironment_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_ITInfrastructureEnvironment_RetrievalProvider = new SqlMETAView_ITInfrastructureEnvironment_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_ITInfrastructureEnvironment_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_ITInfrastructureEnvironment_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_ITInfrastructureEnvironment_RetrievalProvider SqlMETAView_ITInfrastructureEnvironment_RetrievalProvider
        {
            get { return METAView_ITInfrastructureEnvironment_RetrievalProvider as SqlMETAView_ITInfrastructureEnvironment_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Job_ListingProvider"

        private SqlMETAView_Job_ListingProvider innerSqlMETAView_Job_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Job_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Job_ListingProviderBase METAView_Job_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Job_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Job_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Job_ListingProvider = new SqlMETAView_Job_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Job_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Job_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Job_ListingProvider SqlMETAView_Job_ListingProvider
        {
            get { return METAView_Job_ListingProvider as SqlMETAView_Job_ListingProvider; }
        }

        #endregion


        #region "METAView_Job_RetrievalProvider"

        private SqlMETAView_Job_RetrievalProvider innerSqlMETAView_Job_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Job_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Job_RetrievalProviderBase METAView_Job_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Job_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Job_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Job_RetrievalProvider = new SqlMETAView_Job_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Job_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Job_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Job_RetrievalProvider SqlMETAView_Job_RetrievalProvider
        {
            get { return METAView_Job_RetrievalProvider as SqlMETAView_Job_RetrievalProvider; }
        }

        #endregion


        #region "METAView_JobCompetency_ListingProvider"

        private SqlMETAView_JobCompetency_ListingProvider innerSqlMETAView_JobCompetency_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_JobCompetency_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_JobCompetency_ListingProviderBase METAView_JobCompetency_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_JobCompetency_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_JobCompetency_ListingProvider == null)
                        {
                            this.innerSqlMETAView_JobCompetency_ListingProvider = new SqlMETAView_JobCompetency_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_JobCompetency_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_JobCompetency_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_JobCompetency_ListingProvider SqlMETAView_JobCompetency_ListingProvider
        {
            get { return METAView_JobCompetency_ListingProvider as SqlMETAView_JobCompetency_ListingProvider; }
        }

        #endregion


        #region "METAView_JobCompetency_RetrievalProvider"

        private SqlMETAView_JobCompetency_RetrievalProvider innerSqlMETAView_JobCompetency_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_JobCompetency_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_JobCompetency_RetrievalProviderBase METAView_JobCompetency_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_JobCompetency_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_JobCompetency_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_JobCompetency_RetrievalProvider = new SqlMETAView_JobCompetency_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_JobCompetency_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_JobCompetency_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_JobCompetency_RetrievalProvider SqlMETAView_JobCompetency_RetrievalProvider
        {
            get { return METAView_JobCompetency_RetrievalProvider as SqlMETAView_JobCompetency_RetrievalProvider; }
        }

        #endregion


        #region "METAView_JobPosition_ListingProvider"

        private SqlMETAView_JobPosition_ListingProvider innerSqlMETAView_JobPosition_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_JobPosition_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_JobPosition_ListingProviderBase METAView_JobPosition_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_JobPosition_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_JobPosition_ListingProvider == null)
                        {
                            this.innerSqlMETAView_JobPosition_ListingProvider = new SqlMETAView_JobPosition_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_JobPosition_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_JobPosition_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_JobPosition_ListingProvider SqlMETAView_JobPosition_ListingProvider
        {
            get { return METAView_JobPosition_ListingProvider as SqlMETAView_JobPosition_ListingProvider; }
        }

        #endregion


        #region "METAView_JobPosition_RetrievalProvider"

        private SqlMETAView_JobPosition_RetrievalProvider innerSqlMETAView_JobPosition_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_JobPosition_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_JobPosition_RetrievalProviderBase METAView_JobPosition_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_JobPosition_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_JobPosition_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_JobPosition_RetrievalProvider = new SqlMETAView_JobPosition_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_JobPosition_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_JobPosition_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_JobPosition_RetrievalProvider SqlMETAView_JobPosition_RetrievalProvider
        {
            get { return METAView_JobPosition_RetrievalProvider as SqlMETAView_JobPosition_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Location_ListingProvider"

        private SqlMETAView_Location_ListingProvider innerSqlMETAView_Location_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Location_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Location_ListingProviderBase METAView_Location_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Location_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Location_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Location_ListingProvider = new SqlMETAView_Location_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Location_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Location_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Location_ListingProvider SqlMETAView_Location_ListingProvider
        {
            get { return METAView_Location_ListingProvider as SqlMETAView_Location_ListingProvider; }
        }

        #endregion


        #region "METAView_Location_RetrievalProvider"

        private SqlMETAView_Location_RetrievalProvider innerSqlMETAView_Location_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Location_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Location_RetrievalProviderBase METAView_Location_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Location_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Location_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Location_RetrievalProvider = new SqlMETAView_Location_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Location_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Location_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Location_RetrievalProvider SqlMETAView_Location_RetrievalProvider
        {
            get { return METAView_Location_RetrievalProvider as SqlMETAView_Location_RetrievalProvider; }
        }

        #endregion


        #region "METAView_LocationAssociation_ListingProvider"

        private SqlMETAView_LocationAssociation_ListingProvider innerSqlMETAView_LocationAssociation_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_LocationAssociation_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_LocationAssociation_ListingProviderBase METAView_LocationAssociation_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_LocationAssociation_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_LocationAssociation_ListingProvider == null)
                        {
                            this.innerSqlMETAView_LocationAssociation_ListingProvider = new SqlMETAView_LocationAssociation_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_LocationAssociation_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_LocationAssociation_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_LocationAssociation_ListingProvider SqlMETAView_LocationAssociation_ListingProvider
        {
            get { return METAView_LocationAssociation_ListingProvider as SqlMETAView_LocationAssociation_ListingProvider; }
        }

        #endregion


        #region "METAView_LocationAssociation_RetrievalProvider"

        private SqlMETAView_LocationAssociation_RetrievalProvider innerSqlMETAView_LocationAssociation_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_LocationAssociation_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_LocationAssociation_RetrievalProviderBase METAView_LocationAssociation_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_LocationAssociation_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_LocationAssociation_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_LocationAssociation_RetrievalProvider = new SqlMETAView_LocationAssociation_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_LocationAssociation_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_LocationAssociation_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_LocationAssociation_RetrievalProvider SqlMETAView_LocationAssociation_RetrievalProvider
        {
            get { return METAView_LocationAssociation_RetrievalProvider as SqlMETAView_LocationAssociation_RetrievalProvider; }
        }

        #endregion


        #region "METAView_LocationScheme_ListingProvider"

        private SqlMETAView_LocationScheme_ListingProvider innerSqlMETAView_LocationScheme_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_LocationScheme_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_LocationScheme_ListingProviderBase METAView_LocationScheme_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_LocationScheme_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_LocationScheme_ListingProvider == null)
                        {
                            this.innerSqlMETAView_LocationScheme_ListingProvider = new SqlMETAView_LocationScheme_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_LocationScheme_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_LocationScheme_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_LocationScheme_ListingProvider SqlMETAView_LocationScheme_ListingProvider
        {
            get { return METAView_LocationScheme_ListingProvider as SqlMETAView_LocationScheme_ListingProvider; }
        }

        #endregion


        #region "METAView_LocationScheme_RetrievalProvider"

        private SqlMETAView_LocationScheme_RetrievalProvider innerSqlMETAView_LocationScheme_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_LocationScheme_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_LocationScheme_RetrievalProviderBase METAView_LocationScheme_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_LocationScheme_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_LocationScheme_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_LocationScheme_RetrievalProvider = new SqlMETAView_LocationScheme_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_LocationScheme_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_LocationScheme_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_LocationScheme_RetrievalProvider SqlMETAView_LocationScheme_RetrievalProvider
        {
            get { return METAView_LocationScheme_RetrievalProvider as SqlMETAView_LocationScheme_RetrievalProvider; }
        }

        #endregion


        #region "METAView_LocationUnit_ListingProvider"

        private SqlMETAView_LocationUnit_ListingProvider innerSqlMETAView_LocationUnit_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_LocationUnit_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_LocationUnit_ListingProviderBase METAView_LocationUnit_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_LocationUnit_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_LocationUnit_ListingProvider == null)
                        {
                            this.innerSqlMETAView_LocationUnit_ListingProvider = new SqlMETAView_LocationUnit_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_LocationUnit_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_LocationUnit_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_LocationUnit_ListingProvider SqlMETAView_LocationUnit_ListingProvider
        {
            get { return METAView_LocationUnit_ListingProvider as SqlMETAView_LocationUnit_ListingProvider; }
        }

        #endregion


        #region "METAView_LocationUnit_RetrievalProvider"

        private SqlMETAView_LocationUnit_RetrievalProvider innerSqlMETAView_LocationUnit_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_LocationUnit_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_LocationUnit_RetrievalProviderBase METAView_LocationUnit_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_LocationUnit_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_LocationUnit_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_LocationUnit_RetrievalProvider = new SqlMETAView_LocationUnit_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_LocationUnit_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_LocationUnit_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_LocationUnit_RetrievalProvider SqlMETAView_LocationUnit_RetrievalProvider
        {
            get { return METAView_LocationUnit_RetrievalProvider as SqlMETAView_LocationUnit_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Logic_ListingProvider"

        private SqlMETAView_Logic_ListingProvider innerSqlMETAView_Logic_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Logic_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Logic_ListingProviderBase METAView_Logic_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Logic_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Logic_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Logic_ListingProvider = new SqlMETAView_Logic_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Logic_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Logic_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Logic_ListingProvider SqlMETAView_Logic_ListingProvider
        {
            get { return METAView_Logic_ListingProvider as SqlMETAView_Logic_ListingProvider; }
        }

        #endregion


        #region "METAView_Logic_RetrievalProvider"

        private SqlMETAView_Logic_RetrievalProvider innerSqlMETAView_Logic_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Logic_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Logic_RetrievalProviderBase METAView_Logic_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Logic_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Logic_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Logic_RetrievalProvider = new SqlMETAView_Logic_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Logic_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Logic_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Logic_RetrievalProvider SqlMETAView_Logic_RetrievalProvider
        {
            get { return METAView_Logic_RetrievalProvider as SqlMETAView_Logic_RetrievalProvider; }
        }

        #endregion


        #region "METAView_LogicalInformationArtefact_ListingProvider"

        private SqlMETAView_LogicalInformationArtefact_ListingProvider innerSqlMETAView_LogicalInformationArtefact_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_LogicalInformationArtefact_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_LogicalInformationArtefact_ListingProviderBase METAView_LogicalInformationArtefact_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_LogicalInformationArtefact_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_LogicalInformationArtefact_ListingProvider == null)
                        {
                            this.innerSqlMETAView_LogicalInformationArtefact_ListingProvider = new SqlMETAView_LogicalInformationArtefact_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_LogicalInformationArtefact_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_LogicalInformationArtefact_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_LogicalInformationArtefact_ListingProvider SqlMETAView_LogicalInformationArtefact_ListingProvider
        {
            get { return METAView_LogicalInformationArtefact_ListingProvider as SqlMETAView_LogicalInformationArtefact_ListingProvider; }
        }

        #endregion


        #region "METAView_LogicalInformationArtefact_RetrievalProvider"

        private SqlMETAView_LogicalInformationArtefact_RetrievalProvider innerSqlMETAView_LogicalInformationArtefact_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_LogicalInformationArtefact_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_LogicalInformationArtefact_RetrievalProviderBase METAView_LogicalInformationArtefact_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_LogicalInformationArtefact_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_LogicalInformationArtefact_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_LogicalInformationArtefact_RetrievalProvider = new SqlMETAView_LogicalInformationArtefact_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_LogicalInformationArtefact_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_LogicalInformationArtefact_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_LogicalInformationArtefact_RetrievalProvider SqlMETAView_LogicalInformationArtefact_RetrievalProvider
        {
            get { return METAView_LogicalInformationArtefact_RetrievalProvider as SqlMETAView_LogicalInformationArtefact_RetrievalProvider; }
        }

        #endregion


        #region "METAView_LogicalITInfrastructureComponent_ListingProvider"

        private SqlMETAView_LogicalITInfrastructureComponent_ListingProvider innerSqlMETAView_LogicalITInfrastructureComponent_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_LogicalITInfrastructureComponent_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_LogicalITInfrastructureComponent_ListingProviderBase METAView_LogicalITInfrastructureComponent_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_LogicalITInfrastructureComponent_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_LogicalITInfrastructureComponent_ListingProvider == null)
                        {
                            this.innerSqlMETAView_LogicalITInfrastructureComponent_ListingProvider = new SqlMETAView_LogicalITInfrastructureComponent_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_LogicalITInfrastructureComponent_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_LogicalITInfrastructureComponent_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_LogicalITInfrastructureComponent_ListingProvider SqlMETAView_LogicalITInfrastructureComponent_ListingProvider
        {
            get { return METAView_LogicalITInfrastructureComponent_ListingProvider as SqlMETAView_LogicalITInfrastructureComponent_ListingProvider; }
        }

        #endregion


        #region "METAView_LogicalITInfrastructureComponent_RetrievalProvider"

        private SqlMETAView_LogicalITInfrastructureComponent_RetrievalProvider innerSqlMETAView_LogicalITInfrastructureComponent_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_LogicalITInfrastructureComponent_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_LogicalITInfrastructureComponent_RetrievalProviderBase METAView_LogicalITInfrastructureComponent_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_LogicalITInfrastructureComponent_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_LogicalITInfrastructureComponent_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_LogicalITInfrastructureComponent_RetrievalProvider = new SqlMETAView_LogicalITInfrastructureComponent_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_LogicalITInfrastructureComponent_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_LogicalITInfrastructureComponent_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_LogicalITInfrastructureComponent_RetrievalProvider SqlMETAView_LogicalITInfrastructureComponent_RetrievalProvider
        {
            get { return METAView_LogicalITInfrastructureComponent_RetrievalProvider as SqlMETAView_LogicalITInfrastructureComponent_RetrievalProvider; }
        }

        #endregion


        #region "METAView_LogicalSoftwareComponent_ListingProvider"

        private SqlMETAView_LogicalSoftwareComponent_ListingProvider innerSqlMETAView_LogicalSoftwareComponent_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_LogicalSoftwareComponent_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_LogicalSoftwareComponent_ListingProviderBase METAView_LogicalSoftwareComponent_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_LogicalSoftwareComponent_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_LogicalSoftwareComponent_ListingProvider == null)
                        {
                            this.innerSqlMETAView_LogicalSoftwareComponent_ListingProvider = new SqlMETAView_LogicalSoftwareComponent_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_LogicalSoftwareComponent_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_LogicalSoftwareComponent_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_LogicalSoftwareComponent_ListingProvider SqlMETAView_LogicalSoftwareComponent_ListingProvider
        {
            get { return METAView_LogicalSoftwareComponent_ListingProvider as SqlMETAView_LogicalSoftwareComponent_ListingProvider; }
        }

        #endregion


        #region "METAView_LogicalSoftwareComponent_RetrievalProvider"

        private SqlMETAView_LogicalSoftwareComponent_RetrievalProvider innerSqlMETAView_LogicalSoftwareComponent_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_LogicalSoftwareComponent_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_LogicalSoftwareComponent_RetrievalProviderBase METAView_LogicalSoftwareComponent_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_LogicalSoftwareComponent_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_LogicalSoftwareComponent_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_LogicalSoftwareComponent_RetrievalProvider = new SqlMETAView_LogicalSoftwareComponent_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_LogicalSoftwareComponent_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_LogicalSoftwareComponent_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_LogicalSoftwareComponent_RetrievalProvider SqlMETAView_LogicalSoftwareComponent_RetrievalProvider
        {
            get { return METAView_LogicalSoftwareComponent_RetrievalProvider as SqlMETAView_LogicalSoftwareComponent_RetrievalProvider; }
        }

        #endregion


        #region "METAView_MarketSegment_ListingProvider"

        private SqlMETAView_MarketSegment_ListingProvider innerSqlMETAView_MarketSegment_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_MarketSegment_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_MarketSegment_ListingProviderBase METAView_MarketSegment_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_MarketSegment_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_MarketSegment_ListingProvider == null)
                        {
                            this.innerSqlMETAView_MarketSegment_ListingProvider = new SqlMETAView_MarketSegment_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_MarketSegment_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_MarketSegment_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_MarketSegment_ListingProvider SqlMETAView_MarketSegment_ListingProvider
        {
            get { return METAView_MarketSegment_ListingProvider as SqlMETAView_MarketSegment_ListingProvider; }
        }

        #endregion


        #region "METAView_MarketSegment_RetrievalProvider"

        private SqlMETAView_MarketSegment_RetrievalProvider innerSqlMETAView_MarketSegment_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_MarketSegment_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_MarketSegment_RetrievalProviderBase METAView_MarketSegment_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_MarketSegment_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_MarketSegment_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_MarketSegment_RetrievalProvider = new SqlMETAView_MarketSegment_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_MarketSegment_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_MarketSegment_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_MarketSegment_RetrievalProvider SqlMETAView_MarketSegment_RetrievalProvider
        {
            get { return METAView_MarketSegment_RetrievalProvider as SqlMETAView_MarketSegment_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Measure_ListingProvider"

        private SqlMETAView_Measure_ListingProvider innerSqlMETAView_Measure_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Measure_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Measure_ListingProviderBase METAView_Measure_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Measure_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Measure_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Measure_ListingProvider = new SqlMETAView_Measure_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Measure_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Measure_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Measure_ListingProvider SqlMETAView_Measure_ListingProvider
        {
            get { return METAView_Measure_ListingProvider as SqlMETAView_Measure_ListingProvider; }
        }

        #endregion


        #region "METAView_Measure_RetrievalProvider"

        private SqlMETAView_Measure_RetrievalProvider innerSqlMETAView_Measure_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Measure_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Measure_RetrievalProviderBase METAView_Measure_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Measure_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Measure_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Measure_RetrievalProvider = new SqlMETAView_Measure_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Measure_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Measure_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Measure_RetrievalProvider SqlMETAView_Measure_RetrievalProvider
        {
            get { return METAView_Measure_RetrievalProvider as SqlMETAView_Measure_RetrievalProvider; }
        }

        #endregion


        #region "METAView_MeasurementItem_ListingProvider"

        private SqlMETAView_MeasurementItem_ListingProvider innerSqlMETAView_MeasurementItem_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_MeasurementItem_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_MeasurementItem_ListingProviderBase METAView_MeasurementItem_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_MeasurementItem_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_MeasurementItem_ListingProvider == null)
                        {
                            this.innerSqlMETAView_MeasurementItem_ListingProvider = new SqlMETAView_MeasurementItem_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_MeasurementItem_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_MeasurementItem_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_MeasurementItem_ListingProvider SqlMETAView_MeasurementItem_ListingProvider
        {
            get { return METAView_MeasurementItem_ListingProvider as SqlMETAView_MeasurementItem_ListingProvider; }
        }

        #endregion


        #region "METAView_MeasurementItem_RetrievalProvider"

        private SqlMETAView_MeasurementItem_RetrievalProvider innerSqlMETAView_MeasurementItem_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_MeasurementItem_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_MeasurementItem_RetrievalProviderBase METAView_MeasurementItem_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_MeasurementItem_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_MeasurementItem_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_MeasurementItem_RetrievalProvider = new SqlMETAView_MeasurementItem_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_MeasurementItem_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_MeasurementItem_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_MeasurementItem_RetrievalProvider SqlMETAView_MeasurementItem_RetrievalProvider
        {
            get { return METAView_MeasurementItem_RetrievalProvider as SqlMETAView_MeasurementItem_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Model_ListingProvider"

        private SqlMETAView_Model_ListingProvider innerSqlMETAView_Model_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Model_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Model_ListingProviderBase METAView_Model_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Model_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Model_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Model_ListingProvider = new SqlMETAView_Model_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Model_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Model_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Model_ListingProvider SqlMETAView_Model_ListingProvider
        {
            get { return METAView_Model_ListingProvider as SqlMETAView_Model_ListingProvider; }
        }

        #endregion


        #region "METAView_Model_RetrievalProvider"

        private SqlMETAView_Model_RetrievalProvider innerSqlMETAView_Model_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Model_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Model_RetrievalProviderBase METAView_Model_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Model_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Model_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Model_RetrievalProvider = new SqlMETAView_Model_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Model_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Model_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Model_RetrievalProvider SqlMETAView_Model_RetrievalProvider
        {
            get { return METAView_Model_RetrievalProvider as SqlMETAView_Model_RetrievalProvider; }
        }

        #endregion


        #region "METAView_ModelReal_ListingProvider"

        private SqlMETAView_ModelReal_ListingProvider innerSqlMETAView_ModelReal_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_ModelReal_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_ModelReal_ListingProviderBase METAView_ModelReal_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_ModelReal_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_ModelReal_ListingProvider == null)
                        {
                            this.innerSqlMETAView_ModelReal_ListingProvider = new SqlMETAView_ModelReal_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_ModelReal_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_ModelReal_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_ModelReal_ListingProvider SqlMETAView_ModelReal_ListingProvider
        {
            get { return METAView_ModelReal_ListingProvider as SqlMETAView_ModelReal_ListingProvider; }
        }

        #endregion


        #region "METAView_ModelReal_RetrievalProvider"

        private SqlMETAView_ModelReal_RetrievalProvider innerSqlMETAView_ModelReal_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_ModelReal_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_ModelReal_RetrievalProviderBase METAView_ModelReal_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_ModelReal_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_ModelReal_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_ModelReal_RetrievalProvider = new SqlMETAView_ModelReal_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_ModelReal_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_ModelReal_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_ModelReal_RetrievalProvider SqlMETAView_ModelReal_RetrievalProvider
        {
            get { return METAView_ModelReal_RetrievalProvider as SqlMETAView_ModelReal_RetrievalProvider; }
        }

        #endregion


        #region "METAView_MutuallyExclusiveIndicator_ListingProvider"

        private SqlMETAView_MutuallyExclusiveIndicator_ListingProvider innerSqlMETAView_MutuallyExclusiveIndicator_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_MutuallyExclusiveIndicator_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_MutuallyExclusiveIndicator_ListingProviderBase METAView_MutuallyExclusiveIndicator_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_MutuallyExclusiveIndicator_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_MutuallyExclusiveIndicator_ListingProvider == null)
                        {
                            this.innerSqlMETAView_MutuallyExclusiveIndicator_ListingProvider = new SqlMETAView_MutuallyExclusiveIndicator_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_MutuallyExclusiveIndicator_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_MutuallyExclusiveIndicator_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_MutuallyExclusiveIndicator_ListingProvider SqlMETAView_MutuallyExclusiveIndicator_ListingProvider
        {
            get { return METAView_MutuallyExclusiveIndicator_ListingProvider as SqlMETAView_MutuallyExclusiveIndicator_ListingProvider; }
        }

        #endregion


        #region "METAView_MutuallyExclusiveIndicator_RetrievalProvider"

        private SqlMETAView_MutuallyExclusiveIndicator_RetrievalProvider innerSqlMETAView_MutuallyExclusiveIndicator_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_MutuallyExclusiveIndicator_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_MutuallyExclusiveIndicator_RetrievalProviderBase METAView_MutuallyExclusiveIndicator_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_MutuallyExclusiveIndicator_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_MutuallyExclusiveIndicator_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_MutuallyExclusiveIndicator_RetrievalProvider = new SqlMETAView_MutuallyExclusiveIndicator_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_MutuallyExclusiveIndicator_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_MutuallyExclusiveIndicator_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_MutuallyExclusiveIndicator_RetrievalProvider SqlMETAView_MutuallyExclusiveIndicator_RetrievalProvider
        {
            get { return METAView_MutuallyExclusiveIndicator_RetrievalProvider as SqlMETAView_MutuallyExclusiveIndicator_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Network_ListingProvider"

        private SqlMETAView_Network_ListingProvider innerSqlMETAView_Network_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Network_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Network_ListingProviderBase METAView_Network_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Network_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Network_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Network_ListingProvider = new SqlMETAView_Network_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Network_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Network_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Network_ListingProvider SqlMETAView_Network_ListingProvider
        {
            get { return METAView_Network_ListingProvider as SqlMETAView_Network_ListingProvider; }
        }

        #endregion


        #region "METAView_Network_RetrievalProvider"

        private SqlMETAView_Network_RetrievalProvider innerSqlMETAView_Network_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Network_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Network_RetrievalProviderBase METAView_Network_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Network_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Network_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Network_RetrievalProvider = new SqlMETAView_Network_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Network_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Network_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Network_RetrievalProvider SqlMETAView_Network_RetrievalProvider
        {
            get { return METAView_Network_RetrievalProvider as SqlMETAView_Network_RetrievalProvider; }
        }

        #endregion


        #region "METAView_NetworkComponent_ListingProvider"

        private SqlMETAView_NetworkComponent_ListingProvider innerSqlMETAView_NetworkComponent_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_NetworkComponent_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_NetworkComponent_ListingProviderBase METAView_NetworkComponent_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_NetworkComponent_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_NetworkComponent_ListingProvider == null)
                        {
                            this.innerSqlMETAView_NetworkComponent_ListingProvider = new SqlMETAView_NetworkComponent_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_NetworkComponent_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_NetworkComponent_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_NetworkComponent_ListingProvider SqlMETAView_NetworkComponent_ListingProvider
        {
            get { return METAView_NetworkComponent_ListingProvider as SqlMETAView_NetworkComponent_ListingProvider; }
        }

        #endregion


        #region "METAView_NetworkComponent_RetrievalProvider"

        private SqlMETAView_NetworkComponent_RetrievalProvider innerSqlMETAView_NetworkComponent_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_NetworkComponent_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_NetworkComponent_RetrievalProviderBase METAView_NetworkComponent_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_NetworkComponent_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_NetworkComponent_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_NetworkComponent_RetrievalProvider = new SqlMETAView_NetworkComponent_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_NetworkComponent_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_NetworkComponent_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_NetworkComponent_RetrievalProvider SqlMETAView_NetworkComponent_RetrievalProvider
        {
            get { return METAView_NetworkComponent_RetrievalProvider as SqlMETAView_NetworkComponent_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Object_ListingProvider"

        private SqlMETAView_Object_ListingProvider innerSqlMETAView_Object_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Object_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Object_ListingProviderBase METAView_Object_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Object_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Object_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Object_ListingProvider = new SqlMETAView_Object_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Object_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Object_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Object_ListingProvider SqlMETAView_Object_ListingProvider
        {
            get { return METAView_Object_ListingProvider as SqlMETAView_Object_ListingProvider; }
        }

        #endregion


        #region "METAView_Object_RetrievalProvider"

        private SqlMETAView_Object_RetrievalProvider innerSqlMETAView_Object_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Object_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Object_RetrievalProviderBase METAView_Object_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Object_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Object_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Object_RetrievalProvider = new SqlMETAView_Object_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Object_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Object_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Object_RetrievalProvider SqlMETAView_Object_RetrievalProvider
        {
            get { return METAView_Object_RetrievalProvider as SqlMETAView_Object_RetrievalProvider; }
        }

        #endregion


        #region "METAView_OrganizationalUnit_ListingProvider"

        private SqlMETAView_OrganizationalUnit_ListingProvider innerSqlMETAView_OrganizationalUnit_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_OrganizationalUnit_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_OrganizationalUnit_ListingProviderBase METAView_OrganizationalUnit_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_OrganizationalUnit_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_OrganizationalUnit_ListingProvider == null)
                        {
                            this.innerSqlMETAView_OrganizationalUnit_ListingProvider = new SqlMETAView_OrganizationalUnit_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_OrganizationalUnit_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_OrganizationalUnit_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_OrganizationalUnit_ListingProvider SqlMETAView_OrganizationalUnit_ListingProvider
        {
            get { return METAView_OrganizationalUnit_ListingProvider as SqlMETAView_OrganizationalUnit_ListingProvider; }
        }

        #endregion


        #region "METAView_OrganizationalUnit_RetrievalProvider"

        private SqlMETAView_OrganizationalUnit_RetrievalProvider innerSqlMETAView_OrganizationalUnit_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_OrganizationalUnit_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_OrganizationalUnit_RetrievalProviderBase METAView_OrganizationalUnit_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_OrganizationalUnit_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_OrganizationalUnit_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_OrganizationalUnit_RetrievalProvider = new SqlMETAView_OrganizationalUnit_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_OrganizationalUnit_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_OrganizationalUnit_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_OrganizationalUnit_RetrievalProvider SqlMETAView_OrganizationalUnit_RetrievalProvider
        {
            get { return METAView_OrganizationalUnit_RetrievalProvider as SqlMETAView_OrganizationalUnit_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Peripheral_ListingProvider"

        private SqlMETAView_Peripheral_ListingProvider innerSqlMETAView_Peripheral_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Peripheral_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Peripheral_ListingProviderBase METAView_Peripheral_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Peripheral_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Peripheral_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Peripheral_ListingProvider = new SqlMETAView_Peripheral_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Peripheral_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Peripheral_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Peripheral_ListingProvider SqlMETAView_Peripheral_ListingProvider
        {
            get { return METAView_Peripheral_ListingProvider as SqlMETAView_Peripheral_ListingProvider; }
        }

        #endregion


        #region "METAView_Peripheral_RetrievalProvider"

        private SqlMETAView_Peripheral_RetrievalProvider innerSqlMETAView_Peripheral_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Peripheral_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Peripheral_RetrievalProviderBase METAView_Peripheral_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Peripheral_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Peripheral_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Peripheral_RetrievalProvider = new SqlMETAView_Peripheral_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Peripheral_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Peripheral_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Peripheral_RetrievalProvider SqlMETAView_Peripheral_RetrievalProvider
        {
            get { return METAView_Peripheral_RetrievalProvider as SqlMETAView_Peripheral_RetrievalProvider; }
        }

        #endregion


        #region "METAView_PeripheralComponent_ListingProvider"

        private SqlMETAView_PeripheralComponent_ListingProvider innerSqlMETAView_PeripheralComponent_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_PeripheralComponent_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_PeripheralComponent_ListingProviderBase METAView_PeripheralComponent_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_PeripheralComponent_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_PeripheralComponent_ListingProvider == null)
                        {
                            this.innerSqlMETAView_PeripheralComponent_ListingProvider = new SqlMETAView_PeripheralComponent_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_PeripheralComponent_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_PeripheralComponent_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_PeripheralComponent_ListingProvider SqlMETAView_PeripheralComponent_ListingProvider
        {
            get { return METAView_PeripheralComponent_ListingProvider as SqlMETAView_PeripheralComponent_ListingProvider; }
        }

        #endregion


        #region "METAView_PeripheralComponent_RetrievalProvider"

        private SqlMETAView_PeripheralComponent_RetrievalProvider innerSqlMETAView_PeripheralComponent_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_PeripheralComponent_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_PeripheralComponent_RetrievalProviderBase METAView_PeripheralComponent_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_PeripheralComponent_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_PeripheralComponent_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_PeripheralComponent_RetrievalProvider = new SqlMETAView_PeripheralComponent_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_PeripheralComponent_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_PeripheralComponent_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_PeripheralComponent_RetrievalProvider SqlMETAView_PeripheralComponent_RetrievalProvider
        {
            get { return METAView_PeripheralComponent_RetrievalProvider as SqlMETAView_PeripheralComponent_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Person_ListingProvider"

        private SqlMETAView_Person_ListingProvider innerSqlMETAView_Person_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Person_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Person_ListingProviderBase METAView_Person_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Person_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Person_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Person_ListingProvider = new SqlMETAView_Person_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Person_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Person_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Person_ListingProvider SqlMETAView_Person_ListingProvider
        {
            get { return METAView_Person_ListingProvider as SqlMETAView_Person_ListingProvider; }
        }

        #endregion


        #region "METAView_Person_RetrievalProvider"

        private SqlMETAView_Person_RetrievalProvider innerSqlMETAView_Person_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Person_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Person_RetrievalProviderBase METAView_Person_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Person_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Person_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Person_RetrievalProvider = new SqlMETAView_Person_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Person_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Person_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Person_RetrievalProvider SqlMETAView_Person_RetrievalProvider
        {
            get { return METAView_Person_RetrievalProvider as SqlMETAView_Person_RetrievalProvider; }
        }

        #endregion


        #region "METAView_PhysicalDataComponent_ListingProvider"

        private SqlMETAView_PhysicalDataComponent_ListingProvider innerSqlMETAView_PhysicalDataComponent_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_PhysicalDataComponent_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_PhysicalDataComponent_ListingProviderBase METAView_PhysicalDataComponent_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_PhysicalDataComponent_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_PhysicalDataComponent_ListingProvider == null)
                        {
                            this.innerSqlMETAView_PhysicalDataComponent_ListingProvider = new SqlMETAView_PhysicalDataComponent_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_PhysicalDataComponent_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_PhysicalDataComponent_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_PhysicalDataComponent_ListingProvider SqlMETAView_PhysicalDataComponent_ListingProvider
        {
            get { return METAView_PhysicalDataComponent_ListingProvider as SqlMETAView_PhysicalDataComponent_ListingProvider; }
        }

        #endregion


        #region "METAView_PhysicalDataComponent_RetrievalProvider"

        private SqlMETAView_PhysicalDataComponent_RetrievalProvider innerSqlMETAView_PhysicalDataComponent_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_PhysicalDataComponent_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_PhysicalDataComponent_RetrievalProviderBase METAView_PhysicalDataComponent_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_PhysicalDataComponent_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_PhysicalDataComponent_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_PhysicalDataComponent_RetrievalProvider = new SqlMETAView_PhysicalDataComponent_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_PhysicalDataComponent_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_PhysicalDataComponent_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_PhysicalDataComponent_RetrievalProvider SqlMETAView_PhysicalDataComponent_RetrievalProvider
        {
            get { return METAView_PhysicalDataComponent_RetrievalProvider as SqlMETAView_PhysicalDataComponent_RetrievalProvider; }
        }

        #endregion


        #region "METAView_PhysicalInformationArtefact_ListingProvider"

        private SqlMETAView_PhysicalInformationArtefact_ListingProvider innerSqlMETAView_PhysicalInformationArtefact_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_PhysicalInformationArtefact_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_PhysicalInformationArtefact_ListingProviderBase METAView_PhysicalInformationArtefact_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_PhysicalInformationArtefact_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_PhysicalInformationArtefact_ListingProvider == null)
                        {
                            this.innerSqlMETAView_PhysicalInformationArtefact_ListingProvider = new SqlMETAView_PhysicalInformationArtefact_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_PhysicalInformationArtefact_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_PhysicalInformationArtefact_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_PhysicalInformationArtefact_ListingProvider SqlMETAView_PhysicalInformationArtefact_ListingProvider
        {
            get { return METAView_PhysicalInformationArtefact_ListingProvider as SqlMETAView_PhysicalInformationArtefact_ListingProvider; }
        }

        #endregion


        #region "METAView_PhysicalInformationArtefact_RetrievalProvider"

        private SqlMETAView_PhysicalInformationArtefact_RetrievalProvider innerSqlMETAView_PhysicalInformationArtefact_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_PhysicalInformationArtefact_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_PhysicalInformationArtefact_RetrievalProviderBase METAView_PhysicalInformationArtefact_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_PhysicalInformationArtefact_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_PhysicalInformationArtefact_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_PhysicalInformationArtefact_RetrievalProvider = new SqlMETAView_PhysicalInformationArtefact_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_PhysicalInformationArtefact_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_PhysicalInformationArtefact_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_PhysicalInformationArtefact_RetrievalProvider SqlMETAView_PhysicalInformationArtefact_RetrievalProvider
        {
            get { return METAView_PhysicalInformationArtefact_RetrievalProvider as SqlMETAView_PhysicalInformationArtefact_RetrievalProvider; }
        }

        #endregion


        #region "METAView_PhysicalSoftwareComponent_ListingProvider"

        private SqlMETAView_PhysicalSoftwareComponent_ListingProvider innerSqlMETAView_PhysicalSoftwareComponent_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_PhysicalSoftwareComponent_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_PhysicalSoftwareComponent_ListingProviderBase METAView_PhysicalSoftwareComponent_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_PhysicalSoftwareComponent_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_PhysicalSoftwareComponent_ListingProvider == null)
                        {
                            this.innerSqlMETAView_PhysicalSoftwareComponent_ListingProvider = new SqlMETAView_PhysicalSoftwareComponent_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_PhysicalSoftwareComponent_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_PhysicalSoftwareComponent_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_PhysicalSoftwareComponent_ListingProvider SqlMETAView_PhysicalSoftwareComponent_ListingProvider
        {
            get { return METAView_PhysicalSoftwareComponent_ListingProvider as SqlMETAView_PhysicalSoftwareComponent_ListingProvider; }
        }

        #endregion


        #region "METAView_PhysicalSoftwareComponent_RetrievalProvider"

        private SqlMETAView_PhysicalSoftwareComponent_RetrievalProvider innerSqlMETAView_PhysicalSoftwareComponent_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_PhysicalSoftwareComponent_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_PhysicalSoftwareComponent_RetrievalProviderBase METAView_PhysicalSoftwareComponent_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_PhysicalSoftwareComponent_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_PhysicalSoftwareComponent_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_PhysicalSoftwareComponent_RetrievalProvider = new SqlMETAView_PhysicalSoftwareComponent_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_PhysicalSoftwareComponent_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_PhysicalSoftwareComponent_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_PhysicalSoftwareComponent_RetrievalProvider SqlMETAView_PhysicalSoftwareComponent_RetrievalProvider
        {
            get { return METAView_PhysicalSoftwareComponent_RetrievalProvider as SqlMETAView_PhysicalSoftwareComponent_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Position_ListingProvider"

        private SqlMETAView_Position_ListingProvider innerSqlMETAView_Position_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Position_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Position_ListingProviderBase METAView_Position_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Position_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Position_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Position_ListingProvider = new SqlMETAView_Position_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Position_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Position_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Position_ListingProvider SqlMETAView_Position_ListingProvider
        {
            get { return METAView_Position_ListingProvider as SqlMETAView_Position_ListingProvider; }
        }

        #endregion


        #region "METAView_Position_RetrievalProvider"

        private SqlMETAView_Position_RetrievalProvider innerSqlMETAView_Position_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Position_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Position_RetrievalProviderBase METAView_Position_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Position_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Position_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Position_RetrievalProvider = new SqlMETAView_Position_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Position_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Position_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Position_RetrievalProvider SqlMETAView_Position_RetrievalProvider
        {
            get { return METAView_Position_RetrievalProvider as SqlMETAView_Position_RetrievalProvider; }
        }

        #endregion


        #region "METAView_PrimaryDataSourceIndicator_ListingProvider"

        private SqlMETAView_PrimaryDataSourceIndicator_ListingProvider innerSqlMETAView_PrimaryDataSourceIndicator_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_PrimaryDataSourceIndicator_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_PrimaryDataSourceIndicator_ListingProviderBase METAView_PrimaryDataSourceIndicator_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_PrimaryDataSourceIndicator_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_PrimaryDataSourceIndicator_ListingProvider == null)
                        {
                            this.innerSqlMETAView_PrimaryDataSourceIndicator_ListingProvider = new SqlMETAView_PrimaryDataSourceIndicator_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_PrimaryDataSourceIndicator_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_PrimaryDataSourceIndicator_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_PrimaryDataSourceIndicator_ListingProvider SqlMETAView_PrimaryDataSourceIndicator_ListingProvider
        {
            get { return METAView_PrimaryDataSourceIndicator_ListingProvider as SqlMETAView_PrimaryDataSourceIndicator_ListingProvider; }
        }

        #endregion


        #region "METAView_PrimaryDataSourceIndicator_RetrievalProvider"

        private SqlMETAView_PrimaryDataSourceIndicator_RetrievalProvider innerSqlMETAView_PrimaryDataSourceIndicator_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_PrimaryDataSourceIndicator_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_PrimaryDataSourceIndicator_RetrievalProviderBase METAView_PrimaryDataSourceIndicator_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_PrimaryDataSourceIndicator_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_PrimaryDataSourceIndicator_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_PrimaryDataSourceIndicator_RetrievalProvider = new SqlMETAView_PrimaryDataSourceIndicator_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_PrimaryDataSourceIndicator_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_PrimaryDataSourceIndicator_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_PrimaryDataSourceIndicator_RetrievalProvider SqlMETAView_PrimaryDataSourceIndicator_RetrievalProvider
        {
            get { return METAView_PrimaryDataSourceIndicator_RetrievalProvider as SqlMETAView_PrimaryDataSourceIndicator_RetrievalProvider; }
        }

        #endregion


        #region "METAView_ProbabilityofRealization_ListingProvider"

        private SqlMETAView_ProbabilityofRealization_ListingProvider innerSqlMETAView_ProbabilityofRealization_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_ProbabilityofRealization_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_ProbabilityofRealization_ListingProviderBase METAView_ProbabilityofRealization_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_ProbabilityofRealization_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_ProbabilityofRealization_ListingProvider == null)
                        {
                            this.innerSqlMETAView_ProbabilityofRealization_ListingProvider = new SqlMETAView_ProbabilityofRealization_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_ProbabilityofRealization_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_ProbabilityofRealization_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_ProbabilityofRealization_ListingProvider SqlMETAView_ProbabilityofRealization_ListingProvider
        {
            get { return METAView_ProbabilityofRealization_ListingProvider as SqlMETAView_ProbabilityofRealization_ListingProvider; }
        }

        #endregion


        #region "METAView_ProbabilityofRealization_RetrievalProvider"

        private SqlMETAView_ProbabilityofRealization_RetrievalProvider innerSqlMETAView_ProbabilityofRealization_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_ProbabilityofRealization_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_ProbabilityofRealization_RetrievalProviderBase METAView_ProbabilityofRealization_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_ProbabilityofRealization_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_ProbabilityofRealization_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_ProbabilityofRealization_RetrievalProvider = new SqlMETAView_ProbabilityofRealization_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_ProbabilityofRealization_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_ProbabilityofRealization_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_ProbabilityofRealization_RetrievalProvider SqlMETAView_ProbabilityofRealization_RetrievalProvider
        {
            get { return METAView_ProbabilityofRealization_RetrievalProvider as SqlMETAView_ProbabilityofRealization_RetrievalProvider; }
        }

        #endregion


        #region "METAView_ProbOfRealization_ListingProvider"

        private SqlMETAView_ProbOfRealization_ListingProvider innerSqlMETAView_ProbOfRealization_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_ProbOfRealization_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_ProbOfRealization_ListingProviderBase METAView_ProbOfRealization_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_ProbOfRealization_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_ProbOfRealization_ListingProvider == null)
                        {
                            this.innerSqlMETAView_ProbOfRealization_ListingProvider = new SqlMETAView_ProbOfRealization_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_ProbOfRealization_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_ProbOfRealization_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_ProbOfRealization_ListingProvider SqlMETAView_ProbOfRealization_ListingProvider
        {
            get { return METAView_ProbOfRealization_ListingProvider as SqlMETAView_ProbOfRealization_ListingProvider; }
        }

        #endregion


        #region "METAView_ProbOfRealization_RetrievalProvider"

        private SqlMETAView_ProbOfRealization_RetrievalProvider innerSqlMETAView_ProbOfRealization_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_ProbOfRealization_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_ProbOfRealization_RetrievalProviderBase METAView_ProbOfRealization_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_ProbOfRealization_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_ProbOfRealization_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_ProbOfRealization_RetrievalProvider = new SqlMETAView_ProbOfRealization_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_ProbOfRealization_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_ProbOfRealization_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_ProbOfRealization_RetrievalProvider SqlMETAView_ProbOfRealization_RetrievalProvider
        {
            get { return METAView_ProbOfRealization_RetrievalProvider as SqlMETAView_ProbOfRealization_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Process_ListingProvider"

        private SqlMETAView_Process_ListingProvider innerSqlMETAView_Process_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Process_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Process_ListingProviderBase METAView_Process_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Process_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Process_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Process_ListingProvider = new SqlMETAView_Process_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Process_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Process_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Process_ListingProvider SqlMETAView_Process_ListingProvider
        {
            get { return METAView_Process_ListingProvider as SqlMETAView_Process_ListingProvider; }
        }

        #endregion


        #region "METAView_Process_RetrievalProvider"

        private SqlMETAView_Process_RetrievalProvider innerSqlMETAView_Process_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Process_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Process_RetrievalProviderBase METAView_Process_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Process_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Process_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Process_RetrievalProvider = new SqlMETAView_Process_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Process_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Process_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Process_RetrievalProvider SqlMETAView_Process_RetrievalProvider
        {
            get { return METAView_Process_RetrievalProvider as SqlMETAView_Process_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Rationale_ListingProvider"

        private SqlMETAView_Rationale_ListingProvider innerSqlMETAView_Rationale_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Rationale_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Rationale_ListingProviderBase METAView_Rationale_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Rationale_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Rationale_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Rationale_ListingProvider = new SqlMETAView_Rationale_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Rationale_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Rationale_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Rationale_ListingProvider SqlMETAView_Rationale_ListingProvider
        {
            get { return METAView_Rationale_ListingProvider as SqlMETAView_Rationale_ListingProvider; }
        }

        #endregion


        #region "METAView_Rationale_RetrievalProvider"

        private SqlMETAView_Rationale_RetrievalProvider innerSqlMETAView_Rationale_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Rationale_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Rationale_RetrievalProviderBase METAView_Rationale_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Rationale_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Rationale_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Rationale_RetrievalProvider = new SqlMETAView_Rationale_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Rationale_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Rationale_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Rationale_RetrievalProvider SqlMETAView_Rationale_RetrievalProvider
        {
            get { return METAView_Rationale_RetrievalProvider as SqlMETAView_Rationale_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Report_ListingProvider"

        private SqlMETAView_Report_ListingProvider innerSqlMETAView_Report_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Report_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Report_ListingProviderBase METAView_Report_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Report_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Report_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Report_ListingProvider = new SqlMETAView_Report_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Report_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Report_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Report_ListingProvider SqlMETAView_Report_ListingProvider
        {
            get { return METAView_Report_ListingProvider as SqlMETAView_Report_ListingProvider; }
        }

        #endregion


        #region "METAView_Report_RetrievalProvider"

        private SqlMETAView_Report_RetrievalProvider innerSqlMETAView_Report_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Report_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Report_RetrievalProviderBase METAView_Report_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Report_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Report_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Report_RetrievalProvider = new SqlMETAView_Report_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Report_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Report_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Report_RetrievalProvider SqlMETAView_Report_RetrievalProvider
        {
            get { return METAView_Report_RetrievalProvider as SqlMETAView_Report_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Resource_ListingProvider"

        private SqlMETAView_Resource_ListingProvider innerSqlMETAView_Resource_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Resource_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Resource_ListingProviderBase METAView_Resource_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Resource_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Resource_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Resource_ListingProvider = new SqlMETAView_Resource_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Resource_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Resource_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Resource_ListingProvider SqlMETAView_Resource_ListingProvider
        {
            get { return METAView_Resource_ListingProvider as SqlMETAView_Resource_ListingProvider; }
        }

        #endregion


        #region "METAView_Resource_RetrievalProvider"

        private SqlMETAView_Resource_RetrievalProvider innerSqlMETAView_Resource_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Resource_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Resource_RetrievalProviderBase METAView_Resource_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Resource_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Resource_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Resource_RetrievalProvider = new SqlMETAView_Resource_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Resource_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Resource_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Resource_RetrievalProvider SqlMETAView_Resource_RetrievalProvider
        {
            get { return METAView_Resource_RetrievalProvider as SqlMETAView_Resource_RetrievalProvider; }
        }

        #endregion


        #region "METAView_ResourceType_ListingProvider"

        private SqlMETAView_ResourceType_ListingProvider innerSqlMETAView_ResourceType_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_ResourceType_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_ResourceType_ListingProviderBase METAView_ResourceType_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_ResourceType_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_ResourceType_ListingProvider == null)
                        {
                            this.innerSqlMETAView_ResourceType_ListingProvider = new SqlMETAView_ResourceType_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_ResourceType_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_ResourceType_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_ResourceType_ListingProvider SqlMETAView_ResourceType_ListingProvider
        {
            get { return METAView_ResourceType_ListingProvider as SqlMETAView_ResourceType_ListingProvider; }
        }

        #endregion


        #region "METAView_ResourceType_RetrievalProvider"

        private SqlMETAView_ResourceType_RetrievalProvider innerSqlMETAView_ResourceType_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_ResourceType_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_ResourceType_RetrievalProviderBase METAView_ResourceType_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_ResourceType_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_ResourceType_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_ResourceType_RetrievalProvider = new SqlMETAView_ResourceType_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_ResourceType_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_ResourceType_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_ResourceType_RetrievalProvider SqlMETAView_ResourceType_RetrievalProvider
        {
            get { return METAView_ResourceType_RetrievalProvider as SqlMETAView_ResourceType_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Responsibility_ListingProvider"

        private SqlMETAView_Responsibility_ListingProvider innerSqlMETAView_Responsibility_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Responsibility_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Responsibility_ListingProviderBase METAView_Responsibility_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Responsibility_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Responsibility_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Responsibility_ListingProvider = new SqlMETAView_Responsibility_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Responsibility_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Responsibility_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Responsibility_ListingProvider SqlMETAView_Responsibility_ListingProvider
        {
            get { return METAView_Responsibility_ListingProvider as SqlMETAView_Responsibility_ListingProvider; }
        }

        #endregion


        #region "METAView_Responsibility_RetrievalProvider"

        private SqlMETAView_Responsibility_RetrievalProvider innerSqlMETAView_Responsibility_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Responsibility_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Responsibility_RetrievalProviderBase METAView_Responsibility_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Responsibility_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Responsibility_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Responsibility_RetrievalProvider = new SqlMETAView_Responsibility_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Responsibility_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Responsibility_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Responsibility_RetrievalProvider SqlMETAView_Responsibility_RetrievalProvider
        {
            get { return METAView_Responsibility_RetrievalProvider as SqlMETAView_Responsibility_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Risk_ListingProvider"

        private SqlMETAView_Risk_ListingProvider innerSqlMETAView_Risk_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Risk_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Risk_ListingProviderBase METAView_Risk_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Risk_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Risk_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Risk_ListingProvider = new SqlMETAView_Risk_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Risk_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Risk_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Risk_ListingProvider SqlMETAView_Risk_ListingProvider
        {
            get { return METAView_Risk_ListingProvider as SqlMETAView_Risk_ListingProvider; }
        }

        #endregion


        #region "METAView_Risk_RetrievalProvider"

        private SqlMETAView_Risk_RetrievalProvider innerSqlMETAView_Risk_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Risk_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Risk_RetrievalProviderBase METAView_Risk_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Risk_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Risk_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Risk_RetrievalProvider = new SqlMETAView_Risk_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Risk_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Risk_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Risk_RetrievalProvider SqlMETAView_Risk_RetrievalProvider
        {
            get { return METAView_Risk_RetrievalProvider as SqlMETAView_Risk_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Role_ListingProvider"

        private SqlMETAView_Role_ListingProvider innerSqlMETAView_Role_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Role_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Role_ListingProviderBase METAView_Role_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Role_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Role_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Role_ListingProvider = new SqlMETAView_Role_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Role_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Role_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Role_ListingProvider SqlMETAView_Role_ListingProvider
        {
            get { return METAView_Role_ListingProvider as SqlMETAView_Role_ListingProvider; }
        }

        #endregion


        #region "METAView_Role_RetrievalProvider"

        private SqlMETAView_Role_RetrievalProvider innerSqlMETAView_Role_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Role_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Role_RetrievalProviderBase METAView_Role_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Role_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Role_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Role_RetrievalProvider = new SqlMETAView_Role_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Role_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Role_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Role_RetrievalProvider SqlMETAView_Role_RetrievalProvider
        {
            get { return METAView_Role_RetrievalProvider as SqlMETAView_Role_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Scenario_ListingProvider"

        private SqlMETAView_Scenario_ListingProvider innerSqlMETAView_Scenario_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Scenario_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Scenario_ListingProviderBase METAView_Scenario_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Scenario_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Scenario_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Scenario_ListingProvider = new SqlMETAView_Scenario_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Scenario_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Scenario_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Scenario_ListingProvider SqlMETAView_Scenario_ListingProvider
        {
            get { return METAView_Scenario_ListingProvider as SqlMETAView_Scenario_ListingProvider; }
        }

        #endregion


        #region "METAView_Scenario_RetrievalProvider"

        private SqlMETAView_Scenario_RetrievalProvider innerSqlMETAView_Scenario_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Scenario_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Scenario_RetrievalProviderBase METAView_Scenario_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Scenario_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Scenario_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Scenario_RetrievalProvider = new SqlMETAView_Scenario_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Scenario_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Scenario_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Scenario_RetrievalProvider SqlMETAView_Scenario_RetrievalProvider
        {
            get { return METAView_Scenario_RetrievalProvider as SqlMETAView_Scenario_RetrievalProvider; }
        }

        #endregion


        #region "METAView_SecurityClassification_ListingProvider"

        private SqlMETAView_SecurityClassification_ListingProvider innerSqlMETAView_SecurityClassification_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_SecurityClassification_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_SecurityClassification_ListingProviderBase METAView_SecurityClassification_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_SecurityClassification_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_SecurityClassification_ListingProvider == null)
                        {
                            this.innerSqlMETAView_SecurityClassification_ListingProvider = new SqlMETAView_SecurityClassification_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_SecurityClassification_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_SecurityClassification_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_SecurityClassification_ListingProvider SqlMETAView_SecurityClassification_ListingProvider
        {
            get { return METAView_SecurityClassification_ListingProvider as SqlMETAView_SecurityClassification_ListingProvider; }
        }

        #endregion


        #region "METAView_SecurityClassification_RetrievalProvider"

        private SqlMETAView_SecurityClassification_RetrievalProvider innerSqlMETAView_SecurityClassification_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_SecurityClassification_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_SecurityClassification_RetrievalProviderBase METAView_SecurityClassification_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_SecurityClassification_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_SecurityClassification_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_SecurityClassification_RetrievalProvider = new SqlMETAView_SecurityClassification_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_SecurityClassification_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_SecurityClassification_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_SecurityClassification_RetrievalProvider SqlMETAView_SecurityClassification_RetrievalProvider
        {
            get { return METAView_SecurityClassification_RetrievalProvider as SqlMETAView_SecurityClassification_RetrievalProvider; }
        }

        #endregion


        #region "METAView_SelectorAttribute_ListingProvider"

        private SqlMETAView_SelectorAttribute_ListingProvider innerSqlMETAView_SelectorAttribute_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_SelectorAttribute_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_SelectorAttribute_ListingProviderBase METAView_SelectorAttribute_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_SelectorAttribute_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_SelectorAttribute_ListingProvider == null)
                        {
                            this.innerSqlMETAView_SelectorAttribute_ListingProvider = new SqlMETAView_SelectorAttribute_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_SelectorAttribute_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_SelectorAttribute_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_SelectorAttribute_ListingProvider SqlMETAView_SelectorAttribute_ListingProvider
        {
            get { return METAView_SelectorAttribute_ListingProvider as SqlMETAView_SelectorAttribute_ListingProvider; }
        }

        #endregion


        #region "METAView_SelectorAttribute_RetrievalProvider"

        private SqlMETAView_SelectorAttribute_RetrievalProvider innerSqlMETAView_SelectorAttribute_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_SelectorAttribute_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_SelectorAttribute_RetrievalProviderBase METAView_SelectorAttribute_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_SelectorAttribute_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_SelectorAttribute_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_SelectorAttribute_RetrievalProvider = new SqlMETAView_SelectorAttribute_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_SelectorAttribute_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_SelectorAttribute_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_SelectorAttribute_RetrievalProvider SqlMETAView_SelectorAttribute_RetrievalProvider
        {
            get { return METAView_SelectorAttribute_RetrievalProvider as SqlMETAView_SelectorAttribute_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Skill_ListingProvider"

        private SqlMETAView_Skill_ListingProvider innerSqlMETAView_Skill_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Skill_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Skill_ListingProviderBase METAView_Skill_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Skill_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Skill_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Skill_ListingProvider = new SqlMETAView_Skill_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Skill_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Skill_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Skill_ListingProvider SqlMETAView_Skill_ListingProvider
        {
            get { return METAView_Skill_ListingProvider as SqlMETAView_Skill_ListingProvider; }
        }

        #endregion


        #region "METAView_Skill_RetrievalProvider"

        private SqlMETAView_Skill_RetrievalProvider innerSqlMETAView_Skill_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Skill_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Skill_RetrievalProviderBase METAView_Skill_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Skill_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Skill_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Skill_RetrievalProvider = new SqlMETAView_Skill_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Skill_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Skill_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Skill_RetrievalProvider SqlMETAView_Skill_RetrievalProvider
        {
            get { return METAView_Skill_RetrievalProvider as SqlMETAView_Skill_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Software_ListingProvider"

        private SqlMETAView_Software_ListingProvider innerSqlMETAView_Software_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Software_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Software_ListingProviderBase METAView_Software_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Software_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Software_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Software_ListingProvider = new SqlMETAView_Software_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Software_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Software_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Software_ListingProvider SqlMETAView_Software_ListingProvider
        {
            get { return METAView_Software_ListingProvider as SqlMETAView_Software_ListingProvider; }
        }

        #endregion


        #region "METAView_Software_RetrievalProvider"

        private SqlMETAView_Software_RetrievalProvider innerSqlMETAView_Software_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Software_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Software_RetrievalProviderBase METAView_Software_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Software_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Software_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Software_RetrievalProvider = new SqlMETAView_Software_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Software_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Software_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Software_RetrievalProvider SqlMETAView_Software_RetrievalProvider
        {
            get { return METAView_Software_RetrievalProvider as SqlMETAView_Software_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Stakeholder_ListingProvider"

        private SqlMETAView_Stakeholder_ListingProvider innerSqlMETAView_Stakeholder_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Stakeholder_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Stakeholder_ListingProviderBase METAView_Stakeholder_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Stakeholder_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Stakeholder_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Stakeholder_ListingProvider = new SqlMETAView_Stakeholder_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Stakeholder_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Stakeholder_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Stakeholder_ListingProvider SqlMETAView_Stakeholder_ListingProvider
        {
            get { return METAView_Stakeholder_ListingProvider as SqlMETAView_Stakeholder_ListingProvider; }
        }

        #endregion


        #region "METAView_Stakeholder_RetrievalProvider"

        private SqlMETAView_Stakeholder_RetrievalProvider innerSqlMETAView_Stakeholder_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Stakeholder_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Stakeholder_RetrievalProviderBase METAView_Stakeholder_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Stakeholder_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Stakeholder_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Stakeholder_RetrievalProvider = new SqlMETAView_Stakeholder_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Stakeholder_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Stakeholder_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Stakeholder_RetrievalProvider SqlMETAView_Stakeholder_RetrievalProvider
        {
            get { return METAView_Stakeholder_RetrievalProvider as SqlMETAView_Stakeholder_RetrievalProvider; }
        }

        #endregion


        #region "METAView_StatusIndicator_ListingProvider"

        private SqlMETAView_StatusIndicator_ListingProvider innerSqlMETAView_StatusIndicator_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_StatusIndicator_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_StatusIndicator_ListingProviderBase METAView_StatusIndicator_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_StatusIndicator_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_StatusIndicator_ListingProvider == null)
                        {
                            this.innerSqlMETAView_StatusIndicator_ListingProvider = new SqlMETAView_StatusIndicator_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_StatusIndicator_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_StatusIndicator_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_StatusIndicator_ListingProvider SqlMETAView_StatusIndicator_ListingProvider
        {
            get { return METAView_StatusIndicator_ListingProvider as SqlMETAView_StatusIndicator_ListingProvider; }
        }

        #endregion


        #region "METAView_StatusIndicator_RetrievalProvider"

        private SqlMETAView_StatusIndicator_RetrievalProvider innerSqlMETAView_StatusIndicator_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_StatusIndicator_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_StatusIndicator_RetrievalProviderBase METAView_StatusIndicator_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_StatusIndicator_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_StatusIndicator_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_StatusIndicator_RetrievalProvider = new SqlMETAView_StatusIndicator_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_StatusIndicator_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_StatusIndicator_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_StatusIndicator_RetrievalProvider SqlMETAView_StatusIndicator_RetrievalProvider
        {
            get { return METAView_StatusIndicator_RetrievalProvider as SqlMETAView_StatusIndicator_RetrievalProvider; }
        }

        #endregion


        #region "METAView_StorageComponent_ListingProvider"

        private SqlMETAView_StorageComponent_ListingProvider innerSqlMETAView_StorageComponent_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_StorageComponent_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_StorageComponent_ListingProviderBase METAView_StorageComponent_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_StorageComponent_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_StorageComponent_ListingProvider == null)
                        {
                            this.innerSqlMETAView_StorageComponent_ListingProvider = new SqlMETAView_StorageComponent_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_StorageComponent_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_StorageComponent_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_StorageComponent_ListingProvider SqlMETAView_StorageComponent_ListingProvider
        {
            get { return METAView_StorageComponent_ListingProvider as SqlMETAView_StorageComponent_ListingProvider; }
        }

        #endregion


        #region "METAView_StorageComponent_RetrievalProvider"

        private SqlMETAView_StorageComponent_RetrievalProvider innerSqlMETAView_StorageComponent_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_StorageComponent_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_StorageComponent_RetrievalProviderBase METAView_StorageComponent_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_StorageComponent_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_StorageComponent_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_StorageComponent_RetrievalProvider = new SqlMETAView_StorageComponent_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_StorageComponent_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_StorageComponent_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_StorageComponent_RetrievalProvider SqlMETAView_StorageComponent_RetrievalProvider
        {
            get { return METAView_StorageComponent_RetrievalProvider as SqlMETAView_StorageComponent_RetrievalProvider; }
        }

        #endregion


        #region "METAView_StrategicStatement_ListingProvider"

        private SqlMETAView_StrategicStatement_ListingProvider innerSqlMETAView_StrategicStatement_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_StrategicStatement_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_StrategicStatement_ListingProviderBase METAView_StrategicStatement_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_StrategicStatement_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_StrategicStatement_ListingProvider == null)
                        {
                            this.innerSqlMETAView_StrategicStatement_ListingProvider = new SqlMETAView_StrategicStatement_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_StrategicStatement_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_StrategicStatement_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_StrategicStatement_ListingProvider SqlMETAView_StrategicStatement_ListingProvider
        {
            get { return METAView_StrategicStatement_ListingProvider as SqlMETAView_StrategicStatement_ListingProvider; }
        }

        #endregion


        #region "METAView_StrategicStatement_RetrievalProvider"

        private SqlMETAView_StrategicStatement_RetrievalProvider innerSqlMETAView_StrategicStatement_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_StrategicStatement_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_StrategicStatement_RetrievalProviderBase METAView_StrategicStatement_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_StrategicStatement_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_StrategicStatement_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_StrategicStatement_RetrievalProvider = new SqlMETAView_StrategicStatement_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_StrategicStatement_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_StrategicStatement_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_StrategicStatement_RetrievalProvider SqlMETAView_StrategicStatement_RetrievalProvider
        {
            get { return METAView_StrategicStatement_RetrievalProvider as SqlMETAView_StrategicStatement_RetrievalProvider; }
        }

        #endregion


        #region "METAView_StrategicTheme_ListingProvider"

        private SqlMETAView_StrategicTheme_ListingProvider innerSqlMETAView_StrategicTheme_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_StrategicTheme_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_StrategicTheme_ListingProviderBase METAView_StrategicTheme_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_StrategicTheme_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_StrategicTheme_ListingProvider == null)
                        {
                            this.innerSqlMETAView_StrategicTheme_ListingProvider = new SqlMETAView_StrategicTheme_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_StrategicTheme_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_StrategicTheme_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_StrategicTheme_ListingProvider SqlMETAView_StrategicTheme_ListingProvider
        {
            get { return METAView_StrategicTheme_ListingProvider as SqlMETAView_StrategicTheme_ListingProvider; }
        }

        #endregion


        #region "METAView_StrategicTheme_RetrievalProvider"

        private SqlMETAView_StrategicTheme_RetrievalProvider innerSqlMETAView_StrategicTheme_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_StrategicTheme_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_StrategicTheme_RetrievalProviderBase METAView_StrategicTheme_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_StrategicTheme_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_StrategicTheme_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_StrategicTheme_RetrievalProvider = new SqlMETAView_StrategicTheme_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_StrategicTheme_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_StrategicTheme_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_StrategicTheme_RetrievalProvider SqlMETAView_StrategicTheme_RetrievalProvider
        {
            get { return METAView_StrategicTheme_RetrievalProvider as SqlMETAView_StrategicTheme_RetrievalProvider; }
        }

        #endregion


        #region "METAView_SubsetIndicator_ListingProvider"

        private SqlMETAView_SubsetIndicator_ListingProvider innerSqlMETAView_SubsetIndicator_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_SubsetIndicator_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_SubsetIndicator_ListingProviderBase METAView_SubsetIndicator_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_SubsetIndicator_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_SubsetIndicator_ListingProvider == null)
                        {
                            this.innerSqlMETAView_SubsetIndicator_ListingProvider = new SqlMETAView_SubsetIndicator_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_SubsetIndicator_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_SubsetIndicator_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_SubsetIndicator_ListingProvider SqlMETAView_SubsetIndicator_ListingProvider
        {
            get { return METAView_SubsetIndicator_ListingProvider as SqlMETAView_SubsetIndicator_ListingProvider; }
        }

        #endregion


        #region "METAView_SubsetIndicator_RetrievalProvider"

        private SqlMETAView_SubsetIndicator_RetrievalProvider innerSqlMETAView_SubsetIndicator_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_SubsetIndicator_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_SubsetIndicator_RetrievalProviderBase METAView_SubsetIndicator_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_SubsetIndicator_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_SubsetIndicator_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_SubsetIndicator_RetrievalProvider = new SqlMETAView_SubsetIndicator_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_SubsetIndicator_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_SubsetIndicator_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_SubsetIndicator_RetrievalProvider SqlMETAView_SubsetIndicator_RetrievalProvider
        {
            get { return METAView_SubsetIndicator_RetrievalProvider as SqlMETAView_SubsetIndicator_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Sys_ListingProvider"

        private SqlMETAView_Sys_ListingProvider innerSqlMETAView_Sys_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Sys_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Sys_ListingProviderBase METAView_Sys_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Sys_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Sys_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Sys_ListingProvider = new SqlMETAView_Sys_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Sys_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Sys_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Sys_ListingProvider SqlMETAView_Sys_ListingProvider
        {
            get { return METAView_Sys_ListingProvider as SqlMETAView_Sys_ListingProvider; }
        }

        #endregion


        #region "METAView_Sys_RetrievalProvider"

        private SqlMETAView_Sys_RetrievalProvider innerSqlMETAView_Sys_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Sys_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Sys_RetrievalProviderBase METAView_Sys_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Sys_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Sys_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Sys_RetrievalProvider = new SqlMETAView_Sys_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Sys_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Sys_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Sys_RetrievalProvider SqlMETAView_Sys_RetrievalProvider
        {
            get { return METAView_Sys_RetrievalProvider as SqlMETAView_Sys_RetrievalProvider; }
        }

        #endregion


        #region "METAView_SystemComponent_ListingProvider"

        private SqlMETAView_SystemComponent_ListingProvider innerSqlMETAView_SystemComponent_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_SystemComponent_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_SystemComponent_ListingProviderBase METAView_SystemComponent_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_SystemComponent_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_SystemComponent_ListingProvider == null)
                        {
                            this.innerSqlMETAView_SystemComponent_ListingProvider = new SqlMETAView_SystemComponent_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_SystemComponent_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_SystemComponent_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_SystemComponent_ListingProvider SqlMETAView_SystemComponent_ListingProvider
        {
            get { return METAView_SystemComponent_ListingProvider as SqlMETAView_SystemComponent_ListingProvider; }
        }

        #endregion


        #region "METAView_SystemComponent_RetrievalProvider"

        private SqlMETAView_SystemComponent_RetrievalProvider innerSqlMETAView_SystemComponent_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_SystemComponent_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_SystemComponent_RetrievalProviderBase METAView_SystemComponent_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_SystemComponent_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_SystemComponent_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_SystemComponent_RetrievalProvider = new SqlMETAView_SystemComponent_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_SystemComponent_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_SystemComponent_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_SystemComponent_RetrievalProvider SqlMETAView_SystemComponent_RetrievalProvider
        {
            get { return METAView_SystemComponent_RetrievalProvider as SqlMETAView_SystemComponent_RetrievalProvider; }
        }

        #endregion


        #region "METAView_Task_ListingProvider"

        private SqlMETAView_Task_ListingProvider innerSqlMETAView_Task_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Task_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Task_ListingProviderBase METAView_Task_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_Task_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Task_ListingProvider == null)
                        {
                            this.innerSqlMETAView_Task_ListingProvider = new SqlMETAView_Task_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Task_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Task_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Task_ListingProvider SqlMETAView_Task_ListingProvider
        {
            get { return METAView_Task_ListingProvider as SqlMETAView_Task_ListingProvider; }
        }

        #endregion


        #region "METAView_Task_RetrievalProvider"

        private SqlMETAView_Task_RetrievalProvider innerSqlMETAView_Task_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_Task_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_Task_RetrievalProviderBase METAView_Task_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_Task_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_Task_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_Task_RetrievalProvider = new SqlMETAView_Task_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_Task_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_Task_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_Task_RetrievalProvider SqlMETAView_Task_RetrievalProvider
        {
            get { return METAView_Task_RetrievalProvider as SqlMETAView_Task_RetrievalProvider; }
        }

        #endregion


        #region "METAView_TimeDifference_ListingProvider"

        private SqlMETAView_TimeDifference_ListingProvider innerSqlMETAView_TimeDifference_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_TimeDifference_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_TimeDifference_ListingProviderBase METAView_TimeDifference_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_TimeDifference_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_TimeDifference_ListingProvider == null)
                        {
                            this.innerSqlMETAView_TimeDifference_ListingProvider = new SqlMETAView_TimeDifference_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_TimeDifference_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_TimeDifference_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_TimeDifference_ListingProvider SqlMETAView_TimeDifference_ListingProvider
        {
            get { return METAView_TimeDifference_ListingProvider as SqlMETAView_TimeDifference_ListingProvider; }
        }

        #endregion


        #region "METAView_TimeDifference_RetrievalProvider"

        private SqlMETAView_TimeDifference_RetrievalProvider innerSqlMETAView_TimeDifference_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_TimeDifference_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_TimeDifference_RetrievalProviderBase METAView_TimeDifference_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_TimeDifference_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_TimeDifference_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_TimeDifference_RetrievalProvider = new SqlMETAView_TimeDifference_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_TimeDifference_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_TimeDifference_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_TimeDifference_RetrievalProvider SqlMETAView_TimeDifference_RetrievalProvider
        {
            get { return METAView_TimeDifference_RetrievalProvider as SqlMETAView_TimeDifference_RetrievalProvider; }
        }

        #endregion


        #region "METAView_TimeIndicator_ListingProvider"

        private SqlMETAView_TimeIndicator_ListingProvider innerSqlMETAView_TimeIndicator_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_TimeIndicator_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_TimeIndicator_ListingProviderBase METAView_TimeIndicator_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_TimeIndicator_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_TimeIndicator_ListingProvider == null)
                        {
                            this.innerSqlMETAView_TimeIndicator_ListingProvider = new SqlMETAView_TimeIndicator_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_TimeIndicator_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_TimeIndicator_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_TimeIndicator_ListingProvider SqlMETAView_TimeIndicator_ListingProvider
        {
            get { return METAView_TimeIndicator_ListingProvider as SqlMETAView_TimeIndicator_ListingProvider; }
        }

        #endregion


        #region "METAView_TimeIndicator_RetrievalProvider"

        private SqlMETAView_TimeIndicator_RetrievalProvider innerSqlMETAView_TimeIndicator_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_TimeIndicator_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_TimeIndicator_RetrievalProviderBase METAView_TimeIndicator_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_TimeIndicator_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_TimeIndicator_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_TimeIndicator_RetrievalProvider = new SqlMETAView_TimeIndicator_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_TimeIndicator_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_TimeIndicator_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_TimeIndicator_RetrievalProvider SqlMETAView_TimeIndicator_RetrievalProvider
        {
            get { return METAView_TimeIndicator_RetrievalProvider as SqlMETAView_TimeIndicator_RetrievalProvider; }
        }

        #endregion


        #region "METAView_TimeReference_ListingProvider"

        private SqlMETAView_TimeReference_ListingProvider innerSqlMETAView_TimeReference_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_TimeReference_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_TimeReference_ListingProviderBase METAView_TimeReference_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_TimeReference_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_TimeReference_ListingProvider == null)
                        {
                            this.innerSqlMETAView_TimeReference_ListingProvider = new SqlMETAView_TimeReference_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_TimeReference_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_TimeReference_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_TimeReference_ListingProvider SqlMETAView_TimeReference_ListingProvider
        {
            get { return METAView_TimeReference_ListingProvider as SqlMETAView_TimeReference_ListingProvider; }
        }

        #endregion


        #region "METAView_TimeReference_RetrievalProvider"

        private SqlMETAView_TimeReference_RetrievalProvider innerSqlMETAView_TimeReference_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_TimeReference_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_TimeReference_RetrievalProviderBase METAView_TimeReference_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_TimeReference_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_TimeReference_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_TimeReference_RetrievalProvider = new SqlMETAView_TimeReference_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_TimeReference_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_TimeReference_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_TimeReference_RetrievalProvider SqlMETAView_TimeReference_RetrievalProvider
        {
            get { return METAView_TimeReference_RetrievalProvider as SqlMETAView_TimeReference_RetrievalProvider; }
        }

        #endregion


        #region "METAView_TimeScheme_ListingProvider"

        private SqlMETAView_TimeScheme_ListingProvider innerSqlMETAView_TimeScheme_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_TimeScheme_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_TimeScheme_ListingProviderBase METAView_TimeScheme_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_TimeScheme_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_TimeScheme_ListingProvider == null)
                        {
                            this.innerSqlMETAView_TimeScheme_ListingProvider = new SqlMETAView_TimeScheme_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_TimeScheme_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_TimeScheme_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_TimeScheme_ListingProvider SqlMETAView_TimeScheme_ListingProvider
        {
            get { return METAView_TimeScheme_ListingProvider as SqlMETAView_TimeScheme_ListingProvider; }
        }

        #endregion


        #region "METAView_TimeScheme_RetrievalProvider"

        private SqlMETAView_TimeScheme_RetrievalProvider innerSqlMETAView_TimeScheme_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_TimeScheme_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_TimeScheme_RetrievalProviderBase METAView_TimeScheme_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_TimeScheme_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_TimeScheme_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_TimeScheme_RetrievalProvider = new SqlMETAView_TimeScheme_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_TimeScheme_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_TimeScheme_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_TimeScheme_RetrievalProvider SqlMETAView_TimeScheme_RetrievalProvider
        {
            get { return METAView_TimeScheme_RetrievalProvider as SqlMETAView_TimeScheme_RetrievalProvider; }
        }

        #endregion


        #region "METAView_TimeUnit_ListingProvider"

        private SqlMETAView_TimeUnit_ListingProvider innerSqlMETAView_TimeUnit_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_TimeUnit_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_TimeUnit_ListingProviderBase METAView_TimeUnit_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_TimeUnit_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_TimeUnit_ListingProvider == null)
                        {
                            this.innerSqlMETAView_TimeUnit_ListingProvider = new SqlMETAView_TimeUnit_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_TimeUnit_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_TimeUnit_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_TimeUnit_ListingProvider SqlMETAView_TimeUnit_ListingProvider
        {
            get { return METAView_TimeUnit_ListingProvider as SqlMETAView_TimeUnit_ListingProvider; }
        }

        #endregion


        #region "METAView_TimeUnit_RetrievalProvider"

        private SqlMETAView_TimeUnit_RetrievalProvider innerSqlMETAView_TimeUnit_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_TimeUnit_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_TimeUnit_RetrievalProviderBase METAView_TimeUnit_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_TimeUnit_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_TimeUnit_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_TimeUnit_RetrievalProvider = new SqlMETAView_TimeUnit_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_TimeUnit_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_TimeUnit_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_TimeUnit_RetrievalProvider SqlMETAView_TimeUnit_RetrievalProvider
        {
            get { return METAView_TimeUnit_RetrievalProvider as SqlMETAView_TimeUnit_RetrievalProvider; }
        }

        #endregion


        #region "METAView_WorkPackage_ListingProvider"

        private SqlMETAView_WorkPackage_ListingProvider innerSqlMETAView_WorkPackage_ListingProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_WorkPackage_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_WorkPackage_ListingProviderBase METAView_WorkPackage_ListingProvider
        {
            get
            {
                if (innerSqlMETAView_WorkPackage_ListingProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_WorkPackage_ListingProvider == null)
                        {
                            this.innerSqlMETAView_WorkPackage_ListingProvider = new SqlMETAView_WorkPackage_ListingProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_WorkPackage_ListingProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_WorkPackage_ListingProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_WorkPackage_ListingProvider SqlMETAView_WorkPackage_ListingProvider
        {
            get { return METAView_WorkPackage_ListingProvider as SqlMETAView_WorkPackage_ListingProvider; }
        }

        #endregion


        #region "METAView_WorkPackage_RetrievalProvider"

        private SqlMETAView_WorkPackage_RetrievalProvider innerSqlMETAView_WorkPackage_RetrievalProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="METAView_WorkPackage_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override METAView_WorkPackage_RetrievalProviderBase METAView_WorkPackage_RetrievalProvider
        {
            get
            {
                if (innerSqlMETAView_WorkPackage_RetrievalProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlMETAView_WorkPackage_RetrievalProvider == null)
                        {
                            this.innerSqlMETAView_WorkPackage_RetrievalProvider = new SqlMETAView_WorkPackage_RetrievalProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlMETAView_WorkPackage_RetrievalProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="SqlMETAView_WorkPackage_RetrievalProvider"/>.
        /// </summary>
        /// <value></value>
        public SqlMETAView_WorkPackage_RetrievalProvider SqlMETAView_WorkPackage_RetrievalProvider
        {
            get { return METAView_WorkPackage_RetrievalProvider as SqlMETAView_WorkPackage_RetrievalProvider; }
        }

        #endregion
		
        #region "vw_FieldValueProvider"

        private Sqlvw_FieldValueProvider innerSqlvw_FieldValueProvider;

        ///<summary>
        /// This class is the Data Access Logic Component for the <see cref="vw_FieldValue"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        /// <value></value>
        public override vw_FieldValueProviderBase vw_FieldValueProvider
        {
            get
            {
                if (innerSqlvw_FieldValueProvider == null)
                {
                    lock (syncRoot)
                    {
                        if (innerSqlvw_FieldValueProvider == null)
                        {
                            this.innerSqlvw_FieldValueProvider = new Sqlvw_FieldValueProvider(_connectionString, _useStoredProcedure, _providerInvariantName);
                        }
                    }
                }
                return innerSqlvw_FieldValueProvider;
            }
        }

        /// <summary>
        /// Gets the current <c cref="Sqlvw_FieldValueProvider"/>.
        /// </summary>
        /// <value></value>
        public Sqlvw_FieldValueProvider Sqlvw_FieldValueProvider
        {
            get { return vw_FieldValueProvider as Sqlvw_FieldValueProvider; }
        }

        #endregion


        #region "General data access methods"

        #region "ExecuteNonQuery"
        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameterValues">The parameter values.</param>
        /// <returns></returns>
        public override int ExecuteNonQuery(string storedProcedureName, params object[] parameterValues)
        {
            SqlDatabase database = new SqlDatabase(this._connectionString);
            return database.ExecuteNonQuery(storedProcedureName, parameterValues);
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="transactionManager">The transaction manager.</param>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameterValues">The parameter values.</param>
        /// <returns></returns>
        public override int ExecuteNonQuery(TransactionManager transactionManager, string storedProcedureName, params object[] parameterValues)
        {
            SqlDatabase database = new SqlDatabase(this._connectionString);
            return database.ExecuteNonQuery(transactionManager.TransactionObject, storedProcedureName, parameterValues);
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="commandWrapper">The command wrapper.</param>
        public override void ExecuteNonQuery(DbCommand commandWrapper)
        {
            SqlDatabase database = new SqlDatabase(this._connectionString);
            database.ExecuteNonQuery(commandWrapper);

        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="transactionManager">The transaction manager.</param>
        /// <param name="commandWrapper">The command wrapper.</param>
        public override void ExecuteNonQuery(TransactionManager transactionManager, DbCommand commandWrapper)
        {
            SqlDatabase database = new SqlDatabase(this._connectionString);
            database.ExecuteNonQuery(commandWrapper, transactionManager.TransactionObject);
        }


        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="commandText">The command text.</param>
        /// <returns></returns>
        public override int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            SqlDatabase database = new SqlDatabase(this._connectionString);
            return database.ExecuteNonQuery(commandType, commandText);
        }
        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="transactionManager">The transaction manager.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="commandText">The command text.</param>
        /// <returns></returns>
        public override int ExecuteNonQuery(TransactionManager transactionManager, CommandType commandType, string commandText)
        {
            Database database = transactionManager.Database;
            return database.ExecuteNonQuery(transactionManager.TransactionObject, commandType, commandText);
        }
        #endregion

        #region "ExecuteDataReader"
        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameterValues">The parameter values.</param>
        /// <returns></returns>
        public override IDataReader ExecuteReader(string storedProcedureName, params object[] parameterValues)
        {
            SqlDatabase database = new SqlDatabase(this._connectionString);
            return database.ExecuteReader(storedProcedureName, parameterValues);
        }

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="transactionManager">The transaction manager.</param>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameterValues">The parameter values.</param>
        /// <returns></returns>
        public override IDataReader ExecuteReader(TransactionManager transactionManager, string storedProcedureName, params object[] parameterValues)
        {
            Database database = transactionManager.Database;
            return database.ExecuteReader(transactionManager.TransactionObject, storedProcedureName, parameterValues);
        }

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="commandWrapper">The command wrapper.</param>
        /// <returns></returns>
        public override IDataReader ExecuteReader(DbCommand commandWrapper)
        {
            SqlDatabase database = new SqlDatabase(this._connectionString);
            return database.ExecuteReader(commandWrapper);
        }

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="transactionManager">The transaction manager.</param>
        /// <param name="commandWrapper">The command wrapper.</param>
        /// <returns></returns>
        public override IDataReader ExecuteReader(TransactionManager transactionManager, DbCommand commandWrapper)
        {
            Database database = transactionManager.Database;
            return database.ExecuteReader(commandWrapper, transactionManager.TransactionObject);
        }


        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="commandText">The command text.</param>
        /// <returns></returns>
        public override IDataReader ExecuteReader(CommandType commandType, string commandText)
        {
            SqlDatabase database = new SqlDatabase(this._connectionString);
            return database.ExecuteReader(commandType, commandText);
        }
        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="transactionManager">The transaction manager.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="commandText">The command text.</param>
        /// <returns></returns>
        public override IDataReader ExecuteReader(TransactionManager transactionManager, CommandType commandType, string commandText)
        {
            Database database = transactionManager.Database;
            return database.ExecuteReader(transactionManager.TransactionObject, commandType, commandText);
        }
        #endregion

        #region "ExecuteDataSet"
        /// <summary>
        /// Executes the data set.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameterValues">The parameter values.</param>
        /// <returns></returns>
        public override DataSet ExecuteDataSet(string storedProcedureName, params object[] parameterValues)
        {
            SqlDatabase database = new SqlDatabase(this._connectionString);
            return database.ExecuteDataSet(storedProcedureName, parameterValues);
        }

        /// <summary>
        /// Executes the data set.
        /// </summary>
        /// <param name="transactionManager">The transaction manager.</param>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameterValues">The parameter values.</param>
        /// <returns></returns>
        public override DataSet ExecuteDataSet(TransactionManager transactionManager, string storedProcedureName, params object[] parameterValues)
        {
            Database database = transactionManager.Database;
            return database.ExecuteDataSet(transactionManager.TransactionObject, storedProcedureName, parameterValues);
        }

        /// <summary>
        /// Executes the data set.
        /// </summary>
        /// <param name="commandWrapper">The command wrapper.</param>
        /// <returns></returns>
        public override DataSet ExecuteDataSet(DbCommand commandWrapper)
        {
            SqlDatabase database = new SqlDatabase(this._connectionString);
            return database.ExecuteDataSet(commandWrapper);
        }

        /// <summary>
        /// Executes the data set.
        /// </summary>
        /// <param name="transactionManager">The transaction manager.</param>
        /// <param name="commandWrapper">The command wrapper.</param>
        /// <returns></returns>
        public override DataSet ExecuteDataSet(TransactionManager transactionManager, DbCommand commandWrapper)
        {
            Database database = transactionManager.Database;
            return database.ExecuteDataSet(commandWrapper, transactionManager.TransactionObject);
        }


        /// <summary>
        /// Executes the data set.
        /// </summary>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="commandText">The command text.</param>
        /// <returns></returns>
        public override DataSet ExecuteDataSet(CommandType commandType, string commandText)
        {
            SqlDatabase database = new SqlDatabase(this._connectionString);
            return database.ExecuteDataSet(commandType, commandText);
        }
        /// <summary>
        /// Executes the data set.
        /// </summary>
        /// <param name="transactionManager">The transaction manager.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="commandText">The command text.</param>
        /// <returns></returns>
        public override DataSet ExecuteDataSet(TransactionManager transactionManager, CommandType commandType, string commandText)
        {
            Database database = transactionManager.Database;
            return database.ExecuteDataSet(transactionManager.TransactionObject, commandType, commandText);
        }
        #endregion

        #region "ExecuteScalar"
        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameterValues">The parameter values.</param>
        /// <returns></returns>
        public override object ExecuteScalar(string storedProcedureName, params object[] parameterValues)
        {
            SqlDatabase database = new SqlDatabase(this._connectionString);
            return database.ExecuteScalar(storedProcedureName, parameterValues);
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="transactionManager">The transaction manager.</param>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameterValues">The parameter values.</param>
        /// <returns></returns>
        public override object ExecuteScalar(TransactionManager transactionManager, string storedProcedureName, params object[] parameterValues)
        {
            Database database = transactionManager.Database;
            return database.ExecuteScalar(transactionManager.TransactionObject, storedProcedureName, parameterValues);
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="commandWrapper">The command wrapper.</param>
        /// <returns></returns>
        public override object ExecuteScalar(DbCommand commandWrapper)
        {
            SqlDatabase database = new SqlDatabase(this._connectionString);
            return database.ExecuteScalar(commandWrapper);
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="transactionManager">The transaction manager.</param>
        /// <param name="commandWrapper">The command wrapper.</param>
        /// <returns></returns>
        public override object ExecuteScalar(TransactionManager transactionManager, DbCommand commandWrapper)
        {
            Database database = transactionManager.Database;
            return database.ExecuteScalar(commandWrapper, transactionManager.TransactionObject);
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="commandText">The command text.</param>
        /// <returns></returns>
        public override object ExecuteScalar(CommandType commandType, string commandText)
        {
            SqlDatabase database = new SqlDatabase(this._connectionString);
            return database.ExecuteScalar(commandType, commandText);
        }
        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="transactionManager">The transaction manager.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="commandText">The command text.</param>
        /// <returns></returns>
        public override object ExecuteScalar(TransactionManager transactionManager, CommandType commandType, string commandText)
        {
            Database database = transactionManager.Database;
            return database.ExecuteScalar(transactionManager.TransactionObject, commandType, commandText);
        }
        #endregion

        #endregion


    }
}
