using MeterReadingApi.Contracts.Response;
using MeterReadingApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeterReadingApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MeterReadingController : ControllerBase
    {
        private readonly IMeterReadingsService meterReadingService;

        public MeterReadingController(IMeterReadingsService meterReadingService)
        {
            this.meterReadingService = meterReadingService;
        }

        [HttpPost]
        public async Task<ActionResult<PostMeterReadingsResponse>> Post(IFormFile meterReadingsFile)
        {
            using var meterReadingStream = new MemoryStream();
            await meterReadingsFile.CopyToAsync(meterReadingStream);

            (var successfulReadings, var failedReadings)  = this.meterReadingService.ProcessMeterReadings(meterReadingStream);

            var response = new PostMeterReadingsResponse
            {
                SuccessfulReadings = successfulReadings,
                FailedReadings = failedReadings
            };

            return Ok(response);
        }
    }
}
