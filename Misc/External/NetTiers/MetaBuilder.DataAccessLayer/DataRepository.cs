﻿#region Using directives

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Configuration;
using System.Web;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.DataAccessLayer.Bases;

#endregion

namespace MetaBuilder.DataAccessLayer
{
    /// <summary>
    /// This class represents the Data source repository and gives access to all the underlying providers.
    /// </summary>
    [CLSCompliant(true)]
    public sealed class DataRepository
    {
        private static volatile NetTiersProvider _provider = null;
        private static volatile NetTiersProviderCollection _providers = null;
        private static volatile NetTiersServiceSection _section = null;
        private static volatile Configuration _config = null;

        private static object SyncRoot = new object();

        private DataRepository()
        {
        }

        #region Public LoadProvider
        /// <summary>
        /// Enables the DataRepository to programatically create and 
        /// pass in a <c>NetTiersProvider</c> during runtime.
        /// </summary>
        /// <param name="provider">An instatiated NetTiersProvider.</param>
        public static void LoadProvider(NetTiersProvider provider)
        {
            LoadProvider(provider, false);
        }

        /// <summary>
        /// Enables the DataRepository to programatically create and 
        /// pass in a <c>NetTiersProvider</c> during runtime.
        /// </summary>
        /// <param name="provider">An instatiated NetTiersProvider.</param>
        /// <param name="setAsDefault">ability to set any valid provider as the default provider for the DataRepository.</param>
        public static void LoadProvider(NetTiersProvider provider, bool setAsDefault)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            if (_providers == null)
            {
                lock (SyncRoot)
                {
                    if (_providers == null)
                        _providers = new NetTiersProviderCollection();
                }
            }

            if (_providers[provider.Name] == null)
            {
                lock (_providers.SyncRoot)
                {
                    _providers.Add(provider);
                }
            }

            if (_provider == null || setAsDefault)
            {
                lock (SyncRoot)
                {
                    if (_provider == null || setAsDefault)
                        _provider = provider;
                }
            }
        }
        #endregion

