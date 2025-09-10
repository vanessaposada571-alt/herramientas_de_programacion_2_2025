using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class Person
    {
        private String name;
        private long id;
        private MailAddress email;
        private long cellphone;

        public string Name { get => name; set => name = value; }
        public long Id { get => id; set => id = value; }
        public MailAddress Email { get => email; set => email = value; }
        public long Cellphone { get => cellphone; set => cellphone = value; }
    }
}
