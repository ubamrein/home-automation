namespace HomeAutomation.Models
{
    public class Service
    {
        public string DisplayName {get;set;}
        public string Name {get;set;}

        public string Type {get;set;}
        public string Icon {get;set;}

        public string getPlayUrl(string url) {
            return $"/Play?service={Name}&url={url}";
        }
    }
}