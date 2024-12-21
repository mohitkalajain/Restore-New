using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helper.Constants
{
    public static class MessageConstants
    {
         // Success Messages
    public const string Success = "Operation completed successfully.";
    public const string DataRetrieved = "Data retrieved successfully.";
    public const string DataSaved = "Data saved successfully.";
    public const string DataUpdated = "Data updated successfully.";
    public const string DataDeleted = "Data deleted successfully.";
    public const string UserCreated = "User account created successfully.";

    // Error Messages
    public const string Error = "An error occurred while processing data";
    public const string InvalidValue = "Invalid parameter value";
    public const string NotFound = "The requested resource was not found.";
    public const string ValidationFailed = "Validation failed for the provided data.";
    public const string UnauthorizedAccess = "You do not have permission to perform this action.";
    public const string InternalServerError = "An internal server error occurred.";
    public const string DatabaseError = "A database error occurred.";
    public const string TimeoutError = "The request timed out. Please try again.";

    // Warning or Info Messages
    public const string NoDataFound = "No data available.";
    public const string ActionNotAllowed = "This action is not allowed.";
    public const string TokenExpired = "Your session token has expired.";
    }
}