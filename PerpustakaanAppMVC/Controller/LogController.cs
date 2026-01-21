using TWEEKLE.Model.Context;
using TWEEKLE.Model.Entity;
using TWEEKLE.Model.Repository;
using System;
using System.Collections.Generic;

namespace TWEEKLE.Controller
{
    public class LogController
    {
        private LogRepository _repository;

        public LogController()
        {
            _repository = new LogRepository(new DbContext());
        }

        public LogController(DbContext context)
        {
            _repository = new LogRepository(context);
        }

        public List<Log> ReadAll()
        {
            return _repository.GetAll();
        }

        public List<Log> GetByTaskId(int taskId)
        {
            return _repository.GetByTaskId(taskId);
        }

        public List<Log> GetByUserId(int userId)
        {
            return _repository.GetByUserId(userId);
        }

        public int Create(Log log)
        {
            return _repository.Insert(log);
        }

        public int Delete(int id)
        {
            return _repository.Delete(id);
        }
    }
}