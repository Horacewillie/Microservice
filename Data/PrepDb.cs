using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;

namespace PlatformService.Data
{

    //Create a scope using the application builder class that creates
    //a mechanism for request pipeline.
    //Use the applicationbuilder instance to get or set the application service
    //container. The create a scope that will be used to resolve the incoming
    //service, by calling the ServiceProvider on the scope and getting the service.
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {   
                //Pass in AppDbContext using servicescope
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }

        }

        //SeedData needs AppDbContext
        private static void SeedData(AppDbContext context)
        {
            if (!context.Platforms.Any())
            {
                Console.WriteLine("--> Seeding Data...");
                context.Platforms.AddRange(GetStaticPlatforms());
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }

        //List of Platforms
        public static IEnumerable<Platform> GetStaticPlatforms()
        {
            return new List<Platform>()
            {
                new Platform(){Name="Dot Net", Publisher="Microsoft", Cost="Free"},
                new Platform(){Name="SQL Server Express", Publisher="Microsoft", Cost="Free"},
                new Platform(){Name="Kubernetes", Publisher="Cloud Native Computing Foundation", Cost="Free"}
            };
        }
    }
}