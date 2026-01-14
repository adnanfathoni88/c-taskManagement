using PerpustakaanAppMVC.Model.Context;
using PerpustakaanAppMVC.Model.Entity;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace PerpustakaanAppMVC.Model.Repository
{
    public class TaskRepository
    {
        private DbContext _context;

        public TaskRepository(DbContext context)
        {
            _context = context;
        }

        public List<TaskItem> GetAll()
        {
            var tasks = new List<TaskItem>();

            using (var cmd = _context.Conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT t.id, t.title, t.description, t.status, t.priority,
                           t.project_id, t.assigned_to, t.deadline,
                           p.nama AS project_name,
                           u.name AS assigned_to_name
                    FROM Tasks t
                    LEFT JOIN Projects p ON t.project_id = p.id
                    LEFT JOIN Users u ON t.assigned_to = u.id
                    ORDER BY t.id";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var task = new TaskItem
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Title = reader["title"].ToString(),
                            Description = reader["description"].ToString(),
                            Status = reader["status"].ToString(),
                            Priority = reader["priority"].ToString(),
                            ProjectId = reader["project_id"] != DBNull.Value ? Convert.ToInt32(reader["project_id"]) : (int?)null,
                            AssignedTo = reader["assigned_to"] != DBNull.Value ? Convert.ToInt32(reader["assigned_to"]) : (int?)null,
                            Deadline = reader["deadline"].ToString(),
                            ProjectName = reader["project_name"].ToString(),
                            AssignedToName = reader["assigned_to_name"].ToString()
                        };

                        tasks.Add(task);
                    }
                }
            }

            return tasks;
        }

        public TaskItem GetById(int id)
        {
            TaskItem task = null;

            using (var cmd = _context.Conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT t.id, t.title, t.description, t.status, t.priority,
                           t.project_id, t.assigned_to, t.deadline,
                           p.nama AS project_name,
                           u.name AS assigned_to_name
                    FROM Tasks t
                    LEFT JOIN Projects p ON t.project_id = p.id
                    LEFT JOIN Users u ON t.assigned_to = u.id
                    WHERE t.id = @id";

                cmd.Parameters.Add(new SQLiteParameter("@id", id));

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        task = new TaskItem
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Title = reader["title"].ToString(),
                            Description = reader["description"].ToString(),
                            Status = reader["status"].ToString(),
                            Priority = reader["priority"].ToString(),
                            ProjectId = reader["project_id"] != DBNull.Value ? Convert.ToInt32(reader["project_id"]) : (int?)null,
                            AssignedTo = reader["assigned_to"] != DBNull.Value ? Convert.ToInt32(reader["assigned_to"]) : (int?)null,
                            Deadline = reader["deadline"].ToString(),
                            ProjectName = reader["project_name"].ToString(),
                            AssignedToName = reader["assigned_to_name"].ToString()
                        };
                    }
                }
            }

            return task;
        }

        public int Insert(TaskItem task)
        {
            int result = 0;

            using (var cmd = _context.Conn.CreateCommand())
            {
                cmd.CommandText = @"
                    INSERT INTO Tasks (title, description, status, priority, project_id, assigned_to, deadline)
                    VALUES (@title, @description, @status, @priority, @project_id, @assigned_to, @deadline);
                    SELECT last_insert_rowid();";

                cmd.Parameters.Add(new SQLiteParameter("@title", task.Title));
                cmd.Parameters.Add(new SQLiteParameter("@description", task.Description ?? ""));
                cmd.Parameters.Add(new SQLiteParameter("@status", task.Status));
                cmd.Parameters.Add(new SQLiteParameter("@priority", task.Priority));
                cmd.Parameters.Add(new SQLiteParameter("@project_id", task.ProjectId.HasValue ? task.ProjectId.Value : (object)DBNull.Value));
                cmd.Parameters.Add(new SQLiteParameter("@assigned_to", task.AssignedTo.HasValue ? task.AssignedTo.Value : (object)DBNull.Value));
                cmd.Parameters.Add(new SQLiteParameter("@deadline", task.Deadline ?? ""));

                result = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return result;
        }

        public int Update(TaskItem task)
        {
            int result = 0;

            using (var cmd = _context.Conn.CreateCommand())
            {
                cmd.CommandText = @"
                    UPDATE Tasks
                    SET title = @title, description = @description, status = @status,
                        priority = @priority, project_id = @project_id, assigned_to = @assigned_to, deadline = @deadline
                    WHERE id = @id";

                cmd.Parameters.Add(new SQLiteParameter("@id", task.Id));
                cmd.Parameters.Add(new SQLiteParameter("@title", task.Title));
                cmd.Parameters.Add(new SQLiteParameter("@description", task.Description ?? ""));
                cmd.Parameters.Add(new SQLiteParameter("@status", task.Status));
                cmd.Parameters.Add(new SQLiteParameter("@priority", task.Priority));
                cmd.Parameters.Add(new SQLiteParameter("@project_id", task.ProjectId.HasValue ? task.ProjectId.Value : (object)DBNull.Value));
                cmd.Parameters.Add(new SQLiteParameter("@assigned_to", task.AssignedTo.HasValue ? task.AssignedTo.Value : (object)DBNull.Value));
                cmd.Parameters.Add(new SQLiteParameter("@deadline", task.Deadline ?? ""));

                result = cmd.ExecuteNonQuery();
            }

            return result;
        }

        public int Delete(int id)
        {
            int result = 0;

            using (var cmd = _context.Conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM Tasks WHERE id = @id";
                cmd.Parameters.Add(new SQLiteParameter("@id", id));

                result = cmd.ExecuteNonQuery();
            }

            return result;
        }

        public List<TaskItem> GetByProjectId(int projectId)
        {
            var tasks = new List<TaskItem>();

            using (var cmd = _context.Conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT t.id, t.title, t.description, t.status, t.priority,
                           t.project_id, t.assigned_to, t.deadline,
                           p.nama AS project_name,
                           u.name AS assigned_to_name
                    FROM Tasks t
                    LEFT JOIN Projects p ON t.project_id = p.id
                    LEFT JOIN Users u ON t.assigned_to = u.id
                    WHERE t.project_id = @projectId
                    ORDER BY t.id";

                cmd.Parameters.Add(new SQLiteParameter("@projectId", projectId));

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var task = new TaskItem
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Title = reader["title"].ToString(),
                            Description = reader["description"].ToString(),
                            Status = reader["status"].ToString(),
                            Priority = reader["priority"].ToString(),
                            ProjectId = reader["project_id"] != DBNull.Value ? Convert.ToInt32(reader["project_id"]) : (int?)null,
                            AssignedTo = reader["assigned_to"] != DBNull.Value ? Convert.ToInt32(reader["assigned_to"]) : (int?)null,
                            Deadline = reader["deadline"].ToString(),
                            ProjectName = reader["project_name"].ToString(),
                            AssignedToName = reader["assigned_to_name"].ToString()
                        };

                        tasks.Add(task);
                    }
                }
            }

            return tasks;
        }
    }
}