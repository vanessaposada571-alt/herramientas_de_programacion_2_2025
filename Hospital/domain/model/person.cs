using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    public class Person
    {
        private String name;
        private long id;
        private MailAddress email;
        private long cellphone;
        private DateTime birth;
        private bool gender;
        private string direction;

        public string Name { get => name; set => name = value; }
        public long Id { get => id; set => id = value; }
        public MailAddress Email { get => email; set => email = value; }
        public long Cellphone { get => cellphone; set => cellphone = value; }
        public DateTime Birth { get => birth; set => birth = value; }
        public bool Gender { get => gender; set => gender = value; }
        public string Direction { get => direction; set => direction = value; }
    }
}
