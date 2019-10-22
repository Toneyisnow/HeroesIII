using H3Engine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.MapObjects
{
    public class ITeamVisited : CGObject
    {

    }

    public class CGSignBottle : CGObject
    {
        public string Message
        {
            get; set;
        }

    }

    public class CGWitchHut : CGObject
    {
        public CGWitchHut()
        {
            this.AllowedAbilities = new List<int>();
        }

        public List<int> AllowedAbilities
        {
            get; set;
        }

    }

    public class CGScholar : CGObject
    {
        public enum EBonusType
        {
            PrimarySkill = 0,
            SecondarySkill = 1,
            Spell = 2,
            Random = 255
        }

        public EBonusType BonusType
        {
            get; set;
        }

        public int BonusId
        {
            get; set;
        }
    }

    public class CGGarrison : CGObject
    {
        public bool RemovableUnits
        {
            get; set;
        }
    }

    public class CGArtifact : ArmedInstance
    {
        public CGArtifact(EArtifactId artId)
        {

        }
    }

    public class CGResource : ArmedInstance
    {
        public int Amount
        {
            get; set;
        }
    }

    public class CGShrine : ITeamVisited
    {
        public ESpellId SpellId
        {
            get; set;
        }

    }

    public class CGShipyard : CGObject
    {

    }

    public class CGMine : ArmedInstance
    {

    }

    public class TeleportChannel
    {

    }

    public class CGTeleport : CGObject
    {

    }

    public class CGMonolith : CGTeleport
    {

    }

    public class CGSubterraneanGate : CGMonolith
    {

    }

    public class CGWhirlpool : CGMonolith
    {

    }

    public class CGMagicWell : CGObject
    {

    }

    public class CGSirens : CGObject
    {

    }

    public class CGObservatory : CGObject
    {

    }

    public class CGBoat : CGObject
    {
       


    }
    public class CGMagi : CGObject
    {

    }

    public class CGCartographer : ITeamVisited
    {

    }

    public class CGDenOfthieves : CGObject
    {

    }

    public class CGObelisk : ITeamVisited
    {

    }

    public class CGLighthouse : CGObject
    {

    }

    public class CGGrail : CGObject
    {
        public uint Radius
        {
            get; set;
        }
    }

}
