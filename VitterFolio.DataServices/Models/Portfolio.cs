using System;
using System.Collections.Generic;

namespace VitterFolio.DataServices.Models
{
    public partial class PortfolioAsset
    {
        public double units { get; set; }
        public Asset asset { get; set; }
    }
}
