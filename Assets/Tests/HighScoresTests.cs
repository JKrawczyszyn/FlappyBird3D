using Zenject;
using NUnit.Framework;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Entry.Models;
using Entry.Services;

[TestFixture]
public class HighScoresTests : ZenjectUnitTestFixture
{
    private IFileService fileService;
    private HighScoresService highScoresService;

    private DateTime dateTime;

    [SetUp]
    public void SetUp()
    {
        fileService = Substitute.For<IFileService>();
        fileService.Load<Score>(Arg.Any<string>()).Returns(new List<Score>());

        Container.Bind<IFileService>().FromInstance(fileService);
        Container.Bind<HighScoresService>().AsSingle();

        highScoresService = Container.Resolve<HighScoresService>();

        dateTime = new DateTime(2021, 1, 1);
    }

    [Test]
    public void IsScoreHighEnough_ValidScore_ReturnsTrue()
    {
        var result = highScoresService.IsScoreHighEnough(10);

        Assert.That(result, Is.True);
    }

    [Test]
    public void AddHighScore_ValidScore_ScoreAdded()
    {
        var score = new Score(10, dateTime, "Alice");

        highScoresService.AddHighScore(score);

        fileService.Received().Save(Arg.Any<string>(), Arg.Any<List<Score>>());
    }

    [Test]
    public void GetHighScores_ScoresExist_ReturnsScores()
    {
        var scores = new List<Score> { new(10, dateTime, "Alice"), new(20, dateTime, "Bob") };

        fileService.Load<Score>(Arg.Any<string>()).Returns(scores);

        var result = highScoresService.GetHighScores();

        Assert.That(result, Is.EquivalentTo(scores));
    }

    [Test]
    public void LoadIfShould_ScoresNotLoaded_LoadsScores()
    {
        var scores = new List<Score> { new(10, dateTime, "Alice"), new(20, dateTime, "Bob") };

        fileService.Load<Score>(Arg.Any<string>()).Returns(scores);

        highScoresService.IsScoreHighEnough(10);

        fileService.Received().Load<Score>(Arg.Any<string>());
    }

    [Test]
    public void AddHighScore_ListIsFullAndScoreIsNotHighEnough_ScoreNotAdded()
    {
        var lowScore = new Score(5, dateTime, "LowScore");
        var highScores = new List<Score>(Enumerable.Repeat(new Score(10, dateTime, "HighScore"), 20));

        fileService.Load<Score>(Arg.Any<string>()).Returns(highScores);

        highScoresService.AddHighScore(lowScore);

        fileService.DidNotReceive().Save(Arg.Any<string>(), Arg.Any<List<Score>>());
    }

    [Test]
    public void AddHighScore_NewScore_SortedInDescendingOrder()
    {
        var newScore = new Score(15, dateTime, "NewScore");
        var highScores = new List<Score> { new(10, dateTime, "Alice"), new(20, dateTime, "Bob") };

        fileService.Load<Score>(Arg.Any<string>()).Returns(highScores);

        highScoresService.AddHighScore(newScore);

        fileService.Received()
            .Save(Arg.Any<string>(),
                  Arg.Is<List<Score>>(scores => scores[0].value == 20
                                                && scores[1].value == 15
                                                && scores[2].value == 10));
    }

    [Test]
    public void AddHighScore_ScoresAreSame_SortedInTimeOrder()
    {
        var newScore = new Score(10, dateTime, "NewScore");
        var highScores = new List<Score> { new(10, dateTime + TimeSpan.FromSeconds(1), "Alice"), new(10, dateTime - TimeSpan.FromSeconds(1), "Bob") };

        fileService.Load<Score>(Arg.Any<string>()).Returns(highScores);

        highScoresService.AddHighScore(newScore);

        fileService.Received()
            .Save(Arg.Any<string>(),
                  Arg.Is<List<Score>>(scores => scores[0].name == "Bob"
                                                && scores[1].name == "NewScore"
                                                && scores[2].name == "Alice"));
    }

    [Test]
    public void AddHighScore_MoreThanMaxScores_ListCappedAtMaxScores()
    {
        var newScore = new Score(15, dateTime, "NewScore");
        var highScores = new List<Score>(Enumerable.Repeat(new Score(10, dateTime, "HighScore"), 20));

        fileService.Load<Score>(Arg.Any<string>()).Returns(highScores);

        highScoresService.AddHighScore(newScore);

        fileService.Received().Save(Arg.Any<string>(), Arg.Is<List<Score>>(scores => scores.Count == 20));
    }
}
