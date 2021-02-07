using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerador.Core
{
    public class TemplateFrontHtml : TemplateBase, ITemplate
    {
        private const string fileName = "templates\\TemplateFrontHtml.txt";
        private string propertyTemplate = "            {{ item.{0} }}" + Environment.NewLine;
        private StringBuilder propertysGenerated;

        private string diveditor_template = Environment.NewLine +
                                            "      <div class=\"form-group\">" + Environment.NewLine +
                                            "        <label for=\"{0}\">{1}: </label>" + Environment.NewLine +
                                            "        <input type=\"text\" class=\"form-control\" maxlength=\"{2}\" formControlName=\"{0}\" id=\"{0}\">" + Environment.NewLine +
                                            " " + Environment.NewLine +
                                            "        <small class=\"danger\" *ngIf=\"form.controls.{0}.invalid && !form.controls.{0}.pristine\">" + Environment.NewLine +
                                            "          {1} Inválido!" + Environment.NewLine +
                                            "        </small>" + Environment.NewLine +
                                            "      </div>";

        private StringBuilder diveditor;

        public TemplateFrontHtml()
        {
            GeneratedContent = string.Empty;
            Namespace = string.Empty;
            ClassName = string.Empty;
            Fields = new List<TableField>();
        }
        public TemplateFrontHtml(string nspace, string classname, List<TableField> fields)
        {
            GeneratedContent = string.Empty;
            Namespace = nspace;
            ClassName = classname;
            Fields = fields;
        }

        private void GenerateFields()
        {
            propertysGenerated = new StringBuilder();
            diveditor = new StringBuilder();

            foreach (var item in Fields)
            {
                propertysGenerated.AppendFormat(propertyTemplate, item.CodeName);

                if (string.Equals(item.Name, "id", StringComparison.OrdinalIgnoreCase))
                    continue;

                diveditor.AppendFormat(diveditor_template, item.CodeName, item.Name, item.Length);
            }
        }

        public void Generate()
        {
            GeneratedContent = File.ReadAllText(fileName);

            GeneratedContent = GeneratedContent.Replace("{namespace}", Namespace);
            GeneratedContent = GeneratedContent.Replace("{classname}", ClassName);
            GeneratedContent = GeneratedContent.Replace("{Classname}", PascalCase(ClassName));

            GenerateFields();
            GeneratedContent = GeneratedContent.Replace("{listgrid}", propertysGenerated.ToString());
            GeneratedContent = GeneratedContent.Replace("{diveditor}", diveditor.ToString());
        }
    }
}
