using ColdDream.Api.Data;
using ColdDream.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ColdDream.Api.Services;

public interface IBannerService
{
    Task<IEnumerable<Banner>> GetActiveBannersAsync();
}

public class BannerService : IBannerService
{
    private readonly IUnitOfWork _unitOfWork;

    public BannerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Banner>> GetActiveBannersAsync()
    {
        return await _unitOfWork.Repository<Banner>()
            .FindAsync(b => b.IsActive);
    }
}
