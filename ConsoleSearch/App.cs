using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleSearch
{
    public class App
    {
        private readonly SearchLogic mSearchLogic = new SearchLogic();

        public async Task RunAsync()
        {
            Console.WriteLine("Console Search");

            while (true)
            {
                Console.WriteLine("Enter search terms - q for quit [default: hello]");
                string input = Console.ReadLine() ?? "hello";
                if (input.Equals("q", StringComparison.OrdinalIgnoreCase)) break;

                var wordIds = new List<int>();
                var searchTerms = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                foreach (var t in searchTerms)
                {
                    int id = mSearchLogic.GetIdOf(t);
                    if (id != -1)
                    {
                        wordIds.Add(id);
                    }
                    else
                    {
                        Console.WriteLine(t + " will be ignored");
                    }
                }

                DateTime start = DateTime.Now;

                var docIdsTask = mSearchLogic.GetDocumentsAsync(wordIds);
                var retrievedDocIds = await docIdsTask;

                // get details for the first 10
                var top10 = retrievedDocIds.Take(10);

                TimeSpan used = DateTime.Now - start;

                int idx = 0;
                foreach (var docIdCountPair in top10)
                {
                    var docId = docIdCountPair.Key;
                    var count = docIdCountPair.Value;

                    var docDetails = await mSearchLogic.GetDocumentDetailsAsync(new List<int> { docId });
                    Console.WriteLine($"{idx + 1}: {docDetails[0]} -- contains {count} search terms");
                    idx++;
                }
                Console.WriteLine($"Documents: {retrievedDocIds.Count}. Time: {used.TotalMilliseconds} ms");

                await Task.Delay(1000);
            }
        }
    }
}
