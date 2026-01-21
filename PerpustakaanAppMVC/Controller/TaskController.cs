using TWEEKLE.Model.Context;
using TWEEKLE.Model.Entity;
using TWEEKLE.Model.Repository;
using System;
using System.Collections.Generic;

namespace TWEEKLE.Controller
{
    public class TaskController
    {
        private TaskRepository _repository;

        public TaskController()
        {
            _repository = new TaskRepository(new DbContext());
        }

        public TaskController(DbContext context)
        {
            _repository = new TaskRepository(context);
        }

        public List<TaskItem> ReadAll()
        {
            return _repository.GetAll();
        }

        public TaskItem GetById(int id)
        {
            return _repository.GetById(id);
        }

        public int Create(TaskItem task)
        {
            return _repository.Insert(task);
        }

        public int Update(TaskItem task)
        {
            return _repository.Update(task);
        }

        public int Delete(int id)
        {
            return _repository.Delete(id);
        }

        public List<TaskItem> GetByProjectId(int projectId)
        {
            return _repository.GetByProjectId(projectId);
        }

        public int GetTotalTasks(int userId, string role)
        {
            using (var context = new Model.Context.DbContext())
            {
                var _repo = new TaskRepository(context);

                return _repo.CountTasksByRole(userId, role);
            }
        }

        public int GetTotalTasksAssignedToUser(string userId)
        {
            using (var context = new Model.Context.DbContext())
            {
                var _repo = new TaskRepository(context);
                return _repo.CountTasksAssignedToUser(userId);
            }
        }

        public Dictionary<string, int> GetTaskCountByStatus(string userId = null)
        {
            using (var context = new Model.Context.DbContext())
            {
                var _repo = new TaskRepository(context);
                return _repo.GetTaskCountByStatus(userId);
            }
        }

        public Dictionary<string, int> GetTaskCountByStatusForProjectManager(int userId)
        {
            using (var context = new Model.Context.DbContext())
            {
                var _repo = new TaskRepository(context);
                return _repo.GetTaskCountByStatusForProjectManager(userId);
            }
        }

        public List<TaskItem> GetTasksByProjectCreator(int userId)
        {
            using (var context = new Model.Context.DbContext())
            {
                var _repo = new TaskRepository(context);
                return _repo.GetTasksByProjectCreator(userId);
            }
        }

        public List<TaskItem> GetTasksAssignedToUser(int userId)
        {
            using (var context = new Model.Context.DbContext())
            {
                var _repo = new TaskRepository(context);
                return _repo.GetTasksByAssignedUser(userId.ToString());
            }
        }

        public List<TaskItem> GetTasksByRole(int userId, string role, int projectId)
        {
            using (var context = new Model.Context.DbContext())
            {
                var _repo = new TaskRepository(context);
                return _repo.GetTasksByRole(userId, role, projectId);
            }
        }
    }
}