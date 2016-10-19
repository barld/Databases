using EmployeDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class CreateEmployee
    {
        public int BSN { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        /// <summary>
        /// fk to HeadQuater
        /// </summary>
        public string BuildingName { get; set; }

        public List<EmployeeAddress> Adresses { get; set; }

        internal Employee toEmployee()
        {
            var emp = new Employee
            {
                BSN = BSN,
                Name = Name,
                SurName = SurName,
                BuildingName = BuildingName
            };
            Adresses.ForEach(a =>
            {
                a.Country = a.Address.Country;
                a.PostCode = a.Address.PostCode;
                a.HouseNumber = a.Address.HouseNumber;
            }
            );
            emp.Adresses = Adresses.AsEnumerable();

            return emp;
        }
    }
}