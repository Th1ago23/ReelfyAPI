using Application.DTO.Content;
using Domain.Interface.Repository;
using ReelfyAPI.Models;

namespace Application.Services;

public class ContentListService
{
    private readonly IContentsListRepository _contentsListRepository;

    public ContentListService(IContentsListRepository contentsListRepository)
    {
        _contentsListRepository = contentsListRepository;
    }

    public async Task<Response<ContentsListResponseDTO>> ListCreate(ContentsListDTO dto)
    {

    }
}
