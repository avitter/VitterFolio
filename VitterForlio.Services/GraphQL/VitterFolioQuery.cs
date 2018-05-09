
using GraphQL.Types;
using VitterFolio.Api.GraphQL.Resolvers;
using System;
using System.Linq;

namespace VitterFolio.Api.GraphQL
{
    public class VitterFolioQuery : ObjectGraphType
    {
        public VitterFolioQuery(IServiceProvider serviceProvider)
        {
            // Get all types that implement IResolver
            var type = typeof(IResolve);
            var resolversTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));

            // Add each resolver to GraphQL root
            foreach (var resolverType in resolversTypes)
            {            
                var resolver = serviceProvider.GetService(resolverType) as IResolve;
                if(resolver != null)
                {
                    resolver.Resolve(this);
                }
            }
        }
    }
}


 