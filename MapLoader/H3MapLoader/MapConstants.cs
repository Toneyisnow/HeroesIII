using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3MapLoader.Mapping
{
    public class Constants
    {
        public const int PLAYER_LIMIT_T = 8;
        public const int HEROES_QUANTITY = 156;
        public const int ARTIFACTS_QUANTITY = 171;

        public const int SPELLS_QUANTITY = 70;
        public const int SKILL_QUANTITY = 28;

        public const int PRIMARY_SKILLS = 4;
    }


    public enum EVictoryConditionType
    {
        ARTIFACT, GATHERTROOP, GATHERRESOURCE, BUILDCITY, BUILDGRAIL, BEATHERO,
        CAPTURECITY, BEATMONSTER, TAKEDWELLINGS, TAKEMINES, TRANSPORTITEM, WINSTANDARD = 255
    };

    public enum ELossConditionType
    {
        LOSSCASTLE, LOSSHERO, TIMEEXPIRES, LOSSSTANDARD = 255
    };

    
}
