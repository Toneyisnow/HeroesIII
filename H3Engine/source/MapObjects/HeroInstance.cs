using H3Engine.Components;
using H3Engine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.MapObjects
{
    public class CGHeroPlaceHolder : CGObject
    {
        public byte Power
        {
            get; set;
        }

    }

    public class HeroPatrol
    {
        public HeroPatrol()
        {

        }

        public bool IsPatrolling
        {
            get; set;
        }

        public MapPosition InitialPosition
        {
            get; set;
        }

        public uint PatrolRadius
        {
            get; set;
        }


    }

    public class HeroInstance : ArmedInstance
    {
        public HeroInstance(H3Hero data = null)
        {
            if (data == null)
            {
                this.Data = new H3Hero();
            }
            else
            {
                this.Data = data;
            }
        }

        public H3Hero Data
        {
            get; set;
        }

       
        public byte MoveDirection
        {
            get; set;
        }
        
        /*
        public int Mana
        {
            get; set;
        }

        public ArtifactSet ArtifactSet
        {
            get; set;
        }

        public List<H3Spell> Spells
        {
            get; set;
        }

        public HeroSpecialty Specialty
        {
            get; set;
        }
        */

        public HeroPatrol Patrol
        {
            get; set;
        }

        /// Visited
        /// 
        ///
        public List<TownInstance> VisitedTowns
        {
            get; set;
        }

        public List<CGObject> VisitedObjects
        {
            get; set;
        }



    }
}
