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

            Degrees = employee.Degrees.ToList();
            Degrees.Add(new Degree());
            Degrees.Add(new Degree());

            Positions = employee.Positions.ToList();
            Positions.Add(new Position());
            Positions.Add(new Position());

        }

        public int BSN { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        /// <summary>
        /// fk to HeadQuater
        /// </summary>
        public string BuildingName { get; set; }

        public List<EmployeeAddress> Adresses { get; set; }
        public List<Degree> Degrees { get; set; }
        public List<Position> Positions { get; set; }

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
            });
            Degrees.ForEach(d =>
            {
                d.BSN = BSN;
            });
            Positions.ForEach(p =>
            {
                p.BSN = BSN;
            });

            emp.Adresses = Adresses
                .Where(a => 
                    !string.IsNullOrWhiteSpace(a.PostCode) && 
                    !string.IsNullOrWhiteSpace(a.Country) && 
                    !string.IsNullOrWhiteSpace(a.HouseNumber));
            emp.Degrees = Degrees
                .Where(d =>
                !string.IsNullOrWhiteSpace(d.Course) &&
                !string.IsNullOrWhiteSpace(d.Level) &&
                !string.IsNullOrWhiteSpace(d.School));
            emp.Positions = Positions.Where(p => !string.IsNullOrWhiteSpace(p.PositionName));

            return emp;
        }
    }
}