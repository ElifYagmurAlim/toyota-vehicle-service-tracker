using MediatR;
using VehicleServiceTracker.Application.Common.Models;
using VehicleServiceTracker.Application.Interfaces;
using VehicleServiceTracker.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace VehicleServiceTracker.Application.Features.Auth.Commands;

public record LoginCommand(string Username, string Password)
    : IRequest<Result<string>>;

public record LoginRequest(string Username, string Password);

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<string>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IJwtTokenGenerator tokenGenerator,
        IPasswordHasher passwordHasher,
        ILogger<LoginCommandHandler> logger)
    {
        _userRepository = userRepository;
        _tokenGenerator = tokenGenerator;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<Result<string>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetByUsernameAsync(
                request.Username,
                cancellationToken);

            var validationResult = ValidateLoginRequest(request);
            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                _logger.LogWarning("Başarısız giriş denemesi: {Username}", request.Username);
                return Result<string>.Failure("Kullanıcı adı veya şifre hatalı.");
            }

            if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                _logger.LogWarning("Başarısız şifre denemesi: {Username}", request.Username);
                // TODO: Brute force koruması eklenebilir (5 yanlış deneme sonrası kilitle)
                return Result<string>.Failure("Kullanıcı adı veya şifre hatalı.");
            }

            var token = _tokenGenerator.GenerateToken(user.Id, user.Username);
            _logger.LogInformation("Kullanıcı giriş yaptı: {Username}", user.Username);

            return Result<string>.Success(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Giriş işlemi sırasında hata oluştu.");
            return Result<string>.Failure("Giriş işlemi sırasında bir hata oluştu.");
        }
    }

    private Result<bool> ValidateLoginRequest(LoginCommand request)
    {
        if (string.IsNullOrWhiteSpace(request.Username))
            return Result<bool>.Failure("Kullanıcı adı zorunludur.");

        if (request.Username.Length < 3 || request.Username.Length > 100)
            return Result<bool>.Failure("Kullanıcı adı 3-100 karakter arasında olmalıdır.");

        if (string.IsNullOrWhiteSpace(request.Password))
            return Result<bool>.Failure("Şifre zorunludur.");

        if (request.Password.Length < 6)
            return Result<bool>.Failure("Şifre en az 6 karakter olmalıdır.");

        // Username'de tehlikeli karakterler kontrolü (SQL Injection vs)
        if (request.Username.Contains("'") || request.Username.Contains("\"") ||
            request.Username.Contains(";") || request.Username.Contains("--"))
        {
            _logger.LogWarning("Şüpheli login denemesi tespit edildi: {Username}", request.Username);
            return Result<bool>.Failure("Geçersiz kullanıcı adı formatı.");
        }

        return Result<bool>.Success(true);
    }
}