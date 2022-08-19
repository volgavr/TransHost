using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json.Nodes;
using TransHostService.Helpers;
using TransHostService.Models;

namespace TransHostService.Controllers
{
    /// <inheritdoc />
    [ApiController]
    [Route("[controller]")]
    public class HostsController : ControllerBase
    {
        private readonly ILogger<HostsController> _logger;
        private readonly IConfiguration _config;

        public HostsController(ILogger<HostsController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [HttpGet("distance")]
        public async Task<ActionResult<double>> GetAsync(string fromHub, string toHub)
        {
            try
            {                
                var requestUri = new Uri(_config.GetValue<string>("TeleportUrl"));                
                
                using (var client = new HttpClient())
                {
                    client.BaseAddress = requestUri;
                    var fromCoord = JsonHelper.GetAirportCoordinatesAsync(client, fromHub);                    
                    var toCoord = JsonHelper.GetAirportCoordinatesAsync(client, toHub);
                    await Task.WhenAll(fromCoord, toCoord);
                    return GeoPoint.DistanceBetween(fromCoord.Result, toCoord.Result);
                }                
            }
            catch (HttpRequestException ex)
            {                
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    _logger.Log(LogLevel.Warning, ex, ex.Message);
                else 
                    _logger.Log(LogLevel.Error, ex, ex.Message);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Exception thrown");
                return BadRequest();
            }          
            
        }
    }    
}
