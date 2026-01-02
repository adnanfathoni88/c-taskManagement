using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.Reflection.Emit;

namespace PerpustakaanAppMVC.Model.Context
{
    public class DbContext : IDisposable
    {
        private SQLiteConnection _conn;

        public SQLiteConnection Conn
        {
            get
            {
                return _conn ?? (_conn = GetOpenConnection());
            }
        }

        // Method untuk melakukan koneksi ke database
        private SQLiteConnection GetOpenConnection()
        {
            SQLiteConnection conn = null; // deklarasi objek connection
            try // penggunaan blok try-catch untuk penanganan error
            {
                // atur ulang lokasi database yang disesuaikan dengan
                // lokasi database perpustakaan Anda
                string dbName = @"D:\02_CODING\C#\LibraryApp\PerpustakaanAppMVC\Database\DbPerpustakaan.db";
                // deklarasi variabel connectionString, ref: https://www.connectionstrings.com/
                string connectionString = string.Format("Data Source ={0}; FailIfMissing = True", dbName);
                conn = new SQLiteConnection(connectionString); // buat objekconnection
                conn.Open(); // buka koneksi ke database
            }
            // jika terjadi error di blok try, akan ditangani langsung oleh catch
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print("Open Connection Error: {0}",
                ex.Message);
            }
            return conn;
        }

        // Method ini digunakan untuk menghapus objek koneksi dari memory ketika sudah tidak digunakan
        public void Dispose()
        {
            if (_conn != null)
            {
                try
                {
                    if (_conn.State != ConnectionState.Closed) _conn.Close();
                }
                finally
                {
                    _conn.Dispose();
                }
            }
            GC.SuppressFinalize(this);
        }
    }
}
