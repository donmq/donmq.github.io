using Machine_API._Accessor;

namespace Machine_API._Service.service;

public class BaseServices
{
    protected readonly IMachineRepositoryAccessor _repoAccessor;
    protected readonly IMTRepositoryAccessor _repoMTAccessor;
    protected readonly ISAPRepositoryAccessor _repoSAPAccessor;

    protected readonly IConfiguration _configuration;
    protected readonly string sqlDefaultConnection;
    protected readonly string sqlMTConnection;
    protected readonly string sqlSAPConnection;

    public BaseServices(IMachineRepositoryAccessor repoAccessor, IConfiguration configuration, IMTRepositoryAccessor repoMTAccessor, ISAPRepositoryAccessor repoSAPAccessor)
    {
        _repoAccessor = repoAccessor;
        _repoMTAccessor = repoMTAccessor;
        _repoSAPAccessor = repoSAPAccessor;
        _configuration = configuration;
        string area = _configuration.GetSection("AppSettings:Area").Value;
        string factory = configuration.GetSection("AppSettings:Factory").Value;
        sqlDefaultConnection = _configuration.GetConnectionString($"{factory}_{area}_DefaultConnection");
        sqlMTConnection = _configuration.GetConnectionString($"{factory}_{area}_MTConnection");
        sqlSAPConnection = _configuration.GetConnectionString($"{factory}_{area}_SAPConnection");
    }

}