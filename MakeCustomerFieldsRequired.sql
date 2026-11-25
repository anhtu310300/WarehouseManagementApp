BEGIN TRANSACTION;
DECLARE @var nvarchar(max);
SELECT @var = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[warehouse_db_customerMaster]') AND [c].[name] = N'mobile');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [warehouse_db_customerMaster] DROP CONSTRAINT ' + @var + ';');
ALTER TABLE [warehouse_db_customerMaster] ALTER COLUMN [mobile] nvarchar(max) NOT NULL;

DECLARE @var1 nvarchar(max);
SELECT @var1 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[warehouse_db_customerMaster]') AND [c].[name] = N'gstNo');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [warehouse_db_customerMaster] DROP CONSTRAINT ' + @var1 + ';');
ALTER TABLE [warehouse_db_customerMaster] ALTER COLUMN [gstNo] nvarchar(max) NOT NULL;

DECLARE @var2 nvarchar(max);
SELECT @var2 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[warehouse_db_customerMaster]') AND [c].[name] = N'email');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [warehouse_db_customerMaster] DROP CONSTRAINT ' + @var2 + ';');
ALTER TABLE [warehouse_db_customerMaster] ALTER COLUMN [email] nvarchar(max) NOT NULL;

DECLARE @var3 nvarchar(max);
SELECT @var3 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[warehouse_db_customerMaster]') AND [c].[name] = N'address');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [warehouse_db_customerMaster] DROP CONSTRAINT ' + @var3 + ';');
ALTER TABLE [warehouse_db_customerMaster] ALTER COLUMN [address] nvarchar(max) NOT NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261112054115_MakeCustomerFieldsRequired', N'10.0.13');

COMMIT;
GO

