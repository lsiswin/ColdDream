using AutoMapper;
using ColdDream.Api.DTOs;
using ColdDream.Api.Models;
using ColdDream.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ColdDream.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;

    public ProductsController(IProductService productService, IMapper mapper)
    {
        _productService = productService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ApiResponse<IEnumerable<ProductDto>>> GetProducts()
    {
        var products = await _productService.GetAllProductsAsync();
        var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
        return ApiResponse<IEnumerable<ProductDto>>.Ok(productDtos);
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<ProductDto>> GetProduct(Guid id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null) return ApiResponse<ProductDto>.Fail("Product not found");
        return ApiResponse<ProductDto>.Ok(_mapper.Map<ProductDto>(product));
    }

    [HttpPost]
    [Authorize] // Admin only
    public async Task<ApiResponse<ProductDto>> CreateProduct(CreateProductDto createProductDto)
    {
        var product = _mapper.Map<Product>(createProductDto);
        var createdProduct = await _productService.CreateProductAsync(product);
        var productDto = _mapper.Map<ProductDto>(createdProduct);
        
        return ApiResponse<ProductDto>.Ok(productDto, "Product created");
    }

    [HttpPost("{id}/buy")]
    [Authorize]
    public async Task<ApiResponse<string>> BuyProduct(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return ApiResponse<string>.Fail("Unauthorized");

        var success = await _productService.BuyProductAsync(Guid.Parse(userId), id);
        if (!success) return ApiResponse<string>.Fail("Purchase failed. Check points or stock.");

        return ApiResponse<string>.Ok("Purchase successful");
    }
}
