using Application.DTO.Content;
using Application.DTO.Content.Preferences;
using Application.DTO.Users;
using Application.Interface.ContentInterface;
using Application.Interface.Mappers;
using Application.Interface.UserInterface;
using Domain.Interface.HttpContext;
using Domain.Interface.Mappers;
using Domain.Interface.Repository;
using Domain.Models.Users;
using Infraestructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class UserService:IUserService
{
    private readonly IUserRepository _context;
    private readonly IContextUser _contextUser;
    private readonly IContentMapper _contentMapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserMapper _mapper;
    private readonly ICastMapper _castMapper;
    private readonly ICrewMapper _crewMapper;
    private readonly IGenreMapper _genreMapper;
    private readonly IStreamingMapper _streamingMapper;

    public UserService(IUserRepository context, IContextUser contextUser, IUnitOfWork unitOfWork, IUserMapper mapper, ICastMapper castMapper, ICrewMapper crewMapper, IGenreMapper genreMapper, IStreamingMapper streamingMapper, IContentMapper contentMapper)
    {
        _context = context;
        _contextUser = contextUser;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _castMapper = castMapper;
        _crewMapper = crewMapper;
        _genreMapper = genreMapper;
        _streamingMapper = streamingMapper;
        _contentMapper = contentMapper;
    }

    public async Task<UserSummaryDTO> GetUserById(int id)
    {
        var user = await _context.GetById(id);

        if (user == null) throw new NullReferenceException($"Usuário não encontrado com o ID {id}");
        var castsDTO = user.Preference.Casts.Select(_castMapper.ToDTO);
        var crewsDTO = user.Preference.Crews.Select(_crewMapper.ToDTO);
        var genresDTO = user.Preference.Genres.Select(_genreMapper.ToDTO);
        var streamingsDTO = user.Preference.Streamings.Select(_streamingMapper.ToDTO);

        var preference = new PreferenceResponseDTO(user.Id, user.Preference.Id, castsDTO, crewsDTO, genresDTO, streamingsDTO);

        return new UserSummaryDTO(user.Id, user.Name, user.GetAge(), user.PhoneNumber, preference, user.Contents.Select(_contentMapper.ToDTO), user.IsPreemium);
    }

    public async Task<UserSummaryDTO> GetUserByEmail(string email)
    {
        var user = await _context.GetByEmail(email);

        var castsDTO = user.Preference.Casts.Select(_castMapper.ToDTO);
        var crewsDTO = user.Preference.Crews.Select(_crewMapper.ToDTO);
        var genresDTO = user.Preference.Genres.Select(_genreMapper.ToDTO);
        var streamingsDTO = user.Preference.Streamings.Select(_streamingMapper.ToDTO);
        var preference = new PreferenceResponseDTO(user.Id, user.Preference.Id, castsDTO, crewsDTO, genresDTO, streamingsDTO);
        return new UserSummaryDTO(user.Id, user.Name, user.GetAge(), user.PhoneNumber, preference, user.Contents.Select(_contentMapper.ToDTO), user.IsPreemium);
    }
    public async Task<IEnumerable<UserSummaryDTO>> GetAllUsers()
    {
        var users = await _context
                                .GetAll();
        var listDTO = new List<UserSummaryDTO>();

        foreach (var user in users)
        {
            var castsDTO = user.Preference.Casts.Select(_castMapper.ToDTO);
            var crewsDTO = user.Preference.Crews.Select(_crewMapper.ToDTO);
            var genresDTO = user.Preference.Genres.Select(_genreMapper.ToDTO);
            var streamingsDTO = user.Preference.Streamings.Select(_streamingMapper.ToDTO);
            var preference = new PreferenceResponseDTO(user.Id, user.Preference.Id, castsDTO, crewsDTO, genresDTO, streamingsDTO);

            var dto = new UserSummaryDTO(user.Id, user.Name, user.GetAge(), user.PhoneNumber, preference, user.Contents.Select(_contentMapper.ToDTO), user.IsPreemium);

            listDTO.Add(dto);
        }

        return listDTO;
    }
    public async Task<bool> DeleteUser(int id)
    {
        var user = await _context.GetById(id);
        if (user is null) throw new Exception("Usuário não encontrado.");
        _context.Delete(user);
        await _unitOfWork.CommitAsync();
        return true;
    }
    public async Task<bool> VerifyUser(string email)
    {
        return await _context.UserExists(email);
    }

    public async Task<FavoriteDTO> GetFavorite(int id)
    {
        var user = await _context.FindFavorite(id)
            ?? throw new NullReferenceException("Usuário não encontrado");

        return _mapper.ToFavorite(user);
    }

    public async Task<FavoriteDTO> GetFavoriteInContext()
    {
        var user = await _context.FindFavorite(_contextUser.Id) ?? throw new ArgumentNullException();

        return _mapper.ToFavorite(user);

    }

    public async Task<UserResponseDTO> UpdateUser(UpdateUserDTO update)
    {
        var user = await _context.GetById(_contextUser.Id);

        if (user is null) throw new UnauthorizedAccessException("Usuário não encontrado");

        user.Name = update.Name ?? user.Name;
        user.Email = update.Email ?? user.Email;
        user.PhoneNumber = update.PhoneNumber ?? user.PhoneNumber;
        user.UpdatedAt = DateTime.UtcNow;

        _context.Update(user);
        await _unitOfWork.CommitAsync();
        return _mapper.ToUserResponseDTO(user);

    }
    public async Task<UserSummaryDTO> TurnPreemium (int id, bool result)
    {
        var user = await _context.GetById(id);
        user.IsPreemium = result;

        await _unitOfWork.CommitAsync();

        

        var castsDTO = user.Preference.Casts.Select(_castMapper.ToDTO);
        var crewsDTO = user.Preference.Crews.Select(_crewMapper.ToDTO);
        var genresDTO = user.Preference.Genres.Select(_genreMapper.ToDTO);
        var streamingsDTO = user.Preference.Streamings.Select(_streamingMapper.ToDTO);

        var preference = new PreferenceResponseDTO(user.Id, user.Preference.Id, castsDTO, crewsDTO, genresDTO, streamingsDTO);

        return new UserSummaryDTO(user.Id, user.Name, user.GetAge(), user.PhoneNumber, preference, user.Contents.Select(_contentMapper.ToDTO), user.IsPreemium);

    }
}
