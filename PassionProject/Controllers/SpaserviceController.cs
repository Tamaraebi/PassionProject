using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
//using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Services.Description;
using PassionProject.Models;

namespace PassionProject.Controllers
{
    public class SpaserviceController : Controller
    {


        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static SpaserviceController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44364/api/");
        }

        // GET: Service/List
        public ActionResult List()
        {
            string url = "spaservicedata/listspaservices";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<SpaServiceDto> spaservice = response.Content.ReadAsAsync<IEnumerable<SpaServiceDto>>().Result;
            return View(spaservice);
        }

        // GET: Service/Details/5
        public ActionResult Details(int id)
        {
            string url = "spaservicedata/findspaservice/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            SpaServiceDto selectTreatment = response.Content.ReadAsAsync<SpaServiceDto>().Result;
            return View(selectTreatment);
        }

        // GET: Service/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Service/Create
        [HttpPost]
        public ActionResult Create(Service spaservices)
        {

            string url = "servicedata/addservice";

            string jsonpayload = jss.Serialize(spaservices);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return View("Error");
            }


        }

        // GET: Service/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "spaservicedata/findspaservice/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            SpaServiceDto selectedTreatment = response.Content.ReadAsAsync<SpaServiceDto>().Result;
            return View(selectedTreatment);
        }

        // POST: Service/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Service spaservices)
        {
            string url = "spaservicedata/updatespaservice/" + id;
            string jsonpayload = jss.Serialize(spaservices);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);


            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        // GET: Service/Delete/5

        public ActionResult ConfirmDelete(int id)
        {
            string url = "spaservicedata/findspaservice/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            SpaServiceDto selectedService = response.Content.ReadAsAsync<SpaServiceDto>().Result;
            return View(selectedService);
        }

        // POST: Service/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {

            string url = "spaservicedata/deletespaservice/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}
