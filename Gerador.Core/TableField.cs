using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerador.Core
{
    public class TableField
    {
        public string Name { get; private set; }

        public string CodeName { get; set; }

        public string Type { get; private set; }

        public string CodeType { get; set; }

        public bool Nullable { get; set; }

        public int Length { get; set; }

        public TableField(string name, string stype, bool nl, int length)
        {
            Name = name;
            CodeName = name.Replace("_", string.Empty);
            Type = stype;
            CodeType = TranslateType(stype);
            Nullable = nl;
            this.Length = length;
        }

        public TableField(string name, string codeName, string type, string codeType, bool nl, int length)
        {
            Name = name;
            CodeName = codeName;
            Type = type;
            CodeType = codeType;
            Nullable = nl;
        }

        private string TranslateType(string dbType)
        {
            switch (dbType.ToLower())
            {
                case "char":
                case "varchar":
                case "nvarchar":
                case "text": return "string";

                case "int":
                case "int16":
                case "int32":
                case "int64": return "int";

                case "bit": return "bool";

                case "float": return "float";

                case "double": return "double";

                case "decimal": return "decimal";

                case "varbinary": return "byte[]";

                default: return dbType;
            }
        }
    }
}
