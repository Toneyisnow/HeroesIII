using H3Engine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.Mapping
{
    public enum EMapFormat
    {
        INVALID = 0,

        //    HEX     DEC
        ROE = 0x0e, // 14
        AB = 0x15, // 21
        SOD = 0x1c, // 28
                    // HOTA = 0x1e ... 0x20 // 28 ... 30
        WOG = 0x33,  // 51
        VCMI = 0xF0
    };


    public class MapHeader
    {
        static readonly int MAP_SIZE_SMALL = 36;
        static readonly int MAP_SIZE_MIDDLE = 72;
        static readonly int MAP_SIZE_LARGE = 108;
        static readonly int MAP_SIZE_XLARGE = 144;

        public MapHeader()
        {

        }

        public EMapFormat Version
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public string Description
        {
            get; set;
        }

        public uint Height
        {
            get; set;
        }

        public uint Width
        {
            get; set;
        }

        public bool IsTwoLevel
        {
            get; set;
        }

        public bool AreAnyPlayers
        {
            get; set;
        }

        public int Difficulty
        {
            get; set;
        }

        public int LevelLimity
        {
            get; set;
        }

        public int HowManyTeams
        {
            get; set;
        }

        public List<bool> AllowedHeroes
        {
            get; set;
        }

        public List<PlayerInfo> Players
        {
            get; set;
        }



        public string VictoryMessage
        {
            get; set;
        }

        public string DefeatMessage
        {
            get; set;
        }

        public int VictoryIconIndex
        {
            get; set;
        }

        public int DefeatIconIndex
        {
            get; set;
        }









    }
}
