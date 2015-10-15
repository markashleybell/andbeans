using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestSharp;
using RestSharp.Authenticators;
using andbeans.Models.Bing;
using System.Threading;
using andbeans.Models;
using Newtonsoft.Json;

namespace andbeans.Controllers
{
    public class MainController : Controller
    {
        string _bingApiKey;
        DataCache _cache;
        string _cacheKeyPrefix;

        public MainController()
        {
            _bingApiKey = ConfigurationManager.AppSettings["BingApiKey"];
            _cache = new DataCache();
            _cacheKeyPrefix = "andbeans-";
        }

        public ActionResult Index()
        {
            return View();
        }

        private BingSearchResultContainer FetchData(string query, CacheExpiry expiry)
        {
            var key = _cacheKeyPrefix + "-image-query-" + query;
            var data = _cache.Get<BingSearchResultContainer>(key);

            // If the query's not in the cache
            if (data == null)
            {
                // Request the data
                var client = new RestClient("https://api.datamarket.azure.com");
                client.Authenticator = new HttpBasicAuthenticator("", _bingApiKey);

                var req = new RestRequest("Bing/Search/v1/Image");
                req.AddParameter("ImageFilters", "'Size:Large+Aspect:Tall'");
                req.AddParameter("Query", "'" + query.Trim() + "'");

                var response = client.Execute<BingSearchResultContainer>(req);
                
                data = response.Data;

                // And add it to the cache
                _cache.Add(key, data, (int)expiry);
            }

            return data;
        }

        public ActionResult GetImages(string query)
        {
            var data = FetchData(query, CacheExpiry.ThirtySeconds);
            return Json(data.D, JsonRequestBehavior.AllowGet);

            // return Content(System.IO.File.ReadAllText(Server.MapPath("/Tmp/fish.json")), "application/json");
        }

        public ActionResult GetBeans()
        {
            var data = FetchData("baked beans", CacheExpiry.OneMinute);
            return Json(data.D, JsonRequestBehavior.AllowGet);

            // return Content(System.IO.File.ReadAllText(Server.MapPath("/Tmp/beans.json")), "application/json");
        }

        public ActionResult ClearCacheContents()
        {
            _cache.Clear();
            return Content("CLEARED ALL CACHE");            
        }

        public ActionResult ShowCacheContents()
        {
            // We filter on Site Key because newer versions of MVC seem to stuff a load of other data into the memory cache 
            // which we're not interested in (break on the line below and inspect _cache.BaseCache to see what I mean...)
            return View(_cache.BaseCache.Where(x => x.Key.StartsWith(_cacheKeyPrefix, StringComparison.Ordinal)).OrderByDescending(x => x.Hits).ToList());
        }
    }
}