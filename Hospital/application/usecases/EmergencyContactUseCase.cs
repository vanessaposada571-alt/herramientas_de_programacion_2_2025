using Hospital.application.validators;
using Hospital.application.ports;
using Hospital.domain.model;
using Hospital.domain.services;

namespace Hospital.application.usecases
{
    public class EmergencyContactUseCase
    {
        private readonly C_Contact _createContact;
        private readonly U_Contact _updateContact;
        private readonly S_Contact _selectContact;
        private readonly EmergencyContactValidator _validator;
        private readonly IContactPort _contactPort;

        public EmergencyContactUseCase(
            C_Contact createContact,
            U_Contact updateContact,
            S_Contact selectContact,
            EmergencyContactValidator validator,
            IContactPort contactPort)
        {
            _createContact = createContact;
            _updateContact = updateContact;
            _selectContact = selectContact;
            _validator = validator;
            _contactPort = contactPort;
        }

        public ContactDto Register(Contact contact)
        {
            var result = _validator.Validate(contact);
            if (!result.IsValid)
                throw new System.ArgumentException(result.ErrorMessage);

            _createContact.Create(contact);

            var fullName = contact.Name1?.Name ?? contact.Name;
            var (firstNames, lastName) = _validator.SplitFullName(fullName);

            var phone = contact.Cellphone != null ? contact.Cellphone.Cellphone.ToString() : string.Empty;
            var relation = contact.Relation;

            return new ContactDto
            {
                FirstNames = firstNames,
                LastName = lastName,
                Relation = relation,
                Phone = phone
            };
        }

        public void Update(Contact contact)
        {
            var result = _validator.Validate(contact);
            if (!result.IsValid)
                throw new System.ArgumentException(result.ErrorMessage);

            _updateContact.Update(contact);
        }

        public ContactDto GetById(Contact contact)
        {
            var found = _selectContact.Select(contact);
            if (found == null) return null;

            var fullName = found.Name1?.Name ?? found.Name;
            var (firstNames, lastName) = _validator.SplitFullName(fullName);
            var phone = found.Cellphone != null ? found.Cellphone.Cellphone.ToString() : string.Empty;

            return new ContactDto
            {
                FirstNames = firstNames,
                LastName = lastName,
                Relation = found.Relation,
                Phone = phone
            };
        }
    }

    public class ContactDto
    {
        public string FirstNames { get; set; }
        public string LastName { get; set; }
        public string Relation { get; set; }
        public string Phone { get; set; }
    }
}