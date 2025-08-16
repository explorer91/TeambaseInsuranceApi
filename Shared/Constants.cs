using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ResponseMessages
    {
        public static string invalidData = "Data validation failed";
        public static string success = "Success";


        public static string employeNotFound = "Employee not found.";
        public static string employeInserted = "Employee inserted successfully.";
        public static string employeUpdated = "Employee updated successfully.";
        public static string employeDeleted = "Employee deleted successfully.";
        public static string employeCreationFailed = "Failed to create Employee.";
        public static string employeUpdateFailed = "Failed to update Employee.";
        public static string employeBulkStatusUpdateSuccess = "Employee status updated successfully.";
        public static string employeBulkStatusUpdateFailed = "Failed to update Employee status.";

    }
}
