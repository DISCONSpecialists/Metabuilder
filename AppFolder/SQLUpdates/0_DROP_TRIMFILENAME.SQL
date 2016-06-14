
/****** Object:  UserDefinedFunction [dbo].[TrimFileName]    Script Date: 11/12/2008 15:03:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TrimFileName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[TrimFileName]