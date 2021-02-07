using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Gerador.Core
{
    public class TemplateDTO : TemplateBase, ITemplate
    {
        private const string fileName = "templates\\TemplateDTO.txt";
        private string propertyTemplate = "            public {0} {1} {{ get; set; }}" + Environment.NewLine;
        private StringBuilder propertysGenerated;

        public TemplateDTO()
        {
            GeneratedContent = string.Empty;
            Namespace = string.Empty;
            ClassName = string.Empty;
            Fields = new List<TableField>();
        }

        public TemplateDTO(string nspace, string classname, List<TableField> fields)
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
                propertysGenerated.AppendFormat(propertyTemplate, "string", PascalCase(item.CodeName));
        }

        public void Generate()
        {
            GeneratedContent = File.ReadAllText(fileName);

            GeneratedContent = GeneratedContent.Replace("{namespace}", Namespace);
            GeneratedContent = GeneratedContent.Replace("{classname}", ClassName);
            GeneratedContent = GeneratedContent.Replace("{Classname}", PascalCase(ClassName));

            GenerateFields();
            GeneratedContent = GeneratedContent.Replace("{methods}", propertysGenerated.ToString());
        }


    }
}
