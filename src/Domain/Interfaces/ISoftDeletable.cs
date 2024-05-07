namespace Domain.Interfaces;

public interface ISoftDeletable
{
    bool IsActive { get; set; }
}