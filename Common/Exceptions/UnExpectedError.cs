using FluentResults;

namespace Common.Exceptions
{
    public class UnExpectedError : Error
    {
        public UnExpectedError(string message)
            : base(message)
        {
        }
    }
}
