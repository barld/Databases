namespace EmployeDataModels
{
    public class Project
    {
        public int ProjectID { get; set; }
        public string Name { get; set; }
        public float Budget { get; set; }
        public int Hours { get; set; }

        public string BuildingName { get; set; }

        public HeadQuater HeadQuater { get; set; }
    }
}