namespace BuildInBlocks.CQRS.Atributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ExceptionStatusCodeAttribute : Attribute
    {
        public int StatusCode { get; }
        public ExceptionStatusCodeAttribute(int statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
