using FreeImageAPI;
using H3Engine.FileSystem;
using H3Engine.GUI;
using H3Engine.Mapping;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
            // H3ArchiveLoader loader = new H3ArchiveLoader(@"D:\Toney\Personal\Git\toneyisnow\HeroesIII\External\HeroesIII_Data\H3ab_bmp.lod");
            // H3ArchiveLoader loader = new H3ArchiveLoader(@"D:\GitRoot\toneyisnow\HeroesIII\External\HeroesIII_Data\H3ab_bmp.lod");
            H3ArchiveLoader loader = new H3ArchiveLoader(@"D:\PlayGround\SOD_Data\H3sprite.lod");
            loader.DumpAllFiles(@"D:\PlayGround\H3sprite");
        }
        
        public static void TestH3MReader()
        {
            // H3MapLoader mapLoader = new H3MapLoader(@"D:\Toney\Personal\Git\toneyisnow\HeroesIII\MapLoader\maps\Coop_Campaing_Shadow_of_Death\Sandro B\Sandro B");
            // string file = @"D:\Toney\Personal\Git\toneyisnow\HeroesIII\MapLoader\maps\HoMM3 Map Pack by HoMMdb\Shadow of Death & HoMM3 Complete\Astral Venice\Astral Venice.h3m";
            string file = @"D:\GitRoot\toneyisnow\HeroesIII\MapLoader\maps\HoMM3 Map Pack by HoMMdb\Shadow of Death & HoMM3 Complete\Podarok 2001 eng\Podarok 2001 eng.h3m";
            H3MapLoader mapLoader = new H3MapLoader(file);
            H3Map map = mapLoader.LoadMap();

        }

        public static void TestH3DefHandler()
        {
            using (FileStream file = new FileStream(@"D:\PlayGround\H3ab_spr\AVG2elw.def", FileMode.Open, FileAccess.Read))
            {
                H3DefFileHandler def = new H3DefFileHandler(file);

                def.LoadAllFrames();

                AnimationDefinition animation = def.GetAnimation();

                for (int i = 0; i < animation.Groups[0].Frames.Count; i++)
                {
                    ImageData image = animation.ComposeFrameImage(0, i);
                    using (FileStream outputFile = new FileStream(@"D:\PlayGround\H3ab_spr\AVG2elw." + i + ".png", FileMode.Create, FileAccess.Write))
                    {
                        image.SaveAsPng(outputFile);
                    }
                }
            }
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
