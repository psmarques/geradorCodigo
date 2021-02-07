using System.Collections.Generic;

namespace Gerador.Core
{
    public abstract class TemplateBase
    {
        public string GeneratedContent { get; set; }

        public string Namespace { get; set; }

        public string ClassName { get; set; }

        public string SystemName { get; set; }

        public List<TableField> Fields { get; set; }

        public string PascalCase(string txt)
        {
            if (string.IsNullOrEmpty(txt) || txt.Length < 2) return string.Empty;

            return txt[0].ToString().ToUpper() +
                   txt.Substring(1);
        }

    }

    public interface ITemplate
    {
        void Generate();
    }
}
