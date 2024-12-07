namespace WebApiBlock.Responses
{
    public class DefaultResponse<TData> (TData data) where TData : notnull
    {
        public TData Data { get; set; } = data;
        public bool Success { get; set; } = true;
        public List<string>? Errors { get; set; } = null;
    }
}
