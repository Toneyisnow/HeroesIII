using H3Engine.Common;
using H3Engine.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.MapObjects
{
    public class TownInstance : CGDwelling
    {
        public enum EFortLevel { NONE = 0, FORT = 1, CITADEL = 2, CASTLE = 3 };



        public TownInstance()
        {
            this.Buildings = new HashSet<EBuildingId>();
            this.ForbiddenBuildings = new HashSet<EBuildingId>();

            this.PossibleSpells = new List<ESpellId>();
            this.ObligatorySpells = new List<ESpellId>();

            this.Events = new List<CCastleEvent>();
        }


        public string TownName
        {
            get; set;
        }

        public HashSet<EBuildingId> Buildings
        {
            get; set;
        }

        public HashSet<EBuildingId> ForbiddenBuildings
        {
            get; set;
        }

        public List<ESpellId> PossibleSpells
        {
            get; private set;
        }

        public List<ESpellId> ObligatorySpells
        {
            get; private set;
        }

        public List<CCastleEvent> Events
        {
            get; private set;
        }

        public int Alignment
        {
            get; set;
        }
    }
}
