using Application.DTO.Content.Preferences;
using Application.Interface.ContentInterface;
using Application.Interface.Mappers;
using Domain.Interface.HttpContext;
using Domain.Interface.Mappers;
using Domain.Interface.Repository;
using Domain.Models.Contents;
using ReelfyAPI.Models;

namespace Application.Services;

public class PreferenceService : IPreferenceService
{
    private readonly IPreferenceRepository _context;
    private readonly ICastRepository _castRepository;
    private readonly ICrewRepository _crewRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IStreamingRepository _streamingRepository;
    private readonly IUserRepository _userRepository;
    private readonly IContextUser _contextUser;
    private readonly ICastMapper _castMapper;
    private readonly ICrewMapper _crewMapper;
    private readonly IGenreMapper _genreMapper;
    private readonly IStreamingMapper _streamingMapper;
    private readonly IUnitOfWork _unitOfWork;

    public PreferenceService(IPreferenceRepository context, ICastRepository castRepository, ICrewRepository crewRepository, IGenreRepository genreRepository, IStreamingRepository streamingRepository, IUserRepository userRepository, IContextUser contextUser, ICastMapper castMapper, ICrewMapper crewMapper, IGenreMapper genreMapper, IStreamingMapper streamingMapper, IUnitOfWork unitOfWork)
    {
        _context = context;
        _castRepository = castRepository;
        _crewRepository = crewRepository;
        _genreRepository = genreRepository;
        _streamingRepository = streamingRepository;
        _userRepository = userRepository;
        _contextUser = contextUser;
        _castMapper = castMapper;
        _crewMapper = crewMapper;
        _genreMapper = genreMapper;
        _streamingMapper = streamingMapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<PreferenceResponseDTO>> Add(PreferenceAddDTO dto)
    {
        var user = await _userRepository.GetById(_contextUser.Id);
        if (user == null)
        {
            return new Response<PreferenceResponseDTO>(null, "Usuário não encontrado.", 404);
        }

        var preference = await _context.GetPreferences(user.Id);
        if (preference == null)
        {
            preference = new Preference { UserId = user.Id };
            await _context.Add(preference);
        }

        var castList = new List<Cast>();
        foreach (var c in (dto.castDTO ?? new List<CastAddDTO>()).GroupBy(c => c.id).Select(g => g.First()).Where(c => !string.IsNullOrWhiteSpace(c.name)))
        {
            var existing = await _castRepository.Find(c.id);
            var cast = existing ?? new Cast { Id = c.id, Name = c.name };
            if (!preference.Casts.Any(x => x.Id == cast.Id))
            {
                preference.Casts.Add(cast);
            }
            castList.Add(cast);
        }

        var crewList = new List<Crew>();
        foreach (var c in (dto.crewDTO ?? new List<CrewAddDTO>()).GroupBy(c => c.id).Select(g => g.First()).Where(c => !string.IsNullOrWhiteSpace(c.name)))
        {
            var existing = await _crewRepository.Find(c.id);
            var crew = existing ?? new Crew { Id = c.id, Name = c.name };
            if (!preference.Crews.Any(x => x.Id == crew.Id))
            {
                preference.Crews.Add(crew);
            }
            crewList.Add(crew);
        }

        var genreList = new List<Genre>();
        foreach (var g in (dto.genreDTO ?? new List<GenreAddDTO>()).GroupBy(g => g.id).Select(g => g.First()).Where(g => !string.IsNullOrWhiteSpace(g.name)))
        {
            var genre = await _genreRepository.FindById(g.id) ?? new Genre { Id = g.id, Name = g.name };
            if (!preference.Genres.Any(x => x.Id == genre.Id))
            {
                preference.Genres.Add(genre);
            }
            genreList.Add(genre);
        }

        var streamingList = new List<Streaming>();
        foreach (var s in (dto.streamingDTO ?? new List<StreamingAddDTO>()).GroupBy(s => s.id).Select(g => g.First()).Where(s => !string.IsNullOrWhiteSpace(s.name)))
        {
            var streaming = await _streamingRepository.Find(s.id) ?? new Streaming { Id = s.id, Name = s.name };
            if (!preference.Streamings.Any(x => x.Id == streaming.Id))
            {
                preference.Streamings.Add(streaming);
            }
            streamingList.Add(streaming);
        }

        await _unitOfWork.CommitAsync();

        var responseData = new PreferenceResponseDTO(
            preference.UserId,
            preference.Id,
            preference.Casts.Select(c => new CastAddDTO(c.Id, c.Name, c.ProfilePath)),
            preference.Crews.Select(c => new CrewAddDTO(c.Id, c.Name, c.ProfilePath)),
            preference.Genres.Select(g => new GenreAddDTO(g.Id, g.Name)),
            preference.Streamings.Select(s => new StreamingAddDTO(s.Id, s.Name))
        );

        return new Response<PreferenceResponseDTO>(responseData, "Preferências adicionadas com sucesso.", 200);
    }

    public async Task<Response<PreferenceResponseDTO>> GetAllPreferences()
    {
        var user = await _userRepository.GetById(_contextUser.Id);
        if (user == null)
        {
            return new Response<PreferenceResponseDTO>(null, "Usuário não encontrado.", 404);
        }

        var preferences = await _context.GetPreferences(user.Id);
        if (preferences == null)
        {
            return new Response<PreferenceResponseDTO>(null, "Nenhuma preferência encontrada para o usuário.", 404);
        }

        var responseData = new PreferenceResponseDTO(
            preferences.UserId,
            preferences.Id,
            preferences.Casts.Select(_castMapper.ToDTO),
            preferences.Crews.Select(_crewMapper.ToDTO),
            preferences.Genres.Select(_genreMapper.ToDTO),
            preferences.Streamings.Select(_streamingMapper.ToDTO)
        );

        return new Response<PreferenceResponseDTO>(responseData, "Preferências recuperadas com sucesso.", 200);
    }
}