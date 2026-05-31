using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarFleet.Common;
using CarFleet.Infrastructure.Models;

namespace CarFleet.Infrastructure
{
    // Цей сервіс використовує патерн Репозиторій для доступу до БД
    public class DbCrudServiceAsync<T> : ICrudServiceAsync<T> where T : VehicleModel
    {
        private readonly IRepository<T> _repository;

        public DbCrudServiceAsync(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<bool> CreateAsync(T element)
        {
            await _repository.AddAsync(element);
            return true;
        }

        public async Task<T> ReadAsync(Guid id)
        {
            var all = await _repository.GetAllAsync();
            return all.FirstOrDefault(x => x.Id == id);
        }

        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            var all = await _repository.GetAllAsync();
            return all.Skip((page - 1) * amount).Take(amount);
        }

        public async Task<bool> UpdateAsync(T element)
        {
            await _repository.Update(element);
            return true;
        }

        public async Task<bool> RemoveAsync(T element)
        {
            await _repository.Delete(element);
            return true;
        }

        public Task<bool> SaveAsync()
        {
            // Репозиторій вже зберігає дані автоматично (через SaveChangesAsync)
            return Task.FromResult(true);
        }
    }
}