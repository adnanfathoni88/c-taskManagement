using TWEEKLE.Model.Context;
using TWEEKLE.Model.Entity;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TWEEKLE.Model.Repository
{
    public class ProjectRepository
    {
        private SQLiteConnection _conn;
        private Project project;
        public ProjectRepository(DbContext context)
        {
            _conn = context.Conn;
        }

        public int Create(Project project)
        {
            string sql = @"INSERT INTO Projects (nama, deskripsi, status ,start_date, end_date, created_by) VALUES (@nama, @deskripsi, @status, @start_date, @end_date, @created_by);";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@nama", project.Nama);
                cmd.Parameters.AddWithValue("@deskripsi", project.Deskripsi);
                cmd.Parameters.AddWithValue("@status", project.Status);
                cmd.Parameters.AddWithValue("@start_date", project.StartDate);
                cmd.Parameters.AddWithValue("@end_date", project.EndDate);
                cmd.Parameters.AddWithValue("@created_by", project.CreatedBy);
                return cmd.ExecuteNonQuery();
            }
        }

        public int Update(Project project)
        {
            string sql = @"UPDATE Projects SET nama = @nama, deskripsi = @deskripsi, status = @status, start_date = @start_date, end_date = @end_date, created_by = @created_by WHERE id = @id;";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@nama", project.Nama);
                cmd.Parameters.AddWithValue("@deskripsi", project.Deskripsi);
                cmd.Parameters.AddWithValue("@status", project.Status);
                cmd.Parameters.AddWithValue("@start_date", project.StartDate);
                cmd.Parameters.AddWithValue("@end_date", project.EndDate);
                cmd.Parameters.AddWithValue("@created_by", project.CreatedBy);
                cmd.Parameters.AddWithValue("@id", project.Id);
                return cmd.ExecuteNonQuery();
            }
        }

        public int Delete(int id)
        {
            string sql = @"DELETE FROM Projects WHERE id = @id;";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                return cmd.ExecuteNonQuery();
            }
        }

        public List<Project> ReadAll(string role, string id = null)
        {
            List<Project> list = new List<Project>();

            string sql = "";

            // jika admin
            switch (role.ToLower())
            {
                case "project manager":
                    sql = "SELECT p.*, u.name AS created_by_name FROM Projects p JOIN Users u ON p.created_by = u.id WHERE p.created_by = @id";
                    break;
                case "developer":
                    sql = "SELECT DISTINCT p.*, u.name AS created_by_name FROM Projects p JOIN Users u ON p.created_by = u.id JOIN Tasks t ON p.id = t.project_id WHERE t.assigned_to = @id";
                    break;
                default:
                    sql = "SELECT p.*, u.name AS created_by_name FROM Projects p JOIN Users u ON p.created_by = u.id";
                    break;
            }

            sql += " ORDER BY p.nama;";

            using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
            {
                if (role != "admin")
                {
                    cmd.Parameters.AddWithValue("@id", id);
                }

                using (SQLiteDataReader dtr = cmd.ExecuteReader())
                {
                    while (dtr.Read())
                    {
                        Project project = new Project();
                        project.Id = Convert.ToInt32(dtr["id"]);
                        project.Nama = dtr["nama"].ToString();
                        project.Deskripsi = dtr["deskripsi"].ToString();
                        project.Status = dtr["status"].ToString();
                        project.StartDate = Convert.ToDateTime(dtr["start_date"]);
                        project.EndDate = Convert.ToDateTime(dtr["end_date"]);
                        project.CreatedBy = Convert.ToInt32(dtr["created_by"]);
                        project.CreatedByName = dtr["created_by_name"].ToString();

                        list.Add(project);
                    }
                }
            }

            return list;
        }

        public int GetTotalProjects(int userId, string role)
        {
            int total = 0;
            string sql = string.Empty;

            switch (role)
            {
                case "Admin":
                    sql = @"SELECT COUNT(*) FROM projects";
                    break;

                case "Project Manager":
                    sql = @"SELECT COUNT(*) 
                    FROM projects 
                    WHERE created_by = @userId";
                    break;

                default: // Member / role lain
                    sql = @"SELECT COUNT(DISTINCT p.id)
                    FROM projects p
                    INNER JOIN tasks t ON t.project_id = p.id
                    WHERE t.assigned_to = @userId";
                    break;
            }

            using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
            {
                if (role != "Admin")
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                }

                total = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return total;
        }


        public Project GetByName(string projectName)
        {
            string sql = @"SELECT p.*, u.name AS created_by_name FROM Projects p JOIN Users u ON p.created_by = u.id WHERE p.nama = @name COLLATE NOCASE;";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@name", projectName);
                try
                {
                    using (SQLiteDataReader dtr = cmd.ExecuteReader())
                    {
                        if (dtr.Read())
                        {
                            Project project = new Project();
                            project.Id = Convert.ToInt32(dtr["id"]);
                            project.Nama = dtr["nama"].ToString();
                            project.Deskripsi = dtr["deskripsi"].ToString();
                            project.Status = dtr["status"].ToString();
                            project.StartDate = Convert.ToDateTime(dtr["start_date"]);
                            project.EndDate = Convert.ToDateTime(dtr["end_date"]);
                            project.CreatedBy = Convert.ToInt32(dtr["created_by"]);
                            project.CreatedByName = dtr["created_by_name"].ToString();
                            return project;
                        }
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print("GetByName Project Error: " + ex.Message);
                    return null;
                }
            }
        }

        public bool IsProjectInUse(int projectId)
        {
            string sql = @"SELECT COUNT(*) FROM Tasks WHERE project_id = @projectId;";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@projectId", projectId);
                try
                {
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print("IsProjectInUse Error: " + ex.Message);
                    return false; // Assume not in use if there's an error
                }
            }
        }

        public Project GetByNameExcludeId(string projectName, int excludeId)
        {
            string sql = @"SELECT p.*, u.name AS created_by_name FROM Projects p JOIN Users u ON p.created_by = u.id WHERE p.nama = @name COLLATE NOCASE AND p.id != @excludeId;";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@name", projectName);
                cmd.Parameters.AddWithValue("@excludeId", excludeId);
                try
                {
                    using (SQLiteDataReader dtr = cmd.ExecuteReader())
                    {
                        if (dtr.Read())
                        {
                            Project project = new Project();
                            project.Id = Convert.ToInt32(dtr["id"]);
                            project.Nama = dtr["nama"].ToString();
                            project.Deskripsi = dtr["deskripsi"].ToString();
                            project.Status = dtr["status"].ToString();
                            project.StartDate = Convert.ToDateTime(dtr["start_date"]);
                            project.EndDate = Convert.ToDateTime(dtr["end_date"]);
                            project.CreatedBy = Convert.ToInt32(dtr["created_by"]);
                            project.CreatedByName = dtr["created_by_name"].ToString();
                            return project;
                        }
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print("GetByNameExcludeId Project Error: " + ex.Message);
                    return null;
                }
            }
        }
    }
}