namespace dotnet_rpg.Models
{
    // TODO: Remove - should not be necessary anymore
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
