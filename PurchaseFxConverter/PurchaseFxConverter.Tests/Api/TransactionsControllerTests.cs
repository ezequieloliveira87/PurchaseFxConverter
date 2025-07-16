using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;
using PurchaseFxConverter.API;
using PurchaseFxConverter.Application.DTOs;

namespace PurchaseFxConverter.Tests.Api;

public class TransactionsControllerTests
    {
        private WebApplicationFactory<Program> _factory = null!;
        private HttpClient _client = null!;

        [SetUp]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            _client?.Dispose();
            _factory?.Dispose();
        }

        [Test]
        public async Task POST_Should_Create_Transaction()
        {
            var request = new CreatePurchaseTransactionRequest
            {
                Description = "Compra via API",
                TransactionDate = DateTime.UtcNow,
                AmountUSD = 120.50m
            };

            var response = await _client.PostAsJsonAsync("/api/transactions", request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var content = await response.Content.ReadFromJsonAsync<Dictionary<string, Guid>>();
            Assert.That(content, Is.Not.Null);
            Assert.That(content!.ContainsKey("id"), Is.True);
        }

        [Test]
        public async Task GET_Should_Return_Transaction_When_Exists()
        {
            // Cria transação primeiro
            var request = new CreatePurchaseTransactionRequest
            {
                Description = "API Read",
                TransactionDate = DateTime.UtcNow,
                AmountUSD = 75.00m
            };

            var post = await _client.PostAsJsonAsync("/api/transactions", request);
            var body = await post.Content.ReadFromJsonAsync<Dictionary<string, Guid>>();
            var id = body!["id"];

            // Faz GET
            var get = await _client.GetAsync($"/api/transactions/{id}");

            Assert.That(get.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task GET_Should_Return_NotFound_When_Transaction_Not_Exists()
        {
            var get = await _client.GetAsync($"/api/transactions/{Guid.NewGuid()}");
            Assert.That(get.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
        
        [Test]
        public async Task Convert_Should_Return_Ok_When_Successful()
        {
            // Arrange
            var create = new CreatePurchaseTransactionRequest
            {
                Description = "Conversão OK",
                TransactionDate = DateTime.UtcNow,
                AmountUSD = 100
            };

            var post = await _client.PostAsJsonAsync("/api/transactions", create);
            var body = await post.Content.ReadFromJsonAsync<Dictionary<string, Guid>>();
            var id = body!["id"];

            // Act
            var response = await _client.GetAsync($"/api/transactions/{id}/convert?currency=EUR");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Convert_Should_Return_BadRequest_When_Currency_Is_Invalid()
        {
            var create = new CreatePurchaseTransactionRequest
            {
                Description = "Moeda inválida",
                TransactionDate = DateTime.UtcNow,
                AmountUSD = 50
            };

            var post = await _client.PostAsJsonAsync("/api/transactions", create);
            var body = await post.Content.ReadFromJsonAsync<Dictionary<string, Guid>>();
            var id = body!["id"];

            var response = await _client.GetAsync($"/api/transactions/{id}/convert?currency=ZZZ");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Convert_Should_Return_NotFound_When_Transaction_Not_Found()
        {
            var response = await _client.GetAsync($"/api/transactions/{Guid.NewGuid()}/convert?currency=EUR");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }