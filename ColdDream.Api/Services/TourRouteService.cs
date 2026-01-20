using ColdDream.Api.Data;
using ColdDream.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ColdDream.Api.Services;

public class TourRouteService : ITourRouteService
{
    private readonly IUnitOfWork _unitOfWork;

    public TourRouteService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<TourRoute>> GetRoutesByCreatorAsync(Guid creatorId)
    {
        return await _unitOfWork.Repository<TourRoute>().FindAsync(r => r.CreatorId == creatorId);
    }

    public async Task<IEnumerable<TourRoute>> GetPublicRoutesAsync()
    {
        // Only return non-private routes, or maybe all routes but mark private ones?
        // Requirement: "User without login can browse tour route details, if need to reserve or view some private routes need login"
        // So maybe list all, but details are restricted?
        // "User without login can browse tour route details" -> This implies details are public mostly.
        // "view some private routes need login" -> Some routes are private.

        return await _unitOfWork.Repository<TourRoute>().FindAsync(r => !r.IsPrivate);
    }

    public async Task<TourRoute?> GetRouteDetailsAsync(Guid id, bool isAuthenticated)
    {
        var route = await _unitOfWork.Repository<TourRoute>().GetByIdAsync(id);
        if (route == null)
            return null;

        if (route.IsPrivate && !isAuthenticated)
        {
            return null; // Or throw exception
        }

        // Auto-fill missing data for demo purposes
        bool needsUpdate = false;
        if (string.IsNullOrEmpty(route.Itinerary))
        {
            route.Itinerary = "[{\"day\": 1, \"title\": \"行程第一天\", \"content\": \"抵达目的地，入住酒店，自由活动。\"}, {\"day\": 2, \"title\": \"深度游览\", \"content\": \"参观当地著名景点，体验特色文化。\"}, {\"day\": 3, \"title\": \"返程\", \"content\": \"购买纪念品，前往机场/车站。\"}]";
            needsUpdate = true;
        }
        if (string.IsNullOrEmpty(route.RouteMapUrl))
        {
            route.RouteMapUrl = "https://images.unsplash.com/photo-1524661135-423995f22d0b";
            needsUpdate = true;
        }

        if (needsUpdate)
        {
            _unitOfWork.Repository<TourRoute>().Update(route);
            await _unitOfWork.CompleteAsync();
        }

        return route;
    }

    public async Task<TourRoute> CreateRouteAsync(TourRoute route)
    {
        await _unitOfWork.Repository<TourRoute>().AddAsync(route);
        await _unitOfWork.CompleteAsync();
        return route;
    }

    public async Task<TourRoute> UpdateRouteAsync(TourRoute route)
    {
        _unitOfWork.Repository<TourRoute>().Update(route);
        await _unitOfWork.CompleteAsync();
        return route;
    }

    public async Task<IEnumerable<TourRoute>> GetTopSellingRoutesAsync(int count)
    {
        return await _unitOfWork
            .Repository<TourRoute>()
            .GetQueryable()
            .OrderByDescending(r => r.Sales)
            .Take(count)
            .ToListAsync();
    }
}
