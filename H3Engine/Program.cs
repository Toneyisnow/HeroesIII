using H3Engine.Components.FileSystem;
using H3Engine.Components.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            TestH3MReader();

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
            // H3MapLoader mapLoader = new H3MapLoader(@"D:\Toney\Personal\Git\toneyisnow\HeroesIII\MapLoader\maps\Coop_Campaing_Shadow_of_Death\Sandro B\Sandro B");
            H3MapLoader mapLoader = new H3MapLoader(@"D:\Toney\Personal\Git\toneyisnow\HeroesIII\MapLoader\maps\HoMM3 Map Pack by HoMMdb\Shadow of Death & HoMM3 Complete\Andrews Exploits\Andrews Exploits");
            H3Map map = mapLoader.LoadMap();

        }

        public static void TestGZip()
        {
            string filename = @"D:\Toney\Personal\Git\toneyisnow\HeroesIII\MapLoader\maps\HoMM3 Map Pack by HoMMdb\Shadow of Death & HoMM3 Complete\Andrews Exploits.h3m";
            using (FileStream input = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                using (FileStream output = new FileStream(filename + ".out", FileMode.Create, FileAccess.Write))
                {
                    using (CompressedStreamReader compressedStream = new CompressedStreamReader(input, true))
                    {
                        ulong bucketSize = 1024;
                        byte[] buffer = new byte[bucketSize];
                        ulong readSize = 0;

                        do
                        {
                            readSize = compressedStream.Read(buffer, bucketSize);
                            if (readSize > 0)
                            {
                                output.Write(buffer, 0, (int)readSize);
                            }
                        }
                        while (readSize > 0);
                    }
                }
            }
        }

    }
}
