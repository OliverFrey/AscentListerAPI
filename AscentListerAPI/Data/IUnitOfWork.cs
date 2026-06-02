namespace AscentListerAPI.Data;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}
