using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;

namespace CourseRegistration.Controllers
{
    public class LanguageController : Controller
    {

        private readonly IHtmlLocalizer<LanguageController> _localizer;
        public LanguageController(IHtmlLocalizer<LanguageController> localizer)
        {
            _localizer = localizer;
        }




        public IActionResult ChangeCultureByIcon(string culture, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName, 
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMonths(6)});

            return LocalRedirect(returnUrl);
        }





        [HttpPost]
        public IActionResult ChangeCultureBySelectList(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),

                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMonths(6) });

            return LocalRedirect(returnUrl);
        }





    }
}
