using System.Collections.Generic;
using PlatformService.Models;

namespace PlatformService.Data
{
   public interface IPlatformRepo
   {
       bool SaveChanges();

       IEnumerable<Platform> GetAllPlatforms();

       Platform GetPlatformById(int id);

       Platform DeletePlatform(int id);

       void CreatePlatform(Platform platform);
   } 
}