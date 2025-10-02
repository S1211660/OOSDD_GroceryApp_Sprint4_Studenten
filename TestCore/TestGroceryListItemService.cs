using Grocery.Core.Data.Repositories;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Services;

namespace TestCore
{
    public class TestGroceryListItemsService
    {
        private IGroceryListItemsRepository _groceryRepo;
        private IProductRepository _productRepo;
        private GroceryListItemsService _service;

        [SetUp]
        public void Setup()
        {
            _groceryRepo = new GroceryListItemsRepository();
            _productRepo = new ProductRepository();
            _service = new GroceryListItemsService(_groceryRepo, _productRepo);
        }

        [Test]
        public void GetBestSellingProducts_ReturnsMaximum5_ByDefault()
        {
            // Act
            var result = _service.GetBestSellingProducts();

            // Assert
            Assert.IsNotNull(result);
            Assert.LessOrEqual(result.Count, 5);
        }

        [Test]
        public void GetBestSellingProducts_ReturnsNotNull_Always()
        {
            // Act
            var result = _service.GetBestSellingProducts();

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetBestSellingProducts_IsSortedDescending()
        {
            // Act
            var result = _service.GetBestSellingProducts();

            // Assert
            if (result.Count > 1)
            {
                for (int i = 0; i < result.Count - 1; i++)
                {
                    Assert.GreaterOrEqual(result[i].NrOfSells, result[i + 1].NrOfSells,
                        $"Product op positie {i} heeft minder verkocht dan product op positie {i + 1}");
                }
            }
        }

        [Test]
        public void GetBestSellingProducts_HasCorrectRanking()
        {
            // Act
            var result = _service.GetBestSellingProducts();

            // Assert
            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(i + 1, result[i].Ranking,
                    $"Product op positie {i} heeft verkeerde ranking");
            }
        }

        [Test]
        public void GetBestSellingProducts_ReturnsTop3_WhenParameterIs3()
        {
            // Act
            var result = _service.GetBestSellingProducts(3);

            // Assert
            Assert.LessOrEqual(result.Count, 3);
        }

        [Test]
        public void GetBestSellingProducts_AllItemsHaveValidData()
        {
            // Act
            var result = _service.GetBestSellingProducts();

            // Assert
            foreach (var product in result)
            {
                Assert.IsNotNull(product.Name, "Product naam mag niet null zijn");
                Assert.IsNotEmpty(product.Name, "Product naam mag niet leeg zijn");
                Assert.Greater(product.NrOfSells, 0, "Aantal verkocht moet groter dan 0 zijn");
                Assert.GreaterOrEqual(product.Stock, 0, "Voorraad mag niet negatief zijn");
                Assert.Greater(product.Id, 0, "Product ID moet groter dan 0 zijn");
            }
        }

        [Test]
        public void GetBestSellingProducts_ReturnsExactly1_WhenTopXIs1()
        {
            // Act
            var result = _service.GetBestSellingProducts(1);

            // Assert
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public void GetBestSellingProducts_IsConsistent_OnMultipleCalls()
        {
            // Act
            var result1 = _service.GetBestSellingProducts();
            var result2 = _service.GetBestSellingProducts();

            // Assert
            Assert.AreEqual(result1.Count, result2.Count);
            for (int i = 0; i < result1.Count; i++)
            {
                Assert.AreEqual(result1[i].Id, result2[i].Id,
                    $"Product op positie {i} is verschillend bij tweede aanroep");
                Assert.AreEqual(result1[i].NrOfSells, result2[i].NrOfSells,
                    $"Aantal verkocht op positie {i} is verschillend bij tweede aanroep");
            }
        }
    }
}