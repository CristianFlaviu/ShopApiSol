namespace ShopApi.Core
{
    public interface ICommandResult
    {
        string ErrorMessage { get; }
        bool IsSuccess { get; }
    }

    public class CommandResult<T> : ICommandResult
    {
        public T Payload { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsSuccess => ErrorMessage == null;

        public static CommandResult<T> Fail(string message)
        {
            return new CommandResult<T> { ErrorMessage = message };
        }

        public static CommandResult<T> Fail(string[] messages)
        {
            string finalMessage = "";
            foreach (var mes in messages)
            {
                finalMessage = finalMessage + "\n" + mes;
            }
            return new CommandResult<T> { ErrorMessage = finalMessage };
        }

        public static CommandResult<T> Success(T payload)
        {
            return new CommandResult<T> { Payload = payload };
        }
    }
}
