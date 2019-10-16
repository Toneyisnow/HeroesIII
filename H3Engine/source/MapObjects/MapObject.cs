using H3Engine.Components.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.Components.MapObjects
{
    public class MapObject
    {
        public MapObject()
        {

        }

        public int Id
        {
            get;set;
        }

        public MapPosition Position
        {
            get; set;
        }

        public EObjectType ObjectType
        {
            get; set;
        }

        public ObjectTemplate Template
        {
            get; set;
        }

        public EPlayerColor CurrentOwner
        {
            get; set;
        }

        public string InstanceName
        {
            get;set;
        }

        public string TypeName
        {
            get; set;
        }

        public string SubTypeName
        {
            get; set;
        }

        public MapPosition GetSightCenter()
        {
            return null;
        }







    }
}
