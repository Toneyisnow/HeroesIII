using H3MapLoader.Components.FileSystem;
using H3MapLoader.Components.Mapping;
using H3MapLoader.Mapping;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3MapLoader
{
    class Program
    {
        static bool map_twoLevel = false;
        static UInt32 map_height = 0;


        static void Main(string[] args)
        {
            TestCompressedStream();


            Console.ReadKey();
        }


        static void Test()
        {
            //// string pathSource = @"D:\Toney\Personal\Git\toneyisnow\HeroesIII\MapLoader\maps\HoMM3 Map Pack by HoMMdb\Shadow of Death & HoMM3 Complete\2 FRIENDS\2 FRIENDS.h3m";
            //// string pathSource = @"D:\Toney\Personal\Git\toneyisnow\HeroesIII\MapLoader\maps\HoMM3 Map Pack by HoMMdb\Shadow of Death & HoMM3 Complete\Abilities\Abilities.h3m";
            //// string pathSource = @"D:\Toney\Personal\Git\toneyisnow\HeroesIII\MapLoader\maps\HoMM3 Map Pack by HoMMdb\Shadow of Death & HoMM3 Complete\Astrogonia\Astrogonia.h3m";
            //// string pathSource = @"D:\Toney\Personal\Git\toneyisnow\HeroesIII\MapLoader\maps\HoMM3 Map Pack by HoMMdb\Armageddon's Blade\Riverworld\Riverworld";
            string pathSource = @"D:\Toney\Personal\Git\toneyisnow\HeroesIII\MapLoader\maps\HoMM3 Map Pack by HoMMdb\Shadow of Death & HoMM3 Complete\Andrews Exploits\Andrews Exploits";


            MapLoaderH3M loader = new MapLoaderH3M(pathSource);

            HMMap map = loader.LoadMap();


        }
        
        static void TestCompressedStream()
        {
            string pathSource = @"D:\Toney\Personal\Git\toneyisnow\HeroesIII\MapLoader\maps\HoMM3 Map Pack by HoMMdb\Shadow of Death & HoMM3 Complete\Andrews Exploits\Andrews Exploits";

            using (FileStream sw = new FileStream(@"D:\Temp\compressedout.out", FileMode.Create, FileAccess.Write))
            {
                using (BinaryFileReader fileReader = new BinaryFileReader(pathSource))
                {
                    using (CompressedStreamReader reader = new CompressedStreamReader(fileReader, false, 100000))
                    {
                        int batchSize = 300;
                        byte[] tempData = new byte[batchSize];
                        ulong readSize = 0;
                        int count = 0;
                        do
                        {
                            readSize = reader.Read(tempData, (ulong)batchSize);
                            sw.Write(tempData, 0, (int)readSize);
                        }
                        while (readSize > 0 && count ++ < 100);
                    }
                }
            }

        }


        static void TestStream()
        {
            MemoryStream memory = new MemoryStream();
            byte[] bytes = new byte[4] { (byte)'0', (byte)'1', (byte)'2', (byte)'3' };

            memory.Write(bytes, 0, 4);
            memory.Write(bytes, 2, 2);

            Console.WriteLine("Length: " + memory.Length);

            memory.Seek(0, SeekOrigin.Begin);

            byte[] output = new byte[4];
            memory.Read(output, 0, 4);
            Console.WriteLine(string.Format("Output: {0} {1} {2} {3}", output[0], output[1], output[2], output[3]));

            int size = memory.Read(output, 0, 4);
            Console.WriteLine(string.Format("Output: {0} {1} {2} {3} (Size={4})", output[0], output[1], output[2], output[3], size));
        }
        /// /////////////////////////////////////////////////////////



    }


}
