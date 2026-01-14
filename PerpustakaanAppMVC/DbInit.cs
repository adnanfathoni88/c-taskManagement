using PerpustakaanAppMVC.Model.Context;

public static class DbInit
{
    public static void Initialize()
    {
        using (var context = new DbContext())
        {
            enableForeignKey(context);

            createRoleTable(context);
            createUserTable(context);
            createProjectTable(context);
            createTaskTable(context);

            seedRole(context);
            seedUser(context);
            seedProject(context);
            seedTask(context);
        }
    }

    private static void enableForeignKey(DbContext context)
    {
            using (var cmd = context.Conn.CreateCommand())
        {
            cmd.CommandText = "PRAGMA foreign_keys = ON;";
            cmd.ExecuteNonQuery();
        }
    }

    private static void createRoleTable(DbContext context)
    {
        using (var cmd = context.Conn.CreateCommand())
        {
            cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Roles (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT NOT NULL UNIQUE
            );";
            cmd.ExecuteNonQuery();
        }
    }

    private static void createUserTable(DbContext context)
    {
        using (var cmd = context.Conn.CreateCommand())
        {
            cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Users (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT NOT NULL,
                email TEXT NOT NULL UNIQUE,
                password TEXT NOT NULL,
                status INTEGER NOT NULL,
                role_id INTEGER NOT NULL,
                FOREIGN KEY (role_id) REFERENCES Roles(Id)
            );";
            cmd.ExecuteNonQuery();
        }
    }

    private static void createProjectTable(DbContext context)
    {
        using (var cmd = context.Conn.CreateCommand())
        {
            cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Projects (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                nama TEXT NOT NULL,
                deskripsi TEXT,
                status TEXT,
                start_date TEXT,
                end_date TEXT
            );";
            cmd.ExecuteNonQuery();
        }
    }

    private static void createTaskTable(DbContext context)
    {
        using (var cmd = context.Conn.CreateCommand())
        {
            cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Tasks (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                title TEXT NOT NULL,
                description TEXT,
                status TEXT NOT NULL,
                priority TEXT NOT NULL,
                project_id INTEGER,
                deadline TEXT,
                assigned_to INTEGER,
                FOREIGN KEY (project_id) REFERENCES Projects(id),
                FOREIGN KEY (assigned_to) REFERENCES Users(id)
            );";
            cmd.ExecuteNonQuery();
        }
    }

    private static void seedRole(DbContext context)
    {
        using (var cmd = context.Conn.CreateCommand())
        {
            cmd.CommandText = @"
            INSERT OR IGNORE INTO Roles (name) VALUES
            ('Admin'),
            ('User');
            ";
            cmd.ExecuteNonQuery();
        }
    }

    private static void seedUser(DbContext context)
    {
        using (var cmd = context.Conn.CreateCommand())
        {
            cmd.CommandText = @"
            INSERT OR REPLACE INTO Users (id, name, email, password, status, role_id) VALUES
            (1, 'Admin User', 'admin@example.com', 'password123', 1, 1),
            (2, 'John Doe', 'john@example.com', 'password123', 1, 2),
            (3, 'Jane Smith', 'jane@example.com', 'password123', 1, 2);
            ";
            cmd.ExecuteNonQuery();
        }
    }

    private static void seedProject(DbContext context)
    {
        using (var cmd = context.Conn.CreateCommand())
        {
            cmd.CommandText = @"
            INSERT OR REPLACE INTO Projects (id, nama, deskripsi, status, start_date, end_date) VALUES
            (1, 'Website Development', 'Develop company website', 'Active', '2023-01-01', '2023-06-30'),
            (2, 'Mobile App', 'Create mobile application', 'Active', '2023-02-01', '2023-08-31'),
            (3, 'Database Migration', 'Migrate legacy database', 'Inactive', '2022-11-01', '2023-01-31');
            ";
            cmd.ExecuteNonQuery();
        }
    }

    private static void seedTask(DbContext context)
    {
        using (var cmd = context.Conn.CreateCommand())
        {
            cmd.CommandText = @"
            INSERT OR REPLACE INTO Tasks (id, title, description, status, priority, project_id, assigned_to, deadline) VALUES
            (1, 'Design Homepage', 'Create homepage mockup', 'Pending', 'High', 1, 2, '2023-02-15'),
            (2, 'Setup Database', 'Configure database schema', 'In Progress', 'Medium', 1, 3, '2023-01-30'),
            (3, 'API Integration', 'Integrate REST API', 'Completed', 'High', 2, 2, '2023-03-30'),
            (4, 'UI Testing', 'Test user interface', 'Pending', 'Low', 2, 3, '2023-05-15');
            ";
            cmd.ExecuteNonQuery();
        }
    }
}
