using EngageGov.Application.Interfaces;
using EngageGov.Domain.Entities;
using EngageGov.Infrastructure.Data;

namespace EngageGov.Infrastructure.Repositories;

public class TemplateRepository : Repository<Template>, ITemplateRepository
{
    private readonly EngageGovDbContext _ctx;
    public TemplateRepository(EngageGovDbContext context) : base(context)
    {
        _ctx = context;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        const int maxAttempts = 3;
        for (var attempt = 1; ; attempt++)
        {
            try
            {
                await _ctx.SaveChangesAsync(cancellationToken);
                return;
            }
            catch (Exception ex) when (IsTransient(ex))
            {
                if (attempt >= maxAttempts) throw;
                await Task.Delay(200 * attempt, cancellationToken);
            }
        }
    }

    private static bool IsTransient(Exception ex)
    {
        if (ex is OperationCanceledException) return true;
        if (ex is TimeoutException) return true;
        if (ex.Message != null && ex.Message.IndexOf("Timeout", StringComparison.OrdinalIgnoreCase) >= 0) return true;
        if (ex.InnerException != null && ex.InnerException.Message != null && ex.InnerException.Message.IndexOf("Timeout", StringComparison.OrdinalIgnoreCase) >= 0) return true;
        return false;
    }
}
