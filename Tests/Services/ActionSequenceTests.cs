using Chess.Services;
using NUnit.Framework;
using System.Collections.Generic;

namespace Tests.Services
{
    [Category("Unit")]
    [Parallelizable(ParallelScope.Self)]
    internal class ActionSequenceTests
    {
        private ActionSequence _actionSequence;

        [SetUp]
        public void Setup()
        {
            _actionSequence = new ActionSequence();
        }

        [Test]
        public void AddActionInSequence_AddsActionToStack()
        {
            // Arrange
            Action mockAction = () => { /* some action */ };

            // Act
            _actionSequence.AddActionInSequence(mockAction);

            // Assert
            Assert.That(_actionSequence.Actions.Count, Is.EqualTo(1));
            Assert.That(_actionSequence.Actions.Peek(), Is.SameAs(mockAction));
        }

        [Test]
        public void PlayActionSequence_InvokesAllActions()
        {
            // Arrange
            int invoked = 0;
            Action mockAction1 = () => { invoked += 1; };
            Action mockAction2 = () => { invoked += 1; };

            _actionSequence.AddActionInSequence(mockAction1);
            _actionSequence.AddActionInSequence(mockAction2);

            // Act
            _actionSequence.PlayActionSequence();

            // Assert
            Assert.That(invoked, Is.EqualTo(2));
        }

        [Test]
        public void IsActionInSequence_ReturnsTrueIfActionExists()
        {
            // Arrange
            Action mockAction = () => { /* some action */ };

            _actionSequence.AddActionInSequence(mockAction);

            // Act
            bool result = _actionSequence.IsActionInSequence(mockAction);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsActionInSequence_ReturnsFalseIfActionDoesNotExist()
        {
            // Arrange
            Action mockAction = () => { /* some action */ };

            // Act
            bool result = _actionSequence.IsActionInSequence(mockAction);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}