using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Core.Entities.RabbitMq
{
    public class WelcomeCustomerMessage
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        
        public WelcomeCustomerMessage(string name, string login, string email)
        {
            Name = name;
            Login = login;
            Email = email;
        }
    }
}
