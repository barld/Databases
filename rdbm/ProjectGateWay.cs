﻿using EmployeDataModels;
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

        public ProjectGateWay(MDFConnection connection)
        {
            this.con = connection;
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
                cmd.Parameters.Add("@id", SqlDbType.Int);
                cmd.Parameters["@id"].Value = project.ProjectID;

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
    }
}