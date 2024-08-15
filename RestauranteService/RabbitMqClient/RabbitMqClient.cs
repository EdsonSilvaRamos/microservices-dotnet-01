using RabbitMQ.Client;
using RestauranteService.Dtos;
using System.Text;
using System.Text.Json;

namespace RestauranteService.RabbitMqClient
{
    public class RabbitMqClient : IRabbitMqClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqClient(IConfiguration configuration)
        {
            _configuration = configuration;
            // Abrindo conexão através do new ConnectionFactory(). Responsável por abrir conexões entre um cliente e o RabbitMQ.
            _connection = new ConnectionFactory()
            {
                //Necessário passar o HostName para se conectar, o RabbitMqHost, e a porta da conexão.
                //E para criar a conexão é com o método CreateConnection().
                HostName = _configuration["RabbitMqHost"],
                Port = Int32.Parse(_configuration["RabbitMqPort"]),
            }.CreateConnection();

            // Abrir o canal de conexão através do método CreateModel()
            _channel = _connection.CreateModel();

            // Para realizar a comunicação e trafegar os dados pelos canais é necessário declarar algumas "exchanges"
            // É usado o método ExchangeDeclare()

            // A exchange vai ser criada no momento de trigger, significa que quando essa ação por designada para acontecer essa exchange vai ser gerada

            // O tipo Fanout informa que conseguimos usar a exchange para a comunicação através do AMQP.
            // O AMQP (Advanced Message Queuing Protocol ou, em português, "Protocolo Avançado de Filas de Mensagem")
            // é o protocolo que RabbitMQ utiliza para a comunicação entre serviços.
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

        }

        public void PublicaRestaurante(RestauranteReadDto restauranteReadDto)
        {
            // Implementação para disparar a mensagem para a fila do RabbitMQ
            var mensagem = JsonSerializer.Serialize(restauranteReadDto);
            var body = Encoding.UTF8.GetBytes(mensagem);
            _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);
        }
    }
}
