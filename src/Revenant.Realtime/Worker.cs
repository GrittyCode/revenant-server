namespace Revenant.Realtime;

/// <summary>
/// 실시간 서버 진입점 자리. 5주차에 Socket + Pipelines 기반 TCP 리스너로 교체한다.
/// </summary>
public sealed partial class Worker(ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        LogStarted(logger);
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    // LoggerMessage 소스 생성기: 문자열 포맷/박싱 없이 로그를 남긴다 (CA1848)
    [LoggerMessage(Level = LogLevel.Information, Message = "Realtime server placeholder started")]
    private static partial void LogStarted(ILogger logger);
}
