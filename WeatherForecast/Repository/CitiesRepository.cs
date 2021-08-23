using ClimaRmc.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClimaRmc.Repository
{
    public class CitiesRepository
    {
        public List<CityModel> City()
        {
            using (var db = new ApiContext())
            {
                return db.Cities.ToList();
            }
        }
      

        public CityModel CidadesMs(string cidade)
        {
            using (var db = new ApiContext())
            {
                return db.Cities.Where(x => x.CityNormalized == cidade).SingleOrDefault();
            }
        }    
        
        public void EditIdClimatempoCities(string cidade, int id)
        {
            using (var db = new ApiContext())
            {
                var cidadeDb = db.Cities.Where(x => x.CityNormalized == cidade).SingleOrDefault();
                cidadeDb.IdClimaTempo = id;

                db.SaveChanges();
            }
        }


        public List<CidadesMtModel> CidadesMt()
        {
            using (var db = new ApiContext())
            {
                return db.CidadesMt.ToList();
            }
        }


        public CidadesMtModel CidadesMt(string cidade)
        {
            using (var db = new ApiContext())
            {
                return db.CidadesMt.Where(x => x.CidadeApi == cidade).SingleOrDefault();
            }
        }

        public void EditaIdClimatempoCidadesMt(string cidade, int id)
        {
            using (var db = new ApiContext())
            {
                var cidadeDb = db.CidadesMt.Where(x => x.CidadeApi == cidade).SingleOrDefault();
                cidadeDb.IdClimaTempo = id;

                db.SaveChanges();
            }
        }
    }
}
