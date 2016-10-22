using EmployeDataModels;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System;
using System.Data.SqlTypes;

namespace rdbm
{
    public class ProjectGateWay
    {
        private readonly MDFConnection con;
        private readonly IContext context;

        public ProjectGateWay(MDFConnection connection, IContext context)
        {
            this.con = connection;
            this.context = context;

        }

        public IEnumerable<Project> GetAll()
        {
            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();
            var sql = @"SELECT * FROM [Project];";
            using (var cmd = new SqlCommand(sql, con.connection))
            {
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        yield return map(reader);
            }
        }

        private Project map(SqlDataReader reader)
        {
            return new Project
            {
                ProjectID = reader.GetInt32(0),
                Name = reader.GetString(1),
                Budget = (float)reader.GetSqlMoney(2).ToDouble(),
                Hours = reader.GetInt32(3),
                BuildingName = reader.GetString(4)
            };
        }

        public bool CanPay(Project project)
        {
            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();

            var sql = @"SELECT 
	            [Project].*, 
	             Rent.RentPerProject / [project].Budget AS RentPercentage
            FROM 
	            [Project],
	            (
		            SELECT [HeadQuater].BuildingName, [HeadQuater].Rent/Count(*) AS RentPerProject
		            FROM [Project], [HeadQuater]
		            WHERE [Project].BuildingName = [HeadQuater].BuildingName
		            GROUP BY [HeadQuater].BuildingName, [HeadQuater].Rent
	            ) AS Rent
            WHERE [Project].BuildingName = Rent.BuildingName AND [Project].ProjectID = @projectid;";

            decimal rentpercentage = 0;
            using (var cmd = new SqlCommand(sql, con.connection))
            {
                cmd.Parameters.Add("@projectid", SqlDbType.Int);
                cmd.Parameters["@projectid"].Value = project.ProjectID;
                using (var reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                        return (float)reader.GetDecimal(5) * 100 < project.Budget;
                }

            }
            return false;
        }

        public void Add(Project project)
        {
            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();
            var sql = @"INSERT INTO [Project](Name, Budget, Hours, BuildingName) 
                VALUES(@name, @budget, @hours, @bName);";
            using (var cmd = new SqlCommand(sql, con.connection))
            {
                cmd.Parameters.Add("@name", SqlDbType.VarChar);
                cmd.Parameters["@name"].Value = project.Name;
                cmd.Parameters.Add("@budget", SqlDbType.Money);
                cmd.Parameters["@budget"].Value = new SqlMoney((double)project.Budget);
                cmd.Parameters.Add("@hours", SqlDbType.Int);
                cmd.Parameters["@hours"].Value = project.Hours;
                cmd.Parameters.Add("@bName", SqlDbType.VarChar);
                cmd.Parameters["@bName"].Value = project.BuildingName;

                cmd.ExecuteNonQuery();
            }
        }

        public Project FindByProjectId(int id)
        {
            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();
            var sql = @"SELECT TOP 1 * FROM [Project] WHERE ProjectID = @id;";
            using (var cmd = new SqlCommand(sql, con.connection))
            {
                cmd.Parameters.Add("@id", SqlDbType.Int);
                cmd.Parameters["@id"].Value = id;

                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    return map(reader);
                }
            }
        }

        public void Update(Project project)
        {
            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();
            var sql = @"UPDATE [Project] SET Name = @name, Budget = @budget, Hours=@hours, BuildingName=@bName
                WHERE ProjectID = @pid;";
            using (var cmd = new SqlCommand(sql, con.connection))
            {
                cmd.Parameters.Add("@name", SqlDbType.VarChar);
                cmd.Parameters["@name"].Value = project.Name;
                cmd.Parameters.Add("@budget", SqlDbType.Money);
                cmd.Parameters["@budget"].Value = new SqlMoney((double)project.Budget);
                cmd.Parameters.Add("@hours", SqlDbType.Int);
                cmd.Parameters["@hours"].Value = project.Hours;
                cmd.Parameters.Add("@bName", SqlDbType.VarChar);
                cmd.Parameters["@bName"].Value = project.BuildingName;
                cmd.Parameters.Add("@pid", SqlDbType.Int);
                cmd.Parameters["@pid"].Value = project.ProjectID;

                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteById(int id)
        {
            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();
            var sql = @"DELETE FROM Project WHERE ProjectID = @id;";
            using (var cmd = new SqlCommand(sql, con.connection))
            {
                cmd.Parameters.Add("@id", SqlDbType.Int);
                cmd.Parameters["@id"].Value = id;

                cmd.ExecuteNonQuery();
            }
        }

        //costom handlers for project managment

        public void UpdateState(ProjectManagment pm)
        {
            // 1. drop all ProjectPositions for project
            // 2. INSERT all project positions for project
            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();

            const string dropSql = @"DELETE FROM [ProjectPosition] WHERE ProjectID = @pid;";
            using (var cmd = new SqlCommand(dropSql, con.connection))
            {
                cmd.Parameters.Add("@pid", SqlDbType.Int);
                cmd.Parameters["@pid"].Value = pm.ProjectID;

                cmd.ExecuteNonQuery();
            }
            foreach(var d in pm.Data)
            {
                if (!d.Assignd)
                    continue;

                const string sql = @"INSERT INTO [ProjectPosition] VALUES (@pname, @BSN, @pid);";
                using(var cmd = new SqlCommand(sql, con.connection))
                {
                    cmd.Parameters.Add("@pname", SqlDbType.VarChar);
                    cmd.Parameters["@pname"].Value = d.Position.PositionName;
                    cmd.Parameters.Add("@BSN", SqlDbType.Int);
                    cmd.Parameters["@BSN"].Value = d.Employee.BSN;
                    cmd.Parameters.Add("@pid", SqlDbType.Int);
                    cmd.Parameters["@pid"].Value = pm.ProjectID;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        
        public IEnumerable<ManagmentData> GetManageDataProject(int id)
        {
            Func<SqlDataReader, ManagmentData> map = reader =>
             {
                 return new ManagmentData
                 {
                     Employee = new Employee { BSN = reader.GetInt32(0), Name = reader.GetString(1), SurName = reader.GetString(2), BuildingName = reader.GetString(3)},
                     Position = new Position { PositionName = reader.GetString(4), Description=reader.GetString(5), HourFee = (float)reader.GetSqlMoney(6).ToDouble(), BSN = reader.GetInt32(7)},
                     Assignd = reader.GetInt32(8)==1
                 };
             };

            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();
            var sql = @"
SELECT 
	*, 
	(
        SELECT COUNT(*) FROM [ProjectPosition] 
        WHERE [ProjectPosition].PositionName = [Position].PositionName AND [ProjectPosition].BSN = [Position].BSN AND [ProjectPosition].ProjectID = @id
    )
    as selected
FROM [Employee], [Position]
WHERE Position.BSN = Employee.BSN;";
            using (var cmd = new SqlCommand(sql, con.connection))
            {
                cmd.Parameters.Add("@id", SqlDbType.Int);
                cmd.Parameters["@id"].Value = id;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        yield return map(reader);
                }

            }
        }
    }

    public class ProjectManagment : Project
    {
        public ProjectManagment()
        {
            Data = new List<ManagmentData>();
        }
        public ProjectManagment(IEnumerable<ManagmentData> data)
        {
            Data = data.ToList();
        }
        public List<ManagmentData> Data { get; set; }
    }

    public class ManagmentData
    {
        public Employee Employee { get; set; }
        public Position Position { get; set; }
        public bool Assignd { get; set; }
    }
}