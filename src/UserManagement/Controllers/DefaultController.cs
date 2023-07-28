using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using System.Diagnostics;

namespace UserManagement.Controllers
{
    public class DefaultController : Controller
    {
        private readonly ILogger<DefaultController> _logger;

        /// <summary>
        /// DefaultController
        /// </summary>
        /// <param name="logger"></param>
        public DefaultController(
            ILogger<DefaultController> logger) => this._logger = logger;

        /// <summary>
        /// Redirect to "Auth/Index" endpoint with parameters, without validating any authentication
        /// </summary>
        /// <param name="host">Where to go after successfully authenticated</param>
        /// <param name="popupOpened">If this endpoint is opened in popup, if this is true then it will be closed after authentication</param>
        /// <returns>Always redirect to "Auth/Index"</returns>		
        public IActionResult Index([FromQuery(Name = "auth_host")] string host, bool popupOpened = false)
        {
           // LogHelper.LogEvent(this._logger, "Event redirect to Auth/Index", "Redirect to Auth/Index", string.Format("Redirect to Auth/Index with host: {0}, popupOpened: {1}", host, popupOpened), HealthMonitor.ProcessSuccess);

            return this.RedirectToAction("Index", "user",
                new
                {
                    auth_host = host,
                    popup_opened = popupOpened
                }
            );
        }

        ///// <summary>
        ///// Error
        ///// </summary>
        ///// <returns></returns>
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error() => this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });

    }
}
