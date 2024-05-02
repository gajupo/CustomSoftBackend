using FluentResults;
namespace Common.Exceptions
{
    public class DatabaseError : Error
    {
        public DatabaseError(string message)
            : base(message)
        {
        }
    }
}
