using Application.DTO.Content;
using Application.Interface.ContentInterface;
using Domain.Interface.HttpContext;
using Domain.Interface.Repository;
using Domain.Models.Contents;
using ReelfyAPI.Models;


namespace Application.Services;

public class ContentListService : IContentListService
{
    private readonly IContentMapper _contentMapper;
    private readonly IContentRepository _contentRepository;
    private readonly IContentsListRepository _context;
    private readonly IUserRepository _userRepository;
    private readonly IContextUser _contextUser;
    private readonly IUnitOfWork _unitOfWork;

    public ContentListService(IContentRepository contentRepository, IContentMapper contentMapper, IContentsListRepository context, IUserRepository userRepository, IContextUser contextUser, IUnitOfWork unitOfWork)
    {
        _contentRepository = contentRepository;
        _contentMapper = contentMapper;
        _context = context;
        _userRepository = userRepository;
        _contextUser = contextUser;
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<ContentsListResponseDTO>> ListCreate(ContentListCreateDTO dto)
    {
        var user = await _userRepository.GetById(_contextUser.Id);
        if (user == null) return new Response<ContentsListResponseDTO>(null, "Usuário não autorizado. Por favor, faça login novamente ou tente novamente mais tarde.", 401);

        if (user.ContentLists.Any(i => i.Name == dto.name)) return new Response<ContentsListResponseDTO>(null, "Já existe uma lista com este nome", 409);

        var contentInListEntity = new ContentsList
        {
            UserId = user.Id,
            Name = dto.name,
            Description = dto.description
        };

        await _context.Add(contentInListEntity);
        await _unitOfWork.CommitAsync();

        var response = new ContentsListResponseDTO(contentInListEntity.UserId, contentInListEntity.Id, null);

        return new Response<ContentsListResponseDTO>(response, $"Lista {contentInListEntity.Name}", 201);
    }

    public async Task<Response<ContentsListResponseDTO>> AddContentToList(int contentId, int listId)
    {
        var user = await _userRepository.GetById(_contextUser.Id);

        if (user == null) return new Response<ContentsListResponseDTO>(null, "Usuário não autorizado. Por favor, faça login novamente ou tente novamente mais tarde.", 401);

        var content = await _contentRepository.Find(contentId);

        if (content == null) return new Response<ContentsListResponseDTO>(null, "Conteúdo não encontrado.");

        var contentList = await _context.GetById(listId);

        if (!user.ContentLists.Any(i => i.Id == contentList.Id)) return new Response<ContentsListResponseDTO>(null, "Essa lista não existe. Por favor, crie uma lista válida para adicionar os conteúdos.", 401);

        if (contentList.Contents.Any(i => i.Id == content.Id)) return new Response<ContentsListResponseDTO>(null, $"{content.Title} já existe nesta lista.", 409);

        contentList.Contents.Add(content);
        await _unitOfWork.CommitAsync();

        var response = new ContentsListResponseDTO(contentList.UserId, contentList.Id, contentList.Contents.Select(_contentMapper.ToDTO));

        return new Response<ContentsListResponseDTO>(response, $"{content.Title} adicionado a lista {contentList.Id}", 200);

    }
    public async Task<Response<ContentsListResponseDTO>> RemoveContentoFromList(int contentId, int listId)
    {
        var user = await _userRepository.GetById(_contextUser.Id);

        if (user == null) return new Response<ContentsListResponseDTO>(null, "Usuário não autorizado. Por favor, faça login novamente ou tente novamente mais tarde.", 401);

        var contentList = await _context.GetById(listId);

        if (contentList == null || contentList.UserId != user.Id) return new Response<ContentsListResponseDTO>(null, "Erro ao buscar lista. Por favor, crie uma nova lista e tente novamente.", 401);
        if (!contentList.Contents.Any(i => i.Id == contentId)) return new Response<ContentsListResponseDTO>(null, "Esse conteúdo não existe nesta lista.", 404);

        var contentToRemove = contentList.Contents.FirstOrDefault(i => i.Id == contentId);

        contentList.Contents.Remove(contentToRemove);
        await _unitOfWork.CommitAsync();

        var response = new ContentsListResponseDTO(contentList.UserId, contentList.Id, contentList.Contents.Select(_contentMapper.ToDTO));

        return new Response<ContentsListResponseDTO>(response, $"{contentToRemove.Title} removido com sucesso", 200);
    }

    public async Task<Response<ContentsListResponseDTO>> DeleteContentList(int id)
    {
        var user = await _userRepository.GetById(_contextUser.Id);

        if (user == null) return new Response<ContentsListResponseDTO>(null, "Usuário não autorizado. Por favor, faça login novamente ou tente novamente mais tarde.", 401);

        var contentList = await _context.GetById(id);

        if (contentList == null || contentList.UserId != user.Id) return new Response<ContentsListResponseDTO>(null, "Erro ao buscar lista. Por favor, crie uma nova lista e tente novamente.", 401);
        await _context.Delete(id);
        await _unitOfWork.CommitAsync();

        return new Response<ContentsListResponseDTO>(null, $"{contentList.Name} deletada com sucesso.", 200);
    }
    public async Task<Response<ContentFromListDTO>> GetContentFromList(int listId, int contentId)
    {
        var contentList = await _context.GetById(listId);
        if (_contextUser.Id != contentList.UserId) return new Response<ContentFromListDTO>(null, "Erro ao buscar lista do usuário. Faça login novamente.", 401);

        var contentFromList = contentList.Contents.FirstOrDefault(i => i.Id == contentId);

        if (contentFromList is null) return new Response<ContentFromListDTO>(null, "Conteúdo não encontrado.", 409);

        var response = new ContentFromListDTO(contentList.UserId, contentList.Id, _contentMapper.ToDTO(contentFromList));

        return new Response<ContentFromListDTO>(response, "Conteúdo encontrado com sucesso", 200);

    }
    public async Task<Response<ContentsListResponseDTO>> GetAllContentsFromList(int listId)
    {
        var contentList = await _context.GetById(listId);
        if (_contextUser.Id != contentList.UserId) return new Response<ContentsListResponseDTO>(null, "Erro ao buscar lista do usuário. Faça login novamente.", 401);

        var response = new ContentsListResponseDTO(contentList.UserId, contentList.Id, contentList.Contents.Select(_contentMapper.ToDTO));

        return new Response<ContentsListResponseDTO>(response, "Lista de conteúdo encontrada com sucesso.", 200);
    }
}
