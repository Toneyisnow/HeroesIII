using H3Engine.Components.FileSystem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.Components.GUI
{
    public enum EAnimationDefType
    {
        SPELL = 0x40,
        SPRITE = 0x41,
        CREATURE = 0x42,
        MAP = 0x43,
        MAP_HERO = 0x44,
        TERRAIN = 0x45,
        CURSOR = 0x46,
        INTERFACE = 0x47,
        SPRITE_FRAME = 0x48,
        BATTLE_HERO = 0x49
    }

    /// <summary>
    /// This is the data structure from .DEF file
    /// </summary>
    public class AnimationDefinition
    {
        public AnimationDefinition()
        {
            this.Groups = new List<AnimationGroup>();
        }

        public string Name
        {
            get; set;
        }


        /// <summary>
        /// The type of the animation
        /// </summary>
        public EAnimationDefType Type
        {
            get; set;
        }

        public Color[] Palette
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

        public List<AnimationGroup> Groups
        {
            get; set;
        }


        public ImageData ComposeFrameImage(int groupIndex, int frameIndex)
        {
            if (groupIndex >= this.Groups.Count || frameIndex >= this.Groups[groupIndex].Frames.Count)
            {
                return null;
            }

            AnimationFrame frame = this.Groups[groupIndex].Frames[frameIndex];
            ImageData image = new ImageData(Width, Height);

            byte[] imageData = this.Groups[groupIndex].Frames[frameIndex].Data;
            for (int j = 0; j < this.Height; j++)
            {
                for (int i = 0; i < this.Width; i++)
                {
                    if (i < frame.LeftMargin || j < frame.TopMargin || i >= frame.LeftMargin + frame.Width || j >= frame.TopMargin + frame.Height)
                    {
                        image.WriteColor(Palette[0]);
                    }
                    else
                    {
                        byte index = imageData[(j - frame.TopMargin) * frame.Width + i - frame.LeftMargin];
                        image.WriteColor(Palette[index]);
                    }
                }
            }

            return image;
        }

        public ImageData ComposeFrameImage2(int groupIndex, int frameIndex)
        {
            ImageData image = new ImageData(Width, Height);

            byte[] data = this.Groups[groupIndex].Frames[frameIndex].Data;

            for (int i = 0; i < data.Length; i++)
            {
                byte value = data[i];
                Color color = Palette[value];
                image.WriteColor(color);
            }

            return image;
        }
    }
}
