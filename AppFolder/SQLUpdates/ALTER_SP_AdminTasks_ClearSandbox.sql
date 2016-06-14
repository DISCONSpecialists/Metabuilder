ALTER PROCEDURE [dbo].[proc_AdminTasks_ClearSandbox]
	AS
BEGIN
	PRINT 'ClearSandbox'

begin
delete from artifact where artifactobjectid in (select  pkid from metaobject where workspacename='Sandbox')
OR objectid in  (select pkid from metaobject where workspacename='Sandbox')
OR childobjectid in (select pkid from metaobject where workspacename='Sandbox')
DELETE FROM GRAPHFILEASSOCIATION WHERE objectid in (select pkid from metaobject where workspacename='Sandbox')
	DELETE FROM GRAPHFILEASSOCIATION WHERE childobjectid in (select pkid from metaobject where workspacename='Sandbox')


DELETE FROM GRAPHFILEASSOCIATION WHERE GraphFileID in (Select pkid from graphfile where workspacename='Sandbox')
DELETE FROM GRAPHFILEOBJECT WHERE GraphFileID in (Select pkid from graphfile where workspacename='Sandbox')
	DELETE FROM GRAPHFILE where workspacename='Sandbox'
	DELETE FROM OBJECTASSOCIATION WHERE objectid in (select pkid from metaobject where workspacename='Sandbox')
	DELETE FROM OBJECTASSOCIATION WHERE childobjectid in (select pkid from metaobject where workspacename='Sandbox')

	DELETE FROM OBJECTFIELDVALUE  WHERE objectid in (select pkid from metaobject where workspacename='Sandbox')
	DELETE FROM METAOBJECT  where workspacename='Sandbox'
	DELETE FROM USERPERMISSION where workspacename='Sandbox'
-- exec META_DeleteWorkspace @sandboxid

end
END