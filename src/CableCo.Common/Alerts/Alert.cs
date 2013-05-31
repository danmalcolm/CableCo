using System;

namespace CableCo.Common.Alerts
{
    public class Alert
    {
        public static Alert Success(string message)
        {
            return Create(message, AlertType.Success);
        }

        public static Alert Warning(string message)
        {
            return Create(message, AlertType.Warning);
        }

        public static Alert Error(string message)
        {
            return Create(message, AlertType.Error);
        }

        public static Alert Create(string message, AlertType type)
        {
            return new Alert
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
                Message = message,
                Type = type
            };
        }

        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public string Message { get; set; }

        public AlertType Type { get; set; }
    }
}