using System;

namespace PerpustakaanAppMVC.Model.Entity
{
    public class Log
    {
        public int Id { get; set; }
        public string Action { get; set; } // e.g., "Created", "Updated", "Deleted", "Started", "Completed"
        public int UserId { get; set; }
        public int TaskId { get; set; }
        public string Description { get; set; }
        public string Timestamp { get; set; }

        // Helper properties for display
        public string UserName { get; set; }
        public string TaskTitle { get; set; }
    }
}