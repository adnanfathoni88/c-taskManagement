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
            string sql = @"INSERT INTO Projects (nama, deskripsi, start_date, end_date) VALUES (@nama, @deskripsi, @start_date, @end_date);";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@nama", project.Nama);
                cmd.Parameters.AddWithValue("@deskripsi", project.Deskripsi);
                cmd.Parameters.AddWithValue("@start_date", project.StartDate);
                cmd.Parameters.AddWithValue("@end_date", project.EndDate);
                return cmd.ExecuteNonQuery();
            }
        }

        public int Update(Project project)
        {
            string sql = @"UPDATE Projects SET nama = @nama, deskripsi = @deskripsi, start_date = @start_date, end_date = @end_date WHERE id = @id;";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@nama", project.Nama);
                cmd.Parameters.AddWithValue("@deskripsi", project.Deskripsi);
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

        public List<Project> ReadAll()
        {
            List<Project> list = new List<Project>();
            string sql = @"SELECT id, nama, deskripsi, status, start_date, end_date FROM Projects ORDER BY nama;";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
            {
                try
                {
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
                    return list;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }

        }
    }
}