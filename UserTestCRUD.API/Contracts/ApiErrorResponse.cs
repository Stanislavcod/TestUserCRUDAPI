using UserTestCRUD.DAL.Result;

namespace UserTestCRUD.API.Contracts
{
    public class ApiErrorResponse
    {
        public ApiErrorResponse(IReadOnlyCollection<Error> errors) => Errors = errors;
        public IReadOnlyCollection<Error> Errors { get; }
    }
}
