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

        private string NormalizeTableName
        {
            get { return _tableName.Replace("_", ""); }
        }

        public FileFactory(string projectName, string tableName)
        {
            _projectName = projectName;
            _tableName = tableName;
            _templatePath = Path.GetFullPath("./Templates/");
            _outputPath = Path.GetFullPath("./Output/" + projectName + "/");
        }

        public void RenderEntityFile(List<EntityModel> models)
        {
            if (!Directory.Exists(_outputPath + "Entity\\" + NormalizeTableName + "\\"))
            {
                Directory.CreateDirectory(_outputPath + "Entity\\" + NormalizeTableName + "\\");
            }
            var entityOutputPath = _outputPath + "Entity\\" + NormalizeTableName + "\\" + NormalizeTableName + "Entity.cs";
            var modelTemplate = new ModelTemplate(models, NormalizeTableName, _projectName);
            var output = modelTemplate.TransformText();
            File.WriteAllText(entityOutputPath, output);
        }

        public void RenderMappingFile(List<MappingModel> models,List<EntityModel> eModels)
        {
            if (!Directory.Exists(_outputPath + "Mappings\\" + NormalizeTableName + "\\"))
            {
                Directory.CreateDirectory(_outputPath + "Mappings\\" + NormalizeTableName + "\\");
            }
            var outputPath = _outputPath + "Mappings\\" + NormalizeTableName + "\\" + NormalizeTableName + "Mapping.cs";
            var mappingTemplate = new MappingTemplate(models, eModels, _projectName, _tableName);
            var output = mappingTemplate.TransformText();
            File.WriteAllText(outputPath, output);
        }

        public void RenderEnumFile(List<EnumModel> models)
        {
            if (!Directory.Exists(_outputPath + "Entity\\" + NormalizeTableName + "\\"))
            {
                Directory.CreateDirectory(_outputPath + "Entity\\" + NormalizeTableName + "\\");
            }
            var enumTemplate = new EnumTemplate(models,_projectName);
            var output = enumTemplate.TransformText();
            var outputPath = _outputPath + "Entity\\" + NormalizeTableName + "\\" + "Enum" + NormalizeTableName + ".cs";
            File.WriteAllText(outputPath, output);
        }

        public void RenderSearchFile(List<SearchModel> models)
        {
            if (!Directory.Exists(_outputPath + "Entity\\" + NormalizeTableName + "\\"))
            {
                Directory.CreateDirectory(_outputPath + "Entity\\" + NormalizeTableName + "\\");
            }
            var searchTemplate = new ConditionTemplate(models, NormalizeTableName, _projectName);
            var output = searchTemplate.TransformText();
            var outputPath = _outputPath + "Entity\\" + NormalizeTableName + "\\" + NormalizeTableName + "SearchConditon.cs";
            File.WriteAllText(outputPath, output);
        }

        public void RenderServiceFile(List<SearchModel> models)
        {
            if (!Directory.Exists(_outputPath + "Services\\" + NormalizeTableName + "\\"))
            {
                Directory.CreateDirectory(_outputPath + "Services\\" + NormalizeTableName + "\\");
            }
            var serviceTemplate = new ServiceTemplate(_projectName,_tableName.Replace("_",""),models);
            var output = serviceTemplate.TransformText();
            var outputPath = _outputPath + "Services\\" + NormalizeTableName + "\\" + NormalizeTableName + "Service.cs";
            File.WriteAllText(outputPath, output);

            var interfaceTemplate = new IServiceTemplate(_projectName, NormalizeTableName);
            var iOutPut = interfaceTemplate.TransformText();
            var iOutPutPath = _outputPath + "Services\\" + NormalizeTableName + "\\I" + NormalizeTableName + "Service.cs";
            File.WriteAllText(iOutPutPath, iOutPut);
        }

        public void RenderControllerFile( List<SearchModel> search, List<EntityModel> entity)
        {
            if (!Directory.Exists(_outputPath + "Controller\\"))
            {
                Directory.CreateDirectory(_outputPath + "Controller\\");
            }
            var controllerTemplate = new ControllerTemplate(_projectName,_tableName,search,entity);
            var output = controllerTemplate.TransformText();
            var outputpath = _outputPath + "Controller\\" + NormalizeTableName + "Controller.cs";
            File.WriteAllText(outputpath,output);
        }

        public void RenderViewModel(List<EntityModel> entity, List<EnumModel> enums)
        {
            if (!Directory.Exists(_outputPath + "Model\\"))
            {
                Directory.CreateDirectory(_outputPath + "Model\\");
            }
            var controllerTemplate = new ViewModelTemplate(_projectName, _tableName, enums, entity);
            var output = controllerTemplate.TransformText();
            var outputpath = _outputPath + "Model\\" + NormalizeTableName + "Model.cs";
            File.WriteAllText(outputpath, output);
        }
    }
}
