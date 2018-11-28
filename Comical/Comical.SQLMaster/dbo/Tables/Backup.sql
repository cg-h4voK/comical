﻿CREATE TABLE [Backup]
(
	Id INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	FilePath NVARCHAR(1000) NOT NULL,
	BackupDate DATETIME NOT NULL,
	CreatedBy NVARCHAR(500) NOT NULL
)
