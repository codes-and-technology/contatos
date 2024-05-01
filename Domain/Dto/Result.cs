namespace RegionalContacts.Core.Dto;

public class Result<T> where T :class
{
    public bool Success { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
    public T Object { get; set; }
}
