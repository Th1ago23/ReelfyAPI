namespace Domain.Interface.HttpContext
{
    public interface IContextUser
    {
        public int Id { get; }
        public string? Email { get; }
    }
}
