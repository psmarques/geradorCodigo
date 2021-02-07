using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerador.Core
{
    public class GeradorHelper
    {
        public string ErrorMessage { get; set; }
        public string ConnectionString { get; set; }
        private DataTable schemaTables;
        private DataTable schemaColumns;

        public string Namepsace { get; set; }
        public string ClassName { get; set; }
        public List<TableField> Tablefields { get; set; }

        public void GetSchema()
        {

            using (var conn = new SqlConnection(ConnectionString))
            {
                var builder = new System.Data.SqlClient.SqlConnectionStringBuilder();
                builder.ConnectionString = this.ConnectionString;
                ErrorMessage = string.Empty;
                string server = builder.DataSource;
                string db = builder.InitialCatalog;

                try
                {
                    conn.Open();
                    this.schemaTables = conn.GetSchema("Tables");
                    this.schemaColumns = conn.GetSchema("Columns");
                    conn.Close();
                }
                catch (Exception exc)
                {
                    ErrorMessage = exc.Message;
                    if (conn.State == ConnectionState.Open) conn.Close();

                    throw;
                }
            }
        }

        public List<string> GetTableNames()
        {
            var lst = new List<string>();

            foreach (DataRow item in schemaTables.Rows)
            {
                lst.Add(item.ItemArray[2].ToString());
            }

            return lst;
        }

        public List<TableField> GetColumns(string tableName)
        {
            var lst = new List<TableField>();

            foreach (DataRow item in schemaColumns.Rows)
            {
                if (item[2].ToString().Equals(tableName, StringComparison.OrdinalIgnoreCase))
                {
                    int len = 60;
                    int.TryParse(item[8].ToString(), out len);

                    lst.Add(new TableField(item[3].ToString(), //ColumnName
                                           item[7].ToString(), //ColumnType
                                           item[6].ToString().Equals("YES"), //Nullable
                                           len
                                           ));
                }
            }

            return lst;
        }

        public string GerarFrontModelService()
        {
            var t = new TemplateFrontServiceTs(this.Namepsace, this.ClassName);
            t.Generate();
            return t.GeneratedContent;
        }

        public string GerarModelTS()
        {
            var t = new TemplateModelTs(this.Namepsace, this.ClassName, this.Tablefields);
            t.Generate();
            return t.GeneratedContent;
        }

        public string GerarDTO()
        {
            var t = new TemplateDTO(this.Namepsace, this.ClassName, this.Tablefields);
            t.Generate();
            return t.GeneratedContent;
        }

        public string GerarWebApiController()
        {
            var t = new TemplateWebApiController(this.Namepsace, this.ClassName);
            t.Generate();
            return t.GeneratedContent;
        }

        public string GerarFrontHtml()
        {
            var t = new TemplateFrontHtml(this.Namepsace, this.ClassName, this.Tablefields);
            t.Generate();
            return t.GeneratedContent;
        }

        public string GerarFrontComponent()
        {
            var t = new TemplateFrontComponent(this.Namepsace, this.ClassName, this.Tablefields);
            t.Generate();
            return t.GeneratedContent;
        }
    }
}
