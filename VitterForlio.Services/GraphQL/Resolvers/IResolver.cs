using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitterFolio.Api.GraphQL.Resolvers
{
    public interface IResolve
    {
        void Resolve(VitterFolioQuery query);
    }
}
