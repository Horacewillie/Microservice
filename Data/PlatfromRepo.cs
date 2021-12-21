using System;
using System.Collections.Generic;
using System.Linq;
using PlatformService.Models;
using PlatformServie.Data;

namespace PlatformService.Data
{
    public class PlatfromRepo : IPlatformRepo
    {
        private readonly AppDbContext _context;

        public PlatfromRepo(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        void IPlatformRepo.CreatePlatform(Platform platform)
        {
           if(platform == null)
           {
               throw new ArgumentNullException(nameof(platform));
           }
           _context.Platforms.Add(platform);
        }

        IEnumerable<Platform> IPlatformRepo.GetAllPlatforms()
        {
            //Get Platforms from the platform table of the db.
           return _context.Platforms.ToList();
        }

        Platform IPlatformRepo.GetPlatformById(int id)
        {
           return _context.Platforms.FirstOrDefault(p => p.Id == id);
        }

        bool IPlatformRepo.SaveChanges()
        {
          return _context.SaveChanges() >= 0;
        }
    }
}