using Application.DTO.Content;
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
        return new PreferenceResponseDTO(preference.UserId, preference.Id);
    }
        //}
        //public async Task AddCast(int id, CastAddDTO dto)
        //{
        //    var user = await _userRepository.GetById(_contextUser.Id) ?? throw new UnauthorizedAccessException();

        //    if (user.PreferenceId != id) throw new UnauthorizedAccessException();

        //    var cast = new Cast
        //    {
        //        Id = dto.id,
        //        Name = dto.name,
        //    };

        //    user.Preference.Casts.Add(cast);
        //    await _unitOfWork.CommitAsync();        
        //}
        //public async Task AddGenre(int id, GenreAddDTO dto)
        //{
        //    var user = await _userRepository.GetById(_contextUser.Id) ?? throw new UnauthorizedAccessException();
        //    if (user.PreferenceId != id) throw new UnauthorizedAccessException();

        //    var genre = new Genre
        //    {
        //        Id = dto.id,
        //        Name = dto.name,
        //    };

        //    user.Preference.Genres.Add(genre);
        //    await _unitOfWork.CommitAsync();
        //}
        //public async Task AddCrew(int id, CrewAddDTO dto)
        //{
        //    var user = await _userRepository.GetById(_contextUser.Id) ?? throw new UnauthorizedAccessException();
        //    if (user.PreferenceId != id) throw new UnauthorizedAccessException();

        //    var crew = new Crew
        //    {
        //        Id = dto.id,
        //        Name = dto.name,
        //    };

        //    user.Preference.Crews.Add(crew);
        //    await _unitOfWork.CommitAsync();
        //}

        //public async Task AddStreaming (int id, StreamingAddDTO dto)
        //{
        //    var user = await _userRepository.GetById(_contextUser.Id) ?? throw new UnauthorizedAccessException();
        //    if (user.PreferenceId != id) throw new UnauthorizedAccessException();

        //    var streaming = new Streaming
        //    {
        //        Id = dto.id,
        //        Name = dto.name,
        //    };

        //    user.Preference.Streamings.Add(streaming);
        //    await _unitOfWork.CommitAsync();
        //}
        //public async Task RemoveCast(int id, CastAddDTO dto)
        //{
        //    var user = await _userRepository.GetById(_contextUser.Id) ?? throw new UnauthorizedAccessException();
        //    if (user.PreferenceId != id) throw new UnauthorizedAccessException();

        //    var castToRemove = user.Preference.Casts.FirstOrDefault(i =>  i.Id == dto.id)?? throw new NullReferenceException();
        //    user.Preference.Casts.Remove(castToRemove);
        //    await _unitOfWork.CommitAsync();
        //}  
}
