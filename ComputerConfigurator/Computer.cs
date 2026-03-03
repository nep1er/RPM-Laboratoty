using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComputerConfigurator
{
    public class Computer : ICloneable
    {
        public string CPU { get; set; }
        public int RAM { get; set; }
        public string GPU { get; set; }
        public List<string> AdditionalComponents { get; set; }

        public Computer()
        {
            AdditionalComponents = new List<string>();
        }

        public Computer(Computer other)
        {
            CPU = other.CPU;
            RAM = other.RAM;
            GPU = other.GPU;

            AdditionalComponents = other.AdditionalComponents != null
                ? new List<string>(other.AdditionalComponents)
                : new List<string>();
        }

        public void Display()
        {
            Console.WriteLine(new string('-', 50));
            Console.WriteLine($"Процессор: {CPU ?? "Не указан"}");
            Console.WriteLine($"ОЗУ: {RAM} ГБ");
            Console.WriteLine($"Видеокарта: {GPU ?? "Не указана"}");
            Console.WriteLine("Дополнительные компоненты:");
            if (AdditionalComponents != null && AdditionalComponents.Any())
            {
                foreach (var component in AdditionalComponents)
                {
                    Console.WriteLine($"  - {component}");
                }
            }
            else
            {
                Console.WriteLine("  (нет)");
            }
            Console.WriteLine(new string('-', 50));
        }

        public Computer ShallowCopy()
        {
            return (Computer)this.MemberwiseClone();
        }

        public Computer DeepCopy()
        {
            return new Computer(this);
        }

        public object Clone()
        {
            return DeepCopy();
        }
    }
}