using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Infra.RabbitMq.Consumer.Messages
{
    public class WelcomeCustomerMessage
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }

        public WelcomeCustomerMessage() { }
        public WelcomeCustomerMessage(string name, string login, string email)
        {
            Name = name;
            Login = login;
            Email = email;
        }
    }
}
