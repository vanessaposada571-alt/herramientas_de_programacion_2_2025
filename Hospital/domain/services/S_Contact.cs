using Hospital.domain.model;
using Hospital.domain.ports;
using System;

namespace Hospital.domain.services
{
 public class S_Contact
 {
 private Contact_port contact_Port;

 public Contact Select(Contact contact)
 {
 // Use contact_Port.Search to find existing contact. If API provides FindById, prefer that; here use Search and assume it returns or sets fields via reference.
 // We'll call contact_Port.Search(contact) and then return contact assuming implementation populates it; otherwise, if there's a FindById(Contact) method on ports, use it.
 try
 {
 contact_Port.Search(contact);
 return contact;
 }
 catch (Exception)
 {
 return null;
 }
 }
 }
}
