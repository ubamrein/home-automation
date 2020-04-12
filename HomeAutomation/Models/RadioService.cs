using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HomeAutomation.Models
{
    public class RadioService : Service
    {
        public List<RadioStation> RadioStations {get;set;} = new List<RadioStation>();
        
        public async Task LoadStations() {
            var client = new HttpClient();
            var response = await client.GetAsync($"{this.BaseUrl}/RadioBrowse?service={this.Name}");
            var root = XDocument.Parse(await response.Content.ReadAsStringAsync());
            RadioStations.AddRange(await LoadNextStations());
        }

       

        private async Task<List<RadioStation>> LoadNextStations(string url = null) {
            try {
                List<RadioStation> tmpStations = new List<RadioStation>();
                var client = new HttpClient();
                string action = url == null ? $"{this.BaseUrl}/RadioBrowse?service={this.Name}" : $"{this.BaseUrl}/RadioBrowse?service={this.Name}&url={url}";
                var response = await client.GetAsync(action);
                XDocument root = XDocument.Parse(await response.Content.ReadAsStringAsync());
                foreach(var ele in root.Descendants()) {
                    if(ele.Name == "item" && ele.Attribute("type")?.Value == "audio") {
                        var radiostation = new RadioStation();
                        radiostation.URL = ele.Attribute("URL").Value;
                        radiostation.Name = ele.Attribute("text").Value;
                        tmpStations.Add(radiostation);
                    }
                    else if(ele.Name == "item" && ele.Attribute("type")?.Value == "link") {
                        tmpStations.AddRange(await LoadNextStations(ele.Attribute("URL").Value));
                    }
                    if(tmpStations.Count > 10) {
                        return tmpStations;
                    }
                }
                return tmpStations;
            }
            catch(Exception ex) {
                return new List<RadioStation>();
            }
        }
    }
}