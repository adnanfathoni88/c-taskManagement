using TWEEKLE.Model.Context;
using TWEEKLE.Model.Entity;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace TWEEKLE.Model.Repository
{
    public class LogRepository
    {
        private DbContext _context;

        public LogRepository(DbContext context)
        {
            _context = context;
        }

        public List<Log> GetAll()
        {
            var logs = new List<Log>();

            using (var cmd = _context.Conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT l.id, l.action, l.user_id, l.task_id, l.description, l.timestamp,
                           u.name AS user_name,
                           t.title AS task_title
                    FROM Logs l
                    INNER JOIN Users u ON l.user_id = u.id
                    INNER JOIN Tasks t ON l.task_id = t.id
                    ORDER BY l.timestamp DESC";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var log = new Log
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Action = reader["action"].ToString(),
                            UserId = Convert.ToInt32(reader["user_id"]),
                            TaskId = Convert.ToInt32(reader["task_id"]),
                            Description = reader["description"].ToString(),
                            Timestamp = reader["timestamp"].ToString(),
                            UserName = reader["user_name"].ToString(),
                            TaskTitle = reader["task_title"].ToString()
                        };

                        logs.Add(log);
                    }
                }
            }

            return logs;
        }

        public List<Log> GetByTaskId(int taskId)
        {
            var logs = new List<Log>();

            using (var cmd = _context.Conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT l.id, l.action, l.user_id, l.task_id, l.description, l.timestamp,
                           u.name AS user_name,
                           t.title AS task_title
                    FROM Logs l
                    INNER JOIN Users u ON l.user_id = u.id
                    INNER JOIN Tasks t ON l.task_id = t.id
                    WHERE l.task_id = @taskId
                    ORDER BY l.timestamp DESC";

                cmd.Parameters.Add(new SQLiteParameter("@taskId", taskId));

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var log = new Log
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Action = reader["action"].ToString(),
                            UserId = Convert.ToInt32(reader["user_id"]),
                            TaskId = Convert.ToInt32(reader["task_id"]),
                            Description = reader["description"].ToString(),
                            Timestamp = reader["timestamp"].ToString(),
                            UserName = reader["user_name"].ToString(),
                            TaskTitle = reader["task_title"].ToString()
                        };

                        logs.Add(log);
                    }
                }
            }

            return logs;
        }

        public List<Log> GetByUserId(int userId)
        {
            var logs = new List<Log>();

            using (var cmd = _context.Conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT l.id, l.action, l.user_id, l.task_id, l.description, l.timestamp,
                           u.name AS user_name,
                           t.title AS task_title
                    FROM Logs l
                    INNER JOIN Users u ON l.user_id = u.id
                    INNER JOIN Tasks t ON l.task_id = t.id
                    WHERE l.user_id = @userId
                    ORDER BY l.timestamp DESC";

                cmd.Parameters.Add(new SQLiteParameter("@userId", userId));

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var log = new Log
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Action = reader["action"].ToString(),
                            UserId = Convert.ToInt32(reader["user_id"]),
                            TaskId = Convert.ToInt32(reader["task_id"]),
                            Description = reader["description"].ToString(),
                            Timestamp = reader["timestamp"].ToString(),
                            UserName = reader["user_name"].ToString(),
                            TaskTitle = reader["task_title"].ToString()
                        };

                        logs.Add(log);
                    }
                }
            }

            return logs;
        }

        public int Insert(Log log)
        {
            int result = 0;

            using (var cmd = _context.Conn.CreateCommand())
            {
                cmd.CommandText = @"
                    INSERT INTO Logs (action, user_id, task_id, description)
                    VALUES (@action, @user_id, @task_id, @description);
                    SELECT last_insert_rowid();";

                cmd.Parameters.Add(new SQLiteParameter("@action", log.Action));
                cmd.Parameters.Add(new SQLiteParameter("@user_id", log.UserId));
                cmd.Parameters.Add(new SQLiteParameter("@task_id", log.TaskId));
                cmd.Parameters.Add(new SQLiteParameter("@description", log.Description ?? ""));

                result = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return result;
        }

        public int Delete(int id)
        {
            int result = 0;

            using (var cmd = _context.Conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM Logs WHERE id = @id";
                cmd.Parameters.Add(new SQLiteParameter("@id", id));

                result = cmd.ExecuteNonQuery();
            }

            return result;
        }
    }
}