using rdbm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeDataModels;

namespace ConsoleApplication
{
    class Program
    {

        static void Main(string[] args)
        {

            var db = new rdbm.DataContext();

            var test = db.Query<Employee>("Select * From [Employe]").GetEnumerator();





            var _connection = new MDFConnection();
            var _gateway = new EmployeeGateWay(_connection);


            //var _adr = new Address();

            //_adr.City = "Maassluis";
            //_adr.Country = "Netherlands";
            //_adr.HouseNumber = "12";
            //_adr.PostCode = "1234AA";
            //_adr.Street = "lalastraat";
            //_gateway.addADR(_adr);

            //var _hq = new EmployeDataModels.HeadQuater();
            //_hq.BuildingName = "Maassluis";
            //_hq.Rooms = 10;
            //_hq.Rent = 500;
            //_hq.Country = "Netherlands";
            //_hq.PostCode = "1234AA";
            //_hq.HouseNumber = "12";
            //_gateway.addHQ(_hq);

            //var _employee = new EmployeDataModels.Employee();

            //_employee.BSN = 1234567;
            //_employee.Name = "Daan";
            //_employee.SurName = "Grashoff";
            //_employee.BuildingName = _hq.BuildingName;

            //_gateway.add(_employee);
            var Employees = _gateway.GetAll();
        }
    }
}
