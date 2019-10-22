using H3Engine.MapObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.Mapping
{
    public class Rumor
    {
        public string Name
        {
            get; set;
        }

        public string Text
        {
            get; set;
        }
    }

    public class DisposedHero
    {
        public uint HeroId
        {
            get; set;
        }

        public UInt16 Portrait
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public byte Players
        {
            get; set;
        }

    }

    public class H3Map
    {
        
        public H3Map()
        {
            this.Header = new MapHeader();
            

        }

        public MapHeader Header
        {
            get; private set;
        }

        public List<Rumor> Rumors
        {
            get; set;
        }

        public List<HeroInstance> PredefinedHeroes
        {
            get; set;
        }

        public List<DisposedHero> DisposedHeroes
        {
            get; set;
        }

        public List<CGObject> Objects
        {
            get; set;
        }

        public List<ObjectTemplate> ObjectTemplates
        {
            get; set;
        }

        public List<TownInstance> Towns
        {
            get; set;
        }

        public List<CQuest> Quests
        {
            get; set;
        }

        public List<MapEvent> Events
        {
            get; set;
        }

        public CGGrail Grail
        {
            get; set;
        }
    }
}
