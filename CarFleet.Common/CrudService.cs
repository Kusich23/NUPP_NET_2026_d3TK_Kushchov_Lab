using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;

namespace CarFleet.Common
{
    public interface ICrudService<T>
    {
        void Create(T element);
        T Read(Guid id);
        IEnumerable<T> ReadAll();
        void Update(T element);
        void Remove(T element);
    }

    public class CrudService<T> : ICrudService<T> where T : IIdentifiable
    {
        private List<T> _items = new List<T>();

        public void Create(T element) => _items.Add(element);

        public T Read(Guid id) => _items.FirstOrDefault(x => x.Id == id);

        public IEnumerable<T> ReadAll() => _items;

        public void Update(T element)
        {
            var index = _items.FindIndex(x => x.Id == element.Id);
            if (index != -1) _items[index] = element;
        }

        public void Remove(T element) => _items.Remove(element);

        // Додаткове завдання (Load / Save)
        public void Save(string filePath)
        {
            var json = JsonSerializer.Serialize(_items);
            File.WriteAllText(filePath, json);
        }

        public void Load(string filePath)
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                _items = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
        }
    }
}