using SalesManagementAPI.Core.Responses;

namespace SalesManagementAPI.Business.Services
{
    public abstract class BaseManager 
    {
        protected void AddError(BaseResponse response,Action<ErrorOptions> configure)
        {
            var options = new ErrorOptions();
            configure(options);
            response.Errors.Add(new ApiError
            {
                ErrorCode=options.ErrorCode,
                ErrorMessage=options.ErrorMessage,
                PropertyName=options.PropertyName,
                AttemptedValue=options.AttemptedValue,
            });
        }
        protected bool HasError(BaseResponse response) 
        { 
            return response.Errors.Any();
        
        }

    }
}
