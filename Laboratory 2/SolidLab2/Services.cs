namespace SOLID_Fundamentals
{
    public interface IEmailService
    {
        void SendEmail(string to, string subject, string body);
    }

    public interface ISmsService
    {
        void SendSms(string phoneNumber, string message);
    }

    public interface INotificationService
    {
        void SendNotification(string recipient, string message, NotificationType type);
    }

    public enum NotificationType
    {
        Email,
        Sms,
        Both
    }

    public class SmtpEmailService : IEmailService
    {
        public void SendEmail(string to, string subject, string body)
        {
            Console.WriteLine($"Sending email to {to}: {subject} - {body}");
        }
    }

    public class TwilioSmsService : ISmsService
    {
        public void SendSms(string phoneNumber, string message)
        {
            Console.WriteLine($"Sending SMS to {phoneNumber}: {message}");
        }
    }

    public class CompositeNotificationService : INotificationService
    {
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;

        public CompositeNotificationService(IEmailService emailService, ISmsService smsService)
        {
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _smsService = smsService ?? throw new ArgumentNullException(nameof(smsService));
        }

        public void SendNotification(string recipient, string message, NotificationType type)
        {
            switch (type)
            {
                case NotificationType.Email:
                    _emailService.SendEmail(recipient, "Notification", message);
                    break;
                case NotificationType.Sms:
                    _smsService.SendSms(recipient, message);
                    break;
                case NotificationType.Both:
                    _emailService.SendEmail(recipient, "Notification", message);
                    _smsService.SendSms(recipient, message);
                    break;
            }
        }
    }

    public class OrderService
    {
        private readonly INotificationService _notificationService;

        public OrderService(INotificationService notificationService)
        {
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }

        public void PlaceOrder(Order order)
        {
            Console.WriteLine($"Placing order {order.Id}...");

            _notificationService.SendNotification(order.CustomerEmail, "Your order has been placed", NotificationType.Email);

            if (!string.IsNullOrEmpty(order.CustomerPhone))
            {
                _notificationService.SendNotification(order.CustomerPhone, "Your order has been placed", NotificationType.Sms);
            }
        }
    }

    public class PromotionService
    {
        private readonly INotificationService _notificationService;

        public PromotionService(INotificationService notificationService)
        {
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }

        public void SendPromotion(string email, string promotion)
        {
            _notificationService.SendNotification(email, promotion, NotificationType.Email);
        }
    }

    public static class ServiceFactory
    {
        public static OrderService CreateOrderService()
        {
            var emailService = new SmtpEmailService();
            var smsService = new TwilioSmsService();
            var notificationService = new CompositeNotificationService(emailService, smsService);
            return new OrderService(notificationService);
        }

        public static PromotionService CreatePromotionService()
        {
            var emailService = new SmtpEmailService();
            var smsService = new TwilioSmsService();
            var notificationService = new CompositeNotificationService(emailService, smsService);
            return new PromotionService(notificationService);
        }
    }
}