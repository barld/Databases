using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeDataModels
{
    public class HeadQuater
    {
        /// <summary>
        /// pk
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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

    }
}