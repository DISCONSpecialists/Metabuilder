
#region Using directives

using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Configuration.Provider;

using MetaBuilder.BusinessLogic;

#endregion

namespace MetaBuilder.DataAccessLayer.Bases
{	
	///<summary>
	/// The base class to implements to create a .NetTiers provider.
	///</summary>
	public abstract class NetTiersProvider : NetTiersProviderBase
	{
		
		///<summary>
		/// Current VCStatusProviderBase instance.
		///</summary>
		public virtual VCStatusProviderBase VCStatusProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current DomainDefinitionPossibleValueProviderBase instance.
		///</summary>
		public virtual DomainDefinitionPossibleValueProviderBase DomainDefinitionPossibleValueProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current WorkspaceProviderBase instance.
		///</summary>
		public virtual WorkspaceProviderBase WorkspaceProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current ClassTypeProviderBase instance.
		///</summary>
		public virtual ClassTypeProviderBase ClassTypeProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current WorkspaceTypeProviderBase instance.
		///</summary>
		public virtual WorkspaceTypeProviderBase WorkspaceTypeProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current UserProviderBase instance.
		///</summary>
		public virtual UserProviderBase UserProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current GraphFileProviderBase instance.
		///</summary>
		public virtual GraphFileProviderBase GraphFileProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current ClassProviderBase instance.
		///</summary>
		public virtual ClassProviderBase ClassProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current MetaObjectProviderBase instance.
		///</summary>
		public virtual MetaObjectProviderBase MetaObjectProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current UserPermissionProviderBase instance.
		///</summary>
		public virtual UserPermissionProviderBase UserPermissionProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current ConfigProviderBase instance.
		///</summary>
		public virtual ConfigProviderBase ConfigProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current AssociationTypeProviderBase instance.
		///</summary>
		public virtual AssociationTypeProviderBase AssociationTypeProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current ClassAssociationProviderBase instance.
		///</summary>
		public virtual ClassAssociationProviderBase ClassAssociationProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current ObjectAssociationProviderBase instance.
		///</summary>
		public virtual ObjectAssociationProviderBase ObjectAssociationProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current FileTypeProviderBase instance.
		///</summary>
		public virtual FileTypeProviderBase FileTypeProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current ObjectFieldValueProviderBase instance.
		///</summary>
		public virtual ObjectFieldValueProviderBase ObjectFieldValueProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current GraphFileAssociationProviderBase instance.
		///</summary>
		public virtual GraphFileAssociationProviderBase GraphFileAssociationProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current AllowedArtifactProviderBase instance.
		///</summary>
		public virtual AllowedArtifactProviderBase AllowedArtifactProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current DomainDefinitionProviderBase instance.
		///</summary>
		public virtual DomainDefinitionProviderBase DomainDefinitionProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current ArtifactProviderBase instance.
		///</summary>
		public virtual ArtifactProviderBase ArtifactProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current PermissionProviderBase instance.
		///</summary>
		public virtual PermissionProviderBase PermissionProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current GraphFileObjectProviderBase instance.
		///</summary>
		public virtual GraphFileObjectProviderBase GraphFileObjectProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current FieldProviderBase instance.
		///</summary>
		public virtual FieldProviderBase FieldProvider{get {throw new NotImplementedException();}}
		
		
		///<summary>
		/// Current METAView_Activity_ListingProviderBase instance.
		///</summary>
		public virtual METAView_Activity_ListingProviderBase METAView_Activity_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Activity_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_Activity_RetrievalProviderBase METAView_Activity_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Attribute_ListingProviderBase instance.
		///</summary>
		public virtual METAView_Attribute_ListingProviderBase METAView_Attribute_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Attribute_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_Attribute_RetrievalProviderBase METAView_Attribute_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_CAD_ListingProviderBase instance.
		///</summary>
		public virtual METAView_CAD_ListingProviderBase METAView_CAD_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_CAD_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_CAD_RetrievalProviderBase METAView_CAD_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_CADReal_ListingProviderBase instance.
		///</summary>
		public virtual METAView_CADReal_ListingProviderBase METAView_CADReal_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_CADReal_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_CADReal_RetrievalProviderBase METAView_CADReal_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_CategoryFactor_ListingProviderBase instance.
		///</summary>
		public virtual METAView_CategoryFactor_ListingProviderBase METAView_CategoryFactor_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_CategoryFactor_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_CategoryFactor_RetrievalProviderBase METAView_CategoryFactor_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Competency_ListingProviderBase instance.
		///</summary>
		public virtual METAView_Competency_ListingProviderBase METAView_Competency_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Competency_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_Competency_RetrievalProviderBase METAView_Competency_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Condition_ListingProviderBase instance.
		///</summary>
		public virtual METAView_Condition_ListingProviderBase METAView_Condition_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Condition_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_Condition_RetrievalProviderBase METAView_Condition_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Conditional_ListingProviderBase instance.
		///</summary>
		public virtual METAView_Conditional_ListingProviderBase METAView_Conditional_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Conditional_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_Conditional_RetrievalProviderBase METAView_Conditional_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_ConditionalDescription_ListingProviderBase instance.
		///</summary>
		public virtual METAView_ConditionalDescription_ListingProviderBase METAView_ConditionalDescription_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_ConditionalDescription_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_ConditionalDescription_RetrievalProviderBase METAView_ConditionalDescription_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_ConnectionSpeed_ListingProviderBase instance.
		///</summary>
		public virtual METAView_ConnectionSpeed_ListingProviderBase METAView_ConnectionSpeed_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_ConnectionSpeed_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_ConnectionSpeed_RetrievalProviderBase METAView_ConnectionSpeed_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_ConnectionType_ListingProviderBase instance.
		///</summary>
		public virtual METAView_ConnectionType_ListingProviderBase METAView_ConnectionType_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_ConnectionType_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_ConnectionType_RetrievalProviderBase METAView_ConnectionType_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_CSF_ListingProviderBase instance.
		///</summary>
		public virtual METAView_CSF_ListingProviderBase METAView_CSF_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_CSF_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_CSF_RetrievalProviderBase METAView_CSF_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_DataColumn_ListingProviderBase instance.
		///</summary>
		public virtual METAView_DataColumn_ListingProviderBase METAView_DataColumn_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_DataColumn_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_DataColumn_RetrievalProviderBase METAView_DataColumn_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_DataSchema_ListingProviderBase instance.
		///</summary>
		public virtual METAView_DataSchema_ListingProviderBase METAView_DataSchema_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_DataSchema_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_DataSchema_RetrievalProviderBase METAView_DataSchema_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_DataTable_ListingProviderBase instance.
		///</summary>
		public virtual METAView_DataTable_ListingProviderBase METAView_DataTable_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_DataTable_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_DataTable_RetrievalProviderBase METAView_DataTable_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_DataView_ListingProviderBase instance.
		///</summary>
		public virtual METAView_DataView_ListingProviderBase METAView_DataView_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_DataView_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_DataView_RetrievalProviderBase METAView_DataView_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_DatedResponsibility_ListingProviderBase instance.
		///</summary>
		public virtual METAView_DatedResponsibility_ListingProviderBase METAView_DatedResponsibility_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_DatedResponsibility_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_DatedResponsibility_RetrievalProviderBase METAView_DatedResponsibility_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Employee_ListingProviderBase instance.
		///</summary>
		public virtual METAView_Employee_ListingProviderBase METAView_Employee_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Employee_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_Employee_RetrievalProviderBase METAView_Employee_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Entity_ListingProviderBase instance.
		///</summary>
		public virtual METAView_Entity_ListingProviderBase METAView_Entity_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Entity_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_Entity_RetrievalProviderBase METAView_Entity_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Environment_ListingProviderBase instance.
		///</summary>
		public virtual METAView_Environment_ListingProviderBase METAView_Environment_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Environment_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_Environment_RetrievalProviderBase METAView_Environment_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_EnvironmentCategory_ListingProviderBase instance.
		///</summary>
		public virtual METAView_EnvironmentCategory_ListingProviderBase METAView_EnvironmentCategory_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_EnvironmentCategory_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_EnvironmentCategory_RetrievalProviderBase METAView_EnvironmentCategory_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Exception_ListingProviderBase instance.
		///</summary>
		public virtual METAView_Exception_ListingProviderBase METAView_Exception_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Exception_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_Exception_RetrievalProviderBase METAView_Exception_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_FlowDescription_ListingProviderBase instance.
		///</summary>
		public virtual METAView_FlowDescription_ListingProviderBase METAView_FlowDescription_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_FlowDescription_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_FlowDescription_RetrievalProviderBase METAView_FlowDescription_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Function_ListingProviderBase instance.
		///</summary>
		public virtual METAView_Function_ListingProviderBase METAView_Function_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Function_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_Function_RetrievalProviderBase METAView_Function_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_FunctionalDependency_ListingProviderBase instance.
		///</summary>
		public virtual METAView_FunctionalDependency_ListingProviderBase METAView_FunctionalDependency_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_FunctionalDependency_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_FunctionalDependency_RetrievalProviderBase METAView_FunctionalDependency_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_GovernanceMechanism_ListingProviderBase instance.
		///</summary>
		public virtual METAView_GovernanceMechanism_ListingProviderBase METAView_GovernanceMechanism_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_GovernanceMechanism_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_GovernanceMechanism_RetrievalProviderBase METAView_GovernanceMechanism_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Implication_ListingProviderBase instance.
		///</summary>
		public virtual METAView_Implication_ListingProviderBase METAView_Implication_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Implication_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_Implication_RetrievalProviderBase METAView_Implication_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Iteration_ListingProviderBase instance.
		///</summary>
		public virtual METAView_Iteration_ListingProviderBase METAView_Iteration_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Iteration_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_Iteration_RetrievalProviderBase METAView_Iteration_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Job_ListingProviderBase instance.
		///</summary>
		public virtual METAView_Job_ListingProviderBase METAView_Job_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Job_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_Job_RetrievalProviderBase METAView_Job_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_JobPosition_ListingProviderBase instance.
		///</summary>
		public virtual METAView_JobPosition_ListingProviderBase METAView_JobPosition_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_JobPosition_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_JobPosition_RetrievalProviderBase METAView_JobPosition_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Location_ListingProviderBase instance.
		///</summary>
		public virtual METAView_Location_ListingProviderBase METAView_Location_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Location_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_Location_RetrievalProviderBase METAView_Location_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_LocationAssociation_ListingProviderBase instance.
		///</summary>
		public virtual METAView_LocationAssociation_ListingProviderBase METAView_LocationAssociation_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_LocationAssociation_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_LocationAssociation_RetrievalProviderBase METAView_LocationAssociation_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Logic_ListingProviderBase instance.
		///</summary>
		public virtual METAView_Logic_ListingProviderBase METAView_Logic_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Logic_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_Logic_RetrievalProviderBase METAView_Logic_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_MeasurementItem_ListingProviderBase instance.
		///</summary>
		public virtual METAView_MeasurementItem_ListingProviderBase METAView_MeasurementItem_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_MeasurementItem_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_MeasurementItem_RetrievalProviderBase METAView_MeasurementItem_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Model_ListingProviderBase instance.
		///</summary>
		public virtual METAView_Model_ListingProviderBase METAView_Model_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Model_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_Model_RetrievalProviderBase METAView_Model_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_ModelReal_ListingProviderBase instance.
		///</summary>
		public virtual METAView_ModelReal_ListingProviderBase METAView_ModelReal_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_ModelReal_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_ModelReal_RetrievalProviderBase METAView_ModelReal_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_MutuallyExclusiveIndicator_ListingProviderBase instance.
		///</summary>
		public virtual METAView_MutuallyExclusiveIndicator_ListingProviderBase METAView_MutuallyExclusiveIndicator_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_MutuallyExclusiveIndicator_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_MutuallyExclusiveIndicator_RetrievalProviderBase METAView_MutuallyExclusiveIndicator_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_NetworkComponent_ListingProviderBase instance.
		///</summary>
		public virtual METAView_NetworkComponent_ListingProviderBase METAView_NetworkComponent_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_NetworkComponent_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_NetworkComponent_RetrievalProviderBase METAView_NetworkComponent_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Object_ListingProviderBase instance.
		///</summary>
		public virtual METAView_Object_ListingProviderBase METAView_Object_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Object_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_Object_RetrievalProviderBase METAView_Object_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_OrganizationalUnit_ListingProviderBase instance.
		///</summary>
		public virtual METAView_OrganizationalUnit_ListingProviderBase METAView_OrganizationalUnit_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_OrganizationalUnit_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_OrganizationalUnit_RetrievalProviderBase METAView_OrganizationalUnit_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Peripheral_ListingProviderBase instance.
		///</summary>
		public virtual METAView_Peripheral_ListingProviderBase METAView_Peripheral_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Peripheral_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_Peripheral_RetrievalProviderBase METAView_Peripheral_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_ProbOfRealization_ListingProviderBase instance.
		///</summary>
		public virtual METAView_ProbOfRealization_ListingProviderBase METAView_ProbOfRealization_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_ProbOfRealization_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_ProbOfRealization_RetrievalProviderBase METAView_ProbOfRealization_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Process_ListingProviderBase instance.
		///</summary>
		public virtual METAView_Process_ListingProviderBase METAView_Process_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Process_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_Process_RetrievalProviderBase METAView_Process_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Rationale_ListingProviderBase instance.
		///</summary>
		public virtual METAView_Rationale_ListingProviderBase METAView_Rationale_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Rationale_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_Rationale_RetrievalProviderBase METAView_Rationale_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Responsibility_ListingProviderBase instance.
		///</summary>
		public virtual METAView_Responsibility_ListingProviderBase METAView_Responsibility_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Responsibility_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_Responsibility_RetrievalProviderBase METAView_Responsibility_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Role_ListingProviderBase instance.
		///</summary>
		public virtual METAView_Role_ListingProviderBase METAView_Role_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Role_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_Role_RetrievalProviderBase METAView_Role_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Scenario_ListingProviderBase instance.
		///</summary>
		public virtual METAView_Scenario_ListingProviderBase METAView_Scenario_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Scenario_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_Scenario_RetrievalProviderBase METAView_Scenario_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_SelectorAttribute_ListingProviderBase instance.
		///</summary>
		public virtual METAView_SelectorAttribute_ListingProviderBase METAView_SelectorAttribute_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_SelectorAttribute_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_SelectorAttribute_RetrievalProviderBase METAView_SelectorAttribute_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Skill_ListingProviderBase instance.
		///</summary>
		public virtual METAView_Skill_ListingProviderBase METAView_Skill_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Skill_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_Skill_RetrievalProviderBase METAView_Skill_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Software_ListingProviderBase instance.
		///</summary>
		public virtual METAView_Software_ListingProviderBase METAView_Software_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_Software_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_Software_RetrievalProviderBase METAView_Software_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_StorageComponent_ListingProviderBase instance.
		///</summary>
		public virtual METAView_StorageComponent_ListingProviderBase METAView_StorageComponent_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_StorageComponent_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_StorageComponent_RetrievalProviderBase METAView_StorageComponent_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_StrategicTheme_ListingProviderBase instance.
		///</summary>
		public virtual METAView_StrategicTheme_ListingProviderBase METAView_StrategicTheme_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_StrategicTheme_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_StrategicTheme_RetrievalProviderBase METAView_StrategicTheme_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_SystemComponent_ListingProviderBase instance.
		///</summary>
		public virtual METAView_SystemComponent_ListingProviderBase METAView_SystemComponent_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_SystemComponent_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_SystemComponent_RetrievalProviderBase METAView_SystemComponent_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_TimeIndicator_ListingProviderBase instance.
		///</summary>
		public virtual METAView_TimeIndicator_ListingProviderBase METAView_TimeIndicator_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_TimeIndicator_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_TimeIndicator_RetrievalProviderBase METAView_TimeIndicator_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_TimeScheme_ListingProviderBase instance.
		///</summary>
		public virtual METAView_TimeScheme_ListingProviderBase METAView_TimeScheme_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_TimeScheme_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_TimeScheme_RetrievalProviderBase METAView_TimeScheme_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_TimeUnit_ListingProviderBase instance.
		///</summary>
		public virtual METAView_TimeUnit_ListingProviderBase METAView_TimeUnit_ListingProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current METAView_TimeUnit_RetrievalProviderBase instance.
		///</summary>
		public virtual METAView_TimeUnit_RetrievalProviderBase METAView_TimeUnit_RetrievalProvider{get {throw new NotImplementedException();}}
		
		///<summary>
		/// Current vw_FieldValueProviderBase instance.
		///</summary>
		public virtual vw_FieldValueProviderBase vw_FieldValueProvider{get {throw new NotImplementedException();}}
		
	}
}
