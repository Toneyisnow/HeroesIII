using FreeImageAPI;
using H3Engine.Components.FileSystem;
using H3Engine.Components.Mapping;
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
            TestPCXFile();

            Console.WriteLine("Press Any Key...");
            Console.ReadKey();
        }

        public static void TestArchiveLoader()
        {
            H3ArchiveLoader loader = new H3ArchiveLoader(@"D:\Toney\Personal\Git\toneyisnow\HeroesIII\External\HeroesIII_Data\H3ab_bmp.lod");
            loader.DumpAllFiles(@"D:\Temp\h3\H3ab_bmp");

        }

        public static void TestPCXFile()
        {
            using (FileStream dataFile = new FileStream(@"D:\Temp\h3\H3ab_bmp_bak\ArA_CoBl.pcx", FileMode.Open, FileAccess.Read))
            {
                BinaryReader reader = new BinaryReader(dataFile);

                int size = reader.ReadInt32();
                int width = reader.ReadInt32();
                int height = reader.ReadInt32();

                byte[] data = new byte[width * height * 4];
                
                if (size == width * height)
                {
                    byte[] red = new byte[256];
                    byte[] green = new byte[256];
                    byte[] blue = new byte[256];

                    // Read pallete first
                    dataFile.Seek(size, SeekOrigin.Current);
                    for(int i = 0; i < 256; i++)
                    {
                        red[i] = reader.ReadByte();
                        green[i] = reader.ReadByte();
                        blue[i] = reader.ReadByte();
                    }

                    dataFile.Seek(12, SeekOrigin.Begin);
                    int o = 0;
                    for (int i = 0; i < width * height; i++)
                    {
                        byte colorIndex = reader.ReadByte();
                        data[o++] = red[colorIndex];
                        data[o++] = green[colorIndex];
                        data[o++] = blue[colorIndex];
                        data[o++] = 0;
                    }
                }
                else
                {
                    int o = 0;
                    for (int i = 0; i < width * height; i++)
                    {
                        data[o++] = reader.ReadByte();
                        data[o++] = reader.ReadByte();
                        data[o++] = reader.ReadByte();
                        data[o++] = 0;
                    }
                }

                unsafe
                {
                    fixed (byte* ptr = data)
                    {

                        using (Bitmap image = new Bitmap(width, height, width * 4,
                                    PixelFormat.Format32bppRgb, new IntPtr(ptr)))
                        {
                            image.Save(@"D:\Temp\h3\out.png", ImageFormat.Png);
                        }
                    }
                }
            }
        }

        public static void SaveBitmap(string fileName, int width, int height, byte[] imageData)
        {

            byte[] data = new byte[width * height * 4];
            int o = 0;
            for (int i = 0; i < width * height; i++)
            {
                byte value = imageData[i];

                data[o++] = value;
                data[o++] = value;
                data[o++] = value;
                data[o++] = 0;
            }

            unsafe
            {
                fixed (byte* ptr = data)
                {

                    using (Bitmap image = new Bitmap(width, height, width * 4,
                                PixelFormat.Format32bppRgb, new IntPtr(ptr)))
                    {

                        image.Save(Path.ChangeExtension(fileName, ".jpg"));
                    }
                }
            }
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
