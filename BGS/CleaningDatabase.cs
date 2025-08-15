using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Dapper; 
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace BGS
{
    public class CleaningDatabase:BackgroundService
    {
        private readonly ILogger<CleaningDatabase> _logger;
        private readonly IConfiguration _configuration;

        public CleaningDatabase(ILogger<CleaningDatabase> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Serviço de limpeza de filmes iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Iniciando a limpeza dos conteúdos.");

                    var connectionString = _configuration.GetConnectionString("DefaultConnection");

                    using (var connection = new SqlConnection(connectionString))
                    {
                        await connection.OpenAsync(stoppingToken);

                        await connection.ExecuteAsync("DeletarConteudosOrfaos", commandType: CommandType.StoredProcedure);
                        _logger.LogInformation("Procedure 'DeletarConteudosOrfaos' executada com sucesso!");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ocorreu um erro ao executar a limpeza de filmes.");
                }

                _logger.LogInformation("Próxima execução em 24 horas. Serviço irá dormir agora.");
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
