namespace VS.Mvc {

    using System;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Server.Kestrel.Core;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using Serilog.Filters.Expressions;

    public static class Program {
        public static Task Main(string[] args) {

            var config = new LoggerConfiguration();
            config.Enrich.FromLogContext().Filter.ByExcluding("StartsWith(RequestPath, '/mini-profiler')")
            .WriteTo.Console();

            var seq = Environment.GetEnvironmentVariable("SEQ_URI");
            if (seq is object) {
                config = config.WriteTo.Seq(seq, apiKey: Environment.GetEnvironmentVariable("SEQ_API_KEY"));
            }

            Log.Logger = config.CreateLogger();

            try {
                Log.Information("Starting up");
                return CreateHostBuilder(args)
                      .Build()
                      .RunAsync();

            } catch (Exception ex) {
                Log.Fatal(ex, "Application failed!");
                throw;
            } finally {
                Log.CloseAndFlush();
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            new HostBuilder()
                .UseSerilog(Log.Logger)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder
                        .UseShutdownTimeout(TimeSpan.FromSeconds(2))
                        .ConfigureKestrel(serverOptions => {
                            serverOptions.Limits.Http2.MaxStreamsPerConnection = 100;
                            serverOptions.Limits.MaxConcurrentConnections = 100;
                            serverOptions.Limits.MaxConcurrentUpgradedConnections = 100;
                            serverOptions.Limits.MaxRequestBodySize = 10 * 1024;
                            serverOptions.Limits.MinRequestBodyDataRate = new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                            serverOptions.Limits.MinResponseDataRate = new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                            serverOptions.Listen(IPAddress.Any, 5000,
                                listenOptions => {
                                    listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                                    //listenOptions.UseHttps("localhost.pfx",
                                    //    "Evert0nFC");
                                });
                            serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
                            serverOptions.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);
                        })
                        .ConfigureAppConfiguration((context, builder) => {
                            builder.AddEnvironmentVariables()
                               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                               .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: false);

                            foreach (var file in Directory.EnumerateFiles(Path.Combine(context.HostingEnvironment.ContentRootPath, "_Config"), "*.json", SearchOption.AllDirectories)) {
                                builder.AddJsonFile(file, true, false);
                            }
                        })
                    .UseStartup<Startup>();
                });
    }
}