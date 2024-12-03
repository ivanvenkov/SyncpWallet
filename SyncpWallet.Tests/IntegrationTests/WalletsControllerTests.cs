using NSubstitute;
using NUnit.Framework;
using SyncpWallet.Controllers;
using SyncpWallet.Repositories.Interfaces;
using SyncpWallet.Services.Interfaces;
using SyncpWallet.Services.Models;
using SyncpWallet.Validators;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace SyncpWallet.Tests.IntegrationTests
{
    [TestFixture]
    public class WalletsControllerTests
    {
        [Test]
        public async Task Post_WithValidRequest_ShouldReturn200()
        {
            //Arrange
            var mockedWalletService = Substitute.For<IWalletService>();
            var mockedWalletRepository = Substitute.For<IWalletRepository>();
            var mockedValidator = new WalletCreateModelValidator(mockedWalletRepository);
            var ct = new CancellationToken();

            var controller = new WalletsController(mockedWalletService, mockedValidator);
            var request = new WalletCreateModel
            {
                Name = "My Wallet",
                Currency = "BGN",
                Amount = 100,
                 UserId = 1
            };

            var expectedResponse = new WalletDetailsModel(1, "My Wallet", "BGN", 100, 1);

            mockedWalletService.CreateWalletAsync(request, ct)
                .Returns(Task.FromResult(expectedResponse));

            mockedWalletRepository.DoesWalletExistAsync(request.Name, Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(false));

            // Act
            var response = await controller.Post(request, ct);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<OkNegotiatedContentResult<WalletDetailsModel>>());
            var contentResult = response as OkNegotiatedContentResult<WalletDetailsModel>;
            Assert.That(response, Is.Not.Null);
            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.Content.Name, Is.EqualTo(request.Name));
            Assert.That(contentResult.Content.Currency, Is.EqualTo(request.Currency));
            Assert.That(contentResult.Content.Amount, Is.EqualTo(request.Amount));
        }
    }
}
