using H3Engine.Common;
using H3Engine.Core;
using H3Engine.FileSystem;
using H3Engine.MapObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.Components.Mapping
{
    public class H3MapLoader
    {
        private H3Map mapObject = null;

        private string h3mFileFullPath = null;

        public H3MapLoader(string fileFullPath)
        {
            this.h3mFileFullPath = fileFullPath;
        }

        public H3Map LoadMap()
        {
            mapObject = new H3Map();

            using (FileStream file = new FileStream(h3mFileFullPath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(file))
                {

                    ReadHeader(reader);

                    ReadDisposedHeroes(reader);

                    ReadAllowedArtifacts(reader);

                    ReadAllowedSpellsAbilities(reader);

                    ReadRumors(reader);

                    ReadPredefinedHeroes(reader);

                    ReadTerrain(reader);

                    ReadObjectTemplates(reader);

                    // ReadObjects(reader);

                }
            }

            return mapObject;
        }
        
        private void ReadHeader(BinaryReader reader)
        {
            // Check map for validity
            // Note: disabled, causes decompression of the entire file ( = SLOW)
            //if(inputStream->getSize() < 50)
            //{
            //	throw std::runtime_error("Corrupted map file.");
            //}

            // Map version
            UInt32 byte1 = reader.ReadUInt32();
            Console.WriteLine("Map version:" + byte1);
            mapObject.Header.Version = (EMapFormat)byte1;


            mapObject.Header.AreAnyPlayers = reader.ReadBoolean();
            Console.WriteLine("AreAnyPlayers:" + mapObject.Header.AreAnyPlayers);

            mapObject.Header.Height = reader.ReadUInt32();
            mapObject.Header.Width = mapObject.Header.Height;
            Console.WriteLine("Map Height and Width:" + mapObject.Header.Height);

            mapObject.Header.IsTwoLevel = reader.ReadBoolean();
            Console.WriteLine("twoLevel:" + mapObject.Header.IsTwoLevel);


            mapObject.Header.Name = reader.ReadString();
            Console.WriteLine("Name:" + mapObject.Header.Name);

            mapObject.Header.Description = reader.ReadString();
            Console.WriteLine("Description:" + mapObject.Header.Description);

            mapObject.Header.Difficulty = reader.ReadByte();
            Console.WriteLine("Difficulty:" + mapObject.Header.Difficulty);

            int heroLevelLimit = reader.ReadByte();
            Console.WriteLine("HeroLevelLimit:" + heroLevelLimit);


            ReadPlayerInfo(reader);

            ReadVictoryLossConditions(reader);

            ReadTeamInfo(reader);

            ReadAllowedHeroes(reader);
        }


        private void ReadPlayerInfo(BinaryReader reader)
        {
            for (int i = 0; i < GameConstants.PLAYER_LIMIT_T; i++)
            {
                PlayerInfo playerInfo = new PlayerInfo();

                Console.WriteLine("Reading Player [" + i.ToString() + "]");

                playerInfo.CanHumanPlay = reader.ReadBoolean();
                playerInfo.CanComputerPlay = reader.ReadBoolean();
                Console.WriteLine("canHumanPlay: " + playerInfo.CanHumanPlay);
                Console.WriteLine("canComputerPlay: " + playerInfo.CanComputerPlay);

                if (!playerInfo.CanHumanPlay && !playerInfo.CanComputerPlay)
                {
                    switch (mapObject.Header.Version)
                    {
                        case EMapFormat.SOD:
                        case EMapFormat.WOG:
                            reader.Skip(13);
                            break;
                        case EMapFormat.AB:
                            reader.Skip(12);
                            break;
                        case EMapFormat.ROE:
                            reader.Skip(6);
                            break;
                    }
                    continue;
                }

                playerInfo.AiTactic = (EAiTactic)reader.ReadByte();
                Console.WriteLine("aiTactic:" + playerInfo.AiTactic);

                if (mapObject.Header.Version == EMapFormat.SOD || mapObject.Header.Version == EMapFormat.WOG)
                {
                    playerInfo.P7 = reader.ReadByte();
                }
                else
                {
                    playerInfo.P7 = -1;
                }

                Console.WriteLine("p7:" + playerInfo.P7);

                // Reading the Factions for Player
                playerInfo.AllowedFactions = new List<int>();
                int allowedFactionsMask = reader.ReadByte();
                Console.WriteLine("allowedFactionsMask:" + allowedFactionsMask);

                int totalFactionCount = GameConstants.F_NUMBER;
                if (mapObject.Header.Version != EMapFormat.ROE)
                    allowedFactionsMask += reader.ReadByte() << 8;
                else
                    totalFactionCount--; //exclude conflux for ROE
                
                for (int fact = 0; fact < totalFactionCount; ++fact)
                {
                    if ((allowedFactionsMask & (1 << fact)) > 0)
                    {
                        playerInfo.AllowedFactions.Add(fact);
                    }
                }

                playerInfo.IsFactionRandom = reader.ReadBoolean();
                playerInfo.HasMainTown = reader.ReadBoolean();
                Console.WriteLine("isFactionRandom:" + playerInfo.IsFactionRandom);
                Console.WriteLine("hasMainTown:" + playerInfo.HasMainTown);

                if (playerInfo.HasMainTown)
                {
                    /// Added in new version, not tested yet
                    if (mapObject.Header.Version != EMapFormat.ROE)
                    {
                        playerInfo.GenerateHeroAtMainTown = reader.ReadBoolean();
                        playerInfo.GenerateHero = reader.ReadBoolean();
                    }
                    else
                    {
                        playerInfo.GenerateHeroAtMainTown = true;
                        playerInfo.GenerateHero = false;
                    }

                    var townPosition = reader.ReadPosition();
                    Console.WriteLine(string.Format("Main Town Position: {0}, {1}, {2}", townPosition.PosX, townPosition.PosY, townPosition.Level));
                    playerInfo.MainTownPosition = townPosition;
                }

                playerInfo.HasRandomHero = reader.ReadBoolean();
                Console.WriteLine("hasRandomHero:" + playerInfo.HasRandomHero);

                playerInfo.MainCustomHeroId = reader.ReadByte();
                Console.WriteLine("mainCustomHeroId:" + playerInfo.MainCustomHeroId);

                if (playerInfo.MainCustomHeroId != 0xff)
                {
                    playerInfo.MainCustomHeroPortrait = reader.ReadByte();
                    if (playerInfo.MainCustomHeroPortrait == 0xff)
                    {
                        playerInfo.MainCustomHeroPortrait = -1;
                    }

                    playerInfo.MainCustomHeroName = reader.ReadString();
                    Console.WriteLine("mainCustomHeroPortrait:" + playerInfo.MainCustomHeroPortrait);
                    Console.WriteLine("heroName:" + playerInfo.MainCustomHeroName);

                }
                else
                {
                    playerInfo.MainCustomHeroId = -1;
                }

                if (mapObject.Header.Version != EMapFormat.ROE)
                {
                    playerInfo.PowerPlaceHolders = reader.ReadByte();
                    int heroCount = reader.ReadByte();
                    reader.Skip(3);

                    playerInfo.HeroIds = new List<H3HeroId>();
                    for (int pp = 0; pp < heroCount; ++pp)
                    {
                        H3HeroId heroId = new H3HeroId();
                        heroId.Id = reader.ReadByte();
                        heroId.Name = reader.ReadString();
                        playerInfo.HeroIds.Add(heroId);
                    }
                }
            }
        }
        
        private void ReadVictoryLossConditions(BinaryReader reader)
        {
            //// mapObject.Header.TrigggeredEvents = new List<TrigggeredEvent>();

            var vicCondition = (EVictoryConditionType)reader.ReadByte();

            if (vicCondition == EVictoryConditionType.WINSTANDARD)
            {
                // create normal condition
                
            }
            else
            {
                bool allowNormalVictory = reader.ReadBoolean();
                bool appliesToAI = reader.ReadBoolean();

                if (allowNormalVictory)
                {
                    int playersOnMap = 2;
                    if (playersOnMap == 1)
                    {
                        //// logGlobal->warn("Map %s has only one player but allows normal victory?", mapHeader->name);
                        allowNormalVictory = false; // makes sense? Not much. Works as H3? Yes!
                    }
                }

                switch (vicCondition)
                {
                    case EVictoryConditionType.ARTIFACT:
                        {
                            int objectType = reader.ReadByte();
                            break;
                        }
                    case EVictoryConditionType.GATHERTROOP:
                        {
                            int objectType = reader.ReadByte();
                            uint value = reader.ReadUInt32();
                            break;
                        }
                    case EVictoryConditionType.GATHERRESOURCE:
                        {
                            int objectType = reader.ReadByte();
                            uint value = reader.ReadUInt32();
                            break;
                        }
                    case EVictoryConditionType.BUILDCITY:
                        {
                            var pos = reader.ReadPosition();
                            int objectType = reader.ReadByte();
                            int objectType2 = reader.ReadByte();

                            break;
                        }
                    case EVictoryConditionType.BUILDGRAIL:
                        {
                            var pos = reader.ReadPosition();
                            break;
                        }
                    case EVictoryConditionType.BEATHERO:
                        {
                            var pos = reader.ReadPosition();
                            break;
                        }
                    case EVictoryConditionType.CAPTURECITY:
                        {
                            var pos = reader.ReadPosition();
                            break;
                        }
                    case EVictoryConditionType.BEATMONSTER:
                        {
                            var pos = reader.ReadPosition();
                            break;
                        }
                    case EVictoryConditionType.TAKEDWELLINGS:
                        {
                            break;
                        }
                    case EVictoryConditionType.TAKEMINES:
                        {
                            break;
                        }
                    case EVictoryConditionType.TRANSPORTITEM:
                        {
                            uint value = reader.ReadUInt32();
                            var pos = reader.ReadPosition();
                            break;
                        }
                    default:
                        break;
                }



            }


            ELossConditionType loseCondition = (ELossConditionType)reader.ReadByte();
            Console.WriteLine("Lose Condition:" + loseCondition);
            if (loseCondition != ELossConditionType.LOSSSTANDARD)
            {
                switch (loseCondition)
                {
                    case ELossConditionType.LOSSCASTLE:
                        {
                            var pos = reader.ReadPosition();
                            break;
                        }
                    case ELossConditionType.LOSSHERO:
                        {
                            var pos = reader.ReadPosition();
                            break;
                        }
                    case ELossConditionType.TIMEEXPIRES:
                        {
                            int val = reader.ReadUInt16();
                            break;
                        }
                    default:
                        break;
                }
            }

        }


        private void ReadTeamInfo(BinaryReader reader)
        {
            int howManyTeams = reader.ReadByte();
            Console.WriteLine("How Many Teams: " + howManyTeams);
            if (howManyTeams > 0)
            {
                for (int i = 0; i < GameConstants.PLAYER_LIMIT_T; i++)
                {
                    int team = reader.ReadByte();
                }
            }
            else
            {
                for (int i = 0; i < GameConstants.PLAYER_LIMIT_T; i++)
                {

                }
            }
        }


        private void ReadAllowedHeroes(BinaryReader reader)
        {
            int byteCount = 20; //// mapHeader->version == EMapFormat::ROE ? 16 : 20;

            HashSet<int> allowedHeroSet = new HashSet<int>();
            reader.ReadBitMask(allowedHeroSet, byteCount, GameConstants.HEROES_QUANTITY, false);

            // Probably reserved for further heroes
            if (true)
            {
                uint placeholdersQty = reader.ReadUInt32();

                reader.Skip((int)placeholdersQty);

                //		std::vector<ui16> placeholdedHeroes;
                //
                //		for(int p = 0; p < placeholdersQty; ++p)
                //		{
                //			placeholdedHeroes.push_back(reader.ReadByte());
                //		}
            }

        }

        private void ReadDisposedHeroes(BinaryReader reader)
        {
            if (mapObject.Header.Version >= EMapFormat.SOD)
            {
                int disp = reader.ReadByte();
                Console.WriteLine("ReadDisposedHeroes: Total=" + disp);
                for (int g = 0; g < disp; ++g)
                {
                    int heroId = reader.ReadByte();
                    int portrait = reader.ReadByte();
                    string name = reader.ReadString();
                    int players = reader.ReadByte();
                    Console.WriteLine(string.Format("ReadDisposedHeroes: id={0} portrait={1} name={2} players={3}", heroId, portrait, name, players));

                }
            }

            //omitting NULLS
            reader.Skip(31);
        }

        private void ReadAllowedArtifacts(BinaryReader reader)
        {
            if (mapObject.Header.Version != EMapFormat.ROE)
            {
                int bytes = (mapObject.Header.Version == EMapFormat.AB ? 17 : 18);

                HashSet<int> allowedList = new HashSet<int>();
                reader.ReadBitMask(allowedList, bytes, GameConstants.ARTIFACTS_QUANTITY);
            }
        }

        private void ReadAllowedSpellsAbilities(BinaryReader reader)
        {
            if (mapObject.Header.Version >= EMapFormat.SOD)
            {
                HashSet<int> allowedSpells = new HashSet<int>();
                HashSet<int> allowedSkills = new HashSet<int>();

                // Reading allowed spells (9 bytes)
                const int spell_bytes = 9;
                reader.ReadBitMask(allowedSpells, spell_bytes, GameConstants.SPELLS_QUANTITY);
                Console.WriteLine("allowedSpells: " + JsonConvert.SerializeObject(allowedSpells));


                // Allowed hero's abilities (4 bytes)
                const int skill_bytes = 4;
                reader.ReadBitMask(allowedSkills, skill_bytes, GameConstants.SKILL_QUANTITY);
                Console.WriteLine("allowedSkills: " + JsonConvert.SerializeObject(allowedSkills));
            }
        }

        private void ReadRumors(BinaryReader reader)
        {
            uint rumNr = reader.ReadUInt32();
            Console.WriteLine("Rumor count: " + rumNr);

            for (int it = 0; it < rumNr; it++)
            {
                string name = reader.ReadString();
                string text = reader.ReadString();
                Console.WriteLine(string.Format("Rumor: name={0} text={1}", name, text));
            }
        }

        private void ReadPredefinedHeroes(BinaryReader reader)
        {
            if (mapObject.Header.Version == EMapFormat.WOG || mapObject.Header.Version == EMapFormat.SOD)
            {
                for (int z = 0; z < GameConstants.HEROES_QUANTITY; z++)
                {
                    Console.WriteLine(string.Format("===Reading Predefined Hero [{0}]", z));

                    int custom = reader.ReadByte();
                    if (custom == 0)
                    {
                        Console.WriteLine("is not custom.");
                        continue;
                    }

                    // Create Hero
                    bool hasExp = reader.ReadBoolean();
                    if (hasExp)
                    {
                        uint exp = reader.ReadUInt32();
                        Console.WriteLine("Has exp:" + exp);
                    }

                    bool hasSecondSkills = reader.ReadBoolean();
                    if (hasSecondSkills)
                    {
                        uint howMany = reader.ReadUInt32();
                        Console.WriteLine("Has Second Skills count=" + howMany);

                        for (int yy = 0; yy < howMany; ++yy)
                        {
                            int first = reader.ReadByte();
                            int second = reader.ReadByte();
                            Console.WriteLine(string.Format("Skill First: {0} Second: {1}", first, second));
                        }
                    }

                    // Set Artifacts
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
                            LoadArtifactToSlot(reader, null, pom);
                        }

                        if (true)
                        {
                            LoadArtifactToSlot(reader, null, 0);   //catapult
                        }

                        LoadArtifactToSlot(reader, null, 0);   //SpellBook


                        // Misc5 possibly
                        LoadArtifactToSlot(reader, null, 0);   //Misc

                        // Backpack items
                        int amount = reader.ReadUInt16();
                        Console.WriteLine("Backpack item amount:" + amount);
                        for (int ss = 0; ss < amount; ++ss)
                        {
                            LoadArtifactToSlot(reader, null, 0);
                        }
                    }

                    bool hasCustomBio = reader.ReadBoolean();
                    if (hasCustomBio)
                    {
                        string biography = reader.ReadString();
                        Console.WriteLine("biography: " + biography);
                    }

                    int sex = reader.ReadByte();
                    Console.WriteLine("sex: " + sex);

                    // Spells
                    bool hasCustomSpells = reader.ReadBoolean();
                    if (hasCustomSpells)
                    {
                        HashSet<int> spells = new HashSet<int>();
                        reader.ReadBitMask(spells, 9, GameConstants.SPELLS_QUANTITY, false);
                        Console.WriteLine("Spells: " + JsonConvert.SerializeObject(spells));
                    }

                    bool hasCustomPrimSkills = reader.ReadBoolean();
                    if (hasCustomPrimSkills)
                    {
                        Console.WriteLine("Has Custom Primary Skills.");
                        for (int xx = 0; xx < GameConstants.PRIMARY_SKILLS; xx++)
                        {
                            int value = reader.ReadByte();
                            Console.WriteLine("Primary Skills: " + value);
                        }
                    }

                }
            }
        }

        private bool LoadArtifactToSlot(BinaryReader reader, object hero, int slot)
        {
            //// const int artmask = map->version == EMapFormat::ROE ? 0xff : 0xffff;
            const int artmask = 0xffff;
            int aid;

            aid = reader.ReadUInt16();

            bool isArt = (aid != artmask);
            if (isArt)
            {
                Console.WriteLine("loadArtifactToSlot: id={0}, slot={1}", aid, slot);
                return true;
            }

            return false;
        }

        private void ReadTerrain(BinaryReader reader)
        {
            int mapLevel = mapObject.Header.IsTwoLevel ? 2 : 1;
            uint mapHeight = mapObject.Header.Height;
            uint mapWidth = mapObject.Header.Width;
            
            for (int a = 0; a < mapLevel; a++)
            {
                for (int yy = 0; yy < mapHeight; yy++)
                {
                    for (int xx = 0; xx < mapWidth; xx++)
                    {
                        int terrainType = reader.ReadByte();
                        int terrainView = reader.ReadByte();
                        int riverType = reader.ReadByte();
                        int riverDir = reader.ReadByte();
                        int roadType = reader.ReadByte();
                        int roadDir = reader.ReadByte();
                        int extTileFlags = reader.ReadByte();

                        ////Console.WriteLine("Terrain at [{0}, {1}]: type={2} view={3} riverType={4} riverDir={5} roadType={6} roadDir={7}",
                        ////    xx, yy, terrainType, terrainView, riverType, riverDir, roadType, roadDir);
                    }
                }
            }
        }

        private void ReadObjectTemplates(BinaryReader reader)
        {
            uint templateCount = reader.ReadUInt32();
            Console.WriteLine("ReadObjectTemplates totally:" + templateCount);

            // Read custom defs
            for (int idd = 0; idd < templateCount; ++idd)
            {
                ObjectTemplate objectTemplate = new ObjectTemplate();

                objectTemplate.AnimationFile = reader.ReadString();
                Console.WriteLine("Object Animation File:" + objectTemplate.AnimationFile);

                int[] blockMask = new int[6];
                int[] visitMask = new int[6];

                foreach (int val in blockMask)
                {
                    int r = reader.ReadByte();
                    //Console.WriteLine("BlockMask: " + r);
                }

                foreach (int val in visitMask)
                {
                    int r = reader.ReadByte();
                    //Console.WriteLine("VisitMask: " + r);
                }

                reader.ReadUInt16();
                int terrMask = reader.ReadUInt16();

                objectTemplate.Type = (EObjectType)reader.ReadUInt32();
                objectTemplate.SubId = (int)reader.ReadUInt32();

                Console.WriteLine(string.Format("Object Type: {0} SubId: {1}", objectTemplate.Type, objectTemplate.SubId));

                // This type is not the template type, used in isOnVisitableFromTopList
                int type = reader.ReadByte();
                int printPriority = reader.ReadByte() * 100;

                reader.Skip(16);


            }
        }

        private void ReadObjects(BinaryReader reader)
        {
            CGObject resultObject = null;

            int objectCount = (int)reader.ReadUInt32();
            Console.WriteLine(string.Format("Totally {0} objects.", objectCount));
            
            for(int ww = 0; ww < objectCount; ww ++)
            {
                int objectId = this.mapObject.Objects.Count();

                MapPosition objectPosition = reader.ReadPosition();
                int objectTemplateIndex = (int)reader.ReadUInt32();

                ObjectTemplate objTemplate = mapObject.ObjectTemplates[objectTemplateIndex];

                reader.Skip(5);

                switch(objTemplate.Type)
                {
                    case EObjectType.EVENT:

                        H3Event h3event = new H3Event();
                        ReadMessageAndGuards(reader, h3event);

                        var gainedExp = reader.ReadUInt32();
                        var manaDiff = reader.ReadUInt32();
                        var moraleDiff = reader.ReadByte();
                        var luckDiff = reader.ReadByte();

                        ReadResouces(reader);

                        for(int x = 0; x < 4; x++)
                        {
                            var primSkill = (EPrimarySkill)reader.ReadByte();
                        }

                        int gainedAbilities = reader.ReadByte();
                        for (int i = 0; i < gainedAbilities; i++)
                        {
                            h3event.Abilities.Add((ESecondarySkill)reader.ReadByte());
                        }

                        int gainedArtifacts = reader.ReadByte();
                        for (int i = 0; i < gainedArtifacts; i++)
                        {
                            if (mapObject.Header.Version == EMapFormat.ROE)
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
                        break;

                    case EObjectType.HERO:
                    case EObjectType.RANDOM_HERO:
                    case EObjectType.PRISON:

                        resultObject = ReadHero(reader, objectId, objectPosition);
                        break;

                    case EObjectType.MONSTER:  //Monster
                    case EObjectType.RANDOM_MONSTER:
                    case EObjectType.RANDOM_MONSTER_L1:
                    case EObjectType.RANDOM_MONSTER_L2:
                    case EObjectType.RANDOM_MONSTER_L3:
                    case EObjectType.RANDOM_MONSTER_L4:
                    case EObjectType.RANDOM_MONSTER_L5:
                    case EObjectType.RANDOM_MONSTER_L6:
                    case EObjectType.RANDOM_MONSTER_L7:

                        // Create Creature
                        CGCreature creature = new CGCreature();

                        if (mapObject.Header.Version > EMapFormat.ROE)
                        {
                            creature.Id = (int)reader.ReadUInt32();
                            // Quest Identifier?
                        }



                        break;

                    case EObjectType.OCEAN_BOTTLE:
                    case EObjectType.SIGN:
                        {

                            break;
                        }
                    case EObjectType.SEER_HUT:
                        {
                            
                            break;
                        }
                    case EObjectType.WITCH_HUT:
                        {
                            
                            break;
                        }
                    case EObjectType.SCHOLAR:
                        {
                            
                            break;
                        }
                    case EObjectType.GARRISON:
                    case EObjectType.GARRISON2:
                        {
                            
                            break;
                        }
                    case EObjectType.ARTIFACT:
                    case EObjectType.RANDOM_ART:
                    case EObjectType.RANDOM_TREASURE_ART:
                    case EObjectType.RANDOM_MINOR_ART:
                    case EObjectType.RANDOM_MAJOR_ART:
                    case EObjectType.RANDOM_RELIC_ART:
                    case EObjectType.SPELL_SCROLL:
                        {
                            
                            break;
                        }

                    default:
                        break;
                }
            }
        }

        private CGObject ReadHero(BinaryReader reader, int objectId, MapPosition objectPosition)
        {
            HeroInstance hero = new HeroInstance();




            return hero;
        }
        
        private void ReadMessageAndGuards(BinaryReader reader, H3Event eventObject)
        {

        }

        private void ReadResouces(BinaryReader reader)
        {
            for (int x = 0; x < 7; ++x)
            {
                var num = reader.ReadUInt32();
            }
        }

        private object ReadCreatureSet(BinaryReader reader, int numberToRead)
        {
            bool isHighVersion = mapObject.Header.Version > EMapFormat.ROE;
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
}
