using System;

namespace PerpustakaanAppMVC.Model.Entity
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Status { get; set; } // 0 = Pending, 1 = In Progress, 2 = Completed
        public int Priority { get; set; } // 0 = Low, 1 = Medium, 2 = High
        public int ProjectId { get; set; }
        public int AssignedTo { get; set; }
        public DateTime Deadline { get; set; }

        // Computed properties for display
        public string StatusText
        {
            get
            {
                return Status switch
                {
                    0 => "Pending",
                    1 => "In Progress",
                    2 => "Completed",
                    _ => "Unknown"
                };
            }
        }

        public string PriorityText
        {
            get
            {
                return Priority switch
                {
                    0 => "Low",
                    1 => "Medium",
                    2 => "High",
                    _ => "Unknown"
                };
            }
        }

        public string ProjectName { get; set; }
        public string AssignedToName { get; set; }
    }
}