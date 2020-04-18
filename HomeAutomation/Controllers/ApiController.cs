using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq;
using HomeAutomation.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System;

namespace HomeAutomation.Controllers
{
    public class ApiController
    {
        private HttpClient client {get;set;}
        private Regex argpattern {get;set;}
        public ApiController() {
            client = new HttpClient();
            argpattern = new Regex("(?<key>[A-z]+)=::[A-z]+::");
        }
        public async Task<string[]> Action(string baseurl, string controlleraction, string[] arguments) {
            Debug.WriteLine(controlleraction);
            var parsedURL = controlleraction.Split("?").First()+"?";
            var matches = argpattern.Matches(controlleraction);
            for(int i=0;i<Math.Min(matches.Count,arguments.Length);i++){
                Match argument=matches[i];
                var queryKey=argument.Groups["key"].Value;
                parsedURL+=queryKey+"="+arguments[i]+"&";
            }
            parsedURL=parsedURL.Substring(1,parsedURL.Length-1);
            var resp = await client.GetAsync($"{baseurl}/{parsedURL}");
            if(!resp.IsSuccessStatusCode) {
                Debug.WriteLine("Request failed");
            }
            return arguments;
        }

        public async Task<Status> Status(string baseurl, string action) {
            var resp = await client.GetAsync($"{baseurl}/{action}");
            XDocument doc = XDocument.Parse(await resp.Content.ReadAsStringAsync());
            var status = doc.Element("status");

            var resp2 = await client.GetAsync($"{baseurl}/SyncStatus");
            XDocument doc2 = XDocument.Parse(await resp2.Content.ReadAsStringAsync());
            var syncStatus = doc2.Element("SyncStatus");
            if(syncStatus != null && status != null) {
                return new Status() {
                    Artist = status.Element("artist")?.Value,
                    Album = status.Element("album")?.Value,
                    Db = status.Element("db") == null? 0 : double.Parse(status.Element("db").Value),
                    Position = status.Element("secs") == null? 0 : int.Parse(status.Element("secs").Value),
                    Title = status.Element("title1")?.Value + "\n" + status.Element("title2")?.Value + "\n" + status.Element("title3")?.Value,
                    Volume = syncStatus.Attribute("volume") == null? 0 : int.Parse(syncStatus.Attribute("volume").Value),
                    Image = status.Element("image")?.Value,
                    State = status.Element("state")?.Value,
                    TotalLength = status.Element("totlen") == null? 0: int.Parse(status.Element("totlen").Value)
                };
            } else {
                return new Status();
            }
        }
    }
}