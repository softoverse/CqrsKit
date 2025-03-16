namespace Softoverse.CqrsKit.Models.Utility;

public class CqrsError
{
    public string Key { get; set; }
    public string Message { get; set; }
    public IDictionary<string, string[]> Errors { get; set; }

    public static CqrsError Create(string key, params string[] messages)
    {
        return new CqrsError
        {
            Key = key,
            Message = messages.FirstOrDefault() ?? string.Empty,
            Errors = new Dictionary<string, string[]>
            {
                {
                    key, messages
                }
            }
        };
    }

    public CqrsError AddError(string key, params string[] messages)
    {
        Errors.Add(key, messages);
        return this;
    }
}