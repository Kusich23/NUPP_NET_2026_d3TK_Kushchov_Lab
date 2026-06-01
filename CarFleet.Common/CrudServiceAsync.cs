using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CarFleet.Common
{
    // Оновлений інтерфейс строго за методичкою (без IEnumerable<T>)
    public interface ICrudServiceAsync<T>
    {
        public Task<bool> CreateAsync(T element);
        public Task<T> ReadAsync(Guid id);
        public Task<IEnumerable<T>> ReadAllAsync();
        public Task<IEnumerable<T>> ReadAllAsync(int page, int amount);
        public Task<bool> UpdateAsync(T element);
        public Task<bool> RemoveAsync(T element);
        public Task<bool> SaveAsync();
    }

    // Стара реалізація (залишаємо її робочою, просто прибираємо методи IEnumerator)
    public class CrudServiceAsync<T> : ICrudServiceAsync<T> where T : IIdentifiable
    {
        private readonly ConcurrentDictionary<Guid, T> _items = new ConcurrentDictionary<Guid, T>();
        private readonly string _filePath;

        public CrudServiceAsync(string filePath)
        {
            _filePath = filePath;
        }

        public Task<bool> CreateAsync(T element)
        {
            bool added = _items.TryAdd(element.Id, element);
            return Task.FromResult(added);
        }

        public Task<T> ReadAsync(Guid id)
        {
            _items.TryGetValue(id, out T element);
            return Task.FromResult(element);
        }

        public Task<IEnumerable<T>> ReadAllAsync()
        {
            return Task.FromResult<IEnumerable<T>>(_items.Values.ToList());
        }

        public Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            var pagedData = _items.Values
                .Skip((page - 1) * amount)
                .Take(amount)
                .ToList();
            return Task.FromResult<IEnumerable<T>>(pagedData);
        }

        public Task<bool> UpdateAsync(T element)
        {
            if (_items.ContainsKey(element.Id))
            {
                _items[element.Id] = element;
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> RemoveAsync(T element)
        {
            bool removed = _items.TryRemove(element.Id, out _);
            return Task.FromResult(removed);
        }

        public async Task<bool> SaveAsync()
        {
            try
            {
                using FileStream createStream = File.Create(_filePath);
                await JsonSerializer.SerializeAsync(createStream, _items.Values);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}