namespace VS.Mvc {

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Configuration;
    using Serilog;
    using System;

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
                    webBuilder.UseStartup<Startup>()
                        .ConfigureAppConfiguration((context, builder) => {
                            builder.AddEnvironmentVariables();
                        });
                });
    }
}
