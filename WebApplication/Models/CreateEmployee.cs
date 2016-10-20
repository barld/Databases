using EmployeDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class CreateEmployee
    {
        public CreateEmployee() { }

        public CreateEmployee(Employee employee)
        {
            BSN = employee.BSN;
            Name = employee.Name;
            SurName = employee.SurName;
            BuildingName = employee.BuildingName;
            Adresses = employee.Adresses.ToList();
            Adresses.Add(new EmployeeAddress { Address = new Address() });
            Adresses.Add(new EmployeeAddress { Address = new Address() });
        }

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
            emp.Adresses = Adresses
                .Where(a => 
                    !string.IsNullOrWhiteSpace(a.PostCode) && 
                    !string.IsNullOrWhiteSpace(a.Country) && 
                    !string.IsNullOrWhiteSpace(a.HouseNumber));

            return emp;
        }
    }
}