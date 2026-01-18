using PerpustakaanAppMVC.Model.Context;
using PerpustakaanAppMVC.Model.Entity;
using PerpustakaanAppMVC.Model.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerpustakaanAppMVC.Controller
{
    public class RoleController
    {
        private RoleRepository _repository;


        public RoleController() { }

        public int Create(Role role)
        {
            int result = 0;

            if (string.IsNullOrEmpty(role.Name))
            {
                throw new ArgumentException("Role name cannot be null or empty");
            }

            using (DbContext context = new DbContext())
            {
                _repository = new RoleRepository(context);

                try
                {

                    result = _repository.Create(role);
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while creating the role: " + ex.Message);
                }


                return result;
            }
        }

        public List<Role> ReadAll()
        {
            using (DbContext context = new DbContext())
            {
                _repository = new RoleRepository(context);
                return _repository.ReadAll();
            }
        }

        public int Update(Role role)
        {
            if (role.Id <= 0)
            {
                throw new ArgumentException("Invalid role ID");
            }

            using (var context = new DbContext())
            {
                _repository = new RoleRepository(context);
                int result = _repository.Update(role);
                if (result == 0)
                {
                    throw new Exception("Failed to update role");
                }
                return result;
            }
        }


        public int Delete(int id)
        {
            using (var context = new DbContext())
            {
                _repository = new RoleRepository(context);
                int result = _repository.Delete(id);
                if (result == 0)
                {
                    throw new Exception("Failed to delete role");
                }
                return result;
            }
        }

        public bool IsRoleInUse(int roleId)
        {
            using (var context = new DbContext())
            {
                _repository = new RoleRepository(context);
                return _repository.IsRoleInUse(roleId);
            }
        }

        public Role GetByName(string roleName)
        {
            using (var context = new DbContext())
            {
                _repository = new RoleRepository(context);
                var allRoles = _repository.ReadAll();
                return allRoles.FirstOrDefault(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}
