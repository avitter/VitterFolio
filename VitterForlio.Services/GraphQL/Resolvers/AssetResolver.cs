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
    public class AssetResolver : IResolve
    {
        private AssetDB _db;

        public AssetResolver(AssetDB db)
        {
            _db = db;
        }

        public void Resolve(VitterFolioQuery query)
        {
            query.Field<ListGraphType<AssetType>>(
                "assets",
                resolve: context => _db.GetAssets().Data);

            query.Field<AssetType>(
                "asset",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
                //resolve: context => db.GetAsset(context.GetArgument<int>("id")).Data);
                resolve: context =>
                {
                    var result = _db.GetAsset(context.GetArgument<int>("id"));

                    if (result.Status == Vitter.Core.ResponseStatus.Error || result.Status == Vitter.Core.ResponseStatus.Exception)
                    {
                        context.Errors.Add(new ExecutionError(result.Message));
                        return null;
                    }
                    else
                    {
                        return result.Data;
                    }
                });
        }
    }
}
