namespace ReelfyAPI.Models;

public class Response<T>
{
    public T? Data { get; set; }
    public string? Message { get; set; }
    public bool Success => this.StatusCode >= 200 && this.StatusCode <= 299;
    public int StatusCode { get; set; }

    public Response(T? data, string? message = null, int statusCode = 200)
    {
        Data = data;
        Message = message;
        StatusCode = statusCode;
    }
}
