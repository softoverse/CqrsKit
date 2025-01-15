using Microsoft.AspNetCore.Mvc;

namespace Softoverse.CqrsKit.WebApi.Controllers.CommandQuery
{
    [ApiController]
    [Route("api/[controller]")]
    public class BackgroundTaskController : ControllerBase
    {
        [HttpPost("start")]
        public async Task<IActionResult> StartBackgroundTask(CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            Task.Run(() => BackgroundTask(), ct);
            //Thread.Sleep(10000);
            return Ok("Background task started.");
        }

        private async Task BackgroundTask()
        {
            Console.WriteLine($"Background task is running.");
            int count = 1;
            for (int i = 0; i < 1000; i++)
            {
                // Your background task logic goes here...

                // Simulate some work
                await Task.Delay(TimeSpan.FromMilliseconds(10));

                Console.Write($"Background task ran {count} times.\r");
                count++;
            }
            Console.WriteLine();
        }
    }
}