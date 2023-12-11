using API.Models;
using ErrorOr;

namespace API.Services.Employees
{
    public interface IEmployeeService
    {
        void CreateEmployee(Employee createRequest);
        ErrorOr<Updated> UpdateEmployee(UpdateEmployeeRequest updateEmployeeRequest);
        ErrorOr<Deleted> DeleteEmployee(int id);
        ErrorOr<List<Employee>> GetEmployee(int id = 0);
    }
}
