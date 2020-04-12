using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq;
using HomeAutomation.Models;

namespace HomeAutomation.Controllers
{
    public class ApiController
    {
        private HttpClient client {get;set;}
        public ApiController() {
            client = new HttpClient();
        }
        public void Action(string baseurl, string action, string[] arguments) {

        }

        public async Task<Status> Status(string baseurl, string action) {
            var resp = await client.GetAsync($"{baseurl}/{action}");
            XDocument doc = XDocument.Parse(await resp.Content.ReadAsStringAsync());
            var status = doc.Element("status");
            return new Status() {
                Artist = status.Element("artist")?.Value,
                Album = status.Element("album")?.Value,
                Db = double.Parse(status.Element("db")?.Value),
                Position = int.Parse(status.Element("secs")?.Value),
                Title = status.Element("title1")?.Value + "\n" + status.Element("title2")?.Value + "\n" + status.Element("title3")?.Value,
                Volume = int.Parse(status.Element("volume")?.Value),
                Image = status.Element("image")?.Value
            };
        }
    }
}