namespace API.Services.Employees
{
    public interface IEmployeeService
    {
        EmployeeResponse CreateEmployee(EmployeeResponse employeeResponse);
        void UpdateEmployee(UpdateEmployeeRequest updateEmployeeRequest);
        void DeleteEmployee(int id);
        List<EmployeeResponse> GetEmployee(int id = 0);
    }
}
