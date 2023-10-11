namespace UserTestCRUD.DAL.Result
{
    public class Error
    {
        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public string Code { get; }
        public string Message { get; }

        public static Error None => new Error(string.Empty, string.Empty);

        public static implicit operator string(Error error) => error?.Code ?? string.Empty;

        public IEnumerable<object> GetAtomicValues()
        {
            yield return Code;
            yield return Message;
        }
    }
}
