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

            seedRole(context);
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
}
