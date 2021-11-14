using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services
{
    public class AdministratorService : IAdministratorService
    {
        private readonly AppDBContext _context;

        public AdministratorService(AppDBContext context)
        {
            _context = context;
        }

        public About GetAboutUs() 
        {
            var result = _context.AboutUs.Where(x => x.Id == 1).SingleOrDefault();
            return result;
        }

        public SocMed GetContactUs()
        {
            var result = _context.Socials.Where(x => x.Id == 1).SingleOrDefault();
            return result;
        }

        public void CreateAboutUs(About about)
        {
            _context.AboutUs.Add(about);
            _context.SaveChanges();
        }

        public void CreateContactUs(SocMed socMed)
        {
            _context.Socials.Add(socMed);
            _context.SaveChanges();
        }

        public void UpdateAboutUs(About about)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateContactUs(SocMed socMed)
        {
            throw new System.NotImplementedException();
        }
    }
}
