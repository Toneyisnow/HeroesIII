using ComponentAce.Compression.Archiver;
using ComponentAce.Compression.Libs.ZLib;
using ComponentAce.Compression.ZipForge;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace H3Engine.Components.FileSystem
{
    public class FileInfo
    {
        public string FileName
        {
            get; set;
        }

        public uint Offset
        {
            get; set;
        }

        public uint Size
        {
            get; set;
        }
        public uint CSize
        {
            get; set;
        }

    }
    public class H3ArchiveLoader
    {

    }






    class Program
    {
        static void Main(string[] args)
        {
            TestZStream();

            Console.WriteLine("Press Any Key...");
            Console.ReadKey();
        }

        static void TestZStream()
        {
            ////string filename = @"C:\Users\charl\source\repos\ConsoleApp3\bin\Debug\output\AdvEvent.txt.zip";
            string filename = @"C:\Users\charl\source\repos\ConsoleApp3\bin\Debug\output\Dwelling.txt.zip";
            using (FileStream output = new FileStream(@"D:\Temp\Dwelling.txt", FileMode.Create, FileAccess.Write))
            using (BinaryFileReader reader = new BinaryFileReader(filename))
            {
                using (CompressedStreamReader compressedStream = new CompressedStreamReader(reader, false, 500000))
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
                            output.Flush();
                        }
                    }
                    while (readSize > 0);
                }
            }
        }

        static void TestUnzip()
        {
            ZipForge archiver = new ZipForge();
            try
            {
                // The name of the ZIP file to unzip
                archiver.FileName = @"C:\Users\charl\source\repos\ConsoleApp3\bin\Debug\output\AdvEvent.txt.zip";
                // Open an existing archive
                archiver.OpenArchive(System.IO.FileMode.Open);
                // Default path for all operations                
                archiver.BaseDir = @"D:\Temp";
                // Extract all files from the archive to C:\Temp folder
                archiver.ExtractFiles("*.*");
                // Close archive
                archiver.CloseArchive();
            }
            // Catch all exceptions of the ArchiverException type
            catch (ArchiverException ae)
            {
                Console.WriteLine("Message: {0}\t Error code: {1}", ae.Message, ae.ErrorCode);
            }
        }

        static void Test()
        {
            BinaryFileReader reader = new BinaryFileReader(@"D:\PlayGround\SOD_Data\h3ab_bmp.lod");

            reader.Skip(8);
            uint count = reader.ReadUInt32();

            Console.WriteLine("Total count: " + count);

            List<FileInfo> fileList = new List<FileInfo>();
            reader.Seek(92);
            for (int fileIndex = 0; fileIndex < count; fileIndex++)
            {
                byte[] buffer = new byte[16];
                for (int i = 0; i < 16; i++)
                {
                    buffer[i] = reader.ReadByte();
                }
                string filename = System.Text.Encoding.ASCII.GetString(buffer);

                filename = filename.Substring(0, filename.IndexOf('\0'));
                uint offset = reader.ReadUInt32();
                uint size = reader.ReadUInt32();
                uint placeholder = reader.ReadUInt32();
                uint csize = reader.ReadUInt32();

                Console.WriteLine(string.Format("[{4}] filename:{0} offset:{1} size:{2} csize:{3}", filename, offset, size, csize, fileIndex));

                FileInfo info = new FileInfo();
                info.FileName = filename;
                info.Offset = offset;
                info.Size = size;
                info.CSize = csize;

                fileList.Add(info);
            }

            Directory.CreateDirectory(@".\output\");

            Directory.CreateDirectory(@".\output\extracted\");
                                
            for (int fileIndex = 0; fileIndex < count; fileIndex++)
            {
                FileInfo info = fileList[fileIndex];

                reader.Seek(info.Offset);
                byte[] content;
                string filename = @".\output\" + info.FileName;
                if (info.CSize > 0)
                {
                    filename = filename + ".zip";
                    content = reader.ReadBytes((int)info.CSize);
                }
                else
                {
                    content = reader.ReadBytes((int)info.Size);
                }

                using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(content, 0, content.Length);
                }

                //// Thread.Sleep(500);
                Console.WriteLine("Finished writing file " + filename);

                if (filename.EndsWith(".h3c"))
                {
                    /*
                    //ZipFile.ExtractToDirectory(filename, @"D:\Temp\ab.h3c");
                    int length = 100000;
                    byte[] data = new byte[length];
                    for (int i = 0; i < length; i++)
                    {
                        data[i] = System.Convert.ToByte(i % 100 + i % 50);
                    }

                    byte[] o;
                    //serialization into memory stream
                    IFormatter formatter = new BinaryFormatter();

                    using (FileStream decompressedFileStream = File.Create(filename + ".origin"))
                    {
                        using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
                        {
                            using (GZipStream decompressionStream = new GZipStream(file, CompressionMode.Decompress, true))
                            {
                                int dstreamlength = 0;
                                while(decompressionStream.ReadByte() >= 0)
                                {
                                    dstreamlength++;
                                }

                                Console.WriteLine("GZip Decompressed length: " + dstreamlength);
                                
                                ///DecompressFile(@".\output\extracted\", decompressionStream);
                                // decompressionStream.CopyTo(decompressedFileStream);///

                                Console.WriteLine("Decompressed: {0}", filename);
                            }

                            
                            file.Seek(0, SeekOrigin.Begin);
                            var oo = formatter.Deserialize(decompressedFileStream);
                            o = (byte[])oo;
                            
                        }
                    }
                    */
                }

                if (info.CSize > 0)
                {
                    ////ZipFile.ExtractToDirectory(filename, @"D:\Temp");
                    //// ZipFile.ExtractToDirectory(filename, filename.Substring(0, 5));



                    using (FileStream decompressedFileStream = File.Create(filename + ".origin"))
                    {
                        using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
                        {
                            using (DeflateStream decompressionStream = new DeflateStream(file, CompressionMode.Decompress, true))
                            {
                                decompressionStream.CopyTo(decompressedFileStream);
                                Console.WriteLine("Decompressed: {0}", filename);
                            }

                            /*
                            using (GZipStream decompressionStream = new GZipStream(file, CompressionMode.Decompress, false))
                            {
                                decompressionStream.CopyTo(decompressedFileStream);
                                Console.WriteLine("Decompressed: {0}", filename);
                            }
                             */

                        }
                    }
                    
                }

            }
        }


        static bool DecompressFile(string sDir, GZipStream zipStream)
        {
            //Decompress file name
            byte[] bytes = new byte[sizeof(int)];
            int Readed = zipStream.Read(bytes, 0, sizeof(int));
            if (Readed < sizeof(int))
                return false;

            int iNameLen = BitConverter.ToInt32(bytes, 0);
            bytes = new byte[sizeof(char)];
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < iNameLen; i++)
            {
                zipStream.Read(bytes, 0, sizeof(char));
                char c = BitConverter.ToChar(bytes, 0);
                sb.Append(c);
            }
            string sFileName = sb.ToString();

            //Decompress file content
            bytes = new byte[sizeof(int)];
            zipStream.Read(bytes, 0, sizeof(int));
            int iFileLen = BitConverter.ToInt32(bytes, 0);

            bytes = new byte[iFileLen];
            zipStream.Read(bytes, 0, bytes.Length);

            string sFilePath = Path.Combine(sDir, sFileName);
            string sFinalDir = Path.GetDirectoryName(sFilePath);
            if (!Directory.Exists(sFinalDir))
                Directory.CreateDirectory(sFinalDir);

            using (FileStream outFile = new FileStream(sFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                outFile.Write(bytes, 0, iFileLen);

            return true;
        }
    }
}
