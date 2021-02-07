using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerador.Core
{
    public class TemplateModelTs : TemplateBase, ITemplate
    {
        private const string fileName = "templates\\TemplateFrontModel.ts.txt";
        private string propertyTemplate = "            public {0}:String," + Environment.NewLine;
        private StringBuilder propertysGenerated;

        //    public id: String,

        public TemplateModelTs()
        {
            GeneratedContent = string.Empty;
            Namespace = string.Empty;
            ClassName = string.Empty;
            Fields = new List<TableField>();
        }

        public TemplateModelTs(string nspace, string classname, List<TableField> fields)
        {
            GeneratedContent = string.Empty;
            Namespace = nspace;
            ClassName = classname;
            Fields = fields;
        }

        private void GenerateFields()
        {
            propertysGenerated = new StringBuilder();

            foreach (var item in Fields)
                propertysGenerated.AppendFormat(propertyTemplate, item.CodeName);
        }

        public void Generate()
        {
            GeneratedContent = File.ReadAllText(fileName);
            GeneratedContent = GeneratedContent.Replace("{namespace}", Namespace);
            GeneratedContent = GeneratedContent.Replace("{classname}", ClassName.ToLower());
            GeneratedContent = GeneratedContent.Replace("{Classname}", PascalCase(ClassName));

            GenerateFields();
            string str = propertysGenerated.ToString().TrimEnd(Environment.NewLine.ToArray());
            str = str.TrimEnd(',');

            GeneratedContent = GeneratedContent.Replace("{methods}", str);
        }
    }
}
