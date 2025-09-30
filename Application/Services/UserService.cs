using Application.DTO.Content;
using Application.DTO.Content.Preferences;
using Application.DTO.Users;
using Application.Interface.ContentInterface;
using Application.Interface.Mappers;
using Application.Interface.UserInterface;
using Domain.Interface.HttpContext;
using Domain.Interface.Mappers;
using Domain.Interface.Repository;
using ReelfyAPI.Models;

namespace Application.Services;

public class UserService : IUserService
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

    public async Task<Response<UserSummaryDTO>> GetUserById(int id)
    {
        var user = await _context.GetById(id);
        if (user == null)
        {
            return new Response<UserSummaryDTO>(null, $"Usuário não encontrado com o ID {id}.", 404);
        }

        var castsDTO = user.Preference.Casts.Select(_castMapper.ToDTO);
        var crewsDTO = user.Preference.Crews.Select(_crewMapper.ToDTO);
        var genresDTO = user.Preference.Genres.Select(_genreMapper.ToDTO);
        var streamingsDTO = user.Preference.Streamings.Select(_streamingMapper.ToDTO);
        var preference = new PreferenceResponseDTO(user.Id, user.Preference.Id, castsDTO, crewsDTO, genresDTO, streamingsDTO);

        var responseData = new UserSummaryDTO(user.Id, user.Name, user.GetAge(), user.PhoneNumber, preference, user.FavoriteContents.Select(_contentMapper.ToDTO), user.IsPreemium);
        return new Response<UserSummaryDTO>(responseData, "Usuário encontrado com sucesso.", 200);
    }

    public async Task<Response<UserSummaryDTO>> GetUserByEmail(string email)
    {
        var user = await _context.GetByEmail(email);
        if (user == null)
        {
            return new Response<UserSummaryDTO>(null, $"Usuário não encontrado com o e-mail {email}.", 404);
        }

        var castsDTO = user.Preference.Casts.Select(_castMapper.ToDTO);
        var crewsDTO = user.Preference.Crews.Select(_crewMapper.ToDTO);
        var genresDTO = user.Preference.Genres.Select(_genreMapper.ToDTO);
        var streamingsDTO = user.Preference.Streamings.Select(_streamingMapper.ToDTO);
        var preference = new PreferenceResponseDTO(user.Id, user.Preference.Id, castsDTO, crewsDTO, genresDTO, streamingsDTO);

        var responseData = new UserSummaryDTO(user.Id, user.Name, user.GetAge(), user.PhoneNumber, preference, user.FavoriteContents.Select(_contentMapper.ToDTO), user.IsPreemium);
        return new Response<UserSummaryDTO>(responseData, "Usuário encontrado com sucesso.", 200);
    }

    public async Task<Response<IEnumerable<UserSummaryDTO>>> GetAllUsers()
    {
        var users = await _context.GetAll();
        var listDTO = new List<UserSummaryDTO>();

        foreach (var user in users)
        {
            var castsDTO = user.Preference.Casts.Select(_castMapper.ToDTO);
            var crewsDTO = user.Preference.Crews.Select(_crewMapper.ToDTO);
            var genresDTO = user.Preference.Genres.Select(_genreMapper.ToDTO);
            var streamingsDTO = user.Preference.Streamings.Select(_streamingMapper.ToDTO);
            var preference = new PreferenceResponseDTO(user.Id, user.Preference.Id, castsDTO, crewsDTO, genresDTO, streamingsDTO);
            var dto = new UserSummaryDTO(user.Id, user.Name, user.GetAge(), user.PhoneNumber, preference, user.FavoriteContents.Select(_contentMapper.ToDTO), user.IsPreemium);
            listDTO.Add(dto);
        }

        return new Response<IEnumerable<UserSummaryDTO>>(listDTO, "Lista de usuários recuperada com sucesso.", 200);
    }

    public async Task<Response<bool>> DeleteUser(int id)
    {
        var user = await _context.GetById(id);
        if (user == null)
        {
            return new Response<bool>(false, "Usuário não encontrado.", 404);
        }

        _context.Delete(user);
        await _unitOfWork.CommitAsync();
        return new Response<bool>(true, "Usuário deletado com sucesso.", 200);
    }

    public async Task<Response<bool>> VerifyUser(string email)
    {
        var userExists = await _context.UserExists(email);
        if (userExists)
        {
            return new Response<bool>(true, "Usuário já existe.", 200);
        }
        return new Response<bool>(false, "Usuário não existe.", 404);
    }

    public async Task<Response<FavoriteDTO>> GetFavorite(int id)
    {
        var user = await _context.FindFavorite(id);
        if (user == null)
        {
            return new Response<FavoriteDTO>(null, "Usuário não encontrado.", 404);
        }

        var responseData = _mapper.ToFavorite(user);
        return new Response<FavoriteDTO>(responseData, "Favorito encontrado com sucesso.", 200);
    }

    public async Task<Response<FavoriteDTO>> GetFavoriteInContext()
    {
        var user = await _context.FindFavorite(_contextUser.Id);
        if (user == null)
        {
            return new Response<FavoriteDTO>(null, "Usuário não encontrado.", 404);
        }

        var responseData = _mapper.ToFavorite(user);
        return new Response<FavoriteDTO>(responseData, "Favorito encontrado com sucesso.", 200);
    }

    public async Task<Response<UserResponseDTO>> UpdateUser(UpdateUserDTO update)
    {
        var user = await _context.GetById(_contextUser.Id);
        if (user == null)
        {
            return new Response<UserResponseDTO>(null, "Usuário não encontrado.", 404);
        }

        user.Name = update.Name ?? user.Name;
        user.Email = update.Email ?? user.Email;
        user.PhoneNumber = update.PhoneNumber ?? user.PhoneNumber;
        user.UpdatedAt = DateTime.UtcNow;

        _context.Update(user);
        await _unitOfWork.CommitAsync();

        var responseData = _mapper.ToUserResponseDTO(user);
        return new Response<UserResponseDTO>(responseData, "Usuário atualizado com sucesso.", 200);
    }

    public async Task<Response<UserSummaryDTO>> TurnPreemium(int id, bool result)
    {
        var user = await _context.GetById(id);
        if (user == null)
        {
            return new Response<UserSummaryDTO>(null, $"Usuário não encontrado com o ID {id}.", 404);
        }

        user.IsPreemium = result;
        await _unitOfWork.CommitAsync();

        var castsDTO = user.Preference.Casts.Select(_castMapper.ToDTO);
        var crewsDTO = user.Preference.Crews.Select(_crewMapper.ToDTO);
        var genresDTO = user.Preference.Genres.Select(_genreMapper.ToDTO);
        var streamingsDTO = user.Preference.Streamings.Select(_streamingMapper.ToDTO);
        var preference = new PreferenceResponseDTO(user.Id, user.Preference.Id, castsDTO, crewsDTO, genresDTO, streamingsDTO);

        var responseData = new UserSummaryDTO(user.Id, user.Name, user.GetAge(), user.PhoneNumber, preference, user.FavoriteContents.Select(_contentMapper.ToDTO), user.IsPreemium);
        return new Response<UserSummaryDTO>(responseData, "Status de premium atualizado com sucesso.", 200);
    }

    public async Task<Response<ContentAlreadySeensDTO>> ContentsAlreadySeens()
    {
        var user = await _context.GetById(_contextUser.Id);
        if (user == null)
        {
            return new Response<ContentAlreadySeensDTO>(null, "Usuário sem permissão.", 401);
        }

        var contentList = user.ContentLists.SelectMany(i => i.Contents).ToList();
        var favoriteList = user.FavoriteContents.ToList();
        var contents = new List<FavoriteContentDTO>();

        foreach (var f in favoriteList)
        {
            if (f.AlreadySeen == true) contents.Add(_contentMapper.ToDTO(f));
        }

        foreach (var c in contentList)
        {
            if (c.AlreadySeen == true) contents.Add(_contentMapper.ToDTO(c));
        }

        var responseData = new ContentAlreadySeensDTO(user.Id, contents);
        return new Response<ContentAlreadySeensDTO>(responseData, null, 200);
    }
}