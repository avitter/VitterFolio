 
using GraphQL;
using GraphQL.Types;

namespace VitterFolio.Api.GraphQL
{
    public class VitterFolioSchema : Schema
    {
        public VitterFolioSchema(IDependencyResolver resolver): base(resolver)
        {
            Query = resolver.Resolve<VitterFolioQuery>();
            //Mutation = resolver.Resolve<VitterFolioMutation>();
        }
    }
}


 