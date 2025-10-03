using Grocery.Core.Data.Repositories;
using Grocery.Core.Models;
using Grocery.Core.Services;

namespace TestCore
{
    [TestFixture]
    public class UC13_BoughtProductsServiceTests
    {
        private BoughtProductsService _service;
        private GroceryListItemsRepository _groceryListItemsRepository;
        private GroceryListRepository _groceryListRepository;
        private ClientRepository _clientRepository;
        private ProductRepository _productRepository;

        [SetUp]
        public void Setup()
        {
            _groceryListItemsRepository = new GroceryListItemsRepository();
            _groceryListRepository = new GroceryListRepository();
            _clientRepository = new ClientRepository();
            _productRepository = new ProductRepository();

            _service = new BoughtProductsService(
                _groceryListItemsRepository,
                _groceryListRepository,
                _clientRepository,
                _productRepository
            );
        }

        [Test]
        public void UC13_FR2_GetBoughtProducts_ValidProductId_ReturnsCorrectClients()
        {
            // Arrange
            int productId = 1;

            // Act
            var result = _service.Get(productId);

            // Assert
            Assert.That(result, Is.Not.Null, "Result mag niet null zijn");
            Assert.That(result.Count, Is.GreaterThan(0), "Er moeten klanten zijn die dit product hebben gekocht");
            Assert.That(result.All(bp => bp.Product.Id == productId), Is.True, 
                "Alle resultaten moeten het juiste product bevatten");
        }

        [Test]
        public void UC13_FR2_GetBoughtProducts_ValidProductId_ReturnsCorrectGroceryLists()
        {
            // Arrange
            int productId = 1;

            // Act
            var result = _service.Get(productId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.GreaterThan(0));
            
            foreach (var boughtProduct in result)
            {
                Assert.That(boughtProduct.GroceryList, Is.Not.Null, 
                    "Elke BoughtProducts moet een geldige GroceryList bevatten");
                Assert.That(boughtProduct.GroceryList.Id, Is.GreaterThan(0), 
                    "GroceryList moet een geldig Id hebben");
            }
        }

        [Test]
        public void UC13_FR2_GetBoughtProducts_ProductNotBought_ReturnsEmptyList()
        {
            // Arrange
            int productId = 4;

            // Act
            var result = _service.Get(productId);

            // Assert
            Assert.That(result, Is.Not.Null, "Result mag niet null zijn");
            Assert.That(result.Count, Is.EqualTo(0), 
                "Als niemand het product heeft gekocht, moet de lijst leeg zijn");
        }

        [Test]
        public void UC13_FR2_GetBoughtProducts_NullProductId_ReturnsEmptyList()
        {
            // Arrange
            int? productId = null;

            // Act
            var result = _service.Get(productId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0), 
                "Bij null productId moet een lege lijst worden geretourneerd");
        }

        [Test]
        public void UC13_FR2_GetBoughtProducts_ValidProductId_ContainsCorrectAmount()
        {
            // Arrange
            int productId = 1; // Melk

            // Act
            var result = _service.Get(productId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.GreaterThan(0));
            
            foreach (var boughtProduct in result)
            {
                Assert.That(boughtProduct.Amount, Is.GreaterThan(0), 
                    "Amount moet groter dan 0 zijn voor gekochte producten");
            }
        }

        [Test]
        public void UC13_FR2_GetBoughtProducts_MultipleClients_ReturnsAllClients()
        {
            // Arrange
            int productId = 1;

            // Act
            var result = _service.Get(productId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2), 
                "Product 1 (Melk) wordt gekocht in 2 boodschappenlijsten");
            
