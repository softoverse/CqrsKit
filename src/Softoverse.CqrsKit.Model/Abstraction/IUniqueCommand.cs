namespace Softoverse.CqrsKit.Model.Abstraction;

public interface IUniqueCommand
{
    string GetUniqueIdentification();
}