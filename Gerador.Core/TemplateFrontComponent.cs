using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerador.Core
{
    public class TemplateFrontComponent : TemplateBase, ITemplate
    {
        private const string fileName = "templates\\TemplateFrontComponent.txt";
        private string propertyTemplate = "            public {0} {1} {{ get; set; }}" + Environment.NewLine;
        private StringBuilder propertysGenerated;


      //  propertyForm

      //        id: [],
      //primeiroNome: ['', Validators.compose([Validators.required, Validators.minLength(3)])],
      //ultimoNome: ['', Validators.compose([Validators.required, Validators.minLength(3)])],
      //email: ['', Validators.compose([Validators.required, Validators.minLength(5)])],
      //telefoneDDD: ['', Validators.compose([Validators.min(1), Validators.max(99)])],
      //telefoneNumero: ['', Validators.compose([Validators.min(10000000), Validators.max(999999999)])]


        public TemplateFrontComponent()
        {
            GeneratedContent = string.Empty;
            Namespace = string.Empty;
            ClassName = string.Empty;
            Fields = new List<TableField>();
        }
        public TemplateFrontComponent(string nspace, string classname, List<TableField> fields)
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
