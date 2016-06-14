

ALTER PROCEDURE [dbo].[META_CreateObject]
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
