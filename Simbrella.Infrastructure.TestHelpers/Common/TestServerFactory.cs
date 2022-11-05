using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Simbrella.Infrastructure.TestHelpers.Logger;

using Xunit.Abstractions;


namespace Simbrella.Infrastructure.TestHelpers.Common
{
    public static class TestServerFactory
    {
        public static TestServer Create<TStartup>(ITestOutputHelper testOutputHelper) where TStartup : class
        {
            return TestServerFactory.internalCreate<TStartup>("localhost", "localhost", testOutputHelper, collection => { });
        }

        public static TestServer Create<TStartup>(ITestOutputHelper testOutputHelper, Action<IServiceCollection> configureTestServices) where TStartup : class
        {
            return TestServerFactory.internalCreate<TStartup>("localhost", "localhost", testOutputHelper, configureTestServices);
        }

        public static TestServer Create<TStartup>(string dbConnectionString, ITestOutputHelper testOutputHelper) where TStartup : class
        {
            return TestServerFactory.internalCreate<TStartup>(dbConnectionString, "localhost", testOutputHelper, collection => { });
        }

        public static TestServer Create<TStartup>(string dbConnectionString, ITestOutputHelper testOutputHelper, Action<IServiceCollection> configureTestServices) where TStartup : class
        {
            return TestServerFactory.internalCreate<TStartup>(dbConnectionString, "localhost", testOutputHelper, configureTestServices);
        }

        public static TestServer Create<TStartup>(string dbConnectionString, string kafkaBootstrapServers,
            ITestOutputHelper testOutputHelper) where TStartup : class
        {
            return TestServerFactory.internalCreate<TStartup>(dbConnectionString, kafkaBootstrapServers, testOutputHelper, collection => { });
        }

        public static TestServer Create<TStartup>(string dbConnectionString, string kafkaBootstrapServers,
            ITestOutputHelper testOutputHelper, Action<IServiceCollection> configureTestServices) where TStartup : class
        {
            return TestServerFactory.internalCreate<TStartup>(dbConnectionString, kafkaBootstrapServers, testOutputHelper, configureTestServices);
        }


        private static TestServer internalCreate<TStartup>(
            string dbConnectionString, 
            string kafkaBootstrapServers,
            ITestOutputHelper testOutputHelper, 
            Action<IServiceCollection> configureTestServices) where TStartup : class
        {
            return new TestServer(
                new WebHostBuilder()
                    .ConfigureAppConfiguration(
                        (context, builder) =>
                        {
                            builder.AddJsonFile("appsettings.test.json");

                            builder.AddInMemoryCollection(
                                new[]
                                {
                                    new KeyValuePair<string, string>("DbConnection:Default", dbConnectionString),
                                    new KeyValuePair<string, string>("Kafka:BootstrapServers", kafkaBootstrapServers)
                                });
                        })
                    .UseStartup<TStartup>()
                    .ConfigureTestServices(configureTestServices)
                    .ConfigureLogging(
                        loggingBuilder =>
                        {
                            loggingBuilder.Services.AddSingleton<ILoggerProvider>(serviceProvider => new XUnitLoggerProvider(testOutputHelper));
                        }));
        }
    }
}