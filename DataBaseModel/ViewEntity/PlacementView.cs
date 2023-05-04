using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModel.ViewEntity
{
    public class PlacementView
    {
        public int IdP { get; set; }

        public int Floor { get; set; }

        public string Square { get; set; } = null!;

        public int Room { get; set; }

        public string Area { get; set; } = null!;

        public string Street { get; set; } = null!;

        public int Number { get; set; }

        public int IdType { get; set; }

        public int IdR { get; set; }

        public int Size { get; set; }

        public string Description { get; set; } = null!;

    }
}
