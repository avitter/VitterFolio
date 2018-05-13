using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitterFolio.DataServices.Models;

namespace VitterFolio.Api.GraphQL.Types
{
    public class PortfolioAssetType : ObjectGraphType<PortfolioAsset>, IGraphType
    {
        public PortfolioAssetType()
        {
            Field(x => x.units);
            Field<AssetType>(
                "asset",
                resolve: context => context.Source.asset
                );
        }
    }
}