            var groceryListIds = result.Select(bp => bp.GroceryList.Id).ToList();
            Assert.That(groceryListIds, Does.Contain(1), "GroceryList 1 moet in de resultaten zitten");
            Assert.That(groceryListIds, Does.Contain(2), "GroceryList 2 moet in de resultaten zitten");
        }

        [Test]
        public void UC13_FR5_ClientRole_AdminUser_HasAdminRole()
        {
            // Arrange
            string adminEmail = "user3@mail.com";

            // Act
            var adminClient = _clientRepository.Get(adminEmail);

            // Assert
            Assert.That(adminClient, Is.Not.Null, "Admin client moet bestaan");
            Assert.That(adminClient.Role, Is.EqualTo(Role.Admin), 
                "User3 moet de Admin rol hebben");
        }

        [Test]
        public void UC13_FR5_ClientRole_RegularUser_HasNoneRole()
        {
            // Arrange
            string regularUserEmail = "user1@mail.com";

            // Act
            var regularClient = _clientRepository.Get(regularUserEmail);

            // Assert
            Assert.That(regularClient, Is.Not.Null, "Regular client moet bestaan");
            Assert.That(regularClient.Role, Is.EqualTo(Role.None), 
                "User1 moet de None rol hebben (geen admin)");
        }

        [Test]
        public void UC13_FR5_ClientRole_AllClients_OnlyOneAdmin()
        {
            // Arrange & Act
            var allClients = _clientRepository.GetAll();
            var adminClients = allClients.Where(c => c.Role == Role.Admin).ToList();

            // Assert
            Assert.That(allClients.Count, Is.GreaterThan(0), "Er moeten clients zijn");
            Assert.That(adminClients.Count, Is.EqualTo(1), 
                "Er moet precies 1 admin zijn in het systeem");
            Assert.That(adminClients[0].EmailAddress, Is.EqualTo("user3@mail.com"),
                "De admin moet user3 zijn");
        }

        [Test]
        public void UC13_FR5_ClientRole_DefaultRole_IsNone()
        {
            // Arrange & Act
            var client = new Client(999, "Test User", "test@example.com", "hashedpassword");

            // Assert
            Assert.That(client.Role, Is.EqualTo(Role.None), 
                "Default role moet None zijn voor nieuwe clients");
        }

        [Test]
        public void UC13_NFR3_BoughtProducts_ContainsOnlyRelevantClientData()
        {
            // Arrange
            int productId = 1;

            // Act
            var result = _service.Get(productId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.GreaterThan(0));

            foreach (var boughtProduct in result)
            {
                Assert.That(boughtProduct.Client, Is.Not.Null, 
                    "Client object moet aanwezig zijn");
                Assert.That(boughtProduct.Client.Name, Is.Not.Null.And.Not.Empty, 
                    "Client naam moet aanwezig zijn");
                Assert.That(boughtProduct.Client.Id, Is.GreaterThan(0), 
                    "Client Id moet geldig zijn");
                
                Assert.That(boughtProduct.Client.Password, Is.Not.Null, 
                    "Password is opgeslagen maar mag nooit in UI worden getoond");
            }
        }

        [Test]
        public void UC13_NFR3_BoughtProducts_ContainsOnlyRelevantGroceryListData()
        {
            // Arrange
            int productId = 1;

            // Act
            var result = _service.Get(productId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.GreaterThan(0));

            foreach (var boughtProduct in result)
            {
                Assert.That(boughtProduct.GroceryList, Is.Not.Null, 
                    "GroceryList moet aanwezig zijn");
                Assert.That(boughtProduct.GroceryList.Name, Is.Not.Null.And.Not.Empty, 
                    "GroceryList naam moet aanwezig zijn");
                Assert.That(boughtProduct.GroceryList.Id, Is.GreaterThan(0), 
                    "GroceryList Id moet geldig zijn");
            }
        }

        [Test]
        public void UC13_NFR3_BoughtProducts_DoesNotExposeUnrelatedClientData()
        {
            // Arrange
            int productId = 1;

            // Act
            var result = _service.Get(productId);

            // Assert
            Assert.That(result, Is.Not.Null);
            
            var allClients = _clientRepository.GetAll();
            var clientsInResult = result.Select(bp => bp.Client.Id).Distinct().ToList();
            
            Assert.That(clientsInResult.Count, Is.LessThanOrEqualTo(allClients.Count),
                "Er mogen niet meer clients in het resultaat zitten dan er totaal zijn");
            
            var allGroceryListItems = _groceryListItemsRepository.GetAll()
                .Where(item => item.ProductId == productId);
            var groceryListIdsWithProduct = allGroceryListItems.Select(item => item.GroceryListId).ToList();
            
            foreach (var boughtProduct in result)
            {
                Assert.That(groceryListIdsWithProduct, Does.Contain(boughtProduct.GroceryList.Id),
                    "Alleen boodschappenlijsten die het product bevatten mogen worden getoond");
            }
        }

        [Test]
        public void UC13_NFR3_BoughtProducts_ContainsCorrectProductData()
        {
            // Arrange
            int productId = 1;

            // Act
            var result = _service.Get(productId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.GreaterThan(0));

            foreach (var boughtProduct in result)
            {
                Assert.That(boughtProduct.Product, Is.Not.Null, "Product moet aanwezig zijn");
                Assert.That(boughtProduct.Product.Id, Is.EqualTo(productId), 
                    "Product Id moet overeenkomen met de gevraagde productId");
                Assert.That(boughtProduct.Product.Name, Is.Not.Null.And.Not.Empty, 
                    "Product naam moet aanwezig zijn");
            }
        }

        [Test]
        public void UC13_NFR3_BoughtProducts_AmountIsRelevantData()
        {
            // Arrange
            int productId = 1;

            // Act
            var result = _service.Get(productId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.GreaterThan(0));

            foreach (var boughtProduct in result)
            {
                Assert.That(boughtProduct.Amount, Is.GreaterThan(0),
                    "Amount is relevante informatie en moet groter dan 0 zijn");
                
                var groceryListItem = _groceryListItemsRepository.GetAll()
                    .FirstOrDefault(item => item.GroceryListId == boughtProduct.GroceryList.Id 
                                         && item.ProductId == productId);
                
                Assert.That(groceryListItem, Is.Not.Null, 
                    "GroceryListItem moet bestaan voor deze combinatie");
                Assert.That(boughtProduct.Amount, Is.EqualTo(groceryListItem.Amount),
                    "Amount in BoughtProducts moet overeenkomen met GroceryListItem");
            }
        }
    }
}
