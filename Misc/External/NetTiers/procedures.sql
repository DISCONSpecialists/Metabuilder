
USE [MetabuilderServer]
GO
SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_WorkspaceType_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_WorkspaceType_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_WorkspaceType_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the WorkspaceType table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_WorkspaceType_Get_List

AS


				
				SELECT
					[pkid],
					[Description]
				FROM
					[dbo].[WorkspaceType]
					
				SELECT @@ROWCOUNT
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_WorkspaceType_GetPaged procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_WorkspaceType_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_WorkspaceType_GetPaged
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the WorkspaceType table passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_WorkspaceType_GetPaged
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  ,

	@PageIndex int   ,

	@PageSize int   
)
AS


				
				BEGIN
				DECLARE @PageLowerBound int
				DECLARE @PageUpperBound int
				
				-- Set the page bounds
				SET @PageLowerBound = @PageSize * @PageIndex
				SET @PageUpperBound = @PageLowerBound + @PageSize

				-- Create a temp table to store the select results
				CREATE TABLE #PageIndex
				(
				    [IndexId] int IDENTITY (1, 1) NOT NULL,
				    [pkid] int 
				)
				
				-- Insert into the temp table
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = 'INSERT INTO #PageIndex ([pkid])'
				SET @SQL = @SQL + ' SELECT'
				IF @PageSize > 0
				BEGIN
					SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound)
				END
				SET @SQL = @SQL + ' [pkid]'
				SET @SQL = @SQL + ' FROM [dbo].[WorkspaceType]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Populate the temp table
				EXEC sp_executesql @SQL

				-- Return paged results
				SELECT O.[pkid], O.[Description]
				FROM
				    [dbo].[WorkspaceType] O,
				    #PageIndex PageIndex
				WHERE
				    PageIndex.IndexId > @PageLowerBound
					AND O.[pkid] = PageIndex.[pkid]
				ORDER BY
				    PageIndex.IndexId
				
				-- get row count
				SET @SQL = 'SELECT COUNT(*) AS TotalRowCount'
				SET @SQL = @SQL + ' FROM [dbo].[WorkspaceType]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				EXEC sp_executesql @SQL
			
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_WorkspaceType_Insert procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_WorkspaceType_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_WorkspaceType_Insert
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Inserts a record into the WorkspaceType table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_WorkspaceType_Insert
(

	@pkid int    OUTPUT,

	@Description varchar (50)  
)
AS


				
				INSERT INTO [dbo].[WorkspaceType]
					(
					[Description]
					)
				VALUES
					(
					@Description
					)
				
				-- Get the identity value
				SET @pkid = SCOPE_IDENTITY()
									
							
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_WorkspaceType_Update procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_WorkspaceType_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_WorkspaceType_Update
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Updates a record in the WorkspaceType table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_WorkspaceType_Update
(

	@pkid int   ,

	@Description varchar (50)  
)
AS


				
				
				-- Modify the updatable columns
				UPDATE
					[dbo].[WorkspaceType]
				SET
					[Description] = @Description
				WHERE
[pkid] = @pkid 
				
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_WorkspaceType_Delete procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_WorkspaceType_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_WorkspaceType_Delete
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Deletes a record in the WorkspaceType table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_WorkspaceType_Delete
(

	@pkid int   
)
AS


				DELETE FROM [dbo].[WorkspaceType] WITH (ROWLOCK) 
				WHERE
					[pkid] = @pkid
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_WorkspaceType_GetByDescription procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_WorkspaceType_GetByDescription') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_WorkspaceType_GetByDescription
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the WorkspaceType table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_WorkspaceType_GetByDescription
(

	@Description varchar (50)  
)
AS


				SELECT
					[pkid],
					[Description]
				FROM
					[dbo].[WorkspaceType]
				WHERE
					[Description] = @Description
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_WorkspaceType_GetBypkid procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_WorkspaceType_GetBypkid') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_WorkspaceType_GetBypkid
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the WorkspaceType table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_WorkspaceType_GetBypkid
(

	@pkid int   
)
AS


				SELECT
					[pkid],
					[Description]
				FROM
					[dbo].[WorkspaceType]
				WHERE
					[pkid] = @pkid
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_WorkspaceType_Find procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_WorkspaceType_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_WorkspaceType_Find
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Finds records in the WorkspaceType table passing nullable parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_WorkspaceType_Find
(

	@SearchUsingOR bit   = null ,

	@pkid int   = null ,

	@Description varchar (50)  = null 
)
AS


				
  IF ISNULL(@SearchUsingOR, 0) <> 1
  BEGIN
    SELECT
	  [pkid]
	, [Description]
    FROM
	[dbo].[WorkspaceType]
    WHERE 
	 ([pkid] = @pkid OR @pkid IS NULL)
	AND ([Description] = @Description OR @Description IS NULL)
						
  END
  ELSE
  BEGIN
    SELECT
	  [pkid]
	, [Description]
    FROM
	[dbo].[WorkspaceType]
    WHERE 
	 ([pkid] = @pkid AND @pkid is not null)
	OR ([Description] = @Description AND @Description is not null)
	SELECT @@ROWCOUNT			
  END
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_FileType_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_FileType_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_FileType_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the FileType table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_FileType_Get_List

AS


				
				SELECT
					[pkid],
					[Name]
				FROM
					[dbo].[FileType]
					
				SELECT @@ROWCOUNT
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_FileType_GetPaged procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_FileType_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_FileType_GetPaged
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the FileType table passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_FileType_GetPaged
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  ,

	@PageIndex int   ,

	@PageSize int   
)
AS


				
				BEGIN
				DECLARE @PageLowerBound int
				DECLARE @PageUpperBound int
				
				-- Set the page bounds
				SET @PageLowerBound = @PageSize * @PageIndex
				SET @PageUpperBound = @PageLowerBound + @PageSize

				-- Create a temp table to store the select results
				CREATE TABLE #PageIndex
				(
				    [IndexId] int IDENTITY (1, 1) NOT NULL,
				    [pkid] int 
				)
				
				-- Insert into the temp table
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = 'INSERT INTO #PageIndex ([pkid])'
				SET @SQL = @SQL + ' SELECT'
				IF @PageSize > 0
				BEGIN
					SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound)
				END
				SET @SQL = @SQL + ' [pkid]'
				SET @SQL = @SQL + ' FROM [dbo].[FileType]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Populate the temp table
				EXEC sp_executesql @SQL

				-- Return paged results
				SELECT O.[pkid], O.[Name]
				FROM
				    [dbo].[FileType] O,
				    #PageIndex PageIndex
				WHERE
				    PageIndex.IndexId > @PageLowerBound
					AND O.[pkid] = PageIndex.[pkid]
				ORDER BY
				    PageIndex.IndexId
				
				-- get row count
				SET @SQL = 'SELECT COUNT(*) AS TotalRowCount'
				SET @SQL = @SQL + ' FROM [dbo].[FileType]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				EXEC sp_executesql @SQL
			
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_FileType_Insert procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_FileType_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_FileType_Insert
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Inserts a record into the FileType table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_FileType_Insert
(

	@pkid int    OUTPUT,

	@Name varchar (25)  
)
AS


				
				INSERT INTO [dbo].[FileType]
					(
					[Name]
					)
				VALUES
					(
					@Name
					)
				
				-- Get the identity value
				SET @pkid = SCOPE_IDENTITY()
									
							
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_FileType_Update procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_FileType_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_FileType_Update
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Updates a record in the FileType table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_FileType_Update
(

	@pkid int   ,

	@Name varchar (25)  
)
AS


				
				
				-- Modify the updatable columns
				UPDATE
					[dbo].[FileType]
				SET
					[Name] = @Name
				WHERE
[pkid] = @pkid 
				
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_FileType_Delete procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_FileType_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_FileType_Delete
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Deletes a record in the FileType table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_FileType_Delete
(

	@pkid int   
)
AS


				DELETE FROM [dbo].[FileType] WITH (ROWLOCK) 
				WHERE
					[pkid] = @pkid
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_FileType_GetByName procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_FileType_GetByName') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_FileType_GetByName
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the FileType table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_FileType_GetByName
(

	@Name varchar (25)  
)
AS


				SELECT
					[pkid],
					[Name]
				FROM
					[dbo].[FileType]
				WHERE
					[Name] = @Name
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_FileType_GetBypkid procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_FileType_GetBypkid') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_FileType_GetBypkid
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the FileType table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_FileType_GetBypkid
(

	@pkid int   
)
AS


				SELECT
					[pkid],
					[Name]
				FROM
					[dbo].[FileType]
				WHERE
					[pkid] = @pkid
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_FileType_Find procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_FileType_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_FileType_Find
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Finds records in the FileType table passing nullable parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_FileType_Find
(

	@SearchUsingOR bit   = null ,

	@pkid int   = null ,

	@Name varchar (25)  = null 
)
AS


				
  IF ISNULL(@SearchUsingOR, 0) <> 1
  BEGIN
    SELECT
	  [pkid]
	, [Name]
    FROM
	[dbo].[FileType]
    WHERE 
	 ([pkid] = @pkid OR @pkid IS NULL)
	AND ([Name] = @Name OR @Name IS NULL)
						
  END
  ELSE
  BEGIN
    SELECT
	  [pkid]
	, [Name]
    FROM
	[dbo].[FileType]
    WHERE 
	 ([pkid] = @pkid AND @pkid is not null)
	OR ([Name] = @Name AND @Name is not null)
	SELECT @@ROWCOUNT			
  END
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_User_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_User_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_User_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the User table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_User_Get_List

AS


				
				SELECT
					[pkid],
					[Name],
					[Password],
					[CreateDate],
					[LastLogin]
				FROM
					[dbo].[User]
					
				SELECT @@ROWCOUNT
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_User_GetPaged procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_User_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_User_GetPaged
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the User table passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_User_GetPaged
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  ,

	@PageIndex int   ,

	@PageSize int   
)
AS


				
				BEGIN
				DECLARE @PageLowerBound int
				DECLARE @PageUpperBound int
				
				-- Set the page bounds
				SET @PageLowerBound = @PageSize * @PageIndex
				SET @PageUpperBound = @PageLowerBound + @PageSize

				-- Create a temp table to store the select results
				CREATE TABLE #PageIndex
				(
				    [IndexId] int IDENTITY (1, 1) NOT NULL,
				    [pkid] int 
				)
				
				-- Insert into the temp table
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = 'INSERT INTO #PageIndex ([pkid])'
				SET @SQL = @SQL + ' SELECT'
				IF @PageSize > 0
				BEGIN
					SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound)
				END
				SET @SQL = @SQL + ' [pkid]'
				SET @SQL = @SQL + ' FROM [dbo].[User]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Populate the temp table
				EXEC sp_executesql @SQL

				-- Return paged results
				SELECT O.[pkid], O.[Name], O.[Password], O.[CreateDate], O.[LastLogin]
				FROM
				    [dbo].[User] O,
				    #PageIndex PageIndex
				WHERE
				    PageIndex.IndexId > @PageLowerBound
					AND O.[pkid] = PageIndex.[pkid]
				ORDER BY
				    PageIndex.IndexId
				
				-- get row count
				SET @SQL = 'SELECT COUNT(*) AS TotalRowCount'
				SET @SQL = @SQL + ' FROM [dbo].[User]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				EXEC sp_executesql @SQL
			
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_User_Insert procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_User_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_User_Insert
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Inserts a record into the User table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_User_Insert
(

	@pkid int    OUTPUT,

	@Name varchar (100)  ,

	@Password varchar (100)  ,

	@CreateDate datetime   ,

	@LastLogin datetime   
)
AS


				
				INSERT INTO [dbo].[User]
					(
					[Name]
					,[Password]
					,[CreateDate]
					,[LastLogin]
					)
				VALUES
					(
					@Name
					,@Password
					,@CreateDate
					,@LastLogin
					)
				
				-- Get the identity value
				SET @pkid = SCOPE_IDENTITY()
									
							
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_User_Update procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_User_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_User_Update
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Updates a record in the User table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_User_Update
(

	@pkid int   ,

	@Name varchar (100)  ,

	@Password varchar (100)  ,

	@CreateDate datetime   ,

	@LastLogin datetime   
)
AS


				
				
				-- Modify the updatable columns
				UPDATE
					[dbo].[User]
				SET
					[Name] = @Name
					,[Password] = @Password
					,[CreateDate] = @CreateDate
					,[LastLogin] = @LastLogin
				WHERE
[pkid] = @pkid 
				
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_User_Delete procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_User_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_User_Delete
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Deletes a record in the User table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_User_Delete
(

	@pkid int   
)
AS


				DELETE FROM [dbo].[User] WITH (ROWLOCK) 
				WHERE
					[pkid] = @pkid
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_User_GetBypkid procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_User_GetBypkid') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_User_GetBypkid
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the User table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_User_GetBypkid
(

	@pkid int   
)
AS


				SELECT
					[pkid],
					[Name],
					[Password],
					[CreateDate],
					[LastLogin]
				FROM
					[dbo].[User]
				WHERE
					[pkid] = @pkid
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_User_GetByPermissionIDFromUserPermission procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_User_GetByPermissionIDFromUserPermission') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_User_GetByPermissionIDFromUserPermission
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records through a junction table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_User_GetByPermissionIDFromUserPermission
(

	@PermissionID int   
)
AS


SELECT dbo.[User].[pkid]
       ,dbo.[User].[Name]
       ,dbo.[User].[Password]
       ,dbo.[User].[CreateDate]
       ,dbo.[User].[LastLogin]
  FROM dbo.[User]
 WHERE EXISTS (SELECT 1
                 FROM dbo.[UserPermission] 
                WHERE dbo.[UserPermission].[PermissionID] = @PermissionID
                  AND dbo.[UserPermission].[UserID] = dbo.[User].[pkid]
                  )
				SELECT @@ROWCOUNT			
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_User_GetByWorkspaceNameWorkspaceTypeIDFromUserPermission procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_User_GetByWorkspaceNameWorkspaceTypeIDFromUserPermission') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_User_GetByWorkspaceNameWorkspaceTypeIDFromUserPermission
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records through a junction table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_User_GetByWorkspaceNameWorkspaceTypeIDFromUserPermission
(

	@WorkspaceName varchar (100)  ,

	@WorkspaceTypeID int   
)
AS


SELECT dbo.[User].[pkid]
       ,dbo.[User].[Name]
       ,dbo.[User].[Password]
       ,dbo.[User].[CreateDate]
       ,dbo.[User].[LastLogin]
  FROM dbo.[User]
 WHERE EXISTS (SELECT 1
                 FROM dbo.[UserPermission] 
                WHERE dbo.[UserPermission].[WorkspaceName] = @WorkspaceName
                  AND dbo.[UserPermission].[WorkspaceTypeID] = @WorkspaceTypeID
                  AND dbo.[UserPermission].[UserID] = dbo.[User].[pkid]
                  )
				SELECT @@ROWCOUNT			
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_User_Find procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_User_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_User_Find
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Finds records in the User table passing nullable parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_User_Find
(

	@SearchUsingOR bit   = null ,

	@pkid int   = null ,

	@Name varchar (100)  = null ,

	@Password varchar (100)  = null ,

	@CreateDate datetime   = null ,

	@LastLogin datetime   = null 
)
AS


				
  IF ISNULL(@SearchUsingOR, 0) <> 1
  BEGIN
    SELECT
	  [pkid]
	, [Name]
	, [Password]
	, [CreateDate]
	, [LastLogin]
    FROM
	[dbo].[User]
    WHERE 
	 ([pkid] = @pkid OR @pkid IS NULL)
	AND ([Name] = @Name OR @Name IS NULL)
	AND ([Password] = @Password OR @Password IS NULL)
	AND ([CreateDate] = @CreateDate OR @CreateDate IS NULL)
	AND ([LastLogin] = @LastLogin OR @LastLogin IS NULL)
						
  END
  ELSE
  BEGIN
    SELECT
	  [pkid]
	, [Name]
	, [Password]
	, [CreateDate]
	, [LastLogin]
    FROM
	[dbo].[User]
    WHERE 
	 ([pkid] = @pkid AND @pkid is not null)
	OR ([Name] = @Name AND @Name is not null)
	OR ([Password] = @Password AND @Password is not null)
	OR ([CreateDate] = @CreateDate AND @CreateDate is not null)
	OR ([LastLogin] = @LastLogin AND @LastLogin is not null)
	SELECT @@ROWCOUNT			
  END
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_UURI_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_UURI_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_UURI_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the UURI table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_UURI_Get_List

AS


				
				SELECT
					[pkid],
					[FileName]
				FROM
					[dbo].[UURI]
					
				SELECT @@ROWCOUNT
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_UURI_GetPaged procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_UURI_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_UURI_GetPaged
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the UURI table passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_UURI_GetPaged
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  ,

	@PageIndex int   ,

	@PageSize int   
)
AS


				
				BEGIN
				DECLARE @PageLowerBound int
				DECLARE @PageUpperBound int
				
				-- Set the page bounds
				SET @PageLowerBound = @PageSize * @PageIndex
				SET @PageUpperBound = @PageLowerBound + @PageSize

				-- Create a temp table to store the select results
				CREATE TABLE #PageIndex
				(
				    [IndexId] int IDENTITY (1, 1) NOT NULL,
				    [pkid] int 
				)
				
				-- Insert into the temp table
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = 'INSERT INTO #PageIndex ([pkid])'
				SET @SQL = @SQL + ' SELECT'
				IF @PageSize > 0
				BEGIN
					SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound)
				END
				SET @SQL = @SQL + ' [pkid]'
				SET @SQL = @SQL + ' FROM [dbo].[UURI]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Populate the temp table
				EXEC sp_executesql @SQL

				-- Return paged results
				SELECT O.[pkid], O.[FileName]
				FROM
				    [dbo].[UURI] O,
				    #PageIndex PageIndex
				WHERE
				    PageIndex.IndexId > @PageLowerBound
					AND O.[pkid] = PageIndex.[pkid]
				ORDER BY
				    PageIndex.IndexId
				
				-- get row count
				SET @SQL = 'SELECT COUNT(*) AS TotalRowCount'
				SET @SQL = @SQL + ' FROM [dbo].[UURI]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				EXEC sp_executesql @SQL
			
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_UURI_Insert procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_UURI_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_UURI_Insert
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Inserts a record into the UURI table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_UURI_Insert
(

	@pkid int    OUTPUT,

	@FileName varchar (2000)  
)
AS


				
				INSERT INTO [dbo].[UURI]
					(
					[FileName]
					)
				VALUES
					(
					@FileName
					)
				
				-- Get the identity value
				SET @pkid = SCOPE_IDENTITY()
									
							
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_UURI_Update procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_UURI_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_UURI_Update
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Updates a record in the UURI table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_UURI_Update
(

	@pkid int   ,

	@FileName varchar (2000)  
)
AS


				
				
				-- Modify the updatable columns
				UPDATE
					[dbo].[UURI]
				SET
					[FileName] = @FileName
				WHERE
[pkid] = @pkid 
				
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_UURI_Delete procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_UURI_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_UURI_Delete
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Deletes a record in the UURI table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_UURI_Delete
(

	@pkid int   
)
AS


				DELETE FROM [dbo].[UURI] WITH (ROWLOCK) 
				WHERE
					[pkid] = @pkid
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_UURI_GetBypkid procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_UURI_GetBypkid') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_UURI_GetBypkid
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the UURI table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_UURI_GetBypkid
(

	@pkid int   
)
AS


				SELECT
					[pkid],
					[FileName]
				FROM
					[dbo].[UURI]
				WHERE
					[pkid] = @pkid
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_UURI_Find procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_UURI_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_UURI_Find
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Finds records in the UURI table passing nullable parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_UURI_Find
(

	@SearchUsingOR bit   = null ,

	@pkid int   = null ,

	@FileName varchar (2000)  = null 
)
AS


				
  IF ISNULL(@SearchUsingOR, 0) <> 1
  BEGIN
    SELECT
	  [pkid]
	, [FileName]
    FROM
	[dbo].[UURI]
    WHERE 
	 ([pkid] = @pkid OR @pkid IS NULL)
	AND ([FileName] = @FileName OR @FileName IS NULL)
						
  END
  ELSE
  BEGIN
    SELECT
	  [pkid]
	, [FileName]
    FROM
	[dbo].[UURI]
    WHERE 
	 ([pkid] = @pkid AND @pkid is not null)
	OR ([FileName] = @FileName AND @FileName is not null)
	SELECT @@ROWCOUNT			
  END
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Permission_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Permission_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Permission_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the Permission table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Permission_Get_List

AS


				
				SELECT
					[pkid],
					[Description],
					[PermissionType]
				FROM
					[dbo].[Permission]
					
				SELECT @@ROWCOUNT
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Permission_GetPaged procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Permission_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Permission_GetPaged
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the Permission table passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Permission_GetPaged
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  ,

	@PageIndex int   ,

	@PageSize int   
)
AS


				
				BEGIN
				DECLARE @PageLowerBound int
				DECLARE @PageUpperBound int
				
				-- Set the page bounds
				SET @PageLowerBound = @PageSize * @PageIndex
				SET @PageUpperBound = @PageLowerBound + @PageSize

				-- Create a temp table to store the select results
				CREATE TABLE #PageIndex
				(
				    [IndexId] int IDENTITY (1, 1) NOT NULL,
				    [pkid] int 
				)
				
				-- Insert into the temp table
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = 'INSERT INTO #PageIndex ([pkid])'
				SET @SQL = @SQL + ' SELECT'
				IF @PageSize > 0
				BEGIN
					SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound)
				END
				SET @SQL = @SQL + ' [pkid]'
				SET @SQL = @SQL + ' FROM [dbo].[Permission]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Populate the temp table
				EXEC sp_executesql @SQL

				-- Return paged results
				SELECT O.[pkid], O.[Description], O.[PermissionType]
				FROM
				    [dbo].[Permission] O,
				    #PageIndex PageIndex
				WHERE
				    PageIndex.IndexId > @PageLowerBound
					AND O.[pkid] = PageIndex.[pkid]
				ORDER BY
				    PageIndex.IndexId
				
				-- get row count
				SET @SQL = 'SELECT COUNT(*) AS TotalRowCount'
				SET @SQL = @SQL + ' FROM [dbo].[Permission]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				EXEC sp_executesql @SQL
			
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Permission_Insert procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Permission_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Permission_Insert
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Inserts a record into the Permission table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Permission_Insert
(

	@pkid int    OUTPUT,

	@Description varchar (100)  ,

	@PermissionType varchar (50)  
)
AS


				
				INSERT INTO [dbo].[Permission]
					(
					[Description]
					,[PermissionType]
					)
				VALUES
					(
					@Description
					,@PermissionType
					)
				
				-- Get the identity value
				SET @pkid = SCOPE_IDENTITY()
									
							
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Permission_Update procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Permission_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Permission_Update
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Updates a record in the Permission table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Permission_Update
(

	@pkid int   ,

	@Description varchar (100)  ,

	@PermissionType varchar (50)  
)
AS


				
				
				-- Modify the updatable columns
				UPDATE
					[dbo].[Permission]
				SET
					[Description] = @Description
					,[PermissionType] = @PermissionType
				WHERE
[pkid] = @pkid 
				
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Permission_Delete procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Permission_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Permission_Delete
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Deletes a record in the Permission table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Permission_Delete
(

	@pkid int   
)
AS


				DELETE FROM [dbo].[Permission] WITH (ROWLOCK) 
				WHERE
					[pkid] = @pkid
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Permission_GetBypkid procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Permission_GetBypkid') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Permission_GetBypkid
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the Permission table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Permission_GetBypkid
(

	@pkid int   
)
AS


				SELECT
					[pkid],
					[Description],
					[PermissionType]
				FROM
					[dbo].[Permission]
				WHERE
					[pkid] = @pkid
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Permission_GetByUserIDFromUserPermission procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Permission_GetByUserIDFromUserPermission') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Permission_GetByUserIDFromUserPermission
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records through a junction table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Permission_GetByUserIDFromUserPermission
(

	@UserID int   
)
AS


SELECT dbo.[Permission].[pkid]
       ,dbo.[Permission].[Description]
       ,dbo.[Permission].[PermissionType]
  FROM dbo.[Permission]
 WHERE EXISTS (SELECT 1
                 FROM dbo.[UserPermission] 
                WHERE dbo.[UserPermission].[UserID] = @UserID
                  AND dbo.[UserPermission].[PermissionID] = dbo.[Permission].[pkid]
                  )
				SELECT @@ROWCOUNT			
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Permission_GetByWorkspaceNameWorkspaceTypeIDFromUserPermission procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Permission_GetByWorkspaceNameWorkspaceTypeIDFromUserPermission') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Permission_GetByWorkspaceNameWorkspaceTypeIDFromUserPermission
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records through a junction table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Permission_GetByWorkspaceNameWorkspaceTypeIDFromUserPermission
(

	@WorkspaceName varchar (100)  ,

	@WorkspaceTypeID int   
)
AS


SELECT dbo.[Permission].[pkid]
       ,dbo.[Permission].[Description]
       ,dbo.[Permission].[PermissionType]
  FROM dbo.[Permission]
 WHERE EXISTS (SELECT 1
                 FROM dbo.[UserPermission] 
                WHERE dbo.[UserPermission].[WorkspaceName] = @WorkspaceName
                  AND dbo.[UserPermission].[WorkspaceTypeID] = @WorkspaceTypeID
                  AND dbo.[UserPermission].[PermissionID] = dbo.[Permission].[pkid]
                  )
				SELECT @@ROWCOUNT			
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Permission_Find procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Permission_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Permission_Find
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Finds records in the Permission table passing nullable parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Permission_Find
(

	@SearchUsingOR bit   = null ,

	@pkid int   = null ,

	@Description varchar (100)  = null ,

	@PermissionType varchar (50)  = null 
)
AS


				
  IF ISNULL(@SearchUsingOR, 0) <> 1
  BEGIN
    SELECT
	  [pkid]
	, [Description]
	, [PermissionType]
    FROM
	[dbo].[Permission]
    WHERE 
	 ([pkid] = @pkid OR @pkid IS NULL)
	AND ([Description] = @Description OR @Description IS NULL)
	AND ([PermissionType] = @PermissionType OR @PermissionType IS NULL)
						
  END
  ELSE
  BEGIN
    SELECT
	  [pkid]
	, [Description]
	, [PermissionType]
    FROM
	[dbo].[Permission]
    WHERE 
	 ([pkid] = @pkid AND @pkid is not null)
	OR ([Description] = @Description AND @Description is not null)
	OR ([PermissionType] = @PermissionType AND @PermissionType is not null)
	SELECT @@ROWCOUNT			
  END
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ClassType_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ClassType_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ClassType_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the ClassType table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ClassType_Get_List

AS


				
				SELECT
					[ClassType]
				FROM
					[dbo].[ClassType]
					
				SELECT @@ROWCOUNT
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ClassType_GetPaged procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ClassType_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ClassType_GetPaged
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the ClassType table passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ClassType_GetPaged
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  ,

	@PageIndex int   ,

	@PageSize int   
)
AS


				
				BEGIN
				DECLARE @PageLowerBound int
				DECLARE @PageUpperBound int
				
				-- Set the page bounds
				SET @PageLowerBound = @PageSize * @PageIndex
				SET @PageUpperBound = @PageLowerBound + @PageSize

				-- Create a temp table to store the select results
				CREATE TABLE #PageIndex
				(
				    [IndexId] int IDENTITY (1, 1) NOT NULL,
				    [ClassType] varchar(50) COLLATE database_default  
				)
				
				-- Insert into the temp table
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = 'INSERT INTO #PageIndex ([ClassType])'
				SET @SQL = @SQL + ' SELECT'
				IF @PageSize > 0
				BEGIN
					SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound)
				END
				SET @SQL = @SQL + ' [ClassType]'
				SET @SQL = @SQL + ' FROM [dbo].[ClassType]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Populate the temp table
				EXEC sp_executesql @SQL

				-- Return paged results
				SELECT O.[ClassType]
				FROM
				    [dbo].[ClassType] O,
				    #PageIndex PageIndex
				WHERE
				    PageIndex.IndexId > @PageLowerBound
					AND O.[ClassType] = PageIndex.[ClassType]
				ORDER BY
				    PageIndex.IndexId
				
				-- get row count
				SET @SQL = 'SELECT COUNT(*) AS TotalRowCount'
				SET @SQL = @SQL + ' FROM [dbo].[ClassType]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				EXEC sp_executesql @SQL
			
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ClassType_Insert procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ClassType_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ClassType_Insert
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Inserts a record into the ClassType table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ClassType_Insert
(

	@ClassType varchar (50)  
)
AS


				
				INSERT INTO [dbo].[ClassType]
					(
					[ClassType]
					)
				VALUES
					(
					@ClassType
					)
				
									
							
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ClassType_Update procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ClassType_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ClassType_Update
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Updates a record in the ClassType table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ClassType_Update
(

	@ClassType varchar (50)  ,

	@OriginalClassType varchar (50)  
)
AS


				
				
				-- Modify the updatable columns
				UPDATE
					[dbo].[ClassType]
				SET
					[ClassType] = @ClassType
				WHERE
[ClassType] = @OriginalClassType 
				
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ClassType_Delete procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ClassType_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ClassType_Delete
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Deletes a record in the ClassType table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ClassType_Delete
(

	@ClassType varchar (50)  
)
AS


				DELETE FROM [dbo].[ClassType] WITH (ROWLOCK) 
				WHERE
					[ClassType] = @ClassType
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ClassType_GetByClassType procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ClassType_GetByClassType') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ClassType_GetByClassType
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the ClassType table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ClassType_GetByClassType
(

	@ClassType varchar (50)  
)
AS


				SELECT
					[ClassType]
				FROM
					[dbo].[ClassType]
				WHERE
					[ClassType] = @ClassType
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ClassType_Find procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ClassType_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ClassType_Find
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Finds records in the ClassType table passing nullable parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ClassType_Find
(

	@SearchUsingOR bit   = null ,

	@ClassType varchar (50)  = null 
)
AS


				
  IF ISNULL(@SearchUsingOR, 0) <> 1
  BEGIN
    SELECT
	  [ClassType]
    FROM
	[dbo].[ClassType]
    WHERE 
	 ([ClassType] = @ClassType OR @ClassType IS NULL)
						
  END
  ELSE
  BEGIN
    SELECT
	  [ClassType]
    FROM
	[dbo].[ClassType]
    WHERE 
	 ([ClassType] = @ClassType AND @ClassType is not null)
	SELECT @@ROWCOUNT			
  END
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Class_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Class_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Class_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the Class table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Class_Get_List

AS


				
				SELECT
					[Name],
					[DescriptionCode],
					[Category],
					[ClassType],
					[IsActive],
					[Alias]
				FROM
					[dbo].[Class]
					
				SELECT @@ROWCOUNT
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Class_GetPaged procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Class_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Class_GetPaged
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the Class table passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Class_GetPaged
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  ,

	@PageIndex int   ,

	@PageSize int   
)
AS


				
				BEGIN
				DECLARE @PageLowerBound int
				DECLARE @PageUpperBound int
				
				-- Set the page bounds
				SET @PageLowerBound = @PageSize * @PageIndex
				SET @PageUpperBound = @PageLowerBound + @PageSize

				-- Create a temp table to store the select results
				CREATE TABLE #PageIndex
				(
				    [IndexId] int IDENTITY (1, 1) NOT NULL,
				    [Name] varchar(50) COLLATE database_default  
				)
				
				-- Insert into the temp table
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = 'INSERT INTO #PageIndex ([Name])'
				SET @SQL = @SQL + ' SELECT'
				IF @PageSize > 0
				BEGIN
					SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound)
				END
				SET @SQL = @SQL + ' [Name]'
				SET @SQL = @SQL + ' FROM [dbo].[Class]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Populate the temp table
				EXEC sp_executesql @SQL

				-- Return paged results
				SELECT O.[Name], O.[DescriptionCode], O.[Category], O.[ClassType], O.[IsActive], O.[Alias]
				FROM
				    [dbo].[Class] O,
				    #PageIndex PageIndex
				WHERE
				    PageIndex.IndexId > @PageLowerBound
					AND O.[Name] = PageIndex.[Name]
				ORDER BY
				    PageIndex.IndexId
				
				-- get row count
				SET @SQL = 'SELECT COUNT(*) AS TotalRowCount'
				SET @SQL = @SQL + ' FROM [dbo].[Class]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				EXEC sp_executesql @SQL
			
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Class_Insert procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Class_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Class_Insert
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Inserts a record into the Class table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Class_Insert
(

	@Name varchar (50)  ,

	@DescriptionCode varchar (1000)  ,

	@Category varchar (50)  ,

	@ClassType varchar (50)  ,

	@IsActive bit   ,

	@Alias nvarchar (100)  
)
AS


				
				INSERT INTO [dbo].[Class]
					(
					[Name]
					,[DescriptionCode]
					,[Category]
					,[ClassType]
					,[IsActive]
					,[Alias]
					)
				VALUES
					(
					@Name
					,@DescriptionCode
					,@Category
					,@ClassType
					,@IsActive
					,@Alias
					)
				
									
							
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Class_Update procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Class_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Class_Update
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Updates a record in the Class table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Class_Update
(

	@Name varchar (50)  ,

	@OriginalName varchar (50)  ,

	@DescriptionCode varchar (1000)  ,

	@Category varchar (50)  ,

	@ClassType varchar (50)  ,

	@IsActive bit   ,

	@Alias nvarchar (100)  
)
AS


				
				
				-- Modify the updatable columns
				UPDATE
					[dbo].[Class]
				SET
					[Name] = @Name
					,[DescriptionCode] = @DescriptionCode
					,[Category] = @Category
					,[ClassType] = @ClassType
					,[IsActive] = @IsActive
					,[Alias] = @Alias
				WHERE
[Name] = @OriginalName 
				
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Class_Delete procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Class_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Class_Delete
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Deletes a record in the Class table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Class_Delete
(

	@Name varchar (50)  
)
AS


				DELETE FROM [dbo].[Class] WITH (ROWLOCK) 
				WHERE
					[Name] = @Name
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Class_GetByClassType procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Class_GetByClassType') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Class_GetByClassType
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the Class table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Class_GetByClassType
(

	@ClassType varchar (50)  
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[Name],
					[DescriptionCode],
					[Category],
					[ClassType],
					[IsActive],
					[Alias]
				FROM
					[dbo].[Class]
				WHERE
					[ClassType] = @ClassType
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Class_GetByName procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Class_GetByName') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Class_GetByName
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the Class table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Class_GetByName
(

	@Name varchar (50)  
)
AS


				SELECT
					[Name],
					[DescriptionCode],
					[Category],
					[ClassType],
					[IsActive],
					[Alias]
				FROM
					[dbo].[Class]
				WHERE
					[Name] = @Name
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Class_GetByCAidFromAllowedArtifact procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Class_GetByCAidFromAllowedArtifact') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Class_GetByCAidFromAllowedArtifact
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records through a junction table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Class_GetByCAidFromAllowedArtifact
(

	@CAid int   
)
AS


SELECT dbo.[Class].[Name]
       ,dbo.[Class].[DescriptionCode]
       ,dbo.[Class].[Category]
       ,dbo.[Class].[ClassType]
       ,dbo.[Class].[IsActive]
       ,dbo.[Class].[Alias]
  FROM dbo.[Class]
 WHERE EXISTS (SELECT 1
                 FROM dbo.[AllowedArtifact] 
                WHERE dbo.[AllowedArtifact].[CAid] = @CAid
                  AND dbo.[AllowedArtifact].[Class] = dbo.[Class].[Name]
                  )
				SELECT @@ROWCOUNT			
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Class_GetByModelTypeAcronymFromModelTypeClass procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Class_GetByModelTypeAcronymFromModelTypeClass') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Class_GetByModelTypeAcronymFromModelTypeClass
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records through a junction table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Class_GetByModelTypeAcronymFromModelTypeClass
(

	@ModelTypeAcronym varchar (50)  
)
AS


SELECT dbo.[Class].[Name]
       ,dbo.[Class].[DescriptionCode]
       ,dbo.[Class].[Category]
       ,dbo.[Class].[ClassType]
       ,dbo.[Class].[IsActive]
       ,dbo.[Class].[Alias]
  FROM dbo.[Class]
 WHERE EXISTS (SELECT 1
                 FROM dbo.[ModelTypeClass] 
                WHERE dbo.[ModelTypeClass].[ModelTypeAcronym] = @ModelTypeAcronym
                  AND dbo.[ModelTypeClass].[Class] = dbo.[Class].[Name]
                  )
				SELECT @@ROWCOUNT			
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Class_Find procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Class_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Class_Find
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Finds records in the Class table passing nullable parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Class_Find
(

	@SearchUsingOR bit   = null ,

	@Name varchar (50)  = null ,

	@DescriptionCode varchar (1000)  = null ,

	@Category varchar (50)  = null ,

	@ClassType varchar (50)  = null ,

	@IsActive bit   = null ,

	@Alias nvarchar (100)  = null 
)
AS


				
  IF ISNULL(@SearchUsingOR, 0) <> 1
  BEGIN
    SELECT
	  [Name]
	, [DescriptionCode]
	, [Category]
	, [ClassType]
	, [IsActive]
	, [Alias]
    FROM
	[dbo].[Class]
    WHERE 
	 ([Name] = @Name OR @Name IS NULL)
	AND ([DescriptionCode] = @DescriptionCode OR @DescriptionCode IS NULL)
	AND ([Category] = @Category OR @Category IS NULL)
	AND ([ClassType] = @ClassType OR @ClassType IS NULL)
	AND ([IsActive] = @IsActive OR @IsActive IS NULL)
	AND ([Alias] = @Alias OR @Alias IS NULL)
						
  END
  ELSE
  BEGIN
    SELECT
	  [Name]
	, [DescriptionCode]
	, [Category]
	, [ClassType]
	, [IsActive]
	, [Alias]
    FROM
	[dbo].[Class]
    WHERE 
	 ([Name] = @Name AND @Name is not null)
	OR ([DescriptionCode] = @DescriptionCode AND @DescriptionCode is not null)
	OR ([Category] = @Category AND @Category is not null)
	OR ([ClassType] = @ClassType AND @ClassType is not null)
	OR ([IsActive] = @IsActive AND @IsActive is not null)
	OR ([Alias] = @Alias AND @Alias is not null)
	SELECT @@ROWCOUNT			
  END
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_VCStatus_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_VCStatus_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_VCStatus_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the VCStatus table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_VCStatus_Get_List

AS


				
				SELECT
					[pkid],
					[Name]
				FROM
					[dbo].[VCStatus]
					
				SELECT @@ROWCOUNT
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_VCStatus_GetPaged procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_VCStatus_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_VCStatus_GetPaged
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the VCStatus table passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_VCStatus_GetPaged
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  ,

	@PageIndex int   ,

	@PageSize int   
)
AS


				
				BEGIN
				DECLARE @PageLowerBound int
				DECLARE @PageUpperBound int
				
				-- Set the page bounds
				SET @PageLowerBound = @PageSize * @PageIndex
				SET @PageUpperBound = @PageLowerBound + @PageSize

				-- Create a temp table to store the select results
				CREATE TABLE #PageIndex
				(
				    [IndexId] int IDENTITY (1, 1) NOT NULL,
				    [pkid] int 
				)
				
				-- Insert into the temp table
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = 'INSERT INTO #PageIndex ([pkid])'
				SET @SQL = @SQL + ' SELECT'
				IF @PageSize > 0
				BEGIN
					SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound)
				END
				SET @SQL = @SQL + ' [pkid]'
				SET @SQL = @SQL + ' FROM [dbo].[VCStatus]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Populate the temp table
				EXEC sp_executesql @SQL

				-- Return paged results
				SELECT O.[pkid], O.[Name]
				FROM
				    [dbo].[VCStatus] O,
				    #PageIndex PageIndex
				WHERE
				    PageIndex.IndexId > @PageLowerBound
					AND O.[pkid] = PageIndex.[pkid]
				ORDER BY
				    PageIndex.IndexId
				
				-- get row count
				SET @SQL = 'SELECT COUNT(*) AS TotalRowCount'
				SET @SQL = @SQL + ' FROM [dbo].[VCStatus]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				EXEC sp_executesql @SQL
			
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_VCStatus_Insert procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_VCStatus_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_VCStatus_Insert
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Inserts a record into the VCStatus table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_VCStatus_Insert
(

	@pkid int   ,

	@Name varchar (50)  
)
AS


				
				INSERT INTO [dbo].[VCStatus]
					(
					[pkid]
					,[Name]
					)
				VALUES
					(
					@pkid
					,@Name
					)
				
									
							
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_VCStatus_Update procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_VCStatus_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_VCStatus_Update
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Updates a record in the VCStatus table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_VCStatus_Update
(

	@pkid int   ,

	@Originalpkid int   ,

	@Name varchar (50)  
)
AS


				
				
				-- Modify the updatable columns
				UPDATE
					[dbo].[VCStatus]
				SET
					[pkid] = @pkid
					,[Name] = @Name
				WHERE
[pkid] = @Originalpkid 
				
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_VCStatus_Delete procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_VCStatus_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_VCStatus_Delete
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Deletes a record in the VCStatus table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_VCStatus_Delete
(

	@pkid int   
)
AS


				DELETE FROM [dbo].[VCStatus] WITH (ROWLOCK) 
				WHERE
					[pkid] = @pkid
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_VCStatus_GetByName procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_VCStatus_GetByName') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_VCStatus_GetByName
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the VCStatus table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_VCStatus_GetByName
(

	@Name varchar (50)  
)
AS


				SELECT
					[pkid],
					[Name]
				FROM
					[dbo].[VCStatus]
				WHERE
					[Name] = @Name
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_VCStatus_GetBypkid procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_VCStatus_GetBypkid') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_VCStatus_GetBypkid
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the VCStatus table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_VCStatus_GetBypkid
(

	@pkid int   
)
AS


				SELECT
					[pkid],
					[Name]
				FROM
					[dbo].[VCStatus]
				WHERE
					[pkid] = @pkid
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_VCStatus_GetByCAidFromObjectAssociation procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_VCStatus_GetByCAidFromObjectAssociation') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_VCStatus_GetByCAidFromObjectAssociation
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records through a junction table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_VCStatus_GetByCAidFromObjectAssociation
(

	@CAid int   
)
AS


SELECT dbo.[VCStatus].[pkid]
       ,dbo.[VCStatus].[Name]
  FROM dbo.[VCStatus]
 WHERE EXISTS (SELECT 1
                 FROM dbo.[ObjectAssociation] 
                WHERE dbo.[ObjectAssociation].[CAid] = @CAid
                  )
				SELECT @@ROWCOUNT			
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_VCStatus_GetByObjectIDObjectMachineFromObjectAssociation procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_VCStatus_GetByObjectIDObjectMachineFromObjectAssociation') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_VCStatus_GetByObjectIDObjectMachineFromObjectAssociation
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records through a junction table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_VCStatus_GetByObjectIDObjectMachineFromObjectAssociation
(

	@ObjectID int   ,

	@ObjectMachine varchar (50)  
)
AS


SELECT dbo.[VCStatus].[pkid]
       ,dbo.[VCStatus].[Name]
  FROM dbo.[VCStatus]
 WHERE EXISTS (SELECT 1
                 FROM dbo.[ObjectAssociation] 
                WHERE dbo.[ObjectAssociation].[ObjectID] = @ObjectID
                  AND dbo.[ObjectAssociation].[ObjectMachine] = @ObjectMachine
                  )
				SELECT @@ROWCOUNT			
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_VCStatus_GetByChildObjectIDChildObjectMachineFromObjectAssociation procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_VCStatus_GetByChildObjectIDChildObjectMachineFromObjectAssociation') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_VCStatus_GetByChildObjectIDChildObjectMachineFromObjectAssociation
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records through a junction table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_VCStatus_GetByChildObjectIDChildObjectMachineFromObjectAssociation
(

	@ChildObjectID int   ,

	@ChildObjectMachine varchar (50)  
)
AS


SELECT dbo.[VCStatus].[pkid]
       ,dbo.[VCStatus].[Name]
  FROM dbo.[VCStatus]
 WHERE EXISTS (SELECT 1
                 FROM dbo.[ObjectAssociation] 
                WHERE dbo.[ObjectAssociation].[ChildObjectID] = @ChildObjectID
                  AND dbo.[ObjectAssociation].[ChildObjectMachine] = @ChildObjectMachine
                  )
				SELECT @@ROWCOUNT			
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_VCStatus_Find procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_VCStatus_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_VCStatus_Find
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Finds records in the VCStatus table passing nullable parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_VCStatus_Find
(

	@SearchUsingOR bit   = null ,

	@pkid int   = null ,

	@Name varchar (50)  = null 
)
AS


				
  IF ISNULL(@SearchUsingOR, 0) <> 1
  BEGIN
    SELECT
	  [pkid]
	, [Name]
    FROM
	[dbo].[VCStatus]
    WHERE 
	 ([pkid] = @pkid OR @pkid IS NULL)
	AND ([Name] = @Name OR @Name IS NULL)
						
  END
  ELSE
  BEGIN
    SELECT
	  [pkid]
	, [Name]
    FROM
	[dbo].[VCStatus]
    WHERE 
	 ([pkid] = @pkid AND @pkid is not null)
	OR ([Name] = @Name AND @Name is not null)
	SELECT @@ROWCOUNT			
  END
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_MetaObject_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_MetaObject_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_MetaObject_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the MetaObject table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_MetaObject_Get_List

AS


				
				SELECT
					[pkid],
					[Class],
					[WorkspaceName],
					[UserID],
					[Machine],
					[VCStatusID],
					[VCMachineID],
					[WorkspaceTypeID],
					[DateCreated],
					[LastModified]
				FROM
					[dbo].[MetaObject]
					
				SELECT @@ROWCOUNT
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_MetaObject_GetPaged procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_MetaObject_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_MetaObject_GetPaged
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the MetaObject table passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_MetaObject_GetPaged
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  ,

	@PageIndex int   ,

	@PageSize int   
)
AS


				
				BEGIN
				DECLARE @PageLowerBound int
				DECLARE @PageUpperBound int
				
				-- Set the page bounds
				SET @PageLowerBound = @PageSize * @PageIndex
				SET @PageUpperBound = @PageLowerBound + @PageSize

				-- Create a temp table to store the select results
				CREATE TABLE #PageIndex
				(
				    [IndexId] int IDENTITY (1, 1) NOT NULL,
				    [pkid] int, [Machine] varchar(50) COLLATE database_default  
				)
				
				-- Insert into the temp table
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = 'INSERT INTO #PageIndex ([pkid], [Machine])'
				SET @SQL = @SQL + ' SELECT'
				IF @PageSize > 0
				BEGIN
					SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound)
				END
				SET @SQL = @SQL + ' [pkid], [Machine]'
				SET @SQL = @SQL + ' FROM [dbo].[MetaObject]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Populate the temp table
				EXEC sp_executesql @SQL

				-- Return paged results
				SELECT O.[pkid], O.[Class], O.[WorkspaceName], O.[UserID], O.[Machine], O.[VCStatusID], O.[VCMachineID], O.[WorkspaceTypeID], O.[DateCreated], O.[LastModified]
				FROM
				    [dbo].[MetaObject] O,
				    #PageIndex PageIndex
				WHERE
				    PageIndex.IndexId > @PageLowerBound
					AND O.[pkid] = PageIndex.[pkid]
					AND O.[Machine] = PageIndex.[Machine]
				ORDER BY
				    PageIndex.IndexId
				
				-- get row count
				SET @SQL = 'SELECT COUNT(*) AS TotalRowCount'
				SET @SQL = @SQL + ' FROM [dbo].[MetaObject]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				EXEC sp_executesql @SQL
			
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_MetaObject_Insert procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_MetaObject_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_MetaObject_Insert
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Inserts a record into the MetaObject table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_MetaObject_Insert
(

	@pkid int    OUTPUT,

	@Class varchar (50)  ,

	@WorkspaceName varchar (100)  ,

	@UserID int   ,

	@Machine varchar (50)  ,

	@VCStatusID int   ,

	@VCMachineID varchar (50)  ,

	@WorkspaceTypeID int   ,

	@DateCreated datetime   ,

	@LastModified datetime   
)
AS


				
				INSERT INTO [dbo].[MetaObject]
					(
					[Class]
					,[WorkspaceName]
					,[UserID]
					,[Machine]
					,[VCStatusID]
					,[VCMachineID]
					,[WorkspaceTypeID]
					,[DateCreated]
					,[LastModified]
					)
				VALUES
					(
					@Class
					,@WorkspaceName
					,@UserID
					,@Machine
					,@VCStatusID
					,@VCMachineID
					,@WorkspaceTypeID
					,@DateCreated
					,@LastModified
					)
				
				-- Get the identity value
				SET @pkid = SCOPE_IDENTITY()
									
							
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_MetaObject_Update procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_MetaObject_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_MetaObject_Update
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Updates a record in the MetaObject table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_MetaObject_Update
(

	@pkid int   ,

	@Class varchar (50)  ,

	@WorkspaceName varchar (100)  ,

	@UserID int   ,

	@Machine varchar (50)  ,

	@OriginalMachine varchar (50)  ,

	@VCStatusID int   ,

	@VCMachineID varchar (50)  ,

	@WorkspaceTypeID int   ,

	@DateCreated datetime   ,

	@LastModified datetime   
)
AS


				
				
				-- Modify the updatable columns
				UPDATE
					[dbo].[MetaObject]
				SET
					[Class] = @Class
					,[WorkspaceName] = @WorkspaceName
					,[UserID] = @UserID
					,[Machine] = @Machine
					,[VCStatusID] = @VCStatusID
					,[VCMachineID] = @VCMachineID
					,[WorkspaceTypeID] = @WorkspaceTypeID
					,[DateCreated] = @DateCreated
					,[LastModified] = @LastModified
				WHERE
[pkid] = @pkid 
AND [Machine] = @OriginalMachine 
				
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_MetaObject_Delete procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_MetaObject_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_MetaObject_Delete
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Deletes a record in the MetaObject table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_MetaObject_Delete
(

	@pkid int   ,

	@Machine varchar (50)  
)
AS


				DELETE FROM [dbo].[MetaObject] WITH (ROWLOCK) 
				WHERE
					[pkid] = @pkid
					AND [Machine] = @Machine
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_MetaObject_GetByVCStatusID procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_MetaObject_GetByVCStatusID') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_MetaObject_GetByVCStatusID
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the MetaObject table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_MetaObject_GetByVCStatusID
(

	@VCStatusID int   
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[pkid],
					[Class],
					[WorkspaceName],
					[UserID],
					[Machine],
					[VCStatusID],
					[VCMachineID],
					[WorkspaceTypeID],
					[DateCreated],
					[LastModified]
				FROM
					[dbo].[MetaObject]
				WHERE
					[VCStatusID] = @VCStatusID
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_MetaObject_GetByWorkspaceNameWorkspaceTypeID procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_MetaObject_GetByWorkspaceNameWorkspaceTypeID') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_MetaObject_GetByWorkspaceNameWorkspaceTypeID
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the MetaObject table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_MetaObject_GetByWorkspaceNameWorkspaceTypeID
(

	@WorkspaceName varchar (100)  ,

	@WorkspaceTypeID int   
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[pkid],
					[Class],
					[WorkspaceName],
					[UserID],
					[Machine],
					[VCStatusID],
					[VCMachineID],
					[WorkspaceTypeID],
					[DateCreated],
					[LastModified]
				FROM
					[dbo].[MetaObject]
				WHERE
					[WorkspaceName] = @WorkspaceName
					AND [WorkspaceTypeID] = @WorkspaceTypeID
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_MetaObject_GetByUserID procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_MetaObject_GetByUserID') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_MetaObject_GetByUserID
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the MetaObject table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_MetaObject_GetByUserID
(

	@UserID int   
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[pkid],
					[Class],
					[WorkspaceName],
					[UserID],
					[Machine],
					[VCStatusID],
					[VCMachineID],
					[WorkspaceTypeID],
					[DateCreated],
					[LastModified]
				FROM
					[dbo].[MetaObject]
				WHERE
					[UserID] = @UserID
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_MetaObject_GetByClass procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_MetaObject_GetByClass') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_MetaObject_GetByClass
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the MetaObject table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_MetaObject_GetByClass
(

	@Class varchar (50)  
)
AS


				SELECT
					[pkid],
					[Class],
					[WorkspaceName],
					[UserID],
					[Machine],
					[VCStatusID],
					[VCMachineID],
					[WorkspaceTypeID],
					[DateCreated],
					[LastModified]
				FROM
					[dbo].[MetaObject]
				WHERE
					[Class] = @Class
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_MetaObject_GetBypkidMachine procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_MetaObject_GetBypkidMachine') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_MetaObject_GetBypkidMachine
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the MetaObject table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_MetaObject_GetBypkidMachine
(

	@pkid int   ,

	@Machine varchar (50)  
)
AS


				SELECT
					[pkid],
					[Class],
					[WorkspaceName],
					[UserID],
					[Machine],
					[VCStatusID],
					[VCMachineID],
					[WorkspaceTypeID],
					[DateCreated],
					[LastModified]
				FROM
					[dbo].[MetaObject]
				WHERE
					[pkid] = @pkid
					AND [Machine] = @Machine
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_MetaObject_GetByGraphFileIDGraphFileMachineFromGraphFileObject procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_MetaObject_GetByGraphFileIDGraphFileMachineFromGraphFileObject') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_MetaObject_GetByGraphFileIDGraphFileMachineFromGraphFileObject
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records through a junction table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_MetaObject_GetByGraphFileIDGraphFileMachineFromGraphFileObject
(

	@GraphFileID int   ,

	@GraphFileMachine varchar (50)  
)
AS


SELECT dbo.[MetaObject].[pkid]
       ,dbo.[MetaObject].[Class]
       ,dbo.[MetaObject].[WorkspaceName]
       ,dbo.[MetaObject].[UserID]
       ,dbo.[MetaObject].[Machine]
       ,dbo.[MetaObject].[VCStatusID]
       ,dbo.[MetaObject].[VCMachineID]
       ,dbo.[MetaObject].[WorkspaceTypeID]
       ,dbo.[MetaObject].[DateCreated]
       ,dbo.[MetaObject].[LastModified]
  FROM dbo.[MetaObject]
 WHERE EXISTS (SELECT 1
                 FROM dbo.[GraphFileObject] 
                WHERE dbo.[GraphFileObject].[GraphFileID] = @GraphFileID
                  AND dbo.[GraphFileObject].[GraphFileMachine] = @GraphFileMachine
                  AND dbo.[GraphFileObject].[MetaObjectID] = dbo.[MetaObject].[pkid]
                  AND dbo.[GraphFileObject].[MachineID] = dbo.[MetaObject].[Machine]
                  )
				SELECT @@ROWCOUNT			
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_MetaObject_GetByCAidFromObjectAssociation procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_MetaObject_GetByCAidFromObjectAssociation') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_MetaObject_GetByCAidFromObjectAssociation
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records through a junction table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_MetaObject_GetByCAidFromObjectAssociation
(

	@CAid int   
)
AS


SELECT dbo.[MetaObject].[pkid]
       ,dbo.[MetaObject].[Class]
       ,dbo.[MetaObject].[WorkspaceName]
       ,dbo.[MetaObject].[UserID]
       ,dbo.[MetaObject].[Machine]
       ,dbo.[MetaObject].[VCStatusID]
       ,dbo.[MetaObject].[VCMachineID]
       ,dbo.[MetaObject].[WorkspaceTypeID]
       ,dbo.[MetaObject].[DateCreated]
       ,dbo.[MetaObject].[LastModified]
  FROM dbo.[MetaObject]
 WHERE EXISTS (SELECT 1
                 FROM dbo.[ObjectAssociation] 
                WHERE dbo.[ObjectAssociation].[CAid] = @CAid
                  AND dbo.[ObjectAssociation].[ObjectID] = dbo.[MetaObject].[pkid]
                  AND dbo.[ObjectAssociation].[ObjectMachine] = dbo.[MetaObject].[Machine]
                  )
				SELECT @@ROWCOUNT			
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_MetaObject_GetByChildObjectIDChildObjectMachineFromObjectAssociation procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_MetaObject_GetByChildObjectIDChildObjectMachineFromObjectAssociation') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_MetaObject_GetByChildObjectIDChildObjectMachineFromObjectAssociation
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records through a junction table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_MetaObject_GetByChildObjectIDChildObjectMachineFromObjectAssociation
(

	@ChildObjectID int   ,

	@ChildObjectMachine varchar (50)  
)
AS


SELECT dbo.[MetaObject].[pkid]
       ,dbo.[MetaObject].[Class]
       ,dbo.[MetaObject].[WorkspaceName]
       ,dbo.[MetaObject].[UserID]
       ,dbo.[MetaObject].[Machine]
       ,dbo.[MetaObject].[VCStatusID]
       ,dbo.[MetaObject].[VCMachineID]
       ,dbo.[MetaObject].[WorkspaceTypeID]
       ,dbo.[MetaObject].[DateCreated]
       ,dbo.[MetaObject].[LastModified]
  FROM dbo.[MetaObject]
 WHERE EXISTS (SELECT 1
                 FROM dbo.[ObjectAssociation] 
                WHERE dbo.[ObjectAssociation].[ChildObjectID] = @ChildObjectID
                  AND dbo.[ObjectAssociation].[ChildObjectMachine] = @ChildObjectMachine
                  AND dbo.[ObjectAssociation].[ObjectID] = dbo.[MetaObject].[pkid]
                  AND dbo.[ObjectAssociation].[ObjectMachine] = dbo.[MetaObject].[Machine]
                  )
				SELECT @@ROWCOUNT			
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_MetaObject_GetByCAidFromObjectAssociation procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_MetaObject_GetByCAidFromObjectAssociation') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_MetaObject_GetByCAidFromObjectAssociation
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records through a junction table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_MetaObject_GetByCAidFromObjectAssociation
(

	@CAid int   
)
AS


SELECT dbo.[MetaObject].[pkid]
       ,dbo.[MetaObject].[Class]
       ,dbo.[MetaObject].[WorkspaceName]
       ,dbo.[MetaObject].[UserID]
       ,dbo.[MetaObject].[Machine]
       ,dbo.[MetaObject].[VCStatusID]
       ,dbo.[MetaObject].[VCMachineID]
       ,dbo.[MetaObject].[WorkspaceTypeID]
       ,dbo.[MetaObject].[DateCreated]
       ,dbo.[MetaObject].[LastModified]
  FROM dbo.[MetaObject]
 WHERE EXISTS (SELECT 1
                 FROM dbo.[ObjectAssociation] 
                WHERE dbo.[ObjectAssociation].[CAid] = @CAid
                  AND dbo.[ObjectAssociation].[ChildObjectID] = dbo.[MetaObject].[pkid]
                  AND dbo.[ObjectAssociation].[ChildObjectMachine] = dbo.[MetaObject].[Machine]
                  )
				SELECT @@ROWCOUNT			
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_MetaObject_GetByObjectIDObjectMachineFromObjectAssociation procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_MetaObject_GetByObjectIDObjectMachineFromObjectAssociation') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_MetaObject_GetByObjectIDObjectMachineFromObjectAssociation
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records through a junction table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_MetaObject_GetByObjectIDObjectMachineFromObjectAssociation
(

	@ObjectID int   ,

	@ObjectMachine varchar (50)  
)
AS


SELECT dbo.[MetaObject].[pkid]
       ,dbo.[MetaObject].[Class]
       ,dbo.[MetaObject].[WorkspaceName]
       ,dbo.[MetaObject].[UserID]
       ,dbo.[MetaObject].[Machine]
       ,dbo.[MetaObject].[VCStatusID]
       ,dbo.[MetaObject].[VCMachineID]
       ,dbo.[MetaObject].[WorkspaceTypeID]
       ,dbo.[MetaObject].[DateCreated]
       ,dbo.[MetaObject].[LastModified]
  FROM dbo.[MetaObject]
 WHERE EXISTS (SELECT 1
                 FROM dbo.[ObjectAssociation] 
                WHERE dbo.[ObjectAssociation].[ObjectID] = @ObjectID
                  AND dbo.[ObjectAssociation].[ObjectMachine] = @ObjectMachine
                  AND dbo.[ObjectAssociation].[ChildObjectID] = dbo.[MetaObject].[pkid]
                  AND dbo.[ObjectAssociation].[ChildObjectMachine] = dbo.[MetaObject].[Machine]
                  )
				SELECT @@ROWCOUNT			
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_MetaObject_GetByFieldIDFromObjectFieldValue procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_MetaObject_GetByFieldIDFromObjectFieldValue') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_MetaObject_GetByFieldIDFromObjectFieldValue
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records through a junction table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_MetaObject_GetByFieldIDFromObjectFieldValue
(

	@FieldID int   
)
AS


SELECT dbo.[MetaObject].[pkid]
       ,dbo.[MetaObject].[Class]
       ,dbo.[MetaObject].[WorkspaceName]
       ,dbo.[MetaObject].[UserID]
       ,dbo.[MetaObject].[Machine]
       ,dbo.[MetaObject].[VCStatusID]
       ,dbo.[MetaObject].[VCMachineID]
       ,dbo.[MetaObject].[WorkspaceTypeID]
       ,dbo.[MetaObject].[DateCreated]
       ,dbo.[MetaObject].[LastModified]
  FROM dbo.[MetaObject]
 WHERE EXISTS (SELECT 1
                 FROM dbo.[ObjectFieldValue] 
                WHERE dbo.[ObjectFieldValue].[FieldID] = @FieldID
                  AND dbo.[ObjectFieldValue].[ObjectID] = dbo.[MetaObject].[pkid]
                  AND dbo.[ObjectFieldValue].[MachineID] = dbo.[MetaObject].[Machine]
                  )
				SELECT @@ROWCOUNT			
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_MetaObject_Find procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_MetaObject_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_MetaObject_Find
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Finds records in the MetaObject table passing nullable parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_MetaObject_Find
(

	@SearchUsingOR bit   = null ,

	@pkid int   = null ,

	@Class varchar (50)  = null ,

	@WorkspaceName varchar (100)  = null ,

	@UserID int   = null ,

	@Machine varchar (50)  = null ,

	@VCStatusID int   = null ,

	@VCMachineID varchar (50)  = null ,

	@WorkspaceTypeID int   = null ,

	@DateCreated datetime   = null ,

	@LastModified datetime   = null 
)
AS


				
  IF ISNULL(@SearchUsingOR, 0) <> 1
  BEGIN
    SELECT
	  [pkid]
	, [Class]
	, [WorkspaceName]
	, [UserID]
	, [Machine]
	, [VCStatusID]
	, [VCMachineID]
	, [WorkspaceTypeID]
	, [DateCreated]
	, [LastModified]
    FROM
	[dbo].[MetaObject]
    WHERE 
	 ([pkid] = @pkid OR @pkid IS NULL)
	AND ([Class] = @Class OR @Class IS NULL)
	AND ([WorkspaceName] = @WorkspaceName OR @WorkspaceName IS NULL)
	AND ([UserID] = @UserID OR @UserID IS NULL)
	AND ([Machine] = @Machine OR @Machine IS NULL)
	AND ([VCStatusID] = @VCStatusID OR @VCStatusID IS NULL)
	AND ([VCMachineID] = @VCMachineID OR @VCMachineID IS NULL)
	AND ([WorkspaceTypeID] = @WorkspaceTypeID OR @WorkspaceTypeID IS NULL)
	AND ([DateCreated] = @DateCreated OR @DateCreated IS NULL)
	AND ([LastModified] = @LastModified OR @LastModified IS NULL)
						
  END
  ELSE
  BEGIN
    SELECT
	  [pkid]
	, [Class]
	, [WorkspaceName]
	, [UserID]
	, [Machine]
	, [VCStatusID]
	, [VCMachineID]
	, [WorkspaceTypeID]
	, [DateCreated]
	, [LastModified]
    FROM
	[dbo].[MetaObject]
    WHERE 
	 ([pkid] = @pkid AND @pkid is not null)
	OR ([Class] = @Class AND @Class is not null)
	OR ([WorkspaceName] = @WorkspaceName AND @WorkspaceName is not null)
	OR ([UserID] = @UserID AND @UserID is not null)
	OR ([Machine] = @Machine AND @Machine is not null)
	OR ([VCStatusID] = @VCStatusID AND @VCStatusID is not null)
	OR ([VCMachineID] = @VCMachineID AND @VCMachineID is not null)
	OR ([WorkspaceTypeID] = @WorkspaceTypeID AND @WorkspaceTypeID is not null)
	OR ([DateCreated] = @DateCreated AND @DateCreated is not null)
	OR ([LastModified] = @LastModified AND @LastModified is not null)
	SELECT @@ROWCOUNT			
  END
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ObjectFieldValue_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ObjectFieldValue_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ObjectFieldValue_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the ObjectFieldValue table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ObjectFieldValue_Get_List

AS


				
				SELECT
					[ObjectID],
					[FieldID],
					[ValueString],
					[ValueInt],
					[ValueDouble],
					[ValueObjectID],
					[ValueDate],
					[ValueBoolean],
					[ValueLongText],
					[ValueRTF],
					[MachineID]
				FROM
					[dbo].[ObjectFieldValue]
					
				SELECT @@ROWCOUNT
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ObjectFieldValue_GetPaged procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ObjectFieldValue_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ObjectFieldValue_GetPaged
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the ObjectFieldValue table passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ObjectFieldValue_GetPaged
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  ,

	@PageIndex int   ,

	@PageSize int   
)
AS


				
				BEGIN
				DECLARE @PageLowerBound int
				DECLARE @PageUpperBound int
				
				-- Set the page bounds
				SET @PageLowerBound = @PageSize * @PageIndex
				SET @PageUpperBound = @PageLowerBound + @PageSize

				-- Create a temp table to store the select results
				CREATE TABLE #PageIndex
				(
				    [IndexId] int IDENTITY (1, 1) NOT NULL,
				    [ObjectID] int, [FieldID] int, [MachineID] varchar(50) COLLATE database_default  
				)
				
				-- Insert into the temp table
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = 'INSERT INTO #PageIndex ([ObjectID], [FieldID], [MachineID])'
				SET @SQL = @SQL + ' SELECT'
				IF @PageSize > 0
				BEGIN
					SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound)
				END
				SET @SQL = @SQL + ' [ObjectID], [FieldID], [MachineID]'
				SET @SQL = @SQL + ' FROM [dbo].[ObjectFieldValue]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Populate the temp table
				EXEC sp_executesql @SQL

				-- Return paged results
				SELECT O.[ObjectID], O.[FieldID], O.[ValueString], O.[ValueInt], O.[ValueDouble], O.[ValueObjectID], O.[ValueDate], O.[ValueBoolean], O.[ValueLongText], O.[ValueRTF], O.[MachineID]
				FROM
				    [dbo].[ObjectFieldValue] O,
				    #PageIndex PageIndex
				WHERE
				    PageIndex.IndexId > @PageLowerBound
					AND O.[ObjectID] = PageIndex.[ObjectID]
					AND O.[FieldID] = PageIndex.[FieldID]
					AND O.[MachineID] = PageIndex.[MachineID]
				ORDER BY
				    PageIndex.IndexId
				
				-- get row count
				SET @SQL = 'SELECT COUNT(*) AS TotalRowCount'
				SET @SQL = @SQL + ' FROM [dbo].[ObjectFieldValue]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				EXEC sp_executesql @SQL
			
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ObjectFieldValue_Insert procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ObjectFieldValue_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ObjectFieldValue_Insert
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Inserts a record into the ObjectFieldValue table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ObjectFieldValue_Insert
(

	@ObjectID int   ,

	@FieldID int   ,

	@ValueString varchar (255)  ,

	@ValueInt int   ,

	@ValueDouble numeric (18, 2)  ,

	@ValueObjectID int   ,

	@ValueDate datetime   ,

	@ValueBoolean bit   ,

	@ValueLongText text   ,

	@ValueRTF text   ,

	@MachineID varchar (50)  
)
AS


				
				INSERT INTO [dbo].[ObjectFieldValue]
					(
					[ObjectID]
					,[FieldID]
					,[ValueString]
					,[ValueInt]
					,[ValueDouble]
					,[ValueObjectID]
					,[ValueDate]
					,[ValueBoolean]
					,[ValueLongText]
					,[ValueRTF]
					,[MachineID]
					)
				VALUES
					(
					@ObjectID
					,@FieldID
					,@ValueString
					,@ValueInt
					,@ValueDouble
					,@ValueObjectID
					,@ValueDate
					,@ValueBoolean
					,@ValueLongText
					,@ValueRTF
					,@MachineID
					)
				
									
							
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ObjectFieldValue_Update procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ObjectFieldValue_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ObjectFieldValue_Update
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Updates a record in the ObjectFieldValue table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ObjectFieldValue_Update
(

	@ObjectID int   ,

	@OriginalObjectID int   ,

	@FieldID int   ,

	@OriginalFieldID int   ,

	@ValueString varchar (255)  ,

	@ValueInt int   ,

	@ValueDouble numeric (18, 2)  ,

	@ValueObjectID int   ,

	@ValueDate datetime   ,

	@ValueBoolean bit   ,

	@ValueLongText text   ,

	@ValueRTF text   ,

	@MachineID varchar (50)  ,

	@OriginalMachineID varchar (50)  
)
AS


				
				
				-- Modify the updatable columns
				UPDATE
					[dbo].[ObjectFieldValue]
				SET
					[ObjectID] = @ObjectID
					,[FieldID] = @FieldID
					,[ValueString] = @ValueString
					,[ValueInt] = @ValueInt
					,[ValueDouble] = @ValueDouble
					,[ValueObjectID] = @ValueObjectID
					,[ValueDate] = @ValueDate
					,[ValueBoolean] = @ValueBoolean
					,[ValueLongText] = @ValueLongText
					,[ValueRTF] = @ValueRTF
					,[MachineID] = @MachineID
				WHERE
[ObjectID] = @OriginalObjectID 
AND [FieldID] = @OriginalFieldID 
AND [MachineID] = @OriginalMachineID 
				
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ObjectFieldValue_Delete procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ObjectFieldValue_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ObjectFieldValue_Delete
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Deletes a record in the ObjectFieldValue table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ObjectFieldValue_Delete
(

	@ObjectID int   ,

	@FieldID int   ,

	@MachineID varchar (50)  
)
AS


				DELETE FROM [dbo].[ObjectFieldValue] WITH (ROWLOCK) 
				WHERE
					[ObjectID] = @ObjectID
					AND [FieldID] = @FieldID
					AND [MachineID] = @MachineID
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ObjectFieldValue_GetByFieldID procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ObjectFieldValue_GetByFieldID') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ObjectFieldValue_GetByFieldID
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the ObjectFieldValue table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ObjectFieldValue_GetByFieldID
(

	@FieldID int   
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[ObjectID],
					[FieldID],
					[ValueString],
					[ValueInt],
					[ValueDouble],
					[ValueObjectID],
					[ValueDate],
					[ValueBoolean],
					[ValueLongText],
					[ValueRTF],
					[MachineID]
				FROM
					[dbo].[ObjectFieldValue]
				WHERE
					[FieldID] = @FieldID
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ObjectFieldValue_GetByObjectIDMachineID procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ObjectFieldValue_GetByObjectIDMachineID') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ObjectFieldValue_GetByObjectIDMachineID
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the ObjectFieldValue table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ObjectFieldValue_GetByObjectIDMachineID
(

	@ObjectID int   ,

	@MachineID varchar (50)  
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[ObjectID],
					[FieldID],
					[ValueString],
					[ValueInt],
					[ValueDouble],
					[ValueObjectID],
					[ValueDate],
					[ValueBoolean],
					[ValueLongText],
					[ValueRTF],
					[MachineID]
				FROM
					[dbo].[ObjectFieldValue]
				WHERE
					[ObjectID] = @ObjectID
					AND [MachineID] = @MachineID
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ObjectFieldValue_GetByValueString procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ObjectFieldValue_GetByValueString') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ObjectFieldValue_GetByValueString
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the ObjectFieldValue table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ObjectFieldValue_GetByValueString
(

	@ValueString varchar (255)  
)
AS


				SELECT
					[ObjectID],
					[FieldID],
					[ValueString],
					[ValueInt],
					[ValueDouble],
					[ValueObjectID],
					[ValueDate],
					[ValueBoolean],
					[ValueLongText],
					[ValueRTF],
					[MachineID]
				FROM
					[dbo].[ObjectFieldValue]
				WHERE
					[ValueString] = @ValueString
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ObjectFieldValue_GetByValueInt procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ObjectFieldValue_GetByValueInt') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ObjectFieldValue_GetByValueInt
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the ObjectFieldValue table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ObjectFieldValue_GetByValueInt
(

	@ValueInt int   
)
AS


				SELECT
					[ObjectID],
					[FieldID],
					[ValueString],
					[ValueInt],
					[ValueDouble],
					[ValueObjectID],
					[ValueDate],
					[ValueBoolean],
					[ValueLongText],
					[ValueRTF],
					[MachineID]
				FROM
					[dbo].[ObjectFieldValue]
				WHERE
					[ValueInt] = @ValueInt
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ObjectFieldValue_GetByObjectIDFieldIDMachineID procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ObjectFieldValue_GetByObjectIDFieldIDMachineID') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ObjectFieldValue_GetByObjectIDFieldIDMachineID
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the ObjectFieldValue table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ObjectFieldValue_GetByObjectIDFieldIDMachineID
(

	@ObjectID int   ,

	@FieldID int   ,

	@MachineID varchar (50)  
)
AS


				SELECT
					[ObjectID],
					[FieldID],
					[ValueString],
					[ValueInt],
					[ValueDouble],
					[ValueObjectID],
					[ValueDate],
					[ValueBoolean],
					[ValueLongText],
					[ValueRTF],
					[MachineID]
				FROM
					[dbo].[ObjectFieldValue]
				WHERE
					[ObjectID] = @ObjectID
					AND [FieldID] = @FieldID
					AND [MachineID] = @MachineID
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ObjectFieldValue_Find procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ObjectFieldValue_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ObjectFieldValue_Find
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Finds records in the ObjectFieldValue table passing nullable parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ObjectFieldValue_Find
(

	@SearchUsingOR bit   = null ,

	@ObjectID int   = null ,

	@FieldID int   = null ,

	@ValueString varchar (255)  = null ,

	@ValueInt int   = null ,

	@ValueDouble numeric (18, 2)  = null ,

	@ValueObjectID int   = null ,

	@ValueDate datetime   = null ,

	@ValueBoolean bit   = null ,

	@ValueLongText text   = null ,

	@ValueRTF text   = null ,

	@MachineID varchar (50)  = null 
)
AS


				
  IF ISNULL(@SearchUsingOR, 0) <> 1
  BEGIN
    SELECT
	  [ObjectID]
	, [FieldID]
	, [ValueString]
	, [ValueInt]
	, [ValueDouble]
	, [ValueObjectID]
	, [ValueDate]
	, [ValueBoolean]
	, [ValueLongText]
	, [ValueRTF]
	, [MachineID]
    FROM
	[dbo].[ObjectFieldValue]
    WHERE 
	 ([ObjectID] = @ObjectID OR @ObjectID IS NULL)
	AND ([FieldID] = @FieldID OR @FieldID IS NULL)
	AND ([ValueString] = @ValueString OR @ValueString IS NULL)
	AND ([ValueInt] = @ValueInt OR @ValueInt IS NULL)
	AND ([ValueDouble] = @ValueDouble OR @ValueDouble IS NULL)
	AND ([ValueObjectID] = @ValueObjectID OR @ValueObjectID IS NULL)
	AND ([ValueDate] = @ValueDate OR @ValueDate IS NULL)
	AND ([ValueBoolean] = @ValueBoolean OR @ValueBoolean IS NULL)
	AND ([MachineID] = @MachineID OR @MachineID IS NULL)
						
  END
  ELSE
  BEGIN
    SELECT
	  [ObjectID]
	, [FieldID]
	, [ValueString]
	, [ValueInt]
	, [ValueDouble]
	, [ValueObjectID]
	, [ValueDate]
	, [ValueBoolean]
	, [ValueLongText]
	, [ValueRTF]
	, [MachineID]
    FROM
	[dbo].[ObjectFieldValue]
    WHERE 
	 ([ObjectID] = @ObjectID AND @ObjectID is not null)
	OR ([FieldID] = @FieldID AND @FieldID is not null)
	OR ([ValueString] = @ValueString AND @ValueString is not null)
	OR ([ValueInt] = @ValueInt AND @ValueInt is not null)
	OR ([ValueDouble] = @ValueDouble AND @ValueDouble is not null)
	OR ([ValueObjectID] = @ValueObjectID AND @ValueObjectID is not null)
	OR ([ValueDate] = @ValueDate AND @ValueDate is not null)
	OR ([ValueBoolean] = @ValueBoolean AND @ValueBoolean is not null)
	OR ([MachineID] = @MachineID AND @MachineID is not null)
	SELECT @@ROWCOUNT			
  END
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ObjectAssociation_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ObjectAssociation_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ObjectAssociation_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the ObjectAssociation table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ObjectAssociation_Get_List

AS


				
				SELECT
					[CAid],
					[ObjectID],
					[ChildObjectID],
					[Series],
					[ObjectMachine],
					[ChildObjectMachine],
					[VCStatusID],
					[VCMachineID],
					[Machine]
				FROM
					[dbo].[ObjectAssociation]
					
				SELECT @@ROWCOUNT
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ObjectAssociation_GetPaged procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ObjectAssociation_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ObjectAssociation_GetPaged
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the ObjectAssociation table passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ObjectAssociation_GetPaged
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  ,

	@PageIndex int   ,

	@PageSize int   
)
AS


				
				BEGIN
				DECLARE @PageLowerBound int
				DECLARE @PageUpperBound int
				
				-- Set the page bounds
				SET @PageLowerBound = @PageSize * @PageIndex
				SET @PageUpperBound = @PageLowerBound + @PageSize

				-- Create a temp table to store the select results
				CREATE TABLE #PageIndex
				(
				    [IndexId] int IDENTITY (1, 1) NOT NULL,
				    [CAid] int, [ObjectID] int, [ChildObjectID] int, [ObjectMachine] varchar(50) COLLATE database_default , [ChildObjectMachine] varchar(50) COLLATE database_default  
				)
				
				-- Insert into the temp table
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = 'INSERT INTO #PageIndex ([CAid], [ObjectID], [ChildObjectID], [ObjectMachine], [ChildObjectMachine])'
				SET @SQL = @SQL + ' SELECT'
				IF @PageSize > 0
				BEGIN
					SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound)
				END
				SET @SQL = @SQL + ' [CAid], [ObjectID], [ChildObjectID], [ObjectMachine], [ChildObjectMachine]'
				SET @SQL = @SQL + ' FROM [dbo].[ObjectAssociation]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Populate the temp table
				EXEC sp_executesql @SQL

				-- Return paged results
				SELECT O.[CAid], O.[ObjectID], O.[ChildObjectID], O.[Series], O.[ObjectMachine], O.[ChildObjectMachine], O.[VCStatusID], O.[VCMachineID], O.[Machine]
				FROM
				    [dbo].[ObjectAssociation] O,
				    #PageIndex PageIndex
				WHERE
				    PageIndex.IndexId > @PageLowerBound
					AND O.[CAid] = PageIndex.[CAid]
					AND O.[ObjectID] = PageIndex.[ObjectID]
					AND O.[ChildObjectID] = PageIndex.[ChildObjectID]
					AND O.[ObjectMachine] = PageIndex.[ObjectMachine]
					AND O.[ChildObjectMachine] = PageIndex.[ChildObjectMachine]
				ORDER BY
				    PageIndex.IndexId
				
				-- get row count
				SET @SQL = 'SELECT COUNT(*) AS TotalRowCount'
				SET @SQL = @SQL + ' FROM [dbo].[ObjectAssociation]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				EXEC sp_executesql @SQL
			
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ObjectAssociation_Insert procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ObjectAssociation_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ObjectAssociation_Insert
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Inserts a record into the ObjectAssociation table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ObjectAssociation_Insert
(

	@CAid int   ,

	@ObjectID int   ,

	@ChildObjectID int   ,

	@Series int   ,

	@ObjectMachine varchar (50)  ,

	@ChildObjectMachine varchar (50)  ,

	@VCStatusID int   ,

	@VCMachineID varchar (50)  ,

	@Machine varchar (50)  
)
AS


				
				INSERT INTO [dbo].[ObjectAssociation]
					(
					[CAid]
					,[ObjectID]
					,[ChildObjectID]
					,[Series]
					,[ObjectMachine]
					,[ChildObjectMachine]
					,[VCStatusID]
					,[VCMachineID]
					,[Machine]
					)
				VALUES
					(
					@CAid
					,@ObjectID
					,@ChildObjectID
					,@Series
					,@ObjectMachine
					,@ChildObjectMachine
					,@VCStatusID
					,@VCMachineID
					,@Machine
					)
				
									
							
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ObjectAssociation_Update procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ObjectAssociation_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ObjectAssociation_Update
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Updates a record in the ObjectAssociation table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ObjectAssociation_Update
(

	@CAid int   ,

	@OriginalCAid int   ,

	@ObjectID int   ,

	@OriginalObjectID int   ,

	@ChildObjectID int   ,

	@OriginalChildObjectID int   ,

	@Series int   ,

	@ObjectMachine varchar (50)  ,

	@OriginalObjectMachine varchar (50)  ,

	@ChildObjectMachine varchar (50)  ,

	@OriginalChildObjectMachine varchar (50)  ,

	@VCStatusID int   ,

	@VCMachineID varchar (50)  ,

	@Machine varchar (50)  
)
AS


				
				
				-- Modify the updatable columns
				UPDATE
					[dbo].[ObjectAssociation]
				SET
					[CAid] = @CAid
					,[ObjectID] = @ObjectID
					,[ChildObjectID] = @ChildObjectID
					,[Series] = @Series
					,[ObjectMachine] = @ObjectMachine
					,[ChildObjectMachine] = @ChildObjectMachine
					,[VCStatusID] = @VCStatusID
					,[VCMachineID] = @VCMachineID
					,[Machine] = @Machine
				WHERE
[CAid] = @OriginalCAid 
AND [ObjectID] = @OriginalObjectID 
AND [ChildObjectID] = @OriginalChildObjectID 
AND [ObjectMachine] = @OriginalObjectMachine 
AND [ChildObjectMachine] = @OriginalChildObjectMachine 
				
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ObjectAssociation_Delete procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ObjectAssociation_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ObjectAssociation_Delete
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Deletes a record in the ObjectAssociation table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ObjectAssociation_Delete
(

	@CAid int   ,

	@ObjectID int   ,

	@ChildObjectID int   ,

	@ObjectMachine varchar (50)  ,

	@ChildObjectMachine varchar (50)  
)
AS


				DELETE FROM [dbo].[ObjectAssociation] WITH (ROWLOCK) 
				WHERE
					[CAid] = @CAid
					AND [ObjectID] = @ObjectID
					AND [ChildObjectID] = @ChildObjectID
					AND [ObjectMachine] = @ObjectMachine
					AND [ChildObjectMachine] = @ChildObjectMachine
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ObjectAssociation_GetByCAid procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ObjectAssociation_GetByCAid') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ObjectAssociation_GetByCAid
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the ObjectAssociation table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ObjectAssociation_GetByCAid
(

	@CAid int   
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[CAid],
					[ObjectID],
					[ChildObjectID],
					[Series],
					[ObjectMachine],
					[ChildObjectMachine],
					[VCStatusID],
					[VCMachineID],
					[Machine]
				FROM
					[dbo].[ObjectAssociation]
				WHERE
					[CAid] = @CAid
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ObjectAssociation_GetByObjectIDObjectMachine procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ObjectAssociation_GetByObjectIDObjectMachine') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ObjectAssociation_GetByObjectIDObjectMachine
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the ObjectAssociation table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ObjectAssociation_GetByObjectIDObjectMachine
(

	@ObjectID int   ,

	@ObjectMachine varchar (50)  
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[CAid],
					[ObjectID],
					[ChildObjectID],
					[Series],
					[ObjectMachine],
					[ChildObjectMachine],
					[VCStatusID],
					[VCMachineID],
					[Machine]
				FROM
					[dbo].[ObjectAssociation]
				WHERE
					[ObjectID] = @ObjectID
					AND [ObjectMachine] = @ObjectMachine
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ObjectAssociation_GetByChildObjectIDChildObjectMachine procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ObjectAssociation_GetByChildObjectIDChildObjectMachine') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ObjectAssociation_GetByChildObjectIDChildObjectMachine
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the ObjectAssociation table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ObjectAssociation_GetByChildObjectIDChildObjectMachine
(

	@ChildObjectID int   ,

	@ChildObjectMachine varchar (50)  
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[CAid],
					[ObjectID],
					[ChildObjectID],
					[Series],
					[ObjectMachine],
					[ChildObjectMachine],
					[VCStatusID],
					[VCMachineID],
					[Machine]
				FROM
					[dbo].[ObjectAssociation]
				WHERE
					[ChildObjectID] = @ChildObjectID
					AND [ChildObjectMachine] = @ChildObjectMachine
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ObjectAssociation_GetByVCStatusID procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ObjectAssociation_GetByVCStatusID') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ObjectAssociation_GetByVCStatusID
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the ObjectAssociation table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ObjectAssociation_GetByVCStatusID
(

	@VCStatusID int   
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[CAid],
					[ObjectID],
					[ChildObjectID],
					[Series],
					[ObjectMachine],
					[ChildObjectMachine],
					[VCStatusID],
					[VCMachineID],
					[Machine]
				FROM
					[dbo].[ObjectAssociation]
				WHERE
					[VCStatusID] = @VCStatusID
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ObjectAssociation_GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ObjectAssociation_GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ObjectAssociation_GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the ObjectAssociation table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ObjectAssociation_GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine
(

	@CAid int   ,

	@ObjectID int   ,

	@ChildObjectID int   ,

	@ObjectMachine varchar (50)  ,

	@ChildObjectMachine varchar (50)  
)
AS


				SELECT
					[CAid],
					[ObjectID],
					[ChildObjectID],
					[Series],
					[ObjectMachine],
					[ChildObjectMachine],
					[VCStatusID],
					[VCMachineID],
					[Machine]
				FROM
					[dbo].[ObjectAssociation]
				WHERE
					[CAid] = @CAid
					AND [ObjectID] = @ObjectID
					AND [ChildObjectID] = @ChildObjectID
					AND [ObjectMachine] = @ObjectMachine
					AND [ChildObjectMachine] = @ChildObjectMachine
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ObjectAssociation_GetByGraphFileIDGraphFileMachineFromGraphFileAssociation procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ObjectAssociation_GetByGraphFileIDGraphFileMachineFromGraphFileAssociation') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ObjectAssociation_GetByGraphFileIDGraphFileMachineFromGraphFileAssociation
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records through a junction table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ObjectAssociation_GetByGraphFileIDGraphFileMachineFromGraphFileAssociation
(

	@GraphFileID int   ,

	@GraphFileMachine varchar (50)  
)
AS


SELECT dbo.[ObjectAssociation].[CAid]
       ,dbo.[ObjectAssociation].[ObjectID]
       ,dbo.[ObjectAssociation].[ChildObjectID]
       ,dbo.[ObjectAssociation].[Series]
       ,dbo.[ObjectAssociation].[ObjectMachine]
       ,dbo.[ObjectAssociation].[ChildObjectMachine]
       ,dbo.[ObjectAssociation].[VCStatusID]
       ,dbo.[ObjectAssociation].[VCMachineID]
       ,dbo.[ObjectAssociation].[Machine]
  FROM dbo.[ObjectAssociation]
 WHERE EXISTS (SELECT 1
                 FROM dbo.[GraphFileAssociation] 
                WHERE dbo.[GraphFileAssociation].[GraphFileID] = @GraphFileID
                  AND dbo.[GraphFileAssociation].[GraphFileMachine] = @GraphFileMachine
                  AND dbo.[GraphFileAssociation].[CAid] = dbo.[ObjectAssociation].[CAid]
                  AND dbo.[GraphFileAssociation].[ObjectID] = dbo.[ObjectAssociation].[ObjectID]
                  AND dbo.[GraphFileAssociation].[ChildObjectID] = dbo.[ObjectAssociation].[ChildObjectID]
                  AND dbo.[GraphFileAssociation].[ObjectMachine] = dbo.[ObjectAssociation].[ObjectMachine]
                  AND dbo.[GraphFileAssociation].[ChildObjectMachine] = dbo.[ObjectAssociation].[ChildObjectMachine]
                  )
				SELECT @@ROWCOUNT			
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ObjectAssociation_Find procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ObjectAssociation_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ObjectAssociation_Find
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Finds records in the ObjectAssociation table passing nullable parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ObjectAssociation_Find
(

	@SearchUsingOR bit   = null ,

	@CAid int   = null ,

	@ObjectID int   = null ,

	@ChildObjectID int   = null ,

	@Series int   = null ,

	@ObjectMachine varchar (50)  = null ,

	@ChildObjectMachine varchar (50)  = null ,

	@VCStatusID int   = null ,

	@VCMachineID varchar (50)  = null ,

	@Machine varchar (50)  = null 
)
AS


				
  IF ISNULL(@SearchUsingOR, 0) <> 1
  BEGIN
    SELECT
	  [CAid]
	, [ObjectID]
	, [ChildObjectID]
	, [Series]
	, [ObjectMachine]
	, [ChildObjectMachine]
	, [VCStatusID]
	, [VCMachineID]
	, [Machine]
    FROM
	[dbo].[ObjectAssociation]
    WHERE 
	 ([CAid] = @CAid OR @CAid IS NULL)
	AND ([ObjectID] = @ObjectID OR @ObjectID IS NULL)
	AND ([ChildObjectID] = @ChildObjectID OR @ChildObjectID IS NULL)
	AND ([Series] = @Series OR @Series IS NULL)
	AND ([ObjectMachine] = @ObjectMachine OR @ObjectMachine IS NULL)
	AND ([ChildObjectMachine] = @ChildObjectMachine OR @ChildObjectMachine IS NULL)
	AND ([VCStatusID] = @VCStatusID OR @VCStatusID IS NULL)
	AND ([VCMachineID] = @VCMachineID OR @VCMachineID IS NULL)
	AND ([Machine] = @Machine OR @Machine IS NULL)
						
  END
  ELSE
  BEGIN
    SELECT
	  [CAid]
	, [ObjectID]
	, [ChildObjectID]
	, [Series]
	, [ObjectMachine]
	, [ChildObjectMachine]
	, [VCStatusID]
	, [VCMachineID]
	, [Machine]
    FROM
	[dbo].[ObjectAssociation]
    WHERE 
	 ([CAid] = @CAid AND @CAid is not null)
	OR ([ObjectID] = @ObjectID AND @ObjectID is not null)
	OR ([ChildObjectID] = @ChildObjectID AND @ChildObjectID is not null)
	OR ([Series] = @Series AND @Series is not null)
	OR ([ObjectMachine] = @ObjectMachine AND @ObjectMachine is not null)
	OR ([ChildObjectMachine] = @ChildObjectMachine AND @ChildObjectMachine is not null)
	OR ([VCStatusID] = @VCStatusID AND @VCStatusID is not null)
	OR ([VCMachineID] = @VCMachineID AND @VCMachineID is not null)
	OR ([Machine] = @Machine AND @Machine is not null)
	SELECT @@ROWCOUNT			
  END
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFile_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFile_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFile_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the GraphFile table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFile_Get_List

AS


				
				SELECT
					[pkid],
					[MajorVersion],
					[MinorVersion],
					[ModifiedDate],
					[Notes],
					[IsActive],
					[Archived],
					[AppVersion],
					[Blob],
					[WorkspaceName],
					[FileTypeID],
					[PreviousVersionID],
					[Name],
					[VCStatusID],
					[VCMachineID],
					[Machine],
					[WorkspaceTypeID],
					[OriginalFileUniqueID]
				FROM
					[dbo].[GraphFile]
					
				SELECT @@ROWCOUNT
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFile_GetPaged procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFile_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFile_GetPaged
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the GraphFile table passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFile_GetPaged
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  ,

	@PageIndex int   ,

	@PageSize int   
)
AS


				
				BEGIN
				DECLARE @PageLowerBound int
				DECLARE @PageUpperBound int
				
				-- Set the page bounds
				SET @PageLowerBound = @PageSize * @PageIndex
				SET @PageUpperBound = @PageLowerBound + @PageSize

				-- Create a temp table to store the select results
				CREATE TABLE #PageIndex
				(
				    [IndexId] int IDENTITY (1, 1) NOT NULL,
				    [pkid] int, [Machine] varchar(50) COLLATE database_default  
				)
				
				-- Insert into the temp table
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = 'INSERT INTO #PageIndex ([pkid], [Machine])'
				SET @SQL = @SQL + ' SELECT'
				IF @PageSize > 0
				BEGIN
					SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound)
				END
				SET @SQL = @SQL + ' [pkid], [Machine]'
				SET @SQL = @SQL + ' FROM [dbo].[GraphFile]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Populate the temp table
				EXEC sp_executesql @SQL

				-- Return paged results
				SELECT O.[pkid], O.[MajorVersion], O.[MinorVersion], O.[ModifiedDate], O.[Notes], O.[IsActive], O.[Archived], O.[AppVersion], O.[Blob], O.[WorkspaceName], O.[FileTypeID], O.[PreviousVersionID], O.[Name], O.[VCStatusID], O.[VCMachineID], O.[Machine], O.[WorkspaceTypeID], O.[OriginalFileUniqueID]
				FROM
				    [dbo].[GraphFile] O,
				    #PageIndex PageIndex
				WHERE
				    PageIndex.IndexId > @PageLowerBound
					AND O.[pkid] = PageIndex.[pkid]
					AND O.[Machine] = PageIndex.[Machine]
				ORDER BY
				    PageIndex.IndexId
				
				-- get row count
				SET @SQL = 'SELECT COUNT(*) AS TotalRowCount'
				SET @SQL = @SQL + ' FROM [dbo].[GraphFile]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				EXEC sp_executesql @SQL
			
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFile_Insert procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFile_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFile_Insert
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Inserts a record into the GraphFile table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFile_Insert
(

	@pkid int    OUTPUT,

	@MajorVersion int   ,

	@MinorVersion int   ,

	@ModifiedDate datetime   ,

	@Notes varchar (255)  ,

	@IsActive bit   ,

	@Archived bit   ,

	@AppVersion varchar (20)  ,

	@Blob image   ,

	@WorkspaceName varchar (100)  ,

	@FileTypeID int   ,

	@PreviousVersionID int   ,

	@Name varchar (255)  ,

	@VCStatusID int   ,

	@VCMachineID varchar (50)  ,

	@Machine varchar (50)  ,

	@WorkspaceTypeID int   ,

	@OriginalFileUniqueID uniqueidentifier   
)
AS


				
				INSERT INTO [dbo].[GraphFile]
					(
					[MajorVersion]
					,[MinorVersion]
					,[ModifiedDate]
					,[Notes]
					,[IsActive]
					,[Archived]
					,[AppVersion]
					,[Blob]
					,[WorkspaceName]
					,[FileTypeID]
					,[PreviousVersionID]
					,[Name]
					,[VCStatusID]
					,[VCMachineID]
					,[Machine]
					,[WorkspaceTypeID]
					,[OriginalFileUniqueID]
					)
				VALUES
					(
					@MajorVersion
					,@MinorVersion
					,@ModifiedDate
					,@Notes
					,@IsActive
					,@Archived
					,@AppVersion
					,@Blob
					,@WorkspaceName
					,@FileTypeID
					,@PreviousVersionID
					,@Name
					,@VCStatusID
					,@VCMachineID
					,@Machine
					,@WorkspaceTypeID
					,@OriginalFileUniqueID
					)
				
				-- Get the identity value
				SET @pkid = SCOPE_IDENTITY()
									
							
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFile_Update procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFile_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFile_Update
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Updates a record in the GraphFile table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFile_Update
(

	@pkid int   ,

	@MajorVersion int   ,

	@MinorVersion int   ,

	@ModifiedDate datetime   ,

	@Notes varchar (255)  ,

	@IsActive bit   ,

	@Archived bit   ,

	@AppVersion varchar (20)  ,

	@Blob image   ,

	@WorkspaceName varchar (100)  ,

	@FileTypeID int   ,

	@PreviousVersionID int   ,

	@Name varchar (255)  ,

	@VCStatusID int   ,

	@VCMachineID varchar (50)  ,

	@Machine varchar (50)  ,

	@OriginalMachine varchar (50)  ,

	@WorkspaceTypeID int   ,

	@OriginalFileUniqueID uniqueidentifier   
)
AS


				
				
				-- Modify the updatable columns
				UPDATE
					[dbo].[GraphFile]
				SET
					[MajorVersion] = @MajorVersion
					,[MinorVersion] = @MinorVersion
					,[ModifiedDate] = @ModifiedDate
					,[Notes] = @Notes
					,[IsActive] = @IsActive
					,[Archived] = @Archived
					,[AppVersion] = @AppVersion
					,[Blob] = @Blob
					,[WorkspaceName] = @WorkspaceName
					,[FileTypeID] = @FileTypeID
					,[PreviousVersionID] = @PreviousVersionID
					,[Name] = @Name
					,[VCStatusID] = @VCStatusID
					,[VCMachineID] = @VCMachineID
					,[Machine] = @Machine
					,[WorkspaceTypeID] = @WorkspaceTypeID
					,[OriginalFileUniqueID] = @OriginalFileUniqueID
				WHERE
[pkid] = @pkid 
AND [Machine] = @OriginalMachine 
				
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFile_Delete procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFile_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFile_Delete
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Deletes a record in the GraphFile table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFile_Delete
(

	@pkid int   ,

	@Machine varchar (50)  
)
AS


				DELETE FROM [dbo].[GraphFile] WITH (ROWLOCK) 
				WHERE
					[pkid] = @pkid
					AND [Machine] = @Machine
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFile_GetByFileTypeID procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFile_GetByFileTypeID') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFile_GetByFileTypeID
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the GraphFile table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFile_GetByFileTypeID
(

	@FileTypeID int   
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[pkid],
					[MajorVersion],
					[MinorVersion],
					[ModifiedDate],
					[Notes],
					[IsActive],
					[Archived],
					[AppVersion],
					[Blob],
					[WorkspaceName],
					[FileTypeID],
					[PreviousVersionID],
					[Name],
					[VCStatusID],
					[VCMachineID],
					[Machine],
					[WorkspaceTypeID],
					[OriginalFileUniqueID]
				FROM
					[dbo].[GraphFile]
				WHERE
					[FileTypeID] = @FileTypeID
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFile_GetByVCStatusID procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFile_GetByVCStatusID') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFile_GetByVCStatusID
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the GraphFile table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFile_GetByVCStatusID
(

	@VCStatusID int   
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[pkid],
					[MajorVersion],
					[MinorVersion],
					[ModifiedDate],
					[Notes],
					[IsActive],
					[Archived],
					[AppVersion],
					[Blob],
					[WorkspaceName],
					[FileTypeID],
					[PreviousVersionID],
					[Name],
					[VCStatusID],
					[VCMachineID],
					[Machine],
					[WorkspaceTypeID],
					[OriginalFileUniqueID]
				FROM
					[dbo].[GraphFile]
				WHERE
					[VCStatusID] = @VCStatusID
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFile_GetByWorkspaceNameWorkspaceTypeID procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFile_GetByWorkspaceNameWorkspaceTypeID') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFile_GetByWorkspaceNameWorkspaceTypeID
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the GraphFile table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFile_GetByWorkspaceNameWorkspaceTypeID
(

	@WorkspaceName varchar (100)  ,

	@WorkspaceTypeID int   
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[pkid],
					[MajorVersion],
					[MinorVersion],
					[ModifiedDate],
					[Notes],
					[IsActive],
					[Archived],
					[AppVersion],
					[Blob],
					[WorkspaceName],
					[FileTypeID],
					[PreviousVersionID],
					[Name],
					[VCStatusID],
					[VCMachineID],
					[Machine],
					[WorkspaceTypeID],
					[OriginalFileUniqueID]
				FROM
					[dbo].[GraphFile]
				WHERE
					[WorkspaceName] = @WorkspaceName
					AND [WorkspaceTypeID] = @WorkspaceTypeID
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFile_GetBypkidMachine procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFile_GetBypkidMachine') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFile_GetBypkidMachine
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the GraphFile table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFile_GetBypkidMachine
(

	@pkid int   ,

	@Machine varchar (50)  
)
AS


				SELECT
					[pkid],
					[MajorVersion],
					[MinorVersion],
					[ModifiedDate],
					[Notes],
					[IsActive],
					[Archived],
					[AppVersion],
					[Blob],
					[WorkspaceName],
					[FileTypeID],
					[PreviousVersionID],
					[Name],
					[VCStatusID],
					[VCMachineID],
					[Machine],
					[WorkspaceTypeID],
					[OriginalFileUniqueID]
				FROM
					[dbo].[GraphFile]
				WHERE
					[pkid] = @pkid
					AND [Machine] = @Machine
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFile_GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFile_GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFile_GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records through a junction table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFile_GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation
(

	@CAid int   ,

	@ObjectID int   ,

	@ChildObjectID int   ,

	@ObjectMachine varchar (50)  ,

	@ChildObjectMachine varchar (50)  
)
AS


SELECT dbo.[GraphFile].[pkid]
       ,dbo.[GraphFile].[MajorVersion]
       ,dbo.[GraphFile].[MinorVersion]
       ,dbo.[GraphFile].[ModifiedDate]
       ,dbo.[GraphFile].[Notes]
       ,dbo.[GraphFile].[IsActive]
       ,dbo.[GraphFile].[Archived]
       ,dbo.[GraphFile].[AppVersion]
       ,dbo.[GraphFile].[Blob]
       ,dbo.[GraphFile].[WorkspaceName]
       ,dbo.[GraphFile].[FileTypeID]
       ,dbo.[GraphFile].[PreviousVersionID]
       ,dbo.[GraphFile].[Name]
       ,dbo.[GraphFile].[VCStatusID]
       ,dbo.[GraphFile].[VCMachineID]
       ,dbo.[GraphFile].[Machine]
       ,dbo.[GraphFile].[WorkspaceTypeID]
       ,dbo.[GraphFile].[OriginalFileUniqueID]
  FROM dbo.[GraphFile]
 WHERE EXISTS (SELECT 1
                 FROM dbo.[GraphFileAssociation] 
                WHERE dbo.[GraphFileAssociation].[CAid] = @CAid
                  AND dbo.[GraphFileAssociation].[ObjectID] = @ObjectID
                  AND dbo.[GraphFileAssociation].[ChildObjectID] = @ChildObjectID
                  AND dbo.[GraphFileAssociation].[ObjectMachine] = @ObjectMachine
                  AND dbo.[GraphFileAssociation].[ChildObjectMachine] = @ChildObjectMachine
                  AND dbo.[GraphFileAssociation].[GraphFileID] = dbo.[GraphFile].[pkid]
                  AND dbo.[GraphFileAssociation].[GraphFileMachine] = dbo.[GraphFile].[Machine]
                  )
				SELECT @@ROWCOUNT			
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFile_GetByMetaObjectIDMachineIDFromGraphFileObject procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFile_GetByMetaObjectIDMachineIDFromGraphFileObject') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFile_GetByMetaObjectIDMachineIDFromGraphFileObject
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records through a junction table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFile_GetByMetaObjectIDMachineIDFromGraphFileObject
(

	@MetaObjectID int   ,

	@MachineID varchar (50)  
)
AS


SELECT dbo.[GraphFile].[pkid]
       ,dbo.[GraphFile].[MajorVersion]
       ,dbo.[GraphFile].[MinorVersion]
       ,dbo.[GraphFile].[ModifiedDate]
       ,dbo.[GraphFile].[Notes]
       ,dbo.[GraphFile].[IsActive]
       ,dbo.[GraphFile].[Archived]
       ,dbo.[GraphFile].[AppVersion]
       ,dbo.[GraphFile].[Blob]
       ,dbo.[GraphFile].[WorkspaceName]
       ,dbo.[GraphFile].[FileTypeID]
       ,dbo.[GraphFile].[PreviousVersionID]
       ,dbo.[GraphFile].[Name]
       ,dbo.[GraphFile].[VCStatusID]
       ,dbo.[GraphFile].[VCMachineID]
       ,dbo.[GraphFile].[Machine]
       ,dbo.[GraphFile].[WorkspaceTypeID]
       ,dbo.[GraphFile].[OriginalFileUniqueID]
  FROM dbo.[GraphFile]
 WHERE EXISTS (SELECT 1
                 FROM dbo.[GraphFileObject] 
                WHERE dbo.[GraphFileObject].[MetaObjectID] = @MetaObjectID
                  AND dbo.[GraphFileObject].[MachineID] = @MachineID
                  AND dbo.[GraphFileObject].[GraphFileID] = dbo.[GraphFile].[pkid]
                  AND dbo.[GraphFileObject].[GraphFileMachine] = dbo.[GraphFile].[Machine]
                  )
				SELECT @@ROWCOUNT			
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFile_Find procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFile_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFile_Find
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Finds records in the GraphFile table passing nullable parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFile_Find
(

	@SearchUsingOR bit   = null ,

	@pkid int   = null ,

	@MajorVersion int   = null ,

	@MinorVersion int   = null ,

	@ModifiedDate datetime   = null ,

	@Notes varchar (255)  = null ,

	@IsActive bit   = null ,

	@Archived bit   = null ,

	@AppVersion varchar (20)  = null ,

	@Blob image   = null ,

	@WorkspaceName varchar (100)  = null ,

	@FileTypeID int   = null ,

	@PreviousVersionID int   = null ,

	@Name varchar (255)  = null ,

	@VCStatusID int   = null ,

	@VCMachineID varchar (50)  = null ,

	@Machine varchar (50)  = null ,

	@WorkspaceTypeID int   = null ,

	@OriginalFileUniqueID uniqueidentifier   = null 
)
AS


				
  IF ISNULL(@SearchUsingOR, 0) <> 1
  BEGIN
    SELECT
	  [pkid]
	, [MajorVersion]
	, [MinorVersion]
	, [ModifiedDate]
	, [Notes]
	, [IsActive]
	, [Archived]
	, [AppVersion]
	, [Blob]
	, [WorkspaceName]
	, [FileTypeID]
	, [PreviousVersionID]
	, [Name]
	, [VCStatusID]
	, [VCMachineID]
	, [Machine]
	, [WorkspaceTypeID]
	, [OriginalFileUniqueID]
    FROM
	[dbo].[GraphFile]
    WHERE 
	 ([pkid] = @pkid OR @pkid IS NULL)
	AND ([MajorVersion] = @MajorVersion OR @MajorVersion IS NULL)
	AND ([MinorVersion] = @MinorVersion OR @MinorVersion IS NULL)
	AND ([ModifiedDate] = @ModifiedDate OR @ModifiedDate IS NULL)
	AND ([Notes] = @Notes OR @Notes IS NULL)
	AND ([IsActive] = @IsActive OR @IsActive IS NULL)
	AND ([Archived] = @Archived OR @Archived IS NULL)
	AND ([AppVersion] = @AppVersion OR @AppVersion IS NULL)
	AND ([WorkspaceName] = @WorkspaceName OR @WorkspaceName IS NULL)
	AND ([FileTypeID] = @FileTypeID OR @FileTypeID IS NULL)
	AND ([PreviousVersionID] = @PreviousVersionID OR @PreviousVersionID IS NULL)
	AND ([Name] = @Name OR @Name IS NULL)
	AND ([VCStatusID] = @VCStatusID OR @VCStatusID IS NULL)
	AND ([VCMachineID] = @VCMachineID OR @VCMachineID IS NULL)
	AND ([Machine] = @Machine OR @Machine IS NULL)
	AND ([WorkspaceTypeID] = @WorkspaceTypeID OR @WorkspaceTypeID IS NULL)
	AND ([OriginalFileUniqueID] = @OriginalFileUniqueID OR @OriginalFileUniqueID IS NULL)
						
  END
  ELSE
  BEGIN
    SELECT
	  [pkid]
	, [MajorVersion]
	, [MinorVersion]
	, [ModifiedDate]
	, [Notes]
	, [IsActive]
	, [Archived]
	, [AppVersion]
	, [Blob]
	, [WorkspaceName]
	, [FileTypeID]
	, [PreviousVersionID]
	, [Name]
	, [VCStatusID]
	, [VCMachineID]
	, [Machine]
	, [WorkspaceTypeID]
	, [OriginalFileUniqueID]
    FROM
	[dbo].[GraphFile]
    WHERE 
	 ([pkid] = @pkid AND @pkid is not null)
	OR ([MajorVersion] = @MajorVersion AND @MajorVersion is not null)
	OR ([MinorVersion] = @MinorVersion AND @MinorVersion is not null)
	OR ([ModifiedDate] = @ModifiedDate AND @ModifiedDate is not null)
	OR ([Notes] = @Notes AND @Notes is not null)
	OR ([IsActive] = @IsActive AND @IsActive is not null)
	OR ([Archived] = @Archived AND @Archived is not null)
	OR ([AppVersion] = @AppVersion AND @AppVersion is not null)
	OR ([WorkspaceName] = @WorkspaceName AND @WorkspaceName is not null)
	OR ([FileTypeID] = @FileTypeID AND @FileTypeID is not null)
	OR ([PreviousVersionID] = @PreviousVersionID AND @PreviousVersionID is not null)
	OR ([Name] = @Name AND @Name is not null)
	OR ([VCStatusID] = @VCStatusID AND @VCStatusID is not null)
	OR ([VCMachineID] = @VCMachineID AND @VCMachineID is not null)
	OR ([Machine] = @Machine AND @Machine is not null)
	OR ([WorkspaceTypeID] = @WorkspaceTypeID AND @WorkspaceTypeID is not null)
	OR ([OriginalFileUniqueID] = @OriginalFileUniqueID AND @OriginalFileUniqueID is not null)
	SELECT @@ROWCOUNT			
  END
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Workspace_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Workspace_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Workspace_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the Workspace table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Workspace_Get_List

AS


				
				SELECT
					[pkid],
					[Name],
					[WorkspaceTypeID],
					[RequestedByUser],
					[IsActive]
				FROM
					[dbo].[Workspace]
					
				SELECT @@ROWCOUNT
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Workspace_GetPaged procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Workspace_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Workspace_GetPaged
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the Workspace table passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Workspace_GetPaged
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  ,

	@PageIndex int   ,

	@PageSize int   
)
AS


				
				BEGIN
				DECLARE @PageLowerBound int
				DECLARE @PageUpperBound int
				
				-- Set the page bounds
				SET @PageLowerBound = @PageSize * @PageIndex
				SET @PageUpperBound = @PageLowerBound + @PageSize

				-- Create a temp table to store the select results
				CREATE TABLE #PageIndex
				(
				    [IndexId] int IDENTITY (1, 1) NOT NULL,
				    [Name] varchar(100) COLLATE database_default , [WorkspaceTypeID] int 
				)
				
				-- Insert into the temp table
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = 'INSERT INTO #PageIndex ([Name], [WorkspaceTypeID])'
				SET @SQL = @SQL + ' SELECT'
				IF @PageSize > 0
				BEGIN
					SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound)
				END
				SET @SQL = @SQL + ' [Name], [WorkspaceTypeID]'
				SET @SQL = @SQL + ' FROM [dbo].[Workspace]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Populate the temp table
				EXEC sp_executesql @SQL

				-- Return paged results
				SELECT O.[pkid], O.[Name], O.[WorkspaceTypeID], O.[RequestedByUser], O.[IsActive]
				FROM
				    [dbo].[Workspace] O,
				    #PageIndex PageIndex
				WHERE
				    PageIndex.IndexId > @PageLowerBound
					AND O.[Name] = PageIndex.[Name]
					AND O.[WorkspaceTypeID] = PageIndex.[WorkspaceTypeID]
				ORDER BY
				    PageIndex.IndexId
				
				-- get row count
				SET @SQL = 'SELECT COUNT(*) AS TotalRowCount'
				SET @SQL = @SQL + ' FROM [dbo].[Workspace]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				EXEC sp_executesql @SQL
			
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Workspace_Insert procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Workspace_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Workspace_Insert
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Inserts a record into the Workspace table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Workspace_Insert
(

	@pkid int    OUTPUT,

	@Name varchar (100)  ,

	@WorkspaceTypeID int   ,

	@RequestedByUser varchar (50)  ,

	@IsActive bit   
)
AS


				
				INSERT INTO [dbo].[Workspace]
					(
					[Name]
					,[WorkspaceTypeID]
					,[RequestedByUser]
					,[IsActive]
					)
				VALUES
					(
					@Name
					,@WorkspaceTypeID
					,@RequestedByUser
					,@IsActive
					)
				
				-- Get the identity value
				SET @pkid = SCOPE_IDENTITY()
									
							
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Workspace_Update procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Workspace_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Workspace_Update
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Updates a record in the Workspace table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Workspace_Update
(

	@pkid int   ,

	@Name varchar (100)  ,

	@OriginalName varchar (100)  ,

	@WorkspaceTypeID int   ,

	@OriginalWorkspaceTypeID int   ,

	@RequestedByUser varchar (50)  ,

	@IsActive bit   
)
AS


				
				
				-- Modify the updatable columns
				UPDATE
					[dbo].[Workspace]
				SET
					[Name] = @Name
					,[WorkspaceTypeID] = @WorkspaceTypeID
					,[RequestedByUser] = @RequestedByUser
					,[IsActive] = @IsActive
				WHERE
[Name] = @OriginalName 
AND [WorkspaceTypeID] = @OriginalWorkspaceTypeID 
				
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Workspace_Delete procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Workspace_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Workspace_Delete
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Deletes a record in the Workspace table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Workspace_Delete
(

	@Name varchar (100)  ,

	@WorkspaceTypeID int   
)
AS


				DELETE FROM [dbo].[Workspace] WITH (ROWLOCK) 
				WHERE
					[Name] = @Name
					AND [WorkspaceTypeID] = @WorkspaceTypeID
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Workspace_GetByWorkspaceTypeID procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Workspace_GetByWorkspaceTypeID') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Workspace_GetByWorkspaceTypeID
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the Workspace table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Workspace_GetByWorkspaceTypeID
(

	@WorkspaceTypeID int   
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[pkid],
					[Name],
					[WorkspaceTypeID],
					[RequestedByUser],
					[IsActive]
				FROM
					[dbo].[Workspace]
				WHERE
					[WorkspaceTypeID] = @WorkspaceTypeID
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Workspace_GetByNameWorkspaceTypeID procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Workspace_GetByNameWorkspaceTypeID') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Workspace_GetByNameWorkspaceTypeID
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the Workspace table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Workspace_GetByNameWorkspaceTypeID
(

	@Name varchar (100)  ,

	@WorkspaceTypeID int   
)
AS


				SELECT
					[pkid],
					[Name],
					[WorkspaceTypeID],
					[RequestedByUser],
					[IsActive]
				FROM
					[dbo].[Workspace]
				WHERE
					[Name] = @Name
					AND [WorkspaceTypeID] = @WorkspaceTypeID
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Workspace_GetByPermissionIDFromUserPermission procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Workspace_GetByPermissionIDFromUserPermission') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Workspace_GetByPermissionIDFromUserPermission
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records through a junction table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Workspace_GetByPermissionIDFromUserPermission
(

	@PermissionID int   
)
AS


SELECT dbo.[Workspace].[pkid]
       ,dbo.[Workspace].[Name]
       ,dbo.[Workspace].[WorkspaceTypeID]
       ,dbo.[Workspace].[RequestedByUser]
       ,dbo.[Workspace].[IsActive]
  FROM dbo.[Workspace]
 WHERE EXISTS (SELECT 1
                 FROM dbo.[UserPermission] 
                WHERE dbo.[UserPermission].[PermissionID] = @PermissionID
                  AND dbo.[UserPermission].[WorkspaceName] = dbo.[Workspace].[Name]
                  AND dbo.[UserPermission].[WorkspaceTypeID] = dbo.[Workspace].[WorkspaceTypeID]
                  )
				SELECT @@ROWCOUNT			
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Workspace_GetByUserIDFromUserPermission procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Workspace_GetByUserIDFromUserPermission') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Workspace_GetByUserIDFromUserPermission
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records through a junction table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Workspace_GetByUserIDFromUserPermission
(

	@UserID int   
)
AS


SELECT dbo.[Workspace].[pkid]
       ,dbo.[Workspace].[Name]
       ,dbo.[Workspace].[WorkspaceTypeID]
       ,dbo.[Workspace].[RequestedByUser]
       ,dbo.[Workspace].[IsActive]
  FROM dbo.[Workspace]
 WHERE EXISTS (SELECT 1
                 FROM dbo.[UserPermission] 
                WHERE dbo.[UserPermission].[UserID] = @UserID
                  AND dbo.[UserPermission].[WorkspaceName] = dbo.[Workspace].[Name]
                  AND dbo.[UserPermission].[WorkspaceTypeID] = dbo.[Workspace].[WorkspaceTypeID]
                  )
				SELECT @@ROWCOUNT			
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Workspace_Find procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Workspace_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Workspace_Find
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Finds records in the Workspace table passing nullable parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Workspace_Find
(

	@SearchUsingOR bit   = null ,

	@pkid int   = null ,

	@Name varchar (100)  = null ,

	@WorkspaceTypeID int   = null ,

	@RequestedByUser varchar (50)  = null ,

	@IsActive bit   = null 
)
AS


				
  IF ISNULL(@SearchUsingOR, 0) <> 1
  BEGIN
    SELECT
	  [pkid]
	, [Name]
	, [WorkspaceTypeID]
	, [RequestedByUser]
	, [IsActive]
    FROM
	[dbo].[Workspace]
    WHERE 
	 ([pkid] = @pkid OR @pkid IS NULL)
	AND ([Name] = @Name OR @Name IS NULL)
	AND ([WorkspaceTypeID] = @WorkspaceTypeID OR @WorkspaceTypeID IS NULL)
	AND ([RequestedByUser] = @RequestedByUser OR @RequestedByUser IS NULL)
	AND ([IsActive] = @IsActive OR @IsActive IS NULL)
						
  END
  ELSE
  BEGIN
    SELECT
	  [pkid]
	, [Name]
	, [WorkspaceTypeID]
	, [RequestedByUser]
	, [IsActive]
    FROM
	[dbo].[Workspace]
    WHERE 
	 ([pkid] = @pkid AND @pkid is not null)
	OR ([Name] = @Name AND @Name is not null)
	OR ([WorkspaceTypeID] = @WorkspaceTypeID AND @WorkspaceTypeID is not null)
	OR ([RequestedByUser] = @RequestedByUser AND @RequestedByUser is not null)
	OR ([IsActive] = @IsActive AND @IsActive is not null)
	SELECT @@ROWCOUNT			
  END
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ModelTypeClass_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ModelTypeClass_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ModelTypeClass_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the ModelTypeClass table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ModelTypeClass_Get_List

AS


				
				SELECT
					[Class],
					[ModelTypeAcronym],
					[IsDefault]
				FROM
					[dbo].[ModelTypeClass]
					
				SELECT @@ROWCOUNT
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ModelTypeClass_GetPaged procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ModelTypeClass_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ModelTypeClass_GetPaged
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the ModelTypeClass table passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ModelTypeClass_GetPaged
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  ,

	@PageIndex int   ,

	@PageSize int   
)
AS


				
				BEGIN
				DECLARE @PageLowerBound int
				DECLARE @PageUpperBound int
				
				-- Set the page bounds
				SET @PageLowerBound = @PageSize * @PageIndex
				SET @PageUpperBound = @PageLowerBound + @PageSize

				-- Create a temp table to store the select results
				CREATE TABLE #PageIndex
				(
				    [IndexId] int IDENTITY (1, 1) NOT NULL,
				    [Class] varchar(50) COLLATE database_default , [ModelTypeAcronym] varchar(50) COLLATE database_default  
				)
				
				-- Insert into the temp table
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = 'INSERT INTO #PageIndex ([Class], [ModelTypeAcronym])'
				SET @SQL = @SQL + ' SELECT'
				IF @PageSize > 0
				BEGIN
					SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound)
				END
				SET @SQL = @SQL + ' [Class], [ModelTypeAcronym]'
				SET @SQL = @SQL + ' FROM [dbo].[ModelTypeClass]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Populate the temp table
				EXEC sp_executesql @SQL

				-- Return paged results
				SELECT O.[Class], O.[ModelTypeAcronym], O.[IsDefault]
				FROM
				    [dbo].[ModelTypeClass] O,
				    #PageIndex PageIndex
				WHERE
				    PageIndex.IndexId > @PageLowerBound
					AND O.[Class] = PageIndex.[Class]
					AND O.[ModelTypeAcronym] = PageIndex.[ModelTypeAcronym]
				ORDER BY
				    PageIndex.IndexId
				
				-- get row count
				SET @SQL = 'SELECT COUNT(*) AS TotalRowCount'
				SET @SQL = @SQL + ' FROM [dbo].[ModelTypeClass]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				EXEC sp_executesql @SQL
			
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ModelTypeClass_Insert procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ModelTypeClass_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ModelTypeClass_Insert
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Inserts a record into the ModelTypeClass table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ModelTypeClass_Insert
(

	@Class varchar (50)  ,

	@ModelTypeAcronym varchar (50)  ,

	@IsDefault bit   
)
AS


				
				INSERT INTO [dbo].[ModelTypeClass]
					(
					[Class]
					,[ModelTypeAcronym]
					,[IsDefault]
					)
				VALUES
					(
					@Class
					,@ModelTypeAcronym
					,@IsDefault
					)
				
									
							
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ModelTypeClass_Update procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ModelTypeClass_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ModelTypeClass_Update
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Updates a record in the ModelTypeClass table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ModelTypeClass_Update
(

	@Class varchar (50)  ,

	@OriginalClass varchar (50)  ,

	@ModelTypeAcronym varchar (50)  ,

	@OriginalModelTypeAcronym varchar (50)  ,

	@IsDefault bit   
)
AS


				
				
				-- Modify the updatable columns
				UPDATE
					[dbo].[ModelTypeClass]
				SET
					[Class] = @Class
					,[ModelTypeAcronym] = @ModelTypeAcronym
					,[IsDefault] = @IsDefault
				WHERE
[Class] = @OriginalClass 
AND [ModelTypeAcronym] = @OriginalModelTypeAcronym 
				
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ModelTypeClass_Delete procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ModelTypeClass_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ModelTypeClass_Delete
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Deletes a record in the ModelTypeClass table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ModelTypeClass_Delete
(

	@Class varchar (50)  ,

	@ModelTypeAcronym varchar (50)  
)
AS


				DELETE FROM [dbo].[ModelTypeClass] WITH (ROWLOCK) 
				WHERE
					[Class] = @Class
					AND [ModelTypeAcronym] = @ModelTypeAcronym
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ModelTypeClass_GetByModelTypeAcronym procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ModelTypeClass_GetByModelTypeAcronym') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ModelTypeClass_GetByModelTypeAcronym
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the ModelTypeClass table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ModelTypeClass_GetByModelTypeAcronym
(

	@ModelTypeAcronym varchar (50)  
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[Class],
					[ModelTypeAcronym],
					[IsDefault]
				FROM
					[dbo].[ModelTypeClass]
				WHERE
					[ModelTypeAcronym] = @ModelTypeAcronym
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ModelTypeClass_GetByClass procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ModelTypeClass_GetByClass') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ModelTypeClass_GetByClass
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the ModelTypeClass table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ModelTypeClass_GetByClass
(

	@Class varchar (50)  
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[Class],
					[ModelTypeAcronym],
					[IsDefault]
				FROM
					[dbo].[ModelTypeClass]
				WHERE
					[Class] = @Class
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ModelTypeClass_GetByClassModelTypeAcronym procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ModelTypeClass_GetByClassModelTypeAcronym') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ModelTypeClass_GetByClassModelTypeAcronym
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the ModelTypeClass table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ModelTypeClass_GetByClassModelTypeAcronym
(

	@Class varchar (50)  ,

	@ModelTypeAcronym varchar (50)  
)
AS


				SELECT
					[Class],
					[ModelTypeAcronym],
					[IsDefault]
				FROM
					[dbo].[ModelTypeClass]
				WHERE
					[Class] = @Class
					AND [ModelTypeAcronym] = @ModelTypeAcronym
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ModelTypeClass_Find procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ModelTypeClass_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ModelTypeClass_Find
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Finds records in the ModelTypeClass table passing nullable parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ModelTypeClass_Find
(

	@SearchUsingOR bit   = null ,

	@Class varchar (50)  = null ,

	@ModelTypeAcronym varchar (50)  = null ,

	@IsDefault bit   = null 
)
AS


				
  IF ISNULL(@SearchUsingOR, 0) <> 1
  BEGIN
    SELECT
	  [Class]
	, [ModelTypeAcronym]
	, [IsDefault]
    FROM
	[dbo].[ModelTypeClass]
    WHERE 
	 ([Class] = @Class OR @Class IS NULL)
	AND ([ModelTypeAcronym] = @ModelTypeAcronym OR @ModelTypeAcronym IS NULL)
	AND ([IsDefault] = @IsDefault OR @IsDefault IS NULL)
						
  END
  ELSE
  BEGIN
    SELECT
	  [Class]
	, [ModelTypeAcronym]
	, [IsDefault]
    FROM
	[dbo].[ModelTypeClass]
    WHERE 
	 ([Class] = @Class AND @Class is not null)
	OR ([ModelTypeAcronym] = @ModelTypeAcronym AND @ModelTypeAcronym is not null)
	OR ([IsDefault] = @IsDefault AND @IsDefault is not null)
	SELECT @@ROWCOUNT			
  END
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_UserPermission_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_UserPermission_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_UserPermission_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the UserPermission table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_UserPermission_Get_List

AS


				
				SELECT
					[UserID],
					[PermissionID],
					[WorkspaceName],
					[WorkspaceTypeID]
				FROM
					[dbo].[UserPermission]
					
				SELECT @@ROWCOUNT
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_UserPermission_GetPaged procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_UserPermission_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_UserPermission_GetPaged
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the UserPermission table passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_UserPermission_GetPaged
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  ,

	@PageIndex int   ,

	@PageSize int   
)
AS


				
				BEGIN
				DECLARE @PageLowerBound int
				DECLARE @PageUpperBound int
				
				-- Set the page bounds
				SET @PageLowerBound = @PageSize * @PageIndex
				SET @PageUpperBound = @PageLowerBound + @PageSize

				-- Create a temp table to store the select results
				CREATE TABLE #PageIndex
				(
				    [IndexId] int IDENTITY (1, 1) NOT NULL,
				    [UserID] int, [PermissionID] int, [WorkspaceName] varchar(100) COLLATE database_default , [WorkspaceTypeID] int 
				)
				
				-- Insert into the temp table
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = 'INSERT INTO #PageIndex ([UserID], [PermissionID], [WorkspaceName], [WorkspaceTypeID])'
				SET @SQL = @SQL + ' SELECT'
				IF @PageSize > 0
				BEGIN
					SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound)
				END
				SET @SQL = @SQL + ' [UserID], [PermissionID], [WorkspaceName], [WorkspaceTypeID]'
				SET @SQL = @SQL + ' FROM [dbo].[UserPermission]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Populate the temp table
				EXEC sp_executesql @SQL

				-- Return paged results
				SELECT O.[UserID], O.[PermissionID], O.[WorkspaceName], O.[WorkspaceTypeID]
				FROM
				    [dbo].[UserPermission] O,
				    #PageIndex PageIndex
				WHERE
				    PageIndex.IndexId > @PageLowerBound
					AND O.[UserID] = PageIndex.[UserID]
					AND O.[PermissionID] = PageIndex.[PermissionID]
					AND O.[WorkspaceName] = PageIndex.[WorkspaceName]
					AND O.[WorkspaceTypeID] = PageIndex.[WorkspaceTypeID]
				ORDER BY
				    PageIndex.IndexId
				
				-- get row count
				SET @SQL = 'SELECT COUNT(*) AS TotalRowCount'
				SET @SQL = @SQL + ' FROM [dbo].[UserPermission]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				EXEC sp_executesql @SQL
			
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_UserPermission_Insert procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_UserPermission_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_UserPermission_Insert
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Inserts a record into the UserPermission table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_UserPermission_Insert
(

	@UserID int   ,

	@PermissionID int   ,

	@WorkspaceName varchar (100)  ,

	@WorkspaceTypeID int   
)
AS


				
				INSERT INTO [dbo].[UserPermission]
					(
					[UserID]
					,[PermissionID]
					,[WorkspaceName]
					,[WorkspaceTypeID]
					)
				VALUES
					(
					@UserID
					,@PermissionID
					,@WorkspaceName
					,@WorkspaceTypeID
					)
				
									
							
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_UserPermission_Update procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_UserPermission_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_UserPermission_Update
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Updates a record in the UserPermission table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_UserPermission_Update
(

	@UserID int   ,

	@OriginalUserID int   ,

	@PermissionID int   ,

	@OriginalPermissionID int   ,

	@WorkspaceName varchar (100)  ,

	@OriginalWorkspaceName varchar (100)  ,

	@WorkspaceTypeID int   ,

	@OriginalWorkspaceTypeID int   
)
AS


				
				
				-- Modify the updatable columns
				UPDATE
					[dbo].[UserPermission]
				SET
					[UserID] = @UserID
					,[PermissionID] = @PermissionID
					,[WorkspaceName] = @WorkspaceName
					,[WorkspaceTypeID] = @WorkspaceTypeID
				WHERE
[UserID] = @OriginalUserID 
AND [PermissionID] = @OriginalPermissionID 
AND [WorkspaceName] = @OriginalWorkspaceName 
AND [WorkspaceTypeID] = @OriginalWorkspaceTypeID 
				
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_UserPermission_Delete procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_UserPermission_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_UserPermission_Delete
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Deletes a record in the UserPermission table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_UserPermission_Delete
(

	@UserID int   ,

	@PermissionID int   ,

	@WorkspaceName varchar (100)  ,

	@WorkspaceTypeID int   
)
AS


				DELETE FROM [dbo].[UserPermission] WITH (ROWLOCK) 
				WHERE
					[UserID] = @UserID
					AND [PermissionID] = @PermissionID
					AND [WorkspaceName] = @WorkspaceName
					AND [WorkspaceTypeID] = @WorkspaceTypeID
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_UserPermission_GetByPermissionID procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_UserPermission_GetByPermissionID') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_UserPermission_GetByPermissionID
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the UserPermission table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_UserPermission_GetByPermissionID
(

	@PermissionID int   
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[UserID],
					[PermissionID],
					[WorkspaceName],
					[WorkspaceTypeID]
				FROM
					[dbo].[UserPermission]
				WHERE
					[PermissionID] = @PermissionID
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_UserPermission_GetByUserID procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_UserPermission_GetByUserID') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_UserPermission_GetByUserID
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the UserPermission table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_UserPermission_GetByUserID
(

	@UserID int   
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[UserID],
					[PermissionID],
					[WorkspaceName],
					[WorkspaceTypeID]
				FROM
					[dbo].[UserPermission]
				WHERE
					[UserID] = @UserID
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_UserPermission_GetByWorkspaceNameWorkspaceTypeID procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_UserPermission_GetByWorkspaceNameWorkspaceTypeID') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_UserPermission_GetByWorkspaceNameWorkspaceTypeID
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the UserPermission table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_UserPermission_GetByWorkspaceNameWorkspaceTypeID
(

	@WorkspaceName varchar (100)  ,

	@WorkspaceTypeID int   
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[UserID],
					[PermissionID],
					[WorkspaceName],
					[WorkspaceTypeID]
				FROM
					[dbo].[UserPermission]
				WHERE
					[WorkspaceName] = @WorkspaceName
					AND [WorkspaceTypeID] = @WorkspaceTypeID
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_UserPermission_GetByUserIDPermissionIDWorkspaceNameWorkspaceTypeID procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_UserPermission_GetByUserIDPermissionIDWorkspaceNameWorkspaceTypeID') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_UserPermission_GetByUserIDPermissionIDWorkspaceNameWorkspaceTypeID
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the UserPermission table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_UserPermission_GetByUserIDPermissionIDWorkspaceNameWorkspaceTypeID
(

	@UserID int   ,

	@PermissionID int   ,

	@WorkspaceName varchar (100)  ,

	@WorkspaceTypeID int   
)
AS


				SELECT
					[UserID],
					[PermissionID],
					[WorkspaceName],
					[WorkspaceTypeID]
				FROM
					[dbo].[UserPermission]
				WHERE
					[UserID] = @UserID
					AND [PermissionID] = @PermissionID
					AND [WorkspaceName] = @WorkspaceName
					AND [WorkspaceTypeID] = @WorkspaceTypeID
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_UserPermission_Find procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_UserPermission_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_UserPermission_Find
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Finds records in the UserPermission table passing nullable parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_UserPermission_Find
(

	@SearchUsingOR bit   = null ,

	@UserID int   = null ,

	@PermissionID int   = null ,

	@WorkspaceName varchar (100)  = null ,

	@WorkspaceTypeID int   = null 
)
AS


				
  IF ISNULL(@SearchUsingOR, 0) <> 1
  BEGIN
    SELECT
	  [UserID]
	, [PermissionID]
	, [WorkspaceName]
	, [WorkspaceTypeID]
    FROM
	[dbo].[UserPermission]
    WHERE 
	 ([UserID] = @UserID OR @UserID IS NULL)
	AND ([PermissionID] = @PermissionID OR @PermissionID IS NULL)
	AND ([WorkspaceName] = @WorkspaceName OR @WorkspaceName IS NULL)
	AND ([WorkspaceTypeID] = @WorkspaceTypeID OR @WorkspaceTypeID IS NULL)
						
  END
  ELSE
  BEGIN
    SELECT
	  [UserID]
	, [PermissionID]
	, [WorkspaceName]
	, [WorkspaceTypeID]
    FROM
	[dbo].[UserPermission]
    WHERE 
	 ([UserID] = @UserID AND @UserID is not null)
	OR ([PermissionID] = @PermissionID AND @PermissionID is not null)
	OR ([WorkspaceName] = @WorkspaceName AND @WorkspaceName is not null)
	OR ([WorkspaceTypeID] = @WorkspaceTypeID AND @WorkspaceTypeID is not null)
	SELECT @@ROWCOUNT			
  END
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_AllowedArtifact_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_AllowedArtifact_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_AllowedArtifact_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the AllowedArtifact table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_AllowedArtifact_Get_List

AS


				
				SELECT
					[CAid],
					[Class],
					[IsActive]
				FROM
					[dbo].[AllowedArtifact]
					
				SELECT @@ROWCOUNT
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_AllowedArtifact_GetPaged procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_AllowedArtifact_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_AllowedArtifact_GetPaged
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the AllowedArtifact table passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_AllowedArtifact_GetPaged
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  ,

	@PageIndex int   ,

	@PageSize int   
)
AS


				
				BEGIN
				DECLARE @PageLowerBound int
				DECLARE @PageUpperBound int
				
				-- Set the page bounds
				SET @PageLowerBound = @PageSize * @PageIndex
				SET @PageUpperBound = @PageLowerBound + @PageSize

				-- Create a temp table to store the select results
				CREATE TABLE #PageIndex
				(
				    [IndexId] int IDENTITY (1, 1) NOT NULL,
				    [CAid] int, [Class] varchar(50) COLLATE database_default  
				)
				
				-- Insert into the temp table
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = 'INSERT INTO #PageIndex ([CAid], [Class])'
				SET @SQL = @SQL + ' SELECT'
				IF @PageSize > 0
				BEGIN
					SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound)
				END
				SET @SQL = @SQL + ' [CAid], [Class]'
				SET @SQL = @SQL + ' FROM [dbo].[AllowedArtifact]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Populate the temp table
				EXEC sp_executesql @SQL

				-- Return paged results
				SELECT O.[CAid], O.[Class], O.[IsActive]
				FROM
				    [dbo].[AllowedArtifact] O,
				    #PageIndex PageIndex
				WHERE
				    PageIndex.IndexId > @PageLowerBound
					AND O.[CAid] = PageIndex.[CAid]
					AND O.[Class] = PageIndex.[Class]
				ORDER BY
				    PageIndex.IndexId
				
				-- get row count
				SET @SQL = 'SELECT COUNT(*) AS TotalRowCount'
				SET @SQL = @SQL + ' FROM [dbo].[AllowedArtifact]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				EXEC sp_executesql @SQL
			
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_AllowedArtifact_Insert procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_AllowedArtifact_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_AllowedArtifact_Insert
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Inserts a record into the AllowedArtifact table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_AllowedArtifact_Insert
(

	@CAid int   ,

	@Class varchar (50)  ,

	@IsActive bit   
)
AS


				
				INSERT INTO [dbo].[AllowedArtifact]
					(
					[CAid]
					,[Class]
					,[IsActive]
					)
				VALUES
					(
					@CAid
					,@Class
					,@IsActive
					)
				
									
							
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_AllowedArtifact_Update procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_AllowedArtifact_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_AllowedArtifact_Update
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Updates a record in the AllowedArtifact table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_AllowedArtifact_Update
(

	@CAid int   ,

	@OriginalCAid int   ,

	@Class varchar (50)  ,

	@OriginalClass varchar (50)  ,

	@IsActive bit   
)
AS


				
				
				-- Modify the updatable columns
				UPDATE
					[dbo].[AllowedArtifact]
				SET
					[CAid] = @CAid
					,[Class] = @Class
					,[IsActive] = @IsActive
				WHERE
[CAid] = @OriginalCAid 
AND [Class] = @OriginalClass 
				
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_AllowedArtifact_Delete procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_AllowedArtifact_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_AllowedArtifact_Delete
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Deletes a record in the AllowedArtifact table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_AllowedArtifact_Delete
(

	@CAid int   ,

	@Class varchar (50)  
)
AS


				DELETE FROM [dbo].[AllowedArtifact] WITH (ROWLOCK) 
				WHERE
					[CAid] = @CAid
					AND [Class] = @Class
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_AllowedArtifact_GetByClass procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_AllowedArtifact_GetByClass') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_AllowedArtifact_GetByClass
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the AllowedArtifact table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_AllowedArtifact_GetByClass
(

	@Class varchar (50)  
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[CAid],
					[Class],
					[IsActive]
				FROM
					[dbo].[AllowedArtifact]
				WHERE
					[Class] = @Class
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_AllowedArtifact_GetByCAid procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_AllowedArtifact_GetByCAid') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_AllowedArtifact_GetByCAid
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the AllowedArtifact table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_AllowedArtifact_GetByCAid
(

	@CAid int   
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[CAid],
					[Class],
					[IsActive]
				FROM
					[dbo].[AllowedArtifact]
				WHERE
					[CAid] = @CAid
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_AllowedArtifact_GetByCAidClass procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_AllowedArtifact_GetByCAidClass') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_AllowedArtifact_GetByCAidClass
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the AllowedArtifact table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_AllowedArtifact_GetByCAidClass
(

	@CAid int   ,

	@Class varchar (50)  
)
AS


				SELECT
					[CAid],
					[Class],
					[IsActive]
				FROM
					[dbo].[AllowedArtifact]
				WHERE
					[CAid] = @CAid
					AND [Class] = @Class
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_AllowedArtifact_Find procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_AllowedArtifact_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_AllowedArtifact_Find
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Finds records in the AllowedArtifact table passing nullable parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_AllowedArtifact_Find
(

	@SearchUsingOR bit   = null ,

	@CAid int   = null ,

	@Class varchar (50)  = null ,

	@IsActive bit   = null 
)
AS


				
  IF ISNULL(@SearchUsingOR, 0) <> 1
  BEGIN
    SELECT
	  [CAid]
	, [Class]
	, [IsActive]
    FROM
	[dbo].[AllowedArtifact]
    WHERE 
	 ([CAid] = @CAid OR @CAid IS NULL)
	AND ([Class] = @Class OR @Class IS NULL)
	AND ([IsActive] = @IsActive OR @IsActive IS NULL)
						
  END
  ELSE
  BEGIN
    SELECT
	  [CAid]
	, [Class]
	, [IsActive]
    FROM
	[dbo].[AllowedArtifact]
    WHERE 
	 ([CAid] = @CAid AND @CAid is not null)
	OR ([Class] = @Class AND @Class is not null)
	OR ([IsActive] = @IsActive AND @IsActive is not null)
	SELECT @@ROWCOUNT			
  END
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ModelTypeAssociationDefault_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ModelTypeAssociationDefault_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ModelTypeAssociationDefault_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the ModelTypeAssociationDefault table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ModelTypeAssociationDefault_Get_List

AS


				
				SELECT
					[CAid],
					[ModelTypeAcronym],
					[IsDefault]
				FROM
					[dbo].[ModelTypeAssociationDefault]
					
				SELECT @@ROWCOUNT
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ModelTypeAssociationDefault_GetPaged procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ModelTypeAssociationDefault_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ModelTypeAssociationDefault_GetPaged
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the ModelTypeAssociationDefault table passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ModelTypeAssociationDefault_GetPaged
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  ,

	@PageIndex int   ,

	@PageSize int   
)
AS


				
				BEGIN
				DECLARE @PageLowerBound int
				DECLARE @PageUpperBound int
				
				-- Set the page bounds
				SET @PageLowerBound = @PageSize * @PageIndex
				SET @PageUpperBound = @PageLowerBound + @PageSize

				-- Create a temp table to store the select results
				CREATE TABLE #PageIndex
				(
				    [IndexId] int IDENTITY (1, 1) NOT NULL,
				    [CAid] int, [ModelTypeAcronym] varchar(50) COLLATE database_default  
				)
				
				-- Insert into the temp table
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = 'INSERT INTO #PageIndex ([CAid], [ModelTypeAcronym])'
				SET @SQL = @SQL + ' SELECT'
				IF @PageSize > 0
				BEGIN
					SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound)
				END
				SET @SQL = @SQL + ' [CAid], [ModelTypeAcronym]'
				SET @SQL = @SQL + ' FROM [dbo].[ModelTypeAssociationDefault]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Populate the temp table
				EXEC sp_executesql @SQL

				-- Return paged results
				SELECT O.[CAid], O.[ModelTypeAcronym], O.[IsDefault]
				FROM
				    [dbo].[ModelTypeAssociationDefault] O,
				    #PageIndex PageIndex
				WHERE
				    PageIndex.IndexId > @PageLowerBound
					AND O.[CAid] = PageIndex.[CAid]
					AND O.[ModelTypeAcronym] = PageIndex.[ModelTypeAcronym]
				ORDER BY
				    PageIndex.IndexId
				
				-- get row count
				SET @SQL = 'SELECT COUNT(*) AS TotalRowCount'
				SET @SQL = @SQL + ' FROM [dbo].[ModelTypeAssociationDefault]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				EXEC sp_executesql @SQL
			
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ModelTypeAssociationDefault_Insert procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ModelTypeAssociationDefault_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ModelTypeAssociationDefault_Insert
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Inserts a record into the ModelTypeAssociationDefault table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ModelTypeAssociationDefault_Insert
(

	@CAid int   ,

	@ModelTypeAcronym varchar (50)  ,

	@IsDefault bit   
)
AS


				
				INSERT INTO [dbo].[ModelTypeAssociationDefault]
					(
					[CAid]
					,[ModelTypeAcronym]
					,[IsDefault]
					)
				VALUES
					(
					@CAid
					,@ModelTypeAcronym
					,@IsDefault
					)
				
									
							
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ModelTypeAssociationDefault_Update procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ModelTypeAssociationDefault_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ModelTypeAssociationDefault_Update
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Updates a record in the ModelTypeAssociationDefault table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ModelTypeAssociationDefault_Update
(

	@CAid int   ,

	@OriginalCAid int   ,

	@ModelTypeAcronym varchar (50)  ,

	@OriginalModelTypeAcronym varchar (50)  ,

	@IsDefault bit   
)
AS


				
				
				-- Modify the updatable columns
				UPDATE
					[dbo].[ModelTypeAssociationDefault]
				SET
					[CAid] = @CAid
					,[ModelTypeAcronym] = @ModelTypeAcronym
					,[IsDefault] = @IsDefault
				WHERE
[CAid] = @OriginalCAid 
AND [ModelTypeAcronym] = @OriginalModelTypeAcronym 
				
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ModelTypeAssociationDefault_Delete procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ModelTypeAssociationDefault_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ModelTypeAssociationDefault_Delete
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Deletes a record in the ModelTypeAssociationDefault table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ModelTypeAssociationDefault_Delete
(

	@CAid int   ,

	@ModelTypeAcronym varchar (50)  
)
AS


				DELETE FROM [dbo].[ModelTypeAssociationDefault] WITH (ROWLOCK) 
				WHERE
					[CAid] = @CAid
					AND [ModelTypeAcronym] = @ModelTypeAcronym
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ModelTypeAssociationDefault_GetByModelTypeAcronym procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ModelTypeAssociationDefault_GetByModelTypeAcronym') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ModelTypeAssociationDefault_GetByModelTypeAcronym
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the ModelTypeAssociationDefault table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ModelTypeAssociationDefault_GetByModelTypeAcronym
(

	@ModelTypeAcronym varchar (50)  
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[CAid],
					[ModelTypeAcronym],
					[IsDefault]
				FROM
					[dbo].[ModelTypeAssociationDefault]
				WHERE
					[ModelTypeAcronym] = @ModelTypeAcronym
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ModelTypeAssociationDefault_GetByCAid procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ModelTypeAssociationDefault_GetByCAid') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ModelTypeAssociationDefault_GetByCAid
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the ModelTypeAssociationDefault table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ModelTypeAssociationDefault_GetByCAid
(

	@CAid int   
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[CAid],
					[ModelTypeAcronym],
					[IsDefault]
				FROM
					[dbo].[ModelTypeAssociationDefault]
				WHERE
					[CAid] = @CAid
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ModelTypeAssociationDefault_GetByCAidModelTypeAcronym procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ModelTypeAssociationDefault_GetByCAidModelTypeAcronym') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ModelTypeAssociationDefault_GetByCAidModelTypeAcronym
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the ModelTypeAssociationDefault table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ModelTypeAssociationDefault_GetByCAidModelTypeAcronym
(

	@CAid int   ,

	@ModelTypeAcronym varchar (50)  
)
AS


				SELECT
					[CAid],
					[ModelTypeAcronym],
					[IsDefault]
				FROM
					[dbo].[ModelTypeAssociationDefault]
				WHERE
					[CAid] = @CAid
					AND [ModelTypeAcronym] = @ModelTypeAcronym
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ModelTypeAssociationDefault_Find procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ModelTypeAssociationDefault_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ModelTypeAssociationDefault_Find
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Finds records in the ModelTypeAssociationDefault table passing nullable parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ModelTypeAssociationDefault_Find
(

	@SearchUsingOR bit   = null ,

	@CAid int   = null ,

	@ModelTypeAcronym varchar (50)  = null ,

	@IsDefault bit   = null 
)
AS


				
  IF ISNULL(@SearchUsingOR, 0) <> 1
  BEGIN
    SELECT
	  [CAid]
	, [ModelTypeAcronym]
	, [IsDefault]
    FROM
	[dbo].[ModelTypeAssociationDefault]
    WHERE 
	 ([CAid] = @CAid OR @CAid IS NULL)
	AND ([ModelTypeAcronym] = @ModelTypeAcronym OR @ModelTypeAcronym IS NULL)
	AND ([IsDefault] = @IsDefault OR @IsDefault IS NULL)
						
  END
  ELSE
  BEGIN
    SELECT
	  [CAid]
	, [ModelTypeAcronym]
	, [IsDefault]
    FROM
	[dbo].[ModelTypeAssociationDefault]
    WHERE 
	 ([CAid] = @CAid AND @CAid is not null)
	OR ([ModelTypeAcronym] = @ModelTypeAcronym AND @ModelTypeAcronym is not null)
	OR ([IsDefault] = @IsDefault AND @IsDefault is not null)
	SELECT @@ROWCOUNT			
  END
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ModelType_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ModelType_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ModelType_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the ModelType table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ModelType_Get_List

AS


				
				SELECT
					[Acronym],
					[Name],
					[Description],
					[Assignments],
					[Domain]
				FROM
					[dbo].[ModelType]
					
				SELECT @@ROWCOUNT
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ModelType_GetPaged procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ModelType_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ModelType_GetPaged
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the ModelType table passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ModelType_GetPaged
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  ,

	@PageIndex int   ,

	@PageSize int   
)
AS


				
				BEGIN
				DECLARE @PageLowerBound int
				DECLARE @PageUpperBound int
				
				-- Set the page bounds
				SET @PageLowerBound = @PageSize * @PageIndex
				SET @PageUpperBound = @PageLowerBound + @PageSize

				-- Create a temp table to store the select results
				CREATE TABLE #PageIndex
				(
				    [IndexId] int IDENTITY (1, 1) NOT NULL,
				    [Acronym] varchar(50) COLLATE database_default  
				)
				
				-- Insert into the temp table
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = 'INSERT INTO #PageIndex ([Acronym])'
				SET @SQL = @SQL + ' SELECT'
				IF @PageSize > 0
				BEGIN
					SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound)
				END
				SET @SQL = @SQL + ' [Acronym]'
				SET @SQL = @SQL + ' FROM [dbo].[ModelType]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Populate the temp table
				EXEC sp_executesql @SQL

				-- Return paged results
				SELECT O.[Acronym], O.[Name], O.[Description], O.[Assignments], O.[Domain]
				FROM
				    [dbo].[ModelType] O,
				    #PageIndex PageIndex
				WHERE
				    PageIndex.IndexId > @PageLowerBound
					AND O.[Acronym] = PageIndex.[Acronym]
				ORDER BY
				    PageIndex.IndexId
				
				-- get row count
				SET @SQL = 'SELECT COUNT(*) AS TotalRowCount'
				SET @SQL = @SQL + ' FROM [dbo].[ModelType]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				EXEC sp_executesql @SQL
			
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ModelType_Insert procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ModelType_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ModelType_Insert
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Inserts a record into the ModelType table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ModelType_Insert
(

	@Acronym varchar (50)  ,

	@Name varchar (250)  ,

	@Description nvarchar (1500)  ,

	@Assignments varchar (500)  ,

	@Domain varchar (50)  
)
AS


				
				INSERT INTO [dbo].[ModelType]
					(
					[Acronym]
					,[Name]
					,[Description]
					,[Assignments]
					,[Domain]
					)
				VALUES
					(
					@Acronym
					,@Name
					,@Description
					,@Assignments
					,@Domain
					)
				
									
							
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ModelType_Update procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ModelType_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ModelType_Update
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Updates a record in the ModelType table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ModelType_Update
(

	@Acronym varchar (50)  ,

	@OriginalAcronym varchar (50)  ,

	@Name varchar (250)  ,

	@Description nvarchar (1500)  ,

	@Assignments varchar (500)  ,

	@Domain varchar (50)  
)
AS


				
				
				-- Modify the updatable columns
				UPDATE
					[dbo].[ModelType]
				SET
					[Acronym] = @Acronym
					,[Name] = @Name
					,[Description] = @Description
					,[Assignments] = @Assignments
					,[Domain] = @Domain
				WHERE
[Acronym] = @OriginalAcronym 
				
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ModelType_Delete procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ModelType_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ModelType_Delete
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Deletes a record in the ModelType table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ModelType_Delete
(

	@Acronym varchar (50)  
)
AS


				DELETE FROM [dbo].[ModelType] WITH (ROWLOCK) 
				WHERE
					[Acronym] = @Acronym
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ModelType_GetByAcronym procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ModelType_GetByAcronym') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ModelType_GetByAcronym
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the ModelType table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ModelType_GetByAcronym
(

	@Acronym varchar (50)  
)
AS


				SELECT
					[Acronym],
					[Name],
					[Description],
					[Assignments],
					[Domain]
				FROM
					[dbo].[ModelType]
				WHERE
					[Acronym] = @Acronym
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ModelType_GetByCAidFromModelTypeAssociationDefault procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ModelType_GetByCAidFromModelTypeAssociationDefault') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ModelType_GetByCAidFromModelTypeAssociationDefault
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records through a junction table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ModelType_GetByCAidFromModelTypeAssociationDefault
(

	@CAid int   
)
AS


SELECT dbo.[ModelType].[Acronym]
       ,dbo.[ModelType].[Name]
       ,dbo.[ModelType].[Description]
       ,dbo.[ModelType].[Assignments]
       ,dbo.[ModelType].[Domain]
  FROM dbo.[ModelType]
 WHERE EXISTS (SELECT 1
                 FROM dbo.[ModelTypeAssociationDefault] 
                WHERE dbo.[ModelTypeAssociationDefault].[CAid] = @CAid
                  AND dbo.[ModelTypeAssociationDefault].[ModelTypeAcronym] = dbo.[ModelType].[Acronym]
                  )
				SELECT @@ROWCOUNT			
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ModelType_GetByClassFromModelTypeClass procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ModelType_GetByClassFromModelTypeClass') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ModelType_GetByClassFromModelTypeClass
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records through a junction table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ModelType_GetByClassFromModelTypeClass
(

	@Class varchar (50)  
)
AS


SELECT dbo.[ModelType].[Acronym]
       ,dbo.[ModelType].[Name]
       ,dbo.[ModelType].[Description]
       ,dbo.[ModelType].[Assignments]
       ,dbo.[ModelType].[Domain]
  FROM dbo.[ModelType]
 WHERE EXISTS (SELECT 1
                 FROM dbo.[ModelTypeClass] 
                WHERE dbo.[ModelTypeClass].[Class] = @Class
                  AND dbo.[ModelTypeClass].[ModelTypeAcronym] = dbo.[ModelType].[Acronym]
                  )
				SELECT @@ROWCOUNT			
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ModelType_Find procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ModelType_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ModelType_Find
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Finds records in the ModelType table passing nullable parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ModelType_Find
(

	@SearchUsingOR bit   = null ,

	@Acronym varchar (50)  = null ,

	@Name varchar (250)  = null ,

	@Description nvarchar (1500)  = null ,

	@Assignments varchar (500)  = null ,

	@Domain varchar (50)  = null 
)
AS


				
  IF ISNULL(@SearchUsingOR, 0) <> 1
  BEGIN
    SELECT
	  [Acronym]
	, [Name]
	, [Description]
	, [Assignments]
	, [Domain]
    FROM
	[dbo].[ModelType]
    WHERE 
	 ([Acronym] = @Acronym OR @Acronym IS NULL)
	AND ([Name] = @Name OR @Name IS NULL)
	AND ([Description] = @Description OR @Description IS NULL)
	AND ([Assignments] = @Assignments OR @Assignments IS NULL)
	AND ([Domain] = @Domain OR @Domain IS NULL)
						
  END
  ELSE
  BEGIN
    SELECT
	  [Acronym]
	, [Name]
	, [Description]
	, [Assignments]
	, [Domain]
    FROM
	[dbo].[ModelType]
    WHERE 
	 ([Acronym] = @Acronym AND @Acronym is not null)
	OR ([Name] = @Name AND @Name is not null)
	OR ([Description] = @Description AND @Description is not null)
	OR ([Assignments] = @Assignments AND @Assignments is not null)
	OR ([Domain] = @Domain AND @Domain is not null)
	SELECT @@ROWCOUNT			
  END
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_AssociationType_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_AssociationType_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_AssociationType_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the AssociationType table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_AssociationType_Get_List

AS


				
				SELECT
					[pkid],
					[Name],
					[IsTwoWay],
					[LinkSpecification]
				FROM
					[dbo].[AssociationType]
					
				SELECT @@ROWCOUNT
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_AssociationType_GetPaged procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_AssociationType_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_AssociationType_GetPaged
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the AssociationType table passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_AssociationType_GetPaged
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  ,

	@PageIndex int   ,

	@PageSize int   
)
AS


				
				BEGIN
				DECLARE @PageLowerBound int
				DECLARE @PageUpperBound int
				
				-- Set the page bounds
				SET @PageLowerBound = @PageSize * @PageIndex
				SET @PageUpperBound = @PageLowerBound + @PageSize

				-- Create a temp table to store the select results
				CREATE TABLE #PageIndex
				(
				    [IndexId] int IDENTITY (1, 1) NOT NULL,
				    [pkid] int 
				)
				
				-- Insert into the temp table
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = 'INSERT INTO #PageIndex ([pkid])'
				SET @SQL = @SQL + ' SELECT'
				IF @PageSize > 0
				BEGIN
					SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound)
				END
				SET @SQL = @SQL + ' [pkid]'
				SET @SQL = @SQL + ' FROM [dbo].[AssociationType]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Populate the temp table
				EXEC sp_executesql @SQL

				-- Return paged results
				SELECT O.[pkid], O.[Name], O.[IsTwoWay], O.[LinkSpecification]
				FROM
				    [dbo].[AssociationType] O,
				    #PageIndex PageIndex
				WHERE
				    PageIndex.IndexId > @PageLowerBound
					AND O.[pkid] = PageIndex.[pkid]
				ORDER BY
				    PageIndex.IndexId
				
				-- get row count
				SET @SQL = 'SELECT COUNT(*) AS TotalRowCount'
				SET @SQL = @SQL + ' FROM [dbo].[AssociationType]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				EXEC sp_executesql @SQL
			
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_AssociationType_Insert procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_AssociationType_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_AssociationType_Insert
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Inserts a record into the AssociationType table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_AssociationType_Insert
(

	@pkid int    OUTPUT,

	@Name char (25)  ,

	@IsTwoWay bit   ,

	@LinkSpecification nvarchar (4000)  
)
AS


				
				INSERT INTO [dbo].[AssociationType]
					(
					[Name]
					,[IsTwoWay]
					,[LinkSpecification]
					)
				VALUES
					(
					@Name
					,@IsTwoWay
					,@LinkSpecification
					)
				
				-- Get the identity value
				SET @pkid = SCOPE_IDENTITY()
									
							
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_AssociationType_Update procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_AssociationType_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_AssociationType_Update
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Updates a record in the AssociationType table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_AssociationType_Update
(

	@pkid int   ,

	@Name char (25)  ,

	@IsTwoWay bit   ,

	@LinkSpecification nvarchar (4000)  
)
AS


				
				
				-- Modify the updatable columns
				UPDATE
					[dbo].[AssociationType]
				SET
					[Name] = @Name
					,[IsTwoWay] = @IsTwoWay
					,[LinkSpecification] = @LinkSpecification
				WHERE
[pkid] = @pkid 
				
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_AssociationType_Delete procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_AssociationType_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_AssociationType_Delete
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Deletes a record in the AssociationType table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_AssociationType_Delete
(

	@pkid int   
)
AS


				DELETE FROM [dbo].[AssociationType] WITH (ROWLOCK) 
				WHERE
					[pkid] = @pkid
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_AssociationType_GetBypkid procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_AssociationType_GetBypkid') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_AssociationType_GetBypkid
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the AssociationType table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_AssociationType_GetBypkid
(

	@pkid int   
)
AS


				SELECT
					[pkid],
					[Name],
					[IsTwoWay],
					[LinkSpecification]
				FROM
					[dbo].[AssociationType]
				WHERE
					[pkid] = @pkid
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_AssociationType_GetByName procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_AssociationType_GetByName') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_AssociationType_GetByName
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the AssociationType table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_AssociationType_GetByName
(

	@Name char (25)  
)
AS


				SELECT
					[pkid],
					[Name],
					[IsTwoWay],
					[LinkSpecification]
				FROM
					[dbo].[AssociationType]
				WHERE
					[Name] = @Name
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_AssociationType_Find procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_AssociationType_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_AssociationType_Find
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Finds records in the AssociationType table passing nullable parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_AssociationType_Find
(

	@SearchUsingOR bit   = null ,

	@pkid int   = null ,

	@Name char (25)  = null ,

	@IsTwoWay bit   = null ,

	@LinkSpecification nvarchar (4000)  = null 
)
AS


				
  IF ISNULL(@SearchUsingOR, 0) <> 1
  BEGIN
    SELECT
	  [pkid]
	, [Name]
	, [IsTwoWay]
	, [LinkSpecification]
    FROM
	[dbo].[AssociationType]
    WHERE 
	 ([pkid] = @pkid OR @pkid IS NULL)
	AND ([Name] = @Name OR @Name IS NULL)
	AND ([IsTwoWay] = @IsTwoWay OR @IsTwoWay IS NULL)
	AND ([LinkSpecification] = @LinkSpecification OR @LinkSpecification IS NULL)
						
  END
  ELSE
  BEGIN
    SELECT
	  [pkid]
	, [Name]
	, [IsTwoWay]
	, [LinkSpecification]
    FROM
	[dbo].[AssociationType]
    WHERE 
	 ([pkid] = @pkid AND @pkid is not null)
	OR ([Name] = @Name AND @Name is not null)
	OR ([IsTwoWay] = @IsTwoWay AND @IsTwoWay is not null)
	OR ([LinkSpecification] = @LinkSpecification AND @LinkSpecification is not null)
	SELECT @@ROWCOUNT			
  END
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Config_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Config_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Config_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the Config table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Config_Get_List

AS


				
				SELECT
					[ConfigName],
					[ConfigValue]
				FROM
					[dbo].[Config]
					
				SELECT @@ROWCOUNT
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Config_GetPaged procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Config_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Config_GetPaged
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the Config table passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Config_GetPaged
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  ,

	@PageIndex int   ,

	@PageSize int   
)
AS


				
				BEGIN
				DECLARE @PageLowerBound int
				DECLARE @PageUpperBound int
				
				-- Set the page bounds
				SET @PageLowerBound = @PageSize * @PageIndex
				SET @PageUpperBound = @PageLowerBound + @PageSize

				-- Create a temp table to store the select results
				CREATE TABLE #PageIndex
				(
				    [IndexId] int IDENTITY (1, 1) NOT NULL,
				    [ConfigName] varchar(50) COLLATE database_default  
				)
				
				-- Insert into the temp table
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = 'INSERT INTO #PageIndex ([ConfigName])'
				SET @SQL = @SQL + ' SELECT'
				IF @PageSize > 0
				BEGIN
					SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound)
				END
				SET @SQL = @SQL + ' [ConfigName]'
				SET @SQL = @SQL + ' FROM [dbo].[Config]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Populate the temp table
				EXEC sp_executesql @SQL

				-- Return paged results
				SELECT O.[ConfigName], O.[ConfigValue]
				FROM
				    [dbo].[Config] O,
				    #PageIndex PageIndex
				WHERE
				    PageIndex.IndexId > @PageLowerBound
					AND O.[ConfigName] = PageIndex.[ConfigName]
				ORDER BY
				    PageIndex.IndexId
				
				-- get row count
				SET @SQL = 'SELECT COUNT(*) AS TotalRowCount'
				SET @SQL = @SQL + ' FROM [dbo].[Config]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				EXEC sp_executesql @SQL
			
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Config_Insert procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Config_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Config_Insert
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Inserts a record into the Config table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Config_Insert
(

	@ConfigName varchar (50)  ,

	@ConfigValue varchar (50)  
)
AS


				
				INSERT INTO [dbo].[Config]
					(
					[ConfigName]
					,[ConfigValue]
					)
				VALUES
					(
					@ConfigName
					,@ConfigValue
					)
				
									
							
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Config_Update procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Config_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Config_Update
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Updates a record in the Config table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Config_Update
(

	@ConfigName varchar (50)  ,

	@OriginalConfigName varchar (50)  ,

	@ConfigValue varchar (50)  
)
AS


				
				
				-- Modify the updatable columns
				UPDATE
					[dbo].[Config]
				SET
					[ConfigName] = @ConfigName
					,[ConfigValue] = @ConfigValue
				WHERE
[ConfigName] = @OriginalConfigName 
				
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Config_Delete procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Config_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Config_Delete
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Deletes a record in the Config table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Config_Delete
(

	@ConfigName varchar (50)  
)
AS


				DELETE FROM [dbo].[Config] WITH (ROWLOCK) 
				WHERE
					[ConfigName] = @ConfigName
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Config_GetByConfigName procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Config_GetByConfigName') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Config_GetByConfigName
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the Config table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Config_GetByConfigName
(

	@ConfigName varchar (50)  
)
AS


				SELECT
					[ConfigName],
					[ConfigValue]
				FROM
					[dbo].[Config]
				WHERE
					[ConfigName] = @ConfigName
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Config_Find procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Config_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Config_Find
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Finds records in the Config table passing nullable parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Config_Find
(

	@SearchUsingOR bit   = null ,

	@ConfigName varchar (50)  = null ,

	@ConfigValue varchar (50)  = null 
)
AS


				
  IF ISNULL(@SearchUsingOR, 0) <> 1
  BEGIN
    SELECT
	  [ConfigName]
	, [ConfigValue]
    FROM
	[dbo].[Config]
    WHERE 
	 ([ConfigName] = @ConfigName OR @ConfigName IS NULL)
	AND ([ConfigValue] = @ConfigValue OR @ConfigValue IS NULL)
						
  END
  ELSE
  BEGIN
    SELECT
	  [ConfigName]
	, [ConfigValue]
    FROM
	[dbo].[Config]
    WHERE 
	 ([ConfigName] = @ConfigName AND @ConfigName is not null)
	OR ([ConfigValue] = @ConfigValue AND @ConfigValue is not null)
	SELECT @@ROWCOUNT			
  END
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_DomainDefinition_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_DomainDefinition_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_DomainDefinition_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the DomainDefinition table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_DomainDefinition_Get_List

AS


				
				SELECT
					[pkid],
					[Name],
					[IsActive]
				FROM
					[dbo].[DomainDefinition]
					
				SELECT @@ROWCOUNT
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_DomainDefinition_GetPaged procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_DomainDefinition_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_DomainDefinition_GetPaged
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the DomainDefinition table passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_DomainDefinition_GetPaged
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  ,

	@PageIndex int   ,

	@PageSize int   
)
AS


				
				BEGIN
				DECLARE @PageLowerBound int
				DECLARE @PageUpperBound int
				
				-- Set the page bounds
				SET @PageLowerBound = @PageSize * @PageIndex
				SET @PageUpperBound = @PageLowerBound + @PageSize

				-- Create a temp table to store the select results
				CREATE TABLE #PageIndex
				(
				    [IndexId] int IDENTITY (1, 1) NOT NULL,
				    [pkid] int 
				)
				
				-- Insert into the temp table
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = 'INSERT INTO #PageIndex ([pkid])'
				SET @SQL = @SQL + ' SELECT'
				IF @PageSize > 0
				BEGIN
					SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound)
				END
				SET @SQL = @SQL + ' [pkid]'
				SET @SQL = @SQL + ' FROM [dbo].[DomainDefinition]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Populate the temp table
				EXEC sp_executesql @SQL

				-- Return paged results
				SELECT O.[pkid], O.[Name], O.[IsActive]
				FROM
				    [dbo].[DomainDefinition] O,
				    #PageIndex PageIndex
				WHERE
				    PageIndex.IndexId > @PageLowerBound
					AND O.[pkid] = PageIndex.[pkid]
				ORDER BY
				    PageIndex.IndexId
				
				-- get row count
				SET @SQL = 'SELECT COUNT(*) AS TotalRowCount'
				SET @SQL = @SQL + ' FROM [dbo].[DomainDefinition]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				EXEC sp_executesql @SQL
			
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_DomainDefinition_Insert procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_DomainDefinition_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_DomainDefinition_Insert
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Inserts a record into the DomainDefinition table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_DomainDefinition_Insert
(

	@pkid int    OUTPUT,

	@Name varchar (50)  ,

	@IsActive bit   
)
AS


				
				INSERT INTO [dbo].[DomainDefinition]
					(
					[Name]
					,[IsActive]
					)
				VALUES
					(
					@Name
					,@IsActive
					)
				
				-- Get the identity value
				SET @pkid = SCOPE_IDENTITY()
									
							
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_DomainDefinition_Update procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_DomainDefinition_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_DomainDefinition_Update
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Updates a record in the DomainDefinition table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_DomainDefinition_Update
(

	@pkid int   ,

	@Name varchar (50)  ,

	@IsActive bit   
)
AS


				
				
				-- Modify the updatable columns
				UPDATE
					[dbo].[DomainDefinition]
				SET
					[Name] = @Name
					,[IsActive] = @IsActive
				WHERE
[pkid] = @pkid 
				
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_DomainDefinition_Delete procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_DomainDefinition_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_DomainDefinition_Delete
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Deletes a record in the DomainDefinition table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_DomainDefinition_Delete
(

	@pkid int   
)
AS


				DELETE FROM [dbo].[DomainDefinition] WITH (ROWLOCK) 
				WHERE
					[pkid] = @pkid
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_DomainDefinition_GetBypkid procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_DomainDefinition_GetBypkid') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_DomainDefinition_GetBypkid
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the DomainDefinition table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_DomainDefinition_GetBypkid
(

	@pkid int   
)
AS


				SELECT
					[pkid],
					[Name],
					[IsActive]
				FROM
					[dbo].[DomainDefinition]
				WHERE
					[pkid] = @pkid
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_DomainDefinition_Find procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_DomainDefinition_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_DomainDefinition_Find
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Finds records in the DomainDefinition table passing nullable parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_DomainDefinition_Find
(

	@SearchUsingOR bit   = null ,

	@pkid int   = null ,

	@Name varchar (50)  = null ,

	@IsActive bit   = null 
)
AS


				
  IF ISNULL(@SearchUsingOR, 0) <> 1
  BEGIN
    SELECT
	  [pkid]
	, [Name]
	, [IsActive]
    FROM
	[dbo].[DomainDefinition]
    WHERE 
	 ([pkid] = @pkid OR @pkid IS NULL)
	AND ([Name] = @Name OR @Name IS NULL)
	AND ([IsActive] = @IsActive OR @IsActive IS NULL)
						
  END
  ELSE
  BEGIN
    SELECT
	  [pkid]
	, [Name]
	, [IsActive]
    FROM
	[dbo].[DomainDefinition]
    WHERE 
	 ([pkid] = @pkid AND @pkid is not null)
	OR ([Name] = @Name AND @Name is not null)
	OR ([IsActive] = @IsActive AND @IsActive is not null)
	SELECT @@ROWCOUNT			
  END
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ClassAssociation_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ClassAssociation_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ClassAssociation_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the ClassAssociation table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ClassAssociation_Get_List

AS


				
				SELECT
					[CAid],
					[ParentClass],
					[ChildClass],
					[AssociationTypeID],
					[Caption],
					[AssociationObjectClass],
					[CopyIncluded],
					[IsDefault],
					[IsActive]
				FROM
					[dbo].[ClassAssociation]
					
				SELECT @@ROWCOUNT
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ClassAssociation_GetPaged procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ClassAssociation_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ClassAssociation_GetPaged
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the ClassAssociation table passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ClassAssociation_GetPaged
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  ,

	@PageIndex int   ,

	@PageSize int   
)
AS


				
				BEGIN
				DECLARE @PageLowerBound int
				DECLARE @PageUpperBound int
				
				-- Set the page bounds
				SET @PageLowerBound = @PageSize * @PageIndex
				SET @PageUpperBound = @PageLowerBound + @PageSize

				-- Create a temp table to store the select results
				CREATE TABLE #PageIndex
				(
				    [IndexId] int IDENTITY (1, 1) NOT NULL,
				    [CAid] int 
				)
				
				-- Insert into the temp table
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = 'INSERT INTO #PageIndex ([CAid])'
				SET @SQL = @SQL + ' SELECT'
				IF @PageSize > 0
				BEGIN
					SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound)
				END
				SET @SQL = @SQL + ' [CAid]'
				SET @SQL = @SQL + ' FROM [dbo].[ClassAssociation]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Populate the temp table
				EXEC sp_executesql @SQL

				-- Return paged results
				SELECT O.[CAid], O.[ParentClass], O.[ChildClass], O.[AssociationTypeID], O.[Caption], O.[AssociationObjectClass], O.[CopyIncluded], O.[IsDefault], O.[IsActive]
				FROM
				    [dbo].[ClassAssociation] O,
				    #PageIndex PageIndex
				WHERE
				    PageIndex.IndexId > @PageLowerBound
					AND O.[CAid] = PageIndex.[CAid]
				ORDER BY
				    PageIndex.IndexId
				
				-- get row count
				SET @SQL = 'SELECT COUNT(*) AS TotalRowCount'
				SET @SQL = @SQL + ' FROM [dbo].[ClassAssociation]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				EXEC sp_executesql @SQL
			
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ClassAssociation_Insert procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ClassAssociation_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ClassAssociation_Insert
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Inserts a record into the ClassAssociation table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ClassAssociation_Insert
(

	@CAid int    OUTPUT,

	@ParentClass varchar (50)  ,

	@ChildClass varchar (50)  ,

	@AssociationTypeID int   ,

	@Caption varchar (100)  ,

	@AssociationObjectClass varchar (50)  ,

	@CopyIncluded bit   ,

	@IsDefault bit   ,

	@IsActive bit   
)
AS


				
				INSERT INTO [dbo].[ClassAssociation]
					(
					[ParentClass]
					,[ChildClass]
					,[AssociationTypeID]
					,[Caption]
					,[AssociationObjectClass]
					,[CopyIncluded]
					,[IsDefault]
					,[IsActive]
					)
				VALUES
					(
					@ParentClass
					,@ChildClass
					,@AssociationTypeID
					,@Caption
					,@AssociationObjectClass
					,@CopyIncluded
					,@IsDefault
					,@IsActive
					)
				
				-- Get the identity value
				SET @CAid = SCOPE_IDENTITY()
									
							
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ClassAssociation_Update procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ClassAssociation_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ClassAssociation_Update
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Updates a record in the ClassAssociation table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ClassAssociation_Update
(

	@CAid int   ,

	@ParentClass varchar (50)  ,

	@ChildClass varchar (50)  ,

	@AssociationTypeID int   ,

	@Caption varchar (100)  ,

	@AssociationObjectClass varchar (50)  ,

	@CopyIncluded bit   ,

	@IsDefault bit   ,

	@IsActive bit   
)
AS


				
				
				-- Modify the updatable columns
				UPDATE
					[dbo].[ClassAssociation]
				SET
					[ParentClass] = @ParentClass
					,[ChildClass] = @ChildClass
					,[AssociationTypeID] = @AssociationTypeID
					,[Caption] = @Caption
					,[AssociationObjectClass] = @AssociationObjectClass
					,[CopyIncluded] = @CopyIncluded
					,[IsDefault] = @IsDefault
					,[IsActive] = @IsActive
				WHERE
[CAid] = @CAid 
				
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ClassAssociation_Delete procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ClassAssociation_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ClassAssociation_Delete
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Deletes a record in the ClassAssociation table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ClassAssociation_Delete
(

	@CAid int   
)
AS


				DELETE FROM [dbo].[ClassAssociation] WITH (ROWLOCK) 
				WHERE
					[CAid] = @CAid
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ClassAssociation_GetByParentClass procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ClassAssociation_GetByParentClass') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ClassAssociation_GetByParentClass
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the ClassAssociation table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ClassAssociation_GetByParentClass
(

	@ParentClass varchar (50)  
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[CAid],
					[ParentClass],
					[ChildClass],
					[AssociationTypeID],
					[Caption],
					[AssociationObjectClass],
					[CopyIncluded],
					[IsDefault],
					[IsActive]
				FROM
					[dbo].[ClassAssociation]
				WHERE
					[ParentClass] = @ParentClass
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ClassAssociation_GetByChildClass procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ClassAssociation_GetByChildClass') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ClassAssociation_GetByChildClass
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the ClassAssociation table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ClassAssociation_GetByChildClass
(

	@ChildClass varchar (50)  
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[CAid],
					[ParentClass],
					[ChildClass],
					[AssociationTypeID],
					[Caption],
					[AssociationObjectClass],
					[CopyIncluded],
					[IsDefault],
					[IsActive]
				FROM
					[dbo].[ClassAssociation]
				WHERE
					[ChildClass] = @ChildClass
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ClassAssociation_GetByAssociationTypeID procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ClassAssociation_GetByAssociationTypeID') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ClassAssociation_GetByAssociationTypeID
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the ClassAssociation table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ClassAssociation_GetByAssociationTypeID
(

	@AssociationTypeID int   
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[CAid],
					[ParentClass],
					[ChildClass],
					[AssociationTypeID],
					[Caption],
					[AssociationObjectClass],
					[CopyIncluded],
					[IsDefault],
					[IsActive]
				FROM
					[dbo].[ClassAssociation]
				WHERE
					[AssociationTypeID] = @AssociationTypeID
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ClassAssociation_GetByAssociationObjectClass procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ClassAssociation_GetByAssociationObjectClass') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ClassAssociation_GetByAssociationObjectClass
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the ClassAssociation table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ClassAssociation_GetByAssociationObjectClass
(

	@AssociationObjectClass varchar (50)  
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[CAid],
					[ParentClass],
					[ChildClass],
					[AssociationTypeID],
					[Caption],
					[AssociationObjectClass],
					[CopyIncluded],
					[IsDefault],
					[IsActive]
				FROM
					[dbo].[ClassAssociation]
				WHERE
					[AssociationObjectClass] = @AssociationObjectClass
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ClassAssociation_GetByCAid procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ClassAssociation_GetByCAid') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ClassAssociation_GetByCAid
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the ClassAssociation table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ClassAssociation_GetByCAid
(

	@CAid int   
)
AS


				SELECT
					[CAid],
					[ParentClass],
					[ChildClass],
					[AssociationTypeID],
					[Caption],
					[AssociationObjectClass],
					[CopyIncluded],
					[IsDefault],
					[IsActive]
				FROM
					[dbo].[ClassAssociation]
				WHERE
					[CAid] = @CAid
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ClassAssociation_GetByClassFromAllowedArtifact procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ClassAssociation_GetByClassFromAllowedArtifact') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ClassAssociation_GetByClassFromAllowedArtifact
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records through a junction table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ClassAssociation_GetByClassFromAllowedArtifact
(

	@Class varchar (50)  
)
AS


SELECT dbo.[ClassAssociation].[CAid]
       ,dbo.[ClassAssociation].[ParentClass]
       ,dbo.[ClassAssociation].[ChildClass]
       ,dbo.[ClassAssociation].[AssociationTypeID]
       ,dbo.[ClassAssociation].[Caption]
       ,dbo.[ClassAssociation].[AssociationObjectClass]
       ,dbo.[ClassAssociation].[CopyIncluded]
       ,dbo.[ClassAssociation].[IsDefault]
       ,dbo.[ClassAssociation].[IsActive]
  FROM dbo.[ClassAssociation]
 WHERE EXISTS (SELECT 1
                 FROM dbo.[AllowedArtifact] 
                WHERE dbo.[AllowedArtifact].[Class] = @Class
                  AND dbo.[AllowedArtifact].[CAid] = dbo.[ClassAssociation].[CAid]
                  )
				SELECT @@ROWCOUNT			
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ClassAssociation_GetByModelTypeAcronymFromModelTypeAssociationDefault procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ClassAssociation_GetByModelTypeAcronymFromModelTypeAssociationDefault') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ClassAssociation_GetByModelTypeAcronymFromModelTypeAssociationDefault
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records through a junction table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ClassAssociation_GetByModelTypeAcronymFromModelTypeAssociationDefault
(

	@ModelTypeAcronym varchar (50)  
)
AS


SELECT dbo.[ClassAssociation].[CAid]
       ,dbo.[ClassAssociation].[ParentClass]
       ,dbo.[ClassAssociation].[ChildClass]
       ,dbo.[ClassAssociation].[AssociationTypeID]
       ,dbo.[ClassAssociation].[Caption]
       ,dbo.[ClassAssociation].[AssociationObjectClass]
       ,dbo.[ClassAssociation].[CopyIncluded]
       ,dbo.[ClassAssociation].[IsDefault]
       ,dbo.[ClassAssociation].[IsActive]
  FROM dbo.[ClassAssociation]
 WHERE EXISTS (SELECT 1
                 FROM dbo.[ModelTypeAssociationDefault] 
                WHERE dbo.[ModelTypeAssociationDefault].[ModelTypeAcronym] = @ModelTypeAcronym
                  AND dbo.[ModelTypeAssociationDefault].[CAid] = dbo.[ClassAssociation].[CAid]
                  )
				SELECT @@ROWCOUNT			
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ClassAssociation_GetByObjectIDObjectMachineFromObjectAssociation procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ClassAssociation_GetByObjectIDObjectMachineFromObjectAssociation') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ClassAssociation_GetByObjectIDObjectMachineFromObjectAssociation
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records through a junction table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ClassAssociation_GetByObjectIDObjectMachineFromObjectAssociation
(

	@ObjectID int   ,

	@ObjectMachine varchar (50)  
)
AS


SELECT dbo.[ClassAssociation].[CAid]
       ,dbo.[ClassAssociation].[ParentClass]
       ,dbo.[ClassAssociation].[ChildClass]
       ,dbo.[ClassAssociation].[AssociationTypeID]
       ,dbo.[ClassAssociation].[Caption]
       ,dbo.[ClassAssociation].[AssociationObjectClass]
       ,dbo.[ClassAssociation].[CopyIncluded]
       ,dbo.[ClassAssociation].[IsDefault]
       ,dbo.[ClassAssociation].[IsActive]
  FROM dbo.[ClassAssociation]
 WHERE EXISTS (SELECT 1
                 FROM dbo.[ObjectAssociation] 
                WHERE dbo.[ObjectAssociation].[ObjectID] = @ObjectID
                  AND dbo.[ObjectAssociation].[ObjectMachine] = @ObjectMachine
                  AND dbo.[ObjectAssociation].[CAid] = dbo.[ClassAssociation].[CAid]
                  )
				SELECT @@ROWCOUNT			
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ClassAssociation_GetByChildObjectIDChildObjectMachineFromObjectAssociation procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ClassAssociation_GetByChildObjectIDChildObjectMachineFromObjectAssociation') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ClassAssociation_GetByChildObjectIDChildObjectMachineFromObjectAssociation
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records through a junction table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ClassAssociation_GetByChildObjectIDChildObjectMachineFromObjectAssociation
(

	@ChildObjectID int   ,

	@ChildObjectMachine varchar (50)  
)
AS


SELECT dbo.[ClassAssociation].[CAid]
       ,dbo.[ClassAssociation].[ParentClass]
       ,dbo.[ClassAssociation].[ChildClass]
       ,dbo.[ClassAssociation].[AssociationTypeID]
       ,dbo.[ClassAssociation].[Caption]
       ,dbo.[ClassAssociation].[AssociationObjectClass]
       ,dbo.[ClassAssociation].[CopyIncluded]
       ,dbo.[ClassAssociation].[IsDefault]
       ,dbo.[ClassAssociation].[IsActive]
  FROM dbo.[ClassAssociation]
 WHERE EXISTS (SELECT 1
                 FROM dbo.[ObjectAssociation] 
                WHERE dbo.[ObjectAssociation].[ChildObjectID] = @ChildObjectID
                  AND dbo.[ObjectAssociation].[ChildObjectMachine] = @ChildObjectMachine
                  AND dbo.[ObjectAssociation].[CAid] = dbo.[ClassAssociation].[CAid]
                  )
				SELECT @@ROWCOUNT			
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ClassAssociation_Find procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ClassAssociation_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ClassAssociation_Find
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Finds records in the ClassAssociation table passing nullable parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ClassAssociation_Find
(

	@SearchUsingOR bit   = null ,

	@CAid int   = null ,

	@ParentClass varchar (50)  = null ,

	@ChildClass varchar (50)  = null ,

	@AssociationTypeID int   = null ,

	@Caption varchar (100)  = null ,

	@AssociationObjectClass varchar (50)  = null ,

	@CopyIncluded bit   = null ,

	@IsDefault bit   = null ,

	@IsActive bit   = null 
)
AS


				
  IF ISNULL(@SearchUsingOR, 0) <> 1
  BEGIN
    SELECT
	  [CAid]
	, [ParentClass]
	, [ChildClass]
	, [AssociationTypeID]
	, [Caption]
	, [AssociationObjectClass]
	, [CopyIncluded]
	, [IsDefault]
	, [IsActive]
    FROM
	[dbo].[ClassAssociation]
    WHERE 
	 ([CAid] = @CAid OR @CAid IS NULL)
	AND ([ParentClass] = @ParentClass OR @ParentClass IS NULL)
	AND ([ChildClass] = @ChildClass OR @ChildClass IS NULL)
	AND ([AssociationTypeID] = @AssociationTypeID OR @AssociationTypeID IS NULL)
	AND ([Caption] = @Caption OR @Caption IS NULL)
	AND ([AssociationObjectClass] = @AssociationObjectClass OR @AssociationObjectClass IS NULL)
	AND ([CopyIncluded] = @CopyIncluded OR @CopyIncluded IS NULL)
	AND ([IsDefault] = @IsDefault OR @IsDefault IS NULL)
	AND ([IsActive] = @IsActive OR @IsActive IS NULL)
						
  END
  ELSE
  BEGIN
    SELECT
	  [CAid]
	, [ParentClass]
	, [ChildClass]
	, [AssociationTypeID]
	, [Caption]
	, [AssociationObjectClass]
	, [CopyIncluded]
	, [IsDefault]
	, [IsActive]
    FROM
	[dbo].[ClassAssociation]
    WHERE 
	 ([CAid] = @CAid AND @CAid is not null)
	OR ([ParentClass] = @ParentClass AND @ParentClass is not null)
	OR ([ChildClass] = @ChildClass AND @ChildClass is not null)
	OR ([AssociationTypeID] = @AssociationTypeID AND @AssociationTypeID is not null)
	OR ([Caption] = @Caption AND @Caption is not null)
	OR ([AssociationObjectClass] = @AssociationObjectClass AND @AssociationObjectClass is not null)
	OR ([CopyIncluded] = @CopyIncluded AND @CopyIncluded is not null)
	OR ([IsDefault] = @IsDefault AND @IsDefault is not null)
	OR ([IsActive] = @IsActive AND @IsActive is not null)
	SELECT @@ROWCOUNT			
  END
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFileObject_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFileObject_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFileObject_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the GraphFileObject table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFileObject_Get_List

AS


				
				SELECT
					[GraphFileID],
					[MetaObjectID],
					[MachineID],
					[GraphFileMachine]
				FROM
					[dbo].[GraphFileObject]
					
				SELECT @@ROWCOUNT
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFileObject_GetPaged procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFileObject_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFileObject_GetPaged
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the GraphFileObject table passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFileObject_GetPaged
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  ,

	@PageIndex int   ,

	@PageSize int   
)
AS


				
				BEGIN
				DECLARE @PageLowerBound int
				DECLARE @PageUpperBound int
				
				-- Set the page bounds
				SET @PageLowerBound = @PageSize * @PageIndex
				SET @PageUpperBound = @PageLowerBound + @PageSize

				-- Create a temp table to store the select results
				CREATE TABLE #PageIndex
				(
				    [IndexId] int IDENTITY (1, 1) NOT NULL,
				    [GraphFileID] int, [MetaObjectID] int, [MachineID] varchar(50) COLLATE database_default , [GraphFileMachine] varchar(50) COLLATE database_default  
				)
				
				-- Insert into the temp table
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = 'INSERT INTO #PageIndex ([GraphFileID], [MetaObjectID], [MachineID], [GraphFileMachine])'
				SET @SQL = @SQL + ' SELECT'
				IF @PageSize > 0
				BEGIN
					SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound)
				END
				SET @SQL = @SQL + ' [GraphFileID], [MetaObjectID], [MachineID], [GraphFileMachine]'
				SET @SQL = @SQL + ' FROM [dbo].[GraphFileObject]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Populate the temp table
				EXEC sp_executesql @SQL

				-- Return paged results
				SELECT O.[GraphFileID], O.[MetaObjectID], O.[MachineID], O.[GraphFileMachine]
				FROM
				    [dbo].[GraphFileObject] O,
				    #PageIndex PageIndex
				WHERE
				    PageIndex.IndexId > @PageLowerBound
					AND O.[GraphFileID] = PageIndex.[GraphFileID]
					AND O.[MetaObjectID] = PageIndex.[MetaObjectID]
					AND O.[MachineID] = PageIndex.[MachineID]
					AND O.[GraphFileMachine] = PageIndex.[GraphFileMachine]
				ORDER BY
				    PageIndex.IndexId
				
				-- get row count
				SET @SQL = 'SELECT COUNT(*) AS TotalRowCount'
				SET @SQL = @SQL + ' FROM [dbo].[GraphFileObject]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				EXEC sp_executesql @SQL
			
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFileObject_Insert procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFileObject_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFileObject_Insert
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Inserts a record into the GraphFileObject table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFileObject_Insert
(

	@GraphFileID int   ,

	@MetaObjectID int   ,

	@MachineID varchar (50)  ,

	@GraphFileMachine varchar (50)  
)
AS


				
				INSERT INTO [dbo].[GraphFileObject]
					(
					[GraphFileID]
					,[MetaObjectID]
					,[MachineID]
					,[GraphFileMachine]
					)
				VALUES
					(
					@GraphFileID
					,@MetaObjectID
					,@MachineID
					,@GraphFileMachine
					)
				
									
							
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFileObject_Update procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFileObject_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFileObject_Update
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Updates a record in the GraphFileObject table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFileObject_Update
(

	@GraphFileID int   ,

	@OriginalGraphFileID int   ,

	@MetaObjectID int   ,

	@OriginalMetaObjectID int   ,

	@MachineID varchar (50)  ,

	@OriginalMachineID varchar (50)  ,

	@GraphFileMachine varchar (50)  ,

	@OriginalGraphFileMachine varchar (50)  
)
AS


				
				
				-- Modify the updatable columns
				UPDATE
					[dbo].[GraphFileObject]
				SET
					[GraphFileID] = @GraphFileID
					,[MetaObjectID] = @MetaObjectID
					,[MachineID] = @MachineID
					,[GraphFileMachine] = @GraphFileMachine
				WHERE
[GraphFileID] = @OriginalGraphFileID 
AND [MetaObjectID] = @OriginalMetaObjectID 
AND [MachineID] = @OriginalMachineID 
AND [GraphFileMachine] = @OriginalGraphFileMachine 
				
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFileObject_Delete procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFileObject_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFileObject_Delete
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Deletes a record in the GraphFileObject table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFileObject_Delete
(

	@GraphFileID int   ,

	@MetaObjectID int   ,

	@MachineID varchar (50)  ,

	@GraphFileMachine varchar (50)  
)
AS


				DELETE FROM [dbo].[GraphFileObject] WITH (ROWLOCK) 
				WHERE
					[GraphFileID] = @GraphFileID
					AND [MetaObjectID] = @MetaObjectID
					AND [MachineID] = @MachineID
					AND [GraphFileMachine] = @GraphFileMachine
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFileObject_GetByGraphFileIDGraphFileMachine procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFileObject_GetByGraphFileIDGraphFileMachine') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFileObject_GetByGraphFileIDGraphFileMachine
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the GraphFileObject table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFileObject_GetByGraphFileIDGraphFileMachine
(

	@GraphFileID int   ,

	@GraphFileMachine varchar (50)  
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[GraphFileID],
					[MetaObjectID],
					[MachineID],
					[GraphFileMachine]
				FROM
					[dbo].[GraphFileObject]
				WHERE
					[GraphFileID] = @GraphFileID
					AND [GraphFileMachine] = @GraphFileMachine
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFileObject_GetByMetaObjectIDMachineID procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFileObject_GetByMetaObjectIDMachineID') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFileObject_GetByMetaObjectIDMachineID
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the GraphFileObject table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFileObject_GetByMetaObjectIDMachineID
(

	@MetaObjectID int   ,

	@MachineID varchar (50)  
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[GraphFileID],
					[MetaObjectID],
					[MachineID],
					[GraphFileMachine]
				FROM
					[dbo].[GraphFileObject]
				WHERE
					[MetaObjectID] = @MetaObjectID
					AND [MachineID] = @MachineID
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFileObject_GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFileObject_GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFileObject_GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the GraphFileObject table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFileObject_GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine
(

	@GraphFileID int   ,

	@MetaObjectID int   ,

	@MachineID varchar (50)  ,

	@GraphFileMachine varchar (50)  
)
AS


				SELECT
					[GraphFileID],
					[MetaObjectID],
					[MachineID],
					[GraphFileMachine]
				FROM
					[dbo].[GraphFileObject]
				WHERE
					[GraphFileID] = @GraphFileID
					AND [MetaObjectID] = @MetaObjectID
					AND [MachineID] = @MachineID
					AND [GraphFileMachine] = @GraphFileMachine
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFileObject_Find procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFileObject_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFileObject_Find
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Finds records in the GraphFileObject table passing nullable parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFileObject_Find
(

	@SearchUsingOR bit   = null ,

	@GraphFileID int   = null ,

	@MetaObjectID int   = null ,

	@MachineID varchar (50)  = null ,

	@GraphFileMachine varchar (50)  = null 
)
AS


				
  IF ISNULL(@SearchUsingOR, 0) <> 1
  BEGIN
    SELECT
	  [GraphFileID]
	, [MetaObjectID]
	, [MachineID]
	, [GraphFileMachine]
    FROM
	[dbo].[GraphFileObject]
    WHERE 
	 ([GraphFileID] = @GraphFileID OR @GraphFileID IS NULL)
	AND ([MetaObjectID] = @MetaObjectID OR @MetaObjectID IS NULL)
	AND ([MachineID] = @MachineID OR @MachineID IS NULL)
	AND ([GraphFileMachine] = @GraphFileMachine OR @GraphFileMachine IS NULL)
						
  END
  ELSE
  BEGIN
    SELECT
	  [GraphFileID]
	, [MetaObjectID]
	, [MachineID]
	, [GraphFileMachine]
    FROM
	[dbo].[GraphFileObject]
    WHERE 
	 ([GraphFileID] = @GraphFileID AND @GraphFileID is not null)
	OR ([MetaObjectID] = @MetaObjectID AND @MetaObjectID is not null)
	OR ([MachineID] = @MachineID AND @MachineID is not null)
	OR ([GraphFileMachine] = @GraphFileMachine AND @GraphFileMachine is not null)
	SELECT @@ROWCOUNT			
  END
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_DomainDefinitionPossibleValue_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_DomainDefinitionPossibleValue_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the DomainDefinitionPossibleValue table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_Get_List

AS


				
				SELECT
					[DomainDefinitionID],
					[PossibleValue],
					[Series],
					[Description],
					[IsActive],
					[URI_ID]
				FROM
					[dbo].[DomainDefinitionPossibleValue]
					
				SELECT @@ROWCOUNT
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_DomainDefinitionPossibleValue_GetPaged procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_DomainDefinitionPossibleValue_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_GetPaged
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the DomainDefinitionPossibleValue table passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_GetPaged
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  ,

	@PageIndex int   ,

	@PageSize int   
)
AS


				
				BEGIN
				DECLARE @PageLowerBound int
				DECLARE @PageUpperBound int
				
				-- Set the page bounds
				SET @PageLowerBound = @PageSize * @PageIndex
				SET @PageUpperBound = @PageLowerBound + @PageSize

				-- Create a temp table to store the select results
				CREATE TABLE #PageIndex
				(
				    [IndexId] int IDENTITY (1, 1) NOT NULL,
				    [DomainDefinitionID] int, [PossibleValue] varchar(50) COLLATE database_default  
				)
				
				-- Insert into the temp table
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = 'INSERT INTO #PageIndex ([DomainDefinitionID], [PossibleValue])'
				SET @SQL = @SQL + ' SELECT'
				IF @PageSize > 0
				BEGIN
					SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound)
				END
				SET @SQL = @SQL + ' [DomainDefinitionID], [PossibleValue]'
				SET @SQL = @SQL + ' FROM [dbo].[DomainDefinitionPossibleValue]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Populate the temp table
				EXEC sp_executesql @SQL

				-- Return paged results
				SELECT O.[DomainDefinitionID], O.[PossibleValue], O.[Series], O.[Description], O.[IsActive], O.[URI_ID]
				FROM
				    [dbo].[DomainDefinitionPossibleValue] O,
				    #PageIndex PageIndex
				WHERE
				    PageIndex.IndexId > @PageLowerBound
					AND O.[DomainDefinitionID] = PageIndex.[DomainDefinitionID]
					AND O.[PossibleValue] = PageIndex.[PossibleValue]
				ORDER BY
				    PageIndex.IndexId
				
				-- get row count
				SET @SQL = 'SELECT COUNT(*) AS TotalRowCount'
				SET @SQL = @SQL + ' FROM [dbo].[DomainDefinitionPossibleValue]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				EXEC sp_executesql @SQL
			
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_DomainDefinitionPossibleValue_Insert procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_DomainDefinitionPossibleValue_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_Insert
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Inserts a record into the DomainDefinitionPossibleValue table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_Insert
(

	@DomainDefinitionID int   ,

	@PossibleValue varchar (50)  ,

	@Series int   ,

	@Description varchar (120)  ,

	@IsActive bit   ,

	@URI_ID int   
)
AS


				
				INSERT INTO [dbo].[DomainDefinitionPossibleValue]
					(
					[DomainDefinitionID]
					,[PossibleValue]
					,[Series]
					,[Description]
					,[IsActive]
					,[URI_ID]
					)
				VALUES
					(
					@DomainDefinitionID
					,@PossibleValue
					,@Series
					,@Description
					,@IsActive
					,@URI_ID
					)
				
									
							
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_DomainDefinitionPossibleValue_Update procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_DomainDefinitionPossibleValue_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_Update
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Updates a record in the DomainDefinitionPossibleValue table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_Update
(

	@DomainDefinitionID int   ,

	@OriginalDomainDefinitionID int   ,

	@PossibleValue varchar (50)  ,

	@OriginalPossibleValue varchar (50)  ,

	@Series int   ,

	@Description varchar (120)  ,

	@IsActive bit   ,

	@URI_ID int   
)
AS


				
				
				-- Modify the updatable columns
				UPDATE
					[dbo].[DomainDefinitionPossibleValue]
				SET
					[DomainDefinitionID] = @DomainDefinitionID
					,[PossibleValue] = @PossibleValue
					,[Series] = @Series
					,[Description] = @Description
					,[IsActive] = @IsActive
					,[URI_ID] = @URI_ID
				WHERE
[DomainDefinitionID] = @OriginalDomainDefinitionID 
AND [PossibleValue] = @OriginalPossibleValue 
				
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_DomainDefinitionPossibleValue_Delete procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_DomainDefinitionPossibleValue_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_Delete
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Deletes a record in the DomainDefinitionPossibleValue table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_Delete
(

	@DomainDefinitionID int   ,

	@PossibleValue varchar (50)  
)
AS


				DELETE FROM [dbo].[DomainDefinitionPossibleValue] WITH (ROWLOCK) 
				WHERE
					[DomainDefinitionID] = @DomainDefinitionID
					AND [PossibleValue] = @PossibleValue
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_DomainDefinitionPossibleValue_GetByDomainDefinitionID procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_DomainDefinitionPossibleValue_GetByDomainDefinitionID') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_GetByDomainDefinitionID
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the DomainDefinitionPossibleValue table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_GetByDomainDefinitionID
(

	@DomainDefinitionID int   
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[DomainDefinitionID],
					[PossibleValue],
					[Series],
					[Description],
					[IsActive],
					[URI_ID]
				FROM
					[dbo].[DomainDefinitionPossibleValue]
				WHERE
					[DomainDefinitionID] = @DomainDefinitionID
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_DomainDefinitionPossibleValue_GetByURI_ID procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_DomainDefinitionPossibleValue_GetByURI_ID') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_GetByURI_ID
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the DomainDefinitionPossibleValue table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_GetByURI_ID
(

	@URI_ID int   
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[DomainDefinitionID],
					[PossibleValue],
					[Series],
					[Description],
					[IsActive],
					[URI_ID]
				FROM
					[dbo].[DomainDefinitionPossibleValue]
				WHERE
					[URI_ID] = @URI_ID
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_DomainDefinitionPossibleValue_GetByDomainDefinitionIDPossibleValue procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_DomainDefinitionPossibleValue_GetByDomainDefinitionIDPossibleValue') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_GetByDomainDefinitionIDPossibleValue
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the DomainDefinitionPossibleValue table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_GetByDomainDefinitionIDPossibleValue
(

	@DomainDefinitionID int   ,

	@PossibleValue varchar (50)  
)
AS


				SELECT
					[DomainDefinitionID],
					[PossibleValue],
					[Series],
					[Description],
					[IsActive],
					[URI_ID]
				FROM
					[dbo].[DomainDefinitionPossibleValue]
				WHERE
					[DomainDefinitionID] = @DomainDefinitionID
					AND [PossibleValue] = @PossibleValue
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_DomainDefinitionPossibleValue_Find procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_DomainDefinitionPossibleValue_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_Find
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Finds records in the DomainDefinitionPossibleValue table passing nullable parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_DomainDefinitionPossibleValue_Find
(

	@SearchUsingOR bit   = null ,

	@DomainDefinitionID int   = null ,

	@PossibleValue varchar (50)  = null ,

	@Series int   = null ,

	@Description varchar (120)  = null ,

	@IsActive bit   = null ,

	@URI_ID int   = null 
)
AS


				
  IF ISNULL(@SearchUsingOR, 0) <> 1
  BEGIN
    SELECT
	  [DomainDefinitionID]
	, [PossibleValue]
	, [Series]
	, [Description]
	, [IsActive]
	, [URI_ID]
    FROM
	[dbo].[DomainDefinitionPossibleValue]
    WHERE 
	 ([DomainDefinitionID] = @DomainDefinitionID OR @DomainDefinitionID IS NULL)
	AND ([PossibleValue] = @PossibleValue OR @PossibleValue IS NULL)
	AND ([Series] = @Series OR @Series IS NULL)
	AND ([Description] = @Description OR @Description IS NULL)
	AND ([IsActive] = @IsActive OR @IsActive IS NULL)
	AND ([URI_ID] = @URI_ID OR @URI_ID IS NULL)
						
  END
  ELSE
  BEGIN
    SELECT
	  [DomainDefinitionID]
	, [PossibleValue]
	, [Series]
	, [Description]
	, [IsActive]
	, [URI_ID]
    FROM
	[dbo].[DomainDefinitionPossibleValue]
    WHERE 
	 ([DomainDefinitionID] = @DomainDefinitionID AND @DomainDefinitionID is not null)
	OR ([PossibleValue] = @PossibleValue AND @PossibleValue is not null)
	OR ([Series] = @Series AND @Series is not null)
	OR ([Description] = @Description AND @Description is not null)
	OR ([IsActive] = @IsActive AND @IsActive is not null)
	OR ([URI_ID] = @URI_ID AND @URI_ID is not null)
	SELECT @@ROWCOUNT			
  END
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFileAssociation_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFileAssociation_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFileAssociation_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the GraphFileAssociation table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFileAssociation_Get_List

AS


				
				SELECT
					[GraphFileID],
					[GraphFileMachine],
					[ChildObjectMachine],
					[CAid],
					[ObjectID],
					[ChildObjectID],
					[ObjectMachine]
				FROM
					[dbo].[GraphFileAssociation]
					
				SELECT @@ROWCOUNT
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFileAssociation_GetPaged procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFileAssociation_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFileAssociation_GetPaged
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the GraphFileAssociation table passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFileAssociation_GetPaged
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  ,

	@PageIndex int   ,

	@PageSize int   
)
AS


				
				BEGIN
				DECLARE @PageLowerBound int
				DECLARE @PageUpperBound int
				
				-- Set the page bounds
				SET @PageLowerBound = @PageSize * @PageIndex
				SET @PageUpperBound = @PageLowerBound + @PageSize

				-- Create a temp table to store the select results
				CREATE TABLE #PageIndex
				(
				    [IndexId] int IDENTITY (1, 1) NOT NULL,
				    [GraphFileID] int, [GraphFileMachine] varchar(50) COLLATE database_default , [ChildObjectMachine] varchar(50) COLLATE database_default , [CAid] int, [ObjectID] int, [ChildObjectID] int, [ObjectMachine] varchar(50) COLLATE database_default  
				)
				
				-- Insert into the temp table
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = 'INSERT INTO #PageIndex ([GraphFileID], [GraphFileMachine], [ChildObjectMachine], [CAid], [ObjectID], [ChildObjectID], [ObjectMachine])'
				SET @SQL = @SQL + ' SELECT'
				IF @PageSize > 0
				BEGIN
					SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound)
				END
				SET @SQL = @SQL + ' [GraphFileID], [GraphFileMachine], [ChildObjectMachine], [CAid], [ObjectID], [ChildObjectID], [ObjectMachine]'
				SET @SQL = @SQL + ' FROM [dbo].[GraphFileAssociation]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Populate the temp table
				EXEC sp_executesql @SQL

				-- Return paged results
				SELECT O.[GraphFileID], O.[GraphFileMachine], O.[ChildObjectMachine], O.[CAid], O.[ObjectID], O.[ChildObjectID], O.[ObjectMachine]
				FROM
				    [dbo].[GraphFileAssociation] O,
				    #PageIndex PageIndex
				WHERE
				    PageIndex.IndexId > @PageLowerBound
					AND O.[GraphFileID] = PageIndex.[GraphFileID]
					AND O.[GraphFileMachine] = PageIndex.[GraphFileMachine]
					AND O.[ChildObjectMachine] = PageIndex.[ChildObjectMachine]
					AND O.[CAid] = PageIndex.[CAid]
					AND O.[ObjectID] = PageIndex.[ObjectID]
					AND O.[ChildObjectID] = PageIndex.[ChildObjectID]
					AND O.[ObjectMachine] = PageIndex.[ObjectMachine]
				ORDER BY
				    PageIndex.IndexId
				
				-- get row count
				SET @SQL = 'SELECT COUNT(*) AS TotalRowCount'
				SET @SQL = @SQL + ' FROM [dbo].[GraphFileAssociation]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				EXEC sp_executesql @SQL
			
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFileAssociation_Insert procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFileAssociation_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFileAssociation_Insert
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Inserts a record into the GraphFileAssociation table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFileAssociation_Insert
(

	@GraphFileID int   ,

	@GraphFileMachine varchar (50)  ,

	@ChildObjectMachine varchar (50)  ,

	@CAid int   ,

	@ObjectID int   ,

	@ChildObjectID int   ,

	@ObjectMachine varchar (50)  
)
AS


				
				INSERT INTO [dbo].[GraphFileAssociation]
					(
					[GraphFileID]
					,[GraphFileMachine]
					,[ChildObjectMachine]
					,[CAid]
					,[ObjectID]
					,[ChildObjectID]
					,[ObjectMachine]
					)
				VALUES
					(
					@GraphFileID
					,@GraphFileMachine
					,@ChildObjectMachine
					,@CAid
					,@ObjectID
					,@ChildObjectID
					,@ObjectMachine
					)
				
									
							
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFileAssociation_Update procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFileAssociation_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFileAssociation_Update
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Updates a record in the GraphFileAssociation table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFileAssociation_Update
(

	@GraphFileID int   ,

	@OriginalGraphFileID int   ,

	@GraphFileMachine varchar (50)  ,

	@OriginalGraphFileMachine varchar (50)  ,

	@ChildObjectMachine varchar (50)  ,

	@OriginalChildObjectMachine varchar (50)  ,

	@CAid int   ,

	@OriginalCAid int   ,

	@ObjectID int   ,

	@OriginalObjectID int   ,

	@ChildObjectID int   ,

	@OriginalChildObjectID int   ,

	@ObjectMachine varchar (50)  ,

	@OriginalObjectMachine varchar (50)  
)
AS


				
				
				-- Modify the updatable columns
				UPDATE
					[dbo].[GraphFileAssociation]
				SET
					[GraphFileID] = @GraphFileID
					,[GraphFileMachine] = @GraphFileMachine
					,[ChildObjectMachine] = @ChildObjectMachine
					,[CAid] = @CAid
					,[ObjectID] = @ObjectID
					,[ChildObjectID] = @ChildObjectID
					,[ObjectMachine] = @ObjectMachine
				WHERE
[GraphFileID] = @OriginalGraphFileID 
AND [GraphFileMachine] = @OriginalGraphFileMachine 
AND [ChildObjectMachine] = @OriginalChildObjectMachine 
AND [CAid] = @OriginalCAid 
AND [ObjectID] = @OriginalObjectID 
AND [ChildObjectID] = @OriginalChildObjectID 
AND [ObjectMachine] = @OriginalObjectMachine 
				
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFileAssociation_Delete procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFileAssociation_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFileAssociation_Delete
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Deletes a record in the GraphFileAssociation table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFileAssociation_Delete
(

	@GraphFileID int   ,

	@GraphFileMachine varchar (50)  ,

	@ChildObjectMachine varchar (50)  ,

	@CAid int   ,

	@ObjectID int   ,

	@ChildObjectID int   ,

	@ObjectMachine varchar (50)  
)
AS


				DELETE FROM [dbo].[GraphFileAssociation] WITH (ROWLOCK) 
				WHERE
					[GraphFileID] = @GraphFileID
					AND [GraphFileMachine] = @GraphFileMachine
					AND [ChildObjectMachine] = @ChildObjectMachine
					AND [CAid] = @CAid
					AND [ObjectID] = @ObjectID
					AND [ChildObjectID] = @ChildObjectID
					AND [ObjectMachine] = @ObjectMachine
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFileAssociation_GetByGraphFileIDGraphFileMachine procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFileAssociation_GetByGraphFileIDGraphFileMachine') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFileAssociation_GetByGraphFileIDGraphFileMachine
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the GraphFileAssociation table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFileAssociation_GetByGraphFileIDGraphFileMachine
(

	@GraphFileID int   ,

	@GraphFileMachine varchar (50)  
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[GraphFileID],
					[GraphFileMachine],
					[ChildObjectMachine],
					[CAid],
					[ObjectID],
					[ChildObjectID],
					[ObjectMachine]
				FROM
					[dbo].[GraphFileAssociation]
				WHERE
					[GraphFileID] = @GraphFileID
					AND [GraphFileMachine] = @GraphFileMachine
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFileAssociation_GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFileAssociation_GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFileAssociation_GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the GraphFileAssociation table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFileAssociation_GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine
(

	@CAid int   ,

	@ObjectID int   ,

	@ChildObjectID int   ,

	@ObjectMachine varchar (50)  ,

	@ChildObjectMachine varchar (50)  
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[GraphFileID],
					[GraphFileMachine],
					[ChildObjectMachine],
					[CAid],
					[ObjectID],
					[ChildObjectID],
					[ObjectMachine]
				FROM
					[dbo].[GraphFileAssociation]
				WHERE
					[CAid] = @CAid
					AND [ObjectID] = @ObjectID
					AND [ChildObjectID] = @ChildObjectID
					AND [ObjectMachine] = @ObjectMachine
					AND [ChildObjectMachine] = @ChildObjectMachine
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFileAssociation_GetByGraphFileIDGraphFileMachineChildObjectMachineCAidObjectIDChildObjectIDObjectMachine procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFileAssociation_GetByGraphFileIDGraphFileMachineChildObjectMachineCAidObjectIDChildObjectIDObjectMachine') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFileAssociation_GetByGraphFileIDGraphFileMachineChildObjectMachineCAidObjectIDChildObjectIDObjectMachine
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the GraphFileAssociation table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFileAssociation_GetByGraphFileIDGraphFileMachineChildObjectMachineCAidObjectIDChildObjectIDObjectMachine
(

	@GraphFileID int   ,

	@GraphFileMachine varchar (50)  ,

	@ChildObjectMachine varchar (50)  ,

	@CAid int   ,

	@ObjectID int   ,

	@ChildObjectID int   ,

	@ObjectMachine varchar (50)  
)
AS


				SELECT
					[GraphFileID],
					[GraphFileMachine],
					[ChildObjectMachine],
					[CAid],
					[ObjectID],
					[ChildObjectID],
					[ObjectMachine]
				FROM
					[dbo].[GraphFileAssociation]
				WHERE
					[GraphFileID] = @GraphFileID
					AND [GraphFileMachine] = @GraphFileMachine
					AND [ChildObjectMachine] = @ChildObjectMachine
					AND [CAid] = @CAid
					AND [ObjectID] = @ObjectID
					AND [ChildObjectID] = @ChildObjectID
					AND [ObjectMachine] = @ObjectMachine
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_GraphFileAssociation_Find procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_GraphFileAssociation_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_GraphFileAssociation_Find
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Finds records in the GraphFileAssociation table passing nullable parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_GraphFileAssociation_Find
(

	@SearchUsingOR bit   = null ,

	@GraphFileID int   = null ,

	@GraphFileMachine varchar (50)  = null ,

	@ChildObjectMachine varchar (50)  = null ,

	@CAid int   = null ,

	@ObjectID int   = null ,

	@ChildObjectID int   = null ,

	@ObjectMachine varchar (50)  = null 
)
AS


				
  IF ISNULL(@SearchUsingOR, 0) <> 1
  BEGIN
    SELECT
	  [GraphFileID]
	, [GraphFileMachine]
	, [ChildObjectMachine]
	, [CAid]
	, [ObjectID]
	, [ChildObjectID]
	, [ObjectMachine]
    FROM
	[dbo].[GraphFileAssociation]
    WHERE 
	 ([GraphFileID] = @GraphFileID OR @GraphFileID IS NULL)
	AND ([GraphFileMachine] = @GraphFileMachine OR @GraphFileMachine IS NULL)
	AND ([ChildObjectMachine] = @ChildObjectMachine OR @ChildObjectMachine IS NULL)
	AND ([CAid] = @CAid OR @CAid IS NULL)
	AND ([ObjectID] = @ObjectID OR @ObjectID IS NULL)
	AND ([ChildObjectID] = @ChildObjectID OR @ChildObjectID IS NULL)
	AND ([ObjectMachine] = @ObjectMachine OR @ObjectMachine IS NULL)
						
  END
  ELSE
  BEGIN
    SELECT
	  [GraphFileID]
	, [GraphFileMachine]
	, [ChildObjectMachine]
	, [CAid]
	, [ObjectID]
	, [ChildObjectID]
	, [ObjectMachine]
    FROM
	[dbo].[GraphFileAssociation]
    WHERE 
	 ([GraphFileID] = @GraphFileID AND @GraphFileID is not null)
	OR ([GraphFileMachine] = @GraphFileMachine AND @GraphFileMachine is not null)
	OR ([ChildObjectMachine] = @ChildObjectMachine AND @ChildObjectMachine is not null)
	OR ([CAid] = @CAid AND @CAid is not null)
	OR ([ObjectID] = @ObjectID AND @ObjectID is not null)
	OR ([ChildObjectID] = @ChildObjectID AND @ChildObjectID is not null)
	OR ([ObjectMachine] = @ObjectMachine AND @ObjectMachine is not null)
	SELECT @@ROWCOUNT			
  END
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Artifact_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Artifact_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Artifact_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the Artifact table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Artifact_Get_List

AS


				
				SELECT
					[ArtifactID],
					[CAid],
					[ObjectID],
					[ChildObjectID],
					[ArtifactObjectID],
					[ObjectMachine],
					[ChildObjectMachine],
					[ArtefactMachine]
				FROM
					[dbo].[Artifact]
					
				SELECT @@ROWCOUNT
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Artifact_GetPaged procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Artifact_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Artifact_GetPaged
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the Artifact table passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Artifact_GetPaged
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  ,

	@PageIndex int   ,

	@PageSize int   
)
AS


				
				BEGIN
				DECLARE @PageLowerBound int
				DECLARE @PageUpperBound int
				
				-- Set the page bounds
				SET @PageLowerBound = @PageSize * @PageIndex
				SET @PageUpperBound = @PageLowerBound + @PageSize

				-- Create a temp table to store the select results
				CREATE TABLE #PageIndex
				(
				    [IndexId] int IDENTITY (1, 1) NOT NULL,
				    [ArtifactID] int 
				)
				
				-- Insert into the temp table
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = 'INSERT INTO #PageIndex ([ArtifactID])'
				SET @SQL = @SQL + ' SELECT'
				IF @PageSize > 0
				BEGIN
					SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound)
				END
				SET @SQL = @SQL + ' [ArtifactID]'
				SET @SQL = @SQL + ' FROM [dbo].[Artifact]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Populate the temp table
				EXEC sp_executesql @SQL

				-- Return paged results
				SELECT O.[ArtifactID], O.[CAid], O.[ObjectID], O.[ChildObjectID], O.[ArtifactObjectID], O.[ObjectMachine], O.[ChildObjectMachine], O.[ArtefactMachine]
				FROM
				    [dbo].[Artifact] O,
				    #PageIndex PageIndex
				WHERE
				    PageIndex.IndexId > @PageLowerBound
					AND O.[ArtifactID] = PageIndex.[ArtifactID]
				ORDER BY
				    PageIndex.IndexId
				
				-- get row count
				SET @SQL = 'SELECT COUNT(*) AS TotalRowCount'
				SET @SQL = @SQL + ' FROM [dbo].[Artifact]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				EXEC sp_executesql @SQL
			
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Artifact_Insert procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Artifact_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Artifact_Insert
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Inserts a record into the Artifact table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Artifact_Insert
(

	@ArtifactID int    OUTPUT,

	@CAid int   ,

	@ObjectID int   ,

	@ChildObjectID int   ,

	@ArtifactObjectID int   ,

	@ObjectMachine varchar (50)  ,

	@ChildObjectMachine varchar (50)  ,

	@ArtefactMachine varchar (50)  
)
AS


				
				INSERT INTO [dbo].[Artifact]
					(
					[CAid]
					,[ObjectID]
					,[ChildObjectID]
					,[ArtifactObjectID]
					,[ObjectMachine]
					,[ChildObjectMachine]
					,[ArtefactMachine]
					)
				VALUES
					(
					@CAid
					,@ObjectID
					,@ChildObjectID
					,@ArtifactObjectID
					,@ObjectMachine
					,@ChildObjectMachine
					,@ArtefactMachine
					)
				
				-- Get the identity value
				SET @ArtifactID = SCOPE_IDENTITY()
									
							
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Artifact_Update procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Artifact_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Artifact_Update
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Updates a record in the Artifact table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Artifact_Update
(

	@ArtifactID int   ,

	@CAid int   ,

	@ObjectID int   ,

	@ChildObjectID int   ,

	@ArtifactObjectID int   ,

	@ObjectMachine varchar (50)  ,

	@ChildObjectMachine varchar (50)  ,

	@ArtefactMachine varchar (50)  
)
AS


				
				
				-- Modify the updatable columns
				UPDATE
					[dbo].[Artifact]
				SET
					[CAid] = @CAid
					,[ObjectID] = @ObjectID
					,[ChildObjectID] = @ChildObjectID
					,[ArtifactObjectID] = @ArtifactObjectID
					,[ObjectMachine] = @ObjectMachine
					,[ChildObjectMachine] = @ChildObjectMachine
					,[ArtefactMachine] = @ArtefactMachine
				WHERE
[ArtifactID] = @ArtifactID 
				
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Artifact_Delete procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Artifact_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Artifact_Delete
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Deletes a record in the Artifact table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Artifact_Delete
(

	@ArtifactID int   
)
AS


				DELETE FROM [dbo].[Artifact] WITH (ROWLOCK) 
				WHERE
					[ArtifactID] = @ArtifactID
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Artifact_GetByObjectIDObjectMachine procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Artifact_GetByObjectIDObjectMachine') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Artifact_GetByObjectIDObjectMachine
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the Artifact table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Artifact_GetByObjectIDObjectMachine
(

	@ObjectID int   ,

	@ObjectMachine varchar (50)  
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[ArtifactID],
					[CAid],
					[ObjectID],
					[ChildObjectID],
					[ArtifactObjectID],
					[ObjectMachine],
					[ChildObjectMachine],
					[ArtefactMachine]
				FROM
					[dbo].[Artifact]
				WHERE
					[ObjectID] = @ObjectID
					AND [ObjectMachine] = @ObjectMachine
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Artifact_GetByChildObjectIDChildObjectMachine procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Artifact_GetByChildObjectIDChildObjectMachine') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Artifact_GetByChildObjectIDChildObjectMachine
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the Artifact table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Artifact_GetByChildObjectIDChildObjectMachine
(

	@ChildObjectID int   ,

	@ChildObjectMachine varchar (50)  
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[ArtifactID],
					[CAid],
					[ObjectID],
					[ChildObjectID],
					[ArtifactObjectID],
					[ObjectMachine],
					[ChildObjectMachine],
					[ArtefactMachine]
				FROM
					[dbo].[Artifact]
				WHERE
					[ChildObjectID] = @ChildObjectID
					AND [ChildObjectMachine] = @ChildObjectMachine
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Artifact_GetByArtifactObjectIDArtefactMachine procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Artifact_GetByArtifactObjectIDArtefactMachine') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Artifact_GetByArtifactObjectIDArtefactMachine
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the Artifact table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Artifact_GetByArtifactObjectIDArtefactMachine
(

	@ArtifactObjectID int   ,

	@ArtefactMachine varchar (50)  
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[ArtifactID],
					[CAid],
					[ObjectID],
					[ChildObjectID],
					[ArtifactObjectID],
					[ObjectMachine],
					[ChildObjectMachine],
					[ArtefactMachine]
				FROM
					[dbo].[Artifact]
				WHERE
					[ArtifactObjectID] = @ArtifactObjectID
					AND [ArtefactMachine] = @ArtefactMachine
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Artifact_GetByArtifactID procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Artifact_GetByArtifactID') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Artifact_GetByArtifactID
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the Artifact table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Artifact_GetByArtifactID
(

	@ArtifactID int   
)
AS


				SELECT
					[ArtifactID],
					[CAid],
					[ObjectID],
					[ChildObjectID],
					[ArtifactObjectID],
					[ObjectMachine],
					[ChildObjectMachine],
					[ArtefactMachine]
				FROM
					[dbo].[Artifact]
				WHERE
					[ArtifactID] = @ArtifactID
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Artifact_Find procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Artifact_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Artifact_Find
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Finds records in the Artifact table passing nullable parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Artifact_Find
(

	@SearchUsingOR bit   = null ,

	@ArtifactID int   = null ,

	@CAid int   = null ,

	@ObjectID int   = null ,

	@ChildObjectID int   = null ,

	@ArtifactObjectID int   = null ,

	@ObjectMachine varchar (50)  = null ,

	@ChildObjectMachine varchar (50)  = null ,

	@ArtefactMachine varchar (50)  = null 
)
AS


				
  IF ISNULL(@SearchUsingOR, 0) <> 1
  BEGIN
    SELECT
	  [ArtifactID]
	, [CAid]
	, [ObjectID]
	, [ChildObjectID]
	, [ArtifactObjectID]
	, [ObjectMachine]
	, [ChildObjectMachine]
	, [ArtefactMachine]
    FROM
	[dbo].[Artifact]
    WHERE 
	 ([ArtifactID] = @ArtifactID OR @ArtifactID IS NULL)
	AND ([CAid] = @CAid OR @CAid IS NULL)
	AND ([ObjectID] = @ObjectID OR @ObjectID IS NULL)
	AND ([ChildObjectID] = @ChildObjectID OR @ChildObjectID IS NULL)
	AND ([ArtifactObjectID] = @ArtifactObjectID OR @ArtifactObjectID IS NULL)
	AND ([ObjectMachine] = @ObjectMachine OR @ObjectMachine IS NULL)
	AND ([ChildObjectMachine] = @ChildObjectMachine OR @ChildObjectMachine IS NULL)
	AND ([ArtefactMachine] = @ArtefactMachine OR @ArtefactMachine IS NULL)
						
  END
  ELSE
  BEGIN
    SELECT
	  [ArtifactID]
	, [CAid]
	, [ObjectID]
	, [ChildObjectID]
	, [ArtifactObjectID]
	, [ObjectMachine]
	, [ChildObjectMachine]
	, [ArtefactMachine]
    FROM
	[dbo].[Artifact]
    WHERE 
	 ([ArtifactID] = @ArtifactID AND @ArtifactID is not null)
	OR ([CAid] = @CAid AND @CAid is not null)
	OR ([ObjectID] = @ObjectID AND @ObjectID is not null)
	OR ([ChildObjectID] = @ChildObjectID AND @ChildObjectID is not null)
	OR ([ArtifactObjectID] = @ArtifactObjectID AND @ArtifactObjectID is not null)
	OR ([ObjectMachine] = @ObjectMachine AND @ObjectMachine is not null)
	OR ([ChildObjectMachine] = @ChildObjectMachine AND @ChildObjectMachine is not null)
	OR ([ArtefactMachine] = @ArtefactMachine AND @ArtefactMachine is not null)
	SELECT @@ROWCOUNT			
  END
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Field_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Field_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Field_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the Field table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Field_Get_List

AS


				
				SELECT
					[pkid],
					[Class],
					[Name],
					[DataType],
					[Category],
					[Description],
					[IsUnique],
					[SortOrder],
					[IsActive],
					[Alias]
				FROM
					[dbo].[Field]
					
				SELECT @@ROWCOUNT
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Field_GetPaged procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Field_GetPaged') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Field_GetPaged
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the Field table passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Field_GetPaged
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  ,

	@PageIndex int   ,

	@PageSize int   
)
AS


				
				BEGIN
				DECLARE @PageLowerBound int
				DECLARE @PageUpperBound int
				
				-- Set the page bounds
				SET @PageLowerBound = @PageSize * @PageIndex
				SET @PageUpperBound = @PageLowerBound + @PageSize

				-- Create a temp table to store the select results
				CREATE TABLE #PageIndex
				(
				    [IndexId] int IDENTITY (1, 1) NOT NULL,
				    [pkid] int 
				)
				
				-- Insert into the temp table
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = 'INSERT INTO #PageIndex ([pkid])'
				SET @SQL = @SQL + ' SELECT'
				IF @PageSize > 0
				BEGIN
					SET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar, @PageUpperBound)
				END
				SET @SQL = @SQL + ' [pkid]'
				SET @SQL = @SQL + ' FROM [dbo].[Field]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Populate the temp table
				EXEC sp_executesql @SQL

				-- Return paged results
				SELECT O.[pkid], O.[Class], O.[Name], O.[DataType], O.[Category], O.[Description], O.[IsUnique], O.[SortOrder], O.[IsActive], O.[Alias]
				FROM
				    [dbo].[Field] O,
				    #PageIndex PageIndex
				WHERE
				    PageIndex.IndexId > @PageLowerBound
					AND O.[pkid] = PageIndex.[pkid]
				ORDER BY
				    PageIndex.IndexId
				
				-- get row count
				SET @SQL = 'SELECT COUNT(*) AS TotalRowCount'
				SET @SQL = @SQL + ' FROM [dbo].[Field]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				EXEC sp_executesql @SQL
			
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Field_Insert procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Field_Insert') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Field_Insert
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Inserts a record into the Field table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Field_Insert
(

	@pkid int    OUTPUT,

	@Class varchar (50)  ,

	@Name varchar (50)  ,

	@DataType varchar (50)  ,

	@Category varchar (50)  ,

	@Description varchar (120)  ,

	@IsUnique bit   ,

	@SortOrder int   ,

	@IsActive bit   ,

	@Alias nvarchar (100)  
)
AS


				
				INSERT INTO [dbo].[Field]
					(
					[Class]
					,[Name]
					,[DataType]
					,[Category]
					,[Description]
					,[IsUnique]
					,[SortOrder]
					,[IsActive]
					,[Alias]
					)
				VALUES
					(
					@Class
					,@Name
					,@DataType
					,@Category
					,@Description
					,@IsUnique
					,@SortOrder
					,@IsActive
					,@Alias
					)
				
				-- Get the identity value
				SET @pkid = SCOPE_IDENTITY()
									
							
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Field_Update procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Field_Update') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Field_Update
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Updates a record in the Field table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Field_Update
(

	@pkid int   ,

	@Class varchar (50)  ,

	@Name varchar (50)  ,

	@DataType varchar (50)  ,

	@Category varchar (50)  ,

	@Description varchar (120)  ,

	@IsUnique bit   ,

	@SortOrder int   ,

	@IsActive bit   ,

	@Alias nvarchar (100)  
)
AS


				
				
				-- Modify the updatable columns
				UPDATE
					[dbo].[Field]
				SET
					[Class] = @Class
					,[Name] = @Name
					,[DataType] = @DataType
					,[Category] = @Category
					,[Description] = @Description
					,[IsUnique] = @IsUnique
					,[SortOrder] = @SortOrder
					,[IsActive] = @IsActive
					,[Alias] = @Alias
				WHERE
[pkid] = @pkid 
				
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Field_Delete procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Field_Delete') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Field_Delete
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Deletes a record in the Field table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Field_Delete
(

	@pkid int   
)
AS


				DELETE FROM [dbo].[Field] WITH (ROWLOCK) 
				WHERE
					[pkid] = @pkid
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Field_GetByClass procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Field_GetByClass') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Field_GetByClass
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the Field table through a foreign key
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Field_GetByClass
(

	@Class varchar (50)  
)
AS


				SET ANSI_NULLS OFF
				
				SELECT
					[pkid],
					[Class],
					[Name],
					[DataType],
					[Category],
					[Description],
					[IsUnique],
					[SortOrder],
					[IsActive],
					[Alias]
				FROM
					[dbo].[Field]
				WHERE
					[Class] = @Class
				
				SELECT @@ROWCOUNT
				SET ANSI_NULLS ON
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Field_GetByClassName procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Field_GetByClassName') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Field_GetByClassName
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the Field table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Field_GetByClassName
(

	@Class varchar (50)  ,

	@Name varchar (50)  
)
AS


				SELECT
					[pkid],
					[Class],
					[Name],
					[DataType],
					[Category],
					[Description],
					[IsUnique],
					[SortOrder],
					[IsActive],
					[Alias]
				FROM
					[dbo].[Field]
				WHERE
					[Class] = @Class
					AND [Name] = @Name
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Field_GetBypkid procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Field_GetBypkid') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Field_GetBypkid
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Select records from the Field table through an index
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Field_GetBypkid
(

	@pkid int   
)
AS


				SELECT
					[pkid],
					[Class],
					[Name],
					[DataType],
					[Category],
					[Description],
					[IsUnique],
					[SortOrder],
					[IsActive],
					[Alias]
				FROM
					[dbo].[Field]
				WHERE
					[pkid] = @pkid
				SELECT @@ROWCOUNT
					
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Field_GetByObjectIDMachineIDFromObjectFieldValue procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Field_GetByObjectIDMachineIDFromObjectFieldValue') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Field_GetByObjectIDMachineIDFromObjectFieldValue
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records through a junction table
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Field_GetByObjectIDMachineIDFromObjectFieldValue
(

	@ObjectID int   ,

	@MachineID varchar (50)  
)
AS


SELECT dbo.[Field].[pkid]
       ,dbo.[Field].[Class]
       ,dbo.[Field].[Name]
       ,dbo.[Field].[DataType]
       ,dbo.[Field].[Category]
       ,dbo.[Field].[Description]
       ,dbo.[Field].[IsUnique]
       ,dbo.[Field].[SortOrder]
       ,dbo.[Field].[IsActive]
       ,dbo.[Field].[Alias]
  FROM dbo.[Field]
 WHERE EXISTS (SELECT 1
                 FROM dbo.[ObjectFieldValue] 
                WHERE dbo.[ObjectFieldValue].[ObjectID] = @ObjectID
                  AND dbo.[ObjectFieldValue].[MachineID] = @MachineID
                  AND dbo.[ObjectFieldValue].[FieldID] = dbo.[Field].[pkid]
                  )
				SELECT @@ROWCOUNT			
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_Field_Find procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_Field_Find') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_Field_Find
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Finds records in the Field table passing nullable parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_Field_Find
(

	@SearchUsingOR bit   = null ,

	@pkid int   = null ,

	@Class varchar (50)  = null ,

	@Name varchar (50)  = null ,

	@DataType varchar (50)  = null ,

	@Category varchar (50)  = null ,

	@Description varchar (120)  = null ,

	@IsUnique bit   = null ,

	@SortOrder int   = null ,

	@IsActive bit   = null ,

	@Alias nvarchar (100)  = null 
)
AS


				
  IF ISNULL(@SearchUsingOR, 0) <> 1
  BEGIN
    SELECT
	  [pkid]
	, [Class]
	, [Name]
	, [DataType]
	, [Category]
	, [Description]
	, [IsUnique]
	, [SortOrder]
	, [IsActive]
	, [Alias]
    FROM
	[dbo].[Field]
    WHERE 
	 ([pkid] = @pkid OR @pkid IS NULL)
	AND ([Class] = @Class OR @Class IS NULL)
	AND ([Name] = @Name OR @Name IS NULL)
	AND ([DataType] = @DataType OR @DataType IS NULL)
	AND ([Category] = @Category OR @Category IS NULL)
	AND ([Description] = @Description OR @Description IS NULL)
	AND ([IsUnique] = @IsUnique OR @IsUnique IS NULL)
	AND ([SortOrder] = @SortOrder OR @SortOrder IS NULL)
	AND ([IsActive] = @IsActive OR @IsActive IS NULL)
	AND ([Alias] = @Alias OR @Alias IS NULL)
						
  END
  ELSE
  BEGIN
    SELECT
	  [pkid]
	, [Class]
	, [Name]
	, [DataType]
	, [Category]
	, [Description]
	, [IsUnique]
	, [SortOrder]
	, [IsActive]
	, [Alias]
    FROM
	[dbo].[Field]
    WHERE 
	 ([pkid] = @pkid AND @pkid is not null)
	OR ([Class] = @Class AND @Class is not null)
	OR ([Name] = @Name AND @Name is not null)
	OR ([DataType] = @DataType AND @DataType is not null)
	OR ([Category] = @Category AND @Category is not null)
	OR ([Description] = @Description AND @Description is not null)
	OR ([IsUnique] = @IsUnique AND @IsUnique is not null)
	OR ([SortOrder] = @SortOrder AND @SortOrder is not null)
	OR ([IsActive] = @IsActive AND @IsActive is not null)
	OR ([Alias] = @Alias AND @Alias is not null)
	SELECT @@ROWCOUNT			
  END
				

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ActiveDiagramObjects_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ActiveDiagramObjects_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ActiveDiagramObjects_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the ActiveDiagramObjects view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ActiveDiagramObjects_Get_List

AS


				
				SELECT
					[pkid],
					[File Name],
					[MetaObjectID],
					[Machine]
				FROM
					[dbo].[ActiveDiagramObjects]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ActiveDiagramObjects_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ActiveDiagramObjects_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ActiveDiagramObjects_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the ActiveDiagramObjects view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ActiveDiagramObjects_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[ActiveDiagramObjects]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Activity_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Activity_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Activity_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Activity_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Activity_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[ExecutionIndicator],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale]
				FROM
					[dbo].[METAView_Activity_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Activity_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Activity_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Activity_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Activity_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Activity_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Activity_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Activity_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Activity_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Activity_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Activity_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Activity_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[ExecutionIndicator],
					[ContextualIndicator],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[EnvironmentInd],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[SequenceNumber]
				FROM
					[dbo].[METAView_Activity_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Activity_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Activity_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Activity_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Activity_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Activity_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Activity_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ApplicationInterface_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ApplicationInterface_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ApplicationInterface_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_ApplicationInterface_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ApplicationInterface_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[ExecutionIndicator],
					[InterfaceFrequency],
					[IsPlannedOrAdHoc],
					[RunsDuringWorkingHours],
					[IsSynchronised],
					[InterfaceVolume],
					[Name],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[Criticality],
					[InterfaceProtocol],
					[InterfacePattern],
					[ArchitectureStatus],
					[ArchitectureStatusDate]
				FROM
					[dbo].[METAView_ApplicationInterface_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ApplicationInterface_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ApplicationInterface_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ApplicationInterface_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_ApplicationInterface_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ApplicationInterface_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_ApplicationInterface_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ApplicationInterface_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ApplicationInterface_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ApplicationInterface_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_ApplicationInterface_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ApplicationInterface_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[ExecutionIndicator],
					[InterfaceFrequency],
					[IsPlannedOrAdHoc],
					[RunsDuringWorkingHours],
					[IsSynchronised],
					[InterfaceVolume],
					[Name],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[Criticality],
					[InterfaceProtocol],
					[InterfacePattern],
					[ArchitectureStatus],
					[ArchitectureStatusDate]
				FROM
					[dbo].[METAView_ApplicationInterface_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ApplicationInterface_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ApplicationInterface_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ApplicationInterface_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_ApplicationInterface_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ApplicationInterface_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_ApplicationInterface_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Assumption_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Assumption_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Assumption_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Assumption_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Assumption_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name]
				FROM
					[dbo].[METAView_Assumption_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Assumption_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Assumption_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Assumption_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Assumption_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Assumption_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Assumption_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Assumption_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Assumption_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Assumption_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Assumption_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Assumption_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName]
				FROM
					[dbo].[METAView_Assumption_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Assumption_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Assumption_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Assumption_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Assumption_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Assumption_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Assumption_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Attribute_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Attribute_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Attribute_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Attribute_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Attribute_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Length],
					[GapType],
					[DataType],
					[DomainType],
					[DomainDef],
					[AttributeDescription],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[RulesMaturityRating],
					[DataQualityRating],
					[DataRiskRating],
					[RegulatoryRequirement],
					[Synonym]
				FROM
					[dbo].[METAView_Attribute_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Attribute_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Attribute_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Attribute_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Attribute_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Attribute_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Attribute_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Attribute_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Attribute_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Attribute_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Attribute_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Attribute_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[Length],
					[GapType],
					[DataType],
					[DomainType],
					[DomainDef],
					[AttributeDescription],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[RulesMaturityRating],
					[DataQualityRating],
					[DataRiskRating],
					[RegulatoryRequirement],
					[Synonym]
				FROM
					[dbo].[METAView_Attribute_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Attribute_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Attribute_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Attribute_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Attribute_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Attribute_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Attribute_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_BusinessCompetency_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_BusinessCompetency_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_BusinessCompetency_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_BusinessCompetency_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_BusinessCompetency_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name]
				FROM
					[dbo].[METAView_BusinessCompetency_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_BusinessCompetency_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_BusinessCompetency_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_BusinessCompetency_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_BusinessCompetency_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_BusinessCompetency_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_BusinessCompetency_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_BusinessCompetency_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_BusinessCompetency_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_BusinessCompetency_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_BusinessCompetency_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_BusinessCompetency_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName]
				FROM
					[dbo].[METAView_BusinessCompetency_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_BusinessCompetency_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_BusinessCompetency_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_BusinessCompetency_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_BusinessCompetency_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_BusinessCompetency_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_BusinessCompetency_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_BusinessInterface_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_BusinessInterface_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_BusinessInterface_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_BusinessInterface_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_BusinessInterface_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[ExecutionIndicator],
					[InterfaceFrequency],
					[InterfaceProtocol]
				FROM
					[dbo].[METAView_BusinessInterface_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_BusinessInterface_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_BusinessInterface_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_BusinessInterface_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_BusinessInterface_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_BusinessInterface_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_BusinessInterface_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_BusinessInterface_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_BusinessInterface_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_BusinessInterface_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_BusinessInterface_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_BusinessInterface_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[ExecutionIndicator],
					[InterfaceFrequency],
					[InterfaceProtocol]
				FROM
					[dbo].[METAView_BusinessInterface_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_BusinessInterface_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_BusinessInterface_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_BusinessInterface_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_BusinessInterface_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_BusinessInterface_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_BusinessInterface_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_CAD_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_CAD_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_CAD_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_CAD_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_CAD_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeID],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID]
				FROM
					[dbo].[METAView_CAD_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_CAD_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_CAD_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_CAD_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_CAD_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_CAD_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_CAD_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_CAD_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_CAD_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_CAD_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_CAD_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_CAD_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeID],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID]
				FROM
					[dbo].[METAView_CAD_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_CAD_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_CAD_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_CAD_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_CAD_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_CAD_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_CAD_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_CADReal_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_CADReal_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_CADReal_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_CADReal_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_CADReal_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeID],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID]
				FROM
					[dbo].[METAView_CADReal_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_CADReal_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_CADReal_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_CADReal_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_CADReal_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_CADReal_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_CADReal_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_CADReal_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_CADReal_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_CADReal_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_CADReal_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_CADReal_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeID],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID]
				FROM
					[dbo].[METAView_CADReal_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_CADReal_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_CADReal_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_CADReal_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_CADReal_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_CADReal_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_CADReal_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_CategoryFactor_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_CategoryFactor_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_CategoryFactor_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_CategoryFactor_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_CategoryFactor_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_CategoryFactor_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_CategoryFactor_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_CategoryFactor_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_CategoryFactor_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_CategoryFactor_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_CategoryFactor_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_CategoryFactor_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_CategoryFactor_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_CategoryFactor_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_CategoryFactor_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_CategoryFactor_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_CategoryFactor_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_CategoryFactor_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_CategoryFactor_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_CategoryFactor_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_CategoryFactor_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_CategoryFactor_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_CategoryFactor_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_CategoryFactor_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Channel_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Channel_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Channel_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Channel_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Channel_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName]
				FROM
					[dbo].[METAView_Channel_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Channel_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Channel_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Channel_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Channel_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Channel_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Channel_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Channel_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Channel_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Channel_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Channel_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Channel_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName]
				FROM
					[dbo].[METAView_Channel_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Channel_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Channel_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Channel_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Channel_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Channel_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Channel_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Competency_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Competency_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Competency_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Competency_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Competency_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Type],
					[Level],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_Competency_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Competency_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Competency_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Competency_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Competency_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Competency_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Competency_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Competency_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Competency_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Competency_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Competency_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Competency_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Type],
					[Level],
					[GapType],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_Competency_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Competency_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Competency_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Competency_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Competency_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Competency_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Competency_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ComputingComponent_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ComputingComponent_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ComputingComponent_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_ComputingComponent_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ComputingComponent_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[ComputingComponentType],
					[ServerType],
					[SeverityRating],
					[ConfigurationID],
					[Make],
					[Model],
					[ModelNumber],
					[SerialNumber],
					[AssetNumber],
					[DatePurchased],
					[isManaged],
					[ContractNumber],
					[UnderWarranty],
					[NetworkAddress1],
					[NetworkAddress2],
					[NetworkAddress3],
					[NetworkAddress4],
					[NetworkAddress5],
					[IsDNS],
					[IsDHCP],
					[Domain],
					[Capacity],
					[NumberofDisks],
					[CPUType],
					[CPUSpeed],
					[Monitor],
					[VideoCard],
					[MemoryTotal],
					[Name],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[GapType],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale]
				FROM
					[dbo].[METAView_ComputingComponent_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ComputingComponent_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ComputingComponent_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ComputingComponent_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_ComputingComponent_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ComputingComponent_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_ComputingComponent_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ComputingComponent_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ComputingComponent_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ComputingComponent_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_ComputingComponent_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ComputingComponent_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[ComputingComponentType],
					[ServerType],
					[SeverityRating],
					[ConfigurationID],
					[Make],
					[Model],
					[ModelNumber],
					[SerialNumber],
					[AssetNumber],
					[DatePurchased],
					[isManaged],
					[ContractNumber],
					[UnderWarranty],
					[NetworkAddress1],
					[NetworkAddress2],
					[NetworkAddress3],
					[NetworkAddress4],
					[NetworkAddress5],
					[IsDNS],
					[IsDHCP],
					[Domain],
					[Capacity],
					[NumberofDisks],
					[CPUType],
					[CPUSpeed],
					[Monitor],
					[VideoCard],
					[MemoryTotal],
					[Type],
					[Configuration],
					[MACAddress],
					[StaticIP],
					[Number_Of_Disks],
					[CPU_Type],
					[CPU_Speed],
					[Video_Card],
					[Mem_Total],
					[Name],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[GapType],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale]
				FROM
					[dbo].[METAView_ComputingComponent_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ComputingComponent_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ComputingComponent_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ComputingComponent_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_ComputingComponent_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ComputingComponent_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_ComputingComponent_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Condition_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Condition_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Condition_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Condition_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Condition_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[GapType],
					[Value]
				FROM
					[dbo].[METAView_Condition_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Condition_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Condition_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Condition_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Condition_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Condition_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Condition_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Condition_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Condition_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Condition_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Condition_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Condition_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[Value]
				FROM
					[dbo].[METAView_Condition_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Condition_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Condition_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Condition_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Condition_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Condition_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Condition_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Conditional_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Conditional_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Conditional_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Conditional_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Conditional_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[ConditionalType],
					[GapType],
					[Name]
				FROM
					[dbo].[METAView_Conditional_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Conditional_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Conditional_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Conditional_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Conditional_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Conditional_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Conditional_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Conditional_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Conditional_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Conditional_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Conditional_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Conditional_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[ConditionalType],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[Name]
				FROM
					[dbo].[METAView_Conditional_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Conditional_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Conditional_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Conditional_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Conditional_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Conditional_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Conditional_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ConditionalDescription_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ConditionalDescription_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ConditionalDescription_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_ConditionalDescription_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ConditionalDescription_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[GapType],
					[Sequence],
					[Value]
				FROM
					[dbo].[METAView_ConditionalDescription_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ConditionalDescription_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ConditionalDescription_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ConditionalDescription_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_ConditionalDescription_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ConditionalDescription_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_ConditionalDescription_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ConditionalDescription_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ConditionalDescription_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ConditionalDescription_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_ConditionalDescription_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ConditionalDescription_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[Sequence],
					[Value]
				FROM
					[dbo].[METAView_ConditionalDescription_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ConditionalDescription_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ConditionalDescription_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ConditionalDescription_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_ConditionalDescription_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ConditionalDescription_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_ConditionalDescription_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ConnectionSize_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ConnectionSize_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ConnectionSize_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_ConnectionSize_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ConnectionSize_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name]
				FROM
					[dbo].[METAView_ConnectionSize_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ConnectionSize_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ConnectionSize_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ConnectionSize_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_ConnectionSize_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ConnectionSize_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_ConnectionSize_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ConnectionSize_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ConnectionSize_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ConnectionSize_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_ConnectionSize_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ConnectionSize_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName]
				FROM
					[dbo].[METAView_ConnectionSize_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ConnectionSize_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ConnectionSize_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ConnectionSize_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_ConnectionSize_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ConnectionSize_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_ConnectionSize_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ConnectionSpeed_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ConnectionSpeed_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ConnectionSpeed_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_ConnectionSpeed_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ConnectionSpeed_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Value]
				FROM
					[dbo].[METAView_ConnectionSpeed_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ConnectionSpeed_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ConnectionSpeed_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ConnectionSpeed_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_ConnectionSpeed_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ConnectionSpeed_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_ConnectionSpeed_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ConnectionSpeed_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ConnectionSpeed_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ConnectionSpeed_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_ConnectionSpeed_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ConnectionSpeed_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Value],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[DataSourceID],
					[DataSourceName]
				FROM
					[dbo].[METAView_ConnectionSpeed_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ConnectionSpeed_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ConnectionSpeed_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ConnectionSpeed_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_ConnectionSpeed_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ConnectionSpeed_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_ConnectionSpeed_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ConnectionType_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ConnectionType_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ConnectionType_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_ConnectionType_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ConnectionType_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Value]
				FROM
					[dbo].[METAView_ConnectionType_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ConnectionType_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ConnectionType_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ConnectionType_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_ConnectionType_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ConnectionType_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_ConnectionType_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ConnectionType_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ConnectionType_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ConnectionType_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_ConnectionType_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ConnectionType_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Value],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[DataSourceID],
					[DataSourceName]
				FROM
					[dbo].[METAView_ConnectionType_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ConnectionType_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ConnectionType_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ConnectionType_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_ConnectionType_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ConnectionType_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_ConnectionType_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Constraint_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Constraint_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Constraint_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Constraint_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Constraint_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name]
				FROM
					[dbo].[METAView_Constraint_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Constraint_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Constraint_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Constraint_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Constraint_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Constraint_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Constraint_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Constraint_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Constraint_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Constraint_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Constraint_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Constraint_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName]
				FROM
					[dbo].[METAView_Constraint_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Constraint_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Constraint_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Constraint_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Constraint_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Constraint_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Constraint_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_CSF_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_CSF_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_CSF_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_CSF_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_CSF_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Number],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_CSF_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_CSF_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_CSF_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_CSF_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_CSF_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_CSF_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_CSF_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_CSF_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_CSF_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_CSF_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_CSF_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_CSF_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Number],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_CSF_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_CSF_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_CSF_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_CSF_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_CSF_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_CSF_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_CSF_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataAttribute_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataAttribute_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataAttribute_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_DataAttribute_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataAttribute_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[DataType],
					[DataLength],
					[ContentType],
					[IsFactOrMeasure],
					[IsDerived]
				FROM
					[dbo].[METAView_DataAttribute_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataAttribute_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataAttribute_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataAttribute_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_DataAttribute_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataAttribute_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_DataAttribute_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataAttribute_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataAttribute_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataAttribute_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_DataAttribute_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataAttribute_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[DataType],
					[DataLength],
					[ContentType],
					[Value],
					[Length],
					[IsFactOrMeasure],
					[IsDerived],
					[DomainAttributeType],
					[DomainDef],
					[AttributeDescription]
				FROM
					[dbo].[METAView_DataAttribute_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataAttribute_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataAttribute_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataAttribute_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_DataAttribute_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataAttribute_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_DataAttribute_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataColumn_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataColumn_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataColumn_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_DataColumn_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataColumn_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[DomainDef],
					[ColumnType],
					[ColumnLength],
					[GapType],
					[DataType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[NumberofValues],
					[NullValuePercentage],
					[ConsistencyPercentage]
				FROM
					[dbo].[METAView_DataColumn_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataColumn_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataColumn_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataColumn_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_DataColumn_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataColumn_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_DataColumn_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataColumn_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataColumn_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataColumn_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_DataColumn_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataColumn_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[DomainDef],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[ColumnType],
					[ColumnLength],
					[GapType],
					[DataType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[NumberofValues],
					[NullValuePercentage],
					[ConsistencyPercentage]
				FROM
					[dbo].[METAView_DataColumn_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataColumn_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataColumn_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataColumn_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_DataColumn_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataColumn_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_DataColumn_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataDomain_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataDomain_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataDomain_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_DataDomain_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataDomain_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[DataDomainType]
				FROM
					[dbo].[METAView_DataDomain_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataDomain_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataDomain_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataDomain_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_DataDomain_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataDomain_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_DataDomain_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataDomain_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataDomain_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataDomain_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_DataDomain_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataDomain_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[DataDomainType]
				FROM
					[dbo].[METAView_DataDomain_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataDomain_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataDomain_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataDomain_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_DataDomain_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataDomain_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_DataDomain_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataEntity_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataEntity_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataEntity_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_DataEntity_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataEntity_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[DataEntityType],
					[ContentType]
				FROM
					[dbo].[METAView_DataEntity_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataEntity_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataEntity_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataEntity_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_DataEntity_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataEntity_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_DataEntity_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataEntity_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataEntity_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataEntity_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_DataEntity_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataEntity_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[DataEntityType],
					[EntityType],
					[ContentType],
					[DataType],
					[EntityDescription]
				FROM
					[dbo].[METAView_DataEntity_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataEntity_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataEntity_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataEntity_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_DataEntity_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataEntity_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_DataEntity_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataField_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataField_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataField_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_DataField_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataField_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[DataType],
					[DataLength],
					[ContentType],
					[IsFactOrMeasure],
					[IsRequired],
					[IsPrimary],
					[IsStaticOrDynamic],
					[IsIndexed],
					[DataFieldRole]
				FROM
					[dbo].[METAView_DataField_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataField_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataField_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataField_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_DataField_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataField_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_DataField_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataField_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataField_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataField_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_DataField_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataField_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[DataType],
					[DataLength],
					[ColumnLength],
					[ContentType],
					[DomainDef],
					[ColumnType],
					[IsFactOrMeasure],
					[IsRequired],
					[IsPrimary],
					[IsStaticOrDynamic],
					[IsIndexed],
					[DataFieldRole]
				FROM
					[dbo].[METAView_DataField_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataField_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataField_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataField_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_DataField_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataField_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_DataField_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataFlowIndicator_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataFlowIndicator_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataFlowIndicator_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_DataFlowIndicator_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataFlowIndicator_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID]
				FROM
					[dbo].[METAView_DataFlowIndicator_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataFlowIndicator_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataFlowIndicator_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataFlowIndicator_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_DataFlowIndicator_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataFlowIndicator_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_DataFlowIndicator_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataFlowIndicator_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataFlowIndicator_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataFlowIndicator_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_DataFlowIndicator_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataFlowIndicator_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID]
				FROM
					[dbo].[METAView_DataFlowIndicator_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataFlowIndicator_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataFlowIndicator_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataFlowIndicator_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_DataFlowIndicator_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataFlowIndicator_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_DataFlowIndicator_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataSchema_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataSchema_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataSchema_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_DataSchema_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataSchema_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[DataSchemaType],
					[ArchPriority],
					[DatabaseType],
					[GapType],
					[AbbreviatedName],
					[DataType],
					[DataSchemaDescription],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_DataSchema_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataSchema_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataSchema_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataSchema_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_DataSchema_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataSchema_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_DataSchema_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataSchema_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataSchema_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataSchema_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_DataSchema_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataSchema_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[DataSchemaType],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[ArchPriority],
					[DatabaseType],
					[GapType],
					[AbbreviatedName],
					[DataType],
					[DataSchemaDescription],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_DataSchema_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataSchema_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataSchema_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataSchema_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_DataSchema_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataSchema_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_DataSchema_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataSubjectArea_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataSubjectArea_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataSubjectArea_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_DataSubjectArea_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataSubjectArea_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[DataSubjectAreaType],
					[ArchitecturalPriority],
					[ContentType]
				FROM
					[dbo].[METAView_DataSubjectArea_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataSubjectArea_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataSubjectArea_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataSubjectArea_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_DataSubjectArea_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataSubjectArea_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_DataSubjectArea_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataSubjectArea_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataSubjectArea_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataSubjectArea_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_DataSubjectArea_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataSubjectArea_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[AbbreviatedName],
					[DataSubjectAreaType],
					[DataSchemaType],
					[ArchitecturalPriority],
					[ArchPriority],
					[DatabaseType],
					[ContentType],
					[DataType]
				FROM
					[dbo].[METAView_DataSubjectArea_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataSubjectArea_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataSubjectArea_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataSubjectArea_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_DataSubjectArea_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataSubjectArea_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_DataSubjectArea_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataTable_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataTable_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataTable_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_DataTable_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataTable_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[InitialPopulation],
					[GrowthRateOverTime],
					[RecordSize],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[ContentType]
				FROM
					[dbo].[METAView_DataTable_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataTable_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataTable_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataTable_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_DataTable_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataTable_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_DataTable_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataTable_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataTable_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataTable_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_DataTable_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataTable_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[InitialPopulation],
					[GrowthRateOverTime],
					[RecordSize],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[DataType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[ContentType]
				FROM
					[dbo].[METAView_DataTable_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataTable_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataTable_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataTable_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_DataTable_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataTable_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_DataTable_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataValue_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataValue_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataValue_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_DataValue_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataValue_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[IsDefaultValue]
				FROM
					[dbo].[METAView_DataValue_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataValue_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataValue_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataValue_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_DataValue_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataValue_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_DataValue_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataValue_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataValue_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataValue_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_DataValue_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataValue_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[IsDefaultValue]
				FROM
					[dbo].[METAView_DataValue_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataValue_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataValue_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataValue_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_DataValue_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataValue_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_DataValue_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataView_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataView_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataView_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_DataView_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataView_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[DataViewType],
					[GapType],
					[DataType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_DataView_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataView_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataView_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataView_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_DataView_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataView_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_DataView_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataView_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataView_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataView_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_DataView_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataView_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[DataViewType],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[DataType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_DataView_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DataView_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DataView_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DataView_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_DataView_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DataView_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_DataView_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DatedResponsibility_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DatedResponsibility_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DatedResponsibility_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_DatedResponsibility_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DatedResponsibility_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeID],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID]
				FROM
					[dbo].[METAView_DatedResponsibility_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DatedResponsibility_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DatedResponsibility_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DatedResponsibility_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_DatedResponsibility_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DatedResponsibility_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_DatedResponsibility_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DatedResponsibility_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DatedResponsibility_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DatedResponsibility_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_DatedResponsibility_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DatedResponsibility_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeID],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID]
				FROM
					[dbo].[METAView_DatedResponsibility_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DatedResponsibility_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DatedResponsibility_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DatedResponsibility_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_DatedResponsibility_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DatedResponsibility_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_DatedResponsibility_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DependencyDescription_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DependencyDescription_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DependencyDescription_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_DependencyDescription_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DependencyDescription_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[InferenceRule],
					[Condition],
					[Description],
					[CohesionWeight],
					[Name]
				FROM
					[dbo].[METAView_DependencyDescription_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DependencyDescription_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DependencyDescription_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DependencyDescription_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_DependencyDescription_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DependencyDescription_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_DependencyDescription_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DependencyDescription_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DependencyDescription_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DependencyDescription_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_DependencyDescription_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DependencyDescription_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[InferenceRule],
					[Condition],
					[Description],
					[CohesionWeight],
					[Name],
					[Abbreviation],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName]
				FROM
					[dbo].[METAView_DependencyDescription_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_DependencyDescription_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_DependencyDescription_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_DependencyDescription_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_DependencyDescription_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_DependencyDescription_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_DependencyDescription_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Driver_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Driver_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Driver_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Driver_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Driver_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[EnvironmentCategory]
				FROM
					[dbo].[METAView_Driver_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Driver_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Driver_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Driver_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Driver_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Driver_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Driver_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Driver_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Driver_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Driver_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Driver_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Driver_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[EnvironmentCategory]
				FROM
					[dbo].[METAView_Driver_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Driver_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Driver_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Driver_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Driver_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Driver_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Driver_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Employee_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Employee_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Employee_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Employee_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Employee_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Surname],
					[Title],
					[EmployeeNumber],
					[IDNumber],
					[Email],
					[Telephone],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_Employee_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Employee_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Employee_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Employee_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Employee_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Employee_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Employee_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Employee_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Employee_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Employee_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Employee_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Employee_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Surname],
					[Title],
					[EmployeeNumber],
					[IDNumber],
					[Email],
					[Telephone],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[Mobile],
					[Fax],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_Employee_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Employee_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Employee_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Employee_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Employee_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Employee_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Employee_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Entity_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Entity_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Entity_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Entity_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Entity_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[EntityType],
					[GapType],
					[DataType],
					[EntityDescription],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[Synonym]
				FROM
					[dbo].[METAView_Entity_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Entity_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Entity_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Entity_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Entity_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Entity_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Entity_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Entity_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Entity_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Entity_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Entity_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Entity_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[EntityType],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[DataType],
					[EntityDescription],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[Synonym]
				FROM
					[dbo].[METAView_Entity_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Entity_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Entity_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Entity_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Entity_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Entity_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Entity_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Environment_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Environment_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Environment_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Environment_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Environment_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name]
				FROM
					[dbo].[METAView_Environment_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Environment_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Environment_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Environment_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Environment_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Environment_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Environment_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Environment_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Environment_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Environment_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Environment_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Environment_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale]
				FROM
					[dbo].[METAView_Environment_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Environment_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Environment_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Environment_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Environment_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Environment_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Environment_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_EnvironmentCategory_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_EnvironmentCategory_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_EnvironmentCategory_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_EnvironmentCategory_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_EnvironmentCategory_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name]
				FROM
					[dbo].[METAView_EnvironmentCategory_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_EnvironmentCategory_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_EnvironmentCategory_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_EnvironmentCategory_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_EnvironmentCategory_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_EnvironmentCategory_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_EnvironmentCategory_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_EnvironmentCategory_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_EnvironmentCategory_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_EnvironmentCategory_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_EnvironmentCategory_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_EnvironmentCategory_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale]
				FROM
					[dbo].[METAView_EnvironmentCategory_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_EnvironmentCategory_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_EnvironmentCategory_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_EnvironmentCategory_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_EnvironmentCategory_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_EnvironmentCategory_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_EnvironmentCategory_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Event_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Event_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Event_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Event_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Event_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[EventType]
				FROM
					[dbo].[METAView_Event_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Event_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Event_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Event_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Event_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Event_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Event_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Event_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Event_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Event_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Event_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Event_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[EventType]
				FROM
					[dbo].[METAView_Event_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Event_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Event_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Event_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Event_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Event_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Event_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Exception_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Exception_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Exception_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Exception_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Exception_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_Exception_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Exception_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Exception_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Exception_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Exception_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Exception_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Exception_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Exception_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Exception_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Exception_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Exception_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Exception_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_Exception_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Exception_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Exception_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Exception_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Exception_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Exception_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Exception_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ExtractionRule_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ExtractionRule_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ExtractionRule_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_ExtractionRule_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ExtractionRule_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID]
				FROM
					[dbo].[METAView_ExtractionRule_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ExtractionRule_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ExtractionRule_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ExtractionRule_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_ExtractionRule_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ExtractionRule_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_ExtractionRule_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ExtractionRule_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ExtractionRule_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ExtractionRule_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_ExtractionRule_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ExtractionRule_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID]
				FROM
					[dbo].[METAView_ExtractionRule_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ExtractionRule_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ExtractionRule_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ExtractionRule_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_ExtractionRule_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ExtractionRule_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_ExtractionRule_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_FlowDescription_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_FlowDescription_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_FlowDescription_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_FlowDescription_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_FlowDescription_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Sequence],
					[Condition],
					[Description],
					[TimeReference]
				FROM
					[dbo].[METAView_FlowDescription_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_FlowDescription_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_FlowDescription_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_FlowDescription_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_FlowDescription_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_FlowDescription_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_FlowDescription_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_FlowDescription_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_FlowDescription_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_FlowDescription_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_FlowDescription_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_FlowDescription_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Sequence],
					[Condition],
					[Description],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[TimeIndicator],
					[GapType],
					[Name],
					[Abbreviation],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[DataSourceID],
					[DataSourceName],
					[TimeReference]
				FROM
					[dbo].[METAView_FlowDescription_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_FlowDescription_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_FlowDescription_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_FlowDescription_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_FlowDescription_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_FlowDescription_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_FlowDescription_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Function_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Function_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Function_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Function_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Function_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[FunctionCriticality],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[FunctionCategory],
					[OverallEfficiencyRating],
					[OverallEffectivenessRating]
				FROM
					[dbo].[METAView_Function_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Function_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Function_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Function_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Function_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Function_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Function_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Function_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Function_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Function_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Function_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Function_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[ContextualIndicator],
					[FunctionCriticality],
					[ManagementAbility],
					[InfoAvailability],
					[LegalAspects],
					[Technology],
					[Budget],
					[Energy],
					[RawMaterial],
					[SkillAvailability],
					[Efficiency],
					[Effectiveness],
					[Manpower],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[EnvironmentInd],
					[GovernanceMech],
					[CapitalAvailability],
					[Competencies],
					[Ethics],
					[Facilities],
					[ServicesUSage],
					[Equipment],
					[TimeIndicator],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[FunctionCategory],
					[OverallEfficiencyRating],
					[OverallEffectivenessRating]
				FROM
					[dbo].[METAView_Function_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Function_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Function_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Function_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Function_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Function_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Function_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_FunctionalDependency_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_FunctionalDependency_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_FunctionalDependency_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_FunctionalDependency_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_FunctionalDependency_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[InferenceRule],
					[Condition],
					[Description],
					[CohesionWeight],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType]
				FROM
					[dbo].[METAView_FunctionalDependency_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_FunctionalDependency_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_FunctionalDependency_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_FunctionalDependency_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_FunctionalDependency_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_FunctionalDependency_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_FunctionalDependency_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_FunctionalDependency_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_FunctionalDependency_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_FunctionalDependency_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_FunctionalDependency_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_FunctionalDependency_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[InferenceRule],
					[Condition],
					[Description],
					[CohesionWeight],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType]
				FROM
					[dbo].[METAView_FunctionalDependency_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_FunctionalDependency_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_FunctionalDependency_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_FunctionalDependency_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_FunctionalDependency_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_FunctionalDependency_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_FunctionalDependency_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Gateway_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Gateway_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Gateway_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Gateway_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Gateway_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[GatewayType]
				FROM
					[dbo].[METAView_Gateway_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Gateway_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Gateway_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Gateway_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Gateway_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Gateway_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Gateway_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Gateway_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Gateway_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Gateway_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Gateway_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Gateway_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[GatewayType]
				FROM
					[dbo].[METAView_Gateway_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Gateway_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Gateway_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Gateway_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Gateway_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Gateway_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Gateway_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_GatewayDescription_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_GatewayDescription_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_GatewayDescription_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_GatewayDescription_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_GatewayDescription_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name]
				FROM
					[dbo].[METAView_GatewayDescription_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_GatewayDescription_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_GatewayDescription_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_GatewayDescription_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_GatewayDescription_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_GatewayDescription_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_GatewayDescription_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_GatewayDescription_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_GatewayDescription_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_GatewayDescription_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_GatewayDescription_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_GatewayDescription_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[GatewayValue],
					[Value],
					[GatewaySequenceNumber],
					[Sequence]
				FROM
					[dbo].[METAView_GatewayDescription_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_GatewayDescription_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_GatewayDescription_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_GatewayDescription_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_GatewayDescription_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_GatewayDescription_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_GatewayDescription_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_GovernanceMechanism_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_GovernanceMechanism_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_GovernanceMechanism_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_GovernanceMechanism_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_GovernanceMechanism_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[UniqueRef],
					[Description],
					[GapType],
					[Name],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GovernanceMechanismType],
					[EffectiveStartDate]
				FROM
					[dbo].[METAView_GovernanceMechanism_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_GovernanceMechanism_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_GovernanceMechanism_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_GovernanceMechanism_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_GovernanceMechanism_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_GovernanceMechanism_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_GovernanceMechanism_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_GovernanceMechanism_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_GovernanceMechanism_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_GovernanceMechanism_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_GovernanceMechanism_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_GovernanceMechanism_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[EnvironmentInd],
					[GovernanceMechType],
					[UniqueRef],
					[ValidityPeriod],
					[Description],
					[GapType],
					[Name],
					[MeasurementValue],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GovernanceMechanismType],
					[EffectiveStartDate]
				FROM
					[dbo].[METAView_GovernanceMechanism_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_GovernanceMechanism_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_GovernanceMechanism_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_GovernanceMechanism_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_GovernanceMechanism_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_GovernanceMechanism_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_GovernanceMechanism_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Implication_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Implication_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Implication_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Implication_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Implication_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[ExternalInfluenceIndicator],
					[InternalCapabilityIndicator]
				FROM
					[dbo].[METAView_Implication_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Implication_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Implication_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Implication_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Implication_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Implication_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Implication_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Implication_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Implication_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Implication_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Implication_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Implication_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Ext_Inf_Indicator],
					[Int_Capability_Indicator],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[ExternalInfluenceIndicator],
					[InternalCapabilityIndicator]
				FROM
					[dbo].[METAView_Implication_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Implication_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Implication_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Implication_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Implication_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Implication_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Implication_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Iteration_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Iteration_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Iteration_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Iteration_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Iteration_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[IterationType],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_Iteration_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Iteration_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Iteration_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Iteration_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Iteration_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Iteration_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Iteration_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Iteration_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Iteration_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Iteration_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Iteration_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Iteration_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[IterationType],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_Iteration_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Iteration_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Iteration_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Iteration_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Iteration_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Iteration_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Iteration_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ITInfrastructureEnvironment_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ITInfrastructureEnvironment_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ITInfrastructureEnvironment_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_ITInfrastructureEnvironment_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ITInfrastructureEnvironment_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[ITEnvironmentType],
					[Name]
				FROM
					[dbo].[METAView_ITInfrastructureEnvironment_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ITInfrastructureEnvironment_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ITInfrastructureEnvironment_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ITInfrastructureEnvironment_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_ITInfrastructureEnvironment_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ITInfrastructureEnvironment_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_ITInfrastructureEnvironment_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ITInfrastructureEnvironment_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ITInfrastructureEnvironment_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ITInfrastructureEnvironment_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_ITInfrastructureEnvironment_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ITInfrastructureEnvironment_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[ITEnvironmentType],
					[Name],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[GapType],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale]
				FROM
					[dbo].[METAView_ITInfrastructureEnvironment_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ITInfrastructureEnvironment_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ITInfrastructureEnvironment_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ITInfrastructureEnvironment_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_ITInfrastructureEnvironment_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ITInfrastructureEnvironment_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_ITInfrastructureEnvironment_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Job_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Job_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Job_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Job_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Job_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[GapType],
					[TotalRequired],
					[TotalOccupied],
					[TotalAvailable],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[JobType],
					[JobUniqueCode],
					[SalaryGrade]
				FROM
					[dbo].[METAView_Job_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Job_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Job_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Job_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Job_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Job_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Job_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Job_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Job_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Job_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Job_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Job_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Type],
					[GapType],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[TotalRequired],
					[TotalOccupied],
					[TotalAvailable],
					[Timestamp],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[JobType],
					[JobUniqueCode],
					[SalaryGrade]
				FROM
					[dbo].[METAView_Job_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Job_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Job_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Job_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Job_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Job_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Job_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_JobCompetency_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_JobCompetency_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_JobCompetency_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_JobCompetency_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_JobCompetency_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[JobCompetencyType],
					[JobCompetencyLevel]
				FROM
					[dbo].[METAView_JobCompetency_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_JobCompetency_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_JobCompetency_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_JobCompetency_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_JobCompetency_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_JobCompetency_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_JobCompetency_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_JobCompetency_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_JobCompetency_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_JobCompetency_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_JobCompetency_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_JobCompetency_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[JobCompetencyType],
					[Type],
					[JobCompetencyLevel],
					[Level]
				FROM
					[dbo].[METAView_JobCompetency_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_JobCompetency_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_JobCompetency_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_JobCompetency_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_JobCompetency_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_JobCompetency_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_JobCompetency_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_JobPosition_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_JobPosition_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_JobPosition_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_JobPosition_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_JobPosition_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_JobPosition_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_JobPosition_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_JobPosition_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_JobPosition_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_JobPosition_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_JobPosition_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_JobPosition_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_JobPosition_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_JobPosition_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_JobPosition_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_JobPosition_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_JobPosition_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[TotalRequired],
					[TotalOccupied],
					[TotalAvailable],
					[TimeStamp],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_JobPosition_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_JobPosition_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_JobPosition_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_JobPosition_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_JobPosition_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_JobPosition_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_JobPosition_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Location_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Location_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Location_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Location_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Location_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[LocationType],
					[Address],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[TelephoneNumber],
					[FaxNumber],
					[EmailAddress]
				FROM
					[dbo].[METAView_Location_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Location_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Location_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Location_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Location_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Location_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Location_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Location_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Location_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Location_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Location_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Location_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[LocationType],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[Address],
					[Telephone],
					[Fax],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[TelephoneNumber],
					[FaxNumber],
					[EmailAddress]
				FROM
					[dbo].[METAView_Location_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Location_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Location_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Location_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Location_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Location_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Location_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_LocationAssociation_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_LocationAssociation_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_LocationAssociation_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_LocationAssociation_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_LocationAssociation_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[DistanceBetweenLocations],
					[TravelMethod],
					[TimetoTravel],
					[TravelFrequency]
				FROM
					[dbo].[METAView_LocationAssociation_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_LocationAssociation_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_LocationAssociation_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_LocationAssociation_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_LocationAssociation_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_LocationAssociation_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_LocationAssociation_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_LocationAssociation_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_LocationAssociation_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_LocationAssociation_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_LocationAssociation_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_LocationAssociation_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Distance],
					[TimeIndicator],
					[AssociationType],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[DataSourceID],
					[DataSourceName],
					[DistanceBetweenLocations],
					[TravelMethod],
					[TimetoTravel],
					[TravelFrequency]
				FROM
					[dbo].[METAView_LocationAssociation_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_LocationAssociation_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_LocationAssociation_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_LocationAssociation_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_LocationAssociation_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_LocationAssociation_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_LocationAssociation_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_LocationScheme_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_LocationScheme_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_LocationScheme_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_LocationScheme_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_LocationScheme_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale]
				FROM
					[dbo].[METAView_LocationScheme_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_LocationScheme_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_LocationScheme_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_LocationScheme_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_LocationScheme_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_LocationScheme_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_LocationScheme_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_LocationScheme_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_LocationScheme_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_LocationScheme_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_LocationScheme_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_LocationScheme_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[GapType],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale]
				FROM
					[dbo].[METAView_LocationScheme_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_LocationScheme_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_LocationScheme_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_LocationScheme_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_LocationScheme_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_LocationScheme_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_LocationScheme_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_LocationUnit_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_LocationUnit_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_LocationUnit_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_LocationUnit_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_LocationUnit_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Format],
					[LocationUnitType],
					[LocationDomainValues]
				FROM
					[dbo].[METAView_LocationUnit_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_LocationUnit_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_LocationUnit_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_LocationUnit_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_LocationUnit_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_LocationUnit_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_LocationUnit_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_LocationUnit_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_LocationUnit_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_LocationUnit_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_LocationUnit_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_LocationUnit_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[DomainValue],
					[Format],
					[GapType],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[LocationUnitType],
					[LocationDomainValues]
				FROM
					[dbo].[METAView_LocationUnit_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_LocationUnit_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_LocationUnit_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_LocationUnit_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_LocationUnit_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_LocationUnit_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_LocationUnit_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Logic_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Logic_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Logic_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Logic_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Logic_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Description],
					[GapType],
					[Name],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_Logic_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Logic_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Logic_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Logic_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Logic_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Logic_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Logic_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Logic_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Logic_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Logic_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Logic_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Logic_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Description],
					[GapType],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[Name],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_Logic_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Logic_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Logic_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Logic_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Logic_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Logic_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Logic_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_LogicalInformationArtefact_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_LogicalInformationArtefact_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_LogicalInformationArtefact_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_LogicalInformationArtefact_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_LogicalInformationArtefact_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[ArchitectureStatus],
					[ArchitectureStatusDate]
				FROM
					[dbo].[METAView_LogicalInformationArtefact_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_LogicalInformationArtefact_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_LogicalInformationArtefact_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_LogicalInformationArtefact_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_LogicalInformationArtefact_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_LogicalInformationArtefact_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_LogicalInformationArtefact_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_LogicalInformationArtefact_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_LogicalInformationArtefact_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_LogicalInformationArtefact_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_LogicalInformationArtefact_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_LogicalInformationArtefact_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[ArchitectureStatus],
					[ArchitectureStatusDate]
				FROM
					[dbo].[METAView_LogicalInformationArtefact_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_LogicalInformationArtefact_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_LogicalInformationArtefact_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_LogicalInformationArtefact_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_LogicalInformationArtefact_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_LogicalInformationArtefact_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_LogicalInformationArtefact_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_LogicalITInfrastructureComponent_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_LogicalITInfrastructureComponent_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_LogicalITInfrastructureComponent_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_LogicalITInfrastructureComponent_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_LogicalITInfrastructureComponent_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[GapType],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale]
				FROM
					[dbo].[METAView_LogicalITInfrastructureComponent_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_LogicalITInfrastructureComponent_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_LogicalITInfrastructureComponent_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_LogicalITInfrastructureComponent_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_LogicalITInfrastructureComponent_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_LogicalITInfrastructureComponent_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_LogicalITInfrastructureComponent_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_LogicalITInfrastructureComponent_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_LogicalITInfrastructureComponent_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_LogicalITInfrastructureComponent_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_LogicalITInfrastructureComponent_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_LogicalITInfrastructureComponent_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[GapType],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale]
				FROM
					[dbo].[METAView_LogicalITInfrastructureComponent_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_LogicalITInfrastructureComponent_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_LogicalITInfrastructureComponent_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_LogicalITInfrastructureComponent_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_LogicalITInfrastructureComponent_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_LogicalITInfrastructureComponent_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_LogicalITInfrastructureComponent_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_LogicalSoftwareComponent_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_LogicalSoftwareComponent_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_LogicalSoftwareComponent_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_LogicalSoftwareComponent_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_LogicalSoftwareComponent_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName]
				FROM
					[dbo].[METAView_LogicalSoftwareComponent_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_LogicalSoftwareComponent_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_LogicalSoftwareComponent_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_LogicalSoftwareComponent_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_LogicalSoftwareComponent_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_LogicalSoftwareComponent_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_LogicalSoftwareComponent_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_LogicalSoftwareComponent_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_LogicalSoftwareComponent_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_LogicalSoftwareComponent_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_LogicalSoftwareComponent_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_LogicalSoftwareComponent_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName]
				FROM
					[dbo].[METAView_LogicalSoftwareComponent_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_LogicalSoftwareComponent_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_LogicalSoftwareComponent_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_LogicalSoftwareComponent_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_LogicalSoftwareComponent_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_LogicalSoftwareComponent_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_LogicalSoftwareComponent_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_MarketSegment_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_MarketSegment_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_MarketSegment_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_MarketSegment_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_MarketSegment_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name]
				FROM
					[dbo].[METAView_MarketSegment_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_MarketSegment_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_MarketSegment_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_MarketSegment_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_MarketSegment_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_MarketSegment_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_MarketSegment_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_MarketSegment_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_MarketSegment_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_MarketSegment_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_MarketSegment_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_MarketSegment_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName]
				FROM
					[dbo].[METAView_MarketSegment_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_MarketSegment_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_MarketSegment_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_MarketSegment_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_MarketSegment_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_MarketSegment_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_MarketSegment_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Measure_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Measure_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Measure_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Measure_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Measure_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[MeasureType],
					[MeasureCategory],
					[UnitOfMeasure],
					[Name],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[ArchitectureStatus],
					[ArchitectureStatusDate]
				FROM
					[dbo].[METAView_Measure_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Measure_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Measure_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Measure_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Measure_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Measure_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Measure_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Measure_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Measure_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Measure_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Measure_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Measure_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[MeasureType],
					[MeasureCategory],
					[UnitOfMeasure],
					[Name],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[ArchitectureStatus],
					[ArchitectureStatusDate]
				FROM
					[dbo].[METAView_Measure_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Measure_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Measure_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Measure_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Measure_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Measure_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Measure_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_MeasurementItem_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_MeasurementItem_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_MeasurementItem_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_MeasurementItem_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_MeasurementItem_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_MeasurementItem_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_MeasurementItem_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_MeasurementItem_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_MeasurementItem_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_MeasurementItem_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_MeasurementItem_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_MeasurementItem_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_MeasurementItem_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_MeasurementItem_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_MeasurementItem_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_MeasurementItem_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_MeasurementItem_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_MeasurementItem_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_MeasurementItem_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_MeasurementItem_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_MeasurementItem_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_MeasurementItem_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_MeasurementItem_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_MeasurementItem_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Model_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Model_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Model_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Model_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Model_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeID],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID]
				FROM
					[dbo].[METAView_Model_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Model_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Model_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Model_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Model_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Model_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Model_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Model_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Model_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Model_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Model_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Model_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeID],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID]
				FROM
					[dbo].[METAView_Model_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Model_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Model_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Model_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Model_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Model_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Model_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ModelReal_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ModelReal_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ModelReal_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_ModelReal_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ModelReal_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeID],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID]
				FROM
					[dbo].[METAView_ModelReal_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ModelReal_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ModelReal_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ModelReal_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_ModelReal_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ModelReal_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_ModelReal_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ModelReal_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ModelReal_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ModelReal_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_ModelReal_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ModelReal_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeID],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID]
				FROM
					[dbo].[METAView_ModelReal_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ModelReal_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ModelReal_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ModelReal_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_ModelReal_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ModelReal_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_ModelReal_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_MutuallyExclusiveIndicator_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_MutuallyExclusiveIndicator_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_MutuallyExclusiveIndicator_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_MutuallyExclusiveIndicator_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_MutuallyExclusiveIndicator_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_MutuallyExclusiveIndicator_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_MutuallyExclusiveIndicator_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_MutuallyExclusiveIndicator_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_MutuallyExclusiveIndicator_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_MutuallyExclusiveIndicator_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_MutuallyExclusiveIndicator_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_MutuallyExclusiveIndicator_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_MutuallyExclusiveIndicator_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_MutuallyExclusiveIndicator_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_MutuallyExclusiveIndicator_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_MutuallyExclusiveIndicator_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_MutuallyExclusiveIndicator_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[SelectorType],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_MutuallyExclusiveIndicator_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_MutuallyExclusiveIndicator_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_MutuallyExclusiveIndicator_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_MutuallyExclusiveIndicator_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_MutuallyExclusiveIndicator_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_MutuallyExclusiveIndicator_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_MutuallyExclusiveIndicator_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Network_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Network_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Network_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Network_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Network_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[NetworkType],
					[DataStorageNetworkType],
					[ConnectionType],
					[ConnectionSpeed],
					[ConnectionSize],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[GapType],
					[PrimaryOrBackupType],
					[NetworkRange],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale]
				FROM
					[dbo].[METAView_Network_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Network_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Network_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Network_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Network_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Network_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Network_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Network_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Network_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Network_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Network_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Network_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[NetworkType],
					[DataStorageNetworkType],
					[ConnectionType],
					[ConnectionSpeed],
					[ConnectionSize],
					[Range],
					[Managed],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[GapType],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[PrimaryOrBackupType],
					[NetworkRange],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale]
				FROM
					[dbo].[METAView_Network_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Network_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Network_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Network_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Network_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Network_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Network_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_NetworkComponent_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_NetworkComponent_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_NetworkComponent_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_NetworkComponent_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_NetworkComponent_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Description],
					[SeverityRating],
					[NetworkAddress2],
					[NetworkAddress3],
					[NetworkAddress4],
					[NetworkAddress5],
					[Make],
					[Model],
					[SerialNumber],
					[AssetNumber],
					[ConnectionSpeed],
					[Range],
					[IsDNS],
					[IsDHCP],
					[isManaged],
					[DatePurchased],
					[UnderWarranty],
					[GapType],
					[NetworkComponentType],
					[ConfigurationID],
					[ModelNumber],
					[ContractNumber],
					[NetworkAddress1],
					[NumberofPorts],
					[NumberofPortsAvailable],
					[MemoryTotal],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale]
				FROM
					[dbo].[METAView_NetworkComponent_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_NetworkComponent_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_NetworkComponent_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_NetworkComponent_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_NetworkComponent_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_NetworkComponent_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_NetworkComponent_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_NetworkComponent_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_NetworkComponent_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_NetworkComponent_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_NetworkComponent_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_NetworkComponent_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Type],
					[Name],
					[Description],
					[SeverityRating],
					[Configuration],
					[MacAddress],
					[NetworkAddress2],
					[NetworkAddress3],
					[NetworkAddress4],
					[NetworkAddress5],
					[Make],
					[Model],
					[SerialNumber],
					[AssetNumber],
					[ConnectionSpeed],
					[Number_of_Ports],
					[Number_of_Ports_Available],
					[Range],
					[IsDNS],
					[IsDHCP],
					[isManaged],
					[Mem_Total],
					[DatePurchased],
					[UnderWarranty],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[NetworkType],
					[GapType],
					[NetworkComponentType],
					[ConfigurationID],
					[ModelNumber],
					[ContractNumber],
					[NetworkAddress1],
					[NumberofPorts],
					[NumberofPortsAvailable],
					[MemoryTotal],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[DataStorageNetworkType]
				FROM
					[dbo].[METAView_NetworkComponent_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_NetworkComponent_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_NetworkComponent_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_NetworkComponent_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_NetworkComponent_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_NetworkComponent_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_NetworkComponent_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Object_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Object_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Object_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Object_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Object_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[ObjectType]
				FROM
					[dbo].[METAView_Object_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Object_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Object_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Object_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Object_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Object_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Object_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Object_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Object_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Object_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Object_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Object_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[Type],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[ObjectType]
				FROM
					[dbo].[METAView_Object_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Object_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Object_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Object_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Object_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Object_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Object_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_OrganizationalUnit_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_OrganizationalUnit_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_OrganizationalUnit_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_OrganizationalUnit_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_OrganizationalUnit_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[OrganizationalUnitType],
					[TelephoneNumber],
					[FaxNumber],
					[EmailAddress],
					[Headcount]
				FROM
					[dbo].[METAView_OrganizationalUnit_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_OrganizationalUnit_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_OrganizationalUnit_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_OrganizationalUnit_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_OrganizationalUnit_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_OrganizationalUnit_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_OrganizationalUnit_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_OrganizationalUnit_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_OrganizationalUnit_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_OrganizationalUnit_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_OrganizationalUnit_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_OrganizationalUnit_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Type],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[Telephone],
					[Fax],
					[Email],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[OrganizationalUnitType],
					[TelephoneNumber],
					[FaxNumber],
					[EmailAddress],
					[Headcount]
				FROM
					[dbo].[METAView_OrganizationalUnit_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_OrganizationalUnit_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_OrganizationalUnit_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_OrganizationalUnit_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_OrganizationalUnit_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_OrganizationalUnit_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_OrganizationalUnit_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Peripheral_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Peripheral_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Peripheral_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Peripheral_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Peripheral_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Type],
					[Name],
					[Description],
					[SeverityRating],
					[Configuration],
					[NetworkAddress1],
					[NetworkAddress2],
					[NetworkAddress3],
					[NetworkAddress4],
					[NetworkAddress5],
					[Make],
					[Model],
					[SerialNumber],
					[AssetNumber],
					[Copy_PPM],
					[Print_PPM],
					[isColor],
					[isManaged],
					[isNetwork],
					[DatePurchased],
					[UnderWarranty],
					[Contract],
					[GapType],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_Peripheral_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Peripheral_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Peripheral_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Peripheral_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Peripheral_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Peripheral_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Peripheral_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Peripheral_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Peripheral_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Peripheral_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Peripheral_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Peripheral_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Type],
					[Name],
					[Description],
					[SeverityRating],
					[Configuration],
					[NetworkAddress1],
					[NetworkAddress2],
					[NetworkAddress3],
					[NetworkAddress4],
					[NetworkAddress5],
					[Make],
					[Model],
					[SerialNumber],
					[AssetNumber],
					[Copy_PPM],
					[Print_PPM],
					[isColor],
					[isManaged],
					[isNetwork],
					[DatePurchased],
					[UnderWarranty],
					[Contract],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_Peripheral_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Peripheral_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Peripheral_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Peripheral_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Peripheral_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Peripheral_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Peripheral_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_PeripheralComponent_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_PeripheralComponent_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_PeripheralComponent_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_PeripheralComponent_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_PeripheralComponent_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[PeripheralComponentType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[GapType],
					[SeverityRating],
					[ConfigurationID],
					[Make],
					[Model],
					[ModelNumber],
					[SerialNumber],
					[AssetNumber],
					[DatePurchased],
					[UnderWarranty],
					[IsManaged],
					[ContractNumber],
					[NetworkAddress1],
					[NetworkAddress2],
					[NetworkAddress3],
					[NetworkAddress4],
					[NetworkAddress5],
					[CopyPPM],
					[PrintPPM],
					[IsColor],
					[IsNetwork],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale]
				FROM
					[dbo].[METAView_PeripheralComponent_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_PeripheralComponent_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_PeripheralComponent_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_PeripheralComponent_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_PeripheralComponent_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_PeripheralComponent_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_PeripheralComponent_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_PeripheralComponent_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_PeripheralComponent_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_PeripheralComponent_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_PeripheralComponent_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_PeripheralComponent_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[PeripheralComponentType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[GapType],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[SeverityRating],
					[ConfigurationID],
					[Make],
					[Model],
					[ModelNumber],
					[SerialNumber],
					[AssetNumber],
					[DatePurchased],
					[UnderWarranty],
					[IsManaged],
					[ContractNumber],
					[NetworkAddress1],
					[NetworkAddress2],
					[NetworkAddress3],
					[NetworkAddress4],
					[NetworkAddress5],
					[CopyPPM],
					[PrintPPM],
					[IsColor],
					[IsNetwork],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale]
				FROM
					[dbo].[METAView_PeripheralComponent_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_PeripheralComponent_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_PeripheralComponent_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_PeripheralComponent_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_PeripheralComponent_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_PeripheralComponent_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_PeripheralComponent_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Person_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Person_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Person_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Person_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Person_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[PersonType],
					[Surname],
					[OtherInitials],
					[Title],
					[EmployeeNumber],
					[IDOrPassportNumber],
					[TelephoneNumber],
					[FaxNumber],
					[EmailAddress]
				FROM
					[dbo].[METAView_Person_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Person_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Person_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Person_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Person_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Person_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Person_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Person_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Person_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Person_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Person_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Person_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[PersonType],
					[Surname],
					[OtherInitials],
					[Title],
					[EmployeeNumber],
					[IDOrPassportNumber],
					[IDNumber],
					[TelephoneNumber],
					[Telephone],
					[FaxNumber],
					[EmailAddress],
					[Email]
				FROM
					[dbo].[METAView_Person_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Person_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Person_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Person_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Person_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Person_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Person_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_PhysicalDataComponent_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_PhysicalDataComponent_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_PhysicalDataComponent_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_PhysicalDataComponent_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_PhysicalDataComponent_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[DataComponentType],
					[DatabaseType],
					[SecurityClassification],
					[ContentType],
					[Name],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[GapType],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale]
				FROM
					[dbo].[METAView_PhysicalDataComponent_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_PhysicalDataComponent_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_PhysicalDataComponent_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_PhysicalDataComponent_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_PhysicalDataComponent_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_PhysicalDataComponent_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_PhysicalDataComponent_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_PhysicalDataComponent_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_PhysicalDataComponent_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_PhysicalDataComponent_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_PhysicalDataComponent_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_PhysicalDataComponent_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[DataComponentType],
					[DatabaseType],
					[SecurityClassification],
					[ContentType],
					[Name],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[GapType],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale]
				FROM
					[dbo].[METAView_PhysicalDataComponent_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_PhysicalDataComponent_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_PhysicalDataComponent_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_PhysicalDataComponent_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_PhysicalDataComponent_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_PhysicalDataComponent_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_PhysicalDataComponent_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_PhysicalInformationArtefact_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_PhysicalInformationArtefact_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_PhysicalInformationArtefact_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_PhysicalInformationArtefact_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_PhysicalInformationArtefact_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[PhysicalInformationArtefactType],
					[FormatStandard],
					[ContentType],
					[SecurityClassification],
					[IsRecord],
					[Name],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[ArchitectureStatus],
					[ArchitectureStatusDate]
				FROM
					[dbo].[METAView_PhysicalInformationArtefact_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_PhysicalInformationArtefact_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_PhysicalInformationArtefact_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_PhysicalInformationArtefact_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_PhysicalInformationArtefact_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_PhysicalInformationArtefact_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_PhysicalInformationArtefact_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_PhysicalInformationArtefact_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_PhysicalInformationArtefact_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_PhysicalInformationArtefact_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_PhysicalInformationArtefact_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_PhysicalInformationArtefact_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[PhysicalInformationArtefactType],
					[FormatStandard],
					[ContentType],
					[SecurityClassification],
					[IsRecord],
					[Name],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[ArchitectureStatus],
					[ArchitectureStatusDate]
				FROM
					[dbo].[METAView_PhysicalInformationArtefact_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_PhysicalInformationArtefact_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_PhysicalInformationArtefact_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_PhysicalInformationArtefact_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_PhysicalInformationArtefact_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_PhysicalInformationArtefact_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_PhysicalInformationArtefact_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_PhysicalSoftwareComponent_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_PhysicalSoftwareComponent_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_PhysicalSoftwareComponent_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_PhysicalSoftwareComponent_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_PhysicalSoftwareComponent_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[InternalName],
					[ConfigurationID],
					[SoftwareType],
					[SoftwareLevel],
					[IsBespoke],
					[UserInterfaceType],
					[NumberofUsers],
					[SeverityRating],
					[Edition],
					[Release],
					[ServicePackID],
					[VersionNumber],
					[ID],
					[PublisherName],
					[Language],
					[DateCreated],
					[DatePurchased],
					[LicenseNumber],
					[HasCopyright],
					[Name],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[GapType],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale]
				FROM
					[dbo].[METAView_PhysicalSoftwareComponent_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_PhysicalSoftwareComponent_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_PhysicalSoftwareComponent_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_PhysicalSoftwareComponent_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_PhysicalSoftwareComponent_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_PhysicalSoftwareComponent_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_PhysicalSoftwareComponent_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_PhysicalSoftwareComponent_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_PhysicalSoftwareComponent_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_PhysicalSoftwareComponent_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_PhysicalSoftwareComponent_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_PhysicalSoftwareComponent_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[InternalName],
					[ConfigurationID],
					[SoftwareType],
					[SoftwareLevel],
					[IsBespoke],
					[UserInterfaceType],
					[NumberofUsers],
					[SeverityRating],
					[Edition],
					[Release],
					[ServicePackID],
					[VersionNumber],
					[ID],
					[PublisherName],
					[Language],
					[DateCreated],
					[DatePurchased],
					[IsDNS],
					[IsDHCP],
					[isLicensed],
					[LicenseNumber],
					[HasCopyright],
					[Configuration],
					[Type],
					[UserInterface],
					[ServicePack],
					[Version],
					[Publisher],
					[Copyright],
					[Name],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[GapType],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[IsLicenced]
				FROM
					[dbo].[METAView_PhysicalSoftwareComponent_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_PhysicalSoftwareComponent_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_PhysicalSoftwareComponent_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_PhysicalSoftwareComponent_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_PhysicalSoftwareComponent_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_PhysicalSoftwareComponent_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_PhysicalSoftwareComponent_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Position_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Position_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Position_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Position_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Position_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[SalaryGrade]
				FROM
					[dbo].[METAView_Position_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Position_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Position_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Position_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Position_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Position_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Position_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Position_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Position_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Position_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Position_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Position_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[SalaryGrade]
				FROM
					[dbo].[METAView_Position_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Position_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Position_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Position_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Position_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Position_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Position_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_PrimaryDataSourceIndicator_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_PrimaryDataSourceIndicator_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_PrimaryDataSourceIndicator_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_PrimaryDataSourceIndicator_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_PrimaryDataSourceIndicator_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID]
				FROM
					[dbo].[METAView_PrimaryDataSourceIndicator_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_PrimaryDataSourceIndicator_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_PrimaryDataSourceIndicator_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_PrimaryDataSourceIndicator_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_PrimaryDataSourceIndicator_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_PrimaryDataSourceIndicator_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_PrimaryDataSourceIndicator_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_PrimaryDataSourceIndicator_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_PrimaryDataSourceIndicator_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_PrimaryDataSourceIndicator_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_PrimaryDataSourceIndicator_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_PrimaryDataSourceIndicator_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID]
				FROM
					[dbo].[METAView_PrimaryDataSourceIndicator_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_PrimaryDataSourceIndicator_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_PrimaryDataSourceIndicator_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_PrimaryDataSourceIndicator_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_PrimaryDataSourceIndicator_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_PrimaryDataSourceIndicator_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_PrimaryDataSourceIndicator_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ProbabilityofRealization_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ProbabilityofRealization_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ProbabilityofRealization_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_ProbabilityofRealization_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ProbabilityofRealization_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Value]
				FROM
					[dbo].[METAView_ProbabilityofRealization_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ProbabilityofRealization_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ProbabilityofRealization_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ProbabilityofRealization_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_ProbabilityofRealization_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ProbabilityofRealization_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_ProbabilityofRealization_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ProbabilityofRealization_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ProbabilityofRealization_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ProbabilityofRealization_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_ProbabilityofRealization_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ProbabilityofRealization_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[Value]
				FROM
					[dbo].[METAView_ProbabilityofRealization_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ProbabilityofRealization_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ProbabilityofRealization_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ProbabilityofRealization_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_ProbabilityofRealization_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ProbabilityofRealization_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_ProbabilityofRealization_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ProbOfRealization_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ProbOfRealization_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ProbOfRealization_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_ProbOfRealization_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ProbOfRealization_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[GapType],
					[Value]
				FROM
					[dbo].[METAView_ProbOfRealization_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ProbOfRealization_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ProbOfRealization_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ProbOfRealization_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_ProbOfRealization_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ProbOfRealization_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_ProbOfRealization_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ProbOfRealization_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ProbOfRealization_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ProbOfRealization_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_ProbOfRealization_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ProbOfRealization_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[Value]
				FROM
					[dbo].[METAView_ProbOfRealization_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ProbOfRealization_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ProbOfRealization_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ProbOfRealization_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_ProbOfRealization_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ProbOfRealization_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_ProbOfRealization_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Process_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Process_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Process_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Process_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Process_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[ExecutionIndicator],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[ProcessCriticality]
				FROM
					[dbo].[METAView_Process_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Process_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Process_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Process_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Process_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Process_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Process_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Process_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Process_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Process_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Process_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Process_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[ExecutionIndicator],
					[ContextualIndicator],
					[SequenceNumber],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[EnvironmentInd],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[ProcessCriticality]
				FROM
					[dbo].[METAView_Process_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Process_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Process_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Process_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Process_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Process_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Process_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Rationale_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Rationale_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Rationale_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Rationale_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Rationale_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[UniqueRef],
					[RationaleType],
					[Value],
					[AuthorName],
					[EffectiveDate],
					[LongDescription]
				FROM
					[dbo].[METAView_Rationale_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Rationale_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Rationale_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Rationale_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Rationale_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Rationale_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Rationale_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Rationale_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Rationale_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Rationale_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Rationale_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Rationale_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[UniqueRef],
					[RationaleType],
					[Value],
					[AuthorName],
					[EffectiveDate],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[LongDescription],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[DataSourceID],
					[DataSourceName]
				FROM
					[dbo].[METAView_Rationale_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Rationale_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Rationale_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Rationale_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Rationale_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Rationale_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Rationale_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Report_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Report_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Report_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Report_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Report_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID]
				FROM
					[dbo].[METAView_Report_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Report_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Report_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Report_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Report_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Report_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Report_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Report_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Report_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Report_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Report_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Report_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID]
				FROM
					[dbo].[METAView_Report_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Report_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Report_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Report_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Report_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Report_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Report_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Resource_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Resource_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Resource_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Resource_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Resource_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[ResourceType],
					[ResourceAvailabilityRating],
					[ResourceAvailabilityRatingDate]
				FROM
					[dbo].[METAView_Resource_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Resource_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Resource_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Resource_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Resource_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Resource_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Resource_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Resource_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Resource_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Resource_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Resource_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Resource_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[ResourceType],
					[ResourceAvailabilityRating],
					[ResourceAvailabilityRatingDate]
				FROM
					[dbo].[METAView_Resource_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Resource_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Resource_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Resource_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Resource_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Resource_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Resource_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ResourceType_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ResourceType_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ResourceType_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_ResourceType_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ResourceType_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Description],
					[ResourceAvailabilityRating],
					[ResourceAvailabilityRatingDate]
				FROM
					[dbo].[METAView_ResourceType_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ResourceType_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ResourceType_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ResourceType_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_ResourceType_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ResourceType_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_ResourceType_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ResourceType_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ResourceType_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ResourceType_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_ResourceType_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ResourceType_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[ResourceAvailabilityRating],
					[ResourceAvailabilityRatingDate]
				FROM
					[dbo].[METAView_ResourceType_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_ResourceType_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_ResourceType_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_ResourceType_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_ResourceType_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_ResourceType_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_ResourceType_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Responsibility_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Responsibility_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Responsibility_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Responsibility_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Responsibility_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_Responsibility_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Responsibility_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Responsibility_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Responsibility_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Responsibility_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Responsibility_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Responsibility_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Responsibility_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Responsibility_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Responsibility_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Responsibility_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Responsibility_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_Responsibility_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Responsibility_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Responsibility_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Responsibility_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Responsibility_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Responsibility_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Responsibility_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Risk_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Risk_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Risk_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Risk_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Risk_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[RiskStatus],
					[StatusDate],
					[RiskRating]
				FROM
					[dbo].[METAView_Risk_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Risk_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Risk_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Risk_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Risk_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Risk_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Risk_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Risk_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Risk_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Risk_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Risk_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Risk_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[RiskStatus],
					[StatusDate],
					[RiskRating]
				FROM
					[dbo].[METAView_Risk_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Risk_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Risk_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Risk_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Risk_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Risk_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Risk_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Role_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Role_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Role_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Role_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Role_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale]
				FROM
					[dbo].[METAView_Role_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Role_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Role_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Role_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Role_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Role_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Role_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Role_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Role_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Role_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Role_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Role_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[GapType],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale]
				FROM
					[dbo].[METAView_Role_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Role_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Role_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Role_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Role_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Role_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Role_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Scenario_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Scenario_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Scenario_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Scenario_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Scenario_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[AccumulatedProbabilityofRealization],
					[StartDate],
					[EndDate]
				FROM
					[dbo].[METAView_Scenario_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Scenario_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Scenario_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Scenario_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Scenario_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Scenario_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Scenario_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Scenario_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Scenario_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Scenario_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Scenario_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Scenario_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Acc_Prob_of_Real],
					[Start_Date],
					[End_Date],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[AccumulatedProbabilityofRealization],
					[StartDate],
					[EndDate]
				FROM
					[dbo].[METAView_Scenario_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Scenario_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Scenario_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Scenario_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Scenario_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Scenario_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Scenario_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_SecurityClassification_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_SecurityClassification_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_SecurityClassification_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_SecurityClassification_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_SecurityClassification_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID]
				FROM
					[dbo].[METAView_SecurityClassification_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_SecurityClassification_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_SecurityClassification_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_SecurityClassification_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_SecurityClassification_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_SecurityClassification_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_SecurityClassification_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_SecurityClassification_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_SecurityClassification_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_SecurityClassification_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_SecurityClassification_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_SecurityClassification_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID]
				FROM
					[dbo].[METAView_SecurityClassification_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_SecurityClassification_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_SecurityClassification_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_SecurityClassification_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_SecurityClassification_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_SecurityClassification_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_SecurityClassification_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_SelectorAttribute_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_SelectorAttribute_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_SelectorAttribute_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_SelectorAttribute_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_SelectorAttribute_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name]
				FROM
					[dbo].[METAView_SelectorAttribute_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_SelectorAttribute_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_SelectorAttribute_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_SelectorAttribute_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_SelectorAttribute_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_SelectorAttribute_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_SelectorAttribute_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_SelectorAttribute_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_SelectorAttribute_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_SelectorAttribute_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_SelectorAttribute_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_SelectorAttribute_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Value],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[SelectorAttributeDescription],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[DataSourceID],
					[DataSourceName]
				FROM
					[dbo].[METAView_SelectorAttribute_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_SelectorAttribute_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_SelectorAttribute_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_SelectorAttribute_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_SelectorAttribute_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_SelectorAttribute_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_SelectorAttribute_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Skill_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Skill_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Skill_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Skill_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Skill_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_Skill_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Skill_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Skill_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Skill_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Skill_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Skill_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Skill_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Skill_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Skill_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Skill_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Skill_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Skill_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_Skill_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Skill_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Skill_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Skill_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Skill_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Skill_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Skill_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Software_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Software_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Software_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Software_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Software_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Type],
					[Name],
					[Description],
					[SeverityRating],
					[Configuration],
					[Copyright],
					[Publisher],
					[InternalName],
					[Language],
					[DateCreated],
					[isDNS],
					[isDHCP],
					[isLicensed],
					[LicenseNumber],
					[DatePurchased],
					[Version],
					[ID],
					[UserInterface],
					[GapType],
					[AbbreviatedName],
					[Release],
					[Edition],
					[ServicePack],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_Software_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Software_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Software_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Software_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Software_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Software_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Software_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Software_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Software_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Software_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Software_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Software_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Type],
					[Name],
					[Description],
					[SeverityRating],
					[Configuration],
					[Copyright],
					[Publisher],
					[InternalName],
					[Language],
					[DateCreated],
					[isDNS],
					[isDHCP],
					[isLicensed],
					[LicenseNumber],
					[DatePurchased],
					[Version],
					[ID],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[UserInterface],
					[GapType],
					[AbbreviatedName],
					[Release],
					[Edition],
					[ServicePack],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_Software_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Software_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Software_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Software_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Software_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Software_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Software_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Stakeholder_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Stakeholder_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Stakeholder_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Stakeholder_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Stakeholder_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[StakeholderType]
				FROM
					[dbo].[METAView_Stakeholder_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Stakeholder_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Stakeholder_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Stakeholder_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Stakeholder_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Stakeholder_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Stakeholder_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Stakeholder_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Stakeholder_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Stakeholder_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Stakeholder_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Stakeholder_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[StakeholderType]
				FROM
					[dbo].[METAView_Stakeholder_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Stakeholder_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Stakeholder_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Stakeholder_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Stakeholder_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Stakeholder_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Stakeholder_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_StatusIndicator_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_StatusIndicator_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_StatusIndicator_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_StatusIndicator_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_StatusIndicator_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID]
				FROM
					[dbo].[METAView_StatusIndicator_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_StatusIndicator_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_StatusIndicator_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_StatusIndicator_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_StatusIndicator_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_StatusIndicator_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_StatusIndicator_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_StatusIndicator_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_StatusIndicator_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_StatusIndicator_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_StatusIndicator_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_StatusIndicator_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID]
				FROM
					[dbo].[METAView_StatusIndicator_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_StatusIndicator_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_StatusIndicator_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_StatusIndicator_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_StatusIndicator_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_StatusIndicator_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_StatusIndicator_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_StorageComponent_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_StorageComponent_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_StorageComponent_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_StorageComponent_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_StorageComponent_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Description],
					[NetworkAddress1],
					[NetworkAddress2],
					[NetworkAddress3],
					[NetworkAddress4],
					[NetworkAddress5],
					[Make],
					[Model],
					[SerialNumber],
					[AssetNumber],
					[Capacity],
					[DatePurchased],
					[UnderWarranty],
					[FileSystem],
					[GapType],
					[StorageComponentType],
					[IsPrimaryOrBackup],
					[SeverityRating],
					[ConfigurationID],
					[ModelNumber],
					[isManaged],
					[ContractNumber],
					[NumberofDisks],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[PrimaryOrBackupType],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale]
				FROM
					[dbo].[METAView_StorageComponent_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_StorageComponent_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_StorageComponent_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_StorageComponent_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_StorageComponent_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_StorageComponent_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_StorageComponent_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_StorageComponent_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_StorageComponent_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_StorageComponent_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_StorageComponent_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_StorageComponent_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Type],
					[Name],
					[Description],
					[SeverityIndicator],
					[Configuration],
					[NetworkAddress1],
					[NetworkAddress2],
					[NetworkAddress3],
					[NetworkAddress4],
					[NetworkAddress5],
					[Make],
					[Model],
					[SerialNumber],
					[AssetNumber],
					[Capacity],
					[Number_of_Disks],
					[DatePurchased],
					[UnderWarranty],
					[FileSystem],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[StorageComponentType],
					[IsPrimaryOrBackup],
					[SeverityRating],
					[ConfigurationID],
					[ModelNumber],
					[isManaged],
					[ContractNumber],
					[NumberofDisks],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[PrimaryOrBackupType],
					[IsPrimaryOrBackupType],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale]
				FROM
					[dbo].[METAView_StorageComponent_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_StorageComponent_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_StorageComponent_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_StorageComponent_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_StorageComponent_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_StorageComponent_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_StorageComponent_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_StrategicStatement_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_StrategicStatement_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_StrategicStatement_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_StrategicStatement_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_StrategicStatement_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[StrategicStatementType],
					[StrategicStatementLevel],
					[UniqueRef]
				FROM
					[dbo].[METAView_StrategicStatement_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_StrategicStatement_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_StrategicStatement_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_StrategicStatement_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_StrategicStatement_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_StrategicStatement_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_StrategicStatement_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_StrategicStatement_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_StrategicStatement_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_StrategicStatement_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_StrategicStatement_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_StrategicStatement_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[StrategicStatementType],
					[StrategicStatementLevel],
					[UniqueRef],
					[Number]
				FROM
					[dbo].[METAView_StrategicStatement_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_StrategicStatement_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_StrategicStatement_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_StrategicStatement_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_StrategicStatement_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_StrategicStatement_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_StrategicStatement_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_StrategicTheme_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_StrategicTheme_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_StrategicTheme_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_StrategicTheme_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_StrategicTheme_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_StrategicTheme_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_StrategicTheme_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_StrategicTheme_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_StrategicTheme_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_StrategicTheme_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_StrategicTheme_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_StrategicTheme_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_StrategicTheme_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_StrategicTheme_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_StrategicTheme_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_StrategicTheme_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_StrategicTheme_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale]
				FROM
					[dbo].[METAView_StrategicTheme_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_StrategicTheme_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_StrategicTheme_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_StrategicTheme_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_StrategicTheme_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_StrategicTheme_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_StrategicTheme_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_SubsetIndicator_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_SubsetIndicator_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_SubsetIndicator_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_SubsetIndicator_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_SubsetIndicator_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[SubsetIndicatorType],
					[IsCompleteSet]
				FROM
					[dbo].[METAView_SubsetIndicator_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_SubsetIndicator_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_SubsetIndicator_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_SubsetIndicator_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_SubsetIndicator_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_SubsetIndicator_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_SubsetIndicator_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_SubsetIndicator_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_SubsetIndicator_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_SubsetIndicator_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_SubsetIndicator_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_SubsetIndicator_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[SubsetIndicatorType],
					[IsCompleteSet]
				FROM
					[dbo].[METAView_SubsetIndicator_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_SubsetIndicator_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_SubsetIndicator_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_SubsetIndicator_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_SubsetIndicator_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_SubsetIndicator_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_SubsetIndicator_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Sys_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Sys_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Sys_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Sys_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Sys_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[SystemType],
					[TransformationType]
				FROM
					[dbo].[METAView_Sys_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Sys_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Sys_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Sys_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Sys_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Sys_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Sys_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Sys_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Sys_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Sys_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Sys_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Sys_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[SystemType],
					[TransformationType]
				FROM
					[dbo].[METAView_Sys_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Sys_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Sys_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Sys_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Sys_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Sys_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Sys_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_SystemComponent_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_SystemComponent_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_SystemComponent_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_SystemComponent_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_SystemComponent_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Type],
					[Name],
					[Description],
					[SeverityRating],
					[Configuration],
					[MACAddress],
					[StaticIP],
					[NetworkAddress3],
					[NetworkAddress4],
					[NetworkAddress5],
					[Make],
					[Model],
					[SerialNumber],
					[AssetNumber],
					[isDNS],
					[isDHCP],
					[Capacity],
					[Mem_Total],
					[CPU_Type],
					[CPU_Speed],
					[Monitor],
					[Video_Card],
					[Number_Of_Disks],
					[DatePurchased],
					[UnderWarranty],
					[Domain],
					[ServerType],
					[GapType],
					[AbbreviatedName],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_SystemComponent_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_SystemComponent_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_SystemComponent_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_SystemComponent_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_SystemComponent_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_SystemComponent_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_SystemComponent_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_SystemComponent_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_SystemComponent_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_SystemComponent_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_SystemComponent_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_SystemComponent_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Type],
					[Name],
					[Description],
					[SeverityRating],
					[Configuration],
					[MACAddress],
					[StaticIP],
					[NetworkAddress3],
					[NetworkAddress4],
					[NetworkAddress5],
					[Make],
					[Model],
					[SerialNumber],
					[AssetNumber],
					[isDNS],
					[isDHCP],
					[Capacity],
					[Mem_Total],
					[CPU_Type],
					[CPU_Speed],
					[Monitor],
					[Video_Card],
					[Number_Of_Disks],
					[DatePurchased],
					[UnderWarranty],
					[Domain],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[ServerType],
					[GapType],
					[AbbreviatedName],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks]
				FROM
					[dbo].[METAView_SystemComponent_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_SystemComponent_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_SystemComponent_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_SystemComponent_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_SystemComponent_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_SystemComponent_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_SystemComponent_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Task_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Task_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Task_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Task_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Task_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName]
				FROM
					[dbo].[METAView_Task_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Task_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Task_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Task_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Task_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Task_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Task_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Task_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Task_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Task_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_Task_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Task_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName]
				FROM
					[dbo].[METAView_Task_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_Task_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_Task_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_Task_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_Task_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_Task_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_Task_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_TimeDifference_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_TimeDifference_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_TimeDifference_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_TimeDifference_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_TimeDifference_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name]
				FROM
					[dbo].[METAView_TimeDifference_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_TimeDifference_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_TimeDifference_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_TimeDifference_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_TimeDifference_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_TimeDifference_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_TimeDifference_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_TimeDifference_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_TimeDifference_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_TimeDifference_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_TimeDifference_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_TimeDifference_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name]
				FROM
					[dbo].[METAView_TimeDifference_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_TimeDifference_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_TimeDifference_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_TimeDifference_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_TimeDifference_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_TimeDifference_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_TimeDifference_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_TimeIndicator_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_TimeIndicator_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_TimeIndicator_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_TimeIndicator_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_TimeIndicator_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[GapType],
					[Value]
				FROM
					[dbo].[METAView_TimeIndicator_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_TimeIndicator_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_TimeIndicator_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_TimeIndicator_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_TimeIndicator_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_TimeIndicator_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_TimeIndicator_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_TimeIndicator_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_TimeIndicator_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_TimeIndicator_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_TimeIndicator_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_TimeIndicator_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[GapType],
					[Name],
					[Value]
				FROM
					[dbo].[METAView_TimeIndicator_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_TimeIndicator_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_TimeIndicator_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_TimeIndicator_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_TimeIndicator_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_TimeIndicator_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_TimeIndicator_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_TimeReference_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_TimeReference_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_TimeReference_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_TimeReference_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_TimeReference_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale]
				FROM
					[dbo].[METAView_TimeReference_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_TimeReference_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_TimeReference_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_TimeReference_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_TimeReference_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_TimeReference_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_TimeReference_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_TimeReference_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_TimeReference_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_TimeReference_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_TimeReference_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_TimeReference_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[GapType],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale]
				FROM
					[dbo].[METAView_TimeReference_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_TimeReference_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_TimeReference_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_TimeReference_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_TimeReference_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_TimeReference_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_TimeReference_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_TimeScheme_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_TimeScheme_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_TimeScheme_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_TimeScheme_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_TimeScheme_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[GapType],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[TimeSchemeType]
				FROM
					[dbo].[METAView_TimeScheme_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_TimeScheme_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_TimeScheme_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_TimeScheme_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_TimeScheme_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_TimeScheme_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_TimeScheme_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_TimeScheme_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_TimeScheme_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_TimeScheme_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_TimeScheme_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_TimeScheme_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[GapType],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[TimeSchemeType]
				FROM
					[dbo].[METAView_TimeScheme_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_TimeScheme_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_TimeScheme_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_TimeScheme_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_TimeScheme_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_TimeScheme_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_TimeScheme_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_TimeUnit_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_TimeUnit_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_TimeUnit_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_TimeUnit_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_TimeUnit_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Format],
					[TimeUnitType],
					[TimeDomainValues]
				FROM
					[dbo].[METAView_TimeUnit_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_TimeUnit_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_TimeUnit_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_TimeUnit_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_TimeUnit_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_TimeUnit_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_TimeUnit_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_TimeUnit_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_TimeUnit_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_TimeUnit_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_TimeUnit_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_TimeUnit_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[DomainValue],
					[Format],
					[GapType],
					[CustomField1],
					[CustomField2],
					[CustomField3],
					[Description],
					[Abbreviation],
					[IsBusinessExternal],
					[StandardisationStatus],
					[StandardisationStatusDate],
					[DataSourceID],
					[DataSourceName],
					[GeneralRemarks],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[TimeUnitType],
					[TimeDomainValues]
				FROM
					[dbo].[METAView_TimeUnit_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_TimeUnit_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_TimeUnit_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_TimeUnit_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_TimeUnit_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_TimeUnit_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_TimeUnit_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_WorkPackage_Listing_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_WorkPackage_Listing_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_WorkPackage_Listing_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_WorkPackage_Listing view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_WorkPackage_Listing_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[WorkPackageType]
				FROM
					[dbo].[METAView_WorkPackage_Listing]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_WorkPackage_Listing_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_WorkPackage_Listing_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_WorkPackage_Listing_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_WorkPackage_Listing view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_WorkPackage_Listing_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_WorkPackage_Listing]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_WorkPackage_Retrieval_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_WorkPackage_Retrieval_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_WorkPackage_Retrieval_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the METAView_WorkPackage_Retrieval view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_WorkPackage_Retrieval_Get_List

AS


				
				SELECT
					[WorkspaceName],
					[WorkspaceTypeId],
					[VCStatusID],
					[pkid],
					[Machine],
					[VCMachineID],
					[Name],
					[Abbreviation],
					[Description],
					[IsBusinessExternal],
					[ArchitectureStatus],
					[ArchitectureStatusDate],
					[DesignRationale],
					[GeneralRemarks],
					[GapType],
					[DataSourceID],
					[DataSourceName],
					[WorkPackageType]
				FROM
					[dbo].[METAView_WorkPackage_Retrieval]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_METAView_WorkPackage_Retrieval_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_METAView_WorkPackage_Retrieval_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_METAView_WorkPackage_Retrieval_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the METAView_WorkPackage_Retrieval view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_METAView_WorkPackage_Retrieval_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[METAView_WorkPackage_Retrieval]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ORGUNITFUNCTIONROLE_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ORGUNITFUNCTIONROLE_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ORGUNITFUNCTIONROLE_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the ORGUNITFUNCTIONROLE view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ORGUNITFUNCTIONROLE_Get_List

AS


				
				SELECT
					[Function],
					[OrgUnit],
					[Role]
				FROM
					[dbo].[ORGUNITFUNCTIONROLE]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_ORGUNITFUNCTIONROLE_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_ORGUNITFUNCTIONROLE_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_ORGUNITFUNCTIONROLE_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the ORGUNITFUNCTIONROLE view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_ORGUNITFUNCTIONROLE_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[ORGUNITFUNCTIONROLE]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_vw_FieldValue_Get_List procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_vw_FieldValue_Get_List') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_vw_FieldValue_Get_List
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets all records from the vw_FieldValue view
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_vw_FieldValue_Get_List

AS


				
				SELECT
					[Field],
					[ValueString],
					[ValueInt],
					[ValueDouble],
					[ValueObjectID],
					[ValueDate],
					[ValueBoolean]
				FROM
					[dbo].[vw_FieldValue]
					
				SELECT @@ROWCOUNT			
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

	

-- Drop the dbo.PROC_vw_FieldValue_Get procedure
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PROC_vw_FieldValue_Get') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE dbo.PROC_vw_FieldValue_Get
GO

/*
----------------------------------------------------------------------------------------------------

-- Created By: DISCON Specialists (http://www.discon.co.za)
-- Purpose: Gets records from the vw_FieldValue view passing page index and page count parameters
----------------------------------------------------------------------------------------------------
*/


CREATE PROCEDURE dbo.PROC_vw_FieldValue_Get
(

	@WhereClause varchar (2000)  ,

	@OrderBy varchar (2000)  
)
AS


				
				BEGIN

				-- Build the sql query
				DECLARE @SQL AS nvarchar(4000)
				SET @SQL = ' SELECT * FROM [dbo].[vw_FieldValue]'
				IF LEN(@WhereClause) > 0
				BEGIN
					SET @SQL = @SQL + ' WHERE ' + @WhereClause
				END
				IF LEN(@OrderBy) > 0
				BEGIN
					SET @SQL = @SQL + ' ORDER BY ' + @OrderBy
				END
				
				-- Execution the query
				EXEC sp_executesql @SQL
				
				-- Return total count
				SELECT @@ROWCOUNT AS TotalRowCount
				
				END
			

GO
SET QUOTED_IDENTIFIER ON 
GO
SET NOCOUNT ON
GO
SET ANSI_NULLS OFF 
GO

