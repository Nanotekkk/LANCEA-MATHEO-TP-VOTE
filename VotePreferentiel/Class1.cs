using System.Text.Json.Serialization;

namespace VotePreferentiel;

public class VoteModel
{
    [JsonPropertyName("Votes")]
    public List<VoteBallot> Votes { get; set; } = new();
}

public class VoteBallot
{
    [JsonPropertyName("Choices")]
    public List<string> Choices { get; set; } = new();
}

public interface IVoteCounter
{
    HashSet<string> GetCandidates(VoteModel voteModel);
    string GetWinner(VoteModel voteModel);
    List<string> GetAllFirstChoices(VoteModel voteModel);
    IDictionary<string, int> CountFirstChoices(List<string> firstChoices);
    string[] GetCandidatesWithLeastVotes(IDictionary<string, int> firstChoiceCounts);
}

public class VoteCounter : IVoteCounter
{
    public HashSet<string> GetCandidates(VoteModel voteModel)
    {
        if (voteModel is null)
        {
            throw new ArgumentNullException(nameof(voteModel));
        }

        var candidates = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var ballot in voteModel.Votes)
        {
            if (ballot is null || ballot.Choices is null || ballot.Choices.Count == 0)
            {
                throw new InvalidOperationException("Un bulletin est vide.");
            }

            foreach (var choice in ballot.Choices)
            {
                if (!string.IsNullOrWhiteSpace(choice))
                {
                    candidates.Add(choice.Trim());
                }
            }
        }

        return candidates;
    }

    public string GetWinner(VoteModel voteModel)
    {
        if (voteModel is null)
        {
            throw new ArgumentNullException(nameof(voteModel));
        }

        if (voteModel.Votes is null || voteModel.Votes.Count == 0)
        {
            throw new InvalidOperationException("Aucun vote n'a été trouvé.");
        }

        var remainingCandidates = GetCandidates(voteModel);

        while (remainingCandidates.Count > 1)
        {
            var counts = remainingCandidates
                .ToDictionary(candidate => candidate, _ => 0, StringComparer.OrdinalIgnoreCase);

            foreach (var ballot in voteModel.Votes)
            {
                if (ballot is null || ballot.Choices is null || ballot.Choices.Count == 0)
                {
                    throw new InvalidOperationException("Un bulletin est vide.");
                }

                var preferredCandidate = ballot.Choices
                    .Where(choice => !string.IsNullOrWhiteSpace(choice))
                    .Select(choice => choice.Trim())
                    .FirstOrDefault(choice => remainingCandidates.Contains(choice));

                if (preferredCandidate is null)
                {
                    continue;
                }

                counts[preferredCandidate] += 1;
            }

            var minimumVotes = counts.Values.Min();
            var candidatesToEliminate = counts
                .Where(pair => pair.Value == minimumVotes)
                .Select(pair => pair.Key)
                .ToArray();

            if (candidatesToEliminate.Length == 0)
            {
                break;
            }

            foreach (var candidate in candidatesToEliminate)
            {
                remainingCandidates.Remove(candidate);
            }
        }

        if (remainingCandidates.Count == 0)
        {
            throw new InvalidOperationException("Aucun candidat n'a pu être déterminé.");
        }

        return remainingCandidates.Single();
    }

    public List<string> GetAllFirstChoices(VoteModel voteModel)
    {
        if (voteModel is null)
        {
            throw new ArgumentNullException(nameof(voteModel));
        }

        var firstChoices = new List<string>();

        foreach (var ballot in voteModel.Votes)
        {
            if (ballot is null || ballot.Choices is null || ballot.Choices.Count == 0)
            {
                throw new InvalidOperationException("Un bulletin est vide.");
            }

            var firstChoice = ballot.Choices.FirstOrDefault(choice => !string.IsNullOrWhiteSpace(choice));

            if (string.IsNullOrWhiteSpace(firstChoice))
            {
                throw new InvalidOperationException("Le premier choix d'un bulletin est vide.");
            }

            firstChoices.Add(firstChoice.Trim());
        }

        return firstChoices;
    }

    public IDictionary<string, int> CountFirstChoices(List<string> firstChoices)
    {
        if (firstChoices is null)
        {
            throw new ArgumentNullException(nameof(firstChoices));
        }

        var counts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (var firstChoice in firstChoices)
        {
            if (string.IsNullOrWhiteSpace(firstChoice))
            {
                throw new InvalidOperationException("Un bulletin contient un choix vide.");
            }

            var normalized = firstChoice.Trim();
            counts[normalized] = counts.TryGetValue(normalized, out var count) ? count + 1 : 1;
        }

        return counts;
    }

    public string[] GetCandidatesWithLeastVotes(IDictionary<string, int> firstChoiceCounts)
    {
        if (firstChoiceCounts is null)
        {
            throw new ArgumentNullException(nameof(firstChoiceCounts));
        }

        if (firstChoiceCounts.Count == 0)
        {
            return Array.Empty<string>();
        }

        var minimumVotes = firstChoiceCounts.Values.Min();

        return firstChoiceCounts
            .Where(pair => pair.Value == minimumVotes)
            .Select(pair => pair.Key)
            .ToArray();
    }
}
