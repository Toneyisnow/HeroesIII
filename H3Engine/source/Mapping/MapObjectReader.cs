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


        protected void ReadResouces(BinaryReader reader)
        {
            for (int x = 0; x < 7; ++x)
            {
                var num = reader.ReadUInt32();
            }
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
                reader.ReadBitMask(town.Buildings, 6, 48, false);
                reader.ReadBitMask(town.ForbiddenBuildings, 6, 48, false);

                town.Buildings = ConvertBuildings(town.Buildings, castleId);
                town.ForbiddenBuildings = ConvertBuildings(town.ForbiddenBuildings, castleId);
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
                                nt->obligatorySpells.push_back(ESpellId(i * 8 + yy));
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < 9; ++i)
            {
                ui8 c = reader.readUInt8();
                for (int yy = 0; yy < 8; ++yy)
                {
                    int spellid = i * 8 + yy;
                    if (spellid < GameConstants::SPELLS_QUANTITY)
                    {
                        if (c != (c | static_cast<ui8>(std::pow(2., yy))) && map->allowedSpell[spellid]) //add random spell only if it's allowed on entire map
                        {
                            nt->possibleSpells.push_back(SpellID(spellid));
                        }
                    }
                }
            }
            //add all spells from mods
            //TODO: allow customize new spells in towns
            for (int i = SpellID::AFTER_LAST; i < VLC->spellh->objects.size(); ++i)
            {
                nt->possibleSpells.push_back(SpellID(i));
            }

            // Read castle events
            int numberOfEvent = reader.readUInt32();

            for (int gh = 0; gh < numberOfEvent; ++gh)
            {
                CCastleEvent nce;
                nce.town = nt;
                nce.name = reader.readString();
                nce.message = reader.readString();

                readResourses(nce.resources);

                nce.players = reader.readUInt8();
                if (map->version > EMapFormat::AB)
                {
                    nce.humanAffected = reader.readUInt8();
                }
                else
                {
                    nce.humanAffected = true;
                }

                nce.computerAffected = reader.readUInt8();
                nce.firstOccurence = reader.readUInt16();
                nce.nextOccurence = reader.readUInt8();

                reader.skip(17);

                // New buildings

                readBitmask(nce.buildings, 6, 48, false);

                nce.buildings = convertBuildings(nce.buildings, castleID, false);

                nce.creatures.resize(7);
                for (int vv = 0; vv < 7; ++vv)
                {
                    nce.creatures[vv] = reader.readUInt16();
                }
                reader.skip(4);
                nt->events.push_back(nce);
            }

            if (map->version > EMapFormat::AB)
            {
                nt->alignment = reader.readUInt8();
            }
            reader.skip(3);

            return nt;
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
