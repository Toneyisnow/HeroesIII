using H3Engine.Common;
using H3Engine.Components;
using H3Engine.Core;
using H3Engine.FileSystem;
using H3Engine.MapObjects;
using H3Engine.Utils;
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
        public H3Map Map
        {
            get; set;
        }

        public MapHeader MapHeader
        {
            get; set;
        }

        public ObjectTemplate ObjectTemplate
        {
            get; set;
        }

        public abstract CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition);


        protected ResourceSet ReadResources(BinaryReader reader)
        {
            ResourceSet resources = new ResourceSet();

            for (int x = 0; x < 7; ++x)
            {
                var num = reader.ReadUInt32();
            }

            return resources;
        }

        /// <summary>
        /// Read the showing message and guards for this object
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="eventObject"></param>
        protected void ReadMessageAndGuards(BinaryReader reader, ArmedInstance armedObject)
        {
            bool hasMessage = reader.ReadBoolean();
            if (hasMessage)
            {
                armedObject.Message = reader.ReadStringWithLength();
                bool hasGuards = reader.ReadBoolean();
                if (hasGuards)
                {
                    armedObject.GuardArmy = ReadCreatureSet(reader, 7);
                }
                reader.Skip(4);
            }
        }

        protected CreatureSet ReadCreatureSet(BinaryReader reader, int numberToRead)
        {
            bool isHighVersion = this.MapHeader.Version > EMapFormat.ROE;
            int maxID = isHighVersion ? 0xffff : 0xff;

            CreatureSet creatureSet = new CreatureSet();
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

            return creatureSet;
        }

        protected CQuest ReadQuest(BinaryReader reader)
        {
            CQuest quest = new CQuest();
            quest.MissionType = (CQuest.EMissionType)reader.ReadByte();
            
            switch (quest.MissionType)
            {
                case CQuest.EMissionType.MISSION_NONE:
                    break;
                case CQuest.EMissionType.MISSION_PRIMARY_STAT:
                    quest.M2Stats = new List<uint>(4);
                    for (int x = 0; x < 4; ++x)
                    {
                        uint val = (uint)reader.ReadByte();
                        quest.M2Stats.Add(val);
                    }
                    break;
                case CQuest.EMissionType.MISSION_LEVEL:
                case CQuest.EMissionType.MISSION_KILL_HERO:
                case CQuest.EMissionType.MISSION_KILL_CREATURE:
                    quest.M13489val = reader.ReadUInt32();
                    break;
                case CQuest.EMissionType.MISSION_ART:
                    int artNumber = reader.ReadByte();
                    for (int yy = 0; yy < artNumber; ++yy)
                    {
                        ushort artid = reader.ReadUInt16();
                        quest.M5Artifacts.Add(artid);
                        ///// map->allowedArtifact[artid] = false; //these are unavailable for random generation
                    }
                    break;
                case CQuest.EMissionType.MISSION_ARMY:
                    int typeNumber = (int)reader.ReadByte();
                    quest.M6Creatures = new List<StackDescriptor>(typeNumber);
                    for (int hh = 0; hh < typeNumber; ++hh)
                    {
                        UInt16 creatureId = reader.ReadUInt16();
                        StackDescriptor stack = new StackDescriptor();
                        stack.Creature = new H3Creature();
                        stack.Amount = reader.ReadUInt16();
                    }
                    break;
                case CQuest.EMissionType.MISSION_RESOURCES:

                    quest.M7Resources = new List<uint>(7);
                    for (int x = 0; x < 7; ++x)
                    {
                        uint amount = reader.ReadUInt32();
                        quest.M7Resources.Add(amount);
                    }
                    break;
                case CQuest.EMissionType.MISSION_HERO:
                case CQuest.EMissionType.MISSION_PLAYER:
                    quest.M13489val = reader.ReadByte();
                    break;
            }

            uint limit = reader.ReadUInt32();
            if (limit == 0xffffffff)
            {
                quest.LastDay = -1;
            }
            else
            {
                quest.LastDay = (int)limit;
            }

            quest.FirstVisitText = reader.ReadStringWithLength();
            quest.NextVisitText = reader.ReadStringWithLength();
            quest.CompletedText = reader.ReadStringWithLength();
            quest.IsCustomFirst = quest.FirstVisitText.Length > 0;
            quest.IsCustomNext = quest.NextVisitText.Length > 0;
            quest.IsCustomComplete = quest.CompletedText.Length > 0;

            return quest;
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

                case EObjectType.RANDOM_RESOURCE:
                case EObjectType.RESOURCE:
                    return new CGResourceReader();
                
                case EObjectType.RANDOM_TOWN:
                case EObjectType.TOWN:
                    return new CGTownReader();

                case EObjectType.MINE:
                case EObjectType.ABANDONED_MINE:
                    return new CGMineReader();

                case EObjectType.CREATURE_GENERATOR1:
                case EObjectType.CREATURE_GENERATOR2:
                case EObjectType.CREATURE_GENERATOR3:
                case EObjectType.CREATURE_GENERATOR4:
                    return new CGDwellingSimpleReader();

                case EObjectType.SHRINE_OF_MAGIC_GESTURE:
                case EObjectType.SHRINE_OF_MAGIC_INCANTATION:
                case EObjectType.SHRINE_OF_MAGIC_THOUGHT:
                    return new CGShrineReader();

                case EObjectType.PANDORAS_BOX:
                    return new CGPandoraBoxReader();

                case EObjectType.GRAIL:
                    return new CGGrailReader();

                case EObjectType.RANDOM_DWELLING:
                case EObjectType.RANDOM_DWELLING_FACTION:
                case EObjectType.RANDOM_DWELLING_LVL:
                    return new CGDwellingReader();

                case EObjectType.QUEST_GUARD:
                    return new CGQuestGuardReader();

                case EObjectType.HERO_PLACEHOLDER:
                    return new CGHeroPlaceholderReader();

                case EObjectType.BORDERGUARD:
                    return new CGBorderGuardReader();

                case EObjectType.BORDER_GATE:
                    return new CGBorderGateReader();

                case EObjectType.PYRAMID:
                    return new CGPyramidReader();

                case EObjectType.LIGHTHOUSE:
                    return new CGLightHouseReader();
                    
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

            ResourceSet resources = ReadResources(reader);

            for (int x = 0; x < 4; x++)
            {
                var primSkill = (EPrimarySkill)reader.ReadByte();
            }

            int gainedAbilities = reader.ReadByte();
            for (int i = 0; i < gainedAbilities; i++)
            {
                ESecondarySkill skill = (ESecondarySkill)reader.ReadByte();
                ESecondarySkillLevel level = (ESecondarySkillLevel)reader.ReadByte();

                eventObject.Abilities.Add(new AbilitySkill(skill, level));
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
        public static bool LoadArtifactToSlot(BinaryReader reader, H3Map map, HeroInstance hero, int slotIndex)
        {
            int artmask = 0xffff;
            if (map.Header.Version == EMapFormat.ROE)
            {
                artmask = 0xff;
            }

            int aid = reader.ReadUInt16();

            bool isArt = (aid != artmask);
            if (isArt)
            {
                Console.WriteLine("loadArtifactToSlot: id={0}, slot={1}", aid, slotIndex);

                ArtifactSet artifactSet = hero.Data.Artifacts;

                EArtifactId artifactId = (EArtifactId)aid;
                H3Artifact artifact = new H3Artifact(artifactId);

                if (artifact.IsBig() && slotIndex > 19)
                {
                    return false;
                }

                EArtifactPosition slot = (EArtifactPosition)slotIndex;
                if (aid == 0 && slot == EArtifactPosition.MISC5)
                {
                    //TODO: check how H3 handles it -> art 0 in slot 18 in AB map
                    slot = EArtifactPosition.SPELLBOOK;
                }

                // this is needed, because some H3M maps (last scenario of ROE map) contain invalid data like misplaced artifacts
                //// auto artifact = CArtifactInstance::createArtifact(map, aid);
                //// auto artifactPos = ArtifactPosition(slot);

                if (artifactSet.CanPutAt(artifactId, slot))
                {
                    artifactSet.PutAt(artifactId, slot);
                }


                return true;
            }

            return false;
        }

        public static void LoadArtifactsOfHero(BinaryReader reader, H3Map map, HeroInstance hero)
        {
            hero.Data.Artifacts = new ArtifactSet();
            bool artSet = reader.ReadBoolean();
            if (artSet)
            {
                Console.WriteLine("Artifact is set.");

                if (false)
                {
                    // Already set the pack
                }

                for (int pom = 0; pom < 16; pom++)
                {
                    LoadArtifactToSlot(reader, map, hero, pom);
                }

                // misc5 art 17
                if (map.Header.Version >= EMapFormat.SOD)
                {
                    if (!LoadArtifactToSlot(reader, map, hero, (int)EArtifactPosition.MACH4))   //catapult
                    {
                        hero.Data.Artifacts.PutAt(EArtifactId.CATAPULT, EArtifactPosition.MACH4);
                    }
                }

                LoadArtifactToSlot(reader, map, hero, (int)EArtifactPosition.SPELLBOOK);   //SpellBook


                // Misc5 possibly
                if (map.Header.Version > EMapFormat.ROE)
                {
                    LoadArtifactToSlot(reader, map, hero, (int)EArtifactPosition.MISC5);   //Misc
                }
                else
                {
                    reader.Skip(1);
                }

                // Backpack items
                int amount = reader.ReadUInt16();
                Console.WriteLine("Backpack item amount:" + amount);
                for (int ss = 0; ss < amount; ++ss)
                {
                    LoadArtifactToSlot(reader, map, hero, 19 + ss);
                }
            }
        }

        private void ReadSpells(BinaryReader reader, HeroInstance hero)
        {
            hero.Data.Spells = new List<ESpellId>();
            hero.Data.Spells.Add(ESpellId.PRESET);

            HashSet<int> result = new HashSet<int>();
            reader.ReadBitMask(result, 9, GameConstants.SPELLS_QUANTITY, false);
            
            foreach (int sp in result)
            {
                hero.Data.Spells.Add((ESpellId)sp);
            }
        }

        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            HeroInstance hero = new HeroInstance();

            if (MapHeader.Version > EMapFormat.ROE)
            {
                uint identifier = reader.ReadUInt32();
                //// map->questIdentifierToId[identifier] = objectId;
            }

            EPlayerColor owner = (EPlayerColor)reader.ReadByte();
            hero.SubId = reader.ReadByte();
            
            //If hero of this type has been predefined, use that as a base.
            //Instance data will overwrite the predefined values where appropriate.
            foreach(var preHero in this.Map.PredefinedHeroes)
            {
                if (preHero.SubId == hero.SubId)
                {
                    //logGlobal->debug("Hero %d will be taken from the predefined heroes list.", nhi->subID);
                    //delete nhi;
                    hero = preHero;
                    break;
                }
            }
            hero.SetOwner(owner);
            
            //// nhi->portrait = nhi->subID;
            foreach( DisposedHero disHero in this.Map.DisposedHeroes)
            {
                if (disHero.HeroId == hero.SubId)
                {
                    hero.Data.Name = disHero.Name;
                    hero.Data.PortaitIndex = disHero.Portrait;
                    break;
                }
            }

            bool hasName = reader.ReadBoolean();
            if (hasName)
            {
                hero.Data.Name = reader.ReadStringWithLength();
            }

            if (MapHeader.Version > EMapFormat.AB)
            {
                bool hasExp = reader.ReadBoolean();
                if (hasExp)
                {
                    hero.Data.Experience = reader.ReadUInt32();
                }
                else
                {
                    hero.Data.Experience = 0xffffffff;
                }
            }
            else
            {
                hero.Data.Experience = reader.ReadUInt32();

                //0 means "not set" in <=AB maps
                if (hero.Data.Experience == 0)
                {
                    hero.Data.Experience = 0xffffffff;
                }
            }

            bool hasPortrait = reader.ReadBoolean();
            if (hasPortrait)
            {
                hero.Data.PortaitIndex = reader.ReadByte();
            }

            bool hasSecSkills = reader.ReadBoolean();
            if (hasSecSkills)
            {
                // Replacing with current data
                hero.Data.SecondarySkills = new List<AbilitySkill>();

                uint howMany = reader.ReadUInt32();
                for (int yy = 0; yy < howMany; ++yy)
                {
                    ESecondarySkill skill = (ESecondarySkill)reader.ReadByte();
                    ESecondarySkillLevel level = (ESecondarySkillLevel)reader.ReadByte();

                    AbilitySkill abilitySkill = new AbilitySkill(skill, level);
                    hero.Data.SecondarySkills.Add(abilitySkill);
                }
            }

            bool hasGarison = reader.ReadBoolean();
            if (hasGarison)
            {
                hero.Data.Army = ReadCreatureSet(reader, 7);
                hero.Data.Army.FormationType = (EArmyFormationType)reader.ReadByte();
            }
            else
            {
                reader.ReadByte();
            }

            LoadArtifactsOfHero(reader, this.Map, hero);

            hero.Patrol = new HeroPatrol();
            hero.Patrol.PatrolRadius = reader.ReadByte();
            
            if (hero.Patrol.PatrolRadius == 0xff)
            {
                hero.Patrol.IsPatrolling = false;
            }
            else
            {
                hero.Patrol.IsPatrolling = true;
                hero.Patrol.InitialPosition = objectPosition;
            }

            if (this.MapHeader.Version > EMapFormat.ROE)
            {
                bool hasCustomBiography = reader.ReadBoolean();
                if (hasCustomBiography)
                {
                    hero.Data.Biography = reader.ReadStringWithLength();
                }
                hero.Data.Sex = reader.ReadByte();

                // Remove trash
                if (hero.Data.Sex != 0xFF)
                {
                    hero.Data.Sex &= 1;
                }
            }
            else
            {
                hero.Data.Sex = 0xFF;
            }

            // Spells
            if (this.MapHeader.Version > EMapFormat.AB)
            {
                bool hasCustomSpells = reader.ReadBoolean();
                if (hasCustomSpells)
                {
                    ReadSpells(reader, hero);
                }
            }
            else if (this.MapHeader.Version == EMapFormat.AB)
            {
                //we can read one spell
                byte buff = reader.ReadByte();
                if (buff != 254)
                {
                    hero.Data.Spells = new List<ESpellId>();
                    hero.Data.Spells.Add(ESpellId.PRESET);
                    
                    if (buff < 254) //255 means no spells
                    {
                        hero.Data.Spells.Add((ESpellId)buff);
                    }
                }
            }

            if (this.MapHeader.Version > EMapFormat.AB)
            {
                bool hasCustomPrimSkills = reader.ReadBoolean();
                if (hasCustomPrimSkills)
                {
                    hero.Data.PrimarySkills = new List<int>();
                    for (int xx = 0; xx < GameConstants.PRIMARY_SKILLS; ++xx)
                    {
                        hero.Data.PrimarySkills.Add(reader.ReadByte());
                    }
                }
            }

            reader.Skip(16);

            return hero;
        }
    }

    public class CGCreatureReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            if (this.ObjectTemplate.Type == EObjectType.RANDOM_MONSTER)
            {
                for (int i = 0; i < 20; i++)
                {
                    byte[] data = reader.ReadBytes(10);
                    Console.WriteLine(StringUtils.ByteArrayToString(data));
                }

                reader.BaseStream.Seek(-200, SeekOrigin.Current);
            }

            // Create Creature
            CGCreature creature = new CGCreature();

            if (MapHeader.Version > EMapFormat.ROE)
            {
                creature.Identifier = reader.ReadUInt32();
                // Quest Identifier?
            }
            
            StackDescriptor stack = new StackDescriptor();
            stack.Amount = reader.ReadUInt16();

            //type will be set during initialization
            creature.AddStack(0, stack);

            creature.Friendliness = reader.ReadByte();

            bool hasMessage = reader.ReadBoolean();
            if (hasMessage)
            {
                creature.Message = reader.ReadStringWithLength();
                creature.GainResources = ReadResources(reader);

                int artId;
                if (this.MapHeader.Version == EMapFormat.ROE)
                {
                    artId = reader.ReadByte();
                }
                else
                {
                    artId = reader.ReadUInt16();
                }

                if (this.MapHeader.Version == EMapFormat.ROE && artId == 0xff || this.MapHeader.Version != EMapFormat.ROE && artId == 0xffff)
                {
                    creature.GainArtifact = EArtifactId.NONE;
                }
                else
                {
                    creature.GainArtifact = (EArtifactId)artId;
                }
            }

            creature.NeverFlee = (reader.ReadByte() > 0);
            creature.NotGrowingTeam = (reader.ReadByte() > 0);

            reader.Skip(2);

            return creature;
        }
    }

    public class CGSignBottleReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            CGSignBottle signBottle = new CGSignBottle();
            signBottle.Message = reader.ReadStringWithLength();
            reader.Skip(4);

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
                seerHut.Quest = ReadQuest(reader);
            }
            else
            {
                //RoE
                byte artifactId = reader.ReadByte();
                if (artifactId != 255)
                {
                    //not none quest
                    seerHut.Quest.M5Artifacts.Add(artifactId);
                    seerHut.Quest.MissionType = CQuest.EMissionType.MISSION_ART;
                }
                else
                {
                    seerHut.Quest.MissionType = CQuest.EMissionType.MISSION_NONE;
                }

                seerHut.Quest.LastDay = -1; //no timeout
                seerHut.Quest.IsCustomFirst = seerHut.Quest.IsCustomNext = seerHut.Quest.IsCustomComplete = false;
            }

            if (seerHut.Quest.MissionType != CQuest.EMissionType.MISSION_NONE)
            {
                seerHut.RewardType = (CGSeerHut.ERewardType)reader.ReadByte();
                switch (seerHut.RewardType)
                {
                    case CGSeerHut.ERewardType.EXPERIENCE:
                        {
                            seerHut.RewardValue = (int)reader.ReadUInt32();
                            break;
                        }
                    case CGSeerHut.ERewardType.MANA_POINTS:
                        {
                            seerHut.RewardValue = (int)reader.ReadUInt32();
                            break;
                        }
                    case CGSeerHut.ERewardType.MORALE_BONUS:
                        {
                            seerHut.RewardValue = (int)reader.ReadByte();
                            break;
                        }
                    case CGSeerHut.ERewardType.LUCK_BONUS:
                        {
                            seerHut.RewardValue = (int)reader.ReadByte();
                            break;
                        }
                    case CGSeerHut.ERewardType.RESOURCES:
                        {
                            seerHut.RewardId = (int)reader.ReadByte();

                            // Only the first 3 bytes are used. Skip the 4th.
                            seerHut.RewardValue = (int)reader.ReadUInt32() & 0x00ffffff;
                            break;
                        }
                    case CGSeerHut.ERewardType.PRIMARY_SKILL:
                        {
                            seerHut.RewardId = (int)reader.ReadByte();
                            seerHut.RewardValue = (int)reader.ReadByte();
                            break;
                        }
                    case CGSeerHut.ERewardType.SECONDARY_SKILL:
                        {
                            seerHut.RewardId = (int)reader.ReadByte();
                            seerHut.RewardValue = (int)reader.ReadByte();
                            break;
                        }
                    case CGSeerHut.ERewardType.ARTIFACT:
                        {
                            if (MapHeader.Version == EMapFormat.ROE)
                            {
                                seerHut.RewardId = (int)reader.ReadByte();
                            }
                            else
                            {
                                seerHut.RewardId = (int)reader.ReadUInt16();
                            }
                            break;
                        }
                    case CGSeerHut.ERewardType.SPELL:
                        {
                            seerHut.RewardId = (int)reader.ReadByte();
                            break;
                        }
                    case CGSeerHut.ERewardType.CREATURE:
                        {
                            if (MapHeader.Version > EMapFormat.ROE)
                            {
                                seerHut.RewardId = (int)reader.ReadUInt16();
                                seerHut.RewardValue = (int)reader.ReadUInt16();
                            }
                            else
                            {
                                seerHut.RewardId = (int)reader.ReadByte();
                                seerHut.RewardValue = (int)reader.ReadUInt16();
                            }
                            break;
                        }
                }
                reader.Skip(2);
            }
            else
            {
                // missionType==255
                reader.Skip(3);
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
                town.TownName = reader.ReadStringWithLength();
            }

            bool hasGarrison = reader.ReadBoolean();
            if (hasGarrison)
            {
                town.GuardArmy = ReadCreatureSet(reader, 7);
                town.GuardArmy.FormationType = (EArmyFormationType)reader.ReadByte();
            }
            else
            {
                reader.ReadByte();
            }

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
                castleEvent.Name = reader.ReadStringWithLength();
                castleEvent.Message = reader.ReadStringWithLength();

                castleEvent.Resources = ReadResources(reader);

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
            CGMine mine = new CGMine();

            mine.SetOwner((EPlayerColor)reader.ReadByte());
            reader.Skip(3);

            return mine;
        }
    }

    public class CGCreatureGeneratorReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            CGMine mine = new CGMine();
            mine.SetOwner((EPlayerColor)(reader.ReadByte()));
            reader.Skip(3);
            return mine;
        }
    }

    public class CGShrineReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            var shrine = new CGShrine();

            byte rawId = reader.ReadByte();

            if (255 == rawId)
            {
                shrine.SpellId = ESpellId.NONE;
            }
            else
            {
                shrine.SpellId = (ESpellId)rawId;
            }

            reader.Skip(3);
            return shrine;
        }
    }

    public class CGPandoraBoxReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            CGPandoraBox box = new CGPandoraBox();

            ReadMessageAndGuards(reader, box);

            box.GainExperience = reader.ReadUInt32();
            box.ManaDiff = reader.ReadUInt32();
            box.MoraleDiff = reader.ReadByte();
            box.LuckDiff = reader.ReadByte();

            box.GainResources = ReadResources(reader);

            box.GainPrimarySkills = new List<int>(4);
            for (int x = 0; x < 4; ++x)
            {
                box.GainPrimarySkills.Add(reader.ReadByte());
            }

            box.GainSecondarySkills = new List<ESecondarySkill>();
            box.GainAbilityLevels = new List<int>();
            int gabn = reader.ReadByte();//number of gained abilities
            for (int oo = 0; oo < gabn; ++oo)
            {
                box.GainSecondarySkills.Add((ESecondarySkill)reader.ReadByte());
                box.GainAbilityLevels.Add(reader.ReadByte());
            }

            box.GainArtifacts = new List<EArtifactId>();
            int gart = reader.ReadByte(); //number of gained artifacts
            for (int oo = 0; oo < gart; ++oo)
            {
                if (MapHeader.Version > EMapFormat.ROE)
                {
                    box.GainArtifacts.Add((EArtifactId)reader.ReadUInt16());
                }
                else
                {
                    box.GainArtifacts.Add((EArtifactId)reader.ReadByte());
                }
            }

            box.GainSpells = new List<ESpellId>();
            int gspel = reader.ReadByte(); //number of gained spells
            for (int oo = 0; oo < gspel; ++oo)
            {
                box.GainSpells.Add((ESpellId)reader.ReadByte());
            }

            box.GainCreatures = new CreatureSet();
            int gcre = reader.ReadByte(); //number of gained creatures
            box.GainCreatures = ReadCreatureSet(reader, gcre);
            reader.Skip(8);

            return box;
        }
    }

    public class CGGrailReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            CGGrail grail = new CGGrail();

            grail.Position = objectPosition;
            grail.Radius = reader.ReadUInt32();

            return grail;
        }
    }

    public class CGDwellingSimpleReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            CGDwelling dwelling = new CGDwelling();
            dwelling.SetOwner((EPlayerColor)reader.ReadByte());
            reader.Skip(3);

            return dwelling;
        }
    }

    public class CGDwellingReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            CGDwelling dwelling = new CGDwelling();

            CreatureGeneratorAsCastleInfo castleInfo = null;
            CreatureGeneratorAsLeveledInfo levelInfo = null;
            
            DwellingSpecInfo spec = null;
            switch (this.ObjectTemplate.Type)
            {
                case EObjectType.RANDOM_DWELLING:
                    castleInfo = new CreatureGeneratorAsCastleInfo(dwelling);
                    levelInfo = new CreatureGeneratorAsLeveledInfo(dwelling);
                    break;
                case EObjectType.RANDOM_DWELLING_LVL:
                    castleInfo = new CreatureGeneratorAsCastleInfo(dwelling);
                    break;
                case EObjectType.RANDOM_DWELLING_FACTION:
                    levelInfo = new CreatureGeneratorAsLeveledInfo(dwelling);
                    break;
                default:
                    break;
            }
            spec.Owner = dwelling;

            dwelling.SetOwner((EPlayerColor)reader.ReadUInt32());

            //216 and 217
            if (castleInfo != null)
		    {
                castleInfo.InstanceId = string.Empty;
                castleInfo.Identifier = reader.ReadUInt32();
                
                if (castleInfo.Identifier == 0)
                {
                    castleInfo.AsCastle = false;
                    
                    const int MASK_SIZE = 8;
                    byte[] mask = new byte[2];
                    mask[0] = reader.ReadByte();
                    mask[1] = reader.ReadByte();

                    castleInfo.AllowedFactions = new List<bool>();

                    for (int i = 0; i < MASK_SIZE; i++)
                    {
                        bool val = ((mask[0] & (1 << i)) > 0);
                        castleInfo.AllowedFactions.Add(val);
                    }

                    for (int i = 0; i < (GameConstants.F_NUMBER - MASK_SIZE); i++)
                    {
                        bool val = ((mask[1] & (1 << i)) > 0);
                        castleInfo.AllowedFactions.Add(val);
                    }
                }
                else
                {
                    castleInfo.AsCastle = true;
                }

                dwelling.SpecAsCastle = castleInfo;
            }

            //216 and 218
            if (levelInfo != null)
	        {
                levelInfo.MinLevel = Math.Max(reader.ReadByte(), (byte)1);
                levelInfo.MaxLevel = Math.Max(reader.ReadByte(), (byte)7);

                dwelling.SpecAsLeveled = levelInfo;
            }

            return dwelling;
        }
    }

    public class CGQuestGuardReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            CGQuestGuard guard = new CGQuestGuard();
            guard.Quest = ReadQuest(reader);

            return guard;
        }
    }

    public class CGShipyardReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            CGShipyard shipyard = new CGShipyard();
            shipyard.SetOwner((EPlayerColor)reader.ReadUInt32());

            return shipyard;
        }
    }

    public class CGHeroPlaceholderReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            CGHeroPlaceHolder heroPlaceHolder = new CGHeroPlaceHolder();

            heroPlaceHolder.SetOwner((EPlayerColor)reader.ReadByte());

            int heroTypeId = reader.ReadByte();
            heroPlaceHolder.SubId = heroTypeId;

            if (heroTypeId == 0xff)
            {
                heroPlaceHolder.Power = reader.ReadByte();
            }
            else
            {
                heroPlaceHolder.Power = 0;
            }

            return heroPlaceHolder;
        }
    }

    public class CGBorderGuardReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            CGBorderGuard guard = new CGBorderGuard();

            return guard;
        }
    }

    public class CGBorderGateReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            CGBorderGate gate = new CGBorderGate();
            return gate;
        }
    }

    public class CGPyramidReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            // This is WOG Object, ignore it for now

            return new CGObject();
        }
    }

    public class CGLightHouseReader : MapObjectReader
    {
        public override CGObject ReadObject(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            CGLighthouse lighthouse = new CGLighthouse();

            lighthouse.SetOwner((EPlayerColor)reader.ReadUInt32());

            return lighthouse;
        }
    }




}
