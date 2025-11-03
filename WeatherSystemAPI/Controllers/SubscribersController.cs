using Microsoft.AspNetCore.Mvc;
using WeatherApplication;
using WeatherApplication.Display;
using WeatherApplication.DTO;

namespace WeatherSystemAPI.Controllers
{
    [ApiController]
    [Route("api/subscribers")]
    public class SubscribersController : ControllerBase
    {
        private readonly WeatherStation _station;
        private readonly WeatherFacade _facade;

        public SubscribersController(WeatherFacade facade, WeatherStation station)
        {
            _facade = facade;
            _station = station;
        }

        [HttpPost]
        public ActionResult<Guid> Subscribe([FromBody] SubscribeRequest request)
        {
            var id = _facade.Subscribe(request.Subscriber, request.Weather);
            _station.Notify(request.Weather); 
            return Ok(id);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Unsubscribe(Guid id)
        {
            var removed = _facade.Unsubscribe(id);
            return removed ? NoContent() : NotFound();
        }
    }
}
