using Application.Services.Jwt;
using Domain.Entities.RefreshTokens.Repository;
using Domain.Shared.Abstractions;
using Domain.Specifications.BlackListTokens;
using Domain.Specifications.RefreshTokens;
using LuxuzDev.PersonalLogger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application.BackgroundServices.CleanupRefreshTokens;

public class CleanupRefreshTokensService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _interval = TimeSpan.FromHours(24);

    public CleanupRefreshTokensService
        (IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            await Task.Delay(_interval, ct);

            using var scope = _serviceProvider.CreateScope();
            var refreshTokenRepository = scope.ServiceProvider.GetRequiredService<IRefreshTokenRepository>();
            var blackListRepository = scope.ServiceProvider.GetRequiredService<ITokenBlackListRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            // Eliminar tokens expirados o revocados
            var totalTokensDeleted = await refreshTokenRepository.DeleteBySpecAsync(new ExpiredOrRevokedTokensSpecification(DateTime.UtcNow), ct);

            //Eliminar tokens de la lista negra
            var totalTokenDeletedFromBlackList = await blackListRepository.DeleteBySpecAsync(new ExpiredBlackListTokensSpecification(JwtSettings.ExpirationInMinutes , DateTime.UtcNow),ct);

            await unitOfWork.SaveChangesAsync(ct);

            PersonalLogger.Log($"Fueron eliminados {totalTokensDeleted} tokens expirados o revocados.");
            PersonalLogger.Log($"Fueron eliminados {totalTokenDeletedFromBlackList} tokens de la Lista Negra.");
        }
    }
}