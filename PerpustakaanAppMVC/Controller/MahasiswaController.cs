using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PerpustakaanAppMVC.Model.Context;
using PerpustakaanAppMVC.Model.Entity;
using PerpustakaanAppMVC.Model.Repository;
using PerpustakaanAppMVC.Model.Repository.api;


namespace PerpustakaanAppMVC.Controller
{
    public class MahasiswaController
    {

        private MahasiswaRestApiRepository _repoApi;

        private MahasiswaRepository _repository;

        // constructor
        public MahasiswaController()
        {
            _repoApi = new MahasiswaRestApiRepository();
        }

        public int Create(Mahasiswa mhs)
        {
            int result = 0;
            // cek npm yang diinputkan tidak boleh kosong
            if (string.IsNullOrEmpty(mhs.Npm))
            {
                throw new ArgumentException("NPM harus diisi !!!");
            }
            // cek nama yang diinputkan tidak boleh kosong
            if (string.IsNullOrEmpty(mhs.Nama))
            {
                throw new ArgumentException("Nama harus diisi !!!");
            }
            // cek angkatan yang diinputkan tidak boleh kosong
            if (string.IsNullOrEmpty(mhs.Angkatan))
            {
                throw new ArgumentException("Angkatan harus diisi !!!");
            }

            // use database
            //using (DbContext context = new DbContext())
            //{
            //    _repository = new MahasiswaRepository(context);
            //    result = _repository.Create(mhs);
            //}

            // use rest api
            result = _repoApi.Create(mhs);
            if (result == 0)
                throw new Exception("Create gagal (response API tidak sukses)");
            else 
                MessageBox.Show("Mahasiswa dengan NPM " + mhs.Npm + " berhasil ditambahkan.", "Info");

            return result;
        }

        public int Update(Mahasiswa mhs)
        {
            int result = 0;
            // cek npm yang diinputkan tidak boleh kosong
            if (string.IsNullOrEmpty(mhs.Npm))
            {
                throw new ArgumentException("NPM harus diisi !!!");
            }
            // cek nama yang diinputkan tidak boleh kosong
            if (string.IsNullOrEmpty(mhs.Nama))
            {
                throw new ArgumentException("Nama harus diisi !!!");
            }
            // cek angkatan yang diinputkan tidak boleh kosong
            if (string.IsNullOrEmpty(mhs.Angkatan))
            {
                throw new ArgumentException("Angkatan harus diisi !!!");
            }
            // use database
            //using (DbContext context = new DbContext())
            //{
            //    _repository = new MahasiswaRepository(context);
            //    result = _repository.Update(mhs);
            //}
            // use rest api
            result = _repoApi.Update(mhs);
            if (result == 0)
                throw new Exception("Update gagal (response API tidak sukses)");
            else
                MessageBox.Show("Mahasiswa dengan NPM " + mhs.Npm + " berhasil diupdate.", "Info");

            return result;
        }

        public int Delete(Mahasiswa mhs)
        {
            int result = 0;

            if (string.IsNullOrEmpty(mhs.Npm))
            {
                throw new ArgumentException("NPM harus diisi !!!");
            }

            // use rest api
            result = _repoApi.Delete(mhs);
            if (result == 0)
                throw new Exception("Delete gagal (response API tidak sukses)");
            else
                MessageBox.Show("Mahasiswa dengan NPM " + mhs.Npm + " berhasil dihapus.", "Info");
            return result;
        }

        public List<Mahasiswa> ReadByNama(string nama)
        {
            List<Mahasiswa> list = new List<Mahasiswa>();

            // use database
            //using (DbContext context = new DbContext())
            //{
            //    _repository = new MahasiswaRepository(context);
            //    list = _repository.ReadByNama(nama);
            //}

            // use rest api
            list = _repoApi.ReadByNama(nama);

            return list;
        }
        public List<Mahasiswa> ReadAll()
        {
            List<Mahasiswa> list = new List<Mahasiswa>();

            // use database
            //using (DbContext context = new DbContext())
            //{
            //    _repository = new MahasiswaRepository(context);
            //    list = _repository.ReadAll();
            // }

            // use rest api
            list = _repoApi.ReadAll();

            return list;
        }


    }
}