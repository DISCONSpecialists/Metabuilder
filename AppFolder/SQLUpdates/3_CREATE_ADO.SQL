CREATE VIEW [dbo].[ActiveDiagramObjects]
AS
SELECT     dbo.GraphFile.pkid, dbo.TrimFileName(dbo.GraphFile.Name) AS [File Name], dbo.GraphFileObject.MetaObjectID, dbo.GraphFileObject.MachineID AS Machine
FROM         dbo.GraphFile INNER JOIN
                      dbo.GraphFileObject ON dbo.GraphFile.pkid = dbo.GraphFileObject.GraphFileID AND dbo.GraphFile.Machine = dbo.GraphFileObject.GraphFileMachine
