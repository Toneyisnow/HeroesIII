using H3Engine.Common;
using H3Engine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.MapObjects
{

    /// <summary>
    /// A creature object on the Adventure map
    /// </summary>
    public class CGCreature : ArmedInstance
    {
        public enum Action
        {
            FIGHT = -2, FLEE = -1, JOIN_FOR_FREE = 0 //values > 0 mean gold price
        };

        public enum EFriendliness
        {
            COMPLIANT = 0, FRIENDLY = 1, AGRESSIVE = 2, HOSTILE = 3, SAVAGE = 4
        };

        public CGCreature()
        {

        }
        
        public bool NeverFlee
        {
            get; set;
        }

        public bool NotGrowingTeam
        {
            get; set;
        }

        public EArtifactId GainArtifact
        {
            get; set;
        }

        public ResourceSet GainResources
        {
            get; set;
        }
        

        /// <summary>
        /// Character of this set of creatures (0 - the most friendly, 4 - the most hostile) => on init changed to -4 (compliant) ... 10 value (savage)
        /// </summary>
        public byte Friendliness
        {
            get; set;
        }

    }
}
