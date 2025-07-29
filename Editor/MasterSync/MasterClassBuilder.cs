using System.IO;
using System.Text;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using UnityEngine;
using UnityEngine.Assertions;

namespace MyFw
{ 
    class MasterClassBuilder
    {
        private DataTableContext currentContext;
        private CodeCompileUnit compileUnit;

        public void Build(EditorMasterSyncConfig config)
        {
            foreach(var sheetName in config.sheetNameList)
            {
                var path = $"{config.outputDir}/{sheetName}.csv";
                using var sr = new StreamReader(path, Encoding.UTF8);
                var textBody = sr.ReadToEnd();

                var splitedDataList = StringUtility.SplitFromCSVText(textBody);
                Assert.IsNotNull(splitedDataList, $"{path} is not CSV format!!");

                var datatable = new DataTableContext()
                {
                    nameSpace = config.nameSpace,
                    className = sheetName,
                };
                datatable.SetHeader(splitedDataList[0]);

                BuildClassFile(datatable);
                Output(config.scriptDir);
            }
        }

        private void BuildClassFile(DataTableContext context)
        {
            this.currentContext = context;
            this.compileUnit = new CodeCompileUnit();

            // namespace 設定
            var name = new CodeNamespace(context.nameSpace);
            name.Imports.Add(new CodeNamespaceImport("System"));
            name.Imports.Add(new CodeNamespaceImport("MyFw.DS"));
            compileUnit.Namespaces.Add(name);

            // interface 設定
            var interfaceType = new CodeTypeDeclaration("I" + context.className)
            {
                IsInterface = true,
                IsPartial = true,
            };
            interfaceType.BaseTypes.Add(new CodeTypeReference("IDataRow"));
            name.Types.Add(interfaceType);

            //クラス定義 引数にはクラス名を設定
            var classType = new CodeTypeDeclaration(context.className)
            {
                IsPartial = true,
            };
            classType.BaseTypes.Add(new CodeTypeReference("I" + context.className));

            // プロパティの設定
            foreach (var prop in context.colmunContexts)
            {   
                if (string.IsNullOrEmpty(prop.name))
                {
                    continue;
                }

                // インターフェース側に定義
                interfaceType.Members.Add(new CodeMemberProperty
                {
                    HasGet = true,
                    Name = prop.name,
                    Type = new CodeTypeReference(prop.typeName),
                });
                
                // プロパティに対応するフィールドを作成（任意）
                var testValueField = new CodeMemberField(prop.typeName, prop.FieldName)
                {
                    Attributes = MemberAttributes.Family,
                };
                classType.Members.Add(testValueField);

                var property = new CodeMemberProperty
                {
                    HasGet = true,
                    Attributes = MemberAttributes.Public,
                    Name = prop.name,
                    Type = new CodeTypeReference(prop.typeName),
                };
                property.GetStatements.Add(new CodeMethodReturnStatement(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(), prop.FieldName)));

                classType.Members.Add(property);
            }

            name.Types.Add(classType);
        }

        private void Output(string path, bool useDebug = false)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var fullPath = path + "/" + this.currentContext.className + (useDebug ? ".txt" : ".cs");
            Debug.Log("Write File : " + this.currentContext.className);

            var provider = new CSharpCodeProvider();
            var option = new CodeGeneratorOptions();
            var writer = new StreamWriter(fullPath);
            provider.GenerateCodeFromCompileUnit(compileUnit, writer, option);
            writer.Flush();
        }
    }
}