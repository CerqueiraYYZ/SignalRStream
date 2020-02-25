using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static SignalRWebTransactions.Requestlog;

namespace SignalRWebTransactions.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> DoLongPolling()
        {
            SimpleLongPolling lp = new SimpleLongPolling("sample-channel");


            while (true)
            {
                (var message, var recursive) = await lp.WaitAsync();

                      return StatusCode(200, new ObjectResult(new { Message = (message != null ? message : "Long polling timeout!") }));

            }

        }

       // ----original
        //public async Task<IActionResult> DoLongPolling()
        //{
        //    SimpleLongPolling lp = new SimpleLongPolling("sample-channel");
        //    var message = await lp.WaitAsync();
        //    return new ObjectResult(new { Message = (message != null ? message : "Long polling timeout!") });
        //}


        [HttpGet]
        [Route("trigger")]
        public IActionResult SimulateTrigger(string message = "Sample publish message")
        {
            SimpleLongPolling.Publish("sample-channel", message);
            return new ObjectResult(new { Message = message, Status = "Published" });
        }
    }
}
