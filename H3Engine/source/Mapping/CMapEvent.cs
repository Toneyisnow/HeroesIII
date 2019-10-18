using H3Engine.Common;
using H3Engine.Components;
using H3Engine.MapObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.Mapping
{
    public class CMapEvent
    {
        public CMapEvent()
        {

        }

        public string Name
        {
            get; set;
        }

        public string Message
        {
            get; set;
        }

        public ResourceSet Resources
        {
            get; set;
        }


        // Affected Players, bit field?
        public byte Players
        {
            get;set;
        }

        public byte HumanAffected
        {
            get; set;
        }

        public byte ComputerAffected
        {
            get; set;
        }

        public uint FirstOccurence
        {
            get; set;
        }

        public uint NextOccurence
        {
            get; set;
        }

    }

    public class CCastleEvent : CMapEvent
    {
        public CCastleEvent()
        {
            this.Buildings = new List<EBuildingId>();
            this.Creatures = new List<ECreatureId>();
        }

        public TownInstance Town
        {
            get; set;
        }

        public List<EBuildingId> Buildings
        {
            get; private set;
        }

        public List<ECreatureId> Creatures
        {
            get; private set;
        }


    }

}
