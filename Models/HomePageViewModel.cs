using RezervationApp.Entities;

namespace RezervationApp.Models
{
	public class HomePageViewModel
	{
		public IEnumerable<Slider> Sliders {get; set;}
		public IEnumerable<Service> Services {get; set;}
	}
}
