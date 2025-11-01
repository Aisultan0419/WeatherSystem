using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherApplication.Display;
using WeatherApplication.DTO;
using WeatherApplication.Interfaces;

namespace WeatherSystemAPI.Controllers
{
    [ApiController]
    [Route("api/subscribers")]
    public class SubscribersController : ControllerBase
    {
        private readonly WeatherStation _station;
        private readonly IObserverFactory _factory;

        public SubscribersController(WeatherStation station, IObserverFactory factory)
        {
            _station = station; _factory = factory;
        }

        [HttpPost]
        public ActionResult<Guid> Subscribe([FromBody] SubscribeDto dto)
        {
            var observer = _factory.Create(dto);
            var id = _station.Subscribe(observer);
            return Ok(id);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Unsubscribe(Guid id)
        {
            var removed = _station.Unsubscribe(id);
            return removed ? NoContent() : NotFound();
        }
    }

}
