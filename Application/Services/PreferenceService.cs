using Application.DTO.Content.Preferences;
using Application.Interface.ContentInterface;
using Application.Interface.Mappers;
using Domain.Interface.HttpContext;
using Domain.Interface.Mappers;
using Domain.Interface.Repository;
using Domain.Models.Contents;
using Infraestructure.Repository;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Application.Services;

public class PreferenceService: IPreferenceService
{
    private readonly IPreferenceRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly IContextUser _contextUser;
    private readonly ICastMapper _castMapper;
    private readonly ICrewMapper _crewMapper;
    private readonly IGenreMapper _genreMapper;
    private readonly IStreamingMapper _streamingMapper;
    private readonly IUnitOfWork _unitOfWork;

    public PreferenceService(IPreferenceRepository repository, IUserRepository userRepository, IContextUser contextUser, ICastMapper castMapper, ICrewMapper crewMapper, IGenreMapper genreMapper, IStreamingMapper streamingMapper, IUnitOfWork unitOfWork)
    {
        _repository = repository;
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
           ?? throw new UnauthorizedAccessException();

        var casts = _castMapper.ToEntities(
                        (dto.castDTO ?? new List<CastAddDTO>())
                        .GroupBy(c => c.id).Select(g => g.First()))
                    .Where(c => !string.IsNullOrWhiteSpace(c.Name))
                    .ToList();

        var genres = _genreMapper.ToEntities(
                         (dto.genreDTO ?? new List<GenreAddDTO>())
                         .GroupBy(g => g.id).Select(g => g.First()))
                     .Where(g => !string.IsNullOrWhiteSpace(g.Name))
                     .ToList();

        var crews = _crewMapper.ToEntities(
                        (dto.crewDTO ?? new List<CrewAddDTO>())
                        .GroupBy(c => c.id).Select(g => g.First()))
                    .Where(c => !string.IsNullOrWhiteSpace(c.Name))
                    .ToList();

        var streamings = _streamingMapper.ToEntities(
                            (dto.streamingDTO ?? new List<StreamingAddDTO>())
                            .GroupBy(s => s.id).Select(g => g.First()))
                        .Where(s => !string.IsNullOrWhiteSpace(s.Name))
                        .ToList();
        var preference = new Preference
        {
            User = user,
            UserId = user.Id,
            Casts = casts,
            Genres = genres,
            Crews = crews,
            Streamings = streamings,
        };

        await _repository.Add(preference);
        await _unitOfWork.CommitAsync();

        var preferences = await _repository.GetPreferences(user.Preference.Id);

        return new PreferenceResponseDTO(preferences.UserId, preferences.Id, preferences.Casts.Select(_castMapper.ToDTO),preferences.Crews.Select(_crewMapper.ToDTO),preferences.Genres.Select(_genreMapper.ToDTO), preferences.Streamings.Select(_streamingMapper.ToDTO));
    }
    public async Task<PreferenceResponseDTO> GetAllPreferences()
    {
        var user = await _userRepository.GetById(_contextUser.Id);

        var preferences = await _repository.GetPreferences(user.Preference.Id);

        return new PreferenceResponseDTO(preferences.UserId, preferences.Id, preferences.Casts.Select(_castMapper.ToDTO), preferences.Crews.Select(_crewMapper.ToDTO), preferences.Genres.Select(_genreMapper.ToDTO), preferences.Streamings.Select(_streamingMapper.ToDTO));
    
    }
}
