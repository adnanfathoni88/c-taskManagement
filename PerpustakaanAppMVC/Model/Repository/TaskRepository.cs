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

        public int CountTasksByRole(int userId, string role)
        {
            int count = 0;
            string sql = string.Empty;

            switch (role)
            {
                case "Admin":
                    sql = @"SELECT COUNT(*) FROM tasks";
                    break;

                case "Project Manager":
                    sql = @"SELECT COUNT(*)
                    FROM tasks t
                    INNER JOIN projects p ON p.id = t.project_id
                    WHERE p.created_by = @userId";
                    break;

                default: // Member / role lain
                    sql = @"SELECT COUNT(*)
                    FROM tasks
                    WHERE assigned_to = @userId";
                    break;
            }

            using (SQLiteCommand cmd = new SQLiteCommand(sql, _context.Conn))
            {
                if (role != "Admin")
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                }

                count = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return count;
        }


        public int CountTasksAssignedToUser(string userId)
        {
            int count = 0;

            if (!string.IsNullOrEmpty(userId))
            {
                string sql = @"SELECT COUNT(*)
                              FROM Tasks
                              WHERE assigned_to = @userId";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, _context.Conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            else
            {
                // If no userId is provided, return 0 as there are no assigned tasks
                count = 0;
            }

            return count;
        }

        public List<TaskItem> GetTasksByAssignedUser(string userId)
        {
            var tasks = new List<TaskItem>();

            string sql = @"
                SELECT t.id, t.title, t.description, t.status, t.priority,
                       t.project_id, t.assigned_to, t.deadline,
                       p.nama AS project_name,
                       u.name AS assigned_to_name
                FROM Tasks t
                LEFT JOIN Projects p ON t.project_id = p.id
                LEFT JOIN Users u ON t.assigned_to = u.id
                WHERE t.assigned_to = @userId
                ORDER BY t.id";

            using (var cmd = _context.Conn.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.Parameters.Add(new SQLiteParameter("@userId", userId));

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

        public List<TaskItem> GetTasksByProjectCreator(int userId)
        {
            var tasks = new List<TaskItem>();

            string sql = @"
                SELECT t.id, t.title, t.description, t.status, t.priority,
                       t.project_id, t.assigned_to, t.deadline,
                       p.nama AS project_name,
                       u.name AS assigned_to_name
                FROM Tasks t
                INNER JOIN Projects p ON t.project_id = p.id
                LEFT JOIN Users u ON t.assigned_to = u.id
                WHERE p.created_by = @userId
                ORDER BY t.id";

            using (var cmd = _context.Conn.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.Parameters.Add(new SQLiteParameter("@userId", userId));

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

        public Dictionary<string, int> GetTaskCountByStatus(string userId = null)
        {
            var taskCounts = new Dictionary<string, int>();

            string sql;
            if (!string.IsNullOrEmpty(userId))
            {
                sql = @"
                    SELECT status, COUNT(*) as count
                    FROM Tasks
                    WHERE assigned_to = @userId
                    GROUP BY status";
            }
            else
            {
                sql = @"
                    SELECT status, COUNT(*) as count
                    FROM Tasks
                    GROUP BY status";
            }

            using (var cmd = _context.Conn.CreateCommand())
            {
                cmd.CommandText = sql;

                if (!string.IsNullOrEmpty(userId))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@userId", userId));
                }

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string status = reader["status"].ToString();
                        int count = Convert.ToInt32(reader["count"]);
                        taskCounts[status] = count;
                    }
                }
            }

            return taskCounts;
        }

        public Dictionary<string, int> GetTaskCountByStatusForProjectManager(int userId)
        {
            var taskCounts = new Dictionary<string, int>();

            string sql = @"
                SELECT t.status, COUNT(*) as count
                FROM Tasks t
                INNER JOIN Projects p ON p.id = t.project_id
                WHERE p.created_by = @userId
                GROUP BY t.status";

            using (var cmd = _context.Conn.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.Parameters.Add(new SQLiteParameter("@userId", userId));

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string status = reader["status"].ToString();
                        int count = Convert.ToInt32(reader["count"]);
                        taskCounts[status] = count;
                    }
                }
            }

            return taskCounts;
        }


        public List<TaskItem> GetTasksByRole(int userId, string role, int projectId)
        {
            List<TaskItem> tasks = new List<TaskItem>();
            string sql = string.Empty;

            switch (role)
            {
                case "Admin":
                    sql = @"SELECT t.*, 
                           p.nama AS project_name, 
                           u.name AS assigned_to_name
                    FROM Tasks t
                    LEFT JOIN Projects p ON p.id = t.project_id
                    LEFT JOIN Users u ON u.id = t.assigned_to
                    WHERE t.project_id = @projectId";
                    break;

                case "Project Manager":
                    sql = @"SELECT t.*, 
                           p.nama AS project_name, 
                           u.name AS assigned_to_name
                    FROM Tasks t
                    INNER JOIN Projects p ON p.id = t.project_id
                    LEFT JOIN Users u ON u.id = t.assigned_to
                    WHERE p.created_by = @userId
                      AND t.project_id = @projectId";
                    break;

                default: // Member
                    sql = @"SELECT t.*, 
                           p.nama AS project_name, 
                           u.name AS assigned_to_name
                    FROM Tasks t
                    LEFT JOIN Projects p ON p.id = t.project_id
                    LEFT JOIN Users u ON u.id = t.assigned_to
                    WHERE t.assigned_to = @userId
                      AND t.project_id = @projectId";
                    break;
            }

            using (SQLiteCommand cmd = new SQLiteCommand(sql, _context.Conn))
            {
                cmd.Parameters.AddWithValue("@projectId", projectId);

                if (role != "Admin")
                    cmd.Parameters.AddWithValue("@userId", userId);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TaskItem task = new TaskItem
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Title = reader["title"].ToString(),
                            Description = reader["description"].ToString(),
                            Status = reader["status"].ToString(),
                            Priority = reader["priority"].ToString(),
                            ProjectId = reader["project_id"] != DBNull.Value
                                        ? Convert.ToInt32(reader["project_id"])
                                        : (int?)null,
                            AssignedTo = reader["assigned_to"] != DBNull.Value
                                        ? Convert.ToInt32(reader["assigned_to"])
                                        : (int?)null,
                            Deadline = reader["deadline"].ToString(),

                            // ✅ hasil JOIN
                            ProjectName = reader["project_name"]?.ToString(),
                            AssignedToName = reader["assigned_to_name"]?.ToString()
                        };

                        tasks.Add(task);
                    }
                }
            }

            return tasks;
        }

    }
}