namespace VehicleServiceTracker.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string username);
}