using Auth.Api.Configuration;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Auth.Api.InitialSeed;

public static class MigrationManager
{
    public static IHost MigrateDatabase(this WebApplication host)
    {
        using (var scope = host.Services.CreateScope())
        {
            scope.ServiceProvider
                .GetRequiredService<PersistedGrantDbContext>()
                .Database
                .Migrate();

            using (var context = scope.ServiceProvider
                       .GetRequiredService<ConfigurationDbContext>())
            {
                try
                {
                    context.Database.Migrate();

                    if (!context.Clients.Any())
                    {
                        foreach (var client in IdentityServerConfiguration.Clients)
                        {
                            context.Clients.Add(client.ToEntity());
                        }

                        context.SaveChanges();
                    }

                    if (!context.IdentityResources.Any())
                    {
                        foreach (var resource in IdentityServerConfiguration.IdentityResources)
                        {
                            context.IdentityResources.Add(resource.ToEntity());
                        }

                        context.SaveChanges();
                    }

                    if (!context.ApiScopes.Any())
                    {
                        foreach (var apiScope in IdentityServerConfiguration.ApiScopes)
                        {
                            context.ApiScopes.Add(apiScope.ToEntity());
                        }

                        context.SaveChanges();
                    }

                    if (!context.ApiResources.Any())
                    {
                        foreach (var apiResource in IdentityServerConfiguration.ApiResources)
                        {
                            context.ApiResources.Add(apiResource.ToEntity());
                        }

                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }
        }

        return host;
    }
}