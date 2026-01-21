using TWEEKLE.Model.Context;

public static class DbInit
{
    public static void Initialize()
    {
        using (var context = new DbContext())
        {
            enableForeignKey(context);

            // create table
            createRoleTable(context);
            createUserTable(context);
            createProjectTable(context);
            createTaskTable(context);
            createLogTable(context);

            // seeder
            seedRole(context);
            seedUser(context);
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
                end_date TEXT,
                created_by INTEGER NOT NULL,
                FOREIGN KEY (created_by) REFERENCES Users(id)
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

    private static void createLogTable(DbContext context)
    {
        using (var cmd = context.Conn.CreateCommand())
        {
            cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Logs (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                action TEXT NOT NULL,
                user_id INTEGER NOT NULL,
                task_id INTEGER NOT NULL,
                description TEXT,
                timestamp TEXT DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY (user_id) REFERENCES Users(id),
                FOREIGN KEY (task_id) REFERENCES Tasks(id)
            );";
            cmd.ExecuteNonQuery();
        }
    }

    private static void seedRole(DbContext context)
    {
        using (var cmd = context.Conn.CreateCommand())
        {
            cmd.CommandText = @"
            INSERT OR IGNORE INTO Roles (id, name) VALUES
            (1, 'Admin'),
            (2, 'Project Manager'),
            (3, 'Developer'),
            (4, 'Tester');
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
            (1, 'Admin', 'admin@gmail.com', 'admin123', 1, 1),
            (2, 'Dev1', 'dev@gmail.com', 'dev123', 1, 3), 
            (3, 'Tester2', 'tester@gmail.com', 'tester123', 1, 3),
            (4, 'PM1', 'pm@gmail.com', 'pm123', 1, 2),
            (5, 'PM2', 'pm2@gmail.com', 'pm2123', 1, 2);
            ";
            cmd.ExecuteNonQuery();
        }
    }
}
