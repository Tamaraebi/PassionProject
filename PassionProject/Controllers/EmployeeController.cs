using PassionProject.Models;
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

namespace PassionProject.Controllers
{
    public class EmployeeController : ApiController
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static EmployeeController()
        {
            client = new HttpClient;
            client.BaseAddress = new Uri("https://localhost:44364/api/");
     

        }


        // GET: Employee/List
        public ActionResult List()
        {
            string url = "employeedata/listemployees";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<EmployeeDto> employees = response.Content.ReadAsAsync<IEnumerable<EmployeeDto>>().Result;
            return View(employees);
        }

        // GET: Employee/Details/5
        public ActionResult Details(int id)
        {
            string url = "employeedata/findemployee/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            EmployeeDto selectedEmployee = response.Content.ReadAsAsync<EmployeeDto>().Result;

            url = "appointmentdata/FindAppointmentsByEmployee/" + id;
            response = client.GetAsync(url).Result;
            selectedEmployee.Appointments = response.Content.ReadAsAsync<List<AppointmentDto>>().Result;

            //IEnumerable<IGrouping<DateTime,AppointmentDto>> appointmentsGroup = selectedEmployee.Appointments.GroupBy(x => x.StartTime.Date);
            return View(selectedEmployee);
        }

        [HttpGet]
        public JsonResult GetEmployeesByServiceId(int id)
        {
            string url = "employeedata/GetEmployeesByServiceId/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<EmployeeDto> employees = response.Content.ReadAsAsync<IEnumerable<EmployeeDto>>().Result;
            return Json(employees);
        }

        // GET: Employee/Create
        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Employee employee)
        {
            Debug.WriteLine("the json payload is :");

            string url = "employeedata/addemployee";

            string jsonpayload = jss.Serialize(employee);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
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

        // GET: Employee/Delete/5
        [HttpGet]
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "employeedata/findemployee/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            EmployeeDto selectedEmployee = response.Content.ReadAsAsync<EmployeeDto>().Result;
            return View(selectedEmployee);
        }

        // POST: Employee/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
         
                string url = "employeedata/deleteemployee/" + id;
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
    }
}
