using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace HomeAutomation.Models
{
    public class ControllerModel
    {
        public string Name {get;set;}
        public string BaseUrl {get;set;}
        public Dictionary<string, string> Actions {get;set;}
        public List<string> QuickActions {get;set;}
        public List<Service> Services {get;set;} = new List<Service>();
        public string ReadOut {get;set;}
        public string Icon {get;set;}

        public async Task LoadServices() {
            if(Actions.ContainsKey("GetServices")) {
                var client = new HttpClient();
                var response = await client.GetAsync($"{BaseUrl}{Actions["GetServices"]}");
                XDocument doc = XDocument.Parse(await response.Content.ReadAsStringAsync());
                foreach(XElement element in doc.Descendants()) {
                   if(element.Name == "service" && element.Attribute("type").Value == "RadioService") {
                        RadioService service = new RadioService() {
                            DisplayName = element.Attribute("displayname").Value,
                            Name = element.Attribute("name").Value,
                            Type = element.Attribute("type").Value,
                            BaseUrl = BaseUrl
                        };
                        await service.LoadStations();
                        Services.Add(service);
                   }
                }
            }
        }
    }
}