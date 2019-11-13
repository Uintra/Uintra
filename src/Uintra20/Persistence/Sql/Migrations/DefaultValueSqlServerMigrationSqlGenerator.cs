using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.SqlServer;

namespace Uintra20.Persistence.Migrations
{
    public class DefaultValueSqlServerMigrationSqlGenerator : SqlServerMigrationSqlGenerator
    {
        private int dropConstraintCount = 0;

        protected override void Generate(AddColumnOperation addColumnOperation)
        {
            SetAnnotatedColumn(addColumnOperation.Column, addColumnOperation.Table);
            base.Generate(addColumnOperation);
        }

        protected override void Generate(AlterColumnOperation alterColumnOperation)
        {
            SetAnnotatedColumn(alterColumnOperation.Column, alterColumnOperation.Table);
            base.Generate(alterColumnOperation);
        }

        protected override void Generate(CreateTableOperation createTableOperation)
        {
            SetAnnotatedColumns(createTableOperation.Columns, createTableOperation.Name);
            base.Generate(createTableOperation);
        }

        protected override void Generate(AlterTableOperation alterTableOperation)
        {
            SetAnnotatedColumns(alterTableOperation.Columns, alterTableOperation.Name);
            base.Generate(alterTableOperation);
        }

        private void SetAnnotatedColumn(ColumnModel column, string tableName)
        {
            AnnotationValues values;
            if (column.Annotations.TryGetValue("SqlDefaultValue", out values))
            {
                if (values.NewValue == null)
                {
                    column.DefaultValueSql = null;
                    using (var writer = Writer())
                    {
                        // Drop Constraint
                        writer.WriteLine(GetSqlDropConstraintQuery(tableName, column.Name));
                        Statement(writer);
                    }
                }
                else
                {
                    column.DefaultValueSql = (string)values.NewValue;
                }
            }
        }

        private void SetAnnotatedColumns(IEnumerable<ColumnModel> columns, string tableName)
        {
            foreach (var column in columns)
            {
                SetAnnotatedColumn(column, tableName);
            }
        }

        private string GetSqlDropConstraintQuery(string tableName, string columnName)
        {
            var tableNameSplittedByDot = tableName.Split('.');
            var tableSchema = tableNameSplittedByDot[0];
            var tablePureName = tableNameSplittedByDot[1];

            var str = $@"DECLARE @var{dropConstraintCount} nvarchar(128)
                SELECT @var{dropConstraintCount} = name
                FROM sys.default_constraints
                WHERE parent_object_id = object_id(N'{tableSchema}.[{tablePureName}]')
                AND col_name(parent_object_id, parent_column_id) = '{columnName}';
                IF @var{dropConstraintCount} IS NOT NULL
                    EXECUTE('ALTER TABLE {tableSchema}.[{tablePureName}] DROP CONSTRAINT [' + @var{dropConstraintCount} + ']')";

            dropConstraintCount = dropConstraintCount + 1;
            return str;
        }
    }
}