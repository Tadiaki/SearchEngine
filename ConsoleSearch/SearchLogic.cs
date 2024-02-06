using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleSearch
{
    public class SearchLogic
    {
        HttpClient api = new()
        {
            BaseAddress = new Uri("https://localhost:44350/")
        };

        Dictionary<string, int> mWords;

        public SearchLogic()
        {
            InitializeAsync().Wait();
        }

        private async Task InitializeAsync()
        {
            mWords = await GetAllWordsAsync();
        }

        public int GetIdOf(string word)
        {
            if (mWords.ContainsKey(word))
                return mWords[word];
            return -1;
        }

        private async Task<Dictionary<string, int>> GetAllWordsAsync()
        {
            var url = "Word";

            using (var response = await api.GetAsync(url))
            {
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Dictionary<string, int>>(content);
            }
        }

        public async Task<Dictionary<int, int>> GetDocumentsAsync(List<int> wordIds)
        {
            var url = "Document/GetByWordIds?wordIds=" + string.Join("&wordIds=", wordIds);
            var response = await api.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Dictionary<int, int>>(content);
        }

        public async Task<List<string>> GetDocumentDetailsAsync(List<int> docIds)
        {
            var url = "Document/GetByDocIds?docIds=" + string.Join("&docIds=", docIds);
            var response = await api.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<string>>(content);
        }
    }
}
