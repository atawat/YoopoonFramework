namespace YP.CodeGen.Templates
{
    public partial class IServiceTemplate
    {
        private readonly string _projectName;
        private readonly string _entityName;
        public IServiceTemplate(string projecctName,string entityName)
        {
            _projectName = projecctName;
            _entityName = entityName;
        }
    }
}
