using CadastroContatos.Application.Dtos;
using CadastroContatos.Domain.Domain;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace CadastroContatos.Application.Services
{
    public class ContatoService : IContatoService
    {
        private readonly IBus _bus;
        private readonly ILogger<ContatoService> _logger;

        public ContatoService(IBus bus, ILogger<ContatoService> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        public async void EnviarContatoParaFila(ContactDomain? contatoDto)
        {
            var mensagem = new ContactDomain
            {
                Id = 0,
                Name = contatoDto.Name,
                DDD = contatoDto.DDD,
                Phone = contatoDto.Phone,
                Email = contatoDto.Email
            };

            _logger.LogInformation("Sending message to queue: cadastro-queue");

            var endpoint = await _bus.GetSendEndpoint(new Uri("queue:cadastro-queue"));
            await endpoint.Send(mensagem);

            _logger.LogInformation("Message sent successfully: {MessageId}", mensagem.Id);
        }
    }
}