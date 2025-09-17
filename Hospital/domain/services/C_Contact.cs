using Hospital.domain.model;
using Hospital.domain.ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Hospital.domain.services
{
    internal class C_Contact
    {
        private Contact_port contact_Port;
        public void Create(Contact contact)
        {
            if (contact_Port.FindByName1(contact) == null)
            {
                throw new Exception("El nombre no puede ser vacío/nulo");
            }
            if(contact_Port.FindById_patient(contact) == null)
            {
                throw new Exception("El paciente no existe");
            }
            if(contact_Port.FindByCellphone(contact) == null)
            {
                throw new Exception("El número de celular no puede estar vacío");
            }

            contact_Port.Save(contact);

        }
    }
}
