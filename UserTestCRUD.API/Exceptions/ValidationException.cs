using FluentValidation.Results;
using UserTestCRUD.DAL.Result;

namespace UserTestCRUD.API.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(IEnumerable<ValidationFailure> failures)
            : base("One or more validation failures has occurred.") =>
            Errors = failures
                .Distinct()
                .Select(failure => new Error(failure.ErrorCode, failure.ErrorMessage))
                .ToArray();

        public IReadOnlyCollection<Error> Errors { get; }
    }
}
