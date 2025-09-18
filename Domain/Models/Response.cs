namespace ReelfyAPI.Models;

public class Response<T>(T? data, string? message = null, int statusCode = 200)
{
    public T? Data { get; set; } = data;
    public string? Message { get; set; } = message;
    public bool Success => this.StatusCode >= 200 && this.StatusCode <= 299;
    public int StatusCode { get; set; } = statusCode;
}
