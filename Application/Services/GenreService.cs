using Infraestructure.Repository;

namespace Application.Services;

public class GenreService
{
    private GenreRepository _repository;

    public GenreService(GenreRepository repository)
    {
        _repository = repository;
    }
}
