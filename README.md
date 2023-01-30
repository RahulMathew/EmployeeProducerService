Run the following db script

CREATE DATABASE [Workforce]
GO
USE [Workforce]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 2023-01-29 6:24:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[EmployeeId] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeName] [nvarchar](200) NOT NULL,
	[HourlyRate] [decimal](18, 0) NULL,
	[HoursWorked] [int] NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[GetEmployees]    Script Date: 2023-01-29 6:24:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetEmployees]
(
	@PageNumber AS INT = 1,
	@PageSize AS INT = 10
)
AS
BEGIN
	IF @PageNumber = 0 AND @PageSize = 0
	BEGIN
		SET @PageNumber = 1
		SET @PageSize = 10
	END

	;WITH CTX AS
	(
		SELECT ROW_NUMBER() OVER (ORDER BY [EMP].[EmployeeId] ASC) AS [ROW]
			,[EMP].[EmployeeId]
			,[EMP].[EmployeeName]
			,[EMP].[HourlyRate]
			,[EMP].[HoursWorked]
		FROM [DBO].[Employee] AS [EMP]
	),TempCount AS
	(
		SELECT ((COUNT(*) - 1) / @PageSize) + 1 AS TotalPages, COUNT(*) AS TotalRecords
		FROM CTX
	)
	SELECT * FROM
	(
		SELECT D.*, TotalPages, TotalRecords FROM CTX d, TempCount
	)RES WHERE RES.[ROW] BETWEEN (@PageNumber-1) * @PageSize + 1
	AND @PageNumber * @PageSize
END
GO
/****** Object:  StoredProcedure [dbo].[SaveEmployee]    Script Date: 2023-01-29 6:24:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveEmployee]
(
	@EmployeeID INT,
	@EmployeeName NVARCHAR(200),
	@HourlyRate DECIMAL(18,0) NULL,
	@HoursWorked INT NULL
)
AS
BEGIN
	IF NOT EXISTS(SELECT TOP 1 * FROM [dbo].[Employee]
	WHERE EmployeeID = @EmployeeID)
	BEGIN
		INSERT INTO [dbo].[Employee]
		(
			EmployeeName,
			HourlyRate,
			HoursWorked
		)
		VALUES
		(
			@EmployeeName,
			@HourlyRate,
			@HoursWorked
		)
	END
	ELSE
	BEGIN
		UPDATE [dbo].[Employee]
		SET EmployeeName = @EmployeeName,
			HourlyRate = @HourlyRate,
			HoursWorked = @HoursWorked
		WHERE EmployeeId = @EmployeeID
	END
END
GO
