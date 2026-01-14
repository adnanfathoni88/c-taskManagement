using System;

namespace PerpustakaanAppMVC.Model.Entity
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } // Pending, In Progress, Completed
        public string Priority { get; set; } // Low, Medium, High
        public int? ProjectId { get; set; }
        public string Deadline { get; set; } // Using string to store date in SQLite format
        public int? AssignedTo { get; set; }

        // Computed properties for display
        public string ProjectName { get; set; }
        public string AssignedToName { get; set; }
    }
}