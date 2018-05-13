using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitterFolio.Api.GraphQL.Types;
using VitterFolio.DataServices;

namespace VitterFolio.Api.GraphQL.Resolvers
{
    public class PortfolioResolver : IResolve
    {
        private AssetDB _db;

        public PortfolioResolver(AssetDB db)
        {
            _db = db;
        }

        public void Resolve(VitterFolioQuery query)
        {
            query.Field<ListGraphType<PortfolioAssetType>>(
                "portfolioAssets",
                resolve: context => _db.GetPortfolioAssets().Data);
        }
    }
}
