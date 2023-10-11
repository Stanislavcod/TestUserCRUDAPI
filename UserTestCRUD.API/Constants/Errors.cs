using UserTestCRUD.DAL.Result;

namespace UserTestCRUD.API.Constants
{
    public static class Errors
    {
        public static Error UnProcessableRequest => new Error(
            "API.UnProcessableRequest",
            "The server could not process the request.");

        public static Error ServerError => new Error("API.ServerError", "The server encountered an unrecoverable error.");
    }
}
