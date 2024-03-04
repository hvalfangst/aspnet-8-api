﻿using Api.Configs;

namespace Api.Utils
{
    public class ApiLoggerFactory() : ILoggerFactory
    {
        private readonly ILoggerFactory _loggerFactoryInstance = CreateLoggerFactory();

        private static ILoggerFactory CreateLoggerFactory()
        {
            var factory = LoggerFactory.Create(builder =>
            {
                ConfigureConsoleLogger(builder);
                ConfigureFileLogger(builder);
            });

            return factory;
        }

        private static void ConfigureConsoleLogger(ILoggingBuilder builder)
        {
            builder.AddSimpleConsole(options =>
            {
                options.IncludeScopes = true;
                options.SingleLine = true;
                options.TimestampFormat = "yyyy:MM:dd HH:mm:ss ";
            });
        }

        private static void ConfigureFileLogger(ILoggingBuilder builder)
        {
            var path = GetPathWithSuffix("logs/api.log");
            DirectoryExists(path);
            builder.AddProvider(new FileLoggerProvider(path));
        }

        public void AddProvider(ILoggerProvider provider)
        {
            _loggerFactoryInstance.AddProvider(provider);
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggerFactoryInstance.CreateLogger(categoryName);
        }

        public void Dispose() { }

        private static void DirectoryExists(string path)
        {
            var dir = Path.GetDirectoryName(path);

            if (!Directory.Exists(dir))
            {
                try
                {
                    Directory.CreateDirectory(dir!);
                    Console.WriteLine($"Directory created: {dir}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating log directory: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Directory already present: {dir}");
            }
        }

        private static string GetPathWithSuffix(string basePath)
        {
            var currentDate = DateTime.Now;
            var monthSuffix = currentDate.ToString("MM");
            var logFileName = Path.GetFileNameWithoutExtension(basePath);
            var logFileExtension = Path.GetExtension(basePath);

            var suffixedLogFileName = $"{logFileName}_{monthSuffix}{logFileExtension}";
            return Path.Combine(Path.GetDirectoryName(basePath)!, suffixedLogFileName);
        }
    }
}