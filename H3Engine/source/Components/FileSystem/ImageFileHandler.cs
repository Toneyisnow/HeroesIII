using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.Components.FileSystem
{
    public class H3PcxFileHandler
    {
        /// <summary>
        /// Check whether a stream format is PCX: [size][width][height][DATA]
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static bool IsPCX(Stream stream)
        {
            long position = stream.Position;
            BinaryReader reader = new BinaryReader(stream);

            int size = reader.ReadInt32();
            int width = reader.ReadInt32();
            int height = reader.ReadInt32();

            stream.Seek(position, SeekOrigin.Begin);

            return (size == width * height || size == width * height * 3);
        }

        /// <summary>
        /// Save data from H3 files to PNG format stream
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="outputStream"></param>
        public static ImageData ExtractPCXStream(Stream inputStream)
        {
            BinaryReader reader = new BinaryReader(inputStream);

            int size = reader.ReadInt32();
            int width = reader.ReadInt32();
            int height = reader.ReadInt32();

            ImageData image = new ImageData(width, height);
            /// byte[] data = new byte[width * height * 4];

            if (size == width * height)
            {
                byte[] red = new byte[256];
                byte[] green = new byte[256];
                byte[] blue = new byte[256];

                // Read pallete first
                inputStream.Seek(size, SeekOrigin.Current);
                List<byte> transparentColorIndexes = new List<byte>();
                for (int i = 0; i < 256; i++)
                {
                    blue[i] = reader.ReadByte();
                    green[i] = reader.ReadByte();
                    red[i] = reader.ReadByte();

                    if (red[i] == 0 && blue[i] == 255 && green[i] == 255)
                    {
                        transparentColorIndexes.Add((byte)i);
                    }
                }

                inputStream.Seek(12, SeekOrigin.Begin);
                int o = 0;
                for (int i = 0; i < width * height; i++)
                {
                    byte colorIndex = reader.ReadByte();

                    if (colorIndex == 0 || transparentColorIndexes.Contains(colorIndex))
                    {
                        // Transparent bit
                        image.WriteData(0, 0, 0, 255);
                        //data[o++] = 0;
                        //data[o++] = 0;
                        //data[o++] = 0;
                        //data[o++] = 255;
                    }
                    else
                    {
                        image.WriteData(red[colorIndex], green[colorIndex], blue[colorIndex], 0);
                        //data[o++] = red[colorIndex];
                        //data[o++] = green[colorIndex];
                        //data[o++] = blue[colorIndex];
                        //data[o++] = 0;
                    }
                }
            }
            else
            {
                int o = 0;
                for (int i = 0; i < width * height; i++)
                {
                    var r = reader.ReadByte();
                    var g = reader.ReadByte();
                    var b = reader.ReadByte();
                    image.WriteData(r, g, b, 0);
                    //data[o++] = reader.ReadByte();
                    //data[o++] = reader.ReadByte();
                    //data[o++] = reader.ReadByte();
                    //data[o++] = 0;
                }
            }

            return image;
        }

    }
}
