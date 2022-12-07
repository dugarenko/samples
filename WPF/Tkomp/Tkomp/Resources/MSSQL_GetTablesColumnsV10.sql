-- ===============================================================
-- Server version 10
-- Standardowy skrypt, który zwraca normalną ilość informacji
-- o schematach kolumn.
-- ===============================================================
--DECLARE @ObjectSchema1 NVARCHAR(128) = N'dbo'
--DECLARE @ObjectSchema2 NVARCHAR(128) = N'dbo'
--DECLARE @ObjectSchema3 NVARCHAR(128) = N'dbo'
--DECLARE @ObjectName1 NVARCHAR(128)   = N'Table_A'
--DECLARE @ObjectName2 NVARCHAR(128)   = N'Table_B'
--DECLARE @ObjectName3 NVARCHAR(128)   = N'Table_C'
--DECLARE @SqlDbType1 NVARCHAR(128)    = N'int'
--DECLARE @SqlDbType2 NVARCHAR(128)    = N'int'
--DECLARE @SqlDbType3 NVARCHAR(128)    = N'int'
--DECLARE @AllSchemas BIT = 0       -- (1)Wszystkie schematy.
--DECLARE @AllObjects BIT = 0       -- (1)Wszystkie tabele.
--DECLARE @AllSqlDbType BIT = 0     -- (1)Wszystkie typy kolumn.
-- ===============================================================
-- SELECT
-- ===============================================================
SELECT
   -- Table properties.
   tbl.[object_id] AS [IdObject]
   ,SCHEMA_NAME(tbl.schema_id) AS [ObjectSchema]
   ,tbl.name AS [ObjectName]
   ,tbl.[type] AS [ObjectType]
   ,CONVERT(BIT, 1) AS IsTableObject
   --
   ,col.column_id AS [ColumnOrdinal]
   ,col.name AS [ColumnName]
   ,ISNULL(bt.name, st.name) AS [SqlDbType]
   ,st.name AS [UserDbType]
   ,SCHEMA_NAME(st.SCHEMA_ID) AS [SchemaUserDbType]
   ,col2.character_maximum_length AS [CharacterMaximumLength]
   ,col.max_length AS [MaxLength]
   ,CONVERT(TINYINT, col.precision) AS [Precision]
   ,CONVERT(TINYINT, col.scale) AS [Scale]
   ,col.collation_name AS [CollationName]
   ,TdsCollation28 = CONVERT(BINARY(5)
	   ,CASE
		  WHEN col.system_type_id in (241, 40, 41, 42, 43) THEN
			 COLLATIONPROPERTY(CONVERT(sysname, SERVERPROPERTY(N'Collation')), 'TdsCollation')
		  ELSE
			 COLLATIONPROPERTY(col.collation_name, N'TdsCollation')
	   END)
   ,col.is_nullable AS [AllowDBNull]
   ,col.is_ansi_padded AS [IsAnsiPadded]
   ,col.is_rowguidcol AS [IsRowGuid]
   ,CONVERT(BIT, col.is_identity) AS [AutoIncrement]
   ,CONVERT(BIGINT, CASE WHEN(idc.column_id IS NULL) THEN NULL ELSE idc.seed_value END) AS [AutoIncrementSeed]
   ,CONVERT(BIGINT, CASE WHEN(idc.column_id IS NULL) THEN NULL ELSE idc.increment_value END) AS [AutoIncrementStep]
   -- ===========================================
   -- Indexes.
   -- ===========================================
   ,i.name AS [IndexName]
   ,CONVERT(TINYINT, i.type) AS [IndexType]
   ,i.type_desc AS [IndexTypeName]
   ,CONVERT(BIT, ISNULL(i.is_primary_key, 0)) AS [IsPrimaryKey]
   ,CONVERT(BIT, ISNULL(i.is_unique, 0)) AS [IsUnique]
   ,CONVERT(BIT, CASE WHEN((i.name IS NOT NULL) AND (i.is_primary_key = 0) AND (i.is_unique = 0)) THEN 1 ELSE 0 END) AS [IsIndex]
   ,CONVERT(BIT, ISNULL(i.is_disabled, 0)) AS [IsDisabledIndex]
   --
   ,CONVERT(TINYINT, ISNULL(ic.key_ordinal, 0)) AS [KeyOrdinal]
   ,CONVERT(BIT, ISNULL(ic.is_descending_key, 0)) AS [IsDescendingKey]
   ,CONVERT(BIT, ISNULL(ic.is_included_column, 0)) AS [IsIncludedColumn]
   ,CONVERT(TINYINT, ISNULL(ic.index_column_id, 0)) AS [IndexColumnId]
   -- ===============================================================
   ,CONVERT(BIT, ISNULL(cmc.column_id, 0)) AS [IsComputed]
   ,CONVERT(BIT, COLUMNPROPERTY(col.object_id, col.name, N'IsIdNotForRepl')) AS [IsIdNotForRepl]
   ,col.is_replicated AS [IsReplicated]
   ,col.is_non_sql_subscribed AS [IsNonSqlSubscribed]
   ,col.is_dts_replicated AS [IsDtsReplicated]
   ,CONVERT(BIT, ISNULL(m.definition, 0)) AS IsDefault
   ,m.definition AS DefaultDefinition
   ,col.rule_object_id AS [IdRule]
   ,SCHEMA_NAME(robj.schema_id) AS [SchemaRule]
   ,robj.name AS [RuleName]
   ,CONVERT(BIT, ISNULL(OBJECTPROPERTY(col.default_object_id, N'IsDefaultCnst'), 0)) AS [IsDefaultConstraint]
   ,col.default_object_id AS [IdDefaultConstraint]
   ,SCHEMA_NAME(dobj.schema_id) AS [SchemaDefaultConstraint]
   ,dobj.name AS [DefaultConstraintName]
   ,defCst.definition AS [DefaultConstraintValue]
   ,CONVERT(BIT, ISNULL(ftc.column_id, 0)) AS [IsFullTextCol]
   ,COL_NAME(col.object_id, ftc.type_column_id) AS [FullTextTypeColumn]
   ,ISNULL(ftc.language_id, 0) AS [IdFullTextLanguage]
   ,CASE WHEN(cmc.column_id IS NULL) THEN NULL ELSE cmc.definition END AS [Formular]
   ,CONVERT(BIT, CASE WHEN(cmc.column_id IS NULL) THEN 0 ELSE cmc.is_persisted END) AS [IsPersisted]
   ,CONVERT(BIT, ISNULL(COLUMNPROPERTY(col.object_id, col.name, 'IsDeterministic'), 0)) AS [IsDeterministic]
   ,CONVERT(BIT, ISNULL(col.is_xml_document, 0)) AS [IsXmlDocument]
   ,SCHEMA_NAME(xmlcoll.schema_id) AS [SchemaXmlSchema]
   ,xmlcoll.name AS [XmlSchema]
