using H3MapLoader.Components.Mapping;
using H3MapLoader.Mapping;
using Newtonsoft.Json;
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
        static bool map_twoLevel = false;
        static UInt32 map_height = 0;


        static void Main(string[] args)
        {
            Test();


            Console.ReadKey();
        }


        static void Test()
        {
            //// string pathSource = @"D:\Toney\Personal\Git\toneyisnow\HeroesIII\MapLoader\maps\HoMM3 Map Pack by HoMMdb\Shadow of Death & HoMM3 Complete\2 FRIENDS\2 FRIENDS.h3m";
            //// string pathSource = @"D:\Toney\Personal\Git\toneyisnow\HeroesIII\MapLoader\maps\HoMM3 Map Pack by HoMMdb\Shadow of Death & HoMM3 Complete\Abilities\Abilities.h3m";
            //// string pathSource = @"D:\Toney\Personal\Git\toneyisnow\HeroesIII\MapLoader\maps\HoMM3 Map Pack by HoMMdb\Shadow of Death & HoMM3 Complete\Astrogonia\Astrogonia.h3m";
            //// string pathSource = @"D:\Toney\Personal\Git\toneyisnow\HeroesIII\MapLoader\maps\HoMM3 Map Pack by HoMMdb\Armageddon's Blade\Riverworld\Riverworld";
            string pathSource = @"D:\Toney\Personal\Git\toneyisnow\HeroesIII\MapLoader\maps\HoMM3 Map Pack by HoMMdb\Shadow of Death & HoMM3 Complete\Andrews Exploits\Andrews Exploits";


            MapLoaderH3M loader = new MapLoaderH3M(pathSource);

            HMMap map = loader.LoadMap();


        }
        


        /// /////////////////////////////////////////////////////////

        

    }


}
