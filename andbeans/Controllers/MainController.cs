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
using System.Net;

namespace andbeans.Controllers
{
    public class MainController : Controller
    {
        Config _config;
        DataCache _cache;
        string _cacheKeyPrefix;

        public MainController()
        {
            _config = new Config();
            _cache = new DataCache();
            _cacheKeyPrefix = "andbeans-";
        }

        public ActionResult Index()
        {
            return View();
        }

        private BingSearchResultList FetchData(string query, int expirySeconds)
        {
            var key = _cacheKeyPrefix + "-image-query-" + query;
            var data = _cache.Get<BingSearchResultList>(key);

            // If the query's not in the cache
            if (data == null)
            {
                // Request the data
                var client = new RestClient("https://api.datamarket.azure.com");
                client.Authenticator = new HttpBasicAuthenticator("", _config.Get<string>("BingApiKey"));

                var req = new RestRequest("Bing/Search/v1/Image");
                req.AddParameter("ImageFilters", "'Size:Large+Aspect:Tall'");
                req.AddParameter("Query", "'" + query.Trim() + "'");

                var response = client.Execute<BingSearchResultContainer>(req);
                
                data = response.Data.D;

                // And add it to the cache
                _cache.Add(key, data, expirySeconds);
            }

            return data;
        }

        public ActionResult BeansItLikeAMother()
        {
            var data = FetchData("baked beans", _config.Get<int>("BeansCacheTimeSeconds"));
            return Json(data, JsonRequestBehavior.AllowGet);

            // return Content(System.IO.File.ReadAllText(Server.MapPath("/Tmp/beans.json")), "application/json");
        }

        public ActionResult GetImages(string query)
        {
            var data = FetchData(query, _config.Get<int>("QueryCacheTimeSeconds"));
            return Json(data, JsonRequestBehavior.AllowGet);

            // return Content(System.IO.File.ReadAllText(Server.MapPath("/Tmp/fish.json")), "application/json");
        }
        
        public ActionResult ClearCacheContents(string key)
        {
            if (key != _config.Get<string>("CacheViewKey"))
                return new HttpNotFoundResult();

            _cache.Clear();
            return Content("CLEARED CACHE");            
        }

        public ActionResult ShowCacheContents(string key)
        {
            if (key != _config.Get<string>("CacheViewKey"))
                return new HttpNotFoundResult();

            // We filter on Site Key because newer versions of MVC seem to stuff a load of other data into the memory cache 
            // which we're not interested in (break on the line below and inspect _cache.BaseCache to see what I mean...)
            return View(_cache.BaseCache.Where(x => x.Key.StartsWith(_cacheKeyPrefix, StringComparison.Ordinal)).OrderByDescending(x => x.Hits).ToList());
        }
    }
}