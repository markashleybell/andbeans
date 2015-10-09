using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestSharp;
using RestSharp.Authenticators;
using andbeans.Models.Bing;

namespace andbeans.Controllers
{
    public class MainController : Controller
    {
        private string _bingApiKey;

        public MainController()
        {
            _bingApiKey = ConfigurationManager.AppSettings["BingApiKey"];
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetImages(string query)
        {
            var client = new RestClient("https://api.datamarket.azure.com");
            client.Authenticator = new HttpBasicAuthenticator("", _bingApiKey);

            var req = new RestRequest("Bing/Search/v1/Image");
            req.AddParameter("ImageFilters", "'Size:Large'");
            req.AddParameter("Query", "'" + query.Trim() + "'");

            var response = client.Execute<BingSearchResultContainer>(req);

            return Json(response.Data.D, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}