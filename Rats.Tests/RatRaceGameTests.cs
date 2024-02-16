using System;
using Xunit;

namespace Rats.Tests
{
    public class RatRaceGameTests
    {
        [Fact]
        public void IsValidMap_ShouldValidate()
        {
            RatRaceGame game = new RatRaceGame(new TestConsoleRetriever());
            //Arrange
            bool expected = true;

            //Act
            bool actual = game.IsValidMap(Environment.CurrentDirectory + "\\" + "maze1.maze");

            //Assert
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void LoadMap_ShouldLoad()
        {
            // Testing this type of void method implies testing the state change of the object targeted by the method
            RatRaceGame game = new RatRaceGame(new TestConsoleRetriever());

            char[,] initial = game.Map;
            game.LoadMap(Environment.CurrentDirectory + "\\" + "maze1.maze");
            var actual = game.Map;

            Assert.NotEqual(initial, actual);
        }
        [Fact]
        public void AddRatOnMap_ShouldAdd()
        {
            RatRaceGame game = new RatRaceGame(new TestConsoleRetriever());

            game.LoadMap(Environment.CurrentDirectory + "\\" + "maze1.maze");
            char[,] initial = game.Map;

            game.AddRatOnMap(game.Pinky);

            var actual = game.Map;
            Assert.Equal(initial, actual);
        }
        [Fact]
        public void CountSprouts_ShouldCount()
        {
            RatRaceGame game = new RatRaceGame(new TestConsoleRetriever());
            game.LoadMap(Environment.CurrentDirectory + "\\" + "maze1.maze");

            int expected = 3;
            int actual = game.CountSprouts();

            Assert.Equal(expected, actual);
        }
        [Theory]
        [InlineData('#', false)]
        [InlineData('.', true)]
        [InlineData('@', true)]
        [InlineData('P', true)]
        [InlineData('B', true)]
        public void IsValidNextSpace_ShouldValidate(char nextSpace, bool expected)
        {
            // No need for other test cases as other characters would not pass ValidateMap
            RatRaceGame game = new RatRaceGame(new TestConsoleRetriever());
            bool actual = game.IsValidNextSpace(nextSpace);
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void ModifyRatScore_ShouldModify()
        {
            RatRaceGame game = new RatRaceGame(new TestConsoleRetriever());
            int expected = 1;

            game.ModifyRatScore('@', game.Pinky);
            int actual = game.Pinky.Score;

            Assert.Equal(expected, actual);
        }
    }
}
