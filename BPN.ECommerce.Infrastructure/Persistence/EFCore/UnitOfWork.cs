using BPN.ECommerce.Application;

namespace BPN.ECommerce.Infrastructure.Persistence.EFCore;

public class UnitOfWork : IUnitOfWork
{
    private readonly BpnDbContext _bpnDbContext;

    public UnitOfWork(BpnDbContext bpnDbContext)
    {
        _bpnDbContext = bpnDbContext;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _bpnDbContext.SaveChangesAsync(cancellationToken);
    }
}