        ///<summary>
        /// Configuration based provider loading, will load the providers on first call.
        ///</summary>
        private static void LoadProviders()
        {
            // Avoid claiming lock if providers are already loaded
            if (_provider == null)
            {
                lock (SyncRoot)
                {
                    // Do this again to make sure _provider is still null
                    if (_provider == null)
                    {
                        // Load registered providers and point _provider to the default provider
                        _providers = new NetTiersProviderCollection();

                        ProvidersHelper.InstantiateProviders(NetTiersSection.Providers, _providers, typeof(NetTiersProvider));
                        _provider = _providers[NetTiersSection.DefaultProvider];

                        if (_provider == null)
                        {
                            throw new ProviderException("Unable to load default NetTiersProvider");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the provider.
        /// </summary>
        /// <value>The provider.</value>
        public static NetTiersProvider Provider
        {
            get { LoadProviders(); return _provider; }
        }

        /// <summary>
        /// Gets the provider collection.
        /// </summary>
        /// <value>The providers.</value>
        public static NetTiersProviderCollection Providers
        {
            get { LoadProviders(); return _providers; }
        }

        /// <summary>
        /// Creates a new <c cref="TransactionManager"/> instance from the current datasource.
        /// </summary>
        /// <returns></returns>
        public TransactionManager CreateTransaction()
        {
            return _provider.CreateTransaction();
        }

        #region Configuration

        /// <summary>
        /// Gets a reference to the configured NetTiersServiceSection object.
        /// </summary>
        public static NetTiersServiceSection NetTiersSection
        {
            get
            {
                // Try to get a reference to the default <netTiersService> section
                _section = WebConfigurationManager.GetSection("netTiersService") as NetTiersServiceSection;

                if (_section == null)
                {
                    // otherwise look for section based on the assembly name
                    _section = WebConfigurationManager.GetSection("MetaBuilder.DataAccessLayer") as NetTiersServiceSection;
                }

                #region Design-Time Support

                if (_section == null)
                {
                    // lastly, try to find the specific NetTiersServiceSection for this assembly
                    foreach (ConfigurationSection temp in Configuration.Sections)
                    {
                        if (temp is NetTiersServiceSection)
                        {
                            _section = temp as NetTiersServiceSection;
                            break;
                        }
                    }
                }

                #endregion Design-Time Support

                if (_section == null)
                {
                    throw new ProviderException("Unable to load NetTiersServiceSection");
                }

                return _section;
            }
        }

        #region Design-Time Support

        /// <summary>
        /// Gets a reference to the application configuration object.
        /// </summary>
        public static Configuration Configuration
        {
            get
            {
                if (_config == null)
                {
                    // load specific config file
                    if (HttpContext.Current != null)
                    {
                        _config = WebConfigurationManager.OpenWebConfiguration("~");
                    }
                    else
                    {
                        String configFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile.Replace(".config", "").Replace(".temp", "");

                        // check for design mode
                        if (configFile.ToLower().Contains("devenv.exe"))
                        {
                            _config = GetDesignTimeConfig();
                        }
                        else
                        {
                            _config = ConfigurationManager.OpenExeConfiguration(configFile);
                        }
                    }
                }

                return _config;
            }
        }

        private static Configuration GetDesignTimeConfig()
        {
            ExeConfigurationFileMap configMap = null;
            Configuration config = null;
            String path = null;

            // Get an instance of the currently running Visual Studio IDE.
            EnvDTE80.DTE2 dte = (EnvDTE80.DTE2)System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE.8.0");

            if (dte != null)
            {
                dte.SuppressUI = true;

                EnvDTE.ProjectItem item = dte.Solution.FindProjectItem("web.config");
                if (item != null)
                {
                    if (!item.ContainingProject.FullName.ToLower().StartsWith("http:"))
                    {
                        System.IO.FileInfo info = new System.IO.FileInfo(item.ContainingProject.FullName);
                        path = String.Format("{0}\\{1}", info.Directory.FullName, item.Name);
                        configMap = new ExeConfigurationFileMap();
                        configMap.ExeConfigFilename = path;
                    }
                    else
                    {
                        configMap = new ExeConfigurationFileMap();
                        configMap.ExeConfigFilename = item.get_FileNames(0);
                    }
                }

                /*
                Array projects = (Array) dte2.ActiveSolutionProjects;
                EnvDTE.Project project = (EnvDTE.Project) projects.GetValue(0);
                System.IO.FileInfo info;

                foreach ( EnvDTE.ProjectItem item in project.ProjectItems )
                {
                    if ( String.Compare(item.Name, "web.config", true) == 0 )
                    {
                        info = new System.IO.FileInfo(project.FullName);
                        path = String.Format("{0}\\{1}", info.Directory.FullName, item.Name);
                        configMap = new ExeConfigurationFileMap();
                        configMap.ExeConfigFilename = path;
                        break;
                    }
                }
                */
            }

            config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            return config;
        }

        #endregion Design-Time Support

        #endregion Configuration

        #region Connections

        /// <summary>
        /// Gets a reference to the ConnectionStringSettings collection.
        /// </summary>
        public static ConnectionStringSettingsCollection ConnectionStrings
        {
            get
            {
                // use default ConnectionStrings if _section has already been discovered
                if (_config == null && _section != null)
                {
                    return WebConfigurationManager.ConnectionStrings;
                }

                return Configuration.ConnectionStrings.ConnectionStrings;
            }
        }

        // dictionary of connection providers
        private static Dictionary<String, ConnectionProvider> _connections;

        /// <summary>
        /// Gets the dictionary of connection providers.
        /// </summary>
        public static Dictionary<String, ConnectionProvider> Connections
        {
            get
            {
                if (_connections == null)
                {
                    lock (SyncRoot)
                    {
                        if (_connections == null)
                        {
                            _connections = new Dictionary<String, ConnectionProvider>();

                            // add a connection provider for each configured connection string
                            foreach (ConnectionStringSettings conn in ConnectionStrings)
                            {
                                _connections.Add(conn.Name, new ConnectionProvider(conn.Name, conn.ConnectionString));
                            }
                        }
                    }
                }

                return _connections;
            }
        }

        /// <summary>
        /// Adds the specified connection string to the map of connection strings.
        /// </summary>
        /// <param name="connectionStringName">The connection string name.</param>
        /// <param name="connectionString">The provider specific connection information.</param>
        public static void AddConnection(String connectionStringName, String connectionString)
        {
            lock (SyncRoot)
            {
                Connections.Remove(connectionStringName);
                ConnectionProvider connection = new ConnectionProvider(connectionStringName, connectionString);
                Connections.Add(connectionStringName, connection);
            }
        }

        /// <summary>
        /// Provides ability to switch connection string at runtime.
        /// </summary>
        public sealed class ConnectionProvider
        {
            private NetTiersProvider _provider;
            private NetTiersProviderCollection _providers;
            private String _connectionStringName;
            private String _connectionString;


            /// <summary>
            /// Initializes a new instance of the ConnectionProvider class.
            /// </summary>
            /// <param name="connectionStringName">The connection string name.</param>
            /// <param name="connectionString">The provider specific connection information.</param>
            public ConnectionProvider(String connectionStringName, String connectionString)
            {
                _connectionString = connectionString;
                _connectionStringName = connectionStringName;
            }

            /// <summary>
            /// Gets the provider.
            /// </summary>
            public NetTiersProvider Provider
            {
                get { LoadProviders(); return _provider; }
            }

            /// <summary>
            /// Gets the provider collection.
            /// </summary>
            public NetTiersProviderCollection Providers
            {
                get { LoadProviders(); return _providers; }
            }

            /// <summary>
            /// Instantiates the configured providers based on the supplied connection string.
            /// </summary>
            private void LoadProviders()
            {
                DataRepository.LoadProviders();

                // Avoid claiming lock if providers are already loaded
                if (_providers == null)
                {
                    lock (SyncRoot)
                    {
                        // Do this again to make sure _provider is still null
                        if (_providers == null)
                        {
                            // apply connection information to each provider
                            for (int i = 0; i < NetTiersSection.Providers.Count; i++)
                            {
                                NetTiersSection.Providers[i].Parameters["connectionStringName"] = _connectionStringName;
                                // remove previous connection string, if any
                                NetTiersSection.Providers[i].Parameters.Remove("connectionString");

                                if (!String.IsNullOrEmpty(_connectionString))
                                {
                                    NetTiersSection.Providers[i].Parameters["connectionString"] = _connectionString;
                                }
                            }

                            // Load registered providers and point _provider to the default provider
                            _providers = new NetTiersProviderCollection();

                            ProvidersHelper.InstantiateProviders(NetTiersSection.Providers, _providers, typeof(NetTiersProvider));
                            _provider = _providers[NetTiersSection.DefaultProvider];
                        }
                    }
                }
            }
        }

        #endregion Connections

        #region Static properties

        #region VCStatusProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="VCStatus"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static VCStatusProviderBase VCStatusProvider
        {
            get
            {
                LoadProviders();
                return _provider.VCStatusProvider;
            }
        }

        #endregion

        #region DomainDefinitionPossibleValueProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="DomainDefinitionPossibleValue"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static DomainDefinitionPossibleValueProviderBase DomainDefinitionPossibleValueProvider
        {
            get
            {
                LoadProviders();
                return _provider.DomainDefinitionPossibleValueProvider;
            }
        }

        #endregion

        #region WorkspaceProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="Workspace"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static WorkspaceProviderBase WorkspaceProvider
        {
            get
            {
                LoadProviders();
                return _provider.WorkspaceProvider;
            }
        }

        #endregion

        #region ClassTypeProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="ClassType"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static ClassTypeProviderBase ClassTypeProvider
        {
            get
            {
                LoadProviders();
                return _provider.ClassTypeProvider;
            }
        }

        #endregion

        #region WorkspaceTypeProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="WorkspaceType"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static WorkspaceTypeProviderBase WorkspaceTypeProvider
        {
            get
            {
                LoadProviders();
                return _provider.WorkspaceTypeProvider;
            }
        }

        #endregion

        #region UserProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="User"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static UserProviderBase UserProvider
        {
            get
            {
                LoadProviders();
                return _provider.UserProvider;
            }
        }

        #endregion

        #region GraphFileProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="GraphFile"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static GraphFileProviderBase GraphFileProvider
        {
            get
            {
                LoadProviders();
                return _provider.GraphFileProvider;
            }
        }

        #endregion

        #region ClassProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="Class"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static ClassProviderBase ClassProvider
        {
            get
            {
                LoadProviders();
                return _provider.ClassProvider;
            }
        }

        #endregion

        #region MetaObjectProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="MetaObject"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static MetaObjectProviderBase MetaObjectProvider
        {
            get
            {
                LoadProviders();
                return _provider.MetaObjectProvider;
            }
        }

        #endregion

        #region UURIProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="UURI"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static UURIProviderBase UURIProvider
        {
            get
            {
                LoadProviders();
                return _provider.UURIProvider;
            }
        }

        #endregion

        #region UserPermissionProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="UserPermission"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static UserPermissionProviderBase UserPermissionProvider
        {
            get
            {
                LoadProviders();
                return _provider.UserPermissionProvider;
            }
        }

        #endregion

        #region ConfigProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="Config"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static ConfigProviderBase ConfigProvider
        {
            get
            {
                LoadProviders();
                return _provider.ConfigProvider;
            }
        }

        #endregion

        #region AssociationTypeProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="AssociationType"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static AssociationTypeProviderBase AssociationTypeProvider
        {
            get
            {
                LoadProviders();
                return _provider.AssociationTypeProvider;
            }
        }

        #endregion

        #region ClassAssociationProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="ClassAssociation"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static ClassAssociationProviderBase ClassAssociationProvider
        {
            get
            {
                LoadProviders();
                return _provider.ClassAssociationProvider;
            }
        }

        #endregion

        #region ObjectAssociationProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="ObjectAssociation"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static ObjectAssociationProviderBase ObjectAssociationProvider
        {
            get
            {
                LoadProviders();
                return _provider.ObjectAssociationProvider;
            }
        }

        #endregion

        #region FileTypeProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="FileType"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static FileTypeProviderBase FileTypeProvider
        {
            get
            {
                LoadProviders();
                return _provider.FileTypeProvider;
            }
        }

        #endregion

        #region ObjectFieldValueProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="ObjectFieldValue"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static ObjectFieldValueProviderBase ObjectFieldValueProvider
        {
            get
            {
                LoadProviders();
                return _provider.ObjectFieldValueProvider;
            }
        }

        #endregion

        #region GraphFileAssociationProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="GraphFileAssociation"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static GraphFileAssociationProviderBase GraphFileAssociationProvider
        {
            get
            {
                LoadProviders();
                return _provider.GraphFileAssociationProvider;
            }
        }

        #endregion

        #region AllowedArtifactProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="AllowedArtifact"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static AllowedArtifactProviderBase AllowedArtifactProvider
        {
            get
            {
                LoadProviders();
                return _provider.AllowedArtifactProvider;
            }
        }

        #endregion

        #region DomainDefinitionProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="DomainDefinition"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static DomainDefinitionProviderBase DomainDefinitionProvider
        {
            get
            {
                LoadProviders();
                return _provider.DomainDefinitionProvider;
            }
        }

        #endregion

        #region ArtifactProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="Artifact"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static ArtifactProviderBase ArtifactProvider
        {
            get
            {
                LoadProviders();
                return _provider.ArtifactProvider;
            }
        }

        #endregion

        #region PermissionProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="Permission"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static PermissionProviderBase PermissionProvider
        {
            get
            {
                LoadProviders();
                return _provider.PermissionProvider;
            }
        }

        #endregion

        #region GraphFileObjectProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="GraphFileObject"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static GraphFileObjectProviderBase GraphFileObjectProvider
        {
            get
            {
                LoadProviders();
                return _provider.GraphFileObjectProvider;
            }
        }

        #endregion

        #region FieldProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="Field"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static FieldProviderBase FieldProvider
        {
            get
            {
                LoadProviders();
                return _provider.FieldProvider;
            }
        }

        #endregion

        #region METAView_Activity_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Activity_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Activity_ListingProviderBase METAView_Activity_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Activity_ListingProvider;
            }
        }

        #endregion

        #region METAView_Activity_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Activity_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Activity_RetrievalProviderBase METAView_Activity_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Activity_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_ApplicationInterface_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_ApplicationInterface_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_ApplicationInterface_ListingProviderBase METAView_ApplicationInterface_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_ApplicationInterface_ListingProvider;
            }
        }

        #endregion

        #region METAView_ApplicationInterface_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_ApplicationInterface_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_ApplicationInterface_RetrievalProviderBase METAView_ApplicationInterface_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_ApplicationInterface_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Assumption_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Assumption_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Assumption_ListingProviderBase METAView_Assumption_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Assumption_ListingProvider;
            }
        }

        #endregion

        #region METAView_Assumption_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Assumption_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Assumption_RetrievalProviderBase METAView_Assumption_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Assumption_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Attribute_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Attribute_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Attribute_ListingProviderBase METAView_Attribute_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Attribute_ListingProvider;
            }
        }

        #endregion

        #region METAView_Attribute_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Attribute_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Attribute_RetrievalProviderBase METAView_Attribute_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Attribute_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_BusinessCompetency_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_BusinessCompetency_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_BusinessCompetency_ListingProviderBase METAView_BusinessCompetency_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_BusinessCompetency_ListingProvider;
            }
        }

        #endregion

        #region METAView_BusinessCompetency_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_BusinessCompetency_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_BusinessCompetency_RetrievalProviderBase METAView_BusinessCompetency_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_BusinessCompetency_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_BusinessInterface_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_BusinessInterface_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_BusinessInterface_ListingProviderBase METAView_BusinessInterface_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_BusinessInterface_ListingProvider;
            }
        }

        #endregion

        #region METAView_BusinessInterface_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_BusinessInterface_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_BusinessInterface_RetrievalProviderBase METAView_BusinessInterface_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_BusinessInterface_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_CAD_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_CAD_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_CAD_ListingProviderBase METAView_CAD_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_CAD_ListingProvider;
            }
        }

        #endregion

        #region METAView_CAD_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_CAD_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_CAD_RetrievalProviderBase METAView_CAD_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_CAD_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_CADReal_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_CADReal_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_CADReal_ListingProviderBase METAView_CADReal_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_CADReal_ListingProvider;
            }
        }

        #endregion

        #region METAView_CADReal_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_CADReal_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_CADReal_RetrievalProviderBase METAView_CADReal_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_CADReal_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_CategoryFactor_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_CategoryFactor_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_CategoryFactor_ListingProviderBase METAView_CategoryFactor_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_CategoryFactor_ListingProvider;
            }
        }

        #endregion

        #region METAView_CategoryFactor_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_CategoryFactor_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_CategoryFactor_RetrievalProviderBase METAView_CategoryFactor_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_CategoryFactor_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Channel_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Channel_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Channel_ListingProviderBase METAView_Channel_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Channel_ListingProvider;
            }
        }

        #endregion

        #region METAView_Channel_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Channel_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Channel_RetrievalProviderBase METAView_Channel_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Channel_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Competency_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Competency_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Competency_ListingProviderBase METAView_Competency_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Competency_ListingProvider;
            }
        }

        #endregion

        #region METAView_Competency_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Competency_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Competency_RetrievalProviderBase METAView_Competency_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Competency_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_ComputingComponent_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_ComputingComponent_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_ComputingComponent_ListingProviderBase METAView_ComputingComponent_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_ComputingComponent_ListingProvider;
            }
        }

        #endregion

        #region METAView_ComputingComponent_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_ComputingComponent_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_ComputingComponent_RetrievalProviderBase METAView_ComputingComponent_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_ComputingComponent_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Condition_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Condition_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Condition_ListingProviderBase METAView_Condition_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Condition_ListingProvider;
            }
        }

        #endregion

        #region METAView_Condition_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Condition_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Condition_RetrievalProviderBase METAView_Condition_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Condition_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Conditional_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Conditional_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Conditional_ListingProviderBase METAView_Conditional_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Conditional_ListingProvider;
            }
        }

        #endregion

        #region METAView_Conditional_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Conditional_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Conditional_RetrievalProviderBase METAView_Conditional_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Conditional_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_ConditionalDescription_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_ConditionalDescription_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_ConditionalDescription_ListingProviderBase METAView_ConditionalDescription_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_ConditionalDescription_ListingProvider;
            }
        }

        #endregion

        #region METAView_ConditionalDescription_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_ConditionalDescription_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_ConditionalDescription_RetrievalProviderBase METAView_ConditionalDescription_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_ConditionalDescription_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_ConnectionSize_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_ConnectionSize_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_ConnectionSize_ListingProviderBase METAView_ConnectionSize_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_ConnectionSize_ListingProvider;
            }
        }

        #endregion

        #region METAView_ConnectionSize_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_ConnectionSize_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_ConnectionSize_RetrievalProviderBase METAView_ConnectionSize_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_ConnectionSize_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_ConnectionSpeed_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_ConnectionSpeed_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_ConnectionSpeed_ListingProviderBase METAView_ConnectionSpeed_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_ConnectionSpeed_ListingProvider;
            }
        }

        #endregion

        #region METAView_ConnectionSpeed_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_ConnectionSpeed_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_ConnectionSpeed_RetrievalProviderBase METAView_ConnectionSpeed_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_ConnectionSpeed_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_ConnectionType_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_ConnectionType_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_ConnectionType_ListingProviderBase METAView_ConnectionType_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_ConnectionType_ListingProvider;
            }
        }

        #endregion

        #region METAView_ConnectionType_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_ConnectionType_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_ConnectionType_RetrievalProviderBase METAView_ConnectionType_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_ConnectionType_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Constraint_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Constraint_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Constraint_ListingProviderBase METAView_Constraint_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Constraint_ListingProvider;
            }
        }

        #endregion

        #region METAView_Constraint_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Constraint_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Constraint_RetrievalProviderBase METAView_Constraint_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Constraint_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_CSF_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_CSF_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_CSF_ListingProviderBase METAView_CSF_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_CSF_ListingProvider;
            }
        }

        #endregion

        #region METAView_CSF_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_CSF_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_CSF_RetrievalProviderBase METAView_CSF_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_CSF_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_DataAttribute_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_DataAttribute_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_DataAttribute_ListingProviderBase METAView_DataAttribute_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_DataAttribute_ListingProvider;
            }
        }

        #endregion

        #region METAView_DataAttribute_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_DataAttribute_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_DataAttribute_RetrievalProviderBase METAView_DataAttribute_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_DataAttribute_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_DataColumn_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_DataColumn_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_DataColumn_ListingProviderBase METAView_DataColumn_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_DataColumn_ListingProvider;
            }
        }

        #endregion

        #region METAView_DataColumn_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_DataColumn_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_DataColumn_RetrievalProviderBase METAView_DataColumn_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_DataColumn_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_DataDomain_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_DataDomain_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_DataDomain_ListingProviderBase METAView_DataDomain_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_DataDomain_ListingProvider;
            }
        }

        #endregion

        #region METAView_DataDomain_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_DataDomain_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_DataDomain_RetrievalProviderBase METAView_DataDomain_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_DataDomain_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_DataEntity_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_DataEntity_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_DataEntity_ListingProviderBase METAView_DataEntity_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_DataEntity_ListingProvider;
            }
        }

        #endregion

        #region METAView_DataEntity_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_DataEntity_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_DataEntity_RetrievalProviderBase METAView_DataEntity_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_DataEntity_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_DataField_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_DataField_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_DataField_ListingProviderBase METAView_DataField_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_DataField_ListingProvider;
            }
        }

        #endregion

        #region METAView_DataField_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_DataField_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_DataField_RetrievalProviderBase METAView_DataField_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_DataField_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_DataFlowIndicator_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_DataFlowIndicator_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_DataFlowIndicator_ListingProviderBase METAView_DataFlowIndicator_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_DataFlowIndicator_ListingProvider;
            }
        }

        #endregion

        #region METAView_DataFlowIndicator_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_DataFlowIndicator_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_DataFlowIndicator_RetrievalProviderBase METAView_DataFlowIndicator_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_DataFlowIndicator_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_DataSchema_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_DataSchema_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_DataSchema_ListingProviderBase METAView_DataSchema_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_DataSchema_ListingProvider;
            }
        }

        #endregion

        #region METAView_DataSchema_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_DataSchema_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_DataSchema_RetrievalProviderBase METAView_DataSchema_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_DataSchema_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_DataSubjectArea_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_DataSubjectArea_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_DataSubjectArea_ListingProviderBase METAView_DataSubjectArea_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_DataSubjectArea_ListingProvider;
            }
        }

        #endregion

        #region METAView_DataSubjectArea_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_DataSubjectArea_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_DataSubjectArea_RetrievalProviderBase METAView_DataSubjectArea_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_DataSubjectArea_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_DataTable_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_DataTable_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_DataTable_ListingProviderBase METAView_DataTable_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_DataTable_ListingProvider;
            }
        }

        #endregion

        #region METAView_DataTable_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_DataTable_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_DataTable_RetrievalProviderBase METAView_DataTable_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_DataTable_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_DataValue_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_DataValue_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_DataValue_ListingProviderBase METAView_DataValue_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_DataValue_ListingProvider;
            }
        }

        #endregion

        #region METAView_DataValue_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_DataValue_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_DataValue_RetrievalProviderBase METAView_DataValue_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_DataValue_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_DataView_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_DataView_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_DataView_ListingProviderBase METAView_DataView_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_DataView_ListingProvider;
            }
        }

        #endregion

        #region METAView_DataView_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_DataView_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_DataView_RetrievalProviderBase METAView_DataView_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_DataView_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_DatedResponsibility_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_DatedResponsibility_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_DatedResponsibility_ListingProviderBase METAView_DatedResponsibility_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_DatedResponsibility_ListingProvider;
            }
        }

        #endregion

        #region METAView_DatedResponsibility_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_DatedResponsibility_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_DatedResponsibility_RetrievalProviderBase METAView_DatedResponsibility_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_DatedResponsibility_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_DependencyDescription_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_DependencyDescription_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_DependencyDescription_ListingProviderBase METAView_DependencyDescription_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_DependencyDescription_ListingProvider;
            }
        }

        #endregion

        #region METAView_DependencyDescription_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_DependencyDescription_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_DependencyDescription_RetrievalProviderBase METAView_DependencyDescription_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_DependencyDescription_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Driver_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Driver_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Driver_ListingProviderBase METAView_Driver_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Driver_ListingProvider;
            }
        }

        #endregion

        #region METAView_Driver_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Driver_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Driver_RetrievalProviderBase METAView_Driver_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Driver_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Employee_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Employee_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Employee_ListingProviderBase METAView_Employee_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Employee_ListingProvider;
            }
        }

        #endregion

        #region METAView_Employee_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Employee_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Employee_RetrievalProviderBase METAView_Employee_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Employee_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Entity_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Entity_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Entity_ListingProviderBase METAView_Entity_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Entity_ListingProvider;
            }
        }

        #endregion

        #region METAView_Entity_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Entity_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Entity_RetrievalProviderBase METAView_Entity_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Entity_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Environment_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Environment_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Environment_ListingProviderBase METAView_Environment_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Environment_ListingProvider;
            }
        }

        #endregion

        #region METAView_Environment_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Environment_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Environment_RetrievalProviderBase METAView_Environment_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Environment_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_EnvironmentCategory_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_EnvironmentCategory_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_EnvironmentCategory_ListingProviderBase METAView_EnvironmentCategory_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_EnvironmentCategory_ListingProvider;
            }
        }

        #endregion

        #region METAView_EnvironmentCategory_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_EnvironmentCategory_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_EnvironmentCategory_RetrievalProviderBase METAView_EnvironmentCategory_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_EnvironmentCategory_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Event_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Event_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Event_ListingProviderBase METAView_Event_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Event_ListingProvider;
            }
        }

        #endregion

        #region METAView_Event_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Event_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Event_RetrievalProviderBase METAView_Event_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Event_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Exception_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Exception_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Exception_ListingProviderBase METAView_Exception_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Exception_ListingProvider;
            }
        }

        #endregion

        #region METAView_Exception_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Exception_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Exception_RetrievalProviderBase METAView_Exception_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Exception_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_ExtractionRule_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_ExtractionRule_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_ExtractionRule_ListingProviderBase METAView_ExtractionRule_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_ExtractionRule_ListingProvider;
            }
        }

        #endregion

        #region METAView_ExtractionRule_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_ExtractionRule_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_ExtractionRule_RetrievalProviderBase METAView_ExtractionRule_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_ExtractionRule_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_FlowDescription_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_FlowDescription_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_FlowDescription_ListingProviderBase METAView_FlowDescription_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_FlowDescription_ListingProvider;
            }
        }

        #endregion

        #region METAView_FlowDescription_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_FlowDescription_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_FlowDescription_RetrievalProviderBase METAView_FlowDescription_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_FlowDescription_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Function_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Function_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Function_ListingProviderBase METAView_Function_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Function_ListingProvider;
            }
        }

        #endregion

        #region METAView_Function_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Function_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Function_RetrievalProviderBase METAView_Function_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Function_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_FunctionalDependency_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_FunctionalDependency_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_FunctionalDependency_ListingProviderBase METAView_FunctionalDependency_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_FunctionalDependency_ListingProvider;
            }
        }

        #endregion

        #region METAView_FunctionalDependency_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_FunctionalDependency_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_FunctionalDependency_RetrievalProviderBase METAView_FunctionalDependency_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_FunctionalDependency_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Gateway_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Gateway_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Gateway_ListingProviderBase METAView_Gateway_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Gateway_ListingProvider;
            }
        }

        #endregion

        #region METAView_Gateway_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Gateway_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Gateway_RetrievalProviderBase METAView_Gateway_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Gateway_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_GatewayDescription_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_GatewayDescription_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_GatewayDescription_ListingProviderBase METAView_GatewayDescription_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_GatewayDescription_ListingProvider;
            }
        }

        #endregion

        #region METAView_GatewayDescription_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_GatewayDescription_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_GatewayDescription_RetrievalProviderBase METAView_GatewayDescription_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_GatewayDescription_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_GovernanceMechanism_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_GovernanceMechanism_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_GovernanceMechanism_ListingProviderBase METAView_GovernanceMechanism_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_GovernanceMechanism_ListingProvider;
            }
        }

        #endregion

        #region METAView_GovernanceMechanism_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_GovernanceMechanism_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_GovernanceMechanism_RetrievalProviderBase METAView_GovernanceMechanism_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_GovernanceMechanism_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Implication_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Implication_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Implication_ListingProviderBase METAView_Implication_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Implication_ListingProvider;
            }
        }

        #endregion

        #region METAView_Implication_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Implication_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Implication_RetrievalProviderBase METAView_Implication_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Implication_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Iteration_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Iteration_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Iteration_ListingProviderBase METAView_Iteration_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Iteration_ListingProvider;
            }
        }

        #endregion

        #region METAView_Iteration_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Iteration_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Iteration_RetrievalProviderBase METAView_Iteration_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Iteration_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_ITInfrastructureEnvironment_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_ITInfrastructureEnvironment_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_ITInfrastructureEnvironment_ListingProviderBase METAView_ITInfrastructureEnvironment_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_ITInfrastructureEnvironment_ListingProvider;
            }
        }

        #endregion

        #region METAView_ITInfrastructureEnvironment_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_ITInfrastructureEnvironment_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_ITInfrastructureEnvironment_RetrievalProviderBase METAView_ITInfrastructureEnvironment_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_ITInfrastructureEnvironment_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Job_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Job_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Job_ListingProviderBase METAView_Job_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Job_ListingProvider;
            }
        }

        #endregion

        #region METAView_Job_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Job_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Job_RetrievalProviderBase METAView_Job_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Job_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_JobCompetency_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_JobCompetency_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_JobCompetency_ListingProviderBase METAView_JobCompetency_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_JobCompetency_ListingProvider;
            }
        }

        #endregion

        #region METAView_JobCompetency_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_JobCompetency_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_JobCompetency_RetrievalProviderBase METAView_JobCompetency_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_JobCompetency_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_JobPosition_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_JobPosition_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_JobPosition_ListingProviderBase METAView_JobPosition_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_JobPosition_ListingProvider;
            }
        }

        #endregion

        #region METAView_JobPosition_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_JobPosition_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_JobPosition_RetrievalProviderBase METAView_JobPosition_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_JobPosition_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Location_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Location_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Location_ListingProviderBase METAView_Location_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Location_ListingProvider;
            }
        }

        #endregion

        #region METAView_Location_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Location_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Location_RetrievalProviderBase METAView_Location_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Location_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_LocationAssociation_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_LocationAssociation_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_LocationAssociation_ListingProviderBase METAView_LocationAssociation_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_LocationAssociation_ListingProvider;
            }
        }

        #endregion

        #region METAView_LocationAssociation_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_LocationAssociation_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_LocationAssociation_RetrievalProviderBase METAView_LocationAssociation_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_LocationAssociation_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_LocationScheme_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_LocationScheme_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_LocationScheme_ListingProviderBase METAView_LocationScheme_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_LocationScheme_ListingProvider;
            }
        }

        #endregion

        #region METAView_LocationScheme_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_LocationScheme_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_LocationScheme_RetrievalProviderBase METAView_LocationScheme_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_LocationScheme_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_LocationUnit_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_LocationUnit_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_LocationUnit_ListingProviderBase METAView_LocationUnit_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_LocationUnit_ListingProvider;
            }
        }

        #endregion

        #region METAView_LocationUnit_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_LocationUnit_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_LocationUnit_RetrievalProviderBase METAView_LocationUnit_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_LocationUnit_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Logic_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Logic_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Logic_ListingProviderBase METAView_Logic_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Logic_ListingProvider;
            }
        }

        #endregion

        #region METAView_Logic_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Logic_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Logic_RetrievalProviderBase METAView_Logic_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Logic_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_LogicalInformationArtefact_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_LogicalInformationArtefact_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_LogicalInformationArtefact_ListingProviderBase METAView_LogicalInformationArtefact_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_LogicalInformationArtefact_ListingProvider;
            }
        }

        #endregion

        #region METAView_LogicalInformationArtefact_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_LogicalInformationArtefact_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_LogicalInformationArtefact_RetrievalProviderBase METAView_LogicalInformationArtefact_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_LogicalInformationArtefact_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_LogicalITInfrastructureComponent_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_LogicalITInfrastructureComponent_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_LogicalITInfrastructureComponent_ListingProviderBase METAView_LogicalITInfrastructureComponent_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_LogicalITInfrastructureComponent_ListingProvider;
            }
        }

        #endregion

        #region METAView_LogicalITInfrastructureComponent_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_LogicalITInfrastructureComponent_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_LogicalITInfrastructureComponent_RetrievalProviderBase METAView_LogicalITInfrastructureComponent_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_LogicalITInfrastructureComponent_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_LogicalSoftwareComponent_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_LogicalSoftwareComponent_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_LogicalSoftwareComponent_ListingProviderBase METAView_LogicalSoftwareComponent_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_LogicalSoftwareComponent_ListingProvider;
            }
        }

        #endregion

        #region METAView_LogicalSoftwareComponent_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_LogicalSoftwareComponent_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_LogicalSoftwareComponent_RetrievalProviderBase METAView_LogicalSoftwareComponent_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_LogicalSoftwareComponent_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_MarketSegment_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_MarketSegment_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_MarketSegment_ListingProviderBase METAView_MarketSegment_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_MarketSegment_ListingProvider;
            }
        }

        #endregion

        #region METAView_MarketSegment_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_MarketSegment_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_MarketSegment_RetrievalProviderBase METAView_MarketSegment_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_MarketSegment_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Measure_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Measure_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Measure_ListingProviderBase METAView_Measure_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Measure_ListingProvider;
            }
        }

        #endregion

        #region METAView_Measure_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Measure_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Measure_RetrievalProviderBase METAView_Measure_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Measure_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_MeasurementItem_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_MeasurementItem_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_MeasurementItem_ListingProviderBase METAView_MeasurementItem_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_MeasurementItem_ListingProvider;
            }
        }

        #endregion

        #region METAView_MeasurementItem_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_MeasurementItem_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_MeasurementItem_RetrievalProviderBase METAView_MeasurementItem_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_MeasurementItem_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Model_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Model_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Model_ListingProviderBase METAView_Model_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Model_ListingProvider;
            }
        }

        #endregion

        #region METAView_Model_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Model_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Model_RetrievalProviderBase METAView_Model_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Model_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_ModelReal_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_ModelReal_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_ModelReal_ListingProviderBase METAView_ModelReal_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_ModelReal_ListingProvider;
            }
        }

        #endregion

        #region METAView_ModelReal_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_ModelReal_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_ModelReal_RetrievalProviderBase METAView_ModelReal_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_ModelReal_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_MutuallyExclusiveIndicator_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_MutuallyExclusiveIndicator_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_MutuallyExclusiveIndicator_ListingProviderBase METAView_MutuallyExclusiveIndicator_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_MutuallyExclusiveIndicator_ListingProvider;
            }
        }

        #endregion

        #region METAView_MutuallyExclusiveIndicator_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_MutuallyExclusiveIndicator_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_MutuallyExclusiveIndicator_RetrievalProviderBase METAView_MutuallyExclusiveIndicator_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_MutuallyExclusiveIndicator_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Network_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Network_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Network_ListingProviderBase METAView_Network_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Network_ListingProvider;
            }
        }

        #endregion

        #region METAView_Network_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Network_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Network_RetrievalProviderBase METAView_Network_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Network_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_NetworkComponent_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_NetworkComponent_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_NetworkComponent_ListingProviderBase METAView_NetworkComponent_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_NetworkComponent_ListingProvider;
            }
        }

        #endregion

        #region METAView_NetworkComponent_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_NetworkComponent_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_NetworkComponent_RetrievalProviderBase METAView_NetworkComponent_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_NetworkComponent_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Object_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Object_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Object_ListingProviderBase METAView_Object_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Object_ListingProvider;
            }
        }

        #endregion

        #region METAView_Object_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Object_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Object_RetrievalProviderBase METAView_Object_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Object_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_OrganizationalUnit_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_OrganizationalUnit_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_OrganizationalUnit_ListingProviderBase METAView_OrganizationalUnit_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_OrganizationalUnit_ListingProvider;
            }
        }

        #endregion

        #region METAView_OrganizationalUnit_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_OrganizationalUnit_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_OrganizationalUnit_RetrievalProviderBase METAView_OrganizationalUnit_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_OrganizationalUnit_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Peripheral_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Peripheral_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Peripheral_ListingProviderBase METAView_Peripheral_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Peripheral_ListingProvider;
            }
        }

        #endregion

        #region METAView_Peripheral_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Peripheral_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Peripheral_RetrievalProviderBase METAView_Peripheral_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Peripheral_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_PeripheralComponent_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_PeripheralComponent_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_PeripheralComponent_ListingProviderBase METAView_PeripheralComponent_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_PeripheralComponent_ListingProvider;
            }
        }

        #endregion

        #region METAView_PeripheralComponent_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_PeripheralComponent_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_PeripheralComponent_RetrievalProviderBase METAView_PeripheralComponent_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_PeripheralComponent_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Person_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Person_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Person_ListingProviderBase METAView_Person_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Person_ListingProvider;
            }
        }

        #endregion

        #region METAView_Person_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Person_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Person_RetrievalProviderBase METAView_Person_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Person_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_PhysicalDataComponent_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_PhysicalDataComponent_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_PhysicalDataComponent_ListingProviderBase METAView_PhysicalDataComponent_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_PhysicalDataComponent_ListingProvider;
            }
        }

        #endregion

        #region METAView_PhysicalDataComponent_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_PhysicalDataComponent_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_PhysicalDataComponent_RetrievalProviderBase METAView_PhysicalDataComponent_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_PhysicalDataComponent_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_PhysicalInformationArtefact_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_PhysicalInformationArtefact_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_PhysicalInformationArtefact_ListingProviderBase METAView_PhysicalInformationArtefact_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_PhysicalInformationArtefact_ListingProvider;
            }
        }

        #endregion

        #region METAView_PhysicalInformationArtefact_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_PhysicalInformationArtefact_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_PhysicalInformationArtefact_RetrievalProviderBase METAView_PhysicalInformationArtefact_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_PhysicalInformationArtefact_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_PhysicalSoftwareComponent_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_PhysicalSoftwareComponent_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_PhysicalSoftwareComponent_ListingProviderBase METAView_PhysicalSoftwareComponent_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_PhysicalSoftwareComponent_ListingProvider;
            }
        }

        #endregion

        #region METAView_PhysicalSoftwareComponent_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_PhysicalSoftwareComponent_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_PhysicalSoftwareComponent_RetrievalProviderBase METAView_PhysicalSoftwareComponent_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_PhysicalSoftwareComponent_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Position_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Position_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Position_ListingProviderBase METAView_Position_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Position_ListingProvider;
            }
        }

        #endregion

        #region METAView_Position_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Position_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Position_RetrievalProviderBase METAView_Position_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Position_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_PrimaryDataSourceIndicator_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_PrimaryDataSourceIndicator_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_PrimaryDataSourceIndicator_ListingProviderBase METAView_PrimaryDataSourceIndicator_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_PrimaryDataSourceIndicator_ListingProvider;
            }
        }

        #endregion

        #region METAView_PrimaryDataSourceIndicator_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_PrimaryDataSourceIndicator_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_PrimaryDataSourceIndicator_RetrievalProviderBase METAView_PrimaryDataSourceIndicator_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_PrimaryDataSourceIndicator_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_ProbabilityofRealization_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_ProbabilityofRealization_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_ProbabilityofRealization_ListingProviderBase METAView_ProbabilityofRealization_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_ProbabilityofRealization_ListingProvider;
            }
        }

        #endregion

        #region METAView_ProbabilityofRealization_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_ProbabilityofRealization_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_ProbabilityofRealization_RetrievalProviderBase METAView_ProbabilityofRealization_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_ProbabilityofRealization_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_ProbOfRealization_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_ProbOfRealization_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_ProbOfRealization_ListingProviderBase METAView_ProbOfRealization_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_ProbOfRealization_ListingProvider;
            }
        }

        #endregion

        #region METAView_ProbOfRealization_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_ProbOfRealization_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_ProbOfRealization_RetrievalProviderBase METAView_ProbOfRealization_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_ProbOfRealization_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Process_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Process_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Process_ListingProviderBase METAView_Process_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Process_ListingProvider;
            }
        }

        #endregion

        #region METAView_Process_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Process_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Process_RetrievalProviderBase METAView_Process_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Process_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Rationale_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Rationale_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Rationale_ListingProviderBase METAView_Rationale_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Rationale_ListingProvider;
            }
        }

        #endregion

        #region METAView_Rationale_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Rationale_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Rationale_RetrievalProviderBase METAView_Rationale_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Rationale_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Report_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Report_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Report_ListingProviderBase METAView_Report_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Report_ListingProvider;
            }
        }

        #endregion

        #region METAView_Report_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Report_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Report_RetrievalProviderBase METAView_Report_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Report_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Resource_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Resource_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Resource_ListingProviderBase METAView_Resource_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Resource_ListingProvider;
            }
        }

        #endregion

        #region METAView_Resource_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Resource_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Resource_RetrievalProviderBase METAView_Resource_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Resource_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_ResourceType_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_ResourceType_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_ResourceType_ListingProviderBase METAView_ResourceType_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_ResourceType_ListingProvider;
            }
        }

        #endregion

        #region METAView_ResourceType_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_ResourceType_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_ResourceType_RetrievalProviderBase METAView_ResourceType_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_ResourceType_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Responsibility_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Responsibility_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Responsibility_ListingProviderBase METAView_Responsibility_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Responsibility_ListingProvider;
            }
        }

        #endregion

        #region METAView_Responsibility_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Responsibility_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Responsibility_RetrievalProviderBase METAView_Responsibility_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Responsibility_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Risk_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Risk_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Risk_ListingProviderBase METAView_Risk_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Risk_ListingProvider;
            }
        }

        #endregion

        #region METAView_Risk_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Risk_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Risk_RetrievalProviderBase METAView_Risk_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Risk_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Role_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Role_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Role_ListingProviderBase METAView_Role_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Role_ListingProvider;
            }
        }

        #endregion

        #region METAView_Role_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Role_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Role_RetrievalProviderBase METAView_Role_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Role_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Scenario_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Scenario_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Scenario_ListingProviderBase METAView_Scenario_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Scenario_ListingProvider;
            }
        }

        #endregion

        #region METAView_Scenario_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Scenario_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Scenario_RetrievalProviderBase METAView_Scenario_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Scenario_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_SecurityClassification_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_SecurityClassification_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_SecurityClassification_ListingProviderBase METAView_SecurityClassification_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_SecurityClassification_ListingProvider;
            }
        }

        #endregion

        #region METAView_SecurityClassification_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_SecurityClassification_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_SecurityClassification_RetrievalProviderBase METAView_SecurityClassification_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_SecurityClassification_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_SelectorAttribute_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_SelectorAttribute_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_SelectorAttribute_ListingProviderBase METAView_SelectorAttribute_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_SelectorAttribute_ListingProvider;
            }
        }

        #endregion

        #region METAView_SelectorAttribute_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_SelectorAttribute_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_SelectorAttribute_RetrievalProviderBase METAView_SelectorAttribute_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_SelectorAttribute_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Skill_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Skill_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Skill_ListingProviderBase METAView_Skill_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Skill_ListingProvider;
            }
        }

        #endregion

        #region METAView_Skill_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Skill_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Skill_RetrievalProviderBase METAView_Skill_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Skill_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Software_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Software_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Software_ListingProviderBase METAView_Software_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Software_ListingProvider;
            }
        }

        #endregion

        #region METAView_Software_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Software_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Software_RetrievalProviderBase METAView_Software_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Software_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Stakeholder_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Stakeholder_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Stakeholder_ListingProviderBase METAView_Stakeholder_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Stakeholder_ListingProvider;
            }
        }

        #endregion

        #region METAView_Stakeholder_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Stakeholder_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Stakeholder_RetrievalProviderBase METAView_Stakeholder_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Stakeholder_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_StatusIndicator_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_StatusIndicator_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_StatusIndicator_ListingProviderBase METAView_StatusIndicator_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_StatusIndicator_ListingProvider;
            }
        }

        #endregion

        #region METAView_StatusIndicator_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_StatusIndicator_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_StatusIndicator_RetrievalProviderBase METAView_StatusIndicator_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_StatusIndicator_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_StorageComponent_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_StorageComponent_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_StorageComponent_ListingProviderBase METAView_StorageComponent_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_StorageComponent_ListingProvider;
            }
        }

        #endregion

        #region METAView_StorageComponent_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_StorageComponent_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_StorageComponent_RetrievalProviderBase METAView_StorageComponent_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_StorageComponent_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_StrategicStatement_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_StrategicStatement_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_StrategicStatement_ListingProviderBase METAView_StrategicStatement_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_StrategicStatement_ListingProvider;
            }
        }

        #endregion

        #region METAView_StrategicStatement_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_StrategicStatement_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_StrategicStatement_RetrievalProviderBase METAView_StrategicStatement_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_StrategicStatement_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_StrategicTheme_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_StrategicTheme_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_StrategicTheme_ListingProviderBase METAView_StrategicTheme_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_StrategicTheme_ListingProvider;
            }
        }

        #endregion

        #region METAView_StrategicTheme_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_StrategicTheme_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_StrategicTheme_RetrievalProviderBase METAView_StrategicTheme_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_StrategicTheme_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_SubsetIndicator_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_SubsetIndicator_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_SubsetIndicator_ListingProviderBase METAView_SubsetIndicator_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_SubsetIndicator_ListingProvider;
            }
        }

        #endregion

        #region METAView_SubsetIndicator_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_SubsetIndicator_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_SubsetIndicator_RetrievalProviderBase METAView_SubsetIndicator_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_SubsetIndicator_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Sys_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Sys_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Sys_ListingProviderBase METAView_Sys_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Sys_ListingProvider;
            }
        }

        #endregion

        #region METAView_Sys_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Sys_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Sys_RetrievalProviderBase METAView_Sys_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Sys_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_SystemComponent_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_SystemComponent_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_SystemComponent_ListingProviderBase METAView_SystemComponent_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_SystemComponent_ListingProvider;
            }
        }

        #endregion

        #region METAView_SystemComponent_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_SystemComponent_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_SystemComponent_RetrievalProviderBase METAView_SystemComponent_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_SystemComponent_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_Task_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Task_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Task_ListingProviderBase METAView_Task_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Task_ListingProvider;
            }
        }

        #endregion

        #region METAView_Task_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_Task_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_Task_RetrievalProviderBase METAView_Task_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_Task_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_TimeDifference_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_TimeDifference_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_TimeDifference_ListingProviderBase METAView_TimeDifference_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_TimeDifference_ListingProvider;
            }
        }

        #endregion

        #region METAView_TimeDifference_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_TimeDifference_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_TimeDifference_RetrievalProviderBase METAView_TimeDifference_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_TimeDifference_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_TimeIndicator_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_TimeIndicator_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_TimeIndicator_ListingProviderBase METAView_TimeIndicator_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_TimeIndicator_ListingProvider;
            }
        }

        #endregion

        #region METAView_TimeIndicator_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_TimeIndicator_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_TimeIndicator_RetrievalProviderBase METAView_TimeIndicator_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_TimeIndicator_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_TimeReference_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_TimeReference_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_TimeReference_ListingProviderBase METAView_TimeReference_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_TimeReference_ListingProvider;
            }
        }

        #endregion

        #region METAView_TimeReference_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_TimeReference_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_TimeReference_RetrievalProviderBase METAView_TimeReference_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_TimeReference_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_TimeScheme_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_TimeScheme_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_TimeScheme_ListingProviderBase METAView_TimeScheme_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_TimeScheme_ListingProvider;
            }
        }

        #endregion

        #region METAView_TimeScheme_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_TimeScheme_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_TimeScheme_RetrievalProviderBase METAView_TimeScheme_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_TimeScheme_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_TimeUnit_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_TimeUnit_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_TimeUnit_ListingProviderBase METAView_TimeUnit_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_TimeUnit_ListingProvider;
            }
        }

        #endregion

        #region METAView_TimeUnit_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_TimeUnit_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_TimeUnit_RetrievalProviderBase METAView_TimeUnit_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_TimeUnit_RetrievalProvider;
            }
        }

        #endregion

        #region METAView_WorkPackage_ListingProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_WorkPackage_Listing"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_WorkPackage_ListingProviderBase METAView_WorkPackage_ListingProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_WorkPackage_ListingProvider;
            }
        }

        #endregion

        #region METAView_WorkPackage_RetrievalProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="METAView_WorkPackage_Retrieval"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static METAView_WorkPackage_RetrievalProviderBase METAView_WorkPackage_RetrievalProvider
        {
            get
            {
                LoadProviders();
                return _provider.METAView_WorkPackage_RetrievalProvider;
            }
        }

        #endregion


        #region vw_FieldValueProvider

        ///<summary>
        /// Gets the current instance of the Data Access Logic Component for the <see cref="vw_FieldValue"/> business entity.
        /// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
        ///</summary>
        public static vw_FieldValueProviderBase vw_FieldValueProvider
        {
            get
            {
                LoadProviders();
                return _provider.vw_FieldValueProvider;
            }
        }

        #endregion

        #endregion

        private static TList<Field> clientFields;
        public static TList<Field> ClientFields
        {
            get
            {
                if (clientFields == null)
                    clientFields = FieldProvider.GetAll();
                return clientFields;
            }
            set
            {
                clientFields = value;
            }
        }
        public static TList<Field> ClientFieldsByClass(string className)
        {
            return ClientFields.FindAll(FieldColumn.Class, className);
        }

        private static TList<Class> clientClasses;
        private static TList<Class> serverClasses;
        public static TList<Class> Classes(string provider)
        {
            switch (provider)
            {
                case "Server":
                    if (serverClasses == null)
                        serverClasses = Connections[provider].Provider.ClassProvider.GetAll();
                    return serverClasses;
                default:
                    if (clientClasses == null)
                        clientClasses = ClassProvider.GetAll();
                    return clientClasses;
            }
            return null;
        }

        private static TList<DomainDefinition> clientDomains;
        private static TList<DomainDefinition> serverDomains;
        public static TList<DomainDefinition> Domains(string provider)
        {
            switch (provider)
            {
                case "Server":
                    if (serverDomains == null)
                        serverDomains = Connections[provider].Provider.DomainDefinitionProvider.GetAll();
                    return serverDomains;
                default:
                    if (clientDomains == null)
                        clientDomains = DomainDefinitionProvider.GetAll();
                    return clientDomains;
            }
            return null;
        }

        private static TList<UURI> uris;
        public static TList<UURI> UUris
        {
            get
            {
                if (uris == null)
                {
                    uris = UURIProvider.GetAll();
                }
                return uris;
            }
        }
        public static UURI GetUUri(int id)
        {
            foreach (UURI u in UUris)
                if (u.pkid == id)
                    return u;

            return null;
        }
        public static UURI GetUUri(int? id)
        {
            if (id != null)
                foreach (UURI u in UUris)
                    if (u.pkid == id)
                        return u;

            return null;
        }
    }

    #region Query/Filters

    #region VCStatusFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="VCStatus"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class VCStatusFilters : VCStatusFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the VCStatusFilters class.
        /// </summary>
        public VCStatusFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the VCStatusFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public VCStatusFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the VCStatusFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public VCStatusFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion VCStatusFilters

    #region VCStatusQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="VCStatusParameterBuilder"/> class
    /// that is used exclusively with a <see cref="VCStatus"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class VCStatusQuery : VCStatusParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the VCStatusQuery class.
        /// </summary>
        public VCStatusQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the VCStatusQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public VCStatusQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the VCStatusQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public VCStatusQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion VCStatusQuery

    #region DomainDefinitionPossibleValueFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="DomainDefinitionPossibleValue"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class DomainDefinitionPossibleValueFilters : DomainDefinitionPossibleValueFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DomainDefinitionPossibleValueFilters class.
        /// </summary>
        public DomainDefinitionPossibleValueFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the DomainDefinitionPossibleValueFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public DomainDefinitionPossibleValueFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the DomainDefinitionPossibleValueFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public DomainDefinitionPossibleValueFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion DomainDefinitionPossibleValueFilters

    #region DomainDefinitionPossibleValueQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="DomainDefinitionPossibleValueParameterBuilder"/> class
    /// that is used exclusively with a <see cref="DomainDefinitionPossibleValue"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class DomainDefinitionPossibleValueQuery : DomainDefinitionPossibleValueParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DomainDefinitionPossibleValueQuery class.
        /// </summary>
        public DomainDefinitionPossibleValueQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the DomainDefinitionPossibleValueQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public DomainDefinitionPossibleValueQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the DomainDefinitionPossibleValueQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public DomainDefinitionPossibleValueQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion DomainDefinitionPossibleValueQuery

    #region WorkspaceFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="Workspace"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class WorkspaceFilters : WorkspaceFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the WorkspaceFilters class.
        /// </summary>
        public WorkspaceFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the WorkspaceFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public WorkspaceFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the WorkspaceFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public WorkspaceFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion WorkspaceFilters

    #region WorkspaceQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="WorkspaceParameterBuilder"/> class
    /// that is used exclusively with a <see cref="Workspace"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class WorkspaceQuery : WorkspaceParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the WorkspaceQuery class.
        /// </summary>
        public WorkspaceQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the WorkspaceQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public WorkspaceQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the WorkspaceQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public WorkspaceQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion WorkspaceQuery

    #region ClassTypeFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="ClassType"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class ClassTypeFilters : ClassTypeFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ClassTypeFilters class.
        /// </summary>
        public ClassTypeFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the ClassTypeFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public ClassTypeFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the ClassTypeFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public ClassTypeFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion ClassTypeFilters

    #region ClassTypeQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="ClassTypeParameterBuilder"/> class
    /// that is used exclusively with a <see cref="ClassType"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class ClassTypeQuery : ClassTypeParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ClassTypeQuery class.
        /// </summary>
        public ClassTypeQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the ClassTypeQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public ClassTypeQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the ClassTypeQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public ClassTypeQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion ClassTypeQuery

    #region WorkspaceTypeFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="WorkspaceType"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class WorkspaceTypeFilters : WorkspaceTypeFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the WorkspaceTypeFilters class.
        /// </summary>
        public WorkspaceTypeFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the WorkspaceTypeFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public WorkspaceTypeFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the WorkspaceTypeFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public WorkspaceTypeFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion WorkspaceTypeFilters

    #region WorkspaceTypeQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="WorkspaceTypeParameterBuilder"/> class
    /// that is used exclusively with a <see cref="WorkspaceType"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class WorkspaceTypeQuery : WorkspaceTypeParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the WorkspaceTypeQuery class.
        /// </summary>
        public WorkspaceTypeQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the WorkspaceTypeQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public WorkspaceTypeQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the WorkspaceTypeQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public WorkspaceTypeQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion WorkspaceTypeQuery

    #region UserFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="User"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class UserFilters : UserFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the UserFilters class.
        /// </summary>
        public UserFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the UserFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public UserFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the UserFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public UserFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion UserFilters

    #region UserQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="UserParameterBuilder"/> class
    /// that is used exclusively with a <see cref="User"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class UserQuery : UserParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the UserQuery class.
        /// </summary>
        public UserQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the UserQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public UserQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the UserQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public UserQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion UserQuery

    #region GraphFileFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="GraphFile"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class GraphFileFilters : GraphFileFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the GraphFileFilters class.
        /// </summary>
        public GraphFileFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the GraphFileFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public GraphFileFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the GraphFileFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public GraphFileFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion GraphFileFilters

    #region GraphFileQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="GraphFileParameterBuilder"/> class
    /// that is used exclusively with a <see cref="GraphFile"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class GraphFileQuery : GraphFileParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the GraphFileQuery class.
        /// </summary>
        public GraphFileQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the GraphFileQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public GraphFileQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the GraphFileQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public GraphFileQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion GraphFileQuery

    #region ClassFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="Class"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class ClassFilters : ClassFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ClassFilters class.
        /// </summary>
        public ClassFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the ClassFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public ClassFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the ClassFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public ClassFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion ClassFilters

    #region ClassQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="ClassParameterBuilder"/> class
    /// that is used exclusively with a <see cref="Class"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class ClassQuery : ClassParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ClassQuery class.
        /// </summary>
        public ClassQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the ClassQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public ClassQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the ClassQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public ClassQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion ClassQuery

    #region MetaObjectFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="MetaObject"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class MetaObjectFilters : MetaObjectFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MetaObjectFilters class.
        /// </summary>
        public MetaObjectFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the MetaObjectFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public MetaObjectFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the MetaObjectFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public MetaObjectFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion MetaObjectFilters

    #region MetaObjectQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="MetaObjectParameterBuilder"/> class
    /// that is used exclusively with a <see cref="MetaObject"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class MetaObjectQuery : MetaObjectParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MetaObjectQuery class.
        /// </summary>
        public MetaObjectQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the MetaObjectQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public MetaObjectQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the MetaObjectQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public MetaObjectQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion MetaObjectQuery

    #region UserPermissionFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="UserPermission"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class UserPermissionFilters : UserPermissionFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the UserPermissionFilters class.
        /// </summary>
        public UserPermissionFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the UserPermissionFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public UserPermissionFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the UserPermissionFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public UserPermissionFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion UserPermissionFilters

    #region UserPermissionQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="UserPermissionParameterBuilder"/> class
    /// that is used exclusively with a <see cref="UserPermission"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class UserPermissionQuery : UserPermissionParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the UserPermissionQuery class.
        /// </summary>
        public UserPermissionQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the UserPermissionQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public UserPermissionQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the UserPermissionQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public UserPermissionQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion UserPermissionQuery

    #region ConfigFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="Config"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class ConfigFilters : ConfigFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ConfigFilters class.
        /// </summary>
        public ConfigFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the ConfigFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public ConfigFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the ConfigFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public ConfigFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion ConfigFilters

    #region ConfigQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="ConfigParameterBuilder"/> class
    /// that is used exclusively with a <see cref="Config"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class ConfigQuery : ConfigParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ConfigQuery class.
        /// </summary>
        public ConfigQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the ConfigQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public ConfigQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the ConfigQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public ConfigQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion ConfigQuery

    #region AssociationTypeFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="AssociationType"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class AssociationTypeFilters : AssociationTypeFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the AssociationTypeFilters class.
        /// </summary>
        public AssociationTypeFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the AssociationTypeFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public AssociationTypeFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the AssociationTypeFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public AssociationTypeFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion AssociationTypeFilters

    #region AssociationTypeQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="AssociationTypeParameterBuilder"/> class
    /// that is used exclusively with a <see cref="AssociationType"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class AssociationTypeQuery : AssociationTypeParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the AssociationTypeQuery class.
        /// </summary>
        public AssociationTypeQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the AssociationTypeQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public AssociationTypeQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the AssociationTypeQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public AssociationTypeQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion AssociationTypeQuery

    #region ClassAssociationFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="ClassAssociation"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class ClassAssociationFilters : ClassAssociationFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ClassAssociationFilters class.
        /// </summary>
        public ClassAssociationFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the ClassAssociationFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public ClassAssociationFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the ClassAssociationFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public ClassAssociationFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion ClassAssociationFilters

    #region ClassAssociationQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="ClassAssociationParameterBuilder"/> class
    /// that is used exclusively with a <see cref="ClassAssociation"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class ClassAssociationQuery : ClassAssociationParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ClassAssociationQuery class.
        /// </summary>
        public ClassAssociationQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the ClassAssociationQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public ClassAssociationQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the ClassAssociationQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public ClassAssociationQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion ClassAssociationQuery

    #region ObjectAssociationFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="ObjectAssociation"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class ObjectAssociationFilters : ObjectAssociationFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ObjectAssociationFilters class.
        /// </summary>
        public ObjectAssociationFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the ObjectAssociationFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public ObjectAssociationFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the ObjectAssociationFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public ObjectAssociationFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion ObjectAssociationFilters

    #region ObjectAssociationQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="ObjectAssociationParameterBuilder"/> class
    /// that is used exclusively with a <see cref="ObjectAssociation"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class ObjectAssociationQuery : ObjectAssociationParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ObjectAssociationQuery class.
        /// </summary>
        public ObjectAssociationQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the ObjectAssociationQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public ObjectAssociationQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the ObjectAssociationQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public ObjectAssociationQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion ObjectAssociationQuery

    #region FileTypeFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="FileType"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class FileTypeFilters : FileTypeFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the FileTypeFilters class.
        /// </summary>
        public FileTypeFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the FileTypeFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public FileTypeFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the FileTypeFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public FileTypeFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion FileTypeFilters

    #region FileTypeQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="FileTypeParameterBuilder"/> class
    /// that is used exclusively with a <see cref="FileType"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class FileTypeQuery : FileTypeParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the FileTypeQuery class.
        /// </summary>
        public FileTypeQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the FileTypeQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public FileTypeQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the FileTypeQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public FileTypeQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion FileTypeQuery

    #region ObjectFieldValueFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="ObjectFieldValue"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class ObjectFieldValueFilters : ObjectFieldValueFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ObjectFieldValueFilters class.
        /// </summary>
        public ObjectFieldValueFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the ObjectFieldValueFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public ObjectFieldValueFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the ObjectFieldValueFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public ObjectFieldValueFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion ObjectFieldValueFilters

    #region ObjectFieldValueQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="ObjectFieldValueParameterBuilder"/> class
    /// that is used exclusively with a <see cref="ObjectFieldValue"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class ObjectFieldValueQuery : ObjectFieldValueParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ObjectFieldValueQuery class.
        /// </summary>
        public ObjectFieldValueQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the ObjectFieldValueQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public ObjectFieldValueQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the ObjectFieldValueQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public ObjectFieldValueQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion ObjectFieldValueQuery

    #region GraphFileAssociationFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="GraphFileAssociation"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class GraphFileAssociationFilters : GraphFileAssociationFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the GraphFileAssociationFilters class.
        /// </summary>
        public GraphFileAssociationFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the GraphFileAssociationFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public GraphFileAssociationFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the GraphFileAssociationFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public GraphFileAssociationFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion GraphFileAssociationFilters

    #region GraphFileAssociationQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="GraphFileAssociationParameterBuilder"/> class
    /// that is used exclusively with a <see cref="GraphFileAssociation"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class GraphFileAssociationQuery : GraphFileAssociationParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the GraphFileAssociationQuery class.
        /// </summary>
        public GraphFileAssociationQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the GraphFileAssociationQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public GraphFileAssociationQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the GraphFileAssociationQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public GraphFileAssociationQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion GraphFileAssociationQuery

    #region AllowedArtifactFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="AllowedArtifact"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class AllowedArtifactFilters : AllowedArtifactFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the AllowedArtifactFilters class.
        /// </summary>
        public AllowedArtifactFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the AllowedArtifactFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public AllowedArtifactFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the AllowedArtifactFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public AllowedArtifactFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion AllowedArtifactFilters

    #region AllowedArtifactQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="AllowedArtifactParameterBuilder"/> class
    /// that is used exclusively with a <see cref="AllowedArtifact"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class AllowedArtifactQuery : AllowedArtifactParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the AllowedArtifactQuery class.
        /// </summary>
        public AllowedArtifactQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the AllowedArtifactQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public AllowedArtifactQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the AllowedArtifactQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public AllowedArtifactQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion AllowedArtifactQuery

    #region DomainDefinitionFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="DomainDefinition"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class DomainDefinitionFilters : DomainDefinitionFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DomainDefinitionFilters class.
        /// </summary>
        public DomainDefinitionFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the DomainDefinitionFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public DomainDefinitionFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the DomainDefinitionFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public DomainDefinitionFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion DomainDefinitionFilters

    #region DomainDefinitionQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="DomainDefinitionParameterBuilder"/> class
    /// that is used exclusively with a <see cref="DomainDefinition"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class DomainDefinitionQuery : DomainDefinitionParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DomainDefinitionQuery class.
        /// </summary>
        public DomainDefinitionQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the DomainDefinitionQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public DomainDefinitionQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the DomainDefinitionQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public DomainDefinitionQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion DomainDefinitionQuery

    #region ArtifactFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="Artifact"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class ArtifactFilters : ArtifactFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ArtifactFilters class.
        /// </summary>
        public ArtifactFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the ArtifactFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public ArtifactFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the ArtifactFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public ArtifactFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion ArtifactFilters

    #region ArtifactQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="ArtifactParameterBuilder"/> class
    /// that is used exclusively with a <see cref="Artifact"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class ArtifactQuery : ArtifactParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ArtifactQuery class.
        /// </summary>
        public ArtifactQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the ArtifactQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public ArtifactQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the ArtifactQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public ArtifactQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion ArtifactQuery

    #region PermissionFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="Permission"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class PermissionFilters : PermissionFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the PermissionFilters class.
        /// </summary>
        public PermissionFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the PermissionFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public PermissionFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the PermissionFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public PermissionFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion PermissionFilters

    #region PermissionQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="PermissionParameterBuilder"/> class
    /// that is used exclusively with a <see cref="Permission"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class PermissionQuery : PermissionParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the PermissionQuery class.
        /// </summary>
        public PermissionQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the PermissionQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public PermissionQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the PermissionQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public PermissionQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion PermissionQuery

    #region GraphFileObjectFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="GraphFileObject"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class GraphFileObjectFilters : GraphFileObjectFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the GraphFileObjectFilters class.
        /// </summary>
        public GraphFileObjectFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the GraphFileObjectFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public GraphFileObjectFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the GraphFileObjectFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public GraphFileObjectFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion GraphFileObjectFilters

    #region GraphFileObjectQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="GraphFileObjectParameterBuilder"/> class
    /// that is used exclusively with a <see cref="GraphFileObject"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class GraphFileObjectQuery : GraphFileObjectParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the GraphFileObjectQuery class.
        /// </summary>
        public GraphFileObjectQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the GraphFileObjectQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public GraphFileObjectQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the GraphFileObjectQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public GraphFileObjectQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion GraphFileObjectQuery

    #region FieldFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="Field"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class FieldFilters : FieldFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the FieldFilters class.
        /// </summary>
        public FieldFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the FieldFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public FieldFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the FieldFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public FieldFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion FieldFilters

    #region FieldQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="FieldParameterBuilder"/> class
    /// that is used exclusively with a <see cref="Field"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class FieldQuery : FieldParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the FieldQuery class.
        /// </summary>
        public FieldQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the FieldQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public FieldQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the FieldQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public FieldQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion FieldQuery

    #region METAView_Activity_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Activity_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Activity_ListingFilters : METAView_Activity_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Activity_ListingFilters class.
        /// </summary>
        public METAView_Activity_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Activity_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Activity_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Activity_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Activity_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Activity_ListingFilters

    #region METAView_Activity_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Activity_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Activity_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Activity_ListingQuery : METAView_Activity_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Activity_ListingQuery class.
        /// </summary>
        public METAView_Activity_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Activity_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Activity_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Activity_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Activity_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Activity_ListingQuery

    #region METAView_Activity_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Activity_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Activity_RetrievalFilters : METAView_Activity_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Activity_RetrievalFilters class.
        /// </summary>
        public METAView_Activity_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Activity_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Activity_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Activity_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Activity_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Activity_RetrievalFilters

    #region METAView_Activity_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Activity_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Activity_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Activity_RetrievalQuery : METAView_Activity_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Activity_RetrievalQuery class.
        /// </summary>
        public METAView_Activity_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Activity_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Activity_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Activity_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Activity_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Activity_RetrievalQuery

    #region METAView_ApplicationInterface_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_ApplicationInterface_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ApplicationInterface_ListingFilters : METAView_ApplicationInterface_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ApplicationInterface_ListingFilters class.
        /// </summary>
        public METAView_ApplicationInterface_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ApplicationInterface_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ApplicationInterface_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ApplicationInterface_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ApplicationInterface_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ApplicationInterface_ListingFilters

    #region METAView_ApplicationInterface_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_ApplicationInterface_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_ApplicationInterface_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ApplicationInterface_ListingQuery : METAView_ApplicationInterface_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ApplicationInterface_ListingQuery class.
        /// </summary>
        public METAView_ApplicationInterface_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ApplicationInterface_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ApplicationInterface_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ApplicationInterface_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ApplicationInterface_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ApplicationInterface_ListingQuery

    #region METAView_ApplicationInterface_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_ApplicationInterface_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ApplicationInterface_RetrievalFilters : METAView_ApplicationInterface_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ApplicationInterface_RetrievalFilters class.
        /// </summary>
        public METAView_ApplicationInterface_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ApplicationInterface_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ApplicationInterface_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ApplicationInterface_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ApplicationInterface_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ApplicationInterface_RetrievalFilters

    #region METAView_ApplicationInterface_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_ApplicationInterface_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_ApplicationInterface_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ApplicationInterface_RetrievalQuery : METAView_ApplicationInterface_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ApplicationInterface_RetrievalQuery class.
        /// </summary>
        public METAView_ApplicationInterface_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ApplicationInterface_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ApplicationInterface_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ApplicationInterface_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ApplicationInterface_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ApplicationInterface_RetrievalQuery

    #region METAView_Assumption_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Assumption_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Assumption_ListingFilters : METAView_Assumption_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Assumption_ListingFilters class.
        /// </summary>
        public METAView_Assumption_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Assumption_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Assumption_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Assumption_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Assumption_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Assumption_ListingFilters

    #region METAView_Assumption_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Assumption_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Assumption_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Assumption_ListingQuery : METAView_Assumption_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Assumption_ListingQuery class.
        /// </summary>
        public METAView_Assumption_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Assumption_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Assumption_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Assumption_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Assumption_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Assumption_ListingQuery

    #region METAView_Assumption_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Assumption_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Assumption_RetrievalFilters : METAView_Assumption_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Assumption_RetrievalFilters class.
        /// </summary>
        public METAView_Assumption_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Assumption_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Assumption_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Assumption_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Assumption_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Assumption_RetrievalFilters

    #region METAView_Assumption_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Assumption_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Assumption_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Assumption_RetrievalQuery : METAView_Assumption_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Assumption_RetrievalQuery class.
        /// </summary>
        public METAView_Assumption_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Assumption_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Assumption_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Assumption_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Assumption_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Assumption_RetrievalQuery

    #region METAView_Attribute_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Attribute_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Attribute_ListingFilters : METAView_Attribute_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Attribute_ListingFilters class.
        /// </summary>
        public METAView_Attribute_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Attribute_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Attribute_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Attribute_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Attribute_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Attribute_ListingFilters

    #region METAView_Attribute_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Attribute_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Attribute_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Attribute_ListingQuery : METAView_Attribute_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Attribute_ListingQuery class.
        /// </summary>
        public METAView_Attribute_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Attribute_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Attribute_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Attribute_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Attribute_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Attribute_ListingQuery

    #region METAView_Attribute_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Attribute_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Attribute_RetrievalFilters : METAView_Attribute_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Attribute_RetrievalFilters class.
        /// </summary>
        public METAView_Attribute_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Attribute_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Attribute_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Attribute_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Attribute_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Attribute_RetrievalFilters

    #region METAView_Attribute_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Attribute_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Attribute_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Attribute_RetrievalQuery : METAView_Attribute_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Attribute_RetrievalQuery class.
        /// </summary>
        public METAView_Attribute_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Attribute_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Attribute_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Attribute_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Attribute_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Attribute_RetrievalQuery

    #region METAView_BusinessCompetency_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_BusinessCompetency_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_BusinessCompetency_ListingFilters : METAView_BusinessCompetency_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_BusinessCompetency_ListingFilters class.
        /// </summary>
        public METAView_BusinessCompetency_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_BusinessCompetency_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_BusinessCompetency_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_BusinessCompetency_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_BusinessCompetency_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_BusinessCompetency_ListingFilters

    #region METAView_BusinessCompetency_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_BusinessCompetency_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_BusinessCompetency_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_BusinessCompetency_ListingQuery : METAView_BusinessCompetency_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_BusinessCompetency_ListingQuery class.
        /// </summary>
        public METAView_BusinessCompetency_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_BusinessCompetency_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_BusinessCompetency_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_BusinessCompetency_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_BusinessCompetency_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_BusinessCompetency_ListingQuery

    #region METAView_BusinessCompetency_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_BusinessCompetency_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_BusinessCompetency_RetrievalFilters : METAView_BusinessCompetency_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_BusinessCompetency_RetrievalFilters class.
        /// </summary>
        public METAView_BusinessCompetency_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_BusinessCompetency_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_BusinessCompetency_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_BusinessCompetency_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_BusinessCompetency_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_BusinessCompetency_RetrievalFilters

    #region METAView_BusinessCompetency_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_BusinessCompetency_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_BusinessCompetency_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_BusinessCompetency_RetrievalQuery : METAView_BusinessCompetency_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_BusinessCompetency_RetrievalQuery class.
        /// </summary>
        public METAView_BusinessCompetency_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_BusinessCompetency_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_BusinessCompetency_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_BusinessCompetency_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_BusinessCompetency_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_BusinessCompetency_RetrievalQuery

    #region METAView_BusinessInterface_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_BusinessInterface_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_BusinessInterface_ListingFilters : METAView_BusinessInterface_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_BusinessInterface_ListingFilters class.
        /// </summary>
        public METAView_BusinessInterface_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_BusinessInterface_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_BusinessInterface_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_BusinessInterface_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_BusinessInterface_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_BusinessInterface_ListingFilters

    #region METAView_BusinessInterface_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_BusinessInterface_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_BusinessInterface_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_BusinessInterface_ListingQuery : METAView_BusinessInterface_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_BusinessInterface_ListingQuery class.
        /// </summary>
        public METAView_BusinessInterface_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_BusinessInterface_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_BusinessInterface_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_BusinessInterface_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_BusinessInterface_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_BusinessInterface_ListingQuery

    #region METAView_BusinessInterface_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_BusinessInterface_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_BusinessInterface_RetrievalFilters : METAView_BusinessInterface_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_BusinessInterface_RetrievalFilters class.
        /// </summary>
        public METAView_BusinessInterface_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_BusinessInterface_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_BusinessInterface_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_BusinessInterface_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_BusinessInterface_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_BusinessInterface_RetrievalFilters

    #region METAView_BusinessInterface_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_BusinessInterface_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_BusinessInterface_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_BusinessInterface_RetrievalQuery : METAView_BusinessInterface_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_BusinessInterface_RetrievalQuery class.
        /// </summary>
        public METAView_BusinessInterface_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_BusinessInterface_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_BusinessInterface_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_BusinessInterface_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_BusinessInterface_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_BusinessInterface_RetrievalQuery

    #region METAView_CAD_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_CAD_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_CAD_ListingFilters : METAView_CAD_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_CAD_ListingFilters class.
        /// </summary>
        public METAView_CAD_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_CAD_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_CAD_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_CAD_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_CAD_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_CAD_ListingFilters

    #region METAView_CAD_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_CAD_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_CAD_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_CAD_ListingQuery : METAView_CAD_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_CAD_ListingQuery class.
        /// </summary>
        public METAView_CAD_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_CAD_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_CAD_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_CAD_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_CAD_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_CAD_ListingQuery

    #region METAView_CAD_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_CAD_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_CAD_RetrievalFilters : METAView_CAD_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_CAD_RetrievalFilters class.
        /// </summary>
        public METAView_CAD_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_CAD_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_CAD_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_CAD_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_CAD_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_CAD_RetrievalFilters

    #region METAView_CAD_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_CAD_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_CAD_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_CAD_RetrievalQuery : METAView_CAD_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_CAD_RetrievalQuery class.
        /// </summary>
        public METAView_CAD_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_CAD_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_CAD_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_CAD_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_CAD_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_CAD_RetrievalQuery

    #region METAView_CADReal_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_CADReal_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_CADReal_ListingFilters : METAView_CADReal_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_CADReal_ListingFilters class.
        /// </summary>
        public METAView_CADReal_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_CADReal_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_CADReal_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_CADReal_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_CADReal_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_CADReal_ListingFilters

    #region METAView_CADReal_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_CADReal_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_CADReal_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_CADReal_ListingQuery : METAView_CADReal_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_CADReal_ListingQuery class.
        /// </summary>
        public METAView_CADReal_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_CADReal_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_CADReal_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_CADReal_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_CADReal_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_CADReal_ListingQuery

    #region METAView_CADReal_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_CADReal_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_CADReal_RetrievalFilters : METAView_CADReal_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_CADReal_RetrievalFilters class.
        /// </summary>
        public METAView_CADReal_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_CADReal_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_CADReal_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_CADReal_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_CADReal_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_CADReal_RetrievalFilters

    #region METAView_CADReal_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_CADReal_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_CADReal_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_CADReal_RetrievalQuery : METAView_CADReal_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_CADReal_RetrievalQuery class.
        /// </summary>
        public METAView_CADReal_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_CADReal_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_CADReal_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_CADReal_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_CADReal_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_CADReal_RetrievalQuery

    #region METAView_CategoryFactor_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_CategoryFactor_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_CategoryFactor_ListingFilters : METAView_CategoryFactor_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_CategoryFactor_ListingFilters class.
        /// </summary>
        public METAView_CategoryFactor_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_CategoryFactor_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_CategoryFactor_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_CategoryFactor_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_CategoryFactor_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_CategoryFactor_ListingFilters

    #region METAView_CategoryFactor_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_CategoryFactor_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_CategoryFactor_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_CategoryFactor_ListingQuery : METAView_CategoryFactor_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_CategoryFactor_ListingQuery class.
        /// </summary>
        public METAView_CategoryFactor_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_CategoryFactor_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_CategoryFactor_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_CategoryFactor_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_CategoryFactor_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_CategoryFactor_ListingQuery

    #region METAView_CategoryFactor_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_CategoryFactor_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_CategoryFactor_RetrievalFilters : METAView_CategoryFactor_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_CategoryFactor_RetrievalFilters class.
        /// </summary>
        public METAView_CategoryFactor_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_CategoryFactor_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_CategoryFactor_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_CategoryFactor_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_CategoryFactor_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_CategoryFactor_RetrievalFilters

    #region METAView_CategoryFactor_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_CategoryFactor_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_CategoryFactor_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_CategoryFactor_RetrievalQuery : METAView_CategoryFactor_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_CategoryFactor_RetrievalQuery class.
        /// </summary>
        public METAView_CategoryFactor_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_CategoryFactor_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_CategoryFactor_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_CategoryFactor_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_CategoryFactor_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_CategoryFactor_RetrievalQuery

    #region METAView_Channel_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Channel_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Channel_ListingFilters : METAView_Channel_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Channel_ListingFilters class.
        /// </summary>
        public METAView_Channel_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Channel_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Channel_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Channel_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Channel_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Channel_ListingFilters

    #region METAView_Channel_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Channel_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Channel_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Channel_ListingQuery : METAView_Channel_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Channel_ListingQuery class.
        /// </summary>
        public METAView_Channel_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Channel_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Channel_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Channel_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Channel_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Channel_ListingQuery

    #region METAView_Channel_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Channel_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Channel_RetrievalFilters : METAView_Channel_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Channel_RetrievalFilters class.
        /// </summary>
        public METAView_Channel_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Channel_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Channel_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Channel_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Channel_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Channel_RetrievalFilters

    #region METAView_Channel_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Channel_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Channel_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Channel_RetrievalQuery : METAView_Channel_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Channel_RetrievalQuery class.
        /// </summary>
        public METAView_Channel_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Channel_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Channel_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Channel_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Channel_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Channel_RetrievalQuery

    #region METAView_Competency_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Competency_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Competency_ListingFilters : METAView_Competency_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Competency_ListingFilters class.
        /// </summary>
        public METAView_Competency_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Competency_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Competency_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Competency_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Competency_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Competency_ListingFilters

    #region METAView_Competency_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Competency_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Competency_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Competency_ListingQuery : METAView_Competency_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Competency_ListingQuery class.
        /// </summary>
        public METAView_Competency_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Competency_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Competency_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Competency_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Competency_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Competency_ListingQuery

    #region METAView_Competency_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Competency_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Competency_RetrievalFilters : METAView_Competency_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Competency_RetrievalFilters class.
        /// </summary>
        public METAView_Competency_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Competency_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Competency_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Competency_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Competency_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Competency_RetrievalFilters

    #region METAView_Competency_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Competency_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Competency_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Competency_RetrievalQuery : METAView_Competency_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Competency_RetrievalQuery class.
        /// </summary>
        public METAView_Competency_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Competency_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Competency_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Competency_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Competency_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Competency_RetrievalQuery

    #region METAView_ComputingComponent_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_ComputingComponent_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ComputingComponent_ListingFilters : METAView_ComputingComponent_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ComputingComponent_ListingFilters class.
        /// </summary>
        public METAView_ComputingComponent_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ComputingComponent_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ComputingComponent_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ComputingComponent_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ComputingComponent_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ComputingComponent_ListingFilters

    #region METAView_ComputingComponent_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_ComputingComponent_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_ComputingComponent_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ComputingComponent_ListingQuery : METAView_ComputingComponent_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ComputingComponent_ListingQuery class.
        /// </summary>
        public METAView_ComputingComponent_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ComputingComponent_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ComputingComponent_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ComputingComponent_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ComputingComponent_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ComputingComponent_ListingQuery

    #region METAView_ComputingComponent_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_ComputingComponent_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ComputingComponent_RetrievalFilters : METAView_ComputingComponent_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ComputingComponent_RetrievalFilters class.
        /// </summary>
        public METAView_ComputingComponent_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ComputingComponent_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ComputingComponent_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ComputingComponent_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ComputingComponent_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ComputingComponent_RetrievalFilters

    #region METAView_ComputingComponent_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_ComputingComponent_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_ComputingComponent_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ComputingComponent_RetrievalQuery : METAView_ComputingComponent_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ComputingComponent_RetrievalQuery class.
        /// </summary>
        public METAView_ComputingComponent_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ComputingComponent_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ComputingComponent_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ComputingComponent_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ComputingComponent_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ComputingComponent_RetrievalQuery

    #region METAView_Condition_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Condition_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Condition_ListingFilters : METAView_Condition_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Condition_ListingFilters class.
        /// </summary>
        public METAView_Condition_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Condition_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Condition_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Condition_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Condition_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Condition_ListingFilters

    #region METAView_Condition_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Condition_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Condition_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Condition_ListingQuery : METAView_Condition_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Condition_ListingQuery class.
        /// </summary>
        public METAView_Condition_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Condition_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Condition_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Condition_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Condition_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Condition_ListingQuery

    #region METAView_Condition_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Condition_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Condition_RetrievalFilters : METAView_Condition_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Condition_RetrievalFilters class.
        /// </summary>
        public METAView_Condition_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Condition_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Condition_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Condition_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Condition_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Condition_RetrievalFilters

    #region METAView_Condition_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Condition_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Condition_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Condition_RetrievalQuery : METAView_Condition_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Condition_RetrievalQuery class.
        /// </summary>
        public METAView_Condition_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Condition_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Condition_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Condition_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Condition_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Condition_RetrievalQuery

    #region METAView_Conditional_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Conditional_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Conditional_ListingFilters : METAView_Conditional_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Conditional_ListingFilters class.
        /// </summary>
        public METAView_Conditional_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Conditional_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Conditional_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Conditional_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Conditional_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Conditional_ListingFilters

    #region METAView_Conditional_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Conditional_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Conditional_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Conditional_ListingQuery : METAView_Conditional_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Conditional_ListingQuery class.
        /// </summary>
        public METAView_Conditional_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Conditional_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Conditional_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Conditional_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Conditional_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Conditional_ListingQuery

    #region METAView_Conditional_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Conditional_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Conditional_RetrievalFilters : METAView_Conditional_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Conditional_RetrievalFilters class.
        /// </summary>
        public METAView_Conditional_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Conditional_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Conditional_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Conditional_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Conditional_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Conditional_RetrievalFilters

    #region METAView_Conditional_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Conditional_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Conditional_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Conditional_RetrievalQuery : METAView_Conditional_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Conditional_RetrievalQuery class.
        /// </summary>
        public METAView_Conditional_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Conditional_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Conditional_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Conditional_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Conditional_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Conditional_RetrievalQuery

    #region METAView_ConditionalDescription_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_ConditionalDescription_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ConditionalDescription_ListingFilters : METAView_ConditionalDescription_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ConditionalDescription_ListingFilters class.
        /// </summary>
        public METAView_ConditionalDescription_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConditionalDescription_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ConditionalDescription_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConditionalDescription_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ConditionalDescription_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ConditionalDescription_ListingFilters

    #region METAView_ConditionalDescription_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_ConditionalDescription_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_ConditionalDescription_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ConditionalDescription_ListingQuery : METAView_ConditionalDescription_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ConditionalDescription_ListingQuery class.
        /// </summary>
        public METAView_ConditionalDescription_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConditionalDescription_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ConditionalDescription_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConditionalDescription_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ConditionalDescription_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ConditionalDescription_ListingQuery

    #region METAView_ConditionalDescription_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_ConditionalDescription_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ConditionalDescription_RetrievalFilters : METAView_ConditionalDescription_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ConditionalDescription_RetrievalFilters class.
        /// </summary>
        public METAView_ConditionalDescription_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConditionalDescription_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ConditionalDescription_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConditionalDescription_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ConditionalDescription_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ConditionalDescription_RetrievalFilters

    #region METAView_ConditionalDescription_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_ConditionalDescription_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_ConditionalDescription_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ConditionalDescription_RetrievalQuery : METAView_ConditionalDescription_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ConditionalDescription_RetrievalQuery class.
        /// </summary>
        public METAView_ConditionalDescription_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConditionalDescription_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ConditionalDescription_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConditionalDescription_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ConditionalDescription_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ConditionalDescription_RetrievalQuery

    #region METAView_ConnectionSize_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_ConnectionSize_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ConnectionSize_ListingFilters : METAView_ConnectionSize_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionSize_ListingFilters class.
        /// </summary>
        public METAView_ConnectionSize_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionSize_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ConnectionSize_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionSize_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ConnectionSize_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ConnectionSize_ListingFilters

    #region METAView_ConnectionSize_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_ConnectionSize_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_ConnectionSize_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ConnectionSize_ListingQuery : METAView_ConnectionSize_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionSize_ListingQuery class.
        /// </summary>
        public METAView_ConnectionSize_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionSize_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ConnectionSize_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionSize_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ConnectionSize_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ConnectionSize_ListingQuery

    #region METAView_ConnectionSize_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_ConnectionSize_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ConnectionSize_RetrievalFilters : METAView_ConnectionSize_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionSize_RetrievalFilters class.
        /// </summary>
        public METAView_ConnectionSize_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionSize_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ConnectionSize_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionSize_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ConnectionSize_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ConnectionSize_RetrievalFilters

    #region METAView_ConnectionSize_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_ConnectionSize_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_ConnectionSize_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ConnectionSize_RetrievalQuery : METAView_ConnectionSize_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionSize_RetrievalQuery class.
        /// </summary>
        public METAView_ConnectionSize_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionSize_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ConnectionSize_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionSize_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ConnectionSize_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ConnectionSize_RetrievalQuery

    #region METAView_ConnectionSpeed_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_ConnectionSpeed_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ConnectionSpeed_ListingFilters : METAView_ConnectionSpeed_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionSpeed_ListingFilters class.
        /// </summary>
        public METAView_ConnectionSpeed_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionSpeed_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ConnectionSpeed_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionSpeed_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ConnectionSpeed_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ConnectionSpeed_ListingFilters

    #region METAView_ConnectionSpeed_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_ConnectionSpeed_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_ConnectionSpeed_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ConnectionSpeed_ListingQuery : METAView_ConnectionSpeed_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionSpeed_ListingQuery class.
        /// </summary>
        public METAView_ConnectionSpeed_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionSpeed_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ConnectionSpeed_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionSpeed_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ConnectionSpeed_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ConnectionSpeed_ListingQuery

    #region METAView_ConnectionSpeed_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_ConnectionSpeed_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ConnectionSpeed_RetrievalFilters : METAView_ConnectionSpeed_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionSpeed_RetrievalFilters class.
        /// </summary>
        public METAView_ConnectionSpeed_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionSpeed_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ConnectionSpeed_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionSpeed_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ConnectionSpeed_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ConnectionSpeed_RetrievalFilters

    #region METAView_ConnectionSpeed_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_ConnectionSpeed_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_ConnectionSpeed_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ConnectionSpeed_RetrievalQuery : METAView_ConnectionSpeed_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionSpeed_RetrievalQuery class.
        /// </summary>
        public METAView_ConnectionSpeed_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionSpeed_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ConnectionSpeed_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionSpeed_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ConnectionSpeed_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ConnectionSpeed_RetrievalQuery

    #region METAView_ConnectionType_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_ConnectionType_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ConnectionType_ListingFilters : METAView_ConnectionType_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionType_ListingFilters class.
        /// </summary>
        public METAView_ConnectionType_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionType_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ConnectionType_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionType_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ConnectionType_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ConnectionType_ListingFilters

    #region METAView_ConnectionType_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_ConnectionType_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_ConnectionType_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ConnectionType_ListingQuery : METAView_ConnectionType_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionType_ListingQuery class.
        /// </summary>
        public METAView_ConnectionType_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionType_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ConnectionType_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionType_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ConnectionType_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ConnectionType_ListingQuery

    #region METAView_ConnectionType_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_ConnectionType_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ConnectionType_RetrievalFilters : METAView_ConnectionType_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionType_RetrievalFilters class.
        /// </summary>
        public METAView_ConnectionType_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionType_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ConnectionType_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionType_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ConnectionType_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ConnectionType_RetrievalFilters

    #region METAView_ConnectionType_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_ConnectionType_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_ConnectionType_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ConnectionType_RetrievalQuery : METAView_ConnectionType_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionType_RetrievalQuery class.
        /// </summary>
        public METAView_ConnectionType_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionType_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ConnectionType_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ConnectionType_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ConnectionType_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ConnectionType_RetrievalQuery

    #region METAView_Constraint_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Constraint_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Constraint_ListingFilters : METAView_Constraint_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Constraint_ListingFilters class.
        /// </summary>
        public METAView_Constraint_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Constraint_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Constraint_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Constraint_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Constraint_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Constraint_ListingFilters

    #region METAView_Constraint_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Constraint_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Constraint_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Constraint_ListingQuery : METAView_Constraint_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Constraint_ListingQuery class.
        /// </summary>
        public METAView_Constraint_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Constraint_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Constraint_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Constraint_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Constraint_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Constraint_ListingQuery

    #region METAView_Constraint_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Constraint_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Constraint_RetrievalFilters : METAView_Constraint_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Constraint_RetrievalFilters class.
        /// </summary>
        public METAView_Constraint_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Constraint_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Constraint_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Constraint_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Constraint_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Constraint_RetrievalFilters

    #region METAView_Constraint_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Constraint_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Constraint_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Constraint_RetrievalQuery : METAView_Constraint_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Constraint_RetrievalQuery class.
        /// </summary>
        public METAView_Constraint_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Constraint_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Constraint_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Constraint_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Constraint_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Constraint_RetrievalQuery

    #region METAView_CSF_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_CSF_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_CSF_ListingFilters : METAView_CSF_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_CSF_ListingFilters class.
        /// </summary>
        public METAView_CSF_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_CSF_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_CSF_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_CSF_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_CSF_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_CSF_ListingFilters

    #region METAView_CSF_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_CSF_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_CSF_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_CSF_ListingQuery : METAView_CSF_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_CSF_ListingQuery class.
        /// </summary>
        public METAView_CSF_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_CSF_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_CSF_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_CSF_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_CSF_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_CSF_ListingQuery

    #region METAView_CSF_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_CSF_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_CSF_RetrievalFilters : METAView_CSF_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_CSF_RetrievalFilters class.
        /// </summary>
        public METAView_CSF_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_CSF_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_CSF_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_CSF_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_CSF_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_CSF_RetrievalFilters

    #region METAView_CSF_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_CSF_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_CSF_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_CSF_RetrievalQuery : METAView_CSF_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_CSF_RetrievalQuery class.
        /// </summary>
        public METAView_CSF_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_CSF_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_CSF_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_CSF_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_CSF_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_CSF_RetrievalQuery

    #region METAView_DataAttribute_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_DataAttribute_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataAttribute_ListingFilters : METAView_DataAttribute_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataAttribute_ListingFilters class.
        /// </summary>
        public METAView_DataAttribute_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataAttribute_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataAttribute_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataAttribute_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataAttribute_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataAttribute_ListingFilters

    #region METAView_DataAttribute_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_DataAttribute_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_DataAttribute_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataAttribute_ListingQuery : METAView_DataAttribute_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataAttribute_ListingQuery class.
        /// </summary>
        public METAView_DataAttribute_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataAttribute_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataAttribute_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataAttribute_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataAttribute_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataAttribute_ListingQuery

    #region METAView_DataAttribute_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_DataAttribute_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataAttribute_RetrievalFilters : METAView_DataAttribute_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataAttribute_RetrievalFilters class.
        /// </summary>
        public METAView_DataAttribute_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataAttribute_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataAttribute_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataAttribute_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataAttribute_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataAttribute_RetrievalFilters

    #region METAView_DataAttribute_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_DataAttribute_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_DataAttribute_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataAttribute_RetrievalQuery : METAView_DataAttribute_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataAttribute_RetrievalQuery class.
        /// </summary>
        public METAView_DataAttribute_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataAttribute_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataAttribute_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataAttribute_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataAttribute_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataAttribute_RetrievalQuery

    #region METAView_DataColumn_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_DataColumn_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataColumn_ListingFilters : METAView_DataColumn_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataColumn_ListingFilters class.
        /// </summary>
        public METAView_DataColumn_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataColumn_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataColumn_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataColumn_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataColumn_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataColumn_ListingFilters

    #region METAView_DataColumn_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_DataColumn_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_DataColumn_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataColumn_ListingQuery : METAView_DataColumn_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataColumn_ListingQuery class.
        /// </summary>
        public METAView_DataColumn_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataColumn_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataColumn_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataColumn_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataColumn_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataColumn_ListingQuery

    #region METAView_DataColumn_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_DataColumn_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataColumn_RetrievalFilters : METAView_DataColumn_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataColumn_RetrievalFilters class.
        /// </summary>
        public METAView_DataColumn_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataColumn_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataColumn_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataColumn_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataColumn_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataColumn_RetrievalFilters

    #region METAView_DataColumn_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_DataColumn_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_DataColumn_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataColumn_RetrievalQuery : METAView_DataColumn_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataColumn_RetrievalQuery class.
        /// </summary>
        public METAView_DataColumn_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataColumn_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataColumn_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataColumn_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataColumn_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataColumn_RetrievalQuery

    #region METAView_DataDomain_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_DataDomain_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataDomain_ListingFilters : METAView_DataDomain_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataDomain_ListingFilters class.
        /// </summary>
        public METAView_DataDomain_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataDomain_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataDomain_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataDomain_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataDomain_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataDomain_ListingFilters

    #region METAView_DataDomain_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_DataDomain_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_DataDomain_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataDomain_ListingQuery : METAView_DataDomain_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataDomain_ListingQuery class.
        /// </summary>
        public METAView_DataDomain_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataDomain_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataDomain_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataDomain_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataDomain_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataDomain_ListingQuery

    #region METAView_DataDomain_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_DataDomain_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataDomain_RetrievalFilters : METAView_DataDomain_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataDomain_RetrievalFilters class.
        /// </summary>
        public METAView_DataDomain_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataDomain_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataDomain_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataDomain_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataDomain_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataDomain_RetrievalFilters

    #region METAView_DataDomain_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_DataDomain_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_DataDomain_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataDomain_RetrievalQuery : METAView_DataDomain_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataDomain_RetrievalQuery class.
        /// </summary>
        public METAView_DataDomain_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataDomain_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataDomain_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataDomain_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataDomain_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataDomain_RetrievalQuery

    #region METAView_DataEntity_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_DataEntity_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataEntity_ListingFilters : METAView_DataEntity_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataEntity_ListingFilters class.
        /// </summary>
        public METAView_DataEntity_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataEntity_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataEntity_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataEntity_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataEntity_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataEntity_ListingFilters

    #region METAView_DataEntity_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_DataEntity_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_DataEntity_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataEntity_ListingQuery : METAView_DataEntity_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataEntity_ListingQuery class.
        /// </summary>
        public METAView_DataEntity_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataEntity_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataEntity_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataEntity_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataEntity_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataEntity_ListingQuery

    #region METAView_DataEntity_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_DataEntity_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataEntity_RetrievalFilters : METAView_DataEntity_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataEntity_RetrievalFilters class.
        /// </summary>
        public METAView_DataEntity_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataEntity_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataEntity_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataEntity_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataEntity_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataEntity_RetrievalFilters

    #region METAView_DataEntity_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_DataEntity_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_DataEntity_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataEntity_RetrievalQuery : METAView_DataEntity_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataEntity_RetrievalQuery class.
        /// </summary>
        public METAView_DataEntity_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataEntity_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataEntity_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataEntity_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataEntity_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataEntity_RetrievalQuery

    #region METAView_DataField_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_DataField_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataField_ListingFilters : METAView_DataField_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataField_ListingFilters class.
        /// </summary>
        public METAView_DataField_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataField_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataField_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataField_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataField_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataField_ListingFilters

    #region METAView_DataField_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_DataField_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_DataField_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataField_ListingQuery : METAView_DataField_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataField_ListingQuery class.
        /// </summary>
        public METAView_DataField_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataField_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataField_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataField_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataField_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataField_ListingQuery

    #region METAView_DataField_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_DataField_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataField_RetrievalFilters : METAView_DataField_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataField_RetrievalFilters class.
        /// </summary>
        public METAView_DataField_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataField_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataField_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataField_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataField_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataField_RetrievalFilters

    #region METAView_DataField_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_DataField_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_DataField_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataField_RetrievalQuery : METAView_DataField_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataField_RetrievalQuery class.
        /// </summary>
        public METAView_DataField_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataField_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataField_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataField_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataField_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataField_RetrievalQuery

    #region METAView_DataFlowIndicator_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_DataFlowIndicator_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataFlowIndicator_ListingFilters : METAView_DataFlowIndicator_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataFlowIndicator_ListingFilters class.
        /// </summary>
        public METAView_DataFlowIndicator_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataFlowIndicator_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataFlowIndicator_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataFlowIndicator_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataFlowIndicator_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataFlowIndicator_ListingFilters

    #region METAView_DataFlowIndicator_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_DataFlowIndicator_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_DataFlowIndicator_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataFlowIndicator_ListingQuery : METAView_DataFlowIndicator_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataFlowIndicator_ListingQuery class.
        /// </summary>
        public METAView_DataFlowIndicator_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataFlowIndicator_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataFlowIndicator_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataFlowIndicator_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataFlowIndicator_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataFlowIndicator_ListingQuery

    #region METAView_DataFlowIndicator_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_DataFlowIndicator_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataFlowIndicator_RetrievalFilters : METAView_DataFlowIndicator_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataFlowIndicator_RetrievalFilters class.
        /// </summary>
        public METAView_DataFlowIndicator_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataFlowIndicator_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataFlowIndicator_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataFlowIndicator_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataFlowIndicator_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataFlowIndicator_RetrievalFilters

    #region METAView_DataFlowIndicator_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_DataFlowIndicator_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_DataFlowIndicator_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataFlowIndicator_RetrievalQuery : METAView_DataFlowIndicator_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataFlowIndicator_RetrievalQuery class.
        /// </summary>
        public METAView_DataFlowIndicator_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataFlowIndicator_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataFlowIndicator_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataFlowIndicator_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataFlowIndicator_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataFlowIndicator_RetrievalQuery

    #region METAView_DataSchema_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_DataSchema_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataSchema_ListingFilters : METAView_DataSchema_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataSchema_ListingFilters class.
        /// </summary>
        public METAView_DataSchema_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataSchema_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataSchema_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataSchema_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataSchema_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataSchema_ListingFilters

    #region METAView_DataSchema_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_DataSchema_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_DataSchema_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataSchema_ListingQuery : METAView_DataSchema_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataSchema_ListingQuery class.
        /// </summary>
        public METAView_DataSchema_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataSchema_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataSchema_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataSchema_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataSchema_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataSchema_ListingQuery

    #region METAView_DataSchema_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_DataSchema_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataSchema_RetrievalFilters : METAView_DataSchema_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataSchema_RetrievalFilters class.
        /// </summary>
        public METAView_DataSchema_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataSchema_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataSchema_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataSchema_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataSchema_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataSchema_RetrievalFilters

    #region METAView_DataSchema_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_DataSchema_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_DataSchema_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataSchema_RetrievalQuery : METAView_DataSchema_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataSchema_RetrievalQuery class.
        /// </summary>
        public METAView_DataSchema_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataSchema_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataSchema_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataSchema_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataSchema_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataSchema_RetrievalQuery

    #region METAView_DataSubjectArea_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_DataSubjectArea_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataSubjectArea_ListingFilters : METAView_DataSubjectArea_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataSubjectArea_ListingFilters class.
        /// </summary>
        public METAView_DataSubjectArea_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataSubjectArea_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataSubjectArea_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataSubjectArea_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataSubjectArea_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataSubjectArea_ListingFilters

    #region METAView_DataSubjectArea_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_DataSubjectArea_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_DataSubjectArea_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataSubjectArea_ListingQuery : METAView_DataSubjectArea_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataSubjectArea_ListingQuery class.
        /// </summary>
        public METAView_DataSubjectArea_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataSubjectArea_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataSubjectArea_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataSubjectArea_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataSubjectArea_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataSubjectArea_ListingQuery

    #region METAView_DataSubjectArea_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_DataSubjectArea_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataSubjectArea_RetrievalFilters : METAView_DataSubjectArea_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataSubjectArea_RetrievalFilters class.
        /// </summary>
        public METAView_DataSubjectArea_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataSubjectArea_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataSubjectArea_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataSubjectArea_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataSubjectArea_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataSubjectArea_RetrievalFilters

    #region METAView_DataSubjectArea_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_DataSubjectArea_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_DataSubjectArea_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataSubjectArea_RetrievalQuery : METAView_DataSubjectArea_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataSubjectArea_RetrievalQuery class.
        /// </summary>
        public METAView_DataSubjectArea_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataSubjectArea_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataSubjectArea_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataSubjectArea_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataSubjectArea_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataSubjectArea_RetrievalQuery

    #region METAView_DataTable_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_DataTable_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataTable_ListingFilters : METAView_DataTable_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataTable_ListingFilters class.
        /// </summary>
        public METAView_DataTable_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataTable_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataTable_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataTable_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataTable_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataTable_ListingFilters

    #region METAView_DataTable_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_DataTable_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_DataTable_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataTable_ListingQuery : METAView_DataTable_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataTable_ListingQuery class.
        /// </summary>
        public METAView_DataTable_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataTable_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataTable_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataTable_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataTable_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataTable_ListingQuery

    #region METAView_DataTable_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_DataTable_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataTable_RetrievalFilters : METAView_DataTable_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataTable_RetrievalFilters class.
        /// </summary>
        public METAView_DataTable_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataTable_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataTable_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataTable_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataTable_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataTable_RetrievalFilters

    #region METAView_DataTable_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_DataTable_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_DataTable_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataTable_RetrievalQuery : METAView_DataTable_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataTable_RetrievalQuery class.
        /// </summary>
        public METAView_DataTable_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataTable_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataTable_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataTable_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataTable_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataTable_RetrievalQuery

    #region METAView_DataValue_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_DataValue_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataValue_ListingFilters : METAView_DataValue_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataValue_ListingFilters class.
        /// </summary>
        public METAView_DataValue_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataValue_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataValue_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataValue_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataValue_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataValue_ListingFilters

    #region METAView_DataValue_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_DataValue_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_DataValue_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataValue_ListingQuery : METAView_DataValue_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataValue_ListingQuery class.
        /// </summary>
        public METAView_DataValue_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataValue_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataValue_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataValue_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataValue_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataValue_ListingQuery

    #region METAView_DataValue_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_DataValue_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataValue_RetrievalFilters : METAView_DataValue_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataValue_RetrievalFilters class.
        /// </summary>
        public METAView_DataValue_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataValue_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataValue_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataValue_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataValue_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataValue_RetrievalFilters

    #region METAView_DataValue_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_DataValue_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_DataValue_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataValue_RetrievalQuery : METAView_DataValue_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataValue_RetrievalQuery class.
        /// </summary>
        public METAView_DataValue_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataValue_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataValue_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataValue_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataValue_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataValue_RetrievalQuery

    #region METAView_DataView_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_DataView_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataView_ListingFilters : METAView_DataView_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataView_ListingFilters class.
        /// </summary>
        public METAView_DataView_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataView_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataView_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataView_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataView_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataView_ListingFilters

    #region METAView_DataView_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_DataView_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_DataView_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataView_ListingQuery : METAView_DataView_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataView_ListingQuery class.
        /// </summary>
        public METAView_DataView_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataView_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataView_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataView_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataView_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataView_ListingQuery

    #region METAView_DataView_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_DataView_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataView_RetrievalFilters : METAView_DataView_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataView_RetrievalFilters class.
        /// </summary>
        public METAView_DataView_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataView_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataView_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataView_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataView_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataView_RetrievalFilters

    #region METAView_DataView_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_DataView_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_DataView_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataView_RetrievalQuery : METAView_DataView_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DataView_RetrievalQuery class.
        /// </summary>
        public METAView_DataView_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataView_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DataView_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DataView_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DataView_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DataView_RetrievalQuery

    #region METAView_DatedResponsibility_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_DatedResponsibility_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DatedResponsibility_ListingFilters : METAView_DatedResponsibility_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DatedResponsibility_ListingFilters class.
        /// </summary>
        public METAView_DatedResponsibility_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DatedResponsibility_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DatedResponsibility_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DatedResponsibility_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DatedResponsibility_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DatedResponsibility_ListingFilters

    #region METAView_DatedResponsibility_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_DatedResponsibility_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_DatedResponsibility_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DatedResponsibility_ListingQuery : METAView_DatedResponsibility_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DatedResponsibility_ListingQuery class.
        /// </summary>
        public METAView_DatedResponsibility_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DatedResponsibility_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DatedResponsibility_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DatedResponsibility_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DatedResponsibility_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DatedResponsibility_ListingQuery

    #region METAView_DatedResponsibility_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_DatedResponsibility_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DatedResponsibility_RetrievalFilters : METAView_DatedResponsibility_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DatedResponsibility_RetrievalFilters class.
        /// </summary>
        public METAView_DatedResponsibility_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DatedResponsibility_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DatedResponsibility_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DatedResponsibility_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DatedResponsibility_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DatedResponsibility_RetrievalFilters

    #region METAView_DatedResponsibility_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_DatedResponsibility_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_DatedResponsibility_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DatedResponsibility_RetrievalQuery : METAView_DatedResponsibility_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DatedResponsibility_RetrievalQuery class.
        /// </summary>
        public METAView_DatedResponsibility_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DatedResponsibility_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DatedResponsibility_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DatedResponsibility_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DatedResponsibility_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DatedResponsibility_RetrievalQuery

    #region METAView_DependencyDescription_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_DependencyDescription_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DependencyDescription_ListingFilters : METAView_DependencyDescription_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DependencyDescription_ListingFilters class.
        /// </summary>
        public METAView_DependencyDescription_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DependencyDescription_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DependencyDescription_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DependencyDescription_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DependencyDescription_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DependencyDescription_ListingFilters

    #region METAView_DependencyDescription_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_DependencyDescription_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_DependencyDescription_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DependencyDescription_ListingQuery : METAView_DependencyDescription_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DependencyDescription_ListingQuery class.
        /// </summary>
        public METAView_DependencyDescription_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DependencyDescription_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DependencyDescription_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DependencyDescription_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DependencyDescription_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DependencyDescription_ListingQuery

    #region METAView_DependencyDescription_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_DependencyDescription_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DependencyDescription_RetrievalFilters : METAView_DependencyDescription_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DependencyDescription_RetrievalFilters class.
        /// </summary>
        public METAView_DependencyDescription_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DependencyDescription_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DependencyDescription_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DependencyDescription_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DependencyDescription_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DependencyDescription_RetrievalFilters

    #region METAView_DependencyDescription_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_DependencyDescription_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_DependencyDescription_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DependencyDescription_RetrievalQuery : METAView_DependencyDescription_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_DependencyDescription_RetrievalQuery class.
        /// </summary>
        public METAView_DependencyDescription_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_DependencyDescription_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_DependencyDescription_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_DependencyDescription_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_DependencyDescription_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_DependencyDescription_RetrievalQuery

    #region METAView_Driver_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Driver_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Driver_ListingFilters : METAView_Driver_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Driver_ListingFilters class.
        /// </summary>
        public METAView_Driver_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Driver_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Driver_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Driver_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Driver_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Driver_ListingFilters

    #region METAView_Driver_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Driver_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Driver_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Driver_ListingQuery : METAView_Driver_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Driver_ListingQuery class.
        /// </summary>
        public METAView_Driver_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Driver_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Driver_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Driver_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Driver_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Driver_ListingQuery

    #region METAView_Driver_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Driver_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Driver_RetrievalFilters : METAView_Driver_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Driver_RetrievalFilters class.
        /// </summary>
        public METAView_Driver_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Driver_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Driver_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Driver_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Driver_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Driver_RetrievalFilters

    #region METAView_Driver_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Driver_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Driver_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Driver_RetrievalQuery : METAView_Driver_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Driver_RetrievalQuery class.
        /// </summary>
        public METAView_Driver_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Driver_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Driver_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Driver_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Driver_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Driver_RetrievalQuery

    #region METAView_Employee_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Employee_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Employee_ListingFilters : METAView_Employee_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Employee_ListingFilters class.
        /// </summary>
        public METAView_Employee_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Employee_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Employee_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Employee_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Employee_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Employee_ListingFilters

    #region METAView_Employee_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Employee_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Employee_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Employee_ListingQuery : METAView_Employee_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Employee_ListingQuery class.
        /// </summary>
        public METAView_Employee_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Employee_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Employee_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Employee_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Employee_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Employee_ListingQuery

    #region METAView_Employee_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Employee_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Employee_RetrievalFilters : METAView_Employee_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Employee_RetrievalFilters class.
        /// </summary>
        public METAView_Employee_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Employee_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Employee_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Employee_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Employee_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Employee_RetrievalFilters

    #region METAView_Employee_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Employee_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Employee_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Employee_RetrievalQuery : METAView_Employee_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Employee_RetrievalQuery class.
        /// </summary>
        public METAView_Employee_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Employee_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Employee_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Employee_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Employee_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Employee_RetrievalQuery

    #region METAView_Entity_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Entity_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Entity_ListingFilters : METAView_Entity_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Entity_ListingFilters class.
        /// </summary>
        public METAView_Entity_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Entity_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Entity_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Entity_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Entity_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Entity_ListingFilters

    #region METAView_Entity_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Entity_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Entity_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Entity_ListingQuery : METAView_Entity_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Entity_ListingQuery class.
        /// </summary>
        public METAView_Entity_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Entity_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Entity_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Entity_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Entity_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Entity_ListingQuery

    #region METAView_Entity_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Entity_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Entity_RetrievalFilters : METAView_Entity_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Entity_RetrievalFilters class.
        /// </summary>
        public METAView_Entity_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Entity_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Entity_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Entity_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Entity_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Entity_RetrievalFilters

    #region METAView_Entity_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Entity_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Entity_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Entity_RetrievalQuery : METAView_Entity_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Entity_RetrievalQuery class.
        /// </summary>
        public METAView_Entity_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Entity_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Entity_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Entity_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Entity_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Entity_RetrievalQuery

    #region METAView_Environment_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Environment_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Environment_ListingFilters : METAView_Environment_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Environment_ListingFilters class.
        /// </summary>
        public METAView_Environment_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Environment_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Environment_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Environment_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Environment_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Environment_ListingFilters

    #region METAView_Environment_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Environment_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Environment_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Environment_ListingQuery : METAView_Environment_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Environment_ListingQuery class.
        /// </summary>
        public METAView_Environment_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Environment_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Environment_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Environment_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Environment_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Environment_ListingQuery

    #region METAView_Environment_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Environment_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Environment_RetrievalFilters : METAView_Environment_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Environment_RetrievalFilters class.
        /// </summary>
        public METAView_Environment_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Environment_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Environment_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Environment_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Environment_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Environment_RetrievalFilters

    #region METAView_Environment_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Environment_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Environment_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Environment_RetrievalQuery : METAView_Environment_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Environment_RetrievalQuery class.
        /// </summary>
        public METAView_Environment_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Environment_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Environment_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Environment_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Environment_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Environment_RetrievalQuery

    #region METAView_EnvironmentCategory_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_EnvironmentCategory_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_EnvironmentCategory_ListingFilters : METAView_EnvironmentCategory_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_EnvironmentCategory_ListingFilters class.
        /// </summary>
        public METAView_EnvironmentCategory_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_EnvironmentCategory_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_EnvironmentCategory_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_EnvironmentCategory_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_EnvironmentCategory_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_EnvironmentCategory_ListingFilters

    #region METAView_EnvironmentCategory_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_EnvironmentCategory_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_EnvironmentCategory_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_EnvironmentCategory_ListingQuery : METAView_EnvironmentCategory_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_EnvironmentCategory_ListingQuery class.
        /// </summary>
        public METAView_EnvironmentCategory_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_EnvironmentCategory_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_EnvironmentCategory_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_EnvironmentCategory_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_EnvironmentCategory_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_EnvironmentCategory_ListingQuery

    #region METAView_EnvironmentCategory_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_EnvironmentCategory_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_EnvironmentCategory_RetrievalFilters : METAView_EnvironmentCategory_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_EnvironmentCategory_RetrievalFilters class.
        /// </summary>
        public METAView_EnvironmentCategory_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_EnvironmentCategory_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_EnvironmentCategory_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_EnvironmentCategory_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_EnvironmentCategory_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_EnvironmentCategory_RetrievalFilters

    #region METAView_EnvironmentCategory_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_EnvironmentCategory_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_EnvironmentCategory_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_EnvironmentCategory_RetrievalQuery : METAView_EnvironmentCategory_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_EnvironmentCategory_RetrievalQuery class.
        /// </summary>
        public METAView_EnvironmentCategory_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_EnvironmentCategory_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_EnvironmentCategory_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_EnvironmentCategory_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_EnvironmentCategory_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_EnvironmentCategory_RetrievalQuery

    #region METAView_Event_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Event_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Event_ListingFilters : METAView_Event_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Event_ListingFilters class.
        /// </summary>
        public METAView_Event_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Event_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Event_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Event_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Event_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Event_ListingFilters

    #region METAView_Event_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Event_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Event_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Event_ListingQuery : METAView_Event_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Event_ListingQuery class.
        /// </summary>
        public METAView_Event_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Event_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Event_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Event_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Event_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Event_ListingQuery

    #region METAView_Event_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Event_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Event_RetrievalFilters : METAView_Event_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Event_RetrievalFilters class.
        /// </summary>
        public METAView_Event_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Event_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Event_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Event_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Event_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Event_RetrievalFilters

    #region METAView_Event_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Event_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Event_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Event_RetrievalQuery : METAView_Event_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Event_RetrievalQuery class.
        /// </summary>
        public METAView_Event_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Event_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Event_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Event_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Event_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Event_RetrievalQuery

    #region METAView_Exception_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Exception_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Exception_ListingFilters : METAView_Exception_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Exception_ListingFilters class.
        /// </summary>
        public METAView_Exception_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Exception_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Exception_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Exception_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Exception_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Exception_ListingFilters

    #region METAView_Exception_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Exception_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Exception_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Exception_ListingQuery : METAView_Exception_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Exception_ListingQuery class.
        /// </summary>
        public METAView_Exception_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Exception_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Exception_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Exception_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Exception_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Exception_ListingQuery

    #region METAView_Exception_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Exception_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Exception_RetrievalFilters : METAView_Exception_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Exception_RetrievalFilters class.
        /// </summary>
        public METAView_Exception_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Exception_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Exception_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Exception_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Exception_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Exception_RetrievalFilters

    #region METAView_Exception_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Exception_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Exception_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Exception_RetrievalQuery : METAView_Exception_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Exception_RetrievalQuery class.
        /// </summary>
        public METAView_Exception_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Exception_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Exception_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Exception_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Exception_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Exception_RetrievalQuery

    #region METAView_ExtractionRule_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_ExtractionRule_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ExtractionRule_ListingFilters : METAView_ExtractionRule_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ExtractionRule_ListingFilters class.
        /// </summary>
        public METAView_ExtractionRule_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ExtractionRule_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ExtractionRule_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ExtractionRule_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ExtractionRule_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ExtractionRule_ListingFilters

    #region METAView_ExtractionRule_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_ExtractionRule_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_ExtractionRule_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ExtractionRule_ListingQuery : METAView_ExtractionRule_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ExtractionRule_ListingQuery class.
        /// </summary>
        public METAView_ExtractionRule_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ExtractionRule_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ExtractionRule_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ExtractionRule_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ExtractionRule_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ExtractionRule_ListingQuery

    #region METAView_ExtractionRule_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_ExtractionRule_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ExtractionRule_RetrievalFilters : METAView_ExtractionRule_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ExtractionRule_RetrievalFilters class.
        /// </summary>
        public METAView_ExtractionRule_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ExtractionRule_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ExtractionRule_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ExtractionRule_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ExtractionRule_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ExtractionRule_RetrievalFilters

    #region METAView_ExtractionRule_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_ExtractionRule_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_ExtractionRule_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ExtractionRule_RetrievalQuery : METAView_ExtractionRule_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ExtractionRule_RetrievalQuery class.
        /// </summary>
        public METAView_ExtractionRule_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ExtractionRule_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ExtractionRule_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ExtractionRule_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ExtractionRule_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ExtractionRule_RetrievalQuery

    #region METAView_FlowDescription_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_FlowDescription_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_FlowDescription_ListingFilters : METAView_FlowDescription_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_FlowDescription_ListingFilters class.
        /// </summary>
        public METAView_FlowDescription_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_FlowDescription_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_FlowDescription_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_FlowDescription_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_FlowDescription_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_FlowDescription_ListingFilters

    #region METAView_FlowDescription_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_FlowDescription_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_FlowDescription_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_FlowDescription_ListingQuery : METAView_FlowDescription_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_FlowDescription_ListingQuery class.
        /// </summary>
        public METAView_FlowDescription_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_FlowDescription_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_FlowDescription_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_FlowDescription_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_FlowDescription_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_FlowDescription_ListingQuery

    #region METAView_FlowDescription_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_FlowDescription_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_FlowDescription_RetrievalFilters : METAView_FlowDescription_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_FlowDescription_RetrievalFilters class.
        /// </summary>
        public METAView_FlowDescription_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_FlowDescription_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_FlowDescription_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_FlowDescription_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_FlowDescription_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_FlowDescription_RetrievalFilters

    #region METAView_FlowDescription_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_FlowDescription_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_FlowDescription_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_FlowDescription_RetrievalQuery : METAView_FlowDescription_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_FlowDescription_RetrievalQuery class.
        /// </summary>
        public METAView_FlowDescription_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_FlowDescription_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_FlowDescription_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_FlowDescription_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_FlowDescription_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_FlowDescription_RetrievalQuery

    #region METAView_Function_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Function_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Function_ListingFilters : METAView_Function_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Function_ListingFilters class.
        /// </summary>
        public METAView_Function_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Function_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Function_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Function_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Function_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Function_ListingFilters

    #region METAView_Function_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Function_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Function_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Function_ListingQuery : METAView_Function_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Function_ListingQuery class.
        /// </summary>
        public METAView_Function_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Function_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Function_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Function_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Function_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Function_ListingQuery

    #region METAView_Function_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Function_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Function_RetrievalFilters : METAView_Function_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Function_RetrievalFilters class.
        /// </summary>
        public METAView_Function_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Function_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Function_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Function_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Function_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Function_RetrievalFilters

    #region METAView_Function_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Function_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Function_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Function_RetrievalQuery : METAView_Function_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Function_RetrievalQuery class.
        /// </summary>
        public METAView_Function_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Function_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Function_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Function_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Function_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Function_RetrievalQuery

    #region METAView_FunctionalDependency_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_FunctionalDependency_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_FunctionalDependency_ListingFilters : METAView_FunctionalDependency_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_FunctionalDependency_ListingFilters class.
        /// </summary>
        public METAView_FunctionalDependency_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_FunctionalDependency_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_FunctionalDependency_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_FunctionalDependency_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_FunctionalDependency_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_FunctionalDependency_ListingFilters

    #region METAView_FunctionalDependency_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_FunctionalDependency_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_FunctionalDependency_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_FunctionalDependency_ListingQuery : METAView_FunctionalDependency_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_FunctionalDependency_ListingQuery class.
        /// </summary>
        public METAView_FunctionalDependency_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_FunctionalDependency_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_FunctionalDependency_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_FunctionalDependency_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_FunctionalDependency_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_FunctionalDependency_ListingQuery

    #region METAView_FunctionalDependency_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_FunctionalDependency_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_FunctionalDependency_RetrievalFilters : METAView_FunctionalDependency_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_FunctionalDependency_RetrievalFilters class.
        /// </summary>
        public METAView_FunctionalDependency_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_FunctionalDependency_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_FunctionalDependency_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_FunctionalDependency_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_FunctionalDependency_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_FunctionalDependency_RetrievalFilters

    #region METAView_FunctionalDependency_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_FunctionalDependency_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_FunctionalDependency_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_FunctionalDependency_RetrievalQuery : METAView_FunctionalDependency_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_FunctionalDependency_RetrievalQuery class.
        /// </summary>
        public METAView_FunctionalDependency_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_FunctionalDependency_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_FunctionalDependency_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_FunctionalDependency_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_FunctionalDependency_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_FunctionalDependency_RetrievalQuery

    #region METAView_Gateway_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Gateway_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Gateway_ListingFilters : METAView_Gateway_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Gateway_ListingFilters class.
        /// </summary>
        public METAView_Gateway_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Gateway_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Gateway_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Gateway_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Gateway_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Gateway_ListingFilters

    #region METAView_Gateway_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Gateway_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Gateway_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Gateway_ListingQuery : METAView_Gateway_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Gateway_ListingQuery class.
        /// </summary>
        public METAView_Gateway_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Gateway_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Gateway_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Gateway_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Gateway_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Gateway_ListingQuery

    #region METAView_Gateway_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Gateway_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Gateway_RetrievalFilters : METAView_Gateway_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Gateway_RetrievalFilters class.
        /// </summary>
        public METAView_Gateway_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Gateway_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Gateway_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Gateway_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Gateway_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Gateway_RetrievalFilters

    #region METAView_Gateway_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Gateway_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Gateway_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Gateway_RetrievalQuery : METAView_Gateway_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Gateway_RetrievalQuery class.
        /// </summary>
        public METAView_Gateway_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Gateway_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Gateway_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Gateway_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Gateway_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Gateway_RetrievalQuery

    #region METAView_GatewayDescription_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_GatewayDescription_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_GatewayDescription_ListingFilters : METAView_GatewayDescription_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_GatewayDescription_ListingFilters class.
        /// </summary>
        public METAView_GatewayDescription_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_GatewayDescription_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_GatewayDescription_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_GatewayDescription_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_GatewayDescription_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_GatewayDescription_ListingFilters

    #region METAView_GatewayDescription_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_GatewayDescription_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_GatewayDescription_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_GatewayDescription_ListingQuery : METAView_GatewayDescription_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_GatewayDescription_ListingQuery class.
        /// </summary>
        public METAView_GatewayDescription_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_GatewayDescription_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_GatewayDescription_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_GatewayDescription_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_GatewayDescription_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_GatewayDescription_ListingQuery

    #region METAView_GatewayDescription_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_GatewayDescription_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_GatewayDescription_RetrievalFilters : METAView_GatewayDescription_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_GatewayDescription_RetrievalFilters class.
        /// </summary>
        public METAView_GatewayDescription_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_GatewayDescription_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_GatewayDescription_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_GatewayDescription_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_GatewayDescription_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_GatewayDescription_RetrievalFilters

    #region METAView_GatewayDescription_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_GatewayDescription_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_GatewayDescription_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_GatewayDescription_RetrievalQuery : METAView_GatewayDescription_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_GatewayDescription_RetrievalQuery class.
        /// </summary>
        public METAView_GatewayDescription_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_GatewayDescription_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_GatewayDescription_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_GatewayDescription_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_GatewayDescription_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_GatewayDescription_RetrievalQuery

    #region METAView_GovernanceMechanism_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_GovernanceMechanism_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_GovernanceMechanism_ListingFilters : METAView_GovernanceMechanism_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_GovernanceMechanism_ListingFilters class.
        /// </summary>
        public METAView_GovernanceMechanism_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_GovernanceMechanism_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_GovernanceMechanism_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_GovernanceMechanism_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_GovernanceMechanism_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_GovernanceMechanism_ListingFilters

    #region METAView_GovernanceMechanism_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_GovernanceMechanism_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_GovernanceMechanism_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_GovernanceMechanism_ListingQuery : METAView_GovernanceMechanism_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_GovernanceMechanism_ListingQuery class.
        /// </summary>
        public METAView_GovernanceMechanism_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_GovernanceMechanism_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_GovernanceMechanism_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_GovernanceMechanism_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_GovernanceMechanism_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_GovernanceMechanism_ListingQuery

    #region METAView_GovernanceMechanism_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_GovernanceMechanism_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_GovernanceMechanism_RetrievalFilters : METAView_GovernanceMechanism_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_GovernanceMechanism_RetrievalFilters class.
        /// </summary>
        public METAView_GovernanceMechanism_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_GovernanceMechanism_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_GovernanceMechanism_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_GovernanceMechanism_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_GovernanceMechanism_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_GovernanceMechanism_RetrievalFilters

    #region METAView_GovernanceMechanism_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_GovernanceMechanism_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_GovernanceMechanism_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_GovernanceMechanism_RetrievalQuery : METAView_GovernanceMechanism_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_GovernanceMechanism_RetrievalQuery class.
        /// </summary>
        public METAView_GovernanceMechanism_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_GovernanceMechanism_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_GovernanceMechanism_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_GovernanceMechanism_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_GovernanceMechanism_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_GovernanceMechanism_RetrievalQuery

    #region METAView_Implication_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Implication_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Implication_ListingFilters : METAView_Implication_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Implication_ListingFilters class.
        /// </summary>
        public METAView_Implication_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Implication_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Implication_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Implication_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Implication_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Implication_ListingFilters

    #region METAView_Implication_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Implication_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Implication_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Implication_ListingQuery : METAView_Implication_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Implication_ListingQuery class.
        /// </summary>
        public METAView_Implication_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Implication_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Implication_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Implication_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Implication_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Implication_ListingQuery

    #region METAView_Implication_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Implication_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Implication_RetrievalFilters : METAView_Implication_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Implication_RetrievalFilters class.
        /// </summary>
        public METAView_Implication_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Implication_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Implication_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Implication_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Implication_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Implication_RetrievalFilters

    #region METAView_Implication_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Implication_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Implication_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Implication_RetrievalQuery : METAView_Implication_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Implication_RetrievalQuery class.
        /// </summary>
        public METAView_Implication_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Implication_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Implication_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Implication_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Implication_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Implication_RetrievalQuery

    #region METAView_Iteration_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Iteration_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Iteration_ListingFilters : METAView_Iteration_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Iteration_ListingFilters class.
        /// </summary>
        public METAView_Iteration_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Iteration_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Iteration_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Iteration_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Iteration_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Iteration_ListingFilters

    #region METAView_Iteration_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Iteration_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Iteration_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Iteration_ListingQuery : METAView_Iteration_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Iteration_ListingQuery class.
        /// </summary>
        public METAView_Iteration_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Iteration_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Iteration_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Iteration_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Iteration_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Iteration_ListingQuery

    #region METAView_Iteration_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Iteration_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Iteration_RetrievalFilters : METAView_Iteration_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Iteration_RetrievalFilters class.
        /// </summary>
        public METAView_Iteration_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Iteration_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Iteration_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Iteration_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Iteration_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Iteration_RetrievalFilters

    #region METAView_Iteration_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Iteration_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Iteration_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Iteration_RetrievalQuery : METAView_Iteration_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Iteration_RetrievalQuery class.
        /// </summary>
        public METAView_Iteration_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Iteration_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Iteration_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Iteration_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Iteration_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Iteration_RetrievalQuery

    #region METAView_ITInfrastructureEnvironment_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_ITInfrastructureEnvironment_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ITInfrastructureEnvironment_ListingFilters : METAView_ITInfrastructureEnvironment_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ITInfrastructureEnvironment_ListingFilters class.
        /// </summary>
        public METAView_ITInfrastructureEnvironment_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ITInfrastructureEnvironment_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ITInfrastructureEnvironment_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ITInfrastructureEnvironment_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ITInfrastructureEnvironment_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ITInfrastructureEnvironment_ListingFilters

    #region METAView_ITInfrastructureEnvironment_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_ITInfrastructureEnvironment_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_ITInfrastructureEnvironment_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ITInfrastructureEnvironment_ListingQuery : METAView_ITInfrastructureEnvironment_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ITInfrastructureEnvironment_ListingQuery class.
        /// </summary>
        public METAView_ITInfrastructureEnvironment_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ITInfrastructureEnvironment_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ITInfrastructureEnvironment_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ITInfrastructureEnvironment_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ITInfrastructureEnvironment_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ITInfrastructureEnvironment_ListingQuery

    #region METAView_ITInfrastructureEnvironment_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_ITInfrastructureEnvironment_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ITInfrastructureEnvironment_RetrievalFilters : METAView_ITInfrastructureEnvironment_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ITInfrastructureEnvironment_RetrievalFilters class.
        /// </summary>
        public METAView_ITInfrastructureEnvironment_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ITInfrastructureEnvironment_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ITInfrastructureEnvironment_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ITInfrastructureEnvironment_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ITInfrastructureEnvironment_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ITInfrastructureEnvironment_RetrievalFilters

    #region METAView_ITInfrastructureEnvironment_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_ITInfrastructureEnvironment_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_ITInfrastructureEnvironment_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ITInfrastructureEnvironment_RetrievalQuery : METAView_ITInfrastructureEnvironment_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ITInfrastructureEnvironment_RetrievalQuery class.
        /// </summary>
        public METAView_ITInfrastructureEnvironment_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ITInfrastructureEnvironment_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ITInfrastructureEnvironment_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ITInfrastructureEnvironment_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ITInfrastructureEnvironment_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ITInfrastructureEnvironment_RetrievalQuery

    #region METAView_Job_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Job_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Job_ListingFilters : METAView_Job_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Job_ListingFilters class.
        /// </summary>
        public METAView_Job_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Job_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Job_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Job_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Job_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Job_ListingFilters

    #region METAView_Job_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Job_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Job_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Job_ListingQuery : METAView_Job_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Job_ListingQuery class.
        /// </summary>
        public METAView_Job_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Job_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Job_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Job_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Job_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Job_ListingQuery

    #region METAView_Job_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Job_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Job_RetrievalFilters : METAView_Job_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Job_RetrievalFilters class.
        /// </summary>
        public METAView_Job_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Job_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Job_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Job_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Job_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Job_RetrievalFilters

    #region METAView_Job_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Job_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Job_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Job_RetrievalQuery : METAView_Job_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Job_RetrievalQuery class.
        /// </summary>
        public METAView_Job_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Job_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Job_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Job_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Job_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Job_RetrievalQuery

    #region METAView_JobCompetency_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_JobCompetency_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_JobCompetency_ListingFilters : METAView_JobCompetency_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_JobCompetency_ListingFilters class.
        /// </summary>
        public METAView_JobCompetency_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_JobCompetency_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_JobCompetency_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_JobCompetency_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_JobCompetency_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_JobCompetency_ListingFilters

    #region METAView_JobCompetency_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_JobCompetency_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_JobCompetency_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_JobCompetency_ListingQuery : METAView_JobCompetency_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_JobCompetency_ListingQuery class.
        /// </summary>
        public METAView_JobCompetency_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_JobCompetency_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_JobCompetency_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_JobCompetency_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_JobCompetency_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_JobCompetency_ListingQuery

    #region METAView_JobCompetency_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_JobCompetency_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_JobCompetency_RetrievalFilters : METAView_JobCompetency_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_JobCompetency_RetrievalFilters class.
        /// </summary>
        public METAView_JobCompetency_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_JobCompetency_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_JobCompetency_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_JobCompetency_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_JobCompetency_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_JobCompetency_RetrievalFilters

    #region METAView_JobCompetency_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_JobCompetency_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_JobCompetency_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_JobCompetency_RetrievalQuery : METAView_JobCompetency_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_JobCompetency_RetrievalQuery class.
        /// </summary>
        public METAView_JobCompetency_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_JobCompetency_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_JobCompetency_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_JobCompetency_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_JobCompetency_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_JobCompetency_RetrievalQuery

    #region METAView_JobPosition_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_JobPosition_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_JobPosition_ListingFilters : METAView_JobPosition_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_JobPosition_ListingFilters class.
        /// </summary>
        public METAView_JobPosition_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_JobPosition_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_JobPosition_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_JobPosition_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_JobPosition_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_JobPosition_ListingFilters

    #region METAView_JobPosition_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_JobPosition_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_JobPosition_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_JobPosition_ListingQuery : METAView_JobPosition_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_JobPosition_ListingQuery class.
        /// </summary>
        public METAView_JobPosition_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_JobPosition_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_JobPosition_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_JobPosition_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_JobPosition_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_JobPosition_ListingQuery

    #region METAView_JobPosition_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_JobPosition_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_JobPosition_RetrievalFilters : METAView_JobPosition_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_JobPosition_RetrievalFilters class.
        /// </summary>
        public METAView_JobPosition_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_JobPosition_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_JobPosition_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_JobPosition_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_JobPosition_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_JobPosition_RetrievalFilters

    #region METAView_JobPosition_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_JobPosition_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_JobPosition_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_JobPosition_RetrievalQuery : METAView_JobPosition_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_JobPosition_RetrievalQuery class.
        /// </summary>
        public METAView_JobPosition_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_JobPosition_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_JobPosition_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_JobPosition_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_JobPosition_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_JobPosition_RetrievalQuery

    #region METAView_Location_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Location_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Location_ListingFilters : METAView_Location_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Location_ListingFilters class.
        /// </summary>
        public METAView_Location_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Location_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Location_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Location_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Location_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Location_ListingFilters

    #region METAView_Location_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Location_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Location_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Location_ListingQuery : METAView_Location_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Location_ListingQuery class.
        /// </summary>
        public METAView_Location_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Location_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Location_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Location_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Location_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Location_ListingQuery

    #region METAView_Location_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Location_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Location_RetrievalFilters : METAView_Location_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Location_RetrievalFilters class.
        /// </summary>
        public METAView_Location_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Location_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Location_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Location_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Location_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Location_RetrievalFilters

    #region METAView_Location_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Location_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Location_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Location_RetrievalQuery : METAView_Location_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Location_RetrievalQuery class.
        /// </summary>
        public METAView_Location_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Location_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Location_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Location_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Location_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Location_RetrievalQuery

    #region METAView_LocationAssociation_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_LocationAssociation_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_LocationAssociation_ListingFilters : METAView_LocationAssociation_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_LocationAssociation_ListingFilters class.
        /// </summary>
        public METAView_LocationAssociation_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_LocationAssociation_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_LocationAssociation_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_LocationAssociation_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_LocationAssociation_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_LocationAssociation_ListingFilters

    #region METAView_LocationAssociation_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_LocationAssociation_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_LocationAssociation_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_LocationAssociation_ListingQuery : METAView_LocationAssociation_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_LocationAssociation_ListingQuery class.
        /// </summary>
        public METAView_LocationAssociation_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_LocationAssociation_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_LocationAssociation_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_LocationAssociation_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_LocationAssociation_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_LocationAssociation_ListingQuery

    #region METAView_LocationAssociation_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_LocationAssociation_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_LocationAssociation_RetrievalFilters : METAView_LocationAssociation_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_LocationAssociation_RetrievalFilters class.
        /// </summary>
        public METAView_LocationAssociation_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_LocationAssociation_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_LocationAssociation_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_LocationAssociation_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_LocationAssociation_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_LocationAssociation_RetrievalFilters

    #region METAView_LocationAssociation_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_LocationAssociation_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_LocationAssociation_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_LocationAssociation_RetrievalQuery : METAView_LocationAssociation_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_LocationAssociation_RetrievalQuery class.
        /// </summary>
        public METAView_LocationAssociation_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_LocationAssociation_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_LocationAssociation_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_LocationAssociation_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_LocationAssociation_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_LocationAssociation_RetrievalQuery

    #region METAView_LocationScheme_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_LocationScheme_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_LocationScheme_ListingFilters : METAView_LocationScheme_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_LocationScheme_ListingFilters class.
        /// </summary>
        public METAView_LocationScheme_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_LocationScheme_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_LocationScheme_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_LocationScheme_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_LocationScheme_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_LocationScheme_ListingFilters

    #region METAView_LocationScheme_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_LocationScheme_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_LocationScheme_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_LocationScheme_ListingQuery : METAView_LocationScheme_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_LocationScheme_ListingQuery class.
        /// </summary>
        public METAView_LocationScheme_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_LocationScheme_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_LocationScheme_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_LocationScheme_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_LocationScheme_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_LocationScheme_ListingQuery

    #region METAView_LocationScheme_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_LocationScheme_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_LocationScheme_RetrievalFilters : METAView_LocationScheme_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_LocationScheme_RetrievalFilters class.
        /// </summary>
        public METAView_LocationScheme_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_LocationScheme_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_LocationScheme_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_LocationScheme_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_LocationScheme_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_LocationScheme_RetrievalFilters

    #region METAView_LocationScheme_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_LocationScheme_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_LocationScheme_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_LocationScheme_RetrievalQuery : METAView_LocationScheme_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_LocationScheme_RetrievalQuery class.
        /// </summary>
        public METAView_LocationScheme_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_LocationScheme_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_LocationScheme_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_LocationScheme_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_LocationScheme_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_LocationScheme_RetrievalQuery

    #region METAView_LocationUnit_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_LocationUnit_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_LocationUnit_ListingFilters : METAView_LocationUnit_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_LocationUnit_ListingFilters class.
        /// </summary>
        public METAView_LocationUnit_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_LocationUnit_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_LocationUnit_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_LocationUnit_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_LocationUnit_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_LocationUnit_ListingFilters

    #region METAView_LocationUnit_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_LocationUnit_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_LocationUnit_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_LocationUnit_ListingQuery : METAView_LocationUnit_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_LocationUnit_ListingQuery class.
        /// </summary>
        public METAView_LocationUnit_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_LocationUnit_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_LocationUnit_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_LocationUnit_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_LocationUnit_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_LocationUnit_ListingQuery

    #region METAView_LocationUnit_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_LocationUnit_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_LocationUnit_RetrievalFilters : METAView_LocationUnit_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_LocationUnit_RetrievalFilters class.
        /// </summary>
        public METAView_LocationUnit_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_LocationUnit_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_LocationUnit_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_LocationUnit_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_LocationUnit_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_LocationUnit_RetrievalFilters

    #region METAView_LocationUnit_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_LocationUnit_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_LocationUnit_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_LocationUnit_RetrievalQuery : METAView_LocationUnit_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_LocationUnit_RetrievalQuery class.
        /// </summary>
        public METAView_LocationUnit_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_LocationUnit_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_LocationUnit_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_LocationUnit_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_LocationUnit_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_LocationUnit_RetrievalQuery

    #region METAView_Logic_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Logic_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Logic_ListingFilters : METAView_Logic_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Logic_ListingFilters class.
        /// </summary>
        public METAView_Logic_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Logic_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Logic_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Logic_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Logic_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Logic_ListingFilters

    #region METAView_Logic_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Logic_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Logic_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Logic_ListingQuery : METAView_Logic_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Logic_ListingQuery class.
        /// </summary>
        public METAView_Logic_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Logic_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Logic_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Logic_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Logic_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Logic_ListingQuery

    #region METAView_Logic_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Logic_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Logic_RetrievalFilters : METAView_Logic_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Logic_RetrievalFilters class.
        /// </summary>
        public METAView_Logic_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Logic_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Logic_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Logic_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Logic_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Logic_RetrievalFilters

    #region METAView_Logic_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Logic_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Logic_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Logic_RetrievalQuery : METAView_Logic_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Logic_RetrievalQuery class.
        /// </summary>
        public METAView_Logic_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Logic_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Logic_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Logic_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Logic_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Logic_RetrievalQuery

    #region METAView_LogicalInformationArtefact_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_LogicalInformationArtefact_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_LogicalInformationArtefact_ListingFilters : METAView_LogicalInformationArtefact_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalInformationArtefact_ListingFilters class.
        /// </summary>
        public METAView_LogicalInformationArtefact_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalInformationArtefact_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_LogicalInformationArtefact_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalInformationArtefact_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_LogicalInformationArtefact_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_LogicalInformationArtefact_ListingFilters

    #region METAView_LogicalInformationArtefact_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_LogicalInformationArtefact_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_LogicalInformationArtefact_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_LogicalInformationArtefact_ListingQuery : METAView_LogicalInformationArtefact_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalInformationArtefact_ListingQuery class.
        /// </summary>
        public METAView_LogicalInformationArtefact_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalInformationArtefact_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_LogicalInformationArtefact_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalInformationArtefact_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_LogicalInformationArtefact_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_LogicalInformationArtefact_ListingQuery

    #region METAView_LogicalInformationArtefact_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_LogicalInformationArtefact_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_LogicalInformationArtefact_RetrievalFilters : METAView_LogicalInformationArtefact_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalInformationArtefact_RetrievalFilters class.
        /// </summary>
        public METAView_LogicalInformationArtefact_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalInformationArtefact_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_LogicalInformationArtefact_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalInformationArtefact_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_LogicalInformationArtefact_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_LogicalInformationArtefact_RetrievalFilters

    #region METAView_LogicalInformationArtefact_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_LogicalInformationArtefact_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_LogicalInformationArtefact_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_LogicalInformationArtefact_RetrievalQuery : METAView_LogicalInformationArtefact_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalInformationArtefact_RetrievalQuery class.
        /// </summary>
        public METAView_LogicalInformationArtefact_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalInformationArtefact_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_LogicalInformationArtefact_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalInformationArtefact_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_LogicalInformationArtefact_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_LogicalInformationArtefact_RetrievalQuery

    #region METAView_LogicalITInfrastructureComponent_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_LogicalITInfrastructureComponent_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_LogicalITInfrastructureComponent_ListingFilters : METAView_LogicalITInfrastructureComponent_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalITInfrastructureComponent_ListingFilters class.
        /// </summary>
        public METAView_LogicalITInfrastructureComponent_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalITInfrastructureComponent_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_LogicalITInfrastructureComponent_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalITInfrastructureComponent_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_LogicalITInfrastructureComponent_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_LogicalITInfrastructureComponent_ListingFilters

    #region METAView_LogicalITInfrastructureComponent_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_LogicalITInfrastructureComponent_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_LogicalITInfrastructureComponent_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_LogicalITInfrastructureComponent_ListingQuery : METAView_LogicalITInfrastructureComponent_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalITInfrastructureComponent_ListingQuery class.
        /// </summary>
        public METAView_LogicalITInfrastructureComponent_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalITInfrastructureComponent_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_LogicalITInfrastructureComponent_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalITInfrastructureComponent_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_LogicalITInfrastructureComponent_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_LogicalITInfrastructureComponent_ListingQuery

    #region METAView_LogicalITInfrastructureComponent_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_LogicalITInfrastructureComponent_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_LogicalITInfrastructureComponent_RetrievalFilters : METAView_LogicalITInfrastructureComponent_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalITInfrastructureComponent_RetrievalFilters class.
        /// </summary>
        public METAView_LogicalITInfrastructureComponent_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalITInfrastructureComponent_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_LogicalITInfrastructureComponent_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalITInfrastructureComponent_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_LogicalITInfrastructureComponent_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_LogicalITInfrastructureComponent_RetrievalFilters

    #region METAView_LogicalITInfrastructureComponent_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_LogicalITInfrastructureComponent_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_LogicalITInfrastructureComponent_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_LogicalITInfrastructureComponent_RetrievalQuery : METAView_LogicalITInfrastructureComponent_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalITInfrastructureComponent_RetrievalQuery class.
        /// </summary>
        public METAView_LogicalITInfrastructureComponent_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalITInfrastructureComponent_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_LogicalITInfrastructureComponent_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalITInfrastructureComponent_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_LogicalITInfrastructureComponent_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_LogicalITInfrastructureComponent_RetrievalQuery

    #region METAView_LogicalSoftwareComponent_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_LogicalSoftwareComponent_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_LogicalSoftwareComponent_ListingFilters : METAView_LogicalSoftwareComponent_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalSoftwareComponent_ListingFilters class.
        /// </summary>
        public METAView_LogicalSoftwareComponent_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalSoftwareComponent_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_LogicalSoftwareComponent_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalSoftwareComponent_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_LogicalSoftwareComponent_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_LogicalSoftwareComponent_ListingFilters

    #region METAView_LogicalSoftwareComponent_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_LogicalSoftwareComponent_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_LogicalSoftwareComponent_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_LogicalSoftwareComponent_ListingQuery : METAView_LogicalSoftwareComponent_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalSoftwareComponent_ListingQuery class.
        /// </summary>
        public METAView_LogicalSoftwareComponent_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalSoftwareComponent_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_LogicalSoftwareComponent_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalSoftwareComponent_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_LogicalSoftwareComponent_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_LogicalSoftwareComponent_ListingQuery

    #region METAView_LogicalSoftwareComponent_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_LogicalSoftwareComponent_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_LogicalSoftwareComponent_RetrievalFilters : METAView_LogicalSoftwareComponent_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalSoftwareComponent_RetrievalFilters class.
        /// </summary>
        public METAView_LogicalSoftwareComponent_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalSoftwareComponent_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_LogicalSoftwareComponent_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalSoftwareComponent_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_LogicalSoftwareComponent_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_LogicalSoftwareComponent_RetrievalFilters

    #region METAView_LogicalSoftwareComponent_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_LogicalSoftwareComponent_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_LogicalSoftwareComponent_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_LogicalSoftwareComponent_RetrievalQuery : METAView_LogicalSoftwareComponent_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalSoftwareComponent_RetrievalQuery class.
        /// </summary>
        public METAView_LogicalSoftwareComponent_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalSoftwareComponent_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_LogicalSoftwareComponent_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_LogicalSoftwareComponent_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_LogicalSoftwareComponent_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_LogicalSoftwareComponent_RetrievalQuery

    #region METAView_MarketSegment_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_MarketSegment_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_MarketSegment_ListingFilters : METAView_MarketSegment_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_MarketSegment_ListingFilters class.
        /// </summary>
        public METAView_MarketSegment_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_MarketSegment_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_MarketSegment_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_MarketSegment_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_MarketSegment_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_MarketSegment_ListingFilters

    #region METAView_MarketSegment_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_MarketSegment_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_MarketSegment_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_MarketSegment_ListingQuery : METAView_MarketSegment_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_MarketSegment_ListingQuery class.
        /// </summary>
        public METAView_MarketSegment_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_MarketSegment_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_MarketSegment_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_MarketSegment_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_MarketSegment_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_MarketSegment_ListingQuery

    #region METAView_MarketSegment_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_MarketSegment_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_MarketSegment_RetrievalFilters : METAView_MarketSegment_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_MarketSegment_RetrievalFilters class.
        /// </summary>
        public METAView_MarketSegment_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_MarketSegment_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_MarketSegment_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_MarketSegment_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_MarketSegment_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_MarketSegment_RetrievalFilters

    #region METAView_MarketSegment_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_MarketSegment_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_MarketSegment_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_MarketSegment_RetrievalQuery : METAView_MarketSegment_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_MarketSegment_RetrievalQuery class.
        /// </summary>
        public METAView_MarketSegment_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_MarketSegment_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_MarketSegment_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_MarketSegment_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_MarketSegment_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_MarketSegment_RetrievalQuery

    #region METAView_Measure_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Measure_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Measure_ListingFilters : METAView_Measure_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Measure_ListingFilters class.
        /// </summary>
        public METAView_Measure_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Measure_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Measure_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Measure_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Measure_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Measure_ListingFilters

    #region METAView_Measure_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Measure_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Measure_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Measure_ListingQuery : METAView_Measure_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Measure_ListingQuery class.
        /// </summary>
        public METAView_Measure_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Measure_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Measure_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Measure_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Measure_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Measure_ListingQuery

    #region METAView_Measure_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Measure_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Measure_RetrievalFilters : METAView_Measure_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Measure_RetrievalFilters class.
        /// </summary>
        public METAView_Measure_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Measure_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Measure_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Measure_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Measure_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Measure_RetrievalFilters

    #region METAView_Measure_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Measure_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Measure_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Measure_RetrievalQuery : METAView_Measure_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Measure_RetrievalQuery class.
        /// </summary>
        public METAView_Measure_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Measure_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Measure_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Measure_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Measure_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Measure_RetrievalQuery

    #region METAView_MeasurementItem_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_MeasurementItem_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_MeasurementItem_ListingFilters : METAView_MeasurementItem_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_MeasurementItem_ListingFilters class.
        /// </summary>
        public METAView_MeasurementItem_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_MeasurementItem_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_MeasurementItem_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_MeasurementItem_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_MeasurementItem_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_MeasurementItem_ListingFilters

    #region METAView_MeasurementItem_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_MeasurementItem_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_MeasurementItem_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_MeasurementItem_ListingQuery : METAView_MeasurementItem_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_MeasurementItem_ListingQuery class.
        /// </summary>
        public METAView_MeasurementItem_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_MeasurementItem_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_MeasurementItem_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_MeasurementItem_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_MeasurementItem_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_MeasurementItem_ListingQuery

    #region METAView_MeasurementItem_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_MeasurementItem_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_MeasurementItem_RetrievalFilters : METAView_MeasurementItem_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_MeasurementItem_RetrievalFilters class.
        /// </summary>
        public METAView_MeasurementItem_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_MeasurementItem_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_MeasurementItem_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_MeasurementItem_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_MeasurementItem_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_MeasurementItem_RetrievalFilters

    #region METAView_MeasurementItem_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_MeasurementItem_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_MeasurementItem_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_MeasurementItem_RetrievalQuery : METAView_MeasurementItem_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_MeasurementItem_RetrievalQuery class.
        /// </summary>
        public METAView_MeasurementItem_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_MeasurementItem_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_MeasurementItem_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_MeasurementItem_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_MeasurementItem_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_MeasurementItem_RetrievalQuery

    #region METAView_Model_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Model_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Model_ListingFilters : METAView_Model_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Model_ListingFilters class.
        /// </summary>
        public METAView_Model_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Model_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Model_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Model_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Model_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Model_ListingFilters

    #region METAView_Model_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Model_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Model_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Model_ListingQuery : METAView_Model_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Model_ListingQuery class.
        /// </summary>
        public METAView_Model_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Model_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Model_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Model_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Model_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Model_ListingQuery

    #region METAView_Model_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Model_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Model_RetrievalFilters : METAView_Model_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Model_RetrievalFilters class.
        /// </summary>
        public METAView_Model_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Model_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Model_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Model_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Model_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Model_RetrievalFilters

    #region METAView_Model_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Model_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Model_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Model_RetrievalQuery : METAView_Model_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Model_RetrievalQuery class.
        /// </summary>
        public METAView_Model_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Model_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Model_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Model_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Model_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Model_RetrievalQuery

    #region METAView_ModelReal_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_ModelReal_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ModelReal_ListingFilters : METAView_ModelReal_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ModelReal_ListingFilters class.
        /// </summary>
        public METAView_ModelReal_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ModelReal_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ModelReal_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ModelReal_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ModelReal_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ModelReal_ListingFilters

    #region METAView_ModelReal_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_ModelReal_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_ModelReal_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ModelReal_ListingQuery : METAView_ModelReal_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ModelReal_ListingQuery class.
        /// </summary>
        public METAView_ModelReal_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ModelReal_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ModelReal_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ModelReal_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ModelReal_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ModelReal_ListingQuery

    #region METAView_ModelReal_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_ModelReal_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ModelReal_RetrievalFilters : METAView_ModelReal_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ModelReal_RetrievalFilters class.
        /// </summary>
        public METAView_ModelReal_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ModelReal_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ModelReal_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ModelReal_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ModelReal_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ModelReal_RetrievalFilters

    #region METAView_ModelReal_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_ModelReal_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_ModelReal_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ModelReal_RetrievalQuery : METAView_ModelReal_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ModelReal_RetrievalQuery class.
        /// </summary>
        public METAView_ModelReal_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ModelReal_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ModelReal_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ModelReal_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ModelReal_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ModelReal_RetrievalQuery

    #region METAView_MutuallyExclusiveIndicator_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_MutuallyExclusiveIndicator_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_MutuallyExclusiveIndicator_ListingFilters : METAView_MutuallyExclusiveIndicator_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_MutuallyExclusiveIndicator_ListingFilters class.
        /// </summary>
        public METAView_MutuallyExclusiveIndicator_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_MutuallyExclusiveIndicator_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_MutuallyExclusiveIndicator_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_MutuallyExclusiveIndicator_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_MutuallyExclusiveIndicator_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_MutuallyExclusiveIndicator_ListingFilters

    #region METAView_MutuallyExclusiveIndicator_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_MutuallyExclusiveIndicator_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_MutuallyExclusiveIndicator_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_MutuallyExclusiveIndicator_ListingQuery : METAView_MutuallyExclusiveIndicator_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_MutuallyExclusiveIndicator_ListingQuery class.
        /// </summary>
        public METAView_MutuallyExclusiveIndicator_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_MutuallyExclusiveIndicator_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_MutuallyExclusiveIndicator_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_MutuallyExclusiveIndicator_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_MutuallyExclusiveIndicator_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_MutuallyExclusiveIndicator_ListingQuery

    #region METAView_MutuallyExclusiveIndicator_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_MutuallyExclusiveIndicator_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_MutuallyExclusiveIndicator_RetrievalFilters : METAView_MutuallyExclusiveIndicator_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_MutuallyExclusiveIndicator_RetrievalFilters class.
        /// </summary>
        public METAView_MutuallyExclusiveIndicator_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_MutuallyExclusiveIndicator_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_MutuallyExclusiveIndicator_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_MutuallyExclusiveIndicator_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_MutuallyExclusiveIndicator_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_MutuallyExclusiveIndicator_RetrievalFilters

    #region METAView_MutuallyExclusiveIndicator_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_MutuallyExclusiveIndicator_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_MutuallyExclusiveIndicator_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_MutuallyExclusiveIndicator_RetrievalQuery : METAView_MutuallyExclusiveIndicator_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_MutuallyExclusiveIndicator_RetrievalQuery class.
        /// </summary>
        public METAView_MutuallyExclusiveIndicator_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_MutuallyExclusiveIndicator_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_MutuallyExclusiveIndicator_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_MutuallyExclusiveIndicator_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_MutuallyExclusiveIndicator_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_MutuallyExclusiveIndicator_RetrievalQuery

    #region METAView_Network_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Network_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Network_ListingFilters : METAView_Network_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Network_ListingFilters class.
        /// </summary>
        public METAView_Network_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Network_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Network_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Network_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Network_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Network_ListingFilters

    #region METAView_Network_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Network_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Network_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Network_ListingQuery : METAView_Network_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Network_ListingQuery class.
        /// </summary>
        public METAView_Network_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Network_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Network_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Network_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Network_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Network_ListingQuery

    #region METAView_Network_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Network_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Network_RetrievalFilters : METAView_Network_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Network_RetrievalFilters class.
        /// </summary>
        public METAView_Network_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Network_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Network_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Network_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Network_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Network_RetrievalFilters

    #region METAView_Network_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Network_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Network_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Network_RetrievalQuery : METAView_Network_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Network_RetrievalQuery class.
        /// </summary>
        public METAView_Network_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Network_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Network_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Network_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Network_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Network_RetrievalQuery

    #region METAView_NetworkComponent_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_NetworkComponent_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_NetworkComponent_ListingFilters : METAView_NetworkComponent_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_NetworkComponent_ListingFilters class.
        /// </summary>
        public METAView_NetworkComponent_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_NetworkComponent_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_NetworkComponent_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_NetworkComponent_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_NetworkComponent_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_NetworkComponent_ListingFilters

    #region METAView_NetworkComponent_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_NetworkComponent_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_NetworkComponent_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_NetworkComponent_ListingQuery : METAView_NetworkComponent_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_NetworkComponent_ListingQuery class.
        /// </summary>
        public METAView_NetworkComponent_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_NetworkComponent_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_NetworkComponent_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_NetworkComponent_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_NetworkComponent_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_NetworkComponent_ListingQuery

    #region METAView_NetworkComponent_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_NetworkComponent_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_NetworkComponent_RetrievalFilters : METAView_NetworkComponent_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_NetworkComponent_RetrievalFilters class.
        /// </summary>
        public METAView_NetworkComponent_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_NetworkComponent_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_NetworkComponent_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_NetworkComponent_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_NetworkComponent_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_NetworkComponent_RetrievalFilters

    #region METAView_NetworkComponent_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_NetworkComponent_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_NetworkComponent_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_NetworkComponent_RetrievalQuery : METAView_NetworkComponent_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_NetworkComponent_RetrievalQuery class.
        /// </summary>
        public METAView_NetworkComponent_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_NetworkComponent_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_NetworkComponent_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_NetworkComponent_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_NetworkComponent_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_NetworkComponent_RetrievalQuery

    #region METAView_Object_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Object_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Object_ListingFilters : METAView_Object_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Object_ListingFilters class.
        /// </summary>
        public METAView_Object_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Object_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Object_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Object_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Object_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Object_ListingFilters

    #region METAView_Object_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Object_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Object_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Object_ListingQuery : METAView_Object_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Object_ListingQuery class.
        /// </summary>
        public METAView_Object_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Object_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Object_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Object_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Object_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Object_ListingQuery

    #region METAView_Object_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Object_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Object_RetrievalFilters : METAView_Object_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Object_RetrievalFilters class.
        /// </summary>
        public METAView_Object_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Object_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Object_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Object_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Object_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Object_RetrievalFilters

    #region METAView_Object_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Object_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Object_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Object_RetrievalQuery : METAView_Object_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Object_RetrievalQuery class.
        /// </summary>
        public METAView_Object_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Object_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Object_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Object_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Object_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Object_RetrievalQuery

    #region METAView_OrganizationalUnit_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_OrganizationalUnit_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_OrganizationalUnit_ListingFilters : METAView_OrganizationalUnit_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_OrganizationalUnit_ListingFilters class.
        /// </summary>
        public METAView_OrganizationalUnit_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_OrganizationalUnit_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_OrganizationalUnit_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_OrganizationalUnit_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_OrganizationalUnit_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_OrganizationalUnit_ListingFilters

    #region METAView_OrganizationalUnit_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_OrganizationalUnit_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_OrganizationalUnit_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_OrganizationalUnit_ListingQuery : METAView_OrganizationalUnit_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_OrganizationalUnit_ListingQuery class.
        /// </summary>
        public METAView_OrganizationalUnit_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_OrganizationalUnit_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_OrganizationalUnit_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_OrganizationalUnit_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_OrganizationalUnit_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_OrganizationalUnit_ListingQuery

    #region METAView_OrganizationalUnit_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_OrganizationalUnit_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_OrganizationalUnit_RetrievalFilters : METAView_OrganizationalUnit_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_OrganizationalUnit_RetrievalFilters class.
        /// </summary>
        public METAView_OrganizationalUnit_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_OrganizationalUnit_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_OrganizationalUnit_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_OrganizationalUnit_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_OrganizationalUnit_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_OrganizationalUnit_RetrievalFilters

    #region METAView_OrganizationalUnit_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_OrganizationalUnit_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_OrganizationalUnit_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_OrganizationalUnit_RetrievalQuery : METAView_OrganizationalUnit_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_OrganizationalUnit_RetrievalQuery class.
        /// </summary>
        public METAView_OrganizationalUnit_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_OrganizationalUnit_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_OrganizationalUnit_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_OrganizationalUnit_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_OrganizationalUnit_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_OrganizationalUnit_RetrievalQuery

    #region METAView_Peripheral_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Peripheral_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Peripheral_ListingFilters : METAView_Peripheral_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Peripheral_ListingFilters class.
        /// </summary>
        public METAView_Peripheral_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Peripheral_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Peripheral_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Peripheral_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Peripheral_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Peripheral_ListingFilters

    #region METAView_Peripheral_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Peripheral_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Peripheral_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Peripheral_ListingQuery : METAView_Peripheral_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Peripheral_ListingQuery class.
        /// </summary>
        public METAView_Peripheral_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Peripheral_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Peripheral_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Peripheral_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Peripheral_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Peripheral_ListingQuery

    #region METAView_Peripheral_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Peripheral_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Peripheral_RetrievalFilters : METAView_Peripheral_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Peripheral_RetrievalFilters class.
        /// </summary>
        public METAView_Peripheral_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Peripheral_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Peripheral_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Peripheral_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Peripheral_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Peripheral_RetrievalFilters

    #region METAView_Peripheral_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Peripheral_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Peripheral_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Peripheral_RetrievalQuery : METAView_Peripheral_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Peripheral_RetrievalQuery class.
        /// </summary>
        public METAView_Peripheral_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Peripheral_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Peripheral_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Peripheral_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Peripheral_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Peripheral_RetrievalQuery

    #region METAView_PeripheralComponent_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_PeripheralComponent_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_PeripheralComponent_ListingFilters : METAView_PeripheralComponent_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_PeripheralComponent_ListingFilters class.
        /// </summary>
        public METAView_PeripheralComponent_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_PeripheralComponent_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_PeripheralComponent_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_PeripheralComponent_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_PeripheralComponent_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_PeripheralComponent_ListingFilters

    #region METAView_PeripheralComponent_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_PeripheralComponent_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_PeripheralComponent_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_PeripheralComponent_ListingQuery : METAView_PeripheralComponent_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_PeripheralComponent_ListingQuery class.
        /// </summary>
        public METAView_PeripheralComponent_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_PeripheralComponent_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_PeripheralComponent_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_PeripheralComponent_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_PeripheralComponent_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_PeripheralComponent_ListingQuery

    #region METAView_PeripheralComponent_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_PeripheralComponent_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_PeripheralComponent_RetrievalFilters : METAView_PeripheralComponent_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_PeripheralComponent_RetrievalFilters class.
        /// </summary>
        public METAView_PeripheralComponent_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_PeripheralComponent_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_PeripheralComponent_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_PeripheralComponent_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_PeripheralComponent_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_PeripheralComponent_RetrievalFilters

    #region METAView_PeripheralComponent_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_PeripheralComponent_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_PeripheralComponent_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_PeripheralComponent_RetrievalQuery : METAView_PeripheralComponent_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_PeripheralComponent_RetrievalQuery class.
        /// </summary>
        public METAView_PeripheralComponent_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_PeripheralComponent_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_PeripheralComponent_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_PeripheralComponent_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_PeripheralComponent_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_PeripheralComponent_RetrievalQuery

    #region METAView_Person_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Person_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Person_ListingFilters : METAView_Person_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Person_ListingFilters class.
        /// </summary>
        public METAView_Person_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Person_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Person_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Person_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Person_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Person_ListingFilters

    #region METAView_Person_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Person_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Person_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Person_ListingQuery : METAView_Person_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Person_ListingQuery class.
        /// </summary>
        public METAView_Person_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Person_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Person_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Person_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Person_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Person_ListingQuery

    #region METAView_Person_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Person_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Person_RetrievalFilters : METAView_Person_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Person_RetrievalFilters class.
        /// </summary>
        public METAView_Person_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Person_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Person_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Person_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Person_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Person_RetrievalFilters

    #region METAView_Person_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Person_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Person_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Person_RetrievalQuery : METAView_Person_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Person_RetrievalQuery class.
        /// </summary>
        public METAView_Person_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Person_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Person_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Person_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Person_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Person_RetrievalQuery

    #region METAView_PhysicalDataComponent_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_PhysicalDataComponent_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_PhysicalDataComponent_ListingFilters : METAView_PhysicalDataComponent_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalDataComponent_ListingFilters class.
        /// </summary>
        public METAView_PhysicalDataComponent_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalDataComponent_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_PhysicalDataComponent_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalDataComponent_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_PhysicalDataComponent_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_PhysicalDataComponent_ListingFilters

    #region METAView_PhysicalDataComponent_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_PhysicalDataComponent_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_PhysicalDataComponent_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_PhysicalDataComponent_ListingQuery : METAView_PhysicalDataComponent_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalDataComponent_ListingQuery class.
        /// </summary>
        public METAView_PhysicalDataComponent_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalDataComponent_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_PhysicalDataComponent_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalDataComponent_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_PhysicalDataComponent_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_PhysicalDataComponent_ListingQuery

    #region METAView_PhysicalDataComponent_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_PhysicalDataComponent_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_PhysicalDataComponent_RetrievalFilters : METAView_PhysicalDataComponent_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalDataComponent_RetrievalFilters class.
        /// </summary>
        public METAView_PhysicalDataComponent_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalDataComponent_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_PhysicalDataComponent_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalDataComponent_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_PhysicalDataComponent_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_PhysicalDataComponent_RetrievalFilters

    #region METAView_PhysicalDataComponent_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_PhysicalDataComponent_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_PhysicalDataComponent_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_PhysicalDataComponent_RetrievalQuery : METAView_PhysicalDataComponent_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalDataComponent_RetrievalQuery class.
        /// </summary>
        public METAView_PhysicalDataComponent_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalDataComponent_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_PhysicalDataComponent_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalDataComponent_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_PhysicalDataComponent_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_PhysicalDataComponent_RetrievalQuery

    #region METAView_PhysicalInformationArtefact_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_PhysicalInformationArtefact_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_PhysicalInformationArtefact_ListingFilters : METAView_PhysicalInformationArtefact_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalInformationArtefact_ListingFilters class.
        /// </summary>
        public METAView_PhysicalInformationArtefact_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalInformationArtefact_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_PhysicalInformationArtefact_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalInformationArtefact_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_PhysicalInformationArtefact_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_PhysicalInformationArtefact_ListingFilters

    #region METAView_PhysicalInformationArtefact_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_PhysicalInformationArtefact_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_PhysicalInformationArtefact_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_PhysicalInformationArtefact_ListingQuery : METAView_PhysicalInformationArtefact_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalInformationArtefact_ListingQuery class.
        /// </summary>
        public METAView_PhysicalInformationArtefact_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalInformationArtefact_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_PhysicalInformationArtefact_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalInformationArtefact_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_PhysicalInformationArtefact_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_PhysicalInformationArtefact_ListingQuery

    #region METAView_PhysicalInformationArtefact_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_PhysicalInformationArtefact_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_PhysicalInformationArtefact_RetrievalFilters : METAView_PhysicalInformationArtefact_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalInformationArtefact_RetrievalFilters class.
        /// </summary>
        public METAView_PhysicalInformationArtefact_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalInformationArtefact_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_PhysicalInformationArtefact_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalInformationArtefact_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_PhysicalInformationArtefact_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_PhysicalInformationArtefact_RetrievalFilters

    #region METAView_PhysicalInformationArtefact_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_PhysicalInformationArtefact_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_PhysicalInformationArtefact_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_PhysicalInformationArtefact_RetrievalQuery : METAView_PhysicalInformationArtefact_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalInformationArtefact_RetrievalQuery class.
        /// </summary>
        public METAView_PhysicalInformationArtefact_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalInformationArtefact_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_PhysicalInformationArtefact_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalInformationArtefact_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_PhysicalInformationArtefact_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_PhysicalInformationArtefact_RetrievalQuery

    #region METAView_PhysicalSoftwareComponent_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_PhysicalSoftwareComponent_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_PhysicalSoftwareComponent_ListingFilters : METAView_PhysicalSoftwareComponent_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalSoftwareComponent_ListingFilters class.
        /// </summary>
        public METAView_PhysicalSoftwareComponent_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalSoftwareComponent_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_PhysicalSoftwareComponent_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalSoftwareComponent_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_PhysicalSoftwareComponent_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_PhysicalSoftwareComponent_ListingFilters

    #region METAView_PhysicalSoftwareComponent_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_PhysicalSoftwareComponent_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_PhysicalSoftwareComponent_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_PhysicalSoftwareComponent_ListingQuery : METAView_PhysicalSoftwareComponent_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalSoftwareComponent_ListingQuery class.
        /// </summary>
        public METAView_PhysicalSoftwareComponent_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalSoftwareComponent_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_PhysicalSoftwareComponent_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalSoftwareComponent_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_PhysicalSoftwareComponent_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_PhysicalSoftwareComponent_ListingQuery

    #region METAView_PhysicalSoftwareComponent_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_PhysicalSoftwareComponent_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_PhysicalSoftwareComponent_RetrievalFilters : METAView_PhysicalSoftwareComponent_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalSoftwareComponent_RetrievalFilters class.
        /// </summary>
        public METAView_PhysicalSoftwareComponent_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalSoftwareComponent_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_PhysicalSoftwareComponent_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalSoftwareComponent_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_PhysicalSoftwareComponent_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_PhysicalSoftwareComponent_RetrievalFilters

    #region METAView_PhysicalSoftwareComponent_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_PhysicalSoftwareComponent_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_PhysicalSoftwareComponent_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_PhysicalSoftwareComponent_RetrievalQuery : METAView_PhysicalSoftwareComponent_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalSoftwareComponent_RetrievalQuery class.
        /// </summary>
        public METAView_PhysicalSoftwareComponent_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalSoftwareComponent_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_PhysicalSoftwareComponent_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_PhysicalSoftwareComponent_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_PhysicalSoftwareComponent_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_PhysicalSoftwareComponent_RetrievalQuery

    #region METAView_Position_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Position_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Position_ListingFilters : METAView_Position_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Position_ListingFilters class.
        /// </summary>
        public METAView_Position_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Position_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Position_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Position_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Position_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Position_ListingFilters

    #region METAView_Position_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Position_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Position_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Position_ListingQuery : METAView_Position_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Position_ListingQuery class.
        /// </summary>
        public METAView_Position_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Position_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Position_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Position_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Position_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Position_ListingQuery

    #region METAView_Position_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Position_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Position_RetrievalFilters : METAView_Position_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Position_RetrievalFilters class.
        /// </summary>
        public METAView_Position_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Position_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Position_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Position_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Position_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Position_RetrievalFilters

    #region METAView_Position_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Position_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Position_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Position_RetrievalQuery : METAView_Position_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Position_RetrievalQuery class.
        /// </summary>
        public METAView_Position_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Position_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Position_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Position_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Position_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Position_RetrievalQuery

    #region METAView_PrimaryDataSourceIndicator_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_PrimaryDataSourceIndicator_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_PrimaryDataSourceIndicator_ListingFilters : METAView_PrimaryDataSourceIndicator_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_PrimaryDataSourceIndicator_ListingFilters class.
        /// </summary>
        public METAView_PrimaryDataSourceIndicator_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_PrimaryDataSourceIndicator_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_PrimaryDataSourceIndicator_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_PrimaryDataSourceIndicator_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_PrimaryDataSourceIndicator_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_PrimaryDataSourceIndicator_ListingFilters

    #region METAView_PrimaryDataSourceIndicator_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_PrimaryDataSourceIndicator_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_PrimaryDataSourceIndicator_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_PrimaryDataSourceIndicator_ListingQuery : METAView_PrimaryDataSourceIndicator_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_PrimaryDataSourceIndicator_ListingQuery class.
        /// </summary>
        public METAView_PrimaryDataSourceIndicator_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_PrimaryDataSourceIndicator_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_PrimaryDataSourceIndicator_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_PrimaryDataSourceIndicator_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_PrimaryDataSourceIndicator_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_PrimaryDataSourceIndicator_ListingQuery

    #region METAView_PrimaryDataSourceIndicator_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_PrimaryDataSourceIndicator_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_PrimaryDataSourceIndicator_RetrievalFilters : METAView_PrimaryDataSourceIndicator_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_PrimaryDataSourceIndicator_RetrievalFilters class.
        /// </summary>
        public METAView_PrimaryDataSourceIndicator_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_PrimaryDataSourceIndicator_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_PrimaryDataSourceIndicator_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_PrimaryDataSourceIndicator_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_PrimaryDataSourceIndicator_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_PrimaryDataSourceIndicator_RetrievalFilters

    #region METAView_PrimaryDataSourceIndicator_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_PrimaryDataSourceIndicator_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_PrimaryDataSourceIndicator_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_PrimaryDataSourceIndicator_RetrievalQuery : METAView_PrimaryDataSourceIndicator_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_PrimaryDataSourceIndicator_RetrievalQuery class.
        /// </summary>
        public METAView_PrimaryDataSourceIndicator_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_PrimaryDataSourceIndicator_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_PrimaryDataSourceIndicator_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_PrimaryDataSourceIndicator_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_PrimaryDataSourceIndicator_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_PrimaryDataSourceIndicator_RetrievalQuery

    #region METAView_ProbabilityofRealization_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_ProbabilityofRealization_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ProbabilityofRealization_ListingFilters : METAView_ProbabilityofRealization_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ProbabilityofRealization_ListingFilters class.
        /// </summary>
        public METAView_ProbabilityofRealization_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ProbabilityofRealization_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ProbabilityofRealization_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ProbabilityofRealization_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ProbabilityofRealization_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ProbabilityofRealization_ListingFilters

    #region METAView_ProbabilityofRealization_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_ProbabilityofRealization_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_ProbabilityofRealization_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ProbabilityofRealization_ListingQuery : METAView_ProbabilityofRealization_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ProbabilityofRealization_ListingQuery class.
        /// </summary>
        public METAView_ProbabilityofRealization_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ProbabilityofRealization_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ProbabilityofRealization_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ProbabilityofRealization_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ProbabilityofRealization_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ProbabilityofRealization_ListingQuery

    #region METAView_ProbabilityofRealization_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_ProbabilityofRealization_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ProbabilityofRealization_RetrievalFilters : METAView_ProbabilityofRealization_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ProbabilityofRealization_RetrievalFilters class.
        /// </summary>
        public METAView_ProbabilityofRealization_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ProbabilityofRealization_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ProbabilityofRealization_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ProbabilityofRealization_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ProbabilityofRealization_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ProbabilityofRealization_RetrievalFilters

    #region METAView_ProbabilityofRealization_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_ProbabilityofRealization_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_ProbabilityofRealization_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ProbabilityofRealization_RetrievalQuery : METAView_ProbabilityofRealization_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ProbabilityofRealization_RetrievalQuery class.
        /// </summary>
        public METAView_ProbabilityofRealization_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ProbabilityofRealization_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ProbabilityofRealization_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ProbabilityofRealization_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ProbabilityofRealization_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ProbabilityofRealization_RetrievalQuery

    #region METAView_ProbOfRealization_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_ProbOfRealization_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ProbOfRealization_ListingFilters : METAView_ProbOfRealization_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ProbOfRealization_ListingFilters class.
        /// </summary>
        public METAView_ProbOfRealization_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ProbOfRealization_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ProbOfRealization_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ProbOfRealization_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ProbOfRealization_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ProbOfRealization_ListingFilters

    #region METAView_ProbOfRealization_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_ProbOfRealization_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_ProbOfRealization_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ProbOfRealization_ListingQuery : METAView_ProbOfRealization_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ProbOfRealization_ListingQuery class.
        /// </summary>
        public METAView_ProbOfRealization_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ProbOfRealization_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ProbOfRealization_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ProbOfRealization_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ProbOfRealization_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ProbOfRealization_ListingQuery

    #region METAView_ProbOfRealization_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_ProbOfRealization_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ProbOfRealization_RetrievalFilters : METAView_ProbOfRealization_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ProbOfRealization_RetrievalFilters class.
        /// </summary>
        public METAView_ProbOfRealization_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ProbOfRealization_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ProbOfRealization_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ProbOfRealization_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ProbOfRealization_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ProbOfRealization_RetrievalFilters

    #region METAView_ProbOfRealization_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_ProbOfRealization_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_ProbOfRealization_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ProbOfRealization_RetrievalQuery : METAView_ProbOfRealization_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ProbOfRealization_RetrievalQuery class.
        /// </summary>
        public METAView_ProbOfRealization_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ProbOfRealization_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ProbOfRealization_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ProbOfRealization_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ProbOfRealization_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ProbOfRealization_RetrievalQuery

    #region METAView_Process_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Process_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Process_ListingFilters : METAView_Process_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Process_ListingFilters class.
        /// </summary>
        public METAView_Process_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Process_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Process_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Process_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Process_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Process_ListingFilters

    #region METAView_Process_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Process_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Process_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Process_ListingQuery : METAView_Process_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Process_ListingQuery class.
        /// </summary>
        public METAView_Process_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Process_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Process_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Process_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Process_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Process_ListingQuery

    #region METAView_Process_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Process_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Process_RetrievalFilters : METAView_Process_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Process_RetrievalFilters class.
        /// </summary>
        public METAView_Process_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Process_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Process_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Process_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Process_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Process_RetrievalFilters

    #region METAView_Process_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Process_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Process_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Process_RetrievalQuery : METAView_Process_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Process_RetrievalQuery class.
        /// </summary>
        public METAView_Process_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Process_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Process_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Process_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Process_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Process_RetrievalQuery

    #region METAView_Rationale_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Rationale_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Rationale_ListingFilters : METAView_Rationale_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Rationale_ListingFilters class.
        /// </summary>
        public METAView_Rationale_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Rationale_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Rationale_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Rationale_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Rationale_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Rationale_ListingFilters

    #region METAView_Rationale_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Rationale_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Rationale_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Rationale_ListingQuery : METAView_Rationale_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Rationale_ListingQuery class.
        /// </summary>
        public METAView_Rationale_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Rationale_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Rationale_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Rationale_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Rationale_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Rationale_ListingQuery

    #region METAView_Rationale_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Rationale_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Rationale_RetrievalFilters : METAView_Rationale_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Rationale_RetrievalFilters class.
        /// </summary>
        public METAView_Rationale_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Rationale_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Rationale_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Rationale_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Rationale_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Rationale_RetrievalFilters

    #region METAView_Rationale_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Rationale_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Rationale_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Rationale_RetrievalQuery : METAView_Rationale_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Rationale_RetrievalQuery class.
        /// </summary>
        public METAView_Rationale_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Rationale_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Rationale_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Rationale_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Rationale_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Rationale_RetrievalQuery

    #region METAView_Report_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Report_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Report_ListingFilters : METAView_Report_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Report_ListingFilters class.
        /// </summary>
        public METAView_Report_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Report_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Report_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Report_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Report_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Report_ListingFilters

    #region METAView_Report_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Report_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Report_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Report_ListingQuery : METAView_Report_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Report_ListingQuery class.
        /// </summary>
        public METAView_Report_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Report_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Report_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Report_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Report_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Report_ListingQuery

    #region METAView_Report_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Report_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Report_RetrievalFilters : METAView_Report_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Report_RetrievalFilters class.
        /// </summary>
        public METAView_Report_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Report_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Report_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Report_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Report_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Report_RetrievalFilters

    #region METAView_Report_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Report_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Report_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Report_RetrievalQuery : METAView_Report_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Report_RetrievalQuery class.
        /// </summary>
        public METAView_Report_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Report_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Report_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Report_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Report_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Report_RetrievalQuery

    #region METAView_Resource_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Resource_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Resource_ListingFilters : METAView_Resource_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Resource_ListingFilters class.
        /// </summary>
        public METAView_Resource_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Resource_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Resource_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Resource_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Resource_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Resource_ListingFilters

    #region METAView_Resource_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Resource_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Resource_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Resource_ListingQuery : METAView_Resource_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Resource_ListingQuery class.
        /// </summary>
        public METAView_Resource_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Resource_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Resource_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Resource_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Resource_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Resource_ListingQuery

    #region METAView_Resource_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Resource_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Resource_RetrievalFilters : METAView_Resource_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Resource_RetrievalFilters class.
        /// </summary>
        public METAView_Resource_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Resource_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Resource_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Resource_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Resource_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Resource_RetrievalFilters

    #region METAView_Resource_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Resource_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Resource_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Resource_RetrievalQuery : METAView_Resource_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Resource_RetrievalQuery class.
        /// </summary>
        public METAView_Resource_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Resource_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Resource_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Resource_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Resource_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Resource_RetrievalQuery

    #region METAView_ResourceType_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_ResourceType_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ResourceType_ListingFilters : METAView_ResourceType_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ResourceType_ListingFilters class.
        /// </summary>
        public METAView_ResourceType_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ResourceType_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ResourceType_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ResourceType_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ResourceType_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ResourceType_ListingFilters

    #region METAView_ResourceType_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_ResourceType_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_ResourceType_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ResourceType_ListingQuery : METAView_ResourceType_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ResourceType_ListingQuery class.
        /// </summary>
        public METAView_ResourceType_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ResourceType_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ResourceType_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ResourceType_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ResourceType_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ResourceType_ListingQuery

    #region METAView_ResourceType_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_ResourceType_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ResourceType_RetrievalFilters : METAView_ResourceType_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ResourceType_RetrievalFilters class.
        /// </summary>
        public METAView_ResourceType_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ResourceType_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ResourceType_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ResourceType_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ResourceType_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ResourceType_RetrievalFilters

    #region METAView_ResourceType_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_ResourceType_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_ResourceType_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_ResourceType_RetrievalQuery : METAView_ResourceType_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_ResourceType_RetrievalQuery class.
        /// </summary>
        public METAView_ResourceType_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_ResourceType_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_ResourceType_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_ResourceType_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_ResourceType_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_ResourceType_RetrievalQuery

    #region METAView_Responsibility_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Responsibility_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Responsibility_ListingFilters : METAView_Responsibility_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Responsibility_ListingFilters class.
        /// </summary>
        public METAView_Responsibility_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Responsibility_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Responsibility_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Responsibility_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Responsibility_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Responsibility_ListingFilters

    #region METAView_Responsibility_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Responsibility_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Responsibility_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Responsibility_ListingQuery : METAView_Responsibility_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Responsibility_ListingQuery class.
        /// </summary>
        public METAView_Responsibility_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Responsibility_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Responsibility_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Responsibility_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Responsibility_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Responsibility_ListingQuery

    #region METAView_Responsibility_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Responsibility_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Responsibility_RetrievalFilters : METAView_Responsibility_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Responsibility_RetrievalFilters class.
        /// </summary>
        public METAView_Responsibility_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Responsibility_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Responsibility_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Responsibility_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Responsibility_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Responsibility_RetrievalFilters

    #region METAView_Responsibility_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Responsibility_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Responsibility_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Responsibility_RetrievalQuery : METAView_Responsibility_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Responsibility_RetrievalQuery class.
        /// </summary>
        public METAView_Responsibility_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Responsibility_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Responsibility_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Responsibility_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Responsibility_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Responsibility_RetrievalQuery

    #region METAView_Risk_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Risk_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Risk_ListingFilters : METAView_Risk_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Risk_ListingFilters class.
        /// </summary>
        public METAView_Risk_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Risk_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Risk_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Risk_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Risk_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Risk_ListingFilters

    #region METAView_Risk_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Risk_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Risk_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Risk_ListingQuery : METAView_Risk_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Risk_ListingQuery class.
        /// </summary>
        public METAView_Risk_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Risk_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Risk_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Risk_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Risk_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Risk_ListingQuery

    #region METAView_Risk_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Risk_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Risk_RetrievalFilters : METAView_Risk_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Risk_RetrievalFilters class.
        /// </summary>
        public METAView_Risk_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Risk_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Risk_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Risk_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Risk_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Risk_RetrievalFilters

    #region METAView_Risk_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Risk_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Risk_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Risk_RetrievalQuery : METAView_Risk_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Risk_RetrievalQuery class.
        /// </summary>
        public METAView_Risk_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Risk_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Risk_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Risk_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Risk_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Risk_RetrievalQuery

    #region METAView_Role_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Role_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Role_ListingFilters : METAView_Role_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Role_ListingFilters class.
        /// </summary>
        public METAView_Role_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Role_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Role_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Role_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Role_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Role_ListingFilters

    #region METAView_Role_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Role_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Role_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Role_ListingQuery : METAView_Role_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Role_ListingQuery class.
        /// </summary>
        public METAView_Role_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Role_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Role_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Role_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Role_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Role_ListingQuery

    #region METAView_Role_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Role_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Role_RetrievalFilters : METAView_Role_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Role_RetrievalFilters class.
        /// </summary>
        public METAView_Role_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Role_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Role_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Role_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Role_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Role_RetrievalFilters

    #region METAView_Role_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Role_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Role_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Role_RetrievalQuery : METAView_Role_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Role_RetrievalQuery class.
        /// </summary>
        public METAView_Role_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Role_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Role_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Role_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Role_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Role_RetrievalQuery

    #region METAView_Scenario_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Scenario_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Scenario_ListingFilters : METAView_Scenario_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Scenario_ListingFilters class.
        /// </summary>
        public METAView_Scenario_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Scenario_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Scenario_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Scenario_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Scenario_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Scenario_ListingFilters

    #region METAView_Scenario_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Scenario_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Scenario_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Scenario_ListingQuery : METAView_Scenario_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Scenario_ListingQuery class.
        /// </summary>
        public METAView_Scenario_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Scenario_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Scenario_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Scenario_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Scenario_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Scenario_ListingQuery

    #region METAView_Scenario_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Scenario_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Scenario_RetrievalFilters : METAView_Scenario_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Scenario_RetrievalFilters class.
        /// </summary>
        public METAView_Scenario_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Scenario_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Scenario_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Scenario_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Scenario_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Scenario_RetrievalFilters

    #region METAView_Scenario_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Scenario_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Scenario_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Scenario_RetrievalQuery : METAView_Scenario_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Scenario_RetrievalQuery class.
        /// </summary>
        public METAView_Scenario_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Scenario_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Scenario_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Scenario_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Scenario_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Scenario_RetrievalQuery

    #region METAView_SecurityClassification_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_SecurityClassification_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_SecurityClassification_ListingFilters : METAView_SecurityClassification_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_SecurityClassification_ListingFilters class.
        /// </summary>
        public METAView_SecurityClassification_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_SecurityClassification_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_SecurityClassification_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_SecurityClassification_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_SecurityClassification_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_SecurityClassification_ListingFilters

    #region METAView_SecurityClassification_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_SecurityClassification_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_SecurityClassification_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_SecurityClassification_ListingQuery : METAView_SecurityClassification_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_SecurityClassification_ListingQuery class.
        /// </summary>
        public METAView_SecurityClassification_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_SecurityClassification_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_SecurityClassification_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_SecurityClassification_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_SecurityClassification_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_SecurityClassification_ListingQuery

    #region METAView_SecurityClassification_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_SecurityClassification_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_SecurityClassification_RetrievalFilters : METAView_SecurityClassification_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_SecurityClassification_RetrievalFilters class.
        /// </summary>
        public METAView_SecurityClassification_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_SecurityClassification_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_SecurityClassification_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_SecurityClassification_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_SecurityClassification_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_SecurityClassification_RetrievalFilters

    #region METAView_SecurityClassification_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_SecurityClassification_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_SecurityClassification_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_SecurityClassification_RetrievalQuery : METAView_SecurityClassification_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_SecurityClassification_RetrievalQuery class.
        /// </summary>
        public METAView_SecurityClassification_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_SecurityClassification_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_SecurityClassification_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_SecurityClassification_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_SecurityClassification_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_SecurityClassification_RetrievalQuery

    #region METAView_SelectorAttribute_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_SelectorAttribute_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_SelectorAttribute_ListingFilters : METAView_SelectorAttribute_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_SelectorAttribute_ListingFilters class.
        /// </summary>
        public METAView_SelectorAttribute_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_SelectorAttribute_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_SelectorAttribute_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_SelectorAttribute_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_SelectorAttribute_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_SelectorAttribute_ListingFilters

    #region METAView_SelectorAttribute_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_SelectorAttribute_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_SelectorAttribute_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_SelectorAttribute_ListingQuery : METAView_SelectorAttribute_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_SelectorAttribute_ListingQuery class.
        /// </summary>
        public METAView_SelectorAttribute_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_SelectorAttribute_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_SelectorAttribute_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_SelectorAttribute_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_SelectorAttribute_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_SelectorAttribute_ListingQuery

    #region METAView_SelectorAttribute_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_SelectorAttribute_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_SelectorAttribute_RetrievalFilters : METAView_SelectorAttribute_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_SelectorAttribute_RetrievalFilters class.
        /// </summary>
        public METAView_SelectorAttribute_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_SelectorAttribute_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_SelectorAttribute_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_SelectorAttribute_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_SelectorAttribute_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_SelectorAttribute_RetrievalFilters

    #region METAView_SelectorAttribute_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_SelectorAttribute_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_SelectorAttribute_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_SelectorAttribute_RetrievalQuery : METAView_SelectorAttribute_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_SelectorAttribute_RetrievalQuery class.
        /// </summary>
        public METAView_SelectorAttribute_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_SelectorAttribute_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_SelectorAttribute_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_SelectorAttribute_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_SelectorAttribute_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_SelectorAttribute_RetrievalQuery

    #region METAView_Skill_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Skill_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Skill_ListingFilters : METAView_Skill_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Skill_ListingFilters class.
        /// </summary>
        public METAView_Skill_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Skill_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Skill_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Skill_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Skill_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Skill_ListingFilters

    #region METAView_Skill_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Skill_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Skill_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Skill_ListingQuery : METAView_Skill_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Skill_ListingQuery class.
        /// </summary>
        public METAView_Skill_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Skill_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Skill_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Skill_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Skill_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Skill_ListingQuery

    #region METAView_Skill_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Skill_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Skill_RetrievalFilters : METAView_Skill_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Skill_RetrievalFilters class.
        /// </summary>
        public METAView_Skill_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Skill_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Skill_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Skill_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Skill_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Skill_RetrievalFilters

    #region METAView_Skill_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Skill_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Skill_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Skill_RetrievalQuery : METAView_Skill_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Skill_RetrievalQuery class.
        /// </summary>
        public METAView_Skill_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Skill_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Skill_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Skill_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Skill_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Skill_RetrievalQuery

    #region METAView_Software_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Software_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Software_ListingFilters : METAView_Software_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Software_ListingFilters class.
        /// </summary>
        public METAView_Software_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Software_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Software_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Software_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Software_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Software_ListingFilters

    #region METAView_Software_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Software_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Software_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Software_ListingQuery : METAView_Software_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Software_ListingQuery class.
        /// </summary>
        public METAView_Software_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Software_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Software_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Software_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Software_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Software_ListingQuery

    #region METAView_Software_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Software_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Software_RetrievalFilters : METAView_Software_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Software_RetrievalFilters class.
        /// </summary>
        public METAView_Software_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Software_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Software_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Software_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Software_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Software_RetrievalFilters

    #region METAView_Software_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Software_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Software_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Software_RetrievalQuery : METAView_Software_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Software_RetrievalQuery class.
        /// </summary>
        public METAView_Software_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Software_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Software_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Software_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Software_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Software_RetrievalQuery

    #region METAView_Stakeholder_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Stakeholder_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Stakeholder_ListingFilters : METAView_Stakeholder_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Stakeholder_ListingFilters class.
        /// </summary>
        public METAView_Stakeholder_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Stakeholder_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Stakeholder_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Stakeholder_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Stakeholder_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Stakeholder_ListingFilters

    #region METAView_Stakeholder_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Stakeholder_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Stakeholder_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Stakeholder_ListingQuery : METAView_Stakeholder_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Stakeholder_ListingQuery class.
        /// </summary>
        public METAView_Stakeholder_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Stakeholder_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Stakeholder_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Stakeholder_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Stakeholder_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Stakeholder_ListingQuery

    #region METAView_Stakeholder_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Stakeholder_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Stakeholder_RetrievalFilters : METAView_Stakeholder_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Stakeholder_RetrievalFilters class.
        /// </summary>
        public METAView_Stakeholder_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Stakeholder_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Stakeholder_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Stakeholder_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Stakeholder_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Stakeholder_RetrievalFilters

    #region METAView_Stakeholder_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Stakeholder_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Stakeholder_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Stakeholder_RetrievalQuery : METAView_Stakeholder_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Stakeholder_RetrievalQuery class.
        /// </summary>
        public METAView_Stakeholder_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Stakeholder_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Stakeholder_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Stakeholder_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Stakeholder_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Stakeholder_RetrievalQuery

    #region METAView_StatusIndicator_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_StatusIndicator_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_StatusIndicator_ListingFilters : METAView_StatusIndicator_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_StatusIndicator_ListingFilters class.
        /// </summary>
        public METAView_StatusIndicator_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_StatusIndicator_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_StatusIndicator_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_StatusIndicator_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_StatusIndicator_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_StatusIndicator_ListingFilters

    #region METAView_StatusIndicator_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_StatusIndicator_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_StatusIndicator_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_StatusIndicator_ListingQuery : METAView_StatusIndicator_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_StatusIndicator_ListingQuery class.
        /// </summary>
        public METAView_StatusIndicator_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_StatusIndicator_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_StatusIndicator_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_StatusIndicator_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_StatusIndicator_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_StatusIndicator_ListingQuery

    #region METAView_StatusIndicator_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_StatusIndicator_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_StatusIndicator_RetrievalFilters : METAView_StatusIndicator_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_StatusIndicator_RetrievalFilters class.
        /// </summary>
        public METAView_StatusIndicator_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_StatusIndicator_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_StatusIndicator_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_StatusIndicator_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_StatusIndicator_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_StatusIndicator_RetrievalFilters

    #region METAView_StatusIndicator_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_StatusIndicator_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_StatusIndicator_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_StatusIndicator_RetrievalQuery : METAView_StatusIndicator_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_StatusIndicator_RetrievalQuery class.
        /// </summary>
        public METAView_StatusIndicator_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_StatusIndicator_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_StatusIndicator_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_StatusIndicator_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_StatusIndicator_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_StatusIndicator_RetrievalQuery

    #region METAView_StorageComponent_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_StorageComponent_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_StorageComponent_ListingFilters : METAView_StorageComponent_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_StorageComponent_ListingFilters class.
        /// </summary>
        public METAView_StorageComponent_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_StorageComponent_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_StorageComponent_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_StorageComponent_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_StorageComponent_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_StorageComponent_ListingFilters

    #region METAView_StorageComponent_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_StorageComponent_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_StorageComponent_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_StorageComponent_ListingQuery : METAView_StorageComponent_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_StorageComponent_ListingQuery class.
        /// </summary>
        public METAView_StorageComponent_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_StorageComponent_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_StorageComponent_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_StorageComponent_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_StorageComponent_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_StorageComponent_ListingQuery

    #region METAView_StorageComponent_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_StorageComponent_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_StorageComponent_RetrievalFilters : METAView_StorageComponent_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_StorageComponent_RetrievalFilters class.
        /// </summary>
        public METAView_StorageComponent_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_StorageComponent_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_StorageComponent_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_StorageComponent_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_StorageComponent_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_StorageComponent_RetrievalFilters

    #region METAView_StorageComponent_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_StorageComponent_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_StorageComponent_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_StorageComponent_RetrievalQuery : METAView_StorageComponent_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_StorageComponent_RetrievalQuery class.
        /// </summary>
        public METAView_StorageComponent_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_StorageComponent_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_StorageComponent_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_StorageComponent_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_StorageComponent_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_StorageComponent_RetrievalQuery

    #region METAView_StrategicStatement_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_StrategicStatement_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_StrategicStatement_ListingFilters : METAView_StrategicStatement_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_StrategicStatement_ListingFilters class.
        /// </summary>
        public METAView_StrategicStatement_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_StrategicStatement_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_StrategicStatement_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_StrategicStatement_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_StrategicStatement_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_StrategicStatement_ListingFilters

    #region METAView_StrategicStatement_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_StrategicStatement_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_StrategicStatement_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_StrategicStatement_ListingQuery : METAView_StrategicStatement_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_StrategicStatement_ListingQuery class.
        /// </summary>
        public METAView_StrategicStatement_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_StrategicStatement_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_StrategicStatement_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_StrategicStatement_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_StrategicStatement_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_StrategicStatement_ListingQuery

    #region METAView_StrategicStatement_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_StrategicStatement_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_StrategicStatement_RetrievalFilters : METAView_StrategicStatement_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_StrategicStatement_RetrievalFilters class.
        /// </summary>
        public METAView_StrategicStatement_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_StrategicStatement_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_StrategicStatement_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_StrategicStatement_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_StrategicStatement_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_StrategicStatement_RetrievalFilters

    #region METAView_StrategicStatement_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_StrategicStatement_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_StrategicStatement_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_StrategicStatement_RetrievalQuery : METAView_StrategicStatement_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_StrategicStatement_RetrievalQuery class.
        /// </summary>
        public METAView_StrategicStatement_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_StrategicStatement_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_StrategicStatement_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_StrategicStatement_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_StrategicStatement_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_StrategicStatement_RetrievalQuery

    #region METAView_StrategicTheme_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_StrategicTheme_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_StrategicTheme_ListingFilters : METAView_StrategicTheme_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_StrategicTheme_ListingFilters class.
        /// </summary>
        public METAView_StrategicTheme_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_StrategicTheme_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_StrategicTheme_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_StrategicTheme_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_StrategicTheme_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_StrategicTheme_ListingFilters

    #region METAView_StrategicTheme_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_StrategicTheme_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_StrategicTheme_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_StrategicTheme_ListingQuery : METAView_StrategicTheme_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_StrategicTheme_ListingQuery class.
        /// </summary>
        public METAView_StrategicTheme_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_StrategicTheme_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_StrategicTheme_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_StrategicTheme_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_StrategicTheme_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_StrategicTheme_ListingQuery

    #region METAView_StrategicTheme_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_StrategicTheme_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_StrategicTheme_RetrievalFilters : METAView_StrategicTheme_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_StrategicTheme_RetrievalFilters class.
        /// </summary>
        public METAView_StrategicTheme_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_StrategicTheme_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_StrategicTheme_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_StrategicTheme_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_StrategicTheme_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_StrategicTheme_RetrievalFilters

    #region METAView_StrategicTheme_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_StrategicTheme_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_StrategicTheme_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_StrategicTheme_RetrievalQuery : METAView_StrategicTheme_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_StrategicTheme_RetrievalQuery class.
        /// </summary>
        public METAView_StrategicTheme_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_StrategicTheme_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_StrategicTheme_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_StrategicTheme_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_StrategicTheme_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_StrategicTheme_RetrievalQuery

    #region METAView_SubsetIndicator_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_SubsetIndicator_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_SubsetIndicator_ListingFilters : METAView_SubsetIndicator_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_SubsetIndicator_ListingFilters class.
        /// </summary>
        public METAView_SubsetIndicator_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_SubsetIndicator_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_SubsetIndicator_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_SubsetIndicator_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_SubsetIndicator_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_SubsetIndicator_ListingFilters

    #region METAView_SubsetIndicator_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_SubsetIndicator_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_SubsetIndicator_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_SubsetIndicator_ListingQuery : METAView_SubsetIndicator_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_SubsetIndicator_ListingQuery class.
        /// </summary>
        public METAView_SubsetIndicator_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_SubsetIndicator_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_SubsetIndicator_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_SubsetIndicator_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_SubsetIndicator_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_SubsetIndicator_ListingQuery

    #region METAView_SubsetIndicator_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_SubsetIndicator_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_SubsetIndicator_RetrievalFilters : METAView_SubsetIndicator_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_SubsetIndicator_RetrievalFilters class.
        /// </summary>
        public METAView_SubsetIndicator_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_SubsetIndicator_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_SubsetIndicator_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_SubsetIndicator_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_SubsetIndicator_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_SubsetIndicator_RetrievalFilters

    #region METAView_SubsetIndicator_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_SubsetIndicator_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_SubsetIndicator_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_SubsetIndicator_RetrievalQuery : METAView_SubsetIndicator_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_SubsetIndicator_RetrievalQuery class.
        /// </summary>
        public METAView_SubsetIndicator_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_SubsetIndicator_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_SubsetIndicator_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_SubsetIndicator_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_SubsetIndicator_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_SubsetIndicator_RetrievalQuery

    #region METAView_Sys_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Sys_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Sys_ListingFilters : METAView_Sys_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Sys_ListingFilters class.
        /// </summary>
        public METAView_Sys_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Sys_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Sys_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Sys_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Sys_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Sys_ListingFilters

    #region METAView_Sys_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Sys_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Sys_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Sys_ListingQuery : METAView_Sys_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Sys_ListingQuery class.
        /// </summary>
        public METAView_Sys_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Sys_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Sys_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Sys_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Sys_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Sys_ListingQuery

    #region METAView_Sys_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Sys_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Sys_RetrievalFilters : METAView_Sys_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Sys_RetrievalFilters class.
        /// </summary>
        public METAView_Sys_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Sys_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Sys_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Sys_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Sys_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Sys_RetrievalFilters

    #region METAView_Sys_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Sys_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Sys_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Sys_RetrievalQuery : METAView_Sys_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Sys_RetrievalQuery class.
        /// </summary>
        public METAView_Sys_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Sys_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Sys_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Sys_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Sys_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Sys_RetrievalQuery

    #region METAView_SystemComponent_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_SystemComponent_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_SystemComponent_ListingFilters : METAView_SystemComponent_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_SystemComponent_ListingFilters class.
        /// </summary>
        public METAView_SystemComponent_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_SystemComponent_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_SystemComponent_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_SystemComponent_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_SystemComponent_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_SystemComponent_ListingFilters

    #region METAView_SystemComponent_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_SystemComponent_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_SystemComponent_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_SystemComponent_ListingQuery : METAView_SystemComponent_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_SystemComponent_ListingQuery class.
        /// </summary>
        public METAView_SystemComponent_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_SystemComponent_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_SystemComponent_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_SystemComponent_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_SystemComponent_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_SystemComponent_ListingQuery

    #region METAView_SystemComponent_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_SystemComponent_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_SystemComponent_RetrievalFilters : METAView_SystemComponent_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_SystemComponent_RetrievalFilters class.
        /// </summary>
        public METAView_SystemComponent_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_SystemComponent_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_SystemComponent_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_SystemComponent_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_SystemComponent_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_SystemComponent_RetrievalFilters

    #region METAView_SystemComponent_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_SystemComponent_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_SystemComponent_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_SystemComponent_RetrievalQuery : METAView_SystemComponent_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_SystemComponent_RetrievalQuery class.
        /// </summary>
        public METAView_SystemComponent_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_SystemComponent_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_SystemComponent_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_SystemComponent_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_SystemComponent_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_SystemComponent_RetrievalQuery

    #region METAView_Task_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Task_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Task_ListingFilters : METAView_Task_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Task_ListingFilters class.
        /// </summary>
        public METAView_Task_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Task_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Task_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Task_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Task_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Task_ListingFilters

    #region METAView_Task_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Task_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Task_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Task_ListingQuery : METAView_Task_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Task_ListingQuery class.
        /// </summary>
        public METAView_Task_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Task_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Task_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Task_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Task_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Task_ListingQuery

    #region METAView_Task_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_Task_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Task_RetrievalFilters : METAView_Task_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Task_RetrievalFilters class.
        /// </summary>
        public METAView_Task_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Task_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Task_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Task_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Task_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Task_RetrievalFilters

    #region METAView_Task_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_Task_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_Task_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Task_RetrievalQuery : METAView_Task_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_Task_RetrievalQuery class.
        /// </summary>
        public METAView_Task_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_Task_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_Task_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_Task_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_Task_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_Task_RetrievalQuery

    #region METAView_TimeDifference_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_TimeDifference_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_TimeDifference_ListingFilters : METAView_TimeDifference_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_TimeDifference_ListingFilters class.
        /// </summary>
        public METAView_TimeDifference_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeDifference_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_TimeDifference_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeDifference_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_TimeDifference_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_TimeDifference_ListingFilters

    #region METAView_TimeDifference_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_TimeDifference_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_TimeDifference_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_TimeDifference_ListingQuery : METAView_TimeDifference_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_TimeDifference_ListingQuery class.
        /// </summary>
        public METAView_TimeDifference_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeDifference_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_TimeDifference_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeDifference_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_TimeDifference_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_TimeDifference_ListingQuery

    #region METAView_TimeDifference_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_TimeDifference_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_TimeDifference_RetrievalFilters : METAView_TimeDifference_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_TimeDifference_RetrievalFilters class.
        /// </summary>
        public METAView_TimeDifference_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeDifference_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_TimeDifference_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeDifference_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_TimeDifference_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_TimeDifference_RetrievalFilters

    #region METAView_TimeDifference_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_TimeDifference_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_TimeDifference_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_TimeDifference_RetrievalQuery : METAView_TimeDifference_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_TimeDifference_RetrievalQuery class.
        /// </summary>
        public METAView_TimeDifference_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeDifference_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_TimeDifference_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeDifference_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_TimeDifference_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_TimeDifference_RetrievalQuery

    #region METAView_TimeIndicator_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_TimeIndicator_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_TimeIndicator_ListingFilters : METAView_TimeIndicator_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_TimeIndicator_ListingFilters class.
        /// </summary>
        public METAView_TimeIndicator_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeIndicator_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_TimeIndicator_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeIndicator_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_TimeIndicator_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_TimeIndicator_ListingFilters

    #region METAView_TimeIndicator_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_TimeIndicator_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_TimeIndicator_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_TimeIndicator_ListingQuery : METAView_TimeIndicator_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_TimeIndicator_ListingQuery class.
        /// </summary>
        public METAView_TimeIndicator_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeIndicator_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_TimeIndicator_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeIndicator_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_TimeIndicator_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_TimeIndicator_ListingQuery

    #region METAView_TimeIndicator_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_TimeIndicator_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_TimeIndicator_RetrievalFilters : METAView_TimeIndicator_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_TimeIndicator_RetrievalFilters class.
        /// </summary>
        public METAView_TimeIndicator_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeIndicator_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_TimeIndicator_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeIndicator_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_TimeIndicator_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_TimeIndicator_RetrievalFilters

    #region METAView_TimeIndicator_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_TimeIndicator_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_TimeIndicator_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_TimeIndicator_RetrievalQuery : METAView_TimeIndicator_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_TimeIndicator_RetrievalQuery class.
        /// </summary>
        public METAView_TimeIndicator_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeIndicator_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_TimeIndicator_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeIndicator_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_TimeIndicator_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_TimeIndicator_RetrievalQuery

    #region METAView_TimeReference_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_TimeReference_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_TimeReference_ListingFilters : METAView_TimeReference_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_TimeReference_ListingFilters class.
        /// </summary>
        public METAView_TimeReference_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeReference_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_TimeReference_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeReference_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_TimeReference_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_TimeReference_ListingFilters

    #region METAView_TimeReference_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_TimeReference_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_TimeReference_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_TimeReference_ListingQuery : METAView_TimeReference_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_TimeReference_ListingQuery class.
        /// </summary>
        public METAView_TimeReference_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeReference_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_TimeReference_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeReference_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_TimeReference_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_TimeReference_ListingQuery

    #region METAView_TimeReference_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_TimeReference_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_TimeReference_RetrievalFilters : METAView_TimeReference_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_TimeReference_RetrievalFilters class.
        /// </summary>
        public METAView_TimeReference_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeReference_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_TimeReference_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeReference_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_TimeReference_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_TimeReference_RetrievalFilters

    #region METAView_TimeReference_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_TimeReference_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_TimeReference_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_TimeReference_RetrievalQuery : METAView_TimeReference_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_TimeReference_RetrievalQuery class.
        /// </summary>
        public METAView_TimeReference_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeReference_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_TimeReference_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeReference_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_TimeReference_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_TimeReference_RetrievalQuery

    #region METAView_TimeScheme_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_TimeScheme_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_TimeScheme_ListingFilters : METAView_TimeScheme_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_TimeScheme_ListingFilters class.
        /// </summary>
        public METAView_TimeScheme_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeScheme_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_TimeScheme_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeScheme_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_TimeScheme_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_TimeScheme_ListingFilters

    #region METAView_TimeScheme_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_TimeScheme_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_TimeScheme_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_TimeScheme_ListingQuery : METAView_TimeScheme_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_TimeScheme_ListingQuery class.
        /// </summary>
        public METAView_TimeScheme_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeScheme_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_TimeScheme_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeScheme_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_TimeScheme_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_TimeScheme_ListingQuery

    #region METAView_TimeScheme_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_TimeScheme_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_TimeScheme_RetrievalFilters : METAView_TimeScheme_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_TimeScheme_RetrievalFilters class.
        /// </summary>
        public METAView_TimeScheme_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeScheme_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_TimeScheme_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeScheme_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_TimeScheme_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_TimeScheme_RetrievalFilters

    #region METAView_TimeScheme_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_TimeScheme_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_TimeScheme_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_TimeScheme_RetrievalQuery : METAView_TimeScheme_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_TimeScheme_RetrievalQuery class.
        /// </summary>
        public METAView_TimeScheme_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeScheme_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_TimeScheme_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeScheme_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_TimeScheme_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_TimeScheme_RetrievalQuery

    #region METAView_TimeUnit_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_TimeUnit_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_TimeUnit_ListingFilters : METAView_TimeUnit_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_TimeUnit_ListingFilters class.
        /// </summary>
        public METAView_TimeUnit_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeUnit_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_TimeUnit_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeUnit_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_TimeUnit_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_TimeUnit_ListingFilters

    #region METAView_TimeUnit_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_TimeUnit_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_TimeUnit_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_TimeUnit_ListingQuery : METAView_TimeUnit_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_TimeUnit_ListingQuery class.
        /// </summary>
        public METAView_TimeUnit_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeUnit_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_TimeUnit_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeUnit_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_TimeUnit_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_TimeUnit_ListingQuery

    #region METAView_TimeUnit_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_TimeUnit_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_TimeUnit_RetrievalFilters : METAView_TimeUnit_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_TimeUnit_RetrievalFilters class.
        /// </summary>
        public METAView_TimeUnit_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeUnit_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_TimeUnit_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeUnit_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_TimeUnit_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_TimeUnit_RetrievalFilters

    #region METAView_TimeUnit_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_TimeUnit_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_TimeUnit_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_TimeUnit_RetrievalQuery : METAView_TimeUnit_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_TimeUnit_RetrievalQuery class.
        /// </summary>
        public METAView_TimeUnit_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeUnit_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_TimeUnit_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_TimeUnit_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_TimeUnit_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_TimeUnit_RetrievalQuery

    #region METAView_WorkPackage_ListingFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_WorkPackage_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_WorkPackage_ListingFilters : METAView_WorkPackage_ListingFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_WorkPackage_ListingFilters class.
        /// </summary>
        public METAView_WorkPackage_ListingFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_WorkPackage_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_WorkPackage_ListingFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_WorkPackage_ListingFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_WorkPackage_ListingFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_WorkPackage_ListingFilters

    #region METAView_WorkPackage_ListingQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_WorkPackage_ListingParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_WorkPackage_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_WorkPackage_ListingQuery : METAView_WorkPackage_ListingParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_WorkPackage_ListingQuery class.
        /// </summary>
        public METAView_WorkPackage_ListingQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_WorkPackage_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_WorkPackage_ListingQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_WorkPackage_ListingQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_WorkPackage_ListingQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_WorkPackage_ListingQuery

    #region METAView_WorkPackage_RetrievalFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="METAView_WorkPackage_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_WorkPackage_RetrievalFilters : METAView_WorkPackage_RetrievalFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_WorkPackage_RetrievalFilters class.
        /// </summary>
        public METAView_WorkPackage_RetrievalFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_WorkPackage_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_WorkPackage_RetrievalFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_WorkPackage_RetrievalFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_WorkPackage_RetrievalFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_WorkPackage_RetrievalFilters

    #region METAView_WorkPackage_RetrievalQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="METAView_WorkPackage_RetrievalParameterBuilder"/> class
    /// that is used exclusively with a <see cref="METAView_WorkPackage_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_WorkPackage_RetrievalQuery : METAView_WorkPackage_RetrievalParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the METAView_WorkPackage_RetrievalQuery class.
        /// </summary>
        public METAView_WorkPackage_RetrievalQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the METAView_WorkPackage_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public METAView_WorkPackage_RetrievalQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the METAView_WorkPackage_RetrievalQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public METAView_WorkPackage_RetrievalQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion METAView_WorkPackage_RetrievalQuery

    #region vw_FieldValueFilters

    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
    /// that is used exclusively with a <see cref="vw_FieldValue"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class vw_FieldValueFilters : vw_FieldValueFilterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the vw_FieldValueFilters class.
        /// </summary>
        public vw_FieldValueFilters() : base() { }

        /// <summary>
        /// Initializes a new instance of the vw_FieldValueFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public vw_FieldValueFilters(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the vw_FieldValueFilters class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public vw_FieldValueFilters(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion vw_FieldValueFilters

    #region vw_FieldValueQuery

    /// <summary>
    /// A strongly-typed instance of the <see cref="vw_FieldValueParameterBuilder"/> class
    /// that is used exclusively with a <see cref="vw_FieldValue"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class vw_FieldValueQuery : vw_FieldValueParameterBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the vw_FieldValueQuery class.
        /// </summary>
        public vw_FieldValueQuery() : base() { }

        /// <summary>
        /// Initializes a new instance of the vw_FieldValueQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        public vw_FieldValueQuery(bool ignoreCase) : base(ignoreCase) { }

        /// <summary>
        /// Initializes a new instance of the vw_FieldValueQuery class.
        /// </summary>
        /// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
        /// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
        public vw_FieldValueQuery(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

        #endregion Constructors
    }

    #endregion vw_FieldValueQuery
    #endregion

}