using H3Engine.Common;
using H3Engine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.MapObjects
{
    public class CGObject
    {
        public CGObject()
        {

        }

        public uint Identifier
        {
            get;set;
        }

        public int SubId
        {
            get; set;
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

        public void SetOwner(EPlayerColor color)
        {
            this.CurrentOwner = color;
        }





    }
}
