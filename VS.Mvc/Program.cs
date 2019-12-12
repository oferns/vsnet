namespace VS.Mvc {

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Configuration;
    using Serilog;
    using System;
    using Microsoft.AspNetCore.Server.Kestrel.Core;
    using System.Net;

    public class Program {
        public static void Main(string[] args) {
            var config = new LoggerConfiguration();
            config.Enrich.FromLogContext()
            .WriteTo.Console();

            var seq = Environment.GetEnvironmentVariable("SEQ_URL");
            if (seq is object) {

                config = config.WriteTo.Seq(seq);
            }

            Log.Logger = config.CreateLogger();

            try {
                Log.Information("Starting up");
                CreateHostBuilder(args).Build().Run();
            } catch (Exception ex) {
                Log.Fatal(ex, "Application start-up failed");
            } finally {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => {

                    webBuilder.ConfigureKestrel(serverOptions => {
                        serverOptions.Limits.Http2.MaxStreamsPerConnection = 100;
                        serverOptions.Limits.MaxConcurrentConnections = 100;
                        serverOptions.Limits.MaxConcurrentUpgradedConnections = 100;
                        serverOptions.Limits.MaxRequestBodySize = 10 * 1024;
                        serverOptions.Limits.MinRequestBodyDataRate =
                           new MinDataRate(bytesPerSecond: 100,
                               gracePeriod: TimeSpan.FromSeconds(10));
                        serverOptions.Limits.MinResponseDataRate =
                           new MinDataRate(bytesPerSecond: 100,
                               gracePeriod: TimeSpan.FromSeconds(10));
                        //  serverOptions.Listen(IPAddress.Loopback, 5000);
                        serverOptions.Listen(IPAddress.Loopback, 5000,
                            listenOptions => {
                                listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                                listenOptions.UseHttps("localhost.pfx",
                                    "Evert0nFC");
                            });
                        serverOptions.Limits.KeepAliveTimeout =
                            TimeSpan.FromMinutes(2);
                        serverOptions.Limits.RequestHeadersTimeout =
                            TimeSpan.FromMinutes(1);
                    })        
                    .UseStartup<Startup>()
                    .ConfigureAppConfiguration((context, builder) => {
                            builder.AddEnvironmentVariables();
                    });
                });
    }
}
