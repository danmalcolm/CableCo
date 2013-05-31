using System;
using CableCo.Common.Alerts;

namespace CableCo.Accounts.Commands
{
    public class CommandAlert
    {
        public static CommandAlert Success(string message)
        {
            return new CommandAlert
            {
                Date = DateTime.Now,
                Message = message,
                Type = AlertType.Success
            };
        }

        public static CommandAlert Error(string message)
        {
            return new CommandAlert
            {
                Date = DateTime.Now,
                Message = message,
                Type = AlertType.Error
            };
        }

        public static CommandAlert Invalid(string message)
        {
            return new CommandAlert
            {
                Date = DateTime.Now,
                Message = message,
                Type = AlertType.Invalid
            };
        }
        
        public DateTime Date { get; set; }

        public string Message { get; set; }

        public AlertType Type { get; set; }

        public override string ToString()
        {
            return string.Format("Date: {0}, Type: {1}, Message: {2}", Date, Type, Message);
        }
    }
}