using H3Engine.Components.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.Components.FileSystem
{
    public class BinaryFileReader : IDisposable
    {
        private FileStream fileStream = null;


        public BinaryFileReader(string fileFullPath)
        {
            try
            {
                this.fileStream = new FileStream(fileFullPath, FileMode.Open, FileAccess.Read);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Reset()
        {
            this.fileStream.Seek(0, SeekOrigin.Begin);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="count"></param>
        public void Skip(int count)
        {
            this.fileStream.Seek(count, SeekOrigin.Current);
        }

        public void Seek(int position)
        {
            this.fileStream.Seek(position, SeekOrigin.Begin);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="byteCount"></param>
        /// <param name="limit"></param>
        /// <param name="negate"></param>
        public void ReadBitMask(HashSet<int> dest, int byteCount, int limit, bool negate = true)
        {
            bool[] boolDest = new bool[limit];
            ReadBitMask(boolDest, byteCount, limit, negate);

            for (int i = 0; i < limit; i++)
            {
                if (boolDest[i])
                {
                    dest.Add(i);
                }
            }
        }

        public void ReadBitMask(bool[] dest, int byteCount, int limit, bool negate)
        {
            for (int nowByte = 0; nowByte < byteCount; nowByte++)
            {
                int mask = ReadUInt8();
                for (int bit = 0; bit < 8; ++bit)
                {
                    if (nowByte * 8 + bit < limit)
                    {
                        bool flag = (mask & (1 << bit)) > 0;
                        dest[nowByte * 8 + bit] = (flag != negate);        // FIXME: check PR388
                    }
                }
            }
        }

        public MapPosition ReadPosition()
        {
            int x = ReadUInt8();
            int y = ReadUInt8();
            int z = ReadUInt8();
            MapPosition position = new MapPosition(x, y, z);

            return position;
        }

        public UInt32 ReadUInt32()
        {
            byte[] cache = new byte[4];
            fileStream.Read(cache, 0, 4);

            return BitConverter.ToUInt32(cache, 0);
        }

        public bool ReadBool()
        {
            byte[] cache = new byte[1];
            fileStream.Read(cache, 0, 1);

            return BitConverter.ToBoolean(cache, 0);
        }

        public byte ReadByte()
        {
            byte[] cache = new byte[1];
            fileStream.Read(cache, 0, 1);

            return cache[0];
        }

        public byte[] ReadBytes(int length)
        {
            byte[] cache = new byte[length];
            fileStream.Read(cache, 0, length);

            return cache;
        }

        public UInt16 ReadUInt16()
        {
            byte[] cache = new byte[2];
            fileStream.Read(cache, 0, 2);

            return BitConverter.ToUInt16(cache, 0);
        }

        public string ReadString()
        {
            UInt32 length = ReadUInt32();
            byte[] result = new byte[length];

            for (var i = 0; i < length; i++)
            {
                result[i] = ReadByte();
            }

            return Encoding.ASCII.GetString(result);
        }

        public int ReadUInt8()
        {
            byte val = ReadByte();
            return (int)val;
        }

        public void Dispose()
        {
            if (fileStream != null)
            {
                fileStream.Dispose();
            }
        }
    }
}
