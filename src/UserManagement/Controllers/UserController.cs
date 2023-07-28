using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.Extensions.Configuration;

namespace UserManagement.Controllers
{
    //[Authorize(AuthenticationSchemes = AuthenticationConstants.AAD_AUTHENTICATION_SCHEME)]
    [Controller]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserController> _logger;
        private static string CONFIG_KEY_ORIGINS = "AllowedOrigin";

        public UserController(
            IConfiguration configuration,
            ILogger<UserController> logger)
        {
            this._logger = logger;
            this._configuration = configuration;
        }

        //[Authorize]
        [HttpGet]
        public IActionResult Index([FromQuery(Name = "auth_host")] string host, [FromQuery(Name = "popup_opened")] bool popupOpenend)
        {
            if (!string.IsNullOrWhiteSpace(host))
            {
                var hostUri = new Uri(host);
                var urlToCheckAccess = string.Empty;
                if (hostUri.PathAndQuery.Equals("/"))
                {
                    urlToCheckAccess = hostUri.AbsoluteUri;
                    if (!string.IsNullOrWhiteSpace(urlToCheckAccess) && urlToCheckAccess.Last() == '/')
                    {
                        urlToCheckAccess = urlToCheckAccess.Remove(urlToCheckAccess.Length - 1);
                    }
                }
                else
                {
                    urlToCheckAccess = hostUri.AbsoluteUri.Replace(hostUri.PathAndQuery, "");
                }
                if (!string.IsNullOrWhiteSpace(urlToCheckAccess) &&
                    !this._configuration.GetSection(CONFIG_KEY_ORIGINS)
                                                  .AsEnumerable()
                                                  .Any(x => urlToCheckAccess.Equals(x.Value, StringComparison.OrdinalIgnoreCase)))
                {
                    this.ViewData["ErrorHost"] = host;
                    return this.View();
                }
            }
           
            this.ViewData["Host"] = host;
            this.ViewData["popup_openend"] = popupOpenend;
            return this.View();
        }
    }
}
