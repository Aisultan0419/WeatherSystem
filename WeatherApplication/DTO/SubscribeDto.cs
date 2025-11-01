
namespace WeatherApplication.DTO
{
    public class SubscribeDto
    {
        public string Name { get; set; } = "";
        public string Method { get; set; } = ""; 
        public string City { get; set; } = "Almaty";

        public string? ConnectionId { get; set; } 
    }

}
