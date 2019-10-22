using H3Engine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.MapObjects
{
    public class CGDwelling : ArmedInstance
    {

        public CreatureSet Creatures
        {
            get; set;
        }

        public CreatureGeneratorAsCastleInfo SpecAsCastle
        {
            get; set;
        }

        public CreatureGeneratorAsLeveledInfo SpecAsLeveled
        {
            get; set;
        }


    }

    public abstract class DwellingSpecInfo
    {
        public CGDwelling Owner
        {
            get; set;
        }
    }

    public class CreatureGeneratorAsCastleInfo : DwellingSpecInfo
    {
        public CreatureGeneratorAsCastleInfo(CGDwelling owner)
        {
            this.Owner = owner;
        }


        public bool AsCastle
        {
            get; set;
        }

        public uint Identifier
        {
            get; set;
        }

        public List<bool> AllowedFactions
        {
            get; set;
        }

        public string InstanceId
        {
            get; set;
        }

    }
    
    public class CreatureGeneratorAsLeveledInfo : DwellingSpecInfo
    {
        public CreatureGeneratorAsLeveledInfo(CGDwelling owner)
        {
            this.Owner = owner;
        }

        public byte MinLevel
        {
            get; set;
        }

        public byte MaxLevel
        {
            get; set;
        }

    }
    
}
