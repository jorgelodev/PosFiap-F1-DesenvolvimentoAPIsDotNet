using Microsoft.Identity.Client;

namespace FiapStore.Logging
{
    public class CustomLogger : ILogger
    {
        private readonly string _loggerName;
        private readonly CustomLoggerProviderConfiguration _configuration;

        public CustomLogger(string nome, CustomLoggerProviderConfiguration configuration)
        {
            _loggerName = nome;
            _configuration = configuration;
        }

        public IDisposable BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var mensagem = string.Format($"{logLevel}: {eventId} " +
                $"- {formatter(state, exception)}");

            EscreverTextoNoArquivo(mensagem);
        }

        private void EscreverTextoNoArquivo(string mensagem)
        {
            var caminhoArquivo = @$"C:\Users\Jorge\Documents\Estudos\Alura\FiapStore\FiapStore\bin\Log {DateTime.Now:yyyy-MM-dd}.txt";

            if (!File.Exists(caminhoArquivo))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(caminhoArquivo));
                File.Create(caminhoArquivo).Dispose();
            }

            using StreamWriter streamWriter = new StreamWriter(caminhoArquivo, true);
            
            streamWriter.WriteLine($"{DateTime.Now:dd/MM/yyy HH:mm:ss} - {mensagem}");
        }
    }
}
