using TUnit.Assertions;
using TUnit.Core;

namespace VotePreferentiel.Tests;

public class VoteCounterTests
{
    [Test]
    public async Task GetWinner_WhenExampleVotes_ThenChloeWins()
    {
        var voteModel = new VoteModel
        {
            Votes = new List<VoteBallot>
            {
                new() { Choices = new List<string> { "Alice", "Bilal", "Chloé" } },
                new() { Choices = new List<string> { "Alice", "Chloé", "Bilal" } },
                new() { Choices = new List<string> { "Bilal", "Chloé", "Alice" } },
                new() { Choices = new List<string> { "Chloé", "Bilal", "Alice" } },
                new() { Choices = new List<string> { "Chloé", "Alice", "Bilal" } }
            }
        };

        var voteCounter = new VoteCounter();
        var winner = voteCounter.GetWinner(voteModel);

        await Assert.That(winner).IsEqualTo("Chloé");
    }

    [Test]
    public async Task GetWinner_WhenSingleCandidate_ThenCandidateWins()
    {
        var voteModel = new VoteModel
        {
            Votes = new List<VoteBallot>
            {
                new() { Choices = new List<string> { "Alice" } }
            }
        };

        var voteCounter = new VoteCounter();
        var winner = voteCounter.GetWinner(voteModel);

        await Assert.That(winner).IsEqualTo("Alice");
    }

    [Test]
    public async Task GetWinner_WhenDianeIsEliminated_ThenChloeWins()
    {
        var voteModel = new VoteModel
        {
            Votes = new List<VoteBallot>
            {
                new() { Choices = new List<string> { "Alice", "Bilal", "Chloé", "Diane" } },
                new() { Choices = new List<string> { "Alice", "Bilal", "Chloé", "Diane" } },
                new() { Choices = new List<string> { "Alice", "Bilal", "Chloé", "Diane" } },
                new() { Choices = new List<string> { "Alice", "Bilal", "Chloé", "Diane" } },
                new() { Choices = new List<string> { "Bilal", "Chloé", "Alice", "Diane" } },
                new() { Choices = new List<string> { "Bilal", "Chloé", "Alice", "Diane" } },
                new() { Choices = new List<string> { "Bilal", "Chloé", "Alice", "Diane" } },
                new() { Choices = new List<string> { "Chloé", "Alice", "Bilal", "Diane" } },
                new() { Choices = new List<string> { "Chloé", "Alice", "Bilal", "Diane" } },
                new() { Choices = new List<string> { "Chloé", "Alice", "Bilal", "Diane" } },
                new() { Choices = new List<string> { "Diane", "Chloé", "Bilal", "Alice" } }
            }
        };

        var voteCounter = new VoteCounter();
        var winner = voteCounter.GetWinner(voteModel);

        await Assert.That(winner).IsEqualTo("Chloé");
    }

    [Test]
    public async Task GetWinner_WhenLastPositionTie_ThenChloeWins()
    {
        var voteModel = new VoteModel
        {
            Votes = new List<VoteBallot>
            {
                new() { Choices = new List<string> { "Alice", "Chloé", "Bilal" } },
                new() { Choices = new List<string> { "Bilal", "Chloé", "Alice" } },
                new() { Choices = new List<string> { "Chloé", "Alice", "Bilal" } },
                new() { Choices = new List<string> { "Chloé", "Bilal", "Alice" } }
            }
        };

        var voteCounter = new VoteCounter();
        var winner = voteCounter.GetWinner(voteModel);

        await Assert.That(winner).IsEqualTo("Chloé");
    }

    [Test]
    public void GetWinner_WhenBulletinIsEmpty_ThenExceptionIsThrown()
    {
        var voteModel = new VoteModel
        {
            Votes = new List<VoteBallot>
            {
                new() { Choices = new List<string>() }
            }
        };

        var voteCounter = new VoteCounter();

        try
        {
            voteCounter.GetWinner(voteModel);
            throw new Exception("Une exception attendue n'a pas été levée.");
        }
        catch (InvalidOperationException)
        {
        }
    }
}
