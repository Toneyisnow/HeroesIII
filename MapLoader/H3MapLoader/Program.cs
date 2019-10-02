using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3MapLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();


            Console.ReadKey();
        }


        static void Test()
        {
            string pathSource = @"D:\Toney\Personal\Git\toneyisnow\HeroesIII\MapLoader\maps\HoMM3 Map Pack by HoMMdb\Shadow of Death & HoMM3 Complete\2 FRIENDS\2 FRIENDS.h3m";
            using (FileStream fstream = new FileStream(pathSource, FileMode.Open, FileAccess.Read))
            {
                ReadHeader(fstream);
                

            }
        }

        static void ReadHeader(FileStream fileStream)
        {
            // Check map for validity
            // Note: disabled, causes decompression of the entire file ( = SLOW)
            //if(inputStream->getSize() < 50)
            //{
            //	throw std::runtime_error("Corrupted map file.");
            //}

            // Map version
            UInt32 byte1 = ReadUInt32(fileStream);
            Console.WriteLine("Map version:" + byte1);

            bool areAnyPlayers = ReadBool(fileStream);
            Console.WriteLine("AreAnyPlayers:" + areAnyPlayers);

            UInt32 height = ReadUInt32(fileStream);
            Console.WriteLine("Map Height and Width:" + height);

            bool twoLevel = ReadBool(fileStream);
            Console.WriteLine("twoLevel:" + twoLevel);

            string mapName = ReadString(fileStream);
            Console.WriteLine("Name:" + mapName);

            string description = ReadString(fileStream);
            Console.WriteLine("Description:" + description);

            int difficulty = ReadInt(fileStream);
            Console.WriteLine("Difficulty:" + difficulty);

            int heroLevelLimit = ReadInt(fileStream);
            Console.WriteLine("HeroLevelLimit:" + heroLevelLimit);


            ReadPlayerInfo(fileStream);


        }

        static void ReadPlayerInfo(FileStream fileStream)
        {
            int PLAYER_COUNT = 8;
            for(int i = 0; i < PLAYER_COUNT; i++)
            {
                Console.WriteLine("Reading Player [" + i.ToString() + "]");


                bool canHumanPlay = ReadBool(fileStream);
                bool canComputerPlay = ReadBool(fileStream);
                Console.WriteLine("canHumanPlay: " + canHumanPlay);
                Console.WriteLine("canComputerPlay: " + canComputerPlay);

                if (!canHumanPlay && !canComputerPlay)
                {
                    ReadSkip(fileStream, 13);
                    continue;
                }

                int aiTactic = ReadInt(fileStream);
                Console.WriteLine("aiTactic:" + aiTactic);

                int p7 = ReadInt(fileStream);
                Console.WriteLine("p7:" + p7);

                int allowedFactions = ReadInt(fileStream);
                Console.WriteLine("allowedFactions:" + allowedFactions);

                allowedFactions += ReadInt(fileStream) * 256;

                bool isFactionRandom = ReadBool(fileStream);
                bool hasMainTown = ReadBool(fileStream);
                Console.WriteLine("isFactionRandom:" + isFactionRandom);
                Console.WriteLine("hasMainTown:" + hasMainTown);

                if (hasMainTown)
                {
                    var townPosition = ReadPosition(fileStream);
                    Console.WriteLine(string.Format("Main Town Position: {0}, {1}, {2}", townPosition.x, townPosition.y, townPosition.z));

                }

                bool hasRandomHero = ReadBool(fileStream);
                Console.WriteLine("hasRandomHero:" + hasRandomHero);

                int mainCustomHeroId = ReadInt(fileStream);
                Console.WriteLine("mainCustomHeroId:" + mainCustomHeroId);

                if (mainCustomHeroId != 0xff)
                {
                    int mainCustomHeroPortrait = ReadInt(fileStream);
                    string heroName = ReadString(fileStream);
                    Console.WriteLine("mainCustomHeroPortrait:" + mainCustomHeroPortrait);
                    Console.WriteLine("heroName:" + heroName);

                }

                var powerPlaceholders = ReadInt(fileStream);
                int heroCount = ReadInt(fileStream);
                Console.WriteLine("heroCount:" + heroCount);

                ReadSkip(fileStream, 3);
                for (int pp = 0; pp < heroCount; pp++)
                {
                    int heroId = ReadInt(fileStream);
                    string heroName = ReadString(fileStream);
                    Console.WriteLine("heroId:" + heroId);
                    Console.WriteLine("heroName:" + heroName);

                }

            }
        }

        static void ReadSkip(FileStream fileStream, int count)
        {
            for(int i = 0; i < count; i++)
            {
                ReadByte(fileStream);
            }
        }

        static MapPosition ReadPosition(FileStream fileStream)
        {
            MapPosition position = new MapPosition();

            position.x = ReadInt(fileStream);
            position.y = ReadInt(fileStream);
            position.z = ReadInt(fileStream);

            return position;
        }

        static UInt32 ReadUInt32(FileStream fileStream)
        {
            byte[] cache = new byte[4];
            fileStream.Read(cache, 0, 4);
            
            return BitConverter.ToUInt32(cache, 0);
        }

        static bool ReadBool(FileStream fileStream)
        {
            byte[] cache = new byte[1];
            fileStream.Read(cache, 0, 1);

            return BitConverter.ToBoolean(cache, 0);
        }

        static byte ReadByte(FileStream fileStream)
        {
            byte[] cache = new byte[1];
            fileStream.Read(cache, 0, 1);

            return cache[0];
        }

        static string ReadString(FileStream fileStream)
        {
            UInt32 length = ReadUInt32(fileStream);
            byte[] result = new byte[length];

            for(var i = 0; i < length; i++)
            {
                result[i] = ReadByte(fileStream);
            }

            return System.Text.Encoding.ASCII.GetString(result);
        }

        static int ReadInt(FileStream fileStream)
        {
            byte val = ReadByte(fileStream);
            return (int)val;
        }


    }

    class MapPosition
    {
        public int x;
        public int y;
        public int z;
    }


}
