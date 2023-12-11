using ErrorOr;

namespace API.ServiceErrors
{
    public static class Errors
    {
        public static class Employee
        {
            public static Error InvalidDate => Error.Validation(
                code: "Employee.InvalidDate",
                description: "Input date values can't be converted");
            public static Error NothingChanged => Error.Failure(
                code: "Employee.NoResult",
                description: "No records were changed. Check if input Id.");
            public static Error InvalidInput => Error.Validation(
                code: "Employee.InvalidInput",
                description: $"First, Last names and Position must be at least {Models.Employee.minStringLength} and at most {Models.Employee.maxStringLength} characters long."); 
            public static Error NotFound => Error.NotFound(
                code:"Employee.NotFound",
                description:"Employee with given Id was not found");
        }
    }
}
