using Application.Services.Jwt;
using Domain.Entities.RefreshTokens.Repository;
using LuxuzDev.PersonalLogger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application.BackgroundServices.CleanupRefreshTokens;

public class CleanupRefreshTokensService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _interval = TimeSpan.FromHours(24);

    public CleanupRefreshTokensService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_interval, stoppingToken);

            using var scope = _serviceProvider.CreateScope();
            var refreshTokenRepository = scope.ServiceProvider.GetRequiredService<IRefreshTokenRepository>();
            var blackListRepository = scope.ServiceProvider.GetRequiredService<ITokenBlackListRepository>();

            // Eliminar tokens expirados o revocados
            var totalTokensDeleted = await refreshTokenRepository.DeleteExpiredOrRevokedAsync(stoppingToken);

            PersonalLogger.Log($"Fueron eliminados {totalTokensDeleted} tokens expirados o revocados.");

            //Eliminar tokens de la lista negra
            var totalTokenDeletedFromBlackList = await blackListRepository.DeleteExpiredAsync(JwtSettings.ExpirationInMinutes);

            PersonalLogger.Log($"Fueron eliminados {totalTokenDeletedFromBlackList} tokens de la Lista Negra.");
        }
    }
}