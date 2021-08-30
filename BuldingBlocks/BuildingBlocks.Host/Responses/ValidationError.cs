namespace BuildingBlocks.Web.Responses
{
    /// <summary>
    /// Represent validation details
    /// </summary>
    public class ValidationError
    {
        public string Field { get; }

        public string Message { get; }

        public ValidationError(string field, string message)
        {
            Field = !string.IsNullOrEmpty(field) ? field : null;
            Message = message;
        }
    }
}