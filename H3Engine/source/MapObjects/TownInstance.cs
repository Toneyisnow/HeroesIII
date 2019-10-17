using H3Engine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.MapObjects
{
    public class TownInstance : CGDwelling
    {
        public TownInstance()
        {
            this.Buildings = new HashSet<EBuildingId>();
            this.ForbiddenBuildings = new HashSet<EBuildingId>();
        }


        public string TownName
        {
            get; set;
        }

        public HashSet<EBuildingId> Buildings
        {
            get; private set;
        }

        public HashSet<EBuildingId> ForbiddenBuildings
        {
            get; private set;
        }


    }
}
