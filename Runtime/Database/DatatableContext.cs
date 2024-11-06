using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MyFw
{
    public struct PropertyContext
    {
        public string name;
        public string typeName;

        public string FieldName => "field" + this.name;
    }

    public struct DataTableContext
    {
        public string nameSpace;
        public string className;
        public List<PropertyContext> colmunContexts;

        public void SetHeader(string[] colmuns)
        {
            this.colmunContexts = colmuns
                .Select(c => Regex.Match(c, @"(\w+?)\[(.*?)\]"))
                .Select(m => new PropertyContext()
                {
                    name = m.Groups.Count > 1 ? m.Groups[1].ToString() : string.Empty,
                    typeName = m.Groups.Count > 2 ? m.Groups[2].ToString() : string.Empty,
                })
                .ToList();
        }
    }
}