using API.Models;
using API.ServiceErrors;
using ErrorOr;

namespace API.Services.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private static readonly string SelectQuery = @"select * from employees where firstName = @FirstName and lastName = @LastName and birthDate = @BirthDate and startWorkDate = @StartWorkDate and Position = @Position;";
        private readonly string InsertQuery = @"insert into employees(firstName, lastName, birthDate, startWorkDate, Position) values(@FirstName, @LastName, @BirthDate, @StartWorkDate, @Position)";
        private readonly string GetAllQuery = @"select * from employees";
        private readonly string GetOneQuery = @"select * from employees where id = @ID";
        private readonly string DeleteQuery = @"delete from employees where id = @Id";
        private readonly string UpdateQuery = @"update employees set Position = @Position where id = @Id";

        public void CreateEmployee(Employee data)
        {
            DataAccess.InsertData(InsertQuery, data);
        }
        public static int GetEmployeeId(Employee data)
        {
            return DataAccess.LoadData(SelectQuery, data).First().Id;
        }

        public ErrorOr<Deleted> DeleteEmployee(int id)
        {
            var rowsDeleted = DataAccess.UpdateData(DeleteQuery, new { Id = id });
            if (rowsDeleted == 0)
                return Errors.Employee.NothingChanged;
            return Result.Deleted;
        }

        public ErrorOr<List<Employee>> GetEmployee(int id)
        {
            List<Employee> response;
            if (id == 0)
                response = DataAccess.LoadData<Employee>(GetAllQuery, null);
            else
                response = DataAccess.GetDataById<Employee>(GetOneQuery, id);
            if(response.Count > 0)
                return response;
            return Errors.Employee.NotFound;
        }

        public ErrorOr<Updated> UpdateEmployee(UpdateEmployeeRequest data)
        {
            if (data.Position.Length is < 1 or > 50)
                return Errors.Employee.InvalidInput;
            var rowsDeleted =  DataAccess.UpdateData(UpdateQuery, data);
            if (rowsDeleted == 0)
                return Errors.Employee.NothingChanged;
            return Result.Updated;
        }
    }
}
