using System.Net.Http;

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
    }
}