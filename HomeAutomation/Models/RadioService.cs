using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HomeAutomation.Models
{
    public class RadioService : Service
    {
        public List<string> RadioStations {get;set;}
        
        public async Task LoadStations() {
            var client = new HttpClient();
            var response = await client.GetAsync($"{this.BaseUrl}/RadioBrowse?service={this.Name}");
            var root = XDocument.Parse(await response.Content.ReadAsStringAsync());
            RadioStations.AddRange(await LoadNextStations());
        }

       

        private async Task<List<string>> LoadNextStations(string url = null) {
            List<string> tmpStations = new List<string>();
            var client = new HttpClient();
            string action = url == null ? $"{this.BaseUrl}/RadioBrowse?service={this.Name}" : $"{this.BaseUrl}/RadioBrowse?service={this.Name}&url={url}";
            var response = await client.GetAsync(action);
            XDocument root = XDocument.Parse(await response.Content.ReadAsStringAsync());
            foreach(var ele in root.Descendants()) {
                 if(ele.Name == "item" && ele.Attribute("type")?.Value == "audio") {
                    tmpStations.Add(ele.Attribute("url").Value);
                }
                else if(ele.Name == "item" && ele.Attribute("type")?.Value == "link") {
                    tmpStations.AddRange(await LoadNextStations(ele.Attribute("url").Value));
                }
            }
            return tmpStations;
        }
    }
}