using RestauranteService.Dtos;
using System.Text;
using System.Text.Json;

namespace RestauranteService.ItemServiceHttpClient
{
    public class ItemServiceHttpClient : IItemServiceHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ItemServiceHttpClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async void EnviaRestauranteParaItemService(RestauranteReadDto restauranteReadDto)
        {
            var conteudoHttpClient = new StringContent
            (
                JsonSerializer.Serialize(restauranteReadDto),
                Encoding.UTF8,
                "application/json"
            );

            await _httpClient.PostAsync(_configuration["ItemService"], conteudoHttpClient);
        }
    }
}
