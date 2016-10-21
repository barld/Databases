using System;
using EmployeDataModels;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class CreateHeadQuarters
    {
        public CreateHeadQuarters() { }

        public CreateHeadQuarters(HeadQuater headquarter)
        {
            this.BuildingName = headquarter.BuildingName;
            this.Rooms = headquarter.Rooms;
            this.Rent = headquarter.Rent;
            this.PostCode = headquarter.PostCode;
            this.HouseNumber = headquarter.HouseNumber;
            this.Country = headquarter.Country;
            this.Address = new Address();
        }

        public string BuildingName { get; set; }

        /// <summary>
        /// count of rooms in the building
        /// </summary>
        public int Rooms { get; set; }
        public float Rent { get; set; }

        /// <summary>
        /// fk: address
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// fk: address
        /// </summary>
        public string PostCode { get; set; }
        /// <summary>
        /// fk: address
        /// </summary>
        public string HouseNumber { get; set; }

        public Address Address { get; set; }


        internal HeadQuater ToHeadquarter()
        {
            var hq = new HeadQuater
            {
                BuildingName = BuildingName,
                Rooms = Rooms,
                Rent = Rent,
                Country = Country,
                PostCode = PostCode,
                HouseNumber = HouseNumber
            };
            return hq;
        }
    }
}