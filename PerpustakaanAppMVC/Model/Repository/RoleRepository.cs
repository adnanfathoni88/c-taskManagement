using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Management;
using System.Windows.Forms;
using TWEEKLE.Model.Context;
using TWEEKLE.Model.Entity;


namespace TWEEKLE.Model.Repository
{
    public class RoleRepository
    {
        private SQLiteConnection _conn;

        public RoleRepository(DbContext context)
        {
            _conn = context.Conn;
        }


        public int Create(Role role)
        {
            string sql = @"INSERT INTO Roles (name) VALUES (@name);";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@name", role.Name);

                try
                {
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    var error = "Create Role Error: " + ex.Message;
                    System.Diagnostics.Debug.Print(error);
                    MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                }
            }
        }

        public List<Role> ReadAll()
        {
            List<Role> list = new List<Role>();
            string sql = @"SELECT id, name FROM Roles ORDER BY name;";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
            {
                try
                {
                    using (SQLiteDataReader dtr = cmd.ExecuteReader())
                    {
                        while (dtr.Read())
                        {
                            Role role = new Role();
                            role.Id = Convert.ToInt32(dtr["id"]);
                            role.Name = dtr["name"].ToString();
                            list.Add(role);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print("ReadAll Role Error: " + ex.Message);
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return list;
            }
        }

        public int Update(Role role)
        {
            int result = 0;
            string sql = @"UPDATE Roles SET name = @name WHERE id = @id;";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@name", role.Name);
                cmd.Parameters.AddWithValue("@id", role.Id);
                try
                {
                    result = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print("Update Role Error: " + ex.Message);
                }
                return result;
            }
        }

        public int Delete(int id)
        {
            int result = 0;
            string sql = @"DELETE FROM Roles WHERE id = @Id;";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                try
                {
                    result = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print("Delete Role Error: " + ex.Message);
                }
                return result;
            }
        }

        public bool IsRoleInUse(int roleId)
        {
            string sql = @"SELECT COUNT(*) FROM Users WHERE role_id = @roleId;";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@roleId", roleId);
                try
                {
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print("IsRoleInUse Error: " + ex.Message);
                    return false; // Assume not in use if there's an error
                }
            }
        }

        public Role GetByName(string roleName)
        {
            string sql = @"SELECT id, name FROM Roles WHERE name = @name COLLATE NOCASE;";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@name", roleName);
                try
                {
                    using (SQLiteDataReader dtr = cmd.ExecuteReader())
                    {
                        if (dtr.Read())
                        {
                            Role role = new Role();
                            role.Id = Convert.ToInt32(dtr["id"]);
                            role.Name = dtr["name"].ToString();
                            return role;
                        }
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print("GetByName Role Error: " + ex.Message);
                    return null;
                }
            }
        }
    }
}
