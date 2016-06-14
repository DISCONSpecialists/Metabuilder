USE METABUILDER

----New columns
ALTER TABLE ClassAssociation
ADD IsActive bit default 1 NULL
GO
ALTER TABLE Class
ADD IsActive bit default 1 NULL
GO
ALTER TABLE AllowedArtifact
ADD IsActive bit default 1 NULL
GO
ALTER TABLE DomainDefinition
ADD IsActive bit default 1 NULL
GO
ALTER TABLE DomainDefinitionPossibleValue
ADD IsActive bit default 1 NULL
GO
ALTER TABLE MetaObject
ADD DateCreated datetime NULL DEFAULT GETDATE()
GO
ALTER TABLE MetaObject
ADD LastModified datetime NULL DEFAULT GETDATE()
GO
ALTER TABLE dbo.DomainDefinitionPossibleValue ALTER COLUMN Description varchar(120)
ALTER TABLE dbo.Field ALTER COLUMN Description varchar(120)

GO
CREATE PROCEDURE db_AddArtifacts
(
@parentClass varchar(50),
@childClass varchar(50),
@Association varchar(50),
@ArtifactClass varchar(50),
@IsActive int
)
AS
BEGIN
DECLARE @CAID int
PRINT 'PC:' + @parentClass
PRINT 'CC:' + @parentClass
PRINT 'AC:' + @ArtifactClass
SET @CAID = (SELECT TOP (1) ClassAssociation.CAid FROM ClassAssociation INNER JOIN AssociationType ON ClassAssociation.AssociationTypeID = AssociationType.pkid WHERE (ClassAssociation.ParentClass = @parentClass) AND (ClassAssociation.ChildClass = @childClass) AND (AssociationType.Name = @Association))
IF (@CAID IS NULL) OR (@CAID = '') OR (@CAID = 0)
	BEGIN
		--SELECT 'No CAID ' + @parentClass + ':' + @childClass + '-' + @Association + '=' + @ArtifactClass  AS Result
		EXEC db_AddClassAssociations @parentClass=@parentClass,@childClass=@childClass,@Association=@Association,@Caption='',@IsDefault=0,@IsActive=@IsActive
		EXEC db_AddArtifacts @parentClass=@parentClass,@childClass=@childClass,@Association=@Association,@ArtifactClass=@ArtifactClass,@IsActive=@IsActive
	END
ELSE 
	IF EXISTS (SELECT * FROM AllowedArtifact WHERE (CAid = @CAID) AND (Class=@ArtifactClass))
		UPDATE AllowedArtifact SET IsActive = @IsActive WHERE (CAid = @CAID) AND (Class=@ArtifactClass)
	ELSE
		INSERT INTO AllowedArtifact (CAid, Class, IsActive) VALUES (@CAID, @ArtifactClass, @IsActive)
	--END
END
GO

GO
CREATE PROCEDURE db_AddClasses
(
@Class varchar(50),
@DescriptionCode varchar(255),
@Category varchar(50),
@IsActive int
)
AS
BEGIN
	IF EXISTS (SELECT TOP(1) * FROM Class WHERE ([Name] = @Class))
BEGIN

		IF (@DescriptionCode = '') OR (@DescriptionCode IS NULL)
			SET @DescriptionCode = (SELECT TOP(1) DescriptionCode FROM Class WHERE ([Name] = @Class))

		IF (@Category = '') OR (@Category IS NULL)
			SET @Category = (SELECT TOP(1) Category FROM Class WHERE ([Name] = @Class))

		UPDATE Class SET IsActive = @IsActive, DescriptionCode = @DescriptionCode, Category = @Category WHERE ([Name] = @Class)

END
	ELSE
BEGIN
IF (@DescriptionCode = '') OR (@DescriptionCode IS NULL)
	SET @DescriptionCode = 'Name'
IF (@Category = '') OR (@Category IS NULL)
	SET @Category = 'Default'

		INSERT INTO Class ([Name], DescriptionCode, Category, ClassType, IsActive) VALUES (@Class, @DescriptionCode, @Category, 'Primary', @IsActive)
END
	--END
END
GO

GO
CREATE PROCEDURE db_AddFields
(
@Class varchar(50),
@Name varchar(50),
@DataType varchar(50),
@Category varchar(50),
@Description varchar(120),
@IsUnique bit,
@SortOrder int,
@IsActive bit
)
AS
BEGIN
IF (@Category = '') OR (@Category IS NULL)
	SET @Category = 'Primary'
IF (@IsUnique = '') OR (@IsUnique IS NULL)
	SET @IsUnique = 0
	
	IF EXISTS (SELECT TOP (1) * FROM Field WHERE (Class = @Class) AND ([Name] = @Name))
		UPDATE Field
			SET [Class] = @Class,[Name] = @Name,[DataType] = @DataType,[Category] = @Category,[Description] = @Description,[IsUnique] = @IsUnique,[SortOrder] = @SortOrder,[IsActive] = @IsActive
		WHERE ((Class = @Class) AND ([Name] = @Name))
	ELSE
		INSERT INTO Field ([Class],[Name],[DataType],[Category],[Description],[IsUnique],[SortOrder],[IsActive]) VALUES (@Class,@Name,@DataType,@Category,@Description,@IsUnique,@SortOrder,@IsActive)
	--END
END
GO

GO
CREATE PROCEDURE db_AddClassAssociations
(
@parentClass varchar(50),
@childClass varchar(50),
@Association varchar(50),
@Caption varchar(50),
@IsDefault int,
@IsActive int
)
AS
BEGIN
DECLARE @CAID int
DECLARE @AssociationID int
IF (@Caption IS NULL) OR (@Caption = '')
	SET @Caption = (@parentClass + ' to ' + @childClass + ' ' + @Association)

SET @CAID = (SELECT TOP (1) ClassAssociation.CAid FROM ClassAssociation INNER JOIN AssociationType ON ClassAssociation.AssociationTypeID = AssociationType.pkid WHERE (ClassAssociation.ParentClass = @parentClass) AND (ClassAssociation.ChildClass = @childClass) AND (AssociationType.Name = @Association))
SET @AssociationID = (SELECT TOP (1) AssociationType.pkid FROM AssociationType WHERE AssociationType.Name = @Association)
IF (@CAID IS NULL) OR (@CAID = '') OR (@CAID = 0)
	IF (@AssociationID IS NULL) OR (@AssociationID = 0)
		BEGIN
			INSERT INTO AssociationType([Name]) VALUES (@Association)
--Restart this SP (This is probably a badass thing to do!)
			EXEC db_AddClassAssociations @parentClass=@parentClass,@childClass=@childClass,@Association=@Association,@Caption=@Caption,@IsDefault=@IsDefault,@IsActive=@IsActive
		END
	ELSE
		INSERT INTO ClassAssociation (ParentClass, ChildClass, AssociationTypeID, Caption, IsDefault, IsActive) VALUES (@parentClass, @childClass,@AssociationID,@Caption,@IsDefault,@IsActive)	
ELSE 
	UPDATE ClassAssociation	SET IsActive = @IsActive WHERE CAID = @CAID
	--END
END
GO

GO
CREATE PROCEDURE db_AddDefinitions
(
@Name varchar (50),
@IsActive int
)
AS
BEGIN
DECLARE @ID int
SET @ID = (SELECT TOP (1) DomainDefinition.pkid FROM DomainDefinition WHERE DomainDefinition.Name = @Name)
IF (@ID IS NULL) OR (@ID = 0)
	INSERT INTO DomainDefinition(Name,IsActive) VALUES (@Name, @IsActive)
ELSE
	UPDATE DomainDefinition SET IsActive = @IsActive WHERE Name = @Name
END
GO

GO
CREATE PROCEDURE db_AddPossibleValues
(
@DomainDefinition varchar (50),
@PossibleValue varchar (50),
@Series int,
@Description varchar (120),
@IsActive int
)
AS
BEGIN
--GET DefinitionID
DECLARE @ID int
DECLARE @PVID int
SET @ID = (SELECT TOP (1) DomainDefinition.pkid FROM DomainDefinition WHERE DomainDefinition.Name = @DomainDefinition)

IF (@ID IS NULL) OR (@ID = 0)
	BEGIN
		--No DomainDefinition ?ADD
		EXEC db_AddDefinitions @Name = @DomainDefinition, @IsActive = @IsActive
		EXEC db_AddPossibleValues @DomainDefinition=@DomainDefinition, @PossibleValue=@PossibleValue, @Series=@Series, @Description=@Description, @IsActive=@IsActive
	END
ELSE
	BEGIN
		--Check If value exist then update
		SET @PVID = (SELECT TOP (1) DomainDefinitionPossibleValue.DomainDefinitionID FROM DomainDefinitionPossibleValue WHERE DomainDefinitionID = @ID AND PossibleValue = @PossibleValue)
		IF (@PVID IS NULL) OR (@PVID = 0)
			INSERT INTO DomainDefinitionPossibleValue(DomainDefinitionID,PossibleValue,Series,Description,IsActive) VALUES (@ID,@PossibleValue,@Series,@Description,@IsActive)
		ELSE
			UPDATE DomainDefinitionPossibleValue SET IsActive = @IsActive, Description = @Description, Series = @Series WHERE DomainDefinitionID = @ID AND PossibleValue = @PossibleValue 
	END
END
GO
---------------------------------------------------------------------------------------------------------------------------------------------------------

EXEC db_AddClasses @Class ='Activity',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='Activity',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddClasses @Class ='Attribute',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='Attribute',@Name='DomainType',@DataType='DomainAttributeType',@Category='',@Description='Domain Attribute Type',@IsUnique=0,@SortOrder=2,@IsActive = 1
EXEC db_AddFields @Class='Attribute',@Name='DomainDef',@DataType='System.String',@Category='',@Description='Domain Definition',@IsUnique=0,@SortOrder=3,@IsActive = 1
EXEC db_AddFields @Class='Attribute',@Name='Length',@DataType='System.Int32',@Category='',@Description='Data Length',@IsUnique=0,@SortOrder=4,@IsActive = 1
EXEC db_AddFields @Class='Attribute',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddClasses @Class ='Competency',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='Competency',@Name='Name',@DataType='System.String',@Category='',@Description='Name',@IsUnique=0,@SortOrder=1,@IsActive = 1
EXEC db_AddFields @Class='Competency',@Name='Type',@DataType='CompetencyType',@Category='',@Description='Competency Type',@IsUnique=0,@SortOrder=2,@IsActive = 1
EXEC db_AddFields @Class='Competency',@Name='Level',@DataType='System.String',@Category='',@Description='Competency Level',@IsUnique=0,@SortOrder=3,@IsActive = 1
EXEC db_AddFields @Class='Competency',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddFields @Class='Competency',@Name='CustomField1',@DataType='System.String',@Category='',@Description='Custom Field 1',@IsUnique=0,@SortOrder=98,@IsActive = 1
EXEC db_AddFields @Class='Competency',@Name='CustomField2',@DataType='System.String',@Category='',@Description='Custom Field 2',@IsUnique=0,@SortOrder=99,@IsActive = 1
EXEC db_AddFields @Class='Competency',@Name='CustomField3',@DataType='System.String',@Category='',@Description='Custom Field 3',@IsUnique=0,@SortOrder=100,@IsActive = 1
EXEC db_AddClasses @Class ='Conditional',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='Conditional',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddClasses @Class ='ConditionalDescription',@DescriptionCode='Value.ToString()',@Category='',@IsActive = 1
EXEC db_AddFields @Class='ConditionalDescription',@Name='Value',@DataType='System.String',@Category='',@Description='Condition Domain Value',@IsUnique=0,@SortOrder=1,@IsActive = 1
EXEC db_AddFields @Class='ConditionalDescription',@Name='Sequence',@DataType='System.Int32',@Category='',@Description='Condition Sequence Number',@IsUnique=0,@SortOrder=2,@IsActive = 1
EXEC db_AddClasses @Class ='CSF',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='CSF',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddClasses @Class ='DataColumn',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='DataColumn',@Name='ColumnType',@DataType='DomainAttributeType',@Category='',@Description='Column Type',@IsUnique=0,@SortOrder=2,@IsActive = 1
EXEC db_AddFields @Class='DataColumn',@Name='DomainDef',@DataType='System.String',@Category='',@Description='Domain Definition',@IsUnique=0,@SortOrder=3,@IsActive = 1
EXEC db_AddFields @Class='DataColumn',@Name='ColumnLength',@DataType='System.Int32',@Category='',@Description='Column Length',@IsUnique=0,@SortOrder=4,@IsActive = 1
EXEC db_AddFields @Class='DataColumn',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddClasses @Class ='DataSchema',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='DataSchema',@Name='ArchPriority',@DataType='System.String',@Category='',@Description='Architecture Priority / Sequence Number',@IsUnique=0,@SortOrder=3,@IsActive = 1
EXEC db_AddFields @Class='DataSchema',@Name='DatabaseType',@DataType='DatabaseType',@Category='',@Description='Database Type for Physical Database',@IsUnique=0,@SortOrder=4,@IsActive = 1
EXEC db_AddFields @Class='DataSchema',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddClasses @Class ='DataTable',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='DataTable',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddClasses @Class ='DataView',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='DataView',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddClasses @Class ='FunctionalDependency',@DescriptionCode='Description',@Category='',@IsActive = 1
EXEC db_AddFields @Class='FunctionalDependency',@Name='InferenceRule',@DataType='InferenceRule',@Category='',@Description='Inference Rule',@IsUnique=0,@SortOrder=1,@IsActive = 1
EXEC db_AddFields @Class='FunctionalDependency',@Name='Condition',@DataType='System.String',@Category='',@Description='Condition',@IsUnique=0,@SortOrder=2,@IsActive = 1
EXEC db_AddFields @Class='FunctionalDependency',@Name='Description',@DataType='System.String',@Category='',@Description='Description',@IsUnique=0,@SortOrder=3,@IsActive = 1
EXEC db_AddFields @Class='FunctionalDependency',@Name='CohesionWeight',@DataType='System.String',@Category='',@Description='Cohesion Weight',@IsUnique=0,@SortOrder=4,@IsActive = 1
EXEC db_AddClasses @Class ='Employee',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='Employee',@Name='Email',@DataType='System.String',@Category='',@Description='Email Adress',@IsUnique=0,@SortOrder=6,@IsActive = 1
EXEC db_AddFields @Class='Employee',@Name='Telephone',@DataType='System.String',@Category='',@Description='Telephone Number',@IsUnique=0,@SortOrder=7,@IsActive = 1
EXEC db_AddFields @Class='Employee',@Name='Mobile',@DataType='System.String',@Category='',@Description='Mobile Phone Number',@IsUnique=0,@SortOrder=8,@IsActive = 1
EXEC db_AddFields @Class='Employee',@Name='Fax',@DataType='System.String',@Category='',@Description='Fax Number',@IsUnique=0,@SortOrder=9,@IsActive = 1
EXEC db_AddFields @Class='Employee',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddClasses @Class ='Entity',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='Entity',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddFields @Class='FlowDescription',@Name='TimeIndicator',@DataType='System.String',@Category='',@Description='Time Indicator',@IsUnique=0,@SortOrder=4,@IsActive = 1
EXEC db_AddFields @Class='Function',@Name='ContextualIndicator',@DataType='System.String',@Category='',@Description='',@IsUnique=0,@SortOrder=2,@IsActive = 0
EXEC db_AddFields @Class='Function',@Name='EnvironmentInd',@DataType='EnvironmentIndicator',@Category='',@Description='Environment Indicator (Internal, Target or Eternal)',@IsUnique=0,@SortOrder=2,@IsActive = 1
EXEC db_AddFields @Class='Function',@Name='FunctionCriticality',@DataType='System.Int32',@Category='',@Description='Function Criticality - CSFs directly contributing to',@IsUnique=0,@SortOrder=2,@IsActive = 1
EXEC db_AddFields @Class='Function',@Name='ManagementAbility',@DataType='System.String',@Category='',@Description='Rating (0-10) for management ability - management mechanisms in place',@IsUnique=0,@SortOrder=3,@IsActive = 1
EXEC db_AddFields @Class='Function',@Name='InfoAvailability',@DataType='System.String',@Category='',@Description='Rating (0-10) for Adequate and correct information being readily available',@IsUnique=0,@SortOrder=4,@IsActive = 1
EXEC db_AddFields @Class='Function',@Name='GovernanceMech',@DataType='System.String',@Category='',@Description='Rating (0-10) for  the availability of governance mechanisms such as policies and the auditing of compliance',@IsUnique=0,@SortOrder=5,@IsActive = 1
EXEC db_AddFields @Class='Function',@Name='LegalAspects',@DataType='System.String',@Category='',@Description='Legal Aspects',@IsUnique=0,@SortOrder=6,@IsActive = 0
EXEC db_AddFields @Class='Function',@Name='Technology',@DataType='System.String',@Category='',@Description='Rating (0-10) for availability of adequate technology (Incl. IT)',@IsUnique=0,@SortOrder=7,@IsActive = 1
EXEC db_AddFields @Class='Function',@Name='CapitalAvailability',@DataType='System.String',@Category='',@Description='Rating (0-10) for availability of operational capital and CAPEX that needs to be budgeted for.',@IsUnique=0,@SortOrder=8,@IsActive = 1
EXEC db_AddFields @Class='Function',@Name='Budget',@DataType='System.String',@Category='',@Description='Budget',@IsUnique=0,@SortOrder=8,@IsActive = 0
EXEC db_AddFields @Class='Function',@Name='Energy',@DataType='System.String',@Category='',@Description='Rating (0-10) for availability of energy',@IsUnique=0,@SortOrder=9,@IsActive = 1
EXEC db_AddFields @Class='Function',@Name='RawMaterial',@DataType='System.String',@Category='',@Description='Rating (0-10) for availability of raw material',@IsUnique=0,@SortOrder=10,@IsActive = 1
EXEC db_AddFields @Class='Function',@Name='Competencies',@DataType='System.String',@Category='',@Description='Rating (0-10) for availability of people competencies required',@IsUnique=0,@SortOrder=11,@IsActive = 1
EXEC db_AddFields @Class='Function',@Name='SkillAvailability',@DataType='System.String',@Category='',@Description='Skill Availability',@IsUnique=0,@SortOrder=11,@IsActive = 0
EXEC db_AddFields @Class='Function',@Name='Manpower',@DataType='System.String',@Category='',@Description='Rating (0-10) for  availability of adequate number of people',@IsUnique=0,@SortOrder=12,@IsActive = 1
EXEC db_AddFields @Class='Function',@Name='Ethics',@DataType='System.String',@Category='',@Description='Rating (0-10) for correctness of Energy, Culture, etc. of people',@IsUnique=0,@SortOrder=12,@IsActive = 1
EXEC db_AddFields @Class='Function',@Name='Facilities',@DataType='System.String',@Category='',@Description='Rating (0-10) for adequate facilities (Incl. Location)',@IsUnique=0,@SortOrder=12,@IsActive = 1
EXEC db_AddFields @Class='Function',@Name='ServicesUsage',@DataType='System.String',@Category='',@Description='Rating (0-10) for effective usage of services from target environment',@IsUnique=0,@SortOrder=12,@IsActive = 1
EXEC db_AddFields @Class='Function',@Name='Equipment',@DataType='System.String',@Category='',@Description='Rating (0-10) for availability of equipment',@IsUnique=0,@SortOrder=12,@IsActive = 1
EXEC db_AddFields @Class='Function',@Name='TimeIndicator',@DataType='System.String',@Category='',@Description='Rating (0-10) for time as a constraint',@IsUnique=0,@SortOrder=12,@IsActive = 1
EXEC db_AddFields @Class='Function',@Name='Effeciency',@DataType='System.String',@Category='',@Description='Rating (0-10) for overall efficiency (Resource usage vs. Output)',@IsUnique=0,@SortOrder=13,@IsActive = 1
EXEC db_AddFields @Class='Function',@Name='Effectiviness',@DataType='System.String',@Category='',@Description='Rating (0-10) for overall effectiveness (Quality of Output)',@IsUnique=0,@SortOrder=14,@IsActive = 1
EXEC db_AddFields @Class='Function',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddClasses @Class ='Implication',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='Implication',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddClasses @Class ='Iteration',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='Iteration',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddClasses @Class ='Job',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='Job',@Name='Name',@DataType='System.String',@Category='',@Description='Name',@IsUnique=0,@SortOrder=1,@IsActive = 1
EXEC db_AddFields @Class='Job',@Name='Type',@DataType='JobType',@Category='',@Description='Job Type',@IsUnique=0,@SortOrder=2,@IsActive = 1
EXEC db_AddFields @Class='Job',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=3,@IsActive = 1
EXEC db_AddFields @Class='Job',@Name='CustomField1',@DataType='System.String',@Category='',@Description='Custom Field 1',@IsUnique=0,@SortOrder=98,@IsActive = 1
EXEC db_AddFields @Class='Job',@Name='CustomField2',@DataType='System.String',@Category='',@Description='Custom Field 2',@IsUnique=0,@SortOrder=99,@IsActive = 1
EXEC db_AddFields @Class='Job',@Name='CustomField3',@DataType='System.String',@Category='',@Description='Custom Field 3',@IsUnique=0,@SortOrder=100,@IsActive = 1
EXEC db_AddClasses @Class ='JobPosition',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='JobPosition',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddClasses @Class ='Location',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='Location',@Name='Address',@DataType='System.String',@Category='',@Description='Address',@IsUnique=0,@SortOrder=3,@IsActive = 1
EXEC db_AddFields @Class='Location',@Name='Telephone',@DataType='System.String',@Category='',@Description='Telephone Number',@IsUnique=0,@SortOrder=4,@IsActive = 1
EXEC db_AddFields @Class='Location',@Name='Fax',@DataType='System.String',@Category='',@Description='Fax Number',@IsUnique=0,@SortOrder=5,@IsActive = 1
EXEC db_AddFields @Class='Location',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddClasses @Class ='LocationAssociation',@DescriptionCode='(AssociationType!=null && Distance!=null)?Distance + " " + AssociationType:(AssociationType!=null && Distance==null)?AssociationType:Distance',@Category='',@IsActive = 1
EXEC db_AddFields @Class='LocationAssociation',@Name='Distance',@DataType='System.String',@Category='',@Description='Distance Between Locations',@IsUnique=0,@SortOrder=2,@IsActive = 1
EXEC db_AddFields @Class='LocationAssociation',@Name='TimeIndicator',@DataType='System.String',@Category='',@Description='Typical Time, Frequency',@IsUnique=0,@SortOrder=3,@IsActive = 1
EXEC db_AddFields @Class='LocationAssociation',@Name='AssociationType',@DataType='System.String',@Category='',@Description='Association Method',@IsUnique=0,@SortOrder=4,@IsActive = 1

EXEC db_AddClasses @Class ='MeasurementItem',@DescriptionCode='',@Category='',@IsActive = 0
EXEC db_AddClasses @Class ='GovernanceMechanism',@DescriptionCode='Description',@Category='',@IsActive = 1
EXEC db_AddFields @Class='GovernanceMechanism',@Name='EnvironmentInd',@DataType='EnvironmentIndicator',@Category='',@Description='Environment Indicator (Internal, Target or Eternal)',@IsUnique=0,@SortOrder=2,@IsActive = 1
EXEC db_AddFields @Class='GovernanceMechanism',@Name='GovernanceMechType',@DataType='GovernanceMechType',@Category='',@Description='Governance Mechanism Type',@IsUnique=0,@SortOrder=3,@IsActive = 1
EXEC db_AddFields @Class='GovernanceMechanism',@Name='UniqueRef',@DataType='System.String',@Category='',@Description='Unique Reference',@IsUnique=0,@SortOrder=4,@IsActive = 1
EXEC db_AddFields @Class='GovernanceMechanism',@Name='ValidityPeriod',@DataType='System.String',@Category='',@Description='Applicable Time Period',@IsUnique=0,@SortOrder=5,@IsActive = 1
EXEC db_AddFields @Class='GovernanceMechanism',@Name='Description',@DataType='System.String',@Category='',@Description='Description',@IsUnique=0,@SortOrder=6,@IsActive = 1
EXEC db_AddFields @Class='GovernanceMechanism',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddClasses @Class ='MutuallyExclusiveIndicator',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='MutuallyExclusiveIndicator',@Name='Name',@DataType='System.String',@Category='',@Description='Selector Attribute',@IsUnique=0,@SortOrder=1,@IsActive = 0
EXEC db_AddFields @Class='MutuallyExclusiveIndicator',@Name='SelectorType',@DataType='SelectorAttributeType',@Category='',@Description='Selector Attribute',@IsUnique=0,@SortOrder=1,@IsActive = 1
EXEC db_AddClasses @Class ='NetworkComponent',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='NetworkComponent',@Name='NetworkType',@DataType='NetworkType',@Category='',@Description='Network Type',@IsUnique=0,@SortOrder=25,@IsActive = 1
EXEC db_AddFields @Class='NetworkComponent',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddClasses @Class ='Object',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='Object',@Name='Type',@DataType='ObjectType',@Category='',@Description='Object Type',@IsUnique=0,@SortOrder=2,@IsActive = 1
EXEC db_AddFields @Class='Object',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddClasses @Class ='OrganizationalUnit',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='OrganizationalUnit',@Name='Type',@DataType='System.String',@Category='',@Description='Unit Type',@IsUnique=0,@SortOrder=2,@IsActive = 0
EXEC db_AddFields @Class='OrganizationalUnit',@Name='Type',@DataType='OrganizationalUnitType',@Category='',@Description='Organisation Unit Type',@IsUnique=0,@SortOrder=2,@IsActive = 1
EXEC db_AddFields @Class='OrganizationalUnit',@Name='Telephone',@DataType='System.String',@Category='',@Description='Telephone Number',@IsUnique=0,@SortOrder=3,@IsActive = 1
EXEC db_AddFields @Class='OrganizationalUnit',@Name='Fax',@DataType='System.String',@Category='',@Description='Fax Number',@IsUnique=0,@SortOrder=4,@IsActive = 1
EXEC db_AddFields @Class='OrganizationalUnit',@Name='Email',@DataType='System.String',@Category='',@Description='Email Adress',@IsUnique=0,@SortOrder=5,@IsActive = 1
EXEC db_AddFields @Class='OrganizationalUnit',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddClasses @Class ='Peripheral',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='Peripheral',@Name='Type',@DataType='MMType',@Category='',@Description='Multi Media Type',@IsUnique=0,@SortOrder=23,@IsActive = 1
EXEC db_AddFields @Class='Peripheral',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddClasses @Class ='Process',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='Process',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddClasses @Class ='Responsibility',@DescriptionCode='',@Category='',@IsActive = 0
EXEC db_AddFields @Class='Responsibility',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddClasses @Class ='Role',@DescriptionCode='Name',@Category='',@IsActive = 1
EXEC db_AddFields @Class='Role',@Name='Name',@DataType='System.String',@Category='',@Description='Role Name',@IsUnique=0,@SortOrder=1,@IsActive = 1
EXEC db_AddFields @Class='Role',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddFields @Class='Role',@Name='CustomField1',@DataType='System.String',@Category='',@Description='Custom Field 1',@IsUnique=0,@SortOrder=98,@IsActive = 1
EXEC db_AddFields @Class='Role',@Name='CustomField2',@DataType='System.String',@Category='',@Description='Custom Field 2',@IsUnique=0,@SortOrder=99,@IsActive = 1
EXEC db_AddFields @Class='Role',@Name='CustomField3',@DataType='System.String',@Category='',@Description='Custom Field 3',@IsUnique=0,@SortOrder=100,@IsActive = 1
EXEC db_AddClasses @Class ='Scenario',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='Scenario',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddClasses @Class ='SelectorAttribute',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='SelectorAttribute',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddClasses @Class ='Skill',@DescriptionCode='',@Category='',@IsActive = 0
EXEC db_AddFields @Class='Skill',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 0
EXEC db_AddClasses @Class ='Software',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='Software',@Name='UserInterface',@DataType='UserInterfaceType',@Category='',@Description='User Interface Type',@IsUnique=0,@SortOrder=18,@IsActive = 1
EXEC db_AddFields @Class='Software',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddClasses @Class ='StorageComponent',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='StorageComponent',@Name='FileSystem',@DataType='System.String',@Category='',@Description='File System',@IsUnique=0,@SortOrder=19,@IsActive = 1
EXEC db_AddFields @Class='StorageComponent',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddClasses @Class ='StrategicTheme',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='StrategicTheme',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddClasses @Class ='SystemComponent',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='SystemComponent',@Name='ServerType',@DataType='ServerType',@Category='',@Description='Server Type',@IsUnique=0,@SortOrder=20,@IsActive = 1
EXEC db_AddFields @Class='SystemComponent',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddClasses @Class ='TimeScheme',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='TimeScheme',@Name='Name',@DataType='System.String',@Category='',@Description='Name',@IsUnique=0,@SortOrder=1,@IsActive = 1
EXEC db_AddFields @Class='TimeScheme',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddFields @Class='TimeScheme',@Name='CustomField1',@DataType='System.String',@Category='',@Description='Custom Field 1',@IsUnique=0,@SortOrder=98,@IsActive = 1
EXEC db_AddFields @Class='TimeScheme',@Name='CustomField2',@DataType='System.String',@Category='',@Description='Custom Field 2',@IsUnique=0,@SortOrder=99,@IsActive = 1
EXEC db_AddFields @Class='TimeScheme',@Name='CustomField3',@DataType='System.String',@Category='',@Description='Custom Field 3',@IsUnique=0,@SortOrder=100,@IsActive = 1
EXEC db_AddClasses @Class ='TimeUnit',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='TimeUnit',@Name='Name',@DataType='System.String',@Category='',@Description='Name',@IsUnique=0,@SortOrder=1,@IsActive = 1
EXEC db_AddFields @Class='TimeUnit',@Name='TimeUnitType',@DataType='TimeUnitType',@Category='',@Description='Time Unit Type',@IsUnique=0,@SortOrder=2,@IsActive = 1
EXEC db_AddFields @Class='TimeUnit',@Name='Value',@DataType='System.String',@Category='',@Description='Time Value',@IsUnique=0,@SortOrder=3,@IsActive = 1
EXEC db_AddFields @Class='TimeUnit',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddFields @Class='TimeUnit',@Name='CustomField1',@DataType='System.String',@Category='',@Description='Custom Field 1',@IsUnique=0,@SortOrder=98,@IsActive = 1
EXEC db_AddFields @Class='TimeUnit',@Name='CustomField2',@DataType='System.String',@Category='',@Description='Custom Field 2',@IsUnique=0,@SortOrder=99,@IsActive = 1
EXEC db_AddFields @Class='TimeUnit',@Name='CustomField3',@DataType='System.String',@Category='',@Description='Custom Field 3',@IsUnique=0,@SortOrder=100,@IsActive = 1
EXEC db_AddClasses @Class ='Rationale',@DescriptionCode='Value',@Category='',@IsActive = 1
EXEC db_AddFields @Class='Rationale',@Name='UniqueRef',@DataType='System.String',@Category='',@Description='Unique Custom Reference ID',@IsUnique=0,@SortOrder=1,@IsActive = 1
EXEC db_AddFields @Class='Rationale',@Name='RationaleType',@DataType='RationaleType',@Category='',@Description='Rationale Type',@IsUnique=0,@SortOrder=2,@IsActive = 1
EXEC db_AddFields @Class='Rationale',@Name='Value',@DataType='System.String',@Category='',@Description='Value',@IsUnique=0,@SortOrder=3,@IsActive = 1
EXEC db_AddFields @Class='Rationale',@Name='AuthorName',@DataType='System.String',@Category='',@Description='Author of this Rationale',@IsUnique=0,@SortOrder=4,@IsActive = 1
EXEC db_AddFields @Class='Rationale',@Name='EffectiveDate',@DataType='DateTime',@Category='',@Description='Effective Date of Creation, Deletion or Update',@IsUnique=0,@SortOrder=5,@IsActive = 1
EXEC db_AddClasses @Class ='Logic',@DescriptionCode='Description',@Category='',@IsActive = 1
EXEC db_AddFields @Class='Logic',@Name='Description',@DataType='System.String',@Category='',@Description='Logic Description',@IsUnique=0,@SortOrder=1,@IsActive = 1
EXEC db_AddFields @Class='Logic',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddFields @Class='Logic',@Name='CustomField1',@DataType='System.String',@Category='',@Description='Custom Field 1',@IsUnique=0,@SortOrder=98,@IsActive = 1
EXEC db_AddFields @Class='Logic',@Name='CustomField2',@DataType='System.String',@Category='',@Description='Custom Field 2',@IsUnique=0,@SortOrder=99,@IsActive = 1
EXEC db_AddFields @Class='Logic',@Name='CustomField3',@DataType='System.String',@Category='',@Description='Custom Field 3',@IsUnique=0,@SortOrder=100,@IsActive = 1
EXEC db_AddFields @Class='LocationAssociation',@Name='CustomField1',@DataType='System.String',@Category='',@Description='Custom Field 1',@IsUnique=0,@SortOrder=98,@IsActive = 1
EXEC db_AddFields @Class='LocationAssociation',@Name='CustomField2',@DataType='System.String',@Category='',@Description='Custom Field 2',@IsUnique=0,@SortOrder=99,@IsActive = 1
EXEC db_AddFields @Class='LocationAssociation',@Name='CustomField3',@DataType='System.String',@Category='',@Description='Custom Field 3',@IsUnique=0,@SortOrder=100,@IsActive = 1



EXEC db_AddClassAssociations @parentClass='Activity',@childClass='Activity',@Association='Decomposition',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Activity',@childClass='Activity',@Association='Classification',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Activity',@childClass='DataColumn',@Association='Delete',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Activity',@childClass='DataColumn',@Association='Create',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Activity',@childClass='DataColumn',@Association='Maintain',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Activity',@childClass='Competency',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Activity',@childClass='Role',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Activity',@childClass='Rationale',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Activity',@childClass='Attribute',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Activity',@childClass='DataColumn',@Association='Update',@Caption='',@IsDefault=0,@IsActive=1

EXEC db_AddClassAssociations @parentClass='Attribute',@childClass='DataTable',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Attribute',@childClass='Process',@Association='Read',@Caption='',@IsDefault=0,@IsActive=1

EXEC db_AddClassAssociations @parentClass='DataColumn',@childClass='Activity',@Association='Read',@Caption='',@IsDefault=0,@IsActive=1

EXEC db_AddClassAssociations @parentClass='Competency',@childClass='Competency',@Association='Classification',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Competency',@childClass='Competency',@Association='Decomposition',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Competency',@childClass='Employee',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Competency',@childClass='Function',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Competency',@childClass='Job',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Competency',@childClass='JobPosition',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Competency',@childClass='Process',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Competency',@childClass='Responsibility',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=0
EXEC db_AddClassAssociations @parentClass='Competency',@childClass='Role',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Competency',@childClass='Skill',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Competency',@childClass='TimeUnit',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1

EXEC db_AddClassAssociations @parentClass='CSF',@childClass='Job',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='CSF',@childClass='Role',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='CSF',@childClass='TimeScheme',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='CSF',@childClass='TimeUnit',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1

EXEC db_AddClassAssociations @parentClass='DataSchema',@childClass='DataSchema',@Association='SubSetOf',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='DataSchema',@childClass='DataSchema',@Association='Decomposition',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='DataSchema',@childClass='DataSchema',@Association='DynamicFlow',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='DataSchema',@childClass='DataSchema',@Association='Classification',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='DataSchema',@childClass='MutuallyExclusiveIndicator',@Association='MutuallyExclusiveLink',@Caption='',@IsDefault=0,@IsActive=1

EXEC db_AddClassAssociations @parentClass='MutuallyExclusiveIndicator',@childClass='DataSchema',@Association='SubSetOf',@Caption='',@IsDefault=0,@IsActive=1

EXEC db_AddClassAssociations @parentClass='DataView',@childClass='DataView',@Association='SubSetOf',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='DataView',@childClass='DataView',@Association='Decomposition',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='DataView',@childClass='DataView',@Association='Classification',@Caption='',@IsDefault=0,@IsActive=1

EXEC db_AddClassAssociations @parentClass='Employee',@childClass='Role',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1

EXEC db_AddClassAssociations @parentClass='Entity',@childClass='Process',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Entity',@childClass='Role',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Entity',@childClass='Function',@Association='Read',@Caption='',@IsDefault=0,@IsActive=1

EXEC db_AddClassAssociations @parentClass='Function',@childClass='Job',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Function',@childClass='Role',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Function',@childClass='TimeUnit',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1

EXEC db_AddClassAssociations @parentClass='Implication',@childClass='DataSchema',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Implication',@childClass='Entity',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Implication',@childClass='Attribute',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Implication',@childClass='Job',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Implication',@childClass='Location',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Implication',@childClass='GovernanceMechanism',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Implication',@childClass='Process',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Implication',@childClass='Role',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Implication',@childClass='TimeScheme',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Implication',@childClass='TimeUnit',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1

EXEC db_AddClassAssociations @parentClass='Job',@childClass='Job',@Association='Auxiliary',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Job',@childClass='Job',@Association='Classification',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Job',@childClass='Activity',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Job',@childClass='Entity',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Job',@childClass='Job',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Job',@childClass='JobPosition',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Job',@childClass='Location',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Job',@childClass='GovernanceMechanism',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Job',@childClass='NetworkComponent',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Job',@childClass='Object',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Job',@childClass='OrganizationalUnit',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Job',@childClass='Peripheral',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Job',@childClass='Process',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Job',@childClass='Role',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Job',@childClass='Skill',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Job',@childClass='Software',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Job',@childClass='StorageComponent',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Job',@childClass='SystemComponent',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Job',@childClass='TimeUnit',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Job',@childClass='Function',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Job',@childClass='Conditional',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Job',@childClass='DataSchema',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Job',@childClass='DataView',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Job',@childClass='Entity',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Job',@childClass='DataTable',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1

EXEC db_AddClassAssociations @parentClass='JobPosition',@childClass='JobPosition',@Association='Decomposition',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='JobPosition',@childClass='NetworkComponent',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='JobPosition',@childClass='Object',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='JobPosition',@childClass='Peripheral',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='JobPosition',@childClass='Role',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='JobPosition',@childClass='Software',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='JobPosition',@childClass='StorageComponent',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='JobPosition',@childClass='SystemComponent',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='JobPosition',@childClass='Function',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='JobPosition',@childClass='Process',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='JobPosition',@childClass='Activity',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='JobPosition',@childClass='Conditional',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='JobPosition',@childClass='DataSchema',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='JobPosition',@childClass='DataView',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='JobPosition',@childClass='Entity',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='JobPosition',@childClass='DataTable',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1

EXEC db_AddClassAssociations @parentClass='Location',@childClass='Location',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Location',@childClass='CSF',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Location',@childClass='GovernanceMechanism',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Location',@childClass='Role',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1


EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='GovernanceMechanism',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='GovernanceMechanism',@Association='Auxiliary',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='GovernanceMechanism',@Association='Classification',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='GovernanceMechanism',@Association='Decomposition',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='GovernanceMechanism',@Association='FunctionalDependency',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='Attribute',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='CSF',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='DataColumn',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='DataSchema',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='DataTable',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='Conditional',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='Iteration',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='Activity',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='Rationale',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='Competency',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='Employee',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='Function',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='GovernanceMechanism',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='Implication',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='Job',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='JobPosition',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='Location',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='Object',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='OrganizationalUnit',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='Process',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='Responsibility',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=0
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='Scenario',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='Skill',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='Software',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='StorageComponent',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='StrategicTheme',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='SystemComponent',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='TimeScheme',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='DataView',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='Entity',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='Role',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='NetworkComponent',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='Peripheral',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='GovernanceMechanism',@childClass='TimeUnit',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1

EXEC db_AddClassAssociations @parentClass='Object',@childClass='Object',@Association='Auxiliary',@Caption='',@IsDefault=0,@IsActive=1

EXEC db_AddClassAssociations @parentClass='OrganizationalUnit',@childClass='Competency',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='OrganizationalUnit',@childClass='Entity',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='OrganizationalUnit',@childClass='TimeUnit',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='OrganizationalUnit',@childClass='Function',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='OrganizationalUnit',@childClass='Process',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='OrganizationalUnit',@childClass='Activity',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='OrganizationalUnit',@childClass='Conditional',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='OrganizationalUnit',@childClass='DataSchema',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='OrganizationalUnit',@childClass='DataView',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='OrganizationalUnit',@childClass='Entity',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='OrganizationalUnit',@childClass='DataTable',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1

EXEC db_AddClassAssociations @parentClass='Process',@childClass='Role',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Process',@childClass='TimeUnit',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Process',@childClass='Attribute',@Association='Delete',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Process',@childClass='Attribute',@Association='Create',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Process',@childClass='Attribute',@Association='Maintain',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Process',@childClass='Competency',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Process',@childClass='DataColumn',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Process',@childClass='Attribute',@Association='Update',@Caption='',@IsDefault=0,@IsActive=1

EXEC db_AddClassAssociations @parentClass='Responsibility',@childClass='Role',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=0

EXEC db_AddClassAssociations @parentClass='Role',@childClass='Role',@Association='Auxiliary',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Role',@childClass='Role',@Association='Classification',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Role',@childClass='Role',@Association='Decomposition',@Caption='',@IsDefault=0,@IsActive=1

EXEC db_AddClassAssociations @parentClass='Scenario',@childClass='Scenario',@Association='Auxiliary',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Scenario',@childClass='Scenario',@Association='Classification',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Scenario',@childClass='Scenario',@Association='Decomposition',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Scenario',@childClass='TimeUnit',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1

EXEC db_AddClassAssociations @parentClass='Skill',@childClass='TimeUnit',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1

EXEC db_AddClassAssociations @parentClass='Software',@childClass='Software',@Association='Auxiliary',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Software',@childClass='Software',@Association='Classification',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Software',@childClass='SystemComponent',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Software',@childClass='Software',@Association='DynamicFlow',@Caption='',@IsDefault=0,@IsActive=1

EXEC db_AddClassAssociations @parentClass='StorageComponent',@childClass='SystemComponent',@Association='Dependencies',@Caption='',@IsDefault=0,@IsActive=1

EXEC db_AddClassAssociations @parentClass='StrategicTheme',@childClass='TimeScheme',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='StrategicTheme',@childClass='TimeUnit',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1

EXEC db_AddClassAssociations @parentClass='SystemComponent',@childClass='StorageComponent',@Association='Dependencies',@Caption='',@IsDefault=0,@IsActive=1

EXEC db_AddClassAssociations @parentClass='TimeScheme',@childClass='TimeUnit',@Association='Decomposition',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='TimeScheme',@childClass='TimeScheme',@Association='Decomposition',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='TimeScheme',@childClass='TimeScheme',@Association='Classification',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='TimeScheme',@childClass='TimeScheme',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1

EXEC db_AddClassAssociations @parentClass='TimeUnit',@childClass='TimeUnit',@Association='Decomposition',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='TimeUnit',@childClass='TimeUnit',@Association='Classification',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='TimeUnit',@childClass='TimeUnit',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1

EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='Activity',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='Attribute',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='Competency',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='Conditional',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='CSF',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='DataColumn',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='DataSchema',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='DataTable',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='DataView',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='Employee',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='Entity',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='Function',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='Implication',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='Iteration',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='Job',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='JobPosition',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='Location',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='LocationAssociation',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='GovernanceMechanism',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='MutuallyExclusiveIndicator',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='NetworkComponent',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='Object',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='OrganizationalUnit',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='Peripheral',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='Process',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='Role',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='Scenario',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='SelectorAttribute',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='Software',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='StorageComponent',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='StrategicTheme',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='SystemComponent',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='TimeScheme',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='TimeUnit',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Rationale',@childClass='Rationale',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1

EXEC db_AddClassAssociations @parentClass='Employee',@childClass='Object',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Employee',@childClass='SystemComponent',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Employee',@childClass='StorageComponent',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Employee',@childClass='DataSchema',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Employee',@childClass='NetworkComponent',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Employee',@childClass='Peripheral',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Employee',@childClass='Software',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Employee',@childClass='Function',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Employee',@childClass='Process',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Employee',@childClass='Activity',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Employee',@childClass='Conditional',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Employee',@childClass='DataView',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Employee',@childClass='Entity',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Employee',@childClass='DataTable',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Function',@childClass='Object',@Association='Provide',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Function',@childClass='Object',@Association='Use',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Function',@childClass='Object',@Association='EnabledBy',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Process',@childClass='Object',@Association='Provide',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Process',@childClass='Object',@Association='Use',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Process',@childClass='Object',@Association='EnabledBy',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Activity',@childClass='Object',@Association='Provide',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Activity',@childClass='Object',@Association='Use',@Caption='',@IsDefault=0,@IsActive=1
EXEC db_AddClassAssociations @parentClass='Activity',@childClass='Object',@Association='EnabledBy',@Caption='',@IsDefault=0,@IsActive=1


EXEC db_AddDefinitions @Name = 'AttributeType',@IsActive = 1
EXEC db_AddPossibleValues @DomainDefinition='AttributeType',@PossibleValue='CandidateKey',@Series=3,@Description='CandidateKey',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='AttributeType',@PossibleValue='Descriptive',@Series=4,@Description='Descriptive',@IsActive=1
EXEC db_AddDefinitions @Name = 'CompetencyType',@IsActive = 1
EXEC db_AddPossibleValues @DomainDefinition='CompetencyType',@PossibleValue='Skill',@Series=1,@Description='Skill',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='CompetencyType',@PossibleValue='Qualif',@Series=2,@Description='Qualification',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='CompetencyType',@PossibleValue='Know',@Series=3,@Description='Knowledge',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='CompetencyType',@PossibleValue='Attitude',@Series=4,@Description='Attitude',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='CompetencyType',@PossibleValue='Aptitude',@Series=5,@Description='Aptitude',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='CompetencyType',@PossibleValue='Value',@Series=6,@Description='Value',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='CompetencyType',@PossibleValue='Exper',@Series=7,@Description='Experience',@IsActive=1
EXEC db_AddDefinitions @Name = 'ContextualIndicator',@IsActive = 0
EXEC db_AddDefinitions @Name = 'EnvironmentIndicator',@IsActive = 1
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentIndicator',@PossibleValue='BussInt',@Series=1,@Description='BussInt',@IsActive=0
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentIndicator',@PossibleValue='Internal',@Series=1,@Description='Internal Environment',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentIndicator',@PossibleValue='BussExt',@Series=2,@Description='BussExt',@IsActive=0
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentIndicator',@PossibleValue='Target',@Series=2,@Description='Target Environment',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentIndicator',@PossibleValue='External',@Series=3,@Description='External Environment',@IsActive=1
EXEC db_AddDefinitions @Name = 'DatabaseType',@IsActive = 1
EXEC db_AddPossibleValues @DomainDefinition='DatabaseType',@PossibleValue='FlatFile',@Series=1,@Description='FlatFile',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DatabaseType',@PossibleValue='RDBMS',@Series=2,@Description='RDBMS',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DatabaseType',@PossibleValue='ODBMS',@Series=3,@Description='ODBMS',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DatabaseType',@PossibleValue='Network',@Series=4,@Description='Network',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DatabaseType',@PossibleValue='Hierarchy',@Series=5,@Description='Hierachical',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DatabaseType',@PossibleValue='XML',@Series=6,@Description='XML',@IsActive=1
EXEC db_AddDefinitions @Name = 'DataEntityType',@IsActive = 1
EXEC db_AddPossibleValues @DomainDefinition='DataEntityType',@PossibleValue='O',@Series=1,@Description='O',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DataEntityType',@PossibleValue='E',@Series=2,@Description='E',@IsActive=1
EXEC db_AddDefinitions @Name = 'DataSchemaType',@IsActive = 1
EXEC db_AddPossibleValues @DomainDefinition='DataSchemaType',@PossibleValue='Logical',@Series=1,@Description='Logical',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DataSchemaType',@PossibleValue='Physical',@Series=2,@Description='Physical',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DataSchemaType',@PossibleValue='PhysicalDB',@Series=3,@Description='Physical Database',@IsActive=1
EXEC db_AddDefinitions @Name = 'DataViewType',@IsActive = 1
EXEC db_AddPossibleValues @DomainDefinition='DataViewType',@PossibleValue='Logical',@Series=1,@Description='Logical Data View',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DataViewType',@PossibleValue='Physical',@Series=2,@Description='Physical Data View',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DataViewType',@PossibleValue='E_Form',@Series=3,@Description='Electronic Form',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DataViewType',@PossibleValue='Report',@Series=3,@Description='Report',@IsActive=0
EXEC db_AddPossibleValues @DomainDefinition='DataViewType',@PossibleValue='E_Screen',@Series=4,@Description='Electronic Screen',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DataViewType',@PossibleValue='E_Report',@Series=5,@Description='Electronic Report',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DataViewType',@PossibleValue='E_Letter',@Series=6,@Description='Electronic Letter',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DataViewType',@PossibleValue='E_Fax',@Series=7,@Description='Electronic Fax',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DataViewType',@PossibleValue='E_File',@Series=8,@Description='Electronic File',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DataViewType',@PossibleValue='E_Mess',@Series=9,@Description='Electronic Message',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DataViewType',@PossibleValue='E_Doc',@Series=10,@Description='Electronic Doc.',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DataViewType',@PossibleValue='E_MM',@Series=11,@Description='Electronic MMedia',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DataViewType',@PossibleValue='P_Form',@Series=12,@Description='Paper  Form',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DataViewType',@PossibleValue='P_Rep',@Series=13,@Description='Paper  Report',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DataViewType',@PossibleValue='P_Letter',@Series=14,@Description='Paper  Letter',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DataViewType',@PossibleValue='P_Fax',@Series=15,@Description='Paper  Fax',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DataViewType',@PossibleValue='P_Mess',@Series=16,@Description='Paper  Message',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DataViewType',@PossibleValue='P_Doc',@Series=17,@Description='Paper  Doc.',@IsActive=1
EXEC db_AddDefinitions @Name = 'DomainAttributeType',@IsActive = 1
EXEC db_AddPossibleValues @DomainDefinition='DomainAttributeType',@PossibleValue='Text',@Series=1,@Description='Text',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DomainAttributeType',@PossibleValue='Number',@Series=2,@Description='Number',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DomainAttributeType',@PossibleValue='Memo',@Series=3,@Description='Memo',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DomainAttributeType',@PossibleValue='Boolean',@Series=4,@Description='Boolean',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DomainAttributeType',@PossibleValue='DateT',@Series=5,@Description='Date Time',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DomainAttributeType',@PossibleValue='Currency',@Series=6,@Description='Currency',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DomainAttributeType',@PossibleValue='Percent',@Series=7,@Description='Percentage',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DomainAttributeType',@PossibleValue='Artefact',@Series=8,@Description='Artefact Object such as Document or Image',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DomainAttributeType',@PossibleValue='Calculation',@Series=9,@Description='Calculation',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DomainAttributeType',@PossibleValue='List',@Series=10,@Description='Lookup List',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DomainAttributeType',@PossibleValue='URI',@Series=11,@Description='Uniform Resource Identifier such as a URL',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='DomainAttributeType',@PossibleValue='General',@Series=12,@Description='General',@IsActive=1
EXEC db_AddDefinitions @Name = 'EnvironmentCategory',@IsActive = 1
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='Political',@Series=1,@Description='Political',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='Legislative',@Series=2,@Description='Legislative',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='Socio',@Series=2,@Description='Socio',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='SocioPol',@Series=2,@Description='Socio_Political',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='Geo',@Series=3,@Description='Geographical',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='Geographical',@Series=3,@Description='Geographical',@IsActive=0
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='Econo',@Series=4,@Description='Economical',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='Economic',@Series=4,@Description='Economic',@IsActive=0
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='Cultural',@Series=5,@Description='Cultural_Ethics',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='Tech',@Series=6,@Description='Technology',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='Technology',@Series=6,@Description='Technology',@IsActive=0
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='Natural',@Series=7,@Description='Natural_Resources',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='Natural_Resources',@Series=7,@Description='Natural_Resources',@IsActive=0
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='Industrial',@Series=8,@Description='Industrial',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='Socio_Economical',@Series=9,@Description='Socio_Economical',@IsActive=0
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='SocioE',@Series=9,@Description='Socio_Economical',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='Directional',@Series=10,@Description='Directional',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='Demograph',@Series=11,@Description='Demographics',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='Demographics',@Series=11,@Description='Demographics',@IsActive=0
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='Customer',@Series=12,@Description='Customer',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='Stakeholders',@Series=12,@Description='Stakeholders',@IsActive=0
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='Supplier',@Series=13,@Description='Supplier',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='Market',@Series=14,@Description='Market',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='Competitor',@Series=15,@Description='Competitor',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='Contributor',@Series=16,@Description='Contributors',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='Mng',@Series=17,@Description='Management',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='Unions',@Series=18,@Description='Unions',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='Employee',@Series=19,@Description='Employee',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='Shareholder',@Series=20,@Description='Shareholder',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='EnvironmentCategory',@PossibleValue='OtherSH',@Series=21,@Description='Other Stakeholders',@IsActive=1
EXEC db_AddDefinitions @Name = 'ExecutionIndicator',@IsActive = 1
EXEC db_AddPossibleValues @DomainDefinition='ExecutionIndicator',@PossibleValue='Manual',@Series=1,@Description='Manual (No System)',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ExecutionIndicator',@PossibleValue='Auto',@Series=2,@Description='Fully Automated',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ExecutionIndicator',@PossibleValue='SemiAuto',@Series=3,@Description='Semi Automated (User Involved)',@IsActive=1
EXEC db_AddDefinitions @Name = 'GapType',@IsActive = 1
EXEC db_AddPossibleValues @DomainDefinition='GapType',@PossibleValue='As_Is',@Series=1,@Description='No_Change to As-is',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='GapType',@PossibleValue='To_Be',@Series=2,@Description='New To-be Component',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='GapType',@PossibleValue='Fix',@Series=3,@Description='Fix As-is',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='GapType',@PossibleValue='Obsolete',@Series=4,@Description='Decommission As-is (Obsolete)',@IsActive=1
EXEC db_AddDefinitions @Name = 'InferenceRule',@IsActive = 1
EXEC db_AddPossibleValues @DomainDefinition='InferenceRule',@PossibleValue='Trans',@Series=1,@Description='Transivity',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='InferenceRule',@PossibleValue='Reflex',@Series=2,@Description='Reflexive',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='InferenceRule',@PossibleValue='Normal',@Series=3,@Description='Ordinary',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='InferenceRule',@PossibleValue='Aug',@Series=4,@Description='Augmented',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='InferenceRule',@PossibleValue='Decomp',@Series=5,@Description='Decomposition',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='InferenceRule',@PossibleValue='Union',@Series=6,@Description='Union',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='InferenceRule',@PossibleValue='PseudoT',@Series=7,@Description='Pseudo Transitivity',@IsActive=1
EXEC db_AddDefinitions @Name = 'JobType',@IsActive = 1
EXEC db_AddPossibleValues @DomainDefinition='JobType',@PossibleValue='Strategic',@Series=1,@Description='Strategic',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='JobType',@PossibleValue='Tactical',@Series=2,@Description='Tactical',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='JobType',@PossibleValue='Operat',@Series=3,@Description='Operational',@IsActive=1
EXEC db_AddDefinitions @Name = 'GovernanceMechType',@IsActive = 1
EXEC db_AddPossibleValues @DomainDefinition='GovernanceMechType',@PossibleValue='Law',@Series=1,@Description='Case, Common or Statutory Law',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='GovernanceMechType',@PossibleValue='Policy',@Series=2,@Description='Policy',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='GovernanceMechType',@PossibleValue='Directive',@Series=3,@Description='Directive',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='GovernanceMechType',@PossibleValue='SOP',@Series=4,@Description='Standard Operating Procedure',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='GovernanceMechType',@PossibleValue='JobDesc',@Series=5,@Description='Job Description',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='GovernanceMechType',@PossibleValue='Measure',@Series=6,@Description='Measurement Areas such as Performance, Availability, Cost Effeciciency, etc. and the related Indicators.',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='GovernanceMechType',@PossibleValue='Plan',@Series=6,@Description='Plan (e.g MBSP)',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='GovernanceMechType',@PossibleValue='Agreement',@Series=7,@Description='Agreement  such as an SLA or OLA that could be in the form of a contract.',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='GovernanceMechType',@PossibleValue='Standard',@Series=8,@Description='Standard (e.g ISO, Basel, SOX, ITIL, COBIT)',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='GovernanceMechType',@PossibleValue='Principle',@Series=9,@Description='Principle proven to be effective over time.',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='GovernanceMechType',@PossibleValue='Pattern',@Series=10,@Description='Pattern or standard framework.',@IsActive=1
EXEC db_AddDefinitions @Name = 'MMType',@IsActive = 1
EXEC db_AddPossibleValues @DomainDefinition='MMType',@PossibleValue='Video',@Series=1,@Description='Video',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='MMType',@PossibleValue='Photo',@Series=2,@Description='Photo',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='MMType',@PossibleValue='Conference',@Series=3,@Description='Conferencing',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='MMType',@PossibleValue='PaperImage',@Series=4,@Description='Paper Image',@IsActive=1
EXEC db_AddDefinitions @Name = 'NetworkCompType',@IsActive = 1
EXEC db_AddPossibleValues @DomainDefinition='NetworkCompType',@PossibleValue='Network',@Series=7,@Description='Network (WAN, LAN, VPN, etc)',@IsActive=1
EXEC db_AddDefinitions @Name = 'NetworkType',@IsActive = 1
EXEC db_AddPossibleValues @DomainDefinition='NetworkType',@PossibleValue='WAN',@Series=1,@Description='WAN',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='NetworkType',@PossibleValue='LAN',@Series=2,@Description='LAN',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='NetworkType',@PossibleValue='VPN',@Series=3,@Description='VPN',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='NetworkType',@PossibleValue='Web',@Series=4,@Description='Web',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='NetworkType',@PossibleValue='Wireless',@Series=5,@Description='Wireless',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='NetworkType',@PossibleValue='Dialup',@Series=6,@Description='Dial-up',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='NetworkType',@PossibleValue='VLAN',@Series=7,@Description='VLAN',@IsActive=1
EXEC db_AddDefinitions @Name = 'ObjectType',@IsActive = 1
EXEC db_AddPossibleValues @DomainDefinition='ObjectType',@PossibleValue='BusinessSys',@Series=1,@Description='BusinessSystem',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ObjectType',@PossibleValue='BusService',@Series=2,@Description='BusinessService',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ObjectType',@PossibleValue='ITService',@Series=3,@Description='ITService',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ObjectType',@PossibleValue='Product',@Series=4,@Description='Product',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ObjectType',@PossibleValue='ExternalE',@Series=5,@Description='ExternalEntity',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ObjectType',@PossibleValue='TargetE',@Series=6,@Description='TargetEntity',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ObjectType',@PossibleValue='OrgUnit',@Series=7,@Description='OrginizationUnit',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ObjectType',@PossibleValue='JobPosition',@Series=8,@Description='JobPosition',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ObjectType',@PossibleValue='Employee',@Series=9,@Description='Employee',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ObjectType',@PossibleValue='Role',@Series=10,@Description='Role',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ObjectType',@PossibleValue='EArtefact',@Series=11,@Description='EDataArtefact',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ObjectType',@PossibleValue='PArtefect',@Series=12,@Description='PDataArtefact',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ObjectType',@PossibleValue='Software',@Series=13,@Description='Software',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ObjectType',@PossibleValue='Appl',@Series=14,@Description='Application',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ObjectType',@PossibleValue='PC',@Series=15,@Description='PC',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ObjectType',@PossibleValue='Server',@Series=16,@Description='Server',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ObjectType',@PossibleValue='SystemC',@Series=17,@Description='SystemComp',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ObjectType',@PossibleValue='NetworkC',@Series=18,@Description='NetworkComp',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ObjectType',@PossibleValue='StorageC',@Series=19,@Description='StorageComp',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ObjectType',@PossibleValue='PeripheralC',@Series=20,@Description='PeripheralComp',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ObjectType',@PossibleValue='Facility',@Series=21,@Description='Facility',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ObjectType',@PossibleValue='Device',@Series=22,@Description='Device',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ObjectType',@PossibleValue='Database',@Series=23,@Description='Database (Data Schema)',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ObjectType',@PossibleValue='DataSchema',@Series=24,@Description='DataSchema',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ObjectType',@PossibleValue='DataView',@Series=25,@Description='DataView',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ObjectType',@PossibleValue='DataEntity',@Series=26,@Description='DataEntity',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ObjectType',@PossibleValue='DataTable',@Series=27,@Description='DataTable',@IsActive=1
EXEC db_AddDefinitions @Name = 'OrganizationalUnitType',@IsActive = 1
EXEC db_AddPossibleValues @DomainDefinition='OrganizationalUnitType',@PossibleValue='Enterprise',@Series=1,@Description='Enterprise',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='OrganizationalUnitType',@PossibleValue='Division',@Series=2,@Description='Division',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='OrganizationalUnitType',@PossibleValue='Unit',@Series=3,@Description='Unit',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='OrganizationalUnitType',@PossibleValue='Dept',@Series=4,@Description='Department',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='OrganizationalUnitType',@PossibleValue='Section',@Series=5,@Description='Section',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='OrganizationalUnitType',@PossibleValue='Area',@Series=6,@Description='Supervising Area',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='OrganizationalUnitType',@PossibleValue='Project',@Series=7,@Description='Project',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='OrganizationalUnitType',@PossibleValue='Committee',@Series=8,@Description='Committee',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='OrganizationalUnitType',@PossibleValue='Forum',@Series=9,@Description='Forum',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='OrganizationalUnitType',@PossibleValue='Community',@Series=11,@Description='Community',@IsActive=1
EXEC db_AddDefinitions @Name = 'PeripheralType',@IsActive = 1
EXEC db_AddPossibleValues @DomainDefinition='PeripheralType',@PossibleValue='MultiDev',@Series=2,@Description='Multi Func. Device',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='PeripheralType',@PossibleValue='MultiDevice',@Series=2,@Description='Multi Func. Device',@IsActive=0
EXEC db_AddPossibleValues @DomainDefinition='PeripheralType',@PossibleValue='Other_Hand',@Series=9,@Description='Other Handheld Device',@IsActive=1
EXEC db_AddDefinitions @Name = 'SelectorAttributeType',@IsActive = 1
EXEC db_AddPossibleValues @DomainDefinition='SelectorAttributeType',@PossibleValue='MutuallyExclusive',@Series=1,@Description='The subset is mutually exclusive',@IsActive=0
EXEC db_AddPossibleValues @DomainDefinition='SelectorAttributeType',@PossibleValue='X_OR',@Series=1,@Description='Mutually exclusive',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='SelectorAttributeType',@PossibleValue='OR',@Series=2,@Description='The subset is mutually inclusive',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='SelectorAttributeType',@PossibleValue='MutuallyInclusive',@Series=2,@Description='Mutually inclusive',@IsActive=0
EXEC db_AddPossibleValues @DomainDefinition='SelectorAttributeType',@PossibleValue='AND',@Series=3,@Description='AND',@IsActive=0
EXEC db_AddDefinitions @Name = 'ServerType',@IsActive = 1
EXEC db_AddPossibleValues @DomainDefinition='ServerType',@PossibleValue='Mail',@Series=1,@Description='Mail',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ServerType',@PossibleValue='Appl',@Series=2,@Description='Application',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ServerType',@PossibleValue='FilePrint',@Series=3,@Description='FileandPrint',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ServerType',@PossibleValue='DomainC',@Series=4,@Description='DomainController',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ServerType',@PossibleValue='Interface',@Series=5,@Description='Interface',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ServerType',@PossibleValue='Firewall',@Series=6,@Description='Firewall',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ServerType',@PossibleValue='Web',@Series=7,@Description='Web',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='ServerType',@PossibleValue='Multi',@Series=8,@Description='Multi_Service',@IsActive=1
EXEC db_AddDefinitions @Name = 'SoftwareType',@IsActive = 1
EXEC db_AddPossibleValues @DomainDefinition='SoftwareType',@PossibleValue='Patch',@Series=1,@Description='Patch',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='SoftwareType',@PossibleValue='Application',@Series=2,@Description='Application',@IsActive=0
EXEC db_AddPossibleValues @DomainDefinition='SoftwareType',@PossibleValue='Bespoke',@Series=2,@Description='Bespoke Application',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='SoftwareType',@PossibleValue='Package',@Series=3,@Description='Off the Shelf Package',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='SoftwareType',@PossibleValue='DBMS',@Series=3,@Description='Database Management System',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='SoftwareType',@PossibleValue='Op_System',@Series=4,@Description='O/S',@IsActive=0
EXEC db_AddPossibleValues @DomainDefinition='SoftwareType',@PossibleValue='OS',@Series=4,@Description='Operating System',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='SoftwareType',@PossibleValue='Service_Pack',@Series=5,@Description='Service Pack',@IsActive=0
EXEC db_AddPossibleValues @DomainDefinition='SoftwareType',@PossibleValue='ServicePack',@Series=5,@Description='Service Pack',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='SoftwareType',@PossibleValue='Script',@Series=6,@Description='Script',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='SoftwareType',@PossibleValue='Database',@Series=7,@Description='Database',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='SoftwareType',@PossibleValue='Firewall',@Series=8,@Description='Firewall',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='SoftwareType',@PossibleValue='Suite',@Series=9,@Description='Suite',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='SoftwareType',@PossibleValue='App_Component',@Series=10,@Description='Appl Comp',@IsActive=0
EXEC db_AddPossibleValues @DomainDefinition='SoftwareType',@PossibleValue='AppMod',@Series=10,@Description='Application Module',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='SoftwareType',@PossibleValue='Exec',@Series=11,@Description='Executable',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='SoftwareType',@PossibleValue='Executable',@Series=11,@Description='Executable',@IsActive=0
EXEC db_AddPossibleValues @DomainDefinition='SoftwareType',@PossibleValue='Driver',@Series=12,@Description='Driver',@IsActive=1
EXEC db_AddDefinitions @Name = 'SystemCompType',@IsActive = 1
EXEC db_AddPossibleValues @DomainDefinition='SystemCompType',@PossibleValue='PhysicalServer',@Series=2,@Description='Physical Server',@IsActive=0
EXEC db_AddPossibleValues @DomainDefinition='SystemCompType',@PossibleValue='PhysServer',@Series=2,@Description='Physical Server',@IsActive=1
EXEC db_AddDefinitions @Name = 'TimeUnitType',@IsActive = 1
EXEC db_AddPossibleValues @DomainDefinition='TimeUnitType',@PossibleValue='Century',@Series=1,@Description='Century',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='TimeUnitType',@PossibleValue='Year',@Series=2,@Description='Year',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='TimeUnitType',@PossibleValue='Season',@Series=3,@Description='Season',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='TimeUnitType',@PossibleValue='Quarter',@Series=4,@Description='Quarter',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='TimeUnitType',@PossibleValue='Month',@Series=5,@Description='Month',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='TimeUnitType',@PossibleValue='Week',@Series=6,@Description='Week',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='TimeUnitType',@PossibleValue='Day',@Series=7,@Description='Day',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='TimeUnitType',@PossibleValue='Hour',@Series=8,@Description='Hour',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='TimeUnitType',@PossibleValue='Minute',@Series=9,@Description='Minute',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='TimeUnitType',@PossibleValue='Second',@Series=10,@Description='Second',@IsActive=1
EXEC db_AddDefinitions @Name = 'TimeSchemeType',@IsActive = 1
EXEC db_AddPossibleValues @DomainDefinition='TimeSchemeType',@PossibleValue='Project',@Series=1,@Description='Project',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='TimeSchemeType',@PossibleValue='Phase',@Series=2,@Description='Phase',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='TimeSchemeType',@PossibleValue='Milestone',@Series=3,@Description='Milestone',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='TimeSchemeType',@PossibleValue='Shift',@Series=4,@Description='Shift',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='TimeSchemeType',@PossibleValue='Calander',@Series=5,@Description='Calander',@IsActive=1
EXEC db_AddDefinitions @Name = 'UserInterfaceType',@IsActive = 1
EXEC db_AddPossibleValues @DomainDefinition='UserInterfaceType',@PossibleValue='Browser',@Series=1,@Description='Browser',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='UserInterfaceType',@PossibleValue='Text',@Series=2,@Description='Text',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='UserInterfaceType',@PossibleValue='GUI',@Series=3,@Description='GUI',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='UserInterfaceType',@PossibleValue='Pervasive',@Series=4,@Description='Pervasive',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='UserInterfaceType',@PossibleValue='Voice',@Series=5,@Description='VoiceCommand',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='UserInterfaceType',@PossibleValue='Multi',@Series=6,@Description='Multi User Interface Compatibility',@IsActive=1
EXEC db_AddDefinitions @Name = 'RationaleType',@IsActive = 1
EXEC db_AddPossibleValues @DomainDefinition='RationaleType',@PossibleValue='GeneralNote',@Series=1,@Description='General textual note',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='RationaleType',@PossibleValue='GeneralDesc',@Series=2,@Description='General textual explanation or definition',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='RationaleType',@PossibleValue='BusinessRule',@Series=3,@Description='Textutal description of a business rule',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='RationaleType',@PossibleValue='BusinessLogic',@Series=4,@Description='Textual description of logic such as a formula',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='RationaleType',@PossibleValue='Philosophy',@Series=5,@Description='Textual description of a Philosophy',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='RationaleType',@PossibleValue='Principle',@Series=6,@Description='Textual description of a Principle - Heuristicly proven to be a good practise (not an CSF)',@IsActive=1
EXEC db_AddPossibleValues @DomainDefinition='RationaleType',@PossibleValue='TimeDesc',@Series=7,@Description='Textual description of Time Schedule (e.g. Financial Year(xx/xx/Monday/05h00)',@IsActive=1


EXEC db_AddArtifacts @parentClass='Attribute', @childclass='Attribute', @Association='Mapping', @ArtifactClass='Logic', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Attribute', @childclass='DataColumn', @Association='Mapping', @ArtifactClass='Logic', @IsActive = 1
EXEC db_AddArtifacts @parentClass='DataColumn', @childclass='Attribute', @Association='Mapping', @ArtifactClass='Logic', @IsActive = 1
EXEC db_AddArtifacts @parentClass='DataColumn', @childclass='DataColumn', @Association='Mapping', @ArtifactClass='Logic', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Conditional', @childclass='Process', @Association='DynamicFlow', @ArtifactClass='ConditionalDescription', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Conditional', @childclass='Activity', @Association='DynamicFlow', @ArtifactClass='ConditionalDescription', @IsActive = 1
EXEC db_AddArtifacts @parentClass='DataSchema', @childclass='DataSchema', @Association='Dependencies', @ArtifactClass='Rationale', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Entity', @childclass='Entity', @Association='Dependencies', @ArtifactClass='Rationale', @IsActive = 1


EXEC db_AddArtifacts @parentClass='Conditional', @childclass='Process', @Association='DynamicFlow', @ArtifactClass='FlowDescription', @IsActive = 0
EXEC db_AddArtifacts @parentClass='Conditional', @childclass='Activity', @Association='DynamicFlow', @ArtifactClass='FlowDescription', @IsActive = 0
EXEC db_AddArtifacts @parentClass='MutuallyExclusiveIndicator', @childclass='DataSchema', @Association='SubSetOf', @ArtifactClass='SelectorAttribute', @IsActive = 1
EXEC db_AddArtifacts @parentClass='DataSchema', @childclass='DataSchema', @Association='DynamicFlow', @ArtifactClass='FlowDescription', @IsActive = 1

EXEC db_AddArtifacts @parentClass='Location', @childclass='Location', @Association='Mapping', @ArtifactClass='LocationAssociation', @IsActive = 1

EXEC db_AddArtifacts @parentClass='OrganizationalUnit', @childclass='Object', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='OrganizationalUnit', @childclass='Software', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='OrganizationalUnit', @childclass='StorageComponent', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='OrganizationalUnit', @childclass='NetworkComponent', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='OrganizationalUnit', @childclass='SystemComponent', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='OrganizationalUnit', @childclass='Peripheral', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='OrganizationalUnit', @childclass='Function', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='OrganizationalUnit', @childclass='Process', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='OrganizationalUnit', @childclass='Activity', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='OrganizationalUnit', @childclass='Conditional', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='OrganizationalUnit', @childclass='DataSchema', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='OrganizationalUnit', @childclass='DataView', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='OrganizationalUnit', @childclass='Entity', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='OrganizationalUnit', @childclass='DataTable', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1

EXEC db_AddArtifacts @parentClass='Object', @childclass='Object', @Association='DynamicFlow', @ArtifactClass='Activity', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Object', @childclass='Object', @Association='DynamicFlow', @ArtifactClass='Attribute', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Object', @childclass='Object', @Association='DynamicFlow', @ArtifactClass='DataColumn', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Object', @childclass='Object', @Association='DynamicFlow', @ArtifactClass='DataSchema', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Object', @childclass='Object', @Association='DynamicFlow', @ArtifactClass='DataTable', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Object', @childclass='Object', @Association='DynamicFlow', @ArtifactClass='DataView', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Object', @childclass='Object', @Association='DynamicFlow', @ArtifactClass='GovernanceMechanism', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Object', @childclass='Object', @Association='DynamicFlow', @ArtifactClass='Object', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Object', @childclass='Object', @Association='DynamicFlow', @ArtifactClass='Process', @IsActive = 1

EXEC db_AddArtifacts @parentClass='Software', @childclass='OrganizationalUnit', @Association='Mapping', @ArtifactClass='Responsibility', @IsActive = 0

EXEC db_AddArtifacts @parentClass='StorageComponent', @childclass='StorageComponent', @Association='ConnectedTo', @ArtifactClass='DataColumn', @IsActive = 0
EXEC db_AddArtifacts @parentClass='StorageComponent', @childclass='StorageComponent', @Association='ConnectedTo', @ArtifactClass='DataSchema', @IsActive = 0
EXEC db_AddArtifacts @parentClass='StorageComponent', @childclass='StorageComponent', @Association='ConnectedTo', @ArtifactClass='DataTable', @IsActive = 0
EXEC db_AddArtifacts @parentClass='StorageComponent', @childclass='StorageComponent', @Association='ConnectedTo', @ArtifactClass='DataView', @IsActive = 0
EXEC db_AddArtifacts @parentClass='StorageComponent', @childclass='StorageComponent', @Association='ConnectedTo', @ArtifactClass='Rationale', @IsActive = 0

EXEC db_AddArtifacts @parentClass='NetworkComponent', @childclass='NetworkComponent', @Association='ConnectedTo', @ArtifactClass='Rationale', @IsActive = 1

EXEC db_AddArtifacts @parentClass='GovernanceMechanism', @childclass='GovernanceMechanism', @Association='FunctionalDependency', @ArtifactClass='Conditional', @IsActive = 1

EXEC db_AddArtifacts @parentClass='Software', @childclass='Software', @Association='DynamicFlow', @ArtifactClass='Activity', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Software', @childclass='Software', @Association='DynamicFlow', @ArtifactClass='Attribute', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Software', @childclass='Software', @Association='DynamicFlow', @ArtifactClass='DataColumn', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Software', @childclass='Software', @Association='DynamicFlow', @ArtifactClass='DataSchema', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Software', @childclass='Software', @Association='DynamicFlow', @ArtifactClass='DataTable', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Software', @childclass='Software', @Association='DynamicFlow', @ArtifactClass='DataView', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Software', @childclass='Software', @Association='DynamicFlow', @ArtifactClass='FlowDescription', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Software', @childclass='Software', @Association='DynamicFlow', @ArtifactClass='Job', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Software', @childclass='Software', @Association='DynamicFlow', @ArtifactClass='JobPosition', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Software', @childclass='Software', @Association='DynamicFlow', @ArtifactClass='OrganizationalUnit', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Software', @childclass='Software', @Association='DynamicFlow', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Software', @childclass='Software', @Association='DynamicFlow', @ArtifactClass='Process', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Software', @childclass='Software', @Association='DynamicFlow', @ArtifactClass='GovernanceMechanism', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Software', @childclass='Software', @Association='DynamicFlow', @ArtifactClass='Rationale', @IsActive = 1

EXEC db_AddArtifacts @parentClass='TimeUnit', @childclass='TimeUnit', @Association='Mapping', @ArtifactClass='Rationale', @IsActive = 1

EXEC db_AddArtifacts @parentClass='JobPosition', @childclass='Object', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='JobPosition', @childclass='Software', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='JobPosition', @childclass='SystemComponent', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='JobPosition', @childclass='NetworkComponent', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='JobPosition', @childclass='StorageComponent', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='JobPosition', @childclass='Peripheral', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='JobPosition', @childclass='Function', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='JobPosition', @childclass='Process', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='JobPosition', @childclass='Activity', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='JobPosition', @childclass='Conditional', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='JobPosition', @childclass='DataSchema', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='JobPosition', @childclass='DataView', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='JobPosition', @childclass='Entity', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='JobPosition', @childclass='DataTable', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1

EXEC db_AddArtifacts @parentClass='Job', @childclass='Object', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Job', @childclass='Software', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Job', @childclass='SystemComponent', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Job', @childclass='NetworkComponent', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Job', @childclass='StorageComponent', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Job', @childclass='Peripheral', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Job', @childclass='Function', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Job', @childclass='Process', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Job', @childclass='Activity', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Job', @childclass='Conditional', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Job', @childclass='DataSchema', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Job', @childclass='DataView', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Job', @childclass='Entity', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Job', @childclass='DataTable', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Employee', @childclass='Object', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Employee', @childclass='SystemComponent', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Employee', @childclass='StorageComponent', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Employee', @childclass='DataSchema', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Employee', @childclass='NetworkComponent', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Employee', @childclass='Peripheral', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Employee', @childclass='Software', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Employee', @childclass='Function', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Employee', @childclass='Process', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Employee', @childclass='Activity', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Employee', @childclass='Conditional', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Employee', @childclass='DataView', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Employee', @childclass='Entity', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1
EXEC db_AddArtifacts @parentClass='Employee', @childclass='DataTable', @Association='Mapping', @ArtifactClass='Role', @IsActive = 1

--Complete IsActive
UPDATE    Class
SET              IsActive = 1
WHERE     (IsActive IS NULL)
UPDATE    ClassAssociation
SET              IsActive = 1
WHERE     (IsActive IS NULL)
UPDATE    Field
SET              IsActive = 1
WHERE     (IsActive IS NULL)
UPDATE    DomainDefinition
SET              IsActive = 1
WHERE     (IsActive IS NULL)
UPDATE    DomainDefinitionPossibleValue
SET              IsActive = 1
WHERE     (IsActive IS NULL)
UPDATE    AllowedArtifact
SET              IsActive = 1
WHERE     (IsActive IS NULL)

----EXTRA

UPDATE [ClassAssociation] SET IsActive = 0 WHERE ParentClass = 'Entity' AND ChildClass = 'Function' AND AssociationTypeID = 19
UPDATE [DomainDefinitionPossibleValue] SET IsActive = 0 WHERE DomainDefinitionID = 22 AND PossibleValue = 'Geographical'

---Object Collection
/*
EXEC db_AddClasses @Class ='ObjectCollection',@DescriptionCode='',@Category='',@IsActive = 1
EXEC db_AddFields @Class='ObjectCollection',@Name='Name',@DataType='System.String',@Category='',@Description='Name',@IsUnique=0,@SortOrder=1,@IsActive = 1
EXEC db_AddFields @Class='ObjectCollection',@Name='CollectionType',@DataType='System.String',@Category='',@Description='Collection Type',@IsUnique=0,@SortOrder=2,@IsActive = 1
EXEC db_AddFields @Class='ObjectCollection',@Name='GapType',@DataType='GapType',@Category='',@Description='Gap Type(As-is vs. To-be)',@IsUnique=0,@SortOrder=90,@IsActive = 1
EXEC db_AddFields @Class='ObjectCollection',@Name='CustomField1',@DataType='System.String',@Category='',@Description='Custom Field 1',@IsUnique=0,@SortOrder=98,@IsActive = 1
EXEC db_AddFields @Class='ObjectCollection',@Name='CustomField2',@DataType='System.String',@Category='',@Description='Custom Field 2',@IsUnique=0,@SortOrder=99,@IsActive = 1
EXEC db_AddFields @Class='ObjectCollection',@Name='CustomField3',@DataType='System.String',@Category='',@Description='Custom Field 3',@IsUnique=0,@SortOrder=100,@IsActive = 1
EXEC db_AddClassAssociations @parentClass='ObjectCollection',@childClass='Object',@Association='Mapping',@Caption='',@IsDefault=0,@IsActive=1
*/

-- ADD RATIONALE FOR EACH ASSoCIATIONTYPE

DECLARE @CAID int
DECLARE AssociationCursor  CURSOR  FOR 
SELECT CAID from ClassAssociation

OPEN  AssociationCursor  

--  Perform  the  first  fetch. 
FETCH  NEXT  FROM  AssociationCursor  
INTO @CAID

WHILE  @@FETCH_STATUS  =  0 
BEGIN 
	IF NOT EXISTS(SELECT CLASS FROM ALLOWEDARTIFACT WHERE CAID=@CAID AND CLASS='Rationale')
BEGIN
	INSERT INTO ALLOWEDARTIFACT(CAID,CLASS,ISACTIVE) VALUES(@CAID,'Rationale',1)
END
    FETCH  NEXT  FROM  AssociationCursor INTO @CAID
END  

CLOSE  AssociationCursor 
DEALLOCATE  AssociationCursor 

DECLARE @CLASSNAME VARCHAR(100)
DECLARE ClassCursor  CURSOR  FOR 
SELECT Name from Class

OPEN  ClassCursor  

--  Perform  the  first  fetch. 
FETCH  NEXT  FROM  ClassCursor  
INTO @CLASSNAME

WHILE  @@FETCH_STATUS  =  0 
BEGIN 
	INSERT INTO CLASSASSOCIATION(PARENTCLASS,CHILDCLASS,ASSOCIATIONTYPEID,ISACTIVE)
	VALUES(@CLASSNAME,'Rationale',4,1)
	
	IF NOT EXISTS(SELECT NAME FROM FIELD WHERE CLASS=@CLASSNAME AND NAME='CustomField1')
	BEGIN
		INSERT INTO FIELD(NAME,CLASS,DataType,Category,Description,IsUnique,SortOrder,IsActive)
			VALUES('CustomField1',@CLASSNAME,'System.String','Custom','CustomField1',1,97,1)
	END

	IF NOT EXISTS(SELECT NAME FROM FIELD WHERE CLASS=@CLASSNAME AND NAME='CustomField2')
	BEGIN
		INSERT INTO FIELD(NAME,CLASS,DataType,Category,Description,IsUnique,SortOrder,IsActive)
			VALUES('CustomField2',@CLASSNAME,'System.String','Custom','CustomField2',1,98,1)
	END


	IF NOT EXISTS(SELECT NAME FROM FIELD WHERE CLASS=@CLASSNAME AND NAME='CustomField3')
	BEGIN
		INSERT INTO FIELD(NAME,CLASS,DataType,Category,Description,IsUnique,SortOrder,IsActive)
			VALUES('CustomField3',@CLASSNAME,'System.String','Custom','CustomField3',1,99,1)
	END

	IF NOT EXISTS(SELECT NAME FROM FIELD WHERE CLASS=@CLASSNAME AND NAME='GapType')
	BEGIN
		INSERT INTO FIELD(NAME,CLASS,DataType,Category,Description,IsUnique,SortOrder,IsActive)
			VALUES('GapType',@CLASSNAME,'GapType','General','Gap Type(As-is vs. To-be)',1,96,1)
	END

    FETCH  NEXT  FROM  ClassCursor INTO @CLASSNAME
END  

CLOSE  ClassCursor 
DEALLOCATE  ClassCursor 

SELECT @CAID = caid from classassociation where 
parentClass='Function' and childclass='OrganizationalUnit'
AND ASSOCIATIONTYPEID=4

IF NOT EXISTS(SELECT CLASS FROM ALLOWEDARTIFACT WHERE CLASS='Role' AnD CAID=@CAID)
	INSERT INTO ALLOWEDARTIFACT(CAID,CLASS) VALUES(@CAID,'Role')

GO
CREATE VIEW ORGUNITFUNCTIONROLE 
 as 
 SELECT dbo.METAView_Function_Listing.Name AS [Function], dbo.METAView_OrganizationalUnit_Listing.Name AS OrgUnit,  
 dbo.METAView_Role_Listing.Name AS Role   FROM dbo.Artifact INNER JOIN  
 dbo.METAView_Function_Listing ON dbo.Artifact.ObjectID = dbo.METAView_Function_Listing.pkid AND  
 dbo.Artifact.ObjectMachine = dbo.METAView_Function_Listing.Machine INNER JOIN 
 dbo.METAView_OrganizationalUnit_Listing ON dbo.Artifact.ChildObjectID = dbo.METAView_OrganizationalUnit_Listing.pkid AND  
 dbo.Artifact.ChildObjectMachine = dbo.METAView_OrganizationalUnit_Listing.Machine INNER JOIN 
 dbo.METAView_Role_Listing ON dbo.Artifact.ArtefactMachine = dbo.METAView_Role_Listing.Machine AND  
 dbo.Artifact.ArtifactObjectID = dbo.METAView_Role_Listing.pkid 
 UNION 
 SELECT dbo.METAView_Function_Listing.Name AS [Function], dbo.METAView_OrganizationalUnit_Listing.Name AS OrgUnit,  
 dbo.METAView_Role_Listing.Name AS Role 
 FROM dbo.Artifact INNER JOIN 
 dbo.METAView_Role_Listing ON dbo.Artifact.ArtefactMachine = dbo.METAView_Role_Listing.Machine AND  
 dbo.Artifact.ArtifactObjectID = dbo.METAView_Role_Listing.pkid INNER JOIN 
 dbo.METAView_Function_Listing ON dbo.Artifact.ChildObjectMachine = dbo.METAView_Function_Listing.Machine AND  
 dbo.Artifact.ChildObjectID = dbo.METAView_Function_Listing.pkid INNER JOIN 
 dbo.METAView_OrganizationalUnit_Listing ON dbo.Artifact.ObjectID = dbo.METAView_OrganizationalUnit_Listing.pkid AND  
 dbo.Artifact.ObjectMachine = dbo.METAView_OrganizationalUnit_Listing.Machine 


/* DROP ALL PROCEDURES */

IF NOT EXISTS(SELECT PKID FROM VCSTATUS WHERE NAME='PCI_Revoked')
BEGIN
	INSERT INTO VCSTATUS(PKID,NAME) values(9,'PCI_Revoked')
END



/****** Object:  StoredProcedure [dbo].[META_AddArtifact]    Script Date: 09/22/2009 12:33:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_AddArtifact]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_AddArtifact]
GO
/****** Object:  StoredProcedure [dbo].[META_AddObjectAssociation]    Script Date: 09/22/2009 12:33:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_AddObjectAssociation]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_AddObjectAssociation]
GO
/****** Object:  StoredProcedure [dbo].[META_AddQuickAssociation]    Script Date: 09/22/2009 12:33:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_AddQuickAssociation]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_AddQuickAssociation]
GO
/****** Object:  StoredProcedure [dbo].[META_CalculateCriticalities]    Script Date: 09/22/2009 12:33:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_CalculateCriticalities]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_CalculateCriticalities]
GO
/****** Object:  StoredProcedure [dbo].[META_ClearArtifacts]    Script Date: 09/22/2009 12:33:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_ClearArtifacts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_ClearArtifacts]
GO
/****** Object:  StoredProcedure [dbo].[META_CreateObject]    Script Date: 09/22/2009 12:33:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_CreateObject]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_CreateObject]
GO
/****** Object:  StoredProcedure [dbo].[META_CreateWorkspace]    Script Date: 09/22/2009 12:33:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_CreateWorkspace]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_CreateWorkspace]
GO
/****** Object:  StoredProcedure [dbo].[META_DeleteGraphFileAssociationsForFile]    Script Date: 09/22/2009 12:33:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_DeleteGraphFileAssociationsForFile]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_DeleteGraphFileAssociationsForFile]
GO
/****** Object:  StoredProcedure [dbo].[META_DeleteObject]    Script Date: 09/22/2009 12:33:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_DeleteObject]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_DeleteObject]
GO
/****** Object:  StoredProcedure [dbo].[Meta_DeleteObsoleteAssociation]    Script Date: 09/22/2009 12:34:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Meta_DeleteObsoleteAssociation]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Meta_DeleteObsoleteAssociation]
GO
/****** Object:  StoredProcedure [dbo].[META_DeleteWorkspace]    Script Date: 09/22/2009 12:34:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_DeleteWorkspace]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_DeleteWorkspace]
GO
/****** Object:  StoredProcedure [dbo].[META_GetAllowedAssociationClasses]    Script Date: 09/22/2009 12:34:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_GetAllowedAssociationClasses]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_GetAllowedAssociationClasses]
GO
/****** Object:  StoredProcedure [dbo].[META_GetAllowedAssociationClassesLimitToAssociationType]    Script Date: 09/22/2009 12:34:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_GetAllowedAssociationClassesLimitToAssociationType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_GetAllowedAssociationClassesLimitToAssociationType]
GO
/****** Object:  StoredProcedure [dbo].[META_GetAllowedAssociationClassesLimitToCaption]    Script Date: 09/22/2009 12:34:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_GetAllowedAssociationClassesLimitToCaption]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_GetAllowedAssociationClassesLimitToCaption]
GO
/****** Object:  StoredProcedure [dbo].[META_GetAllowedClassAssociationCaptions]    Script Date: 09/22/2009 12:34:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_GetAllowedClassAssociationCaptions]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_GetAllowedClassAssociationCaptions]
GO
/****** Object:  StoredProcedure [dbo].[META_GetAllowedClassAssociationTypes]    Script Date: 09/22/2009 12:34:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_GetAllowedClassAssociationTypes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_GetAllowedClassAssociationTypes]
GO
/****** Object:  StoredProcedure [dbo].[META_GetAllowedRelationships]    Script Date: 09/22/2009 12:34:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_GetAllowedRelationships]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_GetAllowedRelationships]
GO
/****** Object:  StoredProcedure [dbo].[META_GetArrayObjects]    Script Date: 09/22/2009 12:34:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_GetArrayObjects]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_GetArrayObjects]
GO
/****** Object:  StoredProcedure [dbo].[META_GetArtifactData]    Script Date: 09/22/2009 12:34:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_GetArtifactData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_GetArtifactData]
GO
/****** Object:  StoredProcedure [dbo].[META_GetAssociatedClasses]    Script Date: 09/22/2009 12:34:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_GetAssociatedClasses]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_GetAssociatedClasses]
GO
/****** Object:  StoredProcedure [dbo].[META_GetAssociatedObjects]    Script Date: 09/22/2009 12:34:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_GetAssociatedObjects]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_GetAssociatedObjects]
GO
/****** Object:  StoredProcedure [dbo].[META_GetAssociationForParentClassAndChildClass]    Script Date: 09/22/2009 12:34:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_GetAssociationForParentClassAndChildClass]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_GetAssociationForParentClassAndChildClass]
GO
/****** Object:  StoredProcedure [dbo].[META_GetCategories]    Script Date: 09/22/2009 12:34:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_GetCategories]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_GetCategories]
GO
/****** Object:  StoredProcedure [dbo].[META_GetClasses]    Script Date: 09/22/2009 12:34:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_GetClasses]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_GetClasses]
GO
/****** Object:  StoredProcedure [dbo].[META_GetDomainDefinitions]    Script Date: 09/22/2009 12:34:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_GetDomainDefinitions]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_GetDomainDefinitions]
GO
/****** Object:  StoredProcedure [dbo].[META_GetDomainPossibleValues]    Script Date: 09/22/2009 12:34:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_GetDomainPossibleValues]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_GetDomainPossibleValues]
GO
/****** Object:  StoredProcedure [dbo].[META_GetFieldDefinitions]    Script Date: 09/22/2009 12:34:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_GetFieldDefinitions]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_GetFieldDefinitions]
GO
/****** Object:  StoredProcedure [dbo].[META_GetFields]    Script Date: 09/22/2009 12:34:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_GetFields]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_GetFields]
GO
/****** Object:  StoredProcedure [dbo].[META_GetGeneralPermissions]    Script Date: 09/22/2009 12:34:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_GetGeneralPermissions]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_GetGeneralPermissions]
GO
/****** Object:  StoredProcedure [dbo].[Meta_GetStatusForAssociation]    Script Date: 09/22/2009 12:34:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Meta_GetStatusForAssociation]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Meta_GetStatusForAssociation]
GO
/****** Object:  StoredProcedure [dbo].[Meta_GetStatusForGraphFile]    Script Date: 09/22/2009 12:34:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Meta_GetStatusForGraphFile]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Meta_GetStatusForGraphFile]
GO
/****** Object:  StoredProcedure [dbo].[Meta_GetStatusForMetaObject]    Script Date: 09/22/2009 12:34:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Meta_GetStatusForMetaObject]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Meta_GetStatusForMetaObject]
GO
/****** Object:  StoredProcedure [dbo].[META_GetTemplateWorkspaces]    Script Date: 09/22/2009 12:34:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_GetTemplateWorkspaces]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_GetTemplateWorkspaces]
GO
/****** Object:  StoredProcedure [dbo].[META_GetUsers]    Script Date: 09/22/2009 12:34:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_GetUsers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_GetUsers]
GO
/****** Object:  StoredProcedure [dbo].[META_GetWorkspacePermissions]    Script Date: 09/22/2009 12:34:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_GetWorkspacePermissions]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_GetWorkspacePermissions]
GO
/****** Object:  StoredProcedure [dbo].[META_GetWorkspacesForUser]    Script Date: 09/22/2009 12:34:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_GetWorkspacesForUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_GetWorkspacesForUser]
GO
/****** Object:  StoredProcedure [dbo].[META_MarkClassDefinitionsAsClean]    Script Date: 09/22/2009 12:34:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_MarkClassDefinitionsAsClean]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_MarkClassDefinitionsAsClean]
GO
/****** Object:  StoredProcedure [dbo].[META_MarkClassDefinitionsAsDirty]    Script Date: 09/22/2009 12:34:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_MarkClassDefinitionsAsDirty]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_MarkClassDefinitionsAsDirty]
GO
/****** Object:  StoredProcedure [dbo].[META_MarkPreviousGraphFileVersionsInactive]    Script Date: 09/22/2009 12:34:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_MarkPreviousGraphFileVersionsInactive]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_MarkPreviousGraphFileVersionsInactive]
GO
/****** Object:  StoredProcedure [dbo].[META_RemoveObjectAssociation]    Script Date: 09/22/2009 12:34:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_RemoveObjectAssociation]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_RemoveObjectAssociation]
GO
/****** Object:  StoredProcedure [dbo].[META_SetGeneralCaptions]    Script Date: 09/22/2009 12:34:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_SetGeneralCaptions]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_SetGeneralCaptions]
GO
/****** Object:  StoredProcedure [dbo].[Meta_SetStatusForAssociation]    Script Date: 09/22/2009 12:34:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Meta_SetStatusForAssociation]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Meta_SetStatusForAssociation]
GO
/****** Object:  StoredProcedure [dbo].[Meta_SetStatusForGraphFile]    Script Date: 09/22/2009 12:34:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Meta_SetStatusForGraphFile]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Meta_SetStatusForGraphFile]
GO
/****** Object:  StoredProcedure [dbo].[Meta_SetStatusForMetaObject]    Script Date: 09/22/2009 12:34:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Meta_SetStatusForMetaObject]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Meta_SetStatusForMetaObject]
GO
/****** Object:  StoredProcedure [dbo].[META_UpdateObjectAssociation]    Script Date: 09/22/2009 12:34:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_UpdateObjectAssociation]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_UpdateObjectAssociation]
GO
/****** Object:  StoredProcedure [dbo].[META_UpdateObjectFieldValue]    Script Date: 09/22/2009 12:34:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_UpdateObjectFieldValue]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_UpdateObjectFieldValue]
GO
/****** Object:  StoredProcedure [dbo].[META_UserLogin]    Script Date: 09/22/2009 12:34:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[META_UserLogin]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[META_UserLogin]

/****** Object:  StoredProcedure [dbo].[META_AddArtifact]    Script Date: 09/22/2009 12:34:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[META_AddArtifact]
@CAid int,
@ObjectID int,
@ChildObjectID int, 
@ArtifactID int,
@objectmachine varchar(50),
@childmachine varchar(50)
as
insert into artifact(caid, objectid, childobjectid, artifactobjectid,objectmachine,childobjectmachine) values (@caid, @objectid,@childobjectid, @artifactid,@objectmachine,@childmachine)



GO
/****** Object:  StoredProcedure [dbo].[META_AddObjectAssociation]    Script Date: 09/22/2009 12:34:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[META_AddObjectAssociation]
@ObjectID int,
@CAid int,
@ChildObjectID int,
@objectmachine varchar(50),
@childmachine varchar(50),
@VCStatusID int = null,
@VCMachineID varchar(50) = null
as
declare @series int
declare @othercaid int
declare @AssociationTypeID as int




select @associationtypeid = associationtypeid from classassociation where caid = @caid
if (@associationtypeid=4)
begin
	declare @childclass as varchar(50)
	declare @parentclass as varchar(50)

	select @childclass = class from MetaObject where pkid = @childobjectid
	select @parentclass = class from MetaObject where pkid = @objectid
	select @othercaid = caid from classassociation where childclass=@parentclass and parentclass=@childclass and associationtypeid=4
	if not exists(select objectid from objectassociation where objectid=@childobjectid and childobjectid=@objectid and caid=@othercaid and objectmachine=@objectmachine and childobjectmachine=@childmachine)
	begin

		select @series = max([series]) + 1 from objectassociation where objectid = @childobjectid and caid = @othercaid
		if @series is null
		set @series = 1
		insert into objectassociation(objectid,childobjectid,caid,[series],objectmachine,childobjectmachine,vcstatusid,vcmachineid) values(@childobjectid,@objectid,@othercaid,@series,@childmachine,@objectmachine,@vcstatusid,@vcmachineid)
	end
end


if not exists(select objectid from objectassociation where objectid=@objectid and childobjectid=@childobjectid and caid=@caid and objectmachine=@objectmachine and childobjectmachine=@childmachine)
begin
select @series = max(series) + 1 from objectassociation where objectid = @objectid and caid = @caid
if @series is null
set @series = 1
insert into objectassociation(objectid,childobjectid,caid,[series],objectmachine,childobjectmachine,vcstatusid,vcmachineid) values(@objectid,@childobjectid,@caid,@series,@objectmachine,@childmachine,@vcstatusid,@vcmachineid)
end







GO
/****** Object:  StoredProcedure [dbo].[META_AddQuickAssociation]    Script Date: 09/22/2009 12:34:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[META_AddQuickAssociation]
@ObjectID1 int,
@ObjectID2 int,
@AssociationTypeID int,
@CAID int = null output,
@ObjectMachine varchar(50),
@ChildMachine varchar(50)
as
declare @class1 as varchar(50)
declare @class2 as varchar(50)
select @class1 = class from MetaObject where pkid = @objectid1
select @class2 = class from MetaObject where pkid = @objectid2

select @CAID = caid from classassociation where parentclass = @class1 and childclass=@class2 and associationtypeid=@AssociationTypeID
if not (@CAID is null)
begin
	exec META_AddObjectAssociation @ObjectID1,@CAID,@ObjectID2, @ObjectMachine,@ChildMachine,1,null
end

if (@caid is null)
set @caid = 0





GO
/****** Object:  StoredProcedure [dbo].[META_CalculateCriticalities]    Script Date: 09/22/2009 12:34:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[META_CalculateCriticalities]

AS


declare @fieldid int
select @fieldid = pkid from field where class='Function' and Name='FunctionCriticality'



DECLARE MY_CURSOR Cursor 
for
SELECT     dbo.METAView_Function_Listing.PKID AS [FunctionID], COUNT(dbo.ObjectAssociation.ObjectID) AS Criticality,METAView_Function_Listing.machine

FROM         dbo.ObjectAssociation INNER JOIN

                      dbo.METAView_CSF_Listing ON dbo.ObjectAssociation.ObjectID = dbo.METAView_CSF_Listing.pkid INNER JOIN

                      dbo.METAView_Function_Listing ON dbo.ObjectAssociation.ChildObjectID = dbo.METAView_Function_Listing.pkid

GROUP BY dbo.METAView_Function_Listing.PKID,dbo.MetaView_Function_Listing.Machine


Open My_Cursor --- (remember to CLOSE IT LATER)
--- We need to make containers for the Cursor Info
DECLARE @PKID INT
DECLARE @Criticality INT
declare @machinename varchar(50)

Fetch NEXT FROM MY_Cursor INTO @PKID, @Criticality,@machinename
While (@@FETCH_STATUS <> -1)
BEGIN
IF (@@FETCH_STATUS <> -2)

if (exists(select valueint from objectfieldvalue where objectid=@pkid and FieldID=@fieldid))
begin
PRINT 'Updating'
UPDATE ObjectFieldValue set valueint =@criticality where objectid=@pkid and FieldID=@fieldid and machineid=@machinename
END
ELSE
BEGIN
PRINT 'Inserting'
insert into objectfieldvalue(objectid,machineid,fieldid,valueint) values(@pkid,@machinename,@fieldid,@criticality )
END

--- You can even Convert the out put, put in conditional logic, etc.
--- Once you are finished with the first record then you it will check for a new value.
--- You can also supply logic to move back in forth in the cursor, etc.

FETCH NEXT FROM MY_Cursor INTO  @PKID, @Criticality,@machinename
END
CLOSE MY_Cursor

DEALLOCATE MY_Cursor


GO
/****** Object:  StoredProcedure [dbo].[META_ClearArtifacts]    Script Date: 09/22/2009 12:34:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[META_ClearArtifacts]
@CAid int,
@ObjectID int,
@ChildObjectID int,
@objectmachine varchar(50),
@childmachine varchar(50)
as
delete from artifact where caid = @caid and objectid=@objectid and childobjectid = @childobjectid and objectmachine = @objectmachine and childobjectmachine = @childmachine



GO
/****** Object:  StoredProcedure [dbo].[META_CreateObject]    Script Date: 09/22/2009 12:34:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[META_CreateObject]
@Class varchar(50),
@pkid int = 0  output,
@WorkspaceTypeID int,
@WorkspaceName	varchar(100),
@UserID int,
@MachineName varchar(50) = null output
as
if (@pkid>0)
begin
	if not exists(select pkid from metaobject where pkid = @pkid and machine=@machinename)
	begin
		set identity_insert dbo.metaobject on
		insert into metaobject(pkid,class,workspacetypeid,workspacename,userid,machine) values(@pkid,@class,@workspacetypeid,@workspacename,@userid,@MachineName)
	end
	set identity_insert dbo.metaobject  off
	return
end

insert into metaobject(class,workspacetypeid,workspacename,userid,machine) values(@class,@workspacetypeid,@workspacename,@userid,@machinename)
set @pkid = @@IDENTITY

GO
/****** Object:  StoredProcedure [dbo].[META_CreateWorkspace]    Script Date: 09/22/2009 12:34:10 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
/****** Object:  StoredProcedure [dbo].[META_DeleteGraphFileAssociationsForFile]    Script Date: 09/22/2009 12:34:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[META_DeleteGraphFileAssociationsForFile]
@graphfileid int,
@graphfilemachine varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	delete from graphfileassociation where graphfileid = @graphfileid and graphfilemachine = @graphfilemachine
END

GO
/****** Object:  StoredProcedure [dbo].[META_DeleteObject]    Script Date: 09/22/2009 12:34:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[META_DeleteObject]
@objectid int,
@ObjectMachine varchar(50)
as
set nocount on

delete from artefact where 
	(artefactobjectid = @objectid and artefactmachine = @ObjectMachine) OR 
	(objectid = @objectid and objectmachine = @objectmachine) OR
	(childobjectid = @objectid and childobjectmachine = @ObjectMachine)

delete from graphfileobject where metaobjectid=@objectid and machineid=@objectmachine
delete from graphfileassociation where  (objectid=@objectid and ObjectMachine=@objectmachine) or (childobjectid=@objectid and childobjectmachine=@objectmachine)
delete from objectassociation where childobjectid=@objectid and childobjectmachine=@objectmachine
delete from objectassociation where objectid=@objectid and objectmachine=@objectmachine
delete from objectfieldvalue where objectid=@objectid and machineid=@objectmachine
delete from MetaObject where pkid=@objectid  and machine=@objectmachine

GO
/****** Object:  StoredProcedure [dbo].[Meta_DeleteObsoleteAssociation]    Script Date: 09/22/2009 12:34:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[Meta_DeleteObsoleteAssociation]
	@caid int,
	@machine varchar(100),
	@vcstatusid int output,
	@objectid int,
	@childobjectid int,
	@objectmachine varchar(50),
	@childobjectmachine varchar(50),
	@vcmachine varchar(50)

AS
BEGIN
	delete from artefact where CAID = @Caid and objectid=@objectid and childobjectid=@childobjectid
		and objectmachine = @objectmachine and childobjectmachine = @childobjectmachine
	
	delete from objectassociation where CAID = @Caid and objectid=@objectid and childobjectid=@childobjectid
		and objectmachine = @objectmachine and childobjectmachine = @childobjectmachine
	

END




GO
/****** Object:  StoredProcedure [dbo].[META_DeleteWorkspace]    Script Date: 09/22/2009 12:34:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[META_DeleteWorkspace]
	-- Add the parameters for the stored procedure here
	@WorkspaceTypeID int,
	@WorkspaceName varchar(100),
	@NewWorkspaceTypeID int = null,
	@NewWorkspaceName varchar(100) = null
AS
BEGIN

if (@NewWorkspaceTypeID is null)
begin
	DELETE FROM GRAPHFILEOBJECT WHERE cast(GraphFileID as varchar(10)) + GraphFileMachine in (Select cast(GraphFileID as varchar(10)) + machine from graphfile where workspacetypeid=@workspacetypeid and workspacename=@workspacename)
	DELETE FROM GRAPHFILEASSOCIATION WHERE cast(GraphFileID as varchar(10)) + GraphFileMachine in (Select cast(GraphFileID as varchar(10)) + machine from graphfile where workspacetypeid=@workspacetypeid and workspacename=@workspacename)
	DELETE FROM GRAPHFILE WHERE workspacetypeid=@workspacetypeid and workspacename=@workspacename

	DELETE FROM ARTiFACT WHERE cast(objectid as varchar(100)) + objectmachine in ((select cast(pkid as varchar(100)) + machine from metaobject where workspacetypeid=@workspacetypeid and workspacename=@workspacename))
	DELETE FROM ARTiFACT WHERE cast(childobjectid as varchar(100)) + childobjectmachine in ((select cast(pkid as varchar(100)) + machine from metaobject where workspacetypeid=@workspacetypeid and workspacename=@workspacename))

	DELETE FROM OBJECTASSOCIATION WHERE cast(objectid as varchar(100)) + objectmachine in (select cast(pkid as varchar(100)) + machine from metaobject where workspacetypeid=@workspacetypeid and workspacename=@workspacename)
	DELETE FROM OBJECTASSOCIATION WHERE cast(childobjectid as varchar(100)) + childobjectmachine in  (select cast(pkid as varchar(100)) + machine from metaobject where workspacetypeid=@workspacetypeid and workspacename=@workspacename)
	DELETE FROM OBJECTFIELDVALUE  WHERE cast(objectid as varchar(100)) + machineid in (select cast(pkid as varchar(100)) + machine from metaobject where workspacetypeid=@workspacetypeid and workspacename=@workspacename)
	DELETE FROM METAOBJECT  WHERE WorkspaceTypeID=@WorkspaceTypeID and workspacename=@workspacename
	DELETE FROM USERPERMISSION where  workspacetypeid=@workspacetypeid and workspacename=@workspacename
	DELETE FROM WORKSPACE where  workspacetypeid=@workspacetypeid and name=@workspacename
return
end
	Update GRAPHFILE set workspacetypeid=@NewWorkspaceTypeID , workspacename=@newworkspacename WHERE  workspacetypeid=@workspacetypeid and workspacename=@workspacename
	Update METAOBJECT set workspacetypeid=@NewWorkspaceTypeID , workspacename=@newworkspacename where  workspacetypeid=@workspacetypeid and workspacename=@workspacename
	DELETE FROM USERPERMISSION where  workspacetypeid=@workspacetypeid AND workspacename=@workspacename
	DELETE FROM WORKSPACE where WorkspaceTypeID = @WorkspaceTypeID AND Name = @WorkspaceName
	
END



GO
/****** Object:  StoredProcedure [dbo].[META_GetAllowedAssociationClasses]    Script Date: 09/22/2009 12:34:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[META_GetAllowedAssociationClasses]
@ParentClassName varchar(50),
@ActiveOnly bit = NULL
as
if (@ActiveOnly = 1)
begin
SELECT  CAid,dbo.ClassAssociation.Caption + ' - ' + dbo.ClassAssociation.ChildClass as Association ,
                      dbo.AssociationType.Name AS AssociationType,dbo.ClassAssociation.ChildClass,associationtypeid,CopyIncluded,IsDefault,Caption
FROM         dbo.ClassAssociation INNER JOIN
                      dbo.AssociationType ON dbo.ClassAssociation.AssociationTypeID = dbo.AssociationType.pkid where parentclass = @ParentClassName
AND dbo.ClassAssociation.IsActive = 1
return
end

SELECT  CAid,dbo.ClassAssociation.Caption + ' - ' + dbo.ClassAssociation.ChildClass as Association ,
                      dbo.AssociationType.Name AS AssociationType,dbo.ClassAssociation.ChildClass,associationtypeid,CopyIncluded,IsDefault,Caption
FROM         dbo.ClassAssociation INNER JOIN
                      dbo.AssociationType ON dbo.ClassAssociation.AssociationTypeID = dbo.AssociationType.pkid where parentclass = @ParentClassName



GO
/****** Object:  StoredProcedure [dbo].[META_GetAllowedAssociationClassesLimitToAssociationType]    Script Date: 09/22/2009 12:34:12 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[META_GetAllowedAssociationClassesLimitToAssociationType]
@ParentClassName varchar(50),
@LimitToAssociationType int,
@ActiveOnly bit = null
as
if (@ActiveOnly = 1)
BEGIN
SELECT  CAid,dbo.ClassAssociation.Caption + ' - ' + dbo.ClassAssociation.ChildClass as Association ,
                      dbo.AssociationType.Name AS AssociationType,dbo.ClassAssociation.ChildClass
FROM         dbo.ClassAssociation INNER JOIN
                      dbo.AssociationType ON dbo.ClassAssociation.AssociationTypeID = dbo.AssociationType.pkid where parentclass = @ParentClassName and associationtypeid = @LimitToAssociationType
and dbo.ClassAssociation.IsActive = 1
return
END

SELECT  CAid,dbo.ClassAssociation.Caption + ' - ' + dbo.ClassAssociation.ChildClass as Association ,
                      dbo.AssociationType.Name AS AssociationType,dbo.ClassAssociation.ChildClass
FROM         dbo.ClassAssociation INNER JOIN
                      dbo.AssociationType ON dbo.ClassAssociation.AssociationTypeID = dbo.AssociationType.pkid where parentclass = @ParentClassName and associationtypeid = @LimitToAssociationType


GO
/****** Object:  StoredProcedure [dbo].[META_GetAllowedAssociationClassesLimitToCaption]    Script Date: 09/22/2009 12:34:12 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[META_GetAllowedAssociationClassesLimitToCaption]
@ParentClassName varchar(50),
@LimitToCaption varchar(50),
@ActiveOnly bit = NULL
as
if (@ActiveOnly = 1)
BEGIN
SELECT  CAid,dbo.ClassAssociation.Caption + ' - ' + dbo.ClassAssociation.ChildClass as Association ,
                      dbo.AssociationType.Name AS AssociationType,dbo.ClassAssociation.ChildClass
FROM         dbo.ClassAssociation INNER JOIN
                      dbo.AssociationType ON dbo.ClassAssociation.AssociationTypeID = dbo.AssociationType.pkid where parentclass = @ParentClassName and caption=@LimitToCaption
And dbo.ClassAssociation.IsActive = 1
return
END

SELECT  CAid,dbo.ClassAssociation.Caption + ' - ' + dbo.ClassAssociation.ChildClass as Association ,
                      dbo.AssociationType.Name AS AssociationType,dbo.ClassAssociation.ChildClass
FROM         dbo.ClassAssociation INNER JOIN
                      dbo.AssociationType ON dbo.ClassAssociation.AssociationTypeID = dbo.AssociationType.pkid where parentclass = @ParentClassName and caption=@LimitToCaption

GO
/****** Object:  StoredProcedure [dbo].[META_GetAllowedClassAssociationCaptions]    Script Date: 09/22/2009 12:34:12 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[META_GetAllowedClassAssociationCaptions]
@ActiveOnly bit = NULL
as
if (@ActiveOnly = 1)
BEGIN
SELECT   Distinct ParentClass,Caption, childclass,associationtypeid,CAID
FROM         dbo.ClassAssociation INNER JOIN
                      dbo.AssociationType ON dbo.ClassAssociation.AssociationTypeID = dbo.AssociationType.pkid
WHERE dbo.ClassAssociation.IsActive = 1
return
end

SELECT   Distinct ParentClass,Caption, childclass,associationtypeid,CAID
FROM         dbo.ClassAssociation INNER JOIN
                      dbo.AssociationType ON dbo.ClassAssociation.AssociationTypeID = dbo.AssociationType.pkid


GO
/****** Object:  StoredProcedure [dbo].[META_GetAllowedClassAssociationTypes]    Script Date: 09/22/2009 12:34:12 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[META_GetAllowedClassAssociationTypes]
@ActiveOnly bit = NULL
as
if (@ActiveOnly = 1)
BEGIN
SELECT  distinct parentclass, associationtype.name 
FROM         dbo.ClassAssociation INNER JOIN
                      dbo.AssociationType ON dbo.ClassAssociation.AssociationTypeID = dbo.AssociationType.pkid
WHERE dbo.ClassAssociation.IsActive = 1
return
end

SELECT  distinct parentclass, associationtype.name 
FROM         dbo.ClassAssociation INNER JOIN
                      dbo.AssociationType ON dbo.ClassAssociation.AssociationTypeID = dbo.AssociationType.pkid

GO
/****** Object:  StoredProcedure [dbo].[META_GetAllowedRelationships]    Script Date: 09/22/2009 12:34:13 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[META_GetAllowedRelationships]
@Class varchar(100),
@ActiveOnly bit = null
as
if (@ActiveOnly = 1)
BEGIN
SELECT    dbo.AllowedClassRelationship.ACRid, dbo.RelationshipType.Name AS RelationshipType, dbo.AllowedClassRelationship.ChildClass AS Type
FROM         dbo.AllowedClassRelationship INNER JOIN
                      dbo.RelationshipType ON dbo.AllowedClassRelationship.RelationshipTypeID = dbo.RelationshipType.pkid
where parentclass = @class 
AND dbo.AllowedClassRelationship.IsActive = 1 ORDER BY dbo.RelationshipType.Name
return
end

SELECT    dbo.AllowedClassRelationship.ACRid, dbo.RelationshipType.Name AS RelationshipType, dbo.AllowedClassRelationship.ChildClass AS Type
FROM         dbo.AllowedClassRelationship INNER JOIN
                      dbo.RelationshipType ON dbo.AllowedClassRelationship.RelationshipTypeID = dbo.RelationshipType.pkid
where parentclass = @class ORDER BY dbo.RelationshipType.Name


GO
/****** Object:  StoredProcedure [dbo].[META_GetArrayObjects]    Script Date: 09/22/2009 12:34:13 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[META_GetArrayObjects]
--@objects varchar(8000)
@Class varchar(50),
@WorkspaceID int
as

-- Select * from the current class instances
declare @SQL nvarchar(4000)

/*set @SQL = 'Select * from METAView_' + @Class + ' where WorkspaceID=' + cast(@WorkspaceID as varchar(10))
exec sp_executesql @SQL*/


-- Get decompositions
SELECT     dbo.ObjectAssociation.ObjectID, dbo.ObjectAssociation.ChildObjectID, dbo.ClassAssociation.ChildClass, dbo.AssociationType.Name
FROM         dbo.AssociationType INNER JOIN
                      dbo.ClassAssociation ON dbo.AssociationType.pkid = dbo.ClassAssociation.AssociationTypeID INNER JOIN
                      dbo.ObjectAssociation ON dbo.ClassAssociation.CAid = dbo.ObjectAssociation.CAid where dbo.AssociationType.Name = 'Decomposition' and ChildClass=@Class
		-- and  @objects like '[' + rtrim(ltrim(cast(ObjectID as varchar(10)))) + ']'


GO
/****** Object:  StoredProcedure [dbo].[META_GetArtifactData]    Script Date: 09/22/2009 12:34:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[META_GetArtifactData]
@CAid int,
@ObjectID int,
@ChildObjectID int,
@objectmachine varchar(50),
@childmachine varchar(50)
as
SELECT     dbo.AllowedArtifact.Class AS ArtifactClass, dbo.MetaObject.Class AS ObjectClass, ChildObject.Class AS ChildObjectClass
FROM         dbo.AllowedArtifact INNER JOIN
                      dbo.ObjectAssociation ON dbo.AllowedArtifact.CAid = dbo.ObjectAssociation.CAid INNER JOIN
                      dbo.MetaObject ON dbo.ObjectAssociation.ObjectID = dbo.MetaObject.pkid INNER JOIN
                      dbo.MetaObject ChildObject ON dbo.ObjectAssociation.ChildObjectID = ChildObject.pkid where objectid = @objectid and childobjectid = @childobjectid and dbo.objectassociation.caid = @caid and 
					objectmachine = @objectmachine and childobjectmachine = @childmachine

SELECT     dbo.AllowedArtifact.Class AS ArtifactClass, dbo.Artifact.ArtefactMachine as ArtefactMachine,dbo.Artifact.ArtifactID, dbo.ObjectAssociation.CAid, dbo.ObjectAssociation.ObjectID, 
                      dbo.ObjectAssociation.ChildObjectID, dbo.Artifact.ArtifactObjectID
FROM         dbo.AllowedArtifact INNER JOIN
                      dbo.ObjectAssociation ON dbo.AllowedArtifact.CAid = dbo.ObjectAssociation.CAid INNER JOIN
                      dbo.Artifact ON dbo.ObjectAssociation.CAid = dbo.Artifact.CAid AND dbo.ObjectAssociation.ObjectID = dbo.Artifact.ObjectID AND 
                      dbo.ObjectAssociation.ChildObjectID = dbo.Artifact.ChildObjectID INNER JOIN
                      dbo.MetaObject ON dbo.Artifact.ArtifactObjectID = dbo.MetaObject.pkid AND dbo.AllowedArtifact.Class = dbo.MetaObject.Class  where objectassociation.objectid = @objectid and objectassociation.childobjectid = @childobjectid and dbo.objectassociation.caid = @caid



SELECT     AssociationObjectClass
FROM         dbo.ClassAssociation where CAid = @caid and not associationobjectclass is null


GO
/****** Object:  StoredProcedure [dbo].[META_GetAssociatedClasses]    Script Date: 09/22/2009 12:34:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[META_GetAssociatedClasses]
as
SELECT     dbo.AssociationType.Name AS AssociationType, dbo.AssociationType.pkid AS AssociationTypeID, dbo.ClassAssociation.CAid, 
                      dbo.ClassAssociation.ParentClass, dbo.ClassAssociation.ChildClass, dbo.ClassAssociation.Caption,CopyIncluded,IsDefault
FROM         dbo.AssociationType INNER JOIN
                      dbo.ClassAssociation ON dbo.AssociationType.pkid = dbo.ClassAssociation.AssociationTypeID




GO
/****** Object:  StoredProcedure [dbo].[META_GetAssociatedObjects]    Script Date: 09/22/2009 12:34:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[META_GetAssociatedObjects]
@ParentObjectID int,
@ParentObjectMachineName varchar(50)
as
SELECT     dbo.ObjectAssociation.VCStatusID,dbo.ObjectAssociation.CAid, dbo.ObjectAssociation.ChildObjectID, dbo.ObjectAssociation.ObjectID, 
              dbo.ClassAssociation.Caption, dbo.ClassAssociation.ChildClass,ClassAssociation.AssociationTypeID,series,dbo.ObjectAssociation.ObjectMachine,dbo.ObjectAssociation.ChildObjectMachine
FROM         dbo.ObjectAssociation INNER JOIN
                      dbo.ClassAssociation ON dbo.ObjectAssociation.CAid = dbo.ClassAssociation.CAid
		where dbo.ObjectAssociation.ObjectMachine = @ParentObjectMachineName and dbo.ObjectAssociation.ObjectID = @ParentObjectID order by series




GO
/****** Object:  StoredProcedure [dbo].[META_GetAssociationForParentClassAndChildClass]    Script Date: 09/22/2009 12:34:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[META_GetAssociationForParentClassAndChildClass]
	@ParentClass varchar(50),
	@ChildClass varchar(50),
	@LimitToAssociationType int,
	@AssociationID int =null output,
@DisplayMember varchar(150) = null output,
@ActiveOnly bit = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

if (@ActiveOnly = 1)
BEGIN
   
	select @DisplayMember =Caption,@AssociationID = CAid from ClassAssociation where ParentClass = @ParentClass 
		and ChildClass = @ChildClass and AssociationTypeID = @LimitToAssociationType
and IsActive =1
return
END

select @DisplayMember =Caption,@AssociationID = CAid from ClassAssociation where ParentClass = @ParentClass 
		and ChildClass = @ChildClass and AssociationTypeID = @LimitToAssociationType

END





GO
/****** Object:  StoredProcedure [dbo].[META_GetCategories]    Script Date: 09/22/2009 12:34:14 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[META_GetCategories]
as
select distinct Category from class order by Category


GO
/****** Object:  StoredProcedure [dbo].[META_GetClasses]    Script Date: 09/22/2009 12:34:14 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[META_GetClasses]
@ActiveOnly bit = null
as
if (@ActiveOnly = 1)
BEGIN
select * from class where isactive=1
return
end

select * from class


GO
/****** Object:  StoredProcedure [dbo].[META_GetDomainDefinitions]    Script Date: 09/22/2009 12:34:14 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[META_GetDomainDefinitions]
as
select * from domaindefinition


GO
/****** Object:  StoredProcedure [dbo].[META_GetDomainPossibleValues]    Script Date: 09/22/2009 12:34:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[META_GetDomainPossibleValues]
@DomainDefinition varchar(50),
@ActiveOnly bit = null
as
if (@ActiveOnly = 1)
BEGIN
SELECT     dbo.DomainDefinition.Name, dbo.DomainDefinitionPossibleValue.PossibleValue, dbo.DomainDefinitionPossibleValue.Series, 
                      dbo.DomainDefinitionPossibleValue.Description
FROM         dbo.DomainDefinition INNER JOIN
                      dbo.DomainDefinitionPossibleValue ON dbo.DomainDefinition.pkid = dbo.DomainDefinitionPossibleValue.DomainDefinitionID where dbo.DomainDefinition.Name = @domaindefinition
and dbo.DomainDefinitionPossibleValue.IsActive=1
return
end

SELECT     dbo.DomainDefinition.Name, dbo.DomainDefinitionPossibleValue.PossibleValue, dbo.DomainDefinitionPossibleValue.Series, 
                      dbo.DomainDefinitionPossibleValue.Description
FROM         dbo.DomainDefinition INNER JOIN
                      dbo.DomainDefinitionPossibleValue ON dbo.DomainDefinition.pkid = dbo.DomainDefinitionPossibleValue.DomainDefinitionID where dbo.DomainDefinition.Name = @domaindefinition

GO
/****** Object:  StoredProcedure [dbo].[META_GetFieldDefinitions]    Script Date: 09/22/2009 12:34:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[META_GetFieldDefinitions]
@ClassName varchar(50),
@ActiveOnly bit = null
as
if (@ActiveOnly = 1)
BEGIN
select * from field where class = @classname and Isactive=1
return
end

select * from field where class = @classname


GO
/****** Object:  StoredProcedure [dbo].[META_GetFields]    Script Date: 09/22/2009 12:34:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[META_GetFields]
as
select * from field


GO
/****** Object:  StoredProcedure [dbo].[META_GetGeneralPermissions]    Script Date: 09/22/2009 12:34:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[META_GetGeneralPermissions]
@UserID int
as
select Permission.pkid as PermissionID, Permission.Description  from Permission inner join UserPermission on Permission.pkid = UserPermission.PermissionID where Permission.PermissionType='General' and UserPermission.UserID = @UserID


GO
/****** Object:  StoredProcedure [dbo].[Meta_GetStatusForAssociation]    Script Date: 09/22/2009 12:34:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Meta_GetStatusForAssociation]
	@caid int,
	@machine varchar(100),
	@vcstatusid int output,
	@objectid int,
	@childobjectid int,
	@objectmachine varchar(50),
	@childobjectmachine varchar(50)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT @vcstatusid = vcstatusid from objectassociation where caid=@caid and objectid=@objectid and objectmachine=@objectmachine and childobjectid=@childobjectid and childobjectmachine=@childobjectmachine
	if (@vcstatusid is null)
		select @vcstatusid = pkid from vcstatus where name='None'


END


GO
/****** Object:  StoredProcedure [dbo].[Meta_GetStatusForGraphFile]    Script Date: 09/22/2009 12:34:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Meta_GetStatusForGraphFile]
	@pkid int,
	@machine varchar(100),
	@vcstatusid int output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT @vcstatusid = vcstatusid from graphfile where pkid =@pkid and machine = @machine
	if (@vcstatusid is null)
		select @vcstatusid = pkid from vcstatus where name='None'


END


GO
/****** Object:  StoredProcedure [dbo].[Meta_GetStatusForMetaObject]    Script Date: 09/22/2009 12:34:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Meta_GetStatusForMetaObject]
	@pkid int,
	@machine varchar(100),
	@vcstatusid int output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT @vcstatusid = vcstatusid from metaobject where pkid =@pkid and machine = @machine
	if (@vcstatusid is null)
		select @vcstatusid = pkid from vcstatus where name='None'


END


GO
/****** Object:  StoredProcedure [dbo].[META_GetTemplateWorkspaces]    Script Date: 09/22/2009 12:34:16 ******/
GO
/****** Object:  StoredProcedure [dbo].[META_GetUsers]    Script Date: 09/22/2009 12:34:16 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[META_GetUsers]
as
declare @falseboolean as bit
set @falseboolean = 0

select pkid,name,@falseboolean as AllowRead, @falseboolean as AllowWrite,@falseboolean as Reporting, @falseboolean as Admin  from [user]


GO
/****** Object:  StoredProcedure [dbo].[META_GetWorkspacePermissions]    Script Date: 09/22/2009 12:34:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [dbo].[META_GetWorkspacesForUser]    Script Date: 09/22/2009 12:34:17 ******/
SET ANSI_NULLS OFF

GO
/****** Object:  StoredProcedure [dbo].[META_MarkPreviousGraphFileVersionsInactive]    Script Date: 09/22/2009 12:34:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[META_MarkPreviousGraphFileVersionsInactive]
@pkid int,
@machine varchar(100)
as
declare @OriginalFileUniqueID uniqueidentifier
select @OriginalFileUniqueID = OriginalFileUniqueID from GraphFile where pkid = @pkid and machine=@machine
update graphfile set isactive =0 where originalfileuniqueid = @OriginalFileUniqueID and pkid<>@pkid

GO
/****** Object:  StoredProcedure [dbo].[META_RemoveObjectAssociation]    Script Date: 09/22/2009 12:34:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[META_RemoveObjectAssociation]
@ObjectID int,
@CAid int,
@ChildObjectID int,
@ObjectMachine varchar(50),
@ChildMachine varchar(50)
as
declare @AssociationTypeID as int
select @associationtypeid from classassociation where caid = @caid
if (@associationtypeid=4)
delete from objectassociation where objectid = @childobjectid and childobjectid = @objectid and caid = @caid and objectmachine = @objectmachine and childobjectmachine = @childmachine
delete from objectassociation where objectid = @objectid and childobjectid = @childobjectid and caid = @caid and objectmachine = @objectmachine and childobjectmachine = @childmachine



GO
/****** Object:  StoredProcedure [dbo].[META_SetGeneralCaptions]    Script Date: 09/22/2009 12:34:18 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[META_SetGeneralCaptions]
as

DECLARE MappingCursor CURSOR
KEYSET
FOR select caid,associationtype.name as AssociationType,ChildClass from ClassAssociation inner join associationtype on classassociation.associationtypeid = associationtype.pkid


declare @caption varchar(50)
declare @association as varchar(50)
declare @child as varchar(50)
declare @caid as int

OPEN MappingCursor

FETCH NEXT FROM MappingCursor INTO @caid,@association,@child
WHILE (@@fetch_status <> -1)
BEGIN
	IF (@@fetch_status <> -2)
	BEGIN
		if (@association='Auxiliary') 
		set @caption = 'Auxiliaries: ' + @child

		if (@association='Decomposition')
		set @caption= 'Decompositions: '+ @child

		if (@association='Classification')
		set @caption = 'Classifications: ' + @child
		
		if (@association='Mapping')
		set @caption='Mappings: ' + @child
		
		update classassociation set caption =@caption where caid = @caid

	END
	FETCH NEXT FROM MappingCursor INTO @caid,@association,@child
END

CLOSE MappingCursor
DEALLOCATE MappingCursor


GO
/****** Object:  StoredProcedure [dbo].[Meta_SetStatusForAssociation]    Script Date: 09/22/2009 12:34:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Meta_SetStatusForAssociation]
	@caid int,
	@machine varchar(100),
	@vcstatusid int output,
	@objectid int,
	@childobjectid int,
	@objectmachine varchar(50),
	@childobjectmachine varchar(50),
	@vcmachine varchar(50)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
UPDATE objectassociation set vcstatusid=@vcstatusid,vcmachineid=@vcmachine where caid=@caid and objectid=@objectid and objectmachine=@objectmachine and childobjectid=@childobjectid and childobjectmachine=@childobjectmachine
	

END



GO
/****** Object:  StoredProcedure [dbo].[Meta_SetStatusForGraphFile]    Script Date: 09/22/2009 12:34:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Meta_SetStatusForGraphFile]
	@pkid int,
	@machine varchar(100),
	@vcstatusid int,
@vcmachine varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	update graphfile set vcstatusid=@vcstatusid,vcmachineid=@vcmachine where pkid =@pkid and machine = @machine
	

END



GO
/****** Object:  StoredProcedure [dbo].[Meta_SetStatusForMetaObject]    Script Date: 09/22/2009 12:34:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Meta_SetStatusForMetaObject]
	@pkid int,
	@machine varchar(100),
	@vcstatusid int,
	@vcmachine varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE metaobject set vcstatusid=@vcstatusid,vcmachineid=@vcmachine where pkid =@pkid and machine = @machine

END



GO
/****** Object:  StoredProcedure [dbo].[META_UpdateObjectAssociation]    Script Date: 09/22/2009 12:34:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[META_UpdateObjectAssociation]
@ObjectID int,
@CAid int,
@ChildObjectID int,
@Series int,
@ObjectMachine varchar(50),
@ChildMachine varchar(50)
as
update objectassociation set series = @series where objectid = @objectid and childobjectid = @childobjectid and caid = @caid and objectmachine =@objectmachine and childobjectmachine = @childmachine



GO
/****** Object:  StoredProcedure [dbo].[META_UpdateObjectFieldValue]    Script Date: 09/22/2009 12:34:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[META_UpdateObjectFieldValue]
@ObjectID int,
@FieldID int,
@ValueString varchar(255) = null,
@ValueInt int = null,
@ValueDouble numeric(18,2) = null,
@ValueBoolean bit = null,
@ValueDate DateTime = null,
@ValueLongText text = null,
@ValueObjectID int = null,
@ValueRTF text = null,
@MachineName varchar(50)
as


if exists(select fieldid from objectfieldvalue where fieldid = @fieldid and objectid =@objectid and machineid=@machinename)
begin

	update objectfieldvalue set
		valuestring = @valuestring,
		valueint = @valueint,
		valuedouble = @valuedouble,
		valueboolean = @valueboolean,
		valuedate = @valuedate,
		valuelongtext = @valuelongtext,
		valueobjectid = @valueobjectid,
		valuertf = @valuertf
		where objectid = @objectid and fieldid = @fieldid and machineid=@MachineName
	return
end

insert into objectfieldvalue(objectid,fieldid,valuestring,valueint,valuedouble,valueboolean,valuedate,valuelongtext,valueobjectid,valuertf,machineid)	
		values(@objectid,@fieldid,@valuestring,@valueint,@valuedouble,@valueboolean,@valuedate,@valuelongtext,@valueobjectid,@valuertf,@machinename)





GO
/****** Object:  StoredProcedure [dbo].[META_UserLogin]    Script Date: 09/22/2009 12:34:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[META_UserLogin]
@Username varchar(100),
@Password varchar(100),
@UserID int =0 output
as
if exists (select pkid from [user] where name=@username and password = @password)
begin
	select @UserID = pkid from [user] where name = @username and password = @password
	return
end
set @UserID = 0

/* DROP & RECREATE ALL VIEWS */

USE [MetaBuilderBlank11]
GO
/****** Object:  View [dbo].[METAView_Activity_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Activity_Listing]'))
DROP VIEW [dbo].[METAView_Activity_Listing]
GO
/****** Object:  View [dbo].[METAView_Activity_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Activity_Retrieval]'))
DROP VIEW [dbo].[METAView_Activity_Retrieval]
GO
/****** Object:  View [dbo].[METAView_Attribute_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Attribute_Listing]'))
DROP VIEW [dbo].[METAView_Attribute_Listing]
GO
/****** Object:  View [dbo].[METAView_Attribute_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Attribute_Retrieval]'))
DROP VIEW [dbo].[METAView_Attribute_Retrieval]
GO
/****** Object:  View [dbo].[METAView_CAD_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_CAD_Listing]'))
DROP VIEW [dbo].[METAView_CAD_Listing]
GO
/****** Object:  View [dbo].[METAView_CAD_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_CAD_Retrieval]'))
DROP VIEW [dbo].[METAView_CAD_Retrieval]
GO
/****** Object:  View [dbo].[METAView_CADReal_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_CADReal_Listing]'))
DROP VIEW [dbo].[METAView_CADReal_Listing]
GO
/****** Object:  View [dbo].[METAView_CADReal_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_CADReal_Retrieval]'))
DROP VIEW [dbo].[METAView_CADReal_Retrieval]
GO
/****** Object:  View [dbo].[METAView_CategoryFactor_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_CategoryFactor_Listing]'))
DROP VIEW [dbo].[METAView_CategoryFactor_Listing]
GO
/****** Object:  View [dbo].[METAView_CategoryFactor_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_CategoryFactor_Retrieval]'))
DROP VIEW [dbo].[METAView_CategoryFactor_Retrieval]
GO
/****** Object:  View [dbo].[METAView_Competency_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Competency_Listing]'))
DROP VIEW [dbo].[METAView_Competency_Listing]
GO
/****** Object:  View [dbo].[METAView_Competency_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Competency_Retrieval]'))
DROP VIEW [dbo].[METAView_Competency_Retrieval]
GO
/****** Object:  View [dbo].[METAView_Condition_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Condition_Listing]'))
DROP VIEW [dbo].[METAView_Condition_Listing]
GO
/****** Object:  View [dbo].[METAView_Condition_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Condition_Retrieval]'))
DROP VIEW [dbo].[METAView_Condition_Retrieval]
GO
/****** Object:  View [dbo].[METAView_Conditional_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Conditional_Listing]'))
DROP VIEW [dbo].[METAView_Conditional_Listing]
GO
/****** Object:  View [dbo].[METAView_Conditional_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Conditional_Retrieval]'))
DROP VIEW [dbo].[METAView_Conditional_Retrieval]
GO
/****** Object:  View [dbo].[METAView_ConditionalDescription_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_ConditionalDescription_Listing]'))
DROP VIEW [dbo].[METAView_ConditionalDescription_Listing]
GO
/****** Object:  View [dbo].[METAView_ConditionalDescription_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_ConditionalDescription_Retrieval]'))
DROP VIEW [dbo].[METAView_ConditionalDescription_Retrieval]
GO
/****** Object:  View [dbo].[METAView_ConnectionSpeed_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_ConnectionSpeed_Listing]'))
DROP VIEW [dbo].[METAView_ConnectionSpeed_Listing]
GO
/****** Object:  View [dbo].[METAView_ConnectionSpeed_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_ConnectionSpeed_Retrieval]'))
DROP VIEW [dbo].[METAView_ConnectionSpeed_Retrieval]
GO
/****** Object:  View [dbo].[METAView_ConnectionType_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_ConnectionType_Listing]'))
DROP VIEW [dbo].[METAView_ConnectionType_Listing]
GO
/****** Object:  View [dbo].[METAView_ConnectionType_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_ConnectionType_Retrieval]'))
DROP VIEW [dbo].[METAView_ConnectionType_Retrieval]
GO
/****** Object:  View [dbo].[METAView_CSF_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_CSF_Listing]'))
DROP VIEW [dbo].[METAView_CSF_Listing]
GO
/****** Object:  View [dbo].[METAView_CSF_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_CSF_Retrieval]'))
DROP VIEW [dbo].[METAView_CSF_Retrieval]
GO
/****** Object:  View [dbo].[METAView_DataColumn_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_DataColumn_Listing]'))
DROP VIEW [dbo].[METAView_DataColumn_Listing]
GO
/****** Object:  View [dbo].[METAView_DataColumn_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_DataColumn_Retrieval]'))
DROP VIEW [dbo].[METAView_DataColumn_Retrieval]
GO
/****** Object:  View [dbo].[METAView_DataSchema_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_DataSchema_Listing]'))
DROP VIEW [dbo].[METAView_DataSchema_Listing]
GO
/****** Object:  View [dbo].[METAView_DataSchema_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_DataSchema_Retrieval]'))
DROP VIEW [dbo].[METAView_DataSchema_Retrieval]
GO
/****** Object:  View [dbo].[METAView_DataTable_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_DataTable_Listing]'))
DROP VIEW [dbo].[METAView_DataTable_Listing]
GO
/****** Object:  View [dbo].[METAView_DataTable_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_DataTable_Retrieval]'))
DROP VIEW [dbo].[METAView_DataTable_Retrieval]
GO
/****** Object:  View [dbo].[METAView_DataView_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_DataView_Listing]'))
DROP VIEW [dbo].[METAView_DataView_Listing]
GO
/****** Object:  View [dbo].[METAView_DataView_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_DataView_Retrieval]'))
DROP VIEW [dbo].[METAView_DataView_Retrieval]
GO
/****** Object:  View [dbo].[METAView_DatedResponsibility_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_DatedResponsibility_Listing]'))
DROP VIEW [dbo].[METAView_DatedResponsibility_Listing]
GO
/****** Object:  View [dbo].[METAView_DatedResponsibility_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_DatedResponsibility_Retrieval]'))
DROP VIEW [dbo].[METAView_DatedResponsibility_Retrieval]
GO
/****** Object:  View [dbo].[METAView_Employee_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Employee_Listing]'))
DROP VIEW [dbo].[METAView_Employee_Listing]
GO
/****** Object:  View [dbo].[METAView_Employee_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Employee_Retrieval]'))
DROP VIEW [dbo].[METAView_Employee_Retrieval]
GO
/****** Object:  View [dbo].[METAView_Entity_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Entity_Listing]'))
DROP VIEW [dbo].[METAView_Entity_Listing]
GO
/****** Object:  View [dbo].[METAView_Entity_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Entity_Retrieval]'))
DROP VIEW [dbo].[METAView_Entity_Retrieval]
GO
/****** Object:  View [dbo].[METAView_Environment_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Environment_Listing]'))
DROP VIEW [dbo].[METAView_Environment_Listing]
GO
/****** Object:  View [dbo].[METAView_Environment_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Environment_Retrieval]'))
DROP VIEW [dbo].[METAView_Environment_Retrieval]
GO
/****** Object:  View [dbo].[METAView_EnvironmentCategory_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_EnvironmentCategory_Listing]'))
DROP VIEW [dbo].[METAView_EnvironmentCategory_Listing]
GO
/****** Object:  View [dbo].[METAView_EnvironmentCategory_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_EnvironmentCategory_Retrieval]'))
DROP VIEW [dbo].[METAView_EnvironmentCategory_Retrieval]
GO
/****** Object:  View [dbo].[METAView_Exception_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Exception_Listing]'))
DROP VIEW [dbo].[METAView_Exception_Listing]
GO
/****** Object:  View [dbo].[METAView_Exception_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Exception_Retrieval]'))
DROP VIEW [dbo].[METAView_Exception_Retrieval]
GO
/****** Object:  View [dbo].[METAView_FlowDescription_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_FlowDescription_Listing]'))
DROP VIEW [dbo].[METAView_FlowDescription_Listing]
GO
/****** Object:  View [dbo].[METAView_FlowDescription_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_FlowDescription_Retrieval]'))
DROP VIEW [dbo].[METAView_FlowDescription_Retrieval]
GO
/****** Object:  View [dbo].[METAView_Function_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Function_Listing]'))
DROP VIEW [dbo].[METAView_Function_Listing]
GO
/****** Object:  View [dbo].[METAView_Function_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Function_Retrieval]'))
DROP VIEW [dbo].[METAView_Function_Retrieval]
GO
/****** Object:  View [dbo].[METAView_FunctionalDependency_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_FunctionalDependency_Listing]'))
DROP VIEW [dbo].[METAView_FunctionalDependency_Listing]
GO
/****** Object:  View [dbo].[METAView_FunctionalDependency_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_FunctionalDependency_Retrieval]'))
DROP VIEW [dbo].[METAView_FunctionalDependency_Retrieval]
GO
/****** Object:  View [dbo].[METAView_GovernanceMechanism_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_GovernanceMechanism_Listing]'))
DROP VIEW [dbo].[METAView_GovernanceMechanism_Listing]
GO
/****** Object:  View [dbo].[METAView_GovernanceMechanism_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_GovernanceMechanism_Retrieval]'))
DROP VIEW [dbo].[METAView_GovernanceMechanism_Retrieval]
GO
/****** Object:  View [dbo].[METAView_Implication_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Implication_Listing]'))
DROP VIEW [dbo].[METAView_Implication_Listing]
GO
/****** Object:  View [dbo].[METAView_Implication_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Implication_Retrieval]'))
DROP VIEW [dbo].[METAView_Implication_Retrieval]
GO
/****** Object:  View [dbo].[METAView_Iteration_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Iteration_Listing]'))
DROP VIEW [dbo].[METAView_Iteration_Listing]
GO
/****** Object:  View [dbo].[METAView_Iteration_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Iteration_Retrieval]'))
DROP VIEW [dbo].[METAView_Iteration_Retrieval]
GO
/****** Object:  View [dbo].[METAView_Job_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Job_Listing]'))
DROP VIEW [dbo].[METAView_Job_Listing]
GO
/****** Object:  View [dbo].[METAView_Job_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Job_Retrieval]'))
DROP VIEW [dbo].[METAView_Job_Retrieval]
GO
/****** Object:  View [dbo].[METAView_JobPosition_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_JobPosition_Listing]'))
DROP VIEW [dbo].[METAView_JobPosition_Listing]
GO
/****** Object:  View [dbo].[METAView_JobPosition_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_JobPosition_Retrieval]'))
DROP VIEW [dbo].[METAView_JobPosition_Retrieval]
GO
/****** Object:  View [dbo].[METAView_Location_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Location_Listing]'))
DROP VIEW [dbo].[METAView_Location_Listing]
GO
/****** Object:  View [dbo].[METAView_Location_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Location_Retrieval]'))
DROP VIEW [dbo].[METAView_Location_Retrieval]
GO
/****** Object:  View [dbo].[METAView_LocationAssociation_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_LocationAssociation_Listing]'))
DROP VIEW [dbo].[METAView_LocationAssociation_Listing]
GO
/****** Object:  View [dbo].[METAView_LocationAssociation_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_LocationAssociation_Retrieval]'))
DROP VIEW [dbo].[METAView_LocationAssociation_Retrieval]
GO
/****** Object:  View [dbo].[METAView_Logic_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Logic_Listing]'))
DROP VIEW [dbo].[METAView_Logic_Listing]
GO
/****** Object:  View [dbo].[METAView_Logic_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Logic_Retrieval]'))
DROP VIEW [dbo].[METAView_Logic_Retrieval]
GO
/****** Object:  View [dbo].[METAView_MeasurementItem_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_MeasurementItem_Listing]'))
DROP VIEW [dbo].[METAView_MeasurementItem_Listing]
GO
/****** Object:  View [dbo].[METAView_MeasurementItem_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_MeasurementItem_Retrieval]'))
DROP VIEW [dbo].[METAView_MeasurementItem_Retrieval]
GO
/****** Object:  View [dbo].[METAView_Model_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Model_Listing]'))
DROP VIEW [dbo].[METAView_Model_Listing]
GO
/****** Object:  View [dbo].[METAView_Model_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Model_Retrieval]'))
DROP VIEW [dbo].[METAView_Model_Retrieval]
GO
/****** Object:  View [dbo].[METAView_ModelReal_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_ModelReal_Listing]'))
DROP VIEW [dbo].[METAView_ModelReal_Listing]
GO
/****** Object:  View [dbo].[METAView_ModelReal_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_ModelReal_Retrieval]'))
DROP VIEW [dbo].[METAView_ModelReal_Retrieval]
GO
/****** Object:  View [dbo].[METAView_MutuallyExclusiveIndicator_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_MutuallyExclusiveIndicator_Listing]'))
DROP VIEW [dbo].[METAView_MutuallyExclusiveIndicator_Listing]
GO
/****** Object:  View [dbo].[METAView_MutuallyExclusiveIndicator_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_MutuallyExclusiveIndicator_Retrieval]'))
DROP VIEW [dbo].[METAView_MutuallyExclusiveIndicator_Retrieval]
GO
/****** Object:  View [dbo].[METAView_NetworkComponent_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_NetworkComponent_Listing]'))
DROP VIEW [dbo].[METAView_NetworkComponent_Listing]
GO
/****** Object:  View [dbo].[METAView_NetworkComponent_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_NetworkComponent_Retrieval]'))
DROP VIEW [dbo].[METAView_NetworkComponent_Retrieval]
GO
/****** Object:  View [dbo].[METAView_Object_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Object_Listing]'))
DROP VIEW [dbo].[METAView_Object_Listing]
GO
/****** Object:  View [dbo].[METAView_Object_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Object_Retrieval]'))
DROP VIEW [dbo].[METAView_Object_Retrieval]
GO
/****** Object:  View [dbo].[METAView_OrganizationalUnit_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_OrganizationalUnit_Listing]'))
DROP VIEW [dbo].[METAView_OrganizationalUnit_Listing]
GO
/****** Object:  View [dbo].[METAView_OrganizationalUnit_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_OrganizationalUnit_Retrieval]'))
DROP VIEW [dbo].[METAView_OrganizationalUnit_Retrieval]
GO
/****** Object:  View [dbo].[METAView_Peripheral_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Peripheral_Listing]'))
DROP VIEW [dbo].[METAView_Peripheral_Listing]
GO
/****** Object:  View [dbo].[METAView_Peripheral_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Peripheral_Retrieval]'))
DROP VIEW [dbo].[METAView_Peripheral_Retrieval]
GO
/****** Object:  View [dbo].[METAView_ProbOfRealization_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_ProbOfRealization_Listing]'))
DROP VIEW [dbo].[METAView_ProbOfRealization_Listing]
GO
/****** Object:  View [dbo].[METAView_ProbOfRealization_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_ProbOfRealization_Retrieval]'))
DROP VIEW [dbo].[METAView_ProbOfRealization_Retrieval]
GO
/****** Object:  View [dbo].[METAView_Process_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Process_Listing]'))
DROP VIEW [dbo].[METAView_Process_Listing]
GO
/****** Object:  View [dbo].[METAView_Process_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Process_Retrieval]'))
DROP VIEW [dbo].[METAView_Process_Retrieval]
GO
/****** Object:  View [dbo].[METAView_Rationale_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Rationale_Listing]'))
DROP VIEW [dbo].[METAView_Rationale_Listing]
GO
/****** Object:  View [dbo].[METAView_Rationale_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Rationale_Retrieval]'))
DROP VIEW [dbo].[METAView_Rationale_Retrieval]
GO
/****** Object:  View [dbo].[METAView_ResourceMappingValue_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_ResourceMappingValue_Listing]'))
DROP VIEW [dbo].[METAView_ResourceMappingValue_Listing]
GO
/****** Object:  View [dbo].[METAView_ResourceMappingValue_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_ResourceMappingValue_Retrieval]'))
DROP VIEW [dbo].[METAView_ResourceMappingValue_Retrieval]
GO
/****** Object:  View [dbo].[METAView_Responsibility_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Responsibility_Listing]'))
DROP VIEW [dbo].[METAView_Responsibility_Listing]
GO
/****** Object:  View [dbo].[METAView_Responsibility_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Responsibility_Retrieval]'))
DROP VIEW [dbo].[METAView_Responsibility_Retrieval]
GO
/****** Object:  View [dbo].[METAView_Role_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Role_Listing]'))
DROP VIEW [dbo].[METAView_Role_Listing]
GO
/****** Object:  View [dbo].[METAView_Role_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Role_Retrieval]'))
DROP VIEW [dbo].[METAView_Role_Retrieval]
GO
/****** Object:  View [dbo].[METAView_Scenario_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Scenario_Listing]'))
DROP VIEW [dbo].[METAView_Scenario_Listing]
GO
/****** Object:  View [dbo].[METAView_Scenario_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Scenario_Retrieval]'))
DROP VIEW [dbo].[METAView_Scenario_Retrieval]
GO
/****** Object:  View [dbo].[METAView_SelectorAttribute_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_SelectorAttribute_Listing]'))
DROP VIEW [dbo].[METAView_SelectorAttribute_Listing]
GO
/****** Object:  View [dbo].[METAView_SelectorAttribute_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_SelectorAttribute_Retrieval]'))
DROP VIEW [dbo].[METAView_SelectorAttribute_Retrieval]
GO
/****** Object:  View [dbo].[METAView_Skill_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Skill_Listing]'))
DROP VIEW [dbo].[METAView_Skill_Listing]
GO
/****** Object:  View [dbo].[METAView_Skill_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Skill_Retrieval]'))
DROP VIEW [dbo].[METAView_Skill_Retrieval]
GO
/****** Object:  View [dbo].[METAView_Software_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Software_Listing]'))
DROP VIEW [dbo].[METAView_Software_Listing]
GO
/****** Object:  View [dbo].[METAView_Software_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_Software_Retrieval]'))
DROP VIEW [dbo].[METAView_Software_Retrieval]
GO
/****** Object:  View [dbo].[METAView_StorageComponent_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_StorageComponent_Listing]'))
DROP VIEW [dbo].[METAView_StorageComponent_Listing]
GO
/****** Object:  View [dbo].[METAView_StorageComponent_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_StorageComponent_Retrieval]'))
DROP VIEW [dbo].[METAView_StorageComponent_Retrieval]
GO
/****** Object:  View [dbo].[METAView_StrategicTheme_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_StrategicTheme_Listing]'))
DROP VIEW [dbo].[METAView_StrategicTheme_Listing]
GO
/****** Object:  View [dbo].[METAView_StrategicTheme_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_StrategicTheme_Retrieval]'))
DROP VIEW [dbo].[METAView_StrategicTheme_Retrieval]
GO
/****** Object:  View [dbo].[METAView_SystemComponent_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_SystemComponent_Listing]'))
DROP VIEW [dbo].[METAView_SystemComponent_Listing]
GO
/****** Object:  View [dbo].[METAView_SystemComponent_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_SystemComponent_Retrieval]'))
DROP VIEW [dbo].[METAView_SystemComponent_Retrieval]
GO
/****** Object:  View [dbo].[METAView_TimeIndicator_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_TimeIndicator_Listing]'))
DROP VIEW [dbo].[METAView_TimeIndicator_Listing]
GO
/****** Object:  View [dbo].[METAView_TimeIndicator_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_TimeIndicator_Retrieval]'))
DROP VIEW [dbo].[METAView_TimeIndicator_Retrieval]
GO
/****** Object:  View [dbo].[METAView_TimeScheme_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_TimeScheme_Listing]'))
DROP VIEW [dbo].[METAView_TimeScheme_Listing]
GO
/****** Object:  View [dbo].[METAView_TimeScheme_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_TimeScheme_Retrieval]'))
DROP VIEW [dbo].[METAView_TimeScheme_Retrieval]
GO
/****** Object:  View [dbo].[METAView_TimeUnit_Listing]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_TimeUnit_Listing]'))
DROP VIEW [dbo].[METAView_TimeUnit_Listing]
GO
/****** Object:  View [dbo].[METAView_TimeUnit_Retrieval]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_TimeUnit_Retrieval]'))
DROP VIEW [dbo].[METAView_TimeUnit_Retrieval]
GO
/****** Object:  View [dbo].[vw_FieldValue]    Script Date: 09/22/2009 12:38:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_FieldValue]'))
DROP VIEW [dbo].[vw_FieldValue]


/****** Object:  View [dbo].[METAView_Activity_Listing]    Script Date: 09/22/2009 12:38:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Activity_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,ExecutionIndicatorValue.ValueString as ExecutionIndicator,ContextualIndicatorValue.ValueString as ContextualIndicator,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  ExecutionIndicatorField  ON dbo.MetaObject.Class = ExecutionIndicatorField.Class left outer JOIN  
dbo.ObjectFieldValue ExecutionIndicatorValue ON dbo.MetaObject.pkid=ExecutionIndicatorValue.ObjectID and dbo.MetaObject.Machine=ExecutionIndicatorValue.MachineID and ExecutionIndicatorField.pkid = ExecutionIndicatorValue.fieldid  
 INNER JOIN  dbo.Field  ContextualIndicatorField  ON dbo.MetaObject.Class = ContextualIndicatorField.Class left outer JOIN  
dbo.ObjectFieldValue ContextualIndicatorValue ON dbo.MetaObject.pkid=ContextualIndicatorValue.ObjectID and dbo.MetaObject.Machine=ContextualIndicatorValue.MachineID and ContextualIndicatorField.pkid = ContextualIndicatorValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Activity')  AND NameField.Name ='Name'  AND ExecutionIndicatorField.Name ='ExecutionIndicator'  AND ContextualIndicatorField.Name ='ContextualIndicator'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Activity_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Activity_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,ExecutionIndicatorValue.ValueString as ExecutionIndicator,ContextualIndicatorValue.ValueString as ContextualIndicator,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  ExecutionIndicatorField  ON dbo.MetaObject.Class = ExecutionIndicatorField.Class left outer JOIN  
dbo.ObjectFieldValue ExecutionIndicatorValue ON dbo.MetaObject.pkid=ExecutionIndicatorValue.ObjectID and dbo.MetaObject.Machine=ExecutionIndicatorValue.MachineID and ExecutionIndicatorField.pkid = ExecutionIndicatorValue.fieldid  
 INNER JOIN  dbo.Field  ContextualIndicatorField  ON dbo.MetaObject.Class = ContextualIndicatorField.Class left outer JOIN  
dbo.ObjectFieldValue ContextualIndicatorValue ON dbo.MetaObject.pkid=ContextualIndicatorValue.ObjectID and dbo.MetaObject.Machine=ContextualIndicatorValue.MachineID and ContextualIndicatorField.pkid = ContextualIndicatorValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Activity')  AND NameField.Name ='Name'  AND ExecutionIndicatorField.Name ='ExecutionIndicator'  AND ContextualIndicatorField.Name ='ContextualIndicator'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Attribute_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Attribute_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,DomainTypeValue.ValueString as DomainType,DomainDefValue.ValueString as DomainDef,LengthValue.ValueInt as Length,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  DomainTypeField  ON dbo.MetaObject.Class = DomainTypeField.Class left outer JOIN  
dbo.ObjectFieldValue DomainTypeValue ON dbo.MetaObject.pkid=DomainTypeValue.ObjectID and dbo.MetaObject.Machine=DomainTypeValue.MachineID and DomainTypeField.pkid = DomainTypeValue.fieldid  
 INNER JOIN  dbo.Field  DomainDefField  ON dbo.MetaObject.Class = DomainDefField.Class left outer JOIN  
dbo.ObjectFieldValue DomainDefValue ON dbo.MetaObject.pkid=DomainDefValue.ObjectID and dbo.MetaObject.Machine=DomainDefValue.MachineID and DomainDefField.pkid = DomainDefValue.fieldid  
 INNER JOIN  dbo.Field  LengthField  ON dbo.MetaObject.Class = LengthField.Class left outer JOIN  
dbo.ObjectFieldValue LengthValue ON dbo.MetaObject.pkid=LengthValue.ObjectID and dbo.MetaObject.Machine=LengthValue.MachineID and LengthField.pkid = LengthValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Attribute')  AND NameField.Name ='Name'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND DomainTypeField.Name ='DomainType'  AND DomainDefField.Name ='DomainDef'  AND LengthField.Name ='Length'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Attribute_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Attribute_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,DomainTypeValue.ValueString as DomainType,DomainDefValue.ValueString as DomainDef,LengthValue.ValueInt as Length,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  DomainTypeField  ON dbo.MetaObject.Class = DomainTypeField.Class left outer JOIN  
dbo.ObjectFieldValue DomainTypeValue ON dbo.MetaObject.pkid=DomainTypeValue.ObjectID and dbo.MetaObject.Machine=DomainTypeValue.MachineID and DomainTypeField.pkid = DomainTypeValue.fieldid  
 INNER JOIN  dbo.Field  DomainDefField  ON dbo.MetaObject.Class = DomainDefField.Class left outer JOIN  
dbo.ObjectFieldValue DomainDefValue ON dbo.MetaObject.pkid=DomainDefValue.ObjectID and dbo.MetaObject.Machine=DomainDefValue.MachineID and DomainDefField.pkid = DomainDefValue.fieldid  
 INNER JOIN  dbo.Field  LengthField  ON dbo.MetaObject.Class = LengthField.Class left outer JOIN  
dbo.ObjectFieldValue LengthValue ON dbo.MetaObject.pkid=LengthValue.ObjectID and dbo.MetaObject.Machine=LengthValue.MachineID and LengthField.pkid = LengthValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Attribute')  AND NameField.Name ='Name'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND DomainTypeField.Name ='DomainType'  AND DomainDefField.Name ='DomainDef'  AND LengthField.Name ='Length'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_CAD_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_CAD_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID FROM dbo.MetaObject 
 WHERE (dbo.MetaObject.Class = 'CAD') 

GO
/****** Object:  View [dbo].[METAView_CAD_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_CAD_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID FROM dbo.MetaObject 
 WHERE (dbo.MetaObject.Class = 'CAD') 

GO
/****** Object:  View [dbo].[METAView_CADReal_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_CADReal_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID FROM dbo.MetaObject 
 WHERE (dbo.MetaObject.Class = 'CADReal') 

GO
/****** Object:  View [dbo].[METAView_CADReal_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_CADReal_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID FROM dbo.MetaObject 
 WHERE (dbo.MetaObject.Class = 'CADReal') 

GO
/****** Object:  View [dbo].[METAView_CategoryFactor_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_CategoryFactor_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'CategoryFactor')  AND NameField.Name ='Name'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_CategoryFactor_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_CategoryFactor_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'CategoryFactor')  AND NameField.Name ='Name'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Competency_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Competency_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,TypeValue.ValueString as Type,LevelValue.ValueString as Level,GapTypeValue.ValueString as GapType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3 FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  TypeField  ON dbo.MetaObject.Class = TypeField.Class left outer JOIN  
dbo.ObjectFieldValue TypeValue ON dbo.MetaObject.pkid=TypeValue.ObjectID and dbo.MetaObject.Machine=TypeValue.MachineID and TypeField.pkid = TypeValue.fieldid  
 INNER JOIN  dbo.Field  LevelField  ON dbo.MetaObject.Class = LevelField.Class left outer JOIN  
dbo.ObjectFieldValue LevelValue ON dbo.MetaObject.pkid=LevelValue.ObjectID and dbo.MetaObject.Machine=LevelValue.MachineID and LevelField.pkid = LevelValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 WHERE (dbo.MetaObject.Class = 'Competency')  AND NameField.Name ='Name'  AND TypeField.Name ='Type'  AND LevelField.Name ='Level'  AND GapTypeField.Name ='GapType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3' 

GO
/****** Object:  View [dbo].[METAView_Competency_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Competency_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,TypeValue.ValueString as Type,LevelValue.ValueString as Level,GapTypeValue.ValueString as GapType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3 FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  TypeField  ON dbo.MetaObject.Class = TypeField.Class left outer JOIN  
dbo.ObjectFieldValue TypeValue ON dbo.MetaObject.pkid=TypeValue.ObjectID and dbo.MetaObject.Machine=TypeValue.MachineID and TypeField.pkid = TypeValue.fieldid  
 INNER JOIN  dbo.Field  LevelField  ON dbo.MetaObject.Class = LevelField.Class left outer JOIN  
dbo.ObjectFieldValue LevelValue ON dbo.MetaObject.pkid=LevelValue.ObjectID and dbo.MetaObject.Machine=LevelValue.MachineID and LevelField.pkid = LevelValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 WHERE (dbo.MetaObject.Class = 'Competency')  AND NameField.Name ='Name'  AND TypeField.Name ='Type'  AND LevelField.Name ='Level'  AND GapTypeField.Name ='GapType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3' 

GO
/****** Object:  View [dbo].[METAView_Condition_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Condition_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,ValueValue.ValueString as Value,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  ValueField  ON dbo.MetaObject.Class = ValueField.Class left outer JOIN  
dbo.ObjectFieldValue ValueValue ON dbo.MetaObject.pkid=ValueValue.ObjectID and dbo.MetaObject.Machine=ValueValue.MachineID and ValueField.pkid = ValueValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Condition')  AND ValueField.Name ='Value'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Condition_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Condition_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,ValueValue.ValueString as Value,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  ValueField  ON dbo.MetaObject.Class = ValueField.Class left outer JOIN  
dbo.ObjectFieldValue ValueValue ON dbo.MetaObject.pkid=ValueValue.ObjectID and dbo.MetaObject.Machine=ValueValue.MachineID and ValueField.pkid = ValueValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Condition')  AND ValueField.Name ='Value'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Conditional_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Conditional_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,ConditionalTypeValue.ValueString as ConditionalType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  ConditionalTypeField  ON dbo.MetaObject.Class = ConditionalTypeField.Class left outer JOIN  
dbo.ObjectFieldValue ConditionalTypeValue ON dbo.MetaObject.pkid=ConditionalTypeValue.ObjectID and dbo.MetaObject.Machine=ConditionalTypeValue.MachineID and ConditionalTypeField.pkid = ConditionalTypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Conditional')  AND NameField.Name ='Name'  AND ConditionalTypeField.Name ='ConditionalType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Conditional_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Conditional_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,ConditionalTypeValue.ValueString as ConditionalType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  ConditionalTypeField  ON dbo.MetaObject.Class = ConditionalTypeField.Class left outer JOIN  
dbo.ObjectFieldValue ConditionalTypeValue ON dbo.MetaObject.pkid=ConditionalTypeValue.ObjectID and dbo.MetaObject.Machine=ConditionalTypeValue.MachineID and ConditionalTypeField.pkid = ConditionalTypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Conditional')  AND NameField.Name ='Name'  AND ConditionalTypeField.Name ='ConditionalType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_ConditionalDescription_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_ConditionalDescription_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,ValueValue.ValueString as Value,SequenceValue.ValueInt as Sequence,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  ValueField  ON dbo.MetaObject.Class = ValueField.Class left outer JOIN  
dbo.ObjectFieldValue ValueValue ON dbo.MetaObject.pkid=ValueValue.ObjectID and dbo.MetaObject.Machine=ValueValue.MachineID and ValueField.pkid = ValueValue.fieldid  
 INNER JOIN  dbo.Field  SequenceField  ON dbo.MetaObject.Class = SequenceField.Class left outer JOIN  
dbo.ObjectFieldValue SequenceValue ON dbo.MetaObject.pkid=SequenceValue.ObjectID and dbo.MetaObject.Machine=SequenceValue.MachineID and SequenceField.pkid = SequenceValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'ConditionalDescription')  AND ValueField.Name ='Value'  AND SequenceField.Name ='Sequence'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_ConditionalDescription_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_ConditionalDescription_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,ValueValue.ValueString as Value,SequenceValue.ValueInt as Sequence,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  ValueField  ON dbo.MetaObject.Class = ValueField.Class left outer JOIN  
dbo.ObjectFieldValue ValueValue ON dbo.MetaObject.pkid=ValueValue.ObjectID and dbo.MetaObject.Machine=ValueValue.MachineID and ValueField.pkid = ValueValue.fieldid  
 INNER JOIN  dbo.Field  SequenceField  ON dbo.MetaObject.Class = SequenceField.Class left outer JOIN  
dbo.ObjectFieldValue SequenceValue ON dbo.MetaObject.pkid=SequenceValue.ObjectID and dbo.MetaObject.Machine=SequenceValue.MachineID and SequenceField.pkid = SequenceValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'ConditionalDescription')  AND ValueField.Name ='Value'  AND SequenceField.Name ='Sequence'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_ConnectionSpeed_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_ConnectionSpeed_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,ValueValue.ValueString as Value,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  ValueField  ON dbo.MetaObject.Class = ValueField.Class left outer JOIN  
dbo.ObjectFieldValue ValueValue ON dbo.MetaObject.pkid=ValueValue.ObjectID and dbo.MetaObject.Machine=ValueValue.MachineID and ValueField.pkid = ValueValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'ConnectionSpeed')  AND ValueField.Name ='Value'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_ConnectionSpeed_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_ConnectionSpeed_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,ValueValue.ValueString as Value,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  ValueField  ON dbo.MetaObject.Class = ValueField.Class left outer JOIN  
dbo.ObjectFieldValue ValueValue ON dbo.MetaObject.pkid=ValueValue.ObjectID and dbo.MetaObject.Machine=ValueValue.MachineID and ValueField.pkid = ValueValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'ConnectionSpeed')  AND ValueField.Name ='Value'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_ConnectionType_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_ConnectionType_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,ValueValue.ValueString as Value,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  ValueField  ON dbo.MetaObject.Class = ValueField.Class left outer JOIN  
dbo.ObjectFieldValue ValueValue ON dbo.MetaObject.pkid=ValueValue.ObjectID and dbo.MetaObject.Machine=ValueValue.MachineID and ValueField.pkid = ValueValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'ConnectionType')  AND ValueField.Name ='Value'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_ConnectionType_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_ConnectionType_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,ValueValue.ValueString as Value,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  ValueField  ON dbo.MetaObject.Class = ValueField.Class left outer JOIN  
dbo.ObjectFieldValue ValueValue ON dbo.MetaObject.pkid=ValueValue.ObjectID and dbo.MetaObject.Machine=ValueValue.MachineID and ValueField.pkid = ValueValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'ConnectionType')  AND ValueField.Name ='Value'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_CSF_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_CSF_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,NumberValue.ValueString as Number,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  NumberField  ON dbo.MetaObject.Class = NumberField.Class left outer JOIN  
dbo.ObjectFieldValue NumberValue ON dbo.MetaObject.pkid=NumberValue.ObjectID and dbo.MetaObject.Machine=NumberValue.MachineID and NumberField.pkid = NumberValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'CSF')  AND NameField.Name ='Name'  AND NumberField.Name ='Number'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_CSF_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_CSF_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,NumberValue.ValueString as Number,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  NumberField  ON dbo.MetaObject.Class = NumberField.Class left outer JOIN  
dbo.ObjectFieldValue NumberValue ON dbo.MetaObject.pkid=NumberValue.ObjectID and dbo.MetaObject.Machine=NumberValue.MachineID and NumberField.pkid = NumberValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'CSF')  AND NameField.Name ='Name'  AND NumberField.Name ='Number'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_DataColumn_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_DataColumn_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,DomainDefValue.ValueString as DomainDef,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,ColumnTypeValue.ValueString as ColumnType,ColumnLengthValue.ValueInt as ColumnLength,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  DomainDefField  ON dbo.MetaObject.Class = DomainDefField.Class left outer JOIN  
dbo.ObjectFieldValue DomainDefValue ON dbo.MetaObject.pkid=DomainDefValue.ObjectID and dbo.MetaObject.Machine=DomainDefValue.MachineID and DomainDefField.pkid = DomainDefValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  ColumnTypeField  ON dbo.MetaObject.Class = ColumnTypeField.Class left outer JOIN  
dbo.ObjectFieldValue ColumnTypeValue ON dbo.MetaObject.pkid=ColumnTypeValue.ObjectID and dbo.MetaObject.Machine=ColumnTypeValue.MachineID and ColumnTypeField.pkid = ColumnTypeValue.fieldid  
 INNER JOIN  dbo.Field  ColumnLengthField  ON dbo.MetaObject.Class = ColumnLengthField.Class left outer JOIN  
dbo.ObjectFieldValue ColumnLengthValue ON dbo.MetaObject.pkid=ColumnLengthValue.ObjectID and dbo.MetaObject.Machine=ColumnLengthValue.MachineID and ColumnLengthField.pkid = ColumnLengthValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'DataColumn')  AND NameField.Name ='Name'  AND DomainDefField.Name ='DomainDef'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND ColumnTypeField.Name ='ColumnType'  AND ColumnLengthField.Name ='ColumnLength'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_DataColumn_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_DataColumn_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,DomainDefValue.ValueString as DomainDef,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,ColumnTypeValue.ValueString as ColumnType,ColumnLengthValue.ValueInt as ColumnLength,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  DomainDefField  ON dbo.MetaObject.Class = DomainDefField.Class left outer JOIN  
dbo.ObjectFieldValue DomainDefValue ON dbo.MetaObject.pkid=DomainDefValue.ObjectID and dbo.MetaObject.Machine=DomainDefValue.MachineID and DomainDefField.pkid = DomainDefValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  ColumnTypeField  ON dbo.MetaObject.Class = ColumnTypeField.Class left outer JOIN  
dbo.ObjectFieldValue ColumnTypeValue ON dbo.MetaObject.pkid=ColumnTypeValue.ObjectID and dbo.MetaObject.Machine=ColumnTypeValue.MachineID and ColumnTypeField.pkid = ColumnTypeValue.fieldid  
 INNER JOIN  dbo.Field  ColumnLengthField  ON dbo.MetaObject.Class = ColumnLengthField.Class left outer JOIN  
dbo.ObjectFieldValue ColumnLengthValue ON dbo.MetaObject.pkid=ColumnLengthValue.ObjectID and dbo.MetaObject.Machine=ColumnLengthValue.MachineID and ColumnLengthField.pkid = ColumnLengthValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'DataColumn')  AND NameField.Name ='Name'  AND DomainDefField.Name ='DomainDef'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND ColumnTypeField.Name ='ColumnType'  AND ColumnLengthField.Name ='ColumnLength'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_DataSchema_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_DataSchema_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,DataSchemaTypeValue.ValueString as DataSchemaType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,ArchPriorityValue.ValueString as ArchPriority,DatabaseTypeValue.ValueString as DatabaseType,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  DataSchemaTypeField  ON dbo.MetaObject.Class = DataSchemaTypeField.Class left outer JOIN  
dbo.ObjectFieldValue DataSchemaTypeValue ON dbo.MetaObject.pkid=DataSchemaTypeValue.ObjectID and dbo.MetaObject.Machine=DataSchemaTypeValue.MachineID and DataSchemaTypeField.pkid = DataSchemaTypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  ArchPriorityField  ON dbo.MetaObject.Class = ArchPriorityField.Class left outer JOIN  
dbo.ObjectFieldValue ArchPriorityValue ON dbo.MetaObject.pkid=ArchPriorityValue.ObjectID and dbo.MetaObject.Machine=ArchPriorityValue.MachineID and ArchPriorityField.pkid = ArchPriorityValue.fieldid  
 INNER JOIN  dbo.Field  DatabaseTypeField  ON dbo.MetaObject.Class = DatabaseTypeField.Class left outer JOIN  
dbo.ObjectFieldValue DatabaseTypeValue ON dbo.MetaObject.pkid=DatabaseTypeValue.ObjectID and dbo.MetaObject.Machine=DatabaseTypeValue.MachineID and DatabaseTypeField.pkid = DatabaseTypeValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'DataSchema')  AND NameField.Name ='Name'  AND DataSchemaTypeField.Name ='DataSchemaType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND ArchPriorityField.Name ='ArchPriority'  AND DatabaseTypeField.Name ='DatabaseType'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_DataSchema_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_DataSchema_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,DataSchemaTypeValue.ValueString as DataSchemaType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,ArchPriorityValue.ValueString as ArchPriority,DatabaseTypeValue.ValueString as DatabaseType,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  DataSchemaTypeField  ON dbo.MetaObject.Class = DataSchemaTypeField.Class left outer JOIN  
dbo.ObjectFieldValue DataSchemaTypeValue ON dbo.MetaObject.pkid=DataSchemaTypeValue.ObjectID and dbo.MetaObject.Machine=DataSchemaTypeValue.MachineID and DataSchemaTypeField.pkid = DataSchemaTypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  ArchPriorityField  ON dbo.MetaObject.Class = ArchPriorityField.Class left outer JOIN  
dbo.ObjectFieldValue ArchPriorityValue ON dbo.MetaObject.pkid=ArchPriorityValue.ObjectID and dbo.MetaObject.Machine=ArchPriorityValue.MachineID and ArchPriorityField.pkid = ArchPriorityValue.fieldid  
 INNER JOIN  dbo.Field  DatabaseTypeField  ON dbo.MetaObject.Class = DatabaseTypeField.Class left outer JOIN  
dbo.ObjectFieldValue DatabaseTypeValue ON dbo.MetaObject.pkid=DatabaseTypeValue.ObjectID and dbo.MetaObject.Machine=DatabaseTypeValue.MachineID and DatabaseTypeField.pkid = DatabaseTypeValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'DataSchema')  AND NameField.Name ='Name'  AND DataSchemaTypeField.Name ='DataSchemaType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND ArchPriorityField.Name ='ArchPriority'  AND DatabaseTypeField.Name ='DatabaseType'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_DataTable_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_DataTable_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,InitialPopulationValue.ValueInt as InitialPopulation,GrowthRateOverTimeValue.ValueString as GrowthRateOverTime,RecordSizeValue.ValueString as RecordSize,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  InitialPopulationField  ON dbo.MetaObject.Class = InitialPopulationField.Class left outer JOIN  
dbo.ObjectFieldValue InitialPopulationValue ON dbo.MetaObject.pkid=InitialPopulationValue.ObjectID and dbo.MetaObject.Machine=InitialPopulationValue.MachineID and InitialPopulationField.pkid = InitialPopulationValue.fieldid  
 INNER JOIN  dbo.Field  GrowthRateOverTimeField  ON dbo.MetaObject.Class = GrowthRateOverTimeField.Class left outer JOIN  
dbo.ObjectFieldValue GrowthRateOverTimeValue ON dbo.MetaObject.pkid=GrowthRateOverTimeValue.ObjectID and dbo.MetaObject.Machine=GrowthRateOverTimeValue.MachineID and GrowthRateOverTimeField.pkid = GrowthRateOverTimeValue.fieldid  
 INNER JOIN  dbo.Field  RecordSizeField  ON dbo.MetaObject.Class = RecordSizeField.Class left outer JOIN  
dbo.ObjectFieldValue RecordSizeValue ON dbo.MetaObject.pkid=RecordSizeValue.ObjectID and dbo.MetaObject.Machine=RecordSizeValue.MachineID and RecordSizeField.pkid = RecordSizeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'DataTable')  AND NameField.Name ='Name'  AND InitialPopulationField.Name ='InitialPopulation'  AND GrowthRateOverTimeField.Name ='GrowthRateOverTime'  AND RecordSizeField.Name ='RecordSize'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_DataTable_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_DataTable_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,InitialPopulationValue.ValueInt as InitialPopulation,GrowthRateOverTimeValue.ValueString as GrowthRateOverTime,RecordSizeValue.ValueString as RecordSize,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  InitialPopulationField  ON dbo.MetaObject.Class = InitialPopulationField.Class left outer JOIN  
dbo.ObjectFieldValue InitialPopulationValue ON dbo.MetaObject.pkid=InitialPopulationValue.ObjectID and dbo.MetaObject.Machine=InitialPopulationValue.MachineID and InitialPopulationField.pkid = InitialPopulationValue.fieldid  
 INNER JOIN  dbo.Field  GrowthRateOverTimeField  ON dbo.MetaObject.Class = GrowthRateOverTimeField.Class left outer JOIN  
dbo.ObjectFieldValue GrowthRateOverTimeValue ON dbo.MetaObject.pkid=GrowthRateOverTimeValue.ObjectID and dbo.MetaObject.Machine=GrowthRateOverTimeValue.MachineID and GrowthRateOverTimeField.pkid = GrowthRateOverTimeValue.fieldid  
 INNER JOIN  dbo.Field  RecordSizeField  ON dbo.MetaObject.Class = RecordSizeField.Class left outer JOIN  
dbo.ObjectFieldValue RecordSizeValue ON dbo.MetaObject.pkid=RecordSizeValue.ObjectID and dbo.MetaObject.Machine=RecordSizeValue.MachineID and RecordSizeField.pkid = RecordSizeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'DataTable')  AND NameField.Name ='Name'  AND InitialPopulationField.Name ='InitialPopulation'  AND GrowthRateOverTimeField.Name ='GrowthRateOverTime'  AND RecordSizeField.Name ='RecordSize'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_DataView_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_DataView_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,DataViewTypeValue.ValueString as DataViewType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  DataViewTypeField  ON dbo.MetaObject.Class = DataViewTypeField.Class left outer JOIN  
dbo.ObjectFieldValue DataViewTypeValue ON dbo.MetaObject.pkid=DataViewTypeValue.ObjectID and dbo.MetaObject.Machine=DataViewTypeValue.MachineID and DataViewTypeField.pkid = DataViewTypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'DataView')  AND NameField.Name ='Name'  AND DataViewTypeField.Name ='DataViewType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_DataView_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_DataView_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,DataViewTypeValue.ValueString as DataViewType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  DataViewTypeField  ON dbo.MetaObject.Class = DataViewTypeField.Class left outer JOIN  
dbo.ObjectFieldValue DataViewTypeValue ON dbo.MetaObject.pkid=DataViewTypeValue.ObjectID and dbo.MetaObject.Machine=DataViewTypeValue.MachineID and DataViewTypeField.pkid = DataViewTypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'DataView')  AND NameField.Name ='Name'  AND DataViewTypeField.Name ='DataViewType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_DatedResponsibility_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_DatedResponsibility_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID FROM dbo.MetaObject 
 WHERE (dbo.MetaObject.Class = 'DatedResponsibility') 

GO
/****** Object:  View [dbo].[METAView_DatedResponsibility_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_DatedResponsibility_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID FROM dbo.MetaObject 
 WHERE (dbo.MetaObject.Class = 'DatedResponsibility') 

GO
/****** Object:  View [dbo].[METAView_Employee_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Employee_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,SurnameValue.ValueString as Surname,TitleValue.ValueString as Title,EmployeeNumberValue.ValueString as EmployeeNumber,IDNumberValue.ValueString as IDNumber,EmailValue.ValueString as Email,TelephoneValue.ValueString as Telephone,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,MobileValue.ValueString as Mobile,FaxValue.ValueString as Fax,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  SurnameField  ON dbo.MetaObject.Class = SurnameField.Class left outer JOIN  
dbo.ObjectFieldValue SurnameValue ON dbo.MetaObject.pkid=SurnameValue.ObjectID and dbo.MetaObject.Machine=SurnameValue.MachineID and SurnameField.pkid = SurnameValue.fieldid  
 INNER JOIN  dbo.Field  TitleField  ON dbo.MetaObject.Class = TitleField.Class left outer JOIN  
dbo.ObjectFieldValue TitleValue ON dbo.MetaObject.pkid=TitleValue.ObjectID and dbo.MetaObject.Machine=TitleValue.MachineID and TitleField.pkid = TitleValue.fieldid  
 INNER JOIN  dbo.Field  EmployeeNumberField  ON dbo.MetaObject.Class = EmployeeNumberField.Class left outer JOIN  
dbo.ObjectFieldValue EmployeeNumberValue ON dbo.MetaObject.pkid=EmployeeNumberValue.ObjectID and dbo.MetaObject.Machine=EmployeeNumberValue.MachineID and EmployeeNumberField.pkid = EmployeeNumberValue.fieldid  
 INNER JOIN  dbo.Field  IDNumberField  ON dbo.MetaObject.Class = IDNumberField.Class left outer JOIN  
dbo.ObjectFieldValue IDNumberValue ON dbo.MetaObject.pkid=IDNumberValue.ObjectID and dbo.MetaObject.Machine=IDNumberValue.MachineID and IDNumberField.pkid = IDNumberValue.fieldid  
 INNER JOIN  dbo.Field  EmailField  ON dbo.MetaObject.Class = EmailField.Class left outer JOIN  
dbo.ObjectFieldValue EmailValue ON dbo.MetaObject.pkid=EmailValue.ObjectID and dbo.MetaObject.Machine=EmailValue.MachineID and EmailField.pkid = EmailValue.fieldid  
 INNER JOIN  dbo.Field  TelephoneField  ON dbo.MetaObject.Class = TelephoneField.Class left outer JOIN  
dbo.ObjectFieldValue TelephoneValue ON dbo.MetaObject.pkid=TelephoneValue.ObjectID and dbo.MetaObject.Machine=TelephoneValue.MachineID and TelephoneField.pkid = TelephoneValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  MobileField  ON dbo.MetaObject.Class = MobileField.Class left outer JOIN  
dbo.ObjectFieldValue MobileValue ON dbo.MetaObject.pkid=MobileValue.ObjectID and dbo.MetaObject.Machine=MobileValue.MachineID and MobileField.pkid = MobileValue.fieldid  
 INNER JOIN  dbo.Field  FaxField  ON dbo.MetaObject.Class = FaxField.Class left outer JOIN  
dbo.ObjectFieldValue FaxValue ON dbo.MetaObject.pkid=FaxValue.ObjectID and dbo.MetaObject.Machine=FaxValue.MachineID and FaxField.pkid = FaxValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Employee')  AND NameField.Name ='Name'  AND SurnameField.Name ='Surname'  AND TitleField.Name ='Title'  AND EmployeeNumberField.Name ='EmployeeNumber'  AND IDNumberField.Name ='IDNumber'  AND EmailField.Name ='Email'  AND TelephoneField.Name ='Telephone'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND MobileField.Name ='Mobile'  AND FaxField.Name ='Fax'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Employee_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Employee_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,SurnameValue.ValueString as Surname,TitleValue.ValueString as Title,EmployeeNumberValue.ValueString as EmployeeNumber,IDNumberValue.ValueString as IDNumber,EmailValue.ValueString as Email,TelephoneValue.ValueString as Telephone,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,MobileValue.ValueString as Mobile,FaxValue.ValueString as Fax,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  SurnameField  ON dbo.MetaObject.Class = SurnameField.Class left outer JOIN  
dbo.ObjectFieldValue SurnameValue ON dbo.MetaObject.pkid=SurnameValue.ObjectID and dbo.MetaObject.Machine=SurnameValue.MachineID and SurnameField.pkid = SurnameValue.fieldid  
 INNER JOIN  dbo.Field  TitleField  ON dbo.MetaObject.Class = TitleField.Class left outer JOIN  
dbo.ObjectFieldValue TitleValue ON dbo.MetaObject.pkid=TitleValue.ObjectID and dbo.MetaObject.Machine=TitleValue.MachineID and TitleField.pkid = TitleValue.fieldid  
 INNER JOIN  dbo.Field  EmployeeNumberField  ON dbo.MetaObject.Class = EmployeeNumberField.Class left outer JOIN  
dbo.ObjectFieldValue EmployeeNumberValue ON dbo.MetaObject.pkid=EmployeeNumberValue.ObjectID and dbo.MetaObject.Machine=EmployeeNumberValue.MachineID and EmployeeNumberField.pkid = EmployeeNumberValue.fieldid  
 INNER JOIN  dbo.Field  IDNumberField  ON dbo.MetaObject.Class = IDNumberField.Class left outer JOIN  
dbo.ObjectFieldValue IDNumberValue ON dbo.MetaObject.pkid=IDNumberValue.ObjectID and dbo.MetaObject.Machine=IDNumberValue.MachineID and IDNumberField.pkid = IDNumberValue.fieldid  
 INNER JOIN  dbo.Field  EmailField  ON dbo.MetaObject.Class = EmailField.Class left outer JOIN  
dbo.ObjectFieldValue EmailValue ON dbo.MetaObject.pkid=EmailValue.ObjectID and dbo.MetaObject.Machine=EmailValue.MachineID and EmailField.pkid = EmailValue.fieldid  
 INNER JOIN  dbo.Field  TelephoneField  ON dbo.MetaObject.Class = TelephoneField.Class left outer JOIN  
dbo.ObjectFieldValue TelephoneValue ON dbo.MetaObject.pkid=TelephoneValue.ObjectID and dbo.MetaObject.Machine=TelephoneValue.MachineID and TelephoneField.pkid = TelephoneValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  MobileField  ON dbo.MetaObject.Class = MobileField.Class left outer JOIN  
dbo.ObjectFieldValue MobileValue ON dbo.MetaObject.pkid=MobileValue.ObjectID and dbo.MetaObject.Machine=MobileValue.MachineID and MobileField.pkid = MobileValue.fieldid  
 INNER JOIN  dbo.Field  FaxField  ON dbo.MetaObject.Class = FaxField.Class left outer JOIN  
dbo.ObjectFieldValue FaxValue ON dbo.MetaObject.pkid=FaxValue.ObjectID and dbo.MetaObject.Machine=FaxValue.MachineID and FaxField.pkid = FaxValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Employee')  AND NameField.Name ='Name'  AND SurnameField.Name ='Surname'  AND TitleField.Name ='Title'  AND EmployeeNumberField.Name ='EmployeeNumber'  AND IDNumberField.Name ='IDNumber'  AND EmailField.Name ='Email'  AND TelephoneField.Name ='Telephone'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND MobileField.Name ='Mobile'  AND FaxField.Name ='Fax'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Entity_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Entity_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,EntityTypeValue.ValueString as EntityType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  EntityTypeField  ON dbo.MetaObject.Class = EntityTypeField.Class left outer JOIN  
dbo.ObjectFieldValue EntityTypeValue ON dbo.MetaObject.pkid=EntityTypeValue.ObjectID and dbo.MetaObject.Machine=EntityTypeValue.MachineID and EntityTypeField.pkid = EntityTypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Entity')  AND NameField.Name ='Name'  AND EntityTypeField.Name ='EntityType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Entity_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Entity_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,EntityTypeValue.ValueString as EntityType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  EntityTypeField  ON dbo.MetaObject.Class = EntityTypeField.Class left outer JOIN  
dbo.ObjectFieldValue EntityTypeValue ON dbo.MetaObject.pkid=EntityTypeValue.ObjectID and dbo.MetaObject.Machine=EntityTypeValue.MachineID and EntityTypeField.pkid = EntityTypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Entity')  AND NameField.Name ='Name'  AND EntityTypeField.Name ='EntityType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Environment_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Environment_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Environment')  AND NameField.Name ='Name'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Environment_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Environment_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Environment')  AND NameField.Name ='Name'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_EnvironmentCategory_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_EnvironmentCategory_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'EnvironmentCategory')  AND NameField.Name ='Name'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_EnvironmentCategory_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_EnvironmentCategory_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'EnvironmentCategory')  AND NameField.Name ='Name'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Exception_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Exception_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Exception')  AND NameField.Name ='Name'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Exception_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Exception_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Exception')  AND NameField.Name ='Name'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_FlowDescription_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_FlowDescription_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,SequenceValue.ValueString as Sequence,ConditionValue.ValueString as Condition,DescriptionValue.ValueString as Description,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,TimeIndicatorValue.ValueString as TimeIndicator,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  SequenceField  ON dbo.MetaObject.Class = SequenceField.Class left outer JOIN  
dbo.ObjectFieldValue SequenceValue ON dbo.MetaObject.pkid=SequenceValue.ObjectID and dbo.MetaObject.Machine=SequenceValue.MachineID and SequenceField.pkid = SequenceValue.fieldid  
 INNER JOIN  dbo.Field  ConditionField  ON dbo.MetaObject.Class = ConditionField.Class left outer JOIN  
dbo.ObjectFieldValue ConditionValue ON dbo.MetaObject.pkid=ConditionValue.ObjectID and dbo.MetaObject.Machine=ConditionValue.MachineID and ConditionField.pkid = ConditionValue.fieldid  
 INNER JOIN  dbo.Field  DescriptionField  ON dbo.MetaObject.Class = DescriptionField.Class left outer JOIN  
dbo.ObjectFieldValue DescriptionValue ON dbo.MetaObject.pkid=DescriptionValue.ObjectID and dbo.MetaObject.Machine=DescriptionValue.MachineID and DescriptionField.pkid = DescriptionValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  TimeIndicatorField  ON dbo.MetaObject.Class = TimeIndicatorField.Class left outer JOIN  
dbo.ObjectFieldValue TimeIndicatorValue ON dbo.MetaObject.pkid=TimeIndicatorValue.ObjectID and dbo.MetaObject.Machine=TimeIndicatorValue.MachineID and TimeIndicatorField.pkid = TimeIndicatorValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'FlowDescription')  AND SequenceField.Name ='Sequence'  AND ConditionField.Name ='Condition'  AND DescriptionField.Name ='Description'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND TimeIndicatorField.Name ='TimeIndicator'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_FlowDescription_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_FlowDescription_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,SequenceValue.ValueString as Sequence,ConditionValue.ValueString as Condition,DescriptionValue.ValueString as Description,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,TimeIndicatorValue.ValueString as TimeIndicator,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  SequenceField  ON dbo.MetaObject.Class = SequenceField.Class left outer JOIN  
dbo.ObjectFieldValue SequenceValue ON dbo.MetaObject.pkid=SequenceValue.ObjectID and dbo.MetaObject.Machine=SequenceValue.MachineID and SequenceField.pkid = SequenceValue.fieldid  
 INNER JOIN  dbo.Field  ConditionField  ON dbo.MetaObject.Class = ConditionField.Class left outer JOIN  
dbo.ObjectFieldValue ConditionValue ON dbo.MetaObject.pkid=ConditionValue.ObjectID and dbo.MetaObject.Machine=ConditionValue.MachineID and ConditionField.pkid = ConditionValue.fieldid  
 INNER JOIN  dbo.Field  DescriptionField  ON dbo.MetaObject.Class = DescriptionField.Class left outer JOIN  
dbo.ObjectFieldValue DescriptionValue ON dbo.MetaObject.pkid=DescriptionValue.ObjectID and dbo.MetaObject.Machine=DescriptionValue.MachineID and DescriptionField.pkid = DescriptionValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  TimeIndicatorField  ON dbo.MetaObject.Class = TimeIndicatorField.Class left outer JOIN  
dbo.ObjectFieldValue TimeIndicatorValue ON dbo.MetaObject.pkid=TimeIndicatorValue.ObjectID and dbo.MetaObject.Machine=TimeIndicatorValue.MachineID and TimeIndicatorField.pkid = TimeIndicatorValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'FlowDescription')  AND SequenceField.Name ='Sequence'  AND ConditionField.Name ='Condition'  AND DescriptionField.Name ='Description'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND TimeIndicatorField.Name ='TimeIndicator'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Function_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Function_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,ContextualIndicatorValue.ValueString as ContextualIndicator,FunctionCriticalityValue.ValueInt as FunctionCriticality,ManagementAbilityValue.ValueString as ManagementAbility,InfoAvailabilityValue.ValueString as InfoAvailability,LegalAspectsValue.ValueString as LegalAspects,TechnologyValue.ValueString as Technology,BudgetValue.ValueString as Budget,EnergyValue.ValueString as Energy,RawMaterialValue.ValueString as RawMaterial,SkillAvailabilityValue.ValueString as SkillAvailability,EfficiencyValue.ValueString as Efficiency,EffectivenessValue.ValueString as Effectiveness,ManpowerValue.ValueString as Manpower,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,EnvironmentIndValue.ValueString as EnvironmentInd,GovernanceMechValue.ValueString as GovernanceMech,CapitalAvailabilityValue.ValueString as CapitalAvailability,CompetenciesValue.ValueString as Competencies,EthicsValue.ValueString as Ethics,FacilitiesValue.ValueString as Facilities,ServicesUsageValue.ValueString as ServicesUsage,EquipmentValue.ValueString as Equipment,TimeIndicatorValue.ValueString as TimeIndicator,EffeciencyValue.ValueString as Effeciency,EffectivinessValue.ValueString as Effectiviness,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  ContextualIndicatorField  ON dbo.MetaObject.Class = ContextualIndicatorField.Class left outer JOIN  
dbo.ObjectFieldValue ContextualIndicatorValue ON dbo.MetaObject.pkid=ContextualIndicatorValue.ObjectID and dbo.MetaObject.Machine=ContextualIndicatorValue.MachineID and ContextualIndicatorField.pkid = ContextualIndicatorValue.fieldid  
 INNER JOIN  dbo.Field  FunctionCriticalityField  ON dbo.MetaObject.Class = FunctionCriticalityField.Class left outer JOIN  
dbo.ObjectFieldValue FunctionCriticalityValue ON dbo.MetaObject.pkid=FunctionCriticalityValue.ObjectID and dbo.MetaObject.Machine=FunctionCriticalityValue.MachineID and FunctionCriticalityField.pkid = FunctionCriticalityValue.fieldid  
 INNER JOIN  dbo.Field  ManagementAbilityField  ON dbo.MetaObject.Class = ManagementAbilityField.Class left outer JOIN  
dbo.ObjectFieldValue ManagementAbilityValue ON dbo.MetaObject.pkid=ManagementAbilityValue.ObjectID and dbo.MetaObject.Machine=ManagementAbilityValue.MachineID and ManagementAbilityField.pkid = ManagementAbilityValue.fieldid  
 INNER JOIN  dbo.Field  InfoAvailabilityField  ON dbo.MetaObject.Class = InfoAvailabilityField.Class left outer JOIN  
dbo.ObjectFieldValue InfoAvailabilityValue ON dbo.MetaObject.pkid=InfoAvailabilityValue.ObjectID and dbo.MetaObject.Machine=InfoAvailabilityValue.MachineID and InfoAvailabilityField.pkid = InfoAvailabilityValue.fieldid  
 INNER JOIN  dbo.Field  LegalAspectsField  ON dbo.MetaObject.Class = LegalAspectsField.Class left outer JOIN  
dbo.ObjectFieldValue LegalAspectsValue ON dbo.MetaObject.pkid=LegalAspectsValue.ObjectID and dbo.MetaObject.Machine=LegalAspectsValue.MachineID and LegalAspectsField.pkid = LegalAspectsValue.fieldid  
 INNER JOIN  dbo.Field  TechnologyField  ON dbo.MetaObject.Class = TechnologyField.Class left outer JOIN  
dbo.ObjectFieldValue TechnologyValue ON dbo.MetaObject.pkid=TechnologyValue.ObjectID and dbo.MetaObject.Machine=TechnologyValue.MachineID and TechnologyField.pkid = TechnologyValue.fieldid  
 INNER JOIN  dbo.Field  BudgetField  ON dbo.MetaObject.Class = BudgetField.Class left outer JOIN  
dbo.ObjectFieldValue BudgetValue ON dbo.MetaObject.pkid=BudgetValue.ObjectID and dbo.MetaObject.Machine=BudgetValue.MachineID and BudgetField.pkid = BudgetValue.fieldid  
 INNER JOIN  dbo.Field  EnergyField  ON dbo.MetaObject.Class = EnergyField.Class left outer JOIN  
dbo.ObjectFieldValue EnergyValue ON dbo.MetaObject.pkid=EnergyValue.ObjectID and dbo.MetaObject.Machine=EnergyValue.MachineID and EnergyField.pkid = EnergyValue.fieldid  
 INNER JOIN  dbo.Field  RawMaterialField  ON dbo.MetaObject.Class = RawMaterialField.Class left outer JOIN  
dbo.ObjectFieldValue RawMaterialValue ON dbo.MetaObject.pkid=RawMaterialValue.ObjectID and dbo.MetaObject.Machine=RawMaterialValue.MachineID and RawMaterialField.pkid = RawMaterialValue.fieldid  
 INNER JOIN  dbo.Field  SkillAvailabilityField  ON dbo.MetaObject.Class = SkillAvailabilityField.Class left outer JOIN  
dbo.ObjectFieldValue SkillAvailabilityValue ON dbo.MetaObject.pkid=SkillAvailabilityValue.ObjectID and dbo.MetaObject.Machine=SkillAvailabilityValue.MachineID and SkillAvailabilityField.pkid = SkillAvailabilityValue.fieldid  
 INNER JOIN  dbo.Field  EfficiencyField  ON dbo.MetaObject.Class = EfficiencyField.Class left outer JOIN  
dbo.ObjectFieldValue EfficiencyValue ON dbo.MetaObject.pkid=EfficiencyValue.ObjectID and dbo.MetaObject.Machine=EfficiencyValue.MachineID and EfficiencyField.pkid = EfficiencyValue.fieldid  
 INNER JOIN  dbo.Field  EffectivenessField  ON dbo.MetaObject.Class = EffectivenessField.Class left outer JOIN  
dbo.ObjectFieldValue EffectivenessValue ON dbo.MetaObject.pkid=EffectivenessValue.ObjectID and dbo.MetaObject.Machine=EffectivenessValue.MachineID and EffectivenessField.pkid = EffectivenessValue.fieldid  
 INNER JOIN  dbo.Field  ManpowerField  ON dbo.MetaObject.Class = ManpowerField.Class left outer JOIN  
dbo.ObjectFieldValue ManpowerValue ON dbo.MetaObject.pkid=ManpowerValue.ObjectID and dbo.MetaObject.Machine=ManpowerValue.MachineID and ManpowerField.pkid = ManpowerValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  EnvironmentIndField  ON dbo.MetaObject.Class = EnvironmentIndField.Class left outer JOIN  
dbo.ObjectFieldValue EnvironmentIndValue ON dbo.MetaObject.pkid=EnvironmentIndValue.ObjectID and dbo.MetaObject.Machine=EnvironmentIndValue.MachineID and EnvironmentIndField.pkid = EnvironmentIndValue.fieldid  
 INNER JOIN  dbo.Field  GovernanceMechField  ON dbo.MetaObject.Class = GovernanceMechField.Class left outer JOIN  
dbo.ObjectFieldValue GovernanceMechValue ON dbo.MetaObject.pkid=GovernanceMechValue.ObjectID and dbo.MetaObject.Machine=GovernanceMechValue.MachineID and GovernanceMechField.pkid = GovernanceMechValue.fieldid  
 INNER JOIN  dbo.Field  CapitalAvailabilityField  ON dbo.MetaObject.Class = CapitalAvailabilityField.Class left outer JOIN  
dbo.ObjectFieldValue CapitalAvailabilityValue ON dbo.MetaObject.pkid=CapitalAvailabilityValue.ObjectID and dbo.MetaObject.Machine=CapitalAvailabilityValue.MachineID and CapitalAvailabilityField.pkid = CapitalAvailabilityValue.fieldid  
 INNER JOIN  dbo.Field  CompetenciesField  ON dbo.MetaObject.Class = CompetenciesField.Class left outer JOIN  
dbo.ObjectFieldValue CompetenciesValue ON dbo.MetaObject.pkid=CompetenciesValue.ObjectID and dbo.MetaObject.Machine=CompetenciesValue.MachineID and CompetenciesField.pkid = CompetenciesValue.fieldid  
 INNER JOIN  dbo.Field  EthicsField  ON dbo.MetaObject.Class = EthicsField.Class left outer JOIN  
dbo.ObjectFieldValue EthicsValue ON dbo.MetaObject.pkid=EthicsValue.ObjectID and dbo.MetaObject.Machine=EthicsValue.MachineID and EthicsField.pkid = EthicsValue.fieldid  
 INNER JOIN  dbo.Field  FacilitiesField  ON dbo.MetaObject.Class = FacilitiesField.Class left outer JOIN  
dbo.ObjectFieldValue FacilitiesValue ON dbo.MetaObject.pkid=FacilitiesValue.ObjectID and dbo.MetaObject.Machine=FacilitiesValue.MachineID and FacilitiesField.pkid = FacilitiesValue.fieldid  
 INNER JOIN  dbo.Field  ServicesUsageField  ON dbo.MetaObject.Class = ServicesUsageField.Class left outer JOIN  
dbo.ObjectFieldValue ServicesUsageValue ON dbo.MetaObject.pkid=ServicesUsageValue.ObjectID and dbo.MetaObject.Machine=ServicesUsageValue.MachineID and ServicesUsageField.pkid = ServicesUsageValue.fieldid  
 INNER JOIN  dbo.Field  EquipmentField  ON dbo.MetaObject.Class = EquipmentField.Class left outer JOIN  
dbo.ObjectFieldValue EquipmentValue ON dbo.MetaObject.pkid=EquipmentValue.ObjectID and dbo.MetaObject.Machine=EquipmentValue.MachineID and EquipmentField.pkid = EquipmentValue.fieldid  
 INNER JOIN  dbo.Field  TimeIndicatorField  ON dbo.MetaObject.Class = TimeIndicatorField.Class left outer JOIN  
dbo.ObjectFieldValue TimeIndicatorValue ON dbo.MetaObject.pkid=TimeIndicatorValue.ObjectID and dbo.MetaObject.Machine=TimeIndicatorValue.MachineID and TimeIndicatorField.pkid = TimeIndicatorValue.fieldid  
 INNER JOIN  dbo.Field  EffeciencyField  ON dbo.MetaObject.Class = EffeciencyField.Class left outer JOIN  
dbo.ObjectFieldValue EffeciencyValue ON dbo.MetaObject.pkid=EffeciencyValue.ObjectID and dbo.MetaObject.Machine=EffeciencyValue.MachineID and EffeciencyField.pkid = EffeciencyValue.fieldid  
 INNER JOIN  dbo.Field  EffectivinessField  ON dbo.MetaObject.Class = EffectivinessField.Class left outer JOIN  
dbo.ObjectFieldValue EffectivinessValue ON dbo.MetaObject.pkid=EffectivinessValue.ObjectID and dbo.MetaObject.Machine=EffectivinessValue.MachineID and EffectivinessField.pkid = EffectivinessValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Function')  AND NameField.Name ='Name'  AND ContextualIndicatorField.Name ='ContextualIndicator'  AND FunctionCriticalityField.Name ='FunctionCriticality'  AND ManagementAbilityField.Name ='ManagementAbility'  AND InfoAvailabilityField.Name ='InfoAvailability'  AND LegalAspectsField.Name ='LegalAspects'  AND TechnologyField.Name ='Technology'  AND BudgetField.Name ='Budget'  AND EnergyField.Name ='Energy'  AND RawMaterialField.Name ='RawMaterial'  AND SkillAvailabilityField.Name ='SkillAvailability'  AND EfficiencyField.Name ='Efficiency'  AND EffectivenessField.Name ='Effectiveness'  AND ManpowerField.Name ='Manpower'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND EnvironmentIndField.Name ='EnvironmentInd'  AND GovernanceMechField.Name ='GovernanceMech'  AND CapitalAvailabilityField.Name ='CapitalAvailability'  AND CompetenciesField.Name ='Competencies'  AND EthicsField.Name ='Ethics'  AND FacilitiesField.Name ='Facilities'  AND ServicesUsageField.Name ='ServicesUsage'  AND EquipmentField.Name ='Equipment'  AND TimeIndicatorField.Name ='TimeIndicator'  AND EffeciencyField.Name ='Effeciency'  AND EffectivinessField.Name ='Effectiviness'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Function_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Function_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,ContextualIndicatorValue.ValueString as ContextualIndicator,FunctionCriticalityValue.ValueInt as FunctionCriticality,ManagementAbilityValue.ValueString as ManagementAbility,InfoAvailabilityValue.ValueString as InfoAvailability,LegalAspectsValue.ValueString as LegalAspects,TechnologyValue.ValueString as Technology,BudgetValue.ValueString as Budget,EnergyValue.ValueString as Energy,RawMaterialValue.ValueString as RawMaterial,SkillAvailabilityValue.ValueString as SkillAvailability,EfficiencyValue.ValueString as Efficiency,EffectivenessValue.ValueString as Effectiveness,ManpowerValue.ValueString as Manpower,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,EnvironmentIndValue.ValueString as EnvironmentInd,GovernanceMechValue.ValueString as GovernanceMech,CapitalAvailabilityValue.ValueString as CapitalAvailability,CompetenciesValue.ValueString as Competencies,EthicsValue.ValueString as Ethics,FacilitiesValue.ValueString as Facilities,ServicesUsageValue.ValueString as ServicesUsage,EquipmentValue.ValueString as Equipment,TimeIndicatorValue.ValueString as TimeIndicator,EffeciencyValue.ValueString as Effeciency,EffectivinessValue.ValueString as Effectiviness,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  ContextualIndicatorField  ON dbo.MetaObject.Class = ContextualIndicatorField.Class left outer JOIN  
dbo.ObjectFieldValue ContextualIndicatorValue ON dbo.MetaObject.pkid=ContextualIndicatorValue.ObjectID and dbo.MetaObject.Machine=ContextualIndicatorValue.MachineID and ContextualIndicatorField.pkid = ContextualIndicatorValue.fieldid  
 INNER JOIN  dbo.Field  FunctionCriticalityField  ON dbo.MetaObject.Class = FunctionCriticalityField.Class left outer JOIN  
dbo.ObjectFieldValue FunctionCriticalityValue ON dbo.MetaObject.pkid=FunctionCriticalityValue.ObjectID and dbo.MetaObject.Machine=FunctionCriticalityValue.MachineID and FunctionCriticalityField.pkid = FunctionCriticalityValue.fieldid  
 INNER JOIN  dbo.Field  ManagementAbilityField  ON dbo.MetaObject.Class = ManagementAbilityField.Class left outer JOIN  
dbo.ObjectFieldValue ManagementAbilityValue ON dbo.MetaObject.pkid=ManagementAbilityValue.ObjectID and dbo.MetaObject.Machine=ManagementAbilityValue.MachineID and ManagementAbilityField.pkid = ManagementAbilityValue.fieldid  
 INNER JOIN  dbo.Field  InfoAvailabilityField  ON dbo.MetaObject.Class = InfoAvailabilityField.Class left outer JOIN  
dbo.ObjectFieldValue InfoAvailabilityValue ON dbo.MetaObject.pkid=InfoAvailabilityValue.ObjectID and dbo.MetaObject.Machine=InfoAvailabilityValue.MachineID and InfoAvailabilityField.pkid = InfoAvailabilityValue.fieldid  
 INNER JOIN  dbo.Field  LegalAspectsField  ON dbo.MetaObject.Class = LegalAspectsField.Class left outer JOIN  
dbo.ObjectFieldValue LegalAspectsValue ON dbo.MetaObject.pkid=LegalAspectsValue.ObjectID and dbo.MetaObject.Machine=LegalAspectsValue.MachineID and LegalAspectsField.pkid = LegalAspectsValue.fieldid  
 INNER JOIN  dbo.Field  TechnologyField  ON dbo.MetaObject.Class = TechnologyField.Class left outer JOIN  
dbo.ObjectFieldValue TechnologyValue ON dbo.MetaObject.pkid=TechnologyValue.ObjectID and dbo.MetaObject.Machine=TechnologyValue.MachineID and TechnologyField.pkid = TechnologyValue.fieldid  
 INNER JOIN  dbo.Field  BudgetField  ON dbo.MetaObject.Class = BudgetField.Class left outer JOIN  
dbo.ObjectFieldValue BudgetValue ON dbo.MetaObject.pkid=BudgetValue.ObjectID and dbo.MetaObject.Machine=BudgetValue.MachineID and BudgetField.pkid = BudgetValue.fieldid  
 INNER JOIN  dbo.Field  EnergyField  ON dbo.MetaObject.Class = EnergyField.Class left outer JOIN  
dbo.ObjectFieldValue EnergyValue ON dbo.MetaObject.pkid=EnergyValue.ObjectID and dbo.MetaObject.Machine=EnergyValue.MachineID and EnergyField.pkid = EnergyValue.fieldid  
 INNER JOIN  dbo.Field  RawMaterialField  ON dbo.MetaObject.Class = RawMaterialField.Class left outer JOIN  
dbo.ObjectFieldValue RawMaterialValue ON dbo.MetaObject.pkid=RawMaterialValue.ObjectID and dbo.MetaObject.Machine=RawMaterialValue.MachineID and RawMaterialField.pkid = RawMaterialValue.fieldid  
 INNER JOIN  dbo.Field  SkillAvailabilityField  ON dbo.MetaObject.Class = SkillAvailabilityField.Class left outer JOIN  
dbo.ObjectFieldValue SkillAvailabilityValue ON dbo.MetaObject.pkid=SkillAvailabilityValue.ObjectID and dbo.MetaObject.Machine=SkillAvailabilityValue.MachineID and SkillAvailabilityField.pkid = SkillAvailabilityValue.fieldid  
 INNER JOIN  dbo.Field  EfficiencyField  ON dbo.MetaObject.Class = EfficiencyField.Class left outer JOIN  
dbo.ObjectFieldValue EfficiencyValue ON dbo.MetaObject.pkid=EfficiencyValue.ObjectID and dbo.MetaObject.Machine=EfficiencyValue.MachineID and EfficiencyField.pkid = EfficiencyValue.fieldid  
 INNER JOIN  dbo.Field  EffectivenessField  ON dbo.MetaObject.Class = EffectivenessField.Class left outer JOIN  
dbo.ObjectFieldValue EffectivenessValue ON dbo.MetaObject.pkid=EffectivenessValue.ObjectID and dbo.MetaObject.Machine=EffectivenessValue.MachineID and EffectivenessField.pkid = EffectivenessValue.fieldid  
 INNER JOIN  dbo.Field  ManpowerField  ON dbo.MetaObject.Class = ManpowerField.Class left outer JOIN  
dbo.ObjectFieldValue ManpowerValue ON dbo.MetaObject.pkid=ManpowerValue.ObjectID and dbo.MetaObject.Machine=ManpowerValue.MachineID and ManpowerField.pkid = ManpowerValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  EnvironmentIndField  ON dbo.MetaObject.Class = EnvironmentIndField.Class left outer JOIN  
dbo.ObjectFieldValue EnvironmentIndValue ON dbo.MetaObject.pkid=EnvironmentIndValue.ObjectID and dbo.MetaObject.Machine=EnvironmentIndValue.MachineID and EnvironmentIndField.pkid = EnvironmentIndValue.fieldid  
 INNER JOIN  dbo.Field  GovernanceMechField  ON dbo.MetaObject.Class = GovernanceMechField.Class left outer JOIN  
dbo.ObjectFieldValue GovernanceMechValue ON dbo.MetaObject.pkid=GovernanceMechValue.ObjectID and dbo.MetaObject.Machine=GovernanceMechValue.MachineID and GovernanceMechField.pkid = GovernanceMechValue.fieldid  
 INNER JOIN  dbo.Field  CapitalAvailabilityField  ON dbo.MetaObject.Class = CapitalAvailabilityField.Class left outer JOIN  
dbo.ObjectFieldValue CapitalAvailabilityValue ON dbo.MetaObject.pkid=CapitalAvailabilityValue.ObjectID and dbo.MetaObject.Machine=CapitalAvailabilityValue.MachineID and CapitalAvailabilityField.pkid = CapitalAvailabilityValue.fieldid  
 INNER JOIN  dbo.Field  CompetenciesField  ON dbo.MetaObject.Class = CompetenciesField.Class left outer JOIN  
dbo.ObjectFieldValue CompetenciesValue ON dbo.MetaObject.pkid=CompetenciesValue.ObjectID and dbo.MetaObject.Machine=CompetenciesValue.MachineID and CompetenciesField.pkid = CompetenciesValue.fieldid  
 INNER JOIN  dbo.Field  EthicsField  ON dbo.MetaObject.Class = EthicsField.Class left outer JOIN  
dbo.ObjectFieldValue EthicsValue ON dbo.MetaObject.pkid=EthicsValue.ObjectID and dbo.MetaObject.Machine=EthicsValue.MachineID and EthicsField.pkid = EthicsValue.fieldid  
 INNER JOIN  dbo.Field  FacilitiesField  ON dbo.MetaObject.Class = FacilitiesField.Class left outer JOIN  
dbo.ObjectFieldValue FacilitiesValue ON dbo.MetaObject.pkid=FacilitiesValue.ObjectID and dbo.MetaObject.Machine=FacilitiesValue.MachineID and FacilitiesField.pkid = FacilitiesValue.fieldid  
 INNER JOIN  dbo.Field  ServicesUsageField  ON dbo.MetaObject.Class = ServicesUsageField.Class left outer JOIN  
dbo.ObjectFieldValue ServicesUsageValue ON dbo.MetaObject.pkid=ServicesUsageValue.ObjectID and dbo.MetaObject.Machine=ServicesUsageValue.MachineID and ServicesUsageField.pkid = ServicesUsageValue.fieldid  
 INNER JOIN  dbo.Field  EquipmentField  ON dbo.MetaObject.Class = EquipmentField.Class left outer JOIN  
dbo.ObjectFieldValue EquipmentValue ON dbo.MetaObject.pkid=EquipmentValue.ObjectID and dbo.MetaObject.Machine=EquipmentValue.MachineID and EquipmentField.pkid = EquipmentValue.fieldid  
 INNER JOIN  dbo.Field  TimeIndicatorField  ON dbo.MetaObject.Class = TimeIndicatorField.Class left outer JOIN  
dbo.ObjectFieldValue TimeIndicatorValue ON dbo.MetaObject.pkid=TimeIndicatorValue.ObjectID and dbo.MetaObject.Machine=TimeIndicatorValue.MachineID and TimeIndicatorField.pkid = TimeIndicatorValue.fieldid  
 INNER JOIN  dbo.Field  EffeciencyField  ON dbo.MetaObject.Class = EffeciencyField.Class left outer JOIN  
dbo.ObjectFieldValue EffeciencyValue ON dbo.MetaObject.pkid=EffeciencyValue.ObjectID and dbo.MetaObject.Machine=EffeciencyValue.MachineID and EffeciencyField.pkid = EffeciencyValue.fieldid  
 INNER JOIN  dbo.Field  EffectivinessField  ON dbo.MetaObject.Class = EffectivinessField.Class left outer JOIN  
dbo.ObjectFieldValue EffectivinessValue ON dbo.MetaObject.pkid=EffectivinessValue.ObjectID and dbo.MetaObject.Machine=EffectivinessValue.MachineID and EffectivinessField.pkid = EffectivinessValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Function')  AND NameField.Name ='Name'  AND ContextualIndicatorField.Name ='ContextualIndicator'  AND FunctionCriticalityField.Name ='FunctionCriticality'  AND ManagementAbilityField.Name ='ManagementAbility'  AND InfoAvailabilityField.Name ='InfoAvailability'  AND LegalAspectsField.Name ='LegalAspects'  AND TechnologyField.Name ='Technology'  AND BudgetField.Name ='Budget'  AND EnergyField.Name ='Energy'  AND RawMaterialField.Name ='RawMaterial'  AND SkillAvailabilityField.Name ='SkillAvailability'  AND EfficiencyField.Name ='Efficiency'  AND EffectivenessField.Name ='Effectiveness'  AND ManpowerField.Name ='Manpower'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND EnvironmentIndField.Name ='EnvironmentInd'  AND GovernanceMechField.Name ='GovernanceMech'  AND CapitalAvailabilityField.Name ='CapitalAvailability'  AND CompetenciesField.Name ='Competencies'  AND EthicsField.Name ='Ethics'  AND FacilitiesField.Name ='Facilities'  AND ServicesUsageField.Name ='ServicesUsage'  AND EquipmentField.Name ='Equipment'  AND TimeIndicatorField.Name ='TimeIndicator'  AND EffeciencyField.Name ='Effeciency'  AND EffectivinessField.Name ='Effectiviness'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_FunctionalDependency_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_FunctionalDependency_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,InferenceRuleValue.ValueString as InferenceRule,ConditionValue.ValueString as Condition,DescriptionValue.ValueString as Description,CohesionWeightValue.ValueString as CohesionWeight,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  InferenceRuleField  ON dbo.MetaObject.Class = InferenceRuleField.Class left outer JOIN  
dbo.ObjectFieldValue InferenceRuleValue ON dbo.MetaObject.pkid=InferenceRuleValue.ObjectID and dbo.MetaObject.Machine=InferenceRuleValue.MachineID and InferenceRuleField.pkid = InferenceRuleValue.fieldid  
 INNER JOIN  dbo.Field  ConditionField  ON dbo.MetaObject.Class = ConditionField.Class left outer JOIN  
dbo.ObjectFieldValue ConditionValue ON dbo.MetaObject.pkid=ConditionValue.ObjectID and dbo.MetaObject.Machine=ConditionValue.MachineID and ConditionField.pkid = ConditionValue.fieldid  
 INNER JOIN  dbo.Field  DescriptionField  ON dbo.MetaObject.Class = DescriptionField.Class left outer JOIN  
dbo.ObjectFieldValue DescriptionValue ON dbo.MetaObject.pkid=DescriptionValue.ObjectID and dbo.MetaObject.Machine=DescriptionValue.MachineID and DescriptionField.pkid = DescriptionValue.fieldid  
 INNER JOIN  dbo.Field  CohesionWeightField  ON dbo.MetaObject.Class = CohesionWeightField.Class left outer JOIN  
dbo.ObjectFieldValue CohesionWeightValue ON dbo.MetaObject.pkid=CohesionWeightValue.ObjectID and dbo.MetaObject.Machine=CohesionWeightValue.MachineID and CohesionWeightField.pkid = CohesionWeightValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'FunctionalDependency')  AND InferenceRuleField.Name ='InferenceRule'  AND ConditionField.Name ='Condition'  AND DescriptionField.Name ='Description'  AND CohesionWeightField.Name ='CohesionWeight'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_FunctionalDependency_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_FunctionalDependency_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,InferenceRuleValue.ValueString as InferenceRule,ConditionValue.ValueString as Condition,DescriptionValue.ValueString as Description,CohesionWeightValue.ValueString as CohesionWeight,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  InferenceRuleField  ON dbo.MetaObject.Class = InferenceRuleField.Class left outer JOIN  
dbo.ObjectFieldValue InferenceRuleValue ON dbo.MetaObject.pkid=InferenceRuleValue.ObjectID and dbo.MetaObject.Machine=InferenceRuleValue.MachineID and InferenceRuleField.pkid = InferenceRuleValue.fieldid  
 INNER JOIN  dbo.Field  ConditionField  ON dbo.MetaObject.Class = ConditionField.Class left outer JOIN  
dbo.ObjectFieldValue ConditionValue ON dbo.MetaObject.pkid=ConditionValue.ObjectID and dbo.MetaObject.Machine=ConditionValue.MachineID and ConditionField.pkid = ConditionValue.fieldid  
 INNER JOIN  dbo.Field  DescriptionField  ON dbo.MetaObject.Class = DescriptionField.Class left outer JOIN  
dbo.ObjectFieldValue DescriptionValue ON dbo.MetaObject.pkid=DescriptionValue.ObjectID and dbo.MetaObject.Machine=DescriptionValue.MachineID and DescriptionField.pkid = DescriptionValue.fieldid  
 INNER JOIN  dbo.Field  CohesionWeightField  ON dbo.MetaObject.Class = CohesionWeightField.Class left outer JOIN  
dbo.ObjectFieldValue CohesionWeightValue ON dbo.MetaObject.pkid=CohesionWeightValue.ObjectID and dbo.MetaObject.Machine=CohesionWeightValue.MachineID and CohesionWeightField.pkid = CohesionWeightValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'FunctionalDependency')  AND InferenceRuleField.Name ='InferenceRule'  AND ConditionField.Name ='Condition'  AND DescriptionField.Name ='Description'  AND CohesionWeightField.Name ='CohesionWeight'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_GovernanceMechanism_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_GovernanceMechanism_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,EnvironmentIndValue.ValueString as EnvironmentInd,GovernanceMechTypeValue.ValueString as GovernanceMechType,UniqueRefValue.ValueString as UniqueRef,ValidityPeriodValue.ValueString as ValidityPeriod,DescriptionValue.ValueString as Description,GapTypeValue.ValueString as GapType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3 FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  EnvironmentIndField  ON dbo.MetaObject.Class = EnvironmentIndField.Class left outer JOIN  
dbo.ObjectFieldValue EnvironmentIndValue ON dbo.MetaObject.pkid=EnvironmentIndValue.ObjectID and dbo.MetaObject.Machine=EnvironmentIndValue.MachineID and EnvironmentIndField.pkid = EnvironmentIndValue.fieldid  
 INNER JOIN  dbo.Field  GovernanceMechTypeField  ON dbo.MetaObject.Class = GovernanceMechTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GovernanceMechTypeValue ON dbo.MetaObject.pkid=GovernanceMechTypeValue.ObjectID and dbo.MetaObject.Machine=GovernanceMechTypeValue.MachineID and GovernanceMechTypeField.pkid = GovernanceMechTypeValue.fieldid  
 INNER JOIN  dbo.Field  UniqueRefField  ON dbo.MetaObject.Class = UniqueRefField.Class left outer JOIN  
dbo.ObjectFieldValue UniqueRefValue ON dbo.MetaObject.pkid=UniqueRefValue.ObjectID and dbo.MetaObject.Machine=UniqueRefValue.MachineID and UniqueRefField.pkid = UniqueRefValue.fieldid  
 INNER JOIN  dbo.Field  ValidityPeriodField  ON dbo.MetaObject.Class = ValidityPeriodField.Class left outer JOIN  
dbo.ObjectFieldValue ValidityPeriodValue ON dbo.MetaObject.pkid=ValidityPeriodValue.ObjectID and dbo.MetaObject.Machine=ValidityPeriodValue.MachineID and ValidityPeriodField.pkid = ValidityPeriodValue.fieldid  
 INNER JOIN  dbo.Field  DescriptionField  ON dbo.MetaObject.Class = DescriptionField.Class left outer JOIN  
dbo.ObjectFieldValue DescriptionValue ON dbo.MetaObject.pkid=DescriptionValue.ObjectID and dbo.MetaObject.Machine=DescriptionValue.MachineID and DescriptionField.pkid = DescriptionValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 WHERE (dbo.MetaObject.Class = 'GovernanceMechanism')  AND EnvironmentIndField.Name ='EnvironmentInd'  AND GovernanceMechTypeField.Name ='GovernanceMechType'  AND UniqueRefField.Name ='UniqueRef'  AND ValidityPeriodField.Name ='ValidityPeriod'  AND DescriptionField.Name ='Description'  AND GapTypeField.Name ='GapType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3' 

GO
/****** Object:  View [dbo].[METAView_GovernanceMechanism_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_GovernanceMechanism_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,EnvironmentIndValue.ValueString as EnvironmentInd,GovernanceMechTypeValue.ValueString as GovernanceMechType,UniqueRefValue.ValueString as UniqueRef,ValidityPeriodValue.ValueString as ValidityPeriod,DescriptionValue.ValueString as Description,GapTypeValue.ValueString as GapType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3 FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  EnvironmentIndField  ON dbo.MetaObject.Class = EnvironmentIndField.Class left outer JOIN  
dbo.ObjectFieldValue EnvironmentIndValue ON dbo.MetaObject.pkid=EnvironmentIndValue.ObjectID and dbo.MetaObject.Machine=EnvironmentIndValue.MachineID and EnvironmentIndField.pkid = EnvironmentIndValue.fieldid  
 INNER JOIN  dbo.Field  GovernanceMechTypeField  ON dbo.MetaObject.Class = GovernanceMechTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GovernanceMechTypeValue ON dbo.MetaObject.pkid=GovernanceMechTypeValue.ObjectID and dbo.MetaObject.Machine=GovernanceMechTypeValue.MachineID and GovernanceMechTypeField.pkid = GovernanceMechTypeValue.fieldid  
 INNER JOIN  dbo.Field  UniqueRefField  ON dbo.MetaObject.Class = UniqueRefField.Class left outer JOIN  
dbo.ObjectFieldValue UniqueRefValue ON dbo.MetaObject.pkid=UniqueRefValue.ObjectID and dbo.MetaObject.Machine=UniqueRefValue.MachineID and UniqueRefField.pkid = UniqueRefValue.fieldid  
 INNER JOIN  dbo.Field  ValidityPeriodField  ON dbo.MetaObject.Class = ValidityPeriodField.Class left outer JOIN  
dbo.ObjectFieldValue ValidityPeriodValue ON dbo.MetaObject.pkid=ValidityPeriodValue.ObjectID and dbo.MetaObject.Machine=ValidityPeriodValue.MachineID and ValidityPeriodField.pkid = ValidityPeriodValue.fieldid  
 INNER JOIN  dbo.Field  DescriptionField  ON dbo.MetaObject.Class = DescriptionField.Class left outer JOIN  
dbo.ObjectFieldValue DescriptionValue ON dbo.MetaObject.pkid=DescriptionValue.ObjectID and dbo.MetaObject.Machine=DescriptionValue.MachineID and DescriptionField.pkid = DescriptionValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 WHERE (dbo.MetaObject.Class = 'GovernanceMechanism')  AND EnvironmentIndField.Name ='EnvironmentInd'  AND GovernanceMechTypeField.Name ='GovernanceMechType'  AND UniqueRefField.Name ='UniqueRef'  AND ValidityPeriodField.Name ='ValidityPeriod'  AND DescriptionField.Name ='Description'  AND GapTypeField.Name ='GapType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3' 

GO
/****** Object:  View [dbo].[METAView_Implication_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Implication_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,Ext_Inf_IndicatorValue.ValueInt as Ext_Inf_Indicator,Int_Capability_IndicatorValue.ValueInt as Int_Capability_Indicator,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  Ext_Inf_IndicatorField  ON dbo.MetaObject.Class = Ext_Inf_IndicatorField.Class left outer JOIN  
dbo.ObjectFieldValue Ext_Inf_IndicatorValue ON dbo.MetaObject.pkid=Ext_Inf_IndicatorValue.ObjectID and dbo.MetaObject.Machine=Ext_Inf_IndicatorValue.MachineID and Ext_Inf_IndicatorField.pkid = Ext_Inf_IndicatorValue.fieldid  
 INNER JOIN  dbo.Field  Int_Capability_IndicatorField  ON dbo.MetaObject.Class = Int_Capability_IndicatorField.Class left outer JOIN  
dbo.ObjectFieldValue Int_Capability_IndicatorValue ON dbo.MetaObject.pkid=Int_Capability_IndicatorValue.ObjectID and dbo.MetaObject.Machine=Int_Capability_IndicatorValue.MachineID and Int_Capability_IndicatorField.pkid = Int_Capability_IndicatorValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Implication')  AND NameField.Name ='Name'  AND Ext_Inf_IndicatorField.Name ='Ext_Inf_Indicator'  AND Int_Capability_IndicatorField.Name ='Int_Capability_Indicator'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Implication_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Implication_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,Ext_Inf_IndicatorValue.ValueInt as Ext_Inf_Indicator,Int_Capability_IndicatorValue.ValueInt as Int_Capability_Indicator,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  Ext_Inf_IndicatorField  ON dbo.MetaObject.Class = Ext_Inf_IndicatorField.Class left outer JOIN  
dbo.ObjectFieldValue Ext_Inf_IndicatorValue ON dbo.MetaObject.pkid=Ext_Inf_IndicatorValue.ObjectID and dbo.MetaObject.Machine=Ext_Inf_IndicatorValue.MachineID and Ext_Inf_IndicatorField.pkid = Ext_Inf_IndicatorValue.fieldid  
 INNER JOIN  dbo.Field  Int_Capability_IndicatorField  ON dbo.MetaObject.Class = Int_Capability_IndicatorField.Class left outer JOIN  
dbo.ObjectFieldValue Int_Capability_IndicatorValue ON dbo.MetaObject.pkid=Int_Capability_IndicatorValue.ObjectID and dbo.MetaObject.Machine=Int_Capability_IndicatorValue.MachineID and Int_Capability_IndicatorField.pkid = Int_Capability_IndicatorValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Implication')  AND NameField.Name ='Name'  AND Ext_Inf_IndicatorField.Name ='Ext_Inf_Indicator'  AND Int_Capability_IndicatorField.Name ='Int_Capability_Indicator'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Iteration_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Iteration_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,IterationTypeValue.ValueString as IterationType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  IterationTypeField  ON dbo.MetaObject.Class = IterationTypeField.Class left outer JOIN  
dbo.ObjectFieldValue IterationTypeValue ON dbo.MetaObject.pkid=IterationTypeValue.ObjectID and dbo.MetaObject.Machine=IterationTypeValue.MachineID and IterationTypeField.pkid = IterationTypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Iteration')  AND NameField.Name ='Name'  AND IterationTypeField.Name ='IterationType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Iteration_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Iteration_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,IterationTypeValue.ValueString as IterationType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  IterationTypeField  ON dbo.MetaObject.Class = IterationTypeField.Class left outer JOIN  
dbo.ObjectFieldValue IterationTypeValue ON dbo.MetaObject.pkid=IterationTypeValue.ObjectID and dbo.MetaObject.Machine=IterationTypeValue.MachineID and IterationTypeField.pkid = IterationTypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Iteration')  AND NameField.Name ='Name'  AND IterationTypeField.Name ='IterationType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Job_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Job_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,TypeValue.ValueString as Type,GapTypeValue.ValueString as GapType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3 FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  TypeField  ON dbo.MetaObject.Class = TypeField.Class left outer JOIN  
dbo.ObjectFieldValue TypeValue ON dbo.MetaObject.pkid=TypeValue.ObjectID and dbo.MetaObject.Machine=TypeValue.MachineID and TypeField.pkid = TypeValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 WHERE (dbo.MetaObject.Class = 'Job')  AND NameField.Name ='Name'  AND TypeField.Name ='Type'  AND GapTypeField.Name ='GapType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3' 

GO
/****** Object:  View [dbo].[METAView_Job_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Job_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,TypeValue.ValueString as Type,GapTypeValue.ValueString as GapType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3 FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  TypeField  ON dbo.MetaObject.Class = TypeField.Class left outer JOIN  
dbo.ObjectFieldValue TypeValue ON dbo.MetaObject.pkid=TypeValue.ObjectID and dbo.MetaObject.Machine=TypeValue.MachineID and TypeField.pkid = TypeValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 WHERE (dbo.MetaObject.Class = 'Job')  AND NameField.Name ='Name'  AND TypeField.Name ='Type'  AND GapTypeField.Name ='GapType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3' 

GO
/****** Object:  View [dbo].[METAView_JobPosition_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_JobPosition_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,TotalRequiredValue.ValueInt as TotalRequired,TotalOccupiedValue.ValueInt as TotalOccupied,TotalAvailableValue.ValueInt as TotalAvailable,TimeStampValue.ValueDate as TimeStamp,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  TotalRequiredField  ON dbo.MetaObject.Class = TotalRequiredField.Class left outer JOIN  
dbo.ObjectFieldValue TotalRequiredValue ON dbo.MetaObject.pkid=TotalRequiredValue.ObjectID and dbo.MetaObject.Machine=TotalRequiredValue.MachineID and TotalRequiredField.pkid = TotalRequiredValue.fieldid  
 INNER JOIN  dbo.Field  TotalOccupiedField  ON dbo.MetaObject.Class = TotalOccupiedField.Class left outer JOIN  
dbo.ObjectFieldValue TotalOccupiedValue ON dbo.MetaObject.pkid=TotalOccupiedValue.ObjectID and dbo.MetaObject.Machine=TotalOccupiedValue.MachineID and TotalOccupiedField.pkid = TotalOccupiedValue.fieldid  
 INNER JOIN  dbo.Field  TotalAvailableField  ON dbo.MetaObject.Class = TotalAvailableField.Class left outer JOIN  
dbo.ObjectFieldValue TotalAvailableValue ON dbo.MetaObject.pkid=TotalAvailableValue.ObjectID and dbo.MetaObject.Machine=TotalAvailableValue.MachineID and TotalAvailableField.pkid = TotalAvailableValue.fieldid  
 INNER JOIN  dbo.Field  TimeStampField  ON dbo.MetaObject.Class = TimeStampField.Class left outer JOIN  
dbo.ObjectFieldValue TimeStampValue ON dbo.MetaObject.pkid=TimeStampValue.ObjectID and dbo.MetaObject.Machine=TimeStampValue.MachineID and TimeStampField.pkid = TimeStampValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'JobPosition')  AND NameField.Name ='Name'  AND TotalRequiredField.Name ='TotalRequired'  AND TotalOccupiedField.Name ='TotalOccupied'  AND TotalAvailableField.Name ='TotalAvailable'  AND TimeStampField.Name ='TimeStamp'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_JobPosition_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_JobPosition_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,TotalRequiredValue.ValueInt as TotalRequired,TotalOccupiedValue.ValueInt as TotalOccupied,TotalAvailableValue.ValueInt as TotalAvailable,TimeStampValue.ValueDate as TimeStamp,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  TotalRequiredField  ON dbo.MetaObject.Class = TotalRequiredField.Class left outer JOIN  
dbo.ObjectFieldValue TotalRequiredValue ON dbo.MetaObject.pkid=TotalRequiredValue.ObjectID and dbo.MetaObject.Machine=TotalRequiredValue.MachineID and TotalRequiredField.pkid = TotalRequiredValue.fieldid  
 INNER JOIN  dbo.Field  TotalOccupiedField  ON dbo.MetaObject.Class = TotalOccupiedField.Class left outer JOIN  
dbo.ObjectFieldValue TotalOccupiedValue ON dbo.MetaObject.pkid=TotalOccupiedValue.ObjectID and dbo.MetaObject.Machine=TotalOccupiedValue.MachineID and TotalOccupiedField.pkid = TotalOccupiedValue.fieldid  
 INNER JOIN  dbo.Field  TotalAvailableField  ON dbo.MetaObject.Class = TotalAvailableField.Class left outer JOIN  
dbo.ObjectFieldValue TotalAvailableValue ON dbo.MetaObject.pkid=TotalAvailableValue.ObjectID and dbo.MetaObject.Machine=TotalAvailableValue.MachineID and TotalAvailableField.pkid = TotalAvailableValue.fieldid  
 INNER JOIN  dbo.Field  TimeStampField  ON dbo.MetaObject.Class = TimeStampField.Class left outer JOIN  
dbo.ObjectFieldValue TimeStampValue ON dbo.MetaObject.pkid=TimeStampValue.ObjectID and dbo.MetaObject.Machine=TimeStampValue.MachineID and TimeStampField.pkid = TimeStampValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'JobPosition')  AND NameField.Name ='Name'  AND TotalRequiredField.Name ='TotalRequired'  AND TotalOccupiedField.Name ='TotalOccupied'  AND TotalAvailableField.Name ='TotalAvailable'  AND TimeStampField.Name ='TimeStamp'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Location_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Location_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,LocationTypeValue.ValueString as LocationType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,AddressValue.ValueString as Address,TelephoneValue.ValueString as Telephone,FaxValue.ValueString as Fax,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  LocationTypeField  ON dbo.MetaObject.Class = LocationTypeField.Class left outer JOIN  
dbo.ObjectFieldValue LocationTypeValue ON dbo.MetaObject.pkid=LocationTypeValue.ObjectID and dbo.MetaObject.Machine=LocationTypeValue.MachineID and LocationTypeField.pkid = LocationTypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  AddressField  ON dbo.MetaObject.Class = AddressField.Class left outer JOIN  
dbo.ObjectFieldValue AddressValue ON dbo.MetaObject.pkid=AddressValue.ObjectID and dbo.MetaObject.Machine=AddressValue.MachineID and AddressField.pkid = AddressValue.fieldid  
 INNER JOIN  dbo.Field  TelephoneField  ON dbo.MetaObject.Class = TelephoneField.Class left outer JOIN  
dbo.ObjectFieldValue TelephoneValue ON dbo.MetaObject.pkid=TelephoneValue.ObjectID and dbo.MetaObject.Machine=TelephoneValue.MachineID and TelephoneField.pkid = TelephoneValue.fieldid  
 INNER JOIN  dbo.Field  FaxField  ON dbo.MetaObject.Class = FaxField.Class left outer JOIN  
dbo.ObjectFieldValue FaxValue ON dbo.MetaObject.pkid=FaxValue.ObjectID and dbo.MetaObject.Machine=FaxValue.MachineID and FaxField.pkid = FaxValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Location')  AND NameField.Name ='Name'  AND LocationTypeField.Name ='LocationType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND AddressField.Name ='Address'  AND TelephoneField.Name ='Telephone'  AND FaxField.Name ='Fax'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Location_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Location_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,LocationTypeValue.ValueString as LocationType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,AddressValue.ValueString as Address,TelephoneValue.ValueString as Telephone,FaxValue.ValueString as Fax,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  LocationTypeField  ON dbo.MetaObject.Class = LocationTypeField.Class left outer JOIN  
dbo.ObjectFieldValue LocationTypeValue ON dbo.MetaObject.pkid=LocationTypeValue.ObjectID and dbo.MetaObject.Machine=LocationTypeValue.MachineID and LocationTypeField.pkid = LocationTypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  AddressField  ON dbo.MetaObject.Class = AddressField.Class left outer JOIN  
dbo.ObjectFieldValue AddressValue ON dbo.MetaObject.pkid=AddressValue.ObjectID and dbo.MetaObject.Machine=AddressValue.MachineID and AddressField.pkid = AddressValue.fieldid  
 INNER JOIN  dbo.Field  TelephoneField  ON dbo.MetaObject.Class = TelephoneField.Class left outer JOIN  
dbo.ObjectFieldValue TelephoneValue ON dbo.MetaObject.pkid=TelephoneValue.ObjectID and dbo.MetaObject.Machine=TelephoneValue.MachineID and TelephoneField.pkid = TelephoneValue.fieldid  
 INNER JOIN  dbo.Field  FaxField  ON dbo.MetaObject.Class = FaxField.Class left outer JOIN  
dbo.ObjectFieldValue FaxValue ON dbo.MetaObject.pkid=FaxValue.ObjectID and dbo.MetaObject.Machine=FaxValue.MachineID and FaxField.pkid = FaxValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Location')  AND NameField.Name ='Name'  AND LocationTypeField.Name ='LocationType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND AddressField.Name ='Address'  AND TelephoneField.Name ='Telephone'  AND FaxField.Name ='Fax'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_LocationAssociation_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_LocationAssociation_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,DistanceValue.ValueString as Distance,TimeIndicatorValue.ValueString as TimeIndicator,AssociationTypeValue.ValueString as AssociationType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  DistanceField  ON dbo.MetaObject.Class = DistanceField.Class left outer JOIN  
dbo.ObjectFieldValue DistanceValue ON dbo.MetaObject.pkid=DistanceValue.ObjectID and dbo.MetaObject.Machine=DistanceValue.MachineID and DistanceField.pkid = DistanceValue.fieldid  
 INNER JOIN  dbo.Field  TimeIndicatorField  ON dbo.MetaObject.Class = TimeIndicatorField.Class left outer JOIN  
dbo.ObjectFieldValue TimeIndicatorValue ON dbo.MetaObject.pkid=TimeIndicatorValue.ObjectID and dbo.MetaObject.Machine=TimeIndicatorValue.MachineID and TimeIndicatorField.pkid = TimeIndicatorValue.fieldid  
 INNER JOIN  dbo.Field  AssociationTypeField  ON dbo.MetaObject.Class = AssociationTypeField.Class left outer JOIN  
dbo.ObjectFieldValue AssociationTypeValue ON dbo.MetaObject.pkid=AssociationTypeValue.ObjectID and dbo.MetaObject.Machine=AssociationTypeValue.MachineID and AssociationTypeField.pkid = AssociationTypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'LocationAssociation')  AND DistanceField.Name ='Distance'  AND TimeIndicatorField.Name ='TimeIndicator'  AND AssociationTypeField.Name ='AssociationType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_LocationAssociation_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_LocationAssociation_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,DistanceValue.ValueString as Distance,TimeIndicatorValue.ValueString as TimeIndicator,AssociationTypeValue.ValueString as AssociationType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  DistanceField  ON dbo.MetaObject.Class = DistanceField.Class left outer JOIN  
dbo.ObjectFieldValue DistanceValue ON dbo.MetaObject.pkid=DistanceValue.ObjectID and dbo.MetaObject.Machine=DistanceValue.MachineID and DistanceField.pkid = DistanceValue.fieldid  
 INNER JOIN  dbo.Field  TimeIndicatorField  ON dbo.MetaObject.Class = TimeIndicatorField.Class left outer JOIN  
dbo.ObjectFieldValue TimeIndicatorValue ON dbo.MetaObject.pkid=TimeIndicatorValue.ObjectID and dbo.MetaObject.Machine=TimeIndicatorValue.MachineID and TimeIndicatorField.pkid = TimeIndicatorValue.fieldid  
 INNER JOIN  dbo.Field  AssociationTypeField  ON dbo.MetaObject.Class = AssociationTypeField.Class left outer JOIN  
dbo.ObjectFieldValue AssociationTypeValue ON dbo.MetaObject.pkid=AssociationTypeValue.ObjectID and dbo.MetaObject.Machine=AssociationTypeValue.MachineID and AssociationTypeField.pkid = AssociationTypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'LocationAssociation')  AND DistanceField.Name ='Distance'  AND TimeIndicatorField.Name ='TimeIndicator'  AND AssociationTypeField.Name ='AssociationType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Logic_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Logic_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,DescriptionValue.ValueString as Description,GapTypeValue.ValueString as GapType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3 FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  DescriptionField  ON dbo.MetaObject.Class = DescriptionField.Class left outer JOIN  
dbo.ObjectFieldValue DescriptionValue ON dbo.MetaObject.pkid=DescriptionValue.ObjectID and dbo.MetaObject.Machine=DescriptionValue.MachineID and DescriptionField.pkid = DescriptionValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 WHERE (dbo.MetaObject.Class = 'Logic')  AND DescriptionField.Name ='Description'  AND GapTypeField.Name ='GapType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3' 

GO
/****** Object:  View [dbo].[METAView_Logic_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Logic_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,DescriptionValue.ValueString as Description,GapTypeValue.ValueString as GapType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3 FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  DescriptionField  ON dbo.MetaObject.Class = DescriptionField.Class left outer JOIN  
dbo.ObjectFieldValue DescriptionValue ON dbo.MetaObject.pkid=DescriptionValue.ObjectID and dbo.MetaObject.Machine=DescriptionValue.MachineID and DescriptionField.pkid = DescriptionValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 WHERE (dbo.MetaObject.Class = 'Logic')  AND DescriptionField.Name ='Description'  AND GapTypeField.Name ='GapType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3' 

GO
/****** Object:  View [dbo].[METAView_MeasurementItem_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_MeasurementItem_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'MeasurementItem')  AND NameField.Name ='Name'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_MeasurementItem_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_MeasurementItem_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'MeasurementItem')  AND NameField.Name ='Name'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Model_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Model_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID FROM dbo.MetaObject 
 WHERE (dbo.MetaObject.Class = 'Model') 

GO
/****** Object:  View [dbo].[METAView_Model_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Model_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID FROM dbo.MetaObject 
 WHERE (dbo.MetaObject.Class = 'Model') 

GO
/****** Object:  View [dbo].[METAView_ModelReal_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_ModelReal_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID FROM dbo.MetaObject 
 WHERE (dbo.MetaObject.Class = 'ModelReal') 

GO
/****** Object:  View [dbo].[METAView_ModelReal_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_ModelReal_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID FROM dbo.MetaObject 
 WHERE (dbo.MetaObject.Class = 'ModelReal') 

GO
/****** Object:  View [dbo].[METAView_MutuallyExclusiveIndicator_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_MutuallyExclusiveIndicator_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,SelectorTypeValue.ValueString as SelectorType,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  SelectorTypeField  ON dbo.MetaObject.Class = SelectorTypeField.Class left outer JOIN  
dbo.ObjectFieldValue SelectorTypeValue ON dbo.MetaObject.pkid=SelectorTypeValue.ObjectID and dbo.MetaObject.Machine=SelectorTypeValue.MachineID and SelectorTypeField.pkid = SelectorTypeValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'MutuallyExclusiveIndicator')  AND NameField.Name ='Name'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND SelectorTypeField.Name ='SelectorType'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_MutuallyExclusiveIndicator_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_MutuallyExclusiveIndicator_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,SelectorTypeValue.ValueString as SelectorType,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  SelectorTypeField  ON dbo.MetaObject.Class = SelectorTypeField.Class left outer JOIN  
dbo.ObjectFieldValue SelectorTypeValue ON dbo.MetaObject.pkid=SelectorTypeValue.ObjectID and dbo.MetaObject.Machine=SelectorTypeValue.MachineID and SelectorTypeField.pkid = SelectorTypeValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'MutuallyExclusiveIndicator')  AND NameField.Name ='Name'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND SelectorTypeField.Name ='SelectorType'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_NetworkComponent_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_NetworkComponent_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,TypeValue.ValueString as Type,NameValue.ValueString as Name,DescriptionValue.ValueString as Description,SeverityRatingValue.ValueString as SeverityRating,ConfigurationValue.ValueString as Configuration,MacAddressValue.ValueString as MacAddress,NetworkAddress2Value.ValueString as NetworkAddress2,NetworkAddress3Value.ValueString as NetworkAddress3,NetworkAddress4Value.ValueString as NetworkAddress4,NetworkAddress5Value.ValueString as NetworkAddress5,MakeValue.ValueString as Make,ModelValue.ValueString as Model,SerialNumberValue.ValueString as SerialNumber,AssetNumberValue.ValueString as AssetNumber,ConnectionSpeedValue.ValueString as ConnectionSpeed,Number_of_PortsValue.ValueString as Number_of_Ports,Number_of_Ports_AvailableValue.ValueString as Number_of_Ports_Available,RangeValue.ValueString as Range,isDNSValue.ValueString as isDNS,isDHCPValue.ValueString as isDHCP,isManagedValue.ValueString as isManaged,Mem_TotalValue.ValueString as Mem_Total,DatePurchasedValue.ValueString as DatePurchased,UnderWarrantyValue.ValueString as UnderWarranty,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,NetworkTypeValue.ValueString as NetworkType,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  TypeField  ON dbo.MetaObject.Class = TypeField.Class left outer JOIN  
dbo.ObjectFieldValue TypeValue ON dbo.MetaObject.pkid=TypeValue.ObjectID and dbo.MetaObject.Machine=TypeValue.MachineID and TypeField.pkid = TypeValue.fieldid  
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  DescriptionField  ON dbo.MetaObject.Class = DescriptionField.Class left outer JOIN  
dbo.ObjectFieldValue DescriptionValue ON dbo.MetaObject.pkid=DescriptionValue.ObjectID and dbo.MetaObject.Machine=DescriptionValue.MachineID and DescriptionField.pkid = DescriptionValue.fieldid  
 INNER JOIN  dbo.Field  SeverityRatingField  ON dbo.MetaObject.Class = SeverityRatingField.Class left outer JOIN  
dbo.ObjectFieldValue SeverityRatingValue ON dbo.MetaObject.pkid=SeverityRatingValue.ObjectID and dbo.MetaObject.Machine=SeverityRatingValue.MachineID and SeverityRatingField.pkid = SeverityRatingValue.fieldid  
 INNER JOIN  dbo.Field  ConfigurationField  ON dbo.MetaObject.Class = ConfigurationField.Class left outer JOIN  
dbo.ObjectFieldValue ConfigurationValue ON dbo.MetaObject.pkid=ConfigurationValue.ObjectID and dbo.MetaObject.Machine=ConfigurationValue.MachineID and ConfigurationField.pkid = ConfigurationValue.fieldid  
 INNER JOIN  dbo.Field  MacAddressField  ON dbo.MetaObject.Class = MacAddressField.Class left outer JOIN  
dbo.ObjectFieldValue MacAddressValue ON dbo.MetaObject.pkid=MacAddressValue.ObjectID and dbo.MetaObject.Machine=MacAddressValue.MachineID and MacAddressField.pkid = MacAddressValue.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress2Field  ON dbo.MetaObject.Class = NetworkAddress2Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress2Value ON dbo.MetaObject.pkid=NetworkAddress2Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress2Value.MachineID and NetworkAddress2Field.pkid = NetworkAddress2Value.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress3Field  ON dbo.MetaObject.Class = NetworkAddress3Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress3Value ON dbo.MetaObject.pkid=NetworkAddress3Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress3Value.MachineID and NetworkAddress3Field.pkid = NetworkAddress3Value.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress4Field  ON dbo.MetaObject.Class = NetworkAddress4Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress4Value ON dbo.MetaObject.pkid=NetworkAddress4Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress4Value.MachineID and NetworkAddress4Field.pkid = NetworkAddress4Value.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress5Field  ON dbo.MetaObject.Class = NetworkAddress5Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress5Value ON dbo.MetaObject.pkid=NetworkAddress5Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress5Value.MachineID and NetworkAddress5Field.pkid = NetworkAddress5Value.fieldid  
 INNER JOIN  dbo.Field  MakeField  ON dbo.MetaObject.Class = MakeField.Class left outer JOIN  
dbo.ObjectFieldValue MakeValue ON dbo.MetaObject.pkid=MakeValue.ObjectID and dbo.MetaObject.Machine=MakeValue.MachineID and MakeField.pkid = MakeValue.fieldid  
 INNER JOIN  dbo.Field  ModelField  ON dbo.MetaObject.Class = ModelField.Class left outer JOIN  
dbo.ObjectFieldValue ModelValue ON dbo.MetaObject.pkid=ModelValue.ObjectID and dbo.MetaObject.Machine=ModelValue.MachineID and ModelField.pkid = ModelValue.fieldid  
 INNER JOIN  dbo.Field  SerialNumberField  ON dbo.MetaObject.Class = SerialNumberField.Class left outer JOIN  
dbo.ObjectFieldValue SerialNumberValue ON dbo.MetaObject.pkid=SerialNumberValue.ObjectID and dbo.MetaObject.Machine=SerialNumberValue.MachineID and SerialNumberField.pkid = SerialNumberValue.fieldid  
 INNER JOIN  dbo.Field  AssetNumberField  ON dbo.MetaObject.Class = AssetNumberField.Class left outer JOIN  
dbo.ObjectFieldValue AssetNumberValue ON dbo.MetaObject.pkid=AssetNumberValue.ObjectID and dbo.MetaObject.Machine=AssetNumberValue.MachineID and AssetNumberField.pkid = AssetNumberValue.fieldid  
 INNER JOIN  dbo.Field  ConnectionSpeedField  ON dbo.MetaObject.Class = ConnectionSpeedField.Class left outer JOIN  
dbo.ObjectFieldValue ConnectionSpeedValue ON dbo.MetaObject.pkid=ConnectionSpeedValue.ObjectID and dbo.MetaObject.Machine=ConnectionSpeedValue.MachineID and ConnectionSpeedField.pkid = ConnectionSpeedValue.fieldid  
 INNER JOIN  dbo.Field  Number_of_PortsField  ON dbo.MetaObject.Class = Number_of_PortsField.Class left outer JOIN  
dbo.ObjectFieldValue Number_of_PortsValue ON dbo.MetaObject.pkid=Number_of_PortsValue.ObjectID and dbo.MetaObject.Machine=Number_of_PortsValue.MachineID and Number_of_PortsField.pkid = Number_of_PortsValue.fieldid  
 INNER JOIN  dbo.Field  Number_of_Ports_AvailableField  ON dbo.MetaObject.Class = Number_of_Ports_AvailableField.Class left outer JOIN  
dbo.ObjectFieldValue Number_of_Ports_AvailableValue ON dbo.MetaObject.pkid=Number_of_Ports_AvailableValue.ObjectID and dbo.MetaObject.Machine=Number_of_Ports_AvailableValue.MachineID and Number_of_Ports_AvailableField.pkid = Number_of_Ports_AvailableValue.fieldid  
 INNER JOIN  dbo.Field  RangeField  ON dbo.MetaObject.Class = RangeField.Class left outer JOIN  
dbo.ObjectFieldValue RangeValue ON dbo.MetaObject.pkid=RangeValue.ObjectID and dbo.MetaObject.Machine=RangeValue.MachineID and RangeField.pkid = RangeValue.fieldid  
 INNER JOIN  dbo.Field  isDNSField  ON dbo.MetaObject.Class = isDNSField.Class left outer JOIN  
dbo.ObjectFieldValue isDNSValue ON dbo.MetaObject.pkid=isDNSValue.ObjectID and dbo.MetaObject.Machine=isDNSValue.MachineID and isDNSField.pkid = isDNSValue.fieldid  
 INNER JOIN  dbo.Field  isDHCPField  ON dbo.MetaObject.Class = isDHCPField.Class left outer JOIN  
dbo.ObjectFieldValue isDHCPValue ON dbo.MetaObject.pkid=isDHCPValue.ObjectID and dbo.MetaObject.Machine=isDHCPValue.MachineID and isDHCPField.pkid = isDHCPValue.fieldid  
 INNER JOIN  dbo.Field  isManagedField  ON dbo.MetaObject.Class = isManagedField.Class left outer JOIN  
dbo.ObjectFieldValue isManagedValue ON dbo.MetaObject.pkid=isManagedValue.ObjectID and dbo.MetaObject.Machine=isManagedValue.MachineID and isManagedField.pkid = isManagedValue.fieldid  
 INNER JOIN  dbo.Field  Mem_TotalField  ON dbo.MetaObject.Class = Mem_TotalField.Class left outer JOIN  
dbo.ObjectFieldValue Mem_TotalValue ON dbo.MetaObject.pkid=Mem_TotalValue.ObjectID and dbo.MetaObject.Machine=Mem_TotalValue.MachineID and Mem_TotalField.pkid = Mem_TotalValue.fieldid  
 INNER JOIN  dbo.Field  DatePurchasedField  ON dbo.MetaObject.Class = DatePurchasedField.Class left outer JOIN  
dbo.ObjectFieldValue DatePurchasedValue ON dbo.MetaObject.pkid=DatePurchasedValue.ObjectID and dbo.MetaObject.Machine=DatePurchasedValue.MachineID and DatePurchasedField.pkid = DatePurchasedValue.fieldid  
 INNER JOIN  dbo.Field  UnderWarrantyField  ON dbo.MetaObject.Class = UnderWarrantyField.Class left outer JOIN  
dbo.ObjectFieldValue UnderWarrantyValue ON dbo.MetaObject.pkid=UnderWarrantyValue.ObjectID and dbo.MetaObject.Machine=UnderWarrantyValue.MachineID and UnderWarrantyField.pkid = UnderWarrantyValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  NetworkTypeField  ON dbo.MetaObject.Class = NetworkTypeField.Class left outer JOIN  
dbo.ObjectFieldValue NetworkTypeValue ON dbo.MetaObject.pkid=NetworkTypeValue.ObjectID and dbo.MetaObject.Machine=NetworkTypeValue.MachineID and NetworkTypeField.pkid = NetworkTypeValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'NetworkComponent')  AND TypeField.Name ='Type'  AND NameField.Name ='Name'  AND DescriptionField.Name ='Description'  AND SeverityRatingField.Name ='SeverityRating'  AND ConfigurationField.Name ='Configuration'  AND MacAddressField.Name ='MacAddress'  AND NetworkAddress2Field.Name ='NetworkAddress2'  AND NetworkAddress3Field.Name ='NetworkAddress3'  AND NetworkAddress4Field.Name ='NetworkAddress4'  AND NetworkAddress5Field.Name ='NetworkAddress5'  AND MakeField.Name ='Make'  AND ModelField.Name ='Model'  AND SerialNumberField.Name ='SerialNumber'  AND AssetNumberField.Name ='AssetNumber'  AND ConnectionSpeedField.Name ='ConnectionSpeed'  AND Number_of_PortsField.Name ='Number_of_Ports'  AND Number_of_Ports_AvailableField.Name ='Number_of_Ports_Available'  AND RangeField.Name ='Range'  AND isDNSField.Name ='isDNS'  AND isDHCPField.Name ='isDHCP'  AND isManagedField.Name ='isManaged'  AND Mem_TotalField.Name ='Mem_Total'  AND DatePurchasedField.Name ='DatePurchased'  AND UnderWarrantyField.Name ='UnderWarranty'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND NetworkTypeField.Name ='NetworkType'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_NetworkComponent_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_NetworkComponent_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,TypeValue.ValueString as Type,NameValue.ValueString as Name,DescriptionValue.ValueString as Description,SeverityRatingValue.ValueString as SeverityRating,ConfigurationValue.ValueString as Configuration,MacAddressValue.ValueString as MacAddress,NetworkAddress2Value.ValueString as NetworkAddress2,NetworkAddress3Value.ValueString as NetworkAddress3,NetworkAddress4Value.ValueString as NetworkAddress4,NetworkAddress5Value.ValueString as NetworkAddress5,MakeValue.ValueString as Make,ModelValue.ValueString as Model,SerialNumberValue.ValueString as SerialNumber,AssetNumberValue.ValueString as AssetNumber,ConnectionSpeedValue.ValueString as ConnectionSpeed,Number_of_PortsValue.ValueString as Number_of_Ports,Number_of_Ports_AvailableValue.ValueString as Number_of_Ports_Available,RangeValue.ValueString as Range,isDNSValue.ValueString as isDNS,isDHCPValue.ValueString as isDHCP,isManagedValue.ValueString as isManaged,Mem_TotalValue.ValueString as Mem_Total,DatePurchasedValue.ValueString as DatePurchased,UnderWarrantyValue.ValueString as UnderWarranty,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,NetworkTypeValue.ValueString as NetworkType,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  TypeField  ON dbo.MetaObject.Class = TypeField.Class left outer JOIN  
dbo.ObjectFieldValue TypeValue ON dbo.MetaObject.pkid=TypeValue.ObjectID and dbo.MetaObject.Machine=TypeValue.MachineID and TypeField.pkid = TypeValue.fieldid  
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  DescriptionField  ON dbo.MetaObject.Class = DescriptionField.Class left outer JOIN  
dbo.ObjectFieldValue DescriptionValue ON dbo.MetaObject.pkid=DescriptionValue.ObjectID and dbo.MetaObject.Machine=DescriptionValue.MachineID and DescriptionField.pkid = DescriptionValue.fieldid  
 INNER JOIN  dbo.Field  SeverityRatingField  ON dbo.MetaObject.Class = SeverityRatingField.Class left outer JOIN  
dbo.ObjectFieldValue SeverityRatingValue ON dbo.MetaObject.pkid=SeverityRatingValue.ObjectID and dbo.MetaObject.Machine=SeverityRatingValue.MachineID and SeverityRatingField.pkid = SeverityRatingValue.fieldid  
 INNER JOIN  dbo.Field  ConfigurationField  ON dbo.MetaObject.Class = ConfigurationField.Class left outer JOIN  
dbo.ObjectFieldValue ConfigurationValue ON dbo.MetaObject.pkid=ConfigurationValue.ObjectID and dbo.MetaObject.Machine=ConfigurationValue.MachineID and ConfigurationField.pkid = ConfigurationValue.fieldid  
 INNER JOIN  dbo.Field  MacAddressField  ON dbo.MetaObject.Class = MacAddressField.Class left outer JOIN  
dbo.ObjectFieldValue MacAddressValue ON dbo.MetaObject.pkid=MacAddressValue.ObjectID and dbo.MetaObject.Machine=MacAddressValue.MachineID and MacAddressField.pkid = MacAddressValue.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress2Field  ON dbo.MetaObject.Class = NetworkAddress2Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress2Value ON dbo.MetaObject.pkid=NetworkAddress2Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress2Value.MachineID and NetworkAddress2Field.pkid = NetworkAddress2Value.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress3Field  ON dbo.MetaObject.Class = NetworkAddress3Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress3Value ON dbo.MetaObject.pkid=NetworkAddress3Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress3Value.MachineID and NetworkAddress3Field.pkid = NetworkAddress3Value.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress4Field  ON dbo.MetaObject.Class = NetworkAddress4Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress4Value ON dbo.MetaObject.pkid=NetworkAddress4Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress4Value.MachineID and NetworkAddress4Field.pkid = NetworkAddress4Value.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress5Field  ON dbo.MetaObject.Class = NetworkAddress5Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress5Value ON dbo.MetaObject.pkid=NetworkAddress5Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress5Value.MachineID and NetworkAddress5Field.pkid = NetworkAddress5Value.fieldid  
 INNER JOIN  dbo.Field  MakeField  ON dbo.MetaObject.Class = MakeField.Class left outer JOIN  
dbo.ObjectFieldValue MakeValue ON dbo.MetaObject.pkid=MakeValue.ObjectID and dbo.MetaObject.Machine=MakeValue.MachineID and MakeField.pkid = MakeValue.fieldid  
 INNER JOIN  dbo.Field  ModelField  ON dbo.MetaObject.Class = ModelField.Class left outer JOIN  
dbo.ObjectFieldValue ModelValue ON dbo.MetaObject.pkid=ModelValue.ObjectID and dbo.MetaObject.Machine=ModelValue.MachineID and ModelField.pkid = ModelValue.fieldid  
 INNER JOIN  dbo.Field  SerialNumberField  ON dbo.MetaObject.Class = SerialNumberField.Class left outer JOIN  
dbo.ObjectFieldValue SerialNumberValue ON dbo.MetaObject.pkid=SerialNumberValue.ObjectID and dbo.MetaObject.Machine=SerialNumberValue.MachineID and SerialNumberField.pkid = SerialNumberValue.fieldid  
 INNER JOIN  dbo.Field  AssetNumberField  ON dbo.MetaObject.Class = AssetNumberField.Class left outer JOIN  
dbo.ObjectFieldValue AssetNumberValue ON dbo.MetaObject.pkid=AssetNumberValue.ObjectID and dbo.MetaObject.Machine=AssetNumberValue.MachineID and AssetNumberField.pkid = AssetNumberValue.fieldid  
 INNER JOIN  dbo.Field  ConnectionSpeedField  ON dbo.MetaObject.Class = ConnectionSpeedField.Class left outer JOIN  
dbo.ObjectFieldValue ConnectionSpeedValue ON dbo.MetaObject.pkid=ConnectionSpeedValue.ObjectID and dbo.MetaObject.Machine=ConnectionSpeedValue.MachineID and ConnectionSpeedField.pkid = ConnectionSpeedValue.fieldid  
 INNER JOIN  dbo.Field  Number_of_PortsField  ON dbo.MetaObject.Class = Number_of_PortsField.Class left outer JOIN  
dbo.ObjectFieldValue Number_of_PortsValue ON dbo.MetaObject.pkid=Number_of_PortsValue.ObjectID and dbo.MetaObject.Machine=Number_of_PortsValue.MachineID and Number_of_PortsField.pkid = Number_of_PortsValue.fieldid  
 INNER JOIN  dbo.Field  Number_of_Ports_AvailableField  ON dbo.MetaObject.Class = Number_of_Ports_AvailableField.Class left outer JOIN  
dbo.ObjectFieldValue Number_of_Ports_AvailableValue ON dbo.MetaObject.pkid=Number_of_Ports_AvailableValue.ObjectID and dbo.MetaObject.Machine=Number_of_Ports_AvailableValue.MachineID and Number_of_Ports_AvailableField.pkid = Number_of_Ports_AvailableValue.fieldid  
 INNER JOIN  dbo.Field  RangeField  ON dbo.MetaObject.Class = RangeField.Class left outer JOIN  
dbo.ObjectFieldValue RangeValue ON dbo.MetaObject.pkid=RangeValue.ObjectID and dbo.MetaObject.Machine=RangeValue.MachineID and RangeField.pkid = RangeValue.fieldid  
 INNER JOIN  dbo.Field  isDNSField  ON dbo.MetaObject.Class = isDNSField.Class left outer JOIN  
dbo.ObjectFieldValue isDNSValue ON dbo.MetaObject.pkid=isDNSValue.ObjectID and dbo.MetaObject.Machine=isDNSValue.MachineID and isDNSField.pkid = isDNSValue.fieldid  
 INNER JOIN  dbo.Field  isDHCPField  ON dbo.MetaObject.Class = isDHCPField.Class left outer JOIN  
dbo.ObjectFieldValue isDHCPValue ON dbo.MetaObject.pkid=isDHCPValue.ObjectID and dbo.MetaObject.Machine=isDHCPValue.MachineID and isDHCPField.pkid = isDHCPValue.fieldid  
 INNER JOIN  dbo.Field  isManagedField  ON dbo.MetaObject.Class = isManagedField.Class left outer JOIN  
dbo.ObjectFieldValue isManagedValue ON dbo.MetaObject.pkid=isManagedValue.ObjectID and dbo.MetaObject.Machine=isManagedValue.MachineID and isManagedField.pkid = isManagedValue.fieldid  
 INNER JOIN  dbo.Field  Mem_TotalField  ON dbo.MetaObject.Class = Mem_TotalField.Class left outer JOIN  
dbo.ObjectFieldValue Mem_TotalValue ON dbo.MetaObject.pkid=Mem_TotalValue.ObjectID and dbo.MetaObject.Machine=Mem_TotalValue.MachineID and Mem_TotalField.pkid = Mem_TotalValue.fieldid  
 INNER JOIN  dbo.Field  DatePurchasedField  ON dbo.MetaObject.Class = DatePurchasedField.Class left outer JOIN  
dbo.ObjectFieldValue DatePurchasedValue ON dbo.MetaObject.pkid=DatePurchasedValue.ObjectID and dbo.MetaObject.Machine=DatePurchasedValue.MachineID and DatePurchasedField.pkid = DatePurchasedValue.fieldid  
 INNER JOIN  dbo.Field  UnderWarrantyField  ON dbo.MetaObject.Class = UnderWarrantyField.Class left outer JOIN  
dbo.ObjectFieldValue UnderWarrantyValue ON dbo.MetaObject.pkid=UnderWarrantyValue.ObjectID and dbo.MetaObject.Machine=UnderWarrantyValue.MachineID and UnderWarrantyField.pkid = UnderWarrantyValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  NetworkTypeField  ON dbo.MetaObject.Class = NetworkTypeField.Class left outer JOIN  
dbo.ObjectFieldValue NetworkTypeValue ON dbo.MetaObject.pkid=NetworkTypeValue.ObjectID and dbo.MetaObject.Machine=NetworkTypeValue.MachineID and NetworkTypeField.pkid = NetworkTypeValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'NetworkComponent')  AND TypeField.Name ='Type'  AND NameField.Name ='Name'  AND DescriptionField.Name ='Description'  AND SeverityRatingField.Name ='SeverityRating'  AND ConfigurationField.Name ='Configuration'  AND MacAddressField.Name ='MacAddress'  AND NetworkAddress2Field.Name ='NetworkAddress2'  AND NetworkAddress3Field.Name ='NetworkAddress3'  AND NetworkAddress4Field.Name ='NetworkAddress4'  AND NetworkAddress5Field.Name ='NetworkAddress5'  AND MakeField.Name ='Make'  AND ModelField.Name ='Model'  AND SerialNumberField.Name ='SerialNumber'  AND AssetNumberField.Name ='AssetNumber'  AND ConnectionSpeedField.Name ='ConnectionSpeed'  AND Number_of_PortsField.Name ='Number_of_Ports'  AND Number_of_Ports_AvailableField.Name ='Number_of_Ports_Available'  AND RangeField.Name ='Range'  AND isDNSField.Name ='isDNS'  AND isDHCPField.Name ='isDHCP'  AND isManagedField.Name ='isManaged'  AND Mem_TotalField.Name ='Mem_Total'  AND DatePurchasedField.Name ='DatePurchased'  AND UnderWarrantyField.Name ='UnderWarranty'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND NetworkTypeField.Name ='NetworkType'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Object_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Object_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,TypeValue.ValueString as Type,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  TypeField  ON dbo.MetaObject.Class = TypeField.Class left outer JOIN  
dbo.ObjectFieldValue TypeValue ON dbo.MetaObject.pkid=TypeValue.ObjectID and dbo.MetaObject.Machine=TypeValue.MachineID and TypeField.pkid = TypeValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Object')  AND NameField.Name ='Name'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND TypeField.Name ='Type'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Object_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Object_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,TypeValue.ValueString as Type,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  TypeField  ON dbo.MetaObject.Class = TypeField.Class left outer JOIN  
dbo.ObjectFieldValue TypeValue ON dbo.MetaObject.pkid=TypeValue.ObjectID and dbo.MetaObject.Machine=TypeValue.MachineID and TypeField.pkid = TypeValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Object')  AND NameField.Name ='Name'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND TypeField.Name ='Type'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_OrganizationalUnit_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_OrganizationalUnit_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,TypeValue.ValueString as Type,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,TelephoneValue.ValueString as Telephone,FaxValue.ValueString as Fax,EmailValue.ValueString as Email,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  TypeField  ON dbo.MetaObject.Class = TypeField.Class left outer JOIN  
dbo.ObjectFieldValue TypeValue ON dbo.MetaObject.pkid=TypeValue.ObjectID and dbo.MetaObject.Machine=TypeValue.MachineID and TypeField.pkid = TypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  TelephoneField  ON dbo.MetaObject.Class = TelephoneField.Class left outer JOIN  
dbo.ObjectFieldValue TelephoneValue ON dbo.MetaObject.pkid=TelephoneValue.ObjectID and dbo.MetaObject.Machine=TelephoneValue.MachineID and TelephoneField.pkid = TelephoneValue.fieldid  
 INNER JOIN  dbo.Field  FaxField  ON dbo.MetaObject.Class = FaxField.Class left outer JOIN  
dbo.ObjectFieldValue FaxValue ON dbo.MetaObject.pkid=FaxValue.ObjectID and dbo.MetaObject.Machine=FaxValue.MachineID and FaxField.pkid = FaxValue.fieldid  
 INNER JOIN  dbo.Field  EmailField  ON dbo.MetaObject.Class = EmailField.Class left outer JOIN  
dbo.ObjectFieldValue EmailValue ON dbo.MetaObject.pkid=EmailValue.ObjectID and dbo.MetaObject.Machine=EmailValue.MachineID and EmailField.pkid = EmailValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'OrganizationalUnit')  AND NameField.Name ='Name'  AND TypeField.Name ='Type'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND TelephoneField.Name ='Telephone'  AND FaxField.Name ='Fax'  AND EmailField.Name ='Email'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_OrganizationalUnit_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_OrganizationalUnit_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,TypeValue.ValueString as Type,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,TelephoneValue.ValueString as Telephone,FaxValue.ValueString as Fax,EmailValue.ValueString as Email,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  TypeField  ON dbo.MetaObject.Class = TypeField.Class left outer JOIN  
dbo.ObjectFieldValue TypeValue ON dbo.MetaObject.pkid=TypeValue.ObjectID and dbo.MetaObject.Machine=TypeValue.MachineID and TypeField.pkid = TypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  TelephoneField  ON dbo.MetaObject.Class = TelephoneField.Class left outer JOIN  
dbo.ObjectFieldValue TelephoneValue ON dbo.MetaObject.pkid=TelephoneValue.ObjectID and dbo.MetaObject.Machine=TelephoneValue.MachineID and TelephoneField.pkid = TelephoneValue.fieldid  
 INNER JOIN  dbo.Field  FaxField  ON dbo.MetaObject.Class = FaxField.Class left outer JOIN  
dbo.ObjectFieldValue FaxValue ON dbo.MetaObject.pkid=FaxValue.ObjectID and dbo.MetaObject.Machine=FaxValue.MachineID and FaxField.pkid = FaxValue.fieldid  
 INNER JOIN  dbo.Field  EmailField  ON dbo.MetaObject.Class = EmailField.Class left outer JOIN  
dbo.ObjectFieldValue EmailValue ON dbo.MetaObject.pkid=EmailValue.ObjectID and dbo.MetaObject.Machine=EmailValue.MachineID and EmailField.pkid = EmailValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'OrganizationalUnit')  AND NameField.Name ='Name'  AND TypeField.Name ='Type'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND TelephoneField.Name ='Telephone'  AND FaxField.Name ='Fax'  AND EmailField.Name ='Email'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Peripheral_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Peripheral_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,TypeValue.ValueString as Type,NameValue.ValueString as Name,DescriptionValue.ValueString as Description,SeverityRatingValue.ValueString as SeverityRating,ConfigurationValue.ValueLongText as Configuration,NetworkAddress1Value.ValueString as NetworkAddress1,NetworkAddress2Value.ValueString as NetworkAddress2,NetworkAddress3Value.ValueString as NetworkAddress3,NetworkAddress4Value.ValueString as NetworkAddress4,NetworkAddress5Value.ValueString as NetworkAddress5,MakeValue.ValueString as Make,ModelValue.ValueString as Model,SerialNumberValue.ValueString as SerialNumber,AssetNumberValue.ValueString as AssetNumber,Copy_PPMValue.ValueString as Copy_PPM,Print_PPMValue.ValueString as Print_PPM,isColorValue.ValueString as isColor,isManagedValue.ValueString as isManaged,isNetworkValue.ValueString as isNetwork,DatePurchasedValue.ValueString as DatePurchased,UnderWarrantyValue.ValueString as UnderWarranty,ContractValue.ValueString as Contract,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  TypeField  ON dbo.MetaObject.Class = TypeField.Class left outer JOIN  
dbo.ObjectFieldValue TypeValue ON dbo.MetaObject.pkid=TypeValue.ObjectID and dbo.MetaObject.Machine=TypeValue.MachineID and TypeField.pkid = TypeValue.fieldid  
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  DescriptionField  ON dbo.MetaObject.Class = DescriptionField.Class left outer JOIN  
dbo.ObjectFieldValue DescriptionValue ON dbo.MetaObject.pkid=DescriptionValue.ObjectID and dbo.MetaObject.Machine=DescriptionValue.MachineID and DescriptionField.pkid = DescriptionValue.fieldid  
 INNER JOIN  dbo.Field  SeverityRatingField  ON dbo.MetaObject.Class = SeverityRatingField.Class left outer JOIN  
dbo.ObjectFieldValue SeverityRatingValue ON dbo.MetaObject.pkid=SeverityRatingValue.ObjectID and dbo.MetaObject.Machine=SeverityRatingValue.MachineID and SeverityRatingField.pkid = SeverityRatingValue.fieldid  
 INNER JOIN  dbo.Field  ConfigurationField  ON dbo.MetaObject.Class = ConfigurationField.Class left outer JOIN  
dbo.ObjectFieldValue ConfigurationValue ON dbo.MetaObject.pkid=ConfigurationValue.ObjectID and dbo.MetaObject.Machine=ConfigurationValue.MachineID and ConfigurationField.pkid = ConfigurationValue.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress1Field  ON dbo.MetaObject.Class = NetworkAddress1Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress1Value ON dbo.MetaObject.pkid=NetworkAddress1Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress1Value.MachineID and NetworkAddress1Field.pkid = NetworkAddress1Value.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress2Field  ON dbo.MetaObject.Class = NetworkAddress2Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress2Value ON dbo.MetaObject.pkid=NetworkAddress2Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress2Value.MachineID and NetworkAddress2Field.pkid = NetworkAddress2Value.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress3Field  ON dbo.MetaObject.Class = NetworkAddress3Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress3Value ON dbo.MetaObject.pkid=NetworkAddress3Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress3Value.MachineID and NetworkAddress3Field.pkid = NetworkAddress3Value.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress4Field  ON dbo.MetaObject.Class = NetworkAddress4Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress4Value ON dbo.MetaObject.pkid=NetworkAddress4Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress4Value.MachineID and NetworkAddress4Field.pkid = NetworkAddress4Value.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress5Field  ON dbo.MetaObject.Class = NetworkAddress5Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress5Value ON dbo.MetaObject.pkid=NetworkAddress5Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress5Value.MachineID and NetworkAddress5Field.pkid = NetworkAddress5Value.fieldid  
 INNER JOIN  dbo.Field  MakeField  ON dbo.MetaObject.Class = MakeField.Class left outer JOIN  
dbo.ObjectFieldValue MakeValue ON dbo.MetaObject.pkid=MakeValue.ObjectID and dbo.MetaObject.Machine=MakeValue.MachineID and MakeField.pkid = MakeValue.fieldid  
 INNER JOIN  dbo.Field  ModelField  ON dbo.MetaObject.Class = ModelField.Class left outer JOIN  
dbo.ObjectFieldValue ModelValue ON dbo.MetaObject.pkid=ModelValue.ObjectID and dbo.MetaObject.Machine=ModelValue.MachineID and ModelField.pkid = ModelValue.fieldid  
 INNER JOIN  dbo.Field  SerialNumberField  ON dbo.MetaObject.Class = SerialNumberField.Class left outer JOIN  
dbo.ObjectFieldValue SerialNumberValue ON dbo.MetaObject.pkid=SerialNumberValue.ObjectID and dbo.MetaObject.Machine=SerialNumberValue.MachineID and SerialNumberField.pkid = SerialNumberValue.fieldid  
 INNER JOIN  dbo.Field  AssetNumberField  ON dbo.MetaObject.Class = AssetNumberField.Class left outer JOIN  
dbo.ObjectFieldValue AssetNumberValue ON dbo.MetaObject.pkid=AssetNumberValue.ObjectID and dbo.MetaObject.Machine=AssetNumberValue.MachineID and AssetNumberField.pkid = AssetNumberValue.fieldid  
 INNER JOIN  dbo.Field  Copy_PPMField  ON dbo.MetaObject.Class = Copy_PPMField.Class left outer JOIN  
dbo.ObjectFieldValue Copy_PPMValue ON dbo.MetaObject.pkid=Copy_PPMValue.ObjectID and dbo.MetaObject.Machine=Copy_PPMValue.MachineID and Copy_PPMField.pkid = Copy_PPMValue.fieldid  
 INNER JOIN  dbo.Field  Print_PPMField  ON dbo.MetaObject.Class = Print_PPMField.Class left outer JOIN  
dbo.ObjectFieldValue Print_PPMValue ON dbo.MetaObject.pkid=Print_PPMValue.ObjectID and dbo.MetaObject.Machine=Print_PPMValue.MachineID and Print_PPMField.pkid = Print_PPMValue.fieldid  
 INNER JOIN  dbo.Field  isColorField  ON dbo.MetaObject.Class = isColorField.Class left outer JOIN  
dbo.ObjectFieldValue isColorValue ON dbo.MetaObject.pkid=isColorValue.ObjectID and dbo.MetaObject.Machine=isColorValue.MachineID and isColorField.pkid = isColorValue.fieldid  
 INNER JOIN  dbo.Field  isManagedField  ON dbo.MetaObject.Class = isManagedField.Class left outer JOIN  
dbo.ObjectFieldValue isManagedValue ON dbo.MetaObject.pkid=isManagedValue.ObjectID and dbo.MetaObject.Machine=isManagedValue.MachineID and isManagedField.pkid = isManagedValue.fieldid  
 INNER JOIN  dbo.Field  isNetworkField  ON dbo.MetaObject.Class = isNetworkField.Class left outer JOIN  
dbo.ObjectFieldValue isNetworkValue ON dbo.MetaObject.pkid=isNetworkValue.ObjectID and dbo.MetaObject.Machine=isNetworkValue.MachineID and isNetworkField.pkid = isNetworkValue.fieldid  
 INNER JOIN  dbo.Field  DatePurchasedField  ON dbo.MetaObject.Class = DatePurchasedField.Class left outer JOIN  
dbo.ObjectFieldValue DatePurchasedValue ON dbo.MetaObject.pkid=DatePurchasedValue.ObjectID and dbo.MetaObject.Machine=DatePurchasedValue.MachineID and DatePurchasedField.pkid = DatePurchasedValue.fieldid  
 INNER JOIN  dbo.Field  UnderWarrantyField  ON dbo.MetaObject.Class = UnderWarrantyField.Class left outer JOIN  
dbo.ObjectFieldValue UnderWarrantyValue ON dbo.MetaObject.pkid=UnderWarrantyValue.ObjectID and dbo.MetaObject.Machine=UnderWarrantyValue.MachineID and UnderWarrantyField.pkid = UnderWarrantyValue.fieldid  
 INNER JOIN  dbo.Field  ContractField  ON dbo.MetaObject.Class = ContractField.Class left outer JOIN  
dbo.ObjectFieldValue ContractValue ON dbo.MetaObject.pkid=ContractValue.ObjectID and dbo.MetaObject.Machine=ContractValue.MachineID and ContractField.pkid = ContractValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Peripheral')  AND TypeField.Name ='Type'  AND NameField.Name ='Name'  AND DescriptionField.Name ='Description'  AND SeverityRatingField.Name ='SeverityRating'  AND ConfigurationField.Name ='Configuration'  AND NetworkAddress1Field.Name ='NetworkAddress1'  AND NetworkAddress2Field.Name ='NetworkAddress2'  AND NetworkAddress3Field.Name ='NetworkAddress3'  AND NetworkAddress4Field.Name ='NetworkAddress4'  AND NetworkAddress5Field.Name ='NetworkAddress5'  AND MakeField.Name ='Make'  AND ModelField.Name ='Model'  AND SerialNumberField.Name ='SerialNumber'  AND AssetNumberField.Name ='AssetNumber'  AND Copy_PPMField.Name ='Copy_PPM'  AND Print_PPMField.Name ='Print_PPM'  AND isColorField.Name ='isColor'  AND isManagedField.Name ='isManaged'  AND isNetworkField.Name ='isNetwork'  AND DatePurchasedField.Name ='DatePurchased'  AND UnderWarrantyField.Name ='UnderWarranty'  AND ContractField.Name ='Contract'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Peripheral_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Peripheral_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,TypeValue.ValueString as Type,NameValue.ValueString as Name,DescriptionValue.ValueString as Description,SeverityRatingValue.ValueString as SeverityRating,ConfigurationValue.ValueRTF as Configuration,NetworkAddress1Value.ValueString as NetworkAddress1,NetworkAddress2Value.ValueString as NetworkAddress2,NetworkAddress3Value.ValueString as NetworkAddress3,NetworkAddress4Value.ValueString as NetworkAddress4,NetworkAddress5Value.ValueString as NetworkAddress5,MakeValue.ValueString as Make,ModelValue.ValueString as Model,SerialNumberValue.ValueString as SerialNumber,AssetNumberValue.ValueString as AssetNumber,Copy_PPMValue.ValueString as Copy_PPM,Print_PPMValue.ValueString as Print_PPM,isColorValue.ValueString as isColor,isManagedValue.ValueString as isManaged,isNetworkValue.ValueString as isNetwork,DatePurchasedValue.ValueString as DatePurchased,UnderWarrantyValue.ValueString as UnderWarranty,ContractValue.ValueString as Contract,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  TypeField  ON dbo.MetaObject.Class = TypeField.Class left outer JOIN  
dbo.ObjectFieldValue TypeValue ON dbo.MetaObject.pkid=TypeValue.ObjectID and dbo.MetaObject.Machine=TypeValue.MachineID and TypeField.pkid = TypeValue.fieldid  
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  DescriptionField  ON dbo.MetaObject.Class = DescriptionField.Class left outer JOIN  
dbo.ObjectFieldValue DescriptionValue ON dbo.MetaObject.pkid=DescriptionValue.ObjectID and dbo.MetaObject.Machine=DescriptionValue.MachineID and DescriptionField.pkid = DescriptionValue.fieldid  
 INNER JOIN  dbo.Field  SeverityRatingField  ON dbo.MetaObject.Class = SeverityRatingField.Class left outer JOIN  
dbo.ObjectFieldValue SeverityRatingValue ON dbo.MetaObject.pkid=SeverityRatingValue.ObjectID and dbo.MetaObject.Machine=SeverityRatingValue.MachineID and SeverityRatingField.pkid = SeverityRatingValue.fieldid  
 INNER JOIN  dbo.Field  ConfigurationField  ON dbo.MetaObject.Class = ConfigurationField.Class left outer JOIN  
dbo.ObjectFieldValue ConfigurationValue ON dbo.MetaObject.pkid=ConfigurationValue.ObjectID and dbo.MetaObject.Machine=ConfigurationValue.MachineID and ConfigurationField.pkid = ConfigurationValue.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress1Field  ON dbo.MetaObject.Class = NetworkAddress1Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress1Value ON dbo.MetaObject.pkid=NetworkAddress1Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress1Value.MachineID and NetworkAddress1Field.pkid = NetworkAddress1Value.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress2Field  ON dbo.MetaObject.Class = NetworkAddress2Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress2Value ON dbo.MetaObject.pkid=NetworkAddress2Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress2Value.MachineID and NetworkAddress2Field.pkid = NetworkAddress2Value.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress3Field  ON dbo.MetaObject.Class = NetworkAddress3Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress3Value ON dbo.MetaObject.pkid=NetworkAddress3Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress3Value.MachineID and NetworkAddress3Field.pkid = NetworkAddress3Value.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress4Field  ON dbo.MetaObject.Class = NetworkAddress4Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress4Value ON dbo.MetaObject.pkid=NetworkAddress4Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress4Value.MachineID and NetworkAddress4Field.pkid = NetworkAddress4Value.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress5Field  ON dbo.MetaObject.Class = NetworkAddress5Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress5Value ON dbo.MetaObject.pkid=NetworkAddress5Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress5Value.MachineID and NetworkAddress5Field.pkid = NetworkAddress5Value.fieldid  
 INNER JOIN  dbo.Field  MakeField  ON dbo.MetaObject.Class = MakeField.Class left outer JOIN  
dbo.ObjectFieldValue MakeValue ON dbo.MetaObject.pkid=MakeValue.ObjectID and dbo.MetaObject.Machine=MakeValue.MachineID and MakeField.pkid = MakeValue.fieldid  
 INNER JOIN  dbo.Field  ModelField  ON dbo.MetaObject.Class = ModelField.Class left outer JOIN  
dbo.ObjectFieldValue ModelValue ON dbo.MetaObject.pkid=ModelValue.ObjectID and dbo.MetaObject.Machine=ModelValue.MachineID and ModelField.pkid = ModelValue.fieldid  
 INNER JOIN  dbo.Field  SerialNumberField  ON dbo.MetaObject.Class = SerialNumberField.Class left outer JOIN  
dbo.ObjectFieldValue SerialNumberValue ON dbo.MetaObject.pkid=SerialNumberValue.ObjectID and dbo.MetaObject.Machine=SerialNumberValue.MachineID and SerialNumberField.pkid = SerialNumberValue.fieldid  
 INNER JOIN  dbo.Field  AssetNumberField  ON dbo.MetaObject.Class = AssetNumberField.Class left outer JOIN  
dbo.ObjectFieldValue AssetNumberValue ON dbo.MetaObject.pkid=AssetNumberValue.ObjectID and dbo.MetaObject.Machine=AssetNumberValue.MachineID and AssetNumberField.pkid = AssetNumberValue.fieldid  
 INNER JOIN  dbo.Field  Copy_PPMField  ON dbo.MetaObject.Class = Copy_PPMField.Class left outer JOIN  
dbo.ObjectFieldValue Copy_PPMValue ON dbo.MetaObject.pkid=Copy_PPMValue.ObjectID and dbo.MetaObject.Machine=Copy_PPMValue.MachineID and Copy_PPMField.pkid = Copy_PPMValue.fieldid  
 INNER JOIN  dbo.Field  Print_PPMField  ON dbo.MetaObject.Class = Print_PPMField.Class left outer JOIN  
dbo.ObjectFieldValue Print_PPMValue ON dbo.MetaObject.pkid=Print_PPMValue.ObjectID and dbo.MetaObject.Machine=Print_PPMValue.MachineID and Print_PPMField.pkid = Print_PPMValue.fieldid  
 INNER JOIN  dbo.Field  isColorField  ON dbo.MetaObject.Class = isColorField.Class left outer JOIN  
dbo.ObjectFieldValue isColorValue ON dbo.MetaObject.pkid=isColorValue.ObjectID and dbo.MetaObject.Machine=isColorValue.MachineID and isColorField.pkid = isColorValue.fieldid  
 INNER JOIN  dbo.Field  isManagedField  ON dbo.MetaObject.Class = isManagedField.Class left outer JOIN  
dbo.ObjectFieldValue isManagedValue ON dbo.MetaObject.pkid=isManagedValue.ObjectID and dbo.MetaObject.Machine=isManagedValue.MachineID and isManagedField.pkid = isManagedValue.fieldid  
 INNER JOIN  dbo.Field  isNetworkField  ON dbo.MetaObject.Class = isNetworkField.Class left outer JOIN  
dbo.ObjectFieldValue isNetworkValue ON dbo.MetaObject.pkid=isNetworkValue.ObjectID and dbo.MetaObject.Machine=isNetworkValue.MachineID and isNetworkField.pkid = isNetworkValue.fieldid  
 INNER JOIN  dbo.Field  DatePurchasedField  ON dbo.MetaObject.Class = DatePurchasedField.Class left outer JOIN  
dbo.ObjectFieldValue DatePurchasedValue ON dbo.MetaObject.pkid=DatePurchasedValue.ObjectID and dbo.MetaObject.Machine=DatePurchasedValue.MachineID and DatePurchasedField.pkid = DatePurchasedValue.fieldid  
 INNER JOIN  dbo.Field  UnderWarrantyField  ON dbo.MetaObject.Class = UnderWarrantyField.Class left outer JOIN  
dbo.ObjectFieldValue UnderWarrantyValue ON dbo.MetaObject.pkid=UnderWarrantyValue.ObjectID and dbo.MetaObject.Machine=UnderWarrantyValue.MachineID and UnderWarrantyField.pkid = UnderWarrantyValue.fieldid  
 INNER JOIN  dbo.Field  ContractField  ON dbo.MetaObject.Class = ContractField.Class left outer JOIN  
dbo.ObjectFieldValue ContractValue ON dbo.MetaObject.pkid=ContractValue.ObjectID and dbo.MetaObject.Machine=ContractValue.MachineID and ContractField.pkid = ContractValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Peripheral')  AND TypeField.Name ='Type'  AND NameField.Name ='Name'  AND DescriptionField.Name ='Description'  AND SeverityRatingField.Name ='SeverityRating'  AND ConfigurationField.Name ='Configuration'  AND NetworkAddress1Field.Name ='NetworkAddress1'  AND NetworkAddress2Field.Name ='NetworkAddress2'  AND NetworkAddress3Field.Name ='NetworkAddress3'  AND NetworkAddress4Field.Name ='NetworkAddress4'  AND NetworkAddress5Field.Name ='NetworkAddress5'  AND MakeField.Name ='Make'  AND ModelField.Name ='Model'  AND SerialNumberField.Name ='SerialNumber'  AND AssetNumberField.Name ='AssetNumber'  AND Copy_PPMField.Name ='Copy_PPM'  AND Print_PPMField.Name ='Print_PPM'  AND isColorField.Name ='isColor'  AND isManagedField.Name ='isManaged'  AND isNetworkField.Name ='isNetwork'  AND DatePurchasedField.Name ='DatePurchased'  AND UnderWarrantyField.Name ='UnderWarranty'  AND ContractField.Name ='Contract'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_ProbOfRealization_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_ProbOfRealization_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,ValueValue.ValueString as Value,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  ValueField  ON dbo.MetaObject.Class = ValueField.Class left outer JOIN  
dbo.ObjectFieldValue ValueValue ON dbo.MetaObject.pkid=ValueValue.ObjectID and dbo.MetaObject.Machine=ValueValue.MachineID and ValueField.pkid = ValueValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'ProbOfRealization')  AND ValueField.Name ='Value'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_ProbOfRealization_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_ProbOfRealization_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,ValueValue.ValueString as Value,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  ValueField  ON dbo.MetaObject.Class = ValueField.Class left outer JOIN  
dbo.ObjectFieldValue ValueValue ON dbo.MetaObject.pkid=ValueValue.ObjectID and dbo.MetaObject.Machine=ValueValue.MachineID and ValueField.pkid = ValueValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'ProbOfRealization')  AND ValueField.Name ='Value'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Process_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Process_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,ExecutionIndicatorValue.ValueString as ExecutionIndicator,ContextualIndicatorValue.ValueString as ContextualIndicator,SequenceNumberValue.ValueString as SequenceNumber,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  ExecutionIndicatorField  ON dbo.MetaObject.Class = ExecutionIndicatorField.Class left outer JOIN  
dbo.ObjectFieldValue ExecutionIndicatorValue ON dbo.MetaObject.pkid=ExecutionIndicatorValue.ObjectID and dbo.MetaObject.Machine=ExecutionIndicatorValue.MachineID and ExecutionIndicatorField.pkid = ExecutionIndicatorValue.fieldid  
 INNER JOIN  dbo.Field  ContextualIndicatorField  ON dbo.MetaObject.Class = ContextualIndicatorField.Class left outer JOIN  
dbo.ObjectFieldValue ContextualIndicatorValue ON dbo.MetaObject.pkid=ContextualIndicatorValue.ObjectID and dbo.MetaObject.Machine=ContextualIndicatorValue.MachineID and ContextualIndicatorField.pkid = ContextualIndicatorValue.fieldid  
 INNER JOIN  dbo.Field  SequenceNumberField  ON dbo.MetaObject.Class = SequenceNumberField.Class left outer JOIN  
dbo.ObjectFieldValue SequenceNumberValue ON dbo.MetaObject.pkid=SequenceNumberValue.ObjectID and dbo.MetaObject.Machine=SequenceNumberValue.MachineID and SequenceNumberField.pkid = SequenceNumberValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Process')  AND NameField.Name ='Name'  AND ExecutionIndicatorField.Name ='ExecutionIndicator'  AND ContextualIndicatorField.Name ='ContextualIndicator'  AND SequenceNumberField.Name ='SequenceNumber'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Process_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Process_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,ExecutionIndicatorValue.ValueString as ExecutionIndicator,ContextualIndicatorValue.ValueString as ContextualIndicator,SequenceNumberValue.ValueString as SequenceNumber,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  ExecutionIndicatorField  ON dbo.MetaObject.Class = ExecutionIndicatorField.Class left outer JOIN  
dbo.ObjectFieldValue ExecutionIndicatorValue ON dbo.MetaObject.pkid=ExecutionIndicatorValue.ObjectID and dbo.MetaObject.Machine=ExecutionIndicatorValue.MachineID and ExecutionIndicatorField.pkid = ExecutionIndicatorValue.fieldid  
 INNER JOIN  dbo.Field  ContextualIndicatorField  ON dbo.MetaObject.Class = ContextualIndicatorField.Class left outer JOIN  
dbo.ObjectFieldValue ContextualIndicatorValue ON dbo.MetaObject.pkid=ContextualIndicatorValue.ObjectID and dbo.MetaObject.Machine=ContextualIndicatorValue.MachineID and ContextualIndicatorField.pkid = ContextualIndicatorValue.fieldid  
 INNER JOIN  dbo.Field  SequenceNumberField  ON dbo.MetaObject.Class = SequenceNumberField.Class left outer JOIN  
dbo.ObjectFieldValue SequenceNumberValue ON dbo.MetaObject.pkid=SequenceNumberValue.ObjectID and dbo.MetaObject.Machine=SequenceNumberValue.MachineID and SequenceNumberField.pkid = SequenceNumberValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Process')  AND NameField.Name ='Name'  AND ExecutionIndicatorField.Name ='ExecutionIndicator'  AND ContextualIndicatorField.Name ='ContextualIndicator'  AND SequenceNumberField.Name ='SequenceNumber'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Rationale_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Rationale_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,UniqueRefValue.ValueString as UniqueRef,RationaleTypeValue.ValueString as RationaleType,ValueValue.ValueString as Value,AuthorNameValue.ValueString as AuthorName,EffectiveDateValue.ValueString as EffectiveDate,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  UniqueRefField  ON dbo.MetaObject.Class = UniqueRefField.Class left outer JOIN  
dbo.ObjectFieldValue UniqueRefValue ON dbo.MetaObject.pkid=UniqueRefValue.ObjectID and dbo.MetaObject.Machine=UniqueRefValue.MachineID and UniqueRefField.pkid = UniqueRefValue.fieldid  
 INNER JOIN  dbo.Field  RationaleTypeField  ON dbo.MetaObject.Class = RationaleTypeField.Class left outer JOIN  
dbo.ObjectFieldValue RationaleTypeValue ON dbo.MetaObject.pkid=RationaleTypeValue.ObjectID and dbo.MetaObject.Machine=RationaleTypeValue.MachineID and RationaleTypeField.pkid = RationaleTypeValue.fieldid  
 INNER JOIN  dbo.Field  ValueField  ON dbo.MetaObject.Class = ValueField.Class left outer JOIN  
dbo.ObjectFieldValue ValueValue ON dbo.MetaObject.pkid=ValueValue.ObjectID and dbo.MetaObject.Machine=ValueValue.MachineID and ValueField.pkid = ValueValue.fieldid  
 INNER JOIN  dbo.Field  AuthorNameField  ON dbo.MetaObject.Class = AuthorNameField.Class left outer JOIN  
dbo.ObjectFieldValue AuthorNameValue ON dbo.MetaObject.pkid=AuthorNameValue.ObjectID and dbo.MetaObject.Machine=AuthorNameValue.MachineID and AuthorNameField.pkid = AuthorNameValue.fieldid  
 INNER JOIN  dbo.Field  EffectiveDateField  ON dbo.MetaObject.Class = EffectiveDateField.Class left outer JOIN  
dbo.ObjectFieldValue EffectiveDateValue ON dbo.MetaObject.pkid=EffectiveDateValue.ObjectID and dbo.MetaObject.Machine=EffectiveDateValue.MachineID and EffectiveDateField.pkid = EffectiveDateValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Rationale')  AND UniqueRefField.Name ='UniqueRef'  AND RationaleTypeField.Name ='RationaleType'  AND ValueField.Name ='Value'  AND AuthorNameField.Name ='AuthorName'  AND EffectiveDateField.Name ='EffectiveDate'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Rationale_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Rationale_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,UniqueRefValue.ValueString as UniqueRef,RationaleTypeValue.ValueString as RationaleType,ValueValue.ValueString as Value,AuthorNameValue.ValueString as AuthorName,EffectiveDateValue.ValueString as EffectiveDate,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  UniqueRefField  ON dbo.MetaObject.Class = UniqueRefField.Class left outer JOIN  
dbo.ObjectFieldValue UniqueRefValue ON dbo.MetaObject.pkid=UniqueRefValue.ObjectID and dbo.MetaObject.Machine=UniqueRefValue.MachineID and UniqueRefField.pkid = UniqueRefValue.fieldid  
 INNER JOIN  dbo.Field  RationaleTypeField  ON dbo.MetaObject.Class = RationaleTypeField.Class left outer JOIN  
dbo.ObjectFieldValue RationaleTypeValue ON dbo.MetaObject.pkid=RationaleTypeValue.ObjectID and dbo.MetaObject.Machine=RationaleTypeValue.MachineID and RationaleTypeField.pkid = RationaleTypeValue.fieldid  
 INNER JOIN  dbo.Field  ValueField  ON dbo.MetaObject.Class = ValueField.Class left outer JOIN  
dbo.ObjectFieldValue ValueValue ON dbo.MetaObject.pkid=ValueValue.ObjectID and dbo.MetaObject.Machine=ValueValue.MachineID and ValueField.pkid = ValueValue.fieldid  
 INNER JOIN  dbo.Field  AuthorNameField  ON dbo.MetaObject.Class = AuthorNameField.Class left outer JOIN  
dbo.ObjectFieldValue AuthorNameValue ON dbo.MetaObject.pkid=AuthorNameValue.ObjectID and dbo.MetaObject.Machine=AuthorNameValue.MachineID and AuthorNameField.pkid = AuthorNameValue.fieldid  
 INNER JOIN  dbo.Field  EffectiveDateField  ON dbo.MetaObject.Class = EffectiveDateField.Class left outer JOIN  
dbo.ObjectFieldValue EffectiveDateValue ON dbo.MetaObject.pkid=EffectiveDateValue.ObjectID and dbo.MetaObject.Machine=EffectiveDateValue.MachineID and EffectiveDateField.pkid = EffectiveDateValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Rationale')  AND UniqueRefField.Name ='UniqueRef'  AND RationaleTypeField.Name ='RationaleType'  AND ValueField.Name ='Value'  AND AuthorNameField.Name ='AuthorName'  AND EffectiveDateField.Name ='EffectiveDate'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_ResourceMappingValue_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  View [dbo].[METAView_ResourceMappingValue_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  View [dbo].[METAView_Responsibility_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Responsibility_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Responsibility')  AND NameField.Name ='Name'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Responsibility_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Responsibility_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Responsibility')  AND NameField.Name ='Name'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Role_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Role_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,GapTypeValue.ValueString as GapType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3 FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 WHERE (dbo.MetaObject.Class = 'Role')  AND NameField.Name ='Name'  AND GapTypeField.Name ='GapType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3' 

GO
/****** Object:  View [dbo].[METAView_Role_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Role_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,GapTypeValue.ValueString as GapType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3 FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 WHERE (dbo.MetaObject.Class = 'Role')  AND NameField.Name ='Name'  AND GapTypeField.Name ='GapType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3' 

GO
/****** Object:  View [dbo].[METAView_Scenario_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Scenario_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,Acc_Prob_of_RealValue.ValueString as Acc_Prob_of_Real,Start_DateValue.ValueString as Start_Date,End_DateValue.ValueString as End_Date,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  Acc_Prob_of_RealField  ON dbo.MetaObject.Class = Acc_Prob_of_RealField.Class left outer JOIN  
dbo.ObjectFieldValue Acc_Prob_of_RealValue ON dbo.MetaObject.pkid=Acc_Prob_of_RealValue.ObjectID and dbo.MetaObject.Machine=Acc_Prob_of_RealValue.MachineID and Acc_Prob_of_RealField.pkid = Acc_Prob_of_RealValue.fieldid  
 INNER JOIN  dbo.Field  Start_DateField  ON dbo.MetaObject.Class = Start_DateField.Class left outer JOIN  
dbo.ObjectFieldValue Start_DateValue ON dbo.MetaObject.pkid=Start_DateValue.ObjectID and dbo.MetaObject.Machine=Start_DateValue.MachineID and Start_DateField.pkid = Start_DateValue.fieldid  
 INNER JOIN  dbo.Field  End_DateField  ON dbo.MetaObject.Class = End_DateField.Class left outer JOIN  
dbo.ObjectFieldValue End_DateValue ON dbo.MetaObject.pkid=End_DateValue.ObjectID and dbo.MetaObject.Machine=End_DateValue.MachineID and End_DateField.pkid = End_DateValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Scenario')  AND NameField.Name ='Name'  AND Acc_Prob_of_RealField.Name ='Acc_Prob_of_Real'  AND Start_DateField.Name ='Start_Date'  AND End_DateField.Name ='End_Date'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Scenario_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Scenario_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,Acc_Prob_of_RealValue.ValueString as Acc_Prob_of_Real,Start_DateValue.ValueString as Start_Date,End_DateValue.ValueString as End_Date,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  Acc_Prob_of_RealField  ON dbo.MetaObject.Class = Acc_Prob_of_RealField.Class left outer JOIN  
dbo.ObjectFieldValue Acc_Prob_of_RealValue ON dbo.MetaObject.pkid=Acc_Prob_of_RealValue.ObjectID and dbo.MetaObject.Machine=Acc_Prob_of_RealValue.MachineID and Acc_Prob_of_RealField.pkid = Acc_Prob_of_RealValue.fieldid  
 INNER JOIN  dbo.Field  Start_DateField  ON dbo.MetaObject.Class = Start_DateField.Class left outer JOIN  
dbo.ObjectFieldValue Start_DateValue ON dbo.MetaObject.pkid=Start_DateValue.ObjectID and dbo.MetaObject.Machine=Start_DateValue.MachineID and Start_DateField.pkid = Start_DateValue.fieldid  
 INNER JOIN  dbo.Field  End_DateField  ON dbo.MetaObject.Class = End_DateField.Class left outer JOIN  
dbo.ObjectFieldValue End_DateValue ON dbo.MetaObject.pkid=End_DateValue.ObjectID and dbo.MetaObject.Machine=End_DateValue.MachineID and End_DateField.pkid = End_DateValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Scenario')  AND NameField.Name ='Name'  AND Acc_Prob_of_RealField.Name ='Acc_Prob_of_Real'  AND Start_DateField.Name ='Start_Date'  AND End_DateField.Name ='End_Date'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_SelectorAttribute_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_SelectorAttribute_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,ValueValue.ValueString as Value,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  ValueField  ON dbo.MetaObject.Class = ValueField.Class left outer JOIN  
dbo.ObjectFieldValue ValueValue ON dbo.MetaObject.pkid=ValueValue.ObjectID and dbo.MetaObject.Machine=ValueValue.MachineID and ValueField.pkid = ValueValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'SelectorAttribute')  AND ValueField.Name ='Value'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_SelectorAttribute_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_SelectorAttribute_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,ValueValue.ValueString as Value,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  ValueField  ON dbo.MetaObject.Class = ValueField.Class left outer JOIN  
dbo.ObjectFieldValue ValueValue ON dbo.MetaObject.pkid=ValueValue.ObjectID and dbo.MetaObject.Machine=ValueValue.MachineID and ValueField.pkid = ValueValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'SelectorAttribute')  AND ValueField.Name ='Value'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Skill_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Skill_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Skill')  AND NameField.Name ='Name'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Skill_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Skill_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Skill')  AND NameField.Name ='Name'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Software_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Software_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,TypeValue.ValueString as Type,NameValue.ValueString as Name,DescriptionValue.ValueString as Description,SeverityRatingValue.ValueString as SeverityRating,ConfigurationValue.ValueLongText as Configuration,CopyrightValue.ValueString as Copyright,PublisherValue.ValueString as Publisher,InternalNameValue.ValueString as InternalName,LanguageValue.ValueString as Language,DateCreatedValue.ValueString as DateCreated,isDNSValue.ValueString as isDNS,isDHCPValue.ValueString as isDHCP,isLicensedValue.ValueString as isLicensed,LicenseNumberValue.ValueString as LicenseNumber,DatePurchasedValue.ValueString as DatePurchased,VersionValue.ValueString as Version,IDValue.ValueString as ID,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,UserInterfaceValue.ValueString as UserInterface,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  TypeField  ON dbo.MetaObject.Class = TypeField.Class left outer JOIN  
dbo.ObjectFieldValue TypeValue ON dbo.MetaObject.pkid=TypeValue.ObjectID and dbo.MetaObject.Machine=TypeValue.MachineID and TypeField.pkid = TypeValue.fieldid  
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  DescriptionField  ON dbo.MetaObject.Class = DescriptionField.Class left outer JOIN  
dbo.ObjectFieldValue DescriptionValue ON dbo.MetaObject.pkid=DescriptionValue.ObjectID and dbo.MetaObject.Machine=DescriptionValue.MachineID and DescriptionField.pkid = DescriptionValue.fieldid  
 INNER JOIN  dbo.Field  SeverityRatingField  ON dbo.MetaObject.Class = SeverityRatingField.Class left outer JOIN  
dbo.ObjectFieldValue SeverityRatingValue ON dbo.MetaObject.pkid=SeverityRatingValue.ObjectID and dbo.MetaObject.Machine=SeverityRatingValue.MachineID and SeverityRatingField.pkid = SeverityRatingValue.fieldid  
 INNER JOIN  dbo.Field  ConfigurationField  ON dbo.MetaObject.Class = ConfigurationField.Class left outer JOIN  
dbo.ObjectFieldValue ConfigurationValue ON dbo.MetaObject.pkid=ConfigurationValue.ObjectID and dbo.MetaObject.Machine=ConfigurationValue.MachineID and ConfigurationField.pkid = ConfigurationValue.fieldid  
 INNER JOIN  dbo.Field  CopyrightField  ON dbo.MetaObject.Class = CopyrightField.Class left outer JOIN  
dbo.ObjectFieldValue CopyrightValue ON dbo.MetaObject.pkid=CopyrightValue.ObjectID and dbo.MetaObject.Machine=CopyrightValue.MachineID and CopyrightField.pkid = CopyrightValue.fieldid  
 INNER JOIN  dbo.Field  PublisherField  ON dbo.MetaObject.Class = PublisherField.Class left outer JOIN  
dbo.ObjectFieldValue PublisherValue ON dbo.MetaObject.pkid=PublisherValue.ObjectID and dbo.MetaObject.Machine=PublisherValue.MachineID and PublisherField.pkid = PublisherValue.fieldid  
 INNER JOIN  dbo.Field  InternalNameField  ON dbo.MetaObject.Class = InternalNameField.Class left outer JOIN  
dbo.ObjectFieldValue InternalNameValue ON dbo.MetaObject.pkid=InternalNameValue.ObjectID and dbo.MetaObject.Machine=InternalNameValue.MachineID and InternalNameField.pkid = InternalNameValue.fieldid  
 INNER JOIN  dbo.Field  LanguageField  ON dbo.MetaObject.Class = LanguageField.Class left outer JOIN  
dbo.ObjectFieldValue LanguageValue ON dbo.MetaObject.pkid=LanguageValue.ObjectID and dbo.MetaObject.Machine=LanguageValue.MachineID and LanguageField.pkid = LanguageValue.fieldid  
 INNER JOIN  dbo.Field  DateCreatedField  ON dbo.MetaObject.Class = DateCreatedField.Class left outer JOIN  
dbo.ObjectFieldValue DateCreatedValue ON dbo.MetaObject.pkid=DateCreatedValue.ObjectID and dbo.MetaObject.Machine=DateCreatedValue.MachineID and DateCreatedField.pkid = DateCreatedValue.fieldid  
 INNER JOIN  dbo.Field  isDNSField  ON dbo.MetaObject.Class = isDNSField.Class left outer JOIN  
dbo.ObjectFieldValue isDNSValue ON dbo.MetaObject.pkid=isDNSValue.ObjectID and dbo.MetaObject.Machine=isDNSValue.MachineID and isDNSField.pkid = isDNSValue.fieldid  
 INNER JOIN  dbo.Field  isDHCPField  ON dbo.MetaObject.Class = isDHCPField.Class left outer JOIN  
dbo.ObjectFieldValue isDHCPValue ON dbo.MetaObject.pkid=isDHCPValue.ObjectID and dbo.MetaObject.Machine=isDHCPValue.MachineID and isDHCPField.pkid = isDHCPValue.fieldid  
 INNER JOIN  dbo.Field  isLicensedField  ON dbo.MetaObject.Class = isLicensedField.Class left outer JOIN  
dbo.ObjectFieldValue isLicensedValue ON dbo.MetaObject.pkid=isLicensedValue.ObjectID and dbo.MetaObject.Machine=isLicensedValue.MachineID and isLicensedField.pkid = isLicensedValue.fieldid  
 INNER JOIN  dbo.Field  LicenseNumberField  ON dbo.MetaObject.Class = LicenseNumberField.Class left outer JOIN  
dbo.ObjectFieldValue LicenseNumberValue ON dbo.MetaObject.pkid=LicenseNumberValue.ObjectID and dbo.MetaObject.Machine=LicenseNumberValue.MachineID and LicenseNumberField.pkid = LicenseNumberValue.fieldid  
 INNER JOIN  dbo.Field  DatePurchasedField  ON dbo.MetaObject.Class = DatePurchasedField.Class left outer JOIN  
dbo.ObjectFieldValue DatePurchasedValue ON dbo.MetaObject.pkid=DatePurchasedValue.ObjectID and dbo.MetaObject.Machine=DatePurchasedValue.MachineID and DatePurchasedField.pkid = DatePurchasedValue.fieldid  
 INNER JOIN  dbo.Field  VersionField  ON dbo.MetaObject.Class = VersionField.Class left outer JOIN  
dbo.ObjectFieldValue VersionValue ON dbo.MetaObject.pkid=VersionValue.ObjectID and dbo.MetaObject.Machine=VersionValue.MachineID and VersionField.pkid = VersionValue.fieldid  
 INNER JOIN  dbo.Field  IDField  ON dbo.MetaObject.Class = IDField.Class left outer JOIN  
dbo.ObjectFieldValue IDValue ON dbo.MetaObject.pkid=IDValue.ObjectID and dbo.MetaObject.Machine=IDValue.MachineID and IDField.pkid = IDValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  UserInterfaceField  ON dbo.MetaObject.Class = UserInterfaceField.Class left outer JOIN  
dbo.ObjectFieldValue UserInterfaceValue ON dbo.MetaObject.pkid=UserInterfaceValue.ObjectID and dbo.MetaObject.Machine=UserInterfaceValue.MachineID and UserInterfaceField.pkid = UserInterfaceValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Software')  AND TypeField.Name ='Type'  AND NameField.Name ='Name'  AND DescriptionField.Name ='Description'  AND SeverityRatingField.Name ='SeverityRating'  AND ConfigurationField.Name ='Configuration'  AND CopyrightField.Name ='Copyright'  AND PublisherField.Name ='Publisher'  AND InternalNameField.Name ='InternalName'  AND LanguageField.Name ='Language'  AND DateCreatedField.Name ='DateCreated'  AND isDNSField.Name ='isDNS'  AND isDHCPField.Name ='isDHCP'  AND isLicensedField.Name ='isLicensed'  AND LicenseNumberField.Name ='LicenseNumber'  AND DatePurchasedField.Name ='DatePurchased'  AND VersionField.Name ='Version'  AND IDField.Name ='ID'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND UserInterfaceField.Name ='UserInterface'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_Software_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_Software_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,TypeValue.ValueString as Type,NameValue.ValueString as Name,DescriptionValue.ValueString as Description,SeverityRatingValue.ValueString as SeverityRating,ConfigurationValue.ValueRTF as Configuration,CopyrightValue.ValueString as Copyright,PublisherValue.ValueString as Publisher,InternalNameValue.ValueString as InternalName,LanguageValue.ValueString as Language,DateCreatedValue.ValueString as DateCreated,isDNSValue.ValueString as isDNS,isDHCPValue.ValueString as isDHCP,isLicensedValue.ValueString as isLicensed,LicenseNumberValue.ValueString as LicenseNumber,DatePurchasedValue.ValueString as DatePurchased,VersionValue.ValueString as Version,IDValue.ValueString as ID,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,UserInterfaceValue.ValueString as UserInterface,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  TypeField  ON dbo.MetaObject.Class = TypeField.Class left outer JOIN  
dbo.ObjectFieldValue TypeValue ON dbo.MetaObject.pkid=TypeValue.ObjectID and dbo.MetaObject.Machine=TypeValue.MachineID and TypeField.pkid = TypeValue.fieldid  
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  DescriptionField  ON dbo.MetaObject.Class = DescriptionField.Class left outer JOIN  
dbo.ObjectFieldValue DescriptionValue ON dbo.MetaObject.pkid=DescriptionValue.ObjectID and dbo.MetaObject.Machine=DescriptionValue.MachineID and DescriptionField.pkid = DescriptionValue.fieldid  
 INNER JOIN  dbo.Field  SeverityRatingField  ON dbo.MetaObject.Class = SeverityRatingField.Class left outer JOIN  
dbo.ObjectFieldValue SeverityRatingValue ON dbo.MetaObject.pkid=SeverityRatingValue.ObjectID and dbo.MetaObject.Machine=SeverityRatingValue.MachineID and SeverityRatingField.pkid = SeverityRatingValue.fieldid  
 INNER JOIN  dbo.Field  ConfigurationField  ON dbo.MetaObject.Class = ConfigurationField.Class left outer JOIN  
dbo.ObjectFieldValue ConfigurationValue ON dbo.MetaObject.pkid=ConfigurationValue.ObjectID and dbo.MetaObject.Machine=ConfigurationValue.MachineID and ConfigurationField.pkid = ConfigurationValue.fieldid  
 INNER JOIN  dbo.Field  CopyrightField  ON dbo.MetaObject.Class = CopyrightField.Class left outer JOIN  
dbo.ObjectFieldValue CopyrightValue ON dbo.MetaObject.pkid=CopyrightValue.ObjectID and dbo.MetaObject.Machine=CopyrightValue.MachineID and CopyrightField.pkid = CopyrightValue.fieldid  
 INNER JOIN  dbo.Field  PublisherField  ON dbo.MetaObject.Class = PublisherField.Class left outer JOIN  
dbo.ObjectFieldValue PublisherValue ON dbo.MetaObject.pkid=PublisherValue.ObjectID and dbo.MetaObject.Machine=PublisherValue.MachineID and PublisherField.pkid = PublisherValue.fieldid  
 INNER JOIN  dbo.Field  InternalNameField  ON dbo.MetaObject.Class = InternalNameField.Class left outer JOIN  
dbo.ObjectFieldValue InternalNameValue ON dbo.MetaObject.pkid=InternalNameValue.ObjectID and dbo.MetaObject.Machine=InternalNameValue.MachineID and InternalNameField.pkid = InternalNameValue.fieldid  
 INNER JOIN  dbo.Field  LanguageField  ON dbo.MetaObject.Class = LanguageField.Class left outer JOIN  
dbo.ObjectFieldValue LanguageValue ON dbo.MetaObject.pkid=LanguageValue.ObjectID and dbo.MetaObject.Machine=LanguageValue.MachineID and LanguageField.pkid = LanguageValue.fieldid  
 INNER JOIN  dbo.Field  DateCreatedField  ON dbo.MetaObject.Class = DateCreatedField.Class left outer JOIN  
dbo.ObjectFieldValue DateCreatedValue ON dbo.MetaObject.pkid=DateCreatedValue.ObjectID and dbo.MetaObject.Machine=DateCreatedValue.MachineID and DateCreatedField.pkid = DateCreatedValue.fieldid  
 INNER JOIN  dbo.Field  isDNSField  ON dbo.MetaObject.Class = isDNSField.Class left outer JOIN  
dbo.ObjectFieldValue isDNSValue ON dbo.MetaObject.pkid=isDNSValue.ObjectID and dbo.MetaObject.Machine=isDNSValue.MachineID and isDNSField.pkid = isDNSValue.fieldid  
 INNER JOIN  dbo.Field  isDHCPField  ON dbo.MetaObject.Class = isDHCPField.Class left outer JOIN  
dbo.ObjectFieldValue isDHCPValue ON dbo.MetaObject.pkid=isDHCPValue.ObjectID and dbo.MetaObject.Machine=isDHCPValue.MachineID and isDHCPField.pkid = isDHCPValue.fieldid  
 INNER JOIN  dbo.Field  isLicensedField  ON dbo.MetaObject.Class = isLicensedField.Class left outer JOIN  
dbo.ObjectFieldValue isLicensedValue ON dbo.MetaObject.pkid=isLicensedValue.ObjectID and dbo.MetaObject.Machine=isLicensedValue.MachineID and isLicensedField.pkid = isLicensedValue.fieldid  
 INNER JOIN  dbo.Field  LicenseNumberField  ON dbo.MetaObject.Class = LicenseNumberField.Class left outer JOIN  
dbo.ObjectFieldValue LicenseNumberValue ON dbo.MetaObject.pkid=LicenseNumberValue.ObjectID and dbo.MetaObject.Machine=LicenseNumberValue.MachineID and LicenseNumberField.pkid = LicenseNumberValue.fieldid  
 INNER JOIN  dbo.Field  DatePurchasedField  ON dbo.MetaObject.Class = DatePurchasedField.Class left outer JOIN  
dbo.ObjectFieldValue DatePurchasedValue ON dbo.MetaObject.pkid=DatePurchasedValue.ObjectID and dbo.MetaObject.Machine=DatePurchasedValue.MachineID and DatePurchasedField.pkid = DatePurchasedValue.fieldid  
 INNER JOIN  dbo.Field  VersionField  ON dbo.MetaObject.Class = VersionField.Class left outer JOIN  
dbo.ObjectFieldValue VersionValue ON dbo.MetaObject.pkid=VersionValue.ObjectID and dbo.MetaObject.Machine=VersionValue.MachineID and VersionField.pkid = VersionValue.fieldid  
 INNER JOIN  dbo.Field  IDField  ON dbo.MetaObject.Class = IDField.Class left outer JOIN  
dbo.ObjectFieldValue IDValue ON dbo.MetaObject.pkid=IDValue.ObjectID and dbo.MetaObject.Machine=IDValue.MachineID and IDField.pkid = IDValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  UserInterfaceField  ON dbo.MetaObject.Class = UserInterfaceField.Class left outer JOIN  
dbo.ObjectFieldValue UserInterfaceValue ON dbo.MetaObject.pkid=UserInterfaceValue.ObjectID and dbo.MetaObject.Machine=UserInterfaceValue.MachineID and UserInterfaceField.pkid = UserInterfaceValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'Software')  AND TypeField.Name ='Type'  AND NameField.Name ='Name'  AND DescriptionField.Name ='Description'  AND SeverityRatingField.Name ='SeverityRating'  AND ConfigurationField.Name ='Configuration'  AND CopyrightField.Name ='Copyright'  AND PublisherField.Name ='Publisher'  AND InternalNameField.Name ='InternalName'  AND LanguageField.Name ='Language'  AND DateCreatedField.Name ='DateCreated'  AND isDNSField.Name ='isDNS'  AND isDHCPField.Name ='isDHCP'  AND isLicensedField.Name ='isLicensed'  AND LicenseNumberField.Name ='LicenseNumber'  AND DatePurchasedField.Name ='DatePurchased'  AND VersionField.Name ='Version'  AND IDField.Name ='ID'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND UserInterfaceField.Name ='UserInterface'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_StorageComponent_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_StorageComponent_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,TypeValue.ValueString as Type,NameValue.ValueString as Name,DescriptionValue.ValueString as Description,SeverityIndicatorValue.ValueString as SeverityIndicator,ConfigurationValue.ValueLongText as Configuration,NetworkAddress1Value.ValueString as NetworkAddress1,NetworkAddress2Value.ValueString as NetworkAddress2,NetworkAddress3Value.ValueString as NetworkAddress3,NetworkAddress4Value.ValueString as NetworkAddress4,NetworkAddress5Value.ValueString as NetworkAddress5,MakeValue.ValueString as Make,ModelValue.ValueString as Model,SerialNumberValue.ValueString as SerialNumber,AssetNumberValue.ValueString as AssetNumber,CapacityValue.ValueString as Capacity,Number_of_DisksValue.ValueString as Number_of_Disks,DatePurchasedValue.ValueString as DatePurchased,UnderWarrantyValue.ValueString as UnderWarranty,FileSystemValue.ValueString as FileSystem,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  TypeField  ON dbo.MetaObject.Class = TypeField.Class left outer JOIN  
dbo.ObjectFieldValue TypeValue ON dbo.MetaObject.pkid=TypeValue.ObjectID and dbo.MetaObject.Machine=TypeValue.MachineID and TypeField.pkid = TypeValue.fieldid  
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  DescriptionField  ON dbo.MetaObject.Class = DescriptionField.Class left outer JOIN  
dbo.ObjectFieldValue DescriptionValue ON dbo.MetaObject.pkid=DescriptionValue.ObjectID and dbo.MetaObject.Machine=DescriptionValue.MachineID and DescriptionField.pkid = DescriptionValue.fieldid  
 INNER JOIN  dbo.Field  SeverityIndicatorField  ON dbo.MetaObject.Class = SeverityIndicatorField.Class left outer JOIN  
dbo.ObjectFieldValue SeverityIndicatorValue ON dbo.MetaObject.pkid=SeverityIndicatorValue.ObjectID and dbo.MetaObject.Machine=SeverityIndicatorValue.MachineID and SeverityIndicatorField.pkid = SeverityIndicatorValue.fieldid  
 INNER JOIN  dbo.Field  ConfigurationField  ON dbo.MetaObject.Class = ConfigurationField.Class left outer JOIN  
dbo.ObjectFieldValue ConfigurationValue ON dbo.MetaObject.pkid=ConfigurationValue.ObjectID and dbo.MetaObject.Machine=ConfigurationValue.MachineID and ConfigurationField.pkid = ConfigurationValue.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress1Field  ON dbo.MetaObject.Class = NetworkAddress1Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress1Value ON dbo.MetaObject.pkid=NetworkAddress1Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress1Value.MachineID and NetworkAddress1Field.pkid = NetworkAddress1Value.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress2Field  ON dbo.MetaObject.Class = NetworkAddress2Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress2Value ON dbo.MetaObject.pkid=NetworkAddress2Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress2Value.MachineID and NetworkAddress2Field.pkid = NetworkAddress2Value.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress3Field  ON dbo.MetaObject.Class = NetworkAddress3Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress3Value ON dbo.MetaObject.pkid=NetworkAddress3Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress3Value.MachineID and NetworkAddress3Field.pkid = NetworkAddress3Value.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress4Field  ON dbo.MetaObject.Class = NetworkAddress4Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress4Value ON dbo.MetaObject.pkid=NetworkAddress4Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress4Value.MachineID and NetworkAddress4Field.pkid = NetworkAddress4Value.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress5Field  ON dbo.MetaObject.Class = NetworkAddress5Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress5Value ON dbo.MetaObject.pkid=NetworkAddress5Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress5Value.MachineID and NetworkAddress5Field.pkid = NetworkAddress5Value.fieldid  
 INNER JOIN  dbo.Field  MakeField  ON dbo.MetaObject.Class = MakeField.Class left outer JOIN  
dbo.ObjectFieldValue MakeValue ON dbo.MetaObject.pkid=MakeValue.ObjectID and dbo.MetaObject.Machine=MakeValue.MachineID and MakeField.pkid = MakeValue.fieldid  
 INNER JOIN  dbo.Field  ModelField  ON dbo.MetaObject.Class = ModelField.Class left outer JOIN  
dbo.ObjectFieldValue ModelValue ON dbo.MetaObject.pkid=ModelValue.ObjectID and dbo.MetaObject.Machine=ModelValue.MachineID and ModelField.pkid = ModelValue.fieldid  
 INNER JOIN  dbo.Field  SerialNumberField  ON dbo.MetaObject.Class = SerialNumberField.Class left outer JOIN  
dbo.ObjectFieldValue SerialNumberValue ON dbo.MetaObject.pkid=SerialNumberValue.ObjectID and dbo.MetaObject.Machine=SerialNumberValue.MachineID and SerialNumberField.pkid = SerialNumberValue.fieldid  
 INNER JOIN  dbo.Field  AssetNumberField  ON dbo.MetaObject.Class = AssetNumberField.Class left outer JOIN  
dbo.ObjectFieldValue AssetNumberValue ON dbo.MetaObject.pkid=AssetNumberValue.ObjectID and dbo.MetaObject.Machine=AssetNumberValue.MachineID and AssetNumberField.pkid = AssetNumberValue.fieldid  
 INNER JOIN  dbo.Field  CapacityField  ON dbo.MetaObject.Class = CapacityField.Class left outer JOIN  
dbo.ObjectFieldValue CapacityValue ON dbo.MetaObject.pkid=CapacityValue.ObjectID and dbo.MetaObject.Machine=CapacityValue.MachineID and CapacityField.pkid = CapacityValue.fieldid  
 INNER JOIN  dbo.Field  Number_of_DisksField  ON dbo.MetaObject.Class = Number_of_DisksField.Class left outer JOIN  
dbo.ObjectFieldValue Number_of_DisksValue ON dbo.MetaObject.pkid=Number_of_DisksValue.ObjectID and dbo.MetaObject.Machine=Number_of_DisksValue.MachineID and Number_of_DisksField.pkid = Number_of_DisksValue.fieldid  
 INNER JOIN  dbo.Field  DatePurchasedField  ON dbo.MetaObject.Class = DatePurchasedField.Class left outer JOIN  
dbo.ObjectFieldValue DatePurchasedValue ON dbo.MetaObject.pkid=DatePurchasedValue.ObjectID and dbo.MetaObject.Machine=DatePurchasedValue.MachineID and DatePurchasedField.pkid = DatePurchasedValue.fieldid  
 INNER JOIN  dbo.Field  UnderWarrantyField  ON dbo.MetaObject.Class = UnderWarrantyField.Class left outer JOIN  
dbo.ObjectFieldValue UnderWarrantyValue ON dbo.MetaObject.pkid=UnderWarrantyValue.ObjectID and dbo.MetaObject.Machine=UnderWarrantyValue.MachineID and UnderWarrantyField.pkid = UnderWarrantyValue.fieldid  
 INNER JOIN  dbo.Field  FileSystemField  ON dbo.MetaObject.Class = FileSystemField.Class left outer JOIN  
dbo.ObjectFieldValue FileSystemValue ON dbo.MetaObject.pkid=FileSystemValue.ObjectID and dbo.MetaObject.Machine=FileSystemValue.MachineID and FileSystemField.pkid = FileSystemValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'StorageComponent')  AND TypeField.Name ='Type'  AND NameField.Name ='Name'  AND DescriptionField.Name ='Description'  AND SeverityIndicatorField.Name ='SeverityIndicator'  AND ConfigurationField.Name ='Configuration'  AND NetworkAddress1Field.Name ='NetworkAddress1'  AND NetworkAddress2Field.Name ='NetworkAddress2'  AND NetworkAddress3Field.Name ='NetworkAddress3'  AND NetworkAddress4Field.Name ='NetworkAddress4'  AND NetworkAddress5Field.Name ='NetworkAddress5'  AND MakeField.Name ='Make'  AND ModelField.Name ='Model'  AND SerialNumberField.Name ='SerialNumber'  AND AssetNumberField.Name ='AssetNumber'  AND CapacityField.Name ='Capacity'  AND Number_of_DisksField.Name ='Number_of_Disks'  AND DatePurchasedField.Name ='DatePurchased'  AND UnderWarrantyField.Name ='UnderWarranty'  AND FileSystemField.Name ='FileSystem'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_StorageComponent_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_StorageComponent_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,TypeValue.ValueString as Type,NameValue.ValueString as Name,DescriptionValue.ValueString as Description,SeverityIndicatorValue.ValueString as SeverityIndicator,ConfigurationValue.ValueRTF as Configuration,NetworkAddress1Value.ValueString as NetworkAddress1,NetworkAddress2Value.ValueString as NetworkAddress2,NetworkAddress3Value.ValueString as NetworkAddress3,NetworkAddress4Value.ValueString as NetworkAddress4,NetworkAddress5Value.ValueString as NetworkAddress5,MakeValue.ValueString as Make,ModelValue.ValueString as Model,SerialNumberValue.ValueString as SerialNumber,AssetNumberValue.ValueString as AssetNumber,CapacityValue.ValueString as Capacity,Number_of_DisksValue.ValueString as Number_of_Disks,DatePurchasedValue.ValueString as DatePurchased,UnderWarrantyValue.ValueString as UnderWarranty,FileSystemValue.ValueString as FileSystem,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  TypeField  ON dbo.MetaObject.Class = TypeField.Class left outer JOIN  
dbo.ObjectFieldValue TypeValue ON dbo.MetaObject.pkid=TypeValue.ObjectID and dbo.MetaObject.Machine=TypeValue.MachineID and TypeField.pkid = TypeValue.fieldid  
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  DescriptionField  ON dbo.MetaObject.Class = DescriptionField.Class left outer JOIN  
dbo.ObjectFieldValue DescriptionValue ON dbo.MetaObject.pkid=DescriptionValue.ObjectID and dbo.MetaObject.Machine=DescriptionValue.MachineID and DescriptionField.pkid = DescriptionValue.fieldid  
 INNER JOIN  dbo.Field  SeverityIndicatorField  ON dbo.MetaObject.Class = SeverityIndicatorField.Class left outer JOIN  
dbo.ObjectFieldValue SeverityIndicatorValue ON dbo.MetaObject.pkid=SeverityIndicatorValue.ObjectID and dbo.MetaObject.Machine=SeverityIndicatorValue.MachineID and SeverityIndicatorField.pkid = SeverityIndicatorValue.fieldid  
 INNER JOIN  dbo.Field  ConfigurationField  ON dbo.MetaObject.Class = ConfigurationField.Class left outer JOIN  
dbo.ObjectFieldValue ConfigurationValue ON dbo.MetaObject.pkid=ConfigurationValue.ObjectID and dbo.MetaObject.Machine=ConfigurationValue.MachineID and ConfigurationField.pkid = ConfigurationValue.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress1Field  ON dbo.MetaObject.Class = NetworkAddress1Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress1Value ON dbo.MetaObject.pkid=NetworkAddress1Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress1Value.MachineID and NetworkAddress1Field.pkid = NetworkAddress1Value.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress2Field  ON dbo.MetaObject.Class = NetworkAddress2Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress2Value ON dbo.MetaObject.pkid=NetworkAddress2Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress2Value.MachineID and NetworkAddress2Field.pkid = NetworkAddress2Value.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress3Field  ON dbo.MetaObject.Class = NetworkAddress3Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress3Value ON dbo.MetaObject.pkid=NetworkAddress3Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress3Value.MachineID and NetworkAddress3Field.pkid = NetworkAddress3Value.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress4Field  ON dbo.MetaObject.Class = NetworkAddress4Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress4Value ON dbo.MetaObject.pkid=NetworkAddress4Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress4Value.MachineID and NetworkAddress4Field.pkid = NetworkAddress4Value.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress5Field  ON dbo.MetaObject.Class = NetworkAddress5Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress5Value ON dbo.MetaObject.pkid=NetworkAddress5Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress5Value.MachineID and NetworkAddress5Field.pkid = NetworkAddress5Value.fieldid  
 INNER JOIN  dbo.Field  MakeField  ON dbo.MetaObject.Class = MakeField.Class left outer JOIN  
dbo.ObjectFieldValue MakeValue ON dbo.MetaObject.pkid=MakeValue.ObjectID and dbo.MetaObject.Machine=MakeValue.MachineID and MakeField.pkid = MakeValue.fieldid  
 INNER JOIN  dbo.Field  ModelField  ON dbo.MetaObject.Class = ModelField.Class left outer JOIN  
dbo.ObjectFieldValue ModelValue ON dbo.MetaObject.pkid=ModelValue.ObjectID and dbo.MetaObject.Machine=ModelValue.MachineID and ModelField.pkid = ModelValue.fieldid  
 INNER JOIN  dbo.Field  SerialNumberField  ON dbo.MetaObject.Class = SerialNumberField.Class left outer JOIN  
dbo.ObjectFieldValue SerialNumberValue ON dbo.MetaObject.pkid=SerialNumberValue.ObjectID and dbo.MetaObject.Machine=SerialNumberValue.MachineID and SerialNumberField.pkid = SerialNumberValue.fieldid  
 INNER JOIN  dbo.Field  AssetNumberField  ON dbo.MetaObject.Class = AssetNumberField.Class left outer JOIN  
dbo.ObjectFieldValue AssetNumberValue ON dbo.MetaObject.pkid=AssetNumberValue.ObjectID and dbo.MetaObject.Machine=AssetNumberValue.MachineID and AssetNumberField.pkid = AssetNumberValue.fieldid  
 INNER JOIN  dbo.Field  CapacityField  ON dbo.MetaObject.Class = CapacityField.Class left outer JOIN  
dbo.ObjectFieldValue CapacityValue ON dbo.MetaObject.pkid=CapacityValue.ObjectID and dbo.MetaObject.Machine=CapacityValue.MachineID and CapacityField.pkid = CapacityValue.fieldid  
 INNER JOIN  dbo.Field  Number_of_DisksField  ON dbo.MetaObject.Class = Number_of_DisksField.Class left outer JOIN  
dbo.ObjectFieldValue Number_of_DisksValue ON dbo.MetaObject.pkid=Number_of_DisksValue.ObjectID and dbo.MetaObject.Machine=Number_of_DisksValue.MachineID and Number_of_DisksField.pkid = Number_of_DisksValue.fieldid  
 INNER JOIN  dbo.Field  DatePurchasedField  ON dbo.MetaObject.Class = DatePurchasedField.Class left outer JOIN  
dbo.ObjectFieldValue DatePurchasedValue ON dbo.MetaObject.pkid=DatePurchasedValue.ObjectID and dbo.MetaObject.Machine=DatePurchasedValue.MachineID and DatePurchasedField.pkid = DatePurchasedValue.fieldid  
 INNER JOIN  dbo.Field  UnderWarrantyField  ON dbo.MetaObject.Class = UnderWarrantyField.Class left outer JOIN  
dbo.ObjectFieldValue UnderWarrantyValue ON dbo.MetaObject.pkid=UnderWarrantyValue.ObjectID and dbo.MetaObject.Machine=UnderWarrantyValue.MachineID and UnderWarrantyField.pkid = UnderWarrantyValue.fieldid  
 INNER JOIN  dbo.Field  FileSystemField  ON dbo.MetaObject.Class = FileSystemField.Class left outer JOIN  
dbo.ObjectFieldValue FileSystemValue ON dbo.MetaObject.pkid=FileSystemValue.ObjectID and dbo.MetaObject.Machine=FileSystemValue.MachineID and FileSystemField.pkid = FileSystemValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'StorageComponent')  AND TypeField.Name ='Type'  AND NameField.Name ='Name'  AND DescriptionField.Name ='Description'  AND SeverityIndicatorField.Name ='SeverityIndicator'  AND ConfigurationField.Name ='Configuration'  AND NetworkAddress1Field.Name ='NetworkAddress1'  AND NetworkAddress2Field.Name ='NetworkAddress2'  AND NetworkAddress3Field.Name ='NetworkAddress3'  AND NetworkAddress4Field.Name ='NetworkAddress4'  AND NetworkAddress5Field.Name ='NetworkAddress5'  AND MakeField.Name ='Make'  AND ModelField.Name ='Model'  AND SerialNumberField.Name ='SerialNumber'  AND AssetNumberField.Name ='AssetNumber'  AND CapacityField.Name ='Capacity'  AND Number_of_DisksField.Name ='Number_of_Disks'  AND DatePurchasedField.Name ='DatePurchased'  AND UnderWarrantyField.Name ='UnderWarranty'  AND FileSystemField.Name ='FileSystem'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_StrategicTheme_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_StrategicTheme_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'StrategicTheme')  AND NameField.Name ='Name'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_StrategicTheme_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_StrategicTheme_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'StrategicTheme')  AND NameField.Name ='Name'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_SystemComponent_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_SystemComponent_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,TypeValue.ValueString as Type,NameValue.ValueString as Name,DescriptionValue.ValueString as Description,SeverityRatingValue.ValueString as SeverityRating,ConfigurationValue.ValueLongText as Configuration,MACAddressValue.ValueString as MACAddress,StaticIPValue.ValueString as StaticIP,NetworkAddress3Value.ValueString as NetworkAddress3,NetworkAddress4Value.ValueString as NetworkAddress4,NetworkAddress5Value.ValueString as NetworkAddress5,MakeValue.ValueString as Make,ModelValue.ValueString as Model,SerialNumberValue.ValueString as SerialNumber,AssetNumberValue.ValueString as AssetNumber,isDNSValue.ValueString as isDNS,isDHCPValue.ValueString as isDHCP,CapacityValue.ValueString as Capacity,Mem_TotalValue.ValueString as Mem_Total,CPU_TypeValue.ValueString as CPU_Type,CPU_SpeedValue.ValueString as CPU_Speed,MonitorValue.ValueString as Monitor,Video_CardValue.ValueString as Video_Card,Number_Of_DisksValue.ValueString as Number_Of_Disks,DatePurchasedValue.ValueString as DatePurchased,UnderWarrantyValue.ValueString as UnderWarranty,DomainValue.ValueString as Domain,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,ServerTypeValue.ValueString as ServerType,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  TypeField  ON dbo.MetaObject.Class = TypeField.Class left outer JOIN  
dbo.ObjectFieldValue TypeValue ON dbo.MetaObject.pkid=TypeValue.ObjectID and dbo.MetaObject.Machine=TypeValue.MachineID and TypeField.pkid = TypeValue.fieldid  
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  DescriptionField  ON dbo.MetaObject.Class = DescriptionField.Class left outer JOIN  
dbo.ObjectFieldValue DescriptionValue ON dbo.MetaObject.pkid=DescriptionValue.ObjectID and dbo.MetaObject.Machine=DescriptionValue.MachineID and DescriptionField.pkid = DescriptionValue.fieldid  
 INNER JOIN  dbo.Field  SeverityRatingField  ON dbo.MetaObject.Class = SeverityRatingField.Class left outer JOIN  
dbo.ObjectFieldValue SeverityRatingValue ON dbo.MetaObject.pkid=SeverityRatingValue.ObjectID and dbo.MetaObject.Machine=SeverityRatingValue.MachineID and SeverityRatingField.pkid = SeverityRatingValue.fieldid  
 INNER JOIN  dbo.Field  ConfigurationField  ON dbo.MetaObject.Class = ConfigurationField.Class left outer JOIN  
dbo.ObjectFieldValue ConfigurationValue ON dbo.MetaObject.pkid=ConfigurationValue.ObjectID and dbo.MetaObject.Machine=ConfigurationValue.MachineID and ConfigurationField.pkid = ConfigurationValue.fieldid  
 INNER JOIN  dbo.Field  MACAddressField  ON dbo.MetaObject.Class = MACAddressField.Class left outer JOIN  
dbo.ObjectFieldValue MACAddressValue ON dbo.MetaObject.pkid=MACAddressValue.ObjectID and dbo.MetaObject.Machine=MACAddressValue.MachineID and MACAddressField.pkid = MACAddressValue.fieldid  
 INNER JOIN  dbo.Field  StaticIPField  ON dbo.MetaObject.Class = StaticIPField.Class left outer JOIN  
dbo.ObjectFieldValue StaticIPValue ON dbo.MetaObject.pkid=StaticIPValue.ObjectID and dbo.MetaObject.Machine=StaticIPValue.MachineID and StaticIPField.pkid = StaticIPValue.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress3Field  ON dbo.MetaObject.Class = NetworkAddress3Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress3Value ON dbo.MetaObject.pkid=NetworkAddress3Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress3Value.MachineID and NetworkAddress3Field.pkid = NetworkAddress3Value.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress4Field  ON dbo.MetaObject.Class = NetworkAddress4Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress4Value ON dbo.MetaObject.pkid=NetworkAddress4Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress4Value.MachineID and NetworkAddress4Field.pkid = NetworkAddress4Value.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress5Field  ON dbo.MetaObject.Class = NetworkAddress5Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress5Value ON dbo.MetaObject.pkid=NetworkAddress5Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress5Value.MachineID and NetworkAddress5Field.pkid = NetworkAddress5Value.fieldid  
 INNER JOIN  dbo.Field  MakeField  ON dbo.MetaObject.Class = MakeField.Class left outer JOIN  
dbo.ObjectFieldValue MakeValue ON dbo.MetaObject.pkid=MakeValue.ObjectID and dbo.MetaObject.Machine=MakeValue.MachineID and MakeField.pkid = MakeValue.fieldid  
 INNER JOIN  dbo.Field  ModelField  ON dbo.MetaObject.Class = ModelField.Class left outer JOIN  
dbo.ObjectFieldValue ModelValue ON dbo.MetaObject.pkid=ModelValue.ObjectID and dbo.MetaObject.Machine=ModelValue.MachineID and ModelField.pkid = ModelValue.fieldid  
 INNER JOIN  dbo.Field  SerialNumberField  ON dbo.MetaObject.Class = SerialNumberField.Class left outer JOIN  
dbo.ObjectFieldValue SerialNumberValue ON dbo.MetaObject.pkid=SerialNumberValue.ObjectID and dbo.MetaObject.Machine=SerialNumberValue.MachineID and SerialNumberField.pkid = SerialNumberValue.fieldid  
 INNER JOIN  dbo.Field  AssetNumberField  ON dbo.MetaObject.Class = AssetNumberField.Class left outer JOIN  
dbo.ObjectFieldValue AssetNumberValue ON dbo.MetaObject.pkid=AssetNumberValue.ObjectID and dbo.MetaObject.Machine=AssetNumberValue.MachineID and AssetNumberField.pkid = AssetNumberValue.fieldid  
 INNER JOIN  dbo.Field  isDNSField  ON dbo.MetaObject.Class = isDNSField.Class left outer JOIN  
dbo.ObjectFieldValue isDNSValue ON dbo.MetaObject.pkid=isDNSValue.ObjectID and dbo.MetaObject.Machine=isDNSValue.MachineID and isDNSField.pkid = isDNSValue.fieldid  
 INNER JOIN  dbo.Field  isDHCPField  ON dbo.MetaObject.Class = isDHCPField.Class left outer JOIN  
dbo.ObjectFieldValue isDHCPValue ON dbo.MetaObject.pkid=isDHCPValue.ObjectID and dbo.MetaObject.Machine=isDHCPValue.MachineID and isDHCPField.pkid = isDHCPValue.fieldid  
 INNER JOIN  dbo.Field  CapacityField  ON dbo.MetaObject.Class = CapacityField.Class left outer JOIN  
dbo.ObjectFieldValue CapacityValue ON dbo.MetaObject.pkid=CapacityValue.ObjectID and dbo.MetaObject.Machine=CapacityValue.MachineID and CapacityField.pkid = CapacityValue.fieldid  
 INNER JOIN  dbo.Field  Mem_TotalField  ON dbo.MetaObject.Class = Mem_TotalField.Class left outer JOIN  
dbo.ObjectFieldValue Mem_TotalValue ON dbo.MetaObject.pkid=Mem_TotalValue.ObjectID and dbo.MetaObject.Machine=Mem_TotalValue.MachineID and Mem_TotalField.pkid = Mem_TotalValue.fieldid  
 INNER JOIN  dbo.Field  CPU_TypeField  ON dbo.MetaObject.Class = CPU_TypeField.Class left outer JOIN  
dbo.ObjectFieldValue CPU_TypeValue ON dbo.MetaObject.pkid=CPU_TypeValue.ObjectID and dbo.MetaObject.Machine=CPU_TypeValue.MachineID and CPU_TypeField.pkid = CPU_TypeValue.fieldid  
 INNER JOIN  dbo.Field  CPU_SpeedField  ON dbo.MetaObject.Class = CPU_SpeedField.Class left outer JOIN  
dbo.ObjectFieldValue CPU_SpeedValue ON dbo.MetaObject.pkid=CPU_SpeedValue.ObjectID and dbo.MetaObject.Machine=CPU_SpeedValue.MachineID and CPU_SpeedField.pkid = CPU_SpeedValue.fieldid  
 INNER JOIN  dbo.Field  MonitorField  ON dbo.MetaObject.Class = MonitorField.Class left outer JOIN  
dbo.ObjectFieldValue MonitorValue ON dbo.MetaObject.pkid=MonitorValue.ObjectID and dbo.MetaObject.Machine=MonitorValue.MachineID and MonitorField.pkid = MonitorValue.fieldid  
 INNER JOIN  dbo.Field  Video_CardField  ON dbo.MetaObject.Class = Video_CardField.Class left outer JOIN  
dbo.ObjectFieldValue Video_CardValue ON dbo.MetaObject.pkid=Video_CardValue.ObjectID and dbo.MetaObject.Machine=Video_CardValue.MachineID and Video_CardField.pkid = Video_CardValue.fieldid  
 INNER JOIN  dbo.Field  Number_Of_DisksField  ON dbo.MetaObject.Class = Number_Of_DisksField.Class left outer JOIN  
dbo.ObjectFieldValue Number_Of_DisksValue ON dbo.MetaObject.pkid=Number_Of_DisksValue.ObjectID and dbo.MetaObject.Machine=Number_Of_DisksValue.MachineID and Number_Of_DisksField.pkid = Number_Of_DisksValue.fieldid  
 INNER JOIN  dbo.Field  DatePurchasedField  ON dbo.MetaObject.Class = DatePurchasedField.Class left outer JOIN  
dbo.ObjectFieldValue DatePurchasedValue ON dbo.MetaObject.pkid=DatePurchasedValue.ObjectID and dbo.MetaObject.Machine=DatePurchasedValue.MachineID and DatePurchasedField.pkid = DatePurchasedValue.fieldid  
 INNER JOIN  dbo.Field  UnderWarrantyField  ON dbo.MetaObject.Class = UnderWarrantyField.Class left outer JOIN  
dbo.ObjectFieldValue UnderWarrantyValue ON dbo.MetaObject.pkid=UnderWarrantyValue.ObjectID and dbo.MetaObject.Machine=UnderWarrantyValue.MachineID and UnderWarrantyField.pkid = UnderWarrantyValue.fieldid  
 INNER JOIN  dbo.Field  DomainField  ON dbo.MetaObject.Class = DomainField.Class left outer JOIN  
dbo.ObjectFieldValue DomainValue ON dbo.MetaObject.pkid=DomainValue.ObjectID and dbo.MetaObject.Machine=DomainValue.MachineID and DomainField.pkid = DomainValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  ServerTypeField  ON dbo.MetaObject.Class = ServerTypeField.Class left outer JOIN  
dbo.ObjectFieldValue ServerTypeValue ON dbo.MetaObject.pkid=ServerTypeValue.ObjectID and dbo.MetaObject.Machine=ServerTypeValue.MachineID and ServerTypeField.pkid = ServerTypeValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'SystemComponent')  AND TypeField.Name ='Type'  AND NameField.Name ='Name'  AND DescriptionField.Name ='Description'  AND SeverityRatingField.Name ='SeverityRating'  AND ConfigurationField.Name ='Configuration'  AND MACAddressField.Name ='MACAddress'  AND StaticIPField.Name ='StaticIP'  AND NetworkAddress3Field.Name ='NetworkAddress3'  AND NetworkAddress4Field.Name ='NetworkAddress4'  AND NetworkAddress5Field.Name ='NetworkAddress5'  AND MakeField.Name ='Make'  AND ModelField.Name ='Model'  AND SerialNumberField.Name ='SerialNumber'  AND AssetNumberField.Name ='AssetNumber'  AND isDNSField.Name ='isDNS'  AND isDHCPField.Name ='isDHCP'  AND CapacityField.Name ='Capacity'  AND Mem_TotalField.Name ='Mem_Total'  AND CPU_TypeField.Name ='CPU_Type'  AND CPU_SpeedField.Name ='CPU_Speed'  AND MonitorField.Name ='Monitor'  AND Video_CardField.Name ='Video_Card'  AND Number_Of_DisksField.Name ='Number_Of_Disks'  AND DatePurchasedField.Name ='DatePurchased'  AND UnderWarrantyField.Name ='UnderWarranty'  AND DomainField.Name ='Domain'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND ServerTypeField.Name ='ServerType'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_SystemComponent_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_SystemComponent_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,TypeValue.ValueString as Type,NameValue.ValueString as Name,DescriptionValue.ValueString as Description,SeverityRatingValue.ValueString as SeverityRating,ConfigurationValue.ValueRTF as Configuration,MACAddressValue.ValueString as MACAddress,StaticIPValue.ValueString as StaticIP,NetworkAddress3Value.ValueString as NetworkAddress3,NetworkAddress4Value.ValueString as NetworkAddress4,NetworkAddress5Value.ValueString as NetworkAddress5,MakeValue.ValueString as Make,ModelValue.ValueString as Model,SerialNumberValue.ValueString as SerialNumber,AssetNumberValue.ValueString as AssetNumber,isDNSValue.ValueString as isDNS,isDHCPValue.ValueString as isDHCP,CapacityValue.ValueString as Capacity,Mem_TotalValue.ValueString as Mem_Total,CPU_TypeValue.ValueString as CPU_Type,CPU_SpeedValue.ValueString as CPU_Speed,MonitorValue.ValueString as Monitor,Video_CardValue.ValueString as Video_Card,Number_Of_DisksValue.ValueString as Number_Of_Disks,DatePurchasedValue.ValueString as DatePurchased,UnderWarrantyValue.ValueString as UnderWarranty,DomainValue.ValueString as Domain,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,ServerTypeValue.ValueString as ServerType,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  TypeField  ON dbo.MetaObject.Class = TypeField.Class left outer JOIN  
dbo.ObjectFieldValue TypeValue ON dbo.MetaObject.pkid=TypeValue.ObjectID and dbo.MetaObject.Machine=TypeValue.MachineID and TypeField.pkid = TypeValue.fieldid  
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  DescriptionField  ON dbo.MetaObject.Class = DescriptionField.Class left outer JOIN  
dbo.ObjectFieldValue DescriptionValue ON dbo.MetaObject.pkid=DescriptionValue.ObjectID and dbo.MetaObject.Machine=DescriptionValue.MachineID and DescriptionField.pkid = DescriptionValue.fieldid  
 INNER JOIN  dbo.Field  SeverityRatingField  ON dbo.MetaObject.Class = SeverityRatingField.Class left outer JOIN  
dbo.ObjectFieldValue SeverityRatingValue ON dbo.MetaObject.pkid=SeverityRatingValue.ObjectID and dbo.MetaObject.Machine=SeverityRatingValue.MachineID and SeverityRatingField.pkid = SeverityRatingValue.fieldid  
 INNER JOIN  dbo.Field  ConfigurationField  ON dbo.MetaObject.Class = ConfigurationField.Class left outer JOIN  
dbo.ObjectFieldValue ConfigurationValue ON dbo.MetaObject.pkid=ConfigurationValue.ObjectID and dbo.MetaObject.Machine=ConfigurationValue.MachineID and ConfigurationField.pkid = ConfigurationValue.fieldid  
 INNER JOIN  dbo.Field  MACAddressField  ON dbo.MetaObject.Class = MACAddressField.Class left outer JOIN  
dbo.ObjectFieldValue MACAddressValue ON dbo.MetaObject.pkid=MACAddressValue.ObjectID and dbo.MetaObject.Machine=MACAddressValue.MachineID and MACAddressField.pkid = MACAddressValue.fieldid  
 INNER JOIN  dbo.Field  StaticIPField  ON dbo.MetaObject.Class = StaticIPField.Class left outer JOIN  
dbo.ObjectFieldValue StaticIPValue ON dbo.MetaObject.pkid=StaticIPValue.ObjectID and dbo.MetaObject.Machine=StaticIPValue.MachineID and StaticIPField.pkid = StaticIPValue.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress3Field  ON dbo.MetaObject.Class = NetworkAddress3Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress3Value ON dbo.MetaObject.pkid=NetworkAddress3Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress3Value.MachineID and NetworkAddress3Field.pkid = NetworkAddress3Value.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress4Field  ON dbo.MetaObject.Class = NetworkAddress4Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress4Value ON dbo.MetaObject.pkid=NetworkAddress4Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress4Value.MachineID and NetworkAddress4Field.pkid = NetworkAddress4Value.fieldid  
 INNER JOIN  dbo.Field  NetworkAddress5Field  ON dbo.MetaObject.Class = NetworkAddress5Field.Class left outer JOIN  
dbo.ObjectFieldValue NetworkAddress5Value ON dbo.MetaObject.pkid=NetworkAddress5Value.ObjectID and dbo.MetaObject.Machine=NetworkAddress5Value.MachineID and NetworkAddress5Field.pkid = NetworkAddress5Value.fieldid  
 INNER JOIN  dbo.Field  MakeField  ON dbo.MetaObject.Class = MakeField.Class left outer JOIN  
dbo.ObjectFieldValue MakeValue ON dbo.MetaObject.pkid=MakeValue.ObjectID and dbo.MetaObject.Machine=MakeValue.MachineID and MakeField.pkid = MakeValue.fieldid  
 INNER JOIN  dbo.Field  ModelField  ON dbo.MetaObject.Class = ModelField.Class left outer JOIN  
dbo.ObjectFieldValue ModelValue ON dbo.MetaObject.pkid=ModelValue.ObjectID and dbo.MetaObject.Machine=ModelValue.MachineID and ModelField.pkid = ModelValue.fieldid  
 INNER JOIN  dbo.Field  SerialNumberField  ON dbo.MetaObject.Class = SerialNumberField.Class left outer JOIN  
dbo.ObjectFieldValue SerialNumberValue ON dbo.MetaObject.pkid=SerialNumberValue.ObjectID and dbo.MetaObject.Machine=SerialNumberValue.MachineID and SerialNumberField.pkid = SerialNumberValue.fieldid  
 INNER JOIN  dbo.Field  AssetNumberField  ON dbo.MetaObject.Class = AssetNumberField.Class left outer JOIN  
dbo.ObjectFieldValue AssetNumberValue ON dbo.MetaObject.pkid=AssetNumberValue.ObjectID and dbo.MetaObject.Machine=AssetNumberValue.MachineID and AssetNumberField.pkid = AssetNumberValue.fieldid  
 INNER JOIN  dbo.Field  isDNSField  ON dbo.MetaObject.Class = isDNSField.Class left outer JOIN  
dbo.ObjectFieldValue isDNSValue ON dbo.MetaObject.pkid=isDNSValue.ObjectID and dbo.MetaObject.Machine=isDNSValue.MachineID and isDNSField.pkid = isDNSValue.fieldid  
 INNER JOIN  dbo.Field  isDHCPField  ON dbo.MetaObject.Class = isDHCPField.Class left outer JOIN  
dbo.ObjectFieldValue isDHCPValue ON dbo.MetaObject.pkid=isDHCPValue.ObjectID and dbo.MetaObject.Machine=isDHCPValue.MachineID and isDHCPField.pkid = isDHCPValue.fieldid  
 INNER JOIN  dbo.Field  CapacityField  ON dbo.MetaObject.Class = CapacityField.Class left outer JOIN  
dbo.ObjectFieldValue CapacityValue ON dbo.MetaObject.pkid=CapacityValue.ObjectID and dbo.MetaObject.Machine=CapacityValue.MachineID and CapacityField.pkid = CapacityValue.fieldid  
 INNER JOIN  dbo.Field  Mem_TotalField  ON dbo.MetaObject.Class = Mem_TotalField.Class left outer JOIN  
dbo.ObjectFieldValue Mem_TotalValue ON dbo.MetaObject.pkid=Mem_TotalValue.ObjectID and dbo.MetaObject.Machine=Mem_TotalValue.MachineID and Mem_TotalField.pkid = Mem_TotalValue.fieldid  
 INNER JOIN  dbo.Field  CPU_TypeField  ON dbo.MetaObject.Class = CPU_TypeField.Class left outer JOIN  
dbo.ObjectFieldValue CPU_TypeValue ON dbo.MetaObject.pkid=CPU_TypeValue.ObjectID and dbo.MetaObject.Machine=CPU_TypeValue.MachineID and CPU_TypeField.pkid = CPU_TypeValue.fieldid  
 INNER JOIN  dbo.Field  CPU_SpeedField  ON dbo.MetaObject.Class = CPU_SpeedField.Class left outer JOIN  
dbo.ObjectFieldValue CPU_SpeedValue ON dbo.MetaObject.pkid=CPU_SpeedValue.ObjectID and dbo.MetaObject.Machine=CPU_SpeedValue.MachineID and CPU_SpeedField.pkid = CPU_SpeedValue.fieldid  
 INNER JOIN  dbo.Field  MonitorField  ON dbo.MetaObject.Class = MonitorField.Class left outer JOIN  
dbo.ObjectFieldValue MonitorValue ON dbo.MetaObject.pkid=MonitorValue.ObjectID and dbo.MetaObject.Machine=MonitorValue.MachineID and MonitorField.pkid = MonitorValue.fieldid  
 INNER JOIN  dbo.Field  Video_CardField  ON dbo.MetaObject.Class = Video_CardField.Class left outer JOIN  
dbo.ObjectFieldValue Video_CardValue ON dbo.MetaObject.pkid=Video_CardValue.ObjectID and dbo.MetaObject.Machine=Video_CardValue.MachineID and Video_CardField.pkid = Video_CardValue.fieldid  
 INNER JOIN  dbo.Field  Number_Of_DisksField  ON dbo.MetaObject.Class = Number_Of_DisksField.Class left outer JOIN  
dbo.ObjectFieldValue Number_Of_DisksValue ON dbo.MetaObject.pkid=Number_Of_DisksValue.ObjectID and dbo.MetaObject.Machine=Number_Of_DisksValue.MachineID and Number_Of_DisksField.pkid = Number_Of_DisksValue.fieldid  
 INNER JOIN  dbo.Field  DatePurchasedField  ON dbo.MetaObject.Class = DatePurchasedField.Class left outer JOIN  
dbo.ObjectFieldValue DatePurchasedValue ON dbo.MetaObject.pkid=DatePurchasedValue.ObjectID and dbo.MetaObject.Machine=DatePurchasedValue.MachineID and DatePurchasedField.pkid = DatePurchasedValue.fieldid  
 INNER JOIN  dbo.Field  UnderWarrantyField  ON dbo.MetaObject.Class = UnderWarrantyField.Class left outer JOIN  
dbo.ObjectFieldValue UnderWarrantyValue ON dbo.MetaObject.pkid=UnderWarrantyValue.ObjectID and dbo.MetaObject.Machine=UnderWarrantyValue.MachineID and UnderWarrantyField.pkid = UnderWarrantyValue.fieldid  
 INNER JOIN  dbo.Field  DomainField  ON dbo.MetaObject.Class = DomainField.Class left outer JOIN  
dbo.ObjectFieldValue DomainValue ON dbo.MetaObject.pkid=DomainValue.ObjectID and dbo.MetaObject.Machine=DomainValue.MachineID and DomainField.pkid = DomainValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  ServerTypeField  ON dbo.MetaObject.Class = ServerTypeField.Class left outer JOIN  
dbo.ObjectFieldValue ServerTypeValue ON dbo.MetaObject.pkid=ServerTypeValue.ObjectID and dbo.MetaObject.Machine=ServerTypeValue.MachineID and ServerTypeField.pkid = ServerTypeValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'SystemComponent')  AND TypeField.Name ='Type'  AND NameField.Name ='Name'  AND DescriptionField.Name ='Description'  AND SeverityRatingField.Name ='SeverityRating'  AND ConfigurationField.Name ='Configuration'  AND MACAddressField.Name ='MACAddress'  AND StaticIPField.Name ='StaticIP'  AND NetworkAddress3Field.Name ='NetworkAddress3'  AND NetworkAddress4Field.Name ='NetworkAddress4'  AND NetworkAddress5Field.Name ='NetworkAddress5'  AND MakeField.Name ='Make'  AND ModelField.Name ='Model'  AND SerialNumberField.Name ='SerialNumber'  AND AssetNumberField.Name ='AssetNumber'  AND isDNSField.Name ='isDNS'  AND isDHCPField.Name ='isDHCP'  AND CapacityField.Name ='Capacity'  AND Mem_TotalField.Name ='Mem_Total'  AND CPU_TypeField.Name ='CPU_Type'  AND CPU_SpeedField.Name ='CPU_Speed'  AND MonitorField.Name ='Monitor'  AND Video_CardField.Name ='Video_Card'  AND Number_Of_DisksField.Name ='Number_Of_Disks'  AND DatePurchasedField.Name ='DatePurchased'  AND UnderWarrantyField.Name ='UnderWarranty'  AND DomainField.Name ='Domain'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND ServerTypeField.Name ='ServerType'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_TimeIndicator_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_TimeIndicator_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,ValueValue.ValueString as Value,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  ValueField  ON dbo.MetaObject.Class = ValueField.Class left outer JOIN  
dbo.ObjectFieldValue ValueValue ON dbo.MetaObject.pkid=ValueValue.ObjectID and dbo.MetaObject.Machine=ValueValue.MachineID and ValueField.pkid = ValueValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'TimeIndicator')  AND ValueField.Name ='Value'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_TimeIndicator_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_TimeIndicator_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,ValueValue.ValueString as Value,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  ValueField  ON dbo.MetaObject.Class = ValueField.Class left outer JOIN  
dbo.ObjectFieldValue ValueValue ON dbo.MetaObject.pkid=ValueValue.ObjectID and dbo.MetaObject.Machine=ValueValue.MachineID and ValueField.pkid = ValueValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 WHERE (dbo.MetaObject.Class = 'TimeIndicator')  AND ValueField.Name ='Value'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType' 

GO
/****** Object:  View [dbo].[METAView_TimeScheme_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_TimeScheme_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,GapTypeValue.ValueString as GapType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3 FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 WHERE (dbo.MetaObject.Class = 'TimeScheme')  AND NameField.Name ='Name'  AND GapTypeField.Name ='GapType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3' 

GO
/****** Object:  View [dbo].[METAView_TimeScheme_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_TimeScheme_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,GapTypeValue.ValueString as GapType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3 FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 WHERE (dbo.MetaObject.Class = 'TimeScheme')  AND NameField.Name ='Name'  AND GapTypeField.Name ='GapType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3' 

GO
/****** Object:  View [dbo].[METAView_TimeUnit_Listing]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_TimeUnit_Listing]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,TimeUnitTypeValue.ValueString as TimeUnitType,ValueValue.ValueString as Value,GapTypeValue.ValueString as GapType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3 FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  TimeUnitTypeField  ON dbo.MetaObject.Class = TimeUnitTypeField.Class left outer JOIN  
dbo.ObjectFieldValue TimeUnitTypeValue ON dbo.MetaObject.pkid=TimeUnitTypeValue.ObjectID and dbo.MetaObject.Machine=TimeUnitTypeValue.MachineID and TimeUnitTypeField.pkid = TimeUnitTypeValue.fieldid  
 INNER JOIN  dbo.Field  ValueField  ON dbo.MetaObject.Class = ValueField.Class left outer JOIN  
dbo.ObjectFieldValue ValueValue ON dbo.MetaObject.pkid=ValueValue.ObjectID and dbo.MetaObject.Machine=ValueValue.MachineID and ValueField.pkid = ValueValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 WHERE (dbo.MetaObject.Class = 'TimeUnit')  AND NameField.Name ='Name'  AND TimeUnitTypeField.Name ='TimeUnitType'  AND ValueField.Name ='Value'  AND GapTypeField.Name ='GapType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3' 

GO
/****** Object:  View [dbo].[METAView_TimeUnit_Retrieval]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[METAView_TimeUnit_Retrieval]
AS
SELECT  dbo.MetaObject.WorkspaceName,dbo.MetaObject.WorkspaceTypeID,dbo.MetaObject.VCStatusID,dbo.MetaObject.pkid,dbo.MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,TimeUnitTypeValue.ValueString as TimeUnitType,ValueValue.ValueString as Value,GapTypeValue.ValueString as GapType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3 FROM dbo.MetaObject 
 INNER JOIN  dbo.Field  NameField  ON dbo.MetaObject.Class = NameField.Class left outer JOIN  
dbo.ObjectFieldValue NameValue ON dbo.MetaObject.pkid=NameValue.ObjectID and dbo.MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  
 INNER JOIN  dbo.Field  TimeUnitTypeField  ON dbo.MetaObject.Class = TimeUnitTypeField.Class left outer JOIN  
dbo.ObjectFieldValue TimeUnitTypeValue ON dbo.MetaObject.pkid=TimeUnitTypeValue.ObjectID and dbo.MetaObject.Machine=TimeUnitTypeValue.MachineID and TimeUnitTypeField.pkid = TimeUnitTypeValue.fieldid  
 INNER JOIN  dbo.Field  ValueField  ON dbo.MetaObject.Class = ValueField.Class left outer JOIN  
dbo.ObjectFieldValue ValueValue ON dbo.MetaObject.pkid=ValueValue.ObjectID and dbo.MetaObject.Machine=ValueValue.MachineID and ValueField.pkid = ValueValue.fieldid  
 INNER JOIN  dbo.Field  GapTypeField  ON dbo.MetaObject.Class = GapTypeField.Class left outer JOIN  
dbo.ObjectFieldValue GapTypeValue ON dbo.MetaObject.pkid=GapTypeValue.ObjectID and dbo.MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  
 INNER JOIN  dbo.Field  CustomField1Field  ON dbo.MetaObject.Class = CustomField1Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField1Value ON dbo.MetaObject.pkid=CustomField1Value.ObjectID and dbo.MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  
 INNER JOIN  dbo.Field  CustomField2Field  ON dbo.MetaObject.Class = CustomField2Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField2Value ON dbo.MetaObject.pkid=CustomField2Value.ObjectID and dbo.MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  
 INNER JOIN  dbo.Field  CustomField3Field  ON dbo.MetaObject.Class = CustomField3Field.Class left outer JOIN  
dbo.ObjectFieldValue CustomField3Value ON dbo.MetaObject.pkid=CustomField3Value.ObjectID and dbo.MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  
 WHERE (dbo.MetaObject.Class = 'TimeUnit')  AND NameField.Name ='Name'  AND TimeUnitTypeField.Name ='TimeUnitType'  AND ValueField.Name ='Value'  AND GapTypeField.Name ='GapType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3' 

GO
/****** Object:  View [dbo].[vw_FieldValue]    Script Date: 09/22/2009 12:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_FieldValue]
AS
SELECT     dbo.Field.Name AS Field, dbo.ObjectFieldValue.ValueString, dbo.ObjectFieldValue.ValueInt, dbo.ObjectFieldValue.ValueDouble, 
                      dbo.ObjectFieldValue.ValueObjectID, dbo.ObjectFieldValue.ValueDate, dbo.ObjectFieldValue.ValueBoolean
FROM         dbo.Field INNER JOIN
                      dbo.ObjectFieldValue ON dbo.Field.pkid = dbo.ObjectFieldValue.FieldID

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Field"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ObjectFieldValue"
            Begin Extent = 
               Top = 6
               Left = 227
               Bottom = 114
               Right = 378
            End
            DisplayFlags = 280
            TopColumn = 6
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 3945
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_FieldValue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_FieldValue'
UPDATE FIELD SET CATEGORY='MISC' WHERE NAME LIKE 'CUSTOM%'