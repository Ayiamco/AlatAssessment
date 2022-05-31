using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlatAssessment.DTOs
{
    public class APIResponse
    {
        public APIResponse(string respCode, string respDescription)
        {
            this.ResponseCode = respCode;
            this.ResponseDescription = respDescription;
        }

        public APIResponse(){}

        [Required]
        public string ResponseCode { get; set; }

        [Required]
        public string ResponseDescription { get; set; }

    }

    public class APIResponse<T> : APIResponse where T : class
    {
        public APIResponse(string respCode, string respDescription, T data)
        {
            this.ResponseCode = respCode;
            this.ResponseDescription = respDescription;
            this.Data = data;
        }
        public T Data { get; set; }
    }

    public class ErrorAPIResponse : APIResponse
    {
        public IDictionary<string, string[]> Errors { get; set; }
    }

    public class ResponseCodes
    {
        public const string Success = "200";
        public const string ClientFailure = "400";
        public const string ServerError = "500";
    }

    public class ServiceResp
    {
        public ServiceResp(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public ServiceResp()
        {
                
        }

        public string Code { get; set; }

        public string Description { get; set; }
    }


}
