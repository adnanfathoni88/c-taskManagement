using PerpustakaanAppMVC.Model.Entity;
using PerpustakaanAppMVC.Model.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PerpustakaanAppMVC.Controller
{
    public class ProjectController
    {

        // validasi
        private void ProjectValidation(Model.Entity.Project project)
        {
            if (string.IsNullOrEmpty(project.Nama))
            {
                throw new ArgumentException("Nama is required");
            }
            if (string.IsNullOrEmpty(project.Deskripsi))
            {
                throw new ArgumentException("Deskripsi is required");
            }
        }

        public int Create(Project project)
        {
            ProjectValidation(project); // validasi 
            using (var context = new Model.Context.DbContext())
            {
                var _repo = new ProjectRepository(context);
                return _repo.Create(project);
            }
        }

        public int Update(Project project) { 
            ProjectValidation(project); // validasi 
            using (var context = new Model.Context.DbContext())
            {
                var _repo = new ProjectRepository(context);
                return _repo.Update(project);
            }
        }

        public int Delete(int id)
        {
            using (var context = new Model.Context.DbContext())
            {
                var _repo = new ProjectRepository(context);
                return _repo.Delete(id);
            }
        }

        public List<Project> GetAllProjects(string id = null)
        {

            MessageBox.Show("controller" + id);

            using (var context = new Model.Context.DbContext())
            {
                var _repo = new ProjectRepository(context);
                return _repo.ReadAll(id);
            }
        }

        // get total project 
        public int GetTotalProjects(string userId = null)
        {
            using (var context = new Model.Context.DbContext())
            {
                var _repo = new ProjectRepository(context);
                return _repo.GetTotalProjects(userId);
            }
        }
    }
}
