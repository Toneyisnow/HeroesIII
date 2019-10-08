using H3Engine.Components.Core;
using H3Engine.Components.FileSystem;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.Components.Mapping
{
    public class MapLoaderH3M
    {
        private H3Map mapObject = null;

        private string h3mFileFullPath = null;

        public MapLoaderH3M(string fileFullPath)
        {
            this.h3mFileFullPath = fileFullPath;
        }

        public H3Map LoadMap()
        {
            mapObject = new H3Map();
            
            using (BinaryFileReader reader = new BinaryFileReader(h3mFileFullPath))
            {

                ReadHeader(reader);

                ReadDisposedHeroes(reader);

                ReadAllowedArtifacts(reader);

                ReadAllowedSpellsAbilities(reader);

                ReadRumors(reader);

                ReadPredefinedHeroes(reader);

                ReadTerrain(reader);

                ReadObjectTemplates(reader);



            }

            return mapObject;
        }
        
        private void ReadHeader(BinaryFileReader reader)
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


            mapObject.Header.AreAnyPlayers = reader.ReadBool();
            Console.WriteLine("AreAnyPlayers:" + mapObject.Header.AreAnyPlayers);

            mapObject.Header.Height = reader.ReadUInt32();
            mapObject.Header.Width = mapObject.Header.Height;
            Console.WriteLine("Map Height and Width:" + mapObject.Header.Height);

            mapObject.Header.IsTwoLevel = reader.ReadBool();
            Console.WriteLine("twoLevel:" + mapObject.Header.IsTwoLevel);


            mapObject.Header.Name = reader.ReadString();
            Console.WriteLine("Name:" + mapObject.Header.Name);

            mapObject.Header.Description = reader.ReadString();
            Console.WriteLine("Description:" + mapObject.Header.Description);

            mapObject.Header.Difficulty = reader.ReadUInt8();
            Console.WriteLine("Difficulty:" + mapObject.Header.Difficulty);

            int heroLevelLimit = reader.ReadUInt8();
            Console.WriteLine("HeroLevelLimit:" + heroLevelLimit);


            ReadPlayerInfo(reader);

            ReadVictoryLossConditions(reader);

            ReadTeamInfo(reader);

            ReadAllowedHeroes(reader);
        }


        private void ReadPlayerInfo(BinaryFileReader reader)
        {
            for (int i = 0; i < GameConstants.PLAYER_LIMIT_T; i++)
            {
                PlayerInfo playerInfo = new PlayerInfo();

                Console.WriteLine("Reading Player [" + i.ToString() + "]");

                playerInfo.CanHumanPlay = reader.ReadBool();
                playerInfo.CanComputerPlay = reader.ReadBool();
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

                playerInfo.AiTactic = (EAiTactic)reader.ReadUInt8();
                Console.WriteLine("aiTactic:" + playerInfo.AiTactic);

                if (mapObject.Header.Version == EMapFormat.SOD || mapObject.Header.Version == EMapFormat.WOG)
                {
                    playerInfo.P7 = reader.ReadUInt8();
                }
                else
                {
                    playerInfo.P7 = -1;
                }

                Console.WriteLine("p7:" + playerInfo.P7);

                // Reading the Factions for Player
                playerInfo.AllowedFactions = new List<int>();
                int allowedFactionsMask = reader.ReadUInt8();
                Console.WriteLine("allowedFactionsMask:" + allowedFactionsMask);

                int totalFactionCount = GameConstants.F_NUMBER;
                if (mapObject.Header.Version != EMapFormat.ROE)
                    allowedFactionsMask += reader.ReadUInt8() << 8;
                else
                    totalFactionCount--; //exclude conflux for ROE
                
                for (int fact = 0; fact < totalFactionCount; ++fact)
                {
                    if ((allowedFactionsMask & (1 << fact)) > 0)
                    {
                        playerInfo.AllowedFactions.Add(fact);
                    }
                }

                playerInfo.IsFactionRandom = reader.ReadBool();
                playerInfo.HasMainTown = reader.ReadBool();
                Console.WriteLine("isFactionRandom:" + playerInfo.IsFactionRandom);
                Console.WriteLine("hasMainTown:" + playerInfo.HasMainTown);

                if (playerInfo.HasMainTown)
                {
                    /// Added in new version, not tested yet
                    if (mapObject.Header.Version != EMapFormat.ROE)
                    {
                        playerInfo.GenerateHeroAtMainTown = reader.ReadBool();
                        playerInfo.GenerateHero = reader.ReadBool();
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

                playerInfo.HasRandomHero = reader.ReadBool();
                Console.WriteLine("hasRandomHero:" + playerInfo.HasRandomHero);

                playerInfo.MainCustomHeroId = reader.ReadUInt8();
                Console.WriteLine("mainCustomHeroId:" + playerInfo.MainCustomHeroId);

                if (playerInfo.MainCustomHeroId != 0xff)
                {
                    playerInfo.MainCustomHeroPortrait = reader.ReadUInt8();
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
                    playerInfo.PowerPlaceHolders = reader.ReadUInt8();
                    int heroCount = reader.ReadUInt8();
                    reader.Skip(3);

                    playerInfo.HeroIds = new List<H3HeroId>();
                    for (int pp = 0; pp < heroCount; ++pp)
                    {
                        H3HeroId heroId = new H3HeroId();
                        heroId.Id = reader.ReadUInt8();
                        heroId.Name = reader.ReadString();
                        playerInfo.HeroIds.Add(heroId);
                    }
                }
            }
        }
        
        private void ReadVictoryLossConditions(BinaryFileReader reader)
        {
            //// mapObject.Header.TrigggeredEvents = new List<TrigggeredEvent>();

            var vicCondition = (EVictoryConditionType)reader.ReadUInt8();

            if (vicCondition == EVictoryConditionType.WINSTANDARD)
            {
                // create normal condition
                
            }
            else
            {
                bool allowNormalVictory = reader.ReadBool();
                bool appliesToAI = reader.ReadBool();

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
                            int objectType = reader.ReadUInt8();
                            break;
                        }
                    case EVictoryConditionType.GATHERTROOP:
                        {
                            int objectType = reader.ReadUInt8();
                            uint value = reader.ReadUInt32();
                            break;
                        }
                    case EVictoryConditionType.GATHERRESOURCE:
                        {
                            int objectType = reader.ReadUInt8();
                            uint value = reader.ReadUInt32();
                            break;
                        }
                    case EVictoryConditionType.BUILDCITY:
                        {
                            var pos = reader.ReadPosition();
                            int objectType = reader.ReadUInt8();
                            int objectType2 = reader.ReadUInt8();

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


            ELossConditionType loseCondition = (ELossConditionType)reader.ReadUInt8();
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


        private void ReadTeamInfo(BinaryFileReader reader)
        {
            int howManyTeams = reader.ReadUInt8();
            Console.WriteLine("How Many Teams: " + howManyTeams);
            if (howManyTeams > 0)
            {
                for (int i = 0; i < GameConstants.PLAYER_LIMIT_T; i++)
                {
                    int team = reader.ReadUInt8();
                }
            }
            else
            {
                for (int i = 0; i < GameConstants.PLAYER_LIMIT_T; i++)
                {

                }
            }
        }


        private void ReadAllowedHeroes(BinaryFileReader reader)
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
                //			placeholdedHeroes.push_back(reader.readUInt8());
                //		}
            }

        }

        private void ReadDisposedHeroes(BinaryFileReader reader)
        {
            if (mapObject.Header.Version >= EMapFormat.SOD)
            {
                int disp = reader.ReadUInt8();
                Console.WriteLine("ReadDisposedHeroes: Total=" + disp);
                for (int g = 0; g < disp; ++g)
                {
                    int heroId = reader.ReadUInt8();
                    int portrait = reader.ReadUInt8();
                    string name = reader.ReadString();
                    int players = reader.ReadUInt8();
                    Console.WriteLine(string.Format("ReadDisposedHeroes: id={0} portrait={1} name={2} players={3}", heroId, portrait, name, players));

                }
            }

            //omitting NULLS
            reader.Skip(31);
        }

        private void ReadAllowedArtifacts(BinaryFileReader reader)
        {
            if (mapObject.Header.Version != EMapFormat.ROE)
            {
                int bytes = (mapObject.Header.Version == EMapFormat.AB ? 17 : 18);

                HashSet<int> allowedList = new HashSet<int>();
                reader.ReadBitMask(allowedList, bytes, GameConstants.ARTIFACTS_QUANTITY);
            }
        }

        private void ReadAllowedSpellsAbilities(BinaryFileReader reader)
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

        private void ReadRumors(BinaryFileReader reader)
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

        private void ReadPredefinedHeroes(BinaryFileReader reader)
        {
            if (mapObject.Header.Version == EMapFormat.WOG || mapObject.Header.Version == EMapFormat.SOD)
            {
                for (int z = 0; z < GameConstants.HEROES_QUANTITY; z++)
                {
                    Console.WriteLine(string.Format("===Reading Predefined Hero [{0}]", z));

                    int custom = reader.ReadUInt8();
                    if (custom == 0)
                    {
                        Console.WriteLine("is not custom.");
                        continue;
                    }

                    // Create Hero
                    bool hasExp = reader.ReadBool();
                    if (hasExp)
                    {
                        uint exp = reader.ReadUInt32();
                        Console.WriteLine("Has exp:" + exp);
                    }

                    bool hasSecondSkills = reader.ReadBool();
                    if (hasSecondSkills)
                    {
                        uint howMany = reader.ReadUInt32();
                        Console.WriteLine("Has Second Skills count=" + howMany);

                        for (int yy = 0; yy < howMany; ++yy)
                        {
                            int first = reader.ReadUInt8();
                            int second = reader.ReadUInt8();
                            Console.WriteLine(string.Format("Skill First: {0} Second: {1}", first, second));
                        }
                    }

                    // Set Artifacts
                    bool artSet = reader.ReadBool();
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

                    bool hasCustomBio = reader.ReadBool();
                    if (hasCustomBio)
                    {
                        string biography = reader.ReadString();
                        Console.WriteLine("biography: " + biography);
                    }

                    int sex = reader.ReadUInt8();
                    Console.WriteLine("sex: " + sex);

                    // Spells
                    bool hasCustomSpells = reader.ReadBool();
                    if (hasCustomSpells)
                    {
                        HashSet<int> spells = new HashSet<int>();
                        reader.ReadBitMask(spells, 9, GameConstants.SPELLS_QUANTITY, false);
                        Console.WriteLine("Spells: " + JsonConvert.SerializeObject(spells));
                    }

                    bool hasCustomPrimSkills = reader.ReadBool();
                    if (hasCustomPrimSkills)
                    {
                        Console.WriteLine("Has Custom Primary Skills.");
                        for (int xx = 0; xx < GameConstants.PRIMARY_SKILLS; xx++)
                        {
                            int value = reader.ReadUInt8();
                            Console.WriteLine("Primary Skills: " + value);
                        }
                    }

                }
            }
        }

        private bool LoadArtifactToSlot(BinaryFileReader reader, object hero, int slot)
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

        private void ReadTerrain(BinaryFileReader reader)
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
                        int terrainType = reader.ReadUInt8();
                        int terrainView = reader.ReadUInt8();
                        int riverType = reader.ReadUInt8();
                        int riverDir = reader.ReadUInt8();
                        int roadType = reader.ReadUInt8();
                        int roadDir = reader.ReadUInt8();
                        int extTileFlags = reader.ReadUInt8();

                        ////Console.WriteLine("Terrain at [{0}, {1}]: type={2} view={3} riverType={4} riverDir={5} roadType={6} roadDir={7}",
                        ////    xx, yy, terrainType, terrainView, riverType, riverDir, roadType, roadDir);
                    }
                }
            }
        }

        private void ReadObjectTemplates(BinaryFileReader reader)
        {
            uint defAmount = reader.ReadUInt32();
            Console.WriteLine("ReadObjectTemplates totally:" + defAmount);

            // Read custom defs
            for (int idd = 0; idd < defAmount; ++idd)
            {
                string animationFile = reader.ReadString();
                Console.WriteLine("Object Animation File:" + animationFile);

                int[] blockMask = new int[6];
                int[] visitMask = new int[6];

                foreach (int val in blockMask)
                {
                    int r = reader.ReadUInt8();
                    //Console.WriteLine("BlockMask: " + r);
                }

                foreach (int val in visitMask)
                {
                    int r = reader.ReadUInt8();
                    //Console.WriteLine("VisitMask: " + r);
                }

                reader.ReadUInt16();
                int terrMask = reader.ReadUInt16();

                uint objectId = reader.ReadUInt32();
                uint objectSubId = reader.ReadUInt32();
                int type = reader.ReadUInt8();
                int printPriority = reader.ReadUInt8() * 100;

                reader.Skip(16);


            }
        }

        
    }
}
