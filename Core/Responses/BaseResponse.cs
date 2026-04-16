
namespace SalesManagementAPI.Core.Responses
{
    public class BaseResponse
    {
        public bool IsSuccess => !Errors.Any();
        public List<ApiError> Errors { get; set; } = new(); 
    }
}
