using Microsoft.AspNetCore.Mvc.Testing;
using curso.api;
using System.Net.Cache;
using System.Net.Http;
using curso.api.Models.Usuarios;
using Newtonsoft.Json;
using Xunit;
using System.Text;
using System.Net;
using System;
using Xunit.Abstractions;
using System.Threading.Tasks;
using AutoBogus;
using System.ComponentModel.DataAnnotations;

namespace curso.api.tests.Integrations.Controllers
{
    public class UsuarioControllerTests : IClassFixture<WebApplicationFactory<Startup>>, IAsyncLifetime
    {   
        private readonly WebApplicationFactory<Startup> _factory; //simula um dotnet build 
        private readonly HttpClient _httpClient; //Agir como cliente para poder navegar nas rotas
        private readonly ITestOutputHelper _output;
        protected RegistroViewModelInput RegistroViewModelInput;

        public UsuarioControllerTests(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient();
            _output = output;
        }

        [Fact]
        public async Task Cadastrar_InformandoUsuarioESenhaExistentes_DeveRetornarSucesso()
        {   
            //Arrange
            RegistroViewModelInput = new AutoFaker<RegistroViewModelInput>()
                                         .RuleFor(p => p.Email, faker => faker.Person.Email);

            StringContent content = new StringContent(JsonConvert.SerializeObject(RegistroViewModelInput), Encoding.UTF8, "application/json");

            //Act
            var httpClientRequest = await _httpClient.PostAsync("api/v1/usuario/registrar", content);
    
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.Created, httpClientRequest.StatusCode);
        }

        [Fact]
        public async Task Logar_InformandoUsuarioESenhaExistentes_DeveRetornarSucesso()
        {   
            //Arrange
            var loginViewModelInput = new LoginViewModelInput
            {
                Login = RegistroViewModelInput.Login,
                Senha = RegistroViewModelInput.Senha
            };
            StringContent content = new StringContent(JsonConvert.SerializeObject(loginViewModelInput), Encoding.UTF8, "application/json");

            //Act
            var httpClientRequest = await _httpClient.PostAsync("api/v1/usuario/logar", content);
            
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, httpClientRequest.StatusCode);
        }

        public async Task InitializeAsync()
        {
            await Cadastrar_InformandoUsuarioESenhaExistentes_DeveRetornarSucesso();
        }
        
        public async Task DisposeAsync()
        {
            _httpClient.Dispose();
        }

       
    }
}