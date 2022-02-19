using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlantMLM
{
    public class BMSEmployee
    {
        public int ID { get; set; }
        public int Active { get; set; }
        public int Company { get; set; }
        public DateTime CreateDate { get; set; }
        public string Department { get; set; }
        public int DepartmentID { get; set; }
        public DateTime DepartureDate { get; set; }
        public int DutyID { get; set; }
        public string Email { get; set; }
        public int Grad { get; set; }
        public string GradName { get; set; }
        public DateTime LastOnLineTime { get; set; }
        public string LoginID { get; set; }
        public string LoginName { get; set; }
        public int Manager { get; set; }
        public string Name { get; set; }
        public int Online { get; set; }
        public DateTime OnLineTime { get; set; }
        public int OnShift { get; set; }
        public string Operator { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public long PhoneMAC { get; set; }
        public int Position { get; set; }
        public string WeiXin { get; set; }
        public int Type { get; set; }
        public int SuperiorID { get; set; }
        public String ActiveText { get; set; }
        public string PartPower { get; set; }
        public string PartPowerName { get; set; }
    }
}
