using System;
using System.Data.SqlClient;

namespace Tkomp.Models
{
    internal partial class ColumnInfo
    {
        #region Internal methods.

        /// <summary>
        /// Wypełnia obiekt danymi.
        /// </summary>
        /// <param name="dr">Obiekt SqlDataReader.</param>
        internal void Fill(SqlDataReader dr)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                switch (dr.GetName(i).ToLower())
                {
                    case "idobject":
                        IdObject = dr.IsDBNull(i) ? null : (int?)dr.GetInt32(i);
                        break;

                    case "objectschema":
                        ObjectSchema = dr.IsDBNull(i) ? null : dr.GetString(i);
                        break;

                    case "objectname":
                        ObjectName = dr.IsDBNull(i) ? null : dr.GetString(i);
                        break;

                    case "objecttype":
                        ObjectType = dr.IsDBNull(i) ? null : dr.GetString(i);
                        break;

                    case "istableobject":
                        IsTableObject = dr.IsDBNull(i) ? null : (bool?)dr.GetBoolean(i);
                        break;

                    case "columnordinal":
                        ColumnOrdinal = dr.IsDBNull(i) ? null : (int?)dr.GetInt32(i);
                        break;

                    case "columnname":
                        ColumnName = dr.IsDBNull(i) ? null : dr.GetString(i);
                        break;

                    case "sqldbtype":
                        SqlDbType = dr.IsDBNull(i) ? null : dr.GetString(i);
                        break;

                    case "userdbtype":
                        UserDbType = dr.IsDBNull(i) ? null : dr.GetString(i);
                        break;

                    case "schemauserdbtype":
                        SchemaUserDbType = dr.IsDBNull(i) ? null : dr.GetString(i);
                        break;

                    case "charactermaximumlength":
                        CharacterMaximumLength = dr.IsDBNull(i) ? null : (int?)dr.GetInt32(i);
                        break;

                    case "maxlength":
                        MaxLength = dr.IsDBNull(i) ? null : (short?)dr.GetInt16(i);
                        break;

                    case "precision":
                        Precision = dr.IsDBNull(i) ? null : (byte?)dr.GetByte(i);
                        break;

                    case "scale":
                        Scale = dr.IsDBNull(i) ? null : (byte?)dr.GetByte(i);
                        break;

                    case "collationname":
                        CollationName = dr.IsDBNull(i) ? null : dr.GetString(i);
                        break;

                    case "tdscollation28":
                        TdsCollation28 = dr.IsDBNull(i) ? null : Array.ConvertAll<byte, byte?>((byte[])dr.GetValue(i), delegate (byte value) { return value; });
                        break;

                    case "allowdbnull":
                        AllowDBNull = dr.IsDBNull(i) ? null : (bool?)dr.GetBoolean(i);
                        break;

                    case "isansipadded":
                        IsAnsiPadded = dr.IsDBNull(i) ? null : (bool?)dr.GetBoolean(i);
                        break;

                    case "isrowguid":
                        IsRowGuid = dr.IsDBNull(i) ? null : (bool?)dr.GetBoolean(i);
                        break;

                    case "autoincrement":
                        AutoIncrement = dr.IsDBNull(i) ? null : (bool?)dr.GetBoolean(i);
                        break;

                    case "autoincrementseed":
                        AutoIncrementSeed = dr.IsDBNull(i) ? null : (long?)dr.GetInt64(i);
                        break;

                    case "autoincrementstep":
                        AutoIncrementStep = dr.IsDBNull(i) ? null : (long?)dr.GetInt64(i);
                        break;

                    case "indexname":
                        IndexName = dr.IsDBNull(i) ? null : dr.GetString(i);
                        break;

                    case "indextype":
                        IndexType = dr.IsDBNull(i) ? null : (byte?)dr.GetByte(i);
                        break;

                    case "indextypename":
                        IndexTypeName = dr.IsDBNull(i) ? null : dr.GetString(i);
                        break;

                    case "isprimarykey":
                        IsPrimaryKey = dr.IsDBNull(i) ? null : (bool?)dr.GetBoolean(i);
                        break;

                    case "isunique":
                        IsUnique = dr.IsDBNull(i) ? null : (bool?)dr.GetBoolean(i);
                        break;

                    case "isindex":
                        IsIndex = dr.IsDBNull(i) ? null : (bool?)dr.GetBoolean(i);
                        break;

                    case "isdisabledindex":
                        IsDisabledIndex = dr.IsDBNull(i) ? null : (bool?)dr.GetBoolean(i);
                        break;

                    case "keyordinal":
                        KeyOrdinal = dr.IsDBNull(i) ? null : (byte?)dr.GetByte(i);
                        break;

                    case "isdescendingkey":
                        IsDescendingKey = dr.IsDBNull(i) ? null : (bool?)dr.GetBoolean(i);
                        break;

                    case "isincludedcolumn":
                        IsIncludedColumn = dr.IsDBNull(i) ? null : (bool?)dr.GetBoolean(i);
                        break;

                    case "indexcolumnid":
                        IndexColumnId = dr.IsDBNull(i) ? null : (byte?)dr.GetByte(i);
                        break;

                    case "iscomputed":
                        IsComputed = dr.IsDBNull(i) ? null : (bool?)dr.GetBoolean(i);
                        break;

                    case "isidnotforrepl":
                        IsIdNotForRepl = dr.IsDBNull(i) ? null : (bool?)dr.GetBoolean(i);
                        break;

                    case "isreplicated":
                        IsReplicated = dr.IsDBNull(i) ? null : (bool?)dr.GetBoolean(i);
                        break;

                    case "isnonsqlsubscribed":
                        IsNonSqlSubscribed = dr.IsDBNull(i) ? null : (bool?)dr.GetBoolean(i);
                        break;

                    case "isdtsreplicated":
                        IsDtsReplicated = dr.IsDBNull(i) ? null : (bool?)dr.GetBoolean(i);
                        break;

                    case "isdefault":
                        IsDefault = dr.IsDBNull(i) ? null : (bool?)dr.GetBoolean(i);
                        break;

                    case "defaultdefinition":
                        DefaultDefinition = dr.IsDBNull(i) ? null : dr.GetString(i);
                        break;

                    case "idrule":
                        IdRule = dr.IsDBNull(i) ? null : (int?)dr.GetInt32(i);
                        break;

                    case "schemarule":
                        SchemaRule = dr.IsDBNull(i) ? null : dr.GetString(i);
                        break;

                    case "rulename":
                        RuleName = dr.IsDBNull(i) ? null : dr.GetString(i);
                        break;

                    case "isdefaultconstraint":
                        IsDefaultConstraint = dr.IsDBNull(i) ? null : (bool?)dr.GetBoolean(i);
                        break;

                    case "iddefaultconstraint":
                        IdDefaultConstraint = dr.IsDBNull(i) ? null : (int?)dr.GetInt32(i);
                        break;

                    case "schemadefaultconstraint":
                        SchemaDefaultConstraint = dr.IsDBNull(i) ? null : dr.GetString(i);
                        break;

                    case "defaultconstraintname":
                        DefaultConstraintName = dr.IsDBNull(i) ? null : dr.GetString(i);
                        break;

                    case "defaultconstraintvalue":
                        DefaultConstraintValue = dr.IsDBNull(i) ? null : dr.GetString(i);
                        break;

                    case "isfulltextcol":
                        IsFullTextCol = dr.IsDBNull(i) ? null : (bool?)dr.GetBoolean(i);
                        break;

                    case "fulltexttypecolumn":
                        FullTextTypeColumn = dr.IsDBNull(i) ? null : dr.GetString(i);
                        break;

                    case "idfulltextlanguage":
                        IdFullTextLanguage = dr.IsDBNull(i) ? null : (int?)dr.GetInt32(i);
                        break;

                    case "formular":
                        Formular = dr.IsDBNull(i) ? null : dr.GetString(i);
                        break;

                    case "ispersisted":
                        IsPersisted = dr.IsDBNull(i) ? null : (bool?)dr.GetBoolean(i);
                        break;

                    case "isdeterministic":
                        IsDeterministic = dr.IsDBNull(i) ? null : (bool?)dr.GetBoolean(i);
                        break;

                    case "isxmldocument":
                        IsXmlDocument = dr.IsDBNull(i) ? null : (bool?)dr.GetBoolean(i);
                        break;

                    case "schemaxmlschema":
                        SchemaXmlSchema = dr.IsDBNull(i) ? null : dr.GetString(i);
                        break;

                    case "xmlschema":
                        XmlSchema = dr.IsDBNull(i) ? null : dr.GetString(i);
                        break;
                }
            }
        }

        #endregion

        #region Public properties

        public int? IdObject
        { get; set; }

        public string ObjectSchema
        { get; set; }

        public string ObjectName
        { get; set; }

        public string ObjectType
        { get; set; }

        public bool? IsTableObject
        { get; set; }

        public int? ColumnOrdinal
        { get; set; }

        public string ColumnName
        { get; set; }

        public string SqlDbType
        { get; set; }

        public string UserDbType
        { get; set; }

        public string SchemaUserDbType
        { get; set; }

        public int? CharacterMaximumLength
        { get; set; }

        public short? MaxLength
        { get; set; }

        public byte? Precision
        { get; set; }

        public byte? Scale
        { get; set; }

        public string CollationName
        { get; set; }

        public byte?[] TdsCollation28
        { get; set; }

        public bool? AllowDBNull
        { get; set; }

        public bool? IsAnsiPadded
        { get; set; }

        public bool? IsRowGuid
        { get; set; }

        public bool? AutoIncrement
        { get; set; }

        public long? AutoIncrementSeed
        { get; set; }

        public long? AutoIncrementStep
        { get; set; }

        public string IndexName
        { get; set; }

        public byte? IndexType
        { get; set; }

        public string IndexTypeName
        { get; set; }

        public bool? IsPrimaryKey
        { get; set; }

        public bool? IsUnique
        { get; set; }

        public bool? IsIndex
        { get; set; }

        public bool? IsDisabledIndex
        { get; set; }

        public byte? KeyOrdinal
        { get; set; }

        public bool? IsDescendingKey
        { get; set; }

        public bool? IsIncludedColumn
        { get; set; }

        public byte? IndexColumnId
        { get; set; }

        public bool? IsComputed
        { get; set; }

        public bool? IsIdNotForRepl
        { get; set; }

        public bool? IsReplicated
        { get; set; }

        public bool? IsNonSqlSubscribed
        { get; set; }

        public bool? IsDtsReplicated
        { get; set; }

        public bool? IsDefault
        { get; set; }

        public string DefaultDefinition
        { get; set; }

        public int? IdRule
        { get; set; }

        public string SchemaRule
        { get; set; }

        public string RuleName
        { get; set; }

        public bool? IsDefaultConstraint
        { get; set; }

        public int? IdDefaultConstraint
        { get; set; }

        public string SchemaDefaultConstraint
        { get; set; }

        public string DefaultConstraintName
        { get; set; }

        public string DefaultConstraintValue
        { get; set; }

        public bool? IsFullTextCol
        { get; set; }

        public string FullTextTypeColumn
        { get; set; }

        public int? IdFullTextLanguage
        { get; set; }

        public string Formular
        { get; set; }

        public bool? IsPersisted
        { get; set; }

        public bool? IsDeterministic
        { get; set; }

        public bool? IsXmlDocument
        { get; set; }

        public string SchemaXmlSchema
        { get; set; }

        public string XmlSchema
        { get; set; }

        #endregion
    }
}