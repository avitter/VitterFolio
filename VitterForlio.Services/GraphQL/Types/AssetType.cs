using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitterFolio.DataServices.Models;

namespace VitterFolio.Api.GraphQL.Types
{
    public class AssetType : ObjectGraphType<Asset>, IGraphType
    {
        public AssetType()
        {
            Field(x => x.AssetId);
            Field(x => x.Name);
            Field(x => x.Symbol);
        }
    }
}
