using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFDedWeb.Pages
{
    public class IndexModel:PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly PeopleContext _db;
        public IndexModel(ILogger<IndexModel> logger, PeopleContext db)
        {
            _logger = logger;
            _db = db;
        }

        public void OnGet()
        {
            LoadSampleData();

            var people = _db.People
                .Include(a => a.Addresses)
                .Include(e => e.EmailAddresses)
                //.ToList()
                //.Where(x => ApprovedAge(x.Age));
                .Where(x => x.Age >= 28 && x.Age <= 35)
                .ToList();
        }

        private bool ApprovedAge(int age)
        {
            return (age >= 28 && age <= 35);              
        }
        public void LoadSampleData()
        {
            if (_db.People.Count() == 0)
            {
                string file = System.IO.File.ReadAllText("generated.json");
                var people = JsonSerializer.Deserialize<List<Person>>(file);
                _db.AddRange(people);
                _db.SaveChanges();
            }
        }
    }
}
