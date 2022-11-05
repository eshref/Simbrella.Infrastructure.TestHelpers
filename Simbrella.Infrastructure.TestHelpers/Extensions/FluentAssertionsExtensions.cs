using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

using FluentAssertions;
using FluentAssertions.Equivalency;


namespace Simbrella.Infrastructure.TestHelpers.Extensions
{
    public static class FluentAssertionsExtensions
    {
        public static EquivalencyAssertionOptions<T> AllowDateTimeDifference<T>(this EquivalencyAssertionOptions<T> options, TimeSpan precision)
        {
            return options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, precision)).WhenTypeIs<DateTime>();
        }

        public static void ShouldBe(
            this HttpRequestHeaders actualHeaders,
            Dictionary<string, string> expectedHeaders)
        {
            foreach (var (key, value) in expectedHeaders)
            {
                actualHeaders.TryGetValues(key, out var headerValues);

                headerValues.Should().ContainSingle().Which.Should().Be(value);
            }
        }
    }
}