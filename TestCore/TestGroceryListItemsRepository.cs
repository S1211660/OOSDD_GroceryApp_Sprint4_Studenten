using Grocery.Core.Data.Repositories;
using Grocery.Core.Models;

namespace TestCore
{
    [TestFixture]
    public class TestGroceryListItemsRepository
    {
        private GroceryListItemsRepository repository;
        private ProductRepository productRepository;

        [SetUp]
        public void Setup()
        {
            // Arrange
            repository = new GroceryListItemsRepository();
            productRepository = new ProductRepository();
        }

        [Test]
        public void UpdateGroceryListItem_IncreaseAmount_UpdatesCorrectly()
        {
            // Arrange
            var item = repository.Get(1);
            int startAmount = item.Amount;
            item.Amount++;

            // Act
            var updatedItem = repository.Update(item);

            // Assert
            Assert.That(updatedItem, Is.Not.Null, "Update moet een item teruggeven");
            Assert.That(updatedItem.Amount, Is.EqualTo(startAmount + 1),
                "Het aantal moet met 1 verhoogd zijn");

            var itemFromRepo = repository.Get(1);
            Assert.That(itemFromRepo.Amount, Is.EqualTo(startAmount + 1),
                "Het item in de repository moet geupdate zijn");
        }

        [Test]
        public void UpdateGroceryListItem_DecreaseAmount_UpdatesCorrectly()
        {
            // Arrange
            var item = repository.Get(1);
            int startAmount = item.Amount;
            item.Amount--;

            // Act
            var updatedItem = repository.Update(item);

            // Assert
            Assert.That(updatedItem, Is.Not.Null, "Update moet een item teruggeven");
            Assert.That(updatedItem.Amount, Is.EqualTo(startAmount - 1),
                "Het aantal moet met 1 verlaagd zijn");

            var itemFromRepo = repository.Get(1);
            Assert.That(itemFromRepo.Amount, Is.EqualTo(startAmount - 1),
                "Het item in de repository moet geupdate zijn");
        }

        [Test]
        public void UpdateGroceryListItem_AmountZero_ItemRemainsInRepository()
        {
            // Arrange
            var item = repository.Get(1);
            item.Amount = 0;
            int countBefore = repository.GetAll().Count;

            // Act
            var updatedItem = repository.Update(item);

            // Assert
            Assert.That(updatedItem, Is.Not.Null, "Item moet nog steeds bestaan");
            Assert.That(updatedItem.Amount, Is.EqualTo(0), "Amount moet 0 zijn (FR2)");

            int countAfter = repository.GetAll().Count;
            Assert.That(countAfter, Is.EqualTo(countBefore),
                "Aantal items mag niet veranderen (NFR3)");

            var itemFromRepo = repository.Get(1);
            Assert.That(itemFromRepo, Is.Not.Null,
                "Item met amount 0 mag niet verwijderd zijn (NFR3)");
        }

        [Test]
        public void UpdateProduct_DecreaseStock_UpdatesCorrectly()
        {
            // Arrange
            var product = productRepository.Get(1);
            int startStock = product.Stock;
            product.Stock--;

            // Act
            var updatedProduct = productRepository.Update(product);

            // Assert
            Assert.That(updatedProduct, Is.Not.Null, "Update moet een product teruggeven");
            Assert.That(updatedProduct.Stock, Is.EqualTo(startStock - 1),
                "De stock moet met 1 verminderd zijn (FR3)");

            var productFromRepo = productRepository.Get(1);
            Assert.That(productFromRepo.Stock, Is.EqualTo(startStock - 1),
                "Het product in de repository moet geupdate zijn");
        }
    }
}