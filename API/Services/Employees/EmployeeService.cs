namespace API.Services.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly string InsertQuery = @"insert into employees(firstName, lastName, birthDate, startWorkDate, Position) values(@FirstName, @LastName, @BirthDate, @StartWorkDate, @Position)";
        private readonly string SelectQuery = @"select * from employees where firstName = @FirstName and lastName = @LastName and birthDate = @BirthDate and startWorkDate = @StartWorkDate and Position = @Position;";
        private readonly string GetAllQuery = @"select * from employees";
        private readonly string GetOneQuery = @"select * from employees where id = @ID";
        private readonly string DeleteQuery = @"delete from employees where id = @Id";
        private readonly string UpdateQuery = @"update employees set Position = @Position where id = @Id";

        public EmployeeResponse CreateEmployee(EmployeeResponse employeeResponse)
        {
            DataAccess.InsertData(InsertQuery, employeeResponse);
            return DataAccess.LoadData(SelectQuery, employeeResponse).First();
        }

        public int DeleteEmployee(int id)
        {
            return DataAccess.UpdateData(DeleteQuery, new { Id = id });
        }

        public List<EmployeeResponse> GetEmployee(int id)
        {
            if (id == 0)
                return DataAccess.LoadData<EmployeeResponse>(GetAllQuery, null);
            return DataAccess.GetDataById<EmployeeResponse>(GetOneQuery, id);
        }

        public void UpdateEmployee(UpdateEmployeeRequest updateEmployeeRequest)
        {
            DataAccess.UpdateData(UpdateQuery, updateEmployeeRequest);
        }
    }
}
