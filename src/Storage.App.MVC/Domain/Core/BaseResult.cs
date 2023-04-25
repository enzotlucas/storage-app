namespace Storage.App.MVC.Domain.Core
{
    public class BaseResult
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }

        public BaseResult AsSuccess()
        {
            Success = true;
            Errors = default;
            return this;
        }

        public BaseResult AsError(List<string> errors)
        {
            Success = false;
            Errors = errors;
            return this;
        }

        public BaseResult AsError(string error)
        {
            Success = false;
            Errors = new List<string> { error };
            return this;
        }
    }
}
