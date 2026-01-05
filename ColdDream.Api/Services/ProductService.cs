using ColdDream.Api.Data;
using ColdDream.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace ColdDream.Api.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;

    public ProductService(IUnitOfWork unitOfWork, UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _unitOfWork.Repository<Product>().GetAllAsync();
    }

    public async Task<Product?> GetProductByIdAsync(Guid id)
    {
        return await _unitOfWork.Repository<Product>().GetByIdAsync(id);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        await _unitOfWork.Repository<Product>().AddAsync(product);
        await _unitOfWork.CompleteAsync();
        return product;
    }

    public async Task UpdateProductAsync(Product product)
    {
        _unitOfWork.Repository<Product>().Update(product);
        await _unitOfWork.CompleteAsync();
    }

    public async Task DeleteProductAsync(Guid id)
    {
        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
        if (product != null)
        {
            _unitOfWork.Repository<Product>().Remove(product);
            await _unitOfWork.CompleteAsync();
        }
    }

    public async Task<bool> BuyProductAsync(Guid userId, Guid productId)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productId);

            if (user == null || product == null) return false;
            if (product.Stock <= 0) return false;
            if (user.Points < product.PointsPrice) return false;

            // Deduct points
            user.Points -= product.PointsPrice;
            await _userManager.UpdateAsync(user);

            // Deduct stock
            product.Stock -= 1;
            _unitOfWork.Repository<Product>().Update(product);
            await _unitOfWork.CompleteAsync();

            await _unitOfWork.CommitTransactionAsync();
            return true;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            return false;
        }
    }
}
