using RegionalContacts.Domain.Dto.Contato;
using RegionalContacts.Integration.Tests.Setup;
using System.Net;
using System.Net.Http.Json;

namespace RegionalContacts.Integration.Tests
{
    public class ContactsControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>, IClassFixture<DockerFixture>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public ContactsControllerTests(CustomWebApplicationFactory<Program> factory, DockerFixture dockerFixture)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetContacts_ReturnsSuccessStatusCode()
        {
            var newContact = new ContactCreateDto
            {
                Name = "John Doe Jr Consulta",
                Email = "john.doe.consulta@example.com",
                PhoneNumber = "77776699",
                RegionNumber = 11,
            };

            var response = await _client.PostAsJsonAsync("/api/contact", newContact);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var badRequestContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"BadRequest Content: {badRequestContent}");
            }

            response.EnsureSuccessStatusCode();

            var getResponse = await _client.GetFromJsonAsync<List<ContactDto>>("/api/contact");

            Assert.NotNull(getResponse);
            Assert.Contains(getResponse, c =>
                c.Name == newContact.Name &&
                c.Email == newContact.Email &&
                c.PhoneNumber == newContact.PhoneNumber &&
                c.RegionNumber == newContact.RegionNumber
            );
        }

        [Fact]
        public async Task PostContact_AddContact()
        {
            var newContact = new ContactCreateDto
            {
                Name = "John Doe Jr Novo",
                Email = "john.doe.novo@example.com",
                PhoneNumber = "55556699",
                RegionNumber = 11,
            };

            var response = await _client.PostAsJsonAsync("/api/contact", newContact);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var badRequestContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"BadRequest Content: {badRequestContent}");
            }

            response.EnsureSuccessStatusCode();

            var getResponse = await _client.GetFromJsonAsync<List<ContactDto>>("/api/contact");

            Assert.NotNull(getResponse);
            Assert.Contains(getResponse, c =>
                c.Name == newContact.Name &&
                c.Email == newContact.Email &&
                c.PhoneNumber == newContact.PhoneNumber &&
                c.RegionNumber == newContact.RegionNumber
            );
        }

        [Fact]
        public async Task PutContact_UpdateContact()
        {
            // Cria um contato para testar a atualização
            var newContact = new ContactCreateDto
            {
                Name = "John Doe Jr Atualização",
                Email = "john.doe.atualizacao@example.com",
                PhoneNumber = "44446699",
                RegionNumber = 11,
            };

            var postResponse = await _client.PostAsJsonAsync("/api/contact", newContact);
            postResponse.EnsureSuccessStatusCode();

            var contactList = await _client.GetFromJsonAsync<List<ContactDto>>("/api/contact");

            // Atualiza o contato
            ContactUpdateDto updatedContact = new ContactUpdateDto();
            updatedContact.Name = "John Doe Updated";
            updatedContact.Email = "john.doe_upd@example.com";
            updatedContact.PhoneNumber = "99999999";
            updatedContact.RegionNumber = 11;

            var putResponse = await _client.PutAsJsonAsync($"/api/contact/{contactList.FirstOrDefault().Id}", updatedContact);
            putResponse.EnsureSuccessStatusCode();

            // Verifica se o contato foi atualizado
            var getResponse = await _client.GetFromJsonAsync<List<ContactDto>>("/api/contact");
            Assert.NotNull(getResponse);

            var updatedContactInList = getResponse.FirstOrDefault(c => c.Email == updatedContact.Email);
            Assert.NotNull(updatedContactInList);
            Assert.Equal(updatedContact.Name, updatedContactInList.Name);
            Assert.Equal(updatedContact.PhoneNumber, updatedContactInList.PhoneNumber);
        }

        [Fact]
        public async Task DeleteContact_RemovesContact()
        {
            // Cria um contato para testar a exclusão
            var newContact = new ContactCreateDto
            {
                Name = "John Doe Jr Excluir",
                Email = "john.doe.excluir@example.com",
                PhoneNumber = "22226699",
                RegionNumber = 11,
            };

            var postResponse = await _client.PostAsJsonAsync("/api/contact", newContact);
            postResponse.EnsureSuccessStatusCode();

            // Obtem o contato criado
            var contactList = await _client.GetFromJsonAsync<List<ContactDto>>("/api/contact");

            // Exclui o contato
            var deleteResponse = await _client.DeleteAsync($"/api/contact/{contactList.Find(c => c.Email == newContact.Email).Id}");
            deleteResponse.EnsureSuccessStatusCode();

            // Verifica se o contato foi removido
            var getResponse = await _client.GetFromJsonAsync<List<ContactDto>>("/api/contact");
            Assert.NotNull(getResponse);

            Assert.DoesNotContain(getResponse, c => c.Email == newContact.Email);
        }
    }
}
