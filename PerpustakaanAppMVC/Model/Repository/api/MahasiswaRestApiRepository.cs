using PerpustakaanAppMVC.Model.Entity;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace PerpustakaanAppMVC.Model.Repository.api
{
    public class MahasiswaRestApiRepository
    {
        // baseUrl didefinisikan sekali
        private readonly RestClient _client;
        private string endpoint = "api/mahasiswa";

        public MahasiswaRestApiRepository()
        {
            string baseUrl = "http://latihan.coding4ever.net:5555/";
            _client = new RestClient(baseUrl);
        }

        public int Create(Mahasiswa mhs)
        {
            var request = new RestRequest(endpoint, Method.POST);
            request.AddJsonBody(mhs);

            var response = _client.Execute(request);
            return response.IsSuccessful ? 1 : 0;
        }

        public int Update(Mahasiswa mhs)
        {
            var endpoint = $"api/mahasiswa/{mhs.Npm}";
            var request = new RestRequest(endpoint, Method.PUT);
            request.AddJsonBody(mhs);

            var response = _client.Execute(request);
            return response.IsSuccessful ? 1 : 0;
        }

        public int Delete(Mahasiswa mhs)
        {
            var request = new RestRequest($"{endpoint}/{mhs.Npm}", Method.DELETE);

            var response = _client.Execute(request);
            return response.IsSuccessful ? 1 : 0;
        }

        public List<Mahasiswa> ReadAll()
        {
            var request = new RestRequest(endpoint, Method.GET);
            var response = _client.Execute<List<Mahasiswa>>(request);

            return response.Data;
        }

        public Mahasiswa ReadByNpm(string npm)
        {
            var request = new RestRequest($"{endpoint}/{npm}", Method.GET);
            var response = _client.Execute<Mahasiswa>(request);

            return response.Data;
        }

        public List<Mahasiswa> ReadByNama(string nama)
        {
            var request = new RestRequest(endpoint, Method.GET);
            request.AddParameter("nama", nama);

            var response = _client.Execute<List<Mahasiswa>>(request);
            return response.Data;
        }
    }
}