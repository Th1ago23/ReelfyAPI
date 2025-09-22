using Application.DTO.Content.Preferences;
using Application.Interface.ContentInterface;
using Application.Interface.Mappers;
using Domain.Interface.HttpContext;
using Domain.Interface.Mappers;
using Domain.Interface.Repository;
using Domain.Models.Contents;

namespace Application.Services;

public class PreferenceService : IPreferenceService
{
    private readonly IPreferenceRepository _context;
    private readonly IUserRepository _userRepository;
    private readonly IContextUser _contextUser;
    private readonly ICastMapper _castMapper;
    private readonly ICrewMapper _crewMapper;
    private readonly IGenreMapper _genreMapper;
    private readonly IStreamingMapper _streamingMapper;
    private readonly IUnitOfWork _unitOfWork;

    public PreferenceService(IPreferenceRepository repository, IUserRepository userRepository, IContextUser contextUser, ICastMapper castMapper, ICrewMapper crewMapper, IGenreMapper genreMapper, IStreamingMapper streamingMapper, IUnitOfWork unitOfWork)
    {
        _context = repository;
        _userRepository = userRepository;
        _contextUser = contextUser;
        _castMapper = castMapper;
        _crewMapper = crewMapper;
        _genreMapper = genreMapper;
        _streamingMapper = streamingMapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<PreferenceResponseDTO> Add(PreferenceAddDTO dto)
    {
        var user = await _userRepository.GetById(_contextUser.Id)
            ?? throw new UnauthorizedAccessException("Usuário não encontrado.");

        var preference = await _context.GetPreferences(user.Id);

        if (preference == null)
        {
            preference = new Preference { UserId = user.Id };
            await _context.Add(preference);
        }
        var casts = (dto.castDTO ?? new List<CastAddDTO>())
            .GroupBy(c => c.id).Select(g => g.First())
            .Where(c => !string.IsNullOrWhiteSpace(c.name))
            .Select(c => new Cast { Id = c.id, Name = c.name });

        var crews = (dto.crewDTO ?? new List<CrewAddDTO>())
            .GroupBy(c => c.id).Select(g => g.First())
            .Where(c => !string.IsNullOrWhiteSpace(c.name))
            .Select(c => new Crew { Id = c.id, Name = c.name });

        var genres = (dto.genreDTO ?? new List<GenreAddDTO>())
            .GroupBy(g => g.id).Select(g => g.First())
            .Where(g => !string.IsNullOrWhiteSpace(g.name))
            .Select(g => new Genre { Id = g.id, Name = g.name });

        var streamings = (dto.streamingDTO ?? new List<StreamingAddDTO>())
            .GroupBy(s => s.id).Select(g => g.First())
            .Where(s => !string.IsNullOrWhiteSpace(s.name))
            .Select(s => new Streaming { Id = s.id, Name = s.name });

        foreach (var cast in casts)
            if (!preference.Casts.Any(x => x.Id == cast.Id))
                preference.Casts.Add(cast);

        foreach (var crew in crews)
            if (!preference.Crews.Any(x => x.Id == crew.Id))
                preference.Crews.Add(crew);

        foreach (var genre in genres)
            if (!preference.Genres.Any(x => x.Id == genre.Id))
                preference.Genres.Add(genre);

        foreach (var streaming in streamings)
            if (!preference.Streamings.Any(x => x.Id == streaming.Id))
                preference.Streamings.Add(streaming);

        await _unitOfWork.CommitAsync();

        return new PreferenceResponseDTO(
            preference.UserId,
            preference.Id,
            preference.Casts.Select(c => new CastAddDTO(c.Id, c.Name)),
            preference.Crews.Select(c => new CrewAddDTO(c.Id, c.Name)),
            preference.Genres.Select(g => new GenreAddDTO(g.Id, g.Name)),
            preference.Streamings.Select(s => new StreamingAddDTO(s.Id, s.Name))
        );
    }

    public async Task<PreferenceResponseDTO> GetAllPreferences()
    {
        var user = await _userRepository.GetById(_contextUser.Id);

        var preferences = await _context.GetPreferences(user.Id);

        return new PreferenceResponseDTO(preferences.UserId, preferences.Id, preferences.Casts.Select(_castMapper.ToDTO), preferences.Crews.Select(_crewMapper.ToDTO), preferences.Genres.Select(_genreMapper.ToDTO), preferences.Streamings.Select(_streamingMapper.ToDTO));

    }
}
