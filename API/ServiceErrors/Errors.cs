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
           
            public static Error InvalidInput => Error.Validation(
                code: "Employee.InvalidInput",
                description: $"First, Last names and Position must be at least {Models.Employee.minStringLength} and at most {Models.Employee.maxStringLength} characters long."); 
        }
        public static class SupportDirection
        {
            public static Error InvalidName => Error.Validation(
                code: "SupportDirection.InvalidName",
                description: $"Name must be at least {Models.SupportDirection.minNameLength} and at most {Models.SupportDirection.maxNameLength} characters long");
            public static Error InvalidDescription => Error.Validation(
                code: "SupportDirection.InvalidDescription",
                description: $"Description must be at most {Models.SupportDirection.maxDescLength} characters long");
            public static Error InvalidId => Error.Validation(
                code: "SupportDirection.Invalidid",
                description: "Input Id value is invalid");
        }
        public static class General
        {
            public static Error TableEmpty => Error.NotFound(
                code: "DB.TableEmpty",
                description: "Searched table has no records in it");
            public static Error NoResult => Error.Unexpected(
               code: "DB.NoResult",
               description: "No records were changed. Check input Id.");
            public static Error NotFound => Error.NotFound(
                code: "DB.NotFound",
                description: "No results were found for given Id");
        }
    }
}
