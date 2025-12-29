using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace UpdatedDateChecker
{
    internal class Program
    {
        public const string target = "./mods.json";
        static void Main(string[] args)
        {
            HttpClient httpClient = new HttpClient()
            { 
                BaseAddress = new Uri("https://reforger.armaplatform.com/workshop/")
            }
            ;
            Console.WriteLine($"Checking {target}");

            string fileContents = File.ReadAllText(target);

            var modList = JsonSerializer.Deserialize<List<ModMetaData>>(fileContents);
            Console.WriteLine("Mod Name, Last Modified");
            foreach(var mod in modList)
            {
                using HttpResponseMessage response = httpClient.GetAsync(mod.ModId).Result;

                var htmlResponse = response.Content.ReadAsStringAsync().Result;
                int modifiedIndex = htmlResponse.IndexOf("Last Modified</dt>");
                string modifiedDate = htmlResponse.Substring(modifiedIndex + 54, 10);
                Console.WriteLine($"{mod.Name},{modifiedDate}");
            }
            

        }
    }
}
