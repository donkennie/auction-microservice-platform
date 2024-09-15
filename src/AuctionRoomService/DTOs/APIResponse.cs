using System.Net;

namespace AuctionRoomService.DTOs
{
        public class APIResponse
        {
            public HttpStatusCode StatusCode { get; set; }
            public string Message { get; set; }
            public bool IsSuccessful { get; set; }
            public object Data { get; set; }

            public static APIResponse GetFailureMessage(HttpStatusCode statusCode, object data, string msg)
            {
                var failedResponse = new APIResponse()
                {
                    StatusCode = statusCode,
                    Data = data,
                    Message = msg,
                    IsSuccessful = false
                };

                return failedResponse;
            }

            public static APIResponse GetSuccessMessage(HttpStatusCode statusCode, object data, string msg)
            {
                var successResponse = new APIResponse()
                {
                    StatusCode = statusCode,
                    Data = data,
                    Message = msg,
                    IsSuccessful = true
                };

                return successResponse;
            }
        }
}
