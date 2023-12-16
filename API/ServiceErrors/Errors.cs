﻿using API.Models;
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
            public static Error DuplicateName => Error.Validation(
                code: "SupportDirection.DuplicateName",
                description: "Name must be unique within the table");
            public static Error InvalidName => Error.Validation(
                code: "SupportDirection.InvalidName",
                description: $"Name must be at least {Models.SupportDirection.minNameLength} and at most {Models.SupportDirection.maxNameLength} characters long");
            public static Error InvalidDescription => Error.Validation(
                code: "SupportDirection.InvalidDescription",
                description: $"Description must be at most {Models.SupportDirection.maxDescLength} characters long");
            public static Error InvalidId => Error.Validation(
                code: "SupportDirection.InvalidId",
                description: "Input Id value is invalid");
        }
        public static class Project
        {
            public static Error MissingValues => Error.Validation(
                code: "Projec.MissingValues",
                description: "1 or more required values in body are missing");
            public static Error DuplicateName => Error.Validation(
                code: "Project.DuplicateName",
                description: "Name must be unique within the table");
            public static Error InvalidName => Error.Validation(
                code: "Project.InvalidName",
                description: $"Project name must be at least {Models.Project.minStringLength} and at most {Models.Project.maxNameLength} characters long");
            public static Error InvalidLink => Error.Validation(
                code: "Project.InvalidLink",
                description: $"Project link must be at most {Models.Project.maxLinkLength} characters long");
            public static Error InvalidPrice => Error.Validation(
                code: "Project.InvalidPrice",
                description: "Project price mustn't be less than 0");
        }

        public static class Report
        {
            public static Error InvalidLink => Error.Validation(
                code: "Report.InvalidInput",
                description: $"BuyingRecordsLink and RecieverReportLink must be at least {Models.Report.minStringLength} and at most {Models.Report.maxStringLength} characters long.(RecieverReportLink can be null)");
            public static Error InvalidProjectId => Error.Validation(
                code: "Report.InvalidInput",
                description: "Input projectId doesn't exist");
            public static Error MissingDate => Error.Validation(
                code: "Report.InvalidDate",
                description: "DateFulfilled field is invalid or missing");
        }

        public static class General
        {
            public static Error TableEmpty => Error.NotFound(
                code: "DB.TableEmpty",
                description: "Searched table has no records in it");
            public static Error NoResult => Error.Failure(
               code: "DB.NoResult",
               description: "No records were changed. Check input Id.");
            public static Error NotFound => Error.NotFound(
                code: "DB.NotFound",
                description: "No results were found for given Id");
            public static Error InvalidId => Error.Validation(
                code: "General.InvalidId",
                description: "Id must be a positive integer");
        }
    }
}
