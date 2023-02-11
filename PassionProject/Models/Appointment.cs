using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PassionProject.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        [ForeignKey("SpaService")]
        public int Id { get; set; }
        public virtual SpaService SpaService { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        public DateTime AppointmentDateTime { get; set; }

    }

    public class AppointmentDto
    {
        public int AppointmentId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime AppointmentDateTime { get; set; }

    }
}