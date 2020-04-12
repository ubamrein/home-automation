using System.Collections.Generic;

namespace HomeAutomation.Models
{
    public class HomeModel {
        public List<ControllerModel> Controllers {get;set;}
        public List<Sensor> Sensors {get;set;}
    }
}