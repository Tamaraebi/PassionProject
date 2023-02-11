using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Services.Description;

namespace PassionProject.Controllers
{
    public class SpaserviceController : ApiController
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
            string url = "servicedata/listservices";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<ServiceDto> services = response.Content.ReadAsAsync<IEnumerable<ServiceDto>>().Result;
            return View(services);
        }

        // GET: Service/Details/5
        public ActionResult Details(int id)
        {
            string url = "servicedata/findservice/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ServiceDto selectTreatment = response.Content.ReadAsAsync<ServiceDto>().Result;
            return View(selectedTreatment);
        }

        // GET: Service/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Service/Create
        [HttpPost]
        public ActionResult Create(Service spaservice)
        {
    
                string url = "servicedata/addservice";

                string jsonpayload = jss.Serialize(service);

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
      
        }

        // GET: Service/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "servicedata/findservice/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ServiceDto selectedTreatment = response.Content.ReadAsAsync<ServiceDto>().Result;
            return View(selectedTreatment);
        }

        // POST: Service/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Service spaservice)
        {
             string url = "servicedata/updateservice/" + id;
             string jsonpayload = jss.Serialize(service);
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
            string url = "servicedata/findservice/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ServiceDto selectedService = response.Content.ReadAsAsync<ServiceDto>().Result;
            return View(selectedService);
        }

        // POST: Service/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {

            string url = "servicedata/deleteservice/" + id;
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
