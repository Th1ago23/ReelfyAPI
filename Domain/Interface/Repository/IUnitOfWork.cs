namespace Domain.Interface.Repository
{
    public interface IUnitOfWork
    {
        public ICastRepository Cast { get; }
        public ICrewRepository Crew { get; }
        public IGenreRepository Genre { get; }
        public IPreferenceRepository Preference { get; }
        public IContentRepository Content { get; }
        public IStreamingRepository Streaming { get; }
        public IUserRepository User { get; }
        public IFavoriteContentRepository FavoriteContent { get; }
        public IAlreadySeenContentRepository AlreadySeenContent { get; }
        public IContentsListRepository ContentsList { get; }
        public Task<int> CommitAsync();
        public void Dispose();
    }
}
