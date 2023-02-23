using PassionProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace PassionProject.Controllers
{
    public class AppointmentDataController : ApiController
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public IEnumerable<AppointmentDto> ListAppointments()
        {
            List<Appointment> appointments = db.Appointments.ToList();
            List<AppointmentDto> appointmentDtos = new List<AppointmentDto>();

            appointments.ForEach(a => appointmentDtos.Add(new AppointmentDto()
            {
                AppointmentId = a.AppointmentId,
                Id = a.Id,
                CustomerName = a.CustomerName,
                CustomerEmail = a.CustomerEmail,
                EmployeeId = a.EmployeeId,
                AppointmentDateTime = a.AppointmentDateTime,
            }));

            return appointmentDtos;
        }

        /// <summary>
        /// Returns details of the appointment by appointment id
        /// </summary>
        /*
        [ResponseType(typeof(Appointment))]
        [HttpGet]
        public IHttpActionResult FindAppointmentsByEmployee(int id)
        {
            List<Appointment> appointments = db.Appointments.Include(a => a.Id).Where(a => a.EmployeeId == id).ToList();
            List<AppointmentDto> appointmentDtos = new List<AppointmentDto>();

            appointments.ForEach(a => appointmentDtos.Add(new AppointmentDto()
            {
                AppointmentId = a.AppointmentId,
                Id = a.Id,
                CustomerName = a.CustomerName,
                CustomerEmail = a.CustomerEmail,
                EmployeeId = a.EmployeeId,
                AppointmentDateTime = a.AppointmentDateTime,
                
            }));

            return Ok(appointmentDtos);
        }
        */


        /// <summary>
        /// Adds an appointment to the system
        /// </summary>
      
        [ResponseType(typeof(Appointment))]
        [HttpPost]
        public IHttpActionResult AddAppointment(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Appointments.Add(appointment);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { AId = appointment.AppointmentId }, appointment);
        }


        /// <summary>
        /// Deletes an appointment from the system by it's ID.
        /// </summary>
  
        [ResponseType(typeof(Appointment))]
        [HttpPost]
        public IHttpActionResult DeleteAppointment(int AId)
        {
            Appointment appointment = db.Appointments.Find(AId);
            if (appointment == null)
            {
                return NotFound();
            }

            db.Appointments.Remove(appointment);
            db.SaveChanges();

            return Ok(appointment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppointmentExists(int AId)
        {
            return db.Appointments.Count(e => e.AppointmentId == AId) > 0;
        }
    }
}
