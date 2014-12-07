using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Messaging;
using Microsoft.VisualStudio.TextTemplating;
using YP.CodeGen.Host;
using YP.CodeGen.TemplateModel;
using YP.CodeGen.Templates;

namespace YP.CodeGen.Helper
{
    public class FileFactory
    {
        private readonly string _outputPath;
        private readonly string _templatePath;
        private readonly string _tableName;

        public FileFactory(string projectName, string tableName)
        {
            _tableName = tableName;
            _templatePath = Path.GetFullPath("./Templates/");
            _outputPath = Path.GetFullPath("./Output/" + projectName + "/");
        }

        public void RenderEntityFile(List<EntityModel> models)
        {
            if (!Directory.Exists(_outputPath + "Entity\\"))
            {
                Directory.CreateDirectory(_outputPath + "Entity\\");
            }
            var entityOutputPath = _outputPath + "Entity\\" + _tableName + "Entity.cs";
            var templateFileName = _templatePath + "Model.tt";
            var host = new CustomCmdLineHost();
            var engine = new Engine();
            CallContext.LogicalSetData("Model", models);
            host.TemplateFileValue = templateFileName;
            //Read the text template.
            string input = File.ReadAllText(templateFileName);
            //Transform the text template.
            string output = engine.ProcessTemplate(input, host);

            File.WriteAllText(entityOutputPath, output, host.FileEncoding);
            CallContext.FreeNamedDataSlot("Model");
            foreach (CompilerError error in host.Errors)
            {
                Console.WriteLine(error.ToString());
            }
        }

        public void RenderMappingFile(List<MappingModel> models)
        {
            if (!Directory.Exists(_outputPath + "Mappings\\"))
            {
                Directory.CreateDirectory(_outputPath + "Mappings\\");
            }
            var entityOutputPath = _outputPath + "Mappings\\" + _tableName + "Mapping.cs";
            var templateFileName = _templatePath + "Mapping.tt";
            var host = new CustomCmdLineHost();
            var engine = new Engine();
            CallContext.LogicalSetData("Model", models);
            host.TemplateFileValue = templateFileName;
            //Read the text template.
            string input = File.ReadAllText(templateFileName);
            //Transform the text template.
            string output = engine.ProcessTemplate(input, host);

            File.WriteAllText(entityOutputPath, output, host.FileEncoding);
            CallContext.FreeNamedDataSlot("Model");
            foreach (CompilerError error in host.Errors)
            {
                Console.WriteLine(error.ToString());
            }
        }

        public void RenderEnumFile(List<EnumModel> models)
        {
            if (!Directory.Exists(_outputPath + "Entity\\"))
            {
                Directory.CreateDirectory(_outputPath + "Entity\\");
            }
            var enumTemplate = new EnumTemplate(models);
            var output = enumTemplate.TransformText();
            var outputPath = _outputPath + "Entity\\" +"Enum" + _tableName + ".cs";
            File.WriteAllText(outputPath, output);
        }
    }
}
