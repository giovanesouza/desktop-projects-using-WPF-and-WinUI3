using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace FilteringSortingApiWinUi.Services
{
    internal class ApiClient
    {
        private readonly RestClient _client;

        public ApiClient(string baseUrl)
        {
            _client = new RestClient(baseUrl);
        }

        /// <summary>
        /// Retorna uma lista com 'n' nomes.
        /// </summary>
        /// <param name="totalNames">Define o total de itens a serem retornados</param>
        public async Task<List<string>> GetNames(int totalNames)
        {
            return await GetData("nomes", totalNames);
        }

        /// <summary>
        /// Retorna uma lista com 'n' sobrenomes.
        /// </summary>
        /// <param name="totalSurnames">Define o total de itens a serem retornados</param>
        public async Task<List<string>> GetSurname(int totalSurnames)
        {
            return await GetData("apelidos", totalSurnames);
        }

        private async Task<List<string>> GetData(string endpoint, int n)
        {
            var request = new RestRequest($"{endpoint}/{n}");
            var response = await _client.ExecuteAsync<List<string>>(request);
            return response.Data;
        }


    }

}
