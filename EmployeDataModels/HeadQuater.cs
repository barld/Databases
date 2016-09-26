namespace EmployeDataModels
{
    public class HeadQuater
    {
        /// <summary>
        /// pk
        /// </summary>
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
    }
}