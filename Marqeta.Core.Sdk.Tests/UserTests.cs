﻿using System.Linq;
using System.Threading.Tasks;
using Marqeta.Core.Abstractions;
using Marqeta.Core.Sdk.Tests.Factories;
using Marqeta.Core.Sdk.Tests.Helpers;
using Xunit;

// ReSharper disable IdentifierTypo

namespace Marqeta.Core.Sdk.Tests
{
    public class UserTests : BaseTests
    {
        [Fact]
        public async void UsersGetAsync()
        {
            var client = ClientFactory.GetMarqetaClient();
            var response = await client.UsersGetAsync();
            Assert.NotNull(response);
            Assert.True(response.Count > 0);
        }

        [Fact]
        public async void UsersPostAsync()
        {
            await UserHelper.CreateUser();
        }

        [Fact]
        public async void UsersPostAsync_CreateChildren()
        {
            var client = ClientFactory.GetMarqetaClient();

            // Create parent
            var cardHolderModel1 = new Card_holder_model();
            var response1 = await client.UsersPostAsync(cardHolderModel1);
            Assert.NotNull(response1);

            // Create child
            var cardHolderModel2 = new Card_holder_model
            {
                Parent_token = response1.Token,
                Uses_parent_account = true,
            };
            var response2 = await client.UsersPostAsync(cardHolderModel2);
            Assert.NotNull(response2);
            Assert.Equal(response1.Token, response2.Parent_token);
        }

        [Fact]
        public async void UsersChildrenAsync()
        {
            var client = ClientFactory.GetMarqetaClient();

            // Create parent
            var cardHolderModel1 = new Card_holder_model();
            var response1 = await client.UsersPostAsync(cardHolderModel1);
            Assert.NotNull(response1);

            // Create child
            var cardHolderModel2 = new Card_holder_model
            {
                Parent_token = response1.Token,
                Uses_parent_account = true,
            };
            var response2 = await client.UsersPostAsync(cardHolderModel2);
            Assert.NotNull(response2);
            Assert.Equal(response1.Token, response2.Parent_token);

            // Get children
            var response3 = await client.UsersChildrenAsync(response1.Token);
            Assert.NotNull(response3);
            Assert.True(response3.Count > 0);
            var responseChild = response3.Data.First();
            Assert.NotNull(responseChild);
            Assert.Equal(response2.Token, responseChild.Token);
        }
    }
}