FROM sys.tables tbl
JOIN sys.columns col 
   ON (col.[object_id] = tbl.[object_id])
JOIN INFORMATION_SCHEMA.COLUMNS col2
   ON (col2.table_schema = SCHEMA_NAME(tbl.schema_id)
      AND col2.table_name = tbl.name
      AND col2.column_name = col.name)
LEFT JOIN sys.types bt
   ON (bt.user_type_id = col.system_type_id)
LEFT JOIN sys.types st
   ON (st.user_type_id = col.user_type_id)
-- ===========================================
-- Indexes.
-- ===========================================
LEFT JOIN sys.index_columns ic
   ON (ic.[object_id] = col.[object_id]
      AND ic.column_id = col.column_id)
LEFT JOIN sys.indexes i
   ON (i.[object_id] = ic.[object_id]
      AND i.index_id = ic.index_id
      AND i.name IS NOT NULL
      AND i.[type] NOT IN (3, 4))
-- ===========================================
LEFT JOIN sys.objects robj
   ON (robj.object_id = col.rule_object_id)
    AND robj.type = 'R'
LEFT JOIN sys.objects dobj
   ON (dobj.object_id = col.default_object_id)
    AND dobj.type = 'D'
LEFT JOIN sys.default_constraints defCst
   ON (defCst.parent_object_id = col.object_id)
    AND defCst.parent_column_id = col.column_id
LEFT JOIN sys.sql_modules m
   ON (m.object_id = col.default_object_id)
     AND OBJECTPROPERTY(col.default_object_id, N'IsDefault') = 1
LEFT JOIN sys.identity_columns idc
   ON (idc.object_id = col.object_id)
    AND idc.column_id = col.column_id
LEFT JOIN sys.computed_columns cmc
   ON (cmc.object_id = col.object_id)
    AND cmc.column_id = col.column_id
LEFT JOIN sys.fulltext_index_columns ftc
   ON (ftc.object_id = col.object_id)
    AND ftc.column_id = col.column_id
LEFT JOIN sys.xml_schema_collections xmlcoll
   ON (xmlcoll.xml_Collection_id = col.xml_Collection_id)
WHERE
   (@AllSchemas = 1 OR SCHEMA_NAME(tbl.schema_id) IN (@ObjectSchema1, @ObjectSchema2, @ObjectSchema3))
   AND (@AllObjects = 1 OR tbl.name IN (@ObjectName1, @ObjectName2, @ObjectName3))
   AND tbl.name NOT IN('sysdiagrams', '__RefactorLog')
   AND tbl.is_ms_shipped = 0        -- bez tabel które utworzyła replikacja na własne potrzeby.
   AND (@AllSqlDbType = 1 OR ISNULL(bt.name, st.name) IN (@SqlDbType1, @SqlDbType2, @SqlDbType3))
ORDER BY
   SCHEMA_NAME(tbl.schema_id)
   ,tbl.name
   ,col.column_id
-- ===============================================================
