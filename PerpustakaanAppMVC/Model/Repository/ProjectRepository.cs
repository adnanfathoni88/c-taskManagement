using PerpustakaanAppMVC.Model.Context;
using PerpustakaanAppMVC.Model.Entity;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PerpustakaanAppMVC.Model.Repository
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
            string sql = @"UPDATE Projects SET nama = @nama, deskripsi = @deskripsi, status = @status, start_date = @start_date, end_date = @end_date WHERE id = @id;";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@nama", project.Nama);
                cmd.Parameters.AddWithValue("@deskripsi", project.Deskripsi);
                cmd.Parameters.AddWithValue("@status", project.Status);
                cmd.Parameters.AddWithValue("@start_date", project.StartDate);
                cmd.Parameters.AddWithValue("@end_date", project.EndDate);
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
                    sql = "SELECT * FROM Projects WHERE created_by = @id";
                    break;
                case "developer":
                    sql = "SELECT p.* FROM Projects p JOIN Tasks t ON p.id = t.project_id WHERE t.assigned_to = @id";
                    break;
                default:
                    sql = "SELECT * FROM Projects";
                    break;
            }

            sql += " ORDER BY nama;";

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

                        list.Add(project);
                    }
                }
            }

            return list;
        }

        public int GetTotalProjects(string userId = null)
        {
            int total = 0;
            string sql = "SELECT COUNT(*) FROM Projects";
            if (!string.IsNullOrEmpty(userId))
            {
                sql += " WHERE created_by = @userId";
            }
            using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                }
                total = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return total;
        }
    }
}