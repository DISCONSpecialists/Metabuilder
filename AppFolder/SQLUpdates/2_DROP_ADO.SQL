IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ActiveDiagramObjects]'))
DROP VIEW [dbo].[ActiveDiagramObjects]