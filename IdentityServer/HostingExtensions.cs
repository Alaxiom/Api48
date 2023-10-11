using Serilog;

namespace IdentityServer;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // uncomment if you want to add a UI
        //builder.Services.AddRazorPages();

        builder.Services.AddIdentityServer(options =>
            {
                options.AccessTokenJwtType = "JWT";
                // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
                /* Static audience claim is required for .net Framework 4.x
                 * otherwise the token will not be accepted by the token validation package within
                 * those solutions
                 */
                options.EmitStaticAudienceClaim = true;                
            })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients);

        return builder.Build();
    }
    
    public static WebApplication ConfigurePipeline(this WebApplication app)
    { 
        app.UseSerilogRequestLogging();
    
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // uncomment if you want to add a UI
        //app.UseStaticFiles();
        //app.UseRouting();
            
        app.UseIdentityServer();

        // uncomment if you want to add a UI
        //app.UseAuthorization();
        //app.MapRazorPages().RequireAuthorization();

        return app;
    }
}
