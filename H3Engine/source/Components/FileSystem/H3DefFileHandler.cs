﻿using H3Engine.Components.GUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.Components.FileSystem
{
    public class H3DefFile
    {
        public H3DefFile()
        {

        }

        public string Name
        {
            get; set;
        }

        
        /// <summary>
        /// The type of the animation
        /// </summary>
        public int Type
        {
            get; set;
        }

        public int Width
        {
            get; set;
        }

        public int Height
        {
            get; set;
        }



    }


    public class H3DefFileHandler
    {
        private Stream inputStream;

        private List<List<UInt32>> offsets;
        /// <summary>
        /// 
        /// </summary>
        private Color[] h3Palette = null;

        private AnimationDefinition animation = null;

        public H3DefFileHandler(Stream fileStream)
        {
            this.inputStream = fileStream;
            this.offsets = new List<List<UInt32>>();

            this.animation = new AnimationDefinition();

            InitializePalete();

            // Read header
            LoadHeader();
        }

        private void InitializePalete()
        {
            h3Palette = new Color[8]
            {
                Color.FromArgb(0, 0, 0, 0),
                Color.FromArgb(32, 0, 0, 0),
                Color.FromArgb(64, 0, 0, 0),
                Color.FromArgb(128, 0, 0, 0),
                Color.FromArgb(128, 0, 0, 0),
                Color.FromArgb(0, 0, 0, 0),
                Color.FromArgb(128, 0, 0, 0),
                Color.FromArgb(64, 0, 0, 0)
            };
        }

        private void LoadHeader()
        {
            BinaryReader reader = new BinaryReader(inputStream);

            animation.Type = (EAnimationDefType)reader.ReadUInt32();
            animation.Width = (int)reader.ReadUInt32();
            animation.Height = (int)reader.ReadUInt32();

            var groupCount = (int)reader.ReadUInt32();

            Color[] palette = new Color[256];
            for(int i = 0; i < 256; i++)
            {
                byte red = reader.ReadByte();
                byte green = reader.ReadByte();
                byte blue = reader.ReadByte();

                palette[i] = Color.FromArgb(red, green, blue);
            }

            switch(animation.Type)
            {
                case EAnimationDefType.SPELL:
                    palette[0] = h3Palette[0];
                    break;
                case EAnimationDefType.SPRITE:
                case EAnimationDefType.SPRITE_FRAME:
                    for(int i = 0; i < 8; i++)
                        palette[i] = h3Palette[i];
                    break;
                case EAnimationDefType.CREATURE:
                    palette[0] = h3Palette[0];
                    palette[1] = h3Palette[1];
                    palette[4] = h3Palette[4];
                    palette[5] = h3Palette[5];
                    palette[6] = h3Palette[6];
                    palette[7] = h3Palette[7];
                    break;
                case EAnimationDefType.MAP:
                case EAnimationDefType.MAP_HERO:
                    palette[0] = h3Palette[0];
                    palette[1] = h3Palette[1];
                    palette[4] = h3Palette[4];
                    break;
                case EAnimationDefType.TERRAIN:
                    palette[0] = h3Palette[0];
                    palette[1] = h3Palette[1];
                    palette[2] = h3Palette[2];
                    palette[3] = h3Palette[3];
                    palette[4] = h3Palette[4];
                    break;
                case EAnimationDefType.CURSOR:
                    palette[0] = h3Palette[0];
                    break;
                case EAnimationDefType.INTERFACE:
                    palette[0] = h3Palette[0];
                    palette[1] = h3Palette[1];
                    palette[4] = h3Palette[4];
                    break;
                case EAnimationDefType.BATTLE_HERO:
                    palette[0] = h3Palette[0];
                    palette[1] = h3Palette[1];
                    palette[4] = h3Palette[4];
                    break;
                default:
                    break;
            }

            Console.WriteLine(string.Format("Type: {0} Width: {1} Height: {2} GroupCount: {3}", animation.Type, animation.Width, animation.Height, groupCount));

            for(int i = 0; i < groupCount; i++)
            {
                int groupId = (int)reader.ReadUInt32();
                int frameCount = (int)reader.ReadUInt32();

                // Unknown 8 bytes
                reader.ReadUInt32();
                reader.ReadUInt32();
                
                byte[] name = reader.ReadBytes(13 * frameCount);
                
                Console.WriteLine(string.Format(@"Group: {0} Id={1} name={2} frameCount={3}", i, groupId, Encoding.ASCII.GetString(name, 0, name.Length), frameCount));

                List<UInt32> offset = new List<UInt32>();
                for (int j = 0; j < frameCount; j++)
                {
                    UInt32 off = reader.ReadUInt32();
                    offset.Add(off);
                }
                offsets.Add(offset);
            }
        }

        public void DumpFrame(int groupIndex, int frameIndex)
        {
            if (groupIndex >= offsets.Count)
            {
                return;
            }

            var groupOffset = offsets[groupIndex];
            if (frameIndex >= groupOffset.Count)
            {
                return;
            }

            var offset = groupOffset[frameIndex];

            inputStream.Seek(offset, SeekOrigin.Begin);
            BinaryReader reader = new BinaryReader(inputStream);

            AnimationFrame frame = new AnimationFrame();
            UInt32 size = reader.ReadUInt32();
            int format = (int)reader.ReadUInt32();
            frame.FullWidth = (int)reader.ReadUInt32();
            frame.FullHeight = (int)reader.ReadUInt32();
            frame.Width = (int)reader.ReadUInt32();
            frame.Height = (int)reader.ReadUInt32();
            frame.LeftMargin = (int)reader.ReadUInt32();
            frame.TopMargin = (int)reader.ReadUInt32();

            UInt32 baseOffset = 32;
            if (format == 1 && frame.Width > frame.FullWidth && frame.Height > frame.FullHeight)
            {
                frame.LeftMargin = 0;
                frame.TopMargin = 0;
                frame.Width = frame.FullWidth;
                frame.Height = frame.FullHeight;

                inputStream.Seek(-16, SeekOrigin.Current);
                baseOffset = 16;
            }

            Console.WriteLine(string.Format(@"Frame [{0}][{1}]: format={2} FullWidth={3} FullHeight={4} Width={5} Height={6} Left={7} Top={8}", 
                                        groupIndex, frameIndex, format, frame.FullWidth, frame.FullHeight, frame.Width, frame.Height, frame.LeftMargin, frame.TopMargin));

            UInt32 currentOffset = baseOffset;
            long basePosition = inputStream.Position;
            MemoryStream dataStream = new MemoryStream();

            switch (format)
            {
                case 0:


                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:

                    for(int i = 0; i < frame.Height; i++)
                    {
                        inputStream.Seek(basePosition + baseOffset + i * 2 * (frame.Width / 32), SeekOrigin.Begin);

                        uint totalRowLength = 0;
                        while( totalRowLength < frame.Width)
                        {
                            UInt32 segment = reader.ReadUInt32();
                            UInt32 code = segment >> 5;
                            UInt32 length = (segment & 31) + 1;

                            byte[] data = new byte[length];
                            if (code == 7)  // Raw Data
                            {
                                data = reader.ReadBytes((int)length);
                            }
                            else  // RLE
                            {
                                ////
                                for(int k = 0; k < length; k++)
                                {
                                    data[i] = (byte)code;
                                }
                            }
                            dataStream.Write(data, 0, (int)length);
                            totalRowLength += length;
                        }

                    }

                    break;
                default:
                    break;
            }

            frame.Data = dataStream.ToArray();
            Console.WriteLine("Total Data Length: " + frame.Data.Length);
        }
    }
}
