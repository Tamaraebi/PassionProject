using PassionProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Services.Description;

namespace PassionProject.Controllers
{
    public class EmployeeDataController : ApiController
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns a list of all the employees in the system
        /// </summary>
      
        [HttpGet]
        public IHttpActionResult ListEmployees()
        {
            List<Employee> employees = db.Employees.ToList();
            List<EmployeeDto> employeeDtos = new List<EmployeeDto>();
            employees.ForEach(a => employeeDtos.Add(new EmployeeDto()
            {
                EmployeeId = a.EmployeeId,
                Name = a.Name,
                Bio = a.Bio,
            }));
            return Ok(employeeDtos);
        }


        /// <summary>
        /// Returns Employee details by inputting the id
        /// </summary>
        // GET: api/EmployeeData/FindEmployee/5
        [ResponseType(typeof(Employee))]
        [HttpGet]
        public IHttpActionResult FindEmployee(int id)
        {
            Employee employee = db.Employees.Find(id);

            if (employee == null)
            {
                return NotFound();
            }

            EmployeeDto employeedto = new EmployeeDto()
            {
                EmployeeId = employee.EmployeeId,
                Name = employee.Name,
                Bio = employee.Bio,
              
            };

            return Ok(employee);
        }


        /// <summary>
        ///Employee information is updated
        /// </summary>
  
        [ResponseType(typeof(void))]
        [HttpPost]
       
        public IHttpActionResult UpdateEmployee(int id, Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.EmployeeId)
            {
                return BadRequest();
            }

            db.Entry(employee).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Employees and the service they provide are displayed
        /// </summary>
     
        [Route("api/EmployeeData/linkservicewithemployee/{employeeId}/{Id}")]
        [HttpPost]
        public IHttpActionResult LinkServiceWithEmployee(int employeeId, int Id)
        {
            Employee employee = db.Employees.Include(x => x.SpaServices).Where(y => y.EmployeeId == employeeId).FirstOrDefault();
            SpaService spaservice = db.SpaServices.Find(Id);

            if (employee == null || spaservice == null)
            {
                return NotFound();
            }

            employee.SpaServices.Add(spaservice);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Adds an employee to the system
        /// </summary>
   
        [ResponseType(typeof(Employee))]
        [HttpPost]
        public IHttpActionResult AddEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Employees.Add(employee);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = employee.EmployeeId }, employee);
        }

        /// <summary>
        /// Deletes an employee from the system by the employee id.
        /// </summary>
     
        [HttpPost]
        [ResponseType(typeof(Employee))]
  
        public IHttpActionResult DeleteEmployee(int id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            db.Employees.Remove(employee);
            db.SaveChanges();

            return Ok(employee);
        }

     
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeExists(int id)
        {
            return db.Employees.Count(e => e.EmployeeId == id) > 0;
        }
    }
}

