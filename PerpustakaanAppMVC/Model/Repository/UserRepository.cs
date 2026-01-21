using TWEEKLE.Model.Context;
using TWEEKLE.Model.Entity;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWEEKLE.Model.Repository
{
    public class UserRepository
    {
        private SQLiteConnection _conn;

        public UserRepository(DbContext context)
        {
            _conn = context.Conn;
        }

        public int Create(User user)
        {
            string sql = @"INSERT INTO Users (name, email, password, status, role_id) VALUES (@name, @email, @password, @status, @role_id);";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@name", user.Name);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@password", user.Password);
                cmd.Parameters.AddWithValue("@status", user.Status);
                cmd.Parameters.AddWithValue("@role_id", user.RoleId);

                return cmd.ExecuteNonQuery();
            }
        }

        public List<User> ReadAll()
        {
            List<User> list = new List<User>();
            string sql = @"SELECT u.id, u.name, u.email, u.password, u.status, u.role_id, r.name AS role_name
                           FROM Users u
                           JOIN Roles r ON u.role_id = r.id
                           ORDER BY u.name;";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
            {
                try
                {
                    using (SQLiteDataReader dtr = cmd.ExecuteReader())
                    {
                        while (dtr.Read())
                        {
                            User user = new User();
                            user.Id = Convert.ToInt32(dtr["id"]);
                            user.Name = dtr["name"].ToString();
                            user.Email = dtr["email"].ToString();
                            user.Password = dtr["password"].ToString();
                            user.Status = Convert.ToInt32(dtr["status"]);
                            user.RoleId = Convert.ToInt32(dtr["role_id"]);
                            user.RoleName = dtr["role_name"].ToString();
                            list.Add(user);
                        }
                    }
                    return list;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print("ReadAll Users Error: " + ex.Message);
                    return new List<User>();
                }
            }
        }

        public int Update(User user)
        {
            string sql = @"UPDATE Users SET name = @name, email = @email, password = @password, status = @status, role_id = @role_id WHERE id = @id;";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@name", user.Name);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@password", user.Password);
                cmd.Parameters.AddWithValue("@status", user.Status);
                cmd.Parameters.AddWithValue("@role_id", user.RoleId);
                cmd.Parameters.AddWithValue("@id", user.Id);

                return cmd.ExecuteNonQuery();
            }
        }

        public User ReadById(int id)
        {
            User user = null;
            string sql = @"SELECT u.id, u.name, u.email, u.password, u.status, u.role_id, r.name AS role_name
                           FROM Users u
                           JOIN Roles r ON u.role_id = r.id
                           WHERE u.id = @id;";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                try
                {
                    using (SQLiteDataReader dtr = cmd.ExecuteReader())
                    {
                        if (dtr.Read())
                        {
                            user = new User();
                            user.Id = Convert.ToInt32(dtr["id"]);
                            user.Name = dtr["name"].ToString();
                            user.Email = dtr["email"].ToString();
                            user.Password = dtr["password"].ToString();
                            user.Status = Convert.ToInt32(dtr["status"]);
                            user.RoleId = Convert.ToInt32(dtr["role_id"]);
                            user.RoleName = dtr["role_name"].ToString();
                        }
                    }
                    return user;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print("ReadById User Error: " + ex.Message);
                    return null;
                }
            }
        }

        public int Delete(int id)
        {
            string sql = @"DELETE FROM Users WHERE id = @id;";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                return cmd.ExecuteNonQuery();
            }
        }

        public User GetByEmail(string email)
        {
            User user = null;
            string sql = @"SELECT u.id, u.name, u.email, u.password, u.status, u.role_id, r.name AS role_name
                           FROM Users u
                           JOIN Roles r ON u.role_id = r.id
                           WHERE u.email = @email;";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@email", email);
                try
                {
                    using (SQLiteDataReader dtr = cmd.ExecuteReader())
                    {
                        if (dtr.Read())
                        {
                            user = new User();
                            user.Id = Convert.ToInt32(dtr["id"]);
                            user.Name = dtr["name"].ToString();
                            user.Email = dtr["email"].ToString();
                            user.Password = dtr["password"].ToString();
                            user.Status = Convert.ToInt32(dtr["status"]);
                            user.RoleId = Convert.ToInt32(dtr["role_id"]);
                            user.RoleName = dtr["role_name"].ToString();
                        }
                    }
                    return user;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print("GetByEmail User Error: " + ex.Message);
                    return null;
                }
            }
        }
    }
}
