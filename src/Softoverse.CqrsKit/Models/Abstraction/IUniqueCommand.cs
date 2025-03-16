namespace Softoverse.CqrsKit.Models.Abstraction;

public interface IUniqueCommand
{
    string GetUniqueIdentification();
}