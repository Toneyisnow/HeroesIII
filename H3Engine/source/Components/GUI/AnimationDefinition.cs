using System;
using System.Collections.Generic;
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

    }
}
