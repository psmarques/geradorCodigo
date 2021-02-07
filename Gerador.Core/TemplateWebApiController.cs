using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerador.Core
{
    public class TemplateWebApiController : TemplateBase, ITemplate
    {
        private string fileName = "templates\\TemplateWebApi.Controller.txt";

        public TemplateWebApiController(string nspace, string classname)
        {
            GeneratedContent = string.Empty;
            Namespace = nspace;
            ClassName = classname;
        }

        public void Generate()
        {
            GeneratedContent = File.ReadAllText(fileName);

            GeneratedContent = GeneratedContent.Replace("{namespace}", Namespace);
            GeneratedContent = GeneratedContent.Replace("{classname}", ClassName);
            GeneratedContent = GeneratedContent.Replace("{Classname}", PascalCase(ClassName));
        }
    }
}
