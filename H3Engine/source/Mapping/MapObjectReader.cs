using H3Engine.Common;
using H3Engine.Components;
using H3Engine.Core;
using H3Engine.FileSystem;
using H3Engine.MapObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.Mapping
{
    public abstract class MapObjectReader
    {
        public MapHeader MapHeader
        {
            get; set;
        }

        public ObjectTemplate ObjectTemplate
        {
            get; set;
        }

        public abstract CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition);


        protected ResourceSet ReadResouces(BinaryReader reader)
        {
            for (int x = 0; x < 7; ++x)
            {
                var num = reader.ReadUInt32();
            }

            return null;
        }

        /// <summary>
        /// Read the showing message and guards for this object
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="eventObject"></param>
        protected void ReadMessageAndGuards(BinaryReader reader, ArmedInstance armedObject)
        {

        }

        protected CreatureSet ReadCreatureSet(BinaryReader reader, int numberToRead)
        {
            bool isHighVersion = this.MapHeader.Version > EMapFormat.ROE;
            int maxID = isHighVersion ? 0xffff : 0xff;

            for (int ir = 0; ir < numberToRead; ++ir)
            {
                ECreatureId creatureId;

                if (isHighVersion)
                {
                    creatureId = (ECreatureId)(reader.ReadUInt16());
                }
                else
                {
                    creatureId = (ECreatureId)(reader.ReadByte());
                }

                int amount = reader.ReadUInt16();

                // Empty slot
                if ((int)creatureId == maxID)
                    continue;

                // Create StackInstance
                //auto hlp = new CStackInstance();
                // hlp->count = count;

                if ((int)creatureId > maxID - 0xf)
                {
                    //this will happen when random object has random army
                    //hlp->idRand = maxID - (int)creatureId - 1;
                }
                else
                {
                    //hlp->setType((int)creatureId);
                }

                // out->putStack(SlotID(ir), hlp);
            }

            //out->validTypes(true);

            return null;
        }

        protected void ReadQuest(BinaryReader reader, IQuestObject questObject)
        {
            guard->quest->missionType = static_cast<CQuest::Emission>(reader.readUInt8());

            switch (guard->quest->missionType)
            {
                case CQuest::MISSION_NONE:
                    return;
                case CQuest::MISSION_PRIMARY_STAT:
                    {
                        guard->quest->m2stats.resize(4);
                        for (int x = 0; x < 4; ++x)
                        {
                            guard->quest->m2stats[x] = reader.readUInt8();
                        }
                    }
                    break;
                case CQuest::MISSION_LEVEL:
                case CQuest::MISSION_KILL_HERO:
                case CQuest::MISSION_KILL_CREATURE:
                    {
                        guard->quest->m13489val = reader.readUInt32();
                        break;
                    }
                case CQuest::MISSION_ART:
                    {
                        int artNumber = reader.readUInt8();
                        for (int yy = 0; yy < artNumber; ++yy)
                        {
                            int artid = reader.readUInt16();
                            guard->quest->m5arts.push_back(artid);
                            map->allowedArtifact[artid] = false; //these are unavailable for random generation
                        }
                        break;
                    }
                case CQuest::MISSION_ARMY:
                    {
                        int typeNumber = reader.readUInt8();
                        guard->quest->m6creatures.resize(typeNumber);
                        for (int hh = 0; hh < typeNumber; ++hh)
                        {
                            guard->quest->m6creatures[hh].type = VLC->creh->creatures[reader.readUInt16()];
                            guard->quest->m6creatures[hh].count = reader.readUInt16();
                        }
                        break;
                    }
                case CQuest::MISSION_RESOURCES:
                    {
                        guard->quest->m7resources.resize(7);
                        for (int x = 0; x < 7; ++x)
                        {
                            guard->quest->m7resources[x] = reader.readUInt32();
                        }
                        break;
                    }
                case CQuest::MISSION_HERO:
                case CQuest::MISSION_PLAYER:
                    {
                        guard->quest->m13489val = reader.readUInt8();
                        break;
                    }
            }

            int limit = reader.readUInt32();
            if (limit == (static_cast<int>(0xffffffff)))
            {
                guard->quest->lastDay = -1;
            }
            else
            {
                guard->quest->lastDay = limit;
            }
            guard->quest->firstVisitText = reader.readString();
            guard->quest->nextVisitText = reader.readString();
            guard->quest->completedText = reader.readString();
            guard->quest->isCustomFirst = guard->quest->firstVisitText.size() > 0;
            guard->quest->isCustomNext = guard->quest->nextVisitText.size() > 0;
            guard->quest->isCustomComplete = guard->quest->completedText.size() > 0;
        }

    }

    public class MapObjectReaderFactory
    {
        public static MapObjectReader GetObjectReader(EObjectType objectType)
        {
            MapObjectReader readerObject = null;

            switch (objectType)
            {
                case EObjectType.EVENT:
                    return new CGEventReader();

                case EObjectType.HERO:
                case EObjectType.RANDOM_HERO:
                case EObjectType.PRISON:
                    return new CGHeroReader();

                case EObjectType.MONSTER:  //Monster
                case EObjectType.RANDOM_MONSTER:
                case EObjectType.RANDOM_MONSTER_L1:
                case EObjectType.RANDOM_MONSTER_L2:
                case EObjectType.RANDOM_MONSTER_L3:
                case EObjectType.RANDOM_MONSTER_L4:
                case EObjectType.RANDOM_MONSTER_L5:
                case EObjectType.RANDOM_MONSTER_L6:
                case EObjectType.RANDOM_MONSTER_L7:
                    return new CGCreatureReader();

                case EObjectType.OCEAN_BOTTLE:
                case EObjectType.SIGN:
                    return new CGSignBottleReader();

                case EObjectType.SEER_HUT:
                    return new CGSeerHutReader();

                case EObjectType.WITCH_HUT:
                    return new CGWitchHutReader();

                case EObjectType.SCHOLAR:
                    return new CGScholarReader();

                case EObjectType.GARRISON:
                case EObjectType.GARRISON2:
                    return new CGGarrisonReader();

                case EObjectType.ARTIFACT:
                case EObjectType.RANDOM_ART:
                case EObjectType.RANDOM_TREASURE_ART:
                case EObjectType.RANDOM_MINOR_ART:
                case EObjectType.RANDOM_MAJOR_ART:
                case EObjectType.RANDOM_RELIC_ART:
                case EObjectType.SPELL_SCROLL:
                    return new CGArtifactReader();

                default:
                    break;
            }

            return readerObject;
        }
    }
    

    public class CGEventReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            CGEvent eventObject = new CGEvent();
            ReadMessageAndGuards(reader, eventObject);

            var gainedExp = reader.ReadUInt32();
            var manaDiff = reader.ReadUInt32();
            var moraleDiff = reader.ReadByte();
            var luckDiff = reader.ReadByte();

            ReadResouces(reader);

            for (int x = 0; x < 4; x++)
            {
                var primSkill = (EPrimarySkill)reader.ReadByte();
            }

            int gainedAbilities = reader.ReadByte();
            for (int i = 0; i < gainedAbilities; i++)
            {
                eventObject.Abilities.Add((ESecondarySkill)reader.ReadByte());
            }

            int gainedArtifacts = reader.ReadByte();
            for (int i = 0; i < gainedArtifacts; i++)
            {
                if (MapHeader.Version == EMapFormat.ROE)
                {
                    var artId = (EArtifactId)reader.ReadByte();
                }
                else
                {
                    var artId = (EArtifactId)reader.ReadUInt16();
                }
            }

            int gainedSpells = reader.ReadByte();
            for (int i = 0; i < gainedSpells; i++)
            {
                var spellId = (ESpellId)reader.ReadByte();
            }

            int gainedCreatures = reader.ReadByte();
            var creatureSet = ReadCreatureSet(reader, gainedCreatures);

            reader.Skip(8);

            var availableForPlayer = reader.ReadByte();
            var computerActivate = reader.ReadByte();
            var removeAfterVisit = reader.ReadByte();
            var humanActivate = true;

            reader.Skip(4);

            return eventObject;
        }


    }

    public class CGHeroReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            HeroInstance hero = new HeroInstance();




            return hero;
        }
    }

    public class CGCreatureReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            // Create Creature
            CGCreature creature = new CGCreature();

            if (MapHeader.Version > EMapFormat.ROE)
            {
                creature.Identifier = reader.ReadUInt32();
                // Quest Identifier?
            }

            return creature;
        }
    }

    public class CGSignBottleReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            CGSignBottle signBottle = new CGSignBottle();
            signBottle.Message = reader.ReadString();

            return signBottle;
        }
    }

    public class CGSeerHutReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            CGSeerHut seerHut = new CGSeerHut();

            if (MapHeader.Version > EMapFormat.ROE)
            {
                ReadQuest(reader, seerHut);
            }
            else
            {
                //RoE
                byte artifactId = reader.ReadByte();
                if (artifactId != 255)
                {
                    //not none quest
                    seerHut.Quest.m5arts.push_back(artifactId);
                    hut->quest->missionType = CQuest::MISSION_ART;
                }
                else
                {
                    hut->quest->missionType = CQuest::MISSION_NONE;
                }
                hut->quest->lastDay = -1; //no timeout
                hut->quest->isCustomFirst = hut->quest->isCustomNext = hut->quest->isCustomComplete = false;
            }

            if (hut->quest->missionType)
            {
                auto rewardType = static_cast<CGSeerHut::ERewardType>(reader.readUInt8());
                hut->rewardType = rewardType;
                switch (rewardType)
                {
                    case CGSeerHut::EXPERIENCE:
                        {
                            hut->rVal = reader.readUInt32();
                            break;
                        }
                    case CGSeerHut::MANA_POINTS:
                        {
                            hut->rVal = reader.readUInt32();
                            break;
                        }
                    case CGSeerHut::MORALE_BONUS:
                        {
                            hut->rVal = reader.readUInt8();
                            break;
                        }
                    case CGSeerHut::LUCK_BONUS:
                        {
                            hut->rVal = reader.readUInt8();
                            break;
                        }
                    case CGSeerHut::RESOURCES:
                        {
                            hut->rID = reader.readUInt8();
                            // Only the first 3 bytes are used. Skip the 4th.
                            hut->rVal = reader.readUInt32() & 0x00ffffff;
                            break;
                        }
                    case CGSeerHut::PRIMARY_SKILL:
                        {
                            hut->rID = reader.readUInt8();
                            hut->rVal = reader.readUInt8();
                            break;
                        }
                    case CGSeerHut::SECONDARY_SKILL:
                        {
                            hut->rID = reader.readUInt8();
                            hut->rVal = reader.readUInt8();
                            break;
                        }
                    case CGSeerHut::ARTIFACT:
                        {
                            if (map->version == EMapFormat::ROE)
                            {
                                hut->rID = reader.readUInt8();
                            }
                            else
                            {
                                hut->rID = reader.readUInt16();
                            }
                            break;
                        }
                    case CGSeerHut::SPELL:
                        {
                            hut->rID = reader.readUInt8();
                            break;
                        }
                    case CGSeerHut::CREATURE:
                        {
                            if (map->version > EMapFormat::ROE)
                            {
                                hut->rID = reader.readUInt16();
                                hut->rVal = reader.readUInt16();
                            }
                            else
                            {
                                hut->rID = reader.readUInt8();
                                hut->rVal = reader.readUInt16();
                            }
                            break;
                        }
                }
                reader.skip(2);
            }
            else
            {
                // missionType==255
                reader.skip(3);
            }

            return seerHut;
        }
    }

    public class CGWitchHutReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            var witchHut = new CGWitchHut();

            // in RoE we cannot specify it - all are allowed (I hope)
            if (MapHeader.Version > EMapFormat.ROE)
            {
                for (int i = 0; i < 4; ++i)
                {
                    byte c = reader.ReadByte();
                    for (int yy = 0; yy < 8; ++yy)
                    {
                        if (i * 8 + yy < GameConstants.SKILL_QUANTITY)
                        {
                            if (c == (c | 1 << yy))
                            {
                                witchHut.AllowedAbilities.Add(i * 8 + yy);
                            }
                        }
                    }
                }
                // enable new (modded) skills
                if (witchHut.AllowedAbilities.Count != 1)
                {
                    //// for (int skillID = GameConstants.SKILL_QUANTITY; skillID < VLC->skillh->size(); ++skillID)
                    ////     wh->allowedAbilities.push_back(skillID);
                }
            }
            else
            {
                // RoE map
                for (int skillID = 0; skillID < GameConstants.SKILL_QUANTITY; ++skillID)
                    witchHut.AllowedAbilities.Add(skillID);
            }

            return witchHut;
        }
    }

    public class CGScholarReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            var scholar = new CGScholar();
            scholar.BonusType = (CGScholar.EBonusType)(reader.ReadByte());
            scholar.BonusId = reader.ReadByte();
            reader.Skip(6);

            return scholar;
        }
    }

    public class CGGarrisonReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            CGGarrison garrison = new CGGarrison();

            EPlayerColor color = (EPlayerColor)reader.ReadByte();
            garrison.SetOwner(color);

            reader.Skip(3);

            CreatureSet creatureSet = ReadCreatureSet(reader, 7);
            if (MapHeader.Version > EMapFormat.ROE)
            {
                garrison.RemovableUnits = reader.ReadBoolean();
            }
            else
            {
                garrison.RemovableUnits = true;
            }

            reader.Skip(8);

            return garrison;
        }
    }

    public class CGArtifactReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            EArtifactId artId = EArtifactId.NONE; //random, set later
            int spellId = -1;
            CGArtifact artifact = new CGArtifact(artId);

            ReadMessageAndGuards(reader, artifact);

            if (this.ObjectTemplate.Type == EObjectType.SPELL_SCROLL)
            {
                spellId = (int)reader.ReadUInt32();
                artId = EArtifactId.SPELL_SCROLL;
            }
            else if (this.ObjectTemplate.Type == EObjectType.ARTIFACT)
            {
                //specific artifact
                artId = (EArtifactId)this.ObjectTemplate.SubId;
            }

            //// artifact.StoredArtifact = CArtifactInstance::createArtifact(map, artID, spellID);

            return artifact;
        }
    }
    
    public class CGResourceReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            CGResource resource = new CGResource();
            
            ReadMessageAndGuards(reader, resource);

            resource.Amount = (int)reader.ReadUInt32();
            if ((EResourceType)this.ObjectTemplate.SubId == EResourceType.GOLD)
            {
                // Gold is multiplied by 100.
                resource.Amount *= 100;
            }
            reader.Skip(4);

            return resource;
        }
    }

    public class CGTownReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            TownInstance town = new TownInstance();
            if (MapHeader.Version > EMapFormat.ROE)
            {
                town.Identifier = reader.ReadUInt32();
            }

            town.CurrentOwner = (EPlayerColor)reader.ReadByte();

            bool hasName = reader.ReadBoolean();
            if (hasName)
            {
                town.TownName = reader.ReadString();
            }

            bool hasGarrison = reader.ReadBoolean();
            if (hasGarrison)
            {
                town.GuardArmy = ReadCreatureSet(reader, 7);
            }

            town.GuardArmy.FormationType = (EArmyFormationType)reader.ReadByte();

            int castleId = this.ObjectTemplate.SubId;
            bool hasCustomBuildings = reader.ReadBoolean();
            if (hasCustomBuildings)
            {
                HashSet<int> buildings = new HashSet<int>();
                reader.ReadBitMask(buildings, 6, 48, false);

                HashSet<int> forbiddenBuildings = new HashSet<int>();
                reader.ReadBitMask(forbiddenBuildings, 6, 48, false);

                town.Buildings = ConvertBuildings(buildings, castleId);
                town.ForbiddenBuildings = ConvertBuildings(forbiddenBuildings, castleId);
            }
            // Standard buildings
            else
            {
                bool hasFort = reader.ReadBoolean();
                if (hasFort)
                {
                    town.Buildings.Add(EBuildingId.FORT);
                }

                //means that set of standard building should be included
                town.Buildings.Add(EBuildingId.DEFAULT);
            }

            if (MapHeader.Version > EMapFormat.ROE)
            {
                for (int i = 0; i < 9; ++i)
                {
                    byte c = reader.ReadByte();
                    for (int yy = 0; yy < 8; ++yy)
                    {
                        if (i * 8 + yy < GameConstants.SPELLS_QUANTITY)
                        {
                            if (c == (c | 1 << yy)) //add obligatory spell even if it's banned on a map (?)
                            {
                                town.ObligatorySpells.Add((ESpellId)(i * 8 + yy));
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < 9; ++i)
            {
                byte c = reader.ReadByte();
                for (int yy = 0; yy < 8; ++yy)
                {
                    int spellid = i * 8 + yy;
                    if (spellid < GameConstants.SPELLS_QUANTITY)
                    {
                        if (c != (c | 1 << yy)) //add random spell only if it's allowed on entire map
                        {
                            town.PossibleSpells.Add((ESpellId)(spellid));
                        }
                    }
                }
            }

            //add all spells from mods
            //TODO: allow customize new spells in towns
            //for (int i = SpellID::AFTER_LAST; i < VLC->spellh->objects.size(); ++i)
            //{
            //    nt->possibleSpells.push_back(SpellID(i));
            //}

            // Read castle events
            int numberOfEvent = (int)reader.ReadUInt32();
            for (int gh = 0; gh < numberOfEvent; ++gh)
            {
                CCastleEvent castleEvent = new CCastleEvent();
                castleEvent.Town = town;
                castleEvent.Name = reader.ReadString();
                castleEvent.Message = reader.ReadString();

                castleEvent.Resources = ReadResouces(reader);

                castleEvent.Players = reader.ReadByte();
                
                if (MapHeader.Version > EMapFormat.AB)
                {
                    castleEvent.HumanAffected = reader.ReadByte();
                }
                else
                {
                    castleEvent.HumanAffected = 0x01;
                }

                castleEvent.ComputerAffected = reader.ReadByte();
                castleEvent.FirstOccurence = reader.ReadUInt16();
                castleEvent.NextOccurence = reader.ReadByte();

                reader.Skip(17);

                // New buildings
                HashSet<int> buildings = new HashSet<int>();
                reader.ReadBitMask(buildings, 6, 48, false);

                //// castleEvent.Buildings = ConvertBuildings(buildings, castleId, false);

                castleEvent.Creatures.Clear();
                for (int vv = 0; vv < 7; ++vv)
                {
                    var creatureId = reader.ReadUInt16();
                    castleEvent.Creatures.Add((ECreatureId)creatureId);
                }

                reader.Skip(4);
                town.Events.Add(castleEvent);
            }

            if (MapHeader.Version > EMapFormat.AB)
            {
                town.Alignment = reader.ReadByte();
            }
            reader.Skip(3);

            return town;
        }

        private HashSet<EBuildingId> ConvertBuildings(HashSet<int> buildings, int castleId)
        {

            return null;
        }
    }

    public class CGMineReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            throw new NotImplementedException();
        }
    }

    public class CGCreatureGeneratorReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            throw new NotImplementedException();
        }
    }

    public class CGShrineReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            throw new NotImplementedException();
        }
    }

    public class CGPandoraBoxReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            throw new NotImplementedException();
        }
    }

    public class CGGrailReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            throw new NotImplementedException();
        }
    }

    public class CGDwellingReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            throw new NotImplementedException();
        }
    }

    public class CGQuestGuardReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            throw new NotImplementedException();
        }
    }

    public class CGShipyardReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            throw new NotImplementedException();
        }
    }

    public class CGHeroPlaceholderReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            throw new NotImplementedException();
        }
    }

    public class CGBorderGuardReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            throw new NotImplementedException();
        }
    }

    public class CGBorderGateReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            throw new NotImplementedException();
        }
    }

    public class CGPyramidReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            throw new NotImplementedException();
        }
    }


    public class CGLightHouseReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            throw new NotImplementedException();
        }
    }




}
