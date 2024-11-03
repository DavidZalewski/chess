using NUnit.Framework;
using Chess.Services;
using Chess.Controller;
using System;

namespace Tests.Services
{
    [TestFixture]
    public class ActionSequenceTests
    {
        private ActionSequence _actionSequence;
        private GameController _gameController;

        [SetUp]
        public void Setup()
        {
            _actionSequence = new ActionSequence();
            _gameController = new GameController(new Chess.Board.ChessBoard()); // Assuming GameController can be instantiated in this way
        }

        [Test]
        public void AddActionInSequence_AddsActionToStack()
        {
            // Arrange
            Action testAction = () => Console.WriteLine("Test Action");

            // Act
            _actionSequence.AddActionInSequence(testAction);

            // Assert
            Assert.That(_actionSequence.Actions.Peek(), Is.EqualTo(testAction));
        }

        // ... Additional tests for PlayActionSequence, IsActionInSequence, and ContainsRuleSet ...

        [Test]
        public void ContainsRuleSet_ReturnsCorrectResult()
        {
            // Arrange
            _gameController._sequence = _actionSequence; // Assuming _sequence is accessible for testing
            Action testAction = () => Console.WriteLine("Test Rule Set");
            _actionSequence.AddActionInSequence(testAction);

            // Act & Assert
            Assert.IsTrue(_gameController.ContainsRuleSet("Test Rule Set")); // Assuming ContainsRuleSet accepts action names
            Assert.IsFalse(_gameController.ContainsRuleSet("Non-Existent Rule Set"));
        }
    }
}