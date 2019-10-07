using ComponentAce.Compression.Libs.ZLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3MapLoader.Components.FileSystem
{
    public class CompressedStreamReader : IDisposable
    {
        private static readonly int InflateBlockSize = 10000;

        private bool endOfFileReached = false;

        private byte[] compressedBuffer = null;

        private MemoryStream buffer = null;

        private BinaryFileReader inputReader = null;

        private ZStream inflateState = null;

        private UInt64 Position
        {
            get; set;
        }

        public CompressedStreamReader(BinaryFileReader reader, bool isGZip, UInt64 decompressedSize)
        {
            this.inputReader = reader;
            this.compressedBuffer = new byte[InflateBlockSize];
            this.buffer = new MemoryStream();
            ////this.buffer.Length

            this.inflateState = new ZStream();
            this.inflateState.avail_in = 0;
            this.inflateState.next_in = null;

            int windowBits = 15;
            if (isGZip)
            {
                windowBits += 16;
            }

            int ret = inflateState.inflateInit(windowBits);
            if (ret != 0)
            {
                // Log Error
                throw new Exception("inflateInit failed.");
            }
        }

        public void Dispose()
        {
            this.inflateState.inflateEnd();
        }

        public UInt64 Read(byte[] data, UInt64 size)
        {
            EnsureSize(this.Position + size);

            var toRead = Math.Min(size, (ulong)buffer.Length - this.Position);
            /// buffer.Seek(this.Position, SeekOrigin.Begin);
            
            if (toRead > 0)
            {
                buffer.Read(data, 0, (int)toRead);
                this.Position += toRead;
            }

            return toRead;
        }

        public void EnsureSize(UInt64 size)
        {
            while(buffer.Length < (long)size && this.endOfFileReached)
            {
                var initialSize = buffer.Length;
                var currentStep = Math.Min((long)size, initialSize);
                currentStep = (currentStep < 1024 ? 1024 : currentStep);
                UInt64 readSize = this.ReadMore((ulong)currentStep);
            }

            /*
            while((ulong)buffer.Count < size && !this.endOfFileReached)
            {
                UInt64 initialSize = (UInt64)buffer.Count;
                UInt64 currentStep = Math.Min(size, initialSize);
                currentStep = (currentStep < 1024 ? 1024 : currentStep);

                UInt64 readSize = this.ReadMore(initialSize, currentStep);
            }
            */
        }

        public UInt64 Seek(UInt64 position)
        {
            EnsureSize(position);

            this.Position = Math.Min(position, (ulong)(buffer.Length - 1));

            return this.Position;
        }

        public UInt64 Tell()
        {
            return this.Position;
        }

        public UInt64 Skip(UInt64 delta)
        {
            return 0;
        }

        public UInt64 GetSize()
        {
            return (UInt64)buffer.Length;
        }

        public void Reset()
        {
            this.Position = 0;
            this.endOfFileReached = false;
        }

        /// <summary>
        /// Read more data into Buffer
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public UInt64 ReadMore(UInt64 size)
        {
            if (this.inflateState == null)
            {
                return 0;
            }

            bool fileEnded = false;
            bool endLoop = false;

            long decompressed = this.inflateState.total_out;

            byte[] data = new byte[(int)size];

            this.inflateState.avail_out = (int)size;
            this.inflateState.next_out = data;

            do
            {
                if (this.inflateState.avail_in == 0)
                {
                    byte[] newBuffer = inputReader.ReadBytes(compressedBuffer.Length);
                    int availSize = newBuffer.Length;
                    compressedBuffer = newBuffer;
                    if (availSize != compressedBuffer.Length)
                    {
                        this.inputReader.Reset();
                        this.inputReader = null;
                    }

                    this.inflateState.avail_in = availSize;
                    this.inflateState.next_in = compressedBuffer;
                }

                int ret = this.inflateState.inflate(FlushStrategy.Z_NO_FLUSH);

                if (this.inflateState.avail_in == 0 && inputReader == null)
                {
                    fileEnded = true;
                }

                switch (ret)
                {
                    case 0: // Z_OK
                        break;
                    case 1: // Z_STREAM_END
                        endLoop = true;
                        break;
                    case -5: // Z_BUF_ERROR
                        endLoop = true;
                        break;
                    default:
                        throw new Exception("Inflate Error: " + this.inflateState.msg);
                }

            }
            while (!endLoop && inflateState.avail_out != 0);

            buffer.Write(data, 0, data.Length);

            decompressed = inflateState.total_out - decompressed;

            if (fileEnded)
            {
                this.inflateState.inflateEnd();
                this.inflateState = null;
            }

            return (UInt64)decompressed;
        }

        public bool GetNextBlock()
        {
            if(this.inflateState == null)
            {
                return false;
            }

            if (this.inflateState.inflateEnd() < 0)
            {
                return false;
            }

            if (this.inflateState.inflateInit() < 0)
            {
                return false;
            }

            Reset();
            return true;
        }
    }
}
