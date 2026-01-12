using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerpustakaanAppMVC.Model.Entity
{
    public class Project
    {
        public int Id { get; set; }
        public string Nama { get; set; }
        public string Deskripsi { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
