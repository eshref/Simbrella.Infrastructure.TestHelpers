using System;
using System.Reflection;

using AutoFixture;

using FluentAssertions;


namespace Simbrella.Infrastructure.TestHelpers.Utils
{
    public static class AssertUtils
    {
        public static void AssertException<T>(Action act, string expectedMessage) where T : Exception
        {
            FluentActions.Invoking(act)
                .Should()
                .Throw<ObjectCreationException>()
                .WithInnerException<TargetInvocationException>()
                .WithInnerException<T>()
                .WithMessage(expectedMessage);
        }
    }
}