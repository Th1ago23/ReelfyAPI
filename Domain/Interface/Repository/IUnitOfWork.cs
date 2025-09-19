namespace Domain.Interface.Repository
{
    public interface IUnitOfWork
    {
        public Task<int> CommitAsync();
        public void Dispose();
    }
}
