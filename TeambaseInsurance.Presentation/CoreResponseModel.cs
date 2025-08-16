using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeambaseInsurance.Presentation
{
    public class CoreResponseModel
    {
        public object GetSuccessResponse(string? message, object? data)
        {
            return new
            {
                message = message,
                data = data,
                status = 200,
                success = true,
            };
        }

        public object getFailResponse(string? message, object? data)
        {
            return new
            {
                message = message,
                data = data,
                status = 400,
                success = false,
            };
        }

    }

}
