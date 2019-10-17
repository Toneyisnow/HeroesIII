using H3Engine.Components;
using H3Engine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.MapObjects
{
    public class HeroInstance : ArmedInstance
    {
        public HeroInstance()
        {

        }

        public H3Hero Data
        {
            get; set;
        }

       
        public byte MoveDirection
        {
            get; set;
        }
        
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
