using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;

namespace MetaBuilder.WinUI
{
    public static class DatabaseVersions
    {

        #region SQL to hold your hand

        //o.O
        //select 'databaseVersions.Add(new Guid("'+convert(nvarchar(50),newid())+'"), "EXEC db_AddArtifacts '''+parentclass+''','''+childclass+''',''Mapping'',''Automation'',1|28April2015");' from classassociation where associationtypeid=4 AND  isactive=1 AND ( parentclass = 'governancemechanism' OR childclass = 'governancemechanism')

        //new ids formatted c# version addition
        //select top XXXX 'databaseVersions.Add(new Guid("'+convert(nvarchar(50),newid())+'"),' from classassociation

        //this is amazing if you 'rename' a class and it must have the same associations as the 'old' one
        //--ClassAssociations
        //select 'databaseVersions.Add(new Guid("'+convert(nvarchar(50),newid())+'"),EXEC db_addClassAssociations ''' + parentclass + ''',''' + childclass+''','''+ ltrim(rtrim(associationtype.name))+''','''','+ltrim(rtrim(str(isdefault)))+','+ltrim(rtrim(str(isactive))) from classassociation inner join associationtype on associationtypeid = pkid where parentclass = 'Software' or childclass = 'Software'
        //union
        //select 'databaseVersions.Add(new Guid("'+convert(nvarchar(50),newid())+'"),EXEC db_addClassAssociations ''' + parentclass + ''',''' + childclass+''','''+ ltrim(rtrim(associationtype.name))+''','''','+ltrim(rtrim(str(isdefault)))+','+ltrim(rtrim(str(isactive))) from classassociation inner join associationtype on associationtypeid = pkid where parentclass = 'SystemComponent' or childclass = 'SystemComponent'
        //union
        //select 'databaseVersions.Add(new Guid("'+convert(nvarchar(50),newid())+'"),EXEC db_addClassAssociations ''' + parentclass + ''',''' + childclass+''','''+ ltrim(rtrim(associationtype.name))+''','''','+ltrim(rtrim(str(isdefault)))+','+ltrim(rtrim(str(isactive))) from classassociation inner join associationtype on associationtypeid = pkid where parentclass = 'Peripheral' or childclass = 'Peripheral'

        //dont forget about the allowed artifacts that existed...
        //--Artifacts
        //select 'databaseVersions.Add(new Guid("'+convert(nvarchar(50),newid())+'"),EXEC db_addArtifacts ''' + parentclass + ''',''' + childclass+''','''+ ltrim(rtrim(associationtype.name))+''','''+allowedartifact.class+''','+ltrim(rtrim(str(allowedartifact.isactive))) from classassociation inner join associationtype on associationtypeid = pkid inner join allowedartifact on classassociation.caid = allowedartifact.caid where parentclass = 'Software' or childclass = 'Software'
        //union
        //select 'databaseVersions.Add(new Guid("'+convert(nvarchar(50),newid())+'"),EXEC db_addArtifacts ''' + parentclass + ''',''' + childclass+''','''+ ltrim(rtrim(associationtype.name))+''','''+allowedartifact.class+''','+ltrim(rtrim(str(allowedartifact.isactive))) from classassociation inner join associationtype on associationtypeid = pkid inner join allowedartifact on classassociation.caid = allowedartifact.caid where parentclass = 'SystemComponent' or childclass = 'SystemComponent'
        //union
        //select 'databaseVersions.Add(new Guid("'+convert(nvarchar(50),newid())+'"),EXEC db_addArtifacts ''' + parentclass + ''',''' + childclass+''','''+ ltrim(rtrim(associationtype.name))+''','''+allowedartifact.class+''','+ltrim(rtrim(str(allowedartifact.isactive))) from classassociation inner join associationtype on associationtypeid = pkid inner join allowedartifact on classassociation.caid = allowedartifact.caid where parentclass = 'Peripheral' or childclass = 'Peripheral'


        //fields based on other class
        //       select 'databaseVersions.Add(new Guid("'+convert(nvarchar(50),newid())+'"), "EXEC db_addFields ''' + class + ''',''' + name + ''',''' + datatype + ''',''' + category + ''',''' + description + ''','+ltrim(rtrim(str(isunique)))+','+ltrim(rtrim(str(isunique)))+','+ltrim(rtrim(str(isunique)))+'|5 October 2015");'
        //from field 
        //where class = 'physicaldatacomponent' and isactive = 1 
        //order by sortorder asc


        #endregion

        public static OrderedDictionary BuildVersions()
        {
            //should make this load from xml (Then we can package them as the regions are laid out below and save the package to database instead)
            string longQuery = "";
            OrderedDictionary databaseVersions = new OrderedDictionary();
            //7 June 2012
            databaseVersions.Add(new Guid("75E4A9AB-3B4D-4E44-A418-DF497AACA8D9"), "NULL|Base version"); //the first version does not have anything to update so the sql is nothing

            //hopefully this fixes startup new install problem
            databaseVersions.Add(new Guid("5C69B846-8D62-4662-BD36-F1421CC5F9BF"), "EXEC db_AddClasses 'PhysicalDataComponent','Name','ITInfrastructure',1|StartupFix");

            # region 5 February 2013
            databaseVersions.Add(new Guid("BF762D9E-E198-4F47-8088-3F443A7D8FE0"), "UPDATE field SET datatype = 'LongText' WHERE class = 'rationale' AND name = 'value'|Rationale Value to LongText"); //Updates rationale Value field to use longtext
            databaseVersions.Add(new Guid("371B52F8-B90C-4CD4-BFF7-84DEC0D55085"), "UPDATE field SET datatype = 'System.String' WHERE class = 'rationale' AND name = 'uniqueref'|Rationale UniqueRef to LongText"); //Updates rationale UniqueRef field to use longtext

            longQuery += " ALTER VIEW [dbo].[METAView_Rationale_Listing] ";
            longQuery += " AS ";
            longQuery += " SELECT  MetaObject.WorkspaceName,MetaObject.WorkspaceTypeId,MetaObject.VCStatusID,MetaObject.pkid,MetaObject.Machine,VCMachineID,UniqueRefValue.ValueLongText as UniqueRef,RationaleTypeValue.ValueString as RationaleType,ValueValue.ValueLongText as Value,AuthorNameValue.ValueString as AuthorName,EffectiveDateValue.ValueString as EffectiveDate,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType,LongDescriptionValue.ValueLongText as LongDescription FROM MetaObject ";
            longQuery += " INNER JOIN  dbo.Field  UniqueRefField  ON MetaObject.Class = UniqueRefField.Class left outer JOIN  ";
            longQuery += " dbo.ObjectFieldValue UniqueRefValue ON MetaObject.pkid=UniqueRefValue.ObjectID and MetaObject.Machine=UniqueRefValue.MachineID and UniqueRefField.pkid = UniqueRefValue.fieldid  ";
            longQuery += " INNER JOIN  dbo.Field  RationaleTypeField  ON MetaObject.Class = RationaleTypeField.Class left outer JOIN  ";
            longQuery += " dbo.ObjectFieldValue RationaleTypeValue ON MetaObject.pkid=RationaleTypeValue.ObjectID and MetaObject.Machine=RationaleTypeValue.MachineID and RationaleTypeField.pkid = RationaleTypeValue.fieldid  ";
            longQuery += " INNER JOIN  dbo.Field  ValueField  ON MetaObject.Class = ValueField.Class left outer JOIN  ";
            longQuery += " dbo.ObjectFieldValue ValueValue ON MetaObject.pkid=ValueValue.ObjectID and MetaObject.Machine=ValueValue.MachineID and ValueField.pkid = ValueValue.fieldid  ";
            longQuery += " INNER JOIN  dbo.Field  AuthorNameField  ON MetaObject.Class = AuthorNameField.Class left outer JOIN  ";
            longQuery += " dbo.ObjectFieldValue AuthorNameValue ON MetaObject.pkid=AuthorNameValue.ObjectID and MetaObject.Machine=AuthorNameValue.MachineID and AuthorNameField.pkid = AuthorNameValue.fieldid  ";
            longQuery += " INNER JOIN  dbo.Field  EffectiveDateField  ON MetaObject.Class = EffectiveDateField.Class left outer JOIN  ";
            longQuery += " dbo.ObjectFieldValue EffectiveDateValue ON MetaObject.pkid=EffectiveDateValue.ObjectID and MetaObject.Machine=EffectiveDateValue.MachineID and EffectiveDateField.pkid = EffectiveDateValue.fieldid  ";
            longQuery += " INNER JOIN  dbo.Field  CustomField1Field  ON MetaObject.Class = CustomField1Field.Class left outer JOIN  ";
            longQuery += " dbo.ObjectFieldValue CustomField1Value ON MetaObject.pkid=CustomField1Value.ObjectID and MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  ";
            longQuery += " INNER JOIN  dbo.Field  CustomField2Field  ON MetaObject.Class = CustomField2Field.Class left outer JOIN  ";
            longQuery += " dbo.ObjectFieldValue CustomField2Value ON MetaObject.pkid=CustomField2Value.ObjectID and MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  ";
            longQuery += " INNER JOIN  dbo.Field  CustomField3Field  ON MetaObject.Class = CustomField3Field.Class left outer JOIN  ";
            longQuery += " dbo.ObjectFieldValue CustomField3Value ON MetaObject.pkid=CustomField3Value.ObjectID and MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  ";
            longQuery += " INNER JOIN  dbo.Field  GapTypeField  ON MetaObject.Class = GapTypeField.Class left outer JOIN  ";
            longQuery += " dbo.ObjectFieldValue GapTypeValue ON MetaObject.pkid=GapTypeValue.ObjectID and MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  ";
            longQuery += " INNER JOIN  dbo.Field  LongDescriptionField  ON MetaObject.Class = LongDescriptionField.Class left outer JOIN  ";
            longQuery += " dbo.ObjectFieldValue LongDescriptionValue ON MetaObject.pkid=LongDescriptionValue.ObjectID and MetaObject.Machine=LongDescriptionValue.MachineID and LongDescriptionField.pkid = LongDescriptionValue.fieldid  ";
            longQuery += " WHERE (MetaObject.Class = 'Rationale')  AND UniqueRefField.Name ='UniqueRef'  AND RationaleTypeField.Name ='RationaleType'  AND ValueField.Name ='Value'  AND AuthorNameField.Name ='AuthorName'  AND EffectiveDateField.Name ='EffectiveDate'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType'  AND LongDescriptionField.Name ='LongDescription' ";
            databaseVersions.Add(new Guid("AAB3FBD9-ECB2-439F-82C5-2B9EE56C907E"), longQuery + "|Metaview_rationale_listing to Longtext"); //Updates rationale View field to use longtext
            longQuery = "";

            longQuery += " ALTER VIEW [dbo].[METAView_Rationale_Retrieval] ";
            longQuery += " AS ";
            longQuery += " SELECT  MetaObject.WorkspaceName,MetaObject.WorkspaceTypeId,MetaObject.VCStatusID,MetaObject.pkid,MetaObject.Machine,VCMachineID,UniqueRefValue.ValueLongText as UniqueRef,RationaleTypeValue.ValueString as RationaleType,ValueValue.ValueLongText as Value,AuthorNameValue.ValueString as AuthorName,EffectiveDateValue.ValueString as EffectiveDate,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType,LongDescriptionValue.ValueLongText as LongDescription FROM MetaObject ";
            longQuery += " INNER JOIN  dbo.Field  UniqueRefField  ON MetaObject.Class = UniqueRefField.Class left outer JOIN  ";
            longQuery += " dbo.ObjectFieldValue UniqueRefValue ON MetaObject.pkid=UniqueRefValue.ObjectID and MetaObject.Machine=UniqueRefValue.MachineID and UniqueRefField.pkid = UniqueRefValue.fieldid  ";
            longQuery += " INNER JOIN  dbo.Field  RationaleTypeField  ON MetaObject.Class = RationaleTypeField.Class left outer JOIN  ";
            longQuery += " dbo.ObjectFieldValue RationaleTypeValue ON MetaObject.pkid=RationaleTypeValue.ObjectID and MetaObject.Machine=RationaleTypeValue.MachineID and RationaleTypeField.pkid = RationaleTypeValue.fieldid  ";
            longQuery += " INNER JOIN  dbo.Field  ValueField  ON MetaObject.Class = ValueField.Class left outer JOIN  ";
            longQuery += " dbo.ObjectFieldValue ValueValue ON MetaObject.pkid=ValueValue.ObjectID and MetaObject.Machine=ValueValue.MachineID and ValueField.pkid = ValueValue.fieldid  ";
            longQuery += " INNER JOIN  dbo.Field  AuthorNameField  ON MetaObject.Class = AuthorNameField.Class left outer JOIN  ";
            longQuery += " dbo.ObjectFieldValue AuthorNameValue ON MetaObject.pkid=AuthorNameValue.ObjectID and MetaObject.Machine=AuthorNameValue.MachineID and AuthorNameField.pkid = AuthorNameValue.fieldid  ";
            longQuery += " INNER JOIN  dbo.Field  EffectiveDateField  ON MetaObject.Class = EffectiveDateField.Class left outer JOIN  ";
            longQuery += " dbo.ObjectFieldValue EffectiveDateValue ON MetaObject.pkid=EffectiveDateValue.ObjectID and MetaObject.Machine=EffectiveDateValue.MachineID and EffectiveDateField.pkid = EffectiveDateValue.fieldid  ";
            longQuery += " INNER JOIN  dbo.Field  CustomField1Field  ON MetaObject.Class = CustomField1Field.Class left outer JOIN  ";
            longQuery += " dbo.ObjectFieldValue CustomField1Value ON MetaObject.pkid=CustomField1Value.ObjectID and MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  ";
            longQuery += " INNER JOIN  dbo.Field  CustomField2Field  ON MetaObject.Class = CustomField2Field.Class left outer JOIN  ";
            longQuery += " dbo.ObjectFieldValue CustomField2Value ON MetaObject.pkid=CustomField2Value.ObjectID and MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid  ";
            longQuery += " INNER JOIN  dbo.Field  CustomField3Field  ON MetaObject.Class = CustomField3Field.Class left outer JOIN  ";
            longQuery += " dbo.ObjectFieldValue CustomField3Value ON MetaObject.pkid=CustomField3Value.ObjectID and MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid  ";
            longQuery += " INNER JOIN  dbo.Field  GapTypeField  ON MetaObject.Class = GapTypeField.Class left outer JOIN  ";
            longQuery += " dbo.ObjectFieldValue GapTypeValue ON MetaObject.pkid=GapTypeValue.ObjectID and MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid  ";
            longQuery += " INNER JOIN  dbo.Field  LongDescriptionField  ON MetaObject.Class = LongDescriptionField.Class left outer JOIN  ";
            longQuery += " dbo.ObjectFieldValue LongDescriptionValue ON MetaObject.pkid=LongDescriptionValue.ObjectID and MetaObject.Machine=LongDescriptionValue.MachineID and LongDescriptionField.pkid = LongDescriptionValue.fieldid  ";
            longQuery += " WHERE (MetaObject.Class = 'Rationale')  AND UniqueRefField.Name ='UniqueRef'  AND RationaleTypeField.Name ='RationaleType'  AND ValueField.Name ='Value'  AND AuthorNameField.Name ='AuthorName'  AND EffectiveDateField.Name ='EffectiveDate'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType'  AND LongDescriptionField.Name ='LongDescription' ";
            databaseVersions.Add(new Guid("38F7ED5A-5C0F-4891-A282-119F9673241F"), longQuery + "|Metaview_rationale_retrieval to Longtext"); //Updates rationale View field to use longtext
            longQuery = "";
            //update all valuestring values and uniquerefs to longtext

            longQuery += " DECLARE @FieldID INT";
            longQuery += " SET @FieldID = (SELECT pkid FROM field WHERE class = 'rationale' AND name = 'value')";
            longQuery += " UPDATE objectfieldvalue SET valuelongtext = CASE WHEN valuelongtext IS NULL THEN valuestring ELSE valuelongtext END WHERE fieldid = @FieldID";
            databaseVersions.Add(new Guid("80013704-271F-486B-83CE-67CA566839ED"), longQuery + "|Update rationale values to longtext"); //Updates rationale View field to use longtext
            longQuery = "";

            longQuery += " DECLARE @FieldID INT";
            longQuery += " SET @FieldID = (SELECT pkid FROM field WHERE class = 'rationale' AND name = 'uniqueref')";
            longQuery += " UPDATE objectfieldvalue SET valuelongtext = CASE WHEN valuelongtext IS NULL THEN valuestring ELSE valuelongtext END WHERE fieldid = @FieldID";
            databaseVersions.Add(new Guid("93F3C7B9-78E9-4405-9BAB-122F8CF6281C"), longQuery + "|Update rationale uniquerefs to longtext"); //Updates rationale View field to use longtext
            longQuery = "";

            #endregion

            #region 15 April

            longQuery = "string.Join(Convert.ToString('' ''), new string[] { Title, Name, Surname }).Trim()";
            databaseVersions.Add(new Guid("7CC8D40F-DBBF-46CC-96F3-D5B5084EF5C2"), "UPDATE [Class] SET DescriptionCode = '" + longQuery + "' WHERE Name = 'Employee'|Update employee display field");

            #endregion

            #region 15 October 2013 - SAPPI REQUEST

            longQuery = "string.Join(Convert.ToString('' ''), new string[] { Name, Surname }).Trim()";
            databaseVersions.Add(new Guid("4BEB8A57-268D-4F03-936C-0E3C75573970"), "UPDATE [Class] SET DescriptionCode = '" + longQuery + "' WHERE Name = 'Employee'|Update employee display field");

            #endregion

            #region 23 October 2013

            longQuery = "!string.IsNullOrEmpty(Number)?Number:!string.IsNullOrEmpty(Name)?Name:\"\"";
            databaseVersions.Add(new Guid("2B52949B-FBEE-4319-923F-65ABFC8B8088"), "UPDATE [Class] SET DescriptionCode = '" + longQuery + "' WHERE Name = 'CSF'|Update CSF Display Field");
            longQuery = "!string.IsNullOrEmpty(UniqueRef)?UniqueRef:!string.IsNullOrEmpty(Value.ToString())?Value.ToString():!string.IsNullOrEmpty(LongDescription.ToString())?LongDescription.ToString():\"\"";
            databaseVersions.Add(new Guid("E51EE759-4D91-48FE-A270-4476E6199BEC"), "UPDATE [Class] SET DescriptionCode = '" + longQuery + "' WHERE Name = 'Rationale'|Update Rationale Display Field");
            //removes long description
            longQuery = "!string.IsNullOrEmpty(UniqueRef)?UniqueRef:!string.IsNullOrEmpty(Value.ToString())?Value.ToString():\"\"";
            databaseVersions.Add(new Guid("B6EB4114-D0CB-4F47-8100-03823E67733E"), "UPDATE [Class] SET DescriptionCode = '" + longQuery + "' WHERE Name = 'Rationale'|Update Rationale Display Field");
            //updates value == null;
            longQuery = "!string.IsNullOrEmpty(UniqueRef)?UniqueRef:Value!=null?!string.IsNullOrEmpty(Value.ToString())?Value.ToString():\"\":\"\"";
            databaseVersions.Add(new Guid("EC2AB0CA-680D-47E5-9F6F-99254378B2E4"), "UPDATE [Class] SET DescriptionCode = '" + longQuery + "' WHERE Name = 'Rationale'|Update Rationale Display Field NULLVALUES");

            //string UniqueRef = "";
            //object Value=null;
            //string x = !string.IsNullOrEmpty(UniqueRef)?UniqueRef:Value!=null?!string.IsNullOrEmpty(Value.ToString())?Value.ToString():"":"";

            #endregion

            #region XX UPDATE NETWORK ADDRESSES


            //UPDATE Field Set IsUnique = 0 WHERE IsUnique is null
            //UPDATE Field SET [Description] = 'MAC Address',[Name] = 'MACAddress' WHERE [Name] = 'NetworkAddress1'
            //UPDATE Field SET [Description] = 'Static IP Address',[Name] = 'StaticIP' WHERE [Name] = 'NetworkAddress2'

            //UPDATE Field SET [Description] = 'Gateway Address',[Name] = 'GatewayIP' WHERE [Name] = 'NetworkAddress3'
            //UPDATE Field SET [Description] = 'DNS Address',[Name] = 'DNSIP' WHERE [Name] = 'NetworkAddress4'
            //UPDATE Field SET [Description] = 'DHCP Address',[Name] = 'DHCPIP' WHERE [Name] = 'NetworkAddress5'

            //ADD SECONDARY DNS?
            //ADD WINS SERVERS?


            //UPDATE VIEWS :(
            // affected classes will be
            //Location
            //NetworkComponent
            //Peripheral
            //StorageComponent
            //SystemComponent

            #endregion

            #region 24 January 2014 Update Rationale

            longQuery = "ALTER VIEW [dbo].[METAView_Rationale_Listing]  AS  SELECT     MetaObject.WorkspaceName, MetaObject.WorkspaceTypeID, MetaObject.VCStatusID, MetaObject.pkid, MetaObject.Machine, MetaObject.VCMachineID,                                   UniqueRefValue.ValueString AS UniqueRef, RationaleTypeValue.ValueString AS RationaleType, ValueValue.ValueLongText AS Value,                                   AuthorNameValue.ValueString AS AuthorName, EffectiveDateValue.ValueString AS EffectiveDate, CustomField1Value.ValueString AS CustomField1,                                   CustomField2Value.ValueString AS CustomField2, CustomField3Value.ValueString AS CustomField3, GapTypeValue.ValueString AS GapType,                                   LongDescriptionValue.ValueLongText AS LongDescription            FROM         MetaObject INNER JOIN                                  Field AS UniqueRefField ON MetaObject.Class = UniqueRefField.Class LEFT OUTER JOIN                                  ObjectFieldValue AS UniqueRefValue ON MetaObject.pkid = UniqueRefValue.ObjectID AND MetaObject.Machine = UniqueRefValue.MachineID AND                                   UniqueRefField.pkid = UniqueRefValue.FieldID INNER JOIN                                  Field AS RationaleTypeField ON MetaObject.Class = RationaleTypeField.Class LEFT OUTER JOIN                                  ObjectFieldValue AS RationaleTypeValue ON MetaObject.pkid = RationaleTypeValue.ObjectID AND MetaObject.Machine = RationaleTypeValue.MachineID AND                                   RationaleTypeField.pkid = RationaleTypeValue.FieldID INNER JOIN                                  Field AS ValueField ON MetaObject.Class = ValueField.Class LEFT OUTER JOIN                                  ObjectFieldValue AS ValueValue ON MetaObject.pkid = ValueValue.ObjectID AND MetaObject.Machine = ValueValue.MachineID AND                                   ValueField.pkid = ValueValue.FieldID INNER JOIN                                  Field AS AuthorNameField ON MetaObject.Class = AuthorNameField.Class LEFT OUTER JOIN                                  ObjectFieldValue AS AuthorNameValue ON MetaObject.pkid = AuthorNameValue.ObjectID AND MetaObject.Machine = AuthorNameValue.MachineID AND                                   AuthorNameField.pkid = AuthorNameValue.FieldID INNER JOIN                                  Field AS EffectiveDateField ON MetaObject.Class = EffectiveDateField.Class LEFT OUTER JOIN                                  ObjectFieldValue AS EffectiveDateValue ON MetaObject.pkid = EffectiveDateValue.ObjectID AND MetaObject.Machine = EffectiveDateValue.MachineID AND                                   EffectiveDateField.pkid = EffectiveDateValue.FieldID INNER JOIN                                  Field AS CustomField1Field ON MetaObject.Class = CustomField1Field.Class LEFT OUTER JOIN                                  ObjectFieldValue AS CustomField1Value ON MetaObject.pkid = CustomField1Value.ObjectID AND MetaObject.Machine = CustomField1Value.MachineID AND                                   CustomField1Field.pkid = CustomField1Value.FieldID INNER JOIN                                  Field AS CustomField2Field ON MetaObject.Class = CustomField2Field.Class LEFT OUTER JOIN                                  ObjectFieldValue AS CustomField2Value ON MetaObject.pkid = CustomField2Value.ObjectID AND MetaObject.Machine = CustomField2Value.MachineID AND                                   CustomField2Field.pkid = CustomField2Value.FieldID INNER JOIN                                  Field AS CustomField3Field ON MetaObject.Class = CustomField3Field.Class LEFT OUTER JOIN                                  ObjectFieldValue AS CustomField3Value ON MetaObject.pkid = CustomField3Value.ObjectID AND MetaObject.Machine = CustomField3Value.MachineID AND                                   CustomField3Field.pkid = CustomField3Value.FieldID INNER JOIN                                  Field AS GapTypeField ON MetaObject.Class = GapTypeField.Class LEFT OUTER JOIN                                  ObjectFieldValue AS GapTypeValue ON MetaObject.pkid = GapTypeValue.ObjectID AND MetaObject.Machine = GapTypeValue.MachineID AND                                   GapTypeField.pkid = GapTypeValue.FieldID INNER JOIN                                  Field AS LongDescriptionField ON MetaObject.Class = LongDescriptionField.Class LEFT OUTER JOIN                                  ObjectFieldValue AS LongDescriptionValue ON MetaObject.pkid = LongDescriptionValue.ObjectID AND MetaObject.Machine = LongDescriptionValue.MachineID AND                                   LongDescriptionField.pkid = LongDescriptionValue.FieldID            WHERE     (MetaObject.Class = 'Rationale') AND (UniqueRefField.Name = 'UniqueRef') AND (RationaleTypeField.Name = 'RationaleType') AND (ValueField.Name = 'Value') AND                                   (AuthorNameField.Name = 'AuthorName') AND (EffectiveDateField.Name = 'EffectiveDate') AND (CustomField1Field.Name = 'CustomField1') AND                                   (CustomField2Field.Name = 'CustomField2') AND (CustomField3Field.Name = 'CustomField3') AND (GapTypeField.Name = 'GapType') AND                                   (LongDescriptionField.Name = 'LongDescription')";
            databaseVersions.Add(new Guid("31970B97-C527-44AE-8824-EDA049EB9824"), longQuery + " |Rationale View To Select String");

            #endregion

            #region 4 February 2014 Add Delete Permission

            longQuery = "INSERT INTO Permission (Description, PermissionType) VALUES ('Delete', 'WorkSpace')";
            databaseVersions.Add(new Guid("B745CB73-95E1-4187-84E8-612A252BBA93"), longQuery + " |Added Delete Permission");

            #endregion

            #region 5 February 2014 Alter UserPermissionTable to allow multiple permissions

            longQuery = "ALTER TABLE UserPermission DROP CONSTRAINT PK_UserPermission";
            databaseVersions.Add(new Guid("2ED166ED-5226-4DDA-9873-93DE22ADA424"), longQuery + " |Dropped UserPermission PrimaryKey");

            longQuery = "ALTER TABLE UserPermission ADD CONSTRAINT PK_UserPermission PRIMARY KEY (UserID,PermissionID,WorkspaceName,WorkspaceTypeID)";
            databaseVersions.Add(new Guid("79A03D70-814C-4EE6-BCA7-A834434897BC"), longQuery + " |Added UserPermission PrimaryKey");

            #endregion

            #region 14 February 2014

            databaseVersions.Add(new Guid("2ACBD8BE-988E-4F13-B2FC-7ABBA7F219AB"), "IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'DeleteGraphfileObjectAassociationsByMetaObject') AND type IN ( N'P', N'PC' )) DROP PROCEDURE DeleteGraphfileObjectAassociationsByMetaObject|DropDgfoANDaProc");
            databaseVersions.Add(new Guid("75554605-FEB2-4480-846F-27ED7E158938"), "CREATE PROCEDURE DeleteGraphfileObjectAassociationsByMetaObject @pkid INT, @machine varchar (50) AS DELETE FROM GraphFileObject WHERE MetaObjectID = @pkid AND MachineID = @machine DELETE FROM GraphFileAssociation WHERE ObjectID = @pkid AND ObjectMachine = @machine DELETE FROM GraphFileAssociation WHERE ChildObjectID = @pkid AND ChildObjectMachine = @machine|AddDgfoANDaProc");

            #endregion

            #region 29 April 2014 Add connectionsize class

            databaseVersions.Add(new Guid("70DC7EE3-FCB5-4E0B-A8E0-A6A6940D6733"), "exec db_addclasses 'ConnectionSize','Name','AssociationAttribute',1|Insert ConnectionSize Class"); //Updates rationale Value field to use longtext
            databaseVersions.Add(new Guid("D36F56B4-E47D-492F-A261-ECBDF02E69AD"), "exec db_addfields 'ConnectionSize','Name','System.String','General','Connection Size',0,1,1|Add Name field to ConnectionSize"); //Updates rationale Value field to use longtext
            databaseVersions.Add(new Guid("786F7426-F195-4F6F-A4C8-FA9E4B1A09E8"), "exec db_AddArtifacts 'NetworkComponent','NetworkComponent','ConnectedTo','ConnectionSize',1|Add ConnectionSize Artefact between NetworkComponents"); //Updates rationale Value field to use longtext

            databaseVersions.Add(new Guid("3DA4674B-CAB9-46C9-AE6D-7377F28A8F6E"), "IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_ConnectionSize_Listing]')) DROP VIEW [dbo].[METAView_ConnectionSize_Listing]|Drop ConnectionSize View"); //Updates rationale Value field to use longtext
            longQuery = "CREATE VIEW [dbo].[METAView_ConnectionSize_Listing] AS SELECT MetaObject.WorkspaceName, MetaObject.WorkspaceTypeID, MetaObject.VCStatusID, MetaObject.pkid, MetaObject.Machine, MetaObject.VCMachineID,ValueValue.ValueString AS Name FROM MetaObject INNER JOIN Field AS ValueField ON MetaObject.Class = ValueField.Class LEFT OUTER JOIN ObjectFieldValue AS ValueValue ON MetaObject.pkid = ValueValue.ObjectID AND MetaObject.Machine = ValueValue.MachineID AND ValueField.pkid = ValueValue.FieldID WHERE (MetaObject.Class = 'ConnectionSize') AND (ValueField.Name = 'Name')";
            databaseVersions.Add(new Guid("83730327-23D6-43B9-9BDE-E8EC73BEF56D"), longQuery + "|Create ConnectionSize View"); //Updates rationale Value field to use longtext

            #endregion

            #region 8 May 2014 Adding/Defaulting associations

            databaseVersions.Add(new Guid("41209543-F5E2-4CFC-817C-F130D9614036"), "EXEC db_AddClassAssociations 'OrganizationalUnit','Role','Mapping','OrganizationalUnit to Role Mapping',1,1|Defaulting associations");
            databaseVersions.Add(new Guid("04B63EAD-743B-4668-9795-311102C20C2E"), "EXEC db_AddClassAssociations 'Activity','Activity','Start','Activity to Activity Control Flow',0,1|Defaulting associations");
            databaseVersions.Add(new Guid("F3A1FFA7-9F32-4DC7-9EAC-A7AC9A1A2E12"), "EXEC db_AddClassAssociations 'Activity','Activity','Decomposition','Activity to Activity Decomposition',1,1|Defaulting associations");
            databaseVersions.Add(new Guid("B3CD9A73-32C5-4239-B96F-EBCB87372528"), "UPDATE classassociation SET isdefault = 0 WHERE parentclass='Activity' AND childclass='Activity'|Defaulting associations");
            databaseVersions.Add(new Guid("2D61372D-BA26-4E11-8CDE-FA7F798F3C14"), "UPDATE classassociation SET isdefault = 1 WHERE parentclass='Activity' AND childclass='Activity' AND associationtypeid=3|Defaulting associations");
            databaseVersions.Add(new Guid("8A272D58-3F1F-48F1-B5B6-3296A104858B"), "EXEC db_AddClassAssociations 'TimeUnit','TimeUnit','Mapping','TimeUnit to TimeUnit Mapping',1,1|Defaulting associations");
            databaseVersions.Add(new Guid("289905A7-6B86-4063-A946-545DF7EEF4DE"), "EXEC db_AddClassAssociations 'Location','Location','Decomposition','Location to Location Decomposition',1,1|Defaulting associations");
            databaseVersions.Add(new Guid("150BFEA8-CF99-4FDC-802D-FCBC761B957C"), "UPDATE classassociation SET isdefault = 0 WHERE parentclass='Location' AND childclass='Location'|Defaulting associations");
            databaseVersions.Add(new Guid("B1706CDD-5F53-4C60-A991-6BCE7ED1E760"), "UPDATE classassociation SET isdefault = 1 WHERE parentclass='Location' AND childclass='Location' AND associationtypeid=3|Defaulting associations");

            #endregion

            #region 9 May 2014 Metamodel Modifications and additions for time and location

            #region Manipulating Time
            databaseVersions.Add(new Guid("DDC9B814-C6D8-419E-AD23-42DF0FAE33D7"), "update field set name = 'Name' where class = 'TimeIndicator' and name = 'Value'|Updating TimeIndicator Name");
            databaseVersions.Add(new Guid("21F7F828-8853-4907-A498-7992A5462556"), "exec db_addfields 'TimeIndicator','Value','System.String','General','Value',0,2,1|Adding TimeIndicator Field");
            databaseVersions.Add(new Guid("9ECAF9A5-EBB0-46AA-B0AC-CC3EEC70E5FA"), "update class set isactive = 0 where name = 'TimeIndicator'|Making TimeIndicator Inactive");

            databaseVersions.Add(new Guid("4F8AE7F6-BAA5-44BC-8086-E9379E698C52"), "exec db_addclasses 'TimeReference','Name','General',1|Adding TimeReference");
            databaseVersions.Add(new Guid("89C9D792-B85D-4B2A-9B0E-3D6CA47B1A9A"), "exec db_addfields 'TimeReference','Name','System.String','General','Name',1,1,1|Adding TimeReference Field");
            databaseVersions.Add(new Guid("41130750-64DF-40D8-B536-447F0314A034"), "exec db_addfields 'TimeReference','GapType','GapType','General','Gap Type(As-is vs. To-be)',1,90,1|Adding TimeReference Field");
            databaseVersions.Add(new Guid("EEB0F4BC-B8DF-4C16-BBB9-940E2604D250"), "exec db_addfields 'TimeReference','CustomField1','System.String','User Fields','Custom Field 1',0,98,1|Adding TimeReference Field");
            databaseVersions.Add(new Guid("31CCE21C-AFA4-47C4-8AFC-18C7A4287BEF"), "exec db_addfields 'TimeReference','CustomField2','System.String','User Fields','Custom Field 2',0,99,1|Adding TimeReference Field");
            databaseVersions.Add(new Guid("D8103FFF-63FD-440F-AF7F-C9DDEF041C2F"), "exec db_addfields 'TimeReference','CustomField3','System.String','User Fields','Custom Field 3',0,100,1|Adding TimeReference Field");

            databaseVersions.Add(new Guid("0C73AD0E-6259-49A7-AF04-6DE31423DD4C"), "update field set name = 'DomainValue',datatype='System.String',description='Domain Value' where class = 'TimeUnit' and name = 'TimeUnitType'|TimeUnit TimeUnitTypeField Updated to DomainValue");
            databaseVersions.Add(new Guid("655A99EE-0517-42A6-B324-5855A2D7F023"), "update field set name = 'Format',description='Format' where class = 'TimeUnit' and name = 'Value'|TimeUnit ValueField Updated to Format");
            #endregion

            #region Relocating
            databaseVersions.Add(new Guid("D52BED5F-2453-45F1-9AE4-41842477D165"), "exec db_addclasses 'LocationScheme','Name','General',1|Adding LocationScheme Class");
            databaseVersions.Add(new Guid("8BBD8DFD-376F-4E0F-B020-D7BB86796E0B"), "exec db_addfields 'LocationScheme','Name','System.String','General','Name',0,1,1|Adding LocationScheme Field");
            databaseVersions.Add(new Guid("392775EF-048F-44A0-8528-00EEB09BBF6D"), "exec db_addfields 'LocationScheme','GapType','GapType','General','Gap Type(As-is vs. To-be)',0,90,1|Adding LocationScheme Field");
            databaseVersions.Add(new Guid("DC110E41-F650-4FE8-8400-DAD344FDF9AA"), "exec db_addfields 'LocationScheme','CustomField1','System.String','User Fields','Custom Field 1',0,98,1|Adding LocationScheme Field");
            databaseVersions.Add(new Guid("87051166-B4BF-4303-99B3-8203A5997B20"), "exec db_addfields 'LocationScheme','CustomField2','System.String','User Fields','Custom Field 2',0,99,1|Adding LocationScheme Field");
            databaseVersions.Add(new Guid("4F5003A4-124C-4D1E-BCC1-63DB4A5B1AEE"), "exec db_addfields 'LocationScheme','CustomField3','System.String','User Fields','Custom Field 3',0,100,1|Adding LocationScheme Field");

            databaseVersions.Add(new Guid("CC16D6DF-F763-4217-89C7-8DF133CB534A"), "exec db_addclasses 'LocationUnit','Name','General',1|Adding LocationUnit Class");
            databaseVersions.Add(new Guid("5B790C91-36EB-4886-AFC1-25B3D160033E"), "exec db_addfields 'LocationUnit','Name','System.String','General','Name',0,1,1|Adding LocationUnit Field");
            databaseVersions.Add(new Guid("4959E4CB-5C6C-4981-800D-9DE86F6E0699"), "exec db_addfields 'LocationUnit','DomainValue','System.String','General','Domain Value',0,2,1|Adding LocationUnit Field");
            databaseVersions.Add(new Guid("93DAC37D-69C2-441E-9BEC-ED1015262E66"), "exec db_addfields 'LocationUnit','Format','System.String','General','Format',0,3,1|Adding LocationUnit Field");
            databaseVersions.Add(new Guid("C7C766D2-1F22-4281-B686-8F5651E222F0"), "exec db_addfields 'LocationUnit','GapType','GapType','General','Gap Type(As-is vs. To-be)',0,90,1|Adding LocationUnit Field");
            databaseVersions.Add(new Guid("16BFEAE9-E77C-48FA-9F42-33A0B2DB8203"), "exec db_addfields 'LocationUnit','CustomField1','System.String','User Fields','Custom Field 1',0,98,1|Adding LocationUnit Field");
            databaseVersions.Add(new Guid("DFCE3E10-1A19-4418-A609-197C0417C13D"), "exec db_addfields 'LocationUnit','CustomField2','System.String','User Fields','Custom Field 2',0,99,1|Adding LocationUnit Field");
            databaseVersions.Add(new Guid("B8FC7CA6-6432-4329-8D36-D914586F4229"), "exec db_addfields 'LocationUnit','CustomField3','System.String','User Fields','Custom Field 3',0,100,1|Adding LocationUnit Field");

            databaseVersions.Add(new Guid("74221B2F-E600-48E7-BA1F-39420885790C"), "exec db_addclassassociations 'LocationScheme','LocationUnit','Decomposition','LocationScheme to LocationUnit Decomposition',1,1|Adding Location SchemeToUnitDecomposition");
            databaseVersions.Add(new Guid("AE2EE2EC-8E0B-4707-A6AE-E6DFC5A479AF"), "exec db_addclassassociations 'LocationUnit','LocationUnit','Decomposition','LocationUnit to LocationUnit Decomposition',1,1|Adding Location UnitToUnitDecomposition");
            databaseVersions.Add(new Guid("44EA8812-B415-402D-A117-7C900E090A3F"), "exec db_addclassassociations 'LocationUnit','LocationUnit','Mapping','LocationUnit to LocationUnit Mapping',0,1|Adding Location UnitToUnitMapping");
            #endregion

            #region View Regeneration
            //databaseVersions.Add(new Guid("6BBEE2EA-8F01-479E-A238-3336C002EB17"), "IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_LocationReference_Listing]')) DROP VIEW [dbo].[METAView_LocationReference_Listing]|Dropping Views For Recreation");
            //databaseVersions.Add(new Guid("A8BAE52D-8C97-49DA-A831-F5BB56C4D62B"), "IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_LocationReference_Retrieval]'))DROP VIEW [dbo].[METAView_LocationReference_Retrieval]|Dropping Views For Recreation");
            //databaseVersions.Add(new Guid("2DA5AB84-056C-46B8-BFF8-6EE5536CA775"), "IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_LocationScheme_Listing]'))DROP VIEW [dbo].[METAView_LocationScheme_Listing]|Dropping Views For Recreation");
            //databaseVersions.Add(new Guid("2575E97C-E32A-4FE7-9FB9-016E6B99C326"), "IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_LocationScheme_Retrieval]'))DROP VIEW [dbo].[METAView_LocationScheme_Retrieval]|Dropping Views For Recreation");
            //databaseVersions.Add(new Guid("60C974EA-C3D8-4B6B-ABEC-0C82679B99D2"), "IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_LocationUnit_Listing]'))DROP VIEW [dbo].[METAView_LocationUnit_Listing]|Dropping Views For Recreation");
            //databaseVersions.Add(new Guid("05510D28-841C-4B51-8093-65B4266DAC82"), "IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_LocationUnit_Retrieval]'))DROP VIEW [dbo].[METAView_LocationUnit_Retrieval]|Dropping Views For Recreation");
            //databaseVersions.Add(new Guid("6F739B73-4B8F-4EC7-83BA-6067C7812513"), "IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_TimeIndicator_Listing]'))DROP VIEW [dbo].[METAView_TimeIndicator_Listing]|Dropping Views For Recreation");
            //databaseVersions.Add(new Guid("DB87F95B-F37F-439C-B43B-9B3A3C324CD7"), "IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_TimeIndicator_Retrieval]'))DROP VIEW [dbo].[METAView_TimeIndicator_Retrieval]|Dropping Views For Recreation");
            //databaseVersions.Add(new Guid("3CB73632-8D0E-49F9-BE02-7F62FF64D73C"), "IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_TimeReference_Listing]'))DROP VIEW [dbo].[METAView_TimeReference_Listing]|Dropping Views For Recreation");
            //databaseVersions.Add(new Guid("4A2F119D-2A9B-4DAA-B38C-5082A21799F0"), "IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_TimeReference_Retrieval]'))DROP VIEW [dbo].[METAView_TimeReference_Retrieval]|Dropping Views For Recreation");
            //databaseVersions.Add(new Guid("2BA352F9-739E-4AC9-B395-1FC2B6AD4A9B"), "IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_TimeScheme_Listing]'))DROP VIEW [dbo].[METAView_TimeScheme_Listing]|Dropping Views For Recreation");
            //databaseVersions.Add(new Guid("82DC1414-0C7F-4870-8DDA-FBC86615103D"), "IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_TimeScheme_Retrieval]'))DROP VIEW [dbo].[METAView_TimeScheme_Retrieval]|Dropping Views For Recreation");
            //databaseVersions.Add(new Guid("67D7EDC1-DC95-4589-A4C3-8412CD3B55C3"), "IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_TimeUnit_Listing]'))DROP VIEW [dbo].[METAView_TimeUnit_Listing]|Dropping Views For Recreation");
            //databaseVersions.Add(new Guid("83C49DD9-9223-40C4-AF69-08797DAFDC8C"), "IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_TimeUnit_Retrieval]'))DROP VIEW [dbo].[METAView_TimeUnit_Retrieval]|Dropping Views For Recreation");

            //longQuery = "CREATE VIEW [dbo].[METAView_LocationScheme_Listing] AS SELECT  MetaObject.WorkspaceName,MetaObject.WorkspaceTypeId,MetaObject.VCStatusID,MetaObject.pkid,MetaObject.Machine,VCMachineID,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType,NameValue.ValueString as Name FROM MetaObject  INNER JOIN  dbo.Field  CustomField1Field  ON MetaObject.Class = CustomField1Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField1Value ON MetaObject.pkid=CustomField1Value.ObjectID and MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid   INNER JOIN  dbo.Field  CustomField2Field  ON MetaObject.Class = CustomField2Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField2Value ON MetaObject.pkid=CustomField2Value.ObjectID and MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid   INNER JOIN  dbo.Field  CustomField3Field  ON MetaObject.Class = CustomField3Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField3Value ON MetaObject.pkid=CustomField3Value.ObjectID and MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid   INNER JOIN  dbo.Field  GapTypeField  ON MetaObject.Class = GapTypeField.Class left outer JOIN  dbo.ObjectFieldValue GapTypeValue ON MetaObject.pkid=GapTypeValue.ObjectID and MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid   INNER JOIN  dbo.Field  NameField  ON MetaObject.Class = NameField.Class left outer JOIN  dbo.ObjectFieldValue NameValue ON MetaObject.pkid=NameValue.ObjectID and MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid   WHERE (MetaObject.Class = 'LocationScheme')  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType'  AND NameField.Name ='Name' ";
            //databaseVersions.Add(new Guid("461955D4-72F2-4906-963C-3BC761565FAE"), longQuery + "|Recreating Views");
            //longQuery = "CREATE VIEW [dbo].[METAView_LocationScheme_Retrieval] AS SELECT  MetaObject.WorkspaceName,MetaObject.WorkspaceTypeId,MetaObject.VCStatusID,MetaObject.pkid,MetaObject.Machine,VCMachineID,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType,NameValue.ValueString as Name FROM MetaObject  INNER JOIN  dbo.Field  CustomField1Field  ON MetaObject.Class = CustomField1Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField1Value ON MetaObject.pkid=CustomField1Value.ObjectID and MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid   INNER JOIN  dbo.Field  CustomField2Field  ON MetaObject.Class = CustomField2Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField2Value ON MetaObject.pkid=CustomField2Value.ObjectID and MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid   INNER JOIN  dbo.Field  CustomField3Field  ON MetaObject.Class = CustomField3Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField3Value ON MetaObject.pkid=CustomField3Value.ObjectID and MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid   INNER JOIN  dbo.Field  GapTypeField  ON MetaObject.Class = GapTypeField.Class left outer JOIN  dbo.ObjectFieldValue GapTypeValue ON MetaObject.pkid=GapTypeValue.ObjectID and MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid   INNER JOIN  dbo.Field  NameField  ON MetaObject.Class = NameField.Class left outer JOIN  dbo.ObjectFieldValue NameValue ON MetaObject.pkid=NameValue.ObjectID and MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid   WHERE (MetaObject.Class = 'LocationScheme')  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType'  AND NameField.Name ='Name' ";
            //databaseVersions.Add(new Guid("54B61F3B-0A0F-4E3C-AAE1-79A203FB05C8"), longQuery + "|Recreating Views");
            //longQuery = "CREATE VIEW [dbo].[METAView_LocationUnit_Listing] AS SELECT  MetaObject.WorkspaceName,MetaObject.WorkspaceTypeId,MetaObject.VCStatusID,MetaObject.pkid,MetaObject.Machine,VCMachineID,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,DomainValueValue.ValueString as DomainValue,FormatValue.ValueString as Format,GapTypeValue.ValueString as GapType,NameValue.ValueString as Name FROM MetaObject  INNER JOIN  dbo.Field  CustomField1Field  ON MetaObject.Class = CustomField1Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField1Value ON MetaObject.pkid=CustomField1Value.ObjectID and MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  INNER JOIN  dbo.Field  CustomField2Field  ON MetaObject.Class = CustomField2Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField2Value ON MetaObject.pkid=CustomField2Value.ObjectID and MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid   INNER JOIN  dbo.Field  CustomField3Field  ON MetaObject.Class = CustomField3Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField3Value ON MetaObject.pkid=CustomField3Value.ObjectID and MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid   INNER JOIN  dbo.Field  DomainValueField  ON MetaObject.Class = DomainValueField.Class left outer JOIN  dbo.ObjectFieldValue DomainValueValue ON MetaObject.pkid=DomainValueValue.ObjectID and MetaObject.Machine=DomainValueValue.MachineID and DomainValueField.pkid = DomainValueValue.fieldid   INNER JOIN  dbo.Field  FormatField  ON MetaObject.Class = FormatField.Class left outer JOIN  dbo.ObjectFieldValue FormatValue ON MetaObject.pkid=FormatValue.ObjectID and MetaObject.Machine=FormatValue.MachineID and FormatField.pkid = FormatValue.fieldid   INNER JOIN  dbo.Field  GapTypeField  ON MetaObject.Class = GapTypeField.Class left outer JOIN  dbo.ObjectFieldValue GapTypeValue ON MetaObject.pkid=GapTypeValue.ObjectID and MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid   INNER JOIN  dbo.Field  NameField  ON MetaObject.Class = NameField.Class left outer JOIN  dbo.ObjectFieldValue NameValue ON MetaObject.pkid=NameValue.ObjectID and MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid   WHERE (MetaObject.Class = 'LocationUnit')  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND DomainValueField.Name ='DomainValue'  AND FormatField.Name ='Format'  AND GapTypeField.Name ='GapType'  AND NameField.Name ='Name' ";
            //databaseVersions.Add(new Guid("A214C97F-9DC2-4BA7-B72B-D23D4AC9EA51"), longQuery + "|Recreating Views");
            //longQuery = "CREATE VIEW [dbo].[METAView_LocationUnit_Retrieval] AS SELECT  MetaObject.WorkspaceName,MetaObject.WorkspaceTypeId,MetaObject.VCStatusID,MetaObject.pkid,MetaObject.Machine,VCMachineID,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,DomainValueValue.ValueString as DomainValue,FormatValue.ValueString as Format,GapTypeValue.ValueString as GapType,NameValue.ValueString as Name FROM MetaObject  INNER JOIN  dbo.Field  CustomField1Field  ON MetaObject.Class = CustomField1Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField1Value ON MetaObject.pkid=CustomField1Value.ObjectID and MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid   INNER JOIN  dbo.Field  CustomField2Field  ON MetaObject.Class = CustomField2Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField2Value ON MetaObject.pkid=CustomField2Value.ObjectID and MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid   INNER JOIN  dbo.Field  CustomField3Field  ON MetaObject.Class = CustomField3Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField3Value ON MetaObject.pkid=CustomField3Value.ObjectID and MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid   INNER JOIN  dbo.Field  DomainValueField  ON MetaObject.Class = DomainValueField.Class left outer JOIN  dbo.ObjectFieldValue DomainValueValue ON MetaObject.pkid=DomainValueValue.ObjectID and MetaObject.Machine=DomainValueValue.MachineID and DomainValueField.pkid = DomainValueValue.fieldid   INNER JOIN  dbo.Field  FormatField  ON MetaObject.Class = FormatField.Class left outer JOIN  dbo.ObjectFieldValue FormatValue ON MetaObject.pkid=FormatValue.ObjectID and MetaObject.Machine=FormatValue.MachineID and FormatField.pkid = FormatValue.fieldid   INNER JOIN  dbo.Field  GapTypeField  ON MetaObject.Class = GapTypeField.Class left outer JOIN  dbo.ObjectFieldValue GapTypeValue ON MetaObject.pkid=GapTypeValue.ObjectID and MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid   INNER JOIN  dbo.Field  NameField  ON MetaObject.Class = NameField.Class left outer JOIN  dbo.ObjectFieldValue NameValue ON MetaObject.pkid=NameValue.ObjectID and MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid   WHERE (MetaObject.Class = 'LocationUnit')  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND DomainValueField.Name ='DomainValue'  AND FormatField.Name ='Format'  AND GapTypeField.Name ='GapType'  AND NameField.Name ='Name' ";
            //databaseVersions.Add(new Guid("C1D69E82-1A47-40A0-9C26-C44C79DDBBCC"), longQuery + "|Recreating Views");
            //longQuery = "CREATE VIEW [dbo].[METAView_TimeIndicator_Listing] AS SELECT  MetaObject.WorkspaceName,MetaObject.WorkspaceTypeId,MetaObject.VCStatusID,MetaObject.pkid,MetaObject.Machine,VCMachineID,ValueValue.ValueString as Value FROM MetaObject  INNER JOIN  dbo.Field  ValueField  ON MetaObject.Class = ValueField.Class left outer JOIN  dbo.ObjectFieldValue ValueValue ON MetaObject.pkid=ValueValue.ObjectID and MetaObject.Machine=ValueValue.MachineID and ValueField.pkid = ValueValue.fieldid   WHERE (MetaObject.Class = 'TimeIndicator')  AND ValueField.Name ='Value' ";
            //databaseVersions.Add(new Guid("969E6498-66A9-4D07-804C-2B051A56E0F9"), longQuery + "|Recreating Views");
            //longQuery = "CREATE VIEW [dbo].[METAView_TimeIndicator_Retrieval] AS SELECT  MetaObject.WorkspaceName,MetaObject.WorkspaceTypeId,MetaObject.VCStatusID,MetaObject.pkid,MetaObject.Machine,VCMachineID,ValueValue.ValueString as Value FROM MetaObject  INNER JOIN  dbo.Field  ValueField  ON MetaObject.Class = ValueField.Class left outer JOIN  dbo.ObjectFieldValue ValueValue ON MetaObject.pkid=ValueValue.ObjectID and MetaObject.Machine=ValueValue.MachineID and ValueField.pkid = ValueValue.fieldid   WHERE (MetaObject.Class = 'TimeIndicator')  AND ValueField.Name ='Value' ";
            //databaseVersions.Add(new Guid("25F9B0A8-EDB8-4F6C-A75A-3F4A94BB0EEC"), longQuery + "|Recreating Views");

            //longQuery = "CREATE VIEW [dbo].[METAView_TimeReference_Listing] AS SELECT  MetaObject.WorkspaceName,MetaObject.WorkspaceTypeId,MetaObject.VCStatusID,MetaObject.pkid,MetaObject.Machine,VCMachineID,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType,NameValue.ValueString as Name FROM MetaObject  INNER JOIN  dbo.Field  CustomField1Field  ON MetaObject.Class = CustomField1Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField1Value ON MetaObject.pkid=CustomField1Value.ObjectID and MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid   INNER JOIN  dbo.Field  CustomField2Field  ON MetaObject.Class = CustomField2Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField2Value ON MetaObject.pkid=CustomField2Value.ObjectID and MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid   INNER JOIN  dbo.Field  CustomField3Field  ON MetaObject.Class = CustomField3Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField3Value ON MetaObject.pkid=CustomField3Value.ObjectID and MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid   INNER JOIN  dbo.Field  GapTypeField  ON MetaObject.Class = GapTypeField.Class left outer JOIN  dbo.ObjectFieldValue GapTypeValue ON MetaObject.pkid=GapTypeValue.ObjectID and MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid   INNER JOIN  dbo.Field  NameField  ON MetaObject.Class = NameField.Class left outer JOIN  dbo.ObjectFieldValue NameValue ON MetaObject.pkid=NameValue.ObjectID and MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid   WHERE (MetaObject.Class = 'TimeReference')  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType'  AND NameField.Name ='Name' ";
            //databaseVersions.Add(new Guid("43F265B3-A08A-4D2B-AD41-8B29AFBC2345"), longQuery + "|Recreating Views");
            //longQuery = "CREATE VIEW [dbo].[METAView_TimeReference_Retrieval] AS SELECT  MetaObject.WorkspaceName,MetaObject.WorkspaceTypeId,MetaObject.VCStatusID,MetaObject.pkid,MetaObject.Machine,VCMachineID,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3,GapTypeValue.ValueString as GapType,NameValue.ValueString as Name FROM MetaObject  INNER JOIN  dbo.Field  CustomField1Field  ON MetaObject.Class = CustomField1Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField1Value ON MetaObject.pkid=CustomField1Value.ObjectID and MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid   INNER JOIN  dbo.Field  CustomField2Field  ON MetaObject.Class = CustomField2Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField2Value ON MetaObject.pkid=CustomField2Value.ObjectID and MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid   INNER JOIN  dbo.Field  CustomField3Field  ON MetaObject.Class = CustomField3Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField3Value ON MetaObject.pkid=CustomField3Value.ObjectID and MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid   INNER JOIN  dbo.Field  GapTypeField  ON MetaObject.Class = GapTypeField.Class left outer JOIN  dbo.ObjectFieldValue GapTypeValue ON MetaObject.pkid=GapTypeValue.ObjectID and MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid   INNER JOIN  dbo.Field  NameField  ON MetaObject.Class = NameField.Class left outer JOIN  dbo.ObjectFieldValue NameValue ON MetaObject.pkid=NameValue.ObjectID and MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid  WHERE (MetaObject.Class = 'TimeReference')  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3'  AND GapTypeField.Name ='GapType'  AND NameField.Name ='Name' ";
            //databaseVersions.Add(new Guid("5077B14C-7BEC-44EE-A676-11C2D817F96F"), longQuery + "|Recreating Views");

            //longQuery = "CREATE VIEW [dbo].[METAView_TimeScheme_Listing] AS SELECT  MetaObject.WorkspaceName,MetaObject.WorkspaceTypeId,MetaObject.VCStatusID,MetaObject.pkid,MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,GapTypeValue.ValueString as GapType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3 FROM MetaObject  INNER JOIN  dbo.Field  NameField  ON MetaObject.Class = NameField.Class left outer JOIN  dbo.ObjectFieldValue NameValue ON MetaObject.pkid=NameValue.ObjectID and MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid   INNER JOIN  dbo.Field  GapTypeField  ON MetaObject.Class = GapTypeField.Class left outer JOIN  dbo.ObjectFieldValue GapTypeValue ON MetaObject.pkid=GapTypeValue.ObjectID and MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid   INNER JOIN  dbo.Field  CustomField1Field  ON MetaObject.Class = CustomField1Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField1Value ON MetaObject.pkid=CustomField1Value.ObjectID and MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid   INNER JOIN  dbo.Field  CustomField2Field  ON MetaObject.Class = CustomField2Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField2Value ON MetaObject.pkid=CustomField2Value.ObjectID and MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid   INNER JOIN  dbo.Field  CustomField3Field  ON MetaObject.Class = CustomField3Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField3Value ON MetaObject.pkid=CustomField3Value.ObjectID and MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid   WHERE (MetaObject.Class = 'TimeScheme')  AND NameField.Name ='Name'  AND GapTypeField.Name ='GapType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3' ";
            //databaseVersions.Add(new Guid("0176B0F7-09A8-4E39-8E7F-060BF50C4FDB"), longQuery + "|Recreating Views");
            //longQuery = "CREATE VIEW [dbo].[METAView_TimeScheme_Retrieval] AS SELECT  MetaObject.WorkspaceName,MetaObject.WorkspaceTypeId,MetaObject.VCStatusID,MetaObject.pkid,MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,GapTypeValue.ValueString as GapType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3 FROM MetaObject  INNER JOIN  dbo.Field  NameField  ON MetaObject.Class = NameField.Class left outer JOIN  dbo.ObjectFieldValue NameValue ON MetaObject.pkid=NameValue.ObjectID and MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid   INNER JOIN  dbo.Field  GapTypeField  ON MetaObject.Class = GapTypeField.Class left outer JOIN  dbo.ObjectFieldValue GapTypeValue ON MetaObject.pkid=GapTypeValue.ObjectID and MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid   INNER JOIN  dbo.Field  CustomField1Field  ON MetaObject.Class = CustomField1Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField1Value ON MetaObject.pkid=CustomField1Value.ObjectID and MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid   INNER JOIN  dbo.Field  CustomField2Field  ON MetaObject.Class = CustomField2Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField2Value ON MetaObject.pkid=CustomField2Value.ObjectID and MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid   INNER JOIN  dbo.Field  CustomField3Field  ON MetaObject.Class = CustomField3Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField3Value ON MetaObject.pkid=CustomField3Value.ObjectID and MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid   WHERE (MetaObject.Class = 'TimeScheme')  AND NameField.Name ='Name'  AND GapTypeField.Name ='GapType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3' ";
            //databaseVersions.Add(new Guid("BD9FFA68-47D9-4A00-AB68-45E470DF386E"), longQuery + "|Recreating Views");
            //longQuery = "CREATE VIEW [dbo].[METAView_TimeUnit_Listing] AS SELECT  MetaObject.WorkspaceName,MetaObject.WorkspaceTypeId,MetaObject.VCStatusID,MetaObject.pkid,MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,DomainValueValue.ValueString as DomainValue,FormatValue.ValueString as Format,GapTypeValue.ValueString as GapType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3 FROM MetaObject  INNER JOIN  dbo.Field  NameField  ON MetaObject.Class = NameField.Class left outer JOIN  dbo.ObjectFieldValue NameValue ON MetaObject.pkid=NameValue.ObjectID and MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid   INNER JOIN  dbo.Field  DomainValueField  ON MetaObject.Class = DomainValueField.Class left outer JOIN  dbo.ObjectFieldValue DomainValueValue ON MetaObject.pkid=DomainValueValue.ObjectID and MetaObject.Machine=DomainValueValue.MachineID and DomainValueField.pkid = DomainValueValue.fieldid   INNER JOIN  dbo.Field  FormatField  ON MetaObject.Class = FormatField.Class left outer JOIN  dbo.ObjectFieldValue FormatValue ON MetaObject.pkid=FormatValue.ObjectID and MetaObject.Machine=FormatValue.MachineID and FormatField.pkid = FormatValue.fieldid   INNER JOIN  dbo.Field  GapTypeField  ON MetaObject.Class = GapTypeField.Class left outer JOIN  dbo.ObjectFieldValue GapTypeValue ON MetaObject.pkid=GapTypeValue.ObjectID and MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid   INNER JOIN  dbo.Field  CustomField1Field  ON MetaObject.Class = CustomField1Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField1Value ON MetaObject.pkid=CustomField1Value.ObjectID and MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid   INNER JOIN  dbo.Field  CustomField2Field  ON MetaObject.Class = CustomField2Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField2Value ON MetaObject.pkid=CustomField2Value.ObjectID and MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid   INNER JOIN  dbo.Field  CustomField3Field  ON MetaObject.Class = CustomField3Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField3Value ON MetaObject.pkid=CustomField3Value.ObjectID and MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid   WHERE (MetaObject.Class = 'TimeUnit')  AND NameField.Name ='Name'  AND DomainValueField.Name ='DomainValue'  AND FormatField.Name ='Format'  AND GapTypeField.Name ='GapType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3' ";
            //databaseVersions.Add(new Guid("B52D9042-8AEE-4A2E-B3C3-39DC482C39D8"), longQuery + "|Recreating Views");
            //longQuery = "CREATE VIEW [dbo].[METAView_TimeUnit_Retrieval] AS SELECT  MetaObject.WorkspaceName,MetaObject.WorkspaceTypeId,MetaObject.VCStatusID,MetaObject.pkid,MetaObject.Machine,VCMachineID,NameValue.ValueString as Name,DomainValueValue.ValueString as DomainValue,FormatValue.ValueString as Format,GapTypeValue.ValueString as GapType,CustomField1Value.ValueString as CustomField1,CustomField2Value.ValueString as CustomField2,CustomField3Value.ValueString as CustomField3 FROM MetaObject  INNER JOIN  dbo.Field  NameField  ON MetaObject.Class = NameField.Class left outer JOIN  dbo.ObjectFieldValue NameValue ON MetaObject.pkid=NameValue.ObjectID and MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid   INNER JOIN  dbo.Field  DomainValueField  ON MetaObject.Class = DomainValueField.Class left outer JOIN  dbo.ObjectFieldValue DomainValueValue ON MetaObject.pkid=DomainValueValue.ObjectID and MetaObject.Machine=DomainValueValue.MachineID and DomainValueField.pkid = DomainValueValue.fieldid   INNER JOIN  dbo.Field  FormatField  ON MetaObject.Class = FormatField.Class left outer JOIN  dbo.ObjectFieldValue FormatValue ON MetaObject.pkid=FormatValue.ObjectID and MetaObject.Machine=FormatValue.MachineID and FormatField.pkid = FormatValue.fieldid  INNER JOIN  dbo.Field  GapTypeField  ON MetaObject.Class = GapTypeField.Class left outer JOIN  dbo.ObjectFieldValue GapTypeValue ON MetaObject.pkid=GapTypeValue.ObjectID and MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid   INNER JOIN  dbo.Field  CustomField1Field  ON MetaObject.Class = CustomField1Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField1Value ON MetaObject.pkid=CustomField1Value.ObjectID and MetaObject.Machine=CustomField1Value.MachineID and CustomField1Field.pkid = CustomField1Value.fieldid  INNER JOIN  dbo.Field  CustomField2Field  ON MetaObject.Class = CustomField2Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField2Value ON MetaObject.pkid=CustomField2Value.ObjectID and MetaObject.Machine=CustomField2Value.MachineID and CustomField2Field.pkid = CustomField2Value.fieldid   INNER JOIN  dbo.Field  CustomField3Field  ON MetaObject.Class = CustomField3Field.Class left outer JOIN  dbo.ObjectFieldValue CustomField3Value ON MetaObject.pkid=CustomField3Value.ObjectID and MetaObject.Machine=CustomField3Value.MachineID and CustomField3Field.pkid = CustomField3Value.fieldid   WHERE (MetaObject.Class = 'TimeUnit')  AND NameField.Name ='Name'  AND DomainValueField.Name ='DomainValue'  AND FormatField.Name ='Format'  AND GapTypeField.Name ='GapType'  AND CustomField1Field.Name ='CustomField1'  AND CustomField2Field.Name ='CustomField2'  AND CustomField3Field.Name ='CustomField3' ";
            //databaseVersions.Add(new Guid("E39FF970-FFD4-4A7F-B202-E5EC97A24370"), longQuery + "|Recreating Views");

            #endregion

            #endregion

            //Codebase split from here on between prod(mini changes[cascade into dev]) and dev(super changes)
            #region 22 May 2014 LocationAssociationDescriptionCode AND AddTimeDifference Artifact AND URI Table

            databaseVersions.Add(new Guid("4147D1D0-CC91-4A63-935B-D35F84C077E6"), "ALTER TABLE dbo.class ALTER COLUMN descriptioncode varchar(500) NOT NULL|Update class descriptioncode varchar(500)");
            databaseVersions.Add(new Guid("F186EE99-D568-45BB-99B3-B5478D878E25"), "UPDATE class SET descriptioncode = '(AssociationType!=null && Distance!=null && TimeIndicator!=null)?AssociationType + \",\" + Distance + \",\" + TimeIndicator:(AssociationType!=null&& Distance!=null)?AssociationType + \",\" + Distance:(AssociationType!=null&& TimeIndicator!=null)?AssociationType + \",\" + TimeIndicator:(Distance!=null&& TimeIndicator!=null)?Distance + \",\" + TimeIndicator:(AssociationType!=null)?AssociationType:(Distance!=null)?Distance:(TimeIndicator!=null)?TimeIndicator:\"\"' WHERE name = 'LocationAssociation'|LocationAssociation DescriptionCode");

            databaseVersions.Add(new Guid("C03A6B32-B67B-4BAF-8620-5652B6B0ACD2"), "exec db_AddClasses 'TimeDifference','Name','General',1|Add TimeDifference Class");
            databaseVersions.Add(new Guid("69F427AD-09F6-40C2-97AD-29DBF4C74820"), "exec db_AddFields 'TimeDifference','Name','System.String','General','Time Difference',0,1,1|Add TimeDifference Name");
            databaseVersions.Add(new Guid("11EC3C66-FF83-434C-AA82-0AB34329A811"), "IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_TimeDifference_Listing]'))DROP VIEW [dbo].[METAView_TimeDifference_Listing]|TimeDifference View");
            databaseVersions.Add(new Guid("6B7BDA41-7ECA-44BD-B61C-693E3DBE601E"), "CREATE VIEW [dbo].[METAView_TimeDifference_Listing] AS SELECT  MetaObject.WorkspaceName,MetaObject.WorkspaceTypeId,MetaObject.VCStatusID,MetaObject.pkid,MetaObject.Machine,VCMachineID,NameValue.ValueString as Name FROM MetaObject  INNER JOIN  dbo.Field  NameField  ON MetaObject.Class = NameField.Class left outer JOIN  dbo.ObjectFieldValue NameValue ON MetaObject.pkid=NameValue.ObjectID and MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid   WHERE (MetaObject.Class = 'TimeDifference')  AND NameField.Name ='Name'|TimeDifference View");
            databaseVersions.Add(new Guid("1A5B5988-9BC5-4547-A141-32D294F29976"), "IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[METAView_TimeDifference_Retrieval]'))DROP VIEW [dbo].[METAView_TimeDifference_Retrieval]|TimeDifference View");
            databaseVersions.Add(new Guid("02A4CDFB-116A-4CFF-AFE5-152F82820EBC"), "CREATE VIEW [dbo].[METAView_TimeDifference_Retrieval] AS SELECT  MetaObject.WorkspaceName,MetaObject.WorkspaceTypeId,MetaObject.VCStatusID,MetaObject.pkid,MetaObject.Machine,VCMachineID,NameValue.ValueString as Name FROM MetaObject  INNER JOIN  dbo.Field  NameField  ON MetaObject.Class = NameField.Class left outer JOIN  dbo.ObjectFieldValue NameValue ON MetaObject.pkid=NameValue.ObjectID and MetaObject.Machine=NameValue.MachineID and NameField.pkid = NameValue.fieldid   WHERE (MetaObject.Class = 'TimeDifference')  AND NameField.Name ='Name'|TimeDifference View");
            databaseVersions.Add(new Guid("5AF17F6D-6453-4077-ADC7-C550C045ED28"), "exec db_AddArtifacts 'TimeUnit','TimeUnit','Mapping','TimeDifference',1|Add TimeDifference Artifact");

            databaseVersions.Add(new Guid("22AEFB94-EDE3-48AE-A110-3E65B705A5D7"), "IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_DomainDefinitionPossibleValue_URI]') AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1) ALTER TABLE dbo.[DomainDefinitionPossibleValue] DROP CONSTRAINT [FK_DomainDefinitionPossibleValue_URI]|Drop FK URI AND DDPV tables");
            databaseVersions.Add(new Guid("9A71FEB7-ABE5-4F51-AC6F-4D6EF58FF925"), "IF OBJECT_ID('dbo.UURI', 'U') IS NOT NULL  DROP TABLE dbo.UURI|Drop UURI if exists");
            databaseVersions.Add(new Guid("97F61BC2-6C32-4E76-B4EE-F9E4A19E6F1A"), "CREATE TABLE UURI(pkid int IDENTITY(1,1),[FileName] VARCHAR(2000) NOT NULL,CONSTRAINT URI_pk PRIMARY KEY (pkid))|Create URI Table");
            databaseVersions.Add(new Guid("D1D9CA3D-2933-4346-8097-2812928EC19A"), "IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DomainDefinitionPossibleValue' AND COLUMN_NAME = 'URI_ID') BEGIN ALTER TABLE dbo.DomainDefinitionPossibleValue ADD URI_ID int NULL END|Add DDPV Column");

            //check if you already have old uriDATA
            databaseVersions.Add(new Guid("8A2E3427-0A7B-48C4-8E61-FC3715DD0C17"), "if exists (select * from domaindefinitionpossiblevalue where uri_id not in (select pkid from uuri)) update domaindefinitionpossiblevalue set uri_id = null|remove all set images");

            databaseVersions.Add(new Guid("1CB87F42-C09E-4DCC-8FD3-CCB4C7C664EC"), "IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_DomainDefinitionPossibleValue_URI]') AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1) BEGIN    ALTER TABLE [dbo].domaindefinitionpossiblevalue WITH CHECK ADD CONSTRAINT FK_DomainDefinitionPossibleValue_URI FOREIGN KEY([URI_ID]) REFERENCES [dbo].[UURI] (pkid) END|FK URI AND DDPV tables");

            #endregion

            #region 26 May 2014 Nettiers Updates

            databaseVersions.Add(new Guid("2A41C1B0-775B-406D-A933-DFF1A2DBE688"), "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_UURI_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE dbo.PROC_UURI_Get_List|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("E7B254A9-3CA8-4357-82A5-2BC086DAAE9E"), "CREATE PROCEDURE dbo.PROC_UURI_Get_List AS SELECT [pkid],[FileName] FROM [dbo].[UURI] SELECT @@ROWCOUNT|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("C10A3367-FCC3-4749-A6DD-DD7E349AD021"), "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_UURI_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE dbo.PROC_UURI_GetPaged|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("E1BBA225-25F5-4724-A75D-F9F08DCD5083"), "CREATE PROCEDURE dbo.PROC_UURI_GetPaged(@WhereClause varchar (2000)  ,@OrderBy varchar (2000)  ,@PageIndex int   ,@PageSize int) AS BEGIN DECLARE @PageLowerBound int DECLARE @PageUpperBound int SET @PageLowerBound = @PageSize * @PageIndex SET @PageUpperBound = @PageLowerBound + @PageSize CREATE TABLE #PageIndex ( [IndexId] int IDENTITY (1, 1) NOT NULL, [pkid] int )DECLARE @SQL AS nvarchar(4000) SET @SQL = 'INSERT INTO #PageIndex ([pkid])' SET @SQL = @SQL + ' SELECT' IF @PageSize > 0 BEGIN SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound) END SET @SQL = @SQL + ' [pkid]' SET @SQL = @SQL + ' FROM [dbo].[UURI]' IF LEN(@WhereClause) > 0 BEGIN SET @SQL = @SQL + ' WHERE ' + @WhereClause END IF LEN(@OrderBy) > 0 BEGIN SET @SQL = @SQL + ' ORDER BY ' + @OrderBy END EXEC sp_executesql @SQL SELECT O.[pkid], O.[FileName] FROM [dbo].[UURI] O, #PageIndex PageIndex WHERE PageIndex.IndexId > @PageLowerBound AND O.[pkid] = PageIndex.[pkid] ORDER BY PageIndex.IndexId SET @SQL = 'SELECT COUNT(*) AS TotalRowCount' SET @SQL = @SQL + ' FROM [dbo].[UURI]' IF LEN(@WhereClause) > 0 BEGIN SET @SQL = @SQL + ' WHERE ' + @WhereClause END EXEC sp_executesql @SQL END|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("69BA4E11-B5EA-43C6-BA0C-4678DAF9DD59"), "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_UURI_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE dbo.PROC_UURI_Insert|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("7A966145-8E59-4C53-84ED-B5861FF6DD7D"), "CREATE PROCEDURE dbo.PROC_UURI_Insert(@pkid int OUTPUT,@FileName varchar (2000)) AS INSERT INTO [dbo].[UURI]([FileName])VALUES(@FileName) SET @pkid = SCOPE_IDENTITY()|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("4BD5B7DD-4A2C-4857-AE02-92B9B05A3153"), "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_UURI_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE dbo.PROC_UURI_Update|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("8DDD08FC-1B2E-49F1-9C47-00C4D8679510"), "CREATE PROCEDURE dbo.PROC_UURI_Update (@pkid int,@FileName varchar (2000))AS UPDATE [dbo].[UURI] SET [FileName] = @FileName WHERE [pkid] = @pkid |Nettiers PROC Addition");
            databaseVersions.Add(new Guid("B4E60B66-F066-4D62-9F92-6CC0B61D6506"), "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_UURI_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE dbo.PROC_UURI_Delete|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("84329B15-96C8-4867-BF48-DD79C03255A1"), "CREATE PROCEDURE dbo.PROC_UURI_Delete(@pkid int)AS DELETE FROM [dbo].[UURI] WITH (ROWLOCK) WHERE [pkid] = @pkid|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("6F0D181F-7E16-4F16-90A6-86C39F3264CE"), "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_UURI_GetBypkid') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE dbo.PROC_UURI_GetBypkid|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("AF006D64-17BF-47F4-A9E9-D191FF527337"), "CREATE PROCEDURE dbo.PROC_UURI_GetBypkid (@pkid int)AS SELECT [pkid],[FileName] FROM [dbo].[UURI] WHERE [pkid] = @pkid SELECT @@ROWCOUNT|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("D4014B80-77F6-48A8-90DB-6DF8E6E469B3"), "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_UURI_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE dbo.PROC_UURI_Find|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("8D03D6EB-B6E0-4792-AD24-184BB0BC9BA9"), "CREATE PROCEDURE dbo.PROC_UURI_Find(@SearchUsingOR bit   = null ,@pkid int   = null ,@FileName varchar (2000)  = null )AS IF ISNULL(@SearchUsingOR, 0) <> 1 BEGIN SELECT  [pkid], [FileName] FROM [dbo].[UURI] WHERE ([pkid] = @pkid OR @pkid IS NULL) AND ([FileName] = @FileName OR @FileName IS NULL) END ELSE BEGIN SELECT [pkid], [FileName] FROM [dbo].[UURI] WHERE ([pkid] = @pkid AND @pkid is not null) OR ([FileName] = @FileName AND @FileName is not null) SELECT @@ROWCOUNT END|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("FE39134E-5E31-4491-B151-85BF07411F6B"), "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_DomainDefinitionPossibleValue_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_Get_List|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("4A7AFD7A-1B72-40B6-8308-1DFBE3B88B02"), "CREATE PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_Get_List AS SELECT [DomainDefinitionID], [PossibleValue], [Series], [Description], [IsActive], [URI_ID] FROM [dbo].[DomainDefinitionPossibleValue] SELECT @@ROWCOUNT|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("17C6A0A1-3AC3-42C4-95AF-C3BC5BD119DE"), "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_DomainDefinitionPossibleValue_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_GetPaged|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("A5BF3BB8-BA58-4704-B3EB-0AB50E70DEF9"), "CREATE PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_GetPaged ( @WhereClause varchar (2000)  , @OrderBy varchar (2000)  , @PageIndex int   , @PageSize int    ) AS BEGIN DECLARE @PageLowerBound int DECLARE @PageUpperBound int	 SET @PageLowerBound = @PageSize * @PageIndex SET @PageUpperBound = @PageLowerBound + @PageSize CREATE TABLE #PageIndex ( [IndexId] int IDENTITY (1, 1) NOT NULL, [DomainDefinitionID] int, [PossibleValue] varchar(50) COLLATE database_default ) DECLARE @SQL AS nvarchar(4000) SET @SQL = 'INSERT INTO #PageIndex ([DomainDefinitionID], [PossibleValue])' SET @SQL = @SQL + ' SELECT' IF @PageSize > 0 BEGIN SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound) END SET @SQL = @SQL + ' [DomainDefinitionID], [PossibleValue]' SET @SQL = @SQL + ' FROM [dbo].[DomainDefinitionPossibleValue]' IF LEN(@WhereClause) > 0 BEGIN SET @SQL = @SQL + ' WHERE ' + @WhereClause END IF LEN(@OrderBy) > 0 BEGIN SET @SQL = @SQL + ' ORDER BY ' + @OrderBy END EXEC sp_executesql @SQL SELECT O.[DomainDefinitionID], O.[PossibleValue], O.[Series], O.[Description], O.[IsActive], O.[URI_ID] FROM [dbo].[DomainDefinitionPossibleValue] O, #PageIndex PageIndex WHERE PageIndex.IndexId > @PageLowerBound AND O.[DomainDefinitionID] = PageIndex.[DomainDefinitionID] AND O.[PossibleValue] = PageIndex.[PossibleValue] ORDER BY PageIndex.IndexId SET @SQL = 'SELECT COUNT(*) AS TotalRowCount' SET @SQL = @SQL + ' FROM [dbo].[DomainDefinitionPossibleValue]' IF LEN(@WhereClause) > 0 BEGIN SET @SQL = @SQL + ' WHERE ' + @WhereClause END EXEC sp_executesql @SQL END|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("795C3BD2-139F-41DD-BEB5-66F421B29F3B"), "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_DomainDefinitionPossibleValue_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_Insert|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("3269FF5B-A868-46E4-A3D3-E13FA500DC5E"), "CREATE PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_Insert ( @DomainDefinitionID int   , @PossibleValue varchar (50)  , @Series int   , @Description varchar (120)  , @IsActive bit   , @URI_ID int   ) AS INSERT INTO [dbo].[DomainDefinitionPossibleValue] ( [DomainDefinitionID] ,[PossibleValue] ,[Series] ,[Description] ,[IsActive] ,[URI_ID] ) VALUES ( @DomainDefinitionID ,@PossibleValue ,@Series ,@Description ,@IsActive ,@URI_ID )|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("2BD52FDB-254A-4E0B-97A4-29D4434AD48D"), "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_DomainDefinitionPossibleValue_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_Update|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("BCAE48AC-0E27-488F-BCED-2BDADEA0A45D"), "CREATE PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_Update ( @DomainDefinitionID int   , @OriginalDomainDefinitionID int   , @PossibleValue varchar (50)  , @OriginalPossibleValue varchar (50)  , @Series int   , @Description varchar (120)  , @IsActive bit   , @URI_ID int) AS UPDATE [dbo].[DomainDefinitionPossibleValue] SET [DomainDefinitionID] = @DomainDefinitionID ,[PossibleValue] = @PossibleValue ,[Series] = @Series ,[Description] = @Description ,[IsActive] = @IsActive ,[URI_ID] = @URI_ID WHERE [DomainDefinitionID] = @OriginalDomainDefinitionID  AND [PossibleValue] = @OriginalPossibleValue |Nettiers PROC Addition");
            databaseVersions.Add(new Guid("7F4AB518-5E90-4BC7-8294-B4B37E41154C"), "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_DomainDefinitionPossibleValue_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_Delete|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("6D3CE34F-7B51-4EFF-B7CE-D8F78C4BE0DA"), "CREATE PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_Delete ( @DomainDefinitionID int   , @PossibleValue varchar (50)   ) AS DELETE FROM [dbo].[DomainDefinitionPossibleValue] WITH (ROWLOCK)  WHERE [DomainDefinitionID] = @DomainDefinitionID AND [PossibleValue] = @PossibleValue|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("1DDF9714-1740-4DAE-BCA7-2FFC37B191F3"), "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_DomainDefinitionPossibleValue_GetByDomainDefinitionID') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_GetByDomainDefinitionID|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("A1D52F71-30E5-4161-B20C-04140E3F155A"), "CREATE PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_GetByDomainDefinitionID ( 	@DomainDefinitionID int   ) AS SET ANSI_NULLS OFF	SELECT [DomainDefinitionID], [PossibleValue], [Series], [Description], [IsActive], [URI_ID] FROM [dbo].[DomainDefinitionPossibleValue] WHERE [DomainDefinitionID] = @DomainDefinitionID SELECT @@ROWCOUNT|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("BE914D31-F095-42B0-B7BD-90D1FFCB58C1"), "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_DomainDefinitionPossibleValue_GetByURI_ID') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_GetByURI_ID|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("866D3745-68CD-41B5-A4F6-0AD9C9FE55FB"), "CREATE PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_GetByURI_ID ( @URI_ID int) AS SET ANSI_NULLS OFF	 SELECT [DomainDefinitionID], [PossibleValue], [Series], [Description], [IsActive], [URI_ID] FROM [dbo].[DomainDefinitionPossibleValue] WHERE [URI_ID] = @URI_ID SELECT @@ROWCOUNT|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("B4C72EF8-A38B-4CA8-B963-FFAC82E8DED2"), "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_DomainDefinitionPossibleValue_GetByDomainDefinitionIDPossibleValue') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_GetByDomainDefinitionIDPossibleValue|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("D34B87A5-0E50-4FEA-BA86-DA670F469B4B"), "CREATE PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_GetByDomainDefinitionIDPossibleValue ( @DomainDefinitionID int   , @PossibleValue varchar (50) ) AS SELECT [DomainDefinitionID], [PossibleValue], [Series], [Description], [IsActive], [URI_ID] FROM [dbo].[DomainDefinitionPossibleValue] WHERE [DomainDefinitionID] = @DomainDefinitionID AND [PossibleValue] = @PossibleValue SELECT @@ROWCOUNT|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("E8DB74AA-3F1A-467F-939A-90D490953B80"), "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_DomainDefinitionPossibleValue_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_Find|Nettiers PROC Addition");
            databaseVersions.Add(new Guid("4AAF0E06-F5FE-4A30-8FCD-2D018896C9FF"), "CREATE PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_Find ( @SearchUsingOR bit   = null , @DomainDefinitionID int   = null , @PossibleValue varchar (50)  = null , @Series int   = null , @Description varchar (120)  = null , @IsActive bit   = null , @URI_ID int   = null  ) AS IF ISNULL(@SearchUsingOR, 0) <> 1 BEGIN SELECT  [DomainDefinitionID] , [PossibleValue] , [Series] , [Description] , [IsActive] , [URI_ID]  FROM [dbo].[DomainDefinitionPossibleValue]  WHERE  ([DomainDefinitionID] = @DomainDefinitionID OR @DomainDefinitionID IS NULL) AND ([PossibleValue] = @PossibleValue OR @PossibleValue IS NULL) AND ([Series] = @Series OR @Series IS NULL) AND ([Description] = @Description OR @Description IS NULL) AND ([IsActive] = @IsActive OR @IsActive IS NULL) AND ([URI_ID] = @URI_ID OR @URI_ID IS NULL) END ELSE  BEGIN SELECT  [DomainDefinitionID] , [PossibleValue] , [Series] , [Description] , [IsActive] , [URI_ID] FROM [dbo].[DomainDefinitionPossibleValue] WHERE  ([DomainDefinitionID] = @DomainDefinitionID AND @DomainDefinitionID is not null) OR ([PossibleValue] = @PossibleValue AND @PossibleValue is not null) OR ([Series] = @Series AND @Series is not null) OR ([Description] = @Description AND @Description is not null) OR ([IsActive] = @IsActive AND @IsActive is not null) OR ([URI_ID] = @URI_ID AND @URI_ID is not null) SELECT @@ROWCOUNT END|Nettiers PROC Addition");

            #endregion

            #region Major Metamodel Changes SAPPI ONLY

            databaseVersions.Add(new Guid("2F348442-1D02-444A-8A0C-02EED25FC370"), "IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[db_AddFields]') AND type in (N'P', N'PC')) DROP PROCEDURE [dbo].[db_AddFields]|Drop db_addFields");
            databaseVersions.Add(new Guid("E3DA96E5-51DE-4996-97CB-052F46C22A22"), "CREATE PROCEDURE [dbo].[db_AddFields] (@Class varchar(50),@Name varchar(50),@DataType varchar(50),@Category varchar(50),@Description varchar(120),@IsUnique bit,@SortOrder int,@IsActive bit) AS BEGIN IF (@Category = '') OR (@Category IS NULL) SET @Category = (SELECT TOP(1) Category FROM Class WHERE ([Name] = @Class)) IF (@Category = '') OR (@Category IS NULL) SET @Category = 'Primary' ELSE IF (@Description = 'Custom Field 1') OR (@Description = 'Custom Field 2') OR (@Description = 'Custom Field 3') SET @Category = 'User Fields' IF (@IsUnique = '') OR (@IsUnique IS NULL) SET @IsUnique = 0 IF EXISTS (SELECT TOP (1) * FROM Field WHERE (Class = @Class) AND ([Name] = @Name)) UPDATE Field SET [Class] = @Class,[Name] = @Name,[DataType] = @DataType,[Category] = @Category,[Description] = @Description,[IsUnique] = @IsUnique,[SortOrder] = @SortOrder,[IsActive] = @IsActive WHERE ((Class = @Class) AND ([Name] = @Name)) ELSE INSERT INTO Field ([Class],[Name],[DataType],[Category],[Description],[IsUnique],[SortOrder],[IsActive]) VALUES (@Class,@Name,@DataType,@Category,@Description,@IsUnique,@SortOrder,@IsActive) END|update db_addFields");

            databaseVersions.Add(new Guid("5738DADC-2315-453B-9688-D9DAC4DDD422"), "EXEC db_AddPossibleValues 'ComputingComponentType','Laptop',1,'Laptop',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AED2CC80-32AF-4822-9F5F-0B1F656DD2BA"), "EXEC db_AddPossibleValues 'ComputingComponentType','Mainframe',2,'Mainframe',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("596838D3-C969-4DA7-8F47-4AE6875DC3B6"), "EXEC db_AddPossibleValues 'ComputingComponentType','PhysicalServer',1,'Physical Server',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B8051E4C-E229-493A-A5A0-F4C589A3D364"), "EXEC db_AddPossibleValues 'ComputingComponentType','VirtualServer',2,'Virtual Server',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EB70BB1B-2DD7-4BD0-9BD3-7EE799FDD630"), "EXEC db_AddPossibleValues 'ComputingComponentType','Workstation',3,'Workstation',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("63B08F2A-B4FC-4AD4-85D5-A4376411C753"), "EXEC db_AddPossibleValues 'ContentType','TransactionalData',1,'Transactional Data',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7B5E0372-FF29-4BFF-AB5B-AB8C262A5062"), "EXEC db_AddPossibleValues 'ContentType','AnalyticalData',2,'Analytical Data',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A388477E-04FE-4C33-8B3D-65920DA8C3BB"), "EXEC db_AddPossibleValues 'ContentType','MultiMediaData',3,'MultiMedia Data',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("793744EF-6D25-4665-8D0A-E5E3A2070B8A"), "EXEC db_AddPossibleValues 'ContentType','Metadata',4,'Metadata',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A01CAD4D-240A-4F41-A16E-A3A2DBDB038D"), "EXEC db_AddPossibleValues 'DatabaseType','FlatFile',1,'Flat File',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FF3E1360-DEFB-4167-91A2-C251E7DEF679"), "EXEC db_AddPossibleValues 'DatabaseType','HierarchicalDatabase',2,'Hierarchical Database',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("53A8D291-286B-4612-B050-7E6CD760736A"), "EXEC db_AddPossibleValues 'DatabaseType','DimensionalModel',3,'Dimensional Model',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B6A81A55-2B73-4BE5-BCCC-CC629C8090A5"), "EXEC db_AddPossibleValues 'DatabaseType','GraphDatabase',4,'Graph Database',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6D42651E-A380-40A5-BD88-549B21C24A10"), "EXEC db_AddPossibleValues 'DatabaseType','NetworkDatabase',5,'Network Database',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DAE5134E-A525-4E91-AEEE-B49FD4A97AB6"), "EXEC db_AddPossibleValues 'DatabaseType','ObjectorientedDatabase',6,'Object Oriented Database',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2FB7EA3E-464F-4582-A7D4-902412F3D986"), "EXEC db_AddPossibleValues 'DatabaseType','RelationalDatabase',7,'Relational Database',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5EBAD2A7-6390-4184-8D87-30D8092E0DFE"), "EXEC db_AddPossibleValues 'DatabaseType','ArrayDatabase',8,'Array Database',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("11846392-74E2-4069-B05E-F039B9CCDB84"), "EXEC db_AddPossibleValues 'DatabaseType','XMLDatabase',9,'XML Database',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("601990AD-6F05-4C4B-B558-9B4141F14661"), "EXEC db_AddPossibleValues 'DataComponentType','Database',1,'Database',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("983CA0B5-A26D-40C4-BB8A-730522DD86B7"), "EXEC db_AddPossibleValues 'DataComponentType','DataWarehouse',2,'Data Warehouse',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AACA5E18-9A30-4C3B-BED5-C398F4DEFE8F"), "EXEC db_AddPossibleValues 'DataComponentType','DataMart',3,'DataMart',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BEA881E5-C730-4CE2-A203-BE03BE59D45E"), "EXEC db_AddPossibleValues 'DataComponentType','ODS',4,'Operational Data Store',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("016D96D5-A682-483D-B07D-C2A5E1C71CB5"), "EXEC db_AddPossibleValues 'DataComponentType','InMemoryDB',5,'In Memory Database',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D2E85DB3-744C-49CB-8F53-1F25CEC58ECA"), "EXEC db_AddPossibleValues 'DataComponentType','Spreadsheet',6,'Spreadsheet',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B7F270D5-B536-456C-A5E3-289E2EE9578B"), "EXEC db_AddPossibleValues 'DataStorageNetworkType','LocalDataStorage',1,'Local Data Storage',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("99CE5C39-3162-4183-B288-2D04B90F6E98"), "EXEC db_AddPossibleValues 'DataStorageNetworkType','DAS',2,'DAS',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("256C23C7-438F-4A9E-8D3A-E1210968B18C"), "EXEC db_AddPossibleValues 'DataStorageNetworkType','SAN',3,'SAN',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("603367B8-ACED-424A-A1B7-4BBF3FDB4817"), "EXEC db_AddPossibleValues 'DataStorageNetworkType','NAS',4,'NAS',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A7944821-5E28-4136-85D3-EBAA4CBEDC67"), "EXEC db_AddPossibleValues 'GapType','Reuse',1,'Reuse',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("48D01EED-8A8D-4359-ACB0-90A082DDFABB"), "EXEC db_AddPossibleValues 'GapType','Change',2,'Change',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4A1A8FEC-2E88-4BFD-9B94-F855CFAF46FA"), "EXEC db_AddPossibleValues 'GapType','Remove',3,'Remove',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C8908A93-46EC-4D81-8B3F-F77BC2B6C256"), "EXEC db_AddPossibleValues 'GapType','Add',4,'Add',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("110465F6-E302-4693-97E1-1ED01198F899"), "EXEC db_AddPossibleValues 'IsBusinessExternal','Yes',1,'Yes',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AE7F753C-8FB4-45BF-B27A-5DA48024113C"), "EXEC db_AddPossibleValues 'IsBusinessExternal','No',2,'No',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0C955E00-6AC7-42F8-B472-C72F981955B8"), "EXEC db_AddPossibleValues 'IsPrimaryOrBackup','PrimaryConnection',1,'Primary Connection',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FF6A5E78-ECDB-4F08-9222-9430391A212B"), "EXEC db_AddPossibleValues 'IsPrimaryOrBackup','BackupConnection',2,'Backup Connection',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AA43C5F4-617F-46CC-A688-94F66EC8D174"), "EXEC db_AddPossibleValues 'ITEnvironmentType','Dev',1,'Dev',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("11548784-A7F4-4D54-9488-35EFF3F6DC35"), "EXEC db_AddPossibleValues 'ITEnvironmentType','Test',2,'Test',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7C9202BF-8E16-465F-8312-05261F078DDB"), "EXEC db_AddPossibleValues 'ITEnvironmentType','Prod',3,'Prod',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("31828B72-839E-4FB1-81C9-EE140B215FC5"), "EXEC db_AddPossibleValues 'ITEnvironmentType','DR',4,'DR',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("660DA825-8DF9-4AAA-87A5-01EA49625BF5"), "EXEC db_AddPossibleValues 'NetworkComponentType','Hub',1,'Hub',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0BAA3EF5-5B19-4CFF-9F40-8F0D180980CD"), "EXEC db_AddPossibleValues 'NetworkComponentType','Network',2,'Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9F5CD3FE-7CFB-49DA-9FD6-FA8606CCE0A8"), "EXEC db_AddPossibleValues 'NetworkComponentType','PatchPanel',3,'Patch Panel',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("20A1AB94-0470-4274-93A0-F0F14F3D6A36"), "EXEC db_AddPossibleValues 'NetworkComponentType','Router',4,'Router',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0517FF95-8D05-4685-980A-72A24486B6F3"), "EXEC db_AddPossibleValues 'NetworkComponentType','Satellite',5,'Satellite',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3C001128-BC05-4274-9F47-7B0615B6701B"), "EXEC db_AddPossibleValues 'NetworkComponentType','Switch',6,'Switch',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D59ADB2A-083E-4C53-A405-28305BE67D34"), "EXEC db_AddPossibleValues 'NetworkComponentType','WAP',7,'WAP',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5C2935F2-9F90-45C7-A976-5BE94FAEE804"), "EXEC db_AddPossibleValues 'NetworkComponentType','Firewall',8,'Firewall',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D7528AD0-E9DF-4667-B7F9-0CDD556F0DB9"), "EXEC db_AddPossibleValues 'NetworkComponentType','Bridge',9,'Bridge',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EF2524DE-B673-42FF-93B4-F0D6533BD01D"), "EXEC db_AddPossibleValues 'NetworkType','LAN',1,'LAN',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("47DD5557-2B52-4E42-9C49-D09C06BDC70E"), "EXEC db_AddPossibleValues 'NetworkType','VLAN',2,'VLAN',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("160C28D2-7D6F-4EC5-BEE4-3D1149F5E0C0"), "EXEC db_AddPossibleValues 'NetworkType','VPN',3,'VPN',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("78A302D7-6009-4526-B653-DC32626C8FEF"), "EXEC db_AddPossibleValues 'NetworkType','WAN',4,'WAN',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3BCEDDC7-7C5F-4D24-918E-77537054D2F0"), "EXEC db_AddPossibleValues 'NetworkType','Internet',5,'Internet',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DE319EC4-C3CF-47CC-BD57-D4494B9472C0"), "EXEC db_AddPossibleValues 'NetworkType','WirelessNetwork',6,'Wireless Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0CBAC8EF-4B06-4BE5-9B95-DA80F37CCD35"), "EXEC db_AddPossibleValues 'NetworkType','IAN',7,'IAN',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("758EC534-48EF-42F1-999C-01F9894DA85E"), "EXEC db_AddPossibleValues 'NetworkType','CloudNetwork',8,'Cloud Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7006EC26-DA24-47C7-8268-6B3FEC6B500B"), "EXEC db_AddPossibleValues 'NetworkType','PAN',9,'PAN',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("30634959-F5FC-4B59-A04A-DFDC7B8BDEE0"), "EXEC db_AddPossibleValues 'NetworkType','MAN',10,'MAN',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7259828A-36ED-4FC1-88A5-5A0C80AE99E9"), "EXEC db_AddPossibleValues 'ServerType','ApplicationServer',1,'Application Server',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("69A050D7-2550-42A1-BD5A-67F0B8FF1F45"), "EXEC db_AddPossibleValues 'ServerType','DomainControllerServer',2,'Domain Controller Server',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7C99C0FD-12B3-45EE-BEEE-784F4939601F"), "EXEC db_AddPossibleValues 'ServerType','FileServer',3,'File Server',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("11A0957E-0585-4318-AACC-2FC7A6B2D935"), "EXEC db_AddPossibleValues 'ServerType','FirewallServer',4,'Firewall Server',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DDFCE4C4-388B-45F1-9D32-6C9CAFD26473"), "EXEC db_AddPossibleValues 'ServerType','InterfaceServer',5,'Interface Server',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7714A7DB-FBC6-48B7-A66C-61BEF6DD9D6E"), "EXEC db_AddPossibleValues 'ServerType','MailServer',6,'Mail Server',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D424FA33-8109-4074-850A-1FD21FF76CF8"), "EXEC db_AddPossibleValues 'ServerType','MultiServiceServer',7,'Multi Service Server',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6CD1CE2B-A0C4-40EB-B3B3-6BF8C76FFA94"), "EXEC db_AddPossibleValues 'ServerType','WebServer',8,'Web Server',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("28A73556-512B-47EA-ABE9-2D468260F09D"), "EXEC db_AddPossibleValues 'ServerType','DatabaseServer',9,'Database Server',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A4ADA543-10A4-415D-9AE9-FA9A04C84BBA"), "EXEC db_AddPossibleValues 'ServerType','CommunicationServer',10,'Communication Server',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8703FCCA-3032-4FE2-8CC3-689806F400E7"), "EXEC db_AddPossibleValues 'ServerType','ComputeServer',11,'Compute Server',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A2CACEFB-1B95-49F0-83EE-934B313EF8B9"), "EXEC db_AddPossibleValues 'ServerType','FaxServer',12,'Fax Server',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D594BFD0-4C58-4E76-BC8D-7631BBBABCFC"), "EXEC db_AddPossibleValues 'ServerType','ProxyServer',13,'Proxy Server',1|MAJOR METAMODEL CHANGE SAPPI ONLY");

            databaseVersions.Add(new Guid("BDA2A906-A411-4B50-A6CC-3BEF06E1E384"), "EXEC db_AddPossibleValues 'SoftwareLevel','Suite',1,'Suite',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4F0F432A-6A74-4E49-9193-F3112D934320"), "EXEC db_AddPossibleValues 'SoftwareLevel','Application',2,'Application',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FAD68D69-B053-4506-AC86-621DD48F7D78"), "EXEC db_AddPossibleValues 'SoftwareLevel','Module',3,'Module',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D432460E-6020-414B-A1BC-D4BCCF93C022"), "EXEC db_AddPossibleValues 'SoftwareType','Application',1,'Application',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C976D35B-CA07-49B9-8AB2-52F3B209D39A"), "EXEC db_AddPossibleValues 'SoftwareType','AppMod',0,'AppMod',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7AEA650C-FC00-4300-AE9B-37464127A2A8"), "EXEC db_AddPossibleValues 'SoftwareType','Bespoke',0,'Bespoke',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("81F2F06B-2E1B-42A6-969E-F15EF410A438"), "EXEC db_AddPossibleValues 'SoftwareType','DBMS',2,'DBMS',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("97879E3A-FF28-422E-8C7E-607C1AA64C2F"), "EXEC db_AddPossibleValues 'SoftwareType','Driver',3,'Driver',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EF4081C7-9F80-41D1-8E96-777A9136495C"), "EXEC db_AddPossibleValues 'SoftwareType','Executable',0,'Executable',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("41DB5597-6BB5-4148-ABE4-5362204B48C6"), "EXEC db_AddPossibleValues 'SoftwareType','Firewall',4,'Firewall',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("361F9417-731D-4035-8B6C-C1B8C072474E"), "EXEC db_AddPossibleValues 'SoftwareType','Informal',0,'Informal',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9D213BD1-77E9-40DA-9E84-E7DE2DFCDD72"), "EXEC db_AddPossibleValues 'SoftwareType','OS',5,'OS',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B47924F2-8E7A-4C83-896F-9E5B7118BBE7"), "EXEC db_AddPossibleValues 'SoftwareType','Package',10,'Package',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("47CDF968-40C7-4400-8CF7-AC615B9D9CD7"), "EXEC db_AddPossibleValues 'SoftwareType','Patch',6,'Patch',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D1E5FE62-24CA-4C9F-91B4-9250AF4376FE"), "EXEC db_AddPossibleValues 'SoftwareType','Script',7,'Script',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FE8E36DB-0D5D-4F2C-AF7C-60DC57F8B03D"), "EXEC db_AddPossibleValues 'SoftwareType','ServicePack',0,'Service Pack',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7DDDC58B-01F9-410E-8DAB-3B1897D1A8C4"), "EXEC db_AddPossibleValues 'SoftwareType','Suite',0,'Suite',0|MAJOR METAMODEL CHANGE SAPPI ONLY");

            databaseVersions.Add(new Guid("E058E81C-F026-408C-BF10-552204776B00"), "EXEC db_AddPossibleValues 'StandardisationStatus','NonStandard',1,'Non Standard',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E2D33D03-A192-463E-B449-91D827CDEB83"), "EXEC db_AddPossibleValues 'StandardisationStatus','Evaluating',2,'Evaluating',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("77EF810A-70D4-477A-8EA2-6E396FD07405"), "EXEC db_AddPossibleValues 'StandardisationStatus','Phasingin',3,'Phasing In',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C2A43BD0-826D-4B9C-AB74-93DDC4CD76E0"), "EXEC db_AddPossibleValues 'StandardisationStatus','Contained',4,'Contained',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3660B691-DF3B-4243-88DC-394D07319712"), "EXEC db_AddPossibleValues 'StandardisationStatus','Standard',5,'Standard',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DF7F1295-0670-4878-B29E-1BF636E5A153"), "EXEC db_AddPossibleValues 'StandardisationStatus','StandardPhasingout',6,'Standard Phasing Out',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6E024BA2-EB14-4D17-AF12-556D0A61E812"), "EXEC db_AddPossibleValues 'StandardisationStatus','Retired',7,'Retired',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8CFD8933-9BB9-45A7-9DA5-66B49871E9BC"), "EXEC db_AddPossibleValues 'StorageComponentType','LogicalStorage',1,'Logical Unit (LUN)',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9DE5AABF-71A8-4263-9D80-47FFD4B0DBC7"), "EXEC db_AddPossibleValues 'StorageComponentType','PhysicalStorage',2,'Physical Storage Component',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0933FFA5-47B8-4874-BFA2-023AD4C05576"), "EXEC db_AddPossibleValues 'StorageComponentType','TapeDrive',3,'Tape Drive',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("45A62C83-127D-4DD5-ADBE-3A340AA67035"), "EXEC db_AddPossibleValues 'StorageComponentType','OpticalDiskDrive',4,'Optical Disk Drive',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CB2FCDC8-7875-417F-B61C-F4D28223E902"), "EXEC db_AddPossibleValues 'StorageComponentType','HDD',5,'HDD',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CB4057E5-2776-4F4F-A51F-0786401D78B9"), "EXEC db_AddPossibleValues 'StorageComponentType','USBFlashDrive',6,'USB Flash Drive',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("66D36108-E362-4721-B11F-CF6759A356BB"), "EXEC db_AddPossibleValues 'StorageComponentType','BackupDevice',7,'Backup Device',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C89B86F6-C554-4E03-9383-9887FEFB84C1"), "EXEC db_AddPossibleValues 'ComputingComponentType','PhysServer',1,'Physical Server',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("8FAA5A0E-18F5-4B8E-9533-E45287E682F0"), "EXEC db_AddPossibleValues 'ComputingComponentType','VirtualServer',2,'Virtual Server',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("ADD725D0-77A0-473A-B725-5BC4BCDFCCF9"), "EXEC db_AddPossibleValues 'ContentType','Trans',1,'Transactional Data',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9603C4B4-7991-4C5A-8E5F-16325921C681"), "EXEC db_AddPossibleValues 'ContentType','Mng',2,'Analytical Data',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B183089A-7A90-43D5-B9F8-460E9BF3D418"), "EXEC db_AddPossibleValues 'DatabaseType','Hierarchy',2,'Hierarchical Database',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("98C62721-5767-4449-B8C5-F630A43B1A01"), "EXEC db_AddPossibleValues 'DatabaseType','Network',5,'Network Database',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("539CF5AA-AA73-4B10-A315-784353444CAE"), "EXEC db_AddPossibleValues 'DatabaseType','ODBMS',6,'Object-oriented Database',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7EEC5E2A-3318-40A7-B8F9-AB8E3AE24596"), "EXEC db_AddPossibleValues 'DatabaseType','RDBMS',7,'Relational Database',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6812C897-77D8-4594-B8BB-870B32BB848D"), "EXEC db_AddPossibleValues 'DatabaseType','XML',9,'XML Database',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("070D625C-22F3-4483-842E-361AF53DCAFB"), "EXEC db_AddPossibleValues 'GapType','As_Is',1,'Reuse',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BB827239-D468-4484-B43E-B8656089D437"), "EXEC db_AddPossibleValues 'GapType','Fix',2,'Change',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C5681B5C-FF6F-465F-B7C7-C07491D229E0"), "EXEC db_AddPossibleValues 'GapType','Obsolete',3,'Remove',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F92CEED0-54A8-4EBA-9B7B-FF80A6E3C256"), "EXEC db_AddPossibleValues 'GapType','To_Be',4,'Add',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4C595742-AC2D-4478-822F-8E8EF47A382E"), "EXEC db_AddPossibleValues 'ServerType','Appl',1,'Application Server',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("900505EB-7093-492B-AED5-E12211AE367E"), "EXEC db_AddPossibleValues 'ServerType','DomainC',2,'Domain Controller Server',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B3BE147A-05D3-4981-9A96-A643AE2A1BDE"), "EXEC db_AddPossibleValues 'ServerType','FilePrint',3,'FileServer',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("83D7DE05-C1D6-4F15-ADC3-9878B08D305E"), "EXEC db_AddPossibleValues 'ServerType','Firewall',4,'Firewall Server',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B8275E8A-7A3E-43FA-9550-B738A4230290"), "EXEC db_AddPossibleValues 'ServerType','Interface',5,'Interfac eServer',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("63A526EB-AF3B-4405-88B2-ACC4A0395EA8"), "EXEC db_AddPossibleValues 'ServerType','Mail',6,'Mail Server',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F5C2ACA0-1CF8-4469-A8D9-88A940B4929F"), "EXEC db_AddPossibleValues 'ServerType','Multi',7,'Multi Service Server',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A686F1A6-9150-4A26-962B-C39463A533FE"), "EXEC db_AddPossibleValues 'ServerType','Web',8,'Web Server',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8A5FE7F9-EAB9-4B57-9CE5-46B7BC5146BD"), "EXEC db_AddPossibleValues 'SoftwareType','App',1,'Application',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("6C39C4AC-BD1A-43A9-88F0-2B13D65B1D47"), "EXEC db_AddPossibleValues 'StorageComponentType','LogicalStorage',1,'Logical Unit',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("DD0990F0-D9E7-4669-B65E-61D7ADA03820"), "EXEC db_AddPossibleValues 'StorageComponentType','PhysicalStorage',2,'Physical Storage Component',0|MAJOR METAMODEL CHANGE SAPPI ONLY");

            databaseVersions.Add(new Guid("E72C989D-3215-40DE-84C0-DF6A8FB0D318"), "EXEC db_AddPossibleValues 'ConnectionType','CopperCable',1,'Copper Cable',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("00C9BFDD-FE0D-4248-83A8-F4C037798164"), "EXEC db_AddPossibleValues 'ConnectionType','FibreOpticCable',2,'Fibre Optic Cable',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("01807064-BB0F-48EF-9999-07DD1E8F3223"), "EXEC db_AddPossibleValues 'ConnectionType','Wireless',3,'Wireless',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E9A2D3E6-26AA-4859-B314-33B98D9881CA"), "EXEC db_AddPossibleValues 'ConnectionType','Satellite',4,'Satellite',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("97BC5340-F897-47C1-AAB5-BB6664F01B4B"), "EXEC db_AddPossibleValues 'ConnectionType','Cellular',5,'Cellular',1|MAJOR METAMODEL CHANGE SAPPI ONLY");

            databaseVersions.Add(new Guid("D447FF00-F1DF-4AF9-AD35-09ACB2C705E9"), "IF NOT EXISTS (SELECT Name FROM Class WHERE Name= 'PhysicalSoftwareComponent')INSERT INTO CLASS (Name,DescriptionCode,Category,ClassType,IsActive) VALUES ('PhysicalSoftwareComponent','Name','Application','Primary',1)|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("24983AB7-CD07-43A7-9188-4FCB9E2FC6DF"), "IF NOT EXISTS (SELECT Name FROM Class WHERE Name= 'LogicalITInfrastructureComponent')INSERT INTO CLASS (Name,DescriptionCode,Category,ClassType,IsActive) VALUES ('LogicalITInfrastructureComponent','Name','ITInfrastructure','Primary',1)|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1B873770-CCD4-45B0-8DA1-AD1A60769934"), "IF NOT EXISTS (SELECT Name FROM Class WHERE Name= 'ComputingComponent')INSERT INTO CLASS (Name,DescriptionCode,Category,ClassType,IsActive) VALUES ('ComputingComponent','Name','ITInfrastructure','Primary',1)|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("1EFEB949-B77D-4A06-BA50-9AB17AF24BF6"), "INSERT INTO CLASS (Name,DescriptionCode,Category,ClassType,IsActive) VALUES ('StorageComponent','Name','ITInfrastructure','Primary',1)|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("7D1542D7-2023-44AE-87CF-3B7DE7B06013"), "INSERT INTO CLASS (Name,DescriptionCode,Category,ClassType,IsActive) VALUES ('NetworkComponent','Name','ITInfrastructure','Primary',1)|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("915D65ED-FE1E-470C-AA59-57D4D8CC6126"), "IF NOT EXISTS (SELECT Name FROM Class WHERE Name= 'PeripheralComponent')INSERT INTO CLASS (Name,DescriptionCode,Category,ClassType,IsActive) VALUES ('PeripheralComponent','Name','ITInfrastructure','Primary',1)|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("16F5E4FA-B480-4B0E-9DD0-5C9389DDF1E8"), "IF NOT EXISTS (SELECT Name FROM Class WHERE Name= 'ITInfrastructureEnvironment')INSERT INTO CLASS (Name,DescriptionCode,Category,ClassType,IsActive) VALUES ('ITInfrastructureEnvironment','Name','ITInfrastructure','Primary',1)|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B2D18050-3AB9-4787-A9CC-83642963D957"), "IF NOT EXISTS (SELECT Name FROM Class WHERE Name= 'Network')INSERT INTO CLASS (Name,DescriptionCode,Category,ClassType,IsActive) VALUES ('Network','Name','ITInfrastructure','Primary',1)|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CD449F51-FE8A-4798-93E8-1CAA6180FE2E"), "UPDATE Class SET IsActive = 0 where Name = 'Software'|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("047A8909-B931-4373-A860-F12234BA6C44"), "UPDATE Class SET IsActive = 0 where Name = 'SystemComponent'|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BE90E4FB-6E5C-4227-9E8C-409915A13213"), "UPDATE Class SET IsActive = 0 where Name = 'Peripheral'|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3067F20C-612C-4914-9CC3-A1806F9EFFB5"), "UPDATE Class SET IsActive = 0 where Name = 'ConnectionSpeed'|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("332AB8BB-82F0-4FC9-84B9-F8C1260025A4"), "UPDATE Class SET IsActive = 0 where Name = 'ConnectionSize'|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0C75B492-F119-460A-A7E7-526A02A24C34"), "UPDATE Class SET IsActive = 0 where Name = 'ConnectionType'|MAJOR METAMODEL CHANGE SAPPI ONLY");

            databaseVersions.Add(new Guid("A4D6FA21-4FC3-4395-AC17-6593B68FF5A9"), "EXEC db_AddClasses 'PhysicalDataComponent','Name','ITInfrastructure',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("97E9319B-3390-4906-A6D9-7941E3F8DA02"), "EXEC db_AddFields 'PhysicalDataComponent','DataComponentType','DataComponentType','General','',0,1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D8BB63DA-1D05-4E39-9F8D-984C406D670D"), "EXEC db_AddFields 'PhysicalDataComponent','DatabaseType','DatabaseType','General','',0,2,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D368BED7-423C-4AEB-8DF7-2FC8075BEAA8"), "EXEC db_AddFields 'PhysicalDataComponent','SecurityClassification','SecurityClassification','General','',0,3,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("480B0261-91F8-48A2-8BB0-0AE0D963534C"), "EXEC db_AddFields 'PhysicalDataComponent','ContentType','ContentType','General','',0,4,1|MAJOR METAMODEL CHANGE SAPPI ONLY");

            databaseVersions.Add(new Guid("5980762B-FAE7-4D08-8E69-C75FA1848363"), "EXEC db_AddFields 'ITInfrastructureEnvironment','ITEnvironmentType','ITEnvironmentType','General','- Packaging of hardware and software at a location to form a software or data hosting environment. - Managed and referred to as a unit.',0,1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F0012AA0-5DF7-4AE6-A24F-7C28EB2C1F2A"), "EXEC db_AddFields 'PhysicalSoftwareComponent','InternalName','System.String','General','',0,1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("523D4763-7C34-45D6-B988-6FC8FE0DF8E8"), "EXEC db_AddFields 'PhysicalSoftwareComponent','ConfigurationID','System.String','General','',0,2,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FA9A3FB2-834C-410C-AECA-B7D7F3A25F40"), "EXEC db_AddFields 'PhysicalSoftwareComponent','SoftwareType','SoftwareType','General','',0,1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4983EF87-AE13-4902-828A-FCDDD5D2EC91"), "EXEC db_AddFields 'PhysicalSoftwareComponent','SoftwareLevel','SoftwareLevel','General','',0,2,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0513E868-78AD-4514-9E65-056573182A92"), "EXEC db_AddFields 'PhysicalSoftwareComponent','IsBespoke','IsBespoke','General','',0,3,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3A86BA89-3F9F-48C3-8439-9A407C86FAEE"), "EXEC db_AddFields 'PhysicalSoftwareComponent','UserInterfaceType','UserInterfaceType','General','',0,4,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CBDA9A80-7B32-4AC5-B37C-3A1B16753DB6"), "EXEC db_AddFields 'PhysicalSoftwareComponent','NumberofUsers','System.String','General','',0,5,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("768F721F-BD01-45E6-96BC-7E9BEA979804"), "EXEC db_AddFields 'PhysicalSoftwareComponent','SeverityRating','SeverityRating','General','',0,6,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CFC12974-0BA8-47FE-A24E-B16258B62565"), "EXEC db_AddFields 'PhysicalSoftwareComponent','Edition','System.String','General','',0,7,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3F38505B-A4F1-4162-95DA-43D5AFAC6A14"), "EXEC db_AddFields 'PhysicalSoftwareComponent','Release','System.String','General','',0,8,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("890645B8-778D-42DD-89DC-4E5BD50F2236"), "EXEC db_AddFields 'PhysicalSoftwareComponent','ServicePackID','System.String','General','',0,9,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CA56A003-D876-4BD3-B670-EBA8FD162F4F"), "EXEC db_AddFields 'PhysicalSoftwareComponent','VersionNumber','System.String','General','',0,10,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C18BE93D-F826-47E1-B600-3D0A5F2FDE68"), "EXEC db_AddFields 'PhysicalSoftwareComponent','ID','System.String','General','',0,11,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C9521D28-7F26-499B-B567-E3696B0C564C"), "EXEC db_AddFields 'PhysicalSoftwareComponent','PublisherName','System.String','General','',0,12,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5259583B-6EE1-4273-80CF-69AD9EBA1B17"), "EXEC db_AddFields 'PhysicalSoftwareComponent','Language','System.String','General','',0,13,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9E7F34F7-5534-4BC0-A157-5F71DDE21C41"), "EXEC db_AddFields 'PhysicalSoftwareComponent','DateCreated','System.String','General','',0,14,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("91EA3813-FF73-4397-B7F7-2CB1CE7D42FD"), "EXEC db_AddFields 'PhysicalSoftwareComponent','DatePurchased','System.String','General','',0,15,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F6566083-CDA4-4FCB-885C-23982089BF3C"), "EXEC db_AddFields 'PhysicalSoftwareComponent','IsDNS','System.String','General','',0,16,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3C3EC955-B2E1-4546-8008-628ED0FEE9D8"), "EXEC db_AddFields 'PhysicalSoftwareComponent','IsDHCP','System.String','General','',0,17,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("ADA5EF01-C542-4B90-9E3F-911B88D8783D"), "EXEC db_AddFields 'PhysicalSoftwareComponent','IsLicensed','System.String','General','',0,18,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6F6F9A59-6168-4C72-83C5-2BE1062A1FB8"), "EXEC db_AddFields 'PhysicalSoftwareComponent','LicenseNumber','System.String','General','',0,19,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1CB006BC-760D-4F5F-8EB9-1310EF3F0FB2"), "EXEC db_AddFields 'PhysicalSoftwareComponent','HasCopyright','System.String','General','',0,20,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("65AB1C51-5248-42F6-8A66-D6B1CC900AED"), "EXEC db_AddFields 'NetworkComponent','NetworkComponentType','NetworkComponentType','General','',0,1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5CF37245-F50B-4AC3-A2BB-00BCA20DCF7F"), "EXEC db_AddFields 'NetworkComponent','NetworkType','NetworkType','General','',0,25,0|MAJOR METAMODEL CHANGE SAPPI ONLY");

            databaseVersions.Add(new Guid("E14845EA-B961-4E73-A350-C694AB7F3AA2"), "EXEC db_AddFields 'NetworkComponent','SeverityRating','SeverityRating','General','',0,4,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0B058DF7-5C54-42C0-8C3E-E02379E0852C"), "EXEC db_AddFields 'NetworkComponent','ConfigurationID','System.String','General','',0,5,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0F571CC9-133F-42AC-A4BC-75CE18ABC35D"), "EXEC db_AddFields 'NetworkComponent','Make','System.String','General','',0,6,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2F203FD8-F149-45E9-83A8-104A845BCE76"), "EXEC db_AddFields 'NetworkComponent','Model','System.String','General','',0,7,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3117EF34-7E1F-48EB-A1CE-88A1C4A87E04"), "EXEC db_AddFields 'NetworkComponent','ModelNumber','System.String','General','',0,8,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CD9A606B-A74B-4197-849C-57CFFFF95906"), "EXEC db_AddFields 'NetworkComponent','SerialNumber','System.String','General','',0,9,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B4BC2907-CBCB-40C9-9B39-C4CB00D43154"), "EXEC db_AddFields 'NetworkComponent','AssetNumber','System.String','General','',0,10,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("000E9DBA-222F-4727-B66A-06E62E953A52"), "EXEC db_AddFields 'NetworkComponent','DatePurchased','System.String','General','',0,11,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7FA9A0BE-BD44-493D-A06F-2880008AB50E"), "EXEC db_AddFields 'NetworkComponent','isManaged','System.String','General','',0,12,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8C1289FA-EB38-4F08-A6B6-9F9BAE6DB185"), "EXEC db_AddFields 'NetworkComponent','ContractNumber','System.String','General','',0,13,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AC04B612-6B74-4E75-A215-1400AC0E4B6A"), "EXEC db_AddFields 'NetworkComponent','UnderWarranty','System.String','General','',0,14,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("819DF965-ACE2-4CC4-A2B6-C675C31453EF"), "EXEC db_AddFields 'NetworkComponent','NetworkAddress1','System.String','General','',0,15,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1D509097-71B3-445A-B8FA-959BD6052BD0"), "EXEC db_AddFields 'NetworkComponent','NetworkAddress2','System.String','General','',0,16,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("64505DC6-3025-4756-A2F6-D0EC9360B601"), "EXEC db_AddFields 'NetworkComponent','NetworkAddress3','System.String','General','',0,17,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("81B637F0-7C53-44B5-B1E0-742039186466"), "EXEC db_AddFields 'NetworkComponent','NetworkAddress4','System.String','General','',0,18,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5E10C252-D4F9-4B51-9C19-312022647E3A"), "EXEC db_AddFields 'NetworkComponent','NetworkAddress5','System.String','General','',0,19,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DB45C3EE-64B2-4D41-9ACC-103FED007AF0"), "EXEC db_AddFields 'NetworkComponent','IsDNS','System.String','General','',0,20,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("87844C44-7133-4FE2-A8C9-08102EAD3135"), "EXEC db_AddFields 'NetworkComponent','IsDHCP','System.String','General','',0,21,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C8FA74EB-7FDD-4EE3-99DF-569671FADC98"), "EXEC db_AddFields 'NetworkComponent','ConnectionSpeed','System.String','General','',0,22,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("620D25E2-DDDD-439C-A983-D955730A1E53"), "EXEC db_AddFields 'NetworkComponent','NumberofPorts','System.String','General','',0,23,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("312B5D82-DB05-4ACE-934F-01A360AD879D"), "EXEC db_AddFields 'NetworkComponent','NumberofPortsAvailable','System.String','General','',0,24,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("827DA66E-F809-47C2-82A7-123B2C8037CE"), "EXEC db_AddFields 'NetworkComponent','Range','System.String','General','',0,25,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1ED60AC2-F663-4A7E-BF3E-9339EF945F28"), "EXEC db_AddFields 'NetworkComponent','MemoryTotal','System.String','General','',0,26,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6E046942-3C90-46FC-8E82-DA4DC16F97F5"), "EXEC db_AddFields 'StorageComponent','StorageComponentType','StorageComponentType','General','',0,1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("82C8EFD6-FF06-4383-96F8-9B4D5E3F12E0"), "EXEC db_AddFields 'StorageComponent','IsPrimaryOrBackup','IsPrimaryOrBackup','General','',0,2,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("490DC0F6-695E-4737-B5B4-636E4030DEAA"), "EXEC db_AddFields 'StorageComponent','SeverityRating','SeverityRating','General','',0,3,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D79316F3-A003-4FDE-AD73-3E695FB18428"), "EXEC db_AddFields 'StorageComponent','ConfigurationID','System.String','General','',0,4,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("71339315-003A-470D-AB3D-A8D33AD45395"), "EXEC db_AddFields 'StorageComponent','Make','System.String','General','',0,5,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EA1EAFE7-8FD8-4F99-9D78-CE8F29AC2A8A"), "EXEC db_AddFields 'StorageComponent','Model','System.String','General','',0,6,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C0705DAA-0FFD-4603-8A1B-C24494AD38E9"), "EXEC db_AddFields 'StorageComponent','ModelNumber','System.String','General','',0,7,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("177B9074-F37B-4978-A042-B8CA213DE4AF"), "EXEC db_AddFields 'StorageComponent','SerialNumber','System.String','General','',0,8,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("24F83AA1-0A16-4B4E-9D48-BCD38915C02C"), "EXEC db_AddFields 'StorageComponent','AssetNumber','System.String','General','',0,9,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F678206B-D13B-4DF9-811B-06775314D93F"), "EXEC db_AddFields 'StorageComponent','DatePurchased','System.String','General','',0,10,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DB502DD5-5D2B-4446-AC2C-5F0D9204F801"), "EXEC db_AddFields 'StorageComponent','isManaged','System.String','General','',0,11,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F534A9C3-EF41-4F7F-BAC4-5DF5591728AF"), "EXEC db_AddFields 'StorageComponent','ContractNumber','System.String','General','',0,12,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0BC212E9-C349-4FF8-9C93-79EA9FAD64E6"), "EXEC db_AddFields 'StorageComponent','UnderWarranty','System.String','General','',0,13,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FCCE4F70-7702-4129-A99F-83D856119740"), "EXEC db_AddFields 'StorageComponent','NetworkAddress1','System.String','General','',0,14,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("562FEA56-42A5-4907-8429-00740AFAA616"), "EXEC db_AddFields 'StorageComponent','NetworkAddress2','System.String','General','',0,15,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("21C3D61F-30AF-4E6C-A053-FD6666ACEDA6"), "EXEC db_AddFields 'StorageComponent','NetworkAddress3','System.String','General','',0,16,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4BC9E8F3-0068-49B2-B2EF-C1A94FBE0F29"), "EXEC db_AddFields 'StorageComponent','NetworkAddress4','System.String','General','',0,17,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("569E9FF1-CA09-4E22-90C6-E6BC32B20E50"), "EXEC db_AddFields 'StorageComponent','NetworkAddress5','System.String','General','',0,18,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4F383562-C98A-4B62-B65D-379498F08C9C"), "EXEC db_AddFields 'StorageComponent','Capacity','System.String','General','',0,19,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F25D6D02-F38D-441D-9363-8AD7730A6525"), "EXEC db_AddFields 'StorageComponent','NumberofDisks','System.String','General','',0,20,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5040ECB4-F0D0-4D91-A310-C6B42AB5BF8B"), "EXEC db_AddFields 'StorageComponent','FileSystem','System.String','General','',0,21,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7F84BA8A-D9EE-4C5E-A1F4-5AF59B7636D0"), "EXEC db_AddFields 'ComputingComponent','ComputingComponentType','ComputingComponentType','General','',0,1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D66FAB65-8224-4C22-82F4-9F39E1E69011"), "EXEC db_AddFields 'ComputingComponent','ServerType','ServerType','General','',0,2,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F954597C-5ED1-435B-9591-6330554FEBAF"), "EXEC db_AddFields 'ComputingComponent','SeverityRating','SeverityRating','General','',0,3,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("80D6BF44-97E6-48DF-96F1-7A5893010A83"), "EXEC db_AddFields 'ComputingComponent','ConfigurationID','System.String','General','',0,4,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2EA7BA6B-2C2C-4979-BA68-C3A2ECAC4DED"), "EXEC db_AddFields 'ComputingComponent','Make','System.String','General','',0,5,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("345C863D-8CB8-4FAD-804F-7204C2830EDB"), "EXEC db_AddFields 'ComputingComponent','Model','System.String','General','',0,6,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C266B3EF-A9FB-48E8-8BAF-73F8D30A7F72"), "EXEC db_AddFields 'ComputingComponent','ModelNumber','System.String','General','',0,7,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0AA84EAC-D511-439C-AFD1-D3EA052765D0"), "EXEC db_AddFields 'ComputingComponent','SerialNumber','System.String','General','',0,8,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1A993C7B-FE71-4680-B14E-FCA4369E9672"), "EXEC db_AddFields 'ComputingComponent','AssetNumber','System.String','General','',0,9,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D80FC428-45F9-4E77-8145-B394F91A3807"), "EXEC db_AddFields 'ComputingComponent','DatePurchased','System.String','General','',0,10,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8347AC99-E729-459F-B4BF-464C0B00E575"), "EXEC db_AddFields 'ComputingComponent','isManaged','System.String','General','',0,11,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D0E69900-B954-4753-A6C8-290AAF4D0C81"), "EXEC db_AddFields 'ComputingComponent','ContractNumber','System.String','General','',0,12,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("318F2EFA-2235-45AC-86E4-E76A4216BAAB"), "EXEC db_AddFields 'ComputingComponent','UnderWarranty','System.String','General','',0,13,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("966A7061-EE02-4958-BF95-99172FC44662"), "EXEC db_AddFields 'ComputingComponent','NetworkAddress1','System.String','General','',0,14,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DDC55C23-08FA-4ECE-A86E-0E3F7CC52BAA"), "EXEC db_AddFields 'ComputingComponent','NetworkAddress2','System.String','General','',0,15,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F93CE646-A356-471B-938C-6D045231DF68"), "EXEC db_AddFields 'ComputingComponent','NetworkAddress3','System.String','General','',0,16,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B20658CF-65B4-4EEE-A0DF-7024C5F89640"), "EXEC db_AddFields 'ComputingComponent','NetworkAddress4','System.String','General','',0,17,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EB172757-DAED-4038-91CA-8DC843D59839"), "EXEC db_AddFields 'ComputingComponent','NetworkAddress5','System.String','General','',0,18,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("508AECAD-8778-4259-ADEC-66A2AEB0F629"), "EXEC db_AddFields 'ComputingComponent','IsDNS','System.String','General','',0,19,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DE033F43-5E4F-4E34-85DD-9E5379AEF443"), "EXEC db_AddFields 'ComputingComponent','IsDHCP','System.String','General','',0,20,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C03945DA-C369-4ED2-AFFF-B0709349AAFF"), "EXEC db_AddFields 'ComputingComponent','Domain','System.String','General','',0,21,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7908EC65-3877-498A-AA46-3E22AEF9074E"), "EXEC db_AddFields 'ComputingComponent','Capacity','System.String','General','',0,22,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FE7D86EB-50F2-4ABA-BB87-54083672B818"), "EXEC db_AddFields 'ComputingComponent','NumberofDisks','System.String','General','',0,23,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C4A9894E-5E06-49CF-B889-A0F8BB67835E"), "EXEC db_AddFields 'ComputingComponent','CPUType','System.String','General','',0,24,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DD3F1514-B7AB-47A5-8BE1-C28741698D7D"), "EXEC db_AddFields 'ComputingComponent','CPUSpeed','System.String','General','',0,25,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B5B7A8FB-F7C7-4731-9F4E-12714B509E8F"), "EXEC db_AddFields 'ComputingComponent','Monitor','System.String','General','',0,26,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("13BBA2C7-0CB9-4EC8-8FCC-6B316ADBF665"), "EXEC db_AddFields 'ComputingComponent','VideoCard','System.String','General','',0,27,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6D7E794A-32A7-4993-85EA-6F6B0A214BDE"), "EXEC db_AddFields 'ComputingComponent','MemoryTotal','System.String','General','',0,28,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("763E562E-A08A-4CC5-BB7B-3C998DD5BB5C"), "EXEC db_AddFields 'PhysicalSoftwareComponent','Configuration','System.String','General','',0,2,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("82586B8E-B602-4C0E-B10C-53CF573F991B"), "EXEC db_AddFields 'PhysicalSoftwareComponent','Type','SoftwareType','General','',0,1,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("086038F9-F4D0-4A5F-A159-976C68D6F609"), "EXEC db_AddFields 'PhysicalSoftwareComponent','UserInterface','UserInterfaceType','General','',0,4,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CA01FECD-0462-43C6-8806-2BAFFE462C82"), "EXEC db_AddFields 'PhysicalSoftwareComponent','ServicePack','System.String','General','',0,9,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("14822223-A3EE-487F-9516-D0B7DF394023"), "EXEC db_AddFields 'PhysicalSoftwareComponent','Version','System.String','General','',0,10,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("64A1B6AF-2D20-4E19-88B1-84FA15C4B4B7"), "EXEC db_AddFields 'PhysicalSoftwareComponent','Publisher','System.String','General','',0,12,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("AAA7085C-48DE-4A18-ABB0-C0659F82CE44"), "EXEC db_AddFields 'PhysicalSoftwareComponent','isDNS','System.String','General','',0,16,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("B82D6DE4-93BF-4836-A3A8-2A43A0F334B9"), "EXEC db_AddFields 'PhysicalSoftwareComponent','isDHCP','System.String','General','',0,17,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("705FB906-F81C-46CA-9B93-BFD9CC5C57C4"), "EXEC db_AddFields 'PhysicalSoftwareComponent','isLicensed','System.String','General','',0,18,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C0B397D3-157E-4F52-A0DD-C835137749E5"), "EXEC db_AddFields 'PhysicalSoftwareComponent','Copyright','System.String','General','',0,20,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("9A69890B-774E-48F5-A99B-53069DFEF66D"), "EXEC db_AddFields 'NetworkComponent','Type','NetworkComponentType','General','',0,1,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("34A2D9A7-CAED-4DBC-B024-C5C1E6D435EB"), "EXEC db_AddFields 'NetworkComponent','Configuration','System.String','General','',0,5,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1F939B99-012B-4383-9B78-A229D330FA5F"), "EXEC db_AddFields 'NetworkComponent','MacAddress','System.String','General','',0,15,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("CB1DDFCC-9DF7-4457-82CC-D4C9C12806E5"), "EXEC db_AddFields 'NetworkComponent','isDNS','System.String','General','',0,20,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("38BCBABA-5773-4931-917D-43996A5F489C"), "EXEC db_AddFields 'NetworkComponent','isDHCP','System.String','General','',0,21,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F1891EE4-E5B2-4889-93F9-0847F8BE8EF5"), "EXEC db_AddFields 'NetworkComponent','Number_of_Ports','System.String','General','',0,23,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EC33D0E6-5AA6-4ECC-B1DE-0805EE17C5AE"), "EXEC db_AddFields 'NetworkComponent','Number_of_Ports_Available','System.String','General','',0,24,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BF89CD42-5B78-495D-B1BE-63DE1E697895"), "EXEC db_AddFields 'NetworkComponent','Mem_Total','System.String','General','',0,26,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0D50EBFA-A6CB-42F2-979D-6C9761C7E1FA"), "EXEC db_AddFields 'StorageComponent','Type','StorageComponentType','General','',0,1,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E2A875AD-BB72-4988-B15D-0C9F84E4039C"), "EXEC db_AddFields 'StorageComponent','SeverityIndicator','SeverityRating','General','',0,3,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("651C5C4D-39E8-489D-AC55-1AD13E68220C"), "EXEC db_AddFields 'StorageComponent','Configuration','System.String','General','',0,4,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5EE3EA4B-4D2F-4072-BE18-2A97D920058C"), "EXEC db_AddFields 'StorageComponent','Number_of_Disks','System.String','General','',0,20,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A17C9E1F-D2DE-46A9-9D50-2B7D61B10F15"), "EXEC db_AddFields 'ComputingComponent','Type','ComputingComponentType','General','',0,1,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2EF709FB-2CEB-436D-BC9E-54CDCD0BC0B2"), "EXEC db_AddFields 'ComputingComponent','Configuration','System.String','General','',0,4,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6BFA4934-E76E-41A7-BC25-E18A811EA50F"), "EXEC db_AddFields 'ComputingComponent','MACAddress','System.String','General','',0,14,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AD9A507A-CD44-4F02-B484-E2FD943FA013"), "EXEC db_AddFields 'ComputingComponent','StaticIP','System.String','General','',0,15,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("69F3F2E4-B9CF-4803-93CD-9A5E05DC3B48"), "EXEC db_AddFields 'ComputingComponent','isDNS','System.String','General','',0,19,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("06C32E5F-9757-4F50-838C-12683B323641"), "EXEC db_AddFields 'ComputingComponent','isDHCP','System.String','General','',0,20,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2E5E10C4-E6E3-4ADB-9BC9-E9531B2981E7"), "EXEC db_AddFields 'ComputingComponent','Number_Of_Disks','System.String','General','',0,23,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2A9B86B5-7730-45C7-BFFD-67AA7A711E97"), "EXEC db_AddFields 'ComputingComponent','CPU_Type','System.String','General','',0,24,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4E9AC521-D68F-40A1-86BF-55EA1346AF76"), "EXEC db_AddFields 'ComputingComponent','CPU_Speed','System.String','General','',0,25,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("63EC99D8-625D-413D-8B5B-C2196E2BBCD1"), "EXEC db_AddFields 'ComputingComponent','Video_Card','System.String','General','',0,27,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4C4AC8B2-2392-415E-912F-E717DFCBE40B"), "EXEC db_AddFields 'ComputingComponent','Mem_Total','System.String','General','',0,28,0|MAJOR METAMODEL CHANGE SAPPI ONLY");

            databaseVersions.Add(new Guid("6761F787-7DDB-4ABB-9946-EE20BE39FA6D"), "EXEC db_AddFields 'Activity','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DFD14C70-7081-40C1-8000-8A12A4F36DF7"), "EXEC db_AddFields 'Activity','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2290DB62-B392-446E-9DD0-5034571D3D17"), "EXEC db_AddFields 'Activity','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5FDD5E92-4AAA-4AC5-8A66-60122508A278"), "EXEC db_AddFields 'Activity','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("430E5AC0-BCF6-4166-A836-FF02B5FECFFB"), "EXEC db_AddFields 'Activity','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E7C1D5FD-9440-4F18-8B53-37AE3F7A7F39"), "EXEC db_AddFields 'Activity','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("179F652A-9FB8-4E17-8DB6-E83483094EDC"), "EXEC db_AddFields 'Activity','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CD15DEEE-7F1D-41FA-8DC8-6ED64404257D"), "EXEC db_AddFields 'Activity','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A0CE3053-701E-41C8-9DF0-AA96B9DA9430"), "EXEC db_AddFields 'Activity','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D81E52DE-FCD3-4F4A-8A60-D94061F256C1"), "EXEC db_AddFields 'Activity','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //ATTRIBUTE
            databaseVersions.Add(new Guid("7CFD1F82-7E40-45B8-9A7F-03B9A41EA965"), "EXEC db_AddFields 'CategoryFactor','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DAE5646B-B4C8-45B9-9A57-C0A558912B0A"), "EXEC db_AddFields 'CategoryFactor','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("810914D9-98E1-4327-A465-94052AA53131"), "EXEC db_AddFields 'CategoryFactor','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5DE4B0D7-C97B-4114-A4EB-7B07B09210E5"), "EXEC db_AddFields 'CategoryFactor','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("09FC13D7-493A-457F-85C7-280A3ED3F65C"), "EXEC db_AddFields 'CategoryFactor','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2B5B0534-BADF-4805-B70E-5579791B20AD"), "EXEC db_AddFields 'CategoryFactor','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("67526687-C530-4689-82A7-986D76253A34"), "EXEC db_AddFields 'CategoryFactor','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CDD28C73-A4DA-4A0C-A690-E0FCAF83666E"), "EXEC db_AddFields 'CategoryFactor','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("559E0344-AD5D-4E2B-B5F5-3E4F20DE6569"), "EXEC db_AddFields 'CategoryFactor','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("064630F4-DEF4-4DB5-8198-315E7CFE7D35"), "EXEC db_AddFields 'CategoryFactor','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D44F0378-4228-4293-B088-718CB65D5F98"), "EXEC db_AddFields 'Competency','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BF327D7A-8A4E-4135-B43E-CA1491E45206"), "EXEC db_AddFields 'Competency','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("13EC577B-CEF5-4783-BE04-3CE926DB7294"), "EXEC db_AddFields 'Competency','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("51C4C548-F12E-4637-AD55-7AB5024E9AD5"), "EXEC db_AddFields 'Competency','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("55CC189F-DDB0-442C-A6DE-4C87B63045FB"), "EXEC db_AddFields 'Competency','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EE047945-B6C2-43A6-BE7F-558D26C6E374"), "EXEC db_AddFields 'Competency','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("279F9C0C-18D6-40BA-8D36-9D6F51D698A3"), "EXEC db_AddFields 'Competency','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7D30AA17-00CD-4AD9-9371-BBA72218F9C9"), "EXEC db_AddFields 'Competency','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B588CE84-75B0-4936-A3BE-44E38DF308A5"), "EXEC db_AddFields 'Competency','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("88D839D9-934B-4CE4-9CB1-42128B8B45AA"), "EXEC db_AddFields 'Competency','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //Condition(al)description
            //Connectionspeedtypesize
            databaseVersions.Add(new Guid("38716814-36F1-429D-845A-1B2005E1D48E"), "EXEC db_AddFields 'CSF','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B625DDC1-B38C-4FF8-8D84-7F9D44C8B9EE"), "EXEC db_AddFields 'CSF','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7BEFD295-148A-4AE8-B7A7-EE28A4536CA3"), "EXEC db_AddFields 'CSF','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E9EB215A-4EE8-4594-B237-32CB1675A864"), "EXEC db_AddFields 'CSF','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("967F0177-CC11-4B8D-9197-62AA30887043"), "EXEC db_AddFields 'CSF','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D7693454-B37C-4338-AEFE-E9762F90690F"), "EXEC db_AddFields 'CSF','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C8AD5A4E-D678-43F3-888A-DD83863C502A"), "EXEC db_AddFields 'CSF','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F1B9FD57-BB77-4881-9A3E-4DE331D5E4EA"), "EXEC db_AddFields 'CSF','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A5E9DCA4-B9AC-46A4-9AB3-F3EDB88DD329"), "EXEC db_AddFields 'CSF','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6C8DB1AC-27F6-496F-85A6-945BEE1DC6D5"), "EXEC db_AddFields 'CSF','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //DataColumn
            databaseVersions.Add(new Guid("F88D77DF-B259-40BB-958B-F86B501F6B97"), "EXEC db_AddFields 'DataSchema','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C73F7509-DDDB-42F6-9808-535C81478FAE"), "EXEC db_AddFields 'DataSchema','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A65D1CE4-CBC9-4E7E-AE3B-BE9AF29796BF"), "EXEC db_AddFields 'DataSchema','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5AA48871-D00B-4383-8BE0-79CB11BF1C36"), "EXEC db_AddFields 'DataSchema','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("29D2E16B-B948-45A6-9FCA-0CA02F6AF550"), "EXEC db_AddFields 'DataSchema','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A17C9602-BCD5-4D69-A99B-F509E7AE8E4C"), "EXEC db_AddFields 'DataSchema','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F9D7605B-2C28-4C5B-A531-5DBB9D8C9644"), "EXEC db_AddFields 'DataSchema','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9ACF6B70-8756-4D06-A46C-C8D80E9924F6"), "EXEC db_AddFields 'DataSchema','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4E34F96D-2907-4B40-B800-9AA7F44A6C34"), "EXEC db_AddFields 'DataSchema','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B7F64DDD-0DB7-4F32-9EA6-9141343CDBD1"), "EXEC db_AddFields 'DataSchema','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0FEDC29A-1C23-4EAB-8489-685D5E2C4E80"), "EXEC db_AddFields 'DataTable','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7BB8D1B3-72E7-4375-BE68-3E57BB405529"), "EXEC db_AddFields 'DataTable','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("900B0544-0788-46BA-A288-DE9FFDA46A1D"), "EXEC db_AddFields 'DataTable','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E2A927F9-2A17-4800-A424-7548125AB00A"), "EXEC db_AddFields 'DataTable','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9EF7306A-451F-4119-A6DF-46265E117AAB"), "EXEC db_AddFields 'DataTable','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5D971C3E-6733-4AB2-8419-6D5487C7B5AC"), "EXEC db_AddFields 'DataTable','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("644AFD08-3023-490F-9310-EC5EAB198203"), "EXEC db_AddFields 'DataTable','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BD580633-E588-4C42-BDAF-E62C1C8776AA"), "EXEC db_AddFields 'DataTable','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("147185F1-91D9-489E-9E08-DA75FB2B28C4"), "EXEC db_AddFields 'DataTable','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("55F71B32-0ED5-4C0B-90C3-18263403CE4D"), "EXEC db_AddFields 'DataTable','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6E55208D-F892-404F-B529-E616B340AFB6"), "EXEC db_AddFields 'DataView','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0B47EAF8-5084-4DCF-A6BE-2C41F32C3D9F"), "EXEC db_AddFields 'DataView','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("253DDFC0-DF38-4E5D-A33A-10BC06103809"), "EXEC db_AddFields 'DataView','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0F578A9B-39BA-4274-9E14-93013F209873"), "EXEC db_AddFields 'DataView','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C1F0A1CB-5E44-497B-89C7-1E3714B8C076"), "EXEC db_AddFields 'DataView','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("01BEF5C7-62C8-4C65-908C-D64F9135DD73"), "EXEC db_AddFields 'DataView','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DB80D75A-2352-4075-85DF-30B86BECB580"), "EXEC db_AddFields 'DataView','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("36F95F8C-E99A-4AA1-A387-233E141B9826"), "EXEC db_AddFields 'DataView','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FE19F730-0FD1-4AD4-BB41-B887F94E7DFA"), "EXEC db_AddFields 'DataView','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("47EF80B4-7884-4708-9107-6E695161A88D"), "EXEC db_AddFields 'DataView','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //DependancyDesctiption
            databaseVersions.Add(new Guid("7A2A08F6-F58D-4F05-A438-232F1F86E98C"), "EXEC db_AddFields 'Employee','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CB151575-3F79-4A16-B73C-09C70402BE91"), "EXEC db_AddFields 'Employee','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F8E0EFB2-87C0-4DC2-97FC-67B548EB5C8B"), "EXEC db_AddFields 'Employee','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1A85F8FA-0D41-4D21-A6B9-04ADF8FF85CE"), "EXEC db_AddFields 'Employee','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E642D8B7-3791-4B39-BD6A-2F34F568E25E"), "EXEC db_AddFields 'Employee','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D156F8CC-53F4-458C-8F09-171784B573C5"), "EXEC db_AddFields 'Employee','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("78F94733-5D8A-44EB-A1C3-F0440936DA19"), "EXEC db_AddFields 'Employee','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C44FD4B5-B0A2-4D0A-9BB1-09B312C86CD0"), "EXEC db_AddFields 'Employee','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D0EDCB5C-9063-4CB3-98DD-933CD0BC0612"), "EXEC db_AddFields 'Employee','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3A13F6CE-4ACA-48AC-A30A-59D8F47C902E"), "EXEC db_AddFields 'Employee','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("47E688AA-F8CA-4024-8C18-CB7B7701F6E5"), "EXEC db_AddFields 'Entity','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F2982D64-BBC6-44CA-B882-822F7C4591B9"), "EXEC db_AddFields 'Entity','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B1C24F62-136B-400A-8E4C-8738DE7350B3"), "EXEC db_AddFields 'Entity','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("925F7A6B-8E84-4700-97FF-E8D6C5A02876"), "EXEC db_AddFields 'Entity','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CDDA582C-FCE9-4A8B-B238-A425FA1152E1"), "EXEC db_AddFields 'Entity','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("81EED209-7C40-4EA4-9948-72D999E58C93"), "EXEC db_AddFields 'Entity','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6FD9FB1E-EC42-48D6-8C0F-064672A6886D"), "EXEC db_AddFields 'Entity','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("52E40EB5-96BC-4E1D-880E-F36B70040E27"), "EXEC db_AddFields 'Entity','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EEB7CADA-997F-43DA-B23B-32959B823BED"), "EXEC db_AddFields 'Entity','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("796189D6-469B-48A1-A1DF-7AA47862B6EA"), "EXEC db_AddFields 'Entity','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2D6F8A52-57D2-4DB3-BC96-5F2088EA9F10"), "EXEC db_AddFields 'Environment','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8230AF42-C5C0-430A-9617-5902AE2CCA3C"), "EXEC db_AddFields 'Environment','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("716D451C-447F-41A2-B669-FC7FE335C6C4"), "EXEC db_AddFields 'Environment','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B11F8CA7-AFD5-4AA8-88D9-F8968807D7B1"), "EXEC db_AddFields 'Environment','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("88671DD3-FA14-4C58-8661-EDC2DFD46C8B"), "EXEC db_AddFields 'Environment','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1BA3D39D-78EB-4F8B-9183-A09AFF740CE9"), "EXEC db_AddFields 'Environment','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B3693139-8F4F-4607-A17C-1B093F7A0DFE"), "EXEC db_AddFields 'Environment','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("820227A0-6B4E-42B0-A5A5-ACBDC73A7088"), "EXEC db_AddFields 'Environment','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B3C7F94F-28BD-4903-81AF-1354D7C0C8C8"), "EXEC db_AddFields 'Environment','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BFA798E7-1F62-4BEA-979B-A99F6F716A04"), "EXEC db_AddFields 'Environment','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F692402D-9E41-4951-B2FE-63EF164045BC"), "EXEC db_AddFields 'EnvironmentCategory','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2123E5BB-DF90-4028-819B-4F55680CCA2F"), "EXEC db_AddFields 'EnvironmentCategory','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F0D822B6-D580-45D4-8A06-17454E057553"), "EXEC db_AddFields 'EnvironmentCategory','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("12E88D37-7EF8-45F9-8461-C5640243BD8E"), "EXEC db_AddFields 'EnvironmentCategory','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1A542313-B806-403C-84D8-E29845CF6ABE"), "EXEC db_AddFields 'EnvironmentCategory','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4B96395A-26A3-4573-B991-3D53191327DB"), "EXEC db_AddFields 'EnvironmentCategory','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A6B88CA3-6ED0-4200-826F-FB2A25B1E17A"), "EXEC db_AddFields 'EnvironmentCategory','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("18D65356-9421-43FC-83DF-59103FFA494F"), "EXEC db_AddFields 'EnvironmentCategory','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AE2F7501-11F0-4130-8B36-9B48A3FE6B9D"), "EXEC db_AddFields 'EnvironmentCategory','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9091FE4B-ADF1-4DCA-B8A7-7F6F9D90E861"), "EXEC db_AddFields 'EnvironmentCategory','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("49C36CD8-671A-4EF0-8DA5-21C0617CE08C"), "EXEC db_AddFields 'Exception','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FB2AFF3B-9F02-4999-A506-048D14B49B30"), "EXEC db_AddFields 'Exception','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("74052EF4-52C6-470B-A9F8-D5EAE68ED9DB"), "EXEC db_AddFields 'Exception','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("87BD8598-01E1-488F-89AB-C15D2B6F3EED"), "EXEC db_AddFields 'Exception','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2CF101DE-474A-48A9-B2A4-751AD2373721"), "EXEC db_AddFields 'Exception','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6725561A-0547-40BA-9F0A-3A5A07CDE999"), "EXEC db_AddFields 'Exception','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BCE2153F-6EFC-482F-8FE3-D70C60A5593A"), "EXEC db_AddFields 'Exception','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F9B29C46-E6C9-4712-A7C7-B38B94BEEEE5"), "EXEC db_AddFields 'Exception','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A1AE4620-B25D-4419-BAFE-309B81A8D64A"), "EXEC db_AddFields 'Exception','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("34355F54-6500-42E9-9312-9197864EB601"), "EXEC db_AddFields 'Exception','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //FlowDescription
            databaseVersions.Add(new Guid("19AC7012-7013-45F0-BCF4-D695C6AC187D"), "EXEC db_AddFields 'Function','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("15897BBF-2104-4C2B-A80F-C78549607A19"), "EXEC db_AddFields 'Function','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6E1D508C-5FE8-4A3B-804A-1B3F3FD3D171"), "EXEC db_AddFields 'Function','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EE4184AD-B3E0-4245-8325-7ACC413EDF97"), "EXEC db_AddFields 'Function','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A1B2517E-0639-451F-B5C4-37203CA61646"), "EXEC db_AddFields 'Function','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0E084F81-EE8B-485D-A4BE-3E827D4DADDA"), "EXEC db_AddFields 'Function','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3EB3844F-D249-4DFD-9178-C76954357F33"), "EXEC db_AddFields 'Function','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CBB60697-0E60-4215-8C50-A2F178DDD00A"), "EXEC db_AddFields 'Function','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("86542C84-DC8B-4044-9DC2-582B19E05A77"), "EXEC db_AddFields 'Function','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DBAB0A54-4553-4C1F-A8AD-1B1E3FC17689"), "EXEC db_AddFields 'Function','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("21B5763B-415D-44E4-82CA-CF9380FBEC45"), "EXEC db_AddFields 'GovernanceMechanism','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("05420FFB-AA9F-4154-BEB1-B99922E65FB1"), "EXEC db_AddFields 'GovernanceMechanism','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E586C7A3-101D-43C9-8154-81205F019CCD"), "EXEC db_AddFields 'GovernanceMechanism','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9C01ABE9-7FB5-498C-BA9E-9CA05AF09D40"), "EXEC db_AddFields 'GovernanceMechanism','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EEA00AA1-E245-495A-8A28-11A8274D15A0"), "EXEC db_AddFields 'GovernanceMechanism','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("98D555B0-B0E9-4DA1-AAF9-BEBE5306318E"), "EXEC db_AddFields 'GovernanceMechanism','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("236F3F0F-D86F-4C6A-95B6-A1F0951C9111"), "EXEC db_AddFields 'GovernanceMechanism','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3461770D-0DFE-4CF3-9D8A-FD0C4F8FC59A"), "EXEC db_AddFields 'GovernanceMechanism','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A18DCF5A-9280-4E5B-BCBF-FC8BA22B532B"), "EXEC db_AddFields 'GovernanceMechanism','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0C2029BC-4814-4A0A-8DC4-BE3C74C63E5C"), "EXEC db_AddFields 'GovernanceMechanism','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A65BCB78-5D3F-4226-9B12-196E60010D2E"), "EXEC db_AddFields 'Implication','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("56C724D4-CA9E-4758-B65C-EB84F5AB1CE4"), "EXEC db_AddFields 'Implication','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B4DC21A4-D5DE-44E8-A4BF-E679E3888E60"), "EXEC db_AddFields 'Implication','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("01C98F7C-D042-4AFA-9D4D-DAC9592D84FE"), "EXEC db_AddFields 'Implication','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9810B2B8-A079-4340-B9F3-84174D13F7BF"), "EXEC db_AddFields 'Implication','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D9812796-3013-4A3B-A9B0-5F8CE295F920"), "EXEC db_AddFields 'Implication','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4412C377-B881-4942-87E4-5FCBE4DF3689"), "EXEC db_AddFields 'Implication','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E4550264-339E-4761-AF55-83A04E4177A6"), "EXEC db_AddFields 'Implication','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7683372F-9673-468E-9907-1827F524F8D0"), "EXEC db_AddFields 'Implication','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AFCCC259-9C6E-49FF-A446-8B3332B25D08"), "EXEC db_AddFields 'Implication','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F7EAB02A-6611-4CAD-914F-773F7E722C10"), "EXEC db_AddFields 'Iteration','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6D75EB1F-1615-4690-9320-A844546E9FDE"), "EXEC db_AddFields 'Iteration','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0CBC53EF-C256-4FFD-A842-C8B12741A789"), "EXEC db_AddFields 'Iteration','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("71A7C944-28F2-434C-9C8E-7F24F4E3DFA4"), "EXEC db_AddFields 'Iteration','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("17B8573E-31D4-48BD-8B4D-D55A9137DBA0"), "EXEC db_AddFields 'Iteration','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2F0B5471-F831-4816-A55A-F79D706B37A2"), "EXEC db_AddFields 'Iteration','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("75629135-81DC-425D-B41D-C84A1B6DC209"), "EXEC db_AddFields 'Iteration','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("95E9D1AF-1E90-4EB1-BAB2-DC2E3D7E690A"), "EXEC db_AddFields 'Iteration','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A87479EC-B3E1-456F-84D1-BEF07804892B"), "EXEC db_AddFields 'Iteration','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4A18234E-37A9-413A-8BFA-43BA369B8148"), "EXEC db_AddFields 'Iteration','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("89D6A36C-706A-4318-8D99-33EC9278E0C2"), "EXEC db_AddFields 'Job','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2A6D2106-8A27-42FC-B210-DF167B17074C"), "EXEC db_AddFields 'Job','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4B035EFD-2595-4E03-82A0-F76B026BC65B"), "EXEC db_AddFields 'Job','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FD4D910A-2280-439C-BC00-BE937775242A"), "EXEC db_AddFields 'Job','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("78EE72B1-7D25-4D97-8F87-DDA399F2170D"), "EXEC db_AddFields 'Job','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("04DCE865-7D4D-437E-B5AA-2C1EAFF61ECF"), "EXEC db_AddFields 'Job','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C47A0A36-78B4-40E2-A9DE-9455FD0CC215"), "EXEC db_AddFields 'Job','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("29F50807-B43F-4DC9-935B-2E0F843479DD"), "EXEC db_AddFields 'Job','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1608802B-92DF-4CE8-9D1C-7C16646A63E1"), "EXEC db_AddFields 'Job','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E93B6B06-F1BE-4E95-9C3A-93072D3D7AD9"), "EXEC db_AddFields 'Job','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CBFE029D-F20D-4900-8BB4-95281D2D2701"), "EXEC db_AddFields 'JobPosition','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("46BC6BBF-161F-4ADE-B48E-79141B7BE525"), "EXEC db_AddFields 'JobPosition','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6F8A4052-A325-49E9-BBC3-23052A4ED1DB"), "EXEC db_AddFields 'JobPosition','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5BF5E5C7-10BF-4689-89FD-3814351C89FF"), "EXEC db_AddFields 'JobPosition','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E6CC1ED9-F092-44A9-BCBE-101D5599FBA7"), "EXEC db_AddFields 'JobPosition','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FDE12308-B69A-4C77-AE2F-0773C39B8183"), "EXEC db_AddFields 'JobPosition','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("95FC5B5A-416E-4CA7-9D64-25B74439C08A"), "EXEC db_AddFields 'JobPosition','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("94682BEF-1759-49CD-BBB1-DA60F6F09AB3"), "EXEC db_AddFields 'JobPosition','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A65F642C-9B21-4347-82E2-259B509403EE"), "EXEC db_AddFields 'JobPosition','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D8A91158-2708-4D69-ADBE-C07A060403D9"), "EXEC db_AddFields 'JobPosition','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E17517F1-D0A9-4F89-950F-D743E6F92C5A"), "EXEC db_AddFields 'Location','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("22111845-3C07-480E-B388-72E71594B413"), "EXEC db_AddFields 'Location','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("452BE8AB-33E8-4E35-9D73-6CF31BFE2D66"), "EXEC db_AddFields 'Location','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3A741747-7B47-41C9-BB37-E1085DC6433A"), "EXEC db_AddFields 'Location','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F65E3606-FAD8-4262-8F0C-1B96F59DCE7E"), "EXEC db_AddFields 'Location','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("63236F34-3B01-453B-B933-3F001AADF818"), "EXEC db_AddFields 'Location','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8C135ADF-7F88-4DFD-8003-1371041E63A8"), "EXEC db_AddFields 'Location','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7198631C-E06A-4F95-BF42-0578A1CE6DD6"), "EXEC db_AddFields 'Location','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3E3A7DB2-F44E-4EEC-BD4F-3F4491E9605D"), "EXEC db_AddFields 'Location','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("88FC81DA-4F63-4E06-BB2A-F8554143CF9F"), "EXEC db_AddFields 'Location','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //Locationassociation
            databaseVersions.Add(new Guid("10D5FA95-43EC-47E3-8D55-094C40934F9B"), "EXEC db_AddFields 'LocationScheme','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("51911D73-B3AA-4879-8416-8E82A1C6A17C"), "EXEC db_AddFields 'LocationScheme','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("33EE0B66-E224-4694-9749-95ED473953C8"), "EXEC db_AddFields 'LocationScheme','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("645E5131-F01B-47AE-9B5D-CE497E6C1959"), "EXEC db_AddFields 'LocationScheme','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E175BB62-6FFB-4EBD-8C02-5D9ED02E932B"), "EXEC db_AddFields 'LocationScheme','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("122D8D6B-D96A-4CDD-9101-A9AC70FA7446"), "EXEC db_AddFields 'LocationScheme','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7282B42B-6804-4A6D-A762-DE01634B9AC3"), "EXEC db_AddFields 'LocationScheme','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2F3CD0B6-A764-4D0D-A0D1-1CB6EA22E10E"), "EXEC db_AddFields 'LocationScheme','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D5F14472-3DE8-40E8-819C-0630C4A7A194"), "EXEC db_AddFields 'LocationScheme','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D26394C5-7532-4345-9EAE-46BC7677D895"), "EXEC db_AddFields 'LocationScheme','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("42EC77E8-C0B1-4E2F-9A76-A8AA8C410C3E"), "EXEC db_AddFields 'LocationUnit','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("357ED6D3-5194-4899-97CC-7711898F0EBA"), "EXEC db_AddFields 'LocationUnit','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FF212926-E0CC-4920-A996-B370EC48E02D"), "EXEC db_AddFields 'LocationUnit','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("393E9848-2632-4E75-ACBF-1F6788AF4176"), "EXEC db_AddFields 'LocationUnit','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7D1761EB-987B-440A-BC8A-31F70ECAE90F"), "EXEC db_AddFields 'LocationUnit','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2B67DA36-9F0F-44B3-AA74-AE675BF19055"), "EXEC db_AddFields 'LocationUnit','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1D7E8DB9-E7EB-431A-8046-B865A4C1D872"), "EXEC db_AddFields 'LocationUnit','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9FA7FF08-8B34-44CA-9418-56574371C2FC"), "EXEC db_AddFields 'LocationUnit','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5A0DDB20-BABA-4007-96DA-4BA1EE3542AF"), "EXEC db_AddFields 'LocationUnit','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8DCC6834-C3BD-4B55-9D9A-50247E1727C0"), "EXEC db_AddFields 'LocationUnit','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0224EEED-EF9E-493D-BBCA-5A188F09CD71"), "EXEC db_AddFields 'Logic','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C6124765-0FF5-4226-894F-F2A368C66BC4"), "EXEC db_AddFields 'Logic','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5F5AEA58-99BC-4B65-B878-41EA2F7314CE"), "EXEC db_AddFields 'Logic','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F91930BC-A285-482C-BD47-2199A1E63CCE"), "EXEC db_AddFields 'Logic','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F55D43DE-24DD-4E61-9C2A-3226FDACEBCF"), "EXEC db_AddFields 'Logic','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0E2E2E37-D835-4ED6-B827-5C5DA123D587"), "EXEC db_AddFields 'Logic','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("694BA2E0-8735-4B6B-BE5B-3FB9C026F650"), "EXEC db_AddFields 'Logic','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0A7585E7-F695-4C0A-AC35-2A5EF075E5C0"), "EXEC db_AddFields 'Logic','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9494CF20-BEC3-4F29-AC61-E7D9970F713A"), "EXEC db_AddFields 'Logic','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D2965C1E-7C2D-4408-A5BF-7B486C599E12"), "EXEC db_AddFields 'Logic','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B0B3F839-124B-4C14-92E7-B19133058A13"), "EXEC db_AddFields 'MeasurementItem','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("233E36A3-4693-4E33-A194-86062ED52745"), "EXEC db_AddFields 'MeasurementItem','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7DB22374-20A4-4B32-B86D-A4304CF515EB"), "EXEC db_AddFields 'MeasurementItem','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A62EF9B5-3A60-473F-9DE2-5AB8428C87B7"), "EXEC db_AddFields 'MeasurementItem','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5ED19884-3C5C-4891-8A54-B197CB3EC950"), "EXEC db_AddFields 'MeasurementItem','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("92D98352-6E88-4026-B1C6-708C892B876C"), "EXEC db_AddFields 'MeasurementItem','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A5D63E87-3689-419E-8DE9-E1AB794B7C59"), "EXEC db_AddFields 'MeasurementItem','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F774B2EB-632E-433E-A436-FFECA69569C8"), "EXEC db_AddFields 'MeasurementItem','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1F333259-F481-4F68-B799-5A694386911C"), "EXEC db_AddFields 'MeasurementItem','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("891BA24E-82D5-4858-82B9-8AAA581F880E"), "EXEC db_AddFields 'MeasurementItem','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7B2FE3F0-FF2E-48F3-9F55-A962B290E280"), "EXEC db_AddFields 'MutuallyExclusiveIndicator','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B34F4934-0DCB-4502-9BC6-064A247D3F16"), "EXEC db_AddFields 'MutuallyExclusiveIndicator','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2C8323A4-047D-4555-9E78-2D8A50456440"), "EXEC db_AddFields 'MutuallyExclusiveIndicator','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9F0271E2-C0C4-4C55-9A85-CB5D7593351D"), "EXEC db_AddFields 'MutuallyExclusiveIndicator','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DABEE1B3-FC8F-4F87-8FCB-B47084310722"), "EXEC db_AddFields 'MutuallyExclusiveIndicator','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DF20087D-3D19-42B9-89B0-401935B65D47"), "EXEC db_AddFields 'MutuallyExclusiveIndicator','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F0D415CF-6A66-46AE-8447-655E43B48EE6"), "EXEC db_AddFields 'MutuallyExclusiveIndicator','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("022EE8DB-3D12-4FB2-BD23-82A69E7926CD"), "EXEC db_AddFields 'MutuallyExclusiveIndicator','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D0EA42B5-3467-4DFA-A7B9-ABB23A3546AA"), "EXEC db_AddFields 'MutuallyExclusiveIndicator','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C0545629-D426-4FFB-B572-D5854CB88871"), "EXEC db_AddFields 'MutuallyExclusiveIndicator','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4860DF43-288F-431D-81FA-8BEDD1672E85"), "EXEC db_AddFields 'NetworkComponent','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F92E5C25-8693-4B0E-9D56-D8DDFE1CAD9F"), "EXEC db_AddFields 'NetworkComponent','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E42961EF-7371-47CC-987D-2744C08D0858"), "EXEC db_AddFields 'NetworkComponent','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("07D48FF1-91C4-4969-940C-0141F87D14FA"), "EXEC db_AddFields 'NetworkComponent','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C819EAEB-2E50-43B1-870C-AEB5C77F5DB8"), "EXEC db_AddFields 'NetworkComponent','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7CBFD6C2-BF1A-4826-9BB9-ACED72543D5B"), "EXEC db_AddFields 'NetworkComponent','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3FBCB5AB-95F5-4263-9745-107BEA788401"), "EXEC db_AddFields 'NetworkComponent','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("653FFBC3-4E36-40EB-BDEA-EC215B06E5B2"), "EXEC db_AddFields 'NetworkComponent','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C02DAFDB-DEAD-45E3-912D-8E2A30532377"), "EXEC db_AddFields 'NetworkComponent','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1FDBA0BC-B10F-41C4-A8D6-7D470D483AEC"), "EXEC db_AddFields 'NetworkComponent','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C73BB563-AC62-4657-8C74-C6B8807CE385"), "EXEC db_AddFields 'Object','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("071C8DF6-CF98-4636-883D-963709D87AE9"), "EXEC db_AddFields 'Object','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9F596D6A-5A91-4094-B596-627CCFEFA8AB"), "EXEC db_AddFields 'Object','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B9E1B66E-A543-4659-AD5D-6AD89C4D4FCD"), "EXEC db_AddFields 'Object','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3BC74AD6-E1B3-43E8-8A7F-1826EE6C062E"), "EXEC db_AddFields 'Object','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7D5B1503-E984-4BFC-B00F-1EAF4E1065BF"), "EXEC db_AddFields 'Object','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D9D907ED-EA4B-4731-9A1E-D19BD7E8997F"), "EXEC db_AddFields 'Object','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9461B4BB-4ED7-41B8-8247-ED8D06A5CD25"), "EXEC db_AddFields 'Object','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E1115D60-0113-4FD0-8A19-DCACA59ECAB5"), "EXEC db_AddFields 'Object','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D13E0770-25AB-4D2D-A872-C33AA1A726B9"), "EXEC db_AddFields 'Object','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F456F002-85E8-4D12-B0E3-4757E8AE1253"), "EXEC db_AddFields 'OrganizationalUnit','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("68643585-EB0A-4692-ACEE-AE9B49946F73"), "EXEC db_AddFields 'OrganizationalUnit','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B0842F6F-8794-4B6F-BBCF-971099ABDEB9"), "EXEC db_AddFields 'OrganizationalUnit','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("69A8272C-80F5-4216-9D20-3C3D5FDBCCB4"), "EXEC db_AddFields 'OrganizationalUnit','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CDFDD318-933F-4204-826D-7493093B22CB"), "EXEC db_AddFields 'OrganizationalUnit','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("90C3C84B-C2FA-4CAA-AC45-E11677C6872B"), "EXEC db_AddFields 'OrganizationalUnit','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("16F169DA-2793-4B7B-8B61-7B4A973B3CCE"), "EXEC db_AddFields 'OrganizationalUnit','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BC84CF79-2D35-4A23-8E21-E8D237ADB332"), "EXEC db_AddFields 'OrganizationalUnit','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EDB053F4-C9F4-4528-B978-AC8E663FC652"), "EXEC db_AddFields 'OrganizationalUnit','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3D81454F-F7E9-4594-829F-208B18DBEC78"), "EXEC db_AddFields 'OrganizationalUnit','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0A915EBF-475C-4C24-9BE9-4481A1DFB95A"), "EXEC db_AddFields 'Peripheral','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D513A0EF-BDF4-4CFF-927D-F3395B1534B8"), "EXEC db_AddFields 'Peripheral','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3E3D1AF9-3AD0-4628-B6D8-6B6CB1E3BA00"), "EXEC db_AddFields 'Peripheral','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AC16822B-074D-4848-BBE8-AF6C63984AEE"), "EXEC db_AddFields 'Peripheral','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5DCC02DD-4EE1-48A9-98A2-7C6EB3A303D2"), "EXEC db_AddFields 'Peripheral','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0EA37370-4029-4974-9189-464A1FC71661"), "EXEC db_AddFields 'Peripheral','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0E15A756-68E9-4A3B-8BE4-B1D34D4DD687"), "EXEC db_AddFields 'Peripheral','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("446D3D37-A65C-4212-8DBD-33E7E948DDFA"), "EXEC db_AddFields 'Peripheral','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B38C7580-C1B5-4D45-B4EC-BDBE0CA6982F"), "EXEC db_AddFields 'Peripheral','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AC5EC507-EEB4-43B8-8786-FB9692515CAE"), "EXEC db_AddFields 'Peripheral','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //ProbOfRealization
            databaseVersions.Add(new Guid("9E81D3DF-5245-4A2C-BB0D-4C35F51492FB"), "EXEC db_AddFields 'Process','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7ADC640B-DC56-44F6-AE26-77577A32C66B"), "EXEC db_AddFields 'Process','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E8F98A77-CA53-4E59-BCF5-D43EE0F1C8C5"), "EXEC db_AddFields 'Process','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FE92059E-F7B8-4E11-B287-833A7C4308D0"), "EXEC db_AddFields 'Process','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B222D96A-BF38-45D3-B09F-21724F3BD168"), "EXEC db_AddFields 'Process','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("33B2AD20-9320-46DF-B43E-38B8A366F824"), "EXEC db_AddFields 'Process','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3FDD3499-ED67-4BF7-89D3-C53761980EDE"), "EXEC db_AddFields 'Process','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1997630C-A48F-4D82-8BF4-39AE02688F56"), "EXEC db_AddFields 'Process','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B3A103ED-0763-4EE8-B5FF-4C04EF0DD15F"), "EXEC db_AddFields 'Process','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9E83C767-C150-4E60-ACC9-75912C308FD4"), "EXEC db_AddFields 'Process','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //Rationale
            databaseVersions.Add(new Guid("46EC9B97-107C-4411-BFBB-CF4086E9F1A5"), "EXEC db_AddFields 'Responsibility','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("81E8EF03-1E3F-422F-9763-B7418DF5432B"), "EXEC db_AddFields 'Responsibility','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EE1046E6-E788-44B0-84CD-D9ADF85081EF"), "EXEC db_AddFields 'Responsibility','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8B32FAC3-80F8-45B1-9511-123151F2207E"), "EXEC db_AddFields 'Responsibility','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4AFA11B6-97C7-43C2-8FEC-A05A1CE15DE9"), "EXEC db_AddFields 'Responsibility','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B81BD07D-EE6A-4C18-9A80-BEB3268D4AC0"), "EXEC db_AddFields 'Responsibility','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7F15D2AC-B90E-4E6B-B126-1DA6AB85B5DF"), "EXEC db_AddFields 'Responsibility','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B3424386-2E23-43E4-9E57-AA2AA88B91CD"), "EXEC db_AddFields 'Responsibility','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("98A157D0-092C-4563-B990-56CF1D55386C"), "EXEC db_AddFields 'Responsibility','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0BE0EF2E-D62E-4D91-BFFF-9DFFB23C229E"), "EXEC db_AddFields 'Responsibility','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9CB279A2-71E2-4810-A578-86E0317AB031"), "EXEC db_AddFields 'Role','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("706BCD15-3A37-493B-9757-4D241BCB644E"), "EXEC db_AddFields 'Role','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("32210B43-E952-417C-A9EF-BD1375FCCECB"), "EXEC db_AddFields 'Role','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3420A556-6505-45E3-8FAA-97DC86C22969"), "EXEC db_AddFields 'Role','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9C9F77A2-770E-4C75-9954-6DBA13C47201"), "EXEC db_AddFields 'Role','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4098BAED-16BD-46B2-987C-291695107238"), "EXEC db_AddFields 'Role','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9A108CB5-04A9-4393-9102-B623CEB488EC"), "EXEC db_AddFields 'Role','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EBFE3493-E04A-4B67-AA89-52BAC55B5877"), "EXEC db_AddFields 'Role','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1C8A468C-9E6F-4844-A85E-916FFD19C740"), "EXEC db_AddFields 'Role','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("90B859AE-3944-4D36-B125-1C1F795D7782"), "EXEC db_AddFields 'Role','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4C7BEAF6-D8D6-42F3-8775-E52A17977C06"), "EXEC db_AddFields 'Scenario','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E9F0D1BC-6602-46C9-976B-38319E8AB229"), "EXEC db_AddFields 'Scenario','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("95363BE8-950A-436F-A7AA-FA60FEBB8197"), "EXEC db_AddFields 'Scenario','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C0A4E1D5-AAE5-4C21-B3B6-4EB21AA339FC"), "EXEC db_AddFields 'Scenario','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E4737608-A362-4A81-9B7A-6A4B4FC72C35"), "EXEC db_AddFields 'Scenario','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0F0B22F5-ACE8-4143-9296-FDF73EA4B489"), "EXEC db_AddFields 'Scenario','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4A8C0309-EF41-4E3D-89D7-5AEEA7F61EC3"), "EXEC db_AddFields 'Scenario','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("75668FE6-8ECA-406D-8F91-0E5DD0AAE778"), "EXEC db_AddFields 'Scenario','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EE091DA0-1107-46C6-8EE7-6E9D4F92584C"), "EXEC db_AddFields 'Scenario','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B2ADEFA2-5217-44C8-8FF9-453996A300AD"), "EXEC db_AddFields 'Scenario','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //SelectorAtrribute
            databaseVersions.Add(new Guid("674BA836-EA0D-4554-9229-81C2624BA7DA"), "EXEC db_AddFields 'Skill','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("84D44E22-CEE8-48C8-88CB-864D0B702A37"), "EXEC db_AddFields 'Skill','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EDEAE8C8-28B0-49E5-B6FE-939001F0829A"), "EXEC db_AddFields 'Skill','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EBC0A743-4F76-4F3E-AF6B-F79555F9063D"), "EXEC db_AddFields 'Skill','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("43403AC2-90A9-4ED5-99ED-F6CCC1BE20EE"), "EXEC db_AddFields 'Skill','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F037C6B5-EF25-4E6D-96C0-2FBF237D90BA"), "EXEC db_AddFields 'Skill','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E60945E6-991E-4E2B-B233-55342EA0BDF4"), "EXEC db_AddFields 'Skill','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F2782D55-5188-427E-A7DD-9B46BEB758DC"), "EXEC db_AddFields 'Skill','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7DEA76C4-23EF-46A5-892C-865FA3C2758D"), "EXEC db_AddFields 'Skill','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7CB51F96-DF7F-4707-9A7E-2D1A80EDAEC1"), "EXEC db_AddFields 'Skill','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CBB7BA78-2035-433C-91E2-25C859220EFB"), "EXEC db_AddFields 'Software','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8927517E-287F-46F2-98C4-BEFDBE996351"), "EXEC db_AddFields 'Software','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A5C8D810-F75C-47AF-8BC8-814F8166C34F"), "EXEC db_AddFields 'Software','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0ABEB45B-6038-4138-85DB-2E5B1E324690"), "EXEC db_AddFields 'Software','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4B2EB8BE-8C57-4050-B394-D149D1D15A2A"), "EXEC db_AddFields 'Software','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B84C0353-9927-4DF7-B062-536DEAC990D4"), "EXEC db_AddFields 'Software','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("86EB0BFE-62F8-4E55-B56C-C8BC831267AF"), "EXEC db_AddFields 'Software','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("ACA6DE59-BC9A-42D1-8328-19951228DAE3"), "EXEC db_AddFields 'Software','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3EDA2A29-CDE1-4DE0-BDBC-454F5CC854BC"), "EXEC db_AddFields 'Software','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2F26656D-F9DB-484B-BCCE-8BF5F3381834"), "EXEC db_AddFields 'Software','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DB2442E4-4229-495F-A387-8B1E7741EF81"), "EXEC db_AddFields 'StorageComponent','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DE49DB9F-CCA0-45AC-9905-520B4336B37E"), "EXEC db_AddFields 'StorageComponent','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DB8A9D63-4011-437A-8B8A-5C9AC4E9E68A"), "EXEC db_AddFields 'StorageComponent','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F76789A1-EC9C-4280-98AA-37E1AA41FE24"), "EXEC db_AddFields 'StorageComponent','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("65F8B1ED-9E90-4E2C-9167-4A2D078C66C4"), "EXEC db_AddFields 'StorageComponent','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F7BC1356-53C1-4520-B499-4BA6CEA20B2F"), "EXEC db_AddFields 'StorageComponent','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("28E31D15-A0E3-4F2E-A6DD-A352A851D79D"), "EXEC db_AddFields 'StorageComponent','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0E280944-1DAB-48CE-A554-E3383B502DE4"), "EXEC db_AddFields 'StorageComponent','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DE5F8E70-ABE9-4F27-942E-DEC03FEA4F21"), "EXEC db_AddFields 'StorageComponent','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CCB8B611-9FB7-4F3B-951E-3284E7D3EDD6"), "EXEC db_AddFields 'StorageComponent','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("75D888B4-0B31-45F5-8A9F-F3CC73CE03D0"), "EXEC db_AddFields 'StrategicTheme','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("106D613D-D234-4E82-AA2C-ED401B872777"), "EXEC db_AddFields 'StrategicTheme','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D61284CE-6FFA-421F-BE9C-351D1BD6CD7C"), "EXEC db_AddFields 'StrategicTheme','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2BE0C1B6-C3D2-43F0-88BB-042F21B64F2F"), "EXEC db_AddFields 'StrategicTheme','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("786679EC-E993-47D2-82D7-D3DC8E33F1D1"), "EXEC db_AddFields 'StrategicTheme','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("986BC2D1-5F0C-48B6-B52A-B900C069E631"), "EXEC db_AddFields 'StrategicTheme','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F50D4A3E-FD87-4F3E-BD86-70A19B0AFDEF"), "EXEC db_AddFields 'StrategicTheme','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C7BB571F-B494-4F0B-A94E-F384236336D2"), "EXEC db_AddFields 'StrategicTheme','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8A5016FB-7ED2-46D9-9CC1-85D6F0B4C20E"), "EXEC db_AddFields 'StrategicTheme','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("08A71BBC-0660-4F1D-8A6B-94487FDA7B6B"), "EXEC db_AddFields 'StrategicTheme','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FDD1714E-24D0-4187-95F3-F1C884FA7637"), "EXEC db_AddFields 'SystemComponent','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("68DD18B3-36AA-4FE5-82E9-3B97C55D91E6"), "EXEC db_AddFields 'SystemComponent','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("46F7EC27-3B38-4044-9E32-B18C2E6F60A4"), "EXEC db_AddFields 'SystemComponent','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EECE6295-B497-40FD-B396-ADD0E241B993"), "EXEC db_AddFields 'SystemComponent','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E3293AAA-7C88-4DA6-ADC0-822BC8DC898A"), "EXEC db_AddFields 'SystemComponent','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AEF60E64-173B-41B9-86B5-9D767016ADF7"), "EXEC db_AddFields 'SystemComponent','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("052E93A1-B033-4C96-A0A5-BE9406351916"), "EXEC db_AddFields 'SystemComponent','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3061FEAC-2855-4642-BEE2-5A84E64EA4DB"), "EXEC db_AddFields 'SystemComponent','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9E13B73F-DA57-4FDE-A525-230864B8518B"), "EXEC db_AddFields 'SystemComponent','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C141A80F-E106-4D8E-BF42-CA7F077604F4"), "EXEC db_AddFields 'SystemComponent','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //TimeDifferenceIndicator
            databaseVersions.Add(new Guid("A0AC9806-96A1-416C-B3B1-7965D94E1AF8"), "EXEC db_AddFields 'TimeReference','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2054EE02-9F80-4896-A7F1-E88D45A2A4AF"), "EXEC db_AddFields 'TimeReference','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E1DD1A95-5D33-4A50-B6E2-CC055A16B1F3"), "EXEC db_AddFields 'TimeReference','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("07980922-B7DF-469E-BB7F-C3BBF191FF8D"), "EXEC db_AddFields 'TimeReference','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("89A69420-A84C-41C8-B3F1-2FB047A1A290"), "EXEC db_AddFields 'TimeReference','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("214AA454-1855-45F6-A93A-C4BEABE26F7E"), "EXEC db_AddFields 'TimeReference','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C8EF223C-426E-413E-9AEF-B99BEB76C40F"), "EXEC db_AddFields 'TimeReference','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DC8DB95D-50C6-44D5-B1B7-537EEC2527A8"), "EXEC db_AddFields 'TimeReference','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D270B5F5-90F0-4480-AEE4-999837557D70"), "EXEC db_AddFields 'TimeReference','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FBC72EC7-64C5-4A62-828C-8B5639EC997D"), "EXEC db_AddFields 'TimeReference','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AA11DFC4-ADCB-4FD4-9317-DC978888B499"), "EXEC db_AddFields 'TimeScheme','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("50501F76-5480-499A-A98D-9742D8FDE133"), "EXEC db_AddFields 'TimeScheme','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F6ADE7F9-53FB-4EAE-85B8-7E8B3DD93608"), "EXEC db_AddFields 'TimeScheme','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("ED39CAE6-4F8F-4CFD-B814-9BD92891BACD"), "EXEC db_AddFields 'TimeScheme','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F4F39978-D4C2-4678-9BAC-59EE643B22DE"), "EXEC db_AddFields 'TimeScheme','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7D723C16-C980-4C7A-8085-8530CD2A156E"), "EXEC db_AddFields 'TimeScheme','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("93016D5D-D6FD-4D9E-A7BF-A47D3E5FCD54"), "EXEC db_AddFields 'TimeScheme','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BFF890B6-37F8-40CE-9161-5C18836D1B03"), "EXEC db_AddFields 'TimeScheme','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D00947AF-B8D3-4647-BF09-79EE0551D2F7"), "EXEC db_AddFields 'TimeScheme','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4AE7AEDE-F701-4477-A32A-1C98218608DA"), "EXEC db_AddFields 'TimeScheme','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("84FDD204-3E55-441E-8771-0ACDD095C621"), "EXEC db_AddFields 'TimeUnit','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A7394846-131F-4ECA-AA14-27DCC2B8BB3F"), "EXEC db_AddFields 'TimeUnit','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1E2D7EA5-FBA5-4332-8896-07D7762A5851"), "EXEC db_AddFields 'TimeUnit','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0D78DC79-0B58-428A-9138-AEA696AE143A"), "EXEC db_AddFields 'TimeUnit','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4B299CDB-6781-4206-A324-C2552270E43A"), "EXEC db_AddFields 'TimeUnit','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C178E5C0-EB29-44E5-A385-E1FC3D64D880"), "EXEC db_AddFields 'TimeUnit','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7BC3958B-367A-4179-95B6-4EC690DD0B6E"), "EXEC db_AddFields 'TimeUnit','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9B528192-AB36-4578-BD90-F2E0C158779A"), "EXEC db_AddFields 'TimeUnit','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F4634B78-4A71-4E07-89F8-6A9CE43DC032"), "EXEC db_AddFields 'TimeUnit','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("61F6D4AC-E632-4C01-B209-A4DDC740B051"), "EXEC db_AddFields 'TimeUnit','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");

            databaseVersions.Add(new Guid("4FDD708D-266A-4D4E-9061-12C07CB67A3F"), "EXEC db_AddFields 'PhysicalSoftwareComponent','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3E3F73EA-0684-4608-82BD-EA6DE6002034"), "EXEC db_AddFields 'PhysicalSoftwareComponent','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7CA14993-9800-44EA-B18D-EFA4CBBB1A4E"), "EXEC db_AddFields 'PhysicalSoftwareComponent','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("586B0659-13AE-438F-83B2-0F0A74AE8CD0"), "EXEC db_AddFields 'PhysicalSoftwareComponent','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D3A632F8-024A-4FAC-B62C-E90218797A82"), "EXEC db_AddFields 'PhysicalSoftwareComponent','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CBB89651-311B-4BD5-BBB7-24C910221644"), "EXEC db_AddFields 'PhysicalSoftwareComponent','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("84142F38-D3FA-4B2E-8F80-AB723A24A983"), "EXEC db_AddFields 'PhysicalSoftwareComponent','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F6220A29-B1B0-4CAD-87D4-31E208758E45"), "EXEC db_AddFields 'PhysicalSoftwareComponent','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BB0FD9FB-75FC-46B5-97B5-C37EAA6413B4"), "EXEC db_AddFields 'PhysicalSoftwareComponent','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("64D1ECBB-6FEC-4472-8F22-B70787A2A607"), "EXEC db_AddFields 'PhysicalSoftwareComponent','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1F620847-B302-42B0-85B9-91C95623A3DA"), "EXEC db_AddFields 'PhysicalSoftwareComponent','CustomField1','System.String','General','',0,98,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A6398BBF-8185-44E5-AE4B-F16A004209B2"), "EXEC db_AddFields 'PhysicalSoftwareComponent','CustomField2','System.String','General','',0,99,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CE8A9F01-1196-4289-A557-3B3C31026759"), "EXEC db_AddFields 'PhysicalSoftwareComponent','CustomField3','System.String','General','',0,100,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("625D2ED0-F55F-42B3-9D1A-20F8A5D4B99A"), "EXEC db_AddFields 'LogicalITInfrastructureComponent','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("400D5F26-89FC-46EA-8EB9-1B9D64F2AFF9"), "EXEC db_AddFields 'LogicalITInfrastructureComponent','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EF151B1B-3747-4ABF-A3E9-415DFD9F62A4"), "EXEC db_AddFields 'LogicalITInfrastructureComponent','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BDC028D8-5EDB-452D-8176-C36390E22BAC"), "EXEC db_AddFields 'LogicalITInfrastructureComponent','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E84C9A16-4803-4556-AA24-F208E0973E2D"), "EXEC db_AddFields 'LogicalITInfrastructureComponent','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8B618107-769B-4230-B083-43E003A85AF1"), "EXEC db_AddFields 'LogicalITInfrastructureComponent','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D90A84EB-7CF5-44D2-BE7E-CD860BA0CF74"), "EXEC db_AddFields 'LogicalITInfrastructureComponent','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D3C8089B-5D8A-4BED-BBDF-E4E908AF44FC"), "EXEC db_AddFields 'LogicalITInfrastructureComponent','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7A71D49C-3FAD-4449-82F9-C9C945FA530B"), "EXEC db_AddFields 'LogicalITInfrastructureComponent','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A464FB03-5516-40CF-9745-8CD26FB32C36"), "EXEC db_AddFields 'LogicalITInfrastructureComponent','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("49627D2F-BCCF-45BF-8651-AAD9608FACE2"), "EXEC db_AddFields 'LogicalITInfrastructureComponent','CustomField1','System.String','General','',0,98,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CA6CEA1C-444F-4183-982B-A6599AB3AA20"), "EXEC db_AddFields 'LogicalITInfrastructureComponent','CustomField2','System.String','General','',0,99,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("27EE1A76-169E-4E0E-981D-DCE8BC37AA35"), "EXEC db_AddFields 'LogicalITInfrastructureComponent','CustomField3','System.String','General','',0,100,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9A6341BE-C18B-4A30-A7F3-588321B8F427"), "EXEC db_AddFields 'ComputingComponent','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9ECA0AE1-5D1C-424A-BC18-A3FA3F6686B2"), "EXEC db_AddFields 'ComputingComponent','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FB7EEBAC-0D12-4620-8D59-48BE4A29749D"), "EXEC db_AddFields 'ComputingComponent','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("64812FF8-C315-4B8F-9A2F-393A3C07E0CE"), "EXEC db_AddFields 'ComputingComponent','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8BF38378-4399-4345-985E-2C1F721606BE"), "EXEC db_AddFields 'ComputingComponent','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0A3D7696-432B-4F74-846C-FA90D38D3B88"), "EXEC db_AddFields 'ComputingComponent','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E0A8B411-3F88-4DCA-AEA9-EE8683C17BE5"), "EXEC db_AddFields 'ComputingComponent','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0467C1C3-39E6-48DA-9246-E97CBDB2BCD7"), "EXEC db_AddFields 'ComputingComponent','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1347BD98-832E-4B3E-AFDF-17BB5B1C3FB3"), "EXEC db_AddFields 'ComputingComponent','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D09F3935-38E8-45C0-8880-D23BAA2D0D25"), "EXEC db_AddFields 'ComputingComponent','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("385E771F-14AA-4D98-9E92-50256BE789D0"), "EXEC db_AddFields 'ComputingComponent','CustomField1','System.String','General','',0,98,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("23A7B395-DCEF-4DDB-AD0E-A6BA04BAFCFB"), "EXEC db_AddFields 'ComputingComponent','CustomField2','System.String','General','',0,99,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E0E14B2D-20E2-4D4B-9D98-75228BF97971"), "EXEC db_AddFields 'ComputingComponent','CustomField3','System.String','General','',0,100,0|MAJOR METAMODEL CHANGE SAPPI ONLY");

            databaseVersions.Add(new Guid("4ED1265E-0ECB-47E3-99F9-1510F56CE7F0"), "EXEC db_AddFields 'PeripheralComponent','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E5D8F708-109E-447C-9643-ADA4E54890C4"), "EXEC db_AddFields 'PeripheralComponent','PeripheralComponentType','PeripheralType','General','Type',0,1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EB90A8E2-0C1D-46E9-860F-358E2B144052"), "EXEC db_AddFields 'PeripheralComponent','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E6C75005-2922-4A69-8952-067CAA41249F"), "EXEC db_AddFields 'PeripheralComponent','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FC325AB8-08B7-41E8-8459-C6253C1A495F"), "EXEC db_AddFields 'PeripheralComponent','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("264AFCF3-BABD-493B-94CF-5EA58378BC05"), "EXEC db_AddFields 'PeripheralComponent','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E3370C3B-C542-4C2E-8AD6-1ECDD2F9E2DF"), "EXEC db_AddFields 'PeripheralComponent','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A08AC30B-FF85-4C75-B5C9-71C0D6216444"), "EXEC db_AddFields 'PeripheralComponent','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("14B13517-B62E-481E-8F3D-C593C99E8832"), "EXEC db_AddFields 'PeripheralComponent','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E88DAFA6-CBDA-454C-B0BE-43C0E6CF19C4"), "EXEC db_AddFields 'PeripheralComponent','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8326A958-B7AD-4705-96F3-57EE782CA89B"), "EXEC db_AddFields 'PeripheralComponent','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4B4131F7-403B-4701-B4DD-8BA3DB4A3048"), "EXEC db_AddFields 'PeripheralComponent','CustomField1','System.String','General','',0,98,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7AAB3A28-7774-4864-9E7A-43DC023825D2"), "EXEC db_AddFields 'PeripheralComponent','CustomField2','System.String','General','',0,99,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("59B64B20-D68B-4EAF-AB80-0E639D1B373C"), "EXEC db_AddFields 'PeripheralComponent','CustomField3','System.String','General','',0,100,0|MAJOR METAMODEL CHANGE SAPPI ONLY");

            databaseVersions.Add(new Guid("82A16E06-4F09-4681-861F-868274F081B0"), "EXEC db_AddFields 'Network','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6F715C49-0E3B-4985-8F96-0B5C6D14BC6C"), "EXEC db_AddFields 'Network','NetworkType','NetworkType','General','Network Type',0,1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0A01E31E-F964-4E1C-9F58-95B97EF0E700"), "EXEC db_AddFields 'Network','DataStorageNetworkType','DataStorageNetworkType','General','',0,2,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("86292FC4-30EE-4458-8884-8A5224FA4562"), "EXEC db_AddFields 'Network','ConnectionType','ConnectionType','General','Type',0,3,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CFD2CD8D-6ADA-4902-AAA2-479319D3F436"), "EXEC db_AddFields 'Network','ConnectionSpeed','System.String','General','Speed',0,4,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0E9C3767-F017-4D49-B233-36FCB608802C"), "EXEC db_AddFields 'Network','ConnectionSize','System.String','General','Size',0,5,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F84237EA-199B-44B7-98D2-2AA2B6CDD6F6"), "EXEC db_AddFields 'Network','Range','System.String','General','Range',0,6,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A5B8DC4A-1C90-4108-9686-E4941FE85173"), "EXEC db_AddFields 'Network','Managed','System.String','General','Managed',0,7,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("379D2087-1B74-45AE-A840-662504FF55F3"), "EXEC db_AddFields 'Network','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("79721233-39DB-4CEB-BBAA-200254176B66"), "EXEC db_AddFields 'Network','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F5226691-7627-4BCB-A05E-086479F45B31"), "EXEC db_AddFields 'Network','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("420ED3C8-F153-4509-9413-26F0F04CB474"), "EXEC db_AddFields 'Network','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EAB35A43-B8C8-42C4-9C3E-6D2B91541BE6"), "EXEC db_AddFields 'Network','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6F29CA93-206A-4DE3-A175-88BCEA6E8C21"), "EXEC db_AddFields 'Network','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4BF8207C-7D9E-408B-8E02-5E48E5FAFB61"), "EXEC db_AddFields 'Network','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E717E705-DA49-4265-892F-CB00B72111F7"), "EXEC db_AddFields 'Network','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4F07E9C4-C5A3-40E6-B8E8-9551805DAA03"), "EXEC db_AddFields 'Network','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6FDE9D98-C476-4789-86BA-7A05ED2AA365"), "EXEC db_AddFields 'Network','CustomField1','System.String','General','',0,98,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1B2C1AE9-9231-403A-9815-DC7EDBD92DCF"), "EXEC db_AddFields 'Network','CustomField2','System.String','General','',0,99,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B965C8F6-898D-484B-8587-A7149708CCAE"), "EXEC db_AddFields 'Network','CustomField3','System.String','General','',0,100,0|MAJOR METAMODEL CHANGE SAPPI ONLY");

            databaseVersions.Add(new Guid("6AB5432D-4194-4F10-BD76-DBB3B167712A"), "EXEC db_AddFields 'PhysicalDataComponent','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("021FFD9F-1332-4236-8CE6-3EDB756935FF"), "EXEC db_AddFields 'PhysicalDataComponent','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2A4ABF36-4DDF-4F67-9F64-94D0A8E4C2D6"), "EXEC db_AddFields 'PhysicalDataComponent','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B1A5E918-FEB4-4FAF-A93E-2C5AFF165840"), "EXEC db_AddFields 'PhysicalDataComponent','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1E3414FC-FB6C-4F5F-BB16-1C4FC0649B6C"), "EXEC db_AddFields 'PhysicalDataComponent','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("91FB28F4-7734-47AF-B9DD-19A5DA0FA464"), "EXEC db_AddFields 'PhysicalDataComponent','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("549803E0-A156-4E0F-825B-92D792454245"), "EXEC db_AddFields 'PhysicalDataComponent','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F6631BF4-C2D9-4E1E-965A-A72FEB1A3A73"), "EXEC db_AddFields 'PhysicalDataComponent','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EFA8B5EA-A399-46EE-B316-79F318B6DC50"), "EXEC db_AddFields 'PhysicalDataComponent','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0B87A043-2BD8-4F72-9CC2-BA067650C970"), "EXEC db_AddFields 'PhysicalDataComponent','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("B7FC6BFB-53EF-48D2-9EA5-FBAFE631B094"), "EXEC db_AddFields 'PhysicalDataComponent','CustomField1','System.String','General','',0,98,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("C40D81E6-B296-4BA8-90FA-27E9686BE621"), "EXEC db_AddFields 'PhysicalDataComponent','CustomField2','System.String','General','',0,99,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("1CD71F7F-FE97-4B16-9843-77F54346F4BF"), "EXEC db_AddFields 'PhysicalDataComponent','CustomField3','System.String','General','',0,100,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3598400D-98C9-4B62-979C-7E0A316C62A1"), "EXEC db_AddFields 'ITInfrastructureEnvironment','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9BB21CDD-A6F9-47E4-8F78-51F3DEEF4D63"), "EXEC db_AddFields 'ITInfrastructureEnvironment','Description','System.String','General','General description for each object',0,80,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E3C14074-6343-41F8-BB73-411CDAB6EA7B"), "EXEC db_AddFields 'ITInfrastructureEnvironment','Abbreviation','System.String','General','',0,81,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EED89F3F-0CE1-453F-A571-13E82D1EBA06"), "EXEC db_AddFields 'ITInfrastructureEnvironment','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AAC885DD-4893-4C6E-81DF-2BA2EC23D906"), "EXEC db_AddFields 'ITInfrastructureEnvironment','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F90E4A76-56AD-4FCD-8E1F-3B196F7C0DE3"), "EXEC db_AddFields 'ITInfrastructureEnvironment','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EAFB1538-2908-4CC1-A0E0-0D31C95C30A1"), "EXEC db_AddFields 'ITInfrastructureEnvironment','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5069ACF2-AE09-42C1-9970-821BA246A1BB"), "EXEC db_AddFields 'ITInfrastructureEnvironment','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F7FCA1BE-3F25-451C-BC9E-06C06FF26710"), "EXEC db_AddFields 'ITInfrastructureEnvironment','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B808B226-1C4A-4B46-8DEA-4681E3EDD1AB"), "EXEC db_AddFields 'ITInfrastructureEnvironment','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("200134E3-C8FA-44ED-B8F2-C71833B54E69"), "EXEC db_AddFields 'TimeIndicator','CustomField1','System.String','General','',0,98,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("514C8FB5-FFB0-4A3A-A270-818C92E3B7C6"), "EXEC db_AddFields 'TimeIndicator','CustomField2','System.String','General','',0,99,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("E0BD4E78-87B9-4EA6-BF6C-09F6CEBC6E54"), "EXEC db_AddFields 'TimeIndicator','CustomField3','System.String','General','',0,100,0|MAJOR METAMODEL CHANGE SAPPI ONLY");

            databaseVersions.Add(new Guid("02986FFD-07FB-46DB-A603-722D5CD9EAD4"), "EXEC db_addClassAssociations 'Entity','PhysicalSoftwareComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("03058B4D-21C0-4A95-80BC-C5AD4A11A079"), "EXEC db_addClassAssociations 'ComputingComponent','NetworkComponent','ConnectedTo','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("03B63779-17A4-447A-8A96-63097BAB5586"), "EXEC db_addClassAssociations 'Object','PhysicalSoftwareComponent','Decomposition','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("03BD6CA1-7BEA-4A22-8643-8CD11728F109"), "EXEC db_addClassAssociations 'DataTable','PhysicalSoftwareComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("04073DD5-15BF-410B-9225-4B48DD328B25"), "EXEC db_addClassAssociations 'PeripheralComponent','Object','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("062E5833-1334-4D4E-9F47-1C55F72272CA"), "EXEC db_addClassAssociations 'Function','ComputingComponent','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("08EF5CD8-D0DE-4AA3-BC20-D131F12B93A7"), "EXEC db_addClassAssociations 'ComputingComponent','Object','Classification','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("09C1D0BD-9837-408C-8F8A-32BE976326C7"), "EXEC db_addClassAssociations 'Location','ComputingComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0A566F54-C3B3-43ED-B098-CBAB5950C150"), "EXEC db_addClassAssociations 'Implication','PeripheralComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0ABE8918-AE6C-4171-BEAC-D82E94ECBB79"), "EXEC db_addClassAssociations 'PeripheralComponent','Rationale','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0B7E0C86-0591-4157-BDD4-D5928D5B3359"), "EXEC db_addClassAssociations 'ComputingComponent','PhysicalSoftwareComponent','Dependencies','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0C87C919-F23A-4620-9BB7-B2BBC7311A13"), "EXEC db_addClassAssociations 'Implication','PhysicalSoftwareComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0DBFA2EC-A275-4DCC-9C44-A199FD5F0004"), "EXEC db_addClassAssociations 'PeripheralComponent','PhysicalSoftwareComponent','Dependencies','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("12382557-AE17-4054-92E1-A9561EF9E02A"), "EXEC db_addClassAssociations 'Process','PhysicalSoftwareComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("12543722-CDC8-47F7-B7B7-A96BA499BEF7"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','Auxiliary','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("12DEA08A-48C3-440C-9EB2-A4784B80B21F"), "EXEC db_addClassAssociations 'Employee','PhysicalSoftwareComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1353283B-ED30-4252-B2B1-F8B903C19C2E"), "EXEC db_addClassAssociations 'Object','PeripheralComponent','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("16C45C09-7D4B-4E48-A8B1-C131DC3A7820"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','ComputingComponent','Dependencies','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("17EC778D-189C-4396-A6F2-6DBCD9B2BB34"), "EXEC db_addClassAssociations 'StrategicTheme','PeripheralComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("18D58065-A050-42E4-A3C5-350A4B763061"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','PeripheralComponent','Dependencies','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1ABC7D35-7420-4AE1-89E9-0F28C1A570FC"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','Classification','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1C4503B9-5785-4511-8804-AEAB117C5C45"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','Dependencies','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1CBBC52D-1BBB-413D-B811-87ABBFE7EBB0"), "EXEC db_addClassAssociations 'Object','PeripheralComponent','Decomposition','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1CEEE233-A319-430E-81C8-F1D18CD9ACF4"), "EXEC db_addClassAssociations 'ComputingComponent','DataSchema','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1DB3ABC0-791E-4A61-97D0-965CEADFDD6D"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','Activity','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1E034784-E063-4955-9EE7-FD641D3C2B62"), "EXEC db_addClassAssociations 'Function','ComputingComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1E682E07-3FD1-4A72-B682-95C3210A20D7"), "EXEC db_addClassAssociations 'Function','PhysicalSoftwareComponent','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("203E622D-A38E-4842-9434-942C1C02790A"), "EXEC db_addClassAssociations 'ComputingComponent','Entity','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("204D07B4-2E40-4E42-AD97-A4F55AA7A504"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','Process','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("21130234-8E87-4B46-B9B9-E48DBA165F85"), "EXEC db_addClassAssociations 'Function','PeripheralComponent','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("214D3282-EBAE-4748-B1E6-3C66673A6828"), "EXEC db_addClassAssociations 'Implication','ComputingComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("21F4A58D-2544-47F3-A406-5E4A8C9CE4A1"), "EXEC db_addClassAssociations 'MeasurementItem','PeripheralComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("22D30AAB-6BBC-486C-B11F-6592F604F74F"), "EXEC db_addClassAssociations 'PeripheralComponent','StorageComponent','ConnectedTo','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("23FBF621-8C63-410A-916D-CF4FF98118AC"), "EXEC db_addClassAssociations 'PeripheralComponent','Activity','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("244E09BC-2090-4218-938D-F74C15C8F361"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','Process','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("25163136-9820-4FAF-AF8E-64C6753DD62C"), "EXEC db_addClassAssociations 'Employee','ComputingComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("273CF038-EF76-4D11-A379-8DB13F275A53"), "EXEC db_addClassAssociations 'DataSchema','PeripheralComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("27CFE91F-69F1-4BA0-B2FA-293FA87CDF87"), "EXEC db_addClassAssociations 'PeripheralComponent','Job','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2A42DBAD-0909-434F-80D6-2CE942768C81"), "EXEC db_addClassAssociations 'CSF','ComputingComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2A841608-185B-4A1E-B3F9-B3780D5B8B27"), "EXEC db_addClassAssociations 'PeripheralComponent','ComputingComponent','ConnectedTo','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2C7471CC-A72F-487D-8CFA-AAF5596F6021"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','Entity','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2F0E6EC4-F6BC-48AC-BC5B-FADEDCB0A5A6"), "EXEC db_addClassAssociations 'ComputingComponent','StorageComponent','ConnectedTo','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2F582670-A1EC-4AC5-8729-DD3C61B66737"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','OrganizationalUnit','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("30DBADED-17DA-423E-9704-3F988605E195"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','NetworkComponent','Dependencies','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("32101D4D-D6C9-4D4B-B8AE-CED74A8A802D"), "EXEC db_addClassAssociations 'Location','PhysicalSoftwareComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("32F3278A-C666-4293-9934-9D0EF8812C81"), "EXEC db_addClassAssociations 'StorageComponent','ComputingComponent','Dependencies','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("34B1B326-2AAA-4D7D-BDF2-0ADF379032C6"), "EXEC db_addClassAssociations 'MeasurementItem','ComputingComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("356862E9-8DCA-475F-80D2-39188771F27C"), "EXEC db_addClassAssociations 'Job','ComputingComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("35AE1CDB-6FDD-449F-8A87-0A33ED8DB75B"), "EXEC db_addClassAssociations 'Object','ComputingComponent','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("38711A7E-F47F-4650-843F-87D82B4FBB85"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','Implication','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("38B8E050-CFA1-4F46-88C9-2E75F1ABFEDF"), "EXEC db_addClassAssociations 'ComputingComponent','OrganizationalUnit','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("39487DA7-BDB4-40EE-8FC5-ABE5E9061A11"), "EXEC db_addClassAssociations 'DataSchema','PhysicalSoftwareComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3AE293E6-1103-4C93-B127-E7789CAA9A4A"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','Rationale','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3C27EE49-56B6-4ED0-B08D-B141FCAEB922"), "EXEC db_addClassAssociations 'OrganizationalUnit','ComputingComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3D448316-2ECD-4871-830C-9F9CD1A9BBD9"), "EXEC db_addClassAssociations 'ComputingComponent','Location','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3E595431-B4F8-40F0-8896-ABCD89A34998"), "EXEC db_addClassAssociations 'ComputingComponent','Object','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3EBFB13C-5D4F-454A-AAF8-EC8B5883FE24"), "EXEC db_addClassAssociations 'PeripheralComponent','DataSchema','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("420BD7D4-75EF-44CC-9FE5-1DCBDBDE6DBA"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','ComputingComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("42A62C83-E933-4475-9FB8-9A61E02D6363"), "EXEC db_addClassAssociations 'PeripheralComponent','JobPosition','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("42FEF439-5CF8-46FA-96D8-07DD9EDF1519"), "EXEC db_addClassAssociations 'PeripheralComponent','PhysicalSoftwareComponent','Dependencies','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("430FC007-2013-47E1-B501-B79E1F164C1C"), "EXEC db_addClassAssociations 'PeripheralComponent','Activity','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("43C78717-04E7-4DEF-A9F0-FA51FD293D7F"), "EXEC db_addClassAssociations 'StorageComponent','PeripheralComponent','ConnectedTo','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("456A47F9-0C00-422B-A972-BD22548A9DA3"), "EXEC db_addClassAssociations 'Function','PeripheralComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("45AA7493-3390-422A-BE00-0510BC971B2D"), "EXEC db_addClassAssociations 'DataView','PeripheralComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("481CD0FE-013A-4436-9DA9-73354F3CAA53"), "EXEC db_addClassAssociations 'ComputingComponent','PhysicalSoftwareComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("49444EE9-7920-42CF-B896-4B4619440894"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','Job','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4AD41133-D591-429A-B4B6-39A41A4F33F8"), "EXEC db_addClassAssociations 'ComputingComponent','Process','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4D065ADB-AE6E-41A3-BBF7-458A670CC3BF"), "EXEC db_addClassAssociations 'PeripheralComponent','Entity','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4F1F5894-5E11-4207-8C33-90F6E3EA9669"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','DataTable','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5051EF23-7D9A-4597-9C11-D9B047E82254"), "EXEC db_addClassAssociations 'Entity','PeripheralComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("51614476-EFA8-41D5-9EED-F143FEB63949"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','CSF','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("543DEE08-CA4B-4E2D-B00D-6943FF24D5CA"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','Object','Decomposition','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("554071F0-48EF-4C7E-AD56-D0A44B794654"), "EXEC db_addClassAssociations 'Object','PeripheralComponent','Classification','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("58A1FF08-24AA-403B-A5FB-53D5014F64B1"), "EXEC db_addClassAssociations 'ComputingComponent','StorageComponent','Dependencies','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("59776817-8377-4C20-8D1B-7D56D1D668F3"), "EXEC db_addClassAssociations 'DataSchema','ComputingComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5C91CE83-5F5F-4C7A-B860-1E5640576817"), "EXEC db_addClassAssociations 'Activity','ComputingComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5D6EE8D4-03AD-425F-B55D-D0D72FD80841"), "EXEC db_addClassAssociations 'PeripheralComponent','ComputingComponent','Dependencies','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5F315FC3-F0FD-423B-877C-E89D5A54C8A6"), "EXEC db_addClassAssociations 'ComputingComponent','Function','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("603A32F3-93B2-472A-8240-0C08F0337D21"), "EXEC db_addClassAssociations 'DataView','ComputingComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("61291298-DEED-478E-B575-9A16F8037927"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','Object','Classification','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("61937A91-3BF6-4020-8DDA-7953F66E0DA4"), "EXEC db_addClassAssociations 'PeripheralComponent','Object','Decomposition','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6405BB9E-AD20-42CC-A1CE-A13914ED0FCA"), "EXEC db_addClassAssociations 'PeripheralComponent','Process','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("68E6103C-7FEA-47D5-B7C0-9D0C2A6B9267"), "EXEC db_addClassAssociations 'PeripheralComponent','Function','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("69736A94-802F-47A9-B15D-77331659FCB3"), "EXEC db_addClassAssociations 'PeripheralComponent','CSF','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("69CA341B-B640-453A-9E10-FAC971046A20"), "EXEC db_addClassAssociations 'ComputingComponent','Process','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6BF2A47F-583D-4B95-BCCD-BAB1AC528422"), "EXEC db_addClassAssociations 'ComputingComponent','DataTable','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6D1AF7DB-4FD3-46E7-A0FE-C69F9E090828"), "EXEC db_addClassAssociations 'Job','PeripheralComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6DCF9565-13BE-4AE6-9196-F5CAC1B57E78"), "EXEC db_addClassAssociations 'ComputingComponent','Rationale','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6E5B98C1-1E17-4AD4-8CB4-234C6D89DE0B"), "EXEC db_addClassAssociations 'MeasurementItem','PhysicalSoftwareComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6E99EB84-C1BB-4142-BFA5-44887FD89CAD"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','Function','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6F112DB0-78A8-42BD-AEFA-74C506444884"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','Activity','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7385BC10-D3E5-4EDA-8753-FBA151F73627"), "EXEC db_addClassAssociations 'PeripheralComponent','PeripheralComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("73FC46D8-6C71-4FF5-8FA5-FCDACCF7CF21"), "EXEC db_addClassAssociations 'PeripheralComponent','Function','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("743CF6C2-7469-4464-A9B4-A97E579C6B08"), "EXEC db_addClassAssociations 'Rationale','PeripheralComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("764D19BA-08A8-469A-B783-888CCF087B4D"), "EXEC db_addClassAssociations 'ComputingComponent','NetworkComponent','Dependencies','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("780C3346-D2D0-4724-891A-3A3B042F9D47"), "EXEC db_addClassAssociations 'DataView','PhysicalSoftwareComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("79163C2A-8E3E-4021-BD14-696A050663B6"), "EXEC db_addClassAssociations 'Activity','PhysicalSoftwareComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7A092631-AA94-4D42-8940-193BB5BD8C29"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','Object','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7BEEF041-FE7F-452A-9AC5-CD86A991DD2C"), "EXEC db_addClassAssociations 'PeripheralComponent','Object','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7BFA9A4D-89D7-497B-B065-3118AD4AFD85"), "EXEC db_addClassAssociations 'DataTable','ComputingComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7D963EFC-F95C-4F53-8FAB-6C2027296278"), "EXEC db_addClassAssociations 'Employee','PeripheralComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7E550F6F-2637-4191-8D05-AE14AEBE73FF"), "EXEC db_addClassAssociations 'StrategicTheme','ComputingComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7FC5587C-8BF1-4EB4-9950-29CB00348BB8"), "EXEC db_addClassAssociations 'PeripheralComponent','GovernanceMechanism','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("80875105-F824-4A49-B080-D99136174414"), "EXEC db_addClassAssociations 'ComputingComponent','Rationale','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("820C47E4-51AA-48A7-8053-8501366A3726"), "EXEC db_addClassAssociations 'ComputingComponent','CSF','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("83349FB2-45CA-4D31-80F9-F4872CD02417"), "EXEC db_addClassAssociations 'ComputingComponent','Job','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("83796ACC-3801-4770-8AFD-1F312E1C3898"), "EXEC db_addClassAssociations 'ComputingComponent','GovernanceMechanism','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("83E448AB-8A31-4A99-9EBD-A835364ACA91"), "EXEC db_addClassAssociations 'CSF','PeripheralComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("85556DDE-8A0A-4264-A157-DC31EE4118B8"), "EXEC db_addClassAssociations 'Activity','PeripheralComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("86480267-03A6-4213-8112-D14A3674ED9C"), "EXEC db_addClassAssociations 'ComputingComponent','Rationale','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("87FD784D-B5AC-4241-A94B-F7E5DBE88B5D"), "EXEC db_addClassAssociations 'Object','PhysicalSoftwareComponent','Classification','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8866B8F5-B97F-4B7A-B171-1CC7199F6C05"), "EXEC db_addClassAssociations 'Object','PhysicalSoftwareComponent','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8899077B-63DF-4DAC-8607-D4D3D3048B7C"), "EXEC db_addClassAssociations 'ComputingComponent','Implication','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8973BFCB-0AB0-415A-B018-8E1984D01479"), "EXEC db_addClassAssociations 'JobPosition','ComputingComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8A80A69F-E55B-418B-A6F6-A5BC542CF370"), "EXEC db_addClassAssociations 'Process','PeripheralComponent','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8A8E12B6-BF09-4020-9A70-02ED492FA04B"), "EXEC db_addClassAssociations 'Object','PeripheralComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8ADDC04C-BEFB-4545-8A7F-9FCB1D941DB1"), "EXEC db_addClassAssociations 'PeripheralComponent','Rationale','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8D32389F-88E4-4ADF-83ED-E762399111D9"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','Rationale','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8DC7E435-BE66-414F-9C3B-21C77E6AC6D1"), "EXEC db_addClassAssociations 'PeripheralComponent','StrategicTheme','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8FA446BC-2B6B-4698-87B7-AA6AB9B56791"), "EXEC db_addClassAssociations 'StorageComponent','PhysicalSoftwareComponent','Dependencies','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("907F5747-9537-46A8-8D78-E526FD9B8078"), "EXEC db_addClassAssociations 'Object','PhysicalSoftwareComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("91CBEA26-5A1C-433E-899D-9426790F6E75"), "EXEC db_addClassAssociations 'JobPosition','PhysicalSoftwareComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("92B66E17-7024-40F3-A0EF-41AA7DB9E040"), "EXEC db_addClassAssociations 'PeripheralComponent','DataTable','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("935A47D1-A74A-4275-B287-4453DC1D47A7"), "EXEC db_addClassAssociations 'Process','ComputingComponent','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9364767B-D345-4E3D-AAC7-E0A6EF6CC685"), "EXEC db_addClassAssociations 'ComputingComponent','StrategicTheme','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9416B110-FE00-4D9A-9E20-99C8BDA7BA06"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("948A306A-DC3D-49F5-B89F-879CA9624D39"), "EXEC db_addClassAssociations 'Object','ComputingComponent','Decomposition','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("95E11192-4887-4E29-9D3E-9777E5220CB4"), "EXEC db_addClassAssociations 'PeripheralComponent','Rationale','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("979ED8D2-A1F5-4525-880F-F86CA47717AC"), "EXEC db_addClassAssociations 'NetworkComponent','ComputingComponent','ConnectedTo','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("97DD3D55-383D-49ED-974C-ED122DE096DC"), "EXEC db_addClassAssociations 'JobPosition','PeripheralComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("99706AD0-D093-4630-86C0-08FC84269F3E"), "EXEC db_addClassAssociations 'ComputingComponent','Activity','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9BA20EFF-C0BE-4664-9B5C-C61FC06C78C7"), "EXEC db_addClassAssociations 'ComputingComponent','PeripheralComponent','ConnectedTo','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A09E19EE-EF21-4411-BB9E-CA37BF6A1680"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','Employee','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A0FC772C-D0DA-43C9-992C-FE38C1371B3D"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','GovernanceMechanism','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A198236D-D621-4D40-B350-67EEDFE078B1"), "EXEC db_addClassAssociations 'ComputingComponent','Object','Decomposition','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A256626A-74A0-47A3-A675-28532BC872FF"), "EXEC db_addClassAssociations 'PeripheralComponent','DataView','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A2EA75D5-3E29-41E9-90C5-1674004207DB"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','ComputingComponent','Dependencies','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A348C020-9FF3-4B9A-9231-A7773487AAAA"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','JobPosition','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A77953A4-62E5-415B-967E-1A1459A2B360"), "EXEC db_addClassAssociations 'ComputingComponent','JobPosition','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A9A24E9A-DCE4-4303-B1C6-5B61C730B668"), "EXEC db_addClassAssociations 'PeripheralComponent','MeasurementItem','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AD9842D2-F7C0-483E-8A8F-35AF7CCA903E"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','DataSchema','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("ADA6A137-12BC-445C-87CA-98011C190CBD"), "EXEC db_addClassAssociations 'ComputingComponent','Activity','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AED31983-50A3-4DEA-B4D1-FEDA320CF748"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B046FBC6-8D3D-4F5A-B121-D3CB5D10AAEA"), "EXEC db_addClassAssociations 'PeripheralComponent','ComputingComponent','Dependencies','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B06A144F-CB89-47B1-B899-FFDEE85384F2"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','Location','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B325680E-983C-4513-BACA-A4132433E008"), "EXEC db_addClassAssociations 'PeripheralComponent','OrganizationalUnit','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B3C9F713-0972-438B-8695-003BC20B5B9A"), "EXEC db_addClassAssociations 'CSF','PhysicalSoftwareComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B5F62F65-DD35-4469-9907-80D62FEAB8B7"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','StrategicTheme','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B811EC9D-B912-4B09-9AFF-5563FF94F360"), "EXEC db_addClassAssociations 'Activity','PhysicalSoftwareComponent','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B88ABBB3-753A-4629-889D-D7D54B0EA756"), "EXEC db_addClassAssociations 'Object','ComputingComponent','Classification','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BCC10C7F-5F4D-4AEF-A9C8-B0BEFDCD924B"), "EXEC db_addClassAssociations 'Rationale','ComputingComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BEAD260C-95FC-4ED2-A77D-2FD46AFCDED0"), "EXEC db_addClassAssociations 'PeripheralComponent','Location','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BF742DFF-8BAB-43F9-BA41-41BDCD9932EF"), "EXEC db_addClassAssociations 'ComputingComponent','PhysicalSoftwareComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C027371E-D38F-470B-8012-C73D4E6DCB10"), "EXEC db_addClassAssociations 'OrganizationalUnit','PeripheralComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C0BBBF15-6E1E-466B-A314-FDF55B6F42A9"), "EXEC db_addClassAssociations 'ComputingComponent','Object','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C3678AA8-10B3-4AB8-A8CC-DA0685F9E91A"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','ComputingComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CEF2AC79-5627-4D89-B250-F96EA92BCFB1"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','Rationale','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D2649D36-3B8D-48F8-91E2-DDAC1C5D5E77"), "EXEC db_addClassAssociations 'PeripheralComponent','Employee','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D3448F93-7C65-4690-BCC0-947A3DF13E98"), "EXEC db_addClassAssociations 'ComputingComponent','ComputingComponent','Decomposition','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D4C9EA78-9770-4708-B930-B91610518983"), "EXEC db_addClassAssociations 'GovernanceMechanism','ComputingComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D72FAFC1-A149-45D5-B151-90B2AC91178C"), "EXEC db_addClassAssociations 'Location','PeripheralComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D7BC7AA0-2D5F-41F7-B415-2A8B4EF926AE"), "EXEC db_addClassAssociations 'Activity','ComputingComponent','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D9ABEDCE-6C9D-410B-883C-DD96AEDE46B9"), "EXEC db_addClassAssociations 'Rationale','PhysicalSoftwareComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DA02CAFF-90DB-411C-A82A-E1E72AE0B4E5"), "EXEC db_addClassAssociations 'GovernanceMechanism','PeripheralComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DA148702-9151-49EE-B26F-978AA9D69EC5"), "EXEC db_addClassAssociations 'DataTable','PeripheralComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DB4D2C25-3B92-42CA-B1D1-C4A2EBBF43BE"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','Function','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DB9141CB-824A-462F-90B9-D5F05730306A"), "EXEC db_addClassAssociations 'ComputingComponent','ComputingComponent','ConnectedTo','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DD68D5CC-47C7-4235-9637-861B3A564211"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','PeripheralComponent','Dependencies','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DE3EE60A-5747-495D-9525-104BDD05A0B6"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','MeasurementItem','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DE607A72-302D-40E2-AE72-1E878703B217"), "EXEC db_addClassAssociations 'NetworkComponent','PhysicalSoftwareComponent','Dependencies','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DE7CBE6B-5FDF-4BF1-A1F4-8FABA56AFCF9"), "EXEC db_addClassAssociations 'StrategicTheme','PhysicalSoftwareComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E0E0DD35-37D4-437B-BF2F-51C024CE9D03"), "EXEC db_addClassAssociations 'PeripheralComponent','PeripheralComponent','ConnectedTo','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E1055E9F-9CC1-45D6-B85D-580FD022D4D6"), "EXEC db_addClassAssociations 'PeripheralComponent','Object','Classification','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E14C73A0-581F-4539-A673-6E6D803307F3"), "EXEC db_addClassAssociations 'ComputingComponent','Function','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E421D3CB-12C3-4625-8B4E-532E14A61282"), "EXEC db_addClassAssociations 'ComputingComponent','ComputingComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E599FD64-8758-441D-916E-B951766F0351"), "EXEC db_addClassAssociations 'StorageComponent','ComputingComponent','ConnectedTo','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E6AF151A-0F6D-4009-AE78-6B69EC109A11"), "EXEC db_addClassAssociations 'ComputingComponent','Employee','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E6BB9BC6-9435-455D-B332-BC3AED13E17F"), "EXEC db_addClassAssociations 'PeripheralComponent','ComputingComponent','ConnectedTo','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E751BEEE-E0D0-4D1B-BF60-5F9E0967D46C"), "EXEC db_addClassAssociations 'Process','PhysicalSoftwareComponent','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E858914E-B394-437A-A73A-890C9072C47D"), "EXEC db_addClassAssociations 'Process','ComputingComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EA06CDAB-329B-4C92-958E-A20920D36916"), "EXEC db_addClassAssociations 'ComputingComponent','PeripheralComponent','ConnectedTo','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("ED4CF435-30AA-409B-B3C6-2F7DEDF0E29B"), "EXEC db_addClassAssociations 'Job','PhysicalSoftwareComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("ED9127C2-269C-4697-A96D-E49D59ED9758"), "EXEC db_addClassAssociations 'OrganizationalUnit','PhysicalSoftwareComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("ED92B126-692E-4F52-927D-697933924BD8"), "EXEC db_addClassAssociations 'Object','ComputingComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EF18BDDC-45E3-4B46-ABBB-0FA574AD89ED"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','Object','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EFDB54F6-90EF-478F-ADE9-4C6BFD0D8776"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','DataTable','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F1A07834-C521-4775-9905-01F7DE39542A"), "EXEC db_addClassAssociations 'NetworkComponent','PeripheralComponent','ConnectedTo','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F23756F2-7DDB-4876-938F-9494C16E190A"), "EXEC db_addClassAssociations 'PeripheralComponent','NetworkComponent','ConnectedTo','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F24A0EA0-30D1-44C0-8FDF-01EC861419DA"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','Decomposition','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F3733A5C-BD62-4F8D-B71A-499A765E6184"), "EXEC db_addClassAssociations 'Process','PeripheralComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F3B8C3C6-BB9B-4CD0-8077-8D8A198B3BA4"), "EXEC db_addClassAssociations 'ComputingComponent','PhysicalSoftwareComponent','Dependencies','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F3C77C21-498F-44BA-A5F0-B1EC9AF1C32C"), "EXEC db_addClassAssociations 'Activity','PeripheralComponent','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F40FFA49-8A9E-4ABD-9B20-1C71A79C490D"), "EXEC db_addClassAssociations 'GovernanceMechanism','PhysicalSoftwareComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F4403742-01E5-431D-BDD0-EE3C4C4BFA42"), "EXEC db_addClassAssociations 'PeripheralComponent','Rationale','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F473BF45-DF9A-4C81-8947-897C95A4246A"), "EXEC db_addClassAssociations 'Entity','ComputingComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F4E797C2-5DC1-4A26-B760-09C6E52DE1DD"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','Rationale','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F638C37F-7803-4C6C-ADBA-23985C66805C"), "EXEC db_addClassAssociations 'Function','PhysicalSoftwareComponent','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F6CEE05D-CC87-463A-B5FC-75D3FD92CC42"), "EXEC db_addClassAssociations 'PeripheralComponent','Process','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F70EAE0D-C6EC-44A9-831A-9C04D719B045"), "EXEC db_addClassAssociations 'PeripheralComponent','Implication','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F7F9BE13-CF67-4126-90D1-E673EA640AB9"), "EXEC db_addClassAssociations 'ComputingComponent','DataView','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F90D1749-7B7E-4033-8216-8B834EADA531"), "EXEC db_addClassAssociations 'ComputingComponent','Rationale','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FB05B64C-BD3F-4394-BCBB-95E3DFE63376"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','DataView','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FB21E04E-08C7-4414-8237-829734CD756A"), "EXEC db_addClassAssociations 'ComputingComponent','MeasurementItem','Mapping','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FE1CE586-8BAD-451E-BC5B-F9137AADDD9C"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','StorageComponent','Dependencies','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FECF99F8-3081-467C-8190-741770D19000"), "EXEC db_addClassAssociations 'PeripheralComponent','NetworkComponent','Dependencies','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");

            databaseVersions.Add(new Guid("00A7B404-4DB5-4126-9E56-C72211B8C8D0"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','Process','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("022FFBF0-88A1-4BB1-9763-CE2007F2B16B"), "EXEC db_addArtifacts 'ComputingComponent','Function','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("02AA1D43-FD9C-4D93-A7E8-8366498E473A"), "EXEC db_addArtifacts 'PeripheralComponent','ComputingComponent','Dependencies','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("030031B6-9270-4E7D-BBF6-8CBDBFCDC07F"), "EXEC db_addArtifacts 'NetworkComponent','PeripheralComponent','ConnectedTo','ConnectionType',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("05D12161-0821-4AD1-BE4D-1304AE5C01A5"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','ComputingComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("06A431FF-E9F5-46E2-8C48-8C491DC56ED8"), "EXEC db_addArtifacts 'Implication','ComputingComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0733BDBD-D5C5-4E89-A5FD-1211617134D5"), "EXEC db_addArtifacts 'PeripheralComponent','Function','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("09D3E307-3B63-4EF9-8D00-15A4782CC391"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','ComputingComponent','Dependencies','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0B16AF9F-73BE-4424-A5E4-58039898CB4D"), "EXEC db_addArtifacts 'Job','PeripheralComponent','Mapping','Role',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0C65B18C-717D-4CE1-B60B-57466BCF5B8E"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','Location','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0C89003D-45CE-4BAC-BF11-CC5DAC17A592"), "EXEC db_addArtifacts 'Entity','PhysicalSoftwareComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0D4BCA75-9AAC-4314-BD66-581620BD1A08"), "EXEC db_addArtifacts 'NetworkComponent','PhysicalSoftwareComponent','Dependencies','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0EA3FA4F-2B6D-4FE0-A659-0574F8B287F1"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','DynamicFlow','DataSchema',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1304337B-0F45-42D7-B139-C59EC886A393"), "EXEC db_addArtifacts 'ComputingComponent','ComputingComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1399D941-25E7-4146-8978-810B8E6D98FF"), "EXEC db_addArtifacts 'ComputingComponent','NetworkComponent','Dependencies','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("14CFF8C0-ABCF-4050-9920-B99BD556E068"), "EXEC db_addArtifacts 'PeripheralComponent','Process','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("15F28D52-A2CC-49B2-819C-91CDE8B2BC61"), "EXEC db_addArtifacts 'PeripheralComponent','Object','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1738A487-FBB0-4910-B20B-9B88B3283AEF"), "EXEC db_addArtifacts 'DataTable','PeripheralComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("18B31E78-37B9-4702-9E12-3846E55C5251"), "EXEC db_addArtifacts 'Function','PeripheralComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1A9F3D50-865E-4A1A-B1AC-D06F796E4A96"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','GovernanceMechanism','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1B1FE1BE-5F2E-41B3-ADF1-BDE8AF3C98A2"), "EXEC db_addArtifacts 'ComputingComponent','StorageComponent','ConnectedTo','ConnectionSpeed',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1BD98A22-035E-4ABA-A207-D69C3F142485"), "EXEC db_addArtifacts 'Object','PeripheralComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1C1DAC65-D49E-4044-9D7D-8896437B1872"), "EXEC db_addArtifacts 'JobPosition','PhysicalSoftwareComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1C772957-EE04-464B-A806-00F0F7790B5B"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','Function','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1DEC5C8D-1593-499E-B7EE-BC6E15F0B3CE"), "EXEC db_addArtifacts 'Function','PhysicalSoftwareComponent','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1EB8F440-3EC4-47C9-A0DA-5FA99EBF9DC8"), "EXEC db_addArtifacts 'Object','PeripheralComponent','DynamicFlow','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1F1A936A-05C5-4851-B874-213FD36B0817"), "EXEC db_addArtifacts 'Object','ComputingComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("20B3115B-B8F7-4F37-BF6E-E8E88787DE49"), "EXEC db_addArtifacts 'Employee','PeripheralComponent','Mapping','Role',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("21EAF5B6-D3BE-452C-AA71-748A8365CCE5"), "EXEC db_addArtifacts 'ComputingComponent','NetworkComponent','ConnectedTo','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("22856030-F8FD-4FC9-BEF4-1EAE1F094B6D"), "EXEC db_addArtifacts 'Function','ComputingComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("22C7940E-4843-400F-A0C1-15235F297F4E"), "EXEC db_addArtifacts 'ComputingComponent','StorageComponent','ConnectedTo','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2331795F-CEFC-4218-961D-AEB910D00419"), "EXEC db_addArtifacts 'Object','PhysicalSoftwareComponent','DynamicFlow','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2392F516-3B0A-42BB-88E3-8CAA86F6A093"), "EXEC db_addArtifacts 'ComputingComponent','Process','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("239BC89E-6941-4B3B-A250-9AD4733D96EA"), "EXEC db_addArtifacts 'Activity','PhysicalSoftwareComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("23F8D387-F726-47AD-A8E1-42818A2521AC"), "EXEC db_addArtifacts 'ComputingComponent','ComputingComponent','ConnectedTo','ConnectionType',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("240872E3-0DFD-4336-9382-2ABBA1E6C17A"), "EXEC db_addArtifacts 'ComputingComponent','PhysicalSoftwareComponent','Dependencies','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("24089967-D55A-4670-842D-5DCF1E3D4DF6"), "EXEC db_addArtifacts 'PeripheralComponent','Employee','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("25022896-0A43-41E2-AFC7-D34D2311EFB7"), "EXEC db_addArtifacts 'ComputingComponent','PeripheralComponent','ConnectedTo','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("26047361-DDDE-4CC7-AD9B-2965D3AB2879"), "EXEC db_addArtifacts 'PeripheralComponent','CSF','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("263FB318-2791-441D-8D6D-E411BBEC2EF0"), "EXEC db_addArtifacts 'JobPosition','ComputingComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("290FAF3B-254D-4789-A990-1DEBF50DF50E"), "EXEC db_addArtifacts 'ComputingComponent','Location','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2C031C56-CE20-4AC6-932D-A0AD62E1BB2B"), "EXEC db_addArtifacts 'OrganizationalUnit','PeripheralComponent','Mapping','Role',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2CC44986-E318-4628-B561-851052E95ED5"), "EXEC db_addArtifacts 'Object','PhysicalSoftwareComponent','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2E1A68BB-60DC-49BC-AF35-80329BFFB981"), "EXEC db_addArtifacts 'Employee','PhysicalSoftwareComponent','Mapping','Role',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2EBC08DE-A545-418F-976F-E4D108C9ED3E"), "EXEC db_addArtifacts 'PeripheralComponent','Function','DynamicFlow','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2EE45F05-163C-450C-B3E4-3D297F4B2EA2"), "EXEC db_addArtifacts 'ComputingComponent','Object','DynamicFlow','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2FE730A1-47AF-4BCE-B703-05C09EECFFE3"), "EXEC db_addArtifacts 'ComputingComponent','Process','DynamicFlow','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("30296C78-EDB5-44C9-B6B3-F40B195ADD36"), "EXEC db_addArtifacts 'PeripheralComponent','Rationale','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3199B524-B79D-4254-A19C-3CC1D59A266C"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','DynamicFlow','Job',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("331F5DB2-10B4-4943-9474-C360B2969C00"), "EXEC db_addArtifacts 'Function','PeripheralComponent','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("34147E76-2E9C-4459-AA5D-403CE63B0783"), "EXEC db_addArtifacts 'ComputingComponent','Implication','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("368DA75D-6CB9-4023-B79A-672801B2026F"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','Job','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("36B25FC0-21BA-463E-B076-D7D4D909AB48"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','MeasurementItem','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("36EB456B-1474-4033-881D-C359C293AFD6"), "EXEC db_addArtifacts 'PeripheralComponent','Activity','DynamicFlow','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("371BBB6C-D197-40BB-99DF-11BF2325169E"), "EXEC db_addArtifacts 'PeripheralComponent','ComputingComponent','ConnectedTo','ConnectionType',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("380728CB-2704-4EAF-A213-2333518521E5"), "EXEC db_addArtifacts 'PeripheralComponent','ComputingComponent','ConnectedTo','ConnectionSpeed',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3A53239C-C18F-4B87-A3B8-497CF9E1F3FD"), "EXEC db_addArtifacts 'Employee','ComputingComponent','Mapping','Role',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3CDDCA82-60A5-4FE3-A618-AF08AD41FEB6"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','DynamicFlow','GovernanceMechanism',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3D7A7062-56E3-41A7-9B7D-0CAFC446B86A"), "EXEC db_addArtifacts 'PeripheralComponent','Entity','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3E685514-B45F-464B-B03B-1080A320A04A"), "EXEC db_addArtifacts 'PeripheralComponent','Process','DynamicFlow','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3F7C384C-4D9F-4894-80D3-AD096006A2C8"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','DataTable','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3FC45E6D-5984-47D6-B097-E090B3965740"), "EXEC db_addArtifacts 'PeripheralComponent','StorageComponent','ConnectedTo','ConnectionSpeed',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("41C83088-C220-44A6-A067-68D29F54297C"), "EXEC db_addArtifacts 'Process','PeripheralComponent','DynamicFlow','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("41F2F5F4-1915-4B5D-9F28-C5709BAA1910"), "EXEC db_addArtifacts 'GovernanceMechanism','PeripheralComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("433E39A0-A989-4EA6-BA76-D86E39B32029"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','PeripheralComponent','Dependencies','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("449A7A4B-9AD9-41E5-BDE9-88EE9B71EC69"), "EXEC db_addArtifacts 'ComputingComponent','Process','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("450C8139-D2F4-4C30-B0FE-3676DC4816CE"), "EXEC db_addArtifacts 'PeripheralComponent','DataTable','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("462DC3EC-D182-4640-910A-6E446FD5621F"), "EXEC db_addArtifacts 'ComputingComponent','Rationale','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("475C281F-128A-4E04-ACFA-5E7884F32540"), "EXEC db_addArtifacts 'Function','PhysicalSoftwareComponent','DynamicFlow','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("49BCCD18-C94D-42C9-BFB4-8D0E9776928B"), "EXEC db_addArtifacts 'Employee','PeripheralComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("50062307-E03E-4F4A-A595-6AF9A1E265CC"), "EXEC db_addArtifacts 'StorageComponent','ComputingComponent','Dependencies','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("50226149-9248-412A-AA06-C52FD1D930B6"), "EXEC db_addArtifacts 'ComputingComponent','Activity','DynamicFlow','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("524897E7-CA2B-4B02-A348-9D7757FEFB50"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','DataSchema','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5330B0C1-2814-4CCB-A736-96D61B430C2F"), "EXEC db_addArtifacts 'DataTable','PhysicalSoftwareComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("537D55F8-F284-42F5-990C-A38D6BAFF3A8"), "EXEC db_addArtifacts 'PeripheralComponent','MeasurementItem','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("538E2F97-7D82-42C5-B91D-9177A4D72571"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','DataTable','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("53AE955B-86B5-4503-A8A1-ED2459196F4F"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','PeripheralComponent','Dependencies','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("55E385C3-9165-4371-A543-00375BF14343"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','Decomposition','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("56E3C2DD-314E-4986-9CCC-646672A88B71"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','DataView','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("56F82635-1D2E-484A-98F2-70389995B95E"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','Classification','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("57AA444E-9269-4BFF-B627-C251B44B97D4"), "EXEC db_addArtifacts 'ComputingComponent','PhysicalSoftwareComponent','Dependencies','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("589F5F29-CDB8-45DB-BE48-2B6EA4F1D81C"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','DynamicFlow','DataView',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("58D7D1FB-91B7-434E-A1EE-0B928E6F4630"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','DynamicFlow','Process',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("59F136E3-4FEC-48BC-9A57-5A525593DBE5"), "EXEC db_addArtifacts 'Object','ComputingComponent','Classification','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5A33D27F-2451-495F-AB49-3A0594141FC1"), "EXEC db_addArtifacts 'Process','ComputingComponent','DynamicFlow','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5B964EEF-B8E6-449B-ACDB-FD2F5FC28DDC"), "EXEC db_addArtifacts 'CSF','ComputingComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5CCF2CC8-7ECB-41F0-8BCA-64DE3C6FD4B0"), "EXEC db_addArtifacts 'MeasurementItem','ComputingComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5D283D38-6490-4CDD-826D-830C56857792"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','DynamicFlow','OrganizationalUnit',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5DCA07F0-A66B-423E-88C1-5DCC5F9C70B1"), "EXEC db_addArtifacts 'PeripheralComponent','DataView','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5F98535A-0530-4031-9FED-A1D747A7549E"), "EXEC db_addArtifacts 'Object','PeripheralComponent','Decomposition','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("60348DDE-9E0A-4CA4-87F0-F5D69D0AD1FB"), "EXEC db_addArtifacts 'OrganizationalUnit','ComputingComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("61096AC3-7169-421E-8CF4-2E668785CCCD"), "EXEC db_addArtifacts 'PeripheralComponent','StrategicTheme','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("611F25E2-B587-4EF1-A224-C713CE953615"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','StrategicTheme','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6194E881-1E4C-44C0-8E64-E43B16369DA9"), "EXEC db_addArtifacts 'JobPosition','PhysicalSoftwareComponent','Mapping','Role',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("61B058EC-0151-414A-A3B9-7DCE79E27095"), "EXEC db_addArtifacts 'PeripheralComponent','Rationale','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6219E388-2EC2-432C-A78C-11B4BAD38383"), "EXEC db_addArtifacts 'Process','PhysicalSoftwareComponent','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("623FEDEE-0098-442A-9954-C44510706BB8"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','OrganizationalUnit','Mapping','Responsibility',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("624BE611-DBAC-43DC-9FC4-C5F298519650"), "EXEC db_addArtifacts 'Object','ComputingComponent','Decomposition','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("629FF4D5-A603-4092-818D-B442F2615490"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','Rationale','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("62D19385-919D-4D71-ADD5-2C5F04AF9A89"), "EXEC db_addArtifacts 'ComputingComponent','PhysicalSoftwareComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("62D38D14-3221-4642-B291-67484A0A6363"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6404EA4C-E850-4A9C-9AA3-3C0D5FC932E4"), "EXEC db_addArtifacts 'StrategicTheme','PhysicalSoftwareComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6468CFF9-9936-4894-90BC-8D57933D7A7D"), "EXEC db_addArtifacts 'ComputingComponent','GovernanceMechanism','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("64B3CE1A-6105-4DFC-BE61-BB98E616DA34"), "EXEC db_addArtifacts 'ComputingComponent','DataView','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("66C1F6CC-8121-454B-A82D-0B81A79B416D"), "EXEC db_addArtifacts 'Job','PhysicalSoftwareComponent','Mapping','Role',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6702167E-ACF7-44AB-B6C4-C70FD357F1B0"), "EXEC db_addArtifacts 'NetworkComponent','PeripheralComponent','ConnectedTo','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("679778C7-DFAD-4E65-BCBC-35D391599FB5"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','DynamicFlow','Role',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("68EF35A2-A216-4474-AB6A-BF44477A8D68"), "EXEC db_addArtifacts 'Object','PhysicalSoftwareComponent','Decomposition','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("69074882-D069-4CEB-9117-E059BFBBA651"), "EXEC db_addArtifacts 'StrategicTheme','ComputingComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6A6E719F-9C8D-404F-A874-B4823CE9B73F"), "EXEC db_addArtifacts 'StorageComponent','PeripheralComponent','ConnectedTo','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6A97FA6A-E25D-4BBD-A7C4-27581CA649D4"), "EXEC db_addArtifacts 'PeripheralComponent','Activity','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6BAB08B5-E137-494C-9831-7D48A3AFE76D"), "EXEC db_addArtifacts 'PeripheralComponent','PeripheralComponent','ConnectedTo','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6CD380C2-80AE-4BE0-B99B-F2A915438D08"), "EXEC db_addArtifacts 'ComputingComponent','Function','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6CED63F8-321E-4035-AE94-E6B736F3BF89"), "EXEC db_addArtifacts 'ComputingComponent','ComputingComponent','ConnectedTo','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6D380BE1-5D5E-454A-93FA-A936F8C8C390"), "EXEC db_addArtifacts 'PeripheralComponent','Activity','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6DD789D9-4D05-42DA-B74A-BE6852944FE2"), "EXEC db_addArtifacts 'ComputingComponent','PeripheralComponent','ConnectedTo','ConnectionType',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6E63C813-70EF-488A-9EF9-5352A8FEC5DF"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','Entity','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6EB5025F-61AB-4E83-9C64-D04619BEDE83"), "EXEC db_addArtifacts 'PeripheralComponent','DataSchema','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6FC7B90A-6CC9-4A96-B307-8C6C336E58F9"), "EXEC db_addArtifacts 'DataSchema','ComputingComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7069F372-8337-44DE-9946-47F10BA7D58A"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','DynamicFlow','JobPosition',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("711560B6-8C34-415C-B739-6404EA20A9D5"), "EXEC db_addArtifacts 'Employee','ComputingComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("71849F59-6C03-49C6-AC5D-22FBC32CAE43"), "EXEC db_addArtifacts 'Object','PeripheralComponent','Classification','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7218E7E8-AC3B-454C-A7DB-5DA889D899BF"), "EXEC db_addArtifacts 'OrganizationalUnit','PhysicalSoftwareComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("74BD1681-A731-4E31-83EC-9F92164B2B07"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','DynamicFlow','Activity',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("76DED3DB-A87D-4AB8-BF27-8809872CA58C"), "EXEC db_addArtifacts 'StorageComponent','PeripheralComponent','ConnectedTo','ConnectionSpeed',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("78743610-E5A8-47A7-A231-06D4D78079BB"), "EXEC db_addArtifacts 'PeripheralComponent','ComputingComponent','ConnectedTo','ConnectionSpeed',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("795E91CD-0603-4338-91C6-A9432A323C76"), "EXEC db_addArtifacts 'Location','PhysicalSoftwareComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7C1222FF-1B50-4076-9481-586B5DD0D901"), "EXEC db_addArtifacts 'PeripheralComponent','PeripheralComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7CA7753A-700A-404D-8A41-DED5D3B0B069"), "EXEC db_addArtifacts 'JobPosition','ComputingComponent','Mapping','Role',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7D00DAF8-FEC5-4CD6-ACFC-8E6B02A8D164"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','Employee','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7E023143-83D5-4D98-9C91-8B639F155B8C"), "EXEC db_addArtifacts 'Job','PeripheralComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7E6C63DC-7404-4A9C-A2DE-D95B9564D9CA"), "EXEC db_addArtifacts 'PeripheralComponent','PhysicalSoftwareComponent','Dependencies','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7F7DCDDB-C252-4F88-ACBB-D726644BB86A"), "EXEC db_addArtifacts 'ComputingComponent','Activity','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("7FD145CF-D5D3-49F2-9B89-7B96411E8B55"), "EXEC db_addArtifacts 'PeripheralComponent','NetworkComponent','ConnectedTo','ConnectionType',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8039D2F1-9BE1-42A6-98C5-A2E0458157D5"), "EXEC db_addArtifacts 'ComputingComponent','PhysicalSoftwareComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("806EBFC7-F8DA-4B91-B0DD-5D017CD11C47"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','Function','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("808F6C3D-5895-4A8D-9B8C-EA2D320105B9"), "EXEC db_addArtifacts 'Activity','PhysicalSoftwareComponent','DynamicFlow','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("811D88AC-DF82-4482-8F76-1CB3DCBD10AC"), "EXEC db_addArtifacts 'PeripheralComponent','PeripheralComponent','ConnectedTo','ConnectionType',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("831AC557-235E-48F3-8EEC-BB86A276242D"), "EXEC db_addArtifacts 'Object','PeripheralComponent','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("88377935-8BD7-4C7F-AC95-9D695A2C28B4"), "EXEC db_addArtifacts 'MeasurementItem','PeripheralComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("88D2D147-1EF5-404C-A81F-73ABB63E5A04"), "EXEC db_addArtifacts 'ComputingComponent','MeasurementItem','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("88FFF2A3-91BE-4A75-A325-E11B53496322"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','Rationale','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8A697AD0-9114-48DD-87FE-FF5B3ABABE89"), "EXEC db_addArtifacts 'StorageComponent','PhysicalSoftwareComponent','Dependencies','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8B98E02D-D191-41DA-8581-D7DA98CC3C98"), "EXEC db_addArtifacts 'PeripheralComponent','NetworkComponent','ConnectedTo','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8BB497BC-B142-4750-9067-357A56F44CD0"), "EXEC db_addArtifacts 'OrganizationalUnit','ComputingComponent','Mapping','Role',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8D05E9D3-84E9-4011-837E-232ED55CCA13"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','Dependencies','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8E5C84AD-51E9-4A48-873A-FA7E4C8E09C5"), "EXEC db_addArtifacts 'PeripheralComponent','StorageComponent','ConnectedTo','ConnectionType',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8E8211C4-5F53-4565-A5A1-B0F8E07BB197"), "EXEC db_addArtifacts 'Function','PeripheralComponent','DynamicFlow','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9002C7BD-EC42-4AF7-B3E8-7DEF7A82597C"), "EXEC db_addArtifacts 'ComputingComponent','Object','Decomposition','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("90912A48-BB49-4A20-BB1D-065BACE3D81E"), "EXEC db_addArtifacts 'Process','PeripheralComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("92429C38-046F-41E6-B16E-DD3ED712928F"), "EXEC db_addArtifacts 'ComputingComponent','ComputingComponent','Decomposition','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("92D7ACAA-19A8-44F2-9E70-4303040B1677"), "EXEC db_addArtifacts 'Function','PhysicalSoftwareComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("94018C36-012C-40AA-91AE-3D62E737FA4E"), "EXEC db_addArtifacts 'DataSchema','PhysicalSoftwareComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("940CAFC4-C78A-4B5F-B0D3-67BE06F15D03"), "EXEC db_addArtifacts 'Activity','PeripheralComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("96EE32A4-28B5-4E39-B305-F257CDAFFD2D"), "EXEC db_addArtifacts 'ComputingComponent','CSF','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("970105F4-688E-42CE-9B17-3F8B511312C3"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','Rationale','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9960F4FF-40D5-42B9-93F9-30148FE4B4EC"), "EXEC db_addArtifacts 'StorageComponent','ComputingComponent','ConnectedTo','ConnectionType',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9B737724-56F2-4C57-AE15-040FD46EEAE5"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','OrganizationalUnit','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9BFB7B62-82C1-4589-8E10-EA56C1CC1986"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','Implication','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9DA0CE9B-0F54-4EE2-A7DD-8ABD23B5CE84"), "EXEC db_addArtifacts 'ComputingComponent','Activity','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9E88A424-92CB-4BAC-B5CF-B6D5F6DFA138"), "EXEC db_addArtifacts 'Implication','PeripheralComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9F8A78B1-B2A4-4074-8D6D-52ACE31C5067"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','Object','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A11AFAD8-EE43-4352-84D1-B140ECB8F4BD"), "EXEC db_addArtifacts 'GovernanceMechanism','ComputingComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A1946D79-397D-47C5-A557-20A3EBBCC2ED"), "EXEC db_addArtifacts 'ComputingComponent','StorageComponent','Dependencies','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A29848F0-BF8E-47FB-94C4-712EE55325F7"), "EXEC db_addArtifacts 'GovernanceMechanism','PhysicalSoftwareComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A3380DB5-5B3A-45FC-8ECF-D3181F7B9DEB"), "EXEC db_addArtifacts 'DataView','PeripheralComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A4136989-B063-4407-B250-1EE4AEB3B4B3"), "EXEC db_addArtifacts 'CSF','PhysicalSoftwareComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A5DFC382-A3EE-45DF-A058-CD0140DDAB82"), "EXEC db_addArtifacts 'ComputingComponent','NetworkComponent','ConnectedTo','ConnectionSpeed',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A69A87AE-D51C-4804-B0DA-A274B6BCE0FE"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','CSF','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A7F53EB3-F98D-4C12-A7F6-F5A04D3A5636"), "EXEC db_addArtifacts 'ComputingComponent','PeripheralComponent','ConnectedTo','ConnectionType',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AA55E413-A868-49E1-A593-487C8D020994"), "EXEC db_addArtifacts 'PeripheralComponent','GovernanceMechanism','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AB1EFE55-792D-4838-B6D0-1481A1373B23"), "EXEC db_addArtifacts 'Activity','PhysicalSoftwareComponent','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AB59488B-DDBF-416C-B224-7897F97DE296"), "EXEC db_addArtifacts 'Job','ComputingComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("ABCE7F50-B5FD-4A18-8094-92DCBFE6475B"), "EXEC db_addArtifacts 'PeripheralComponent','Implication','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AD0EF000-A183-47CD-89B2-14C2DFC5D83D"), "EXEC db_addArtifacts 'NetworkComponent','PeripheralComponent','ConnectedTo','ConnectionSpeed',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AD0FB187-0D13-45F2-B984-9CCA93B5A07A"), "EXEC db_addArtifacts 'Rationale','PhysicalSoftwareComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AD48B6CB-C36A-4067-B591-6CCB326656F8"), "EXEC db_addArtifacts 'PeripheralComponent','ComputingComponent','ConnectedTo','ConnectionType',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AD8933F9-7D31-4F0B-B2C9-72E157D56EA2"), "EXEC db_addArtifacts 'PeripheralComponent','Job','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AE20C281-2E3A-42F1-9E49-C51F77F06B01"), "EXEC db_addArtifacts 'Function','ComputingComponent','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AE21BC0E-76C6-49A3-A28B-9A04530DB5FC"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','Object','Classification','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AEC33D41-8E93-4AB3-A7B6-917E19C659F2"), "EXEC db_addArtifacts 'Object','ComputingComponent','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AF1AC3C0-C55F-4BD1-99F4-3774A52A99F0"), "EXEC db_addArtifacts 'Employee','PhysicalSoftwareComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AF77ABE6-B384-468E-8E02-09ABC5D69360"), "EXEC db_addArtifacts 'Activity','ComputingComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AFC454D6-ABF4-47F5-9366-20BDEF88C1A4"), "EXEC db_addArtifacts 'Job','PhysicalSoftwareComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AFF407FC-D871-4045-8610-72C2C1F901B7"), "EXEC db_addArtifacts 'StorageComponent','ComputingComponent','ConnectedTo','ConnectionSpeed',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B1A6AC9B-DDE8-4AEE-B852-3DE1F0C5C680"), "EXEC db_addArtifacts 'PeripheralComponent','Location','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B2181FE2-4A07-4E9E-AFD3-0F93DF2CD2C0"), "EXEC db_addArtifacts 'PeripheralComponent','NetworkComponent','Dependencies','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B22F25AC-A6AB-4FC6-BCE6-7C6929068BD3"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','DynamicFlow','DataColumn',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B3176A9F-9F72-44D0-BB38-7C6060987E80"), "EXEC db_addArtifacts 'PeripheralComponent','PeripheralComponent','ConnectedTo','ConnectionSpeed',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B51A8257-8894-4CE7-9546-E045D16C824E"), "EXEC db_addArtifacts 'PeripheralComponent','Process','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B58BABCA-2400-46CC-BC8B-398F2AFB1DF8"), "EXEC db_addArtifacts 'Function','ComputingComponent','DynamicFlow','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B73EB4C7-6C3B-4AFF-8332-9314F9F716EF"), "EXEC db_addArtifacts 'OrganizationalUnit','PeripheralComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B7D44DC4-1550-4E2B-91B2-6978DFDB85AB"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','DynamicFlow','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B81A4F1D-6B24-43FE-9895-B5A946EF918E"), "EXEC db_addArtifacts 'ComputingComponent','PeripheralComponent','ConnectedTo','ConnectionSpeed',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BA0E4FC1-4C75-470E-A825-0551D0B634A5"), "EXEC db_addArtifacts 'Entity','ComputingComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BA1BBD1A-26DF-46B9-89D9-35EC26644294"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','Process','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BB49BDAC-F507-4C61-AFA5-7F3181DE7A00"), "EXEC db_addArtifacts 'Activity','PeripheralComponent','DynamicFlow','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BBA0FE16-7956-4625-9276-25B2F4C7EA3B"), "EXEC db_addArtifacts 'Location','ComputingComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BBAE82E3-1A35-403C-8537-9EFF27B73581"), "EXEC db_addArtifacts 'DataSchema','PeripheralComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BDE697BC-E9CB-4A59-A335-A55886D3A200"), "EXEC db_addArtifacts 'OrganizationalUnit','PhysicalSoftwareComponent','Mapping','Role',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BE66D43F-CC27-44AD-B334-DEA12A460200"), "EXEC db_addArtifacts 'PeripheralComponent','PhysicalSoftwareComponent','Dependencies','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BF607F96-D90E-4308-BD08-1BBACC58D076"), "EXEC db_addArtifacts 'Object','PhysicalSoftwareComponent','Classification','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BF87C7DA-05F1-4EAD-A01D-3ADFA0833FC4"), "EXEC db_addArtifacts 'Object','ComputingComponent','DynamicFlow','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C01A1578-D196-466F-820C-A20D2808C6EA"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','Activity','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C0258ECA-F983-493E-864E-6697705F3290"), "EXEC db_addArtifacts 'ComputingComponent','Rationale','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C0A7561A-EB56-49B5-AD32-80C45056285F"), "EXEC db_addArtifacts 'ComputingComponent','PeripheralComponent','ConnectedTo','ConnectionSpeed',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C14F8CBC-C6D3-4C1B-B5F3-FE4254683608"), "EXEC db_addArtifacts 'DataView','PhysicalSoftwareComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C164DE64-CED2-4935-84E7-A484DA622E49"), "EXEC db_addArtifacts 'Activity','ComputingComponent','DynamicFlow','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C18A84D8-8580-48AF-8680-0C7217C355B9"), "EXEC db_addArtifacts 'NetworkComponent','ComputingComponent','ConnectedTo','ConnectionType',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C20CC693-F8EC-4AB0-A1A0-02CFC72D4AA9"), "EXEC db_addArtifacts 'PeripheralComponent','Object','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C4D0BD77-391E-482B-BE82-12E0B54746E5"), "EXEC db_addArtifacts 'Process','PhysicalSoftwareComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C4E02344-37C9-4098-B698-0067AB3E9E4B"), "EXEC db_addArtifacts 'StorageComponent','ComputingComponent','ConnectedTo','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C53A53F9-6456-4909-9B4F-F961140DA91E"), "EXEC db_addArtifacts 'ComputingComponent','DataTable','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C663F944-5C50-4008-8305-2A54B0CEF9F3"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','ComputingComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C764E757-0C13-4DBE-B72C-938E60CF916F"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','Auxiliary','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C811C72E-230A-49A1-816A-ABA98A064631"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','NetworkComponent','Dependencies','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C8F84253-6840-4623-B2D9-0A0C53956D9C"), "EXEC db_addArtifacts 'ComputingComponent','Object','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C9158BD7-068F-4BA0-B616-1745D3634A39"), "EXEC db_addArtifacts 'ComputingComponent','NetworkComponent','ConnectedTo','ConnectionType',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CA9F308E-FDF9-4316-B0D8-43F1B77196E6"), "EXEC db_addArtifacts 'ComputingComponent','Function','DynamicFlow','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CBA03AAB-F875-4F18-AB32-49E63A4E5574"), "EXEC db_addArtifacts 'ComputingComponent','ComputingComponent','ConnectedTo','ConnectionSpeed',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CCD556A0-C100-40D4-B009-434573D34F8F"), "EXEC db_addArtifacts 'StrategicTheme','PeripheralComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CDB01CAD-9AC2-4C79-9232-801A9E46F17A"), "EXEC db_addArtifacts 'NetworkComponent','ComputingComponent','ConnectedTo','ConnectionSpeed',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CE87369D-06A2-447A-B8C0-DC59B043C160"), "EXEC db_addArtifacts 'PeripheralComponent','StorageComponent','ConnectedTo','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D014E7EA-45F9-4B23-8CFE-7FB76E624BB3"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','Function','DynamicFlow','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D07753F8-C8E8-459E-9EFB-86F7109684BF"), "EXEC db_addArtifacts 'Activity','ComputingComponent','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D0896B7C-3993-4B07-9F4C-242AB15FA8CA"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','ComputingComponent','Dependencies','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D0E7E487-5155-459B-97F9-637F01359D89"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','DynamicFlow','Attribute',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D298B1A5-1583-4712-91CB-F1493EADF9D9"), "EXEC db_addArtifacts 'ComputingComponent','OrganizationalUnit','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D2E79150-C854-4BEB-98E0-B21E0094CEC1"), "EXEC db_addArtifacts 'CSF','PeripheralComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D32F61E5-0E9D-4308-80B1-5758C5DCCCCD"), "EXEC db_addArtifacts 'Process','PeripheralComponent','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D3766585-3DA1-4DF8-8A07-DE087818278E"), "EXEC db_addArtifacts 'Entity','PeripheralComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D42BB312-9617-4B6A-87A3-7B168D656E47"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','Object','DynamicFlow','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D69834D0-F451-40E9-AE62-074D48530D89"), "EXEC db_addArtifacts 'ComputingComponent','Object','Classification','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D7111482-F01A-42F9-B427-9B2974F6C94C"), "EXEC db_addArtifacts 'Activity','PeripheralComponent','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D9671821-F42D-4193-8B22-E982282395A7"), "EXEC db_addArtifacts 'PeripheralComponent','Object','Classification','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D97BEC91-E0E4-4FE9-A07C-1373065C6E7C"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','Process','DynamicFlow','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DBADB3B8-F234-4745-BF2D-E9F03034D660"), "EXEC db_addArtifacts 'PeripheralComponent','NetworkComponent','ConnectedTo','ConnectionSpeed',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DBB127ED-1105-4180-8A84-05E48E5D3F44"), "EXEC db_addArtifacts 'MeasurementItem','PhysicalSoftwareComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DBBBEFCB-4524-4658-91DF-E6392A62F063"), "EXEC db_addArtifacts 'Process','PhysicalSoftwareComponent','DynamicFlow','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DBDA4AC9-D1FE-40DD-A855-520B8BDCDAA5"), "EXEC db_addArtifacts 'Job','ComputingComponent','Mapping','Role',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DE7CDB66-0A94-44F0-BAE8-50F74FF6AF8D"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','Object','Decomposition','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DE86954B-30D6-4337-A53F-5D0AE610EF08"), "EXEC db_addArtifacts 'PeripheralComponent','ComputingComponent','ConnectedTo','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("DEB24C00-D1AA-4279-9201-89A2D9CA8544"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','Activity','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E0E1F825-F87E-4CD3-B12C-D272A784BCF9"), "EXEC db_addArtifacts 'ComputingComponent','Rationale','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E150801F-250C-4CA0-A288-E3C6505D38DF"), "EXEC db_addArtifacts 'Process','ComputingComponent','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E1BF7C1A-C377-4AE0-A35B-A842F6AC71B0"), "EXEC db_addArtifacts 'Process','ComputingComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E1C1F006-68D7-4397-BD3E-222645774849"), "EXEC db_addArtifacts 'PeripheralComponent','Rationale','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E215870E-E9FD-4595-8477-7C0A019620AC"), "EXEC db_addArtifacts 'ComputingComponent','Job','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E356AFD1-A38A-45E8-967D-B58944103A53"), "EXEC db_addArtifacts 'ComputingComponent','Entity','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E4FEAB12-2C5D-4050-BF2D-3D1D8CA48D46"), "EXEC db_addArtifacts 'PeripheralComponent','OrganizationalUnit','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E6871F01-78F8-49F2-BDEE-7201020B5A9C"), "EXEC db_addArtifacts 'Location','PeripheralComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E69AF9DE-62EF-4B03-85F1-F15D68AE1207"), "EXEC db_addArtifacts 'ComputingComponent','JobPosition','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E71B6220-FC5E-400E-A6B4-4BF3F8BB6693"), "EXEC db_addArtifacts 'Rationale','PeripheralComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E735A6C5-E923-4279-BF73-0D9471A8F82B"), "EXEC db_addArtifacts 'ComputingComponent','DataSchema','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E97E2BBF-2704-4FAA-9DB1-2560DAE2BDEA"), "EXEC db_addArtifacts 'StorageComponent','PeripheralComponent','ConnectedTo','ConnectionType',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E9C573D1-F6BA-424D-83A4-41999B788892"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','JobPosition','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EB58195C-A482-49BF-854C-10DC8D0204FE"), "EXEC db_addArtifacts 'NetworkComponent','ComputingComponent','ConnectedTo','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("ECA1D548-C32B-4149-AD03-979D3F9505E7"), "EXEC db_addArtifacts 'Implication','PhysicalSoftwareComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("ED897C8B-6C68-407F-837C-1A4BBB5A4C54"), "EXEC db_addArtifacts 'ComputingComponent','PeripheralComponent','ConnectedTo','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EE2BF956-1B2F-4DEA-A8E7-117C71E1CB5A"), "EXEC db_addArtifacts 'DataTable','ComputingComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EE69E398-CF58-4971-A199-777E01FFB6C2"), "EXEC db_addArtifacts 'PeripheralComponent','Function','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EF1655E6-C6A3-456B-B298-1032B520B192"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EF952E9E-9960-4CAC-809F-9A40D7A30965"), "EXEC db_addArtifacts 'ComputingComponent','StorageComponent','ConnectedTo','ConnectionType',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F11BE1A1-5820-4637-96FC-85D54842E64B"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','DynamicFlow','DataTable',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F2B29DCF-FB3C-48FD-9988-A31E145B561F"), "EXEC db_addArtifacts 'Rationale','ComputingComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F37D522B-32D0-4FCC-BA36-60647EAC8549"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','Object','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F42CFAE5-78AC-4C72-805D-1812A828C9C5"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','Activity','DynamicFlow','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F5903F7C-4E00-4143-BD3A-CC9F24721C16"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','StorageComponent','Dependencies','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F6A0EEBE-B5BA-4BE2-AEE3-33901A7B4B4C"), "EXEC db_addArtifacts 'ComputingComponent','StrategicTheme','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F6ECE9CA-891B-40F9-B5C0-909EBE12200F"), "EXEC db_addArtifacts 'PeripheralComponent','Object','Decomposition','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F7B8422B-743E-4FE3-A9B8-0D80BFAAEC30"), "EXEC db_addArtifacts 'Object','PhysicalSoftwareComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F8162A29-4F6A-4828-91B0-9394558A0A7B"), "EXEC db_addArtifacts 'JobPosition','PeripheralComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F842B4B4-C524-49ED-A2CB-304BE5517451"), "EXEC db_addArtifacts 'PeripheralComponent','JobPosition','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F897AED1-26A0-4ECF-9EC6-04CCF572598A"), "EXEC db_addArtifacts 'PeripheralComponent','ComputingComponent','ConnectedTo','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FA105EDE-FC92-4A43-A332-6A6D153896FD"), "EXEC db_addArtifacts 'PeripheralComponent','Object','DynamicFlow','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FA6E456C-849B-428C-954E-AFC96F895374"), "EXEC db_addArtifacts 'ComputingComponent','Employee','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FB986453-93BD-4057-A9CB-6B968569C27D"), "EXEC db_addArtifacts 'DataView','ComputingComponent','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FC7E6F49-B0D5-4BBC-8F35-0331F5A11A98"), "EXEC db_addArtifacts 'ComputingComponent','Object','Mapping','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FD8B31E4-6213-4A0E-AB4F-A85015600711"), "EXEC db_addArtifacts 'PeripheralComponent','ComputingComponent','Dependencies','Rationale',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FE11F70B-C218-4A49-ACFD-E6781C54F068"), "EXEC db_addArtifacts 'JobPosition','PeripheralComponent','Mapping','Role',1|MAJOR METAMODEL CHANGE SAPPI ONLY");

            databaseVersions.Add(new Guid("C3AEACD4-7E0D-435C-82EF-3828366D0167"), "EXEC db_addClassAssociations 'LogicalITInfrastructureComponent','LogicalITInfrastructureComponent','Decomposition','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2C14CE7B-7E76-4CA6-8F4A-029A9336D363"), "EXEC db_addClassAssociations 'LogicalITInfrastructureComponent','LogicalITInfrastructureComponent','Classification','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("09EA9FCF-6E0D-4739-AAF8-A357E1A8DA89"), "EXEC db_addClassAssociations 'LogicalITInfrastructureComponent','LogicalITInfrastructureComponent','Auxiliary','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9A1319C7-DEC7-4B4D-840C-2AD28BCC7CDD"), "EXEC db_addClassAssociations 'LogicalITInfrastructureComponent','LogicalITInfrastructureComponent','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("971088AA-013C-43A9-B4C7-52F5AED55903"), "EXEC db_addClassAssociations 'LogicalITInfrastructureComponent','SystemComponent','Mapping','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1961524F-DAFF-4F00-9F59-46D38669DF44"), "EXEC db_addClassAssociations 'LogicalITInfrastructureComponent','ComputingComponent','Mapping','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1330FA5A-7859-4FA4-B99F-A101D56452F0"), "EXEC db_addClassAssociations 'LogicalITInfrastructureComponent','Software','Mapping','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("50B9B5B9-8E11-4189-8311-5B714B49427B"), "EXEC db_addClassAssociations 'LogicalITInfrastructureComponent','PhysicalSoftwareComponent','Mapping','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("27544DE8-8E65-4C5D-B49A-AA3ABAA86F16"), "EXEC db_addClassAssociations 'LogicalITInfrastructureComponent','NetworkComponent','Mapping','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("27298289-7BAD-4D6C-BE4D-01C4F51A4EE5"), "EXEC db_addClassAssociations 'LogicalITInfrastructureComponent','StorageComponent','Mapping','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FDB09500-FB9C-41C7-991C-39B6DB46D55C"), "EXEC db_addArtifacts 'LogicalITInfrastructureComponent','LogicalITInfrastructureComponent','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("198D45D8-6074-4584-8667-D46F824B0AFB"), "EXEC db_addClassAssociations 'Network','NetworkComponent','ConnectedTo','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A7A5C3F6-3715-4673-8C83-B04431E05644"), "EXEC db_addClassAssociations 'NetworkComponent','Network','ConnectedTo','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3E6420E8-558F-4E4B-922D-11F8E87B6AEA"), "EXEC db_addClassAssociations 'Network','SystemComponent','ConnectedTo','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B6ACA3D3-A9AD-40B7-8B6F-6CBAB212ECC4"), "EXEC db_addClassAssociations 'Network','ComputingComponent','ConnectedTo','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B8F43E82-4430-4FB3-9D86-A82E856BB977"), "EXEC db_addClassAssociations 'SystemComponent','Network','ConnectedTo','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9438A44C-2C8B-415C-B59C-DB661B994209"), "EXEC db_addClassAssociations 'ComputingComponent','Network','ConnectedTo','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9C3796CF-7B0C-4D06-8127-872EEF6EB418"), "EXEC db_addClassAssociations 'Network','NetworkComponent','ConnectedTo','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9A0FA595-95E2-4581-A033-6C0202CB49C7"), "EXEC db_addClassAssociations 'Network','StorageComponent','ConnectedTo','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FA52CDDA-A8EF-461B-AA14-C4D2E633582D"), "EXEC db_addClassAssociations 'Network','Peripheral','ConnectedTo','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5B2B5CC9-2CBD-4828-BE90-CDFB8FEDD6D1"), "EXEC db_addClassAssociations 'Peripheral','Network','ConnectedTo','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("35399C5F-C771-48A1-93E1-F3BDD6546BA1"), "EXEC db_addClassAssociations 'Network','PeripheralComponent','ConnectedTo','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6E368EFC-9A9A-49AC-A19E-6944D4F45809"), "EXEC db_addClassAssociations 'PeripheralComponent','Network','ConnectedTo','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9FA589E1-CD4F-4579-9B87-D64A2121C2AF"), "EXEC db_addClassAssociations 'NetworkComponent','Network','ConnectedTo','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F9FFADE9-9982-45B7-9E6A-067EACEEE40C"), "EXEC db_addClassAssociations 'StorageComponent','Network','ConnectedTo','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("1021D57D-89D1-4E04-8F5F-43BA0A12B417"), "EXEC db_addArtifacts 'Network','NetworkComponent','ConnectedTo','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("7768894A-C183-406A-8B10-0427BBD7F708"), "EXEC db_addArtifacts 'NetworkComponent','Network','ConnectedTo','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0F2D82F7-1F28-4561-A4A8-15BCEDC8D505"), "EXEC db_addArtifacts 'Network','SystemComponent','ConnectedTo','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F0E9ED63-7971-4B95-A155-4D7C390AE25B"), "EXEC db_addArtifacts 'Network','ComputingComponent','ConnectedTo','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BFF88814-2211-41A1-B10D-02D542030034"), "EXEC db_addArtifacts 'SystemComponent','Network','ConnectedTo','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("102560E2-639B-4AF0-956F-6C640E81918D"), "EXEC db_addArtifacts 'ComputingComponent','Network','ConnectedTo','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("0262480C-B827-48A5-90C8-13D36A8F42DC"), "EXEC db_addArtifacts 'Network','NetworkComponent','ConnectedTo','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("86F10BBD-6FDA-4A1B-9E0F-B5FBF86516D9"), "EXEC db_addArtifacts 'Network','StorageComponent','ConnectedTo','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2F069149-5C92-4BAE-8FB4-AAEBE0912788"), "EXEC db_addArtifacts 'Network','Peripheral','ConnectedTo','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("191B8E19-BF2B-4EE8-9450-7A4ACE8A89E9"), "EXEC db_addArtifacts 'Peripheral','Network','ConnectedTo','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("6802711D-2794-44C2-84C6-E56C67337683"), "EXEC db_addArtifacts 'Network','PeripheralComponent','ConnectedTo','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5C1DC422-FF89-43E3-95B6-605D5CCC4DD4"), "EXEC db_addArtifacts 'PeripheralComponent','Network','ConnectedTo','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("8DE8DE20-51FC-4558-8069-1CBF9F04E0FA"), "EXEC db_addArtifacts 'NetworkComponent','Network','ConnectedTo','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1CD7A349-0F09-4A7E-8847-7D4B01163592"), "EXEC db_addArtifacts 'StorageComponent','Network','ConnectedTo','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");

            databaseVersions.Add(new Guid("D695B5C8-EB09-4E53-80D9-BF576EB846AC"), "EXEC db_addClassAssociations 'Object','Software','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("9DC804AE-F077-453C-B429-3AF46372D16C"), "EXEC db_addClassAssociations 'Software','Object','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("719C1B38-74E8-4B64-B66C-3640E9131A55"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','Object','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C8C06E12-400F-4178-B0C3-B521EFAF08BF"), "EXEC db_addClassAssociations 'Object','PhysicalSoftwareComponent','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BA5131CF-664C-4E08-91AC-E80E325B7B4D"), "EXEC db_addArtifacts 'Object','Software','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3C0F8041-53B0-408A-9ED0-DE2798D86F59"), "EXEC db_addArtifacts 'Software','Object','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3FA8B6FE-2920-4AE4-8315-682C28190FDA"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','Object','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B47E901C-65EC-42A1-A0CA-A6BDD6F13CB5"), "EXEC db_addArtifacts 'Object','PhysicalSoftwareComponent','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY|MAJOR METAMODEL CHANGE SAPPI ONLY");

            databaseVersions.Add(new Guid("22F69390-622A-4487-8A9F-27F8EB6F4DF8"), "EXEC db_AddPossibleValues 'SeverityRating','Extreme',1,'Extreme',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("68F3C134-3261-41FA-A942-55F8DC6894CF"), "EXEC db_AddPossibleValues 'SeverityRating','Moderate',2,'Moderate',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("69541678-B92B-410A-BB38-54849FC5C2F2"), "EXEC db_AddPossibleValues 'SeverityRating','Low',3,'Low',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C47809D8-38EF-4CDC-AD60-AAEA9CC47308"), "EXEC db_AddPossibleValues 'SeverityRating','None',4,'None',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3913ACC1-7FF0-49CA-99AB-A504DA726CD9"), "EXEC db_AddPossibleValues 'SecurityClassification','PublicDomain',1,'PublicDomain',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("074E5D49-B2DE-4B70-A6A4-6639B6F2861E"), "EXEC db_AddPossibleValues 'SecurityClassification','ControlledDisclosure',2,'ControlledDisclosure',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("86734A16-8963-459A-BBC0-2E4615353D52"), "EXEC db_AddPossibleValues 'SecurityClassification','Confidential',3,'Confidential',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0E67B902-9200-4B48-A3E7-AA4375DD588C"), "EXEC db_AddPossibleValues 'SecurityClassification','Secret',4,'Secret',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EDC4FAE7-A05F-4290-877B-9F4907B2FF94"), "EXEC db_AddPossibleValues 'IsBespoke','Yes',1,'Yes',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8D84F35F-FF4E-4C8B-B304-720665C2B3F6"), "EXEC db_AddPossibleValues 'IsBespoke','No',2,'No',1|MAJOR METAMODEL CHANGE SAPPI ONLY");

            //Update fields with renamed domain definitions            
            databaseVersions.Add(new Guid("8E52FFB0-29ED-475B-9287-F9B24179292F"), "UPDATE Field SET DataType = 'ComputingComponentType' WHERE DataType = 'SystemCompType'|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("350C5B1B-7C72-4FDA-8DF8-07942EAD6E7E"), "UPDATE Field SET DataType = 'NetworkComponentType' WHERE DataType = 'NetworkCompType'|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("24627B62-3DBD-49BE-BF0F-C5CBA35AB6EF"), "UPDATE Field SET DataType = 'ContentType' WHERE DataType = 'DataType'|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //Deactivate all customField*
            databaseVersions.Add(new Guid("AD1BD4E4-87D4-4A44-9695-0E0A3A05331A"), "UPDATE Field Set IsActive = 0 WHERE Name LIKE '%CustomField%'|MAJOR METAMODEL CHANGE SAPPI ONLY");

            #endregion

            //Must happen after metamodel so 'new' values get images
            #region Default Possible Value Images

            string ImagesPath = AppDomain.CurrentDomain.BaseDirectory + "MetaData\\Images";
            databaseVersions.Add(new Guid("0508BD7E-7715-462F-A47B-0104C148474B"), "DELETE FROM UURI|Clear DDPV Images");
            databaseVersions.Add(new Guid("8ECE9CC6-AE36-4ECF-90C5-1B8E7B3E3E51"), "DBCC CHECKIDENT (UURI, reseed, 1)|Reset UURI Seed");
            databaseVersions.Add(new Guid("5424C884-37CB-4729-AC10-25BB2795D2A4"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\application.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("97E0FA6E-DFCE-4549-B71C-BD48D59BFE84"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Bridge.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("E3E6F36D-FD5E-4245-8EC7-F06925B9AFB5"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\CloudNetwork.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("F6ADC9A6-475C-45C1-96CD-E306D5F8246A"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\DatabaseMigration.jpg')|Default DDPV Images");
            databaseVersions.Add(new Guid("60C88F54-9598-44D4-B22F-F300C73C5F92"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\DatabaseServer.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("0D9D9190-11A2-4020-89C6-D87F96520F96"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\DBMS.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("97294FAE-D04D-4CBE-80C4-271815816FA5"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Fax.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("88666D7E-C305-496A-A638-A94B98A167C8"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\FaxServer.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("25BE56B1-19AF-4749-B80C-FA1396B24F61"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\FileServer.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("91869428-F2DD-45DF-94CB-B9894E168FED"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Firewall.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("FB4CE54B-63D7-4B3F-8B44-E52DC673680D"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\HDD.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("9053C3AE-6EB8-4DFA-9F42-6E6940B7B85A"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Hub.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("E0258AF3-B522-4111-AE71-246200DF68D1"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Internet.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("652397BF-6404-423C-AF28-5BBFDB7BFBC8"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\LAN.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("D11F88A0-0B84-4DFE-ADF5-CD1F147363AB"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Laptop.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("99BD15CD-BA15-4A66-A050-4E939563196D"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\MailServer.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("E06C2DA8-9DEC-43B1-AC71-F8751776EAFA"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Mainframe.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("E53CBB73-C6EE-42FA-8AF9-A76103D97C08"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Manual.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("510F38D3-D481-44F2-9851-E0700BA01EFB"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\MultiFunctionalDevice.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("089C0484-F370-4BBF-9CF9-8DC07FCF4945"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\NAS.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("BEF1E470-96CC-4322-A87F-C554D32BD43C"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Network.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("25633D34-8FEE-4C93-9F59-4D3B0A255D20"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\OpticalDiskDrive.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("E647165B-3269-4021-A67B-E1C619048F6C"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\OS.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("C38ADC8D-B5DE-4831-805A-A1B992E359F8"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Package.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("5C29DC28-FFF3-4D3D-A42C-5BD911F37B86"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\PDA.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("57E0C6E5-4BA8-4D11-8BBC-1603972DABD3"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Peripheral.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("B53C2D44-2804-44BD-910B-7721AA9EE85A"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Phone.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("886AEEA5-4CE9-4F55-A45B-59FDB39D5497"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\PhysicalServer.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("D2671A69-E214-4F48-B5C1-4609FE8D6CAE"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Printer.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("26540E7F-0AE4-4907-874F-FBB60674E153"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Projector.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("3D60D8BA-9AE3-4B13-B154-A58F08C43A4A"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Router.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("75594D77-B50A-4168-B06D-FDCA89B14A41"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Satelite.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("27E4523D-2214-4B05-8E7E-3D8FADAC7D0C"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Scanner.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("8F64DB68-19EA-412B-B31F-7F475B1636FF"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Script.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("47E140E0-6314-4CCE-974D-C04672C26AF2"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Software.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("6138F337-CF8A-4366-AC8B-078359E73083"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Spreadsheet.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("526ACDA2-9610-42DC-A122-DFD5F8BA06E5"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Storage.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("009FF7AE-D071-4730-9B20-D7C17BE9DADD"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\SystemComponent.ico')|Default DDPV Images");
            databaseVersions.Add(new Guid("C2C3291A-3049-41A4-A7EF-5CBC0C62D60A"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\TapeDrive.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("EBC53F43-980F-4963-8DE1-2D0AA12088F8"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\USBFlashDrive.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("862E26F8-3EF0-4F43-9EB5-0CD45027F08E"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\VirtualServer.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("996900F3-4837-4280-B3C8-AA5D87AAD2BA"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\WAP.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("4664A34F-7541-46DC-9C70-8325067E9FCB"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\WebServer.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("5F2DEFD7-0C89-416A-8FFA-2D961E852B00"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\WirelessNetwork.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("51F6D464-67A6-47F9-8A74-5915F6B6BDBA"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Workstation.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("848324C1-AE57-4F71-96B6-14E9293E7095"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\XML.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("96F05974-D3D2-4594-AC39-D6BBD6654518"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\PatchPanel.png')|Default DDPV Images");

            databaseVersions.Add(new Guid("D54D1954-77F6-4950-847A-112B06A81604"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 1 WHERE PossibleValue = 'application'|Default Images");
            databaseVersions.Add(new Guid("F085FCBE-672B-4A8A-9158-ED0650E660B6"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 1 WHERE PossibleValue = 'app'|Default Images");
            databaseVersions.Add(new Guid("FDA4AA58-E7E8-4E50-AC81-8596BF102EF6"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 2 WHERE PossibleValue = 'Bridge'|Default Images");
            databaseVersions.Add(new Guid("701CD0BA-CF1D-4447-9616-FFA0219FD98A"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 3 WHERE PossibleValue = 'CloudNetwork'|Default Images");
            databaseVersions.Add(new Guid("8AAE3EE6-A2D8-43A4-89EB-29A24C07FFC9"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 4 WHERE PossibleValue = 'DatabaseMigration'|Default Images");
            databaseVersions.Add(new Guid("CC6FCBEA-2350-4535-8E14-7744D69238C9"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 5 WHERE PossibleValue = 'DatabaseServer'|Default Images");
            databaseVersions.Add(new Guid("1BDDC8D3-EC04-47ED-8FF1-C67687848F15"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 6 WHERE PossibleValue = 'DBMS'|Default Images");
            databaseVersions.Add(new Guid("9B710101-B876-4C55-8F0B-72FB0F6931F3"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 7 WHERE PossibleValue = 'Fax'|Default Images");
            databaseVersions.Add(new Guid("848527E0-EE60-41DD-8E88-CC0C8E9D9AA7"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 8 WHERE PossibleValue = 'FaxServer'|Default Images");
            databaseVersions.Add(new Guid("49C891D2-4146-4551-B16A-FF783541DF96"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 9 WHERE PossibleValue = 'FileServer'|Default Images");
            databaseVersions.Add(new Guid("4D12F51A-1EAE-47F7-8AD3-23421025E3E2"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 10 WHERE PossibleValue = 'Firewall'|Default Images");
            databaseVersions.Add(new Guid("30333BD4-D341-4270-A234-CCACE222F9C3"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 11 WHERE PossibleValue = 'HDD'|Default Images");
            databaseVersions.Add(new Guid("86FA5A58-DC63-4C83-AF2A-4E08DC5F7DC9"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 12 WHERE PossibleValue = 'Hub'|Default Images");
            databaseVersions.Add(new Guid("D307D7E4-3C7E-42D9-85D7-AED26A56EA5B"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 13 WHERE PossibleValue = 'Internet'|Default Images");
            databaseVersions.Add(new Guid("A0022067-E36E-45B9-A7DC-06F81D345BCC"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 14 WHERE PossibleValue = 'LAN'|Default Images");
            databaseVersions.Add(new Guid("6AA1EB41-DFA0-486E-86B4-61A57CEEB74E"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 15 WHERE PossibleValue = 'Laptop'|Default Images");
            databaseVersions.Add(new Guid("D5848091-73F8-4098-8023-7ED56B276933"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 16 WHERE PossibleValue = 'MailServer'|Default Images");
            databaseVersions.Add(new Guid("89E02042-3CCF-4A8F-9F5F-7651F7BDB03A"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 17 WHERE PossibleValue = 'Mainframe'|Default Images");
            databaseVersions.Add(new Guid("995D714D-DB30-47AC-8B43-6A619D18BC3A"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 18 WHERE PossibleValue = 'Manual'|Default Images");
            databaseVersions.Add(new Guid("3E378D23-7A82-405F-95E1-F324C0DE2BEF"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 19 WHERE PossibleValue = 'Multi'|Default Images");
            databaseVersions.Add(new Guid("0511CD38-2CA6-4AA5-AC1F-677BCEA4F845"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 20 WHERE PossibleValue = 'NAS'|Default Images");
            databaseVersions.Add(new Guid("DEA3D0C4-160D-4CE4-B7EC-23D75505C0FC"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 21 WHERE PossibleValue = 'Network'|Default Images");
            databaseVersions.Add(new Guid("CA4E6279-2D69-485D-97E1-F7DC0A9E6EB1"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 22 WHERE PossibleValue = 'OpticalDiskDrive'|Default Images");
            databaseVersions.Add(new Guid("D8DFAD74-422B-4751-80F3-8911AD64EB25"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 23 WHERE PossibleValue = 'OS'|Default Images");
            databaseVersions.Add(new Guid("964BFF2B-8181-4A98-AB3C-116EB365A82B"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 24 WHERE PossibleValue = 'Package'|Default Images");
            databaseVersions.Add(new Guid("0CCB1AA4-F65E-49C0-AD2D-E84ADB957D7C"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 25 WHERE PossibleValue = 'PDA'|Default Images");
            databaseVersions.Add(new Guid("6FF314AE-E5CB-49F1-93E0-5D8309B2751B"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 26 WHERE PossibleValue = 'PeripheralComponent'|Default Images");
            databaseVersions.Add(new Guid("0070B1D5-8327-450B-BD74-BDD7B35CCC6B"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 27 WHERE PossibleValue = 'Phone'|Default Images");
            databaseVersions.Add(new Guid("F36F3B41-CFF0-4BE9-9820-244DCB7E789E"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 28 WHERE PossibleValue = 'PhysicalServer'|Default Images");
            databaseVersions.Add(new Guid("3A71716A-76F2-46AA-8E27-FB6EF646171B"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 28 WHERE PossibleValue = 'PhysServer'|Default Images");
            databaseVersions.Add(new Guid("4AC948DF-494F-4BD7-882C-2044C65F0E07"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 29 WHERE PossibleValue = 'Printer'|Default Images");
            databaseVersions.Add(new Guid("7BBA018A-55C3-4125-AF8D-6E392A5EF587"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 30 WHERE PossibleValue = 'Projector'|Default Images");
            databaseVersions.Add(new Guid("E65BA2E2-0431-4080-ADF0-48B981E9574C"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 31 WHERE PossibleValue = 'Router'|Default Images");
            databaseVersions.Add(new Guid("E1E37740-4F79-467C-87F2-50D6A3201344"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 32 WHERE PossibleValue = 'Satellite'|Default Images");
            databaseVersions.Add(new Guid("92C8C042-23E2-4FA9-B60D-7690AF026528"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 33 WHERE PossibleValue = 'Scanner'|Default Images");
            databaseVersions.Add(new Guid("B4D5601B-DF91-40E2-A542-8ABF4311982C"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 34 WHERE PossibleValue = 'Script'|Default Images");
            databaseVersions.Add(new Guid("3AAF2317-C57C-4E8F-82F4-69E0FDD50D8D"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 35 WHERE PossibleValue = 'Software'|Default Images");
            databaseVersions.Add(new Guid("E1DC51EC-DF09-4355-88E5-5F1ABA2DE45E"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 36 WHERE PossibleValue = 'Spreadsheet'|Default Images");
            databaseVersions.Add(new Guid("8753FBCB-E756-4B19-A475-05E992FF8B11"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 37 WHERE PossibleValue LIKE '%Storage%'|Default Images");
            databaseVersions.Add(new Guid("0E58314E-7CD3-4DA0-A251-FA6E0AEEE640"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 38 WHERE PossibleValue = 'SystemComponent'|Default Images");
            databaseVersions.Add(new Guid("B52457DD-74DE-4E79-8F28-819033244F9E"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 39 WHERE PossibleValue = 'TapeDrive'|Default Images");
            databaseVersions.Add(new Guid("29F47055-4D4A-48D5-99AC-CC0C13925CC2"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 40 WHERE PossibleValue = 'USBFlashDrive'|Default Images");
            databaseVersions.Add(new Guid("9FE95ECE-CA00-4629-A0FE-0C6B0129760F"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 41 WHERE PossibleValue = 'VirtualServer'|Default Images");
            databaseVersions.Add(new Guid("C4C2E18A-6978-4C99-ABE9-8721820D6BC8"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 42 WHERE PossibleValue = 'WAP'|Default Images");
            databaseVersions.Add(new Guid("DD4456C4-637E-45AF-8DF7-089AF42CCBDD"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 43 WHERE PossibleValue = 'WebServer'|Default Images");
            databaseVersions.Add(new Guid("838F495E-BBE4-4C19-9347-E8AA18752554"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 44 WHERE PossibleValue = 'WirelessNetwork'|Default Images");
            databaseVersions.Add(new Guid("3A10891E-CCAC-4F52-A5E2-63785EFF4F7A"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 45 WHERE PossibleValue = 'Workstation'|Default Images");
            databaseVersions.Add(new Guid("E2101707-B6CE-4CE6-8CDB-52F9A543D32B"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 46 WHERE PossibleValue = 'XML'|Default Images");
            databaseVersions.Add(new Guid("626512E0-DAA2-48E1-9CB5-0AACCCE79A8F"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 47 WHERE PossibleValue = 'PatchPanel'|Default Images");

            databaseVersions.Add(new Guid("BA8C9B4A-FA2D-4FC9-94DC-5C2A5B0309C7"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\database.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("013CE2BA-23CC-455A-B2C0-D7B42F55E462"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\datawarehouse.png')|Default DDPV Images");

            databaseVersions.Add(new Guid("712265FD-E941-4BD0-B70B-06DC1BCC5F5C"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 36 WHERE PossibleValue = 'SpreadSheet'|Default Images");
            databaseVersions.Add(new Guid("90DC7B61-213D-4222-A545-A994143FF847"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 49 WHERE PossibleValue = 'DataWarehouse'|Default Images");
            databaseVersions.Add(new Guid("6E936ECC-5FEF-4E95-98FB-ACD9847FDDF3"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 48 WHERE PossibleValue = 'DataMart'|Default Images");
            databaseVersions.Add(new Guid("B489B7F0-7AFA-47E0-9250-1F8D3E9E2FFF"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 48 WHERE PossibleValue = 'InMemoryDatabase'|Default Images");
            databaseVersions.Add(new Guid("978B9570-9C4B-4671-A68C-579B51088E5E"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 48 WHERE PossibleValue = 'OperationalDataStore'|Default Images");
            databaseVersions.Add(new Guid("35282996-36C7-469A-AD36-DFD5112A3443"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 48 WHERE PossibleValue = 'Database'|Default Images");

            //databaseVersions.Add(new Guid("65C9CFC9-1CF2-4B04-9008-C340A7BE3C1E"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\physicalserver.ico')|Default DDPV Images");
            //databaseVersions.Add(new Guid("9442AF55-D696-49AE-9CE7-7FAB06AA9D30"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 50 WHERE PossibleValue = 'PhysicalServer'|Default Images");

            #endregion

            #region 9 June Metamodel Additions

            databaseVersions.Add(new Guid("702C905B-B871-4232-A0CE-44C3288F71E6"), "EXEC db_AddPossibleValues 'PeripheralType','Copier',5,'Copier',0|MAJOR METAMODEL CHANGES SAPPI ONLY");
            databaseVersions.Add(new Guid("276B00BA-97E7-44E1-A077-DCC4409EF3A0"), "EXEC db_AddPossibleValues 'PeripheralType','Other',9,'Other',0|MAJOR METAMODEL CHANGES SAPPI ONLY");

            databaseVersions.Add(new Guid("E251D796-717D-4804-8C01-C25A8937C17C"), "EXEC db_AddFields 'NetworkComponent','Type','Type','General','',0,1,0|MAJOR METAMODEL CHANGE SAPPI ONLY");

            //databaseVersions.Add(new Guid("B0C4C1A8-D397-41C6-A3CE-EE2F71DB8FD2"), "EXEC db_addClassAssociations 'Network','NetworkComponent','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("DC7C3F1F-F653-4188-A897-6E62E10B746B"), "EXEC db_addClassAssociations 'Networkcomponent','Network','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("9E6D27EF-39BE-4D68-BAAE-BD3BD2F61866"), "EXEC db_addClassAssociations 'Network','SystemComponent','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("3C8B1B56-B281-42B0-A149-A2AD989A49B9"), "EXEC db_addClassAssociations 'Network','ComputingComponent','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("3EC821ED-595F-4FF8-B0DC-C434FA7BB5B4"), "EXEC db_addClassAssociations 'ComputingComponent','Network','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("A8B30FDB-68EC-4604-ACB2-0717CE5ADCAE"), "EXEC db_addClassAssociations 'Systemcomponent','Network','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("1C73746D-345C-473D-9876-6CA1E4A61984"), "EXEC db_addClassAssociations 'Network','NetworkComponent','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("C8092629-C570-4547-B2C9-1AF3CDCEA8B0"), "EXEC db_addClassAssociations 'Network','StorageComponent','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("9FCBBC76-7177-4465-9067-762E2A532D06"), "EXEC db_addClassAssociations 'Network','Peripheral','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("5F1E3253-EC56-46D1-AC82-3F34110B1B6F"), "EXEC db_addClassAssociations 'Peripheral','Network','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("3640097D-9053-4A3A-B566-4A99E0232692"), "EXEC db_addClassAssociations 'Network','PeripheralComponent','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("DAC6F402-3586-4B4F-BF08-B71B6491F173"), "EXEC db_addClassAssociations 'PeripheralComponent','Network','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("FC73283F-2049-4483-89CC-A096DE3DA405"), "EXEC db_addClassAssociations 'NetworkComponent','Network','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("D7A4FE54-02DA-406C-AB8C-B17660DE140C"), "EXEC db_addClassAssociations 'StorageComponent','Network','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("0BF2AD9C-1623-49A0-ACDF-19207E8521A7"), "EXEC db_addArtifacts 'Network','NetworkComponent','DynamicFlow','Network',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("F20B7EAE-3380-46F5-ABD0-1DF4B1B8963E"), "EXEC db_addArtifacts 'networkcomponent','Network','DynamicFlow','Network',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("85EA72DE-6B4F-464C-A81B-31F0B6B26855"), "EXEC db_addArtifacts 'Network','SystemComponent','DynamicFlow','Network',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("2F055802-8CA6-45AA-8591-D26799FE2D64"), "EXEC db_addArtifacts 'Network','ComputingComponent','DynamicFlow','Network',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("379CE77B-76F5-4EC7-BDC6-18BBA42E6968"), "EXEC db_addArtifacts 'ComputingComponent','Network','DynamicFlow','Network',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("1434CA06-6D8D-471C-895A-BFBAE63B64AE"), "EXEC db_addArtifacts 'Systemcomponent','Network','DynamicFlow','Network',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("157CDAFC-9AD2-4AFD-B221-4478034B7DD8"), "EXEC db_addArtifacts 'Network','NetworkComponent','DynamicFlow','Network',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("C01674E5-6D83-4F13-BFAC-9FFF78C82E60"), "EXEC db_addArtifacts 'Network','StorageComponent','DynamicFlow','Network',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("8DBC92B7-A82E-4A4A-937F-9034AAAA6BA2"), "EXEC db_addArtifacts 'Network','Peripheral','DynamicFlow','Network',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("2D16DC4E-FBAB-4A92-B58B-05B2C3E3E330"), "EXEC db_addArtifacts 'Peripheral','Network','DynamicFlow','Network',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("98E1C144-E25C-4323-90EE-22AE2A382FE9"), "EXEC db_addArtifacts 'Network','PeripheralComponent','DynamicFlow','Network',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("5E7DB1C6-9AE8-4F37-A8C4-F14CEA1AB8D9"), "EXEC db_addArtifacts 'PeripheralComponent','Network','DynamicFlow','Network',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("466001C9-1CCC-4D01-BEB6-7B0C7286A573"), "EXEC db_addArtifacts 'NetworkComponent','Network','DynamicFlow','Network',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            //databaseVersions.Add(new Guid("9B385CE3-DE11-4A07-8112-639D4C3F588F"), "EXEC db_addArtifacts 'StorageComponent','Network','DynamicFlow','Network',0|MAJOR METAMODEL CHANGE SAPPI ONLY");

            databaseVersions.Add(new Guid("43035984-4541-469E-BFCF-A0FB7CE68270"), "EXEC db_addClassAssociations 'NetworkComponent','NetworkComponent','ConnectedTo','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D81724D2-36A8-4BF0-AEFD-2F013AF92F2C"), "EXEC db_addClassAssociations 'NetworkComponent','StorageComponent','ConnectedTo','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B23CC989-E1AA-42EA-8652-9CF9802FDB92"), "EXEC db_addClassAssociations 'StorageComponent','NetworkComponent','ConnectedTo','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("666A9C43-B20F-43E1-BC60-06B1D283DEF3"), "EXEC db_addClassAssociations 'ComputingComponent','NetworkComponent','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("636D2204-7F51-42EF-ADCB-93CFA7D11AE2"), "EXEC db_addClassAssociations 'NetworkComponent','Network','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2EF5537B-09B7-4F6C-A196-601CBD11E9D7"), "EXEC db_addClassAssociations 'Network','SystemComponent','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E38E7FB3-2AE3-4A33-A24C-13DEC12EFCF8"), "EXEC db_addClassAssociations 'Network','ComputingComponent','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5895F146-8F13-46DC-B799-D64EDCD29263"), "EXEC db_addClassAssociations 'SystemComponent','Network','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("155C8F10-B4D4-4CBA-B288-0D1BB515D5F5"), "EXEC db_addClassAssociations 'ComputingComponent','Network','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("055181BD-3695-4B7D-B201-4DCB79DB2D17"), "EXEC db_addClassAssociations 'Network','NetworkComponent','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("00096A2B-C335-4C10-94B1-38FDD216CB15"), "EXEC db_addClassAssociations 'Network','StorageComponent','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("ADD89469-AFC5-45AF-BFC9-8D98562BE41D"), "EXEC db_addClassAssociations 'Network','Peripheral','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("386B7196-33D2-4FAB-A5E7-25617A568BE0"), "EXEC db_addClassAssociations 'Peripheral','Network','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FA7429BC-A478-4C45-BE26-441345C1A767"), "EXEC db_addClassAssociations 'Network','PeripheralComponent','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("E663F078-9EEF-4F7B-A472-346F94D20BC6"), "EXEC db_addClassAssociations 'PeripheralComponent','Network','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("F4A31A31-A9BB-4638-9046-86728EA524DC"), "EXEC db_addClassAssociations 'NetworkComponent','Network','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("96F1E303-A87D-48F2-B153-778E660767C2"), "EXEC db_addClassAssociations 'StorageComponent','Network','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C564ADAD-47FE-4CEA-A76E-158162057D6B"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','NetworkComponent','Mapping','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D4CCAFAD-02EC-4412-B193-720D9E3E3CB6"), "EXEC db_addClassAssociations 'PhysicalDataComponent','PhysicalSoftwareComponent','Mapping','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BEA5F6F0-5FD2-4632-93F0-E7FBA297F837"), "EXEC db_addClassAssociations 'PhysicalDataComponent','ComputingComponent','Mapping','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BA0DFB1F-DA27-4E74-83B4-9EA1FD40D23B"), "EXEC db_addClassAssociations 'PhysicalDataComponent','NetworkComponent','Mapping','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3C93719B-F7EF-48B5-AAD4-659345B4B51F"), "EXEC db_addClassAssociations 'PhysicalDataComponent','GovernanceMechanism','Mapping','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("21E34A30-8163-4F28-9057-BB019F936555"), "EXEC db_addClassAssociations 'PhysicalDataComponent','Function','Mapping','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2BE334CF-60E2-4CAC-8AF2-12502BADD35C"), "EXEC db_addClassAssociations 'PhysicalDataComponent','Process','Mapping','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B7E48F13-ACF3-44ED-BED4-1C48FFF07D99"), "EXEC db_addClassAssociations 'PhysicalDataComponent','Activity','Mapping','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1232ADF2-E016-427B-B5C2-2622CD888926"), "EXEC db_addClassAssociations 'PhysicalDataComponent','Object','Mapping','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("85EC43F2-BB90-4BA0-9B4F-5E6E99BA495B"), "EXEC db_addClassAssociations 'PhysicalDataComponent','Function','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EA747CEB-1752-4C95-8D25-845078042FDB"), "EXEC db_addClassAssociations 'PhysicalDataComponent','Process','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3A3545CB-F24F-48AE-A6AB-E95D6371157E"), "EXEC db_addClassAssociations 'PhysicalDataComponent','Activity','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("204392CC-177A-4417-9625-72CE882EDA97"), "EXEC db_addClassAssociations 'PhysicalDataComponent','PhysicalDataComponent','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A3186AAD-BAE2-4AD9-ACE7-919A86C67EBC"), "EXEC db_addClassAssociations 'PhysicalDataComponent','PhysicalSoftwareComponent','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("D7CBA8B0-0067-4B4D-BB5F-04D174C36CDB"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','PhysicalDataComponent','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("AA8353F9-230E-405E-91E3-089DE78949EB"), "EXEC db_addClassAssociations 'Object','PhysicalSoftwareComponent','DynamicFlow','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("25516C1C-3D81-4353-93B3-D32F46D119D3"), "EXEC db_AddPossibleValues 'ObjectType','ExternalE',5,'ExternalEntity',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EB72F532-D7E1-4CCB-9E8D-CFD6E8B8B32D"), "EXEC db_AddPossibleValues 'ObjectType','ExternalEntity',5,'ExternalEntity',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CC4E1806-3BA4-4347-AB19-EDA3017A5629"), "EXEC db_AddPossibleValues 'GovernanceMechType','Control',11,'Control',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("788C32A4-8465-426F-A45C-3EDA2210AA90"), "EXEC db_addArtifacts 'NetworkComponent','NetworkComponent','ConnectedTo','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BB3D809D-746C-4427-94A4-CB5C4249A56F"), "EXEC db_addArtifacts 'NetworkComponent','StorageComponent','ConnectedTo','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("58B333DE-7087-4E78-B838-232F44D4F267"), "EXEC db_addArtifacts 'StorageComponent','NetworkComponent','ConnectedTo','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("48812FD8-15BA-4382-A046-C5499F105436"), "EXEC db_addArtifacts 'ComputingComponent','NetworkComponent','DynamicFlow','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3C826386-3A95-4DE3-9505-73AC5103ABD1"), "EXEC db_addArtifacts 'NetworkComponent','Network','DynamicFlow','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A0E37E04-2E2D-41C1-9C2E-20B380B60A34"), "EXEC db_addArtifacts 'Network','SystemComponent','DynamicFlow','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("95995F6B-5E9D-4615-AC28-44B3A0164BCB"), "EXEC db_addArtifacts 'Network','ComputingComponent','DynamicFlow','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CAB548B4-FEA2-4FFE-AE1C-7EBB99CBECEA"), "EXEC db_addArtifacts 'SystemComponent','Network','DynamicFlow','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A9CAFCCB-B7DC-4F9C-BBA1-FB73049AAA18"), "EXEC db_addArtifacts 'ComputingComponent','Network','DynamicFlow','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2988B535-8607-49B8-BECF-EBB4D2D32526"), "EXEC db_addArtifacts 'Network','NetworkComponent','DynamicFlow','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("516862D8-8BD7-481E-8E8F-2F5A350C21BE"), "EXEC db_addArtifacts 'Network','StorageComponent','DynamicFlow','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("341C45A0-FB4C-457F-B356-33E3B40CE20F"), "EXEC db_addArtifacts 'Network','Peripheral','DynamicFlow','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("3E11FAFA-3495-4209-BA6B-4573F44F4990"), "EXEC db_addArtifacts 'Peripheral','Network','DynamicFlow','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("5C97E8FD-F9D7-4EB1-BE3B-75ADDD9BBB72"), "EXEC db_addArtifacts 'Network','PeripheralComponent','DynamicFlow','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("82A2280E-0611-492E-AFE4-666D811B08A9"), "EXEC db_addArtifacts 'PeripheralComponent','Network','DynamicFlow','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A0B2E9F3-5CF8-4989-856F-61E136F2E4D4"), "EXEC db_addArtifacts 'NetworkComponent','Network','DynamicFlow','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("258ECBC9-71F9-463E-8D94-F4A9637BAEF3"), "EXEC db_addArtifacts 'StorageComponent','Network','DynamicFlow','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CC2810F3-EBEA-4EB2-8A2F-26D82388152B"), "EXEC db_addArtifacts 'PhysicalDataComponent','Function','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("79C8F178-DC86-4123-87A8-4F551E46F38C"), "EXEC db_addArtifacts 'PhysicalDataComponent','Process','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CE66AFED-397F-4506-9C8F-53A81DF7A90A"), "EXEC db_addArtifacts 'PhysicalDataComponent','Activity','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C2A1C7E2-55F5-45DA-87D6-AA1DD3CA14A5"), "EXEC db_addArtifacts 'PhysicalDataComponent','PhysicalDataComponent','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("8145C535-86D9-420B-A92B-12BEDE1D1296"), "EXEC db_addArtifacts 'PhysicalDataComponent','PhysicalSoftwareComponent','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A9EBF6D5-B9A6-4112-8BA3-7BB059F73667"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','PhysicalDataComponent','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("60D58E91-CE1D-49AA-8BFF-7D94A15860CD"), "EXEC db_addArtifacts 'Object','PhysicalSoftwareComponent','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");

            databaseVersions.Add(new Guid("635E9D62-3401-44F4-BC73-FEDFD2D21F3E"), "UPDATE AllowedArtifact SET IsActive = 0 WHERE Class LIKE 'Connection%'|MAJOR METAMODEL CHANGE SAPPI ONLY");

            #endregion

            #region 11 June 2014

            databaseVersions.Add(new Guid("53F4B86B-62B3-48CE-A499-0ACB6DE0A797"), "EXEC db_addClassAssociations 'Object','NetworkComponent','Mapping','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("64C7E5B5-53FC-4ECD-A2B4-1C7B07F86214"), "EXEC db_addClassAssociations 'NetworkComponent','Network','DynamicFlow','',0,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("FBAD51D8-4CF9-4C1F-BE37-EB16AF484CC6"), "EXEC db_addClassAssociations 'NetworkComponent','Network','ConnectedTo','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4EA6CA30-E7FC-44FA-AAA6-F4601B2EF6A4"), "EXEC db_addClassAssociations 'NetworkComponent','Object','Mapping','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("29F6484B-F439-4869-A3B3-BC84801EDF6A"), "EXEC db_addClassAssociations 'Network','NetworkComponent','DynamicFlow','',0,0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("82E5C874-2646-4526-9E8A-B5D2EC08E631"), "EXEC db_addClassAssociations 'Network','NetworkComponent','ConnectedTo','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("CEDF66FA-0339-40C8-A0A2-C56234191544"), "EXEC db_addClassAssociations 'Object','Network','Mapping','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("BEEACA59-D6A2-474B-B160-F2E571A8AE04"), "EXEC db_addClassAssociations 'NetworkComponent','NetworkComponent','ConnectedTo','',1,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("87458CAC-A319-42F9-8945-C175C0E87410"), "EXEC db_AddPossibleValues 'NetworkType','Wireless',5,'Wireless',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("C09ACF75-BB4F-4008-9AB9-74FE7764E2C0"), "EXEC db_AddPossibleValues 'SoftwareType','Exec',11,'Exec',0|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("474EBDCF-69C2-421B-8C37-78E0AD659CCA"), "EXEC db_addArtifacts 'NetworkComponent','NetworkComponent','ConnectedTo','Network',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("B4D3474B-5A51-43C8-979A-AC2E114AC8C2"), "UPDATE DomainDefinition SET Name = 'PrimaryOrBackupType' WHERE Name = 'IsPrimaryOrBackup'|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("553C43EF-020F-4C7C-8FD8-22AA76BC347F"), "UPDATE Field SET DataType = 'PrimaryOrBackupType' WHERE DataType = 'IsPrimaryOrBackup'|MAJOR METAMODEL CHANGE SAPPI ONLY");

            databaseVersions.Add(new Guid("DAE50981-1030-4F71-9C44-88954F98C8F8"), "EXEC db_AddFields 'Network','PrimaryOrBackupType','PrimaryOrBackupType','General','',0,2,1|MAJOR METAMODEL CHANGE SAPPI ONLY");

            databaseVersions.Add(new Guid("18741855-4B7E-4817-8F34-B39D4A0B8C5A"), "update classassociation set isdefault = 0, isactive = 0 where parentclass = 'networkcomponent' and childclass = 'network'|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("316248C3-B37D-4030-9281-44F885800E9B"), "update classassociation set isdefault = 1, isactive = 1 where parentclass = 'networkcomponent' and childclass = 'network' and associationtypeid=30|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2883D2DF-E05B-4722-AE62-C6E196C2D948"), "update classassociation set isdefault = 0, isactive = 0 where parentclass = 'network' and childclass = 'networkcomponent'|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("0CFCCDF2-996E-4A62-A6F5-03DC225CA1DB"), "update classassociation set isdefault = 1, isactive = 1 where parentclass = 'network' and childclass = 'networkcomponent' and associationtypeid=30|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("384938E0-A4B7-4BF0-8999-AFE3A441FA2E"), "update classassociation set isdefault = 0 where parentclass = 'object' and childclass = 'networkcomponent'|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("13F94B2F-4020-4558-ABC5-FFD8CD6225D2"), "update classassociation set isdefault = 1 where parentclass = 'object' and childclass = 'networkcomponent' and associationtypeid=4|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("4A05030D-BF3B-4103-B114-77600ACF4031"), "update classassociation set isdefault = 0 where parentclass = 'networkcomponent' and childclass = 'networkcomponent'|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("EE53CCE5-133F-4027-B21E-D3DA5DF51B60"), "update classassociation set isdefault = 1 where parentclass = 'networkcomponent' and childclass = 'networkcomponent' and associationtypeid=30|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("A52C9C42-5BDA-4173-8871-5DBF71A07F74"), "update classassociation set isdefault = 0 where parentclass = 'networkcomponent' and childclass = 'object'|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("51547F8F-2F23-4CD2-896B-86CC69208788"), "update classassociation set isdefault = 1 where parentclass = 'networkcomponent' and childclass = 'object' and associationtypeid=4|MAJOR METAMODEL CHANGE SAPPI ONLY");

            databaseVersions.Add(new Guid("892138E1-F29C-40C4-BF6C-CC423E923E51"), "EXEC db_addClassAssociations 'PhysicalDataComponent','Object','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("16DE7A2B-2AD0-42C4-9AC1-5C4C717F3AF4"), "EXEC db_addClassAssociations 'Object','PhysicalDataComponent','DynamicFlow','',0,1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("2007E40C-3F66-46CD-8225-8D5472D56076"), "EXEC db_addArtifacts 'PhysicalDataComponent','Object','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");
            databaseVersions.Add(new Guid("1EDA4062-432E-4258-95C4-03F6CBF7606A"), "EXEC db_addArtifacts 'Object','PhysicalDataComponent','DynamicFlow','FlowDescription',1|MAJOR METAMODEL CHANGE SAPPI ONLY");

            #endregion

            //remap all objects
            //databaseVersions.Add(new Guid("5C254ED9-FC30-48F2-82A2-CE4A862BD8B6"), "REMAP|REMAP");

            #region ExecutionIndicatorImages

            databaseVersions.Add(new Guid("1847122C-0F8B-45FF-8CFD-38CBF1E58354"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\auto.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("4E18D406-B049-4E90-83AE-FA1F3A69469C"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\ManualEI.png')|Default DDPV Images");
            databaseVersions.Add(new Guid("21657574-B955-41F5-A631-EBBB9D4453C0"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\semiauto.png')|Default DDPV Images");

            databaseVersions.Add(new Guid("98E61C18-4E99-4945-BD47-EE5719E56A48"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 50 WHERE PossibleValue = 'Auto'|Default Images");
            databaseVersions.Add(new Guid("48F214BD-3BF1-4D28-9766-88B75736ED2E"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 51 WHERE PossibleValue = 'Manual' AND DomainDefinitionID IN (SELECT pkid FROM DomainDefinition WHERE Name LIKE 'ExecutionIndicator')|Default Images");
            databaseVersions.Add(new Guid("2273E7E1-82B3-4C7B-A7C6-16276B560095"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 52 WHERE PossibleValue = 'SemiA'|Default Images");

            #endregion

            #region 26 June 2014

            databaseVersions.Add(new Guid("AAF1F275-3260-4291-ABEC-B6E99C6B51E9"), "UPDATE Field SET SortOrder = 2 WHERE Class = 'Network' AND Name = 'NetworkType'|Reorder Network Fields");
            databaseVersions.Add(new Guid("59ABD731-AC0C-483C-AE96-76EBB08CACEF"), "UPDATE Field SET SortOrder = 1 WHERE Class = 'Network' AND Name = 'PrimaryOrBackupType'|Reorder Network Fields");

            #endregion

            #region 30 June 2014

            databaseVersions.Add(new Guid("7811647F-FA89-4127-8FE2-41E8FF06C70F"), "UPDATE DomainDefinitionPossibleValue SET PossibleValue = 'Primary' WHERE PossibleValue = 'PrimaryConnection' AND DomainDefinitionID IN (SELECT pkid FROM DomainDefinition WHERE Name = 'PrimaryOrBackupType')|Rename PorBValues");
            databaseVersions.Add(new Guid("7E425282-3DCD-48EA-8957-A99CAD25A658"), "UPDATE DomainDefinitionPossibleValue SET PossibleValue = 'Backup' WHERE PossibleValue = 'BackupConnection' AND DomainDefinitionID IN (SELECT pkid FROM DomainDefinition WHERE Name = 'PrimaryOrBackupType')|Rename PorBValues");

            //the next 4 are for REALLY OLD databases
            longQuery = "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[______CLEAROBJECTSMFD]') AND type in (N'P', N'PC')) DROP PROCEDURE [dbo].[______CLEAROBJECTSMFD]";
            databaseVersions.Add(new Guid("B3695C49-0638-4B16-9D94-31259894FF5D"), longQuery + "|Drop Create Clear");

            longQuery = "CREATE PROCEDURE [dbo].[______CLEAROBJECTSMFD]    @IncludeVCItems bit AS BEGIN    if (@IncludeVCItems=1)    BEGIN        delete from graphfileobject        delete from graphfileassociation        delete from graphfile        delete from artifact        delete from objectassociation        delete from objectfieldvalue        delete from metaobject        delete from userpermission where not workspacename= 'Sandbox'        delete from Workspace where not name= 'Sandbox'        RETURN    END DELETE FROM graphfileobject WHERE NOT dbo.GetIdentifier(GraphFileID,GraphFileMachine) IN (    SELECT dbo.GetIdentifier(pkid,machine) FROM GraphFile    WHERE     VCStatusID=2) OR dbo.GetIdentifier(metaobjectid,machineid)  NOT IN (    SELECT dbo.GetIdentifier(pkid,machine) FROM metaobject WHERE    VCStatusID=2) DELETE FROM graphfileassociation DELETE FROM graphfile WHERE NOT dbo.GetIdentifier(pkid,machine) IN (    SELECT dbo.GetIdentifier(pkid,machine)    FROM GraphFile    WHERE VCStatusID=2)DELETE FROM artifact DELETE FROM objectassociation DELETE FROM objectfieldvalue WHERE NOT dbo.GetIdentifier(objectid,machineid) IN (    SELECT dbo.GetIdentifier(pkid,machine)    FROM metaobject    WHERE VCStatusID=2) DELETE FROM graphfileobject where dbo.GetIdentifier(graphfileid,graphfilemachine) IN (    SELECT dbo.GetIdentifier(pkid, machine)    FROM graphfile    WHERE IsActive=0) DELETE FROM graphfile where isactive=0 DELETE FROM metaobject WHERE (NOT VCStatusID=2) DELETE FROM userpermission WHERE NOT workspacename= 'Sandbox' DELETE FROM Workspace WHERE NOT (name= 'Sandbox' OR WorkspaceTypeID=3) END";
            databaseVersions.Add(new Guid("065F4AC4-DA30-4A66-AD18-529E57A8B23C"), longQuery + "|Drop Create Clear");

            longQuery = "IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ActiveDiagramObjects]')) DROP VIEW [dbo].[ActiveDiagramObjects]";
            databaseVersions.Add(new Guid("D6496726-C162-442B-8876-0E12EB8B73E7"), longQuery + "|Drop Create ADO View");

            longQuery = "CREATE VIEW [dbo].[ActiveDiagramObjects] AS SELECT dbo.GraphFile.pkid, dbo.TrimFileName(dbo.GraphFile.Name) AS [File Name], dbo.GraphFileObject.MetaObjectID, dbo.GraphFileObject.MachineID AS Machine FROM dbo.GraphFile INNER JOIN dbo.GraphFileObject ON dbo.GraphFile.pkid = dbo.GraphFileObject.GraphFileID AND dbo.GraphFile.Machine = dbo.GraphFileObject.GraphFileMachine";
            databaseVersions.Add(new Guid("C16FF078-740F-498F-A121-969D9745E8E0"), longQuery + "|Drop Create ADO View");

            #endregion

            #region 'Satallite'
            databaseVersions.Add(new Guid("E1EBE773-1DF1-4BCC-82E9-9D6A23E3DB4A"), "UPDATE ObjectFieldValue SET ValueString = 'Satellite' WHERE FieldID IN (SELECT pkid FROM Field WHERE DataType = 'ConnectionType')|Rename Satallite (Value)");
            databaseVersions.Add(new Guid("183A7284-1188-40A0-9DFA-068E76997E17"), "UPDATE DomainDefinitionPossibleValue SET PossibleValue = 'Satellite',Description = 'Satellite' WHERE PossibleValue = 'Satallite'|Rename Satallite (PossibleValue)");
            databaseVersions.Add(new Guid("C6046CB1-8779-4D99-BF4C-8D99F274532D"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = 32 WHERE PossibleValue = 'Satellite' AND URI_ID IS NULL|Rename Satallite (Image)");
            #endregion

            #region ADO Above does not include isactive (?)
            longQuery = "IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ActiveDiagramObjects]')) DROP VIEW [dbo].[ActiveDiagramObjects]";
            databaseVersions.Add(new Guid("888B7BDC-B866-4965-930E-2B0BDA497FD2"), longQuery + "|Drop Create ADO View");
            longQuery = "CREATE VIEW [dbo].[ActiveDiagramObjects] AS SELECT dbo.GraphFile.pkid, dbo.TrimFileName(dbo.GraphFile.Name) AS [File Name], dbo.GraphFileObject.MetaObjectID, dbo.GraphFileObject.MachineID AS Machine FROM dbo.GraphFile INNER JOIN dbo.GraphFileObject ON dbo.GraphFile.pkid = dbo.GraphFileObject.GraphFileID AND dbo.GraphFile.Machine = dbo.GraphFileObject.GraphFileMachine WHERE dbo.GraphFile.IsActive=1";
            databaseVersions.Add(new Guid("5096B4CA-25BB-40B2-A3FA-F9FCB2337E16"), longQuery + "|Drop Create ADO View");

            databaseVersions.Add(new Guid("6EAA053F-924B-45F7-9739-D7FA6346EC30"), "UPDATE ObjectFieldValue SET ValueString = 'Satellite' WHERE FieldID IN (SELECT pkid FROM Field WHERE DataType = 'ConnectionType') AND ValueString = 'Satallite'|Rename Satallite (Value-Correctly)");

            databaseVersions.Add(new Guid("F9BD2B6B-287D-4D28-8CF1-86A466A81789"), "EXEC db_addClassAssociations 'DataView','PhysicalDataComponent','Mapping','',1,1|dataviewMdatacomp");
            databaseVersions.Add(new Guid("75C97706-8DFC-4BDE-8FB6-A4E19B1B5341"), "EXEC db_addClassAssociations 'PhysicalDataComponent','DataTable','Mapping','',1,1|datacompMtable");

            #endregion

            #region 30 July 2014

            //reactivate associations
            databaseVersions.Add(new Guid("FD0E7DFA-C1F1-465C-8C82-FF24E197FD7F"), "update classassociation set isactive = 1 where associationtypeid = 32 or associationtypeid = 10 or associationtypeid = 18 or associationtypeid =20 or associationtypeid = 23|Reactivating Associations");
            databaseVersions.Add(new Guid("040F8C76-1BD6-4136-8773-FF293FE3C05C"), "exec db_AddClassAssociations 'Activity','DataView','Mapping','',0,1|Reactivating Associations");
            databaseVersions.Add(new Guid("106AB1A4-D480-4C9B-AD98-FE1AFEDB95A4"), "exec db_AddClassAssociations 'DataView','Activity','Mapping','',0,1|Reactivating Associations");
            databaseVersions.Add(new Guid("45C9871E-0F04-4731-AA6D-3D83969EAE68"), "exec db_AddClassAssociations 'DataView','Function','Mapping','',0,1|Reactivating Associations");
            databaseVersions.Add(new Guid("C6BB3293-1EB3-4150-B783-3230CFFE203C"), "exec db_AddClassAssociations 'DataView','Process','Mapping','',0,1|Reactivating Associations");
            databaseVersions.Add(new Guid("3F0DE8EC-B2B7-4DA0-8829-A936C5E3044B"), "exec db_AddClassAssociations 'Entity','Function','DynamicFlow','',0,1|Reactivating Associations");
            databaseVersions.Add(new Guid("B18FB88A-B626-49D4-A1A2-60ECF5A81555"), "exec db_AddClassAssociations 'Function','DataView','Mapping','',0,1|Reactivating Associations");
            databaseVersions.Add(new Guid("3A6F3A07-E43E-4D3A-8D70-48953ED8AB92"), "exec db_AddClassAssociations 'Process','DataView','Mapping','',0,1|Reactivating Associations");

            databaseVersions.Add(new Guid("56C111BB-D59C-4C79-8BB6-0BEA1EA73223"), "update classassociation set associationtypeid=1 where associationtypeid=37|Aux Fix");
            databaseVersions.Add(new Guid("8759B483-2E32-430C-BC90-EAA8270DCF8F"), "delete from associationtype where name = 'Auxilary'|Aux Fix");
            #endregion

            #region data
            databaseVersions.Add(new Guid("94C98941-1B97-496C-9BC8-5B88289A1931"), "exec db_AddClassAssociations 'DataTable','DataTable','One_To_One','',0,1|New Associations");
            databaseVersions.Add(new Guid("C7CB6696-F853-49D6-AAAD-87EBB9E2F5DF"), "exec db_AddClassAssociations 'Entity','Entity','Dependencies','',0,1|New Associations");
            //databaseVersions.Add(new Guid(""), "exec db_AddClassAssociations 'Entity','Entity','FunctionalDependency','',0,0|New Associations");
            databaseVersions.Add(new Guid("B6E447BC-7562-4A5A-B05D-F98A6D81CFAC"), "exec db_AddClassAssociations 'GovernanceMechanism','GovernanceMechanism','FunctionalDependency','',0,0|New Associations");
            databaseVersions.Add(new Guid("7E2E13DD-2CF1-4D5F-8737-3AD811CC1A7B"), "exec db_AddClassAssociations 'GovernanceMechanism','GovernanceMechanism','Dependencies','',0,1|New Associations");
            databaseVersions.Add(new Guid("0CB59984-0850-4132-80D8-4E2FE4572A22"), "update associationtype set name = 'Dependency' where name = 'Dependencies'|New Associations");
            //databaseVersions.Add(new Guid(""), "update associationtype set isactive = 0 where name = 'FunctionalDependency'|New Associations");
            //rename dependencies to dependency
            databaseVersions.Add(new Guid("D426BEF1-23A7-4C5F-AE62-5EDF1A06DF7D"), "exec db_AddClassAssociations 'PhysicalDataComponent','DataView','Mapping','',0,1|New Associations");
            databaseVersions.Add(new Guid("073EB690-2FD0-4F60-B7C8-39BFCD9089E6"), "exec db_AddClassAssociations 'PhysicalDataComponent','DataSchema','Mapping','',0,1|New Associations");
            databaseVersions.Add(new Guid("BD1B488B-C591-4032-A29B-91B0A4D6AEB9"), "exec db_AddClassAssociations 'DataView','PhysicalDataComponent','Mapping','',0,1|New Associations");
            //databaseVersions.Add(new Guid("073EB690-2FD0-4F60-B7C8-39BFCD9089E6"), "exec db_AddClassAssociations 'DataSchema','PhysicalDataComponent','Mapping','',0,1|New Associations"
            //flow description tostring
            databaseVersions.Add(new Guid("E009C700-0C12-4A74-B4C7-7368989B4F55"), "EXEC db_AddPossibleValues 'DataSchemaType','Physical',2,'Physical',0|DataSchemaType Values");
            databaseVersions.Add(new Guid("DAF07F35-8594-4385-9D7C-D30E215D3071"), "EXEC db_AddPossibleValues 'DataSchemaType','PhysicalDB',3,'Physical Database',0|DataSchemaType Values");
            databaseVersions.Add(new Guid("E10ED507-50CB-49EB-AD15-752022433DD5"), "EXEC db_AddPossibleValues 'DataSchemaType','Conceptual',2,'Conceptual',1|DataSchemaType Values");

            databaseVersions.Add(new Guid("8E33390A-510A-4D71-AD11-53D33BADB64C"), "REBUILDVIEWS|REBUILDVIEWS");

            databaseVersions.Add(new Guid("62C82907-58CE-43EA-8BA7-212C46E0CA67"), "Update Classassociation set parentclass = 'DataView' where parentclass = 'Dataview'|CaseFix");
            databaseVersions.Add(new Guid("299E1376-C2EF-4835-86F5-2B659AF12CA4"), "Update Classassociation set childclass = 'DataView' where childclass = 'Dataview'|CaseFix");
            #endregion

            //MAJOR METAMODEL CHANGES - Remove Dataview
            #region Application Interfaces Model

            databaseVersions.Add(new Guid("478AAC08-C5F9-45D8-A5E4-88F468B4E055"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','E_Mess',1,'E_Mess',0|AppInterfaceModel");
            databaseVersions.Add(new Guid("3A195B41-8C5F-41DD-9D40-7BA07651C9B9"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','E_Fax',4,'E_Fax',0|AppInterfaceModel");
            databaseVersions.Add(new Guid("144891A3-C508-4127-B56B-EB7C855B563A"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','E_File',5,'E_File',0|AppInterfaceModel");
            databaseVersions.Add(new Guid("4102E2AD-FAF8-42A3-AA04-E9AECD00348E"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','E_Doc',6,'E_Doc',0|AppInterfaceModel");
            databaseVersions.Add(new Guid("AA3DC49F-F4E6-4DCE-BC50-B5AC6B3AEBAE"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','E_Letter',7,'E_Letter',0|AppInterfaceModel");
            databaseVersions.Add(new Guid("188DF2EF-E410-422E-90DD-FEADDDEB88AB"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','E_Report',8,'E_Report',0|AppInterfaceModel");
            databaseVersions.Add(new Guid("44A9CE34-0746-4996-81A6-3EEC2A24CC61"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','E_MM',9,'E_MM',0|AppInterfaceModel");
            databaseVersions.Add(new Guid("A1F48F35-E227-4F45-8621-F2669E6C4666"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','E_Screen',10,'E_Screen',0|AppInterfaceModel");
            databaseVersions.Add(new Guid("DD1C16B8-31D6-4A8B-91DE-C5A8CA5EF4CD"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','E_Form',11,'E_Form',0|AppInterfaceModel");
            databaseVersions.Add(new Guid("EBB2D881-5B5D-4479-94A1-9E3162E18B02"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','P_Doc',12,'P_Doc',0|AppInterfaceModel");
            databaseVersions.Add(new Guid("71F60D1E-079A-4EC9-8795-64EBAEDD541D"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','P_Fax',13,'P_Fax',0|AppInterfaceModel");
            databaseVersions.Add(new Guid("5789F27F-866C-45A1-A22D-E07F4F740C27"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','P_Form',14,'P_Form',0|AppInterfaceModel");
            databaseVersions.Add(new Guid("C19052E3-7AAC-400C-B8D6-FDB6D26B1620"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','P_Letter',15,'P_Letter',0|AppInterfaceModel");
            databaseVersions.Add(new Guid("23A3E017-D8FD-4ED7-A7E6-46CEDB3E2A78"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','P_Mess',16,'P_Mess',0|AppInterfaceModel");
            databaseVersions.Add(new Guid("1349A813-A9BC-4DC2-B02D-410D4319D825"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','P_Rep',17,'P_Rep',0|AppInterfaceModel");
            databaseVersions.Add(new Guid("F57D8EF3-83C4-471E-8F50-AC84E7026B41"), "EXEC db_AddPossibleValues 'AbstractionLevel','Conceptual',1,'Conceptual',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("0B630F95-D138-4E92-B9A6-219E73FD66CD"), "EXEC db_AddPossibleValues 'AbstractionLevel','Logical',2,'Logical',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("AD0EB5B2-C09F-4BF8-889B-0196092C6221"), "EXEC db_AddPossibleValues 'AbstractionLevel','Physical',3,'Physical',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("3A75C26A-E72D-41BE-BB58-1221B31CA2E4"), "EXEC db_AddPossibleValues 'AchitectureState','Reference',1,'Reference',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("2EDC3F2B-841F-4B4B-8CF1-ECC78D59A717"), "EXEC db_AddPossibleValues 'AchitectureState','Required',2,'Required',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("82AD60AC-7F0F-4AEB-A1EF-5F163EB17674"), "EXEC db_AddPossibleValues 'AchitectureState','Baseline',3,'Baseline',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("93CCBBCB-3B64-4084-873C-83380D1B5B48"), "EXEC db_AddPossibleValues 'AchitectureState','Transition',4,'Transition',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("15742C2F-CF67-4A4C-943B-16719AF94413"), "EXEC db_AddPossibleValues 'AchitectureState','Target',5,'Target',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("06DF2EDA-3F01-4F9C-8ADE-30AFAAA02086"), "EXEC db_AddPossibleValues 'AchitectureState','Decommissioned',6,'Decommissioned',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("C4C10452-4DC1-416F-9BBD-CBE857219A0D"), "EXEC db_AddPossibleValues 'DataType','Boolean',1,'Boolean',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("25EBF877-B37D-42DE-9363-5E70C49B47D5"), "EXEC db_AddPossibleValues 'DataType','Calculation',2,'Calculation',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("3DC382E4-DAD4-49C6-8041-3C3D975614D2"), "EXEC db_AddPossibleValues 'DataType','Currency',3,'Currency',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("11251C7C-76AD-4EA5-9AD4-D7B7782481BA"), "EXEC db_AddPossibleValues 'DataType','Date',4,'Date',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("A9E23851-7794-4F39-A507-5A05085A2D9D"), "EXEC db_AddPossibleValues 'DataType','DateTime',5,'DateTime',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("34C9C37D-AB81-4A66-B5A1-E8979B1026A4"), "EXEC db_AddPossibleValues 'DataType','General',12,'General',0|AppInterfaceModel");
            databaseVersions.Add(new Guid("723FFA79-6560-46EC-9421-0E8EACBD506E"), "EXEC db_AddPossibleValues 'DataType','Integer',6,'Integer',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("C557465C-C008-4AD2-9381-DC0D94F38054"), "EXEC db_AddPossibleValues 'DataType','List',7,'List',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("23760102-1FAD-4533-A938-39323C1D2C83"), "EXEC db_AddPossibleValues 'DataType','Memo',8,'Memo',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("FAC1F4C2-6820-4D15-94AB-193E39A73FC9"), "EXEC db_AddPossibleValues 'DataType','MultiMedia',9,'MultiMedia',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("6598BE10-78F2-4AE8-9436-68F479352003"), "EXEC db_AddPossibleValues 'DataType','Number',10,'Number',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("AE8A82F1-74F9-48E0-AEF7-F3DB17498C2D"), "EXEC db_AddPossibleValues 'DataType','Percentage',11,'Percentage',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("561BB63B-047E-470B-AB38-8392195605FF"), "EXEC db_AddPossibleValues 'DataType','String',12,'String',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("2BCC4C6B-60C1-4D13-AEF8-BE53BBFCB79C"), "EXEC db_AddPossibleValues 'DataType','URI',13,'URI',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("A20FFFB9-F632-41F1-8AFE-0C36F376B382"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','E_Message',1,'E_Message',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("09C0522A-C71C-408E-92E6-8040BD39865F"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','TextMessage',2,'TextMessage',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("A342E655-2537-4A74-A2F6-30EAA213FCAC"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','Email',3,'Email',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("0B2824D0-86D4-45DE-B1D6-83A79A8756CB"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','Fax',4,'Fax',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("B9FE71AF-1D38-4C6E-B2B8-D01A519682A0"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','ElectronicFile',5,'ElectronicFile',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("8E9509F6-BF9C-4551-92E5-8F61614D77E3"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','ElectronicDocument',6,'ElectronicDocument',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("9685AE60-D041-4618-B2D3-E714F6CB79C2"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','ElectronicLetter',7,'ElectronicLetter',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("F1B864F9-0614-46CA-A3C9-838EAB9C447C"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','ElectronicReport',8,'ElectronicReport',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("E0E0DD96-B4EF-4A7E-9ABC-FDE5C35F70F9"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','MultiMedia',9,'MultiMedia',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("D2288074-CC78-459A-ADFE-3658DDE136AD"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','ComputerDisplay',10,'ComputerDisplay',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("860BDA60-0331-409D-A6E7-6607ED270E79"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','eForm',11,'eForm',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("9202BCB9-179A-4746-BF29-6D0473C59EAC"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','PaperDocument',12,'PaperDocument',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("A6C15E0A-1422-40BF-A7FA-446A3356414D"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','PaperFax',13,'PaperFax',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("AC3C8285-0C62-45FF-8144-DD4AE04B9A09"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','PaperForm',14,'PaperForm',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("BF9020BE-D25E-447B-9E16-D02125E949E0"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','PaperLetter',15,'PaperLetter',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("3F2D3D32-2F56-48FB-8DDE-822511574DDF"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','PaperMessage',16,'PaperMessage',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("63A4B7B6-82DF-40F0-A650-5F7D3EEA640C"), "EXEC db_AddPossibleValues 'PhysicalInformationArtefactType','PaperReport',17,'PaperReport',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("CADD58B5-5B52-4AE5-BF37-1642EEA329E3"), "EXEC db_AddPossibleValues 'IsRecord','Yes',1,'Yes',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("183F2655-6EA8-4056-82DC-A14BEC1717B1"), "EXEC db_AddPossibleValues 'IsRecord','No',2,'No',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("702D3A56-3125-437F-A04E-D4256E3B23C5"), "EXEC db_AddPossibleValues 'MeasureCategory','Performance',1,'Performance',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("536D5F1D-FBC0-409C-86DF-B1D92B099A65"), "EXEC db_AddPossibleValues 'MeasureCategory','Value',2,'Value',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("EF014EC5-A0A3-4216-81C9-67786D428A24"), "EXEC db_AddPossibleValues 'MeasureCategory','Cost',3,'Cost',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("45E82B69-4FA6-4814-9F56-5AD7429A3268"), "EXEC db_AddPossibleValues 'MeasureCategory','Quality',4,'Quality',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("ED6847ED-4A0F-4673-B992-ACD51CC655A7"), "EXEC db_AddPossibleValues 'MeasureCategory','Availability',5,'Availability',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("2FBC12A4-A1AA-476F-95B6-29C0DDBEF56B"), "EXEC db_AddPossibleValues 'MeasureCategory','Reliability',6,'Reliability',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("2E64FFAF-11F6-45D9-8AAB-C6BAFCC18C4D"), "EXEC db_AddPossibleValues 'MeasureCategory','Security',7,'Security',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("63A19210-0224-480B-BE17-D1F3E9C1BF4D"), "EXEC db_AddPossibleValues 'MeasureCategory','Maintainability',8,'Maintainability',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("41B122C6-FC57-4EF4-895B-CCBB2BBBE797"), "EXEC db_AddPossibleValues 'MeasureCategory','Privacy',9,'Privacy',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("EBF5C043-E065-4D30-B2F4-AAC0942CED20"), "EXEC db_AddPossibleValues 'DiagramStatus','Created',1,'Created',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("4C6210E3-4184-401E-AD34-0FD28F4B46A2"), "EXEC db_AddPossibleValues 'DiagramStatus','Reviewed',2,'Reviewed',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("CAF934B5-F4DA-4519-A17E-FCEBCEF95773"), "EXEC db_AddPossibleValues 'DiagramStatus','Updated',3,'Updated',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("02888CEC-B76D-43E3-B05C-B870ECBA0820"), "EXEC db_AddPossibleValues 'DiagramStatus','QAed',4,'QAed',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("42FA8AA3-AAB7-49B5-A7DF-E287C0EFB0BF"), "EXEC db_AddPossibleValues 'DiagramStatus','Published',5,'Published',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("EA64A24E-CAD4-4C04-B515-DE84E05F08F8"), "EXEC db_AddPossibleValues 'DiagramType','PrimitiveModel',1,'PrimitiveModel',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("C30DDF86-51F1-4E14-B793-7A779FEA7BFB"), "EXEC db_AddPossibleValues 'DiagramType','CompositeModel',2,'CompositeModel',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("2342589A-941F-48BD-A8CC-F206B5AD9D9B"), "EXEC db_AddPossibleValues 'DiagramType','Matrix',3,'Matrix',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("7DBEEBB6-77A7-433E-B951-E929BB9F33A7"), "EXEC db_AddPossibleValues 'DiagramType','View',4,'View',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("573FEEBA-B822-4AA7-A2D5-1C66577636D5"), "EXEC db_AddPossibleValues 'DiagramType','Viewpoint',5,'Viewpoint',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("41989194-4F17-48CE-A5E1-4896EAD823B1"), "EXEC db_AddPossibleValues 'Priority','High',1,'High',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("F1C14166-A5A6-47D1-8C84-0B3A66E29FB5"), "EXEC db_AddPossibleValues 'Priority','Medium',2,'Medium',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("FB5A4E96-F8F0-47D0-890D-4B7FD1CC1BC2"), "EXEC db_AddPossibleValues 'Priority','Low',3,'Low',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("1B646E81-8C7D-4EAC-A3F0-E949A33FFF6F"), "EXEC db_AddPossibleValues 'RequirementRealizationStatus','Logged',1,'Logged',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("FE58A0B4-DD36-42A4-8791-E999712979D8"), "EXEC db_AddPossibleValues 'RequirementRealizationStatus','Planned',2,'Planned',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("FBAD2955-8A13-440C-87D2-A8C84B49D7CA"), "EXEC db_AddPossibleValues 'RequirementRealizationStatus','InProgress',3,'InProgress',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("A1DB0EF7-26FC-4865-852E-4C7E77F28FF6"), "EXEC db_AddPossibleValues 'RequirementRealizationStatus','Realized',4,'Realized',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("4F38FBAB-8B22-41F7-B211-CDA8C9144D5F"), "EXEC db_AddPossibleValues 'SecurityClassification','Unclassified',1,'Unclassified',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("1338A978-08D5-425D-AB48-EA2B8F916694"), "EXEC db_AddPossibleValues 'SecurityClassification','Official',2,'Official',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("037ED819-8E86-47D6-ADE5-75284165E107"), "EXEC db_AddPossibleValues 'SecurityClassification','Restricted',3,'Restricted',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("35F7606A-6C94-476C-A6D1-7EE1FE34D2F8"), "EXEC db_AddPossibleValues 'SecurityClassification','Confidential',4,'Confidential',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("F798DA79-4AEE-4141-96E1-36F19AE440DC"), "EXEC db_AddPossibleValues 'SecurityClassification','Secret',5,'Secret',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("E207D451-4B2F-48B2-BC5D-77CDB3013FD3"), "EXEC db_AddPossibleValues 'FormatStandard','iDoc',1,'iDoc',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("7260F55E-21AA-412E-8751-6FFE4BB2F60B"), "EXEC db_AddPossibleValues 'FormatStandard','XMI',2,'XMI',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("9E8E75C4-F946-45FE-BEC6-85796717433C"), "EXEC db_AddPossibleValues 'FormatStandard','XML',3,'XML',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("D7EACBAC-6AFC-4392-9D93-18765A4A6C66"), "EXEC db_AddPossibleValues 'FormatStandard','SWIFT',4,'SWIFT',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("405BB5C8-2AC2-48D4-AC4E-03281D714D95"), "EXEC db_AddPossibleValues 'InterfaceFrequency','RealTime',1,'RealTime',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("DD55FF16-10B1-4A33-98E3-030860FCEEF8"), "EXEC db_AddPossibleValues 'InterfaceFrequency','Every5Minutes',2,'Every5Minutes',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("7292CC68-0338-4F79-9020-C576A18F007E"), "EXEC db_AddPossibleValues 'InterfaceFrequency','Every10Minutes',3,'Every10Minutes',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("463D2317-6377-4EF3-9876-A6EB898C1406"), "EXEC db_AddPossibleValues 'InterfaceFrequency','Every15Minutes',4,'Every15Minutes',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("76101A71-E785-4113-967D-113C3CC5595A"), "EXEC db_AddPossibleValues 'InterfaceFrequency','Hourly',5,'Hourly',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("216C826A-8623-46BE-BF25-E52D345E4A0E"), "EXEC db_AddPossibleValues 'InterfaceFrequency','TwiceDaily',6,'TwiceDaily',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("F25279AB-5388-40F0-BAB3-64729436224A"), "EXEC db_AddPossibleValues 'InterfaceFrequency','Daily',7,'Daily',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("B04FF8C3-2D41-4018-848F-0649971B8093"), "EXEC db_AddPossibleValues 'InterfaceFrequency','Weekly',8,'Weekly',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("2322D005-DCF5-4FB7-B0C6-660A552347B2"), "EXEC db_AddPossibleValues 'InterfaceFrequency','Monthly',9,'Monthly',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("6BD87006-BF3A-41AD-9AAE-AA33A8B847BF"), "EXEC db_AddPossibleValues 'InterfaceFrequency','Annually',10,'Annually',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("9204C557-B263-4697-AB87-1CE8638D4D83"), "EXEC db_AddPossibleValues 'IsPlannedOrAdHoc','Planned',1,'Planned',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("1308082D-4B50-4BCC-AD03-05EB2B0FEF73"), "EXEC db_AddPossibleValues 'IsPlannedOrAdHoc','Adhoc',2,'Adhoc',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("6036BDBF-ED5F-4ABE-BB8C-12DA9F8FE5E6"), "EXEC db_AddPossibleValues 'RunsDuringWorkingHours','Yes',1,'Yes',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("4AF4C84E-F57B-4DDD-B88A-1FAC026B088A"), "EXEC db_AddPossibleValues 'RunsDuringWorkingHours','No',2,'No',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("CD3C869F-B25C-4FF0-A282-8D49DE2536D9"), "EXEC db_AddPossibleValues 'IsSynchronised','Yes',1,'Yes',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("F427C057-E97D-45AA-BDE7-1BE2CB734981"), "EXEC db_AddPossibleValues 'IsSynchronised','No',2,'No',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("D4AB4A12-D7FD-41A0-A14C-0EBDF580D1DB"), "EXEC db_addclasses 'Measure','Name','Strategy',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("6977AE67-7810-4B4B-8DB6-3393BA523C99"), "EXEC db_addclasses 'LogicalInformationArtefact','Name','Data',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("0F9A3BA6-7817-4591-BE35-3FA8EBB735E0"), "EXEC db_addclasses 'PhysicalInformationArtefact','Name','Data',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("EEE98EA1-3341-439E-97B2-EC6DF035516A"), "EXEC db_addclasses 'ApplicationInterface','Name','Application',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("D95F84AB-974F-4001-AE99-089EF18D9276"), "update class set isactive = 0 where name = 'dataview'|AppInterfaceModel");
            databaseVersions.Add(new Guid("3930F6FC-2482-4DD1-88FB-48C456238EAE"), "update classassociation set isactive = 0 where parentclass = 'dataview'|AppInterfaceModel");
            databaseVersions.Add(new Guid("99B9D31F-F795-4B79-8C02-6CFC3959EBD9"), "update classassociation set isactive = 0 where childclass = 'dataview'|AppInterfaceModel");
            databaseVersions.Add(new Guid("DF739787-E64E-4810-8623-EFA70320DD79"), "exec db_addfields 'Measure','MeasureType','System.String','General','',1,1,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("110F441A-6DA4-448D-8562-5D3730861F8F"), "exec db_addfields 'Measure','MeasureCategory','MeasureCategory','General','',1,2,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("7C0719E9-BEC6-4CE1-94F8-0DF6C3BFFCB6"), "exec db_addfields 'Measure','UnitOfMeasure','System.String','General','',1,3,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("4FB26826-48DF-4BD6-BD2A-A54DA78772E2"), "exec db_addfields 'PhysicalInformationArtefact','PhysicalInformationArtefactType','PhysicalInformationArtefactType','General','',1,1,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("45AB94D3-965D-47DC-B801-FBA39A9F43DE"), "exec db_addfields 'PhysicalInformationArtefact','FormatStandard','FormatStandard','General','',1,2,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("D6E316CD-6836-4ADB-9B68-507B2B732F16"), "exec db_addfields 'PhysicalInformationArtefact','ContentType','ContentType','General','',1,3,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("8E2D0917-41E2-4AD3-A661-85C41B0B6407"), "exec db_addfields 'PhysicalInformationArtefact','SecurityClassification','SecurityClassification','General','',1,4,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("6F49B6EE-5837-4377-A298-D90E82BD2777"), "exec db_addfields 'PhysicalInformationArtefact','IsRecord','IsRecord','General','',1,5,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("668DBB50-99A2-47F8-A72E-08963FA2EFDE"), "exec db_addfields 'ApplicationInterface','ExecutionIndicator','ExecutionIndicator','General','',1,1,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("9ECEF5BA-6800-4F5A-AA99-BFA958A1AE76"), "exec db_addfields 'ApplicationInterface','InterfaceFrequency','InterfaceFrequency','General','',1,2,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("C67314A4-82ED-42D0-9F2E-B10C549F175C"), "exec db_addfields 'ApplicationInterface','IsPlannedOrAdHoc','IsPlannedOrAdHoc','General','',1,3,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("94E15C6F-F36F-4DFD-8BC0-29961EB68C49"), "exec db_addfields 'ApplicationInterface','RunsDuringWorkingHours','RunsDuringWorkingHours','General','',1,4,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("912ABA6E-51F0-409A-9BA9-E1238CDF1EE4"), "exec db_addfields 'ApplicationInterface','IsSynchronised','IsSynchronised','General','',1,5,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("2F6219A3-37D9-49DF-818C-27C4C737C38E"), "exec db_addfields 'ApplicationInterface','InterfaceVolume','System.String','General','',1,6,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("12077B36-83DE-4166-B422-E04FD21ADC79"), "exec db_addfields 'Network','NetworkRange','System.String','General','',1,6,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("4310979C-EDDA-4F86-BFAF-68075BA8D369"), "exec db_addfields 'PeripheralComponent','SeverityRating','SeverityRating','General','',1,1,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("BCF90074-AD3A-4249-9F60-24FE218B86A7"), "exec db_addfields 'PeripheralComponent','ConfigurationID','System.String','General','',1,2,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("983352DC-AC8E-4FB2-A9D0-39A5BCCCC307"), "exec db_addfields 'PeripheralComponent','Make','System.String','General','',1,3,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("C3DB13C7-7CB4-42BB-BFDD-8A381F327653"), "exec db_addfields 'PeripheralComponent','Model','System.String','General','',1,4,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("1F04322B-CD8F-4FC8-A370-F234ED9F6777"), "exec db_addfields 'PeripheralComponent','ModelNumber','System.String','General','',1,5,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("93EACC02-75F2-46E8-9912-2319474BB882"), "exec db_addfields 'PeripheralComponent','SerialNumber','System.String','General','',1,6,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("25FA0E45-9E2D-48E4-8467-F574DBB86535"), "exec db_addfields 'PeripheralComponent','AssetNumber','System.String','General','',1,7,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("E1DA3764-0310-4928-8324-86BC63E48911"), "exec db_addfields 'PeripheralComponent','DatePurchased','System.String','General','',1,8,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("1558242D-B4B0-4388-BF4A-01D2E541F488"), "exec db_addfields 'PeripheralComponent','UnderWarranty','System.String','General','',1,9,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("4B10353A-2001-4ED9-86B6-4681DC3E532A"), "exec db_addfields 'PeripheralComponent','IsManaged','System.String','General','',1,10,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("C64EE8FA-DD9A-4562-A320-7EDED6C4E966"), "exec db_addfields 'PeripheralComponent','ContractNumber','System.String','General','',1,11,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("0E152767-158C-483F-894F-697791FB58BF"), "exec db_addfields 'PeripheralComponent','NetworkAddress1','System.String','General','',1,12,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("B423F4F6-25BE-487F-8BDC-3EDF08AE2E1B"), "exec db_addfields 'PeripheralComponent','NetworkAddress2','System.String','General','',1,13,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("90CD3E66-8044-4A5D-B72F-381E1E3DC3B5"), "exec db_addfields 'PeripheralComponent','NetworkAddress3','System.String','General','',1,14,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("FDB672B5-0F2E-4AEF-94F5-52AF16BA0F8E"), "exec db_addfields 'PeripheralComponent','NetworkAddress4','System.String','General','',1,15,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("88340389-C7E7-4E9A-A525-BA2FA1E8CF13"), "exec db_addfields 'PeripheralComponent','NetworkAddress5','System.String','General','',1,16,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("773614AA-777B-4D6D-8AC4-DD4D7AC69F6D"), "exec db_addfields 'PeripheralComponent','CopyPPM','System.String','General','',1,17,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("A167CB1F-8CCD-44A5-8076-361F11CBD14A"), "exec db_addfields 'PeripheralComponent','PrintPPM','System.String','General','',1,18,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("4CFA2C99-343D-4E38-9314-C1B4F4A89811"), "exec db_addfields 'PeripheralComponent','IsColor','System.String','General','',1,19,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("1AC640EE-A739-4891-ABBA-43CBEE69D2B6"), "exec db_addfields 'PeripheralComponent','IsNetwork','System.String','General','',1,20,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("7EAE8E11-40B2-4153-9177-D245EEDE5B39"), "exec db_addfields 'StorageComponent','PrimaryOrBackupType','PrimaryOrBackupType','General','',1,2,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("D3E63BA3-4930-4BF4-A5D5-CC7A76BF0ECE"), "exec db_addfields 'Measure','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',1,1,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("752D686B-09E9-43C2-AC9A-1B375FACAAB2"), "exec db_addfields 'Measure','Description','System.String','General','General description for each object',1,2,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("A5523EAA-C9ED-452B-B319-D5C4E96CDC9D"), "exec db_addfields 'Measure','Abbreviation','System.String','General','System.String',1,3,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("E8CDF2A5-532C-46CC-BFF1-FB4A9F2BCE15"), "exec db_addfields 'Measure','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',1,4,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("DD4A7AA7-4864-4270-8C69-32F46582B5EE"), "exec db_addfields 'Measure','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',1,5,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("51DC2420-282C-4DFA-9E6F-0627DB0CE0F1"), "exec db_addfields 'Measure','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',1,6,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("D7513258-F474-4686-AF8A-476ECC452DCB"), "exec db_addfields 'Measure','DesignRationale','System.String','General','System.String',1,7,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("6893E723-1728-4723-8361-C4BD4E1C24C3"), "exec db_addfields 'Measure','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',1,8,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("06739FB6-DC82-43A7-8E4E-66A169F612A8"), "exec db_addfields 'Measure','GapType','GapType','General','Result of a gap analysis between architecture states.',1,9,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("1DDB24E8-E901-40D6-A750-88BB4B44E936"), "exec db_addfields 'Measure','DataSourceID','System.String','General','Unique identified of for object as per external data source.',1,80,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("200C945D-53AA-4C2B-8787-B49AC542886E"), "exec db_addfields 'Measure','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',1,81,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("7A2B8908-2419-4BF7-B97A-0BB72027B103"), "exec db_addfields 'LogicalInformationArtefact','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',1,1,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("70FF5C9D-18C4-4DE3-B6DB-65DF19F024BF"), "exec db_addfields 'LogicalInformationArtefact','Description','System.String','General','General description for each object',1,2,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("63C80753-DE26-44E2-9801-41974B29C2BA"), "exec db_addfields 'LogicalInformationArtefact','Abbreviation','System.String','General','System.String',1,3,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("CB45519D-95FC-4715-BDF1-6D73D38AB1B3"), "exec db_addfields 'LogicalInformationArtefact','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',1,4,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("D964DA8A-C0F4-4439-8D8F-613B595CC095"), "exec db_addfields 'LogicalInformationArtefact','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',1,5,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("B250AAB2-4519-412F-88F3-CD29F137B123"), "exec db_addfields 'LogicalInformationArtefact','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',1,6,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("B19AB035-1F11-4890-9435-EF64E8568262"), "exec db_addfields 'LogicalInformationArtefact','DesignRationale','System.String','General','System.String',1,7,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("F3D3AFE3-C9F7-42D0-8303-01108EA344FD"), "exec db_addfields 'LogicalInformationArtefact','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',1,8,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("52C9ECBA-9F8E-447B-8E44-57B255D8006E"), "exec db_addfields 'LogicalInformationArtefact','GapType','GapType','General','Result of a gap analysis between architecture states.',1,9,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("C7B12D27-3114-415B-8F97-9B53475403C0"), "exec db_addfields 'LogicalInformationArtefact','DataSourceID','System.String','General','Unique identified of for object as per external data source.',1,80,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("A4C67FA6-F99A-4DCA-873C-A3F7E9BAA9BB"), "exec db_addfields 'LogicalInformationArtefact','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',1,81,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("964C32BE-5D81-447D-A892-47A756AA5E4B"), "exec db_addfields 'PhysicalInformationArtefact','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',1,1,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("AD5A9611-9490-4213-B807-A08C4D4A07B9"), "exec db_addfields 'PhysicalInformationArtefact','Description','System.String','General','General description for each object',1,2,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("92553144-DA09-48D2-A884-5F8897338535"), "exec db_addfields 'PhysicalInformationArtefact','Abbreviation','System.String','General','System.String',1,3,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("CA562F1D-9766-461E-952D-75A5E2F483D1"), "exec db_addfields 'PhysicalInformationArtefact','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',1,4,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("B831664E-608C-4118-B52B-F8BFEE440BE7"), "exec db_addfields 'PhysicalInformationArtefact','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',1,5,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("72D74D90-0E38-41DA-B62C-8BE9E8D8A104"), "exec db_addfields 'PhysicalInformationArtefact','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',1,6,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("A677BAB6-ECB6-4416-9AC6-E1B123EC6E4D"), "exec db_addfields 'PhysicalInformationArtefact','DesignRationale','System.String','General','System.String',1,7,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("EF132620-003A-4E9B-8CB0-E154CF9BF5AC"), "exec db_addfields 'PhysicalInformationArtefact','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',1,8,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("5691D857-990B-4735-8FD1-0418D1028D4B"), "exec db_addfields 'PhysicalInformationArtefact','GapType','GapType','General','Result of a gap analysis between architecture states.',1,9,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("0163075B-5834-4FB3-90E5-E96593AA0A34"), "exec db_addfields 'PhysicalInformationArtefact','DataSourceID','System.String','General','Unique identified of for object as per external data source.',1,80,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("ABD2746C-0379-44E7-AF58-8E3E3B1211C4"), "exec db_addfields 'PhysicalInformationArtefact','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',1,81,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("0A4FBE09-BBF8-44D4-B940-AC8E97F6100D"), "exec db_addfields 'ApplicationInterface','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',1,1,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("DDF68F30-49FE-46FA-8061-EA4FBDF23421"), "exec db_addfields 'ApplicationInterface','Description','System.String','General','General description for each object',1,2,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("13ACB391-FBC6-46B8-BF1D-6606C7E65684"), "exec db_addfields 'ApplicationInterface','Abbreviation','System.String','General','System.String',1,3,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("B90A4209-50F6-4A7B-B70D-D6E00B11BA9E"), "exec db_addfields 'ApplicationInterface','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',1,4,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("340CEBEF-6EF4-4577-912C-34CB44D9F208"), "exec db_addfields 'ApplicationInterface','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',1,5,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("DA0E4C9E-521C-4AFA-B786-8244E694D16D"), "exec db_addfields 'ApplicationInterface','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',1,6,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("3B52F97C-5F96-4B73-A2A0-AE0D58AB85BB"), "exec db_addfields 'ApplicationInterface','DesignRationale','System.String','General','System.String',1,7,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("FA876D9F-7BCF-4E71-99C2-F8AA010C05C5"), "exec db_addfields 'ApplicationInterface','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',1,8,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("89DCC54D-83E6-41BA-B2A3-E33C7975992D"), "exec db_addfields 'ApplicationInterface','GapType','GapType','General','Result of a gap analysis between architecture states.',1,9,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("B1CA0D7F-89E5-41F6-BE6B-56B5F388E5FA"), "exec db_addfields 'ApplicationInterface','DataSourceID','System.String','General','Unique identified of for object as per external data source.',1,80,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("379CCA46-3865-41D3-ABF9-2D7C4975F3B6"), "exec db_addfields 'ApplicationInterface','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',1,81,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("5819488B-B759-4A14-A4CD-34070BBB7117"), "exec db_addfields 'Network','Range','System.String','General','',1,6,0|AppInterfaceModel");
            databaseVersions.Add(new Guid("54FCC690-F253-4BC8-9611-79EC0DC454B2"), "exec db_addfields 'StorageComponent','IsPrimaryOrBackupType','System.String','General','',1,2,0|AppInterfaceModel");
            databaseVersions.Add(new Guid("C31B5386-24FA-49E5-B719-183E2003ECF4"), "EXEC db_addClassAssociations 'ApplicationInterface','PhysicalSoftwareComponent','DynamicFlow','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("75BA0CFF-B3C5-46ED-BFAB-48377BB673BB"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','ApplicationInterface','DynamicFlow','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("970D34BC-E703-4DEE-9BC4-A2018A7A404E"), "EXEC db_addClassAssociations 'ApplicationInterface','PhysicalDataComponent','DynamicFlow','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("41DBBECD-5A86-42BD-A2EF-FD6B72B84EEB"), "EXEC db_addClassAssociations 'PhysicalDataComponent','ApplicationInterface','DynamicFlow','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("4A966112-94F3-4C92-8441-4C7C6820F0AB"), "EXEC db_addClassAssociations 'ApplicationInterface','OrganizationalUnit','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("C76EC7A6-261F-431D-8A18-1AC695DDF4C3"), "EXEC db_addClassAssociations 'OrganizationalUnit','ApplicationInterface','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("CB57CCB3-F1D0-4E25-B789-9A14E7930E20"), "EXEC db_addClassAssociations 'ApplicationInterface','Employee','Mapping','',0,1|AppInterfaceModel");
            //databaseVersions.Add(new Guid("85875E7D-3C25-4E98-AB72-3E92066723EE"), "EXEC db_addClassAssociations 'Person','ApplicationInterface','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("8F985488-2B93-444C-8BB3-4DF99AFAE5B5"), "EXEC db_addClassAssociations 'Function','ApplicationInterface','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("C8A870B4-FE70-43AD-938C-36709A82F54D"), "EXEC db_addClassAssociations 'Process','ApplicationInterface','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("4A10C1A4-240B-4A26-A182-2F156AC00699"), "EXEC db_addClassAssociations 'Activity','ApplicationInterface','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("45D28162-4E1B-46C7-9828-A47502C40D10"), "EXEC db_addClassAssociations 'ApplicationInterface','Function','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("09AB4F8C-1017-4CD4-B94C-08DDEF00F4D2"), "EXEC db_addClassAssociations 'ApplicationInterface','Process','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("91E40540-6502-44AE-9DC3-4C05A2ACDC5A"), "EXEC db_addClassAssociations 'ApplicationInterface','Activity','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("241CFCBD-94AA-42F5-BB74-BE572CCE46C3"), "EXEC db_addClassAssociations 'ApplicationInterface','PhysicalSoftwareComponent','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("FF40ECDB-DEB2-44AB-8C05-85D915C8799A"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','ApplicationInterface','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("5F6EE486-E07F-4947-B6AF-B3BE149DA58E"), "EXEC db_addClassAssociations 'ApplicationInterface','PhysicalDataComponent','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("98FAAE6F-1240-4B38-B6CE-091ADF460FBC"), "EXEC db_addClassAssociations 'PhysicalDataComponent','ApplicationInterface','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("F5490520-9940-4E07-A393-16D2077F8506"), "EXEC db_addClassAssociations 'ApplicationInterface','ComputingComponent','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("14E2EA02-2BED-47F9-81FD-13386D9BDB44"), "EXEC db_addClassAssociations 'ComputingComponent','ApplicationInterface','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("8DEFB841-19E3-4CF8-962D-14281396BC12"), "EXEC db_addClassAssociations 'ApplicationInterface','Object','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("FD64DF81-378E-4FD5-BB34-C488FFF93682"), "EXEC db_addClassAssociations 'Object','ApplicationInterface','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("96680DAF-ADD9-4142-9CBF-ADA022AEA72A"), "EXEC db_addClassAssociations 'ApplicationInterface','GovernanceMechanism','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("CDFA28B4-CE22-420B-B4A0-D4F4A264532A"), "EXEC db_addClassAssociations 'GovernanceMechanism','ApplicationInterface','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("38B67665-8A0D-45D9-A218-85FC0CF50A50"), "EXEC db_addClassAssociations 'ApplicationInterface','Measure','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("92F47523-07F4-445A-A0F4-A050491C63BB"), "EXEC db_addClassAssociations 'Measure','ApplicationInterface','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("3157CF59-75F3-43E5-9A32-AC77BF8A82DD"), "EXEC db_addClassAssociations 'DataColumn','PhysicalInformationArtefact','Mapping','',1,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("BA83AD8C-2647-488D-AF67-BD5C0ACF2709"), "EXEC db_addClassAssociations 'Attribute','LogicalInformationArtefact','Mapping','',1,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("1B297E15-7234-4F74-A3C6-C73D787050C6"), "EXEC db_addArtifacts 'ApplicationInterface','PhysicalSoftwareComponent','DynamicFlow','PhysicalInformationArtefact',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("00E7EA52-F5BE-4ED6-AEAE-C79C23F5475F"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','ApplicationInterface','DynamicFlow','PhysicalInformationArtefact',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("042C31D3-9BB7-4B5E-96EE-20C40730977E"), "EXEC db_addArtifacts 'ApplicationInterface','PhysicalDataComponent','DynamicFlow','PhysicalInformationArtefact',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("5022E1CE-2BCB-439D-854A-208C4DE8EDF6"), "EXEC db_addArtifacts 'PhysicalDataComponent','ApplicationInterface','DynamicFlow','PhysicalInformationArtefact',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("BE709EFA-B9E3-4977-83CB-7FF8265E5E92"), "EXEC db_addArtifacts 'ApplicationInterface','OrganizationalUnit','Mapping','Role',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("8EFB2487-CDCA-427D-8EF4-508077EF73B6"), "EXEC db_addArtifacts 'OrganizationalUnit','ApplicationInterface','Mapping','Role',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("519C3CF5-EDA6-497A-B441-B942C3E5BEB0"), "EXEC db_addArtifacts 'ApplicationInterface','Employee','Mapping','Role',1|AppInterfaceModel");
            //databaseVersions.Add(new Guid("331A12FC-4ED5-4939-BF8F-DC2349C13E81"), "EXEC db_addArtifacts 'Person','ApplicationInterface','Mapping','Role',1|AppInterfaceModel");

            databaseVersions.Add(new Guid("FBE9CB4A-C305-4961-86A9-81977EA2B13B"), "EXEC db_AddPossibleValues 'DataType','Mng',12,'Management',0|AppInterfaceModel");
            databaseVersions.Add(new Guid("8F4EF826-696C-4A86-9D09-D1883F6A6C96"), "EXEC db_AddPossibleValues 'DataType','Trans',13,'Transaction',0|AppInterfaceModel");

            #region double Associations

            databaseVersions.Add(new Guid("E6863AF8-6A03-4EAE-A4A6-F7276637E001"), "EXEC db_addClassAssociations 'Activity','PhysicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("1B1ACD6E-8D44-4BA2-BC51-AFD9E0DAA8FF"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','Activity','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("F8489AD1-2007-4505-9283-7ACF3069333A"), "EXEC db_addClassAssociations 'Activity','PhysicalInformationArtefact','Maintain','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("052696DB-807A-472A-B216-AC4A2898392D"), "EXEC db_addClassAssociations 'DataColumn','PhysicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("664F6F67-65CF-4C1E-BFCC-BDE11FFB0A06"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','DataColumn','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("F0213CDD-0CEB-4BB7-8B31-750E684E1CEE"), "EXEC db_addClassAssociations 'DataSchema','PhysicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("6B411749-DE97-4502-828D-A9D232C9D6F4"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','DataSchema','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("62B76BDF-5C25-49E5-9C20-0182659805CF"), "EXEC db_addClassAssociations 'DataTable','PhysicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("6FA6663F-65FF-4318-8431-C19792EBD2A7"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','DataTable','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("428EA843-9B61-46AD-86EE-5AF609B3E6F1"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','Process','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("B52B0A72-AB9F-4525-BC14-B5EB665008BA"), "EXEC db_addClassAssociations 'Process','PhysicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("C304EE82-C02C-4CD5-8CF1-43B759F2CDA0"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','Activity','Read','',1,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("56830D6D-7C19-4149-88D3-2362584BF336"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','OrganizationalUnit','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("4FA272CC-4646-43F9-8AC9-711C3CDCEDDF"), "EXEC db_addClassAssociations 'OrganizationalUnit','PhysicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("6C9B0098-EEB0-47E0-943B-F56F7F88A1D3"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','Entity','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("D5ADD21A-8E3D-4952-8BD5-A9B51E3092B9"), "EXEC db_addClassAssociations 'Entity','PhysicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("0528E9F7-3ABD-4E56-AE7F-66E7BFEDBF85"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','PhysicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("8B26221D-2961-4AA7-9C3A-CBC78DE80C53"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','Object','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("0BDA77C7-1216-48B4-8CD2-0993FE26DBD6"), "EXEC db_addClassAssociations 'Object','PhysicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("00EC80A7-5A0F-4F39-8D2B-C4C7CC854681"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','Location','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("E9A1006D-2E42-42B0-B1C9-EEE4C5B08654"), "EXEC db_addClassAssociations 'Location','PhysicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("C7E9B6A6-D36A-42E0-A7B4-DEC87E083F4D"), "EXEC db_addClassAssociations 'Software','PhysicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("FA8E7C6C-16EB-43EC-AE41-938233575833"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','Software','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("C2E42F1D-3652-4576-A93F-4FB2E7E4D398"), "EXEC db_addClassAssociations 'StorageComponent','PhysicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("D4BF8EC2-8EE3-46BF-BC22-25088767BA0B"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','StorageComponent','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("1D52DE29-6C9E-4BF2-B501-DB150BF2E4A6"), "EXEC db_addClassAssociations 'NetworkComponent','PhysicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("FF48A714-9C1C-434B-9AC3-8AEEA4F0EE5F"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','NetworkComponent','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("2145F18E-0DEC-4C17-93C8-E3A94D2AA450"), "EXEC db_addClassAssociations 'SystemComponent','PhysicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("7791FF01-F489-4BCA-8CE4-EE4863B58808"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','SystemComponent','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("D3FE8D56-F293-47E6-85E9-9BF05DFC30F6"), "EXEC db_addClassAssociations 'Peripheral','PhysicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("E3434CFB-D16A-48EA-B2EA-4CB304EF4ABC"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','Peripheral','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("0E0F4840-1B8C-414A-8D51-88C1FDA15EA9"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','Process','Read','',1,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("83531313-4441-4FD4-BB73-1BC0C68FF4B7"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','Function','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("D97570AE-4ACF-4A80-8094-7A197BABBE11"), "EXEC db_addClassAssociations 'Function','PhysicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("0C30F211-E051-4B4A-AD11-6743EA80869E"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','Function','Read','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("A36E4D70-9CC3-4361-A5EF-E2BEC654A173"), "EXEC db_addClassAssociations 'Function','PhysicalInformationArtefact','Maintain','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("71C99374-23E2-4815-B451-F744E06102BD"), "EXEC db_addClassAssociations 'Process','PhysicalInformationArtefact','Delete','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("5CF67135-277A-4CC0-BFA3-4D7B5E479022"), "EXEC db_addClassAssociations 'Process','PhysicalInformationArtefact','Create','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("468CD8A3-0967-483D-B485-406DA8650301"), "EXEC db_addClassAssociations 'Process','PhysicalInformationArtefact','Update','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("2564B43E-A98D-46A7-9877-247E8693A787"), "EXEC db_addClassAssociations 'Process','PhysicalInformationArtefact','Maintain','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("58506BB8-3215-4B9E-A349-1BECF1FAB416"), "EXEC db_addClassAssociations 'Activity','PhysicalInformationArtefact','Create','',1,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("5C994E34-F9E3-40D2-9B45-008E97768587"), "EXEC db_addClassAssociations 'Activity','PhysicalInformationArtefact','Update','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("16739AA4-B427-486F-B014-713E28FF343A"), "EXEC db_addClassAssociations 'Activity','PhysicalInformationArtefact','Delete','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("7DF49963-E8FE-4DEA-8B85-371E0F7DD6EF"), "EXEC db_addClassAssociations 'Function','PhysicalInformationArtefact','Create','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("D9DFDC45-CAE0-4E2C-A314-CA078C994835"), "EXEC db_addClassAssociations 'Function','PhysicalInformationArtefact','Update','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("13781C58-DE0D-4F0B-B9B4-89EEB12D98C5"), "EXEC db_addClassAssociations 'Function','PhysicalInformationArtefact','Delete','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("D780FBCA-7DEA-47DA-8BF9-A77003ABD125"), "EXEC db_addClassAssociations 'Attribute','PhysicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("689B739E-FD71-4C0E-B326-7CA94C0708B1"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','Attribute','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("931DEC6E-8A2F-424E-990F-C816C7673908"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','PhysicalInformationArtefact','SubSetOf','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("09CC1CDA-B39A-439B-938D-E66F1518CF8A"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','PhysicalInformationArtefact','Decomposition','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("17EF3F19-3B2B-4933-B99D-878A8C6AE8E8"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','PhysicalInformationArtefact','Classification','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("55FCA61F-6408-417C-977C-10998A13DB45"), "EXEC db_addClassAssociations 'Job','PhysicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("DDC18E34-07C1-488D-98A7-A53B9126CCB0"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','Job','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("43244D48-C992-4FF1-B799-89FAD6D2F7CF"), "EXEC db_addClassAssociations 'JobPosition','PhysicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("AA036D8B-5734-41F6-AD2A-0514277D04F6"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','JobPosition','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("0A48D01A-D2A1-42ED-AC02-870917C5CACD"), "EXEC db_addClassAssociations 'GovernanceMechanism','PhysicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("4168F4CA-9F02-4819-B2FF-569E2E42A504"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','GovernanceMechanism','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("18F23520-9EA8-4BE0-A811-DB5E1870CDC4"), "EXEC db_addClassAssociations 'Rationale','PhysicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("63F73319-26C3-42A2-A5B4-C1174C52E844"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','Rationale','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("5CC2B379-34EE-48EA-824B-2B36DACB8E4D"), "EXEC db_addClassAssociations 'Employee','PhysicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("67610755-F83C-452A-807E-12B8E8B6C816"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','Employee','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("6E9CCB25-A1C0-4B0A-9C61-A77A222CADA0"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','Rationale','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("AF407738-397D-4356-8201-3CA9F1155D90"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','Rationale','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("2EF902BD-2E84-4EAA-8D71-41CEF7B0123A"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','PeripheralComponent','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("7F7C5027-611D-46E9-A577-748AC24492CD"), "EXEC db_addClassAssociations 'PeripheralComponent','PhysicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("F5AF76F3-8704-4133-8812-3C4D09D8FE71"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','ComputingComponent','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("61BFC6F7-EBC3-4E64-BCBB-BDE11EF3C598"), "EXEC db_addClassAssociations 'ComputingComponent','PhysicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("84CB9C2E-22EC-4CC0-A4A1-740911C1CE1A"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','PhysicalSoftwareComponent','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("7ABE1E60-4742-45F5-987B-30D1E84140CE"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','PhysicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("8B9912AD-40A3-45E5-BF4B-186F1ED3F698"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','PhysicalDataComponent','Mapping','',1,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("9559EFCD-F337-448A-A84B-62E40246DC9E"), "EXEC db_addClassAssociations 'PhysicalDataComponent','PhysicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");

            databaseVersions.Add(new Guid("E752B857-6807-4FEF-B486-64B90956A4A5"), "EXEC db_addClassAssociations 'Activity','LogicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("0CAFDD60-2F88-40D7-8339-C642887AB019"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','Activity','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("BEFDB75A-5916-4526-9908-14751F74CEF1"), "EXEC db_addClassAssociations 'Activity','LogicalInformationArtefact','Maintain','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("6E843820-234B-41F3-BB29-43F99C3F96E9"), "EXEC db_addClassAssociations 'DataColumn','LogicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("4DA42FDF-4E72-4DEE-BFA0-B2E7050F8697"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','DataColumn','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("B2081772-B1A0-4401-A2E3-164E2BDAD4E5"), "EXEC db_addClassAssociations 'DataSchema','LogicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("96B54983-A6A1-4D90-9028-1C4A6135555F"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','DataSchema','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("79034F2D-0DD0-4DC8-A669-A59BC7EFC241"), "EXEC db_addClassAssociations 'DataTable','LogicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("FACD3C32-951E-42D3-A4D4-102C9B222956"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','DataTable','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("0D6C9318-CD0B-4813-86B3-57B1CD465695"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','Process','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("6733A6F3-26C5-4592-A638-D7DAB39D2540"), "EXEC db_addClassAssociations 'Process','LogicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("CE35837F-14F1-4201-8768-5567AFBCC45D"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','Activity','Read','',1,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("0D37E175-D826-4FD2-9E03-A855A329FE4E"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','OrganizationalUnit','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("947AFF3A-89D9-4665-BE7E-8B603A841A3F"), "EXEC db_addClassAssociations 'OrganizationalUnit','LogicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("F5E4F90B-15C7-4279-A3DE-11C6EA523345"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','Entity','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("D13A7D81-AD26-4830-85A0-BDF83C7CC867"), "EXEC db_addClassAssociations 'Entity','LogicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("D3F3F173-F0A8-4E18-802B-8E76886F08E4"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','LogicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("3703C388-8C99-4CFB-9267-49E84D2F1FC0"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','Object','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("ABA2BCDE-9792-4AEB-9CC7-B3C5CBA820C0"), "EXEC db_addClassAssociations 'Object','LogicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("A1E74464-AB1F-4F5F-892C-2466862F8200"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','Location','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("681B94D2-9C88-4F35-A6DC-2C1206070BC5"), "EXEC db_addClassAssociations 'Location','LogicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("CCA8FC3D-CD05-4BFC-97DE-AF7480B16022"), "EXEC db_addClassAssociations 'Software','LogicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("D9B0A26D-A98C-48C2-BA6D-C8B7479684CE"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','Software','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("591E2BB3-6FDA-4670-B269-358FFC62C362"), "EXEC db_addClassAssociations 'StorageComponent','LogicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("693AA5FD-DA7C-43CD-ACFB-59113884A406"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','StorageComponent','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("21A751BF-AFAE-4E31-9D0A-D2E9DB4D8CFF"), "EXEC db_addClassAssociations 'NetworkComponent','LogicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("D2E2A50B-A883-42D4-A74F-06B22BA054FF"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','NetworkComponent','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("E7E7C7A5-735D-4A2D-B377-ED2458573148"), "EXEC db_addClassAssociations 'SystemComponent','LogicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("FEA19343-C603-4420-8109-0C44D07D2E9A"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','SystemComponent','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("3193F57A-9756-4DC0-BE36-62FE32F97395"), "EXEC db_addClassAssociations 'Peripheral','LogicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("48028781-DA75-4F08-BBB1-549FA175CF25"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','Peripheral','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("0AE66FEF-63BE-4B38-9F17-2DC7244BBF35"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','Process','Read','',1,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("96E96089-5472-46A8-AC62-CE4CA3CDC83E"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','Function','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("64A9A665-B2A2-4BED-A893-8D73B530A7B1"), "EXEC db_addClassAssociations 'Function','LogicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("115888B8-80DC-4C45-9C47-7258CC934E8A"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','Function','Read','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("3B77CCF0-9417-4455-9AAB-38EB0C63325F"), "EXEC db_addClassAssociations 'Function','LogicalInformationArtefact','Maintain','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("C99169AC-9F70-4ABE-B895-B7E1A9A33453"), "EXEC db_addClassAssociations 'Process','LogicalInformationArtefact','Delete','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("11061FB8-3C57-439B-9178-7068D22C882C"), "EXEC db_addClassAssociations 'Process','LogicalInformationArtefact','Create','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("EF838312-024E-4A85-9AC2-5720C9E7CE68"), "EXEC db_addClassAssociations 'Process','LogicalInformationArtefact','Update','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("83E44950-F5BA-4133-BB51-E71E1344ECCA"), "EXEC db_addClassAssociations 'Process','LogicalInformationArtefact','Maintain','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("E42BF049-32AF-407C-B990-3C068C060FAE"), "EXEC db_addClassAssociations 'Activity','LogicalInformationArtefact','Create','',1,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("7562E421-3EC2-4A37-BFD4-2DF27B66CBFB"), "EXEC db_addClassAssociations 'Activity','LogicalInformationArtefact','Update','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("D67421BA-9205-4CE5-8DBD-81BA2AD33AEF"), "EXEC db_addClassAssociations 'Activity','LogicalInformationArtefact','Delete','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("EB55E07E-FC78-41DC-A270-9651E4CFC60B"), "EXEC db_addClassAssociations 'Function','LogicalInformationArtefact','Create','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("14B8DFAF-3E90-4885-AD84-9E4BB60314D1"), "EXEC db_addClassAssociations 'Function','LogicalInformationArtefact','Update','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("132632E3-268E-452C-B018-C62478D9955D"), "EXEC db_addClassAssociations 'Function','LogicalInformationArtefact','Delete','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("F5819C01-D04B-47DC-AC72-6DCA7986C0B1"), "EXEC db_addClassAssociations 'Attribute','LogicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("67679766-A986-4EF8-9CA5-BBE1A68C1AFB"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','Attribute','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("9F82E3EF-EB16-4A19-8946-0E6AB3812C1F"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','LogicalInformationArtefact','SubSetOf','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("69BAE465-990F-4E4A-BF05-AE7841F97723"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','LogicalInformationArtefact','Decomposition','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("75725B4A-53D3-4438-AFF6-063569B04966"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','LogicalInformationArtefact','Classification','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("6A54E38B-EC18-41F1-8787-8A5266B2097A"), "EXEC db_addClassAssociations 'Job','LogicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("DBCB5517-12AB-44A6-A644-CAA6441A828A"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','Job','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("1B2FDEA8-2CF4-454D-BD8D-E9B8160314C9"), "EXEC db_addClassAssociations 'JobPosition','LogicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("4F4F3EE3-4EEC-44B5-AFF4-1592E6C389EF"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','JobPosition','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("37A32C0A-1893-45FC-9B8B-7FD09E0AD502"), "EXEC db_addClassAssociations 'GovernanceMechanism','LogicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("E2AAC945-D7B9-4487-B589-1B207F4AE71A"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','GovernanceMechanism','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("E1C49969-72AD-43C2-9847-DDE86ABD87A1"), "EXEC db_addClassAssociations 'Rationale','LogicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("4B09A555-D653-48D0-9C41-AFD3BC3AF0E9"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','Rationale','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("6F2AFD29-4DBF-401A-950E-A08240EDEF37"), "EXEC db_addClassAssociations 'Employee','LogicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("4703DAAC-6F48-4709-B0A3-65E6467FD8A8"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','Employee','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("31CCBDA3-81DA-47A0-B102-2B7CF91505A8"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','Rationale','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("5A6F0190-12BC-4972-BB76-FBA0630BD059"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','Rationale','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("AF687528-746C-4AE4-9FA4-74CF7DE76720"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','PeripheralComponent','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("89F3C161-213E-4592-8441-883F7308810D"), "EXEC db_addClassAssociations 'PeripheralComponent','LogicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("9E340418-7CD1-4E26-B8C9-20D56BAA10C9"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','ComputingComponent','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("C7919C09-47FE-4A6D-95E8-03CB266EA6DD"), "EXEC db_addClassAssociations 'ComputingComponent','LogicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("7F84A803-9343-41A3-BFDE-17DF11DF577B"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','PhysicalSoftwareComponent','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("4C93D8B2-AC20-4230-B8D8-3F87CCD82DD1"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','LogicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("C7821DFB-61F7-4981-AF63-099955AA4251"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','PhysicalDataComponent','Mapping','',1,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("EE5B4229-8E17-4B75-812C-1BAA6043B53C"), "EXEC db_addClassAssociations 'PhysicalDataComponent','LogicalInformationArtefact','Mapping','',0,1|AppInterfaceModel");

            databaseVersions.Add(new Guid("AB9EEDB0-E948-46D9-8001-C47070FED5E3"), "EXEC db_addArtifacts 'Activity','PhysicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("49EF2299-1751-4807-961D-382341DB8ED2"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','Activity','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("C6EBED55-C719-4260-9255-E806E900B4C3"), "EXEC db_addArtifacts 'Activity','PhysicalInformationArtefact','Maintain','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("CFCF66C5-19F7-4401-AF5C-2CB627FCF3CE"), "EXEC db_addArtifacts 'Activity','PhysicalInformationArtefact','Maintain','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("BC58EACB-CBAD-4322-84A0-65EB767E789F"), "EXEC db_addArtifacts 'DataColumn','PhysicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("74CF918C-3A0E-4263-B4EE-7DFB2443954E"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','DataColumn','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("0EBB0361-48F1-41E2-B140-8FC5F83CFC3A"), "EXEC db_addArtifacts 'DataSchema','PhysicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("284C3E54-876D-41DF-901C-FB1DB5B51B7B"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','DataSchema','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("8AE11485-9586-43F1-8176-22C58840B7EC"), "EXEC db_addArtifacts 'DataTable','PhysicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("2D4CDD28-0DD4-42F6-A59A-54753A55E398"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','DataTable','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("43CDB8F9-56E0-4386-8FF1-F2E64CD9BCDA"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','Process','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("42B7574E-ED33-41A0-B8E5-8885497CC362"), "EXEC db_addArtifacts 'Process','PhysicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("F5AAF798-43BE-4740-9F84-0D0C6DF6E38E"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','Activity','Read','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("A793A399-E1F7-465D-AC3F-809A4F69E72A"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','Activity','Read','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("7658F117-D2BC-4F42-9617-4B9002B410FE"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','OrganizationalUnit','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("7DEF8A93-3441-447B-8326-1388D3AB9BC5"), "EXEC db_addArtifacts 'OrganizationalUnit','PhysicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("3B25581F-B0B7-4427-85CF-0EB06FF660EC"), "EXEC db_addArtifacts 'OrganizationalUnit','PhysicalInformationArtefact','Mapping','Role',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("E5E9492A-A3AD-430A-934B-271C3E23B56A"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','Entity','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("00E08258-2C27-440F-ABD6-31816727B0E5"), "EXEC db_addArtifacts 'Entity','PhysicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("BD817B22-7B92-4CB4-B50D-6C26827C9B25"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','PhysicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("1E8647E4-DB44-4076-9F02-915640C5A5E8"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','Object','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("EB93BD75-AA65-45C5-AA62-4DA8616FAA89"), "EXEC db_addArtifacts 'Object','PhysicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("8087C1F6-984E-4ED3-8D70-16F4B8867589"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','Location','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("79EFF1E6-0F8F-4185-8549-FB88075F7A3F"), "EXEC db_addArtifacts 'Location','PhysicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("7E088AA8-BFCC-48E2-9F86-8897051C83C5"), "EXEC db_addArtifacts 'Software','PhysicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("93B2EA51-1F81-411E-A4AD-B1680D8E9490"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','Software','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("A978A2B9-1A04-48B2-B26C-69AE16F36295"), "EXEC db_addArtifacts 'StorageComponent','PhysicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("31117280-65EA-4FED-9B85-7C85308D4036"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','StorageComponent','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("7DEE716E-E501-48C5-AD6E-00D2191CF20C"), "EXEC db_addArtifacts 'NetworkComponent','PhysicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("22307CFF-17F7-41EA-82FB-7E5A958ACA75"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','NetworkComponent','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("8461ECC8-66A2-46C0-8AF4-6D68A7172E19"), "EXEC db_addArtifacts 'SystemComponent','PhysicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("5DB16E48-2362-4AC8-9F83-9921F9ACD0E6"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','SystemComponent','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("33F7C3D0-2E3B-4393-8F86-37B1A0986480"), "EXEC db_addArtifacts 'Peripheral','PhysicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("897F9046-559D-477E-B6FA-D21D0C68BF39"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','Peripheral','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("A678E329-8B10-4885-956E-CAC9AEFD0078"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','Process','Read','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("9B08F914-3BD5-4F40-A976-DAEF176B555F"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','Process','Read','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("F4CB39E8-9E66-43A4-AAB9-9ACED26208B8"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','Function','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("D981958B-1010-4F36-B4BA-8843EB59E962"), "EXEC db_addArtifacts 'Function','PhysicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("0ADB1324-B924-4B37-A78D-565DB28F1F02"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','Function','Read','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("179B2370-9329-4B6E-9547-91AB4C258ADF"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','Function','Read','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("F0C49BF8-B2A1-420E-A749-BB3D16467AB7"), "EXEC db_addArtifacts 'Function','PhysicalInformationArtefact','Maintain','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("0D8EF1CE-1F70-4727-A8E8-4AA02D0ED378"), "EXEC db_addArtifacts 'Function','PhysicalInformationArtefact','Maintain','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("93A0CCF9-6C27-4557-BEC0-63F3185A44B6"), "EXEC db_addArtifacts 'Process','PhysicalInformationArtefact','Delete','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("CB91071F-AC43-4179-98B9-4E51804B61DA"), "EXEC db_addArtifacts 'Process','PhysicalInformationArtefact','Delete','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("95619EA2-97B3-4AF0-B715-3DF09B799521"), "EXEC db_addArtifacts 'Process','PhysicalInformationArtefact','Create','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("1C50C5DE-F93A-4042-9C65-43288C691F63"), "EXEC db_addArtifacts 'Process','PhysicalInformationArtefact','Create','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("05F2D040-FAD3-41A8-9055-A7019E97B438"), "EXEC db_addArtifacts 'Process','PhysicalInformationArtefact','Update','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("212931D2-6308-430A-AF5D-2B07DC7B53BA"), "EXEC db_addArtifacts 'Process','PhysicalInformationArtefact','Update','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("F3F1F5D4-4EE0-4A53-A4DC-18EACD3E2906"), "EXEC db_addArtifacts 'Process','PhysicalInformationArtefact','Maintain','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("D4227568-E672-459E-B8CB-CFE5AF2C0A35"), "EXEC db_addArtifacts 'Process','PhysicalInformationArtefact','Maintain','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("1146DDB4-919C-47DF-9B6F-01B94D7979B1"), "EXEC db_addArtifacts 'Activity','PhysicalInformationArtefact','Create','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("9A8E2C59-8E41-437E-973F-988BF0F45623"), "EXEC db_addArtifacts 'Activity','PhysicalInformationArtefact','Create','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("BFE64709-97F7-4C03-A27B-C27769A3C426"), "EXEC db_addArtifacts 'Activity','PhysicalInformationArtefact','Update','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("471A71EA-E9E9-4E03-9402-F2E82AF820A7"), "EXEC db_addArtifacts 'Activity','PhysicalInformationArtefact','Update','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("CA794715-723B-45CE-B290-F4326166E64D"), "EXEC db_addArtifacts 'Activity','PhysicalInformationArtefact','Delete','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("36F59BD7-4FD5-4EA4-9C58-08A0CA2826B4"), "EXEC db_addArtifacts 'Activity','PhysicalInformationArtefact','Delete','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("44A10A35-C76A-4618-813C-8DA9E61D5C36"), "EXEC db_addArtifacts 'Function','PhysicalInformationArtefact','Create','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("DF6B7C4D-7873-4AAA-8204-E746D29AFF93"), "EXEC db_addArtifacts 'Function','PhysicalInformationArtefact','Create','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("BAD80A1A-4C31-4705-AB1C-01189E4714D7"), "EXEC db_addArtifacts 'Function','PhysicalInformationArtefact','Update','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("E7238548-0533-403F-8B9D-2F4DE85EF1E5"), "EXEC db_addArtifacts 'Function','PhysicalInformationArtefact','Update','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("F5F01371-ADF8-458F-8BB0-9C21896DD055"), "EXEC db_addArtifacts 'Function','PhysicalInformationArtefact','Delete','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("36E4B102-A117-4F26-94F2-EA3AAD2AC6E8"), "EXEC db_addArtifacts 'Function','PhysicalInformationArtefact','Delete','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("4F114DC5-3F18-4EE8-9E99-9B5F254E429B"), "EXEC db_addArtifacts 'Attribute','PhysicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("577AEE4C-E1FD-43A1-A0C0-653301362CF6"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','Attribute','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("F64F28E0-F680-42F0-A4E8-629E1CBFB3C8"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','PhysicalInformationArtefact','SubSetOf','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("013B8C8F-D2EF-4077-9B19-751DE1949295"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','PhysicalInformationArtefact','Decomposition','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("8816C2C2-622C-4D04-939F-3C6817B20B77"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','PhysicalInformationArtefact','Classification','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("9C23D18F-A7E8-410F-A710-E08ECA994D43"), "EXEC db_addArtifacts 'Job','PhysicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("474B0EC9-E286-4562-876A-FCFB22722EAB"), "EXEC db_addArtifacts 'Job','PhysicalInformationArtefact','Mapping','Role',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("961BDCD2-5C30-4B27-B225-A5789F01B184"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','Job','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("0DE7D612-EAC3-4806-BF68-D58958D8CD3A"), "EXEC db_addArtifacts 'JobPosition','PhysicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("144A6DB4-0B42-4F8F-BCC8-364561FE8546"), "EXEC db_addArtifacts 'JobPosition','PhysicalInformationArtefact','Mapping','Role',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("E0980BDA-2A77-4BCE-8DEA-883E8C9C8C32"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','JobPosition','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("65A09EC8-4997-4122-AFE1-E4000A38572A"), "EXEC db_addArtifacts 'GovernanceMechanism','PhysicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("B5E11FC7-E67A-464C-AB87-419187A928F3"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','GovernanceMechanism','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("286ECDB1-E35D-4B60-AB4B-942B6AD917D9"), "EXEC db_addArtifacts 'Rationale','PhysicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("5A78579C-0F70-424F-9242-B8D07E1F715F"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','Rationale','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("FD02672A-DB1E-4378-8F66-66C8326E613F"), "EXEC db_addArtifacts 'Employee','PhysicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("BD5FE080-16C4-42D3-844E-8E1A696DC4ED"), "EXEC db_addArtifacts 'Employee','PhysicalInformationArtefact','Mapping','Role',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("FFA7B55E-1EC9-4E78-BBBB-9B79580AD983"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','Employee','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("81CD3B92-A352-42FC-9411-BDF950F06F9D"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','Rationale','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("A43F8B09-979A-495D-A068-CB8F61F90578"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','PeripheralComponent','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("19B0E6FE-6442-4A9F-94B9-635E3B696C58"), "EXEC db_addArtifacts 'PeripheralComponent','PhysicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("78D2540C-9A00-4EF0-8A01-40268D824B85"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','ComputingComponent','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("B0C3688D-2036-4BEE-B130-08F3B1DC7876"), "EXEC db_addArtifacts 'ComputingComponent','PhysicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("95EA1AAE-0746-40D3-AE63-68653E27C3C9"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','PhysicalSoftwareComponent','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("B6614A70-4706-4E8B-8609-E97B541B362C"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','PhysicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");

            databaseVersions.Add(new Guid("BF63E5B7-AB1C-487E-8C0C-E15461124B37"), "EXEC db_addArtifacts 'Activity','LogicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("23323E3C-150E-43AA-A5BC-DA3BCE74A9A5"), "EXEC db_addArtifacts 'LogicalInformationArtefact','Activity','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("C20B4B88-C172-4145-9AB9-B0DE1087097D"), "EXEC db_addArtifacts 'Activity','LogicalInformationArtefact','Maintain','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("FAA9CB4A-0BD5-4B04-AE3C-C663CA5B5495"), "EXEC db_addArtifacts 'Activity','LogicalInformationArtefact','Maintain','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("9EA3FC09-3D4A-4E16-B772-3C4B07B713EC"), "EXEC db_addArtifacts 'DataColumn','LogicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("BD49C9FA-C5A1-4F7B-88A6-E08E5772C5D3"), "EXEC db_addArtifacts 'LogicalInformationArtefact','DataColumn','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("C8E524B2-6A53-4F64-9DA5-41B6C453E74E"), "EXEC db_addArtifacts 'DataSchema','LogicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("66DA6B50-5060-46E3-855B-97803DBF35F9"), "EXEC db_addArtifacts 'LogicalInformationArtefact','DataSchema','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("2D06E933-7209-4206-AC3A-E3D55882B1F5"), "EXEC db_addArtifacts 'DataTable','LogicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("236A6858-9549-4F2C-8EA5-B667407C98CE"), "EXEC db_addArtifacts 'LogicalInformationArtefact','DataTable','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("BBB35731-40AD-4B7C-8FF1-85518FA06CE8"), "EXEC db_addArtifacts 'LogicalInformationArtefact','Process','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("93225933-63CA-4214-B5D4-FF4DBB1FBFC5"), "EXEC db_addArtifacts 'Process','LogicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("37B43B3D-8BEA-4D8C-B7E5-0F7CCE43572D"), "EXEC db_addArtifacts 'LogicalInformationArtefact','Activity','Read','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("CC4087FA-7804-4431-BE02-5FC5213AF3B4"), "EXEC db_addArtifacts 'LogicalInformationArtefact','Activity','Read','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("3ECC5E75-1A28-4D8A-BDC9-E1B579601AE1"), "EXEC db_addArtifacts 'LogicalInformationArtefact','OrganizationalUnit','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("A0E04415-58A9-459A-8CE7-4794FC69253E"), "EXEC db_addArtifacts 'OrganizationalUnit','LogicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("8447363E-F23A-40E6-AA70-59A99D498240"), "EXEC db_addArtifacts 'OrganizationalUnit','LogicalInformationArtefact','Mapping','Role',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("72E7F4CE-AE44-41AA-B02C-951866D29609"), "EXEC db_addArtifacts 'LogicalInformationArtefact','Entity','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("FE32B19C-3FA2-49F0-BC82-46C8FB8F21B1"), "EXEC db_addArtifacts 'Entity','LogicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("96CCA3AD-794F-4C87-AE05-A3E4B98078A7"), "EXEC db_addArtifacts 'LogicalInformationArtefact','LogicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("4162C82E-1645-48F5-86DA-9DB3A0661168"), "EXEC db_addArtifacts 'LogicalInformationArtefact','Object','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("65602B59-1F9D-40AB-98FC-2C543350E625"), "EXEC db_addArtifacts 'Object','LogicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("0CF6B7FC-5702-4751-A70C-201132431178"), "EXEC db_addArtifacts 'LogicalInformationArtefact','Location','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("6FBE9EAA-B94B-440F-9969-62AE8E5BA3C1"), "EXEC db_addArtifacts 'Location','LogicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("245831C5-162C-4E28-8CE7-5439DA7A73BE"), "EXEC db_addArtifacts 'Software','LogicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("08A90E57-4691-41DB-ABE1-28D77B539D52"), "EXEC db_addArtifacts 'LogicalInformationArtefact','Software','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("08693E7A-18DB-4CE7-AE83-D73745174A9A"), "EXEC db_addArtifacts 'StorageComponent','LogicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("2CD9BD19-27B2-4049-9BFD-00233EBBC1B6"), "EXEC db_addArtifacts 'LogicalInformationArtefact','StorageComponent','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("1E4AAFA7-EDE4-4672-BA29-253D837B15AA"), "EXEC db_addArtifacts 'NetworkComponent','LogicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("E549A828-4C08-48A4-ACB3-B308EB4D2172"), "EXEC db_addArtifacts 'LogicalInformationArtefact','NetworkComponent','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("4FDA933B-53D3-4800-8833-73C21A49FB7C"), "EXEC db_addArtifacts 'SystemComponent','LogicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("3D32FFC5-1A44-49B7-AE3F-5432D3645334"), "EXEC db_addArtifacts 'LogicalInformationArtefact','SystemComponent','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("2F4EAC58-6BF3-4ECC-88F0-FD1284852606"), "EXEC db_addArtifacts 'Peripheral','LogicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("B3D039C7-9200-4167-9608-5E945EAFFF84"), "EXEC db_addArtifacts 'LogicalInformationArtefact','Peripheral','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("FFA038E5-3DB4-42D7-BC71-4DA1AB355AD1"), "EXEC db_addArtifacts 'LogicalInformationArtefact','Process','Read','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("EA016B39-E97D-415C-97F6-DA935AD5D37C"), "EXEC db_addArtifacts 'LogicalInformationArtefact','Process','Read','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("ED9C85DF-1CF4-41E3-BFA0-39CB541245A8"), "EXEC db_addArtifacts 'LogicalInformationArtefact','Function','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("6247185A-D8A4-4512-B7DC-121FA5E4EE87"), "EXEC db_addArtifacts 'Function','LogicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("A794BC9F-F1EC-49B4-BC43-E5D257C939E9"), "EXEC db_addArtifacts 'LogicalInformationArtefact','Function','Read','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("9A8649D4-A40D-4CE3-9E93-415BBFA17890"), "EXEC db_addArtifacts 'LogicalInformationArtefact','Function','Read','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("87A021CA-CC4C-4DF6-A3CE-44D9EBFDD0A0"), "EXEC db_addArtifacts 'Function','LogicalInformationArtefact','Maintain','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("9B6D40EE-670E-448B-BC30-65C757B37ACF"), "EXEC db_addArtifacts 'Function','LogicalInformationArtefact','Maintain','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("16BB0DF4-1A12-42D9-9240-8A824938E07B"), "EXEC db_addArtifacts 'Process','LogicalInformationArtefact','Delete','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("8CA08287-1BB1-4718-BF11-8ED6DCE7D6DD"), "EXEC db_addArtifacts 'Process','LogicalInformationArtefact','Delete','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("0B38DE0C-A827-4679-A2BE-C347CB808E5B"), "EXEC db_addArtifacts 'Process','LogicalInformationArtefact','Create','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("771502FA-392C-432C-96CA-659A85DCF40D"), "EXEC db_addArtifacts 'Process','LogicalInformationArtefact','Create','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("4A3D3FD4-16DC-41BD-8533-8AFDE39731BA"), "EXEC db_addArtifacts 'Process','LogicalInformationArtefact','Update','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("E29EDD26-D3B9-45D5-8F71-827CBD69E805"), "EXEC db_addArtifacts 'Process','LogicalInformationArtefact','Update','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("0961EF00-ADD2-40CA-A30E-46EF4F8949DB"), "EXEC db_addArtifacts 'Process','LogicalInformationArtefact','Maintain','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("15644E3D-6DF5-4FC0-AAF6-E8662F97C7B5"), "EXEC db_addArtifacts 'Process','LogicalInformationArtefact','Maintain','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("4B1A20DD-FC1A-4A46-AD27-EFF254880E17"), "EXEC db_addArtifacts 'Activity','LogicalInformationArtefact','Create','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("D2C153ED-1BCC-4979-B070-D92DFC55D7C2"), "EXEC db_addArtifacts 'Activity','LogicalInformationArtefact','Create','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("5E0311FB-756C-4F9D-BA4C-1FA804BFB9A7"), "EXEC db_addArtifacts 'Activity','LogicalInformationArtefact','Update','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("9FB4E637-24B1-4773-A7CC-8C2623CFDDDB"), "EXEC db_addArtifacts 'Activity','LogicalInformationArtefact','Update','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("D14575EF-7D7A-4CB6-A7BD-98EB63447766"), "EXEC db_addArtifacts 'Activity','LogicalInformationArtefact','Delete','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("BD46B578-1CC4-465A-AF8E-83FEF62CE2EC"), "EXEC db_addArtifacts 'Activity','LogicalInformationArtefact','Delete','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("30E362CB-66BC-4AAD-A413-21808FCE5C73"), "EXEC db_addArtifacts 'Function','LogicalInformationArtefact','Create','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("B4D8D357-5020-426B-BD4C-AC8AFD47C7DB"), "EXEC db_addArtifacts 'Function','LogicalInformationArtefact','Create','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("7D19C3C2-ED64-4C1F-8689-D049C49C0131"), "EXEC db_addArtifacts 'Function','LogicalInformationArtefact','Update','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("406AA39E-D5AC-454C-BBBF-B2FAA526338C"), "EXEC db_addArtifacts 'Function','LogicalInformationArtefact','Update','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("6C659611-2953-4605-8A83-7B4ED3EAB337"), "EXEC db_addArtifacts 'Function','LogicalInformationArtefact','Delete','FlowDescription',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("8099224C-B737-4F4C-8275-9161CC0087B2"), "EXEC db_addArtifacts 'Function','LogicalInformationArtefact','Delete','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("C37FA92E-FCF9-4AE6-81FD-245E8C59D729"), "EXEC db_addArtifacts 'Attribute','LogicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("62229F36-A1BC-40A2-8C5F-677B87327A88"), "EXEC db_addArtifacts 'LogicalInformationArtefact','Attribute','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("D897DBF2-5E29-436E-85A9-001A4E0F97DC"), "EXEC db_addArtifacts 'LogicalInformationArtefact','LogicalInformationArtefact','SubSetOf','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("A5DACA6C-12E6-40F7-A735-64A6C80E024E"), "EXEC db_addArtifacts 'LogicalInformationArtefact','LogicalInformationArtefact','Decomposition','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("78971FCB-7600-4AA9-AC16-D7C1FFD6430E"), "EXEC db_addArtifacts 'LogicalInformationArtefact','LogicalInformationArtefact','Classification','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("6637FFC1-678C-41B8-8ACC-56BD19509E41"), "EXEC db_addArtifacts 'Job','LogicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("59844954-39E0-4CE4-8E69-C8E0CA4BF6F0"), "EXEC db_addArtifacts 'Job','LogicalInformationArtefact','Mapping','Role',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("3E6CCEA8-4237-48ED-A5FE-96C77EE60826"), "EXEC db_addArtifacts 'LogicalInformationArtefact','Job','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("9EB86023-4013-4CC4-871F-86A1299C8CFF"), "EXEC db_addArtifacts 'JobPosition','LogicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("FFC02B97-1518-40BB-8666-F53BEC65CD34"), "EXEC db_addArtifacts 'JobPosition','LogicalInformationArtefact','Mapping','Role',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("F8201D53-8565-44C3-9384-CB4C103AA364"), "EXEC db_addArtifacts 'LogicalInformationArtefact','JobPosition','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("134E5B2A-D04E-4357-9370-A4A82440DF0E"), "EXEC db_addArtifacts 'GovernanceMechanism','LogicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("F735C0B5-92C0-48B1-842E-F6E00262ED18"), "EXEC db_addArtifacts 'LogicalInformationArtefact','GovernanceMechanism','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("3FD450AC-7D21-4EAA-AB2B-5AEC3157B5F0"), "EXEC db_addArtifacts 'Rationale','LogicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("F2556466-23E0-4922-81A8-BEA7F6548006"), "EXEC db_addArtifacts 'LogicalInformationArtefact','Rationale','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("819708E4-944C-44CD-A0F0-B079140235B0"), "EXEC db_addArtifacts 'Employee','LogicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("D277BA12-7717-4954-95C0-1AFA491331D6"), "EXEC db_addArtifacts 'Employee','LogicalInformationArtefact','Mapping','Role',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("7D05A0E6-2C1E-4180-BEE0-EEC21EAE32D9"), "EXEC db_addArtifacts 'LogicalInformationArtefact','Employee','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("55476262-A83D-44F6-B31E-8A5AFFF4889C"), "EXEC db_addArtifacts 'LogicalInformationArtefact','Rationale','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("496C48DE-F604-43C5-9911-59F91363443B"), "EXEC db_addArtifacts 'LogicalInformationArtefact','PeripheralComponent','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("05DEA781-AEF6-46C9-86CC-3D66A3AB0F61"), "EXEC db_addArtifacts 'PeripheralComponent','LogicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("06E7DBC8-537E-4B95-A7FA-1E7A2CF90790"), "EXEC db_addArtifacts 'LogicalInformationArtefact','ComputingComponent','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("C3A4184F-B5AA-4F85-BE1C-9A0EBC4A2912"), "EXEC db_addArtifacts 'ComputingComponent','LogicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("D9BDAAD4-0F13-458E-997A-3E633252B80C"), "EXEC db_addArtifacts 'LogicalInformationArtefact','PhysicalSoftwareComponent','Mapping','Rationale',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("11E24747-8641-46BD-A103-4B1187A1986A"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','LogicalInformationArtefact','Mapping','Rationale',1|AppInterfaceModel");

            #endregion

            databaseVersions.Add(new Guid("C4785F12-A502-4D19-B042-9C66B71A791F"), "EXEC db_AddPossibleValues 'Criticality','High',1,'High',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("BDE96C69-BCE5-4ADA-844A-600E371FBAAF"), "EXEC db_AddPossibleValues 'Criticality','Medium',2,'Medium',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("E9715469-D4CE-4690-87D7-A55805B025F6"), "EXEC db_AddPossibleValues 'Criticality','Low',3,'Low',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("FF470898-8462-45C9-AC0D-70F0490A257A"), "EXEC db_AddPossibleValues 'FormatStandard','CSV',5,'CSV',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("5477DC35-476B-4325-B485-554F7AF1CDB3"), "EXEC db_AddPossibleValues 'InterfaceProtocol','SSL',1,'SSL',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("2239F368-C286-47CC-ADE2-2047ADD0A059"), "EXEC db_AddPossibleValues 'InterfaceProtocol','HTTPS',2,'HTTPS',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("8B1A492D-40C2-436D-B59B-248A45CD1B99"), "EXEC db_AddPossibleValues 'InterfaceProtocol','FTPS',3,'FTPS',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("AAD318B6-6E2D-4BD6-910D-3EA198ADD51E"), "EXEC db_AddPossibleValues 'InterfaceProtocol','FTP',4,'FTP',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("3C723A78-7F0C-4C80-A2BE-769A75B54073"), "EXEC db_AddPossibleValues 'InterfaceProtocol','HTTP',5,'HTTP',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("E78E3CCC-D282-41DD-9643-ED665754DDEE"), "EXEC db_AddPossibleValues 'InterfaceProtocol','ALE',6,'ALE',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("9CC5D63D-9B0A-4F44-A4E6-EF947D692C4C"), "EXEC db_AddPossibleValues 'InterfaceProtocol','JMS',7,'JMS',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("CD7F4CB4-82EF-47D9-8556-1C03F4D83DB0"), "EXEC db_AddPossibleValues 'InterfaceProtocol','NTP',8,'NTP',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("C5D04E6E-07CA-46FC-9DBA-27B6CF901563"), "EXEC db_AddPossibleValues 'InterfaceProtocol','SOAP',9,'SOAP',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("44B07A4C-5951-4BD0-ACE7-7E5C4FCAA829"), "EXEC db_AddPossibleValues 'InterfaceProtocol','SSH',10,'SSH',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("25C71C5F-7A52-4519-89BB-D99AA47819D7"), "EXEC db_AddPossibleValues 'InterfaceProtocol','Queue',12,'Queue',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("8A2B51BD-DAE4-448F-9772-BAA2F4A75D7D"), "EXEC db_AddPossibleValues 'InterfacePattern','DatabaseScript',1,'DatabaseScript',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("A03F7F84-6771-40B1-986C-38894112C4CB"), "EXEC db_AddPossibleValues 'InterfacePattern','FileTransfer',2,'FileTransfer',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("B9628816-1A20-46EB-9BD3-3B574C2EC1BF"), "EXEC db_AddPossibleValues 'InterfacePattern','Event',3,'Event',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("2C0C7B26-6192-4D1D-B73B-E6860FD27410"), "EXEC db_AddPossibleValues 'InterfacePattern','BIETL',4,'BIETL',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("A7B0C275-2285-4529-BC3F-7CE13F3EB734"), "EXEC db_AddPossibleValues 'InterfacePattern','XAIService',5,'XAIService',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("0ADD6171-90AA-4938-A2E5-C60069DE475E"), "EXEC db_AddPossibleValues 'InterfacePattern','Singleton',6,'Singleton',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("B4E69C5B-DBCC-499B-BCA5-7BED898DBF7E"), "EXEC db_AddPossibleValues 'InterfacePattern','WebService',7,'WebService',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("3074F3F3-1D2E-4D0F-8A62-B0685B7E274A"), "EXEC db_AddPossibleValues 'InterfacePattern','PublishAndSubscribe',8,'PublishAndSubscribe',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("D0872C78-7459-41B5-8B5D-4171D8272E5B"), "EXEC db_AddPossibleValues 'InterfacePattern','LibraryImplementation',9,'LibraryImplementation',1|AppInterfaceModel");

            databaseVersions.Add(new Guid("6FF5EFDC-DEA9-4A1D-A22C-A3CB3BDEBBC8"), "exec db_addfields 'ApplicationInterface','Criticality','Criticality','General','',1,4,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("DA696B42-B3BC-4348-BFD7-8CFC5D007091"), "exec db_addfields 'ApplicationInterface','InterfaceProtocol','InterfaceProtocol','General','',1,4,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("CE64BBC4-D5F8-4432-8154-1E7623781289"), "exec db_addfields 'ApplicationInterface','InterfacePattern','InterfacePattern','General','',1,4,1|AppInterfaceModel");

            databaseVersions.Add(new Guid("FE8F8613-66A8-4350-8860-DA94E74BBD20"), "EXEC db_addClassAssociations 'ApplicationInterface','LogicalITInfrastructureComponent','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("FD29EDA6-F8E1-4012-969C-A878E8E7DCB5"), "EXEC db_addClassAssociations 'LogicalITInfrastructureComponent','ApplicationInterface','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("98431AD4-3F63-4ED6-AE28-2468577434B7"), "EXEC db_addClassAssociations 'ApplicationInterface','Role','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("13D59B7A-BF3B-4949-BD99-861CE589800F"), "EXEC db_addClassAssociations 'ApplicationInterface','Job','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("44C928C3-F246-418F-B1D0-D8796FDEAA48"), "EXEC db_addClassAssociations 'ApplicationInterface','JobPosition','Mapping','',0,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("1E715414-4046-4C14-9F1E-FF3749D06563"), "EXEC db_addClassAssociations 'GovernanceMechanism','ApplicationInterface','Mapping','',0,1|AppInterfaceModel");

            databaseVersions.Add(new Guid("C6B8440B-012B-4A75-B7B2-3DD17CFF7CA5"), "EXEC db_addArtifacts 'ApplicationInterface','Role','Mapping','Role',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("A887F06C-9EC2-4AE3-9B48-1E5F925EC662"), "EXEC db_addArtifacts 'ApplicationInterface','Job','Mapping','Role',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("AE56C40F-5EEF-42DB-9CCA-E8046D7C2F05"), "EXEC db_addArtifacts 'ApplicationInterface','JobPosition','Mapping','Role',1|AppInterfaceModel");

            databaseVersions.Add(new Guid("975ECA0D-5567-4084-9E62-34705795E1F5"), "EXEC db_AddPossibleValues 'MeasureType','Area',1,'Area',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("98EDEF67-335F-45E9-A128-62EAD64CE1CD"), "EXEC db_AddPossibleValues 'MeasureType','Indicator',2,'Indicator',1|AppInterfaceModel");
            databaseVersions.Add(new Guid("96391BE4-9CA1-4BCB-BC2E-37EA92658C20"), "EXEC db_AddPossibleValues 'MeasureType','Value',3,'Value',1|AppInterfaceModel");

            databaseVersions.Add(new Guid("91BD2B53-0E24-48D4-87E2-4D701C67928F"), "update field set datatype = 'MeasureType' where name = 'MeasureType'|AppInterfaceModel");

            databaseVersions.Add(new Guid("91B1E1B0-C110-4EA3-8BD7-E791BDBB620E"), "EXEC db_AddFields 'Environment','Name','EnvironmentType','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|Fix Environment");
            databaseVersions.Add(new Guid("572BBC34-7653-4FCA-B456-048426E61F7A"), "EXEC db_AddFields 'EnvironmentCategory','Name','EnvironmentCategory','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|Fix Environment");

            #endregion

            #region august bugfixing
            databaseVersions.Add(new Guid("58688216-FE93-4B5E-9EB7-1E18CDA49D46"), "ALTER TABLE dbo.class ALTER COLUMN descriptioncode varchar(750) NOT NULL|Update class descriptioncode varchar(750)");
            databaseVersions.Add(new Guid("FDD00F4F-4039-43CA-B577-C713714C3257"), "Update field set isactive = 0 where datatype = 'contextualindicator'|DomainFix");
            databaseVersions.Add(new Guid("37A344C0-AB42-484B-8AB9-D0379FC027B8"), "Update field set isactive = 0 where datatype = 'environmentindicator'|DomainFix");

            databaseVersions.Add(new Guid("8A2F13DB-5C26-455D-B588-B6E358EC61C1"), "Exec db_AddClassAssociations 'ITInfrastructureEnvironment','ITInfrastructureEnvironment','Mapping','',1,1|IIE Associations");
            databaseVersions.Add(new Guid("9331CFB2-B134-4D6E-86B8-522595473CE0"), "Exec db_AddClassAssociations 'ITInfrastructureEnvironment','PhysicalSoftwareComponent','Mapping','',1,1|IIE Associations");
            databaseVersions.Add(new Guid("D2C0E954-F33D-47A4-B17E-7C805620986C"), "Exec db_AddClassAssociations 'ITInfrastructureEnvironment','PhysicalDataComponent','Mapping','',1,1|IIE Associations");
            databaseVersions.Add(new Guid("FA7BD5C1-D6E3-400B-BF64-AED9FEEBF432"), "Exec db_AddClassAssociations 'ITInfrastructureEnvironment','ComputingComponent','Mapping','',1,1|IIE Associations");
            databaseVersions.Add(new Guid("C9CF8D42-F008-4731-BB06-A095256B5373"), "Exec db_AddClassAssociations 'ITInfrastructureEnvironment','StorageComponent','Mapping','',1,1|IIE Associations");
            databaseVersions.Add(new Guid("B26CF2BC-B71B-4560-AD72-22297ED1593C"), "Exec db_AddClassAssociations 'ITInfrastructureEnvironment','NetworkComponent','Mapping','',1,1|IIE Associations");
            databaseVersions.Add(new Guid("485FC4FD-36EC-4B51-B9AD-E2396B4C6DC5"), "Exec db_AddClassAssociations 'ITInfrastructureEnvironment','PeripheralComponent','Mapping','',1,1|IIE Associations");

            databaseVersions.Add(new Guid("F228E779-9EDE-440D-B723-CF5446194BBC"), "ALTER TABLE dbo.class ALTER COLUMN descriptioncode varchar(1000) NOT NULL|Update class descriptioncode varchar(1000)");
#if !DEBUG
            databaseVersions.Add(new Guid("87BFAB27-090D-4BE2-8AB4-122BCE99418A"), "REBUILDVIEWS|REBUILDVIEWS"); //AppInterface
            databaseVersions.Add(new Guid("0558A793-3523-4412-B30D-7D303496B031"), "REBUILDVIEWS|REBUILDVIEWS"); //AppInterface 2
            databaseVersions.Add(new Guid("24D2548B-11D6-4709-8160-1C37FB21D44D"), "REBUILDVIEWS|REBUILDVIEWS"); //DomainFix
            //databaseVersions.Add(new Guid(""), "UNDUPLICATE|UNDUPLICATE");
            databaseVersions.Add(new Guid("0AB6C9C1-5107-4340-82B4-D7ED0978079D"), "REBUILDVIEWS|REBUILDVIEWS"); //TimeIndicatorDeactiveName
#endif
            databaseVersions.Add(new Guid("88044033-7901-4E46-9B6C-65D26BC61F99"), "EXEC db_AddFields 'Environment','Name','EnvironmentIndicator','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|Fix Environment");
            //databaseVersions.Add(new Guid("ED284134-A0ED-4794-B39A-6436F1195BFB"), "EXEC db_AddFields 'TimeIndicator','Name','System.String','General','Value',0,0,0|Fix Time Indicator");

            //DynamicDataFlow 
            //to and from Process and PhysicalDataComponent
            //databaseVersions.Add(new Guid("D551D286-12AB-4233-96A8-202F53717578"), "if not exists (select name from associationtype where name = 'DynamicDataFlow') INSERT INTO AssociationType (Name,IsTwoWay,LinkSpecification) values ('DynamicDataFlow',NULL,NULL)|DDF Associations");
            //databaseVersions.Add(new Guid("5F60048C-248D-40EE-8C25-860A1E4AC2A3"), "Exec db_AddClassAssociations 'Process','PhysicalDataComponent','DynamicDataFlow','',0,1|DDF Associations");
            //databaseVersions.Add(new Guid("B23CE256-3E6D-4258-9ECC-4D394BE25341"), "Exec db_AddClassAssociations 'PhysicalDataComponent','Process','DynamicDataFlow','',0,1|DDF Associations");

            databaseVersions.Add(new Guid("F98F33AC-FBB5-4C53-83D2-45A386F9F0FA"), "Exec db_AddClassAssociations 'Process','PhysicalDataComponent','DynamicFlow','',1,1|DDF Associations");
            databaseVersions.Add(new Guid("FB5FC23D-9C73-4861-85B2-3195C72CFA26"), "Exec db_AddClassAssociations 'PhysicalDataComponent','Process','DynamicFlow','',1,1|DDF Associations");
            databaseVersions.Add(new Guid("B8BEE1F7-DC23-47E9-91E2-5C5B56538E82"), "EXEC db_addArtifacts 'Process','PhysicalDataComponent','DynamicFlow','FlowDescription',1|DDF Associations");
            databaseVersions.Add(new Guid("C86E6097-53C5-4F2A-926C-03B1817A5522"), "EXEC db_addArtifacts 'PhysicalDataComponent','Process','DynamicFlow','FlowDescription',1|DDF Associations");

            databaseVersions.Add(new Guid("DEE05963-B23B-4DC8-947A-ED422696A4EE"), "EXEC db_AddPossibleValues 'SoftwareType','WindowsService',8,'WindowsService',1|Add Software Types");
            databaseVersions.Add(new Guid("5C4ED906-36C7-427A-A22A-B95DECE17D04"), "EXEC db_AddPossibleValues 'SoftwareType','WebService',9,'WebService',1|Add Software Types");

            databaseVersions.Add(new Guid("1B271F7B-73B9-4A17-9307-EB875C9F0CEA"), "delete allowedartifact where caid in (select caid from classassociation where caption is null)|Remove Duplicates");
            databaseVersions.Add(new Guid("DD0FDDF4-69FC-45A6-89DB-32053CE09359"), "delete classassociation where caption is null|Remove Duplicates");

            databaseVersions.Add(new Guid("FF4B22E8-75EB-4481-A3B1-9CBCF858611E"), "Exec db_AddClassAssociations 'Process','PhysicalDataComponent','Mapping','',0,1|DDF Associations");
            databaseVersions.Add(new Guid("9FB27309-2C0B-41E0-B0F3-0D10180CBB93"), "Exec db_AddClassAssociations 'PhysicalDataComponent','Process','Mapping','',0,1|DDF Associations");

            databaseVersions.Add(new Guid("16AC7445-0C3C-4FB3-85DF-B53A33F083D0"), "EXEC db_AddPossibleValues 'ComputingComponentType','ServerTemplate',4,'ServerTemplate',1|Add Software Types");
            databaseVersions.Add(new Guid("53A13AEC-1F9D-4597-A5F8-EDBD43AFB348"), "EXEC db_AddPossibleValues 'ServerType','DevelopmentServer',15,'DevelopmentServer',1|Add Software Types");
            databaseVersions.Add(new Guid("2C64AE68-B8F4-435B-8B45-888D26302405"), "EXEC db_AddPossibleValues 'ServerType','UATServer',16,'UATServer',1|Add Software Types");

            databaseVersions.Add(new Guid("1FF81F1D-A7C9-4705-AD0B-DAC3743BB438"), "Exec db_AddClassAssociations 'Conditional','PhysicalSoftwareComponent','DynamicFlow','',1,1|Associations");
            databaseVersions.Add(new Guid("2EC596C6-4B14-45D1-A9AB-2AA990B1FB11"), "Exec db_AddClassAssociations 'Conditional','Object','DynamicFlow','',1,1|Associations");
            databaseVersions.Add(new Guid("5990730D-C681-413A-BFBF-A568B5BACD34"), "EXEC db_addArtifacts 'Conditional','PhysicalSoftwareComponent','DynamicFlow','FlowDescription',1|Associations");
            databaseVersions.Add(new Guid("E6C9EB90-BA3F-4D63-B303-DC64FFB2A2DA"), "EXEC db_addArtifacts 'Conditional','Object','DynamicFlow','FlowDescription',1|Associations");

            databaseVersions.Add(new Guid("B272C4A3-920E-49B2-9594-0DC992DD7079"), "EXEC db_AddPossibleValues 'ServerType','Appl',2,'Application',0|Add Server Type");
            databaseVersions.Add(new Guid("775FC8A9-465A-4171-AF85-884912A10C51"), "EXEC db_AddPossibleValues 'ServerType','HostServer',17,'Host Server',1|Add Server Type");

            longQuery = "IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[db_AddClassAssociations]') AND type in (N'P', N'PC')) DROP PROCEDURE [dbo].[db_AddClassAssociations]";
            databaseVersions.Add(new Guid("3A3D373F-7B03-48E0-8D3C-89487D128481"), longQuery + "|RemoveCAProc");
            longQuery = "CREATE PROCEDURE [dbo].[db_AddClassAssociations] ( @parentClass varchar(50), @childClass varchar(50), @Association varchar(50), @Caption varchar(50), @IsDefault int, @IsActive int ) AS BEGIN DECLARE @CAID int DECLARE @AssociationID int IF (@Caption IS NULL) OR (@Caption = '') 	SET @Caption = (@parentClass + ' to ' + @childClass + ' ' + @Association) SET @CAID = (SELECT TOP (1) ClassAssociation.CAid FROM ClassAssociation INNER JOIN AssociationType ON ClassAssociation.AssociationTypeID = AssociationType.pkid WHERE (ClassAssociation.ParentClass = @parentClass) AND (ClassAssociation.ChildClass = @childClass) AND (AssociationType.Name = @Association)) SET @AssociationID = (SELECT TOP (1) AssociationType.pkid FROM AssociationType WHERE AssociationType.Name = @Association) IF (@CAID IS NULL) OR (@CAID = '') OR (@CAID = 0) IF (@AssociationID IS NULL) OR (@AssociationID = 0) BEGIN INSERT INTO AssociationType([Name]) VALUES (@Association) EXEC db_AddClassAssociations @parentClass=@parentClass,@childClass=@childClass,@Association=@Association,@Caption=@Caption,@IsDefault=@IsDefault,@IsActive=@IsActive END ELSE INSERT INTO ClassAssociation (ParentClass, ChildClass, AssociationTypeID, Caption, IsDefault, IsActive) VALUES (@parentClass, @childClass,@AssociationID,@Caption,@IsDefault,@IsActive) ELSE UPDATE ClassAssociation SET IsActive = @IsActive, IsDefault = @IsDefault, Caption = @Caption WHERE CAID = @CAID END";
            databaseVersions.Add(new Guid("54E594B6-3397-45D5-A92E-57EDDEE669E7"), longQuery + "|AddCAProce");

            longQuery = "Update ClassAssociation SET IsDefault = 0  where parentclass = 'PhysicalDataComponent' and childclass = 'Process'|UnDefault";
            databaseVersions.Add(new Guid("2A238AA6-B714-4E09-A844-44B4FB0A32E0"), longQuery);
            longQuery = "Update ClassAssociation SET IsDefault = 0  where parentclass = 'Process' and childclass = 'PhysicalDataComponent'|UnDefault";
            databaseVersions.Add(new Guid("17D0B2AE-7C9B-4903-B407-DC8F76CE5566"), longQuery);

            databaseVersions.Add(new Guid("A76A1357-1885-4848-9B48-2EDDA802D32D"), "Exec db_AddClassAssociations 'Process','PhysicalDataComponent','DynamicFlow','',1,1|DDF Associations");
            databaseVersions.Add(new Guid("8B1B9676-4177-4662-AC04-FA419B2E28AD"), "Exec db_AddClassAssociations 'PhysicalDataComponent','Process','DynamicFlow','',1,1|DDF Associations");

            databaseVersions.Add(new Guid("A3CE5678-361A-4AA5-96B1-19EB04247D09"), "UPDATE Class SET DescriptionCode = '!string.IsNullOrEmpty(Description)?Description:!string.IsNullOrEmpty(Name)?Name:\"\"' WHERE Name = 'Logic'|Fix Logic Code");

            databaseVersions.Add(new Guid("2819E17F-6EF9-4EEA-96E1-15FEE59B859D"), "UPDATE Class SET IsActive = 0 WHERE Name = 'Logic'|Deactivate Logic");

            databaseVersions.Add(new Guid("124D9447-AAC8-4CCF-91A0-2BA0EA7F017A"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\WindowsService.png')|Add DDPV Image");
            databaseVersions.Add(new Guid("DA85D0E0-F6B4-4C00-AB0C-EFFBD46D77FE"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\WebService.png')|Add DDPV Image");
            databaseVersions.Add(new Guid("A600BF77-C0EE-4CC7-A0F3-E98D844C3A56"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\WindowsService.png') WHERE PossibleValue = 'WindowsService'|Add Image");
            databaseVersions.Add(new Guid("74ABA04D-EB0E-4E82-8917-F48F006F52CD"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\WebService.png') WHERE PossibleValue = 'WebService'|Add Image");

            databaseVersions.Add(new Guid("F471EC24-38A5-4F47-927D-D58F788E252C"), "Exec db_AddClassAssociations 'Conditional','Object','DynamicFlow','',1,1|Add Association");
            databaseVersions.Add(new Guid("E05E24CE-BFBB-4328-8A96-52F6A0084E19"), "EXEC db_addArtifacts 'Conditional','Object','DynamicFlow','FlowDescription',1|Add Artifact");

            databaseVersions.Add(new Guid("140C6869-2F8A-46CD-BDE4-224C07F9DF04"), "Exec db_AddClassAssociations 'Function','Conditional','DynamicFlow','',1,1|Add Association");
            databaseVersions.Add(new Guid("DE5C84D7-B61D-458F-95DF-83A135A863A0"), "EXEC db_addArtifacts 'Function','Conditional','DynamicFlow','FlowDescription',1|Add Artifact");
            databaseVersions.Add(new Guid("01D54AF3-26DE-464B-8ED5-A22E3816C119"), "Exec db_AddClassAssociations 'Conditional','Function','DynamicFlow','',1,1|Add Association");
            databaseVersions.Add(new Guid("F46E0BEA-8CF6-4EE6-85B8-E69709CA442F"), "EXEC db_addArtifacts 'Conditional','Function','DynamicFlow','FlowDescription',1|Add Artifact");

            #endregion

            //Function/Process/Activity/Measure Mapping to businessinterface
            //Function/Process/Activity DynamicFlow(DEFAULT) to businessinterface with FlowDescription,Measure,Rationale
            //businessinterface mapping rationale

            #region BusinessInterface
            databaseVersions.Add(new Guid("A96D4DD2-A2C3-485E-A8D6-22E7866031F5"), "exec db_addfields 'ApplicationInterface','InterfaceFrequency','InterfaceFrequency','General','Unique identified of for object as per external data source.',1,6,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("1FF18245-9158-46A5-970B-F100A77BDEC6"), "exec db_addfields 'ApplicationInterface','InterfaceProtocol','InterfaceProtocol','General','Name of external data source where object and properties are sourced from.',1,12,1|AppInterfaceModel");
            databaseVersions.Add(new Guid("7A90F77B-8360-4608-9FDB-7E8FC73D6A56"), "Exec db_AddClassAssociations 'ApplicationInterface','Measure','Mapping','',1,1|AIM Associations");

            //add businessinterface class
            databaseVersions.Add(new Guid("C80BC9E9-E555-4112-8605-C411F38FE865"), "exec db_addClasses 'BusinessInterface','Name','Function',1|BussInterfaceModel");
            // Defaults
            databaseVersions.Add(new Guid("1A587D85-C33C-419A-878C-A5389CA430AF"), "exec db_addfields 'BusinessInterface','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',1,1,1|BussInterfaceModel");
            databaseVersions.Add(new Guid("27D61751-9A87-4C27-B179-422F08A2E050"), "exec db_addfields 'BusinessInterface','Description','System.String','General','General description for each object',1,2,1|BussInterfaceModel");
            databaseVersions.Add(new Guid("2BBB8C93-3DEB-42BA-9615-FF28F8DFDD2A"), "exec db_addfields 'BusinessInterface','Abbreviation','System.String','General','System.String',1,3,1|BussInterfaceModel");
            databaseVersions.Add(new Guid("A4E9C600-5F7A-420C-AC3F-36D5E81A6F9B"), "exec db_addfields 'BusinessInterface','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',1,4,1|BussInterfaceModel");
            databaseVersions.Add(new Guid("2E61E5A9-E186-4049-99B7-F64D12324CEE"), "exec db_addfields 'BusinessInterface','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',1,5,1|BussInterfaceModel");
            databaseVersions.Add(new Guid("064C7D3B-6ED7-400C-9DF5-A1E7F81D470D"), "exec db_addfields 'BusinessInterface','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',1,6,1|BussInterfaceModel");
            databaseVersions.Add(new Guid("14B29574-9107-49DB-8F74-23BA22F90D3B"), "exec db_addfields 'BusinessInterface','DesignRationale','System.String','General','System.String',1,7,1|BussInterfaceModel");
            databaseVersions.Add(new Guid("6FC4180A-D75E-45FD-A94E-A5DEDEC5F1A8"), "exec db_addfields 'BusinessInterface','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',1,8,1|BussInterfaceModel");
            databaseVersions.Add(new Guid("F9066BDE-0C5A-434E-BE5A-CD5928A27CD1"), "exec db_addfields 'BusinessInterface','GapType','GapType','General','Result of a gap analysis between architecture states.',1,9,1|BussInterfaceModel");
            databaseVersions.Add(new Guid("C445BD7B-83C7-4E96-883F-231FA4AAF20D"), "exec db_addfields 'BusinessInterface','DataSourceID','System.String','General','Unique identified of for object as per external data source.',1,80,1|BussInterfaceModel");
            databaseVersions.Add(new Guid("F2F5CB01-A018-43E0-A786-E3A93A62619C"), "exec db_addfields 'BusinessInterface','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',1,81,1|BussInterfaceModel");
            //ExecutionIndicator 5
            //InterfaceFrequency 6
            //InterfaceProtocol 12
            databaseVersions.Add(new Guid("48C2B818-46B1-4AAD-B97E-3C9C4B9D3547"), "exec db_addfields 'BusinessInterface','ExecutionIndicator','ExecutionIndicator','General','Result of a gap analysis between architecture states.',1,5,1|BussInterfaceModel");
            databaseVersions.Add(new Guid("0D4B68D9-4437-4743-8BD4-7F11F3AF2C13"), "exec db_addfields 'BusinessInterface','InterfaceFrequency','InterfaceFrequency','General','Unique identified of for object as per external data source.',1,6,1|BussInterfaceModel");
            databaseVersions.Add(new Guid("5980055A-E61D-421C-974C-F5D13CDE5FC0"), "exec db_addfields 'BusinessInterface','InterfaceProtocol','InterfaceProtocol','General','Name of external data source where object and properties are sourced from.',1,12,1|BussInterfaceModel");

            databaseVersions.Add(new Guid("7B973FE3-27F3-4B7E-BB96-97E79E044165"), "Exec db_AddClassAssociations 'BusinessInterface','BusinessInterface','Mapping','',1,1|BIM Associations");
            databaseVersions.Add(new Guid("53A35DF5-3C39-4C6C-B620-55095311A636"), "Exec db_AddClassAssociations 'BusinessInterface','Rationale','Mapping','',1,1|BIM Associations");
            databaseVersions.Add(new Guid("4D281C21-A690-46F8-BFCF-F7ED62467680"), "Exec db_AddClassAssociations 'BusinessInterface','Measure','Mapping','',1,1|BIM Associations");
            databaseVersions.Add(new Guid("F6CDD3C0-89AC-4B72-8A74-4BAE57CA3894"), "Exec db_AddClassAssociations 'BusinessInterface','Process','Mapping','',0,1|BIM Associations");
            databaseVersions.Add(new Guid("6E9DB552-ACC7-464E-844A-89BEAD641D86"), "Exec db_AddClassAssociations 'BusinessInterface','Function','Mapping','',0,1|BIM Associations");
            databaseVersions.Add(new Guid("EE8CA731-70D5-4421-BBDA-3FB079121272"), "Exec db_AddClassAssociations 'BusinessInterface','Activity','Mapping','',0,1|BIM Associations");

            databaseVersions.Add(new Guid("CFCA453B-3E7E-49C7-A7B8-524B4F63DD90"), "Exec db_AddClassAssociations 'BusinessInterface','Process','DynamicFlow','',1,1|BIM Associations");
            databaseVersions.Add(new Guid("DB46A142-5B19-4E05-A196-AA892A2F7D5D"), "Exec db_AddClassAssociations 'BusinessInterface','Function','DynamicFlow','',1,1|BIM Associations");
            databaseVersions.Add(new Guid("62AA133C-C9B7-46D5-B4F9-6FDAF4F0D965"), "Exec db_AddClassAssociations 'BusinessInterface','Activity','DynamicFlow','',1,1|BIM Associations");

            databaseVersions.Add(new Guid("44B7935B-F1AC-4810-9FBF-7CDB73D9EAB7"), "Exec db_AddClassAssociations 'Process','BusinessInterface','DynamicFlow','',1,1|BIM Associations");
            databaseVersions.Add(new Guid("1E86C3DB-D9F0-47A0-8336-1DB8A9D4F2C9"), "Exec db_AddClassAssociations 'Function','BusinessInterface','DynamicFlow','',1,1|BIM Associations");
            databaseVersions.Add(new Guid("C51F0D3F-BD73-45C5-941F-4922D7585300"), "Exec db_AddClassAssociations 'Activity','BusinessInterface','DynamicFlow','',1,1|BIM Associations");

            databaseVersions.Add(new Guid("94EC7BE8-FF79-48DC-8D55-01A0553F7409"), "EXEC db_addArtifacts 'BusinessInterface','Process','DynamicFlow','FlowDescription',1|BIM Artifacts");
            databaseVersions.Add(new Guid("3C49DABB-E414-44A3-B2A7-F4407F38DC5A"), "EXEC db_addArtifacts 'BusinessInterface','Process','DynamicFlow','Measure',1|BIM Artifacts");
            databaseVersions.Add(new Guid("58B888D0-F828-4E18-A313-73F22FBE74E4"), "EXEC db_addArtifacts 'BusinessInterface','Process','DynamicFlow','Rationale',1|BIM Artifacts");

            databaseVersions.Add(new Guid("4F6D3F59-A7AB-4A30-A5D5-446106DEDB76"), "EXEC db_addArtifacts 'BusinessInterface','Function','DynamicFlow','FlowDescription',1|BIM Artifacts");
            databaseVersions.Add(new Guid("6E821290-A689-44AD-B92F-B10CCAC9D8F9"), "EXEC db_addArtifacts 'BusinessInterface','Function','DynamicFlow','Measure',1|BIM Artifacts");
            databaseVersions.Add(new Guid("C1195C8D-C25C-4303-A8CE-5C919F49832F"), "EXEC db_addArtifacts 'BusinessInterface','Function','DynamicFlow','Rationale',1|BIM Artifacts");

            databaseVersions.Add(new Guid("E0C7A608-A092-4677-AA53-48F6F2AAD5B0"), "EXEC db_addArtifacts 'BusinessInterface','Activity','DynamicFlow','FlowDescription',1|BIM Artifacts");
            databaseVersions.Add(new Guid("CE6D336E-8DED-494C-B6D8-C32DD8CF7487"), "EXEC db_addArtifacts 'BusinessInterface','Activity','DynamicFlow','Measure',1|BIM Artifacts");
            databaseVersions.Add(new Guid("BF875C69-3139-4CCE-BB7F-5654FCA0A03E"), "EXEC db_addArtifacts 'BusinessInterface','Activity','DynamicFlow','Rationale',1|BIM Artifacts");

            databaseVersions.Add(new Guid("34091312-C15B-4738-A43E-26553028EE2B"), "EXEC db_addArtifacts 'Process','BusinessInterface','DynamicFlow','FlowDescription',1|BIM Artifacts");
            databaseVersions.Add(new Guid("AFE21C74-6E04-4B28-8AF6-BA9114870C5A"), "EXEC db_addArtifacts 'Process','BusinessInterface','DynamicFlow','Measure',1|BIM Artifacts");
            databaseVersions.Add(new Guid("586D99F4-2F55-4A61-9452-43803DD13E5E"), "EXEC db_addArtifacts 'Process','BusinessInterface','DynamicFlow','Rationale',1|BIM Artifacts");

            databaseVersions.Add(new Guid("CF7166FA-9EF3-46C0-A2D6-CE25633F4598"), "EXEC db_addArtifacts 'Function','BusinessInterface','DynamicFlow','FlowDescription',1|BIM Artifacts");
            databaseVersions.Add(new Guid("DD042C4D-5208-4C6F-B412-86B9F8BDDA4C"), "EXEC db_addArtifacts 'Function','BusinessInterface','DynamicFlow','Measure',1|BIM Artifacts");
            databaseVersions.Add(new Guid("6184B348-08C2-4AAB-A06B-FB126783418C"), "EXEC db_addArtifacts 'Function','BusinessInterface','DynamicFlow','Rationale',1|BIM Artifacts");

            databaseVersions.Add(new Guid("1FC2727C-E8E1-4C41-882F-9DA005C0E8AB"), "EXEC db_addArtifacts 'Activity','BusinessInterface','DynamicFlow','FlowDescription',1|BIM Artifacts");
            databaseVersions.Add(new Guid("5D75E771-AEF5-4CA1-92C4-A2667C814849"), "EXEC db_addArtifacts 'Activity','BusinessInterface','DynamicFlow','Measure',1|BIM Artifacts");
            databaseVersions.Add(new Guid("220EB72A-EDB8-4F11-B814-62A5460FA9BD"), "EXEC db_addArtifacts 'Activity','BusinessInterface','DynamicFlow','Rationale',1|BIM Artifacts");

            databaseVersions.Add(new Guid("4A9CB502-67CB-4B25-B157-82F210A2B579"), "Exec db_AddClassAssociations 'Measure','LogicalInformationArtefact','Mapping','',1,1|IA Associations");
            databaseVersions.Add(new Guid("7B60FD51-C2AD-41B1-B75D-948FC5EDF74C"), "Exec db_AddClassAssociations 'Measure','PhysicalInformationArtefact','Mapping','',1,1|IA Associations");

            databaseVersions.Add(new Guid("3BC5AC85-1C59-4A1A-BA02-764BCDAD54A4"), "Exec db_AddClassAssociations 'Measure','Function','Mapping','',1,1|IA Associations");
            databaseVersions.Add(new Guid("6ED5866F-2C41-4395-B11B-DBE971E1C7E3"), "Exec db_AddClassAssociations 'Measure','Process','Mapping','',1,1|IA Associations");
            databaseVersions.Add(new Guid("FED1ADF9-AEE6-4A3B-A902-D0B0479C6EFC"), "Exec db_AddClassAssociations 'Measure','Activity','Mapping','',1,1|IA Associations");

            #endregion

            #region Att/Colum Common Attribute Addition

            databaseVersions.Add(new Guid("3DE24EC9-54FD-4B2E-8772-EFC5A85C2F4E"), "EXEC db_AddFields 'Attribute','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|AddCommonFldsForAttAndCol");
            databaseVersions.Add(new Guid("046E8CB6-CE25-49E3-B91B-A312D7D68B16"), "EXEC db_AddFields 'Attribute','Description','System.String','General','General description for each object',0,80,1|AddCommonFldsForAttAndCol");
            databaseVersions.Add(new Guid("6FE8DB1C-600F-478D-A26F-3BB26F9A398D"), "EXEC db_AddFields 'Attribute','Abbreviation','System.String','General','',0,81,1|AddCommonFldsForAttAndCol");
            databaseVersions.Add(new Guid("B5F66D37-4538-45F1-93DF-8B4513EC68DA"), "EXEC db_AddFields 'Attribute','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|AddCommonFldsForAttAndCol");
            databaseVersions.Add(new Guid("121D0876-DE14-484C-9B0E-3870E56E6487"), "EXEC db_AddFields 'Attribute','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|AddCommonFldsForAttAndCol");
            databaseVersions.Add(new Guid("5B5507F1-0F93-48EB-8CC0-56599109B714"), "EXEC db_AddFields 'Attribute','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|AddCommonFldsForAttAndCol");
            databaseVersions.Add(new Guid("442F5FFA-E3E1-4E4F-B5FA-D5A85F20044A"), "EXEC db_AddFields 'Attribute','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|AddCommonFldsForAttAndCol");
            databaseVersions.Add(new Guid("7B549148-36F8-4CE7-81BA-6A9EBDC4DB0D"), "EXEC db_AddFields 'Attribute','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|AddCommonFldsForAttAndCol");
            databaseVersions.Add(new Guid("36275B2C-BF73-4C8D-B8D3-B4D52C32C5D3"), "EXEC db_AddFields 'Attribute','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|AddCommonFldsForAttAndCol");
            databaseVersions.Add(new Guid("11B24EB8-35C6-4F26-AD40-78EB07FE30AE"), "EXEC db_AddFields 'Attribute','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|AddCommonFldsForAttAndCol");

            databaseVersions.Add(new Guid("FF9015E7-754A-489C-9C42-B157CD02DA90"), "EXEC db_AddFields 'DataColumn','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,0,1|AddCommonFldsForAttAndCol");
            databaseVersions.Add(new Guid("AA6D3E4E-DF78-4081-9756-6A75CF859A23"), "EXEC db_AddFields 'DataColumn','Description','System.String','General','General description for each object',0,80,1|AddCommonFldsForAttAndCol");
            databaseVersions.Add(new Guid("8609445F-EA64-49F2-B7C2-1941063E5376"), "EXEC db_AddFields 'DataColumn','Abbreviation','System.String','General','',0,81,1|AddCommonFldsForAttAndCol");
            databaseVersions.Add(new Guid("3530222C-D76A-4701-9171-32A796A74171"), "EXEC db_AddFields 'DataColumn','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',0,82,1|AddCommonFldsForAttAndCol");
            databaseVersions.Add(new Guid("FF54CDD8-C6C8-46A2-B5D9-1D61E6C1061A"), "EXEC db_AddFields 'DataColumn','StandardisationStatus','StandardisationStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',0,83,1|AddCommonFldsForAttAndCol");
            databaseVersions.Add(new Guid("0ABBCFE4-767D-484A-B046-CEEC6DF8D647"), "EXEC db_AddFields 'DataColumn','StandardisationStatusDate','System.String','General','Effective date of standardisation status.',0,84,1|AddCommonFldsForAttAndCol");
            databaseVersions.Add(new Guid("66AC629C-F7F3-4357-973B-A8C4FEE28922"), "EXEC db_AddFields 'DataColumn','DataSourceID','System.String','General','Unique identified of for object as per external data source.',0,85,1|AddCommonFldsForAttAndCol");
            databaseVersions.Add(new Guid("352884F9-0F35-4519-BF0B-4BC83C37DD98"), "EXEC db_AddFields 'DataColumn','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',0,86,1|AddCommonFldsForAttAndCol");
            databaseVersions.Add(new Guid("6AF9BD5C-3D06-4BE7-B83C-CF95A35A0D21"), "EXEC db_AddFields 'DataColumn','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',0,87,1|AddCommonFldsForAttAndCol");
            databaseVersions.Add(new Guid("296D7F17-896F-4529-B476-1941266D6E58"), "EXEC db_AddFields 'DataColumn','GapType','GapType','General','Result of a gap analysis between architecture states.',0,90,1|AddCommonFldsForAttAndCol");

            databaseVersions.Add(new Guid("A69F403B-0B2C-4FA8-B733-3D35716650C4"), "REBUILDVIEWS|REBUILDVIEWS");
            #endregion

            //task/loginfoartefact/physinfoartefact mapping to measure

            #region RationalFix

            databaseVersions.Add(new Guid("31D62196-4F5C-4B07-B741-7FB336276877"), "UPDATE Field SET DataType = 'LongText' WHERE Class = 'Rationale' AND Name = 'UniqueRef'|Fix rationale");
            databaseVersions.Add(new Guid("4ACF3489-656B-4438-8FEA-64654AF70D24"), "ALTER VIEW [dbo].[METAView_Rationale_Listing] AS SELECT  MetaObject.WorkspaceName,MetaObject.WorkspaceTypeId,MetaObject.VCStatusID,MetaObject.pkid,MetaObject.Machine,VCMachineID,UniqueRefValue.ValueLongText as UniqueRef,RationaleTypeValue.ValueString as RationaleType,ValueValue.ValueLongText as Value,AuthorNameValue.ValueString as AuthorName,EffectiveDateValue.ValueString as EffectiveDate,GapTypeValue.ValueString as GapType FROM MetaObject INNER JOIN  dbo.Field  UniqueRefField  ON MetaObject.Class = UniqueRefField.Class left outer JOIN  dbo.ObjectFieldValue UniqueRefValue ON MetaObject.pkid=UniqueRefValue.ObjectID and MetaObject.Machine=UniqueRefValue.MachineID and UniqueRefField.pkid = UniqueRefValue.fieldid  INNER JOIN  dbo.Field  RationaleTypeField  ON MetaObject.Class = RationaleTypeField.Class left outer JOIN  dbo.ObjectFieldValue RationaleTypeValue ON MetaObject.pkid=RationaleTypeValue.ObjectID and MetaObject.Machine=RationaleTypeValue.MachineID and RationaleTypeField.pkid = RationaleTypeValue.fieldid  INNER JOIN  dbo.Field  ValueField  ON MetaObject.Class = ValueField.Class left outer JOIN  dbo.ObjectFieldValue ValueValue ON MetaObject.pkid=ValueValue.ObjectID and MetaObject.Machine=ValueValue.MachineID and ValueField.pkid = ValueValue.fieldid  INNER JOIN  dbo.Field  AuthorNameField  ON MetaObject.Class = AuthorNameField.Class left outer JOIN  dbo.ObjectFieldValue AuthorNameValue ON MetaObject.pkid=AuthorNameValue.ObjectID and MetaObject.Machine=AuthorNameValue.MachineID and AuthorNameField.pkid = AuthorNameValue.fieldid  INNER JOIN  dbo.Field  EffectiveDateField  ON MetaObject.Class = EffectiveDateField.Class left outer JOIN  dbo.ObjectFieldValue EffectiveDateValue ON MetaObject.pkid=EffectiveDateValue.ObjectID and MetaObject.Machine=EffectiveDateValue.MachineID and EffectiveDateField.pkid = EffectiveDateValue.fieldid  INNER JOIN  dbo.Field  GapTypeField  ON MetaObject.Class = GapTypeField.Class left outer JOIN   dbo.ObjectFieldValue GapTypeValue ON MetaObject.pkid=GapTypeValue.ObjectID and MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid   WHERE (MetaObject.Class = 'Rationale')  AND UniqueRefField.Name ='UniqueRef'  AND RationaleTypeField.Name ='RationaleType'  AND ValueField.Name ='Value'  AND AuthorNameField.Name ='AuthorName'  AND EffectiveDateField.Name ='EffectiveDate'  AND GapTypeField.Name ='GapType'|Metaview_rationale_listing to Longtext");
            databaseVersions.Add(new Guid("13916EFC-B652-487F-9673-086FA746A837"), "ALTER VIEW [dbo].[METAView_Rationale_Retrieval] AS SELECT  MetaObject.WorkspaceName,MetaObject.WorkspaceTypeId,MetaObject.VCStatusID,MetaObject.pkid,MetaObject.Machine,VCMachineID,UniqueRefValue.ValueLongText as UniqueRef,RationaleTypeValue.ValueString as RationaleType,ValueValue.ValueLongText as Value,AuthorNameValue.ValueString as AuthorName,EffectiveDateValue.ValueString as EffectiveDate,GapTypeValue.ValueString as GapType FROM MetaObject INNER JOIN  dbo.Field  UniqueRefField  ON MetaObject.Class = UniqueRefField.Class left outer JOIN  dbo.ObjectFieldValue UniqueRefValue ON MetaObject.pkid=UniqueRefValue.ObjectID and MetaObject.Machine=UniqueRefValue.MachineID and UniqueRefField.pkid = UniqueRefValue.fieldid  INNER JOIN  dbo.Field  RationaleTypeField  ON MetaObject.Class = RationaleTypeField.Class left outer JOIN  dbo.ObjectFieldValue RationaleTypeValue ON MetaObject.pkid=RationaleTypeValue.ObjectID and MetaObject.Machine=RationaleTypeValue.MachineID and RationaleTypeField.pkid = RationaleTypeValue.fieldid  INNER JOIN  dbo.Field  ValueField  ON MetaObject.Class = ValueField.Class left outer JOIN  dbo.ObjectFieldValue ValueValue ON MetaObject.pkid=ValueValue.ObjectID and MetaObject.Machine=ValueValue.MachineID and ValueField.pkid = ValueValue.fieldid  INNER JOIN  dbo.Field  AuthorNameField  ON MetaObject.Class = AuthorNameField.Class left outer JOIN  dbo.ObjectFieldValue AuthorNameValue ON MetaObject.pkid=AuthorNameValue.ObjectID and MetaObject.Machine=AuthorNameValue.MachineID and AuthorNameField.pkid = AuthorNameValue.fieldid  INNER JOIN  dbo.Field  EffectiveDateField  ON MetaObject.Class = EffectiveDateField.Class left outer JOIN  dbo.ObjectFieldValue EffectiveDateValue ON MetaObject.pkid=EffectiveDateValue.ObjectID and MetaObject.Machine=EffectiveDateValue.MachineID and EffectiveDateField.pkid = EffectiveDateValue.fieldid  INNER JOIN  dbo.Field  GapTypeField  ON MetaObject.Class = GapTypeField.Class left outer JOIN   dbo.ObjectFieldValue GapTypeValue ON MetaObject.pkid=GapTypeValue.ObjectID and MetaObject.Machine=GapTypeValue.MachineID and GapTypeField.pkid = GapTypeValue.fieldid   WHERE (MetaObject.Class = 'Rationale')  AND UniqueRefField.Name ='UniqueRef'  AND RationaleTypeField.Name ='RationaleType'  AND ValueField.Name ='Value'  AND AuthorNameField.Name ='AuthorName'  AND EffectiveDateField.Name ='EffectiveDate'  AND GapTypeField.Name ='GapType'|Metaview_rationale_retrieval to Longtext");

            databaseVersions.Add(new Guid("F32B63C3-A896-40E3-A6D2-A66F0D4042AD"), "DECLARE @FieldID INT  SET @FieldID = (SELECT pkid FROM field WHERE class = 'rationale' AND name = 'value') UPDATE objectfieldvalue SET valuelongtext = CASE WHEN valuelongtext IS NULL THEN valuestring ELSE valuelongtext END WHERE fieldid = @FieldID|Update rationale values to longtext");
            databaseVersions.Add(new Guid("E1BC0D64-9CD8-4C82-B5C2-9C1533AE9573"), "DECLARE @FieldID INT  SET @FieldID = (SELECT pkid FROM field WHERE class = 'rationale' AND name = 'uniqueref') UPDATE objectfieldvalue SET valuelongtext = CASE WHEN valuelongtext IS NULL THEN valuestring ELSE valuelongtext END WHERE fieldid = @FieldID|Update rationale uniquerefs to longtext");

            #endregion

            #region Additional Entity Fields For Standard Model

            //Entity
            //  Synonym 10
            databaseVersions.Add(new Guid("06DF18DA-9011-411F-B712-78976F0B637C"), "EXEC db_AddFields 'Entity','Synonym','System.String','General','',0,10,1|AEACFFSM");
            //Data Attribute
            databaseVersions.Add(new Guid("70D2BF19-9234-4FB6-A494-578FE2B27879"), "EXEC db_AddFields 'Attribute','RulesMaturityRating','System.String','General','',0,6,1|AEACFFSM");
            databaseVersions.Add(new Guid("C7CF8D65-069D-4A02-9C24-86987C5439BB"), "EXEC db_AddFields 'Attribute','DataQualityRating','System.String','General','',0,7,1|AEACFFSM");
            databaseVersions.Add(new Guid("1ECD486B-B15F-4FD4-BE56-A75707DC7B54"), "EXEC db_AddFields 'Attribute','DataRiskRating','System.String','General','',0,8,1|AEACFFSM");
            databaseVersions.Add(new Guid("539B321C-ECE9-4EC2-83DB-3F4392B33BE7"), "EXEC db_AddFields 'Attribute','RegulatoryRequirement','System.String','General','',0,9,1|AEACFFSM");
            databaseVersions.Add(new Guid("9DDBBFD7-1278-444D-979A-57EF30FB32E9"), "EXEC db_AddFields 'Attribute','Synonym','System.String','General','',0,10,1|AEACFFSM");
            //  Rules Maturity Rating 6
            //  Data Quality Rating 7
            //  Data Risk Rating 8
            //  Regulatory Requirement 9
            //  Synonym 10
            //Data Column
            databaseVersions.Add(new Guid("4986F895-FB03-4249-B7A9-ECBDD9CB110A"), "EXEC db_AddFields 'DataColumn','NumberofValues','System.String','General','',0,6,1|AEACFFSM");
            databaseVersions.Add(new Guid("85E1BF7E-FABA-403F-B9C0-7FF42A3E744C"), "EXEC db_AddFields 'DataColumn','NullValuePercentage','System.String','General','',0,7,1|AEACFFSM");
            databaseVersions.Add(new Guid("9618F645-4BDA-44B1-ACC0-B408A49BAC88"), "EXEC db_AddFields 'DataColumn','ConsistencyPercentage','System.String','General','',0,8,1|AEACFFSM");
            //  Number of Values 6
            //  Null Value Percentage 7
            //  Consistency Percentage 8
            databaseVersions.Add(new Guid("EA0C9A8E-11F3-44F9-9B08-907CABC6BFFD"), "REBUILDSINGLEVIEWS-Entity-DataColumn-Attribute-|AEACFFSM-Views");
            #endregion

            //databaseVersions.Add(new Guid("767DB802-0884-4EC1-B24E-68BCEB8E5726"), "REMAP|REMAP");
            //databaseVersions.Add(new Guid("4DDA0362-32B2-44B2-8B18-5109CDF2B2D4"), "REMAP|REMAP");

            #region 2015
            //For when metamodel v005 For Release is activated!
            databaseVersions.Add(new Guid("43E3E52E-3F89-4194-9D77-D095C8FAB85D"), "REMAP|REMAP");
            databaseVersions.Add(new Guid("B3F51003-0909-4EB3-A742-32C0CC3F2C2C"), "REBUILDVIEWS|REBUILDVIEWS");
            //#endif
            databaseVersions.Add(new Guid("F5FCAB17-28E8-4E91-BC75-A0AF32C9D9AA"), "EXEC db_AddClassAssociations 'Gateway','GovernanceMechanism','Mapping','',0,1|New Association");

            //RMB additive mappings
            databaseVersions.Add(new Guid("5853DCCE-9283-4B70-9979-A00E490373B8"), "EXEC db_AddClassAssociations 'DataValue','Object','Mapping','',0,1|New Association");
            databaseVersions.Add(new Guid("E190CF1C-091C-4F39-8809-170A544F30C5"), "EXEC db_AddClassAssociations 'DataValue','PhysicalDataComponent','Mapping','',0,1|New Association");
            databaseVersions.Add(new Guid("DC6014D7-0F11-41FD-A365-EF1FB5EE6D28"), "EXEC db_AddClassAssociations 'DataValue','PhysicalSoftwareComponent','Mapping','',0,1|New Association");
            databaseVersions.Add(new Guid("2759812E-5198-4CFD-88F5-55F2F2424308"), "EXEC db_AddClassAssociations 'DataValue','DataField','Mapping','',0,1|New Association");
            //databaseVersions.Add(new Guid(""), "EXEC db_AddClassAssociations 'DataValue','','Mapping','',0,1");

            //Class: Data Attribute
            //Class: Data Entity
            //Class: Logical Information Artefact
            //          Add Property: Synonyms
            //                Sort just after Abbreviation property

            databaseVersions.Add(new Guid("FE3B2E02-B5E0-4EA2-A095-5E1E05DC8D86"), "EXEC db_addFields 'DataAttribute','Synonyms','System.String','General','',1,2,1|Add field");
            databaseVersions.Add(new Guid("D22FA6D3-D87B-4FAC-9198-7F27C6C3C416"), "EXEC db_addFields 'DataEntity','Synonyms','System.String','General','',1,2,1|Add field");
            databaseVersions.Add(new Guid("8E61FE64-CB7D-4F36-8C79-7E26E714874A"), "EXEC db_addFields 'LogicalInformationArtefact','Synonyms','System.String','General','',1,2,1|Add field");

            //Class: Physical Information Artefact
            //Class: Data Table
            //Class: Physical Data Component
            //                Add Property: IsMasterDataSource
            //                                Sort just before Security Classification

            databaseVersions.Add(new Guid("F86843B6-0BDD-43CC-AFC7-A70ABFEBB787"), "EXEC db_addFields 'PhysicalInformationArtefact','IsMasterDataSource','System.String','General','',1,8,1|Add field");
            databaseVersions.Add(new Guid("D4714335-EDEF-4BD3-8004-3352082A712D"), "EXEC db_addFields 'DataTable','IsMasterDataSource','System.String','General','',1,8,1|Add field");
            databaseVersions.Add(new Guid("73A586D7-24CC-4BF0-AA56-C1432E86ACD8"), "EXEC db_addFields 'PhysicalDataComponent','IsMasterDataSource','System.String','General','',1,7,1|Add field");

            //databaseVersions.Add(new Guid("5566D8A5-7C5A-44C1-8540-F24237EF61F4"), "EXEC db_AddPossibleValues 'DataComponentType','Database',1,'Database',1|DataCompTypeUpdate");
            //databaseVersions.Add(new Guid("DBDBEDDB-E35B-41E6-8E21-0C09456EDFB8"), "EXEC db_AddPossibleValues 'DataComponentType','DataWarehouse(DWH)',2,'DataWarehouse(DWH)',1|DataCompTypeUpdate");
            //databaseVersions.Add(new Guid("96EA0CE2-0F11-4863-BFB9-381CA0C25325"), "EXEC db_AddPossibleValues 'DataComponentType','DataMart(DM)',3,'DataMart(DM)',1|DataCompTypeUpdate");
            //databaseVersions.Add(new Guid("38D4F498-9120-4017-9997-402F09814C04"), "EXEC db_AddPossibleValues 'DataComponentType','OperationalDataStore(ODS)',4,'OperationalDataStore(ODS)',1|DataCompTypeUpdate");
            //databaseVersions.Add(new Guid("91EA2503-F2CB-4710-BF7B-B86FD946C103"), "EXEC db_AddPossibleValues 'DataComponentType','Spreadsheet',5,'Spreadsheet',1|DataCompTypeUpdate");
            //databaseVersions.Add(new Guid("96D6B80A-4B5F-41AA-9DD0-7B1149B4A784"), "EXEC db_AddPossibleValues 'DataComponentType','InMemoryDatabase',6,'InMemoryDatabase',1|DataCompTypeUpdate");
            databaseVersions.Add(new Guid("407B3A24-03A6-4AFA-B36D-EC6B2083523B"), "EXEC db_AddPossibleValues 'DataComponentType','DataStageArea',7,'DataStage Area',1|DataCompTypeUpdate");
            databaseVersions.Add(new Guid("84B28D7B-44C7-49E7-8FD5-B53F6169C557"), "EXEC db_AddPossibleValues 'DataComponentType','DataCube',8,'DataCube',1|DataCompTypeUpdate");
            databaseVersions.Add(new Guid("E5701C7D-81BB-4E20-A9DD-D2DBEF49F91E"), "EXEC db_AddPossibleValues 'DataComponentType','RecordsManagementStore',9,'RecordsManagementStore',1|DataCompTypeUpdate");
            databaseVersions.Add(new Guid("FEE8A007-F318-445D-ADB2-982D3BAD7905"), "EXEC db_AddPossibleValues 'DataComponentType','ContentStore',10,'ContentStore',1|DataCompTypeUpdate");
            databaseVersions.Add(new Guid("BCBADB06-F29E-4303-9BC8-189F99F8686B"), "EXEC db_AddPossibleValues 'DataComponentType','UnstructuredDataStore',11,'UnstructuredDataStore',1|DataCompTypeUpdate");
            databaseVersions.Add(new Guid("6FA3AEDF-5AD2-402B-89FC-34F93A795ED7"), "EXEC db_AddPossibleValues 'DataComponentType','MasterDataManagementHub',12,'MasterDataManagementHub',1|DataCompTypeUpdate");
            databaseVersions.Add(new Guid("F1E20CBF-2F2B-4A38-8FD8-26DEB3DEAE0B"), "EXEC db_AddPossibleValues 'DataComponentType','MetadataRepository',13,'MetadataRepository',1|DataCompTypeUpdate");
            databaseVersions.Add(new Guid("9BDFECB8-8FE0-4002-B28B-1CBB76EDF3E6"), "EXEC db_AddPossibleValues 'DataComponentType','DataLake',14,'DataLake',1|DataCompTypeUpdate");
            //databaseVersions.Add(new Guid("1A49DDB9-C1A9-48D3-8FB4-905A742DDAFF"), "EXEC db_AddPossibleValues 'DataComponentType','DataWarehouse',2,'DataWarehouse',0|DataCompTypeUpdate");
            //databaseVersions.Add(new Guid("FF156A25-D855-469C-9D40-BF83E2D7BE15"), "EXEC db_AddPossibleValues 'DataComponentType','DataMart',3,'DataMart',0|DataCompTypeUpdate");
            //databaseVersions.Add(new Guid("45284054-2335-4ED8-8272-1DD4D7F6FA64"), "EXEC db_AddPossibleValues 'DataComponentType','OperationalDataStore',4,'OperationalDataStore',0|DataCompTypeUpdate");

            databaseVersions.Add(new Guid("ABAAAEF8-6FEA-4355-9BBE-D48C467297DC"), "EXEC db_AddClassAssociations 'LogicalInformationArtefact','ResourceType','Mapping','',0,1|HCL Mapping");

            databaseVersions.Add(new Guid("F0960946-2A3B-46D6-93E5-1A71188601E4"), "EXEC db_AddPossibleValues 'GatewayType','Complex',1,'Complex',0|GatewayTypeChange");
            databaseVersions.Add(new Guid("86359A80-06E5-41FA-BBD0-A2FBEE8557F9"), "EXEC db_AddPossibleValues 'GatewayType','EventBased',2,'EventBased',0|GatewayTypeChange");
            databaseVersions.Add(new Guid("381F2996-C2AA-4C3F-BE14-766FEE452627"), "EXEC db_AddPossibleValues 'GatewayType','Exclusive',3,'Exclusive',0|GatewayTypeChange");
            databaseVersions.Add(new Guid("2DD70BC2-9FBB-40E2-A78A-AFC33DB3BFE9"), "EXEC db_AddPossibleValues 'GatewayType','ExclusiveEventBased',4,'ExclusiveEventBased',0|GatewayTypeChange");
            databaseVersions.Add(new Guid("992CC03B-5401-433A-80D4-88BD068C7887"), "EXEC db_AddPossibleValues 'GatewayType','Inclusive',5,'Inclusive',0|GatewayTypeChange");
            databaseVersions.Add(new Guid("1C5CBCFA-84E0-40A1-9C60-6C167DF034A1"), "EXEC db_AddPossibleValues 'GatewayType','Parralel',6,'Parralel',0|GatewayTypeChange");
            databaseVersions.Add(new Guid("323C1608-A1BE-4CEE-9EF0-DA94C3D41834"), "EXEC db_AddPossibleValues 'GatewayType','ParralelEventBased',7,'ParralelEventBased',0|GatewayTypeChange");
            databaseVersions.Add(new Guid("63D327A9-7DF5-45F4-AF9F-B749F50A5DB1"), "EXEC db_AddPossibleValues 'GatewayType','Parallel',6,'Parallel',0|GatewayTypeChange");
            databaseVersions.Add(new Guid("07409A2D-B10F-4474-8118-36C57AFE227F"), "EXEC db_AddPossibleValues 'GatewayType','ParallelEventBased',7,'ParallelEventBased',0|GatewayTypeChange");

            databaseVersions.Add(new Guid("79B6F58D-163A-43BF-B04E-FEF29A698126"), "EXEC db_AddPossibleValues 'GatewayType','Condition',1,'Condition',1|GatewayTypeChange");
            databaseVersions.Add(new Guid("2F7568A0-F1CF-4B4C-8557-E93D8AC31BB6"), "EXEC db_AddPossibleValues 'GatewayType','Iteration',2,'Iteration',1|GatewayTypeChange");
            databaseVersions.Add(new Guid("B2582EDD-CF4B-4827-A5BC-BC7C91D1B137"), "EXEC db_AddPossibleValues 'GatewayType','Exception',3,'Exception',1|GatewayTypeChange");

            databaseVersions.Add(new Guid("B2BCDC9B-6266-4A60-AE91-E1BE693F9914"), "EXEC db_addFields 'Location','SpatialReference','System.String','General','',1,6,1|FirstRandFields");
            databaseVersions.Add(new Guid("8FD68723-CD54-4A36-8E6F-0AF2FF32B301"), "EXEC db_addFields 'Implication','UniqueReference','System.String','General','',1,1,1|FirstRandFields");
            databaseVersions.Add(new Guid("B9805974-CDFB-4293-B1B4-ED638234299F"), "EXEC db_addFields 'PhysicalSoftwareComponent','CriticalityLevel','System.String','General','',1,13,1|FirstRandFields");
            databaseVersions.Add(new Guid("E29CC721-A22B-40D5-95EB-3E3C962F550F"), "EXEC db_addFields 'PhysicalSoftwareComponent','DataUpdateStatus','System.String','General','',1,100,1|FirstRandFields");
            databaseVersions.Add(new Guid("583098C1-4ACF-4C20-977C-B6611DD047AF"), "EXEC db_addFields 'PhysicalSoftwareComponent','DataUpdateStatusDate','System.String','General','',1,100,1|FirstRandFields");
            databaseVersions.Add(new Guid("D48ECE4B-6601-4E45-8C3A-8E56C61F1AE0"), "EXEC db_addFields 'PhysicalSoftwareComponent','Synonyms','System.String','General','',1,2,1|FirstRandFields");
            databaseVersions.Add(new Guid("5FCF0B7C-EA11-4E19-A4EA-1D8FA1AA05D0"), "EXEC db_addFields 'PhysicalDataComponent','CriticalityLevel','System.String','General','',1,13,1|FirstRandFields");
            databaseVersions.Add(new Guid("4E0A8901-AC6C-4E13-AE29-DF04374127C5"), "EXEC db_addFields 'PhysicalDataComponent','DataUpdateStatus','System.String','General','',1,100,1|FirstRandFields");
            databaseVersions.Add(new Guid("E29B1E9F-2CBF-4A87-8536-B4DD806955E4"), "EXEC db_addFields 'PhysicalDataComponent','DataUpdateStatusDate','System.String','General','',1,100,1|FirstRandFields");
            databaseVersions.Add(new Guid("5BB397CC-AD40-4B5E-B3EC-2E6D24E57CCB"), "EXEC db_addFields 'PhysicalDataComponent','Synonyms','System.String','General','',1,2,1|FirstRandFields");

            #endregion

            //databaseVersions.Add(new Guid(""), "EXEC db_AddClassAssociations 'ApplicationInterface','Stakeholder','Mapping','',0,1|13March2015");

            //databaseVersions.Add(new Guid(""), "EXEC db_AddClassAssociations 'Network','Network','ConnectedTo','',0,1|13March2015");
            //databaseVersions.Add(new Guid(""), "UPDATE Class SET DescriptionCode = '' WHERE Name = 'NetworkDescription'|13March2015");

            //databaseVersions.Add(new Guid(""), "EXEC db_AddArtifacts 'NetworkComponent','NetworkComponent','ConnectedTo','Network',0|13March2015");

            #region March 2015

            databaseVersions.Add(new Guid("8F4E5E85-99BE-4DFC-901F-AF853BBBB7A8"), "EXEC db_AddClasses 'Requirement','Name','System',1|18March2015");
            databaseVersions.Add(new Guid("DCCD2A90-1DFC-4C34-8645-31F440B1803B"), "EXEC db_addFields 'Requirement','Name','System.String','General','Description',1,1,1|18March2015");
            databaseVersions.Add(new Guid("E420E588-07A5-41D9-AA92-C98EC68E7D77"), "EXEC db_AddClasses 'BusinessCompetency','Name','Strategy',0|18March2015");
            databaseVersions.Add(new Guid("0E66335E-8083-4147-97B3-F5EE9B94DE42"), "EXEC db_AddClassAssociations 'Resource','Location','Mapping','',1,1|18March2015");
            databaseVersions.Add(new Guid("3C013D9C-0F9D-463F-B650-47D770A8339B"), "EXEC db_AddClassAssociations 'Sys','Measure','Mapping','',1,1|18March2015");
            databaseVersions.Add(new Guid("3F508814-06A6-4E3C-80FA-77A58AA66F88"), "EXEC db_AddClassAssociations 'ITInfrastructureEnvironment','Location','Mapping','',1,1|18March2015");
            databaseVersions.Add(new Guid("B956C239-7020-4965-BABE-D12D2D374B83"), "EXEC db_AddClassAssociations 'Sys','ITInfrastructureEnvironment','Mapping','',1,1|18March2015");
            databaseVersions.Add(new Guid("B66D1274-1FC1-4B02-90A6-854EB9DD0FD0"), "EXEC db_AddClassAssociations 'Sys','Risk','Mapping','',1,1|18March2015");
            databaseVersions.Add(new Guid("93858639-0921-4660-9AF9-4FD81C88EDB8"), "EXEC db_AddClassAssociations 'Sys','WorkPackage','Mapping','',1,1|18March2015");
            databaseVersions.Add(new Guid("2A5CFCA1-7904-47FB-9CD8-060BCA9469AF"), "EXEC db_AddClassAssociations 'Sys','Network','Mapping','',1,1|18March2015");
            databaseVersions.Add(new Guid("0C0FED51-0A3C-4891-A48D-859832FB2695"), "EXEC db_AddClassAssociations 'Network','Network','Mapping','',0,1|18March2015");
            databaseVersions.Add(new Guid("BE062866-3691-4DD7-9F69-7355CF6A7EB1"), "EXEC db_AddClassAssociations 'Network','Network','Decomposition','',0,1|18March2015");
            databaseVersions.Add(new Guid("3A742FC3-0B01-4988-8F98-7AAC682E5342"), "EXEC db_AddClassAssociations 'WorkPackage','OrganizationalUnit','Mapping','',1,1|18March2015");
            databaseVersions.Add(new Guid("A87981B9-9150-4AE9-A628-85C371DF10C1"), "EXEC db_AddClassAssociations 'WorkPackage','Resource','Mapping','',1,1|18March2015");
            databaseVersions.Add(new Guid("905F8740-4703-45BC-8D27-61DD26381211"), "EXEC db_AddClassAssociations 'WorkPackage','TimeReference','Mapping','',1,1|18March2015");
            databaseVersions.Add(new Guid("BBCC0352-ADEE-4795-B0C0-EA90CD812169"), "EXEC db_AddClassAssociations 'WorkPackage','Location','Mapping','',1,1|18March2015");

            databaseVersions.Add(new Guid("6B38BD39-FD2D-42F4-A45D-0A32865D6FD8"), "EXEC db_AddClassAssociations 'Requirement','WorkPackage','Mapping','',1,1|18March2015");
            databaseVersions.Add(new Guid("79013AD9-9BD8-41B4-8A27-5260F2C35709"), "EXEC db_AddClassAssociations 'Requirement','Assumption','Mapping','',1,1|18March2015");
            databaseVersions.Add(new Guid("F573E25F-BF78-4E3F-9959-F2BB19AFFEF5"), "EXEC db_AddClassAssociations 'Requirement','Constraint','Mapping','',1,1|18March2015");
            databaseVersions.Add(new Guid("1AFEA5E1-8C92-42A2-9BF4-86DAFD4E3A3B"), "EXEC db_AddClassAssociations 'Network','Network','ConnectedTo','',1,1|18March2015");

            databaseVersions.Add(new Guid("F85E84DA-6CEE-41DA-98AD-5BCE0C7082FD"), "EXEC db_AddClasses 'NetworkConnectionDescription','(!string.IsNullOrEmpty(ConnectionType) && !string.IsNullOrEmpty(ConnectionSize) && !string.IsNullOrEmpty(ConnectionSpeed)) ? ConnectionType + \" \" + ConnectionSpeed + \" \" + ConnectionSize : (!string.IsNullOrEmpty(ConnectionType) && !string.IsNullOrEmpty(ConnectionSpeed)) ? ConnectionType + \" \" + ConnectionSpeed : (!string.IsNullOrEmpty(ConnectionType) && !string.IsNullOrEmpty(ConnectionSize)) ? ConnectionType + \" \" + ConnectionSize : (!string.IsNullOrEmpty(ConnectionSize) && !string.IsNullOrEmpty(ConnectionSpeed)) ? ConnectionSize + \" \" + ConnectionSpeed : (!string.IsNullOrEmpty(ConnectionType)) ? ConnectionType : (!string.IsNullOrEmpty(ConnectionSpeed)) ? ConnectionSpeed : (!string.IsNullOrEmpty(ConnectionSize)) ? ConnectionSize : \"\"','Network',1|18March2015");
            databaseVersions.Add(new Guid("DB8ED7A5-68D5-48D1-BF39-3D3D08B6A3FE"), "EXEC db_addFields 'NetworkConnectionDescription','ConnectionType','ConnectionType','General','Description',1,1,1|18March2015");
            databaseVersions.Add(new Guid("F4106817-8575-4EA3-B854-622FECAFC6AC"), "EXEC db_addFields 'NetworkConnectionDescription','ConnectionSize','System.String','General','Description',1,2,1|18March2015");
            databaseVersions.Add(new Guid("86CC8011-2ED1-4D69-9CE5-ABFCF07E60CE"), "EXEC db_addFields 'NetworkConnectionDescription','ConnectionSpeed','System.String','General','Description',1,3,1|18March2015");

            databaseVersions.Add(new Guid("3B9D69B0-308D-41F4-BC97-DAD4FF4743D2"), "EXEC db_AddArtifacts 'StorageComponent','StorageComponent','ConnectedTo','NetworkConnectionDescription',1|18March2015");
            databaseVersions.Add(new Guid("91C75FCB-7754-43B0-B925-479546FBDAB0"), "EXEC db_AddArtifacts 'StorageComponent','NetworkComponent','ConnectedTo','NetworkConnectionDescription',1|18March2015");
            databaseVersions.Add(new Guid("4AD21BBE-ACBE-4CD7-88CD-807E787BEA8D"), "EXEC db_AddArtifacts 'NetworkComponent','NetworkComponent','ConnectedTo','NetworkConnectionDescription',1|18March2015");
            databaseVersions.Add(new Guid("BF24F635-A100-4CF2-945A-889A9320C295"), "EXEC db_AddArtifacts 'NetworkComponent','StorageComponent','ConnectedTo','NetworkConnectionDescription',1|18March2015");
            databaseVersions.Add(new Guid("F809B2E5-57BB-4F29-8A34-32C771BF7D31"), "EXEC db_AddArtifacts 'ComputingComponent','NetworkComponent','ConnectedTo','NetworkConnectionDescription',1|18March2015");
            databaseVersions.Add(new Guid("86755B99-9C36-4F53-A15A-92A0327B0D38"), "EXEC db_AddArtifacts 'PeripheralComponent','StorageComponent','ConnectedTo','NetworkConnectionDescription',1|18March2015");
            databaseVersions.Add(new Guid("C10F5C8E-E693-495E-A826-2FFC0282490E"), "EXEC db_AddArtifacts 'PeripheralComponent','ComputingComponent','ConnectedTo','NetworkConnectionDescription',1|18March2015");
            databaseVersions.Add(new Guid("71C6BBC2-C4E5-4AF3-BDDF-029A7B144199"), "EXEC db_AddArtifacts 'ComputingComponent','StorageComponent','ConnectedTo','NetworkConnectionDescription',1|18March2015");
            databaseVersions.Add(new Guid("7DDF042D-C9EF-4910-B079-4904BF00BB36"), "EXEC db_AddArtifacts 'StorageComponent','PeripheralComponent','ConnectedTo','NetworkConnectionDescription',1|18March2015");
            databaseVersions.Add(new Guid("4B1A4998-1882-4478-A3A1-CEDF59F4A1C4"), "EXEC db_AddArtifacts 'NetworkComponent','ComputingComponent','ConnectedTo','NetworkConnectionDescription',1|18March2015");
            databaseVersions.Add(new Guid("CAD9C5BC-E858-44FF-9EB7-91ECA52ABEC1"), "EXEC db_AddArtifacts 'ComputingComponent','PeripheralComponent','ConnectedTo','NetworkConnectionDescription',1|18March2015");
            databaseVersions.Add(new Guid("B965BB65-733B-46A2-A119-7B47351B538C"), "EXEC db_AddArtifacts 'ComputingComponent','ComputingComponent','ConnectedTo','NetworkConnectionDescription',1|18March2015");
            databaseVersions.Add(new Guid("48E7AE1B-6284-4EE3-9F69-2BB1B58B8D5C"), "EXEC db_AddArtifacts 'PeripheralComponent','PeripheralComponent','ConnectedTo','NetworkConnectionDescription',1|18March2015");
            databaseVersions.Add(new Guid("47DCF9F9-B1FD-45FB-8107-0F361735603D"), "EXEC db_AddArtifacts 'StorageComponent','ComputingComponent','ConnectedTo','NetworkConnectionDescription',1|18March2015");
            databaseVersions.Add(new Guid("2111F3B1-79E7-4154-92BC-29B67DFB1D4C"), "EXEC db_AddArtifacts 'NetworkComponent','PeripheralComponent','ConnectedTo','NetworkConnectionDescription',1|18March2015");
            databaseVersions.Add(new Guid("669D32A5-67CF-4FAF-8AB3-D7F1682CF315"), "EXEC db_AddArtifacts 'PeripheralComponent','NetworkComponent','ConnectedTo','NetworkConnectionDescription',1|18March2015");
            databaseVersions.Add(new Guid("6E20AA9E-CDCF-47A4-9C68-F21B7B552F60"), "EXEC db_AddArtifacts 'Network','NetworkComponent','ConnectedTo','NetworkConnectionDescription',1|18March2015");
            databaseVersions.Add(new Guid("16BAB19C-6AD4-4F23-8BAA-D8015B3A2DD2"), "EXEC db_AddArtifacts 'NetworkComponent','Network','ConnectedTo','NetworkConnectionDescription',1|18March2015");
            databaseVersions.Add(new Guid("970F3A05-093C-45E0-9BC1-2E3F3AA3A301"), "EXEC db_AddArtifacts 'Network','ComputingComponent','ConnectedTo','NetworkConnectionDescription',1|18March2015");
            databaseVersions.Add(new Guid("7B7EE26D-A463-4BD3-B3B0-0D38B6C063AC"), "EXEC db_AddArtifacts 'ComputingComponent','Network','ConnectedTo','NetworkConnectionDescription',1|18March2015");
            databaseVersions.Add(new Guid("53086D4F-B5CD-4AC9-A897-E15C0AA87FB6"), "EXEC db_AddArtifacts 'Network','StorageComponent','ConnectedTo','NetworkConnectionDescription',1|18March2015");
            databaseVersions.Add(new Guid("C054AE6B-F778-4B7B-B5D9-5DCE799A2F35"), "EXEC db_AddArtifacts 'Network','PeripheralComponent','ConnectedTo','NetworkConnectionDescription',1|18March2015");
            databaseVersions.Add(new Guid("AAD184E4-5E88-43F6-A1D9-B4D158CC4EEC"), "EXEC db_AddArtifacts 'PeripheralComponent','Network','ConnectedTo','NetworkConnectionDescription',1|18March2015");
            databaseVersions.Add(new Guid("A3290911-BA08-4AE3-84FF-25F4973AD758"), "EXEC db_AddArtifacts 'StorageComponent','Network','ConnectedTo','NetworkConnectionDescription',1|18March2015");
            databaseVersions.Add(new Guid("82528821-D776-447A-B9E8-E139DE996A92"), "EXEC db_AddArtifacts 'Network','Network','ConnectedTo','NetworkConnectionDescription',1|18March2015");

            databaseVersions.Add(new Guid("986533E0-5C29-4C0A-86C8-3E99580B9B2F"), "REBUILDVIEWS|REBUILDVIEWS");

            databaseVersions.Add(new Guid("33417A9E-488D-46BE-8EFA-F1CD4121B5CA"), "IF NOT EXISTS (select * from sys.columns where object_id = Object_ID(N'[dbo].[Class]') and name = 'Uri_ID') Alter Table Class Add Uri_ID int null|TableURI");

            databaseVersions.Add(new Guid("66DCBA09-BECF-4749-9F2C-02237F2303EB"), "EXEC db_AddClassAssociations 'Network','Location','Mapping','',1,1|19March2015");
            databaseVersions.Add(new Guid("BF231D9B-DE71-4AE4-860D-B8DBD834694D"), "EXEC db_AddClassAssociations 'Network','NetworkComponent','Mapping','',1,1|19March2015");
            databaseVersions.Add(new Guid("973E3D2B-332A-4F64-9367-07EC10F01808"), "EXEC db_AddClassAssociations 'Sys','NetworkComponent','Mapping','',1,1|19March2015");

            databaseVersions.Add(new Guid("B858F749-A27C-4B27-80FC-98D5119C57D1"), "EXEC db_AddClassAssociations 'DataEntity','Stakeholder','Mapping','',1,1|RoneAddedAssociation");
            databaseVersions.Add(new Guid("BBC45327-9993-4407-8A0B-AE32B318ED60"), "EXEC db_AddClassAssociations 'DataEntity','Network','Mapping','',1,1|RoneAddedAssociation");
            databaseVersions.Add(new Guid("8AEC4B35-90EA-4B79-B00B-C2CFA145F665"), "EXEC db_AddClassAssociations 'DataEntity','ITInfrastructureEnvironment','Mapping','',1,1|RoneAddedAssociation");
            databaseVersions.Add(new Guid("3F406356-15CC-4826-8FEE-C7E6811A6D10"), "EXEC db_AddClassAssociations 'EnvironmentCategory','GovernanceMechanism','Mapping','',1,1|RoneAddedAssociation");
            databaseVersions.Add(new Guid("8C93F118-3EE0-4392-AE15-7BE0F72639E4"), "EXEC db_AddClassAssociations 'GovernanceMechanism','Driver','Mapping','',1,1|RoneAddedAssociation");
            databaseVersions.Add(new Guid("AC47A708-B07C-43AE-BBC7-7E121F6D0168"), "EXEC db_AddClassAssociations 'Risk','Implication','Mapping','',1,1|RoneAddedAssociation");

            #region DefaultClassImage

            databaseVersions.Add(new Guid("AD414BD1-D923-4740-BC37-913688B28374"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\WorkPackage.png')|AddClassImage");
            databaseVersions.Add(new Guid("4FCD1C47-0D61-4E9E-84F1-E94498F8FAB1"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\WorkPackage.png') WHERE Name = 'WorkPackage'|BindClassImage");
            databaseVersions.Add(new Guid("8CFF7F45-7CEA-4B67-9F20-2AF551F53987"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\TimeUnit.png')|AddClassImage");
            databaseVersions.Add(new Guid("2E4DFAE3-F44E-4991-A66F-9D25EEDF266B"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\TimeUnit.png') WHERE Name = 'TimeUnit'|BindClassImage");
            databaseVersions.Add(new Guid("7CBC8BD8-0827-4241-804B-BC4837E7D2AB"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\TimeScheme.png')|AddClassImage");
            databaseVersions.Add(new Guid("0F31B4D3-30A8-492B-81E6-783259A821BC"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\TimeScheme.png') WHERE Name = 'TimeScheme'|BindClassImage");
            databaseVersions.Add(new Guid("B81C215E-EA3A-4E5E-8F1E-CCB83DD266F0"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\TimeReference.png')|AddClassImage");
            databaseVersions.Add(new Guid("EB3CF51E-D461-429D-9475-FA0296010D4D"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\TimeReference.png') WHERE Name = 'TimeReference'|BindClassImage");
            databaseVersions.Add(new Guid("456ACAD9-9F4D-41C1-BC2D-8F6425F1CB59"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Sys.png')|AddClassImage");
            databaseVersions.Add(new Guid("C33E5541-8CDC-44A4-A046-64B89AA50D69"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\Sys.png') WHERE Name = 'Sys'|BindClassImage");
            databaseVersions.Add(new Guid("9B34DB6E-5E8B-4670-AAFF-500169D521C1"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\StrategicTheme.png')|AddClassImage");
            databaseVersions.Add(new Guid("4B0610AF-6FC3-4601-8A9A-9A1534DF8369"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\StrategicTheme.png') WHERE Name = 'StrategicTheme'|BindClassImage");
            databaseVersions.Add(new Guid("F3B538DB-3598-4D8E-94F3-75F0AAAEA520"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\StrategicStatement.png')|AddClassImage");
            databaseVersions.Add(new Guid("9BD3EEA1-6FC0-47AA-B2AD-79FD403FBFD4"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\StrategicStatement.png') WHERE Name = 'StrategicStatement'|BindClassImage");
            databaseVersions.Add(new Guid("1E2C94B5-B7BA-4C21-85FC-ECF92C24D20D"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\StorageComponent.png')|AddClassImage");
            databaseVersions.Add(new Guid("D85EA993-D86B-4C1D-8F38-66E12E0644D7"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\StorageComponent.png') WHERE Name = 'StorageComponent'|BindClassImage");
            databaseVersions.Add(new Guid("E8E3BA65-3FDD-4F50-A535-33362E2A5522"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Stakeholder.png')|AddClassImage");
            databaseVersions.Add(new Guid("B833B288-4D83-4609-958F-DC3365959AE2"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\Stakeholder.png') WHERE Name = 'Stakeholder'|BindClassImage");
            databaseVersions.Add(new Guid("FB83057F-E16C-4889-A075-A80D46CC9344"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Scenario.png')|AddClassImage");
            databaseVersions.Add(new Guid("763E4219-66B2-4983-B38F-613F9B577266"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\Scenario.png') WHERE Name = 'Scenario'|BindClassImage");
            databaseVersions.Add(new Guid("BD67C4EE-2283-4272-B556-F0B832F350EE"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Role.png')|AddClassImage");
            databaseVersions.Add(new Guid("2DA576AE-78B0-4A08-8F08-C36731DDB217"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\Role.png') WHERE Name = 'Role'|BindClassImage");
            databaseVersions.Add(new Guid("3CEF283A-A89F-4E72-A116-BE6A76A666F8"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Risk.png')|AddClassImage");
            databaseVersions.Add(new Guid("74AD8250-34EA-49B5-B75B-DDB01E3B4222"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\Risk.png') WHERE Name = 'Risk'|BindClassImage");
            databaseVersions.Add(new Guid("E0471BF7-8CD8-402C-B168-06790413AB50"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Resource.png')|AddClassImage");
            databaseVersions.Add(new Guid("8B8F76A9-82E5-4C37-AB22-18AE49338778"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\Resource.png') WHERE Name = 'Resource'|BindClassImage");
            databaseVersions.Add(new Guid("B5B98882-3A47-4794-8218-2768B4085F57"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Requirement.png')|AddClassImage");
            databaseVersions.Add(new Guid("F066DEFD-EDD0-433C-8353-1E44A29F0B9F"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\Requirement.png') WHERE Name = 'Requirement'|BindClassImage");
            databaseVersions.Add(new Guid("CEAF7908-4AB0-46BF-8F35-AA151906F78F"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Process.png')|AddClassImage");
            databaseVersions.Add(new Guid("5205D4ED-150E-4779-8828-F45C43343027"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\Process.png') WHERE Name = 'Process'|BindClassImage");
            databaseVersions.Add(new Guid("BEC5B175-D3CE-46E4-9EF0-945EEC5E3A2E"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Position.png')|AddClassImage");
            databaseVersions.Add(new Guid("FB01E6B8-F22D-4819-B361-CFA8D4F62DC2"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\Position.png') WHERE Name = 'Position'|BindClassImage");
            databaseVersions.Add(new Guid("AA3EB475-BB3E-48AB-8608-289DD8C2557B"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\PhysicalSoftwareComponent.png')|AddClassImage");
            databaseVersions.Add(new Guid("D29A4229-78F7-4DCC-8D5F-1FA8CE5796AA"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\PhysicalSoftwareComponent.png') WHERE Name = 'PhysicalSoftwareComponent'|BindClassImage");
            databaseVersions.Add(new Guid("5D95B0F7-86DE-4825-BD22-6DE478A0926C"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\PhysicalInformationArtefact.png')|AddClassImage");
            databaseVersions.Add(new Guid("2BBFBD7B-E9E7-4CF1-83DE-D5CCFFBB12C7"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\PhysicalInformationArtefact.png') WHERE Name = 'PhysicalInformationArtefact'|BindClassImage");
            databaseVersions.Add(new Guid("581679BC-AFF8-4853-9E7D-4681025C559B"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\PhysicalDataComponent.png')|AddClassImage");
            databaseVersions.Add(new Guid("FBCCA99F-06D3-42A3-AFCC-AA267735AB78"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\PhysicalDataComponent.png') WHERE Name = 'PhysicalDataComponent'|BindClassImage");
            databaseVersions.Add(new Guid("4B19B2EF-3E25-4FC2-BE36-26E487A6ED8F"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Person.png')|AddClassImage");
            databaseVersions.Add(new Guid("BEA2EC14-6D67-42BB-917F-D0E8DF4FEDA8"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\Person.png') WHERE Name = 'Person'|BindClassImage");
            databaseVersions.Add(new Guid("C24954BD-3125-44E3-A80F-0D83449A33CB"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\PeripheralComponent.png')|AddClassImage");
            databaseVersions.Add(new Guid("EB831CD6-BB7A-44EA-9D3A-5D6649B7A723"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\PeripheralComponent.png') WHERE Name = 'PeripheralComponent'|BindClassImage");
            databaseVersions.Add(new Guid("61087936-D70C-47AD-BD32-A8472AD129AC"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\OrganizationalUnit.png')|AddClassImage");
            databaseVersions.Add(new Guid("A16A8033-95B4-4563-A376-4388B4DB31E3"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\OrganizationalUnit.png') WHERE Name = 'OrganizationalUnit'|BindClassImage");
            databaseVersions.Add(new Guid("280FA337-1E66-4F62-9A1A-5A88D0C98B24"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Object.png')|AddClassImage");
            databaseVersions.Add(new Guid("EB83FAE4-2577-4C6E-9B05-390B2315955F"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\Object.png') WHERE Name = 'Object'|BindClassImage");
            databaseVersions.Add(new Guid("2B37A9B1-8138-4075-AED5-56A6044B05BF"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\NetworkComponent.png')|AddClassImage");
            databaseVersions.Add(new Guid("EE5AD066-0E56-4C7C-BA57-573D2306BB92"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\NetworkComponent.png') WHERE Name = 'NetworkComponent'|BindClassImage");
            databaseVersions.Add(new Guid("D5FEBC1D-83F2-46A7-969C-C3FD2AAD3FDE"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Network (1).png')|AddClassImage");
            databaseVersions.Add(new Guid("899BF473-8584-4F70-97BA-06BC6B199F08"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\Network (1).png') WHERE Name = 'Network'|BindClassImage");
            databaseVersions.Add(new Guid("183D17A6-8491-4728-A842-630B8FC083FC"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Measure.png')|AddClassImage");
            databaseVersions.Add(new Guid("DBA3D42C-EE06-4967-9D05-4B7CAC49177C"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\Measure.png') WHERE Name = 'Measure'|BindClassImage");
            databaseVersions.Add(new Guid("BDE521E1-3FA9-45DA-9E71-6EC8DB1EED23"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\MarketSegment.png')|AddClassImage");
            databaseVersions.Add(new Guid("5AAFFE20-9FED-4FDB-A940-CCD7C14E5C43"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\MarketSegment.png') WHERE Name = 'MarketSegment'|BindClassImage");
            databaseVersions.Add(new Guid("A023647D-1C6A-468A-9E3E-CF26C1B43F97"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\LogicalSoftwareComponentpng.png')|AddClassImage");
            databaseVersions.Add(new Guid("429F57ED-7F8B-43CB-8887-F96664296349"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\LogicalSoftwareComponentpng.png') WHERE Name = 'LogicalSoftwareComponentpng'|BindClassImage");
            databaseVersions.Add(new Guid("F33CAB31-E7A4-43A7-80A3-950F40A4BA49"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\LogicalITInfrastructureComponent.png')|AddClassImage");
            databaseVersions.Add(new Guid("95C0F402-33C4-4C65-B31B-7EEBC6C159BD"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\LogicalITInfrastructureComponent.png') WHERE Name = 'LogicalITInfrastructureComponent'|BindClassImage");
            databaseVersions.Add(new Guid("4E0AAC5B-4716-4777-BDE3-A5E2DF406AA2"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\LogicalInformationArtefact.png')|AddClassImage");
            databaseVersions.Add(new Guid("B9344B99-1935-4B26-BDA4-D8C113753C74"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\LogicalInformationArtefact.png') WHERE Name = 'LogicalInformationArtefact'|BindClassImage");
            databaseVersions.Add(new Guid("473EBA11-7B50-447A-9BE2-F78847949208"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\LocationUnit.png')|AddClassImage");
            databaseVersions.Add(new Guid("60F7206D-9335-4221-AD2C-6691140C0BED"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\LocationUnit.png') WHERE Name = 'LocationUnit'|BindClassImage");
            databaseVersions.Add(new Guid("8AE14E0B-2D21-4517-AD4D-B9ED703B641E"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\LocationScheme.png')|AddClassImage");
            databaseVersions.Add(new Guid("3EFF7C69-DE47-441A-A81D-703FB63C932E"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\LocationScheme.png') WHERE Name = 'LocationScheme'|BindClassImage");
            databaseVersions.Add(new Guid("D7AD5013-F062-469A-9C5D-3CDCE10D10C0"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Location.png')|AddClassImage");
            databaseVersions.Add(new Guid("04F74330-E233-4DDC-9DD3-C3EF51FA434B"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\Location.png') WHERE Name = 'Location'|BindClassImage");
            databaseVersions.Add(new Guid("CC2441ED-79B6-43CE-B6FF-8910F1C7B83B"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\JobCompetency.png')|AddClassImage");
            databaseVersions.Add(new Guid("954549EC-58C6-4A18-9AAD-69E1F2213CF5"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\JobCompetency.png') WHERE Name = 'JobCompetency'|BindClassImage");
            databaseVersions.Add(new Guid("8B0949D9-B0B9-4550-BA7E-F949CE6D3B11"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Job.png')|AddClassImage");
            databaseVersions.Add(new Guid("96876B6B-3AC2-4B7F-816C-1B597319F746"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\Job.png') WHERE Name = 'Job'|BindClassImage");
            databaseVersions.Add(new Guid("31AD3CBE-3D63-46D0-AA82-869D51CF8824"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\ITInfrastructureEnvironment.png')|AddClassImage");
            databaseVersions.Add(new Guid("1B670360-15CC-4833-9BB3-D4015750AB1A"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\ITInfrastructureEnvironment.png') WHERE Name = 'ITInfrastructureEnvironment'|BindClassImage");
            databaseVersions.Add(new Guid("64452501-95A9-4048-AD13-67D4DB7FDD2F"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Implication.png')|AddClassImage");
            databaseVersions.Add(new Guid("5DFCCF56-3896-4617-86B8-40666BF51733"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\Implication.png') WHERE Name = 'Implication'|BindClassImage");
            databaseVersions.Add(new Guid("E63542CB-294A-43A0-90CD-2C4464538718"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\GovernanceMechanism.png')|AddClassImage");
            databaseVersions.Add(new Guid("0FC78BBD-50A8-4289-AF47-22C4AD55DE40"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\GovernanceMechanism.png') WHERE Name = 'GovernanceMechanism'|BindClassImage");
            databaseVersions.Add(new Guid("61547EC3-D0DA-4839-978C-D688374B9BC6"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Gateway.png')|AddClassImage");
            databaseVersions.Add(new Guid("1A4B273A-FA38-4C30-A4FD-3C14E50FE373"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\Gateway.png') WHERE Name = 'Gateway'|BindClassImage");
            databaseVersions.Add(new Guid("AE5FD1C7-B07B-4E48-AABF-C1092E9C3C99"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Function.png')|AddClassImage");
            databaseVersions.Add(new Guid("47083B3B-E13C-4EE4-9B2A-7DAD41BE81A9"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\Function.png') WHERE Name = 'Function'|BindClassImage");
            databaseVersions.Add(new Guid("1879FD39-94C1-4867-903A-6EED605E7AA2"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Event.png')|AddClassImage");
            databaseVersions.Add(new Guid("3A2391C9-48A9-4861-8725-79B5D14C4101"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\Event.png') WHERE Name = 'Event'|BindClassImage");
            databaseVersions.Add(new Guid("38C70D4E-7F70-4C6D-8C6B-4762BFC8ECA3"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\EnvironmentCategory.png')|AddClassImage");
            databaseVersions.Add(new Guid("2A6C6319-C974-44B2-AAF8-1F0A2481C5D3"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\EnvironmentCategory.png') WHERE Name = 'EnvironmentCategory'|BindClassImage");
            databaseVersions.Add(new Guid("1430202B-2EF7-4514-BB5C-6ACCA764C47A"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Environment.png')|AddClassImage");
            databaseVersions.Add(new Guid("451F9807-DDDC-41B3-94B5-D929B2737493"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\Environment.png') WHERE Name = 'Environment'|BindClassImage");
            databaseVersions.Add(new Guid("99799DA2-9729-4856-BECC-ECDFCC101EC2"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Driver.png')|AddClassImage");
            databaseVersions.Add(new Guid("12C71E78-73B6-4210-93DB-8AE8FD89D049"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\Driver.png') WHERE Name = 'Driver'|BindClassImage");
            databaseVersions.Add(new Guid("05E0A95D-55D3-435C-84D5-DD3A998CAA22"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\DataValue.png')|AddClassImage");
            databaseVersions.Add(new Guid("481B3695-ECA4-4BD2-8323-C32785CFECFE"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\DataValue.png') WHERE Name = 'DataValue'|BindClassImage");
            databaseVersions.Add(new Guid("B28D8E9D-8C75-4B98-B418-A451B21E7EF9"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\DataTable.png')|AddClassImage");
            databaseVersions.Add(new Guid("5C1BA8B4-C7DA-407A-8549-62D5BC3A08AE"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\DataTable.png') WHERE Name = 'DataTable'|BindClassImage");
            databaseVersions.Add(new Guid("59A739E9-E70F-4131-BDFE-C7236FA3BD21"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\DataSubjectArea.png')|AddClassImage");
            databaseVersions.Add(new Guid("9BE0D857-792F-42A2-BE9C-A8265B64661B"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\DataSubjectArea.png') WHERE Name = 'DataSubjectArea'|BindClassImage");
            databaseVersions.Add(new Guid("0CFBF522-47A4-439E-B5A4-DC957ED7D2A6"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\DataDomain.png')|AddClassImage");
            databaseVersions.Add(new Guid("EB5B862B-6116-4DD7-A047-FC69AFD8EBB0"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\DataDomain.png') WHERE Name = 'DataDomain'|BindClassImage");
            databaseVersions.Add(new Guid("F50C0A5F-B5E6-4EFF-8D21-F0D4B1333526"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Constraint.png')|AddClassImage");
            databaseVersions.Add(new Guid("48AC6A95-B4B2-44E5-9D73-6123EE16DAF9"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\Constraint.png') WHERE Name = 'Constraint'|BindClassImage");
            databaseVersions.Add(new Guid("43725DDE-26CC-4476-B026-89DACCC79845"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\ComputingComponent.png')|AddClassImage");
            databaseVersions.Add(new Guid("E99EC589-53FF-46C7-9BAD-271C145A2DB4"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\ComputingComponent.png') WHERE Name = 'ComputingComponent'|BindClassImage");
            databaseVersions.Add(new Guid("69F23E4E-ABDC-47B8-B6BB-58BE93E3399B"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\BusinessInterface.png')|AddClassImage");
            databaseVersions.Add(new Guid("5888455E-6C78-4989-92FC-7D173EACA937"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\BusinessInterface.png') WHERE Name = 'BusinessInterface'|BindClassImage");
            databaseVersions.Add(new Guid("6970B1BB-2716-4BE8-BC62-5580134E6DC3"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Assumption.png')|AddClassImage");
            databaseVersions.Add(new Guid("66F4E9AB-090F-411C-82BE-FA61B3CF88A8"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\Assumption.png') WHERE Name = 'Assumption'|BindClassImage");
            databaseVersions.Add(new Guid("DA21F3EB-6D9D-4002-9710-00B1A40D50F1"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\ApplicationInterface.png')|AddClassImage");
            databaseVersions.Add(new Guid("539D6F38-6416-45B1-AF05-DC49CE28508F"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\ApplicationInterface.png') WHERE Name = 'ApplicationInterface'|BindClassImage");
            databaseVersions.Add(new Guid("0077F787-5BA6-439F-B7A6-B7794DE338E4"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Activity.png')|AddClassImage");
            databaseVersions.Add(new Guid("A8F72BE1-280F-4919-93E8-BDDB4484BBE1"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\Activity.png') WHERE Name = 'Activity'|BindClassImage");

            #endregion

            databaseVersions.Add(new Guid("9E6B845C-6454-4840-9596-9447576D3D4B"), "EXEC db_AddPossibleValues 'ObjectType','Capability',7,'Capability',1|AddObjectCapability");
            databaseVersions.Add(new Guid("5AB83C33-130B-4078-AA61-A6BA6EFE9F55"), "EXEC db_AddPossibleValues 'OrganizationalUnitType','Board',7,'Board',1|AddOrgBoard");

            databaseVersions.Add(new Guid("666F9B62-3F20-45BD-A55F-56AFF55DA44F"), "EXEC db_AddPossibleValues 'SoftwareType','AppMod',12,'AppMod',1|SAPPI");
            databaseVersions.Add(new Guid("D3275EE6-A9BC-4F39-872E-17779FB70E79"), "EXEC db_AddPossibleValues 'SoftwareType','Suite',12,'Suite',1|SAPPI");
            databaseVersions.Add(new Guid("97F866FA-2BF2-454E-99B6-CA91543F2901"), "EXEC db_addFields 'PhysicalSoftwareComponent','SoftwareLevel','SoftwareLevel','General','',1,9,0|SAPPI");
            databaseVersions.Add(new Guid("E1D3BC15-F2DA-4BC4-BEDF-484DDF6317B5"), "update objectfieldvalue set valuestring = 'PhysicalServer' where fieldid in (select pkid from field where datatype = 'computingcomponenttype') and valuestring = 'PhysServer'|SAPPI");
            databaseVersions.Add(new Guid("81BABA70-5D32-4ADA-86C2-2BDFF79BD464"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = (SELECT Pkid from uuri WHERE FileName LIKE '%PhysicalServer.png%') WHERE PossibleValue = 'PhysicalServer'|SAPPI");
            databaseVersions.Add(new Guid("6F5C152E-0606-4FFD-8ACC-46BDE20F8FD7"), "REMAP|REMAP");

            databaseVersions.Add(new Guid("36891BB7-F269-4632-B7D7-1796470E50C3"), "UPDATE DomainDefinitionPossibleValue Set PossibleValue = 'Development', [Description] = 'Development' WHERE PossibleValue = 'Dev'|SAPPI");
            databaseVersions.Add(new Guid("FDECEF18-3108-426C-8718-B53D4054CA2C"), "UPDATE DomainDefinitionPossibleValue Set PossibleValue = 'DisasterRecovery', [Description] = 'DisasterRecovery' WHERE PossibleValue = 'DR'|SAPPI");
            databaseVersions.Add(new Guid("92A58714-DCAA-4E36-80E4-DFA71244663D"), "UPDATE DomainDefinitionPossibleValue Set PossibleValue = 'Production', [Description] = 'Production' WHERE PossibleValue = 'Prod'|SAPPI");
            databaseVersions.Add(new Guid("982CD6DB-E265-4850-8CBC-D77A7C169242"), "UPDATE DomainDefinitionPossibleValue Set PossibleValue = 'SystemTest', [Description] = 'SystemTest' WHERE PossibleValue = 'SystemTest'|SAPPI");
            databaseVersions.Add(new Guid("A8AEEFC0-6F9F-4843-9F94-818BD02A8D13"), "UPDATE DomainDefinitionPossibleValue Set PossibleValue = 'UserAcceptanceTesting', [Description] = 'UserAcceptanceTesting' WHERE PossibleValue = 'UAT'|SAPPI");

            databaseVersions.Add(new Guid("4A77947D-4C8A-4892-B6A8-EB38C52F4DFA"), "UPDATE Class SET IsActive = 0 WHERE Name = 'Employee'|SAPPI");
            databaseVersions.Add(new Guid("8591063C-0799-457B-BE30-C5764DFCBBCB"), "UPDATE Field SET IsActive = 0 WHERE Class = 'Employee'|SAPPI");

            databaseVersions.Add(new Guid("76F0FC37-64B5-4299-9828-13A8EE78899A"), "UPDATE Class SET IsActive = 0 WHERE Name = 'CSF'|SAPPI");
            databaseVersions.Add(new Guid("6CD38A47-F965-4F9E-8440-7D6BC12DA0D3"), "UPDATE Field SET IsActive = 0 WHERE Class = 'CSF'|SAPPI");

            databaseVersions.Add(new Guid("4B7ED5A9-E438-4C87-B992-08DB66189D99"), "UPDATE Class SET IsActive = 0 WHERE Name = 'Task'|SAPPI");
            databaseVersions.Add(new Guid("F2A0A5E8-FECE-4B21-B7B7-4B9DDD63AD83"), "UPDATE Field SET IsActive = 0 WHERE Class = 'Task'|SAPPI");

            databaseVersions.Add(new Guid("2EF3146F-D4B6-46C1-AB5A-194DC722407B"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\PhysicalSoftwareComponent.png') WHERE Name = 'LogicalSoftwareComponent'|BindClassImage");

            databaseVersions.Add(new Guid("38736770-6BEB-4272-B5D1-642424A09DCD"), "UPDATE Class SET URI_ID = (select top 1 pkid from uuri where filename = '" + ImagesPath + "\\Resource.png') WHERE Name = 'ResourceType'|BindClassImage");

            databaseVersions.Add(new Guid("60739E47-04B5-43DA-985D-CF6C395BF473"), "UPDATE Field SET DataType = 'ResourceType' WHERE Class = 'Resource' AND Name = 'ResourceType'|Resource");
            databaseVersions.Add(new Guid("330ABF86-4805-487D-99B8-D9052DD03FF7"), "UPDATE Field SET DataType = 'ResourceType' WHERE Class = 'ResourceType' AND Name = 'Name'|Resource");
            //add description field : ResourceType
            databaseVersions.Add(new Guid("5FF3FF08-22F0-4539-B70B-7AA053B82C84"), "EXEC db_addFields 'ResourceType','ResourceAvailabilityRatingDescription','System.String','General','',1,5,1|Resource");
            //add description field : ResourceAvailability
            databaseVersions.Add(new Guid("70C3F7E6-1BD4-4B90-8533-41702F6DF64B"), "EXEC db_AddClasses 'ResourceAvailability','(!string.IsNullOrEmpty(ResourceAvailabilityRating) && !string.IsNullOrEmpty(ResourceAvailabilityRatingDescription)) ? ResourceAvailabilityRating + \" \" + ResourceAvailabilityRatingDescription : (!string.IsNullOrEmpty(ResourceAvailabilityRating)) ? ResourceAvailabilityRating : (!string.IsNullOrEmpty(ResourceAvailabilityRatingDescription)) ? ResourceAvailabilityRatingDescription : \"\"','System',1|Resource");
            databaseVersions.Add(new Guid("5AA65A8B-0122-41B8-A577-C5C949820551"), "EXEC db_addFields 'ResourceAvailability','ResourceAvailabilityRating','System.String','General','',1,1,1|Resource");
            databaseVersions.Add(new Guid("D9788ECB-CCE1-4550-878F-C15300F5294C"), "EXEC db_addFields 'ResourceAvailability','ResourceAvailabilityRatingDescription','System.String','General','',1,2,1|Resource");
            databaseVersions.Add(new Guid("1365FF97-6D6E-4862-994C-4A967585AF6F"), "EXEC db_addFields 'ResourceAvailability','ResourceAvailabilityRatingDate','System.String','General','',1,3,1|Resource");

            databaseVersions.Add(new Guid("D514B26C-C429-4318-BB3D-30DE1BEF4AD7"), "EXEC db_AddArtifacts 'Function','ResourceType','Mapping','ResourceAvailability',1|Resource");
            databaseVersions.Add(new Guid("2BDFE22A-71AC-4F2C-BAF2-68E14C7F2AD2"), "EXEC db_AddArtifacts 'ResourceType','Function','Mapping','ResourceAvailability',1|Resource");

            #endregion

            #region 28 April 2015
            databaseVersions.Add(new Guid("8FF65FD6-352C-4571-A7B2-4DC3A63A7BA1"), "EXEC db_AddClassAssociations 'Process','OrganizationalUnit','DynamicFlow','',1,1|28April2015");
            databaseVersions.Add(new Guid("C1A8621F-E4F4-43FA-9165-CE2FD16EB00B"), "EXEC db_AddArtifacts 'Process','OrganizationalUnit','DynamicFlow','FlowDescription',1|28April2015");
            databaseVersions.Add(new Guid("F359EA43-D200-43F7-A16A-2EB579802351"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Committee.png')|28April2015");
            databaseVersions.Add(new Guid("4D481886-9A3C-480E-BA83-9FFCA865D9E8"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = (select top 1 pkid from uuri where filename LIKE '%Committee.png') WHERE PossibleValue = 'Committee' AND domaindefinitionid = (select pkid from domaindefinition where name like '%OrganizationalUnitType%')|28April2015");
            databaseVersions.Add(new Guid("7279D4E9-A334-4680-854C-D49A6B493CA4"), "EXEC db_AddClasses 'Automation','this.AutomationType','Strategy',1|28April2015");
            databaseVersions.Add(new Guid("52C0D8C5-A246-43DE-AE12-A3CF59A6799D"), "EXEC db_addFields 'Automation','AutomationType','AutomationType','General','',1,1,1|28April2015");
            databaseVersions.Add(new Guid("481775E9-6541-47B0-B4F3-1E6FD04088BC"), "EXEC db_AddPossibleValues 'AutomationType','Manual',1,'Manual',1|28April2015");
            databaseVersions.Add(new Guid("43612927-52C3-4ED2-ADA7-94AEC5BEEE09"), "EXEC db_AddPossibleValues 'AutomationType','Automated',2,'Automated',1|28April2015");
            databaseVersions.Add(new Guid("5B8266F0-9D96-42FF-B383-711C5FCD0F02"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\SLA.png')|28April2015");
            databaseVersions.Add(new Guid("8A25AC29-33B2-40F9-A973-DE0514EF73E5"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Control.png')|28April2015");
            databaseVersions.Add(new Guid("E88C3BE2-BB17-4234-9A95-007BE9861237"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = (select top 1 pkid from uuri where filename LIKE '%Control.png') WHERE PossibleValue = 'Control' AND domaindefinitionid = (select pkid from domaindefinition where name like '%GovernanceMechanismType%')|28April2015");
            databaseVersions.Add(new Guid("23BE0122-A279-4B07-B6BC-FA9C7B515695"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = (select top 1 pkid from uuri where filename LIKE '%SLA.png') WHERE PossibleValue = 'SLA' AND domaindefinitionid = (select pkid from domaindefinition where name like '%GovernanceMechanismType%')|28April2015");
            databaseVersions.Add(new Guid("B14E6F4F-0067-4833-BEE7-669C10B37887"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\ExternalEntity.png')|28April2015");
            databaseVersions.Add(new Guid("977D3CF7-7CC8-4001-AC94-904FD9A2C28E"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\RiskModel.png')|28April2015");
            databaseVersions.Add(new Guid("532EDC7B-0C69-447D-84A8-7126F866F7C7"), "EXEC db_AddPossibleValues 'ObjectType','RiskModel',2,'RiskModel',1|28April2015");
            databaseVersions.Add(new Guid("BBAAEF98-1D2C-47D0-8B81-C0352CCB7A08"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = (select top 1 pkid from uuri where filename LIKE '%ExternalEntity.png') WHERE PossibleValue = 'ExternalEntity' AND domaindefinitionid = (select pkid from domaindefinition where name like '%ObjectType%')|28April2015");
            databaseVersions.Add(new Guid("E4CB5FD0-B869-4B8D-9F84-BE98C74A9544"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = (select top 1 pkid from uuri where filename LIKE '%RiskModel.png') WHERE PossibleValue = 'RiskModel' AND domaindefinitionid = (select pkid from domaindefinition where name like '%ObjectType%')|28April2015");
            databaseVersions.Add(new Guid("053302FF-7E19-459B-9ADF-7E87B7FD2D1E"), "EXEC db_AddPossibleValues 'SoftwareType','Engine',2,'Engine',1|28April2015");
            databaseVersions.Add(new Guid("C575E5B4-0D1A-4C10-8539-7DB0FB83DC27"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Engine.png')|28April2015");
            databaseVersions.Add(new Guid("33C63F6A-A342-4E6D-91C8-FC28CD93DA8B"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = (select top 1 pkid from uuri where filename LIKE '%Engine.png') WHERE PossibleValue = 'Engine' AND domaindefinitionid = (select pkid from domaindefinition where name like '%SoftwareType%')|28April2015");
            databaseVersions.Add(new Guid("81583365-3312-4B3A-9BAD-3E0A3A91C3E6"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = (select top 1 pkid from uuri where filename LIKE '%Spreadsheet.png') WHERE PossibleValue = 'Spreadsheet' AND domaindefinitionid = (select pkid from domaindefinition where name like '%SoftwareType%')|28April2015");
            databaseVersions.Add(new Guid("763F4FC3-0C25-4B11-B01C-6616F77AD9B2"), "EXEC db_addFields 'PhysicalDataComponent','DataUpdateStatus','DataUpdateStatusType','General','',1,100,1|28April2015");
            databaseVersions.Add(new Guid("01D09715-B826-4034-A29A-072140E38205"), "EXEC db_AddPossibleValues 'DataUpdateStatusType','ToBeReviewed',1,'ToBeReviewed',1|28April2015");
            databaseVersions.Add(new Guid("F7DF17CC-AB2E-46F5-8625-C427F7F6298F"), "EXEC db_AddPossibleValues 'DataUpdateStatusType','Reviewed',2,'Reviewed',1|28April2015");
            databaseVersions.Add(new Guid("E7397953-05B5-4CBB-9FB4-D65E2CF83F98"), "EXEC db_AddPossibleValues 'IsRecord','Yes',1,'Yes',0|28April2015");
            databaseVersions.Add(new Guid("A3C27932-ADE2-4025-9C1F-1C1F1940E40A"), "EXEC db_AddPossibleValues 'IsRecord','No',2,'No',0|28April2015");
            databaseVersions.Add(new Guid("F7397FFD-16F9-451B-8F68-48101CF78EB5"), "EXEC db_AddPossibleValues 'IsRecord','Vital',1,'Vital',1|28April2015");
            databaseVersions.Add(new Guid("EBDCE304-573D-4659-ABE0-A187EB6AD917"), "EXEC db_AddPossibleValues 'IsRecord','Operational',2,'Operational',1|28April2015");
            databaseVersions.Add(new Guid("D96F4F2F-337B-4098-9797-0F9148F72D93"), "INSERT INTO UURI ([FileName]) VALUES ('" + ImagesPath + "\\Vital.png')|28April2015");
            databaseVersions.Add(new Guid("303C0EF2-9DBE-471A-9794-0246D0487BA0"), "UPDATE DomainDefinitionPossibleValue SET URI_ID = (select top 1 pkid from uuri where filename LIKE '%Vital.png') WHERE PossibleValue = 'Vital' AND domaindefinitionid = (select pkid from domaindefinition where name like '%IsRecord%')|28April2015");

            #region AutomationMappings
            databaseVersions.Add(new Guid("E179522B-4C55-4485-A537-59846FD45E65"), "EXEC db_AddArtifacts 'Implication','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("4046CBCF-19C3-4559-864B-20F0A5B3282C"), "EXEC db_AddArtifacts 'GovernanceMechanism','Implication','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("AB0EA103-7104-4A0B-BDC2-C38D3764C438"), "EXEC db_AddArtifacts 'Job','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("5CF839F6-1503-4E70-BB7F-AEFD28AA3DE7"), "EXEC db_AddArtifacts 'GovernanceMechanism','Job','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("54E6F01D-CE89-4FE4-ADE8-350A8FCAB30C"), "EXEC db_AddArtifacts 'Location','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("5DE13813-B0A5-46C9-B32B-24617E1B9D8F"), "EXEC db_AddArtifacts 'GovernanceMechanism','Location','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("B1D323BA-07AE-405A-A4E6-E982B3AAD765"), "EXEC db_AddArtifacts 'GovernanceMechanism','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("43EE029D-696D-412B-A6E7-E59EBFB31164"), "EXEC db_AddArtifacts 'GovernanceMechanism','DataSchema','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("3BDCB192-02E8-4784-B149-F09CCB5DFE32"), "EXEC db_AddArtifacts 'GovernanceMechanism','DataTable','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("4837C8F0-937B-42D0-AA78-38433660B15A"), "EXEC db_AddArtifacts 'DataTable','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("E069C16A-E774-4350-B804-2D87B23C94CD"), "EXEC db_AddArtifacts 'GovernanceMechanism','Activity','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("969A2823-3359-4FB3-9691-6796ADF42322"), "EXEC db_AddArtifacts 'Activity','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("9260EF87-8D2C-4C27-B8FA-C31BB4C6CFA4"), "EXEC db_AddArtifacts 'GovernanceMechanism','Rationale','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("B963A7D0-A48F-4FF5-8CF0-1B04EFDFC01F"), "EXEC db_AddArtifacts 'Rationale','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("F0785224-E9AA-4DF5-BDC4-90A0136D066E"), "EXEC db_AddArtifacts 'GovernanceMechanism','Function','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("469C2227-63EF-4BBC-B9DF-3259DCA84E5B"), "EXEC db_AddArtifacts 'Function','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("F653DD2A-7B06-472C-89A8-23F6B528453D"), "EXEC db_AddArtifacts 'GovernanceMechanism','Object','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("616732BC-BBCE-42DC-8C14-169AC6767D1C"), "EXEC db_AddArtifacts 'Object','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("8CA284BF-0F75-46A4-941F-2FD14E8D85F5"), "EXEC db_AddArtifacts 'GovernanceMechanism','OrganizationalUnit','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("569ACB46-E7B5-403E-8923-204009861718"), "EXEC db_AddArtifacts 'OrganizationalUnit','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("B6E9F8DE-47A8-4FA7-93EC-255A83612FC7"), "EXEC db_AddArtifacts 'GovernanceMechanism','Process','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("B22391E2-5250-4D39-AC73-C65CD867E899"), "EXEC db_AddArtifacts 'Process','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("B8E8E3C7-B5EF-41A0-A3CE-EB603B87F799"), "EXEC db_AddArtifacts 'GovernanceMechanism','Scenario','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("880E371D-E896-46AD-AA05-AE81E6E73209"), "EXEC db_AddArtifacts 'Scenario','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("E7171B43-07AB-4848-8342-1A81AA0F157F"), "EXEC db_AddArtifacts 'GovernanceMechanism','Skill','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("9415B094-EB80-4402-8B88-7C90B73744F7"), "EXEC db_AddArtifacts 'Skill','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("2E7801A9-C6FB-4BD9-A075-38D1F455C1FE"), "EXEC db_AddArtifacts 'GovernanceMechanism','StorageComponent','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("2FC76BD2-9726-4525-96E6-6FC2A76968AA"), "EXEC db_AddArtifacts 'StorageComponent','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("5DADA2D0-B08E-44E2-BC36-71FAA14172F0"), "EXEC db_AddArtifacts 'GovernanceMechanism','StrategicTheme','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("8DD29E08-E93E-430E-A66C-153443CE3E1E"), "EXEC db_AddArtifacts 'StrategicTheme','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("78D2B272-EC38-4E2B-A5ED-463EB28D1AA1"), "EXEC db_AddArtifacts 'GovernanceMechanism','TimeScheme','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("180F3F63-963A-44FF-8E20-9CE8B8BA0453"), "EXEC db_AddArtifacts 'TimeScheme','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("DFA49F24-6830-4EA3-B160-FF01AB3B9AA9"), "EXEC db_AddArtifacts 'GovernanceMechanism','Role','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("E8B4B7A3-E635-4F9D-BB23-42A01E4CC78F"), "EXEC db_AddArtifacts 'Role','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("D400DF37-95DF-42C8-B5EF-962AB58F9C12"), "EXEC db_AddArtifacts 'GovernanceMechanism','NetworkComponent','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("FD49D2BE-0FCF-45EC-B12C-66D6DD8C6A96"), "EXEC db_AddArtifacts 'NetworkComponent','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("4E141D9A-CAE3-423E-A5AF-807ACB70638D"), "EXEC db_AddArtifacts 'GovernanceMechanism','TimeUnit','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("40ED3284-9C9F-4197-9DDD-A06BB048D200"), "EXEC db_AddArtifacts 'TimeUnit','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("FD455FEE-50B5-49F3-8D03-4D191F2FBB46"), "EXEC db_AddArtifacts 'PeripheralComponent','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("7919CF88-5CAB-470A-8921-ECDDCD6A8790"), "EXEC db_AddArtifacts 'GovernanceMechanism','PeripheralComponent','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("8E894ADE-2794-4C35-B4D7-B62CB750A2F8"), "EXEC db_AddArtifacts 'ComputingComponent','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("3FD4CA7D-05FE-4B37-9548-E99F9DEFAB8B"), "EXEC db_AddArtifacts 'GovernanceMechanism','ComputingComponent','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("56963A61-5202-4AED-B064-43C5778A3C18"), "EXEC db_AddArtifacts 'PhysicalSoftwareComponent','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("3E7988D7-3DFB-4640-BF12-015D8D2315BA"), "EXEC db_AddArtifacts 'GovernanceMechanism','PhysicalSoftwareComponent','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("885C2491-D3E7-4B3B-81F9-829D6D43E7A8"), "EXEC db_AddArtifacts 'PhysicalDataComponent','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("0BF16107-3485-42DE-B522-1C5259F5C884"), "EXEC db_AddArtifacts 'GovernanceMechanism','PhysicalDataComponent','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("92F0B8E3-AE76-40CC-AB79-0DB791FB1215"), "EXEC db_AddArtifacts 'ApplicationInterface','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("F882EBB9-DFDB-4297-BA3B-C87574E7CE5B"), "EXEC db_AddArtifacts 'GovernanceMechanism','ApplicationInterface','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("7DE60150-C757-4177-87E0-2A7C96FEA698"), "EXEC db_AddArtifacts 'GovernanceMechanism','PhysicalInformationArtefact','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("E96C3BE5-94E8-4268-BF09-EA164BE17379"), "EXEC db_AddArtifacts 'PhysicalInformationArtefact','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("49930E6D-D350-4959-B100-8EF5712072DB"), "EXEC db_AddArtifacts 'GovernanceMechanism','LogicalInformationArtefact','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("7194C899-346D-408E-993B-B9B105098110"), "EXEC db_AddArtifacts 'LogicalInformationArtefact','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("40D276A0-B076-40DC-82C0-60916242E97F"), "EXEC db_AddArtifacts 'GovernanceMechanism','StrategicStatement','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("A6D284D1-6CDC-4FD3-8624-4F05A063ABD7"), "EXEC db_AddArtifacts 'StrategicStatement','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("671A4C92-8F1D-4DBC-9497-5C11CF082C7D"), "EXEC db_AddArtifacts 'GovernanceMechanism','DataSubjectArea','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("FF07BABC-D759-434E-929F-FB080F9E81E7"), "EXEC db_AddArtifacts 'DataSubjectArea','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("06082CBC-4BD2-4540-8B6A-D7DF019C74A4"), "EXEC db_AddArtifacts 'GovernanceMechanism','DataEntity','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("8CAE84A8-9365-4CE0-AA04-BFCD9A9D43C7"), "EXEC db_AddArtifacts 'DataEntity','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("38DA37F6-0737-48C1-AF38-A69BAF481CED"), "EXEC db_AddArtifacts 'GovernanceMechanism','DataAttribute','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("C0E0180D-9666-411E-8C39-4932ACC36DD6"), "EXEC db_AddArtifacts 'DataAttribute','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("466407CE-9DAB-445A-866F-ED0061102369"), "EXEC db_AddArtifacts 'GovernanceMechanism','DataField','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("3D79A1B1-2C89-4C34-BE67-8D8F9625BB5C"), "EXEC db_AddArtifacts 'DataField','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("A5C69FB1-2329-445A-8683-A87675A38014"), "EXEC db_AddArtifacts 'GovernanceMechanism','Position','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("25B8B1E1-75CC-4F61-BB2C-0A7B55F9225B"), "EXEC db_AddArtifacts 'Position','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("503EA455-882F-4CCC-BAC8-E702548A5268"), "EXEC db_AddArtifacts 'GovernanceMechanism','Person','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("43714860-5E3C-4F3B-9E6F-6119718254FD"), "EXEC db_AddArtifacts 'Person','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("9CFA8869-50F7-43E0-98B6-E36DDA13412F"), "EXEC db_AddArtifacts 'GovernanceMechanism','JobCompetency','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("531B29CF-D77E-457C-B767-7EABE7B730F3"), "EXEC db_AddArtifacts 'JobCompetency','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("5906113B-7DE3-4014-975E-1CE842529097"), "EXEC db_AddArtifacts 'LogicalSoftwareComponent','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("0377E3AA-9396-4925-8ED1-6D3F920E1025"), "EXEC db_AddArtifacts 'GovernanceMechanism','LogicalSoftwareComponent','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("EE95071B-11AE-4DC2-A059-C94C68ACCB3C"), "EXEC db_AddArtifacts 'Stakeholder','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("DFCBF1CB-3C26-42BD-B605-DD78895B879E"), "EXEC db_AddArtifacts 'GovernanceMechanism','Stakeholder','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("10EFF23F-1BB1-4DF5-B042-2CB2A05E2604"), "EXEC db_AddArtifacts 'Sys','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("0F7F13F4-A834-432C-A182-CD0714AB014F"), "EXEC db_AddArtifacts 'GovernanceMechanism','Sys','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("2E876A4A-D7EE-40D3-A64B-6575AAE3021A"), "EXEC db_AddArtifacts 'Task','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("552C0BB3-73DF-4C60-9178-3F66A43C9B96"), "EXEC db_AddArtifacts 'GovernanceMechanism','Task','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("66BEE94A-F25C-4CFD-B4C3-217ECFCAC171"), "EXEC db_AddArtifacts 'Gateway','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("92A941CF-D4E2-4059-ACF0-A31DA441560A"), "EXEC db_AddArtifacts 'GovernanceMechanism','Gateway','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("733696DF-E792-4951-9C72-01955BA3C951"), "EXEC db_AddArtifacts 'EnvironmentCategory','GovernanceMechanism','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("A2FB6C08-F36E-4F34-B216-B63533AE8FC6"), "EXEC db_AddArtifacts 'GovernanceMechanism','EnvironmentCategory','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("B895506E-B4C5-4542-BC03-4B3295AD93F6"), "EXEC db_AddArtifacts 'GovernanceMechanism','Driver','Mapping','Automation',1|28April2015");
            databaseVersions.Add(new Guid("E09A3894-B485-4586-A470-CAB1256C18A0"), "EXEC db_AddArtifacts 'Driver','GovernanceMechanism','Mapping','Automation',1|28April2015");
            #endregion

            databaseVersions.Add(new Guid("23D91EA3-008F-46FB-A123-D2F5331450DC"), "REBUILDVIEWS|REBUILDVIEWS");
            #endregion

            #region 11 May 2015
            databaseVersions.Add(new Guid("7F9B0FB3-B914-488C-9321-64822BE3E180"), "EXEC db_AddClassAssociations 'Object','Function','Use','',0,1|11 May 2015");
            databaseVersions.Add(new Guid("54EFE712-EFD8-4C74-B6B2-3A629B653790"), "EXEC db_AddClassAssociations 'Object','Process','Use','',0,1|11 May 2015");
            databaseVersions.Add(new Guid("53CAB7E0-9F36-445C-8AF8-610A50C79263"), "EXEC db_AddClassAssociations 'Object','Activity','Use','',0,1|11 May 2015");
            databaseVersions.Add(new Guid("5B193E8F-BC02-40B8-ACF0-DD3AC349C8B9"), "EXEC db_AddClassAssociations 'Function','Object','Create','',0,1|11 May 2015");
            databaseVersions.Add(new Guid("C0CBE44A-9807-40FF-80DC-493D914E0E48"), "EXEC db_AddClassAssociations 'Process','Object','Create','',0,1|11 May 2015");
            databaseVersions.Add(new Guid("22A441DE-068B-469E-81E6-0A15153A08BB"), "EXEC db_AddClassAssociations 'Activity','Object','Create','',0,1|11 May 2015");
            #endregion

            #region 12 May 2015
            databaseVersions.Add(new Guid("C8A068BE-0474-4DB4-9BC5-8887E7646D13"), "exec db_AddClassAssociations 'ApplicationInterface','ApplicationInterface','DynamicFlow','',0,1|12 May 2015");
            databaseVersions.Add(new Guid("7DC413A6-77C2-4CAE-A545-64699F46667E"), "exec db_AddArtifacts 'ApplicationInterface','ApplicationInterface','DynamicFlow','FlowDescription',1|12 May 2015");
            databaseVersions.Add(new Guid("54F36B05-A3AA-4626-B906-C5E2E6FEB164"), "exec db_AddClassAssociations 'PhysicalSoftwareComponent','PhysicalDataComponent','DynamicFlow','',0,1|12 May 2015");
            databaseVersions.Add(new Guid("FDB422DF-31DB-4D87-918F-8DE8A9EA2063"), "exec db_AddArtifacts 'PhysicalSoftwareComponent','PhysicalDataComponent','DynamicFlow','FlowDescription',1|12 May 2015");
            databaseVersions.Add(new Guid("263806C2-E99B-4155-867B-983B64E45D09"), "exec db_AddClassAssociations 'PhysicalDataComponent','PhysicalSoftwareComponent','DynamicFlow','',0,1|12 May 2015");
            databaseVersions.Add(new Guid("28758A98-F189-42D7-9A50-3FBD6CA196C8"), "exec db_AddArtifacts 'PhysicalDataComponent','PhysicalSoftwareComponent','DynamicFlow','FlowDescription',1|12 May 2015");
            databaseVersions.Add(new Guid("C28C526E-EDBB-41D4-A91F-65EE53FF2734"), "exec db_AddClassAssociations 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','DynamicFlow','',0,1|12 May 2015");
            databaseVersions.Add(new Guid("6BF386BC-4769-44A2-BC19-321116345E3A"), "exec db_AddArtifacts 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','DynamicFlow','FlowDescription',1|12 May 2015");
            databaseVersions.Add(new Guid("EEB0F930-77E5-4CE4-8F7A-FC8B152A5845"), "exec db_AddClassAssociations 'PhysicalDataComponent','PhysicalDataComponent','DynamicFlow','',0,1|12 May 2015");
            databaseVersions.Add(new Guid("F17F3404-16D7-42FD-84DF-7A733F0AFD91"), "exec db_AddArtifacts 'PhysicalDataComponent','PhysicalDataComponent','DynamicFlow','FlowDescription',1|12 May 2015");
            databaseVersions.Add(new Guid("0CBB4530-8582-456C-9AC3-97D36F6FB4FA"), "exec db_AddClassAssociations 'OrganizationalUnit','ApplicationInterface','Mapping','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("EF744798-76B9-4C0D-885B-FCDB38DC4633"), "exec db_AddClassAssociations 'Job','ApplicationInterface','Mapping','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("99FFFD74-B49D-4209-B6B2-2AEA8C73BF42"), "exec db_AddClassAssociations 'Position','ApplicationInterface','Mapping','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("7FAD1F94-2C93-4287-86A9-C060512471B1"), "exec db_AddClassAssociations 'Role','ApplicationInterface','Mapping','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("2CDA7965-E4DC-45E1-BB7B-E53A58EACF39"), "exec db_AddClassAssociations 'Person','ApplicationInterface','Mapping','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("F20654A4-7D94-47C5-B539-66733DE0E418"), "exec db_AddClassAssociations 'OrganizationalUnit','DataSubjectArea','Mapping','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("973EC679-8F8F-45EB-B7CC-EF9A37478D6D"), "exec db_AddClassAssociations 'Job','DataSubjectArea','Mapping','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("1AE937DB-2B61-4354-9ED0-8E0E45EBFC4E"), "exec db_AddClassAssociations 'Position','DataSubjectArea','Mapping','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("F75B6C25-A8C8-48FE-B999-44A160689B6B"), "exec db_AddClassAssociations 'Role','DataSubjectArea','Mapping','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("BF250ECA-378F-4B09-8343-74C81D675A29"), "exec db_AddClassAssociations 'Person','DataSubjectArea','Mapping','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("2ADEE058-65C5-45C4-922F-8B0905CCC34C"), "exec db_AddClassAssociations 'OrganizationalUnit','PhysicalDataComponent','Mapping','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("EB4BD95E-6BDD-4854-9CA8-04A013A28976"), "exec db_AddClassAssociations 'Job','PhysicalDataComponent','Mapping','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("2A0EAE7E-A793-4AC3-9DAF-DB238973C673"), "exec db_AddClassAssociations 'Position','PhysicalDataComponent','Mapping','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("B359AD51-8158-4115-8CE0-3B1C3B1BA1F4"), "exec db_AddClassAssociations 'Role','PhysicalDataComponent','Mapping','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("9EEEB9AB-2F9E-4AD6-A806-D23F4289666E"), "exec db_AddClassAssociations 'Person','PhysicalDataComponent','Mapping','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("EB2F0485-6348-4BA0-8C1E-53C8FD2CF5D3"), "exec db_AddClassAssociations 'OrganizationalUnit','PhysicalSoftwareComponent','Mapping','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("277C0E65-78A6-45BA-80D0-D5415B188A4F"), "exec db_AddClassAssociations 'Job','PhysicalSoftwareComponent','Mapping','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("B63A8F09-B310-4B4E-BB0B-049F9BA452BC"), "exec db_AddClassAssociations 'Position','PhysicalSoftwareComponent','Mapping','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("BA40D583-84A6-42E9-9074-4DF6F52F755A"), "exec db_AddClassAssociations 'Role','PhysicalSoftwareComponent','Mapping','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("CE354E6B-CFC5-46B6-91FE-BDB6BF1BCACF"), "exec db_AddClassAssociations 'Person','PhysicalSoftwareComponent','Mapping','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("D2F99613-4A70-402A-B458-59F21EBDCF4B"), "exec db_AddClassAssociations 'OrganizationalUnit','PhysicalInformationArtefact','Mapping','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("69B93162-0090-4F4B-B830-40D2887FDF88"), "exec db_AddClassAssociations 'Job','PhysicalInformationArtefact','Mapping','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("6E7BDA44-30E0-4DF0-933A-453A07BC87F0"), "exec db_AddClassAssociations 'Position','PhysicalInformationArtefact','Mapping','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("FE11FFD7-B6AD-43EC-AF14-EB5D547BD734"), "exec db_AddClassAssociations 'Role','PhysicalInformationArtefact','Mapping','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("D87D33D1-B804-475B-A175-1835CA6926F5"), "exec db_AddClassAssociations 'Person','PhysicalInformationArtefact','Mapping','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("BB00D76D-BACC-4031-87C0-6A26642376AA"), "exec db_AddArtifacts 'OrganizationalUnit','ApplicationInterface','Mapping','Role',1|12 May 2015");
            databaseVersions.Add(new Guid("CE36A705-9C98-4276-B971-BD67EA136012"), "exec db_AddArtifacts 'Job','ApplicationInterface','Mapping','Role',1|12 May 2015");
            databaseVersions.Add(new Guid("6C88A1DE-C7C4-428D-9FA9-DF2B9AA98149"), "exec db_AddArtifacts 'Position','ApplicationInterface','Mapping','Role',1|12 May 2015");
            databaseVersions.Add(new Guid("A15E941A-18AB-46F9-9FF6-4195236605A1"), "exec db_AddArtifacts 'Role','ApplicationInterface','Mapping','Role',1|12 May 2015");
            databaseVersions.Add(new Guid("B362D969-72F4-4D4C-9DD6-034E85F83690"), "exec db_AddArtifacts 'Person','ApplicationInterface','Mapping','Role',1|12 May 2015");
            databaseVersions.Add(new Guid("965B0CF6-7A8D-4C9F-B0D9-D6B996AC7F15"), "exec db_AddArtifacts 'OrganizationalUnit','DataSubjectArea','Mapping','Role',1|12 May 2015");
            databaseVersions.Add(new Guid("FFA034D2-94F0-450A-BB1A-C439ADCDBBE6"), "exec db_AddArtifacts 'Job','DataSubjectArea','Mapping','Role',1|12 May 2015");
            databaseVersions.Add(new Guid("079C659A-1093-4523-92E3-628F9E408253"), "exec db_AddArtifacts 'Position','DataSubjectArea','Mapping','Role',1|12 May 2015");
            databaseVersions.Add(new Guid("AFADEAE5-1857-4E7A-8BFA-C547417B0E0B"), "exec db_AddArtifacts 'Role','DataSubjectArea','Mapping','Role',1|12 May 2015");
            databaseVersions.Add(new Guid("B3CC98E4-ABAC-4E65-AA39-616F67D7BE78"), "exec db_AddArtifacts 'Person','DataSubjectArea','Mapping','Role',1|12 May 2015");
            databaseVersions.Add(new Guid("8DB6EAC0-BBDD-4016-8467-37D754469182"), "exec db_AddArtifacts 'OrganizationalUnit','PhysicalDataComponent','Mapping','Role',1|12 May 2015");
            databaseVersions.Add(new Guid("A50F99EB-E67B-492B-9401-2765D68876A6"), "exec db_AddArtifacts 'Job','PhysicalDataComponent','Mapping','Role',1|12 May 2015");
            databaseVersions.Add(new Guid("C76F09F4-2928-416A-B464-E510B7C8B494"), "exec db_AddArtifacts 'Position','PhysicalDataComponent','Mapping','Role',1|12 May 2015");
            databaseVersions.Add(new Guid("1CB874E2-2BE6-4249-A881-F2C53E569F5A"), "exec db_AddArtifacts 'Role','PhysicalDataComponent','Mapping','Role',1|12 May 2015");
            databaseVersions.Add(new Guid("F77AD7FC-49FD-4700-95DE-C94ED9A1C26A"), "exec db_AddArtifacts 'Person','PhysicalDataComponent','Mapping','Role',1|12 May 2015");
            databaseVersions.Add(new Guid("343936DE-3857-4B2E-B0D1-BB3E1B358F8F"), "exec db_AddArtifacts 'OrganizationalUnit','PhysicalSoftwareComponent','Mapping','Role',1|12 May 2015");
            databaseVersions.Add(new Guid("DE363466-5A47-494B-B8A8-5AA034997A60"), "exec db_AddArtifacts 'Job','PhysicalSoftwareComponent','Mapping','Role',1|12 May 2015");
            databaseVersions.Add(new Guid("EDE960B1-405F-4A5C-A067-8674744E964D"), "exec db_AddArtifacts 'Position','PhysicalSoftwareComponent','Mapping','Role',1|12 May 2015");
            databaseVersions.Add(new Guid("AD22C474-A5C9-4655-9B53-19BD36723529"), "exec db_AddArtifacts 'Role','PhysicalSoftwareComponent','Mapping','Role',1|12 May 2015");
            databaseVersions.Add(new Guid("648F9D82-0D32-436A-A18E-DFAF90A58394"), "exec db_AddArtifacts 'Person','PhysicalSoftwareComponent','Mapping','Role',1|12 May 2015");
            databaseVersions.Add(new Guid("0C3F426F-3471-40E6-A91A-3EF500138C5D"), "exec db_AddArtifacts 'OrganizationalUnit','PhysicalInformationArtefact','Mapping','Role',1|12 May 2015");
            databaseVersions.Add(new Guid("11FE97FA-E5DE-4AA3-8E45-7093371B801B"), "exec db_AddArtifacts 'Job','PhysicalInformationArtefact','Mapping','Role',1|12 May 2015");
            databaseVersions.Add(new Guid("6B690E12-4987-403C-A9BD-F7C88853E18E"), "exec db_AddArtifacts 'Position','PhysicalInformationArtefact','Mapping','Role',1|12 May 2015");
            databaseVersions.Add(new Guid("975725A3-0FA5-42C8-8DD2-67F08EFB49EC"), "exec db_AddArtifacts 'Role','PhysicalInformationArtefact','Mapping','Role',1|12 May 2015");
            databaseVersions.Add(new Guid("3D2F180F-3DE6-4628-A05B-EEA530DDF9A5"), "exec db_AddArtifacts 'Person','PhysicalInformationArtefact','Mapping','Role',1|12 May 2015");

            databaseVersions.Add(new Guid("7AA75F95-39DC-4A85-8FF8-AC89C4846420"), "exec db_AddClassAssociations 'PhysicalDataComponent','PhysicalDataComponent','Mapping','',1,1|12 May 2015");

            databaseVersions.Add(new Guid("99CA54FF-927D-4B3D-B2E8-888AD5B2249E"), "exec db_AddArtifacts 'PhysicalDataComponent','PhysicalDataComponent','DynamicFlow','PhysicalInformationArtefact',1|12 May 2015");
            databaseVersions.Add(new Guid("BA748859-E0D0-44B2-910F-98D19B55A8EE"), "exec db_AddArtifacts 'PhysicalDataComponent','PhysicalSoftwareComponent','DynamicFlow','PhysicalInformationArtefact',1|12 May 2015");
            databaseVersions.Add(new Guid("FBF62649-5B18-422D-8539-397F53B4A907"), "exec db_AddArtifacts 'PhysicalSoftwareComponent','PhysicalDataComponent','DynamicFlow','PhysicalInformationArtefact',1|12 May 2015");
            databaseVersions.Add(new Guid("D69B1853-D16E-4CB8-A2DD-4C9FD2F7AF2B"), "exec db_AddArtifacts 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','DynamicFlow','PhysicalInformationArtefact',1|12 May 2015");

            databaseVersions.Add(new Guid("476E7950-EFB5-4A46-B8C9-228055523E15"), "exec db_AddClassAssociations 'PhysicalInformationArtefact','PhysicalDataComponent','DynamicFlow','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("34BD1774-AF04-4496-9529-ED2692FFCDDB"), "exec db_AddClassAssociations 'PhysicalInformationArtefact','PhysicalSoftwareComponent','DynamicFlow','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("CA34B288-68BA-4B17-9D00-640C70B907D6"), "exec db_AddClassAssociations 'PhysicalDataComponent','PhysicalInformationArtefact','DynamicFlow','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("A9C82E1B-4A25-4DD5-9C5E-91743D6BB3FD"), "exec db_AddClassAssociations 'PhysicalSoftwareComponent','PhysicalInformationArtefact','DynamicFlow','',1,1|12 May 2015");
            databaseVersions.Add(new Guid("9932D6B1-6DD1-41FE-93AC-75AE7CFD61EB"), "exec db_AddArtifacts 'PhysicalDataComponent','Object','DynamicFlow','PhysicalInformationArtefact',1|12 May 2015");
            databaseVersions.Add(new Guid("D00E4D78-6996-4057-93DC-307C2CC0CB50"), "exec db_AddArtifacts 'PhysicalSoftwareComponent','Object','DynamicFlow','PhysicalInformationArtefact',1|12 May 2015");
            databaseVersions.Add(new Guid("786F7E5D-E294-4EBA-B159-7C019C35BB35"), "exec db_AddArtifacts 'Object','PhysicalSoftwareComponent','DynamicFlow','PhysicalInformationArtefact',1|12 May 2015");
            databaseVersions.Add(new Guid("0CC431E8-66F5-4217-BBC4-75BF6018A427"), "exec db_AddArtifacts 'Object','PhysicalDataComponent','DynamicFlow','PhysicalInformationArtefact',1|12 May 2015");

            #endregion

            #region 14 May 2015
            databaseVersions.Add(new Guid("751D3763-DC8D-4A1F-B8FE-FFFD2FC33FCE"), "EXEC db_AddPossibleValues 'DataComponentType','Transactional',15,'Transactional',1|14May2015");
            #endregion

            #region 25 May 2015
            databaseVersions.Add(new Guid("748DE8BB-4817-4541-BAC8-05D47BEB6CBD"), "exec db_AddClassAssociations 'PhysicalSoftwareComponent','OrganizationalUnit','DynamicFlow','',1,1|25 May 2015");
            databaseVersions.Add(new Guid("62055EA1-7471-4543-9F52-58FF26388342"), "exec db_AddArtifacts 'PhysicalSoftwareComponent','OrganizationalUnit','DynamicFlow','PhysicalInformationArtefact',1|25 May 2015");
            databaseVersions.Add(new Guid("D8CF5D1F-DEEA-4E8D-8C14-FA7418DAA3C6"), "exec db_AddArtifacts 'PhysicalSoftwareComponent','OrganizationalUnit','DynamicFlow','DataSubjectArea',1|25 May 2015");

            databaseVersions.Add(new Guid("D37A8B8E-1116-472C-B226-DE3A6FB04FEC"), "exec db_AddClassAssociations 'PhysicalDataComponent','OrganizationalUnit','DynamicFlow','',1,1|25 May 2015");
            databaseVersions.Add(new Guid("859C57C6-C05E-4AF0-8885-BAEE90796E21"), "exec db_AddArtifacts 'PhysicalDataComponent','OrganizationalUnit','DynamicFlow','PhysicalInformationArtefact',1|25 May 2015");
            databaseVersions.Add(new Guid("E14D51D5-DD42-4080-87C7-12BC1FE047E9"), "exec db_AddArtifacts 'PhysicalDataComponent','OrganizationalUnit','DynamicFlow','DataSubjectArea',1|25 May 2015");

            databaseVersions.Add(new Guid("3501BFCF-75B1-44E2-9C61-482876EF4D30"), "EXEC db_AddClassAssociations 'Object','LocationUnit','Mapping','',1,1|25 May 2015");
            databaseVersions.Add(new Guid("F8C82ACA-7E96-414F-886D-93C1C8261790"), "EXEC db_AddClassAssociations 'Object','TimeUnit','Mapping','',1,1|25 May 2015");
            databaseVersions.Add(new Guid("E6EFC05E-540D-4BE9-BB00-E7BDA9416492"), "EXEC db_AddClassAssociations 'Object','TimeReference','Mapping','',1,1|25 May 2015");
            databaseVersions.Add(new Guid("910F48B0-9BCD-47EA-A7EF-C31F3787A426"), "EXEC db_AddClassAssociations 'Object','Resource','Mapping','',1,1|25 May 2015");
            databaseVersions.Add(new Guid("01E307D0-8361-414C-81B4-6F1627D576C8"), "EXEC db_AddClassAssociations 'Object','ResourceType','Mapping','',1,1|25 May 2015");
            databaseVersions.Add(new Guid("46CFC45E-48E2-4365-BFDA-066CAED074B2"), "EXEC db_AddClassAssociations 'WorkPackage','LocationUnit','Mapping','',1,1|25 May 2015");
            databaseVersions.Add(new Guid("F5AD941A-B30F-4B0B-9834-73AB7B2D43DE"), "EXEC db_AddClassAssociations 'LocationScheme','TimeReference','Mapping','',1,1|25 May 2015");
            databaseVersions.Add(new Guid("66546439-ECAD-4E6F-99CA-B4C5F4006FFC"), "EXEC db_AddClassAssociations 'LocationScheme','Location','Mapping','',1,1|25 May 2015");
            databaseVersions.Add(new Guid("82A3429F-D93D-48EE-BD58-EBEDDB1EFF16"), "EXEC db_AddClassAssociations 'LocationScheme','TimeReference','Mapping','',1,1|25 May 2015");
            databaseVersions.Add(new Guid("5E31B22E-D43F-4AB5-895A-2E0810514F1D"), "EXEC db_AddClassAssociations 'WorkPackage','ResourceType','Mapping','',1,1|25 May 2015");
            #endregion

            #region 29 May 2015
            databaseVersions.Add(new Guid("B9612019-C871-4FD6-8E35-194146A15DA0"), "EXEC db_AddClassAssociations 'DataField','DataField','DynamicFlow','',1,1|29 May 2015");
            databaseVersions.Add(new Guid("4E6E1C8B-028C-4C36-82CE-4BFF6FD6547F"), "exec db_AddArtifacts 'DataField','DataField','DynamicFlow','Rationale',1|29 May 2015");
            #endregion

            #region 4 June 2015

            databaseVersions.Add(new Guid("C389EC32-3A78-495A-A6B5-3426AF2967C8"), "EXEC db_AddClassAssociations 'OrganizationalUnit','PhysicalSoftwareComponent','DynamicFlow','',1,1|4 June 2015");
            databaseVersions.Add(new Guid("D2823161-B5ED-4BDA-838B-D91E4F5B7ED1"), "EXEC db_AddArtifacts 'OrganizationalUnit','PhysicalSoftwareComponent','DynamicFlow','DataSubjectArea',1|4 June 2015");
            databaseVersions.Add(new Guid("8A58EA53-F6BD-4E83-8854-7F96081D56E9"), "EXEC db_AddArtifacts 'OrganizationalUnit','PhysicalSoftwareComponent','DynamicFlow','PhysicalInformationArtefact',1|4 June 2015");
            databaseVersions.Add(new Guid("17CF3EB6-86FF-4A99-86D9-FB98A74B4A95"), "EXEC db_AddClassAssociations 'PhysicalSoftwareComponent','OrganizationalUnit','DynamicFlow','',1,1|4 June 2015");
            databaseVersions.Add(new Guid("B01FC17C-9F3B-45EA-AB34-7DCEB62E20CB"), "EXEC db_AddArtifacts 'PhysicalSoftwareComponent','OrganizationalUnit','DynamicFlow','DataSubjectArea',1|4 June 2015");
            databaseVersions.Add(new Guid("F7C2CA1E-3124-4E35-A0A3-77C2480DD3A8"), "EXEC db_AddArtifacts 'PhysicalSoftwareComponent','OrganizationalUnit','DynamicFlow','PhysicalInformationArtefact',1|4 June 2015");
            databaseVersions.Add(new Guid("889D9198-A820-4CB5-83CB-00D4621EBA3E"), "EXEC db_AddClassAssociations 'OrganizationalUnit','PhysicalDataComponent','DynamicFlow','',1,1|4 June 2015");
            databaseVersions.Add(new Guid("FD90DC95-9499-4143-94A1-F6463026CB50"), "EXEC db_AddClassAssociations 'PhysicalDataComponent','OrganizationalUnit','DynamicFlow','',1,1|4 June 2015");
            databaseVersions.Add(new Guid("6C4A4674-D9D0-493B-98BE-FBCF570D93D2"), "EXEC db_AddArtifacts 'OrganizationalUnit','PhysicalDataComponent','DynamicFlow','DataSubjectArea',1|4 June 2015");
            databaseVersions.Add(new Guid("36E448C1-EBA9-40D2-8C59-AC954CE23E07"), "EXEC db_AddArtifacts 'OrganizationalUnit','PhysicalDataComponent','DynamicFlow','PhysicalInformationArtefact',1|4 June 2015");
            databaseVersions.Add(new Guid("EE34DFE6-4DC6-4F2D-8F48-9A418710A6C0"), "EXEC db_AddArtifacts 'PhysicalDataComponent','OrganizationalUnit','DynamicFlow','DataSubjectArea',1|4 June 2015");
            databaseVersions.Add(new Guid("8D6B8A33-286B-4530-94C6-4404A9F74799"), "EXEC db_AddArtifacts 'PhysicalDataComponent','OrganizationalUnit','DynamicFlow','PhysicalInformationArtefact',1|4 June 2015");
            databaseVersions.Add(new Guid("B34902DE-47D0-4F08-BBC1-3B4AA91BE21A"), "EXEC db_AddClassAssociations 'PhysicalDataComponent','PhysicalDataComponent','DynamicFlow','',1,1|4 June 2015");
            databaseVersions.Add(new Guid("90521399-8367-484A-ADAE-833F9754EB43"), "EXEC db_AddArtifacts 'PhysicalDataComponent','PhysicalDataComponent','DynamicFlow','DataSubjectArea',1|4 June 2015");
            databaseVersions.Add(new Guid("1F610BE8-0AE3-4957-BB2D-F5248C64D89C"), "EXEC db_AddClassAssociations 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','DynamicFlow','',1,1|4 June 2015");
            databaseVersions.Add(new Guid("88A455A2-6FF1-4165-84D2-12684C602508"), "EXEC db_AddArtifacts 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','DynamicFlow','DataSubjectArea',1|4 June 2015");
            databaseVersions.Add(new Guid("6D16AB54-54EA-45F7-BF10-53DE5CEB0035"), "EXEC db_AddClassAssociations 'PhysicalSoftwareComponent','PhysicalDataComponent','DynamicFlow','',1,1|4 June 2015");
            databaseVersions.Add(new Guid("7ABFAE68-C13D-4F7D-B1F2-03259640216E"), "EXEC db_AddClassAssociations 'PhysicalDataComponent','PhysicalSoftwareComponent','DynamicFlow','',1,1|4 June 2015");
            databaseVersions.Add(new Guid("19A319E4-3593-4F28-85DB-B64781BB1047"), "EXEC db_AddArtifacts 'PhysicalSoftwareComponent','PhysicalDataComponent','DynamicFlow','DataSubjectArea',1|4 June 2015");
            databaseVersions.Add(new Guid("1A3655AA-ED66-45F9-B521-ED692AFCAEF2"), "EXEC db_AddArtifacts 'PhysicalDataComponent','PhysicalSoftwareComponent','DynamicFlow','DataSubjectArea',1|4 June 2015");


            #endregion

            #region 5 June 2015
            databaseVersions.Add(new Guid("6ADB6B58-D300-4E2A-80FC-5E4AC662F46E"), "exec db_addfields 'DataSubjectArea','Synonyms','System.String','General','',0,2,1|5 June 2015");
            databaseVersions.Add(new Guid("6BA91E1D-2429-46BB-B8F7-5F6D30DC2CA1"), "REBUILDSINGLEVIEWS-DataSubjectArea-|5 June 2015");
            #endregion

            #region 17 June 2015
            databaseVersions.Add(new Guid("732660BF-2202-4B80-808E-5FFBB64D19B8"), "exec db_addFields 'SelectorAttribute','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',1,1,1|June 17 2015");
            databaseVersions.Add(new Guid("3AA05BB5-B320-499F-B12D-53865B0D8D48"), "exec db_addFields 'SelectorAttribute','Abbreviation','System.String','General','',1,2,1|June 17 2015");
            databaseVersions.Add(new Guid("BB2DAACF-BB8F-4239-8AED-2FBA36B4F16B"), "exec db_addFields 'SelectorAttribute','Synonyms','System.String','General','',1,2,1|June 17 2015");
            databaseVersions.Add(new Guid("FEE8A99F-6429-42FB-918D-FB84CEAD231E"), "exec db_addFields 'SelectorAttribute','Description','System.String','General','General description for each object',1,3,1|June 17 2015");
            databaseVersions.Add(new Guid("6979803B-E96B-49B3-8BA4-ED5D6EBD6699"), "exec db_addFields 'SelectorAttribute','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',1,4,1|June 17 2015");
            databaseVersions.Add(new Guid("806A1713-7559-4FD3-8478-EE17A11BBF64"), "exec db_addFields 'SelectorAttribute','DataType','DataType','General','',1,5,1|June 17 2015");
            databaseVersions.Add(new Guid("37884DC6-D101-4BDA-B510-46BF3E0058F6"), "exec db_addFields 'SelectorAttribute','DataLength','System.String','General','',1,6,1|June 17 2015");
            databaseVersions.Add(new Guid("9CBD00CB-02F3-4E31-A4C0-39FC385E1F14"), "exec db_addFields 'SelectorAttribute','ContentType','ContentType','General','',1,7,1|June 17 2015");
            databaseVersions.Add(new Guid("CD99D92E-203A-4D8C-85F5-C409ED0B3982"), "exec db_addFields 'SelectorAttribute','IsFactOrMeasure','IsFactOrMeasure','General','',1,8,1|June 17 2015");
            databaseVersions.Add(new Guid("B9008F2D-173E-45C5-BAB6-E9DA46895C06"), "exec db_addFields 'SelectorAttribute','IsDerived','System.String','General','',1,9,1|June 17 2015");
            databaseVersions.Add(new Guid("AFD460F2-62AE-4D32-8F19-6CB3D3655D12"), "exec db_addFields 'SelectorAttribute','ArchitectureStatus','ArchitectureStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',1,80,1|June 17 2015");
            databaseVersions.Add(new Guid("66521135-BBFC-46FA-8846-4F1DD7448BBC"), "exec db_addFields 'SelectorAttribute','ArchitectureStatusDate','System.String','General','Effective date of standardisation status.',1,81,1|June 17 2015");
            databaseVersions.Add(new Guid("D015A754-4E5F-415D-9A73-1E9256C31397"), "exec db_addFields 'SelectorAttribute','DesignRationale','System.String','General','',1,82,1|June 17 2015");
            databaseVersions.Add(new Guid("2D813734-F47C-44F2-A3D7-E366C80E6AA9"), "exec db_addFields 'SelectorAttribute','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',1,83,1|June 17 2015");
            databaseVersions.Add(new Guid("C247A454-0865-46B3-A8FD-B54739201B55"), "exec db_addFields 'SelectorAttribute','GapType','GapType','General','Result of a gap analysis between architecture states.',1,84,1|June 17 2015");
            databaseVersions.Add(new Guid("75FBDFE1-1D44-4731-B3AB-80D00B9EC7CE"), "exec db_addFields 'SelectorAttribute','DataSourceID','System.String','General','Unique identified of for object as per external data source.',1,85,1|June 17 2015");
            databaseVersions.Add(new Guid("9B50E824-EEE9-45FA-A0DF-198B5A51453E"), "exec db_addFields 'SelectorAttribute','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',1,86,1|June 17 2015");
            #endregion

            #region 23 June 2015

            databaseVersions.Add(new Guid("D1E39B25-8AA8-4407-B9C6-FFA8D3DD1AF8"), "EXEC db_AddPossibleValues 'ObjectType','Competency',7,'Competency',1|June 23 2015");

            databaseVersions.Add(new Guid("223B7B66-9004-41FF-BA47-BB4197209DB7"), "EXEC db_AddPossibleValues 'MeasureTypeName','Strategy',1,'Strategy',1|June 23 2015");
            databaseVersions.Add(new Guid("41A92B31-8113-49BD-A788-AA1C4FE507CC"), "EXEC db_AddPossibleValues 'MeasureTypeName','PerformanceSkills',2,'PerformanceSkills',1|June 23 2015");
            databaseVersions.Add(new Guid("1CD37438-E5F3-4037-9DF7-1532F1CE4C6A"), "EXEC db_AddPossibleValues 'MeasureTypeName','PerformanceProcess',3,'PerformanceProcess',1|June 23 2015");
            databaseVersions.Add(new Guid("B241A989-BE3C-441D-8E26-8626D00107E6"), "EXEC db_AddPossibleValues 'MeasureTypeName','PerformanceInformation',4,'PerformanceInformation',1|June 23 2015");
            databaseVersions.Add(new Guid("4D39D546-6F67-4D4A-9E06-4EDA87144406"), "EXEC db_AddPossibleValues 'MeasureTypeName','PerformanceTechnology',5,'PerformanceTechnology',1|June 23 2015");
            databaseVersions.Add(new Guid("B7D371A2-0388-4BEB-8446-F72A1AA24F4B"), "EXEC db_AddPossibleValues 'MeasureTypeName','Differentiating',5,'Differentiating',1|June 23 2015");

            databaseVersions.Add(new Guid("971A4A5B-5509-47AC-BD6D-06771AAA8B28"), "EXEC db_AddPossibleValues 'MeasureValueName','High',1,'High',1|June 23 2015");
            databaseVersions.Add(new Guid("8D8973D4-29CC-4889-885B-B49667AF0296"), "EXEC db_AddPossibleValues 'MeasureValueName','Medium',2,'Medium',1|June 23 2015");
            databaseVersions.Add(new Guid("5D55685D-810C-4407-B8F7-0D0F723AB427"), "EXEC db_AddPossibleValues 'MeasureValueName','Low',3,'Low',1|June 23 2015");
            databaseVersions.Add(new Guid("4504984B-9651-4993-A778-68282BEA990F"), "EXEC db_AddPossibleValues 'MeasureValueName','None',4,'None',1|June 23 2015");

            databaseVersions.Add(new Guid("E4AD2F09-51E6-4F9C-BF35-4569B0A19859"), "exec db_addClasses 'MeasureType','Name','Strategy',1|June 23 2015");
            databaseVersions.Add(new Guid("250F9658-0C0F-4F09-9AA6-E4A584DE9CD7"), "exec db_addClasses 'MeasureValue','Name','Strategy',1|June 23 2015");

            databaseVersions.Add(new Guid("ED18B376-3126-420F-BE63-E492FC57C3C1"), "exec db_addFields 'MeasureType','Name','MeasureTypeName','General','',1,1,1|June 23 2015");
            databaseVersions.Add(new Guid("843142D9-44BC-4B24-81E2-A31287111B6D"), "exec db_addFields 'MeasureValue','Name','MeasureValueName','General','',1,1,1|June 23 2015");

            databaseVersions.Add(new Guid("9D099DF2-79A2-437B-9C77-282A75680B11"), "EXEC db_AddClassAssociations 'MeasureType','Object','Mapping','',1,1|June 23 2015");
            databaseVersions.Add(new Guid("2596BB2F-3139-4716-A29C-CD1ECE09465B"), "EXEC db_AddArtifacts 'MeasureType','Object','Mapping','MeasureValue',1|June 23 2015");
            databaseVersions.Add(new Guid("81A73F28-A2C8-4028-B82D-FE3413FB0A93"), "EXEC db_AddArtifacts 'Object','MeasureType','Mapping','MeasureValue',1|June 23 2015");

            databaseVersions.Add(new Guid("C545E507-16EA-425A-97E7-08ADDE5ADE02"), "REBUILDSINGLEVIEWS-MeasureType-MeasureValue-|June 23 2015");

            #endregion

            #region 2 July 2015

            databaseVersions.Add(new Guid("7448D66F-874B-45F4-AAD2-94779FEC3127"), "exec db_addFields 'Object','AverageMeasureValue','System.String','General','',1,6,1|July 2 2015");
            databaseVersions.Add(new Guid("358E29F9-26E7-40C0-AF88-22BA51B791C9"), "exec db_addFields 'Function','AverageMeasureValue','System.String','General','',1,12,1|July 2 2015");
            databaseVersions.Add(new Guid("D3F2E330-B828-48D7-AF9B-E7F1BDD47AC2"), "REBUILDSINGLEVIEWS-Function-Object-|July 2 2015");

            #endregion

            #region 13 July 2015
            databaseVersions.Add(new Guid("599694B4-8B6C-4D33-B0AD-DEA3BFB98D6D"), "EXEC db_AddClassAssociations 'PhysicalSoftwareComponent','ComputingComponent','Dependency','',0,1|July 13 2015");
            databaseVersions.Add(new Guid("8CB0F78A-FF4B-44DB-B689-878DC1EE1AB3"), "EXEC db_AddClassAssociations 'PhysicalSoftwareComponent','ComputingComponent','Mapping','',1,1|July 13 2015");
            #endregion

            #region 14 July 2015
            databaseVersions.Add(new Guid("378028CE-AE75-4999-B098-F0F8BAA02CD4"), "UPDATE Field SET Description = 'Unique identifier (foreign key) for an object as per the data source.' WHERE Name = 'DataSourceID'|July 14 2015");
            #endregion

            #region 13 July 2015
            //databaseVersions.Add(new Guid(""), "EXEC db_AddClassAssociations 'LogicalSoftwareComponent','Function','Mapping','',1,1|July 20 2015");
            databaseVersions.Add(new Guid("16A01B1A-EF26-4B63-96C5-6FEDE0E5A1FE"), "EXEC db_AddClassAssociations 'LogicalITInfrastructureComponent','Function','Mapping','',1,1|July 20 2015");
            #endregion

            #region 17 August 2015
            databaseVersions.Add(new Guid("B92BD20B-B354-4746-9A97-62765B740464"), "EXEC db_AddClassAssociations 'Gateway','Role','Mapping','',1,1|August 17 2015");
            databaseVersions.Add(new Guid("01DE01D1-1C38-4190-9706-E4F3F64BC4E9"), "EXEC db_AddClassAssociations 'Gateway','JobPosition','Mapping','',1,1|August 17 2015");
            databaseVersions.Add(new Guid("9BD49D06-7FB4-4428-A815-91096C98C41C"), "EXEC db_AddClassAssociations 'Gateway','Position','Mapping','',1,1|August 17 2015");
            databaseVersions.Add(new Guid("7912F06A-3514-4F30-BB4B-BDC38DEC242C"), "EXEC db_AddClassAssociations 'Gateway','Job','Mapping','',1,1|August 17 2015");

            databaseVersions.Add(new Guid("B365280A-EFF3-4748-8664-ADFCFECE63AC"), "EXEC db_AddClassAssociations 'Event','Role','Mapping','',1,1|August 17 2015");
            databaseVersions.Add(new Guid("C435A5A8-D11C-4EF3-B4B6-019095F3C987"), "EXEC db_AddClassAssociations 'Event','JobPosition','Mapping','',1,1|August 17 2015");
            databaseVersions.Add(new Guid("87322A3F-D8D2-448E-9405-EEBC2EC4BD2A"), "EXEC db_AddClassAssociations 'Event','Position','Mapping','',1,1|August 17 2015");

            #endregion

            #region 18 August 2015 (Alter Meta_CreateObject with lastmodified)
            databaseVersions.Add(new Guid("6B904431-0813-46A9-A476-9F714C809374"), "ALTER PROCEDURE [dbo].[META_CreateObject] @Class varchar(50), @pkid int = 0  output, @WorkspaceTypeID int, @WorkspaceName	varchar(100), @UserID int, @MachineName varchar(50) = null output as if (@pkid>0) begin if exists(select pkid from metaobject where pkid = @pkid and machine=@machinename) 		begin 				UPDATE MetaObject Set LastModified = GetDate() where pkid = @pkid and machine=@machinename	return 			end	if not exists(select pkid from metaobject where pkid = @pkid and machine=@machinename)	begin		set identity_insert dbo.metaobject on		insert into metaobject(pkid,class,workspacetypeid,workspacename,userid,machine) values(@pkid,@class,@workspacetypeid,@workspacename,@userid,@MachineName)	end	set identity_insert dbo.metaobject  off	return end insert into metaobject(class,workspacetypeid,workspacename,userid,machine) values(@class,@workspacetypeid,@workspacename,@userid,@machinename) set @pkid = @@IDENTITY |August 18 2015");
            #endregion

            #region 19 August 2015
            databaseVersions.Add(new Guid("F7B3325B-566D-46E4-81BD-73D8FD592936"), "EXECUTE [dbo].[db_AddFields] 'Resource' ,'FacilitySize' ,'System.String' ,'++Facilities++' ,'Size of the facility in square metres' ,1 ,1 ,1|August 19 2015");
            databaseVersions.Add(new Guid("DA2E5CBC-BF38-4E65-9368-A77A18125717"), "ALTER TABLE Field ALTER COLUMN Category varchar(500) NOT NULL|August 19 2015");
            #endregion

            #region 2 August 2015

            databaseVersions.Add(new Guid("E692E994-A55C-4574-8AD3-B7D7BE521A2B"), "EXEC db_AddClassAssociations 'Job','Job','Decomposition','',0,1|August 24 2015");
            databaseVersions.Add(new Guid("7CEC3E06-AFB1-41B2-8554-20B1DBEAD7F6"), "EXEC db_addFields 'OrganizationalUnit','CostCentre','System.String','General','',1,10,1|August 24 2015");
            databaseVersions.Add(new Guid("0458BFD5-9322-4DCD-B9E5-FEF98B5CB04A"), "EXEC db_addFields 'Position','CostCentre','System.String','General','',1,6,1|August 24 2015");
            databaseVersions.Add(new Guid("F6EB0E13-CA46-417A-A430-947DA1A66707"), "REBUILDSINGLEVIEWS-OrganizationalUnit-Position-|August 24 2015");

            #endregion

            #region 3 September 2015

            databaseVersions.Add(new Guid("EFB53CFA-3F32-460B-B019-BD91F44310F9"), "EXEC db_AddClasses 'AverageOutputPerMetricUnit','Name','Strategy',1|September 3 2015");
            databaseVersions.Add(new Guid("FB55D5AD-2598-468A-8242-7CBF414CA9E6"), "EXEC db_AddClasses 'PlannedOutput','Name','Strategy',1|September 3 2015");
            databaseVersions.Add(new Guid("759DB35B-F523-45E0-9D20-61D19DBA523C"), "EXEC db_AddClasses 'PlannedTime','Name','Strategy',1|September 3 2015");
            databaseVersions.Add(new Guid("F2F3C38E-C11C-40B5-9F1B-52BC6BF39E3E"), "EXEC db_AddClasses 'ActualOutput','Name','Strategy',1|September 3 2015");
            databaseVersions.Add(new Guid("BE40ADCB-4023-40A5-B7F6-E335F83FA08E"), "EXEC db_AddClasses 'ActualTime','Name','Strategy',1|September 3 2015");
            databaseVersions.Add(new Guid("4D16D945-2F56-4EF6-B03D-1B1076D995C8"), "EXEC db_AddFields 'AverageOutputPerMetricUnit','Name','System.String','General','',1,1,1|September 3 2015");
            databaseVersions.Add(new Guid("E5B43377-601F-4214-B11F-1F104AA4505C"), "EXEC db_AddFields 'PlannedOutput','Name','System.String','General','',1,1,1|September 3 2015");
            databaseVersions.Add(new Guid("43BEC68E-297D-4F2A-867F-276771BD0C6A"), "EXEC db_AddFields 'PlannedTime','Name','System.String','General','',1,1,1|September 3 2015");
            databaseVersions.Add(new Guid("B6D0FAA0-5715-4230-9E12-11F131E06B0F"), "EXEC db_AddFields 'ActualOutput','Name','System.String','General','',1,1,1|September 3 2015");
            databaseVersions.Add(new Guid("CA664196-5103-485D-BE22-C6896F3437AB"), "EXEC db_AddFields 'ActualTime','Name','System.String','General','',1,1,1|September 3 2015");
            databaseVersions.Add(new Guid("6BF0C4B5-0C01-495B-A809-54D19E43EE4D"), "EXEC db_AddFields 'Process','OutputMetricUnit','System.String','General','',1,7,1|September 3 2015");
            databaseVersions.Add(new Guid("A8032E38-D1F4-4800-B5A4-5927566B01AA"), "EXEC db_AddFields 'Activity','OutputMetricUnit','System.String','General','',1,7,1|September 3 2015");
            databaseVersions.Add(new Guid("EEDCC3E7-B373-41AD-BD24-5EA6FF6CE49A"), "EXEC db_AddFields 'TimeUnit','StartTime','System.String','General','',1,8,1|September 3 2015");
            databaseVersions.Add(new Guid("FECAB291-5C1C-411A-ABA2-923BCB0D4FBB"), "EXEC db_AddFields 'TimeUnit','EndTime','System.String','General','',1,9,1|September 3 2015");
            databaseVersions.Add(new Guid("2FB5DCB5-066D-4C4E-91D8-359042DC1F77"), "EXEC db_AddClassAssociations 'Process','LocationUnit','Mapping','',1,1|September 3 2015");
            databaseVersions.Add(new Guid("03204B4E-C5A4-4DC5-B93B-27D5E49B9F44"), "EXEC db_AddClassAssociations 'Activity','LocationUnit','Mapping','',1,1|September 3 2015");
            databaseVersions.Add(new Guid("709EC9BA-AE09-4CF7-B1D4-D6AB5C3B0718"), "EXEC db_AddArtifacts 'Process','LocationUnit','Mapping','AverageOutputPerMetricUnit',1|September 3 2015");
            databaseVersions.Add(new Guid("DF1CEDA5-A72F-4CB5-B46B-41DC6BB06694"), "EXEC db_AddArtifacts 'Activity','LocationUnit','Mapping','AverageOutputPerMetricUnit',1|September 3 2015");
            databaseVersions.Add(new Guid("B51A316D-EFC0-4352-AE19-3BEB0F124795"), "EXEC db_AddArtifacts 'Process','LocationUnit','Mapping','PlannedOutput',1|September 3 2015");
            databaseVersions.Add(new Guid("92BE28F2-4A79-445F-927E-08CE4378F6A4"), "EXEC db_AddArtifacts 'Activity','LocationUnit','Mapping','PlannedOutput',1|September 3 2015");
            databaseVersions.Add(new Guid("232A1459-E4DE-4899-8FFE-A6DB1B1E9AF3"), "EXEC db_AddArtifacts 'Process','LocationUnit','Mapping','PlannedTime',1|September 3 2015");
            databaseVersions.Add(new Guid("914A1325-7C10-4A18-9B8D-2A5F64C56389"), "EXEC db_AddArtifacts 'Activity','LocationUnit','Mapping','PlannedTime',1|September 3 2015");
            databaseVersions.Add(new Guid("D03116DC-72AD-4EE3-AAFA-D38C3B8AAA9C"), "EXEC db_AddArtifacts 'Process','LocationUnit','Mapping','ActualOutput',1|September 3 2015");
            databaseVersions.Add(new Guid("F83B715A-EB37-493C-886C-F3A688F1A00A"), "EXEC db_AddArtifacts 'Activity','LocationUnit','Mapping','ActualOutput',1|September 3 2015");
            databaseVersions.Add(new Guid("E4D34A2D-FF9D-406D-8CDE-3CCAF5F9E263"), "EXEC db_AddArtifacts 'Process','LocationUnit','Mapping','ActualTime',1|September 3 2015");
            databaseVersions.Add(new Guid("BB13CAE3-C024-4949-B6A3-51A9A5F5DE58"), "EXEC db_AddArtifacts 'Activity','LocationUnit','Mapping','ActualTime',1|September 3 2015");

            databaseVersions.Add(new Guid("0DC08FDA-412F-491D-B479-9A8A27FC5151"), "REBUILDVIEWS|September 3 2015");

            #endregion

            #region 8 September 2015

            databaseVersions.Add(new Guid("789F22BD-D235-4FCE-A0BA-3BF07F0515A4"), "EXEC db_AddPossibleValues 'LocationUnitType','Country',1,'Country',1|September 8 2015");
            databaseVersions.Add(new Guid("8774DBE5-4242-45A5-A59D-EBA151DAB2A2"), "EXEC db_AddPossibleValues 'LocationUnitType','Province',2,'Province',1|September 8 2015");
            databaseVersions.Add(new Guid("BAC22149-6A71-4F33-81E6-BFC0D3F6874E"), "EXEC db_AddPossibleValues 'LocationUnitType','Region',3,'Region',1|September 8 2015");
            databaseVersions.Add(new Guid("32ECE2C2-31AA-498C-8ACD-502026690C0B"), "EXEC db_AddPossibleValues 'LocationUnitType','Area',4,'Area',1|September 8 2015");
            databaseVersions.Add(new Guid("A66CF4DD-6ACD-451A-B984-24E14DDF6865"), "EXEC db_AddPossibleValues 'LocationUnitType','Town',5,'Town',1|September 8 2015");
            databaseVersions.Add(new Guid("3FEB1FDE-C9F1-425B-9D96-5918E5C8DC5E"), "EXEC db_AddPossibleValues 'LocationUnitType','Zone',6,'Zone',1|September 8 2015");
            databaseVersions.Add(new Guid("E15BA725-6003-4CC7-BD86-D97654FECDAC"), "EXEC db_AddPossibleValues 'LocationUnitType','Section',7,'Section',1|September 8 2015");
            databaseVersions.Add(new Guid("7B6F2EF2-5D1D-4860-A9C2-E80AD921297F"), "EXEC db_AddPossibleValues 'LocationUnitType','Block',8,'Block',1|September 8 2015");
            databaseVersions.Add(new Guid("A1C539C2-8DB8-48B3-BF47-F6A4E676A5F3"), "EXEC db_AddPossibleValues 'LocationUnitType','Panel',9,'Panel',1|September 8 2015");

            databaseVersions.Add(new Guid("306D6C05-D0E4-46F7-AEE1-A80C34D8CFCB"), "EXEC db_AddFields 'LocationUnit','LocationUnitType','LocationUnitType','General','',1,5,1|September 8 2015");

            databaseVersions.Add(new Guid("5A0F6B41-16DA-498F-89CB-93178610999A"), "EXEC db_AddClassAssociations 'Location','LocationUnit','Mapping','',1,1|September 8 2015");

            #endregion

            #region 10 September 2015

            databaseVersions.Add(new Guid("BC6DA212-F06C-48A2-A6E4-443DE53D7E1F"), "EXEC db_AddFields 'Requirement','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',1,1,1|September 10 2015");
            databaseVersions.Add(new Guid("C41E27A8-CBB6-44BE-841B-3DD9A90A56B6"), "EXEC db_AddFields 'Requirement','Abbreviation','System.String','General','',1,2,1|September 10 2015");
            databaseVersions.Add(new Guid("E01BC24E-6A63-4C86-AD54-00CD2E56D182"), "EXEC db_AddFields 'Requirement','Description','System.String','General','General description for each object',1,3,1|September 10 2015");

            databaseVersions.Add(new Guid("855F90B2-6EAE-42D8-AAD8-AD1BFFB184ED"), "EXEC db_AddFields 'Requirement','RequirementType','System.String','General','',1,4,1|September 10 2015");

            databaseVersions.Add(new Guid("6B30A7F6-A648-4E8B-8179-5E081029D7E3"), "EXEC db_AddFields 'Requirement','DesignRationale','System.String','General','',1,82,1|September 10 2015");
            databaseVersions.Add(new Guid("FC14C666-0146-453B-980A-0B771179F4F0"), "EXEC db_AddFields 'Requirement','GapType','GapType','General','Result of a gap analysis between architecture states.',1,84,1|September 10 2015");
            databaseVersions.Add(new Guid("BCD6510E-1A5E-468D-8D67-F285D34FF716"), "EXEC db_AddFields 'Requirement','DataSourceID','System.String','General','Unique identifier (foreign key) for an object as per the data source.',1,85,1|September 10 2015");
            databaseVersions.Add(new Guid("12B74FF3-28AF-482E-AAF7-EAB39B2339D2"), "EXEC db_AddFields 'Requirement','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',1,86,1|September 10 2015");

            databaseVersions.Add(new Guid("EC0794DE-6CFC-4751-8C27-508AB55DE47E"), "REBUILDSINGLEVIEWS-Requirement-|September 10 2015");

            #endregion

            #region 5 October 2015

            databaseVersions.Add(new Guid("D9D7FB9D-D50D-48F7-A7E4-F5BFFC170C65"), "EXEC db_AddClasses 'LogicalDataComponent','Name','ITInfrastructure',1|5 October 2015");

            databaseVersions.Add(new Guid("F57566F1-A832-41D7-B187-EE78A0992C9F"), "EXEC db_addFields 'LogicalDataComponent','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',1,1,1|5 October 2015");
            databaseVersions.Add(new Guid("28166C4E-F26B-4842-8452-584AAB95246E"), "EXEC db_addFields 'LogicalDataComponent','Abbreviation','System.String','General','',1,2,1|5 October 2015");
            databaseVersions.Add(new Guid("BE2DDA70-E9ED-40DF-ACB5-B97F27F2180F"), "EXEC db_addFields 'LogicalDataComponent','Synonyms','System.String','General','',1,2,1|5 October 2015");
            databaseVersions.Add(new Guid("A3A78CC0-AF34-4F8F-8B5F-2996A2F1C039"), "EXEC db_addFields 'LogicalDataComponent','Description','System.String','General','General description for each object',1,3,1|5 October 2015");
            databaseVersions.Add(new Guid("4FA13926-5703-4F60-BC15-93EBE782BB42"), "EXEC db_addFields 'LogicalDataComponent','IsBusinessExternal','IsBusinessExternal','General','Object is managed and owned by an external entity given the business context.',1,4,1|5 October 2015");
            databaseVersions.Add(new Guid("5F0D500A-F8A0-46D9-A0E7-0D97405C6276"), "EXEC db_addFields 'LogicalDataComponent','DataComponentType','DataComponentType','General','',1,5,1|5 October 2015");
            databaseVersions.Add(new Guid("15C3673C-95A6-4E93-A8BD-9C54A542D1A6"), "EXEC db_addFields 'LogicalDataComponent','DatabaseType','DatabaseType','General','',1,6,1|5 October 2015");
            databaseVersions.Add(new Guid("67FEFEEA-3DD8-4913-8173-9BD269FE1B0D"), "EXEC db_addFields 'LogicalDataComponent','SecurityClassification','SecurityClassification','General','',1,7,1|5 October 2015");
            databaseVersions.Add(new Guid("A1E3EAC8-CEEF-4712-812D-4F9B5B099CBF"), "EXEC db_addFields 'LogicalDataComponent','IsMasterDataSource','System.String','General','',1,7,1|5 October 2015");
            databaseVersions.Add(new Guid("47394F7C-837E-4C1D-A1F8-A23FCAAC7879"), "EXEC db_addFields 'LogicalDataComponent','ContentType','ContentType','General','',1,8,1|5 October 2015");
            databaseVersions.Add(new Guid("BF269222-D53C-4663-A74F-3F4C0DAAD69D"), "EXEC db_addFields 'LogicalDataComponent','CriticalityLevel','System.String','General','',1,13,1|5 October 2015");
            databaseVersions.Add(new Guid("8DBADE2D-B7E2-45DD-87E3-3CDFBE0B8A72"), "EXEC db_addFields 'LogicalDataComponent','ArchitectureStatus','ArchitectureStatus','General','Life cycle status for all object to depict transition from evaluated to standard and to decommissioned.',1,80,1|5 October 2015");
            databaseVersions.Add(new Guid("916A416F-E31E-47B6-885C-B5887CFB77A1"), "EXEC db_addFields 'LogicalDataComponent','ArchitectureStatusDate','System.String','General','Effective date of standardisation status.',1,81,1|5 October 2015");
            databaseVersions.Add(new Guid("0F9588C5-2E5F-4216-8504-341169734B1F"), "EXEC db_addFields 'LogicalDataComponent','DesignRationale','System.String','General','',1,82,1|5 October 2015");
            databaseVersions.Add(new Guid("8A69EEA8-3AF9-449D-A250-21DA0A0AA4B8"), "EXEC db_addFields 'LogicalDataComponent','GeneralRemarks','System.String','General','General remarks for each object such as notes for future reference.',1,83,1|5 October 2015");
            databaseVersions.Add(new Guid("1C886D83-EB44-4AA8-A26E-A475C3A39029"), "EXEC db_addFields 'LogicalDataComponent','GapType','GapType','General','Result of a gap analysis between architecture states.',1,84,1|5 October 2015");
            databaseVersions.Add(new Guid("B31A2FA1-F727-4FDA-93FF-761362727D5F"), "EXEC db_addFields 'LogicalDataComponent','DataSourceID','System.String','General','Unique identifier (foreign key) for an object as per the data source.',1,85,1|5 October 2015");
            databaseVersions.Add(new Guid("573451A1-22AC-4EBC-9577-D995444B1470"), "EXEC db_addFields 'LogicalDataComponent','DataSourceName','System.String','General','Name of external data source where object and properties are sourced from.',1,86,1|5 October 2015");
            databaseVersions.Add(new Guid("B12DBC4D-61BB-4E1F-93DC-84932909E59D"), "EXEC db_addFields 'LogicalDataComponent','DataUpdateStatus','DataUpdateStatusType','General','',1,100,1|5 October 2015");
            databaseVersions.Add(new Guid("FBCE339A-1CCA-4AC9-92E0-46C68B08C474"), "EXEC db_addFields 'LogicalDataComponent','DataUpdateStatusDate','System.String','General','',1,100,1|5 October 2015");

            databaseVersions.Add(new Guid("0F57111E-D494-446E-8819-9B688F56FE24"), "EXEC db_addClassAssociations 'LogicalDataComponent','PhysicalSoftwareComponent','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("A6885114-9F8F-4C91-A400-2E8597C1238D"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','LogicalDataComponent','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("85ED385D-1570-4019-B3C9-E19E06CF232D"), "EXEC db_addClassAssociations 'LogicalDataComponent','ComputingComponent','Mapping','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("D829E111-FEE4-469B-AB91-9C42C44A3C04"), "EXEC db_addClassAssociations 'ComputingComponent','LogicalDataComponent','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("1155B8E8-FFAD-4E5E-A668-875026F5F696"), "EXEC db_addClassAssociations 'LogicalDataComponent','NetworkComponent','Mapping','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("2FE34CD2-3465-4AD0-8529-2A7D38F2D613"), "EXEC db_addClassAssociations 'NetworkComponent','LogicalDataComponent','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("ECC24DAD-9955-4A47-891F-66DA3378D9E6"), "EXEC db_addClassAssociations 'LogicalDataComponent','GovernanceMechanism','Mapping','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("F29BD159-A9E1-41C6-8659-6F02683AA145"), "EXEC db_addClassAssociations 'GovernanceMechanism','LogicalDataComponent','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("C5FF4B86-0E76-4C18-A7FC-67B35473BB02"), "EXEC db_addClassAssociations 'LogicalDataComponent','Function','Mapping','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("F9D09B60-A439-4AA5-8F36-1490A559A6C5"), "EXEC db_addClassAssociations 'Function','LogicalDataComponent','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("CC5437DE-E459-45C3-88C1-31166D83CC3B"), "EXEC db_addClassAssociations 'LogicalDataComponent','Process','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("97F8A85E-D91A-4791-9FBC-BAA567148DD9"), "EXEC db_addClassAssociations 'Process','LogicalDataComponent','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("FE10B188-D169-4ECF-9495-047D08402AA5"), "EXEC db_addClassAssociations 'LogicalDataComponent','Activity','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("F9705E32-8B53-47A7-94D9-6AB997C5251C"), "EXEC db_addClassAssociations 'Activity','LogicalDataComponent','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("850E2E1C-9379-4271-BEAE-691C3B44FD84"), "EXEC db_addClassAssociations 'LogicalDataComponent','Object','Mapping','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("7ADF19E2-86B0-4746-9DBC-8E6953FE33FA"), "EXEC db_addClassAssociations 'Object','LogicalDataComponent','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("72C4B83F-1D43-43DE-8EAB-9E716CD6138C"), "EXEC db_addClassAssociations 'LogicalDataComponent','Function','DynamicFlow','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("01BFC980-980A-4888-AAE0-6AB7B234AB35"), "EXEC db_addClassAssociations 'LogicalDataComponent','Process','DynamicFlow','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("2882CED6-71A3-451E-A553-3D5F8E710ACD"), "EXEC db_addClassAssociations 'LogicalDataComponent','Activity','DynamicFlow','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("D1ECDABF-686F-4C87-9A44-79D760135619"), "EXEC db_addClassAssociations 'LogicalDataComponent','LogicalDataComponent','DynamicFlow','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("D0738F04-6962-4805-B986-E178CBD1451C"), "EXEC db_addClassAssociations 'LogicalDataComponent','PhysicalSoftwareComponent','DynamicFlow','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("204F714E-9F53-4FDB-B6DA-ACE442DE1C42"), "EXEC db_addClassAssociations 'PhysicalSoftwareComponent','LogicalDataComponent','DynamicFlow','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("B881377C-5B6D-4913-8D37-EAFFFE2C81EF"), "EXEC db_addClassAssociations 'LogicalDataComponent','Object','DynamicFlow','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("5CC7A6EE-0ED7-45E9-85DE-EC4D06F2BAD8"), "EXEC db_addClassAssociations 'Object','LogicalDataComponent','DynamicFlow','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("8181048B-FBD9-4DFD-BAC4-C22433C4562F"), "EXEC db_addClassAssociations 'DataView','LogicalDataComponent','Mapping','',1,0|5 October 2015");
            databaseVersions.Add(new Guid("314929C3-E3EA-41FC-BBFB-E1648B6F8C5D"), "EXEC db_addClassAssociations 'LogicalDataComponent','DataView','Mapping','',0,0|5 October 2015");
            databaseVersions.Add(new Guid("0F4178AD-5409-49E1-A889-9A2F576DB3CC"), "EXEC db_addClassAssociations 'LogicalDataComponent','DataTable','Mapping','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("5641EA5C-26EF-4712-BF27-B26FA47EB6C3"), "EXEC db_addClassAssociations 'DataTable','LogicalDataComponent','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("0E7CA660-B894-40E8-B24E-9A010E740014"), "EXEC db_addClassAssociations 'LogicalDataComponent','DataSchema','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("5DDFA4B5-5CCB-4848-A23E-971F75FB0EC9"), "EXEC db_addClassAssociations 'DataSchema','LogicalDataComponent','Mapping','',0,0|5 October 2015");
            databaseVersions.Add(new Guid("D8C72FF1-F209-4E63-A729-6E16994537CA"), "EXEC db_addClassAssociations 'ApplicationInterface','LogicalDataComponent','DynamicFlow','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("69C79845-B0EF-4A5F-813C-FD29DB365E3E"), "EXEC db_addClassAssociations 'LogicalDataComponent','ApplicationInterface','DynamicFlow','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("B18F1E98-A3C4-46EE-89D7-3F537D2B6AE1"), "EXEC db_addClassAssociations 'ApplicationInterface','LogicalDataComponent','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("5CAC4846-3240-42B0-8BD2-72AD3119544D"), "EXEC db_addClassAssociations 'LogicalDataComponent','ApplicationInterface','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("E62436F4-9198-46BD-B79F-67CA2109FB50"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','LogicalDataComponent','Mapping','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("B1F201AD-7370-4981-B431-5ABD009A9E68"), "EXEC db_addClassAssociations 'LogicalDataComponent','PhysicalInformationArtefact','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("8F833D3B-D68C-4E1E-92B0-9E2F801AB2B6"), "EXEC db_addClassAssociations 'LogicalInformationArtefact','LogicalDataComponent','Mapping','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("97BAC57A-C74B-4530-A979-38EC4F77F250"), "EXEC db_addClassAssociations 'LogicalDataComponent','LogicalInformationArtefact','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("D6FC60AA-4263-4054-92EB-A4A1BCC94EA3"), "EXEC db_addClassAssociations 'ITInfrastructureEnvironment','LogicalDataComponent','Mapping','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("C7BC7D58-B02D-481D-A764-158DF52EFDFB"), "EXEC db_addClassAssociations 'LogicalDataComponent','ITInfrastructureEnvironment','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("73A2227C-9BEE-4E2C-8736-11D12F4CDC42"), "EXEC db_addClassAssociations 'Process','LogicalDataComponent','DynamicFlow','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("5FAF5DBD-3533-49A9-A8CD-20809DBB1267"), "EXEC db_addClassAssociations 'LogicalDataComponent','DataSubjectArea','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("109A8CFB-717B-46FE-B0CC-338C569A6E64"), "EXEC db_addClassAssociations 'DataSubjectArea','LogicalDataComponent','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("1F98E623-59C2-4E97-939E-38F0D876CCC1"), "EXEC db_addClassAssociations 'DataDomain','LogicalDataComponent','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("CDD83704-1A29-476B-BEA1-9EE4DF49EDFE"), "EXEC db_addClassAssociations 'LogicalDataComponent','DataDomain','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("903BECE6-C8A9-408D-A900-958CB095200F"), "EXEC db_addClassAssociations 'LogicalSoftwareComponent','LogicalDataComponent','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("6D354706-4D2E-4772-B914-8925E425CE52"), "EXEC db_addClassAssociations 'LogicalDataComponent','LogicalSoftwareComponent','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("1F8A4C1B-9042-48E6-A6EE-139670217257"), "EXEC db_addClassAssociations 'Measure','LogicalDataComponent','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("DDA7DC9F-C276-405A-87E0-7CAEF078B5CC"), "EXEC db_addClassAssociations 'LogicalDataComponent','Measure','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("717DB02F-D525-4300-ACFC-96D3B1E709BA"), "EXEC db_addClassAssociations 'Risk','LogicalDataComponent','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("BC2EF9BA-9A9E-4EA2-B4BB-32DB013FC2D4"), "EXEC db_addClassAssociations 'LogicalDataComponent','Risk','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("DF0EFB83-FA17-40D6-BD33-9E5D2731B062"), "EXEC db_addClassAssociations 'Stakeholder','LogicalDataComponent','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("4E2A8CE8-31BB-4C8E-850B-B3AD3B9C101F"), "EXEC db_addClassAssociations 'LogicalDataComponent','Stakeholder','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("43A9A993-1A81-4319-9747-BB4F96CC98C0"), "EXEC db_addClassAssociations 'Sys','LogicalDataComponent','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("525D0591-5D46-4F8B-8442-FE69C029F81E"), "EXEC db_addClassAssociations 'LogicalDataComponent','Sys','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("894B7804-6FFB-453D-90D3-0E259EF6A4C0"), "EXEC db_addClassAssociations 'Task','LogicalDataComponent','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("96202794-4A10-4D11-AD84-E74AE4B7A2B6"), "EXEC db_addClassAssociations 'LogicalDataComponent','Task','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("B84AFBFB-4646-4BF4-A4F2-184E46B75331"), "EXEC db_addClassAssociations 'Task','LogicalDataComponent','Create','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("4001999A-42A4-438A-A456-B36BB7737B02"), "EXEC db_addClassAssociations 'Task','LogicalDataComponent','Update','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("447FE3F2-7204-4F49-B2E9-1624F00A1B8E"), "EXEC db_addClassAssociations 'Task','LogicalDataComponent','Maintain','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("2CA2A6E2-A483-4844-B1EC-C73293D9FF50"), "EXEC db_addClassAssociations 'Task','LogicalDataComponent','Delete','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("4D297115-A9F6-4F25-8DC9-7ED6806510FE"), "EXEC db_addClassAssociations 'WorkPackage','LogicalDataComponent','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("B25FAF90-8255-4EDF-9E18-DC73153F0B39"), "EXEC db_addClassAssociations 'LogicalDataComponent','WorkPackage','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("1093D63F-A6A9-47C5-B2AA-4EC381CD6A63"), "EXEC db_addClassAssociations 'DataValue','LogicalDataComponent','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("3CB02A6A-FF5F-48FC-869F-AB8F1999B6DD"), "EXEC db_addClassAssociations 'LogicalDataComponent','DataValue','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("028240EA-ADAB-4AB6-B21D-4580F3840191"), "EXEC db_addClassAssociations 'OrganizationalUnit','LogicalDataComponent','Mapping','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("F1F2FCBF-CBD6-433F-863E-D25B264E4461"), "EXEC db_addClassAssociations 'LogicalDataComponent','OrganizationalUnit','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("A4EAF1B1-04BC-4EE0-99B0-E8EEF3B8ED1C"), "EXEC db_addClassAssociations 'Job','LogicalDataComponent','Mapping','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("AD62B000-A14D-4F8D-9877-05F0F5FCA390"), "EXEC db_addClassAssociations 'LogicalDataComponent','Job','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("55C0F358-ED4C-4888-BB83-22034F62DFCC"), "EXEC db_addClassAssociations 'Position','LogicalDataComponent','Mapping','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("4871A04D-C18F-4869-9518-36E9308D911D"), "EXEC db_addClassAssociations 'LogicalDataComponent','Position','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("EE9E7ECC-AB89-40BD-9C78-10675BC1CE72"), "EXEC db_addClassAssociations 'Role','LogicalDataComponent','Mapping','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("A9A8DBFE-28EA-4599-A069-0C7DC31729D2"), "EXEC db_addClassAssociations 'LogicalDataComponent','Role','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("36040E4C-9D6A-40D8-8386-6CC20A727D16"), "EXEC db_addClassAssociations 'Person','LogicalDataComponent','Mapping','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("C0B50B01-A840-4131-BA7B-79852B0DB66A"), "EXEC db_addClassAssociations 'LogicalDataComponent','Person','Mapping','',0,1|5 October 2015");
            databaseVersions.Add(new Guid("CD2ECAF8-367B-49EE-95E8-76D687CC9927"), "EXEC db_addClassAssociations 'LogicalDataComponent','LogicalDataComponent','Mapping','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("16D34419-3087-41FB-BF0D-2ECABA2F553D"), "EXEC db_addClassAssociations 'PhysicalInformationArtefact','LogicalDataComponent','DynamicFlow','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("D6E94AE8-FD33-4A73-8A59-C9FF912321F4"), "EXEC db_addClassAssociations 'LogicalDataComponent','PhysicalInformationArtefact','DynamicFlow','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("BBA698D2-B865-41C6-B38B-D507AF866A73"), "EXEC db_addClassAssociations 'LogicalDataComponent','OrganizationalUnit','DynamicFlow','',1,1|5 October 2015");
            databaseVersions.Add(new Guid("9AE4C318-27C8-429F-BA98-221FCCC6FF3B"), "EXEC db_addClassAssociations 'OrganizationalUnit','LogicalDataComponent','DynamicFlow','',1,1|5 October 2015");

            databaseVersions.Add(new Guid("49362CE0-7C79-4856-9D78-F0C40F4B7C1A"), "EXEC db_addArtifacts 'LogicalDataComponent','PhysicalSoftwareComponent','Mapping','Rationale',1|5 October 2015");
            databaseVersions.Add(new Guid("007C1C40-4595-44A1-8ECB-CE0E21CD6E6F"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','LogicalDataComponent','Mapping','Rationale',1|5 October 2015");
            databaseVersions.Add(new Guid("B5BFBB8D-DCD8-4397-BA04-EF4EF895C33A"), "EXEC db_addArtifacts 'LogicalDataComponent','ComputingComponent','Mapping','Rationale',1|5 October 2015");
            databaseVersions.Add(new Guid("57850F50-AB99-4656-821C-0584C3922F28"), "EXEC db_addArtifacts 'LogicalDataComponent','NetworkComponent','Mapping','Rationale',1|5 October 2015");
            databaseVersions.Add(new Guid("371ACFC5-7E1D-4AB7-ACEF-91CFA12EF3FF"), "EXEC db_addArtifacts 'NetworkComponent','LogicalDataComponent','Mapping','Rationale',1|5 October 2015");
            databaseVersions.Add(new Guid("7A9913C5-877E-49DA-84DB-A5FA4ED72DB4"), "EXEC db_addArtifacts 'LogicalDataComponent','GovernanceMechanism','Mapping','Automation',1|5 October 2015");
            databaseVersions.Add(new Guid("D2D498DF-7BCF-4165-93B5-01DB0600B1EF"), "EXEC db_addArtifacts 'LogicalDataComponent','GovernanceMechanism','Mapping','Rationale',1|5 October 2015");
            databaseVersions.Add(new Guid("EA77F756-602C-43AD-99D9-3A0B03294D3F"), "EXEC db_addArtifacts 'GovernanceMechanism','LogicalDataComponent','Mapping','Automation',1|5 October 2015");
            databaseVersions.Add(new Guid("11D5AED7-244A-480B-A4CB-8F79BD14A720"), "EXEC db_addArtifacts 'GovernanceMechanism','LogicalDataComponent','Mapping','Rationale',1|5 October 2015");
            databaseVersions.Add(new Guid("1266E1CD-EE1F-4B3A-AA92-B85C62B05C15"), "EXEC db_addArtifacts 'LogicalDataComponent','Function','Mapping','Rationale',1|5 October 2015");
            databaseVersions.Add(new Guid("34A623E9-CF72-4A1A-BB71-ACF083019E96"), "EXEC db_addArtifacts 'Function','LogicalDataComponent','Mapping','Rationale',1|5 October 2015");
            databaseVersions.Add(new Guid("96B82104-4698-41AE-ACF1-5A656A8A615B"), "EXEC db_addArtifacts 'LogicalDataComponent','Process','Mapping','Rationale',1|5 October 2015");
            databaseVersions.Add(new Guid("AFDCA926-6740-4A5A-A2CA-F5D6C0940BFE"), "EXEC db_addArtifacts 'Process','LogicalDataComponent','Mapping','Rationale',1|5 October 2015");
            databaseVersions.Add(new Guid("E7B982E6-93DC-4A95-B605-7CCF41A18395"), "EXEC db_addArtifacts 'LogicalDataComponent','Activity','Mapping','Rationale',0|5 October 2015");
            databaseVersions.Add(new Guid("C1D51553-0D63-4343-886B-AB2AC53894E2"), "EXEC db_addArtifacts 'Activity','LogicalDataComponent','Mapping','Rationale',1|5 October 2015");
            databaseVersions.Add(new Guid("F49D6C41-6A4F-4B8B-815A-9D31FAF71FAA"), "EXEC db_addArtifacts 'LogicalDataComponent','Object','Mapping','Rationale',1|5 October 2015");
            databaseVersions.Add(new Guid("32D4A35A-7BB4-4493-93B8-48616D8B2C53"), "EXEC db_addArtifacts 'Object','LogicalDataComponent','Mapping','Rationale',1|5 October 2015");
            databaseVersions.Add(new Guid("810BD60E-F14E-479C-8DBA-64AA73814B93"), "EXEC db_addArtifacts 'LogicalDataComponent','Function','DynamicFlow','FlowDescription',1|5 October 2015");
            databaseVersions.Add(new Guid("284A6E1C-7F22-4B98-BE85-7E8589481598"), "EXEC db_addArtifacts 'LogicalDataComponent','Process','DynamicFlow','FlowDescription',1|5 October 2015");
            databaseVersions.Add(new Guid("38A4AD6F-3D68-4D40-A76A-9C1AC0CB7680"), "EXEC db_addArtifacts 'LogicalDataComponent','Activity','DynamicFlow','FlowDescription',1|5 October 2015");
            databaseVersions.Add(new Guid("E07860EC-6C78-4EFC-BABF-2AAE03AC51BB"), "EXEC db_addArtifacts 'LogicalDataComponent','LogicalDataComponent','DynamicFlow','DataSubjectArea',1|5 October 2015");
            databaseVersions.Add(new Guid("5E7287EE-586B-431E-BF3E-77A18692924D"), "EXEC db_addArtifacts 'LogicalDataComponent','LogicalDataComponent','DynamicFlow','FlowDescription',1|5 October 2015");
            databaseVersions.Add(new Guid("9F9A85E0-BD89-4787-80B5-BA64ACE46D9A"), "EXEC db_addArtifacts 'LogicalDataComponent','LogicalDataComponent','DynamicFlow','PhysicalInformationArtefact',1|5 October 2015");
            databaseVersions.Add(new Guid("206EF6BB-B1E2-44DC-86D1-92D56D9DF2FD"), "EXEC db_addArtifacts 'LogicalDataComponent','PhysicalSoftwareComponent','DynamicFlow','DataSubjectArea',1|5 October 2015");
            databaseVersions.Add(new Guid("F7B56683-ED4B-4E36-BA21-F06E386F9029"), "EXEC db_addArtifacts 'LogicalDataComponent','PhysicalSoftwareComponent','DynamicFlow','FlowDescription',1|5 October 2015");
            databaseVersions.Add(new Guid("89A9F1A2-58D8-4634-A21A-F9736060558F"), "EXEC db_addArtifacts 'LogicalDataComponent','PhysicalSoftwareComponent','DynamicFlow','PhysicalInformationArtefact',1|5 October 2015");
            databaseVersions.Add(new Guid("DB9F2464-BFA4-400C-B5E8-B72294EF2CC3"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','LogicalDataComponent','DynamicFlow','DataSubjectArea',1|5 October 2015");
            databaseVersions.Add(new Guid("B5C3FAD3-1265-433A-80B5-21DC67954449"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','LogicalDataComponent','DynamicFlow','FlowDescription',1|5 October 2015");
            databaseVersions.Add(new Guid("D93206E8-D9C3-4F4D-B8D6-8F9F4F46E393"), "EXEC db_addArtifacts 'PhysicalSoftwareComponent','LogicalDataComponent','DynamicFlow','PhysicalInformationArtefact',1|5 October 2015");
            databaseVersions.Add(new Guid("71FC875B-6081-4C46-9946-DD75743F69F5"), "EXEC db_addArtifacts 'LogicalDataComponent','Object','DynamicFlow','FlowDescription',1|5 October 2015");
            databaseVersions.Add(new Guid("C1C3B81A-94CE-460A-A700-5451198E605F"), "EXEC db_addArtifacts 'LogicalDataComponent','Object','DynamicFlow','PhysicalInformationArtefact',1|5 October 2015");
            databaseVersions.Add(new Guid("9D797FE9-EBAC-43BB-B64E-507F0D630AA3"), "EXEC db_addArtifacts 'Object','LogicalDataComponent','DynamicFlow','FlowDescription',1|5 October 2015");
            databaseVersions.Add(new Guid("AC410208-0075-4807-BDB5-BD67E0148478"), "EXEC db_addArtifacts 'Object','LogicalDataComponent','DynamicFlow','PhysicalInformationArtefact',1|5 October 2015");
            databaseVersions.Add(new Guid("D7AD672D-CB0E-4805-955F-F6A5049A1555"), "EXEC db_addArtifacts 'DataView','LogicalDataComponent','Mapping','Rationale',1|5 October 2015");
            databaseVersions.Add(new Guid("07D8F4B3-2648-407E-8955-E0F45A542FED"), "EXEC db_addArtifacts 'LogicalDataComponent','DataView','Mapping','Rationale',0|5 October 2015");
            databaseVersions.Add(new Guid("23093C29-FFF7-4FD0-907C-9FEA1008EA64"), "EXEC db_addArtifacts 'LogicalDataComponent','DataTable','Mapping','Rationale',1|5 October 2015");
            databaseVersions.Add(new Guid("8ACC5F85-45C3-4259-A3C9-61CBE9A1ACAF"), "EXEC db_addArtifacts 'DataTable','LogicalDataComponent','Mapping','Rationale',1|5 October 2015");
            databaseVersions.Add(new Guid("6226F161-1809-44DA-8368-866B9A95E7B1"), "EXEC db_addArtifacts 'LogicalDataComponent','DataSchema','Mapping','Rationale',0|5 October 2015");
            databaseVersions.Add(new Guid("2DCCA4AC-BF0B-4CCB-B69D-0E4AF3E16148"), "EXEC db_addArtifacts 'DataSchema','LogicalDataComponent','Mapping','Rationale',0|5 October 2015");
            databaseVersions.Add(new Guid("07479686-0CEF-40D1-953B-18A205735955"), "EXEC db_addArtifacts 'ApplicationInterface','LogicalDataComponent','DynamicFlow','PhysicalInformationArtefact',1|5 October 2015");
            databaseVersions.Add(new Guid("64EEEEB8-351E-40B2-872F-EB0D4458AADA"), "EXEC db_addArtifacts 'LogicalDataComponent','ApplicationInterface','DynamicFlow','PhysicalInformationArtefact',1|5 October 2015");
            databaseVersions.Add(new Guid("303F1FD9-587C-41E7-B6EB-A9D0FF8477EE"), "EXEC db_addArtifacts 'ApplicationInterface','LogicalDataComponent','Mapping','Rationale',1|5 October 2015");
            databaseVersions.Add(new Guid("A0B8935C-4817-43B4-A0FF-05C55E1A508D"), "EXEC db_addArtifacts 'LogicalDataComponent','ApplicationInterface','Mapping','Rationale',0|5 October 2015");
            databaseVersions.Add(new Guid("F7B55C1A-CDBC-477D-ABC1-4BF0603E0657"), "EXEC db_addArtifacts 'PhysicalInformationArtefact','LogicalDataComponent','Mapping','Rationale',1|5 October 2015");
            databaseVersions.Add(new Guid("1897B3AC-7477-4F4B-BC71-463176483CA0"), "EXEC db_addArtifacts 'LogicalDataComponent','PhysicalInformationArtefact','Mapping','Rationale',1|5 October 2015");
            databaseVersions.Add(new Guid("6E784DB5-E2F7-4FEF-82A0-5C2919DD7A62"), "EXEC db_addArtifacts 'LogicalInformationArtefact','LogicalDataComponent','Mapping','Rationale',1|5 October 2015");
            databaseVersions.Add(new Guid("C5A8823F-A658-471F-889A-7664941A9F97"), "EXEC db_addArtifacts 'LogicalDataComponent','LogicalInformationArtefact','Mapping','Rationale',1|5 October 2015");
            databaseVersions.Add(new Guid("4B1104C4-EBF3-4206-87CB-7565F5F3B65E"), "EXEC db_addArtifacts 'ITInfrastructureEnvironment','LogicalDataComponent','Mapping','Rationale',1|5 October 2015");
            databaseVersions.Add(new Guid("443E36AA-DF6D-4574-8DDE-4BCB01F84F82"), "EXEC db_addArtifacts 'LogicalDataComponent','ITInfrastructureEnvironment','Mapping','Rationale',1|5 October 2015");
            databaseVersions.Add(new Guid("E8C1E62F-DCF6-4C34-8905-955A2D95C073"), "EXEC db_addArtifacts 'Process','LogicalDataComponent','DynamicFlow','FlowDescription',1|5 October 2015");
            databaseVersions.Add(new Guid("161FDAA2-289E-407B-ADD3-2B40D42B05BE"), "EXEC db_addArtifacts 'LogicalDataComponent','DataSubjectArea','Mapping','Rationale',0|5 October 2015");
            databaseVersions.Add(new Guid("BF52FEF6-ACA5-4186-827F-6D7B196EEA88"), "EXEC db_addArtifacts 'DataSubjectArea','LogicalDataComponent','Mapping','Rationale',1|5 October 2015");
            databaseVersions.Add(new Guid("EE081D28-F9DC-426D-82BA-B798A491AAD5"), "EXEC db_addArtifacts 'Task','LogicalDataComponent','Mapping','Rationale',1|5 October 2015");
            databaseVersions.Add(new Guid("04016113-B416-4715-A096-95B0992DCAA2"), "EXEC db_addArtifacts 'Task','LogicalDataComponent','Create','FlowDescription',1|5 October 2015");
            databaseVersions.Add(new Guid("F5462658-C0FD-4A83-8F16-CDEA0F1FBBF3"), "EXEC db_addArtifacts 'Task','LogicalDataComponent','Update','FlowDescription',1|5 October 2015");
            databaseVersions.Add(new Guid("4ACFC0B8-A131-4C9B-A48F-83652D250A6A"), "EXEC db_addArtifacts 'Task','LogicalDataComponent','Maintain','FlowDescription',1|5 October 2015");
            databaseVersions.Add(new Guid("8187781A-BA02-4958-81DD-05A30763983F"), "EXEC db_addArtifacts 'Task','LogicalDataComponent','Delete','FlowDescription',1|5 October 2015");
            databaseVersions.Add(new Guid("AE9FF0A9-C0B8-4E4F-9443-2A0513196413"), "EXEC db_addArtifacts 'OrganizationalUnit','LogicalDataComponent','Mapping','Role',1|5 October 2015");
            databaseVersions.Add(new Guid("45C371BD-BB32-4232-87A0-E9F413542B08"), "EXEC db_addArtifacts 'Job','LogicalDataComponent','Mapping','Role',1|5 October 2015");
            databaseVersions.Add(new Guid("24ABD6CE-5475-41A6-A8E2-06CC341E9152"), "EXEC db_addArtifacts 'Position','LogicalDataComponent','Mapping','Role',1|5 October 2015");
            databaseVersions.Add(new Guid("4D1D5701-24E1-4E57-9516-1664ED7F5A4D"), "EXEC db_addArtifacts 'Role','LogicalDataComponent','Mapping','Role',1|5 October 2015");
            databaseVersions.Add(new Guid("61E4C9B7-825C-4300-9BA3-833363918D28"), "EXEC db_addArtifacts 'Person','LogicalDataComponent','Mapping','Role',1|5 October 2015");
            databaseVersions.Add(new Guid("0B10D449-CB47-464C-AA24-CF2ADFC576C4"), "EXEC db_addArtifacts 'LogicalDataComponent','OrganizationalUnit','DynamicFlow','DataSubjectArea',1|5 October 2015");
            databaseVersions.Add(new Guid("90933F7B-CD22-4F85-A2F3-2C1D7675C4BF"), "EXEC db_addArtifacts 'LogicalDataComponent','OrganizationalUnit','DynamicFlow','PhysicalInformationArtefact',1|5 October 2015");
            databaseVersions.Add(new Guid("79E1D932-5AAD-4D70-81E3-2BD816B7B7DA"), "EXEC db_addArtifacts 'OrganizationalUnit','LogicalDataComponent','DynamicFlow','DataSubjectArea',1|5 October 2015");
            databaseVersions.Add(new Guid("0A7F0BC0-C71A-46C3-9D99-432472FC633B"), "EXEC db_addArtifacts 'OrganizationalUnit','LogicalDataComponent','DynamicFlow','PhysicalInformationArtefact',1|5 October 2015");

            databaseVersions.Add(new Guid("A2499414-EB3E-4F92-AC15-A86B3735EA2C"), "REBUILDSINGLEVIEWS-LogicalDataComponent-|5 October 2015");

            #endregion

            #region 3 November 2015

            databaseVersions.Add(new Guid("94EBF199-4E49-4F4D-842C-F782363D9393"), "exec db_addclassassociations 'DataTable','ApplicationInterface','DynamicFlow','',1,1|3 November 2015");
            databaseVersions.Add(new Guid("ED4CA530-FA32-4473-AE9E-D8C11FE4D735"), "exec db_addclassassociations 'ApplicationInterface','DataTable','DynamicFlow','',1,1|3 November 2015");
            databaseVersions.Add(new Guid("DE8656A9-2A41-492A-8E87-6E7FDA758ADC"), "exec db_addclassassociations 'DataTable','ApplicationInterface','Mapping','',0,1|3 November 2015");
            databaseVersions.Add(new Guid("AB213BCF-15EE-41F1-A89B-BA01E643CEFE"), "exec db_addclassassociations 'ApplicationInterface','DataTable','Mapping','',0,1|3 November 2015");
            databaseVersions.Add(new Guid("11F13474-EF5B-42B1-8373-A71735790F93"), "exec db_addclassassociations 'PhysicalInformationArtefact','ApplicationInterface','DynamicFlow','',1,1|3 November 2015");
            databaseVersions.Add(new Guid("52351F17-5039-49B8-94EE-2442C8AAF77D"), "exec db_addclassassociations 'ApplicationInterface','PhysicalInformationArtefact','DynamicFlow','',1,1|3 November 2015");
            databaseVersions.Add(new Guid("B9ECA2C2-EBCB-4976-841B-D85854969014"), "exec db_addclassassociations 'ApplicationInterface','PhysicalInformationArtefact','Mapping','',0,1|3 November 2015");
            databaseVersions.Add(new Guid("B81BEFD5-488D-4A61-81CD-B168AA856551"), "exec db_addclassassociations 'PhysicalInformationArtefact','ApplicationInterface','Mapping','',0,1|3 November 2015");

            #endregion

            #region 2 December 2015
            databaseVersions.Add(new Guid("A1B9CD09-6BA0-4CB1-82B1-2EBC25A466C8"), "EXEC db_addclassassociations 'PhysicalSoftwareComponent','PhysicalSoftwareComponent','Decomposition','',0,1|2 December 2015");
            #endregion

            #region 15 December 2015
            ////Anri
            databaseVersions.Add(new Guid("7CB31D89-A42D-41CC-B041-22E5E3D130E6"), "exec db_addclassassociations 'Gateway','Object','Mapping','',0,1|15 December 2015");
            databaseVersions.Add(new Guid("E124523C-C693-432D-86B3-FB4351C80FE2"), "exec db_addclassassociations 'Object','Gateway','DynamicFlow','',1,1|15 December 2015");
            databaseVersions.Add(new Guid("C23C0D55-D707-4E98-A58E-01191EF9DBD5"), "exec db_addclassassociations 'Object','Gateway','Mapping','',0,1|15 December 2015");

            //HCL
            databaseVersions.Add(new Guid("9B6A33C9-19A1-48A2-B785-2B54B8383C11"), "exec db_addclassassociations 'LocationUnit','Measure','Mapping','',1,1|15 December 2015");
            databaseVersions.Add(new Guid("3748C7FE-F948-4433-B1BF-BC74D08248A3"), "exec db_addclassassociations 'LocationUnit','Job','Mapping','',1,1|15 December 2015");
            databaseVersions.Add(new Guid("420F199D-F40E-4419-9675-1694F083E1BF"), "exec db_addclassassociations 'LocationUnit','TimeUnit','Mapping','',1,1|15 December 2015");
            databaseVersions.Add(new Guid("648F4C20-1D7A-4989-9E16-67B87426EFDC"), "exec db_addclassassociations 'LocationUnit','ResourceType','Mapping','',1,1|15 December 2015");

            databaseVersions.Add(new Guid("92DD74CE-86A6-4BE1-8137-FDB69F41B6CD"), "exec db_addClasses 'NumberOf','Name','System',1|15 December 2015"); //artifact
            databaseVersions.Add(new Guid("A714CD88-427D-43F4-B639-19DE5530CD15"), "exec db_addFields 'NumberOf','Name','System.String','General','The name of the object (class instance) that uniquely identifies the object.',0,1,1|15 December 2015");

            databaseVersions.Add(new Guid("A5C54CF0-C2D7-47DC-913C-A608754382C0"), "exec db_addartifacts 'LocationUnit','ResourceType','Mapping','NumberOf',1|15 December 2015");

            databaseVersions.Add(new Guid("D226F7AD-A0F0-4F78-A372-1075CFBA9DCB"), "exec db_addclassassociations 'LocationUnit','Function','Mapping','',1,1|15 December 2015");
            databaseVersions.Add(new Guid("78D18F15-64C6-47D8-866C-7FFA1BCAD71F"), "exec db_addclassassociations 'LocationUnit','WorkPackage','Mapping','',1,1|15 December 2015");

            databaseVersions.Add(new Guid("74CB6575-5E47-469A-A6B9-8914CCA392B4"), "exec db_addartifacts 'Function','WorkPackage','Mapping','NumberOf',1|15 December 2015");
            databaseVersions.Add(new Guid("4338E16C-F6CE-4268-A68B-7ECE121061FA"), "exec db_addartifacts 'WorkPackage','Function','Mapping','NumberOf',1|15 December 2015");

            databaseVersions.Add(new Guid("BE9AB32A-4CBE-4A5A-974B-E540D33B529B"), "exec db_addclassassociations 'LocationUnit','LocationUnit','Classification','',0,1|15 December 2015"); //default is decomposition??
            //exec db_addclassassociations 'LocationUnit','LocationUnit','Decomposition','',0,0,1

            databaseVersions.Add(new Guid("0FF3FD84-FD4C-41D7-9FC3-488796B56DD4"), "exec db_addclassassociations 'WorkPackage','WorkPackage','Start','',0,1|15 December 2015"); //default is decomposition
            //exec db_addclassassociations 'WorkPackage','WorkPackage','Decomposition','',0,0,1|15 December 2015");
            databaseVersions.Add(new Guid("5EB5CC9C-D6A0-45C0-A09A-9EDF149969C1"), "exec db_addclassassociations 'LocationUnit','ResourceType','Start','',0,1|15 December 2015");
            #endregion

            #region 1 February 2016
            databaseVersions.Add(new Guid("C6A7BFC0-544A-4E04-9C29-A5E071364376"), "DROP PROCEDURE [dbo].[META_UpdateObjectFieldValue]|1 February 2016");
            databaseVersions.Add(new Guid("31B7CCA1-F2B6-4591-BFF9-86672C0DF0E9"), "CREATE PROCEDURE [dbo].[META_UpdateObjectFieldValue] @ObjectID int,@FieldID int,@ValueString varchar(900) = null,@ValueInt int = null,@ValueDouble numeric(18,2) = null,@ValueBoolean bit = null,@ValueDate DateTime = null,@ValueLongText text = null,@ValueObjectID int = null,@ValueRTF text = null,@MachineName varchar(50) as if exists(select fieldid from objectfieldvalue where fieldid = @fieldid and objectid =@objectid and machineid=@machinename) begin	update objectfieldvalue set		valuestring = @valuestring,		valueint = @valueint,		valuedouble = @valuedouble,		valueboolean = @valueboolean,		valuedate = @valuedate,		valuelongtext = @valuelongtext,		valueobjectid = @valueobjectid,		valuertf = @valuertf		where objectid = @objectid and fieldid = @fieldid and machineid=@MachineName 	return end insert into objectfieldvalue(objectid,fieldid,valuestring,valueint,valuedouble,valueboolean,valuedate,valuelongtext,valueobjectid,valuertf,machineid)		values(@objectid,@fieldid,@valuestring,@valueint,@valuedouble,@valueboolean,@valuedate,@valuelongtext,@valueobjectid,@valuertf,@machinename)|1 February 2016");

            databaseVersions.Add(new Guid("29C9E346-12E3-4425-BD37-202E699EA127"), "UPDATE FIELD SET DataType = 'LongText' WHERE Name = 'Description'|1 February 2016");
            databaseVersions.Add(new Guid("7BA7DF4D-EEAF-4C1C-B389-E67D441273B2"), "ALTER TABLE ObjectFieldValue ALTER COLUMN ValueString varchar(900)|1 February 2016");
            databaseVersions.Add(new Guid("AA070499-B7C2-453A-8446-9698AD7480D6"), "REBUILDVIEWS|REBUILDVIEWS");
            #endregion

            #region 25 February 2016
            databaseVersions.Add(new Guid("98A0C809-F799-4A94-8B57-3D83B78DC1A1"), "exec db_addFields 'DataField','DomainValue','System.String','General','',1,13,1|25 February 2016");
            databaseVersions.Add(new Guid("BAD02E17-A3AC-4B82-B0AB-449FA49DDB01"), "exec db_addFields 'DataAttribute','DomainValue','System.String','General','',1,10,1|25 February 2016");

            databaseVersions.Add(new Guid("CFE14AEF-A0F6-4ED9-9AFF-007368F6DB1C"), "REBUILDSINGLEVIEWS-DataField-DataAttribute-|25 February 2016");
            #endregion

            //#if !DEBUG
            //#endif

            #region 10 March 2016 - SAPPI

            databaseVersions.Add(new Guid("50EDEDBC-FC0E-4E50-B016-6FAA90B9270E"), "EXEC db_AddPossibleValues 'ServerType','VirtualHost',20,'VirtualHost',1|10 March 2016");
            databaseVersions.Add(new Guid("51C8A1E3-A089-436E-85DC-BA438F4481F4"), "EXEC db_AddPossibleValues 'ServerType','VirtualCluster',21,'VirtualCluster',1|10 March 2016");

            databaseVersions.Add(new Guid("E9BFC4C0-54E7-448F-BA06-7A2A52436745"), "exec db_addFields 'Network','ComputingComponentType','ComputingComponentType','General','',1,5,1|10 March 2016");
            databaseVersions.Add(new Guid("1395E3E0-B753-4A96-B844-FF6C96A34E40"), "exec db_addFields 'Network','ServerType','ServerType','General','',1,6,1|10 March 2016");
            databaseVersions.Add(new Guid("FF9A87DB-89D7-4F4B-B75B-995C606C5711"), "exec db_addFields 'Network','SeverityRating','System.String','General','',1,7,1|10 March 2016");
            databaseVersions.Add(new Guid("0E2B98DD-5EC9-47BE-959B-EA1A7871E191"), "exec db_addFields 'Network','ConfigurationID','System.String','General','',1,8,1|10 March 2016");
            databaseVersions.Add(new Guid("A5798A97-7CDB-4CC0-89EB-2AD7C002F137"), "exec db_addFields 'Network','Make','System.String','General','',1,9,1|10 March 2016");
            databaseVersions.Add(new Guid("B53978D2-F3C6-48E6-90FE-DA7CE2070688"), "exec db_addFields 'Network','Model','System.String','General','',1,10,1|10 March 2016");
            databaseVersions.Add(new Guid("3D5EC341-ED3B-4AB0-B79E-4C95C9EC76B6"), "exec db_addFields 'Network','ModelNumber','System.String','General','',1,11,1|10 March 2016");
            databaseVersions.Add(new Guid("39025252-89B1-4421-9EB1-2FB8FD1FFD0D"), "exec db_addFields 'Network','SerialNumber','System.String','General','',1,12,1|10 March 2016");
            databaseVersions.Add(new Guid("ADAD46FE-63A2-45D7-9769-26C4BEF219B1"), "exec db_addFields 'Network','AssetNumber','System.String','General','',1,13,1|10 March 2016");
            databaseVersions.Add(new Guid("2557EB28-842F-47A6-819C-108DCE7EE3D8"), "exec db_addFields 'Network','DatePurchased','System.String','General','',1,14,1|10 March 2016");
            databaseVersions.Add(new Guid("B306073E-6436-4CD8-9DE0-2B86883DAA20"), "exec db_addFields 'Network','isManaged','System.String','General','',1,15,1|10 March 2016");
            databaseVersions.Add(new Guid("ED8B47F2-F69C-4F37-A5C7-329507F63A8F"), "exec db_addFields 'Network','ContractNumber','System.String','General','',1,16,1|10 March 2016");
            databaseVersions.Add(new Guid("0E3E196E-C3AC-404F-829C-39A53AF77470"), "exec db_addFields 'Network','UnderWarranty','System.String','General','',1,17,1|10 March 2016");
            databaseVersions.Add(new Guid("075EBF69-8CD1-4FAC-9CB3-8AAAF24429A5"), "exec db_addFields 'Network','NetworkAddress1','System.String','General','',1,18,1|10 March 2016");
            databaseVersions.Add(new Guid("64B4B5A6-35F6-4C34-A707-C75D724AE2E4"), "exec db_addFields 'Network','NetworkAddress2','System.String','General','',1,19,1|10 March 2016");
            databaseVersions.Add(new Guid("7A85C8D0-B353-41F2-B7BD-C7B322A889F7"), "exec db_addFields 'Network','NetworkAddress3','System.String','General','',1,20,1|10 March 2016");
            databaseVersions.Add(new Guid("C914308C-6B8E-4F07-BADB-B5B981D90E9F"), "exec db_addFields 'Network','NetworkAddress4','System.String','General','',1,21,1|10 March 2016");
            databaseVersions.Add(new Guid("78B3488B-59B6-4403-B5EF-E786F7A51556"), "exec db_addFields 'Network','NetworkAddress5','System.String','General','',1,22,1|10 March 2016");
            databaseVersions.Add(new Guid("26C15686-682A-434E-B474-B7B5A06BFFD7"), "exec db_addFields 'Network','IsDNS','System.String','General','',1,23,1|10 March 2016");
            databaseVersions.Add(new Guid("E7CC3BD6-728B-415D-B429-7D389F0E30D1"), "exec db_addFields 'Network','IsDHCP','System.String','General','',1,24,1|10 March 2016");
            databaseVersions.Add(new Guid("3EBCEF70-9A32-4244-BD1B-17FBD0A7A444"), "exec db_addFields 'Network','Domain','System.String','General','',1,25,1|10 March 2016");
            databaseVersions.Add(new Guid("6B58889A-6DDC-4662-9439-66FF02A1D481"), "exec db_addFields 'Network','Capacity','System.String','General','',1,26,1|10 March 2016");
            databaseVersions.Add(new Guid("DF2D10CA-62AA-48DE-B4C2-9F7E48588978"), "exec db_addFields 'Network','NumberofDisks','System.String','General','',1,27,1|10 March 2016");
            databaseVersions.Add(new Guid("15EE192D-C18C-43B2-8B11-81766C142A0B"), "exec db_addFields 'Network','CPUType','System.String','General','',1,28,1|10 March 2016");
            databaseVersions.Add(new Guid("0D12F9B3-85DA-40C4-911B-DA14F442EAD8"), "exec db_addFields 'Network','CPUSpeed','System.String','General','',1,29,1|10 March 2016");
            databaseVersions.Add(new Guid("1C3AAE48-6663-431F-92CF-F050499D1C0B"), "exec db_addFields 'Network','Monitor','System.String','General','',1,30,1|10 March 2016");
            databaseVersions.Add(new Guid("2CB04D32-B2F8-46C8-B96F-2E015E2B36D1"), "exec db_addFields 'Network','VideoCard','System.String','General','',1,31,1|10 March 2016");
            databaseVersions.Add(new Guid("2A561DA7-27EE-4D14-A4A3-F878C273D8B8"), "exec db_addFields 'Network','MemoryTotal','System.String','General','',1,32,1|10 March 2016");

            databaseVersions.Add(new Guid("1B93D319-41FC-419A-91B0-92EB5BE5277C"), "REBUILDSINGLEVIEWS-Network-|25 February 2016");

            #endregion

            databaseVersions.Add(new Guid("B2BAA33C-468D-45f1-BBD1-D284FA4ECA18"), "EXEC db_AddPossibleValues 'ServerType','VirtualHost',20,'VirtualHost',0|14 March 2016");
            databaseVersions.Add(new Guid("C9AF5DE4-2D32-4486-A653-E9EAE0B36700"), "EXEC db_AddPossibleValues 'ServerType','VirtualCluster',21,'VirtualCluster',0|14 March 2016");
            databaseVersions.Add(new Guid("2B735B4A-D149-4f54-8CD7-4B1B9679021F"), "EXEC db_AddPossibleValues 'ComputingComponentType','VirtualHost',20,'VirtualHost',1|14 March 2016");
            databaseVersions.Add(new Guid("89B82A2C-2A27-43c5-9FA4-CE3EFD5FF1B4"), "EXEC db_AddPossibleValues 'ComputingComponentType','VirtualCluster',21,'VirtualCluster',1|14 March 2016");


            //databaseVersions.Add(new Guid(""), "REBUILDSINGLEVIEWS-DataField-DataAttribute-|25 February 2016");
            //databaseVersions.Add(new Guid("DE6E642C-E6FA-45D0-B206-F32F54FCF935"), "REBUILDVIEWS|REBUILDVIEWS");
            return databaseVersions;
        }
    }
}

//1	Auxiliary                
//2	Classification           
//3	Decomposition            
//4	Mapping                  
//5	LeadsTo                  
//6	LocatedAt                
//7	Dependency               
//8	SubSetOf                 
//10	Create                   
//11	Read                     
//12	OneWayCurved             
//13	Start                    
//14	Stop                     
//15	Concurrent               
//16	NonConcurrent            
//18	Update                   
//19	DynamicFlow              
//20	Maintain                 
//21	Suspend                  
//22	Resume                   
//23	Delete                   
//24	Interrupt                
//25	Synchronise              
//26	Zero_To_One              
//27	ZeroOrMore_To_One        
//28	One_To_Many              
//29	Many_To_Many             
//30	ConnectedTo              
//31	MutuallyExclusiveLink    
//32	CreateOLD                
//33	FunctionalDependency     
//34	Provide                  
//35	Use                      
//36	EnabledBy                
//38	One_To_One               

//Duplicate default associations parent-child-type-default
//Activity	Activity	13	1
//Activity	Activity	3	1
//Activity	Process	4	1
//Activity	Process	13	1
//ComputingComponent	ComputingComponent
//ComputingComponent	ComputingComponent
//ComputingComponent	Network
//ComputingComponent	Network
//LogicalDataComponent	Function	19	1
//LogicalDataComponent	Function	4	1
//LogicalDataComponent	LogicalDataComponent	19	1
//LogicalDataComponent	LogicalDataComponent	4	1
//Network	Network	30	1
//Network	Network	4	1
//Network	Network	3	1
//Network	NetworkComponent	4	1
//Network	NetworkComponent	30	1
//OrganizationalUnit	PhysicalDataComponent	19	1
//OrganizationalUnit	PhysicalDataComponent	4	1
//OrganizationalUnit	PhysicalSoftwareComponent	4	1
//OrganizationalUnit	PhysicalSoftwareComponent	19	1
//PhysicalDataComponent	PhysicalDataComponent	4	1
//PhysicalDataComponent	PhysicalDataComponent	19	1
//PhysicalInformationArtefact	LogicalDataComponent	19	1
//PhysicalInformationArtefact	LogicalDataComponent	4	1
//PhysicalInformationArtefact	PhysicalDataComponent	4	1
//PhysicalInformationArtefact	PhysicalDataComponent	19	1
//Process	Activity	13	1
//Process	Activity	4	1
//Process	Function	4	1
//Process	Function	13	1
//Process	Process	13	1
//Process	Process	3	1
//Task	Activity	13	1
//Task	Activity	4	1