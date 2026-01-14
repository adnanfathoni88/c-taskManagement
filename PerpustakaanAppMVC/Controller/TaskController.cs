using PerpustakaanAppMVC.Model.Context;
using PerpustakaanAppMVC.Model.Entity;
using PerpustakaanAppMVC.Model.Repository;
using System;
using System.Collections.Generic;

namespace PerpustakaanAppMVC.Controller
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
    }
}