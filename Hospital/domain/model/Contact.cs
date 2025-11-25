using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    public class Contact:Person
    {
        private Patient id_patient;
        private string relation;

        // Wrappers for compatibility
        private Person _name1;
        private long _cellphonePrimitive;
        private Person _cellphoneWrapper;

        internal Patient Id_patient{ get => id_patient; set => id_patient = value; }
        public string Relation { get => relation; set => relation = value; }

        internal Person Name1 { get => _name1; set => _name1 = value; }

        // Some parts of the code expect contact.Cellphone.Cellphone, keep a wrapper
        internal Person Cellphone { get => _cellphoneWrapper; set => _cellphoneWrapper = value; }

        // Nuevo: guarda el PersonId (BIGINT) correspondiente a la fila en dbo.Person
        public long PersonId { get; set; }
    }
}
