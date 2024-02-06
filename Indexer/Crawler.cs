using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Indexer
{
    public class Crawler
    {
        private readonly char[] sep = " \\\n\t\"$'!,?;.:-_**+=)([]{}<>/@&%€#".ToCharArray();

        private Dictionary<string, int> words = new Dictionary<string, int>();
        private Dictionary<string, int> documents = new Dictionary<string, int>();

        HttpClient api = new()
        {
            BaseAddress = new Uri("https://localhost:44350")
        };

        private async Task<ISet<string>> ExtractWordsInFileAsync(FileInfo f)
        {
            ISet<string> res = new HashSet<string>();
            var content = await File.ReadAllLinesAsync(f.FullName);
            foreach (var line in content)
            {
                foreach (var aWord in line.Split(sep, StringSplitOptions.RemoveEmptyEntries))
                {
                    res.Add(aWord);
                }
            }
            return res;
        }

        private Task<ISet<int>> GetWordIdFromWordsAsync(ISet<string> src)
        {
            ISet<int> res = new HashSet<int>();
            foreach (var p in src)
            {
                if (words.ContainsKey(p))
                {
                    res.Add(words[p]);
                }
                else
                {
                    Console.WriteLine($"Word '{p}' not found in dictionary.");
                }
            }
            return Task.FromResult(res as ISet<int>);
        }

        public async Task IndexFilesInAsync(DirectoryInfo dir, List<string> extensions)
        {
            Console.WriteLine("Crawling " + dir.FullName);
            foreach (var file in dir.EnumerateFiles())
            {
                if (extensions.Contains(file.Extension))
                {
                    documents.Add(file.FullName, documents.Count + 1);
                    var documentMessage = new HttpRequestMessage(HttpMethod.Post, "Document?id=" + documents[file.FullName] + "&url=" + Uri.EscapeDataString(file.FullName));
                    await api.SendAsync(documentMessage);

                    Dictionary<string, int> newWords = new Dictionary<string, int>();
                    ISet<string> wordsInFile = await ExtractWordsInFileAsync(file);
                    foreach (var aWord in wordsInFile)
                    {
                        if (!words.ContainsKey(aWord))
                        {
                            words.Add(aWord, words.Count + 1);
                            newWords.Add(aWord, words[aWord]);
                        }
                    }

                    var wordMessage = new HttpRequestMessage(HttpMethod.Post, "Word");
                    wordMessage.Content = JsonContent.Create(newWords);
                    await api.SendAsync(wordMessage);

                    var occurrenceMessage = new HttpRequestMessage(HttpMethod.Post, "Occurrence?docId=" + documents[file.FullName]);
                    occurrenceMessage.Content = JsonContent.Create(await GetWordIdFromWordsAsync(wordsInFile));
                    await api.SendAsync(occurrenceMessage);
                }
            }

            foreach (var d in dir.EnumerateDirectories())
            {
                await IndexFilesInAsync(d, extensions);
            }
        }
    }
}
