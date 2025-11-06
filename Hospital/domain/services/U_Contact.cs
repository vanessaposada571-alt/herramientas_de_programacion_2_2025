using Hospital.domain.model;
using Hospital.domain.ports;
using System;

namespace Hospital.domain.services
{
 public class U_Contact
 {
 private Contact_port contact_Port;

 public void Update(Contact contact)
 {
 // Verify associated patient exists
 if (contact_Port.FindById_patient(contact) == null)
 {
 throw new Exception("El paciente no existe");
 }

 contact_Port.Update(contact);
 }
 }
}
