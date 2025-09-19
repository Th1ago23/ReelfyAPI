using Application.DTO.Content;
using Application.Interface.Mappers;
using Domain.Interface.HttpContext;
using Domain.Interface.Mappers;
using Domain.Interface.Repository;
using Domain.Models.Contents;
using Infraestructure.Repository;

namespace Application.Services;

public class PreferenceService
{
    private readonly IPreferenceRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly IContextUser _contextUser;
    private readonly ICastMapper _castMapper;
    private readonly ICrewMapper _crewMapper;
    private readonly IGenreMapper _genreMapper;
    private readonly IStreamingMapper _streamingMapper;

    public PreferenceService(IPreferenceRepository repository, IUserRepository userRepository, IContextUser contextUser, ICastMapper castMapper, ICrewMapper crewMapper, IGenreMapper genreMapper, IStreamingMapper streamingMapper)
    {
        _repository = repository;
        _userRepository = userRepository;
        _contextUser = contextUser;
        _castMapper = castMapper;
        _crewMapper = crewMapper;
        _genreMapper = genreMapper;
        _streamingMapper = streamingMapper;
    }

    public async Task Add(PreferenceAddDTO dto)
    {
        var user = await _userRepository.GetById(_contextUser.Id);
        var preference = new Preference
        {
            User = user,
            UserId = user.Id,
            Casts = _castMapper.ToEntities(dto.castDTO).ToList(),
            Genres = _genreMapper.ToEntities(dto.genreDTO).ToList(),
            Crews = _crewMapper.ToEntities(dto.crewDTO).ToList(),
            Streamings = _streamingMapper.ToEntities(dto.streamingDTO).ToList(),

        };

        await _repository.Add(preference);
    }
}
