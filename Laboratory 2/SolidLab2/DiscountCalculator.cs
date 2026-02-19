namespace SOLID_Fundamentals
{
    // Интерфейс стратегии скидки
    public interface IDiscountStrategy
    {
        decimal CalculateDiscount(decimal orderAmount);
        string CustomerType { get; }
    }

    // конкретные стратегии
    public class RegularDiscount : IDiscountStrategy
    {
        public string CustomerType => "Regular";
        public decimal CalculateDiscount(decimal orderAmount) => orderAmount * 0.05m;
    }

    public class PremiumDiscount : IDiscountStrategy
    {
        public string CustomerType => "Premium";
        public decimal CalculateDiscount(decimal orderAmount) => orderAmount * 0.10m;
    }

    public class VIPDiscount : IDiscountStrategy
    {
        public string CustomerType => "VIP";
        public decimal CalculateDiscount(decimal orderAmount) => orderAmount * 0.15m;
    }

    public class StudentDiscount : IDiscountStrategy
    {
        public string CustomerType => "Student";
        public decimal CalculateDiscount(decimal orderAmount) => orderAmount * 0.08m;
    }

    public class SeniorDiscount : IDiscountStrategy
    {
        public string CustomerType => "Senior";
        public decimal CalculateDiscount(decimal orderAmount) => orderAmount * 0.07m;
    }

    public interface IShippingStrategy
    {
        decimal CalculateShippingCost(decimal weight, string destination);
        string ShippingMethod { get; }
    }

    public class StandardShipping : IShippingStrategy
    {
        public string ShippingMethod => "Standard";
        public decimal CalculateShippingCost(decimal weight, string destination)
            => 5.00m + (weight * 0.5m);
    }

    public class ExpressShipping : IShippingStrategy
    {
        public string ShippingMethod => "Express";
        public decimal CalculateShippingCost(decimal weight, string destination)
            => 15.00m + (weight * 1.0m);
    }

    public class OvernightShipping : IShippingStrategy
    {
        public string ShippingMethod => "Overnight";
        public decimal CalculateShippingCost(decimal weight, string destination)
            => 25.00m + (weight * 2.0m);
    }

    public class InternationalShipping : IShippingStrategy
    {
        public string ShippingMethod => "International";
        private readonly Dictionary<string, decimal> _destinationRates = new()
        {
            ["USA"] = 30.00m,
            ["Europe"] = 35.00m,
            ["Asia"] = 40.00m
        };

        public decimal CalculateShippingCost(decimal weight, string destination)
        {
            return _destinationRates.TryGetValue(destination, out decimal rate)
                ? rate
                : 50.00m;
        }
    }

    public class DiscountCalculator
    {
        private readonly Dictionary<string, IDiscountStrategy> _discountStrategies;

        public DiscountCalculator()
        {
            _discountStrategies = new Dictionary<string, IDiscountStrategy>();
        }

        public void RegisterStrategy(IDiscountStrategy strategy)
        {
            _discountStrategies[strategy.CustomerType] = strategy;
        }

        public decimal CalculateDiscount(string customerType, decimal orderAmount)
        {
            if (_discountStrategies.TryGetValue(customerType, out var strategy))
            {
                return strategy.CalculateDiscount(orderAmount);
            }
            return 0;
        }
    }

    public class ShippingCalculator
    {
        private readonly Dictionary<string, IShippingStrategy> _shippingStrategies;

        public ShippingCalculator()
        {
            _shippingStrategies = new Dictionary<string, IShippingStrategy>();
        }

        public void RegisterStrategy(IShippingStrategy strategy)
        {
            _shippingStrategies[strategy.ShippingMethod] = strategy;
        }

        public decimal CalculateShippingCost(string shippingMethod, decimal weight, string destination)
        {
            if (_shippingStrategies.TryGetValue(shippingMethod, out var strategy))
            {
                return strategy.CalculateShippingCost(weight, destination);
            }
            return 0;
        }
    }
}