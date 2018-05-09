using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VitterFolio.BusinessServices;
using VitterFolio.DataServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;
using GraphQL;
using GraphQL.Types;
using VitterFolio.Api.GraphQL;
using VitterFolio.Api.GraphQL.Types;
using VitterFolio.DataServices.Models;
using VitterFolio.Api.GraphQL.Resolvers;
using System.Reflection;

namespace VitterForlioServices
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Business Service Layer
            services.AddTransient<AssetManager, AssetManager>();
            // Data Layer
            services.AddTransient<AssetDB, AssetDB>();
            // Database
            var connection = Configuration.GetConnectionString("default");
            services.AddDbContext<VitterFolioContext>(
                options => {
                    options.UseSqlServer(connection);
                    }
            );

            // Token Authentication
            #region Token Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;

                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Configuration["Tokens:Issuer"],
                        ValidAudience = Configuration["Tokens:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                    };

                });
            #endregion

            // GraphQL
            #region GraphQL
            services.AddSingleton<VitterFolioQuery>();
            // services.AddSingleton<VitterFolioMutation>();
            // Register all GraphQL types through reflection
            services.Scan(
                x =>
                {
                    var entryAssembly = Assembly.GetEntryAssembly();
                    var referencedAssemblies = entryAssembly.GetReferencedAssemblies().Select(Assembly.Load);
                    var assemblies = new List<Assembly> { entryAssembly }.Concat(referencedAssemblies);

                    // Register GraphQL Resolvers - Implement IResolver
                    x.FromAssemblies(assemblies)
                        .AddClasses(classes => classes.AssignableTo(typeof(IResolve)))
                            .AsImplementedInterfaces()
                            .WithScopedLifetime();

                    // Register GraphQL Types - Implement IGraphType
                    x.FromAssemblies(assemblies)
                        .AddClasses(classes => classes.AssignableTo(typeof(GraphQL.Types.IGraphType)))
                        .AsSelf();
                });
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            var sp = services.BuildServiceProvider();
            services.AddSingleton<ISchema>(new VitterFolioSchema(new FuncDependencyResolver(type => sp.GetService(type))));
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // GraphiQL
            app.UseGraphiQl();

            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}");
            });
        }
    }
}
