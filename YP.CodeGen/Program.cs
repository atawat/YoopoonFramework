using System;
using System.IO;
using System.Linq;
using YP.CodeGen.ExcelModel;
using YP.CodeGen.Helper;

namespace YP.CodeGen
{
    public class Program
    {
        static void Main(string[] args)
        {
            string excelDirPath = Path.GetFullPath("./DesignExcel/");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("          优         优                      朋朋朋朋    朋朋朋朋");
            Console.WriteLine("        优           优   优                 朋    朋    朋    朋");
            Console.WriteLine("      优             优    优                朋    朋    朋    朋");
            Console.WriteLine("    优优     优优优优优优优优优              朋朋朋朋    朋朋朋朋");
            Console.WriteLine("  优  优            优优                     朋    朋    朋    朋");
            Console.WriteLine("      优           优 优                     朋    朋    朋    朋");
            Console.WriteLine("      优          优  优                     朋朋朋朋    朋朋朋朋");
            Console.WriteLine("      优         优   优                     朋    朋    朋    朋");
            Console.WriteLine("      优        优    优                     朋    朋    朋    朋");
            Console.WriteLine("      优       优     优    优               朋    朋    朋    朋");
            Console.WriteLine("      优      优       优  优               朋  朋 朋   朋     朋");
            Console.WriteLine("      优     优         优优              朋      朋  朋     朋朋");
            Console.WriteLine("----------------------------代码生成器---------------------------");
            Console.ForegroundColor = ConsoleColor.White;
            //var files = Directory.GetFiles(excelDirPath);
            var folder = new DirectoryInfo(excelDirPath);
            var excelFile = folder.GetFiles("*.xlsx");
            if (excelFile.Any())
            {
                Console.WriteLine("查找到的Excel设计文件如下:" + Environment.NewLine);
                int i = 0;
                foreach (var file in excelFile)
                {
                    Console.WriteLine(i + "…………" + file.Name);
                    i++;
                }
            }
            Console.Write("请输入需要生成代码的文件序号：");
            var index = Console.ReadLine();
            if (string.IsNullOrEmpty(index))
                return;
            var factModel = new WorkGroupModel
            {
                ProjectName = Path.GetFileNameWithoutExtension(excelFile[int.Parse(index)].FullName)
            };
            using (var helper = new ExcelHelper(excelFile[int.Parse(index)].FullName))
            {
                var model = helper.CreateModels();
                factModel.Sheets = model;
            }
            Console.WriteLine("模板读取完毕，开始生成代码");
            if (CreateCodeFiles(factModel))
                Console.WriteLine("代码生成完毕");
        }

        private static bool CreateCodeFiles(WorkGroupModel model)
        {
            try
            {
                foreach (var sheet in model.Sheets)
                {
                    var fac = new FileFactory(model.ProjectName, sheet.TabbleEnName);
                    fac.RenderEntityFile(sheet.Entity);
                    fac.RenderMappingFile(sheet.Mapping,sheet.Entity);
                    fac.RenderEnumFile(sheet.Enums);
                    fac.RenderSearchFile(sheet.Search);
                    fac.RenderServiceFile(sheet.Search);
                }
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }
    }
}
