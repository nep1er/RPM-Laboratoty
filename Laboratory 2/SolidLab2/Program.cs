using SOLID_Fundamentals;
using System;

namespace SolidLab2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var savings = new SavingsAccount();
            savings.Deposit(1000m);
            Console.WriteLine($"Создан сберегательный счет. Баланс =  {savings.Balance} руб.");

            var checking = new CheckingAccount();
            checking.Deposit(2000m);
            Console.WriteLine($"Создан текущий счет. Баланс = {checking.Balance} руб.");

            var fixedDeposit = new FixedDepositAccount(DateTime.Now.AddMonths(6));
            fixedDeposit.Deposit(5000m);
            Console.WriteLine($"Создан срочный депозит. Баланс =  {fixedDeposit.Balance} руб. (доступен через 6 месяцев)");

            
            Console.WriteLine("\n--- Операции со счетами ---");
            var bank = new Bank();

            Console.WriteLine("Снимаем 500 руб со сберегательного счета...");
            bank.ProcessWithdrawal(savings, 500m);

            Console.WriteLine("Переводим 200 руб сберегательный на текущий...");
            bank.Transfer(savings, checking, 200m);

            Console.WriteLine($"Баланс сберегательного счета: {savings.Balance} руб.");
            Console.WriteLine($"Баланс текущего счета: {checking.Balance} руб.");

            
            Console.WriteLine("\n--- ЗАКАЗ ---\n");

            var order = new Order
            {
                Id = 1,
                TotalAmount = 1500m,
                CustomerEmail = "customer@mail.com",
                CustomerPhone = "123456789",
                PaymentMethod = "CreditCard",
                Items = new List<string> { "Item1", "Item2" }
            };

            Console.WriteLine($"Создан заказ #{order.Id} на сумму {order.TotalAmount} руб.");

            
            Console.WriteLine("\n--- Обработка заказа ---");

            var repository = new OrderRepository();
            var validator = new OrderValidator();
            var paymentProcessor = new PaymentProcessor();
            var inventoryService = new InventoryService();
            var notificationSender = new EmailNotificationSender();
            var logger = new DatabaseLogger();
            var receiptGenerator = new ReceiptGenerator();

            var orderProcessor = new OrderProcessor(
                repository, validator, paymentProcessor,
                inventoryService, notificationSender,
                logger, receiptGenerator);

            orderProcessor.AddOrder(order);
            orderProcessor.ProcessOrder(1);

            
            Console.WriteLine("\n--- Отчеты ---");
            var reportGenerator = new ReportGenerator();
            reportGenerator.GenerateMonthlyReport(repository.GetAll());

            
            Console.WriteLine("\n--- Уведомления ---");

            var emailService = new SmtpEmailService();
            var smsService = new TwilioSmsService();
            var notificationService = new CompositeNotificationService(emailService, smsService);

            var orderService = new OrderService(notificationService);
            orderService.PlaceOrder(order);

            var promotionService = new PromotionService(notificationService);
            promotionService.SendPromotion(order.CustomerEmail, "20% discount this weekend!");


            Console.WriteLine("\n--- Счет ---");
            Console.WriteLine($"Текущий счет: {checking.Balance} руб.");
            Console.WriteLine($"Сберегательный счет: {savings.Balance} руб.");
            Console.WriteLine($"Срочный депозит: {fixedDeposit.Balance} руб.");

            Console.ReadKey();
        }
    }
}