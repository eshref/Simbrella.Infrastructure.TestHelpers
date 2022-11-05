using System;

using Microsoft.Extensions.Logging;

using Xunit.Abstractions;


namespace Simbrella.LoanManagement.TestHelpers.Logger
{
    public class XUnitLoggerProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper _output;
        private readonly LogLevel _minLevel;
        private readonly DateTimeOffset? _logStart;

        public XUnitLoggerProvider(ITestOutputHelper output)
            : this(output, LogLevel.Trace)
        {
        }

        public XUnitLoggerProvider(ITestOutputHelper output, LogLevel minLevel)
            : this(output, minLevel, null)
        {
        }

        public XUnitLoggerProvider(ITestOutputHelper output, LogLevel minLevel, DateTimeOffset? logStart)
        {
            _output = output;
            _minLevel = minLevel;
            _logStart = logStart;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new XUnitLogger(_output, categoryName, _minLevel, _logStart);
        }

        public void Dispose()
        {
        }
    }
}