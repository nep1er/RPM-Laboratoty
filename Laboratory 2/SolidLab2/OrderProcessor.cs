namespace SOLID_Fundamentals
{
    // Отвечает за хранение и поиск заказов
    public class OrderRepository
    {
        private List<Order> _orders = new List<Order>();

        public void Add(Order order)
        {
            _orders.Add(order);
            Console.WriteLine($"Order {order.Id} added");
        }

        public Order GetById(int orderId)
        {
            return _orders.FirstOrDefault(o => o.Id == orderId);
        }

        public List<Order> GetAll() => _orders.ToList();
    }

    // Отвечаетза валидацию заказов
    public class OrderValidator
    {
        public bool IsValid(Order order)
        {
            return order.TotalAmount > 0;
        }
    }

    // Отвечает за обработку платежей
    public class PaymentProcessor
    {
        public void ProcessPayment(string paymentMethod, decimal amount)
        {
            Console.WriteLine($"Processing payment: {amount} via {paymentMethod}");
        }
    }

    public class InventoryService
    {
        public void UpdateInventory(List<string> items)
        {
            Console.WriteLine($"Updating inventory for {items.Count} items");
        }
    }

    // Отвечает за отправку уведомлений
    public interface INotificationSender
    {
        void Send(string to, string message);
    }

    public class EmailNotificationSender : INotificationSender
    {
        public void Send(string to, string message)
        {
            Console.WriteLine($"Sending email to {to}: {message}");
        }
    }

    public interface ILogger
    {
        void Log(string message);
    }

    public class DatabaseLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"Logging to database: {message}");
        }
    }

    // Отвечает за генерацию квитанций
    public class ReceiptGenerator
    {
        public void GenerateReceipt(Order order)
        {
            Console.WriteLine($"Generating receipt for order {order.Id}");
        }
    }

    public class ReportGenerator
    {
        public void GenerateMonthlyReport(List<Order> orders)
        {
            decimal totalRevenue = orders.Sum(o => o.TotalAmount);
            int totalOrders = orders.Count;
            Console.WriteLine($"Monthly Report: {totalOrders} orders, Revenue: {totalRevenue:C}");
        }
    }

    public class ExcelExporter
    {
        public void ExportToExcel(string filePath, List<Order> orders)
        {
            Console.WriteLine($"Exporting {orders.Count} orders to {filePath}");
        }
    }

    public class OrderProcessor
    {
        private readonly OrderRepository _repository;
        private readonly OrderValidator _validator;
        private readonly PaymentProcessor _paymentProcessor;
        private readonly InventoryService _inventoryService;
        private readonly INotificationSender _notificationSender;
        private readonly ILogger _logger;
        private readonly ReceiptGenerator _receiptGenerator;

        public OrderProcessor(
            OrderRepository repository,
            OrderValidator validator,
            PaymentProcessor paymentProcessor,
            InventoryService inventoryService,
            INotificationSender notificationSender,
            ILogger logger,
            ReceiptGenerator receiptGenerator)
        {
            _repository = repository;
            _validator = validator;
            _paymentProcessor = paymentProcessor;
            _inventoryService = inventoryService;
            _notificationSender = notificationSender;
            _logger = logger;
            _receiptGenerator = receiptGenerator;
        }

        public void AddOrder(Order order)
        {
            _repository.Add(order);
        }

        public void ProcessOrder(int orderId)
        {
            var order = _repository.GetById(orderId);
            if (order == null)
            {
                Console.WriteLine($"Order {orderId} not found");
                return;
            }

            Console.WriteLine($"Processing order {orderId}");

            if (!_validator.IsValid(order))
                throw new Exception("Invalid order amount");

            _paymentProcessor.ProcessPayment(order.PaymentMethod, order.TotalAmount);
            _inventoryService.UpdateInventory(order.Items);
            _notificationSender.Send(order.CustomerEmail, $"Order {orderId} processed");
            _logger.Log($"Order {orderId} processed at {DateTime.Now}");
            _receiptGenerator.GenerateReceipt(order);
        }
    }
}