ALTER PROCEDURE [dbo].[META_DeleteObject]
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