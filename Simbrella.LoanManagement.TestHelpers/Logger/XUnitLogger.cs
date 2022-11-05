using System;
using System.Linq;
using System.Text;

using Microsoft.Extensions.Logging;

using Xunit.Abstractions;


namespace Simbrella.LoanManagement.TestHelpers.Logger
{
    class XUnitLogger<T> : XUnitLogger, ILogger<T>
    {
        public XUnitLogger(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper, typeof(T).FullName, LogLevel.Trace, null) { }
    }


    public class XUnitLogger : ILogger
    {
        private static readonly string[] _newLineChars = { Environment.NewLine };

        private readonly string _category;

        private readonly LogLevel _minLogLevel;

        private readonly ITestOutputHelper _output;

        private DateTimeOffset? _logStart;


        public XUnitLogger(ITestOutputHelper output, string category, LogLevel minLogLevel, DateTimeOffset? logStart)
        {
            _minLogLevel = minLogLevel;
            _category = category;
            _output = output;
            _logStart = logStart;
        }

        public void Log<TState>(
            LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!this.IsEnabled(logLevel))
                return;

            // Buffer the message into a single string in order to avoid shearing the message when running across multiple threads.
            var messageBuilder = new StringBuilder();

            var timestamp = _logStart.HasValue
                ? $"{(DateTimeOffset.Now - _logStart.Value).TotalSeconds:N3}s"
                : DateTimeOffset.Now.ToString("s");

            var firstLinePrefix = $"[{timestamp}] [{_category}] {logLevel}: ";

            var lines = formatter(state, exception).Split(_newLineChars, StringSplitOptions.RemoveEmptyEntries);

            messageBuilder.AppendLine(firstLinePrefix + lines.FirstOrDefault());

            var additionalLinePrefix = "|" + new string(' ', firstLinePrefix.Length - 1);

            foreach (var line in lines.Skip(1))
            {
                messageBuilder.AppendLine(additionalLinePrefix + line);
            }

            if (exception != null)
            {
                lines = exception.ToString().Split(_newLineChars, StringSplitOptions.RemoveEmptyEntries);

                additionalLinePrefix = "| ";

                foreach (var line in lines)
                {
                    messageBuilder.AppendLine(additionalLinePrefix + line);
                }
            }

            // Remove the last line-break, because ITestOutputHelper only has WriteLine.
            var message = messageBuilder.ToString();

            if (message.EndsWith(Environment.NewLine))
            {
                message = message.Substring(0, message.Length - Environment.NewLine.Length);
            }

            try
            {
                _output.WriteLine(message);
            }
            catch (Exception)
            {
                // We could fail because we're on a background thread and our captured ITestOutputHelper is
                // busted (if the test "completed" before the background thread fired).
                // So, ignore this. There isn't really anything we can do but hope the
                // caller has additional loggers registered
            }
        }

        public bool IsEnabled(LogLevel logLevel)
            => logLevel >= _minLogLevel;

        public IDisposable BeginScope<TState>(TState state)
            => new NullScope();

        private class NullScope : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}