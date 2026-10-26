using VotePreferentiel;

namespace VotePreferentiel.Worker;

public class Worker(ILogger<Worker> logger, VoteProvider voteProvider, IVoteCounter voteCounter) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            logger.LogInformation("Téléchargement du fichier JSON depuis GitHub...");
            var voteModel = await voteProvider.GetVotesAsync(cancellationToken: stoppingToken);

            var winner = voteCounter.GetWinner(voteModel);
            logger.LogInformation("Le gagnant est : {Winner}", winner);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur s'est produite lors du calcul du gagnant.");
            throw;
        }
    }
}
