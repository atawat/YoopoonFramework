using System.Collections.Generic;
using System.IO;
using YP.CodeGen.TemplateModel;
using YP.CodeGen.Templates;

namespace YP.CodeGen.Helper
{
    public class FileFactory
    {
        private readonly string _outputPath;
        private readonly string _templatePath;
        private readonly string _tableName;
        private readonly string _projectName;

        public FileFactory(string projectName, string tableName)
        {
            _projectName = projectName;
            _tableName = tableName;
            _templatePath = Path.GetFullPath("./Templates/");
            _outputPath = Path.GetFullPath("./Output/" + projectName + "/");
        }

        public void RenderEntityFile(List<EntityModel> models)
        {
            if (!Directory.Exists(_outputPath + "Entity\\"+ _tableName +"\\"))
            {
                Directory.CreateDirectory(_outputPath + "Entity\\" + _tableName + "\\");
            }
            var entityOutputPath = _outputPath + "Entity\\" + _tableName + "\\" + _tableName + "Entity.cs";
            var modelTemplate = new ModelTemplate(models, _tableName,_projectName);
            var output = modelTemplate.TransformText();
            File.WriteAllText(entityOutputPath, output);
        }

        public void RenderMappingFile(List<MappingModel> models,List<EntityModel> eModels)
        {
            if (!Directory.Exists(_outputPath + "Mappings\\" + _tableName + "\\"))
            {
                Directory.CreateDirectory(_outputPath + "Mappings\\" + _tableName + "\\");
            }
            var outputPath = _outputPath + "Mappings\\" + _tableName + "\\" + _tableName + "Mapping.cs";
            var mappingTemplate = new MappingTemplate(models,eModels,_projectName,_tableName);
            var output = mappingTemplate.TransformText();
            File.WriteAllText(outputPath, output);
        }

        public void RenderEnumFile(List<EnumModel> models)
        {
            if (!Directory.Exists(_outputPath + "Entity\\" + _tableName + "\\"))
            {
                Directory.CreateDirectory(_outputPath + "Entity\\" + _tableName + "\\");
            }
            var enumTemplate = new EnumTemplate(models,_projectName);
            var output = enumTemplate.TransformText();
            var outputPath = _outputPath + "Entity\\" + _tableName + "\\" + "Enum" + _tableName + ".cs";
            File.WriteAllText(outputPath, output);
        }

        public void RenderSearchFile(List<SearchModel> models)
        {
            if (!Directory.Exists(_outputPath + "Entity\\" + _tableName + "\\"))
            {
                Directory.CreateDirectory(_outputPath + "Entity\\" + _tableName + "\\");
            }
            var searchTemplate = new ConditionTemplate(models,_tableName,_projectName);
            var output = searchTemplate.TransformText();
            var outputPath = _outputPath + "Entity\\" + _tableName + "\\" + _tableName + "SearchConditon.cs";
            File.WriteAllText(outputPath, output);
        }
    }
}
