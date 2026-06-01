using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using CarFleet.Common;

namespace CarFleet.Tests
{
    public class CrudTests
    {
        [Fact]
        public async Task CreateAsync_ShouldAddElementSuccessfully()
        {
            // Arrange
            var service = new CrudServiceAsync<Car>("test.json");
            var car = Car.CreateNew();

            // Act
            bool result = await service.CreateAsync(car);
            var allCars = await service.ReadAllAsync();

            // Assert
            Assert.True(result);
            Assert.Single(allCars);
        }

        [Fact]
        public async Task ReadAllAsync_Pagination_ShouldReturnCorrectAmount()
        {
            // Arrange
            var service = new CrudServiceAsync<Car>("test.json");
            for (int i = 0; i < 10; i++)
            {
                await service.CreateAsync(Car.CreateNew());
            }

            // Act
            var pageData = await service.ReadAllAsync(page: 2, amount: 3);

            // Assert
            Assert.Equal(3, pageData.Count());
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyElement()
        {
            // Arrange
            var service = new CrudServiceAsync<Car>("test.json");
            var car = Car.CreateNew();
            await service.CreateAsync(car);

            // Act
            car.Year = 2050;
            bool result = await service.UpdateAsync(car);
            var updatedCar = await service.ReadAsync(car.Id);

            // Assert
            Assert.True(result);
            Assert.Equal(2050, updatedCar.Year);
        }
    }
}