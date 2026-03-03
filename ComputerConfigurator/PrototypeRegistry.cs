using ComputerConfigurator.Factories;
using System;
using System.Collections.Generic;

namespace ComputerConfigurator
{
    public sealed class PrototypeRegistry
    {
        private static readonly Lazy<PrototypeRegistry> _instance =
            new Lazy<PrototypeRegistry>(() => new PrototypeRegistry());

        private readonly Dictionary<string, Computer> _prototypes;

        private PrototypeRegistry()
        {
            _prototypes = new Dictionary<string, Computer>();

            _prototypes["office"] = new OfficeComputerFactory().CreateComputer();
            _prototypes["gaming"] = new GamingComputerFactory().CreateComputer();
            _prototypes["home"] = new HomeComputerFactory().CreateComputer();
        }

        public static PrototypeRegistry Instance => _instance.Value;

        public Computer GetPrototype(string key)
        {
            if (_prototypes.ContainsKey(key))
            {
                return _prototypes[key].DeepCopy();
            }
            throw new KeyNotFoundException($"Прототип с ключом '{key}' не найден");
        }

        public Computer GetOriginalPrototype(string key)
        {
            if (_prototypes.ContainsKey(key))
            {
                return _prototypes[key];
            }
            throw new KeyNotFoundException($"Прототип с ключом '{key}' не найден");
        }

        public void AddPrototype(string key, Computer prototype)
        {
            _prototypes[key] = prototype;
        }

        public void DisplayAllPrototypes()
        {
            Console.WriteLine("\n=== ВСЕ ПРОТОТИПЫ ===");
            foreach (var key in _prototypes.Keys)
            {
                Console.WriteLine($"Ключ: '{key}'");
                _prototypes[key].Display();
            }
        }
    }
}