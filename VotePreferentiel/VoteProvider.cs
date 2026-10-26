using System.Text.Json;

namespace VotePreferentiel;

public class VoteProvider(HttpClient httpClient)
{
    private const string DefaultUrl = "https://raw.githubusercontent.com/JLou/dotnet-course/refs/heads/main/eval-1/votes.json";

    public async Task<VoteModel> GetVotesAsync(string? url = null, CancellationToken cancellationToken = default)
    {
        var requestUrl = url ?? DefaultUrl;

        using var response = await httpClient.GetAsync(requestUrl, cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var voteModel = JsonSerializer.Deserialize<VoteModel>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip
        });

        if (voteModel is null)
        {
            throw new InvalidOperationException("Le fichier JSON ne contient pas de données valides.");
        }

        return voteModel;
    }
}
