using H3Engine.Components.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            Console.WriteLine("Press Any Key...");
            Console.ReadKey();
        }

        public static void TestArchiveLoader()
        {
            H3ArchiveLoader loader = new H3ArchiveLoader(@"D:\Toney\Personal\Git\toneyisnow\HeroesIII\External\HeroesIII_Data\H3ab_bmp.lod");
            loader.DumpAllFiles(@"D:\Temp\h3\H3ab_bmp");

        }

        public static void TestH3MReader()
        {

        }
    }
}
