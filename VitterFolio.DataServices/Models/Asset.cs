using System;
using System.Collections.Generic;

namespace VitterFolio.DataServices.Models
{
    public partial class Asset
    {
        public int AssetId { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
    }
}
